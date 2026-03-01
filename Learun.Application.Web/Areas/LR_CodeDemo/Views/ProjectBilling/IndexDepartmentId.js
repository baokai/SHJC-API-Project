/* * 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
 * Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
 * 创建人：超级管理员
 * 日  期：2022-03-11 00:59
 * 描  述：项目开票
 */
var refreshGirdData;
var bootstrap = function ($, learun) {
    "use strict";
    var startTime;
    var endTime;
    var datelist;
    var processId = '';
    var select;
    var querselect;
    var quer;
    var page = {
        init: function () {
            page.initGird();
            page.bind();
            page.sum();
        },
        sum: function () {
            learun.loading(true);
            $.QueryJson = $.QueryJson || {};
            $.QueryJson.StartTime = startTime;
            $.QueryJson.EndTime = endTime;
            if (select == 1) {
                var keyword = $('#txt_Keyword').val();
                $.QueryJson.keyword = keyword;
            }
            if (querselect == 2) {
                $.QueryJson.BillingStatus = quer.BillingStatus;
                $.QueryJson.BillingUnit = quer.BillingUnit;
                $.QueryJson.BillingAmount = quer.BillingAmount;
                $.QueryJson.ContractNo = quer.ContractNo;
                $.QueryJson.ProjectId = quer.ProjectId;

            }
            learun.httpAsync('Get', top.$.rootUrl + '/LR_CodeDemo/ProjectBilling/GetPageListAllSUMDepartmentId', { queryJson: JSON.stringify($.QueryJson) }, function (data) {
                $("#BillingAmountSUM").val(data.BillingAmountSUM.toFixed(2));
            });
            learun.loading(false);
        },
        bind: function () {
            // 查询 
            $('#btn_Search').on('click', function () {
                select = 1;
                var keyword = $('#txt_Keyword').val();
                page.search({ keyword: keyword });
                page.sum();
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
                    page.sum();
                }
            });
            $('#multiple_condition_query').lrMultipleQuery(function (queryJson) {
                $('#txt_Keyword').val("")
                querselect = 2;
                quer = queryJson;
                page.search(queryJson);
                page.sum();
            }, 250, 400);
            $('#F_PurchaseType').lrDataItemSelect({ code: 'menuTrage' });
            $('#F_Appler').lrDataSourceSelect({ code: 'userdata', value: 'f_userid', text: 'f_realname' });
            $('#F_Department').lrDataSourceSelect({ code: 'company', value: 'f_companyid', text: 'f_shortname' });

            $('#ProjectId').lrformselect({
                layerUrl: top.$.rootUrl + '/LR_CodeDemo/ProjectManage/SelectProjectForm',
                layerUrlW: 800,
                layerUrlH: 520,
                dataUrl: top.$.rootUrl + '/LR_CodeDemo/ProjectManage/GetEntityName',
                select: function (item) { }
            });
            $('#BillingUnit').lrDataItemSelect({ code: 'ContractSubject', maxHeight: 230 });
            $('#BillingStatus').lrDataItemSelect({ code: 'BillingStatus', maxHeight: 230 });
            // 刷新
            $('#lr_refresh').on('click', function () {
                location.reload();
            });
            //导出
            $('#lr-export').on('click', function () {
                learun.loading(true);
                $.QueryJson = $.QueryJson || {}; 
                $.QueryJson.StartTime = startTime;
                $.QueryJson.EndTime = endTime;
                if (select == 1) {
                    var keyword = $('#txt_Keyword').val();
                    $.QueryJson.keyword = keyword;
                }
                if (querselect == 2) {
                    $.QueryJson.BillingStatus = quer.BillingStatus;
                    $.QueryJson.BillingUnit = quer.BillingUnit;
                    $.QueryJson.BillingAmount = quer.BillingAmount;
                    $.QueryJson.ContractNo = quer.ContractNo;
                    $.QueryJson.ProjectId = quer.ProjectId;
                   
                }
                learun.httpAsync('Get', top.$.rootUrl + '/LR_CodeDemo/ProjectBilling/GetPageListAllDepartmentId', { queryJson: JSON.stringify($.QueryJson) }, function (data) {
                    var uuid = data.rows;
                    learun.download({
                        method: "POST",
                        url: '/Utility/ExportExcelBilling',
                        param: {
                            fileName: "导出开票",
                            columnJson: JSON.stringify($('#gridtable').jfGridGet('settingInfo').headData),
                            dataJson: uuid
                        }
                    });
                    learun.loading(false);
                });

            });
            // 取消
            $('#lr_cancel').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('Id');             
                var projectName = $('#gridtable').jfGridValue('ProjectName');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: projectName + '-开票(取消)',
                        url: top.$.rootUrl + '/LR_CodeDemo/ProjectBilling/ZuofeiForm?keyValue=' + keyValue,
                        width: 800,
                        height: 300,
                        callBack: function (id) {
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

               /* if (learun.checkrow(keyValue)) {
                    learun.layerConfirm('是否确认取消该项！', function (res) {
                        if (res) {
                            learun.postForm(top.$.rootUrl + '/LR_CodeDemo/ProjectBilling/Zuofei', { keyValue: keyValue }, function () {
                                refreshGirdData();
                            });
                        }
                    });
                }*/
            });
            //双击打开预览
            $("#gridtable").dblclick(function () {
                var keyValue = $('#gridtable').jfGridValue('Id');
                var projectName = $('#gridtable').jfGridValue('ProjectName');
                if (keyValue == null) {
                    learun.alert.warning("请选择预览数据");
                }
                else {
                    if (learun.checkrow(keyValue)) {
                        learun.layerForm({
                            id: 'form',
                            title: projectName + '-开票预览',
                            url: top.$.rootUrl + '/LR_CodeDemo/ProjectBilling/FormId?keyValue=' + keyValue,
                            width: 600,
                            height: 600,
                            callBack: function (id) {

                                var res = false;
                                // 验证数据
                                res = top[id].validForm();
                                return res;
                            }
                        });
                    }
                }
            });
            //预览
            $('#lr_preview').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('Id');
                var projectName = $('#gridtable').jfGridValue('ProjectName');
                if (keyValue == null) {
                    learun.alert.warning("请选择预览数据");
                }
                else {
                    if (learun.checkrow(keyValue)) {
                        learun.layerForm({
                            id: 'form',
                            title: projectName + '-开票预览',
                            url: top.$.rootUrl + '/LR_CodeDemo/ProjectBilling/FormId?keyValue=' + keyValue,
                            width: 600,
                            height: 600,

                        });
                    }
                }
            });

            // 新增
            $('#lr_add').on('click', function () {
                learun.layerForm({
                    id: 'form',
                    title: '开票申请新增',
                    url: top.$.rootUrl + '/LR_CodeDemo/ProjectBilling/FormDepartmentId',
                    width: 600,
                    height: 600,
                    callBack: function (id) {
                        var res = false;
                        // 验证数据
                        res = top[id].validForm();
                        // 保存数据
                        if (res) {
                            res = top[id].save("", refreshGirdData);
                        }
                        return res;
                    },
                });

            });

            // 审核
            $('#lr_approve').on('click', function () {
                
                var keyValue = $('#gridtable').jfGridValue('Id');
                var workFlowId = $('#gridtable').jfGridValue('WorkFlowId');
                var BillingStatus = $('#gridtable').jfGridValue('BillingStatus');
                var projectName = $('#gridtable').jfGridValue('ProjectName');
                if (BillingStatus != 1) {
                    if (BillingStatus == 11 && workFlowId != '') {
                        var postData = {
                            processId: workFlowId
                        };
                        learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectBilling/UpdateFlowId', { keyValue: keyValue, ProcessId: workFlowId }, function (data) {
                            learun.httpAsync('Post', top.$.rootUrl + '/LR_NewWorkFlow/NWFProcess/AgainCreateFlow', postData, function (_data) {
                                learun.alert.success("提交成功");
                                learun.layerClose('', index); //再执行关闭
                                refreshGirdData();
                            });
                        });
                    } else {
                        learun.alert.error("当前开票已经再审核，请勿重复提交。");
                        return;
                    }
                    
                }
                else {
                    if (learun.checkrow(keyValue)) {
                        var lock = false;//未锁
                        learun.layerConfirm('是否确认提审该项目的开票！', function (res, index) {
                            if (res) {
                                if (!lock) {
                                    lock = true;//锁
                                    learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectBilling/UpdateFlowId', { keyValue: keyValue, ProcessId: workFlowId }, function (data) {
                                        learun.layerClose('', index); //再执行关闭  
                                        learun.alert.success("提交成功");
                                        refreshGirdData();
                                    });
                                }
                                
                                /*if (workFlowId) {
                                    var postData = {
                                        processId: workFlowId
                                    };
                                    learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectBilling/UpdateFlowId', { keyValue: keyValue, ProcessId: workFlowId }, function (data) {
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
                                        schemeCode: 'ProjectBilling',// 填写流程对应模板编号
                                        processId: processId,
                                        level: '1',
                                        title: projectName
                                    };  
                                    //更新合同的状态并且创建审核流程
                                    learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectBilling/UpdateFlowId', { keyValue: keyValue, ProcessId: processId }, function (data) {
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
                var BillingStatus = $('#gridtable').jfGridValue('BillingStatus');
                var projectName = $('#gridtable').jfGridValue('ProjectName');
                if (BillingStatus != 4 && BillingStatus != 7) {
                    learun.alert.error("审核通过才能变更!");
                    return;
                }
                else {
                    if (learun.checkrow(keyValue)) {
                        learun.layerConfirm('是否确认变更项目的开票！', function (res, index) {
                            if (res) {
                                learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectBilling/UpdateContractStatus', { keyValue: keyValue, ProcessId: workFlowId }, function (data) {
                                    learun.layerClose('', index); //再执行关闭  
                                    learun.alert.success("变更成功");
                                    refreshGirdData();
                                });
                            }
                        })
                    }
                }
            })
            // 编辑
            $('#lr_edit').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('Id');
                var BillingStatus = $('#gridtable').jfGridValue('BillingStatus');
                var projectName = $('#gridtable').jfGridValue('ProjectName');
                if (keyValue == null) {
                    learun.alert.warning("当前项目暂时没有开过票。");
                }
                else {
                    if (learun.checkrow(keyValue)) {

                        
                        learun.layerForm({
                            id: 'form',
                            title: projectName + '-开票修改',
                            url: top.$.rootUrl + '/LR_CodeDemo/ProjectBilling/FormDepartmentId?keyValue=' + keyValue + '&BillingStatus=' + BillingStatus,
                            width: 600,
                            height: 600,
                            callBack: function (id) {

                                if (BillingStatus != 1 & BillingStatus!=11) {

                                    learun.alert.error("当前开票已经再审核，请勿重复提交。");
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
            // 删除
            $('#lr_delete').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('Id');
                if (learun.checkrow(keyValue)) {
                    var BillingStatus = $('#gridtable').jfGridValue('BillingStatus');
                    if (BillingStatus != 1) {
                        learun.alert.error("当前开票已经再审核，请勿删除。");
                        return;
                    }
                    learun.layerConfirm('是否确认删除该项！', function (res) {
                        if (res) {
                            learun.deleteForm(top.$.rootUrl + '/LR_CodeDemo/ProjectBilling/DeleteForm', { keyValue: keyValue }, function () {
                                refreshGirdData();
                            });
                        }
                    });
                }
            });
            // 打印
            $('#lr_print').on('click', function () {
                $('#gridtable').jqprintTable();
            });
        },
        // 初始化列表
        initGird: function () {
            $('#gridtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/LR_CodeDemo/ProjectBilling/GetPageListDepartmentId',
                headData: [
                    {
                        label: "创建日期", name: "CreateTime", width: 90, align: "center",
                        formatter: function (cellvalue) {
                            return learun.formatDate(cellvalue, 'yyyy-MM-dd');
                        }                     },
                    {
                        label: "项目名称", name: "ProjectName", width: 350, align: "left",
                    },
                    { label: "开票金额", name: "BillingAmount", width: 100, align: "left" },
                    {
                        label: "开票内容", name: "BillingContent", width: 80, align: "center",
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
                    { label: "开票信息", name: "BillingInformation", width: 400, align: "left" },
                    {
                        label: "开票类型", name: "BillingType", width: 70, align: "center",
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
                    { label: "发票备注", name: "Remark", width: 300, align: "left" },
                    {
                        label: "开票单位", name: "BillingUnit", width: 70, align: "center",
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
                        label: "开票状态", name: "BillingStatus", width: 80, align: "center",
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
                    {
                        label: "营销部门", name: "FDepartmentId", width: 90, align: "center",
                        formatterAsync: function (callback, value, row) {
                            learun.clientdata.getAsync('department', {
                                key: value,
                                callback: function (item) {
                                    callback(item.name);
                                }
                            });
                        }

                    },
                    { label: "备注", name: "B_Remark", width: 150, align: "left" }
                ],
                mainId: 'Id',
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
            //learun.httpAsync('Get', top.$.rootUrl + '/LR_CodeDemo/ProjectBilling/GetPageListAll', { queryJson: JSON.stringify(param) }, function (data) {
            //    $("#BillingAmountSUM").val(data.BillingAmountSUM.toFixed(2));
               
            //});
        }
    };
    refreshGirdData = function (res, postData) {
        page.search();
    };
    page.init();
}
