// ReSharper disable InconsistentNaming
"use strict";
module Elfar {
    export class App {
        static init() {
            console.log("Elfar.App initialising...");
            app = new App();
            app.addTab(app._dashboard = new Dashboard());
            ko.applyBindings(app);
            console.log("Elfar.App initialised");
        }
        addTab = (tab: Tab) => {
            var tabs = this.tabs;
            if (tabs.indexOf(tab) === -1) {
                tabs.push(tab);
            }
            this.selectTab(tab);
        };
        removeTab = (tab: Tab) => {
            var tabs = this.tabs;
            var i = tabs.indexOf(tab);
            tabs.remove(tab);
            if (tab.selected()) tabs()[--i].selected(true);
        };
        selectTab = (selection: string | Tab) => {
            var tab: Tab;
            if (typeof selection === "string") tab = this.tabs().first(t => t.name === selection);
            else tab = selection;
            ko.utils.arrayForEach(this.tabs(), i => i.selected(false));
            if (!tab) tab = this.tabs()[0];
            tab.selected(true);
        };
        get dashboard(): Dashboard {
            return this._dashboard;
        }
        static get path(): string {
            return location.pathname;
        }
        private _dashboard: Dashboard;
        private tabs = ko.observableArray<Tab>([]);
    }
    export class _Object {
        constructor(public name: string, public title: string, private _template?: string) {
            this._template = (_template || name) + "-template";
        }
        get template(): string {
            return this._template;
        }
    }
    export class Tab extends _Object {
        constructor(name: string, title: string) {
            super(name, title);
        }
        get closeable(): boolean {
            return true;
        }
        selected = ko.observable(false);
    }
    export class Dashboard extends Tab {
        constructor() {
            super("dashboard", "Dashboard");
            $.get(App.path + "/Summaries",(data: any[]) => {
                this.summaries = data.select(i => new _Summary(i)).orderByDescending(i => i.Date);
                this.sections.push(new Summary(this.summaries));
                this.sections.push(new Section(null, "Latest", "latest"));
                this.sections.push(new Section(null, "Most Common", "common"));
                //this.latest(this.summaries.take(10));
                //this.common(this.summaries.groupBy(i => i.Type).orderBy(g => g.length).take(10));
            });
            this.selected(true);
        }
        add(section: Section) {
            this.sections.push(section);
        }
        get closeable(): boolean {
            return false;
        }
        //latest = ko.observableArray<_Summary>();
        //common = ko.observableArray<_Summary>();
        sections = ko.observableArray<Section>();
        private summaries: _Summary[];
    }
    export class Section extends _Object {
        constructor(name: string, title: string, template?: string) {
            super(name, title, template);
        }
    }
    class _Summary {
        constructor(obj: any) {
            $.extend(this, obj);
            this.dateTime = new Date(obj.Date + " " + obj.Time);
        }
        Action: string;
        Area: string;
        Controller: string;
        dateTime: Date;
    }
    class Summary extends Section {
        constructor(data: any[]) {
            super("summary", "_Summary");

            var areas = data.groupBy(i => `~/${i.Area ? i.Area + "/" : ""}`).select((g, i) => new Area(g, Chart.colours[i]));
            this.items.push(new Tile(new Donut("Controllers", areas), "chart", TileSize.Large));
            //this.items.push(new Tile(new Actions(areas.selectMany(a => a.controllers).selectMany(c => c.actions)), "chart", TileSize.Large));
            this.items.push(new Tile(new Chart(1), "chart", TileSize.Large));
            this.items.push(new Tile(new Chart(2), "chart", TileSize.Wide));
            this.items.push(new Tile(new Chart(3), "chart", TileSize.Small));
            this.items.push(new Tile(new Chart(4), "chart", TileSize.Small));

            var today = new Date().setHours(0, 0, 0, 0);
            this.items.push(new Tile(new Term(90, data.where(i => today <= i.dateTime.addDays(90))), "term-tile"));
            this.items.push(new Tile(new Term(30, data.where(i => today <= i.dateTime.addDays(30))), "term-tile"));
            this.items.push(new Tile(new Term(7, data.where(i => today <= i.dateTime.addDays(7))), "term-tile"));
            this.items.push(new Tile(new Term(1, data.where(i => today <= i.dateTime.valueOf())), "term-tile"));
        }
        items = ko.observableArray<Tile>();
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
        data = ko.observableArray<any>();
    }
    class Chart {
        constructor(public id: any) {
        }
        static get colours(): string[] {
            return ["#68217A", "#007ACC", "#217167", "#A83A95", "#1BA1E2", "#571C75", "#009CCC", "#9ACD32", "#F2700F"].concat(Highcharts.getOptions().colors);
        }
    }
    class Donut extends Chart {
        constructor(id: any, data: Area[]) {
            super(id);
            var donutOptions: HighchartsOptions = {
                chart: { type: "pie", backgroundColor: "#F1F1F1", animation: false },
                credits: { enabled: false },
                title: { text: this.id, floating: true, align: "left", style: { fontSize: "16px" } },
                tooltip: { formatter() { return this.point.name + " (<b>" + this.point.y + "</b>)"; } },
                plotOptions: { pie: { shadow: false, center: ["50%", "55%"], dataLabels: { enabled: false }, showInLegend: true }, series: { animation: false } },
                series: [
                    { name: "Areas", data: data.select(d => d.data), size: "59%", showInLegend: false },
                    { name: "Controllers", data: data.selectMany(a => a.controllers).select(c => c.data), size: "84%", innerSize: "60%" }
                ]
            };
            setTimeout(() => $(`#${this.id}`).highcharts(donutOptions), 1);
        }
    }
    //class Controllers extends Donut {
    //    constructor(data: Area[]) {
    //        super("Controllers", data);
    //    }
    //}
    //class Actions extends Donut {
    //    constructor(data: Series[]) {
    //        super("Actions", data);
    //    }
    //}
    class Series {
        constructor(data: any[], public colour: string) {
            this.name = data.key;
            this.value = data.length;
        }
        get data(): any {
            return { name: this.name, y: this.value, color: this.colour };
        }
        value: number;
        name: string;
    }
    class Area extends Series {
        constructor(data: any[], colour: string) {
            super(data, colour);
            var gradient: HighchartsGradient = Highcharts.Color(colour);
            this.controllers = data.groupBy(i => data.key + i.Controller).select((g, i) => {
                var c: HighchartsGradient = gradient.brighten(0.1 - (i / g.length / 50));
                return new Controller(g, c.get(null));
            });
        }
        controllers: Controller[];
    }
    class Controller extends Series {
        constructor(data: _Summary[], colour: string) {
            super(data, colour);
            var gradient: HighchartsGradient = Highcharts.Color(colour);
            this.actions = data.groupBy(i => data.key + "/" + i.Action).select((g, i) => {
                var c: HighchartsGradient = gradient.brighten(0.1 - (i / g.length / 50));
                return new Series(g, c.get(null));
            });
        }
        actions: Series[];
    }
    enum TileSize { Large, Small, Wide }
    export var app: App;
}
$(() => Elfar.App.init());