Array.prototype.contains = function (item) {
    return this.indexOf(item) !== -1;
};
Array.prototype.first = function (predicate) {
    var result = predicate ? this.where(predicate) : this;
    return result.length ? result[0] : undefined;
};
Array.prototype.groupBy = function (keySelector, elementSelector, resultSelector) {
    if (!keySelector) {
        throw new Error("Argument cannot be null or undefined. Parameter name: keySelector");
    }
    var result = [], groups = {};
    this.forEach(function (item) {
        var key = keySelector(item), hash = key.hash, group = groups[hash];
        if (!group) {
            result.push(groups[hash] = group = []);
            group.key = key;
        }
        group.push(elementSelector ? elementSelector(item) : item);
    });
    if (resultSelector) {
        result.forEach(function (item, index) { result[index] = resultSelector(item.key, item); });
    }
    return result;
};
Array.prototype.max = function (selector) {
    return this.length ? (selector ? this.select(selector) : this).reduce(function (accumulator, value) { return value > accumulator ? value : accumulator; }, Number.MIN_VALUE) : undefined;
};
Array.prototype.orderBy = function (selector) {
    if (!selector) {
        throw new Error("Argument cannot be null or undefined. Parameter name: selector");
    }
    var result = this.slice();
    result.sort(function (a, b) {
        a = selector(a).hash;
        b = selector(b).hash;
        for (var i = 0; i < a.length && i < b.length; i++) {
            var v = a.charCodeAt(i) - b.charCodeAt(i);
            if (v === 0) {
                continue;
            }
            return v;
        }
        return Number(a) - Number(b);
    });
    return result;
};
Array.prototype.orderByDescending = function (selector) {
    if (!selector) {
        throw new Error("Argument cannot be null or undefined. Parameter name: selector");
    }
    return this.orderBy(selector).reverse();
};
Array.prototype.select = function (selector) {
    if (!selector) {
        throw new Error("Argument cannot be null or undefined. Parameter name: selector");
    }
    return this.map(selector);
};
Array.prototype.take = function (count) {
    if (isNaN(count)) {
        throw new Error("Argument cannot be null or undefined. Parameter name: count");
    }
    return this.slice(0, count);
};
Array.prototype.where = function (predicate) {
    if (!predicate) {
        throw new Error("Argument cannot be null or undefined. Parameter name: predicate");
    }
    return this.filter(predicate);
};
//# sourceMappingURL=Linq.js.map