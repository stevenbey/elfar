var Convert = (function () {
    function Convert() {
    }
    Convert.toDictionary = function (obj) {
        return Object.keys(obj).select(function (key) { return ({ key: key, value: obj[key] }); });
    };
    return Convert;
}());
//# sourceMappingURL=Convert.js.map