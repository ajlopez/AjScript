
Function.prototype.method = function(name, func) {
    this.prototype[name] = func;
    return this;
};

Function.method('add1', function(x) { return x + 1; });

var x = new Function();
var result = x.add1(2); // 3

