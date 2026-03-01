/*
 * 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
 * Copyright (c) 2013-2020 上海力软信息技术有限公司
 * 创建人：力软-前端开发组
 * 日 期：2017.04.18
 * 描 述：创建流程
 */
// 验证数据是否填写完整
var validForm;
// 保存数据
var save;
var bootstrap = function ($, learun) {
    "use strict";
    var processId = request('processId');      // 流程进程主键
    var taskId = request('taskId');            // 流程任务主键
    var page = {
        init: function () { }
    };
    
    // 验证数据是否填写完整
    validForm = function () {
        if (!$('body').lrValidform()) {
            return false;
        }
        return true;
    };
    // 保存数据
    save = function (callBack) {
        var formData = $('body').lrGetFormData();
        formData.nodId = taskId;
        formData.WorkFlowId = processId;
        console.log(formData)
        var postData = {
            strEntity: JSON.stringify(formData)
        };
        $.lrSaveForm(top.$.rootUrl + '/LR_NewWorkFlow/NWFProcess/CommentsSave', postData, function (res) {
            // 保存成功后才回调
            if (!!callBack) {
                callBack(res);
            }
        });

    };
    page.init();
}