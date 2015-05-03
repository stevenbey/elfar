Date.prototype.addDays = function (value) {
    return new Date(this.valueOf()).setDate(this.getDate() + value);
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
    var app;
    var App = (function () {
        function App() {
            var _this = this;
            this.select = function (selection) {
                var tab = typeof selection === "string" ? _this.tabs().first(function (t) { return t.name === selection; }) : selection;
                if (!tab) {
                    return;
                }
                _this.tabs().forEach(function (t) { return t.selected(false); });
                tab.selected(true);
            };
            this.add = function (item) {
                if (item instanceof Tab) {
                    var tabs = _this.tabs;
                    if (!tabs().contains(item)) {
                        tabs.push(item);
                    }
                    _this.select(item);
                    return;
                }
                _this.dashboard.add(item);
            };
            this.remove = function (tab) {
                var tabs = _this.tabs;
                var i = tabs.indexOf(tab);
                tabs.remove(tab);
                if (tab.selected()) {
                    tabs()[--i].selected(true);
                }
            };
            this.show = function (errorLog) {
                if (errorLog.Url) {
                    _this.errorLog(errorLog);
                    return;
                }
                $.get(App.path + "/Detail/" + errorLog.ID, function (data) {
                    $.extend(errorLog, data);
                    _this.errorLog(errorLog);
                });
            };
            this[0x0] = ko.observableArray([]);
            this.add(this[0x1] = new Dashboard());
            this[0x2] = ko.observable();
        }
        App.init = function () {
            ko.applyBindings(app = new App());
        };
        Object.defineProperty(App.prototype, "dashboard", {
            get: function () {
                return this[0x1];
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(App.prototype, "errorLog", {
            get: function () {
                return this[0x2];
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
    var ErrorLog = (function () {
        function ErrorLog(obj) {
            $.extend(this, obj);
            if (this.Area)
                this.Area = "/" + this.Area;
            this.dateTime = new Date(obj.Date + " " + obj.Time);
        }
        return ErrorLog;
    })();
    Elfar.ErrorLog = ErrorLog;
    var _Object = (function () {
        function _Object(name, title, template) {
            this.name = name;
            this.title = title;
            this[0x0] = (template || name) + "-template";
        }
        Object.defineProperty(_Object.prototype, "template", {
            get: function () {
                return this[0x0];
            },
            enumerable: true,
            configurable: true
        });
        return _Object;
    })();
    Elfar._Object = _Object;
    var Tab = (function (_super) {
        __extends(Tab, _super);
        function Tab(name, title, template, selected) {
            if (selected === void 0) { selected = false; }
            _super.call(this, name, title, template);
            this[0x1] = ko.observable(selected);
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
                return this[0x1];
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
            _super.call(this, "dashboard", "Dashboard", null, true);
            this.add = function (item) {
                if (item instanceof Section) {
                    var sections = _this.sections;
                    if (!sections().contains(item)) {
                        sections.push(item);
                    }
                    return;
                }
                _this.summary.add(item);
            };
            this[0x2] = ko.observableArray();
            $.get(App.path + "/Summaries", function (data) {
                data = data.reverse().select(function (i) { return new ErrorLog(i); });
                _this.add(_this[0x3] = new Summary(data));
                _this.add(new Latest(data));
            });
        }
        Object.defineProperty(Dashboard.prototype, "closeable", {
            get: function () {
                return false;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Dashboard.prototype, "sections", {
            get: function () {
                return this[0x2];
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Dashboard.prototype, "summary", {
            get: function () {
                return this[0x3];
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
    var Summary = (function (_super) {
        __extends(Summary, _super);
        function Summary(errorLogs) {
            var _this = this;
            _super.call(this, "summary", "Summary");
            this.add = function (content, template, size) {
                if (size === void 0) { size = 1 /* Small */; }
                if (!(content instanceof Tile))
                    content = new Tile(content, template, size);
                _this.tiles.push(content);
            };
            this[0x1] = ko.observableArray();
            this.add(new Donut("Actions", errorLogs.groupBy(function (e) { return new key(e.Area, e.Controller, e.Action); })), "donut", 0 /* Large */);
            this.add(new Donut("Controllers", errorLogs.groupBy(function (e) { return new key(e.Area, e.Controller); })), "donut", 0 /* Large */);
            this.add(new Donut("Areas", errorLogs.groupBy(function (e) { return new key(e.Area); })), "donut", 0 /* Large */);
            var today = new Date().setHours(0, 0, 0, 0);
            this.add(new Term(90, errorLogs = errorLogs.where(function (e) { return today <= e.dateTime.addDays(90); })), "term");
            this.add(new Term(30, errorLogs = errorLogs.where(function (e) { return today <= e.dateTime.addDays(30); })), "term");
            this.add(new Term(7, errorLogs = errorLogs.where(function (e) { return today <= e.dateTime.addDays(7); })), "term");
            this.add(new Term(1, errorLogs.where(function (e) { return today <= e.dateTime.valueOf(); })), "term");
        }
        Object.defineProperty(Summary.prototype, "tiles", {
            get: function () {
                return this[0x1];
            },
            enumerable: true,
            configurable: true
        });
        return Summary;
    })(Section);
    Elfar.Summary = Summary;
    var Tile = (function (_super) {
        __extends(Tile, _super);
        function Tile(content, template, size) {
            if (size === void 0) { size = 1 /* Small */; }
            _super.call(this, null, null, template);
            this.content = content;
            this[0x1] = size;
        }
        Object.defineProperty(Tile.prototype, "size", {
            get: function () {
                return TileSize[this[0x1]].toLowerCase();
            },
            enumerable: true,
            configurable: true
        });
        return Tile;
    })(_Object);
    Elfar.Tile = Tile;
    var Chart = (function () {
        function Chart(id, series) {
            this.id = id;
            this.series = series;
        }
        Object.defineProperty(Chart, "colours", {
            get: function () {
                return ["#68217A", "#007ACC", "#217167", "#A83A95", "#1BA1E2", "#571C75", "#009CCC", "#9ACD32", "#F2700F"].concat(Highcharts.getOptions().colors);
            },
            enumerable: true,
            configurable: true
        });
        return Chart;
    })();
    var Donut = (function (_super) {
        __extends(Donut, _super);
        function Donut(id, groups) {
            var _this = this;
            _super.call(this, id, [new Series(id, groups)]);
            var series = this.series[0];
            var click = function (event) { return app.add(series.points.first(function (p) { return p.title === event.point.name; })); };
            var donutOptions = {
                chart: { type: "pie", backgroundColor: "#F1F1F1", animation: false },
                credits: { enabled: false },
                tooltip: { enabled: false },
                title: { text: series.count.toString(), y: 27, verticalAlign: "middle", style: { fontSize: "33px" } },
                plotOptions: { pie: { shadow: false, center: ["50%", "56%"], cursor: "pointer", events: { click: click } }, series: { animation: false } },
                series: [{ name: this.id, data: series.data, size: "64%", innerSize: "53%", dataLabels: { color: "#FFF", format: "{y}", distance: -24, style: { fontWeight: "normal", textShadow: "none" } } }]
            };
            setTimeout(function () { return $("#" + _this.id).highcharts(donutOptions); }, 1);
        }
        return Donut;
    })(Chart);
    var Series = (function () {
        function Series(name, groups) {
            this.points = groups.select(function (g, i) { return new Point(name, g, Chart.colours[i]); });
        }
        Object.defineProperty(Series.prototype, "data", {
            get: function () {
                return this.points.select(function (p) { return ({ name: p.title, y: p.value, color: p.colour }); });
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Series.prototype, "count", {
            get: function () {
                return this.points.length;
            },
            enumerable: true,
            configurable: true
        });
        return Series;
    })();
    var List = (function (_super) {
        __extends(List, _super);
        function List(name, title, errorLogs) {
            _super.call(this, name, title, "list");
            this.errorLogs = ko.observableArray();
            this.errorLogs(errorLogs);
        }
        return List;
    })(Tab);
    var Point = (function (_super) {
        __extends(Point, _super);
        function Point(name, errorLogs, colour) {
            _super.call(this, errorLogs.key[name.slice(0, -1)] || "[root]", errorLogs.key.toString(), errorLogs);
            this.colour = colour;
        }
        Object.defineProperty(Point.prototype, "value", {
            get: function () {
                return this.errorLogs().length;
            },
            enumerable: true,
            configurable: true
        });
        return Point;
    })(List);
    var Term = (function (_super) {
        __extends(Term, _super);
        function Term(length, errorLogs) {
            _super.call(this, "term-" + length, length === 1 ? "Today" : "Last " + length + " days", errorLogs);
        }
        Object.defineProperty(Term.prototype, "count", {
            get: function () {
                return this.errorLogs().length;
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
    })(List);
    var Latest = (function (_super) {
        __extends(Latest, _super);
        function Latest(errorLogs) {
            _super.call(this, "latest", "Most recent");
            this.errorLogs = errorLogs.take(10);
        }
        return Latest;
    })(Section);
    var key = (function () {
        function key(area, controller, action) {
            var _this = this;
            var prefix = function (value, enforce) {
                if (enforce === void 0) { enforce = false; }
                return (value ? "/" + value : (enforce ? "/" : ""));
            };
            this.Area = area, this.Controller = controller, this.Action = action;
            this.toString = function () {
                return "~" + prefix(_this.Area, !_this.Controller) + prefix(_this.Controller) + prefix(_this.Action);
            };
        }
        return key;
    })();
    var TileSize;
    (function (TileSize) {
        TileSize[TileSize["Large"] = 0] = "Large";
        TileSize[TileSize["Small"] = 1] = "Small";
        TileSize[TileSize["Wide"] = 2] = "Wide";
    })(TileSize || (TileSize = {}));
})(Elfar || (Elfar = {}));
$(function () { return Elfar.App.init(); });
