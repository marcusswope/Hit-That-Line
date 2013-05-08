$(function () {
    var timeZoneCookieName = "timeZoneOffset";
    var offset = new Date().getTimezoneOffset();
    offset = -offset;

    var timeZoneCookie = getCookie(timeZoneCookieName);
    if (!timeZoneCookie || timeZoneCookie != offset) {
        setCookie(timeZoneCookieName, offset);
        location.reload(true);
    }

    $('#main-logo').click(function () { window.location = $.htl.url.home; });
});

function setCookie(name, value) {
    var date = new Date();
    date.setTime(date.getTime() + (20 * 365 * 24 * 60 * 60 * 1000));
    var expires = "; expires=" + date.toGMTString();
    document.cookie = name + "=" + value + expires + "; path=/";
}

$.extend({
    htl: {
        url: {},
        urls: function (a) {
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

jQuery.fn.insertAt = function (index, element) {
    var lastIndex = this.children().size();
    if (index < 0) {
        index = Math.max(0, lastIndex + 1 + index);
    }
    this.append(element);
    if (index < lastIndex) {
        this.children().eq(index).before(this.children().last());
    }
    return this;
};

function removeA(arr) {
    var what, a = arguments, L = a.length, ax;
    while (L > 1 && arr.length) {
        what = a[--L];
        while ((ax = arr.indexOf(what)) != -1) {
            arr.splice(ax, 1);
        }
    }
    return arr;
}

if (typeof String.prototype.trimLeft !== "function") {
    String.prototype.trimLeft = function () {
        return this.replace(/^\s+/, "");
    };
}
if (typeof String.prototype.trimRight !== "function") {
    String.prototype.trimRight = function () {
        return this.replace(/\s+$/, "");
    };
}
if (typeof Array.prototype.map !== "function") {
    Array.prototype.map = function (callback, thisArg) {
        for (var i = 0, n = this.length, a = []; i < n; i++) {
            if (i in this) a[i] = callback.call(thisArg, this[i]);
        }
        return a;
    };
}
function getCookies() {
    var c = document.cookie, v = 0, cookies = {};
    if (document.cookie.match(/^\s*\$Version=(?:"1"|1);\s*(.*)/)) {
        c = RegExp.$1;
        v = 1;
    }
    if (v === 0) {
        c.split(/[,;]/).map(function (cookie) {
            var parts = cookie.split(/=/, 2),
                name = decodeURIComponent(parts[0].trimLeft()),
                value = parts.length > 1 ? decodeURIComponent(parts[1].trimRight()) : null;
            cookies[name] = value;
        });
    } else {
        c.match(/(?:^|\s+)([!#$%&'*+\-.0-9A-Z^`a-z|~]+)=([!#$%&'*+\-.0-9A-Z^`a-z|~]*|"(?:[\x20-\x7E\x80\xFF]|\\[\x00-\x7F])*")(?=\s*[,;]|$)/g).map(function ($0, $1) {
            var name = $0,
                value = $1.charAt(0) === '"'
                          ? $1.substr(1, -1).replace(/\\(.)/g, "$1")
                          : $1;
            cookies[name] = value;
        });
    }
    return cookies;
}
function getCookie(name) {
    return getCookies()[name];
}