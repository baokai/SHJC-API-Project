/* * 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
 * Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
 * 创建人：超级管理员
 * 日  期：2022-03-11 00:04
 * 描  述：行政付款
 */
var refreshGirdData;
var bootstrap = function ($, learun) {
    "use strict";
    var startTime;
    var endTime;
    var processId = '';
    var page = {
        init: function () {
            page.initGird();
            page.bind();
        },
        bind: function () {
            // 查询 
            $('#btn_Search').on('click', function () {
                var keyword = $('#txt_Keyword').val();
                page.search({ keyword: keyword });
            });
            // 时间搜索框
            $('#datesearch').lrdate({
                dfdata: [
                    { name: '今天', begin: function () { return learun.getDate('yyyy-MM-dd 00:00:00') }, end: function () { return learun.getDate('yyyy-MM-dd 23:59:59') } },
                    { name: '近7天', begin: function () { return learun.getDate('yyyy-MM-dd 00:00:00', 'd', -6) }, end: function () { return learun.getDate('yyyy-MM-dd 23:59:59') } },
                    { name: '近1个月', begin: function () { return learun.getDate('yyyy-MM-dd 00:00:00', 'm', -1) }, end: function () { return learun.getDate('yyyy-MM-dd 23:59:59') } },
                    { name: '近3个月', begin: function () { return learun.getDate('yyyy-MM-dd 00:00:00', 'm', -3) }, end: function () { return learun.getDate('yyyy-MM-dd 23:59:59') } }
                ],
                // 月
                mShow: false,
                premShow: false,
                // 季度
                jShow: false,
                prejShow: false,
                // 年
                ysShow: false,
                yxShow: false,
                preyShow: false,
                yShow: false,
                // 默认
                dfvalue: '3',
                selectfn: function (begin, end) {
                    startTime = begin;
                    endTime = end;
                    page.search();
                }
            });
            $('#multiple_condition_query').lrMultipleQuery(function (queryJson) {
                page.search(queryJson);
            }, 320, 400);
            $('#PaymentStatus').lrDataItemSelect({ code: 'PaymentStatus', maxHeight: 230 });
            $('#PayType').lrDataItemSelect({ code: 'PaymentType', maxHeight: 230 });
            $('#F_PurchaseType').lrDataItemSelect({ code: 'menuTrage' });
            $('#F_Appler').lrDataSourceSelect({ code: 'userdata', value: 'f_userid', text: 'f_realname' });
            $('#F_Department').lrDataSourceSelect({ code: 'company', value: 'f_companyid', text: 'f_shortname' });
            $('#ContractSubmitter').lrformselect({
                layerUrl: top.$.rootUrl + '/LR_OrganizationModule/User/SelectForm',
                layerUrlW: 800,
                layerUrlH: 520,
                dataUrl: top.$.rootUrl + '/LR_OrganizationModule/User/GetListByUserIds'
            });
            $('#ProjectId').lrformselect({
                layerUrl: top.$.rootUrl + '/LR_CodeDemo/ProjectManage/SelectProjectForm',
                layerUrlW: 800,
                layerUrlH: 520,
                dataUrl: top.$.rootUrl + '/LR_CodeDemo/ProjectManage/GetEntityName',
                select: function (item) {
                }
            });
            $('#ProjectSource').lrRadioCheckbox({
                type: 'radio',
                code: 'ProjectSource',
            });
            // 刷新
            $('#lr_refresh').on('click', function () {
                location.reload();
            });
            //双击打开预览
            $("#gridtable").dblclick(function () {
                var keyValue = $('#gridtable').jfGridValue('Id');
                var projectId = $('#gridtable').jfGridValue('Pid');
                var projectName = $('#gridtable').jfGridValue('ProjectName');
                var paymentStatus = $('#gridtable').jfGridValue('PaymentStatus');
                if (keyValue == null) {
                    learun.alert.warning("当前项目暂时没有付款信息。");
                }
                else {
                    if (learun.checkrow(keyValue)) {
                        learun.layerForm({
                            id: 'form',
                            title: '行政付款(预览)',
                            url: top.$.rootUrl + '/LR_CodeDemo/Payment/PreviewForm?keyValue=' + keyValue + "&ProjectId=" + projectId,
                            width: 650,
                            height: 700,
                            callBack: function (id) {
                                if (paymentStatus != 1) {
                                    learun.alert.error("当前款项申请已经再审核，请勿修改。");
                                    return;
                                }
                                var res = false;
                                // 验证数据
                                res = top[id].validForm();
                                // 保存数据
                                if (res) {
                                    res = top[id].save('', function () {
                                        page.search();
                                    });
                                }
                                return res;
                            }
                        });
                    }
                }
            });
            //预览
            $('#lr_preview').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('Id');
                var projectId = $('#gridtable').jfGridValue('Pid');
                var projectName = $('#gridtable').jfGridValue('ProjectName');
                var paymentStatus = $('#gridtable').jfGridValue('PaymentStatus');
                if (keyValue == null) {
                    learun.alert.warning("当前项目暂时没有付款信息。");
                }
                else {
                    if (learun.checkrow(keyValue)) {
                        learun.layerForm({
                            id: 'form',
                            title:  '行政付款付款(预览)',
                            url: top.$.rootUrl + '/LR_CodeDemo/Payment/PreviewForm?keyValue=' + keyValue + "&ProjectId=" + projectId,
                            width: 650,
                            height: 700,
                            callBack: function (id) {
                                if (paymentStatus != 1) {
                                    learun.alert.error("当前款项申请已经再审核，请勿修改。");
                                    return;
                                }
                                var res = false;
                                // 验证数据
                                res = top[id].validForm();
                                // 保存数据
                                if (res) {
                                    res = top[id].save('', function () {
                                        page.search();
                                    });
                                }
                                return res;
                            }
                        });
                    }
                }
            })

            //表单打印
            $('#lr_Print').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('Id');
                var projectId = $('#gridtable').jfGridValue('Pid');
                var projectName = $('#gridtable').jfGridValue('ProjectName');
                var paymentStatus = $('#gridtable').jfGridValue('PaymentStatus');
                if (keyValue == null) {
                    learun.alert.warning("当前项目暂时没有付款信息。");
                }
                else {
                    if (learun.checkrow(keyValue)) {
                        learun.layerForm({
                            id: 'form',
                            title: projectName + '-付款(表单打印)',
                            url: top.$.rootUrl + '/LR_CodeDemo/Payment/PrintForm?keyValue=' + keyValue + "&ProjectId=" + projectId,
                            width: 860,
                            height: 1024,
                            callBack: function (id) {
                                if (paymentStatus != 1) {
                                    learun.alert.error("当前款项申请已经再审核，请勿修改。");
                                    return;
                                }
                                var res = false;
                                // 验证数据
                                res = top[id].validForm();
                                // 保存数据
                                if (res) {
                                    res = top[id].save('', function () {
                                        page.search();
                                    });
                                }
                                return res;
                            }
                        });
                    }
                }
            })
            // 新增
            $('#lr_add').on('click', function () {
                learun.layerForm({
                    id: 'form',
                    title: '付款(新增)',
                    url: top.$.rootUrl + '/LR_CodeDemo/Payment/Form',
                    width: 650,
                    height: 700,
                    callBack: function (id) {
                        var res = false;
                        // 验证数据
                        res = top[id].validForm();
                        // 保存数据
                        if (res) {
                            res = top[id].save("", refreshGirdData);
                        }
                        return res;
                    }
                });

            });
            // 编辑
            $('#lr_edit').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('Id');
                var projectId = $('#gridtable').jfGridValue('Pid');
                var projectName = $('#gridtable').jfGridValue('ProjectName');
                var paymentStatus = $('#gridtable').jfGridValue('PaymentStatus');
                if (keyValue == null) {
                    learun.alert.warning("当前项目暂时没有付款信息。");
                }
                else {
                    if (learun.checkrow(keyValue)) {
                       
                        learun.layerForm({
                            id: 'form',
                            title: '行政付款(编辑)',
                            url: top.$.rootUrl + '/LR_CodeDemo/Payment/Form?keyValue=' + keyValue + "&ProjectId=" + projectId + "&PaymentStatus=" + paymentStatus,
                            width: 650,
                            height: 700,
                            callBack: function (id) {

                                if (paymentStatus != 1& paymentStatus!=11) {

                               
                                    learun.alert.error("当前款项申请已经再审核，请勿修改。");
                                    return;
                                }
                                var res = false;
                                // 验证数据
                                res = top[id].validForm();
                                // 保存数据
                                if (res) {
                                    res = top[id].save('', function () {
                                        page.search();
                                    });
                                }
                                return res;
                            }
                        });
                    }
                }

            });
            // 审核
            $('#lr_approve').on('click', function () {
               /* $('#lr_approve').attr('disabled', true);
                setTimeout(() => {
                    console.log(123)
                    $('#lr_approve').attr('disabled', false);
                }, 1000)
                console.log(1)*/
                var keyValue = $('#gridtable').jfGridValue('Id');
                var workFlowId = $('#gridtable').jfGridValue('WorkFlowId');
                var paymentStatus = $('#gridtable').jfGridValue('PaymentStatus');
                var projectName = $('#gridtable').jfGridValue('ProjectName');
                if (paymentStatus != 1) {
                    if (paymentStatus == 11 && workFlowId != null) {
                        var postData = {
                            processId: workFlowId 
                        };
                        learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/Payment/UpdateFlowId', { keyValue: keyValue, ProcessId: workFlowId }, function (data) {
                            learun.httpAsync('Post', top.$.rootUrl + '/LR_NewWorkFlow/NWFProcess/AgainCreateFlow', postData, function (_data) {
                                learun.alert.success("提交成功");
                                learun.layerClose('', index); //再执行关闭  
                                refreshGirdData();
                            });
                        });
                    } else {
                        learun.alert.error("当前款项申请已经再审核，请勿重复提交。");
                        return;
                    }
                }
                else {
                    if (learun.checkrow(keyValue)) {
                        var lock = false;//未锁
                        learun.layerConfirm('是否确认提审该项目的款项申请！', function (res, index) {
                            if (res) {
                                if (!lock) {
                                    lock = true;//锁
                                    learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/Payment/UpdateFlowId', { keyValue: keyValue, ProcessId: workFlowId }, function (data) {
                                        learun.layerClose('', index); //再执行关闭  
                                        learun.alert.success("提交成功");
                                        refreshGirdData();
                                    });
                                }                             
                             /*   if (workFlowId) {
                                    var postData = {
                                        processId: workFlowId,
                                        keyValue: keyValue,
                                        schemeCode: 'Payment',// 填写流程对应模板编号
                                        level: '1',
                                        title: projectName
                                    };
                                    learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/Payment/UpdateFlowId', postData, function (data) {
                                        learun.httpAsync('Post', top.$.rootUrl + '/LR_NewWorkFlow/NWFProcess/AgainCreateFlow', postData, function (_data) {
                                            learun.layerClose('', index); //再执行关闭  
                                            learun.alert.success("提交成功");
                                            refreshGirdData();
                                        });
                                    });
                                } else {
                                    // 发起流程
                                    processId = learun.newGuid();
                                    var postData = {
                                        keyValue: keyValue,
                                        schemeCode: 'Payment',// 填写流程对应模板编号
                                        processId: processId,
                                        level: '1',
                                        title: projectName
                                    };
                                    //更新合同的状态并且创建审核流程
                                    learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/Payment/UpdateFlowId', { keyValue: keyValue, ProcessId: processId }, function (data) {
                                        learun.httpAsync('Post', top.$.rootUrl + '/LR_NewWorkFlow/NWFProcess/CreateFlow', postData, function (data) {
                                            learun.layerClose('', index); //再执行关闭  
                                            learun.alert.success("提交成功");
                                            refreshGirdData();
                                        });
                                    });
                                }*/
                            }
                        });
                    }
                }
            });

            // 变更
            $('#lr_change').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('Id');
                var workFlowId = $('#gridtable').jfGridValue('WorkFlowId');
                var paymentStatus = $('#gridtable').jfGridValue('PaymentStatus');
                if (paymentStatus != 4) {
                    learun.alert.error("审核通过才能变更!");
                    return;
                }
                else {
                    if (learun.checkrow(keyValue)) {
                        learun.layerConfirm('是否确认变更项目的款项申请！', function (res, index) {
                            if (res) {
                                learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/Payment/UpdateFlowIdStatus', { keyValue: keyValue, ProcessId: workFlowId }, function (data) {
                                    learun.layerClose('', index); //再执行关闭  
                                    learun.alert.success("变更成功");
                                    refreshGirdData();
                                });
                            }
                        })
                    }
                }
            })
            // 删除
            $('#lr_delete').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('Id');
                var paymentStatus = $('#gridtable').jfGridValue('PaymentStatus');
                if (paymentStatus != 1) {
                    learun.alert.error("当前款项申请已经再审核，请勿删除。");
                    return;
                } else {
                    if (learun.checkrow(keyValue)) {
                        learun.layerConfirm('是否确认删除该项！', function (res) {
                            if (res) {
                                learun.deleteForm(top.$.rootUrl + '/LR_CodeDemo/Payment/DeleteForm', { keyValue: keyValue }, function () {
                                    refreshGirdData();
                                });
                            }
                        });
                    }
                }
            });
            // 打印
            $('#lr_print').on('click', function () {
                $('#gridtable').jqprintTable();
            });
            //导出
            $('#lr-export').on('click', function () {
                learun.loading(true);
                $.QueryJson = $.QueryJson || {};             
                $.QueryJson.StartTime = startTime;
                $.QueryJson.EndTime = endTime;
                learun.httpAsync('Get', top.$.rootUrl + '/LR_CodeDemo/Payment/GetPageListAllDepartmentId', { queryJson: JSON.stringify($.QueryJson) }, function (data) {
                    var uuid = data.rows;
                    learun.download({
                        method: "POST",
                        url: '/Utility/ExportExcelPayment',
                        param: {
                            fileName: "导出行政付款",
                            columnJson: JSON.stringify($('#gridtable').jfGridGet('settingInfo').headData),
                            dataJson: uuid
                        }
                    });
                    learun.loading(false);
                });
            });
        },
        // 初始化列表
        initGird: function () {
            $('#gridtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/LR_CodeDemo/Payment/GetPageListDepartmentId',
                headData: [
                    {
                        label: "创建日期", name: "CreateTime", width: 90, align: "center",
                        formatter: function (cellvalue) {
                            return learun.formatDate(cellvalue, 'yyyy-MM-dd');
                        }
                    },
                    { label: "支付公司", name: "Payee", width: 250, align: "left" },
                    { label: "银行", name: "PayeeBank", width: 200, align: "left" },
                    { label: "外付收款银行账号", name: "BankAccount", width: 200, align: "left" },

                    { label: "创建人", name: "CreateUser", width: 250, align: "left" },
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
                        label: "当前审核人", name: "PaymentSubmitter", width: 90, align: "center",
                        formatterAsync: function (callback, value, row) {
                            learun.clientdata.getAsync('user', {
                                key: value,
                                callback: function (item) {
                                    callback(item.name);
                                }
                            });
                        }
                    },
                    { label: "支付金额", name: "PaymentAmount", width: 100, align: "left" },
                    {
                        label: "付款类型", name: "PayType", width: 100, align: "center",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'PaymentType',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        }
                    },
                    {
                        label: "支付方式", name: "PaymentMethod", width: 60, align: "center",
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
                    { label: "支付理由", name: "PaymentReason", width: 250, align: "left" },
                    {
                        label: "支付状态", name: "PaymentStatus", width: 70, align: "left",
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
        },
        search: function (param) {
            param = param || {};
            param.StartTime = startTime;
            param.EndTime = endTime;
            $('#gridtable').jfGridSet('reload', { queryJson: JSON.stringify(param) });
        }
    };
    refreshGirdData = function (res, postData) {
        page.search();
    };
    page.init();
}
