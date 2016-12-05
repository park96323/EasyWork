

$(function () {

    //按钮菜单点击事件
    $("#btn-menu a").click(function () {
        $("#btn-menu a").each(function () {     //把所以项的active类删除
            $(this).removeClass("active").css({ height: "40px", color: "#000000", borderBottom: "0px solid #1A97F0" });
        });
        $(this).addClass("active").css({ height: "40px", color: " #1A97F0", borderBottom: "2px solid #1A97F0" });
    });
});