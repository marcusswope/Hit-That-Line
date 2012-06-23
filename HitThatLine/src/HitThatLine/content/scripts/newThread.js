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

    $('#tagInputContainer').tagEditor();
});

var RichTagEditor = function (inputContainerElement, inputElement) {
    var inputContainer = $(inputContainerElement);
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

        if (tagCounts.length == 0) {
            removeDropDown();
        }
        else {
            editor.insertAfter(inputContainer);
        }
    };

    var buildTagWrapper = function () {
        var editor = $('<div class="tagEditor" id="{0}"></div>'.f(tagEditorId));
        editor.width(input.outerWidth());
        var inputOffset = inputContainer.offset();
        editor.offset({ top: inputOffset.top + inputContainer.outerHeight(), left: inputOffset.left });
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
    var inputElement = $(this).children("#tagInput")[0];
    var editor = new RichTagEditor(this.get(0), inputElement);
    this.keyup(function () {
        var input = $($(this).children("#tagInput")[0]);
        if (!input.val() || input.val() == "") {
            editor.removeDropDown();
        }
        else {
            editor.populateDropDown();
        }
    });
    //    this.blur(function (e) {
    //        window.setTimeout(editor.removeDropDown, 200);
    //    });
    $('.tagSelector').live('click', function () {
        editor.selectTag($(this));
    });
};