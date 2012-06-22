$(function () {
    new Markdown.Editor(Markdown.getSanitizingConverter()).run();
    $('#threadForm').validate({
        errorPlacement: function (error, element) {
            if (element.attr('id') === 'wmd-input') {
                return;
            }
            error.insertAfter(element);
        }
    });

    $('#tags').change(function () {
        $.ajax({
            type: "GET",
            url: $.htl.url.tagCounts,
            dataType: "json",
            cache: false,
            success: function (data) {
                alert(data);
            }
        });
    });
});