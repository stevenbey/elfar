Date.prototype.addDays = function (value) {
    return new Date().setDate(this.getDate() + value);
};
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var Elfar;
(function (Elfar) {
    "use strict";
    Elfar.app;
    function registerPlugin() {
    }
    Elfar.registerPlugin = registerPlugin;
    var App = (function () {
        function App() {
            var _this = this;
            this.select = function (selection) {
                var tab = typeof selection === "string" ? _this.tabs().first(function (t) { return t.name === selection; }) : selection;
                if (!tab) {
                    return;
                }
                ko.utils.arrayForEach(_this.tabs(), function (t) { return t.selected(false); });
                tab.selected(true);
            };
            this.add = function (tab) {
                var tabs = _this.tabs;
                if (tabs.indexOf(tab) === -1) {
                    tabs.push(tab);
                }
                _this.select(tab);
            };
            this.remove = function (tab) {
                var tabs = _this.tabs;
                var i = tabs.indexOf(tab);
                tabs.remove(tab);
                if (tab.selected()) {
                    tabs()[--i].selected(true);
                }
            };
            this[0x0] = ko.observableArray([]);
            this.add(this[0x1] = new Dashboard());
        }
        App.init = function () {
            ko.applyBindings(Elfar.app = new App());
        };
        Object.defineProperty(App.prototype, "dashboard", {
            get: function () {
                return this[0x1];
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(App, "path", {
            get: function () {
                return location.pathname;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(App.prototype, "tabs", {
            get: function () {
                return this[0x0];
            },
            enumerable: true,
            configurable: true
        });
        return App;
    })();
    Elfar.App = App;
    var _Object = (function () {
        function _Object(name, title, _template) {
            this.name = name;
            this.title = title;
            this._template = _template;
            this._template = (_template || name) + "-template";
        }
        Object.defineProperty(_Object.prototype, "template", {
            get: function () {
                return this._template;
            },
            enumerable: true,
            configurable: true
        });
        return _Object;
    })();
    Elfar._Object = _Object;
    var Tab = (function (_super) {
        __extends(Tab, _super);
        function Tab(name, title, selected) {
            if (selected === void 0) { selected = false; }
            _super.call(this, name, title);
            this._selected = ko.observable(false);
            this._selected(selected);
        }
        Object.defineProperty(Tab.prototype, "closeable", {
            get: function () {
                return true;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Tab.prototype, "selected", {
            get: function () {
                return this._selected;
            },
            enumerable: true,
            configurable: true
        });
        return Tab;
    })(_Object);
    Elfar.Tab = Tab;
    var Dashboard = (function (_super) {
        __extends(Dashboard, _super);
        function Dashboard() {
            var _this = this;
            _super.call(this, "dashboard", "Dashboard", true);
            this.sections = ko.observableArray();
            $.get(App.path + "/Summaries", function (data) {
                _this.summaries = data.select(function (i) { return new _Summary(i); }).orderByDescending(function (i) { return i.Date; });
                _this.sections.push(new Summary(_this.summaries));
                _this.sections.push(new Section(null, "Latest", "latest"));
                _this.sections.push(new Section(null, "Most Common", "common"));
            });
        }
        Dashboard.prototype.add = function (section) {
            this.sections.push(section);
        };
        Object.defineProperty(Dashboard.prototype, "closeable", {
            get: function () {
                return false;
            },
            enumerable: true,
            configurable: true
        });
        return Dashboard;
    })(Tab);
    Elfar.Dashboard = Dashboard;
    var Section = (function (_super) {
        __extends(Section, _super);
        function Section(name, title, template) {
            _super.call(this, name, title, template);
        }
        return Section;
    })(_Object);
    Elfar.Section = Section;
    var _Summary = (function () {
        function _Summary(obj) {
            $.extend(this, obj);
            this.dateTime = new Date(obj.Date + " " + obj.Time);
        }
        return _Summary;
    })();
    var Summary = (function (_super) {
        __extends(Summary, _super);
        function Summary(data) {
            _super.call(this, "summary", "Summary");
            this.items = ko.observableArray();
            var key = function (area, controller) {
                var _this = this;
                this.area = area, this.controller = controller, this.toString = function () {
                    return "~/" + (_this.area ? _this.area + "/" : "") + _this.controller;
                };
            };
            var controllers = data.groupBy(function (i) { return new key(i.Area, i.Controller); }).select(function (g, i) { return new Controller(g, Chart.colours[i]); });
            this.items.push(new Tile(new Donut("Controllers", controllers), "donut", 0 /* Large */));
            this.items.push(new Tile(new Chart(1), "chart", 0 /* Large */));
            this.items.push(new Tile(new Chart(2), "chart", 2 /* Wide */));
            this.items.push(new Tile(new Chart(3), "chart", 1 /* Small */));
            this.items.push(new Tile(new Chart(4), "chart", 1 /* Small */));
            var today = new Date().setHours(0, 0, 0, 0);
            this.items.push(new Tile(new Term(90, data.where(function (i) { return today <= i.dateTime.addDays(90); })), "term-tile"));
            this.items.push(new Tile(new Term(30, data.where(function (i) { return today <= i.dateTime.addDays(30); })), "term-tile"));
            this.items.push(new Tile(new Term(7, data.where(function (i) { return today <= i.dateTime.addDays(7); })), "term-tile"));
            this.items.push(new Tile(new Term(1, data.where(function (i) { return today <= i.dateTime.valueOf(); })), "term-tile"));
        }
        return Summary;
    })(Section);
    var Tile = (function (_super) {
        __extends(Tile, _super);
        function Tile(content, template, _size) {
            if (_size === void 0) { _size = 1 /* Small */; }
            _super.call(this, null, null, template);
            this.content = content;
            this._size = _size;
        }
        Object.defineProperty(Tile.prototype, "size", {
            get: function () {
                switch (this._size) {
                    case 0 /* Large */: return "large";
                    case 2 /* Wide */: return "wide";
                    default: return "small";
                }
            },
            enumerable: true,
            configurable: true
        });
        return Tile;
    })(_Object);
    var Term = (function (_super) {
        __extends(Term, _super);
        function Term(length, data) {
            _super.call(this, "term-tab", length === 1 ? "Today" : "Last " + length + " days");
            this.data = ko.observableArray();
            this.data(data);
        }
        Object.defineProperty(Term.prototype, "count", {
            get: function () {
                return this.data().length;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Term.prototype, "css", {
            get: function () {
                return this.count ? "lightblue-bg clickable" : "grey-bg";
            },
            enumerable: true,
            configurable: true
        });
        return Term;
    })(Tab);
    var Chart = (function (_super) {
        __extends(Chart, _super);
        function Chart(id) {
            _super.call(this, id, id);
            this.id = id;
        }
        Chart.prototype.toString = function () {
            return this.id;
        };
        Object.defineProperty(Chart, "colours", {
            get: function () {
                return ["#68217A", "#007ACC", "#217167", "#A83A95", "#1BA1E2", "#571C75", "#009CCC", "#9ACD32", "#F2700F"].concat(Highcharts.getOptions().colors);
            },
            enumerable: true,
            configurable: true
        });
        return Chart;
    })(Tab);
    var Donut = (function (_super) {
        __extends(Donut, _super);
        function Donut(id, series) {
            var _this = this;
            _super.call(this, id);
            this.series = series;
            var donutOptions = {
                chart: { type: "pie", backgroundColor: "#F1F1F1", animation: false },
                credits: { enabled: false },
                title: { text: "" + series.sum(function (s) { return s.value; }), y: 27, verticalAlign: "middle", style: { fontSize: "33px" } },
                plotOptions: { pie: { shadow: false, center: ["50%", "56%"] }, series: { animation: false } },
                series: [{ name: this.id, data: series.select(function (c) { return c.data; }), size: "64%", innerSize: "53%", dataLabels: { color: "#FFF", format: "{y}", distance: -24, style: { fontWeight: "normal", textShadow: "none" } } }]
            };
            setTimeout(function () { return $("#" + _this.id).highcharts(donutOptions); }, 1);
        }
        return Donut;
    })(Chart);
    var Series = (function () {
        function Series(key, value, colour) {
            this.key = key;
            this.value = value;
            this.colour = colour;
        }
        Object.defineProperty(Series.prototype, "data", {
            get: function () {
                return { name: this.key.toString(), y: this.value, color: this.colour };
            },
            enumerable: true,
            configurable: true
        });
        return Series;
    })();
    var Controller = (function (_super) {
        __extends(Controller, _super);
        function Controller(data, colour) {
            _super.call(this, data.key, data.length, colour);
        }
        return Controller;
    })(Series);
    var TileSize;
    (function (TileSize) {
        TileSize[TileSize["Large"] = 0] = "Large";
        TileSize[TileSize["Small"] = 1] = "Small";
        TileSize[TileSize["Wide"] = 2] = "Wide";
    })(TileSize || (TileSize = {}));
})(Elfar || (Elfar = {}));
$(function () { return Elfar.App.init(); });
