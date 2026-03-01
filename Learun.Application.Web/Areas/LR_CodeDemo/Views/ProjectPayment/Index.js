/* * 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
 * Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
 * 创建人：超级管理员
 * 日  期：2022-03-11 00:04
 * 描  述：合同支付
 */
var refreshGirdData;
var bootstrap = function ($, learun) {
    "use strict";
    var startTime;
    var endTime;
    var processId = '';
    var select;
    var querselect;
    var quer;
    var page = {
        init: function () {
            page.initGird();
            page.bind();
        },
        bind: function () {
            // 查询 
            $('#btn_Search').on('click', function () {
                select=1;
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

                $('#txt_Keyword').val("")
                querselect = 2;
                quer = queryJson;
               
                page.search(queryJson);
            }, 360, 400);
            $('#PaymentStatus').lrDataItemSelect({ code: 'PaymentStatus', maxHeight: 230 });
            $('#F_PurchaseType').lrDataItemSelect({ code: 'menuTrage' });
            $('#F_Appler').lrDataSourceSelect({ code: 'userdata', value: 'f_userid', text: 'f_realname' });
            $('#F_Department').lrDataSourceSelect({ code: 'company', value: 'f_companyid', text: 'f_shortname' });
            $('#PayType').lrDataItemSelect({ code: 'PayType', maxHeight: 230 });

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
                var keyValue = $('#gridtable').jfGridValue('id');
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
                            title: projectName + '-付款(预览)',
                            url: top.$.rootUrl + '/LR_CodeDemo/ProjectPayment/PreviewForm?keyValue=' + keyValue + "&ProjectId=" + projectId,
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
                var keyValue = $('#gridtable').jfGridValue('id');
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
                            title: projectName + '-付款(预览)',
                            url: top.$.rootUrl + '/LR_CodeDemo/ProjectPayment/PreviewForm?keyValue=' + keyValue + "&ProjectId=" + projectId,
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
            // 批量添加
            $('#lr_batchAudit').on('click', function () {

                learun.layerForm({
                    id: 'BatchAuditIndex',
                    title: '批量添加',
                    url: top.$.rootUrl + '/LR_CodeDemo/ProjectPayment/BatchAuditAdd',
                    height: 700,
                    width: 920,
                    maxmin: true,
                    btn: null,
                    //end: function () {
                    //    location.reload();
                    //}
                });
                
            });
            //表单打印
            $('#lr_Print').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
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
                            url: top.$.rootUrl + '/LR_CodeDemo/ProjectPayment/PrintForm?keyValue=' + keyValue + "&ProjectId=" + projectId,
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
                    url: top.$.rootUrl + '/LR_CodeDemo/ProjectPayment/Form',
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
                var keyValue = $('#gridtable').jfGridValue('id');
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
                            title: projectName + '-付款(编辑)',
                            url: top.$.rootUrl + '/LR_CodeDemo/ProjectPayment/Form?keyValue=' + keyValue + "&ProjectId=" + projectId + "&PaymentStatus=" + paymentStatus,
                            width: 650,
                            height: 700,
                            callBack: function (id) {
                                if (paymentStatus != 1 & paymentStatus!=11) {
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
                var keyValue = $('#gridtable').jfGridValue('id');
                var workFlowId = $('#gridtable').jfGridValue('WorkFlowId');
                var paymentStatus = $('#gridtable').jfGridValue('PaymentStatus');
                var projectName = $('#gridtable').jfGridValue('ProjectName');
                alert(workFlowId)
                if (paymentStatus != 1) {
                    var postData = {  
                        processId: workFlowId
                    };
                    if (paymentStatus == 11 && workFlowId != '') {
                        learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectPayment/UpdateFlowId', { keyValue: keyValue, ProcessId: workFlowId }, function (data) {
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
                                    lock = true;
                                    learun.layerClose('', index); //再执行关闭  
                                    learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectPayment/UpdateFlowId', { keyValue: keyValue, ProcessId: workFlowId }, function (data) {
                                        learun.alert.success("提交成功");
                                        refreshGirdData();
                                    });
                                }

                                /*if (workFlowId) {
                                    var postData = {
                                        keyValue: keyValue,
                                        schemeCode: 'ProjectPayment',// 填写流程对应模板编号
                                        processId: workFlowId,
                                        level: '1',
                                        title: projectName
                                    };
                                    learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectPayment/UpdateFlowId', postData, function (data) {

                                        learun.httpAsync('Post', top.$.rootUrl + '/LR_NewWorkFlow/NWFProcess/AgainCreateFlow', postData, function (_data) {

                                            learun.layerClose('', index); //再执行关闭  
                                            //learun.alert.success("提交成功");
                                            refreshGirdData();
                                        });
                                    });
                                } else {
                                    // 发起流程
                                    processId = learun.newGuid();
                                    var postData = {
                                        keyValue: keyValue,
                                        schemeCode: 'ProjectPayment',// 填写流程对应模板编号
                                        processId: processId,
                                        level: '1',
                                        title: projectName
                                    };
                                    //更新合同的状态并且创建审核流程
                                    learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectPayment/UpdateFlowId', { keyValue: keyValue, ProcessId: processId }, function (data) {
                                        if (data == null) {
                                            learun.layerClose('', index); //再执行关闭 
                                            return;
                                        }
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
                var keyValue = $('#gridtable').jfGridValue('id');
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
                                learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectPayment/UpdateFlowIdStatus', { keyValue: keyValue, ProcessId: workFlowId }, function (data) {
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
                var keyValue = $('#gridtable').jfGridValue('id');
                var paymentStatus = $('#gridtable').jfGridValue('PaymentStatus');
                if (paymentStatus != 1) {
                    learun.alert.error("只能删除草稿状态的数据!");
                    return;
                } else {
                    if (learun.checkrow(keyValue)) {
                        learun.layerConfirm('是否确认删除该项！', function (res) {
                            if (res) {
                                learun.deleteForm(top.$.rootUrl + '/LR_CodeDemo/ProjectPayment/DeleteForm', { keyValue: keyValue }, function () {
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
                if (select == 1) {
                    var keyword = $('#txt_Keyword').val();
                    $.QueryJson.keyword = keyword;
                }
                if (querselect == 2) {
                    $.QueryJson.ContractNo = quer.ContractNo;
                    $.QueryJson.ProjectName = quer.ProjectName;
                    $.QueryJson.CustName = quer.CustName; 
                    $.QueryJson.ProjectSource = quer.ProjectSource;
                    $.QueryJson.Payee = quer.Payee;
                    $.QueryJson.PaymentAmount = quer.PaymentAmount;
                    $.QueryJson.PaymentStatus = quer.PaymentStatus;
                    $.QueryJson.PayType = quer.PayType;
                }
                $.QueryJson.StartTime = startTime;
                $.QueryJson.EndTime = endTime;
                learun.httpAsync('Get', top.$.rootUrl + '/LR_CodeDemo/ProjectPayment/GetPageListAll', { queryJson: JSON.stringify($.QueryJson) }, function (data) {
                    var uuid = data.rows;
                    learun.download({
                        method: "POST",
                        url: '/Utility/ExportExcelProjectPayment',
                        param: {
                            fileName: "导出付款",
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
                url: top.$.rootUrl + '/LR_CodeDemo/ProjectPayment/GetPageList',
                headData: [
                    {
                        label: "创建日期", name: "CreateTime", width: 90, align: "center",
                        formatter: function (cellvalue) {
                            return learun.formatDate(cellvalue, 'yyyy-MM-dd');
                        }                      },
                    {
                        label: "项目名称", name: "ProjectName", width: 250, align: "left",

                    },
                    {
                        label: "委托单位", name: "CustName", width: 200, align: "left",

                    },
                    {
                        label: "项目来源", name: "ProjectSource", width: 75, align: "center",
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
                    { label: "收款单位名称", name: "Payee", width: 250, align: "left" },
                    { label: "银行", name: "PayeeBank", width: 250, align: "left" },
                    {
                        label: "营销部门", name: "DepartmentId", width: 90, align: "center",
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
                        label: "付款类型", name: "PayType", width: 110, align: "center",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'PayType',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        }
                    },
                    { label: "支付金额", name: "PaymentAmount", width: 100, align: "left" },
                    {
                        label: "支付方式", name: "PaymentMethod", width: 90, align: "center",
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
                        label: "支付抬头", name: "PaymentHeader", width: 70, align: "center",
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
                    { label: "支付理由", name: "PaymentReason", width: 150, align: "left" },
                    {
                        label: "支付状态", name: "PaymentStatus", width: 80, align: "left",
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
