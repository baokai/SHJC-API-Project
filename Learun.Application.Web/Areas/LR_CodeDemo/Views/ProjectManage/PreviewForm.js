
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
            page.initGird();
        },
        bind: function () {
            // 编辑
            $('#lr_form_tabs').lrFormTab();
            $('#lr-projectCode').on('click', function () {
                learun.httpAsync('GET', top.$.rootUrl + '/LR_SystemModule/CodeRule/GetEnCode', { code: '10002' }, function (data) {
                    if (!$('#ProjectCode').val()) {
                        $('#ProjectCode').val(data);
                        autoFlag = 1;
                    }
                });
            });
            $('#ProjectStatus').lrRadioCheckbox({
                type: 'radio',
                code: 'projectStatus'
            });
            $('#TenderFlg').lrRadioCheckbox({
                type: 'radio',
                code: 'TenderFlg',
                fristSelected: true
            });
            $('#FollowPerson').lrformselect({
                layerUrl: top.$.rootUrl + '/LR_OrganizationModule/User/SelectForm',
                layerUrlW: 800,
                layerUrlH: 520,
                dataUrl: top.$.rootUrl + '/LR_OrganizationModule/User/GetListByUserIds'
            });
            var loginInfo = learun.clientdata.get(['userinfo']);
            var userId = loginInfo.userId;
            $('#PreparedPerson').lrformselect({
                layerUrl: top.$.rootUrl + '/LR_OrganizationModule/User/SelectForm',
                layerUrlW: 800,
                layerUrlH: 520,
                dataUrl: top.$.rootUrl + '/LR_OrganizationModule/User/GetListByUserIds'
            });
            $('#ProjectSource').lrRadioCheckbox({
                type: 'radio',
                code: 'ProjectSource',
            });
            $('#PreparedPerson').lrformselectSet(userId);
            // 省市区
            $('#area').lrAreaSelect();
        },
        initData: function () {
            if (!!keyValue) {
                $.lrSetForm(top.$.rootUrl + '/LR_CodeDemo/ProjectManage/GetFormData?keyValue=' + keyValue, function (data) {
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
        },
         // 初始化列表
        initGird: function () {
            $('#contractTable').jfGrid({
                url: top.$.rootUrl + '/LR_CodeDemo/ProjectContract/GetPageList',
                headData: [
                    {
                        label: "创建日期", name: "CreateTime", width: 90, align: "center",
                        formatter: function (cellvalue) {
                            return learun.formatDate(cellvalue, 'yyyy-MM-dd');
                        }
                    },
                    { label: "合同编号", name: "ContractNo", width: 110, align: "center" },
                    {
                        label: "项目名称", name: "ProjectName", width: 300, align: "left",

                    },
                    {
                        label: "委托单位", name: "CustName", width: 200, align: "left",

                    },

                    {
                        label: "合同主体", name: "ContractSubject", width: 70, align: "center",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'ContractSubject',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        }
                    },
                    {
                        label: "营销部门", name: "DepartmentId", width: 70, align: "center",
                        formatterAsync: function (callback, value, row) {
                            learun.clientdata.getAsync('department', {
                                key: value,
                                callback: function (item) {
                                    callback(item.name);
                                }
                            });
                        }

                    },
                    {
                        label: "营销人员", name: "FollowPerson", width: 70, align: "center",
                        formatterAsync: function (callback, value, row) {
                            learun.clientdata.getAsync('user', {
                                key: value,
                                callback: function (item) {
                                    callback(item.name);
                                }
                            });
                        }

                    },
                    {
                        label: "项目来源", name: "ProjectSource", width: 80, align: "center",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'ProjectSource',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        }

                    },
                    {
                        label: "合同状态", name: "ContractStatus", width: 80, align: "center",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'ContractStatus',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        }
                    },

                    { label: "合同金额", name: "ContractAmount", width: 90, align: "center" },
                    { label: "有效合同额", name: "EffectiveAmount", width: 90, align: "center" },
                    {
                        label: "合同分类", name: "ContractType", width: 80, align: "center",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'ContractType',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        }
                    },
                    //{
                    //    label: "是否收到合同", name: "ReceivedFlag", width: 100, align: "left",
                    //    formatterAsync: function (callback, value, row, op, $cell) {
                    //        learun.clientdata.getAsync('dataItem', {
                    //            key: value,
                    //            code: 'YesOrNo',
                    //            callback: function (_data) {
                    //                callback(_data.text);
                    //            }
                    //        });
                    //    }
                    //},
                ],
                sidx: 'CreateTime',
                sord: 'DESC',
                mainId: 'id',
                isPage: true
            });
            $('#taskTable').jfGrid({
                url: top.$.rootUrl + '/LR_CodeDemo/ProjectTask/GetPageList',
                headData: [
                    {
                        label: "创建日期", name: "CreateTime", width: 90, align: "center",
                        formatter: function (cellvalue) {
                            return learun.formatDate(cellvalue, 'yyyy-MM-dd');
                        }
                    },
                    {
                        label: "合同编号", name: "ContractNo", width: 120, align: "center",
                    },
                    {
                        label: "项目名称", name: "ProjectName", width: 300, align: "left",
                    },
                    {
                        label: "委托单位", name: "CustName", width: 250, align: "left",
                    },
                    //{
                    //    label: "合同主体", name: "ContractSubject", width: 90, align: "left",
                    //    formatterAsync: function (callback, value, row, op, $cell) {
                    //        learun.clientdata.getAsync('dataItem', {
                    //            key: value,
                    //            code: 'ContractSubject',
                    //            callback: function (_data) {
                    //                callback(_data.text);
                    //            }
                    //        });
                    //    }
                    //},
                    {
                        label: "所属部门", name: "DepartmentId", width: 90, align: "center",
                        formatterAsync: function (callback, value, row) {
                            learun.clientdata.getAsync('department', {
                                key: value,
                                callback: function (item) {
                                    callback(item.name);
                                }
                            });
                        }
                    },
                    {
                        label: "项目负责人", name: "ProjectResponsible", width: 90, align: "center",
                        formatterAsync: function (callback, value, row) {
                            learun.clientdata.getAsync('user', {
                                key: value,
                                callback: function (item) {
                                    callback(item.name);
                                }
                            });
                        }
                    },
                    {
                        label: "检测员", name: "Inspector", width: 90, align: "center",
                        formatterAsync: function (callback, value, row) {
                            learun.clientdata.getAsync('user', {
                                key: value,
                                callback: function (item) {
                                    callback(item.name);
                                }
                            });
                        }
                    },
                    {
                        label: "进场时间", name: "ApproachTime", width: 100, align: "center",
                        formatter: function (cellvalue) {
                            return learun.formatDate(cellvalue, 'yyyy-MM-dd');
                        }                    },
                    {
                        label: "报告计划时间", name: "PlanTime", width: 0, align: "center",
                        formatter: function (cellvalue) {
                            return learun.formatDate(cellvalue, 'yyyy-MM-dd');
                        }                    },
                    {
                        label: "报告状态", name: "TaskStatus", width: 80, align: "center",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'TaskStatus',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        }
                    }
                ],
                mainId: 'id',
                sidx: 'CreateTime',
                sord: 'DESC',
                isPage: true
            });
            $('#recruitTable').jfGrid({
                url: top.$.rootUrl + '/LR_CodeDemo/ProjectRecruit/GetPageList',
                headData: [
                    {
                        label: "创建日期", name: "CreateTime", width: 75, align: "left",
                        formatter: function (cellvalue) {
                            return learun.formatDate(cellvalue, 'yyyy-MM-dd');
                        }
                    },
                    {
                        label: "合同编号", name: "ContractNo", width: 105, align: "left",
                    },
                    {
                        label: "项目名称", name: "ProjectName", width: 260, align: "left",
                    },
                    {
                        label: "申请人", name: "ApplyPerson", width: 60, align: "left",
                        formatterAsync: function (callback, value, row) {
                            learun.clientdata.getAsync('user', {
                                key: value,
                                callback: function (item) {
                                    callback(item.name);
                                }
                            });
                        }
                    },
                    { label: "用工资源", name: "WageSource", width: 70, align: "left" },
                    { label: "作业类型", name: "JobType", width: 80, align: "left" },
                    { label: "人数", name: "PersonQty", width: 50, align: "left" },
                    { label: "用工单价", name: "Price", width: 60, align: "left" },
                    { label: "总金额", name: "Amount", width: 70, align: "left" },
                    { label: "收款人/单位", name: "PayeeUnit", width: 100, align: "left" },
                    { label: "收款银行", name: "PayeeBank", width: 155, align: "left" },
                    { label: "银行账号", name: "PayeeAccount", width: 165, align: "left" },
                    {
                        label: "付款方式", name: "PaymentMethod", width: 60, align: "left",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'PaymentMethod',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        }
                    },
                    {
                        label: "申请状态", name: "RecruitStatus", width: 80, align: "left",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'RecruitStatus',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        }
                    },
                ],
                mainId: 'id',
                sidx: 'CreateTime',
                sord: 'DESC',
                isPage: true
            });
            $('#billingtable').jfGrid({
                url: top.$.rootUrl + '/LR_CodeDemo/ProjectBilling/GetPageList',
                headData: [
                    {
                        label: "创建日期", name: "CreateTime", width: 120, align: "left",
                        formatter: function (cellvalue) {
                            return learun.formatDate(cellvalue, 'yyyy-MM-dd');
                        }                    },
                    {
                        label: "项目名称", name: "ProjectName", width: 350, align: "left",
                    },
                    { label: "开票金额", name: "BillingAmount", width: 100, align: "left" },
                    {
                        label: "开票内容", name: "BillingContent", width: 100, align: "left",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'BillingContent',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        }
                    },
                    { label: "开票信息", name: "BillingInformation", width: 100, align: "left" },
                    {
                        label: "开票类型", name: "BillingType", width: 100, align: "left",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'BillingType',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        }
                    },
                    { label: "备注", name: "Remark", width: 100, align: "left" },
                    {
                        label: "开票单位", name: "BillingUnit", width: 100, align: "left",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'ContractSubject',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        }
                    },
                    {
                        label: "开票状态", name: "BillingStatus", width: 100, align: "left",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'BillingStatus',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        }
                    },
                ],
                mainId: 'Id',
                sidx: 'CreateTime',
                sord: 'DESC',
                isPage: true
            });
            $('#payCollectionTable').jfGrid({
                url: top.$.rootUrl + '/LR_CodeDemo/ProjectPayCollection/GetPageList',
                headData: [
                    {
                        label: "创建日期", name: "CreateTime", width: 100, align: "left",
                        formatter: function (cellvalue) {
                            return learun.formatDate(cellvalue, 'yyyy-MM-dd');
                        }                    },
                    { label: "项目名称", name: "ProjectName", width: 250, align: "left" },
                    { label: "委托单位", name: "CustName", width: 250, align: "left" },
                    { label: "合同编号", name: "ContractNo", width: 150, align: "left" },
                    { label: "本次到款金额", name: "Amount", width: 100, align: "left" },
                    { label: "到账日期", name: "ReceiptDate", width: 100, align: "left" },
                    { label: "到款单位", name: "PaymentUnit", width: 100, align: "left" },
                ],
                mainId: 'id',
                isPage: true
            });
            $('#paymentTable').jfGrid({
                url: top.$.rootUrl + '/LR_CodeDemo/ProjectPayment/GetPageList',
                headData: [
                    {
                        label: "创建日期", name: "CreateTime", width: 75, align: "left",
                        formatter: function (cellvalue) {
                            return learun.formatDate(cellvalue, 'yyyy-MM-dd');
                        }                    },
                    {
                        label: "项目名称", name: "ProjectName", width: 280, align: "left",

                    },
                    {
                        label: "委托单位", name: "CustName", width: 150, align: "left",

                    },
                    {
                        label: "项目来源", name: "ProjectSource", width: 70, align: "center",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'ProjectSource',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        }

                    },
                    { label: "支付公司", name: "Payee", width: 170, align: "left" },
                    { label: "银行", name: "PayeeBank", width: 170, align: "left" },
                    { label: "支付金额", name: "PaymentAmount", width: 70, align: "center" },
                    {
                        label: "支付方式", name: "PaymentMethod", width: 70, align: "center",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'Client_PaymentMode',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        }
                    },
                    {
                        label: "支付抬头", name: "PaymentHeader", width: 65, align: "center",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'PaymentHeader',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        }
                    },
                    { label: "支付理由", name: "PaymentReason", width: 180, align: "left" },
                    {
                        label: "支付状态", name: "PaymentStatus", width: 65, align: "center",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'PaymentStatus',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        }
                    },
                ],
                mainId: 'id',
                sidx: 'CreateTime',
                sord: 'DESC',
                isPage: true
            });
            var param = {};
            param.ProjectId = keyValue;
           
            $('#taskTable').jfGridSet('reload', { queryJson: JSON.stringify(param) });
            $('#recruitTable').jfGridSet('reload', { queryJson: JSON.stringify(param) });
            $('#billingtable').jfGridSet('reload', { queryJson: JSON.stringify(param) });
            $('#payCollectionTable').jfGridSet('reload', { queryJson: JSON.stringify(param) });
            $('#paymentTable').jfGridSet('reload', { queryJson: JSON.stringify(param) });
            $('#contractTable').jfGridSet('reload', { queryJson: JSON.stringify(param) });
        },
    };
    // 设置表单数据
    setFormData = function (processId, param, callback) {
        if (!!processId) {
            $.lrSetForm(top.$.rootUrl + '/LR_CodeDemo/ProjectManage/GetFormDataByProcessId?processId=' + processId, function (data) {
                for (var id in data) {
                    if (!!data[id] && data[id].length > 0) {
                        $('#' + id).jfGridSet('refreshdata', data[id]);
                    }
                    else {
                        if (id == 'Project' && data[id]) {
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
        $.lrSaveForm(top.$.rootUrl + '/LR_CodeDemo/ProjectManage/SaveForm?keyValue=' + keyValue + "&autoFlag=" + autoFlag, postData, function (res) {
            // 保存成功后才回调
            if (!!callBack) {
                callBack(res, i);
            }
        });
    };
    page.init();
}
