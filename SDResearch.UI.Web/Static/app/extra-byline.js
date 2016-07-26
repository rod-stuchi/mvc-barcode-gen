CodeBarGen.CodeBar.changeLinkActive("/Extra/ByLine");
var ByLine;
(function (ByLine) {
    var ByLineUI = (function () {
        function ByLineUI() {
        }
        return ByLineUI;
    })();
    ByLine.ByLineUI = ByLineUI;
    var UIObj = new ByLineUI();
    var Utils = (function () {
        function Utils() {
        }
        Utils.prototype.BindUIObject = function () {
            var reg = new RegExp("\\d+", "g");
            if ($("#linha").val()) {
                UIObj.fromLine = $("#linha").val().match(reg).join("");
            }
        };
        Utils.prototype.Validate = function () {
            byLineUtils.BindUIObject();
            var valid = UIObj && UIObj.fromLine && (UIObj.fromLine.length == 47 || UIObj.fromLine.length == 48);
            if (!valid) {
                var count = 0;
                var interval = setInterval(function () {
                    $("#linha").toggleClass("text-error");
                    if (count > 4)
                        clearInterval(interval);
                    count++;
                }, 250);
            }
            return valid;
        };
        Utils.prototype.GenCodigo = function () {
            $("#gen-codigo").on("click", function () {
                if (byLineUtils.Validate()) {
                    $.ajax({
                        url: '/Extra/ByLine',
                        method: 'post',
                        async: true,
                        cache: false,
                        dataType: 'json',
                        data: { "line": JSON.stringify(UIObj) },
                        success: function (data) {
                            if (data) {
                                var obj = JSON.parse(data);
                                var zoomFactor = $("#slider-zoom").slider("value");
                                var img = '<img src="data:image/png;base64,' + obj.barcodeBase64 + '" style="width:' + ((zoomFactor - 1) * 27.857142 + 450) + 'px;"/>';
                                $("#barcode64").html(img);
                                $("#div-barcode64").collapse('show');
                                $("#linha1-text").text(obj.line);
                                $("#div-linha1").collapse('show');
                                $("#linha2-text").text(obj.lineFormatted);
                                $("#div-linha2").collapse('show');
                                $(".hr-bottom").show();
                                if (obj.value) {
                                    $("#row-valor").removeClass("hidden");
                                    $("#valor").val(obj.value);
                                }
                                if (obj.expirate) {
                                    $("#row-vencimento").removeClass("hidden");
                                    $("#vencimento").val(obj.expirate);
                                }
                                if (obj.bank) {
                                    $("#row-banco").removeClass("hidden");
                                    $("#row-concessionaria").addClass("hidden");
                                    $("#banco").val(obj.bank);
                                }
                                if (obj.dealership) {
                                    $("#row-concessionaria").removeClass("hidden");
                                    $("#row-banco").addClass("hidden");
                                    $("#row-vencimento").addClass("hidden");
                                    $("#concessionaria").val(obj.dealership);
                                }
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            console.log(jqXHR);
                            console.log(textStatus);
                            console.log(errorThrown);
                        }
                    });
                }
            });
        };
        return Utils;
    })();
    ByLine.Utils = Utils;
})(ByLine || (ByLine = {}));
var byLineUtils = new ByLine.Utils();
$(document).ready(function () {
    var cmmn = new Common();
    cmmn.CopyClipboard();
    cmmn.SliderZoom();
    $("#linha").focus();
    $(document).on('keydown', null, 'f2', function () {
        $("#gen-codigo").click();
        $("input").blur();
    });
    $("input").on('keydown', null, 'f2', function () {
        $("#gen-codigo").click();
        $("input").blur();
    });
    $("#linha-limpar").on("click", function () {
        $("#linha").val("");
        $("#linha").focus();
    });
    $('[data-toggle="tooltip"]').tooltip({ container: 'body', delay: { "show": 1500, "hide": 300 } });
    $("button").on("click", function () { this.blur(); });
    byLineUtils.BindUIObject();
    byLineUtils.GenCodigo();
    $("[id^=slider] *, #navbar *, [class*='navbar-header'] *").prop("tabindex", "-1");
});
//# sourceMappingURL=extra-byline.js.map