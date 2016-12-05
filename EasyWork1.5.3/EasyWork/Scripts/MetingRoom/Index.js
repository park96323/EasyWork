

$(function () {
    //查询会议室
    $("#Sel").click(function () {
        $.post("../MeetingRoom/SelMeet", { sel: $("#txtSel").val }, function (data) {
            //alert(data)
        })
    })

    //菜单点击事件
    $("#menu-tools a:eq(3)").click(function () {
        if ($(this).hasClass("active")) {
            $(this).removeClass("active");
            //隐藏菜单按钮
            for (var i = 0; i < 3; i++) {
                $("#menu-tools a:eq(" + i + ")").animate({
                    right: 5,
                    opactiy: "show"
                }, 30 - i * 10);
            }
        } else {
            $(this).addClass("active");
            //显示菜单按钮
            for (var i = 2; i > -1 ; i--) {
                $("#menu-tools a:eq(" + i + ")").removeClass("hidden").animate({
                    right: 60 + (2 - i) * 55,   //距离右边距的距离
                    opactiy: "show"
                }, 10 + (2 - i) * 10);
            }
        }
    });
});