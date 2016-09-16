/// <reference path="../jquery/jquery.d.ts" />
/// <reference path="../jquery/jquery-ui/jqueryui.d.ts" />
/// <reference path="../bootstrap/js/bootstrap.d.ts" />
var moment;
var ZeroClipboard;
var CodeBarGen;
(function (CodeBarGen) {
    var CodeBar = (function () {
        function CodeBar() {
        }
        CodeBar.changeLinkActive = function (name) {
            $("#navbar>ul").eq(0).find('a').parent().removeClass("active-stu");
            $("#navbar>ul").eq(1).find('a').parent().removeClass("active");
            if (name == "Home" || name == "home") {
                $("#navbar>ul").eq(0).find('a').parent().eq(0).addClass("active-stu");
            }
            else {
                if (name.indexOf("/") >= 0) {
                    $("#navbar>ul").eq(1).find('a[href^="' + name + '"]').parent().addClass("active");
                    $("#navbar>ul").eq(1).find('li.dropdown').addClass('active-stu');
                }
                else {
                    $("#navbar>ul").eq(0).find('a:contains("' + name + '")').parent().addClass("active-stu");
                }
            }
        };
        return CodeBar;
    }());
    CodeBarGen.CodeBar = CodeBar;
})(CodeBarGen || (CodeBarGen = {}));
String.prototype.padZero = function (len, c) {
    var s = this, c = c || '0';
    while (s.length < len)
        s = c + s;
    return s;
};
//# sourceMappingURL=base.js.map