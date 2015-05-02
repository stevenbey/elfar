// ReSharper disable InconsistentNaming
module Elfar {
    "use strict";
    export var app: App;
    export function registerPlugin(): void {
    }
    export class App {
        select = (selection: string | Tab) => {
            var tab = typeof selection === "string" ? this.tabs().first((t: Tab) => t.name === selection) : selection;
            if (!tab) { return; }
            ko.utils.arrayForEach(this.tabs(), (t: Tab) => t.selected(false));
            tab.selected(true);
        };
        add = (tab: Tab) => {
            var tabs = this.tabs;
            if (tabs.indexOf(tab) === -1) {
                tabs.push(tab);
            }
            this.select(tab);
        };
        remove = (tab: Tab) => {
            var tabs = this.tabs;
            var i = tabs.indexOf(tab);
            tabs.remove(tab);
            if (tab.selected()) { tabs()[--i].selected(true); }
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
    export class _Summary {
        Action: string;
        Area: string;
        Controller: string;
        dateTime: Date;
        constructor(obj: any) {
            $.extend(this, obj);
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
        constructor(name: string, title: string, selected: boolean = false) {
            super(name, title);
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
        add = (section: Section) => {
            var sections = this.sections;
            if (sections.indexOf(section) === -1) {
                sections.push(section);
            }
        };
        constructor() {
            super("dashboard", "Dashboard", true);
            this[0x2] = ko.observableArray<Section>();
            $.get(App.path + "/Summaries",(data: any[]) => {
                data = data.reverse().select((i: any) => new _Summary(i));
                this.add(this[0x3] = new Summary(data));
                this.add(new Section(null, "Latest", "latest"));
                this.add(new Section(null, "Most Common", "common"));
                // this.latest(this.summaries.take(10));
                // this.common(this.summaries.groupBy(i => i.Type).orderBy(g => g.length).take(10));
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
        // latest = ko.observableArray<_Summary>();
        // common = ko.observableArray<_Summary>();
    }
    export class Section extends _Object {
        constructor(name: string, title: string, template?: string) {
            super(name, title, template);
        }
    }
    export class Summary extends Section {
        items = ko.observableArray<Tile>();
        constructor(data: _Summary[]) {
            super("summary", "Summary");

            var key = function(area: string, controller: string) {
                this.area = area,
                this.controller = controller,
                this.toString = () => { return `~/${this.area ? this.area + "/" : ""}${this.controller}`; };
            };
            var controllers = data.groupBy((i: any) => new key(i.Area, i.Controller)).select((g: _Summary[], i: number) => new Controller(g, Chart.colours[i]));
            this.items.push(new Tile(new Donut("Controllers", controllers), "donut", TileSize.Large));
            // this.items.push(new Tile(new Actions(areas.selectMany(a => a.controllers).selectMany(c => c.actions)), "chart", TileSize.Large));
            this.items.push(new Tile(new Chart(1), "chart", TileSize.Large));
            this.items.push(new Tile(new Chart(2), "chart", TileSize.Wide));
            this.items.push(new Tile(new Chart(3), "chart", TileSize.Small));
            this.items.push(new Tile(new Chart(4), "chart", TileSize.Small));

            var today = new Date().setHours(0, 0, 0, 0);
            this.items.push(new Tile(new Term(90, data.where((i: any) => today <= i.dateTime.addDays(90))), "term-tile"));
            this.items.push(new Tile(new Term(30, data.where((i: any) => today <= i.dateTime.addDays(30))), "term-tile"));
            this.items.push(new Tile(new Term(7, data.where((i: any) => today <= i.dateTime.addDays(7))), "term-tile"));
            this.items.push(new Tile(new Term(1, data.where((i: any) => today <= i.dateTime.valueOf())), "term-tile"));
        }
    }
    class Tile extends _Object {
        constructor(public content: any, template: string, private _size: TileSize = TileSize.Small) {
            super(null, null, template);
        }
        get size(): string {
            switch (this._size) {
                case TileSize.Large: return "large";
                case TileSize.Wide: return "wide";
                default: return "small";
            }
        }
    }
    class Term extends Tab {
        data = ko.observableArray<any>();
        constructor(length: number, data: any[]) {
            super("term-tab", length === 1 ? "Today" : `Last ${length} days`);
            this.data(data);
        }
        get count(): number {
            return this.data().length;
        }
        get css(): string {
            return this.count ? "lightblue-bg clickable" : "grey-bg";
        }
    }
    class Chart extends Tab {
        constructor(public id: any) {
            super(id, id);
        }
        toString() {
            return this.id;
        }
        static get colours(): string[] {
            return ["#68217A", "#007ACC", "#217167", "#A83A95", "#1BA1E2", "#571C75", "#009CCC", "#9ACD32", "#F2700F"].concat(Highcharts.getOptions().colors);
        }
    }
    class Donut extends Chart {
        constructor(id: any, public series: Series[]) {
            super(id);
            var donutOptions: HighchartsOptions = {
                chart: { type: "pie", backgroundColor: "#F1F1F1", animation: false },
                credits: { enabled: false },
                title: { text: `${series.sum((s: Series) => s.value)}`, y: 27, verticalAlign: "middle", style: { fontSize: "33px" } },
                plotOptions: { pie: { shadow: false, center: ["50%", "56%"] }, series: { animation: false } },
                series: [{ name: this.id, data: series.select((c: Series) => c.data), size: "64%", innerSize: "53%", dataLabels: { color: "#FFF", format: "{y}", distance: -24, style: { fontWeight: "normal", textShadow: "none" } } } ]
            };
            setTimeout(() => $(`#${this.id}`).highcharts(donutOptions), 1);
        }
    }
    class Series {
        constructor(public key: any, public value: number, public colour: string) {}
        get data(): any {
            return { name: this.key.toString(), y: this.value, color: this.colour };
        }
    }
    class Controller extends Series {
        //actions: Series[];
        constructor(data: _Summary[], colour: string) {
            super(data.key, data.length, colour);
            //var gradient: HighchartsGradient = Highcharts.Color(colour);
            //this.actions = data.groupBy((i: _Summary) => data.key + "/" + i.Action).select((g: _Summary[], i: number) => {
            //    var c: HighchartsGradient = gradient.brighten(0.1 - (i / g.length / 50));
            //    return new Series(g.key, g.length, c.get(null));
            //});
        }
    }
    enum TileSize { Large, Small, Wide }
}
$(() => Elfar.App.init());