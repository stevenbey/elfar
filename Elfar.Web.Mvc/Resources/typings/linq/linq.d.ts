interface Array<T> {
    contains(item: T): boolean;
    first(predicate: (item: T) => boolean): T;
    groupBy(keySelector: (item: T) => any, elementSelector?: (item: T) => any, resultSelector?: (key: any, array: any[]) => any): any[];
    max(selector: (item: T) => number): number;
    orderBy(selector: (item: T) => any): any[];
    orderByDescending(selector: (item: T) => any): any[];
    select(selector: (item: T, index: number) => any): any[];
    take(count: number): T[];
    where(predicate: (item: T, index: number) => boolean): T[];

    key: string;
} 