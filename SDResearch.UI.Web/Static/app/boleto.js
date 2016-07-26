CodeBarGen.CodeBar.changeLinkActive("Boleto");
var Boleto;
(function (Boleto) {
    var Member = (function () {
        function Member() {
        }
        return Member;
    })();
    var member = new Member();
    var BoletoUI = (function () {
        function BoletoUI() {
        }
        return BoletoUI;
    })();
    Boleto.BoletoUI = BoletoUI;
    var UIObj = new BoletoUI();
    var Utils = (function () {
        function Utils() {
        }
        Utils.prototype.BindAutocomplete = function (dataArray) {
            $("#banco").autocomplete({
                source: function (request, response) {
                    var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
                    response($.grep(dataArray, function (value) {
                        return matcher.test(value.label) || matcher.test(value.value);
                    }));
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#banco-codigo").text(ui.item.value.padZero(3));
                    $("#banco-codigo").removeClass("hidden");
                    //$("#auto-check").bootstrapToggle("off");
                }
            });
        };
        Utils.prototype.CalculateDate = function (start, end) {
            return moment().add(start, 'd').format("DD/MM/YYYY") + " - " + moment().add(end, 'd').format("DD/MM/YYYY");
        };
        Utils.prototype.Valor = function () {
            $("#valor").maskMoney({ prefix: 'R$ ', allowNegative: false, thousands: '.', decimal: ',', affixesStay: true });
        };
        Utils.prototype.SliderValor = function () {
            $("#slider-value").slider({
                range: true,
                min: 1,
                max: 1500,
                values: [1, 200],
                slide: function (event, ui) {
                    member.ValueStart = ui.values[0];
                    member.ValueEnd = ui.values[1];
                    $("#slider-value-text").text("R$ " + ui.values[0] + " - R$ " + ui.values[1]);
                }
            });
            $("#slider-value-text").text("R$ " + $("#slider-value").slider("values", 0) + " - R$ " + $("#slider-value").slider("values", 1));
            member.ValueStart = $("#slider-value").slider("values", 0);
            member.ValueEnd = $("#slider-value").slider("values", 1);
        };
        Utils.prototype.SliderVencimento = function () {
            $("#slider-vencimento").slider({
                range: true,
                min: 0,
                max: 720,
                values: [0, 90],
                slide: function (event, ui) {
                    member.ExpirateStart = moment().add(ui.values[0], 'd').format("DD/MM/YYYY");
                    member.ExpirateEnd = moment().add(ui.values[1], 'd').format("DD/MM/YYYY");
                    $("#slider-vencimento-text").text(boletoUtils.CalculateDate(ui.values[0], ui.values[1]));
                }
            });
            $("#slider-vencimento-text").text(boletoUtils.CalculateDate($("#slider-vencimento").slider("values", 0), $("#slider-vencimento").slider("values", 1)));
            var p1 = $("#slider-vencimento").slider("values", 0);
            var p2 = $("#slider-vencimento").slider("values", 1);
            member.ExpirateStart = moment().add(p1, 'd').format("DD/MM/YYYY");
            member.ExpirateEnd = moment().add(p2, 'd').format("DD/MM/YYYY");
        };
        Utils.prototype.BindUIObject = function () {
            UIObj.bankCode = $("#banco-codigo").text();
            UIObj.value = $("#valor").maskMoney('unmasked')[0];
            UIObj.valueCheck = $("#valor-check").is(":checked");
            UIObj.valueStart = member.ValueStart;
            UIObj.valueEnd = member.ValueEnd;
            UIObj.expirate = $("#vencimento").val();
            UIObj.expirateCheck = $("#vencimento-check").is(":checked");
            UIObj.expirateStart = member.ExpirateStart;
            UIObj.expirateEnd = member.ExpirateEnd;
            UIObj.autoGenerate = $("#auto-check").is(":checked");
        };
        Utils.prototype.Validate = function () {
            boletoUtils.BindUIObject();
            if (!UIObj.autoGenerate) {
                if (!UIObj.bankCode || (UIObj.valueCheck && UIObj.value === 0) || (UIObj.expirateCheck && !UIObj.expirate)) {
                    var count = 0;
                    var interval = setInterval(function () {
                        if (!UIObj.bankCode) {
                            $("#banco").toggleClass("text-error");
                        }
                        if (UIObj.value === 0 && UIObj.valueCheck) {
                            $("#valor").toggleClass("text-error");
                        }
                        if (!UIObj.expirate && UIObj.expirateCheck) {
                            $("#vencimento").toggleClass("text-error");
                        }
                        if (count > 4)
                            clearInterval(interval);
                        count++;
                    }, 250);
                    return false;
                }
                else {
                    return true;
                }
            }
            else
                return true;
        };
        Utils.prototype.GenBoleto = function () {
            $("#gen-boleto").on("click", function () {
                if (boletoUtils.Validate()) {
                    $.ajax({
                        url: '/Boleto/GenerateBoleto',
                        method: 'post',
                        async: true,
                        cache: false,
                        dataType: 'json',
                        data: { "boleto": JSON.stringify(UIObj) },
                        success: function (data) {
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
                            $("#valor").val(obj.value);
                            $("#valor").maskMoney('mask');
                            $("#vencimento").val(obj.expirate);
                            $("#banco-codigo").text(obj.bankCode.padZero(3));
                            $("#banco-codigo").removeClass("hidden");
                            _bankdata.map(function (e) {
                                if (e.value === obj.bankCode) {
                                    $("#banco").val(e.label);
                                }
                            });
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
    Boleto.Utils = Utils;
})(Boleto || (Boleto = {}));
var boletoUtils = new Boleto.Utils();
var _bankdata;
$(document).ready(function () {
    var cmmn = new Common();
    cmmn.CopyClipboard();
    cmmn.SliderZoom();
    boletoUtils.BindAutocomplete(_bankdata);
    $(document).on('keydown', null, 'f2', function () {
        $("#gen-boleto").click();
        $("input").blur();
    });
    $("input").on('keydown', null, 'f2', function () {
        $("#gen-boleto").click();
        $("input").blur();
    });
    $("#banco").on("change", function () {
        if ($(this).val() === "") {
            $("#banco-codigo").text("");
            $("#banco-codigo").addClass("hidden");
        }
    });
    $("#banco-limpar").on("click", function () {
        $("#banco").val("");
        $("#banco").focus();
        $("#banco-codigo").addClass("hidden");
        $("#banco-codigo").text("");
    });
    $("#valor-limpar").on("click", function () {
        $("#valor").val("");
        $("#valor").focus();
    });
    $("#vencimento-limpar").on("click", function () {
        $("#vencimento").val("");
        $("#vencimento").focus();
    });
    $("#vencimento").datepicker({
        showOtherMonths: true,
        selectOtherMonths: true
    });
    $("#icon-calendar").on("click", function () {
        $("#vencimento").datepicker("show");
    });
    $('[data-toggle="tooltip"]').tooltip({ container: 'body', delay: { "show": 1500, "hide": 300 } });
    $("#vencimento").mask("99/99/9999");
    $("#vencimento-check").on("change", function (e) {
        if ($(this).is(":checked")) {
            $("#vencimento").removeProp("disabled");
        }
        else {
            $("#vencimento").val("");
            $("#vencimento").prop("disabled", "disabled");
        }
    });
    $("#valor-check").on("change", function (e) {
        if ($(this).is(":checked")) {
            $("#valor").removeProp("disabled");
        }
        else {
            $("#valor").val("");
            $("#valor").prop("disabled", "disabled");
        }
    });
    $("#auto-check").on("change", function () {
        $("#banco, #valor, #vencimento").val("");
        $("#banco-codigo").addClass("hidden").text("");
        $("#div-barcode64").collapse('hide');
        $('#div-barcode64').on('hidden.bs.collapse', function () {
            $("#div-linha2").collapse('hide');
            $("#div-linha1").collapse('hide');
            $(".hr-bottom").hide();
        });
    });
    $("button").on("click", function () { this.blur(); });
    boletoUtils.Valor();
    boletoUtils.SliderValor();
    boletoUtils.SliderVencimento();
    boletoUtils.BindUIObject();
    boletoUtils.GenBoleto();
    $("[id^=slider] *, #navbar *, [class*='navbar-header'] *").prop("tabindex", "-1");
});
//# sourceMappingURL=boleto.js.map