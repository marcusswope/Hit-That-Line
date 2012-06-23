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

    $('#taginput').tagEditor();
});

var RichTagEditor = function (inputElement) {
    var input = $(inputElement);
    var tagEditorId = "tagEditor";

    var populateDropDown = function () {
        $.ajax({
            type: "POST",
            url: $.htl.url.tagCounts,
            dataType: "json",
            data: { TagQuery: input.val() },
            cache: false,
            success: function (data) {
                buildSelector(data.Tags);
            },
            error: function (ea) {
                
            }
        });
    };

    var removeDropDown = function () {
        $('#' + tagEditorId).remove();
    };

    var buildSelector = function (tagCounts) {
        var editor = $('#' + tagEditorId).length > 0 ? $('#' + tagEditorId) : buildTagWrapper();
        editor.children().remove();

        for (var tagCountKey in tagCounts) {
            var tagCount = tagCounts[tagCountKey];
            var tagSelector = $('<div rel="{0}" class="tagSelector">{0}({1})</div>'.f(tagCount.Tag, tagCount.Count));
            editor.append(tagSelector);
        }

        editor.insertAfter(input);
    };

    var buildTagWrapper = function () {
        var editor = $('<div class="tagEditor" id="{0}"></div>'.f(tagEditorId));
        editor.width(input.outerWidth() - 2);
        var inputOffset = input.offset();
        editor.offset({ top: inputOffset.top + input.outerHeight(), left: inputOffset.left });
        return editor;
    };

    var selectTag = function (tagSelected) {
        input.val(tagSelected.attr('rel'));
        removeDropDown();
    };

    return {
        populateDropDown: populateDropDown,
        removeDropDown: removeDropDown,
        selectTag: selectTag
    };
};

$.fn.tagEditor = function () {
    var editor = new RichTagEditor(this.get(0));
    this.keyup(function () {
        if (!$(this).val() || $(this).val() == "") {
            editor.removeDropDown();
        }
        else {
            editor.populateDropDown();
        }
    });
    this.blur(function (e) {
        window.setTimeout(editor.removeDropDown, 200);
    });
    $('.tagSelector').live('click', function () {
        editor.selectTag($(this));
    });
};