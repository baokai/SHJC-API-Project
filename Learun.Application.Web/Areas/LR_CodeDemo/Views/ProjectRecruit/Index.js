/* * 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
 * Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
 * 创建人：超级管理员
 * 日  期：2022-03-16 18:06
 * 描  述：用工申请
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
            }, 290, 400);
            $('#F_PurchaseType').lrDataItemSelect({ code: 'menuTrage' });
            $('#F_Appler').lrDataSourceSelect({ code: 'userdata', value: 'f_userid', text: 'f_realname' });
            $('#F_Department').lrDataSourceSelect({ code: 'company', value: 'f_companyid', text: 'f_shortname' });

            $('#ProjectId').lrformselect({
                layerUrl: top.$.rootUrl + '/LR_CodeDemo/ProjectManage/SelectProjectForm',
                layerUrlW: 800,
                layerUrlH: 520,
                dataUrl: top.$.rootUrl + '/LR_CodeDemo/ProjectManage/GetEntityName',
                select: function (item) {
                    $("#ContractNo").val(item.ContractNo)
                }
            });
            $('#ApplyPerson').lrformselect({
                layerUrl: top.$.rootUrl + '/LR_OrganizationModule/User/SelectForm',
                layerUrlW: 800,
                layerUrlH: 520,
                dataUrl: top.$.rootUrl + '/LR_OrganizationModule/User/GetListByUserIds'
            });
            // 刷新
            $('#lr_refresh').on('click', function () {
                location.reload();
            });
            //双击打开预览
            $("#gridtable").dblclick(function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '用工预览',
                        url: top.$.rootUrl + '/LR_CodeDemo/ProjectRecruit/PreviewForm?keyValue=' + keyValue,
                        width: 700,
                        height: 800,
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
            });
            // 预览
            $('#lr_preview').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '用工预览',
                        url: top.$.rootUrl + '/LR_CodeDemo/ProjectRecruit/PreviewForm?keyValue=' + keyValue,
                        width: 700,
                        height: 800,
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
            });
            // 审核
            $('#lr_approve').on('click', function () {
               
                var keyValue = $('#gridtable').jfGridValue('id');
                var workFlowId = $('#gridtable').jfGridValue('WorkFlowId');
                var recruitStatus = $('#gridtable').jfGridValue('RecruitStatus');
                var projectName = $('#gridtable').jfGridValue('ProjectName');
                if (recruitStatus != 1) {
                    if (recruitStatus == 11 && workFlowId != '') {
                        var postData = {
                            processId: workFlowId
                        };
                        learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectRecruit/UpdateFlowId', { keyValue: keyValue, ProcessId: workFlowId }, function (data) {
                            learun.httpAsync('Post', top.$.rootUrl + '/LR_NewWorkFlow/NWFProcess/AgainCreateFlow', postData, function (_data) {
                                learun.alert.success("提交成功");
                                learun.layerClose('', index); //再执行关闭  
                                refreshGirdData();
                            });
                        });
                    } else {
                        learun.alert.error("当前用工已经再审核，请勿重复提交。");
                        return;
                    }  
                }
                else {
                    if (learun.checkrow(keyValue)) {
                        var lock = false;//未锁
                        learun.layerConfirm('是否确认提审该项目的合同！', function (res, index) {
                            if (res) {
                                if (!lock) {
                                    lock = true;
                                    learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectRecruit/UpdateFlowId', { keyValue: keyValue, ProcessId: workFlowId }, function (data) {
                                        learun.layerClose('', index); //再执行关闭  
                                        learun.alert.success("提交成功");
                                        refreshGirdData();
                                    });
                                }
                                /*if (workFlowId) {
                                    var postData = {
                                        processId: workFlowId
                                    };
                                    learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectRecruit/UpdateFlowId', { keyValue: keyValue, ProcessId: workFlowId }, function (data) {
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
                                        schemeCode: 'ProjectRecruit',// 填写流程对应模板编号
                                        processId: processId,
                                        level: '1',
                                        title: projectName
                                    };
                                    //更新合同的状态并且创建审核流程
                                    learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectRecruit/UpdateFlowId', { keyValue: keyValue, ProcessId: processId }, function (data) {
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
            // 新增
            $('#lr_add').on('click', function () {
                learun.layerForm({
                    id: 'form',
                    title: '新增',
                    url: top.$.rootUrl + '/LR_CodeDemo/ProjectRecruit/Form',
                    width: 700,
                    height: 800,
                    callBack: function (id) {
                        var res = false;
                        // 验证数据
                        res = top[id].validForm();
                        // 保存数据
                        /*if (res) {
                            res = top[id].save(refreshGirdData);
                        }*/
                      if (res) {
                            res = top[id].save("", function () {
                                page.search();
                            });
                        }
                        return res;
                    }
                });
            });
            // 编辑
            $('#lr_edit').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                var recruitStatus = $('#gridtable').jfGridValue('RecruitStatus');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '编辑',
                        url: top.$.rootUrl + '/LR_CodeDemo/ProjectRecruit/Form?keyValue=' + keyValue,
                        width: 700,
                        height: 800,
                        callBack: function (id) {
                            if (recruitStatus != 1 & recruitStatus!=11) {
                                learun.alert.error("当前用工已经再审核，请勿重复提交。");
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
            });
            // 删除
            $('#lr_delete').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                if (learun.checkrow(keyValue)) {
                    var recruitStatus = $('#gridtable').jfGridValue('RecruitStatus');
                    if (recruitStatus != 1) {
                        learun.alert.error("当前用工已经再审核，请勿重复提交。");
                        return;
                    }
                    learun.layerConfirm('是否确认删除该项！', function (res) {
                        if (res) {
                            learun.deleteForm(top.$.rootUrl + '/LR_CodeDemo/ProjectRecruit/DeleteForm', { keyValue: keyValue }, function () {
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
                url: top.$.rootUrl + '/LR_CodeDemo/ProjectRecruit/GetPageList',
                headData: [
                    {
                        label: "创建日期", name: "CreateTime", width: 90, align: "center",
                        formatter: function (cellvalue) {
                            return learun.formatDate(cellvalue, 'yyyy-MM-dd');
                        }                      },
                    {
                        label: "合同编号", name: "ContractNo", width: 105, align: "",
                    },
                    {
                        label: "项目名称", name: "ProjectName", width: 300, align: "left",
                    },
                    {
                        label: "申请人", name: "ApplyPerson", width: 65, align: "center",
                        formatterAsync: function (callback, value, row) {
                            learun.clientdata.getAsync('user', {
                                key: value,
                                callback: function (item) {
                                    callback(item.name);
                                }
                            });
                        }
                    },
                    { label: "用工资源", name: "WageSource", width: 80, align: "center" },
                    { label: "作业类型", name: "JobType", width: 100, align: "left" },
                    {
                        label: "现场用工时间", name: "WorkingTime", width: 90, align: "center",
                        formatter: function (cellvalue) {
                            return learun.formatDate(cellvalue, 'yyyy-MM-dd');
                        }                      },
                    { label: "人数", name: "PersonQty", width: 60, align: "left" },
                    { label: "用工单价", name: "Price", width: 80, align: "left" },
                    { label: "总金额", name: "Amount", width: 80, align: "left" },
                    { label: "收款人/单位", name: "PayeeUnit", width: 100, align: "left" },
                    { label: "收款银行", name: "PayeeBank", width: 145, align: "center" },
                    { label: "银行账号", name: "PayeeAccount", width: 145, align: "center" },
                    {
                        label: "付款方式", name: "PaymentMethod", width: 70, align: "center",
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
                        label: "申请状态", name: "RecruitStatus", width: 70, align: "center",
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
                    { label: "备注", name: "Remark", width: 160, align: "left" }
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
