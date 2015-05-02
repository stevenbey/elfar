interface Date {
    addDays(value: number): number;
}
Date.prototype.addDays = function (value: number) {
    return new Date(this.valueOf()).setDate(this.getDate() + value);
}