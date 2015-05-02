// ReSharper disable InconsistentNaming
module Elfar {
    "use strict";
    var app: App;
    //export function registerPlugin(): void {
    //}
    export class App {
        errorLog = ko.observable<ErrorLog>();
        select = (selection: string | Tab) => {
            var tab = typeof selection === "string" ? this.tabs().first((t: Tab) => t.name === selection) : selection;
            if (!tab) { return; }
            this.tabs().forEach((t: Tab) => t.selected(false));
            tab.selected(true);
        };
        add = (item: Tab | Section | Tile) => {
            if (item instanceof Tab) {
                var tabs = this.tabs;
                if (!tabs().contains(item)) {
                    tabs.push(item);
                }
                this.select(item);
                return;
            }
            this.dashboard.add(item);
        };
        remove = (tab: Tab) => {
            var tabs = this.tabs;
            var i = tabs.indexOf(tab);
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
        }
        static init() {
            ko.applyBindings(app = new App());
        }
        get dashboard(): Dashboard {
            return this[0x1];
        }
        static get path(): string {
            return location.pathname;
        }
        get tabs(): KnockoutObservableArray<Tab> {
            return this[0x0];
        }
    }
    export class ErrorLog {
        Action: string;
        Area: string;
        Controller: string;
        dateTime: Date;
        ID: string;
        Type: string;
        Url: string;
        constructor(obj: any) {
            $.extend(this, obj);
            if(this.Area) this.Area = `/${this.Area}`;
            this.dateTime = new Date(obj.Date + " " + obj.Time);
        }
    }
    export class _Object {
        constructor(public name: string, public title: string, template?: string) {
            this[0x0] = (template || name) + "-template";
        }
        get template(): string {
            return this[0x0];
        }
    }
    export class Tab extends _Object {
        constructor(name: string, title: string, template?: string, selected: boolean = false) {
            super(name, title, template);
            this[0x1] = ko.observable(selected);
        }
        get closeable(): boolean {
            return true;
        }
        get selected(): KnockoutObservable<boolean> {
            return this[0x1];
        }
    }
    export class Dashboard extends Tab {
        add = (item: Section | Tile) => {
            if (item instanceof Section) {
                var sections = this.sections;
                if (!sections().contains(item)) {
                    sections.push(item);
                }
                return;
            }
            this.summary.add(item);
        };
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
        get closeable(): boolean {
            return false;
        }
        get sections(): KnockoutObservableArray<Section> {
            return this[0x2];
        }
        get summary(): Summary {
            return this[0x3];
        }
    }
    export class Section extends _Object {
        constructor(name: string, title: string, template?: string) {
            super(name, title, template);
        }
    }
    export class Summary extends Section {
        add = (content: any, template?: string, size: TileSize = TileSize.Small) => {
            if (!(content instanceof Tile)) content = new Tile(content, template, size);
            this.tiles.push(content);
        };
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
            this.add(new Term(90, errorLogs = errorLogs.where((e: ErrorLog) => today <= e.dateTime.addDays(90))), "term");
            this.add(new Term(30, errorLogs = errorLogs.where((e: ErrorLog) => today <= e.dateTime.addDays(30))), "term");
            this.add(new Term(7, errorLogs = errorLogs.where((e: ErrorLog) => today <= e.dateTime.addDays(7))), "term");
            this.add(new Term(1, errorLogs.where((e: ErrorLog) => today <= e.dateTime.valueOf())), "term");
        }
        get tiles(): KnockoutObservableArray<Tile> {
            return this[0x1];
        }
    }
    export class Tile extends _Object {
        constructor(public content: any, template: string, size: TileSize = TileSize.Small) {
            super(null, null, template);
            this[0x1] = size;
        }
        get size(): string {
            return TileSize[this[0x1]].toLowerCase();
        }
    }
    class Chart {
        constructor(public id: any, public series?: Series[]) {}
        static get colours(): string[] {
            return ["#68217A", "#007ACC", "#217167", "#A83A95", "#1BA1E2", "#571C75", "#009CCC", "#9ACD32", "#F2700F"].concat(Highcharts.getOptions().colors);
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
        get data(): any[] {
            return this.points.select((p: Point) => ({ name: p.title, y: p.value, color: p.colour }));
        }
        get count(): number {
            return this.points.length;
        }
    }
    class List extends Tab {
        errorLogs = ko.observableArray<ErrorLog>();
        constructor(name: string, title: string, errorLogs: ErrorLog[]) {
            super(name, title, "list");
            this.errorLogs(errorLogs);
        }
    }
    class Point extends List {
        constructor(name: string, errorLogs: ErrorLog[], public colour: string) {
            super(errorLogs.key[name.slice(0, -1)] || "[root]", errorLogs.key.toString(), errorLogs);
        }
        get value(): number {
            return this.errorLogs().length;
        }
    }
    class Term extends List {
        constructor(length: number, errorLogs: ErrorLog[]) {
            super(`term-${length}`, length === 1 ? "Today" : `Last ${length} days`, errorLogs);
        }
        get count(): number {
            return this.errorLogs().length;
        }
        get css(): string {
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
        toString: () => string;
        Action: string;
        Controller: string;
        Area: string;
        constructor(area: string, controller?: string, action?: string) {
            var prefix = (value: string, enforce: boolean = false) => (value ? `/${value}` : (enforce ? "/" : ""));
            this.Area = area,
            this.Controller = controller,
            this.Action = action;
            this.toString = () => { return "~" + prefix(this.Area, !this.Controller) + prefix(this.Controller) + prefix(this.Action); };
        }
    }
    enum TileSize { Large, Small, Wide }
}
$(() => Elfar.App.init());