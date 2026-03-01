/* * 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
 * Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
 * 创建人：超级管理员
 * 日  期：2022-03-11 00:18
 * 描  述：项目任务单
 */
var refreshGirdData;
var refreshRepotGirdData;
var myDate = new Date;
var datetime = ([myDate.getFullYear(), myDate.getMonth() + 1, myDate.getDate()].join('-'));
var bootstrap = function ($, learun) {
    "use strict";
    var startTime;
    var endTime;
    var processId = '';
    var select;
    var select1;
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
                select = 1;
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
                querselect = 2;
                quer = queryJson;
                page.search(queryJson);
            }, 360, 400);
           
            $('#lr_timeout').on('click', function () {
                select1 = 0;
                var YCS = -1;
                page.search({ YCS: YCS });
               
                
               
            });
            $('#ProjectResponsible').lrformselect({
                layerUrl: top.$.rootUrl + '/LR_OrganizationModule/User/SelectForm',
                layerUrlW: 800,
                layerUrlH: 520,
                dataUrl: top.$.rootUrl + '/LR_OrganizationModule/User/GetListByUserIds'
            });
            $('#TaskStatus').lrDataItemSelect({ code: 'TaskStatus', maxHeight: 230 });
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
                        title: '任务管理预览',
                        url: top.$.rootUrl + '/LR_CodeDemo/ProjectTask/PreviewForm?keyValue=' + keyValue,
                        width: 600,
                        height: 850,
                        callBack: function (id) {

                            var res = false;

                            // 验证数据
                            res = top[id].validForm();
                            //保存数据
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
            //导出

            $('#lr-export').on('click', function () {
                learun.loading(true);
                $.QueryJson = $.QueryJson || {};
                if (select == 1) {
                    var keyword = $('#txt_Keyword').val();
                    $.QueryJson.keyword = keyword;
                }
                if (select1 == 0) {
                    var YCS = -1;
                    $.QueryJson.YCS = YCS;
                }
                if (querselect == 2) {
                    $.QueryJson.ContractNo = quer.ContractNo;
                    $.QueryJson.ProjectName = quer.ProjectName;
                    $.QueryJson.CustName = quer.CustName;
                    $.QueryJson.ContactPhone = quer.ContactPhone;
                    $.QueryJson.ProjectResponsible = quer.ProjectResponsible;
                    $.QueryJson.ApproachTime = quer.ApproachTime;
                    $.QueryJson.PlanTime = quer.PlanTime;
                    $.QueryJson.TaskStatus = quer.TaskStatus;
                }
                $.QueryJson.StartTime = startTime;
                $.QueryJson.EndTime = endTime;
                learun.httpAsync('Get', top.$.rootUrl + '/LR_CodeDemo/ProjectTask/GetPageListAll', { queryJson: JSON.stringify($.QueryJson) }, function (data) {
                    var uuid = data.rows;
                    learun.download({
                        method: "POST",
                        url: '/Utility/ExportExcelTask',
                        param: {
                            fileName: "导出报告",
                            columnJson: JSON.stringify($('#gridtable').jfGridGet('settingInfo').headData),
                            dataJson: uuid
                        }
                    });
                    learun.loading(false);
                });
            });
            // 合同预览
            $('#lr_Contractpreview').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('ProjectId');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '合同管理预览',
                        url: top.$.rootUrl + '/LR_CodeDemo/ProjectTask/ContractPreviewForm?keyValue=' + keyValue,
                        width: 600,
                        height: 500,
                        callBack: function (id) {

                            var res = false;

                            // 验证数据
                            res = top[id].validForm();
                            //保存数据
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
                        title: '任务管理预览',
                        url: top.$.rootUrl + '/LR_CodeDemo/ProjectTask/PreviewForm?keyValue=' + keyValue,
                        width: 600,
                        height: 850,
                        callBack: function (id) {

                            var res = false;

                            // 验证数据
                            res = top[id].validForm();
                            //保存数据
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
            //二维码
            $('#QRCode').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
               
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '生成二维码',
                        url: top.$.rootUrl + '/LR_CodeDemo/ProjectTask/CreateQRCode?keyValue=' + keyValue,
                        width: 400,
                        height: 510,
                        callBack: function (id) {
                        }
                    });
                }
            });
            // 新增
            $('#lr_add').on('click', function () {
                learun.layerForm({
                    id: 'form',
                    title: '新增',
                    url: top.$.rootUrl + '/LR_CodeDemo/ProjectTask/Form',
                    width: 600,
                    height: 800,
                    callBack: function (id) {
                        var res = false;
                        // 验证数据
                        res = top[id].validForm();
                        // 保存数据
                        if (res) {
                            res = top[id].save(processId, refreshGirdData);
                        }
                        return res;
                    }
                });
            });
            //变更
            $('#lr_change').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                var taskStatus = $('#gridtable').jfGridValue('TaskStatus');
                if (taskStatus != 5) {
                    learun.alert.error("已完成才能变更!");
                    return;
                } else {
                    learun.layerForm({
                        id: 'form',
                        title: '变更',
                        url: top.$.rootUrl + '/LR_CodeDemo/ProjectTask/FormChange?keyValue=' + keyValue,
                        width: 600,
                        height: 400,
                        callBack: function (id) {
                            var res = false;
                            // 验证数据
                            res = top[id].validForm();
                            // 保存数据
                            if (res) {
                                res = top[id].save(processId, refreshGirdData);
                            }
                            return res;
                        }
                    });
                }

            });
            // 编辑
            $('#lr_edit').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                var taskStatus = $('#gridtable').jfGridValue('TaskStatus');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '编辑',
                        url: top.$.rootUrl + '/LR_CodeDemo/ProjectTask/Form?keyValue=' + keyValue + '&TaskStatus=' + taskStatus,
                        width: 600,
                        height: 1200,
                        callBack: function (id) {

                            var res = false;
                            if (taskStatus!=1 & taskStatus!=11) {
                                learun.alert.error("当前任务已经在审核，请勿修改。");
                                return;
                            }
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
            //子报告添加
            $('#lr_editTask').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                var taskStatus = $('#gridtable').jfGridValue('TaskStatus');
                if (keyValue == null) {
                    learun.alert.warning("当前项目暂时没有创建报告");
                } else {
                    if (learun.checkrow(keyValue)) {
                        learun.layerForm({
                            id: 'form',
                            title: '子报告添加',
                            url: top.$.rootUrl + '/LR_CodeDemo/ProjectTask/TaskForm?keyValue=' + keyValue,
                            width: 600,
                            height: 900,
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

                }
            });
            // 编辑
            $('#lr_editName').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                var taskStatus = $('#gridtable').jfGridValue('TaskStatus');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '人员修改',
                        url: top.$.rootUrl + '/LR_CodeDemo/ProjectTask/UpdateNameForm?keyValue=' + keyValue ,
                        width: 600,
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
            });
            // 签到
            $('#lr_fielded').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                var taskStatus = $('#gridtable').jfGridValue('TaskStatus');
                if (taskStatus != 1) {
                    learun.alert.error("待进场才能签到!");
                    return;
                } else {
                    if (learun.checkrow(keyValue)) {
                        learun.layerConfirm('是否确认更改该任务状态任务单！', function (res, index) {

                            if (res) {
                                learun.httpAsync('Get', top.$.rootUrl + '/LR_CodeDemo/ProjectTask/UpdateFielded', { keyValue: keyValue }, function (data) {
                                    learun.layerClose('', index); //再执行关闭  
                                    learun.alert.success("签到成功");
                                    refreshGirdData();
                                });
                            }
                        });
                    }
                }
            });
            // 离场
            $('#lr_leave').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                var taskStatus = $('#gridtable').jfGridValue('TaskStatus');
                if (taskStatus != 2) {
                    learun.alert.error("现场中才能签到!");
                    return;
                } else {
                    if (learun.checkrow(keyValue)) {
                        learun.layerConfirm('是否确认更改该任务状态任务单！', function (res, index) {

                            if (res) {
                                learun.httpAsync('Get', top.$.rootUrl + '/LR_CodeDemo/ProjectTask/UpdateFielded', { keyValue: keyValue }, function (data) {
                                    learun.layerClose('', index); //再执行关闭  
                                    learun.alert.success("登记成功");
                                    refreshGirdData();
                                });
                            }
                        });
                    }
                }
            });
            // 审核
            $('#lr_approve').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                var ProcessId = $('#gridtable').jfGridValue('WorkFlowId');
                var taskStatus = $('#gridtable').jfGridValue('TaskStatus');
                if (taskStatus != 9) {
                    if (taskStatus == 11 && ProcessId != '') {
                        var postData = {
                            processId: ProcessId
                        }; 
                        learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectTask/UpdateFlowId', { keyValue: keyValue, ProcessId: ProcessId }, function (data) {
                            learun.httpAsync('Post', top.$.rootUrl + '/LR_NewWorkFlow/NWFProcess/AgainCreateFlow', postData, function (_data) {
                              learun.alert.success("提交成功");
                              learun.layerClose('', index); //再执行关闭 
                                refreshGirdData();
                                });
                            });
                    } else {
                            learun.alert.error("当前任务不能提审，请检查。");
                            return;
                        } 
                }
                else {
                    if (learun.checkrow(keyValue)) {
                        var lock = false;//未锁
                        learun.layerConfirm('是否确认提审该项目的任务单！', function (res, index) {
                            if (res) {
                                if (!lock) {
                                lock = true;//锁
                                learun.layerClose('', index); //再执行关闭  
                                learun.httpAsync('Get', top.$.rootUrl + '/LR_CodeDemo/ProjectTask/UpdateFlowId', { keyValue: keyValue, ProcessId: ProcessId }, function (data) {
                                    learun.alert.success("提交成功");
                                    refreshGirdData();
                                });
                                }
                                /*if (workFlowId) {
                                    var postData = {
                                        processId: workFlowId
                                    };
                                    learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectTask/UpdateFlowId', { keyValue: keyValue, ProcessId: workFlowId }, function (data) {
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
                                        schemeCode: 'ProjectTask',// 填写流程对应模板编号
                                        processId: processId,
                                        level: '1',
                                        title: projectName
                                    };
                                    //更新合同的状态并且创建审核流程
                                    learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectTask/UpdateFlowId', { keyValue: keyValue, ProcessId: processId }, function (data) {
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
            // 上传报告
            $('#lr_report').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                if (learun.checkrow(keyValue)) {
                    var taskStatus = $('#gridtable').jfGridValue('TaskStatus');
                    if (taskStatus != 8 & taskStatus != 11 & taskStatus!=9) {
                        learun.alert.error("离场后或拒绝才能上传报告");
                        return;
                    }
                    learun.layerForm({
                        id: 'reportform',
                        title: '上传报告',
                        url: top.$.rootUrl + '/LR_CodeDemo/ProjectTask/ReportForm?keyValue=' + keyValue,
                        width: 650,
                        height: 900,
                        callBack: function (id) {
                            var res = false;
                            // 验证数据
                            res = top[id].validForm();
                            // 保存数据
                            if (res) {
                                res = top[id].save('', refreshRepotGirdData);
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
                    var taskStatus = $('#gridtable').jfGridValue('TaskStatus');
                    var ActualDepartureTime = $('#gridtable').jfGridValue('ActualDepartureTime');
                    if (taskStatus == 1 || (taskStatus == 8) ) {
                        if (ActualDepartureTime == null) {
                            learun.layerConfirm('是否确认删除该项！', function (res) {
                                if (res) {
                                    learun.deleteForm(top.$.rootUrl + '/LR_CodeDemo/ProjectTask/DeleteForm', { keyValue: keyValue }, function () {
                                        refreshGirdData();
                                    });
                                }
                            });
                        } else {
                            learun.alert.error("当前任务已经再审核，请勿删除。");
                            return;
                        }
                    } else {
                        learun.alert.error("当前任务已经再审核，请勿删除。");
                        return;
                    }                          
                }
            });
            // 删除
            $('#lr_delete1').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                if (learun.checkrow(keyValue)) {
                    var taskStatus = $('#gridtable').jfGridValue('TaskStatus');
                    if (taskStatus ==9) {
                            learun.layerConfirm('是否确认删除该项！', function (res) {
                                if (res) {
                                    learun.deleteForm(top.$.rootUrl + '/LR_CodeDemo/ProjectTask/DeleteForm', { keyValue: keyValue }, function () {
                                        refreshGirdData();
                                    });
                                }
                            });

                    } else {
                        learun.alert.error("当前任务已经再审核，请勿删除。");
                        return;
                    }
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
                url: top.$.rootUrl + '/LR_CodeDemo/ProjectTask/GetPageList',
               // url: top.$.rootUrl + '/LR_CodeDemo/ProjectTask/GetPageToBeDetectList',
                headData: [
                    {
                        label: "创建日期", name: "CreateTime", width: 90, align: "center",
                        formatter: function (cellvalue) {
                            return learun.formatDate(cellvalue, 'yyyy-MM-dd');
                        }
                    },
                    {
                        label: "合同编号", name: "ContractNo", width: 110, align: "center",
                    },
                    {
                        label: "报告编号", name: "ProjectTaskNo", width: 110, align: "center",
                    },
                    {
                        label: "项目名称", name: "ProjectName", width: 300, align: "left",
                    },
                    {
                        label: "委托单位", name: "CustName", width: 250, align: "left",
                    },
                    {
                        label: "合同主体", name: "ContractSubject", width: 250, align: "left",
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
                        label: "项目负责人", name: "ProjectResponsible", width: 100, align: "center",
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
                        label: "进场时间", name: "ApproachTime", width: 90, align: "center",
                        formatter: function (cellvalue) {
                            return learun.formatDate(cellvalue, 'yyyy-MM-dd');
                        }
                    },
                    {
                        label: "离场时间", name: "ActualDepartureTime", width: 90, align: "center",
                        formatter: function (cellvalue) {
                            return learun.formatDate(cellvalue, 'yyyy-MM-dd');
                        }
                    },
                    {
                        label: "报告计划时间", name: "PlanTime", width: 90, align: "center",
                        formatter: function (cellvalue) {
                            return learun.formatDate(cellvalue, 'yyyy-MM-dd');
                        }
                    },
                
                    {
                        label: "任务状态", name: "TaskStatus", width: 100, align: "center",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'TaskStatus',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        }
                    },
                    
                    {
                        label: "记录", name: "Change", width: 250, align: "left",
                    },
                  
                    {
                        label: "预警", name: "YJ", width: 150, align: "left",
                        formatter: function (cellvalue, row, op, $cell) {
                            if (row.TaskStatus == 5) {
                                $cell.html('<span class="label label-success">已完成</span>');
                            }
                            
                            else if (cellvalue==999) {
                                $cell.html('<span  class="label label-warning" style="color:white;background-color:#ff0000" >超时</span>')
                            }
                            else if (row.TaskStatus != 5 && cellvalue != 999 && cellvalue != 111 ) {
                                $cell.html('<span class="label label-warning">剩余时间:' + cellvalue + '</span>')
                            }
                             else if (cellvalue==111) {
                                $cell.html('<span class="label label-danger">严重超时</span>')
                            }
                           
                        }
                    }
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
           //var categoryId = '1';
            //$('#gridtable').jfGridSet('reload', { queryJson: JSON.stringify(param), categoryId: categoryId});
            $('#gridtable').jfGridSet('reload', { queryJson: JSON.stringify(param)});
        }
    };
    refreshGirdData = function (res, postData) {
        //if (!!res) {
        //    if (res.code == 200) {
        //        // 发起流程
        //        var postData = {
        //            schemeCode: 'ProjectTask',// 填写流程对应模板编号
        //            processId: processId,
        //            level: '1',
        //        };
        //        learun.httpAsync('Post', top.$.rootUrl + '/LR_NewWorkFlow/NWFProcess/CreateFlow', postData, function (data) {
        //            learun.loading(false);
        //        });
        //    }

        //}
        page.search();
    };
    refreshRepotGirdData = function (res, postData) {
        page.search();
    };
    
    page.init();
}
