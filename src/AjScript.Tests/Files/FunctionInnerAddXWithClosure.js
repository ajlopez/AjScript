
addx = function(x) {
    return function(y) { return addx(y); };

    function addx(y) { return y + x; }
};

result = addx(2)(3); // 5
