$(document).ready(function () {
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