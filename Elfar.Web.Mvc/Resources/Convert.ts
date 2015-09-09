class Convert {
    static toDictionary(obj: any) {
        return Object.keys(obj).select((key: string) => ({ key: key, value: obj[key] }));
    }
} 