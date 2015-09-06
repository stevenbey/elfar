Object.defineProperty(Object.prototype, "hash", {
    get() {
        return this instanceof Array ? this.reduce((hash, item) => hash + item.hash, "") :
               this instanceof Date ? this.toISOString() :
               this instanceof Number || this instanceof Boolean ? this.toString() :
               this instanceof String ? this :
               Object.getOwnPropertyNames(this).reduce((hash, key) => hash + this[key].hash, "");
    }
}); 