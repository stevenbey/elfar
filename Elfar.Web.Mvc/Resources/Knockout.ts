// ReSharper disable InconsistentNaming
interface KnockoutBindingHandlers {
    content: KnockoutBindingHandler;
    props: KnockoutBindingHandler;
}
interface KnockoutExtenders {
    binding(target: any, binding: string): any;
    bindings(target: any, bindings: string): any;
}
ko.bindingHandlers.props = {
    init(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var value = ko.utils.unwrapObservable(valueAccessor()),
            properties = Convert.toDictionary(value).orderBy(o => o.key);
        ko.applyBindingsToNode(element, { foreach: properties }, bindingContext);
        return { controlsDescendantBindings: true };
    }
};
ko.bindingHandlers.content = {
    init(element: any, valueAccessor: () => any) {
        var document = element.contentWindow.document;
        document.close();
        document.write(ko.unwrap(valueAccessor()));
    }
};
ko.extenders.binding = (target: any, binding: string) => {
    target.binding = binding;
    return target;
};
ko.extenders.bindings = (target: any, bindings: any) => {
    target.bindings = bindings;
    return target;
};
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
                if (result && location.hostname === "localhost") {
                    node.setAttribute("data-bind", result);
                }
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
            if ("push" in value || v instanceof Array) {
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