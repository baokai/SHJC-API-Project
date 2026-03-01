
/* * 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
 * Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
 * 创建人：超级管理员
 * 日  期：2022-03-11 00:59
 * 描  述：项目开票
 */
var acceptClick;
var keyValue = request('keyValue');
var projectId = request('ProjectId');
var BillingStatus = request('BillingStatus');
var contractNo = "";
// 设置权限
var setAuthorize;
// 设置表单数据
var setFormData;
// 验证数据是否填写完整
var validForm;
// 保存数据
var save;
var bootstrap = function ($, learun) {
    "use strict";
    // 设置权限
    setAuthorize = function (data, isLook) {
        if (!!data) {
            for (var field in data) {
                if (data[field].isLook != 1) {// 如果没有查看权限就直接移除
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
            page.bind();
            page.initData();
        },
        bind: function () {
        
            $('#ReportFile').lrUploader({ isDown: true, isView: true, isUpload: false });
           
          
        },
        initData: function () {
          
            if (!!keyValue) {
                $.lrSetForm(top.$.rootUrl + '/LR_CodeDemo/ProjectBilling/GetPriewFormData?keyValue=' + keyValue, function (data) {
                    for (var id in data) {
                        if (!!data[id].length && data[id].length > 0) {
                            $('#' + id).jfGridSet('refreshdata', data[id]);
                        }
                        else {
                            $('[data-table="' + id + '"]').lrSetFormData(data[id]);

                        }
                    }
                });
            }
            $("#ProjectSourceId").hide();
            //$("#ReportFileId").hide();
        }
    };
    // 设置表单数据
    setFormData = function (processId, param, callback) {
        $("#ProjectSourceId").show();
        if (!!processId) {
            $.lrSetForm(top.$.rootUrl + '/LR_CodeDemo/ProjectBilling/GetFormDataByProcessId?processId=' + processId, function (data) {
                var projectId = data["ProjectBilling"].ProjectId;
                $.lrSetForm(top.$.rootUrl + '/LR_CodeDemo/ProjectManage/GetFormData?keyValue=' + projectId, function (data1) {
                    for (var id1 in data1) {
                        $('[data-table="' + id1 + '"]').lrSetFormData(data1[id1]);
                    }
                });
                for (var id in data) {
                    if (!!data[id] && data[id].length > 0) {
                        $('#' + id).jfGridSet('refreshdata', data[id]);
                    }
                    else {
                        if (id == 'ProjectBilling' && data[id]) {
                            keyValue = data[id].Id;
                        }
                        $('[data-table="' + id + '"]').lrSetFormData(data[id]);

                    }
                }
            });
        }
       

        callback && callback();
    }
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
        if (!!processId) {
            formData.WorkFlowId = processId;
        }
        var postData = {
            strEntity: JSON.stringify(formData)
        };
        if (contractNo == 1) {
            alert("合同编号为空不能添加")
            contractNo = 0;
            return;
        } else {
            $.lrSaveForm(top.$.rootUrl + '/LR_CodeDemo/ProjectBilling/SaveForm?keyValue=' + keyValue, postData, function (res) {
                // 保存成功后才回调
                if (!!callBack) {
                    callBack(res, i);
                }
            });
        }

    };
    page.init();
}
