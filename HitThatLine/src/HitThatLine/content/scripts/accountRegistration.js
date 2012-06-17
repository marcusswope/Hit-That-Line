var DuplicateValidator = function (control) {

    validateUserName = function () {
        validateControl($.htl.url.duplicateUsername, { Username: control.val() });
    };

    validateEmailAddress = function () {
        validateControl($.htl.url.duplicateEmailAddress, { EmailAddress: control.val() });
    };

    var validateControl = function (postUrl, postData) {
        if (!control.val() || control.val() == "") return;
        
        control.addClass('waiting');
        $.ajax({
            type: "POST",
            url: postUrl,
            data: postData,
            success: function (data) {
                if (data.IsValid) {
                    control.addClass('passed');
                    control.removeClass('failed');
                    control.removeClass('waiting');
                }
                else {
                    control.addClass('failed');
                    control.removeClass('passed');
                    control.removeClass('waiting');
                }
            },
            error: function (data) {
                alert(data);
            }
        });
    };

    return {
        validateUserName: validateUserName,
        validateEmailAddress: validateEmailAddress
    };
};

$(function () {

    $('#Username').blur(function () {
        var validator = new DuplicateValidator($(this));
        validator.validateUserName();
    });

    $('#EmailAddress').blur(function () {
        var validator = new DuplicateValidator($(this));
        validator.validateEmailAddress();
    });

    $('#registrationForm').validate({
        rules: {
            ConfirmPassword: {
                equalTo: "#Password"
            }
        },
        messages: {
            ConfirmPassword: {
                equalTo: "password does not match"
            }
        }
    });
});