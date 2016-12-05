/// <reference path="jquery-2.1.4.min.js" />
/// <reference path="sweetalert.min.js" />
$(function () {
    $("#del").click(function () {
        $.ajax({
            type: 'POST',
            url: "/Notice/Delete",
            data: {
            },
            dataType: "json"
        });
        location.href = '../Notice/Index';

        //$.getJSON("../Notice/Index", {}, function () {

        //});
    })
    
    //上一页
    $(".previous").click(function () {
        var id=$("#aid").val();
        
        $.post("../Notice/Previous", {}, function (back) {
            var str=back.split(',');
            var j=str[1];
            location.href = "../Notice/Details?id="+str[0]+ '&i='+j;
        })
        })

    //下一页
    $(".next").click(function () {
            var id = $("#aid").val();
            $.post("../Notice/Previous1", {}, function (back) {
                var str = back.split(',');
                var j = str[1];
                location.href = "../Notice/Details?id=" + str[0] + '&i=' + j;
        })
    })
})