var Convert = (function () {
    function Convert() {
    }
    Convert.toDictionary = function (obj) {
        return Object.keys(obj).select(function (key) { return ({ key: key, value: obj[key] }); });
    };
    return Convert;
})();
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
        function App(callback) {
            var _this = this;
            this._errorLog = ko.observable().extend({ binding: "with" });
            this._tabs = ko.observableArray([]);
            this.select = function (tab) {
                if (!tab) {
                    return;
                }
                if (_this._currentTab) {
                    _this._currentTab.blur();
                }
                (_this._currentTab = tab).focus();
            };
            this.add = function (item) {
                if (item instanceof Tab) {
                    var tabs = _this.tabs;
                    if (tabs && !tabs().contains(item)) {
                        tabs.push(item);
                    }
                    _this.select(item);
                    return;
                }
                _this.dashboard.add(item);
            };
            this.remove = function (tab) {
                if (!tab || tab === _this.dashboard) {
                    return;
                }
                var tabs = _this.tabs;
                var i = tabs.indexOf(tab);
                if (tab instanceof List) {
                    tab.filter("");
                }
                tabs.remove(tab);
                if (tab.selected()) {
                    _this.select(tabs()[--i]);
                }
            };
            this.show = function (errorLog) {
                if (errorLog.Url) {
                    _this.errorLog(errorLog);
                    return;
                }
                $.get(App.path + "Detail/" + errorLog.ID, function (data) {
                    _this.errorLog($.extend(errorLog, data));
                });
            };
            this.add(this._dashboard = new Dashboard(callback));
            this._errorLog.subscribe(function () { return $("#errorLog").modal("show"); });
        }
        App.init = function () {
            var timeout;
            $("#content").on("focus", ".filter-wrapper input", function () {
                var input = $(this);
                input.removeAttr("placeholder");
                input.parent().addClass("active");
            }).on("blur", ".filter-wrapper input", function () {
                var data = ko.dataFor(this);
                if (!data.filter()) {
                    var input = $(this);
                    timeout = setTimeout(function () {
                        input.parent().removeClass("active");
                        input.attr("placeholder", "Filter");
                    }, 150);
                }
            }).on("click", ".filter-wrapper span", function () {
                var data = ko.dataFor(this);
                if (data.filter()) {
                    data.filter("");
                    $(".filter-wrapper input").blur();
                }
                else {
                    if ($(this).parent().hasClass("active")) {
                        clearTimeout(timeout);
                    }
                    $(".filter-wrapper input").focus();
                }
            });
            ko.bindingProvider.instance = new unobtrusiveBindingsProvider({
                "aspnet-html": "with:errorLog",
                chart: "attr:{id:id},chart:options",
                "click(errorLog)": "click:$root.show",
                "click(tab)": "click:$root.add",
                "close(tab)": "visible:closeable,click:$root.remove",
                "colour(legend)": "style:{backgroundColor:colour}",
                content: "component:{name:'content',params:$data}",
                "content(HTML)": "content:Html",
                "content(section)": "component:{name:template,params:$data}",
                "content(tab)": "visible:selected,component:{name:template,params:$data}",
                "content(value)": "if:!(valueinstanceofObject)",
                cookies: "component:{name:'dictionary',params:Cookies}",
                dataTokens: "component:{name:'dictionary',params:DataTokens}",
                details: "component:{name:'details',params:{errorLog:errorLog}}",
                "filter-wrapper": "css:{filtering:filter}",
                form: "component:{name:'dictionary',params:Form}",
                html: "component:{name:'html',params:{errorLog:errorLog}}",
                ignore: { bindings: "", override: true },
                list: "attr:{id:id}",
                keys: "props:$data",
                queryString: "component:{name:'dictionary',params:QueryString}",
                routeData: "component:{name:'dictionary',params:RouteData}",
                routeDefaults: "component:{name:'dictionary',params:RouteDefaults}",
                routeConstraints: "component:{name:'dictionary',params:RouteConstraints}",
                section: "attr:{id:name}",
                serverVariables: "component:{name:'dictionary',params:ServerVariables}",
                "show(!rows)": "visible:!rows().length",
                "show(Cookies)": "visible:show(Cookies)",
                "show(DataTokens)": "visible:show(DataTokens)",
                "show(Form)": "visible:show(Form)",
                "show(HTML)": "visible:Html",
                "show(QueryString)": "visible:show(QueryString)",
                "show(RouteConstraints)": "visible:show(RouteConstraints)",
                tab: "css:{selected:selected},click:$root.select,attr:{title:title},component:{name:'tab',params:$data}",
                tabs: "visible:tabs().length",
                term: "click:count?$root.add:null,css:css",
                tile: "component:{name:template,params:content},css:size",
                "title(Action)": "attr:{title:Action}",
                "title(Area)": "attr:{title:Area}",
                "title(Controller)": "attr:{title:Controller}",
                "title(Date)": "attr:{title:Date}",
                "title(key)": "attr:{title:key}",
                "title(legend)": "attr:{title:title}",
                "title(Method)": "attr:{title:HttpMethod}",
                "title(name)": "attr:{title:name}",
                "title(term)": "attr:{title:title}",
                "title(Time)": "attr:{title:Time}",
                "title(Type)": "attr:{title:Type}"
            });
            app = new App(function () { return ko.applyBindings(app, $("main")[0]); });
        };
        Object.defineProperty(App.prototype, "dashboard", {
            get: function () {
                return this._dashboard;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(App.prototype, "errorLog", {
            get: function () {
                return this._errorLog;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(App, "path", {
            get: function () {
                return App._path;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(App.prototype, "tabs", {
            get: function () {
                return this._tabs;
            },
            enumerable: true,
            configurable: true
        });
        App._path = location.pathname + "/";
        return App;
    })();
    Elfar.App = App;
    var ErrorLog = (function () {
        function ErrorLog(obj) {
            if (obj) {
                $.extend(this, obj);
                if (this.Area) {
                    this.Area = "/" + this.Area;
                }
                this.DateTime = new Date(obj.Date + "T" + obj.Time);
            }
        }
        ErrorLog.prototype.show = function (obj) {
            return Object.keys(obj).where(function (key) { return !(obj[key] instanceof Object); }).length;
        };
        Object.defineProperty(ErrorLog.prototype, "action", {
            get: function () {
                return this.Action;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(ErrorLog.prototype, "area", {
            get: function () {
                return this.Area;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(ErrorLog.prototype, "controller", {
            get: function () {
                return this.Controller;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(ErrorLog.prototype, "date", {
            get: function () {
                return this.Date;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(ErrorLog.prototype, "method", {
            get: function () {
                return this.HttpMethod;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(ErrorLog.prototype, "time", {
            get: function () {
                return this.Time;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(ErrorLog.prototype, "type", {
            get: function () {
                return this.Type;
            },
            enumerable: true,
            configurable: true
        });
        return ErrorLog;
    })();
    Elfar.ErrorLog = ErrorLog;
    var _Object = (function () {
        function _Object(name, title, template) {
            this.name = name;
            this.title = title;
            this._template = (template || name);
            if (!title) {
                this.title = name;
            }
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
        function Tab(name, title, template, selected) {
            if (selected === void 0) { selected = false; }
            _super.call(this, name, title, template);
            this._selected = ko.observable(selected);
        }
        Tab.prototype.blur = function () {
            this.selected(false);
        };
        Tab.prototype.focus = function () {
            this.selected(true);
        };
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
        function Dashboard(callback) {
            var _this = this;
            _super.call(this, "dashboard", "Dashboard", null, true);
            this._sections = ko.observableArray();
            $.get(App.path + "Summaries", function (data) {
                data = data.reverse().select(function (i) { return new ErrorLog(i); });
                _this.add(_this._summary = new Summary(data));
                if (data.length) {
                    _this.add(new Latest(data));
                    _this.add(new Frequent("Type", data, function (l) { return l.Type; }));
                }
                callback();
            });
        }
        Dashboard.prototype.add = function (item) {
            if (item instanceof Section) {
                var sections = this.sections;
                if (!sections().contains(item)) {
                    sections.push(item);
                }
                return;
            }
            this.summary.add(item);
        };
        Object.defineProperty(Dashboard.prototype, "closeable", {
            get: function () {
                return false;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Dashboard.prototype, "sections", {
            get: function () {
                return this._sections;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Dashboard.prototype, "summary", {
            get: function () {
                return this._summary;
            },
            enumerable: true,
            configurable: true
        });
        return Dashboard;
    })(Tab);
    Elfar.Dashboard = Dashboard;
    var Section = (function (_super) {
        __extends(Section, _super);
        function Section(name, head, template) {
            _super.call(this, name, head, template);
            this.head = head;
        }
        return Section;
    })(_Object);
    Elfar.Section = Section;
    var Summary = (function (_super) {
        __extends(Summary, _super);
        function Summary(errorLogs) {
            _super.call(this, "summary", "Summary");
            this._tiles = ko.observableArray();
            this.add(new Timeline("Timeline", errorLogs.length ? errorLogs.groupBy(function (e) { return e.Date; }) : [errorLogs], 90), "timeline", 3 /* ExtraWide */);
            var today = new Date().setHours(0, 0, 0, 0), logs = errorLogs;
            this.add(new Term(90, logs = logs.where(function (e) { return today <= e.DateTime.addDays(90); })), "term");
            this.add(new Term(30, logs = logs.where(function (e) { return today <= e.DateTime.addDays(30); })), "term");
            this.add(new Term(7, logs = logs.where(function (e) { return today <= e.DateTime.addDays(7); })), "term");
            this.add(new Term(1, logs.where(function (e) { return today <= e.DateTime.valueOf(); })), "term");
            if (errorLogs.length) {
                this.add(new Donut("Actions", errorLogs.groupBy(function (e) { return new key(e.Area, e.Controller, e.Action); })), "donut", 0 /* Large */);
                this.add(new Donut("Controllers", errorLogs.groupBy(function (e) { return new key(e.Area, e.Controller); })), "donut", 0 /* Large */);
            }
        }
        Summary.prototype.add = function (content, template, size) {
            if (size === void 0) { size = 1 /* Small */; }
            if (!(content instanceof Tile)) {
                content = new Tile(content, template, size);
            }
            this.tiles.push(content);
        };
        Object.defineProperty(Summary.prototype, "tiles", {
            get: function () {
                return this._tiles;
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
            this._size = size;
        }
        Object.defineProperty(Tile.prototype, "size", {
            get: function () {
                return TileSize[this._size].toLowerCase();
            },
            enumerable: true,
            configurable: true
        });
        return Tile;
    })(_Object);
    Elfar.Tile = Tile;
    var Chart = (function () {
        function Chart() {
        }
        Object.defineProperty(Chart, "colours", {
            get: function () {
                return Chart._colours;
            },
            enumerable: true,
            configurable: true
        });
        Chart._colours = ["#68217A", "#007ACC", "#217167", "#A83A95", "#1BA1E2", "#571C75", "#009CCC", "#9ACD32", "#F2700F", "#748189"].concat(Highcharts.getOptions().colors);
        return Chart;
    })();
    var Donut = (function () {
        function Donut(id, groups) {
            this.id = id;
            var series = (this.series = [new Series(id, groups)])[0];
            var click = function (event) { return app.add(series.points.first(function (p) { return p.title === event.point.name; })); };
            this.options = {
                chart: { type: "pie", backgroundColor: "#F1F1F1", animation: false },
                credits: { enabled: false },
                tooltip: { enabled: false },
                title: { text: series.count.toString(), y: 27, verticalAlign: "middle", style: { fontSize: "33px" } },
                plotOptions: { pie: { shadow: false, center: ["50%", "56%"], cursor: "pointer", events: { click: click } }, series: { animation: false, states: { hover: { enabled: false } } } },
                series: [{ name: id, data: series.data, size: "64%", innerSize: "53%", dataLabels: { color: "#FFF", format: "{y}", distance: -24, style: { fontWeight: "normal", textShadow: "none" } } }]
            };
        }
        Object.defineProperty(Donut.prototype, "legend", {
            get: function () {
                return this.series[0].points;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Donut.prototype, "title", {
            get: function () {
                return this.id;
            },
            enumerable: true,
            configurable: true
        });
        return Donut;
    })();
    var Timeline = (function () {
        function Timeline(id, groups, days) {
            this.id = id;
            this.groups = groups;
            this.days = days;
            this.start = new Date().addDays(-days);
            this.options = {
                chart: { backgroundColor: "#F1F1F1" },
                credits: { enabled: false },
                xAxis: { type: "datetime", labels: { enabled: false }, lineWidth: 0, tickLength: 0 },
                yAxis: { title: "", labels: { enabled: false }, min: 0, max: groups.max(function (g) { return g.length; }), gridLineWidth: 0 },
                legend: { enabled: false },
                plotOptions: { area: { color: "#BBB", marker: { enabled: false, states: { hover: { enabled: false }, select: { enabled: false } } } }, series: { animation: false, states: { hover: { enabled: false } } } },
                title: { text: "" },
                tooltip: { enabled: false },
                series: [{ type: "area", name: "Error Logs", pointInterval: 86400000, pointStart: this.start, data: this.data }]
            };
        }
        Object.defineProperty(Timeline.prototype, "data", {
            get: function () {
                var values = [], date = new Date(this.start);
                if (this.groups.length) {
                    for (var i = -this.days; i < 0; i++) {
                        var group = this.groups.first(function (g) { return g.key === date.toISOString().split("T")[0]; });
                        values.push(group ? group.length : 0);
                        date.setDate(date.getDate() + 1);
                    }
                }
                return values;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Timeline.prototype, "title", {
            get: function () {
                return this.id;
            },
            enumerable: true,
            configurable: true
        });
        return Timeline;
    })();
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
        function List(errorLogs, name, title) {
            var _this = this;
            _super.call(this, name, title, "list");
            this.errorLogs = errorLogs;
            this._filter = ko.observable("").extend({ binding: "textInput" });
            this.rows = ko.computed(function () {
                var filter = _this.filter().toLowerCase();
                if (!filter || filter.length < 2) {
                    return _this.errorLogs;
                }
                var props = List.props;
                var regex = new RegExp(filter, "g");
                return _this.errorLogs.where(function (errorLog) {
                    for (var i = 0; i < props.length; i++) {
                        if (regex.test(errorLog[props[i]].toLowerCase())) {
                            return true;
                        }
                    }
                    return false;
                });
            });
        }
        Object.defineProperty(List.prototype, "filter", {
            get: function () {
                return this._filter;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(List.prototype, "id", {
            get: function () {
                return this.title.replace(/~?\/|\s/g, "") || "root";
            },
            enumerable: true,
            configurable: true
        });
        List.props = ["Type", "Action", "Controller", "Area", "HttpMethod", "Date"];
        return List;
    })(Tab);
    var Point = (function (_super) {
        __extends(Point, _super);
        function Point(name, errorLogs, colour) {
            _super.call(this, errorLogs, errorLogs.key[name.slice(0, -1)] || "[root]", errorLogs.key.toString());
            this.colour = colour;
        }
        Object.defineProperty(Point.prototype, "value", {
            get: function () {
                return this.errorLogs.length;
            },
            enumerable: true,
            configurable: true
        });
        return Point;
    })(List);
    var Term = (function (_super) {
        __extends(Term, _super);
        function Term(length, errorLogs) {
            _super.call(this, errorLogs, "term-" + length, length === 1 ? "Today" : "Last " + length + " days");
        }
        Object.defineProperty(Term.prototype, "body", {
            get: function () {
                return this.count;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Term.prototype, "count", {
            get: function () {
                return this.errorLogs.length;
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
        Object.defineProperty(Term.prototype, "head", {
            get: function () {
                return this.title;
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
    var Frequent = (function (_super) {
        __extends(Frequent, _super);
        function Frequent(type, errorLogs, keySelector) {
            _super.call(this, "frequent", "Most frequent (by " + type + ")");
            this.items = errorLogs.groupBy(keySelector).orderByDescending(function (g) { return g.length; }).take(10).select(function (g) { return new List(g, g.key); });
        }
        return Frequent;
    })(Section);
    var key = (function () {
        function key(area, controller, action, method) {
            this.Area = area || "", this.Controller = controller || "", this.Action = action || "";
            this.Method = method || "";
        }
        key.prototype.toString = function () {
            return "~" + key.prefix(this.Area, !this.Controller) + key.prefix(this.Controller) + key.prefix(this.Action) + (this.Method ? " [" + this.Method + "]" : "");
        };
        key.prefix = function (value, enforce) {
            if (enforce === void 0) { enforce = false; }
            return value ? "/" + value : (enforce ? "/" : "");
        };
        return key;
    })();
    var TileSize;
    (function (TileSize) {
        TileSize[TileSize["Large"] = 0] = "Large";
        TileSize[TileSize["Small"] = 1] = "Small";
        TileSize[TileSize["Wide"] = 2] = "Wide";
        TileSize[TileSize["ExtraWide"] = 3] = "ExtraWide";
    })(TileSize || (TileSize = {}));
})(Elfar || (Elfar = {}));
$(Elfar.App.init);
ko.bindingHandlers.chart = {
    init: function (element, valueAccessor) {
        setTimeout(function () { return $(element).highcharts(ko.unwrap(valueAccessor())); }, 1);
    }
};
ko.bindingHandlers.content = {
    init: function (element, valueAccessor) {
        var document = element.contentWindow.document;
        document.close();
        document.write(ko.unwrap(valueAccessor()));
    }
};
ko.bindingHandlers.props = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var value = ko.utils.unwrapObservable(valueAccessor()), properties = Convert.toDictionary(value).orderBy(function (o) { return o.key; });
        ko.applyBindingsToNode(element, { foreach: properties }, bindingContext);
        return { controlsDescendantBindings: true };
    }
};
ko.components.register("content", { template: "<div class='tabs'><div class='content(tab)'></div></div>" });
ko.components.register("dashboard", { template: "<div id='sections'><div class='section'><div class='head'></div><div class='body content(section)'></div></div></div>" });
ko.components.register("details", { template: { view: "Details" } });
ko.components.register("dictionary", { template: "<table class='dictionary'><tbody class='keys'><tr class='content(value)'><th class='key title(key)'></th><td class='value'></td></tr></tbody></table>" });
ko.components.register("donut", { template: "<div class='chart'></div><div class='title'></div><ul class='legend'><li class='title(legend)'><span class='colour(legend)'></span><a class='name click(tab)'></a></li></ul>" });
ko.components.register("frequent", { template: "<ul class='items'><li class='title(name)'><a class='name click(tab)'></a></li></ul>" });
ko.components.register("html", { template: { view: "Html" } });
ko.components.register("latest", { template: "<table class='latest'><tbody class='errorLogs'><tr><td class='title(Type)'><a class='type click(errorLog)'></a></td><td class='date'></td><td class='time'></td></tr></tbody></table>" });
ko.components.register("list", { template: { view: "List" } });
ko.components.register("tab", { template: "<span class='title'></span>&nbsp; <i class='close(tab)'>&times;</i>" });
ko.components.register("term", { template: "<div class='term'><div class='head title(term)'></div><div class='body'></div><div class='foot'>Error Logs</div></div>" });
ko.components.register("summary", { template: "<ul id='tiles'><li class='tile'></li></ul>" });
ko.components.register("timeline", { template: "<div class='chart'></div><div class='title'></div>" });
ko.extenders.binding = function (target, binding) {
    target.binding = binding;
    return target;
};
ko.extenders.bindings = function (target, bindings) {
    target.bindings = bindings;
    return target;
};
ko.components.loaders.unshift({
    loadTemplate: function (name, templateConfig, callback) {
        if (templateConfig.view) {
            $.get(location.pathname + "/Template/" + templateConfig.view, function (html) { return ko.components.defaultLoader.loadTemplate(name, html, callback); });
        }
        else {
            callback(null);
        }
    }
});
var Bindings = (function () {
    function Bindings() {
    }
    Bindings.for = function (node, bindingContext, bindings) {
        if (node.nodeType === 1) {
            var name = node.id || node.name || node.className;
            if (name) {
                var nodeName = node.nodeName.toLowerCase(), key = Bindings.path(node), result = Bindings.cache[key];
                if (result === undefined) {
                    var p = Bindings.find(name, bindings), overridden = false;
                    if (p.value !== undefined) {
                        var v = p.value;
                        if (typeof v === "string") {
                            result = v;
                        }
                        else {
                            if ("bindings" in v) {
                                overridden = v.override;
                                v = v.bindings;
                            }
                            result = Bindings.from(v);
                        }
                    }
                    if (!overridden) {
                        p = Bindings.find(name, bindingContext.$data);
                        var parents = bindingContext.$parents;
                        for (var i = 0, l = parents.length; i < l && p.value === undefined; i++) {
                            p = Bindings.find(name, parents[i]);
                            if (p.value !== undefined) {
                                p.key = "$parents[" + i + "]." + p.key;
                                break;
                            }
                        }
                        if (p.value !== undefined) {
                            var s = Bindings.get(p, nodeName);
                            if (result) {
                                result += "," + s;
                            }
                            else {
                                result = s;
                            }
                        }
                    }
                    Bindings.cache[key] = result;
                }
                return result;
            }
        }
        return undefined;
    };
    Bindings.find = function (name, data) {
        var p = { key: null, value: undefined }, classes = name.split(" "), value = undefined;
        for (var i = 0, l = classes.length; i < l && value === undefined; i++) {
            name = classes[i];
            value = data[name];
            if (value === undefined && /-/.test(name)) {
                var names = name.split("-");
                for (var j = 0, m = names.length; j < m; j++) {
                    name = names[j];
                    value = data[name];
                    if (ko.isObservable(value)) {
                        names[j] += "()";
                        data = ko.unwrap(value);
                    }
                    else if (typeof value === "object") {
                        data = value;
                    }
                    else if (value === undefined) {
                        break;
                    }
                }
                name = names.join(".");
            }
        }
        if (value !== undefined) {
            p.key = name;
            p.value = value;
        }
        return p;
    };
    Bindings.from = function (value) {
        if (typeof value === "string") {
            return value;
        }
        else {
            value = ko.toJSON(value);
            return value.substr(1, value.length - 2);
        }
    };
    Bindings.get = function (p, nodeName) {
        var key = p.key, value = p.value, binding = "text";
        switch (nodeName) {
            case "input":
            case "select":
                binding = "value";
        }
        if (ko.isObservable(value)) {
            var v = value.peek();
            if (v && "push" in v) {
                binding = nodeName === "select" ? "selectedOptions" : "foreach";
            }
            else if (typeof v === "object") {
                binding = "with";
            }
            var b = value.binding;
            if (b && (nodeName === "input" || b !== "textInput")) {
                binding = b;
            }
            b = value.bindings;
            if (b) {
                if (typeof b !== "string") {
                    if (b.override) {
                        return b.value;
                    }
                    b = Bindings.from(b.value);
                }
                key += "," + b;
            }
        }
        else if (value instanceof Function) {
            binding = "click";
        }
        else if (value instanceof Array) {
            binding = nodeName === "select" ? "selectedOptions" : "foreach";
        }
        else if (typeof value === "object") {
            binding = "with";
        }
        return binding + ":" + key;
    };
    Bindings.path = function (node) {
        return $(node).parents().addBack().toArray().reduce(function (s, e) {
            var name = e.nodeName.toLowerCase();
            if (name !== "html" && name !== "body") {
                s += "/" + name;
                if (e.id !== "") {
                    s += "#" + e.id;
                }
                if (e.name !== "") {
                    s += ":" + e.name;
                }
                if (e.className !== "") {
                    s += "." + e.className;
                }
            }
            return s;
        }, "");
    };
    Bindings.cache = {};
    return Bindings;
})();
var unobtrusiveBindingsProvider = (function () {
    function unobtrusiveBindingsProvider(bindings) {
        if (bindings === void 0) { bindings = {}; }
        this.inner = ko.bindingProvider.instance;
        this.inner["getBindingsString"] = function (node, bindingContext) { return Bindings.for(node, bindingContext, bindings); };
    }
    unobtrusiveBindingsProvider.prototype.nodeHasBindings = function (node) {
        return node.nodeType === 1 && (node.id || node.name || node.className);
    };
    unobtrusiveBindingsProvider.prototype.getBindingAccessors = function (node, bindingContext) {
        return this.inner.getBindingAccessors(node, bindingContext);
    };
    unobtrusiveBindingsProvider.prototype.getBindings = function (node, bindingContext) {
        return this.inner.getBindings(node, bindingContext);
    };
    return unobtrusiveBindingsProvider;
})();
