/* * 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
 * Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
 * 创建人：超级管理员
 * 日  期：2022-03-10 22:29
 * 描  述：项目管理
 */
var acceptClick;
var keyValue = request('keyValue');
// 设置权限
var setAuthorize;
// 设置表单数据
var setFormData;
// 验证数据是否填写完整
var validForm;
// 保存数据
var save;
var autoFlag;
var bootstrap = function ($, learun) {
    "use strict";
    // 设置权限
    setAuthorize = function (data) {
        if (!!data) {
            for (var field in data) {
                if (data[field].isLook != 1) {
                    // 如果没有查看权限就直接移除
                    $('#' + data[field].fieldId).parent().remove();
                }
                else {
                    if (data[field].isEdit != 1) {
                        $('#' + data[field].fieldId).attr('disabled', 'disabled');
                        if ($('#' + data[field].fieldId).hasClass('lrUploader-wrap')) {
                            $('#' + data[field].fieldId).css({ 'padding-right': '58px' });
                            $('#' + data[field].fieldId).find('.btn-success').remove();
                        }
                    }
                }
            }
        }
    };
    var page = {
        init: function () {
            $('.lr-form-wrap').lrscroll();
            //page.bind();
            //page.initData();
        },
        //bind: function () {
           
        //},
        //initData: function () {
        //    if (!!keyValue) {
        //        $.lrSetForm(top.$.rootUrl + '/LR_CodeDemo/ProjectManage/GetPreviewFormDataBy?keyValue=' + keyValue, function (data) {
        //            for (var id in data) {
        //                if (!!data[id].length && data[id].length > 0) {
        //                    $('#' + id).jfGridSet('refreshdata', data[id]);
        //                }
        //                else {
        //                    $('[data-table="' + id + '"]').lrSetFormData(data[id]);
        //                }
        //            }
        //        });
        //    }
        //}
    };
    // 设置表单数据
    //setFormData = function (processId, param, callback) {
    //    if (!!processId) {
    //        $.lrSetForm(top.$.rootUrl + '/LR_CodeDemo/ProjectManage/GetFormDataByProcessId?processId=' + processId, function (data) {
    //            for (var id in data) {
    //                if (!!data[id] && data[id].length > 0) {
    //                    $('#' + id).jfGridSet('refreshdata', data[id]);
    //                }
    //                else {
    //                    if (id == 'Project' && data[id]) {
    //                        keyValue = data[id].Id;
    //                    }
    //                    $('[data-table="' + id + '"]').lrSetFormData(data[id]);
    //                }
    //            }
    //        });
    //    }
       
    //    callback && callback();
    //}
    // 验证数据是否填写完整
    validForm = function () {
        if (!$('body').lrValidform()) {
            return false;
        }
        return true;
    };
    // 保存数据
    save = function (processId, callBack, i) {
        var formData = $('body').lrGetFormData();
        
        var postData = {
            strEntity: JSON.stringify(formData)
        };
        $.lrSaveForm(top.$.rootUrl + '/LR_CodeDemo/ReportForms/SaveFormReportForms?keyValue=' + keyValue , postData, function (res) {
            // 保存成功后才回调wd
            if (!!callBack) {
                callBack(res, i);
            }
        });
    };
    page.init();
}
