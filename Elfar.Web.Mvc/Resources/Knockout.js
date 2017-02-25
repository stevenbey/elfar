// ReSharper disable InconsistentNaming
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
}());
var handlers = ko.bindingHandlers;
handlers.chart = {
    init: function (element, valueAccessor) {
        setTimeout(function () { return $(element).highcharts(ko.unwrap(valueAccessor())); }, 1);
    }
};
handlers.content = {
    init: function (element, valueAccessor) {
        var document = element.contentWindow.document;
        document.close();
        document.write(ko.unwrap(valueAccessor()));
    }
};
handlers.props = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var value = ko.utils.unwrapObservable(valueAccessor()), properties = Convert.toDictionary(value).orderBy(function (o) { return o.key; });
        ko.applyBindingsToNode(element, { foreach: properties }, bindingContext);
        return { controlsDescendantBindings: true };
    }
};
var components = ko.components;
components.loaders.unshift({
    loadTemplate: function (componentName, templateConfig, callback) {
        if (templateConfig.view) {
            $.get(location.pathname + "/Template/" + templateConfig.view, function (html) { return ko.components.defaultLoader.loadTemplate(name, html, callback); });
        }
        else {
            callback(null);
        }
    }
});
var register = components.register;
register("content", { template: "<div class='tabs'><div class='content(tab)'></div></div>" });
register("dashboard", { template: "<div id='sections'><div class='section'><div class='head'></div><div class='body content(section)'></div></div></div>" });
register("details", { template: { view: "Details" } });
register("dictionary", { template: "<table class='dictionary'><tbody class='keys'><tr><th class='key title(key)'></th><td class='value'></td></tr></tbody></table>" });
register("donut", { template: "<div class='chart'></div><div class='title'></div><ul class='legend'><li class='title(legend)'><span class='colour(legend)'></span><a class='name click(tab)'></a></li></ul>" });
register("frequent", { template: "<ul class='items'><li class='title(name)'><a class='name click(tab)'></a></li></ul>" });
register("html", { template: { view: "Html" } });
register("latest", { template: "<table class='latest'><tbody class='errorLogs'><tr><td class='title(Type)'><a class='type click(errorLog)'></a></td><td class='date'></td><td class='time'></td></tr></tbody></table>" });
register("list", { template: { view: "List" } });
register("modals", { template: "<div id='details'></div><div id='html'></div>" });
register("tab", { template: "<span class='title'></span>&nbsp; <i class='close(tab)'>&times;</i>" });
register("term", { template: "<div class='term'><div class='head title(term)'></div><div class='body'></div><div class='foot'>Error Logs</div></div>" });
register("summary", { template: "<ul id='tiles'><li class='tile'></li></ul>" });
register("timeline", { template: "<div class='chart'></div><div class='title'></div>" });
var extenders = ko.extenders;
extenders.binding = function (target, binding) {
    target.binding = binding;
    return target;
};
extenders.bindings = function (target, bindings) {
    target.bindings = bindings;
    return target;
};
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
                if (result && location.hostname === "localhost") {
                    node.setAttribute("data-bind", result);
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
                    if (value === undefined) {
                        break;
                    }
                    else if (ko.isObservable(value)) {
                        names[j] += "()";
                        data = ko.unwrap(value);
                    }
                    else if (j < m - 1) {
                        if (typeof value === "object") {
                            data = value;
                        }
                        else {
                            value = undefined;
                            break;
                        }
                    }
                    else if (typeof value === "object") {
                        value = "with";
                    }
                    else if (typeof value === "function") {
                        value = "click";
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
        return typeof value === "string" ? value : (value = ko.toJSON(value)).substr(1, value.length - 2);
    };
    Bindings.get = function (kvp, nodeName) {
        var key = kvp.key, value = kvp.value, binding = "text";
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
}());
//# sourceMappingURL=Knockout.js.map