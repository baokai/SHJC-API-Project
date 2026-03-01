/*
 * 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
 * Copyright (c) 2013-2020 上海力软信息技术有限公司
 * 创建人：力软-前端开发组
 * 日 期：2019.08.27
 * 描 述：单据编码
 */
//style="padding:20px 100px 20px 100px;"
var keyValue = request('keyValue');
var bootstrap = function ($, learun) {
    "use strict";

    function makeCode() {
        var count =1;
        if (count > 100) { return false; }
        var text = $("#txt_code").val();
        var myCode = new Array();
        $("#qrcode").html("");
        for (var i = 0; i < count; ++i) {
            myCode[i] = text;
            var html = '<div id="qrcode' + i + '" style=" "><div>';
            $("#qrcode").append(html);
            var codeValue = myCode[i] + "?QRCode=" + keyValue
            console.log(codeValue)
            var qrcode = new QRCode(document.getElementById("qrcode" + i), {
                width: 200,
                height: 200
            });
            qrcode.makeCode(codeValue);
        }
    }

    makeCode();

    //打印
    $('#lr-print').on('click', function () {
        $("#qrcode").jqprint();
    });

    //生成
    $('#lr-ok').on('click', function () {
        makeCode();
    });
}
