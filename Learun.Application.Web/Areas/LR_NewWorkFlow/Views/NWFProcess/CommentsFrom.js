/*
 * 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
 * Copyright (c) 2013-2020 上海力软信息技术有限公司
 * 创建人：力软-前端开发组
 * 日 期：2018.12.09
 * 描 述：选择发起流程模板
 */
var acceptClick;
var bootstrap = function ($, learun) {
    "use strict";
    var processId = request('processId');      // 流程实例主键



    var page = {
        init: function () {
            page.bind();
        },
        bind: function () {
        }
    };
    // 保存数据
    acceptClick = function (processId, callBack) {
        var formData = $('body').lrGetFormData();
        formData.WorkFlowId = processId;
        var postData = {         
                strEntity: JSON.stringify(formData)
        };
        
        $.lrSaveForm(top.$.rootUrl + '/LR_NewWorkFlow/NWFProcess/CommentsSave', postData, function (res) {
           
                // 保存成功后才回调
                if (!!callBack) {
                    callBack(res, i);
                }
            });
        };
    page.init();
}