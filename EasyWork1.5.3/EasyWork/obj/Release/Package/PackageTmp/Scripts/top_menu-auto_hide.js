
$(function () {
    //默认根据设备屏宽检查并修改样式
    auto_styles();

    //当屏幕大小改变时，修改样式
    window.onresize = function () {
        auto_styles();
    }

    function auto_styles() {
        var _width = $(document.body).width();
        if (_width <= 570) {
            $("body").css({ "marginTop": 0 });
        }
        else {
            $("body").css({ "marginTop": 55 });
        }
    }
});