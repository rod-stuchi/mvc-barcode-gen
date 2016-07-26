/// <reference path="../jquery/jquery.d.ts" />

interface JQuery {
    maskMoney: Function;
    mask: Function;
    collapse: Function;
    bootstrapToggle: Function;
}

var moment: Function;
var ZeroClipboard: any;


interface String {
    padZero(len: number, c : string) : String,
}

module CodeBarGen {
    export class CodeBar {
        static changeLinkActive(name: string) {
            $("#navbar>ul").eq(0).find('a').parent().removeClass("active-stu");
            $("#navbar>ul").eq(1).find('a').parent().removeClass("active");

            if (name == "Home" || name == "home") {
                $("#navbar>ul").eq(0).find('a').parent().eq(0).addClass("active-stu");
            }
            else {
                if (name.indexOf("/") >= 0) {
                    $("#navbar>ul").eq(1).find('a[href^="' + name + '"]').parent().addClass("active");
                    $("#navbar>ul").eq(1).find('li.dropdown').addClass('active-stu');

                } else {
                    $("#navbar>ul").eq(0).find('a:contains("' + name + '")').parent().addClass("active-stu");
                }
            }
        }
    }
}


String.prototype.padZero = function (len, c) {
    var s = this, c = c || '0';
    while (s.length < len) s = c + s;
    return s;
}