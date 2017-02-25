Object.defineProperty(Object.prototype, "hash", {
    get: function () {
        var _this = this;
        return this instanceof Array ? this.reduce(function (hash, item) { return hash + item.hash; }, "") :
            this instanceof Date ? this.toISOString() :
                this instanceof Number || this instanceof Boolean ? this.toString() :
                    this instanceof String ? this :
                        Object.getOwnPropertyNames(this).reduce(function (hash, key) { return hash + _this[key].hash; }, "");
    }
});
//# sourceMappingURL=Object.js.map