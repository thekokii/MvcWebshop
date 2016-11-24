
var bindMinusPlusBtns = function () {
    $(".cart-minus").click(function () {
        var input = $("input[data-id='" + $(this).attr("data-id") + "'");
        var value = parseInt(input.val());
        if (value > 2) {
            input.val(value - 1);
        } else {
            input.val(1);
        }
    });

    $(".cart-plus").click(function () {
        var input = $("input[data-id='" + $(this).attr("data-id") + "'");
        var value = parseInt(input.val());
        if (value < 100) {
            input.val(value + 1);
        } else if (isNaN(value)) {
            input.val(1);
        }
    });
};