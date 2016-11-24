$(function () {
    $(document).ajaxError(function (e, xhr) {
        if (xhr.status == 403) {
            //var response = $.parseJSON(xhr.responseText);
            window.location = "/Account/Login";
        }
    });
});