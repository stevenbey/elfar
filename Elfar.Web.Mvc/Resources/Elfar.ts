/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
"use strict";
module Elfar {
    export class App {
        static init() {
            console.log("Elfar.App initialising...");
            var app = new App();
            app.tabs.push(new Dashboard());
            app.tabs.push(new Tab("test", "Test"));
            ko.applyBindings(app);
        }
        selectTab = (tab: Tab) => {
            ko.utils.arrayForEach(this.tabs(), i => i.selected(false));
            tab.selected(true);
        };
        removeTab = (tab: Tab) => {
            var i = this.tabs.indexOf(tab);
            this.tabs.remove(tab);
            if (tab.selected()) this.tabs()[--i].selected(true);
        };
        static get path(): string {
            return location.pathname;
        }
        tabs = ko.observableArray<Tab>([]);
    }
    class Tab {
        constructor(name: string, title: string) {
            this.name = name;
            this.title = title;
        }
        get disposable(): boolean {
            return true;
        }
        name: string;
        selected = ko.observable(false);
        title: string;
    }
    class Dashboard extends Tab {
        constructor() {
            super("dashboard", "Dashboard");
            $.get(App.path + "/Summaries",(data: any) => {
                this.summaries = data;
                var grouped = data.groupBy(i => new Date(i.Date));
                var today = new Date().setHours(0, 0, 0, 0);
                this.ninety(grouped.takeWhile(g => today <= g.key.addDays(90)));
                this.thirty(this.ninety().takeWhile(g => today <= g.key.addDays(30)));
                this.seven(this.thirty().takeWhile(g => today <= g.key.addDays(7)));
                this.today(this.seven().takeWhile(g => today <= g.key.valueOf()));
            });
            this.selected(true);
        }
        get disposable(): boolean {
            return false;
        }
        ninety = ko.observableArray<any>();
        thirty = ko.observableArray<any>();
        seven = ko.observableArray<any>();
        today = ko.observableArray<any>();
        private summaries: Array<any>;
    }
}
$(() => Elfar.App.init());