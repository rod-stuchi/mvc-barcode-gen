class Common {

    constructor() {
        $(".click-zoom").on("click", function () {
            var value = $("#slider-zoom").slider("option", "value");

            if ($(this).hasClass("fa-minus-square-o")) {
                var v = --value > 0 ? value-- : 1;
                $("#slider-zoom").slider("option", "value", v);
            } else {
                var v = ++value < 16 ? value++ : 15;
                $("#slider-zoom").slider("option", "value", v);
            }
        });
    }

    public SliderZoom() {
        $("#slider-zoom").slider({
            range: "min",
            min: 1,
            max: 15,
            value: 8,
            orientation: "vertical",
            slide: function (event, ui) {
                var zoomFactor = ui.value;
                $("#barcode64 img").css("width", ((zoomFactor - 1) * 27.857142 + 450) + "px");
            },
            change: function (event, ui) {
                var zoomFactor = ui.value;
                $("#barcode64 img").css("width", ((zoomFactor - 1) * 27.857142 + 450) + "px");
            }
        });
    }

    public CopyClipboard() {
        ZeroClipboard.config({ swfPath: "/Static/plugins/ZeroClipboard.swf" });
        var client = new ZeroClipboard($("#copiar-linha1"));
        client.on('ready', function (event) {
            client.on('copy', function (event) {
                Common.AnimateCopyLine("#linha1-text", "line-under-hover1");
                event.clipboardData.setData('text/plain', $("#linha1-text").text());
            });

            client.on('aftercopy', function (event) {
                //console.log('Copied text to clipboard: ' + event.data['text/plain']);
            });
        });


        var client2 = new ZeroClipboard($("#copiar-linha2"));
        client2.on('ready', function (event) {
            client2.on('copy', function (event) {
                Common.AnimateCopyLine("#linha2-text", "line-under-hover2");
                event.clipboardData.setData('text/plain', $("#linha2-text").text());
            });
        });
    }

    private static AnimateCopyLine(element, line) {
        $(element).addClass("line-under-flash")
            .delay(50)
            .queue(function () {
                $(this).addClass(line).dequeue();
            })
            .delay(350)
            .queue(function () {
                $(this).removeClass("line-under-flash").removeClass(line).dequeue();
            });
    }
}


 