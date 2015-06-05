$(document).ready(function () {
    $(document).on("click", "a.confirm-link", function () {
        var link = $(this);
        var form = link.siblings("form");

        $("a.confirm-link").show();
        $(".confirm-section form").hide();

        $(document).on("click", function () {
            link.show();
            form.hide();
        });

        link.hide();
        form.show();

        return false;
    });
});