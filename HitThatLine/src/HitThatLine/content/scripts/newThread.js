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

    $('#tagInputContainer').tagEditor('#tags');
});

var RichTagEditor = function (inputContainerElement, inputElement, tagHolderElement) {
    var inputContainer = $(inputContainerElement);
    var input = $(inputElement);
    var tagHolder = $(tagHolderElement);
    var tagEditorId = "tagEditor";
    var currentlySelectedTags = [];

    var populateDropDown = function () {
        $.ajax({
            type: "POST",
            url: $.htl.url.tagCounts,
            dataType: "json",
            data: { TagQuery: input.val() },
            cache: true,
            success: function (data) {
                buildSelector(data.Tags);
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
        editor.width(inputContainer.outerWidth() - 2);
        var inputOffset = inputContainer.offset();
        editor.offset({ top: inputOffset.top + inputContainer.outerHeight(), left: inputOffset.left });
        return editor;
    };

    var selectTag = function (tagSelected) {
        var tagName = tagSelected.attr('rel');
        var selectedTag = $('<div class="selectedTag">{0}</div>'.f(tagName));
        var imageToRemove = $('<img rel="{0}" class="removeTag" src="{1}" />'.f(tagName, $.htl.url.removeTagImage));
        imageToRemove.click(function () {
            removeTag($(this).parent('.selectedTag'), $(this).attr('rel'));
        });
        selectedTag.append(imageToRemove);
        inputContainer.insertAt(currentlySelectedTags.length, selectedTag);
        input.val('');
        var currentMargin = input.css('margin-left').replace('px', '');
        input.css('margin-left', '{0}px'.f(selectedTag.width() + 15 + parseInt(currentMargin)));
        var currentWidth = input.css('width').replace('px', '');
        var newWidth = parseInt(currentWidth) - (selectedTag.outerWidth(true) - 23);
        input.css('width', newWidth + 'px');
        input.focus();
        removeDropDown();
        (!tagHolder.val() || tagHolder.val() == '')
            ? tagHolder.val(tagName)
            : tagHolder.val("{0},{1}".f(tagHolder.val(), tagName));
        currentlySelectedTags.push(tagName);
    };

    var removeTag = function (tag, tagName) {
        removeA(currentlySelectedTags, tagName);
        var re = new RegExp(',{0}|{0},|{0}'.f(tagName));
        tagHolder.val(tagHolder.val().replace(re, ""));
        var tagWidth = tag.outerWidth(true);
        tag.remove();
        var currentMargin = input.css('margin-left').replace('px', '');
        input.css('margin-left', '{0}px'.f(parseInt(currentMargin) - tagWidth));
        var currentWidth = input.css('width').replace('px', '');
        var newWidth = parseInt(currentWidth) + tagWidth + 25;
        input.css('width', newWidth + 'px');
        input.focus();
    };

    return {
        populateDropDown: populateDropDown,
        removeDropDown: removeDropDown,
        selectTag: selectTag
    };
};

$.fn.tagEditor = function (tagHolder) {
    var inputElement = $(this).children("#tagInput")[0];
    var editor = new RichTagEditor(this.get(0), inputElement, tagHolder);
    this.keyup(function () {
        var input = $($(this).children("#tagInput")[0]);
        if (!input.val() || input.val() == "") {
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