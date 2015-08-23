// ReSharper disable InconsistentNaming
interface KnockoutBindingHandlers {
    content: KnockoutBindingHandler;
}
ko.bindingHandlers.content = {
    init(element, valueAccessor) {
        var document = element.contentWindow.document;
        document.close();
        document.write(ko.unwrap(valueAccessor()));
    }
};
module Elfar {
    "use strict";
    var app: App;
    export class App {
        static _path = location.pathname + "/";
        private _currentTab: Tab;
        private _dashboard = new Dashboard();
        private _errorLog = ko.observable<ErrorLog>();
        private _tabs = ko.observableArray<Tab>([]);
        select = (tab: Tab) => {
            if (!tab) { return; }
            if (this._currentTab) { this._currentTab.blur(); }
            (this._currentTab = tab).focus();
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
            if (!tab || tab === this.dashboard) { return; }
            var tabs = this.tabs;
            var i = tabs.indexOf(tab);
            if (tab instanceof List) {
                tab.filter("");
                tab.clear();
            }
            tabs.remove(tab);
            if (tab.selected()) { this.select(tabs()[--i]); }
        };
        show = (errorLog: ErrorLog) => {
            if (errorLog.Url) {
                this.errorLog(errorLog);
                return;
            }
            $.get(App.path + "Detail/" + errorLog.ID, (data: any) => {
                this.errorLog($.extend(errorLog, data));
            });
        };
        constructor() {
            this.add(this.dashboard);
        }
        static init() {
            ko.applyBindings(app = new App(), $("body > div:first-child")[0]);
            var timeout: number;
            $("#content").on("focus", ".filter input", () => {
                var input = $(this);
                input.removeAttr("placeholder");
                input.parent().addClass("active");
            }).on("blur", ".filter input", () => {
                var data = ko.dataFor(this);
                if (!data.filter()) {
                    var input = $(this);
                    timeout = setTimeout(() => {
                        input.parent().removeClass("active");
                        input.attr("placeholder", "Filter");
                    }, 150);
                }
            }).on("click", ".filter span", () => {
                var data = ko.dataFor(this);
                if (data.filter()) {
                    data.filter("");
                    $(".filter input").blur();
                } else {
                    if ($(this).parent().hasClass("active")) { clearTimeout(timeout); }
                    $(".filter input").focus();
                }
            }).on("click", ".list .body > div", () => {
                var parent = ko.contextFor(this).$parent;
                parent.clear();
                parent.row = $(this).addClass("current selected");
            });
        }
        get dashboard() {
            return this._dashboard;
        }
        get errorLog() {
            return this._errorLog;
        }
        static get path() {
            return App._path;
        }
        get tabs() {
            return this._tabs;
        }
    }
    export class ErrorLog {
        Action: string;
        Area: string;
        Controller: string;
        Date: string;
        DateTime: Date;
        ID: string;
        Type: string;
        Url: string;
        constructor(obj: any) {
            $.extend(this, obj);
            if(this.Area) { this.Area = `/${this.Area}`; }
            this.DateTime = new Date(obj.Date + "T" + obj.Time);
        }
        show(obj: any) {
            return !!Object.keys(obj).length;
        }
    }
    export class _Object {
        _template;
        constructor(public name: string, public title: string, template?: string) {
            this._template = (template || name) + "-template";
            if (!title) this.title = name;
        }
        get template() {
            return this._template;
        }
    }
    export class Tab extends _Object {
        _selected: KnockoutObservable<boolean>;
        constructor(name: string, title: string, template?: string, selected: boolean = false) {
            super(name, title, template);
            this._selected = ko.observable(selected);
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
            return this._selected;
        }
    }
    export class Dashboard extends Tab {
        private _sections = ko.observableArray<Section>();
        private _summary: Summary;
        constructor() {
            super("dashboard", "Dashboard", null, true);
            $.get(App.path + "Summaries", (data: any[]) => {
                data = data.reverse().select((i: any) => new ErrorLog(i));
                this.add(this._summary = new Summary(data));
                this.add(new Latest(data));
                this.add(new Frequent("Type", data, (l: ErrorLog) => l.Type));
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
            return this._sections;
        }
        get summary() {
            return this._summary;
        }
    }
    export class Section extends _Object {
        constructor(name: string, title: string, template?: string) {
            super(name, title, template);
        }
    }
    export class Summary extends Section {
        private _tiles: KnockoutObservableArray<Tile>;
        constructor(errorLogs: ErrorLog[]) {
            super("summary", "Summary");
            this._tiles = ko.observableArray<Tile>();
            this.add(new Timeline("Timeline", errorLogs.groupBy((e: ErrorLog) => e.Date), 90), "chart", TileSize.ExtraWide);
            var today = new Date().setHours(0, 0, 0, 0), logs = errorLogs;
            this.add(new Term(90, logs = logs.where((e: ErrorLog) => today <= e.DateTime.addDays(90))), "term");
            this.add(new Term(30, logs = logs.where((e: ErrorLog) => today <= e.DateTime.addDays(30))), "term");
            this.add(new Term(7, logs = logs.where((e: ErrorLog) => today <= e.DateTime.addDays(7))), "term");
            this.add(new Term(1, logs.where((e: ErrorLog) => today <= e.DateTime.valueOf())), "term");
            this.add(new Donut("Actions", errorLogs.groupBy((e: ErrorLog) => new key(e.Area, e.Controller, e.Action))), "donut", TileSize.Large);
            this.add(new Donut("Controllers", errorLogs.groupBy((e: ErrorLog) => new key(e.Area, e.Controller))), "donut", TileSize.Large);
            //this.add(new Donut("Areas", errorLogs.groupBy((e: ErrorLog) => new key(e.Area))), "donut", TileSize.Large);
        }
        add(content: any, template?: string, size: TileSize = TileSize.Small) {
            if (!(content instanceof Tile)) { content = new Tile(content, template, size); }
            this.tiles.push(content);
        }
        get tiles() {
            return this._tiles;
        }
    }
    export class Tile extends _Object {
        private _size: TileSize;
        constructor(public content: any, template: string, size: TileSize = TileSize.Small) {
            super(null, null, template);
            this._size = size;
        }
        get size() {
            return TileSize[this._size].toLowerCase();
        }
    }
    class Chart {
        private static _colours = ["#68217A", "#007ACC", "#217167", "#A83A95", "#1BA1E2", "#571C75", "#009CCC", "#9ACD32", "#F2700F", "#748189"].concat(Highcharts.getOptions().colors);
        static get colours() {
            return Chart._colours;
        }
    }
    class Donut {
        series: Series[];
        constructor(public id: string, groups: ErrorLog[][]) {
            var series = (this.series = [new Series(id, groups)])[0];
            var click = (event: HighchartsAreaClickEvent) => app.add(series.points.first((p: Point) => p.title === event.point.name));
            var options: HighchartsOptions = {
                chart: { type: "pie", backgroundColor: "#F1F1F1", animation: false },
                credits: { enabled: false },
                tooltip: { enabled: false },
                title: { text: series.count.toString(), y: 27, verticalAlign: "middle", style: { fontSize: "33px" } },
                plotOptions: { pie: { shadow: false, center: ["50%", "56%"], cursor: "pointer", events: { click: click } }, series: { animation: false, states: { hover: { enabled: false } } } },
                series: [{ name: id, data: series.data, size: "64%", innerSize: "53%", dataLabels: { color: "#FFF", format: "{y}", distance: -24, style: { fontWeight: "normal", textShadow: "none" } } } ]
            };
            setTimeout(() => $(`#${id}`).highcharts(options), 1);
        }
    }
    class Timeline {
        private start: number;
        constructor(public id: string, private groups: ErrorLog[][], private days: number) {
            this.start = new Date().addDays(-days + 1);
            var options: HighchartsOptions = {
                chart: { backgroundColor: "#F1F1F1" },
                credits: { enabled: false },
                xAxis: { type: "datetime", labels: { enabled: false }, lineWidth: 0, tickLength: 0 },
                yAxis: { title: "", labels: { enabled: false }, min: 0, max: groups.max((g: ErrorLog[]) => g.length), gridLineWidth: 0 },
                legend: { enabled: false },
                plotOptions: { area: { color: "#BBB", marker: { enabled: false, states: { hover: { enabled: false }, select: { enabled: false } } } }, series: { animation: false, states: { hover: { enabled: false } } } },
                title: { text: "" },
                tooltip: { enabled: false /*formatter() { return `${new Date(this.x).toISOString().split("T")[0]}:<b>${this.y}</b>`; }*/ },
                series: [{ type: "area", name: "Error Logs", pointInterval: 86400000, pointStart: this.start, data: this.data }]
            };
            setTimeout(() => $(`#${id}`).highcharts(options), 1);
        }
        get data() {
            var values = [], date = new Date(this.start);
            for (var i = -this.days; i < 0; i++) {
                var group = this.groups.first((g: ErrorLog[]) => g.key === date.toISOString().split("T")[0]);
                values.push(group ? group.length : 0);
                date.setDate(date.getDate() + 1);
            }
            return values;
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
        row: JQuery;
        rows: KnockoutComputed<ErrorLog[]>;
        static props = ["Type", "Action", "Controller", "Area", "HttpMethod", "Date"];
        private _filter: KnockoutObservable<string>;
        constructor(public errorLogs: ErrorLog[], name: string, title?: string) {
            super(name, title, "list");
            this._filter = ko.observable("");
            this.rows = ko.computed(() => {
                var filter = this.filter().toLowerCase();
                if (!filter || filter.length < 3) { return this.errorLogs; }
                var props = List.props;
                return this.errorLogs.where((errorLog: ErrorLog) => {
                    for (var i = 0; i < props.length; i++) {
                        if (errorLog[props[i]].toLowerCase().indexOf(filter) !== -1) { return true; }
                    }
                    return false;
                });
            });
        }
        blur() {
            super.blur();
            if (this.row) { this.row.removeClass("current"); }
        }
        clear() {
            if (this.row) { this.row.removeClass("selected"); }
        }
        get filter() {
            return this._filter;
        }
        get id() {
            return this.title.replace(/~?\/|\s/g, "") || "root";
        }
    }
    class Point extends List {
        constructor(name: string, errorLogs: ErrorLog[], public colour: string) {
            super(errorLogs, errorLogs.key[name.slice(0, -1)] || "[root]", errorLogs.key.toString());
        }
        get value() {
            return this.errorLogs.length;
        }
    }
    class Term extends List {
        constructor(length: number, errorLogs: ErrorLog[]) {
            super(errorLogs, `term-${length}`, length === 1 ? "Today" : `Last ${length} days`);
        }
        get count() {
            return this.errorLogs.length;
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
    class Frequent extends Section {
        items: Tab[];
        constructor(type: string, errorLogs: ErrorLog[], keySelector: (item: ErrorLog) => any) {
            super("frequent", `Most frequent (by ${type})`);
            this.items = errorLogs.groupBy(keySelector).orderByDescending((g: any[]) => g.length).take(10).select((g: ErrorLog[]) => new List(g, g.key));
        }
    }
    class key {
        Action: string;
        Area: string;
        Controller: string;
        Method: string;
        static prefix = (value: string, enforce: boolean = false) => value ? `/${value}` : (enforce ? "/" : "");
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
    enum TileSize { Large, Small, Wide, ExtraWide }
    //export interface IErrorLogPlugin { }
    //export function register(plugin: IErrorLogPlugin): void {}
}
$(() => Elfar.App.init());