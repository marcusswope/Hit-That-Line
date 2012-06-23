$.extend({
    htl: {
        url: { },
        urls: function(a) {
            $.extend($.htl.url, a);
        }
    }
});

var ADAPT_CONFIG;
$(function () {
    ADAPT_CONFIG = {
        path: $.htl.url.stylesDir,
        callback: updateHtmlClass,
        dynamic: true,
        range: ['0px    to 760px  = mobile.min.css',
            '760px  to 980px  = 720.min.css',
            '980px  to 1280px = 960.min.css',
            '1280px to 1600px = 1200.min.css',
            '1600px           = 1560.min.css']
    };
    adapt();
});

function updateHtmlClass(i, width) {
    var html = document.documentElement;
    html.className = html.className.replace(/(\s+)?range_\d/g, '');
    if (i > -1) {
        html.className += ' range_' + i;
    }
}

$.extend($.validator.messages, {
    required: "required",
    email: "email is not valid"
});

String.prototype.format = String.prototype.f = function () {
    var s = this,
        i = arguments.length;

    while (i--) {
        s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);
    }
    return s;
};