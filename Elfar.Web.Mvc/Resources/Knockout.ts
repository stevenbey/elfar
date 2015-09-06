// ReSharper disable InconsistentNaming
interface KnockoutBindingHandlers {
    chart: KnockoutBindingHandler;
    content: KnockoutBindingHandler;
    props: KnockoutBindingHandler;
}
interface KnockoutExtenders {
    binding(target: any, binding: string): any;
    bindings(target: any, bindings: string): any;
}
ko.bindingHandlers.chart = {
    init(element: any, valueAccessor: () => any) {
        setTimeout(() => $(element).highcharts(ko.unwrap(valueAccessor())), 1);
    }
};
ko.bindingHandlers.content = {
    init(element: any, valueAccessor: () => any) {
        var document = element.contentWindow.document;
        document.close();
        document.write(ko.unwrap(valueAccessor()));
    }
};
ko.bindingHandlers.props = {
    init(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var value = ko.utils.unwrapObservable(valueAccessor()),
            properties = Convert.toDictionary(value).orderBy(o => o.key);
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
ko.extenders.binding = (target: any, binding: string) => {
    target.binding = binding;
    return target;
};
ko.extenders.bindings = (target: any, bindings: any) => {
    target.bindings = bindings;
    return target;
};
ko.components.loaders.unshift({
    loadTemplate(name, templateConfig, callback) {
        if (templateConfig.view) {
            $.get(location.pathname + "/Template/" + templateConfig.view, html => ko.components.defaultLoader.loadTemplate(name, html, callback));
        } else {
            callback(null);
        }
    }
});
class Bindings {
    static for(node: any, bindingContext: KnockoutBindingContext, bindings: any) {
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
                        } else {
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
                                p.key = `$parents[${i}].${p.key}`;
                                break;
                            }
                        }
                        if (p.value !== undefined) {
                            var s = Bindings.get(p, nodeName);
                            if (result) {
                                 result += `,${s}`;
                            } else { result = s; }
                        }
                    }
                    Bindings.cache[key] = result;
                }
                //if (result && location.hostname === "localhost") {
                //    node.setAttribute("data-bind", result);
                //}
                return result;
            }
        }
        return undefined;
    }
    private static find(name: string, data: any) {
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
                    } else if (typeof value === "object") {
                         data = value;
                    } else if (value === undefined) { break; }
                }
                name = names.join(".");
            }
        }
        if (value !== undefined) {
            p.key = name;
            p.value = value;
        }
        return p;
    }
    private static from(value: any) {
        if (typeof value === "string") {
             return value;
        } else {
            value = ko.toJSON(value);
            return value.substr(1, value.length - 2);
        }
    }
    private static get(p: any, nodeName: string) {
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
            } else if (typeof v === "object") {
                binding = "with";
            }
            var b = value.binding;
            if (b && (nodeName === "input" || b !== "textInput")) { binding = b; }
            b = value.bindings;
            if (b) {
                if (typeof b !== "string") {
                    if (b.override) { return b.value; }
                    b = Bindings.from(b.value);
                }
                key += `,${b}`;
            }
        } else if (value instanceof Function) {
             binding = "click";
        } else if (value instanceof Array) {
             binding = nodeName === "select" ? "selectedOptions" : "foreach";
        } else if (typeof value === "object") {
            binding = "with";
        }
        return binding + ":" + key;
    }
    private static path(node: any) {
        return $(node).parents().addBack().toArray().reduce((s: string, e: any) => {
            var name = e.nodeName.toLowerCase();
            if (name !== "html" && name !== "body") {
                s += `/${name}`;
                if (e.id !== "") {
                    s += `#${e.id}`;
                }
                if (e.name !== "") {
                    s += `:${e.name}`;
                }
                if (e.className !== "") {
                    s += `.${e.className}`;
                }
            }
            return s;
        }, "");
    }
    private static cache = {};
}
class unobtrusiveBindingsProvider implements KnockoutBindingProvider {
    constructor(bindings: any = {}) {
        this.inner["getBindingsString"] = (node: Node, bindingContext: KnockoutBindingContext) => Bindings.for(node, bindingContext, bindings);
    }
    nodeHasBindings(node: any) {
        return node.nodeType === 1 && (node.id || node.name || node.className);
    }
    getBindingAccessors(node: Node, bindingContext: KnockoutBindingContext) {
        return this.inner.getBindingAccessors(node, bindingContext);
    }
    getBindings(node: Node, bindingContext: KnockoutBindingContext) {
        return this.inner.getBindings(node, bindingContext);
    }
    private inner = ko.bindingProvider.instance;
}