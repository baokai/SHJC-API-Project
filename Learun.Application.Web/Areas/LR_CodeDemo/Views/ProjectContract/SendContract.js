/* * 
 * 日  期：2022-03-10 23:22
 * 描  述：项目合同申请
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
            autoFlag = 0;
            $('#ContractType').lrRadioCheckbox({
                type: 'radio',
                code: 'ContractType',
            });
            $('#ContractStatus').lrRadioCheckbox({
                type: 'radio',
                code: 'ContractStatus',
            });
            $('#MainDepartmentId').lrDepartmentSelect();
            $('#SubDepartmentId').lrDepartmentSelect();
            $('#DepartmentId').lrDepartmentSelect();
            $('#ContractSubject').lrDataItemSelect({ code: 'ContractSubject', maxHeight: 230 });
            $('#ProjectSource').lrRadioCheckbox({
                type: 'radio',
                code: 'ProjectSource',
            });
            $('#ReceiptType').lrRadioCheckbox({
                type: 'radio',
                code: 'ReceiptType',
            });
            // 编辑
            $('#lr-contract').on('click', function () {
                learun.httpAsync('GET', top.$.rootUrl + '/LR_SystemModule/CodeRule/GetEnCode', { code: '10010' }, function (data) {
                    if (!$('#ContractNo').val()) {
                        $('#ContractNo').val(data);
                        autoFlag = 1;
                    }
                });
            });
            $('#lr-contract').on('click', function () {
                var formData = $('body').lrGetFormData();
                learun.httpAsync('GET', top.$.rootUrl + '/LR_SystemModule/CodeRule/GetEnCode', { code: '1' }, function (data) {
                    if (!$('#ReceivedFlagNo').val()) {
                        $('#ReceivedFlagNo').val(data);
                        autoFlag = 1;
                    }
                });
                /*if (formData.ContractType == 1) {
                    learun.httpAsync('GET', top.$.rootUrl + '/LR_SystemModule/CodeRule/GetEnCode', { code: '10010' }, function (data) {
                        if (!$('#ContractNo').val()) {
                            $('#ContractNo').val(data);
                            autoFlag = 1;
                        }
                    });
                } else {
                    learun.httpAsync('GET', top.$.rootUrl + '/LR_SystemModule/CodeRule/GetEnCode', { code: '10011' }, function (data) {
                        if (!$('#ContractNo').val()) {
                            $('#ContractNo').val(data);
                            autoFlag = 1;
                        }
                    });
                }*/
               /* $("#ContractType").on('change', function () {
                    $('#ReceivedFlagNo').val("")
                });*/
            });
            $('#FollowPerson').lrformselect({
                layerUrl: top.$.rootUrl + '/LR_OrganizationModule/User/SelectForm',
                layerUrlW: 800,
                layerUrlH: 520,
                dataUrl: top.$.rootUrl + '/LR_OrganizationModule/User/GetListByUserIds'
            });
         //   $('#ContractFile').lrUploader();
            $('#ProjectId').lrformselect({
                layerUrl: top.$.rootUrl + '/LR_CodeDemo/ProjectManage/SelectProjectForm',
                layerUrlW: 800,
                layerUrlH: 520,
                dataUrl: top.$.rootUrl + '/LR_CodeDemo/ProjectManage/GetEntityName',
                select: function (item) {
                    console.log(item)
                    var projectId = item.Id;
                    $.lrSetForm(top.$.rootUrl + '/LR_CodeDemo/ProjectManage/GetFormData?keyValue=' + projectId, function (data1) {
                        for (var id1 in data1) {
                            $('[data-table="' + id1 + '"]').lrSetFormData(data1[id1]);
                        }
                    });
                }
            });
        },
        initData: function () {
            if (!!keyValue) {

                $.lrSetForm(top.$.rootUrl + '/LR_CodeDemo/ProjectContract/GetFormData?keyValue=' + keyValue, function (data) {
                    var projectId = data["ProjectContract"].ProjectId;
                    $.lrSetForm(top.$.rootUrl + '/LR_CodeDemo/ProjectManage/GetFormData?keyValue=' + projectId, function (data1) {
                        for (var id1 in data1) {
                            $('[data-table="' + id1 + '"]').lrSetFormData(data1[id1]);
                        }
                    });
                    for (var id in data) {
                        if (!!data[id].length && data[id].length > 0) {
                            $('#' + id).jfGridSet('refreshdata', data[id]);
                        }
                        else {
                            console.log(data[id])
                            $('[data-table="' + id + '"]').lrSetFormData(data[id]);
                        }
                    }
                });
            }
            $('input,textarea').attr('readonly', 'readonly');
            $('.lr-select').attr('readonly', 'readonly'); 
            $("#Remark").removeAttr("readonly");
        }
    };
    // 设置表单数据
    setFormData = function (processId, param, callback) {
        if (!!processId) {
            $.lrSetForm(top.$.rootUrl + '/LR_CodeDemo/ProjectContract/GetFormDataByProcessId?processId=' + processId, function (data) {
                var projectId = data["ProjectContract"].ProjectId;

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
                        if (id == 'ProjectContract' && data[id]) {
                            keyValue = data[id].id;
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
        $.lrSaveForm(top.$.rootUrl + '/LR_CodeDemo/ProjectContract/UpdateReceivedFlag?keyValue=' + keyValue + "&autoFlag=" + autoFlag, postData, function (res) {
            // 保存成功后才回调
            if (!!callBack) {
                callBack(res, i);
            }
        });
    };
    page.init();
}
