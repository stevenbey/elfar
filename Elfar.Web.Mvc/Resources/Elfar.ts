// ReSharper disable InconsistentNaming
module Elfar {
    "use strict";
    var app: App;
    export class App {
        select = (selection: string | Tab) => {
            var tab = typeof selection === "string" ? this.tabs().first((t: Tab) => t.name === selection) : selection;
            if (!tab) { return; }
            this.tabs().forEach((t: Tab) => t.blur());
            tab.focus();
        };
        add = (item: Tab | Section | Tile) => {
            if (item instanceof Tab) {
                var tabs = this.tabs;
                if (!tabs().contains(item)) { tabs.push(item); }
                this.select(item);
                return;
            }
            this.dashboard.add(item);
        };
        remove = (tab: Tab) => {
            if (!tab) { return; }
            var tabs = this.tabs;
            var i = tabs.indexOf(tab);
            if (tab instanceof List) { tab.clear(); }
            tabs.remove(tab);
            if (tab.selected()) { tabs()[--i].selected(true); }
        };
        show = (errorLog: ErrorLog) => {
            if (errorLog.Url) {
                this.errorLog(errorLog);
                return;
            }
            $.get(App.path + "/Detail/" + errorLog.ID, (data: any) => {
                $.extend(errorLog, data);
                this.errorLog(errorLog);
            });
        };
        constructor() {
            this[0x0] = ko.observableArray<Tab>([]);
            this.add(this[0x1] = new Dashboard());
            this[0x2] = ko.observable<ErrorLog>();
        }
        static init() {
            ko.applyBindings(app = new App());
            //window.onhashchange = () => app.select(location.hash.replace(/^#/, ""));
        }
        get dashboard() {
            return this[0x1];
        }
        get errorLog() {
            return this[0x2];
        }
        static get path() {
            return location.pathname;
        }
        get tabs() {
            return this[0x0];
        }
    }
    export class ErrorLog {
        Action: string;
        Area: string;
        Controller: string;
        DateTime: Date;
        ID: string;
        Type: string;
        Url: string;
        constructor(obj: any) {
            $.extend(this, obj);
            if(this.Area) { this.Area = `/${this.Area}`; }
            this.DateTime = new Date(obj.Date + "T" + obj.Time);
        }
    }
    export class _Object {
        constructor(public name: string, public title: string, template?: string) {
            this[0x0] = (template || name) + "-template";
        }
        get template() {
            return this[0x0];
        }
    }
    export class Tab extends _Object {
        constructor(name: string, title: string, template?: string, selected: boolean = false) {
            super(name, title, template);
            this[0x1] = ko.observable(selected);
        }
        blur() {
            this.selected(false);
        }
        focus() {
            this.selected(true);
        }
        get closeable() {
            return true;
        }
        get selected() {
            return this[0x1];
        }
    }
    export class Dashboard extends Tab {
        constructor() {
            super("dashboard", "Dashboard", null, true);
            this[0x2] = ko.observableArray<Section>();
            $.get(App.path + "/Summaries", (data: any[]) => {
                data = data.reverse().select((i: any) => new ErrorLog(i));
                this.add(this[0x3] = new Summary(data));
                this.add(new Latest(data));
                //this.add(new Frequent("Area", data, e => new key(e.Area)));
                //this.add(new Frequent("Controller", data, e => new key(e.Area, e.Controller)));
                //this.add(new Frequent("Action", data, e => new key(e.Area, e.Controller, e.Action)));
                //this.add(new Frequent("Type", data, e => e.Type));
            });
        }
        add(item: Section | Tile) {
            if (item instanceof Section) {
                var sections = this.sections;
                if (!sections().contains(item)) { sections.push(item); }
                return;
            }
            this.summary.add(item);
        }
        get closeable() {
            return false;
        }
        get sections() {
            return this[0x2];
        }
        get summary() {
            return this[0x3];
        }
    }
    export class Section extends _Object {
        constructor(name: string, title: string, template?: string) {
            super(name, title, template);
        }
    }
    export class Summary extends Section {
        constructor(errorLogs: ErrorLog[]) {
            super("summary", "Summary");
            this[0x1] = ko.observableArray<Tile>();
            this.add(new Donut("Actions", errorLogs.groupBy((e: ErrorLog) => new key(e.Area, e.Controller, e.Action))), "donut", TileSize.Large);
            this.add(new Donut("Controllers", errorLogs.groupBy((e: ErrorLog) => new key(e.Area, e.Controller))), "donut", TileSize.Large);
            this.add(new Donut("Areas", errorLogs.groupBy((e: ErrorLog) => new key(e.Area))), "donut", TileSize.Large);
            //this.add(new Chart(1), "chart", TileSize.Wide);
            //this.add(new Chart(2), "chart");
            //this.add(new Chart(3), "chart");
            var today = new Date().setHours(0, 0, 0, 0);
            this.add(new Term(90, errorLogs = errorLogs.where((e: ErrorLog) => today <= e.DateTime.addDays(90))), "term");
            this.add(new Term(30, errorLogs = errorLogs.where((e: ErrorLog) => today <= e.DateTime.addDays(30))), "term");
            this.add(new Term(7, errorLogs = errorLogs.where((e: ErrorLog) => today <= e.DateTime.addDays(7))), "term");
            this.add(new Term(1, errorLogs.where((e: ErrorLog) => today <= e.DateTime.valueOf())), "term");
        }
        add(content: any, template?: string, size: TileSize = TileSize.Small) {
            if (!(content instanceof Tile)) { content = new Tile(content, template, size); }
            this.tiles.push(content);
        }
        get tiles() {
            return this[0x1];
        }
    }
    export class Tile extends _Object {
        constructor(public content: any, template: string, size: TileSize = TileSize.Small) {
            super(null, null, template);
            this[0x1] = size;
        }
        get size() {
            return TileSize[this[0x1]].toLowerCase();
        }
    }
    class Chart {
        private static _colours = ["#68217A", "#007ACC", "#217167", "#A83A95", "#1BA1E2", "#571C75", "#009CCC", "#9ACD32", "#F2700F"].concat(Highcharts.getOptions().colors);
        constructor(public id: any, public series?: Series[]) {}
        static get colours() {
            return Chart._colours;
        }
    }
    class Donut extends Chart {
        constructor(id: string, groups: ErrorLog[][]) {
            super(id, [new Series(id, groups)]);
            var series = this.series[0];
            var click = (event: HighchartsAreaClickEvent) => app.add(series.points.first((p: Point) => p.title === event.point.name));
            var donutOptions: HighchartsOptions = {
                chart: { type: "pie", backgroundColor: "#F1F1F1", animation: false },
                credits: { enabled: false },
                tooltip: { enabled: false },
                title: { text: series.count.toString(), y: 27, verticalAlign: "middle", style: { fontSize: "33px" } },
                plotOptions: { pie: { shadow: false, center: ["50%", "56%"], cursor: "pointer", events: { click: click } }, series: { animation: false } },
                series: [{ name: this.id, data: series.data, size: "64%", innerSize: "53%", dataLabels: { color: "#FFF", format: "{y}", distance: -24, style: { fontWeight: "normal", textShadow: "none" } } } ]
            };
            setTimeout(() => $(`#${this.id}`).highcharts(donutOptions), 1);
        }
    }
    class Series {
        points: Point[];
        constructor(name: string, groups: ErrorLog[][]) {
            this.points = groups.select((g: ErrorLog[], i: number) => new Point(name, g, Chart.colours[i]));
        }
        get data() {
            return this.points.select((p: Point) => ({ name: p.title, y: p.value, color: p.colour }));
        }
        get count() {
            return this.points.length;
        }
    }
    class List extends Tab {
        errorLogs: KnockoutComputed<ErrorLog[]>;
        select = (errorLog: ErrorLog, event: any) => {
            this.clear();
            (this[0x4] = <HTMLDivElement> event.currentTarget).className = "current selected";
        }
        static props = ["Type", "Action", "Controller", "Area", "HttpMethod", "Date"];
        constructor(name: string, title: string, errorLogs: ErrorLog[]) {
            super(name, title, "list");
            this[0x2] = errorLogs;
            this[0x3] = ko.observable("");
            this.errorLogs = ko.computed(() => {
                var filter = this.filter().toLowerCase();
                if (!filter || filter.length < 3) return this[0x2];
                var props = List.props;
                return this[0x2].where((errorLog: ErrorLog) => {
                    for (var i = 0; i < props.length; i++) {
                        if (errorLog[props[i]].toLowerCase().indexOf(filter) !== -1) { return true; }
                    }
                    return false;
                });
            });
        }
        blur() {
            super.blur();
            if (this[0x4]) { this[0x4].className = "selected"; }
        }
        clear() {
            if (this[0x4]) { this[0x4].className = null; }
        }
        focus() {
            super.focus();
            var timeout: number;
            var div = $(`#${this.id} .filter`);
            var input = $("input", div)
                .focus(() => {
                input.removeAttr("placeholder");
                div.addClass("active");
            }).blur(() => {
                if (!this.filter()) {
                    timeout = setTimeout(() => {
                        div.removeClass("active");
                        input.attr("placeholder", "Filter");
                    }, 150);
                }
            });
            $("span", div).click(() => {
                if (this.filter()) {
                    this.filter("");
                    input.blur();
                } else {
                    if (div.hasClass("active")) { clearTimeout(timeout); }
                    input.focus();
                }
            });
        }
        get filter() {
            return this[0x3];
        }
        get id() {
            return this.title.replace(/~?\//g, "") || "root";
        }
    }
    class Point extends List {
        constructor(name: string, errorLogs: ErrorLog[], public colour: string) {
            super(errorLogs.key[name.slice(0, -1)] || "[root]", errorLogs.key.toString(), errorLogs);
        }
        get value() {
            return this.errorLogs().length;
        }
    }
    class Term extends List {
        constructor(length: number, errorLogs: ErrorLog[]) {
            super(`term-${length}`, length === 1 ? "Today" : `Last ${length} days`, errorLogs);
        }
        get count() {
            return this.errorLogs().length;
        }
        get css() {
            return this.count ? "lightblue-bg clickable" : "grey-bg";
        }
    }
    class Latest extends Section {
        errorLogs: ErrorLog[];
        constructor(errorLogs: ErrorLog[]) {
            super("latest", "Most recent");
            this.errorLogs = errorLogs.take(10);
        }
    }
    //class Frequent extends Section {
    //    errorLogs: ErrorLog[];
    //    constructor(type: string, errorLogs: ErrorLog[], keySelector: (item: ErrorLog) => any) {
    //        super("frequent", `Most frequent (by ${type})`);
    //        this.errorLogs = errorLogs.groupBy(keySelector).orderByDescending(g => g.length).take(10);
    //    }
    //}
    class key {
        Action: string;
        Area: string;
        Controller: string;
        Method: string;
        private static prefix = (value: string, enforce: boolean = false) => value? `/${value}` : (enforce ? "/" : "");
        constructor(area: string, controller?: string, action?: string, method?: string) {
            this.Area = area,
            this.Controller = controller,
            this.Action = action;
            this.Method = method;
        }
        toString() {
            return "~" + key.prefix(this.Area, !this.Controller) + key.prefix(this.Controller) + key.prefix(this.Action) + (this.Method ? ` [${this.Method}]` : "");
        }
    }
    enum TileSize { Large, Small, Wide }
    //export interface IErrorLogPlugin { }
    //export function register(plugin: IErrorLogPlugin): void {}
}
$(() => Elfar.App.init());