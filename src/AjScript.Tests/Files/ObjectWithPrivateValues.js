
var myObject = function() {
    var value = 0;
    return {
        increment: function(inc) {
            value = value + inc;
        },
        getValue: function() {
            return value;
        }
    };
} ();

myObject.increment(2);
myObject.increment(3);

result = myObject.getValue(); // 5


