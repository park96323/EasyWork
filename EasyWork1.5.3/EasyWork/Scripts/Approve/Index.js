/// <reference path="../jquery-2.1.4.js" />
/// <reference path="../sweetalert.min.js" />

$(function () {
    

    //按钮菜单点击事件
    $("#btn-menu a").click(function () {
        $("#btn-menu a").each(function () {     //把所以项的active类删除
            $(this).removeClass("active").css({ height: "40px", color: "#000000", borderBottom: "0px solid #1A97F0" });
        });
        $(this).addClass("active").css({ height: "40px", color: " #1A97F0", borderBottom: "2px solid #1A97F0" });
        switch ($(this).attr("id")) {
            case "btn-approval":
                //alert("你点击了待审批选项！");
                break;
            case "btn-approved":
                //alert("你点击了已审批选项！");
                break;
            default:
                //alert("你点击了我发出审批的选项！");
                break;
        }
    });

    //发起审批按钮点击事件
    $("#add-approve a:eq(3)").click(function () {
        if ($(this).hasClass("active")) {
            //已点开选项按钮
            $(this).removeClass("active");
            for (var i = 0; i < 3; i++) {
                $("#add-approve a:eq(" + i + ")").animate({
                    right: 5,
                    opactiy: "show"
                }, 40 - i * 10);
            }
        } else {
            //未点开选项按钮
            $(this).addClass("active");
            //显示按钮菜单
            for (var i = 2; i > -1; i--) {
                $("#add-approve a:eq(" + i + ")").removeClass("hidden").animate({
                    right: 60 + (2 - i) * 55,
                    opactiy: "show"
                }, 10 + (2 - i) * 10);
            }
        }
    });

});