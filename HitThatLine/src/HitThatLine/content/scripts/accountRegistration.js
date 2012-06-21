$(function () {

    $('#registrationForm').validate({
        onkeyup: false,
        rules: {
            ConfirmPassword: {
                equalTo: "#Password"
            },
            Username: {
                remote: function (control) {
                    return validateDuplicate(control, { Username: $(control).val() }, $.htl.url.duplicateUsername);
                }
            },
            EmailAddress: {
                remote: function (control) {
                    return validateDuplicate(control, { EmailAddress: $(control).val() }, $.htl.url.duplicateEmailAddress);
                }
            }
        },

        messages: {
            ConfirmPassword: {
                equalTo: "password does not match"
            },
            Username: {
                remote: "already in use"
            },
            EmailAddress: {
                remote: "address already in use"
            }
        }
    });
});

function validateDuplicate(control, postData, url) {
    $(control).bind("ajaxStart", function () {
        $(this).addClass('waiting');
        $(this).unbind('ajaxStart');
    }).bind("ajaxStop", function () {
        $(this).removeClass('waiting');
        $(this).unbind('ajaxStop');
    });
    return {
        type: "POST",
        url: url,
        dataType: "json",
        data: postData,
        cache: false,
        dataFilter: function (data) {
            var isValid = JSON.parse(data).IsValid;
            return JSON.stringify(isValid);
        }
    };
}