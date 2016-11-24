$(document).ready(function () {
    $(document).on("click", "#contentPager a", function () {
        if ($(this).attr("href") != null) {
            $.ajax({
                url: $(this).attr("href"),
                type: 'GET',
                cache: false,
                success: function (result) {
                    $('#content').html(result);
                    //bindAddToCartBtns();
                }
            });
        }
        return false;
    });
});