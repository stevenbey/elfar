class Convert {
    static toDictionary(obj: any) {
        return Object.keys(obj).select(key => ({ key: key, value: obj[key] }));
    }
} 