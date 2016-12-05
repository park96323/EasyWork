/// <reference path="dropDownList.js" />

$(function () {
    var json = [{ "key": "1", "value": "鸡老板都换手机了" },
        { "key": "2", "value": "---------------" },
        { "key": "3", "value": "陈涛是傻逼" }];
    $("#rd_1").click(function () {
        $("#box1").css("display", "block")
        $("#box2").css("display", "none")
        $("#btnSel").text("查看已预约会议室");
    });
    $("#rd_2").click(function () {
        $("#box2").css("display", "block")
        $("#box1").css("display", "none")
        $("#btnSel").text("查找可预约会议室");
    });
   
    $("#btnSel").click(function () {
        
        if ($("#box1").css("display")=='block')//按时间查找
        {
            $.ajax({
                type: 'POST',
                url: "/MeetingRoom/LookRoom1",
                data: {
                    timeStart: $("#timeStart").val(),
                    timeEnd: $("#timeEnd").val(),
                },
                dataType: "json",
                success: function (dict) {
                    var s=dict.split(",");
                    location.href = "/MeetingRoom/LookRoom?timeStart="+s[0]+"&timeEnd="+s[1];
                }
            });
            
        }
        else//按楼层查找
        {
            //$.ajax({
            //    type: 'POST',
            //    url: "/MeetingRoom/SelLc1",
            //    data: {
            //        wifi: $("#wifi").val(),
            //        projection:$("#projection").val()
            //    },
            //    dataType: "json",
            //    success: function (Lc) {
            //        //alert("Lc")
            //       // location.href = "/MeetingRoom/SelLc?Lc="+Lc;
            //    }
            //});
            location.href = "/MeetingRoom/SelRoom?wifi=" + $("#wifi").is(':checked') + "&projection="+$("#projection").is(':checked');
           
        }
    })
    //$.dropDownList("dorpDownList1", 350, 50, json, 1);
})
