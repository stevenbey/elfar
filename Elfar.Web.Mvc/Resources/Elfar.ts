// ReSharper disable InconsistentNaming
"use strict";
module Elfar {
    export class App {
        static init() {
            console.log("Elfar.App initialising...");
            var app = new App();
            app.addTab(new Dashboard());
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
        static get path(): string {
            return location.pathname;
        }
        private tabs = ko.observableArray<Tab>([]);
    }
    class _Object {
        constructor(name: string, title: string, template?: string) {
            this.name = name;
            this.title = title;
            this._template = (template || name) + "-template";
        }
        get template(): string {
            return this._template;
        }
        name: string;
        title: string;
        private _template: string;
    }
    class Tab extends _Object {
        constructor(name: string, title: string) {
            super(name, title);
        }
        get closeable(): boolean {
            return true;
        }
        selected = ko.observable(false);
    }
    class Dashboard extends Tab {
        constructor() {
            super("dashboard", "Dashboard");
            $.get(App.path + "/Summaries",(data: any[]) => {
                this.summaries = data.select(i => new Summary(i)).orderByDescending(i => i.Date);
                this.sections.push(new Stats(this.summaries));
                //this.latest(this.summaries.take(10));
                //this.common(this.summaries.groupBy(i => i.Type).orderBy(g => g.length).take(10));
            });
            this.selected(true);
        }
        get closeable(): boolean {
            return false;
        }
        //latest = ko.observableArray<Summary>();
        //common = ko.observableArray<Summary>();
        sections = ko.observableArray<Section>();
        private summaries: Summary[];
    }
    class Summary {
        constructor(obj: any) {
            $.extend(this, obj);
            this.dateTime = new Date(obj.Date + " " + obj.Time);
        }
        dateTime: Date;
    }
    class Section extends _Object {
        constructor(name: string, title: string) {
            super(name, title);
        }
    }
    class Stats extends Section {
        constructor(data: any[]) {
            super("statistics", "Statistics");
            var today = new Date().setHours(0, 0, 0, 0);
            this.items.push(new Tile(new Term(90, data.where(i => today <= i.dateTime.addDays(90))), "term-tile"));
            this.items.push(new Tile(new Term(30, data.where(i => today <= i.dateTime.addDays(30))), "term-tile"));
            this.items.push(new Tile(new Term(7, data.where(i => today <= i.dateTime.addDays(7))), "term-tile"));
            this.items.push(new Tile(new Term(1, data.where(i => today <= i.dateTime.valueOf())), "term-tile"));
            //new Donut("donut", data.groupBy(i => `~/${i.Area || ""}`).select((g, i) => ({ name: g.key, y: g.length, color: Chart.colours[i] })), "Areas")
            this.items.push(new Tile(null, "donut", TileSize.Large));
            this.items.push(new Tile(null, "donut", TileSize.Wide));
            this.items.push(new Tile(null, "donut", TileSize.Small));
            this.items.push(new Tile(null, "donut", TileSize.Small));
            this.items.push(new Tile(null, "donut", TileSize.Small));
        }
        items = ko.observableArray<Tile>();
    }
    class Tile extends _Object {
        constructor(content: any, template: string, size: TileSize = TileSize.Small) {
            super(null, null, template);
            this.content = content;
            this._size = size;
        }
        get size(): string {
            switch (this._size) {
                case TileSize.Large: return "large";
                case TileSize.Wide: return "wide";
                default: return "small";
            }
        }
        content: any;
        private _size: TileSize;
    }
    enum TileSize { Large, Small, Wide }
    class Term extends Tab {
        constructor(length: number, data: any[]) {
            super("term-tab", length === 1 ? "Today" : length + " days");
            this.data(data);
        }
        get count(): number {
            return this.data().length;
        }
        get css(): string {
            return this.count ? "teal-bg clickable" : "lightgrey-bg";
        }
        data = ko.observableArray<any>();
    }
    class Chart {
        static get colours(): string[] {
            return ["#68217A", "#A83A95", "#571C75", "#009CCC", "#007ACC", "#9ACD32", "#F2700F"].concat(Highcharts.getOptions().colors);
        }
    }
    class Donut {
        constructor(id: string, data: any, title: string) {
            //var donutOptions: HighchartsOptions = {
            //    chart: { type: "pie", backgroundColor: "#F1F1F1", animation: false },
            //    credits: { enabled: false },
            //    title: { text: title, floating: true, align: "left", style: { fontSize: "16px" } },
            //    tooltip: { enabled: false },
            //    plotOptions: { pie: { shadow: false, center: ["50%", "50%"] }, series: { animation: false } },
            //    series: [{
            //        name: title,
            //        data: data,
            //        size: "80%",
            //        innerSize: "40%",
            //        dataLabels: {
            //            formatter() { return `<b>${this.point.name} (${this.y})</b>`; },
            //            distance: -40,
            //            color: "white",
            //            shadow: false,
            //            style: { textShadow: "none" }
            //        }
            //    }]
            //};
            //setTimeout(() => $(`#${id}`).highcharts(donutOptions), 1);
        }
    }
    //class Series {
    //    constructor(data: any[], colour: string) {
    //        this.name = data.key;
    //        this.count = data.length;
    //        this.colour = colour;
    //    }
    //    get data(): any {
    //        return { name: this.name, y: this.count, color: this.colour };
    //    }
    //    colour: string;
    //    count: number;
    //    name: string;
    //}
    //class Area extends Series {
    //    constructor(data: any[], colour: string) {
    //        super(data, colour);
    //        var gradient: HighchartsGradient = Highcharts.Color(colour);
    //        this.controllers = data.groupBy(i => i.Controller).select((g, i) => {
    //            var c: HighchartsGradient = gradient.brighten(0.1 - (i / g.length / 50));
    //            return new Controller(g, c.get(null));
    //        });
    //    }
    //    controllers: Controller[];
    //}
    //class Controller extends Series {
    //    constructor(data: any[], colour: string) {
    //        super(data, colour);
    //        var gradient: HighchartsGradient = Highcharts.Color(colour);
    //        this.actions = data.groupBy(i => i.Action).select((g, i) => {
    //            var c: HighchartsGradient = gradient.brighten(0.1 - (i / g.length / 50));
    //            return new Series(g, c.get(null));
    //        });
    //    }
    //    actions: Series[];
    //}
}
$(() => Elfar.App.init());