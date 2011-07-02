
var quo = function(status) {
    return {
        get_status: function() {
            return status;
        }
    };
};
// Make an instance of quo.
var myQuo = quo("amazed");

result = myQuo.get_status();

