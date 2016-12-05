/// <reference path="../jquery-1.8.2.min.js" />

$(function () {
    $("#btn_sign").click(function () {
        //alert($("#site").text());
        location.href = "/Sign/Content?site=" + $("#site").text();
    })
   
    //getLocation();
    //function getLocation() {
    //    if (navigator.geolocation) {
    //        navigator.geolocation.getCurrentPosition(showPosition);
    //    }
    //    else {
    //        alert("当前不支持获取位置");
    //    }
    //}
    //function showPosition(position) {
    //    $.post("../Sign/GetCurrentLocation",
    //                {
    //                    longitude: position.coords.longitude,
    //                    latitude: position.coords.latitude
    //                },
    //                function (data) {
    //                   // alert(data);
    //                    $("#site").append(data);
    //                }
    //                )
    //}



    //初始化签到按钮
    auto_size();

    //窗体大小改变时执行的事件
    window.onresize = function () {
        auto_size();
    }

    //根据屏幕修改签到按钮的大小及位置
    function auto_size() {
        var _width = $(document.body).width();
        var btn = $("#btn_sign");

        //btn.css({ "width": _width * 9 / 20, "height": _width * 9 / 20, "margin-left": _width * 9 / 20 / 2 });
        btn.animate({
            width: _width * 9 / 20,
            height: _width * 9 / 20,
            marginLeft: _width * 9 / 20 / 2,
            borderWidth: 1
        }, 10);


        //根据签到按钮大小调整签到图标大小
        $("#btn_sign img:eq(0)").css({ "width": _width * 1 / 6, "height": _width * 1 / 6, "marginTop": _width * 9 / 80 });

        $("#addres, #sign_info").css({ "width": _width * 85 / 100 });
    }

});