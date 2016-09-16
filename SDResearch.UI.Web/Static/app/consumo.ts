CodeBarGen.CodeBar.changeLinkActive("Conta de Consumo"); 

module Consumo {
    class Member {
        ValueStart: number;
        ValueEnd: number;
    }

    var member = new Member();

    export class ConsumoUI {
        segment: string;
        code: string;

        value: number;
        valueStart: number;
        valueEnd: number;

        autoGenerate: boolean;

        barcodeImg: string;
    }

    var UIObj = new ConsumoUI();

    export class Utils {
        BindAutocomplete(dataArray) {
            $("#concessionaria").autocomplete({
                source: function (request, response) {
                    var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
                    response($.grep(dataArray, function (value: any) {
                        return matcher.test(value.label) || matcher.test(value.value);
                    }))
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#concessionaria-codigo").val(ui.item.value.padZero(3));
                }
            });
        }

        Valor() {
            $("#valor").maskMoney({ prefix: 'R$ ', allowNegative: false, thousands: '.', decimal: ',', affixesStay: true });
        }

        SliderValor() {
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
            $("#slider-value-text").text("R$ " + $("#slider-value")
                .slider("values", 0) + " - R$ " + $("#slider-value")
                .slider("values", 1));
            member.ValueStart = $("#slider-value").slider("values", 0);
            member.ValueEnd = $("#slider-value").slider("values", 1);
        }

        BindUIObject() {
            UIObj.segment = $("#concessionaria-codigo").val().toString().substring(0, 1);
            UIObj.code = $("#concessionaria-codigo").val().toString().substring(1, 5);

            UIObj.value = $("#valor").maskMoney('unmasked')[0];
            UIObj.valueStart = member.ValueStart;
            UIObj.valueEnd = member.ValueEnd;

            UIObj.autoGenerate = $("#auto-check").is(":checked");
        }

        Validate(): boolean {
            consumoUtils.BindUIObject();

            if (!UIObj.autoGenerate) {
                if (!UIObj.code || UIObj.value === 0) {
                    var count = 0;
                    var interval = setInterval(function () {
                        if (!UIObj.code) {
                            $("#concessionaria").toggleClass("text-error");
                        }

                        if (UIObj.value === 0) {
                            $("#valor").toggleClass("text-error");
                        }
                       
                        if (count > 4)
                            clearInterval(interval);
                        count++;
                    }, 250);

                    return false;
                } else {
                    return true;
                }
            } else
                return true;
        }

        GenConsumo() {
            $("#gen-consumo").on("click", function () {
                if (consumoUtils.Validate()) {
                    $.ajax({
                        url: '/ContaConsumo/GenerateConsumo',
                        method: 'post',
                        async: true,
                        cache: false,
                        dataType: 'json',
                        data: { "consumo": JSON.stringify(UIObj) },
                        success: function (data) {
                            var obj = JSON.parse(data);
                            var zoomFactor = $("#slider-zoom").slider("value");
                            var img = '<img src="data:image/png;base64,' + obj.barcodeBase64 + '" style="width:' + ((zoomFactor - 1) * 27.857142 + 450) + 'px;"/>';

                            $("#barcode64").html(img);
                            $("#div-barcode64").collapse('show')

                            $("#linha1-text").text(obj.line);
                            $("#div-linha1").collapse('show');

                            $("#linha2-text").text(obj.lineFormatted);
                            $("#div-linha2").collapse('show');

                            $(".hr-bottom").show();

                            $("#valor").val(obj.value);
                            $("#valor").maskMoney('mask');

                            $("#concessionaria-codigo").text(obj.segment + '' + obj.code);

                            _dealershipdata.map(function (e) {
                                if (e.value === (obj.segment + '' + obj.code)) {
                                    $("#concessionaria").val(e.label);
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
        }
    }

}

var consumoUtils = new Consumo.Utils();
var _dealershipdata: Array<any>;

$(document).ready(function () {
    var cmmn = new Common();
    cmmn.CopyClipboard();
    cmmn.SliderZoom();

    consumoUtils.BindAutocomplete(_dealershipdata);

    $(document).on('keydown', null, 'f2', function () {
        $("#gen-consumo").click();
        $("input").blur();
    });

    $("input").on('keydown', null, 'f2', function () {
        $("#gen-consumo").click();
        $("input").blur();
    });

    $("#concessionaria").on("change", function () {
        if ($(this).val() === "") {
            $("#concessionaria-codigo").val("");
        }
    });

    $("#concessionaria-limpar").on("click", function () {
        $("#concessionaria").val("");
        $("#concessionaria").focus();
        $("#concessionaria-codigo").val("");
    });

    $("#valor-limpar").on("click", function () {
        $("#valor").val("");
        $("#valor").focus();
    });

    $('[data-toggle="tooltip"]').tooltip({ container: 'body', delay: { "show": 1500, "hide": 300 } });

    $("#auto-check").on("change", function () {
        $("#concessionaria, #valor").val("");
        $("#concessionaria-codigo").val("");

        $("#div-barcode64").collapse('hide');
        $('#div-barcode64').on('hidden.bs.collapse', function () {
            $("#div-linha2").collapse('hide');
            $("#div-linha1").collapse('hide');
            $(".hr-bottom").hide();
        });
    });

    $("button").on("click", function () { this.blur(); });

    consumoUtils.Valor();

    consumoUtils.SliderValor();

    consumoUtils.BindUIObject();

    consumoUtils.GenConsumo();

    $("[id^=slider] *, #navbar *, [class*='navbar-header'] *").prop("tabindex", "-1");
});