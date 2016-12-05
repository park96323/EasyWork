/// <reference path="sweetalert.min.js" />

$(function () {
    var i = 0;//0表示点击未读，1表示点击已读，2表示点击全部
    //默认根据设备屏宽检查并修改样式
    auto_styles();

    //当屏幕大小改变时，修改样式
    window.onresize = function () {
        auto_styles();
    }
    //验证标题不能为空
    $("#txtTitle").blur(function () {


        if ($("#txtTitle").val() != "") {
            $("#CheckTitle").hide();
        }
        else if ($("#txtTitle").val().length <= 20) {
            $("#CheckTitle").hide();
        }

    });
    $("#txtContent").blur(function () {
        if ($("#txtContent").val() != "") {
            $("#CheckContent").hide();
        }
    });
    //添加公告
    $("#btn_add").click(function () {
        window.location.href = "../Notice/AddNotice";
    })

    function auto_styles() {
        var _width = $(document.body).width();
        if (_width <= 570) {
            $("#main_content").css("margin-top", "0px");
            $("#btn_tools span:eq(0)").removeAttr("class").addClass("glyphicon glyphicon-option-vertical");
            $(".top-menu").slideUp("slow");
            $("body").css({ "marginTop": 0 });
        }
        else {
            $("#main_content").css("margin-top", "10px");
            $("#btn_tools span:eq(0)").removeAttr("class").addClass("fa fa-plus");
            $(".top-menu").slideDown("slow");
            $("body").css({ "marginTop": 55 });

            //电脑显示时隐藏按钮组
            btn_hide();
        }
    }

    //点击小工具按钮的事件
    $("#btn_tools button:gt(3)").click(function () {
        if ($(document.body).width() > 570 || $(this).attr("id") == "btn_add") {
            window.location.href = "../Notice/AddNotice";
        }
        else {
            if (!$(this).hasClass("active")) {
                $(this).addClass("active");

                $("#btn_tools button:eq(3)").removeClass("hidden").animate({
                    right: 60,
                    opactiy: 'show'
                }, 10, function () {
                    $("#btn_tools button:eq(2)").removeClass("hidden").animate({
                        right: 115,
                        opactiy: 'show'
                    }, 20, function () {
                        $("#btn_tools button:eq(1)").removeClass("hidden").animate({
                            right: 170,
                            opactiy: 'show'
                        }, 30, function () {
                            $("#btn_tools button:eq(0)").animate({
                                right: 225,
                                opactiy: 'show'
                            }, 40, function () {

                            });
                        });
                    });
                });
            } else {
                $(this).removeClass("active");
                btn_hide();
            }
        }
    });
    function NewDate(str) {
        var time = new Date(str);
        str = str.split('-');
        var s = time.getSeconds();
        s = s < 10 ? ('0' + s) : s;
        var date = str[0] + '/' + str[1] + '/' + str[2].substring(0, 2) + ' ' + str[2].substring(3, 5) + str[2].substring(5, 8) + ':' + s;
        return date;
    }


    //按钮菜单点击事件
    $("#btn-menu a").click(function () {
        $("#btn-menu a").each(function () {     //把所以项的active类删除
            $(this).removeClass("active").css({ height: "40px", color: "#000000", borderBottom: "0px solid #1A97F0" });
        });
        $(this).addClass("active").css({ height: "40px", color: " #1A97F0", borderBottom: "2px solid #1A97F0" });
        switch ($(this).attr("id")) {
            case "notioce_unread":
                //alert("你点击了未读选项！");
                break;
            case "notioce_read":
                //alert("你点击了已读选项！");
                break;
            default:
                //alert("你点击了全部选项！");
                break;
        }
    });

    //点击未读
    $("#notioce_unread,#notioce_unread2").click(function () {
        $("#block_unread").show();
        $("#block_all").html("");
        $("#block_read").html("");
        i = 0;

    })

    //点击已读
    $("#notioce_read,#notioce_read2").click(function () {
        i = 1;
        $("#block_unread").hide();
        $("#block_read").html("");
        $("#block_all").html("");
        $.post("../Notice/Index1", {}, function (back) {
            var result = JSON.parse(back);
            var s = "";
            for (var item in result) {
                var image = result[item].A_Image == "" ? '../Content/Images/notice.jpg' : image = result[item].A_Image;
                var s1 = '<div class="col-xs-12 col-sm-6 col-md-4 col-lg-3"><div class="media" style="padding:12px 5px;border-bottom:1px solid #808184;margin:0px 3px;width:97%;"><a class="pull-left" href="/Notice/Details?id=' + result[item].A_ID + '&i=1"><img class="img-circle media-object" src="' + image + '" style="width:80px;height:80px;display:block;" /></a><div class="media-body" style="margin-top:10px;"><h4 class="media-heading">' + result[item].A_Title + '</h4> <p>' + result[item].Name + '</p> <p>' + NewDate(result[item].A_Time) + '</p></div> </div> </div>';
                s += s1;
            }

            $("#block_read").html(s);
        });

    })
    //点击全部

    $("#notioce_all,#notioce_all2").click(function () {
        i = 2;
        $("#block_read").html("");
        $("#block_unread").hide();
        $.post("../Notice/Index2", {}, function (back) {
            var result = JSON.parse(back);
            var s = "";
            for (var item in result) {
                var image = result[item].A_Image == "" ? '../Content/Images/notice.jpg' : image = result[item].A_Image;
                var s1 = '<div class="col-xs-12 col-sm-6 col-md-4 col-lg-3"><div class="media" style="padding:12px 5px;border-bottom:1px solid #808184;margin:0px 3px;width:97%;"><a class="pull-left" href="/Notice/Details?id=' + result[item].A_ID + '&i=2"><img class="img-circle media-object" src="' + image + '" style="width:80px;height:80px;display:block;" /></a><div class="media-body" style="margin-top:10px;"><h4 class="media-heading">' + result[item].A_Title + '</h4> <p>' + result[item].Name + '</p> <p>' + NewDate(result[item].A_Time) + '</p></div> </div> </div>';
                s += s1;
            }

            $("#block_all").html(s);

        });
    })

    //工具按钮组隐藏的方法
    function btn_hide() {
        $("#btn_tools button:eq(0)").animate({
            right: 5,
            opactiy: 'show'
        }, 40, function () {
            $("#btn_tools button:eq(1)").animate({
                right: 5,
                opactiy: 'show'
            }, 30, function () {
                $("#btn_tools button:eq(2)").animate({
                    right: 5,
                    opactiy: 'show'
                }, 20, function () {
                    $("#btn_tools button:eq(3)").animate({
                        right: 5,
                        opactiy: 'show'
                    }, 10, function () {
                        $(this).addClass("hidden");
                    });
                    $(this).addClass("hidden");
                });
                $(this).addClass("hidden");
            });
        });
    }
});