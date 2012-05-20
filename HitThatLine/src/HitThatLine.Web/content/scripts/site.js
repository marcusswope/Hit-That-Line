$.extend({
    htl: {
        url: { },
        urls: function(a) {
            $.extend($.htl.url, a);
        }
    }
});

var ADAPT_CONFIG;
$(document).ready(function () {
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