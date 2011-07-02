
addx = function(x) {
    return function(y) { return x + y; };
};

result = addx(2)(3); // 5
