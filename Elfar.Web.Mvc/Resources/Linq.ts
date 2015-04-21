interface Array<T> {
    groupBy(keySelector: (item: T) => any, elementSelector?: (item: T) => any, resultSelector?: (key: any, array: any[]) => any): any[];
    takeWhile(predicate: (item: T, index: number) => boolean): T[];
}
var generateHash = (obj: any) => {
    if (obj instanceof Array) return obj.reduce((accumulator, item) => accumulator + generateHash(item), "");
    if (obj instanceof Date) return obj.toString();
    if (obj instanceof Object) {
        var hash = "";
        for (var attr in obj) if (obj.hasOwnProperty(attr)) hash += generateHash(obj[attr]) || "";
        return hash;
    }
    return obj.toString();
};
Array.prototype.groupBy = function (keySelector: (item: any) => any, elementSelector?: (item: any) => any, resultSelector?: (key: any, array: any[]) => any) {
    if (!keySelector) throw "Argument cannot be null. Parameter name: keySelector";
    var result = [], groups = {};
    this.forEach(item => {
        var key = keySelector(item), hash = generateHash(key), group = groups[hash];
        if (!group) {
            result.push(groups[hash] = group = []);
            group.key = key;
        }
        group.push(elementSelector ? elementSelector(item) : item);
    });
    if (resultSelector) result.forEach((item, index) => { result[index] = resultSelector(item.key, item); });
    return result;
};
Array.prototype.takeWhile = function (predicate: (item: any, index: number) => boolean) {
    var result = [];
    for (var i = 0; i < this.length; i++) {
        var item = this[i];
        if (!predicate(item, i)) break;
        result.push(item);
    }
    return result;
};