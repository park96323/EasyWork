/// <reference path="../jquery-1.8.2.min.js" />
/// <reference path="../sweetalert.min.js" />
$(function () {


    //初始化输入文本框的大小
    auto_size();

    //窗体大小改变时执行的事件
    window.onresize = function () {
        auto_size();
    }

    function auto_size() {
        var _width = $(window).width();
        var _height = $(window).height();

        $("#txt_remarks").css({ "width": _width, "height": _height * 0.5 });
    }


    //提交签到

    $("#btn_submit").click(function () {
       
        $.ajax({
            type: 'POST',
            url: "/Sign/Content1",
            data: {
                time: $("#time").html(),
                site: $("#site").val(),
                remark: $("#txt_remarks").val()
            },
            dataType: "json",
            success: function (is) {

                if (is == "ok") {
                   
                    swal({ title: "签到成功！", type: "success", timer: 1500, showConfirmButton: false });
                    setTimeout('location.href = "../Sign/Index"; ', 1500)
                    
                }
                else {
                }
            }
        });

    })


});