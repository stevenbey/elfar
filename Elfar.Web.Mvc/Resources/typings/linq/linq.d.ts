interface Array<T> {
    aggregate(func: (accumulator: T, value: T) => T, seed?: T, selector?: (result: T) => any): any;
    all(predicate: (item: T) => boolean): boolean;
    any(predicate?: (item: T) => boolean): boolean;
    average(selector?: (item: T) => number): number;
    contains(item: T): boolean;
    distinct(selector: (item: T) => any): T[];
    first(predicate: (item: T) => boolean): T;
    groupBy(keySelector: (item: T) => any, elementSelector?: (item: T) => any, resultSelector?: (key: any, array: any[]) => any): any[];
    last(predicate: (item: T) => boolean): T;
    max(selector: (item: T) => number): number;
    min(selector: (item: T) => number): number;
    orderBy(selector: (item: T) => any): any[];
    orderByDescending(selector: (item: T) => any): any[];
    select(selector: (item: T, index: number) => any): any[];
    selectMany(collectionSelector: (item: T, index: number) => any[], resultSelector?: (item: T, array: any[]) => any): any[];
    skip(count: number): T[];
    skipWhile(predicate: (item: T, index: number) => boolean): T[];
    sum(selector: (item: T) => number): number;
    take(count: number): T[];
    takeWhile(predicate: (item: T, index: number) => boolean): T[];
    where(predicate: (item: T, index: number) => boolean): T[];

    key: string;
} 