/// <reference path="http://g.alicdn.com/ilw/ding/0.3.8/scripts/dingtalk.js?v=2697369616_6771" />

//钉钉就绪函数
dd.ready(function () {

    //时间选择器
    function getTime(f, v) {
        dd.biz.util.timepicker({
            format: f,
            value: v,
            onSuccess: function (time) {
                //返回选择的时间
                return time.value;
            }, onFail: function (err) {
            }
        });
    }


    function test() {
        return "Hello World！";
    }

});