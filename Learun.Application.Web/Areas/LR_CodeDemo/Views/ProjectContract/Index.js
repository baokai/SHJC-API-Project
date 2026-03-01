 ////* 创建人：超级管理员
 ////* 日  期：2022-03-10 23:22
 ////* 描  述：项目合同申请
 ////*/
var refreshGirdData;
var bootstrap = function ($, learun) {
    "use strict";
    var startTime;
    var endTime;
    var processId = '';
    var datelist;
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
            $('#lr_import').on('click', function () {
                learun.layerForm({
                    id: 'ImportForm',
                    title: '导入模板',
                    url: top.$.rootUrl + '/LR_CodeDemo/ProjectContract/ImportForm',
                    width: 600,
                    height: 400,
                    maxmin: true,
                    btn: null
                });
            });
            $('#multiple_condition_query').lrMultipleQuery(function (queryJson) {
                querselect = 2;
                quer = queryJson;
                page.search(queryJson);
                //console.log(queryJson)
            }, 520, 400);
            $('#F_PurchaseType').lrDataItemSelect({ code: 'menuTrage' });
            $('#F_Appler').lrDataSourceSelect({ code: 'userdata', value: 'f_userid', text: 'f_realname' });
            $('#F_Department').lrDataSourceSelect({ code: 'company', value: 'f_companyid', text: 'f_shortname' });

            $('#ProjectSource').lrRadioCheckbox({
                type: 'radio',
                code: 'ProjectSource',
            });
            $('#ContractType').lrRadioCheckbox({
                type: 'radio',
                code: 'ContractType',
            });
            $('#ContractSubject').lrDataItemSelect({ code: 'ContractSubject', maxHeight: 230 });
            $('#ContractStatus').lrDataItemSelect({ code: 'ContractStatus', maxHeight: 230 });
            $('#ReceivedFlag').lrDataItemSelect({ code: 'ReceivedFlag', maxHeight: 230 });
            $('#DepartmentId').lrDepartmentSelect();
            $('#FollowPerson').lrformselect({
                layerUrl: top.$.rootUrl + '/LR_OrganizationModule/User/SelectForm',
                layerUrlW: 800,
                layerUrlH: 520,
                dataUrl: top.$.rootUrl + '/LR_OrganizationModule/User/GetListByUserIds'
            });
            $('#lr_daoru').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'reportform',
                        title: '编辑',
                        url: top.$.rootUrl + '/LR_CodeDemo/ProjectTask/ReportForm?keyValue=' + keyValue,
                        width: 650,
                        height: 750,
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
            // 刷新
            $('#lr_refresh').on('click', function () {
                location.reload();
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
                    $.QueryJson.ContactPhone = quer.ContactPhone;
                    $.QueryJson.ContractSubject = quer.ContractSubject;
                    $.QueryJson.DepartmentId = quer.DepartmentId;
                    $.QueryJson.FollowPerson = quer.FollowPerson;
                    $.QueryJson.ProjectSource = quer.ProjectSource;
                    $.QueryJson.ContractStatus = quer.ContractStatus;
                    $.QueryJson.ContractAmount = quer.ContractAmount;
                    $.QueryJson.ContractType = quer.ContractType;
                    $.QueryJson.ReceivedFlag = quer.ReceivedFlag;
                }                
                $.QueryJson.StartTime = startTime;
                $.QueryJson.EndTime = endTime;
                learun.httpAsync('Get', top.$.rootUrl + '/LR_CodeDemo/ProjectContract/GetPageListAll', { queryJson: JSON.stringify($.QueryJson) }, function (data) {
                    var uuid = data.rows;
                    learun.download({
                        method: "POST",
                        url: '/Utility/ExportExcelContract',
                        param: {
                            fileName: "导出合同",
                            columnJson: JSON.stringify($('#gridtable').jfGridGet('settingInfo').headData),
                            dataJson: uuid
                        }
                    });
                    learun.loading(false);
                });
            });

        
            // 新增
            $('#lr_add').on('click', function () {
               
                learun.layerForm({
                    id: 'form',
                    title: '合同(新增)',
                    url: top.$.rootUrl + '/LR_CodeDemo/ProjectContract/Form',
                    width: 800,
                    height: 700,
                    callBack: function (id) {
                        var res = false;
                        // 验证数据
                        res = top[id].validForm();
                        // 保存数据
                        if (res) {
                            res = top[id].save("", function () {
                                page.search();
                            });
                        }
                        return res;
                    }
                });
            }); 
            $('#lr_fill').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');              
                var projectName = $('#gridtable').jfGridValue('ProjectName');
                var contractNo = $('#gridtable').jfGridValue('ContractNo');
                if (keyValue == null) {
                    learun.alert.warning("当前项目暂时没有创建合同。");
                }
                else {
                    if (learun.checkrow(keyValue)) {
                        learun.layerForm({
                            id: 'form',
                            title: projectName + '-合同(编辑)',
                            url: top.$.rootUrl + '/LR_CodeDemo/ProjectContract/FillForm?keyValue=' + keyValue + "&ContractNo=" + contractNo,
                            width: 800,
                            height: 700,
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
            $('#lr_edit').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                var projectId = $('#gridtable').jfGridValue('Pid');
                var projectName = $('#gridtable').jfGridValue('ProjectName');
                var contractStatus = $('#gridtable').jfGridValue('ContractStatus'); 
                var contractNo = $('#gridtable').jfGridValue('ContractNo');
                if (keyValue == null) {
                    learun.alert.warning("当前项目暂时没有创建合同。");
                }
                else {
                    if (learun.checkrow(keyValue)) {
                        learun.layerForm({
                            id: 'form',
                            title: projectName + '-合同(编辑)',
                            url: top.$.rootUrl + '/LR_CodeDemo/ProjectContract/Form?keyValue=' + keyValue + "&ProjectId=" + projectId + "&ContractStatus=" + contractStatus + "&ContractNo=" + contractNo,
                            width: 800,
                            height: 700,
                            callBack: function (id) {
                                learun.httpAsyncGet(top.$.rootUrl + '/UserCenter/GetLoginUserInfo', function (response) {
                                    if (response.code == 200) {
                                        var userid = response.data.userId;
                                        if (userid != 'fae74e8a-3dcc-45f2-b6e1-b6800654eaf9') {
                                            if (contractStatus != 1 & contractStatus != 11) {
                                                learun.alert.error("当前合同已经再审核，请勿修改。");
                                                return;
                                            }
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
                                    else {
                                        learun.alert.error('数据加载失败');
                                    }
                                });
                            }
                        });
                    }
                }

            });
            // 审核编辑
            $('#edit').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                var projectId = $('#gridtable').jfGridValue('Pid');
                var projectName = $('#gridtable').jfGridValue('ProjectName');
                var contractStatus = $('#gridtable').jfGridValue('ContractStatus');
                var contractNo = $('#gridtable').jfGridValue('ContractNo');
                if (keyValue == null) {
                    learun.alert.warning("当前项目暂时没有创建合同。");
                }
                else {
                    if (learun.checkrow(keyValue)) {
                        learun.layerForm({
                            id: 'form',
                            title: projectName + '-合同(审核编辑)',
                            url: top.$.rootUrl + '/LR_CodeDemo/ProjectContract/EditForm1?keyValue=' + keyValue + "&ProjectId=" + projectId + "&ContractNo=" + contractNo,
                            width: 800,
                            height: 700,
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
            // 审核
            $('#lr_approve').on('click', function () {
               
                var keyValue = $('#gridtable').jfGridValue('id');
                var workFlowId = $('#gridtable').jfGridValue('WorkFlowId');
                var contractStatus = $('#gridtable').jfGridValue('ContractStatus');
                var projectName = $('#gridtable').jfGridValue('ProjectName');

                if (contractStatus != 1) { 
                    if (contractStatus == 11 && workFlowId != '') {
                        var postData = {
                            processId: workFlowId
                        };
                        learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectContract/UpdateFlowId', { keyValue: keyValue, ProcessId: workFlowId }, function (data) {
                        learun.httpAsync('Post', top.$.rootUrl + '/LR_NewWorkFlow/NWFProcess/AgainCreateFlow', postData, function (_data) {
                            learun.alert.success("提交成功");
                            learun.layerClose('', index); //再执行关闭
                            
                          
                        refreshGirdData();
                            });
                        });
                    } else {
                        learun.alert.error("当前合同已经再审核，请勿重复提交。");
                        return;
                    }                   
                }
                else {
                    if (learun.checkrow(keyValue)) {
                        var lock = false;//未锁
                        learun.layerConfirm('是否确认提审该项目的合同！', function (res, index) {
                           
                            if (res) {
                                
                               if (!lock) {
                                    lock = true;//锁
                                    learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectContract/UpdateFlowId', { keyValue: keyValue, ProcessId: workFlowId }, function (data) {
                                        learun.layerClose('', index); //再执行关闭
                                        learun.alert.success("提交成功");
                                        refreshGirdData();
                                    })

                                }

                                /*if (workFlowId) {
                                    var postData = {
                                        processId: workFlowId
                                    };

                                    learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectContract/UpdateFlowId', { keyValue: keyValue, ProcessId: workFlowId }, function (data) {
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
                                        schemeCode: 'ProjectContract',// 填写流程对应模板编号
                                        processId: processId,
                                        level: '1',
                                        title: projectName
                                    };
                                    //更新合同的状态并且创建审核流程
                                    learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectContract/UpdateFlowId', { keyValue: keyValue, ProcessId: processId }, function (data) {
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
            //变更
            $('#lr_change').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                var workFlowId = $('#gridtable').jfGridValue('WorkFlowId');
                var contractStatus = $('#gridtable').jfGridValue('ContractStatus');
                var projectName = $('#gridtable').jfGridValue('ProjectName');
                if (contractStatus != 4) {
                    learun.alert.error("审核通过才能变更!");
                    return;
                }
                else {
                    if (learun.checkrow(keyValue)) {
                        learun.layerConfirm('是否确认更改项目的合同！', function (res, index) {
                            if (res) {
                                var postData = {
                                    keyValue: keyValue,
                                    ProcessId: workFlowId
                                };
                                learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectContract/UpdateContractStatus', postData, function (data) {
                                    learun.layerClose('', index); //再执行关闭  
                                    learun.alert.success("提交成功");
                                    refreshGirdData();
                                });
                            }
                        })
                    }
                }

            })
            //刷新有效合同额
            $('#AmountList').on('click', function () {                     
                    
                        
                                learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectContract/UpdateEffectiveAmount', function (data) {
                                    learun.layerClose('', index); //再执行关闭  
                                    learun.alert.success("刷新成功");
                                    refreshGirdData();
                                });
                          
                    
                

            })
            // 取消
            $('#lr_cancel').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                var projectName = $('#gridtable').jfGridValue('ProjectName');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: projectName + '-合同(取消)',
                        url: top.$.rootUrl + '/LR_CodeDemo/ProjectContract/ZuofeiForm?keyValue=' + keyValue,
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
                            learun.postForm(top.$.rootUrl + '/LR_CodeDemo/ProjectContract/Zuofei', { keyValue: keyValue }, function () {
                                refreshGirdData();
                            });
                        }
                    });
                }*/
            });
            // 修改
            $('#lr_update').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                var projectId = $('#gridtable').jfGridValue('Pid');
                var projectName = $('#gridtable').jfGridValue('ProjectName');
                if (keyValue == null) {
                    learun.alert.warning("当前项目暂时没有创建合同。");
                }
                else {
                    if (learun.checkrow(keyValue)) {
                        learun.layerForm({
                            id: 'form',
                            title: projectName + '-合同(合同金额编辑)',
                            url: top.$.rootUrl + '/LR_CodeDemo/ProjectContract/updateForm?keyValue=' + keyValue + "&ProjectId=" + projectId + "&ContractStatus=" + ContractStatus,
                            width: 800,
                            height: 700,
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
            // 删除
            $('#lr_delete').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                if (learun.checkrow(keyValue)) {
                    var contractStatus = $('#gridtable').jfGridValue('ContractStatus');
                    if (contractStatus != 1) {
                        learun.alert.error("当前合同已经再审核，请勿删除");
                        return;
                    }
                    learun.layerConfirm('是否确认删除该项！', function (res) {
                        if (res) {
                            learun.postForm(top.$.rootUrl + '/LR_CodeDemo/ProjectContract/DeleteForm', { keyValue: keyValue }, function () {
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
            $('#lr_receipt').on('click', function () {
                //var keyValue = $('#gridtable').jfGridValue('id');
                //if (learun.checkrow(keyValue)) {
                //    learun.layerConfirm('是否确认收到该项目的合同！', function (res, index) {
                //        if (res) {
                //            learun.httpAsync('POST', top.$.rootUrl + '/LR_CodeDemo/ProjectContract/UpdateReceivedFlag', { keyValue: keyValue }, function (data) {
                //                learun.layerClose('', index); //再执行关闭  
                //                refreshGirdData();
                //            });
                //        }
                //    });
                //}
                var keyValue = $('#gridtable').jfGridValue('id');
                var projectName = $('#gridtable').jfGridValue('ProjectName');
                var contractStatus = $('#gridtable').jfGridValue('ContractStatus');
                var receivedFlag = $('#gridtable').jfGridValue('ReceivedFlag');
                var projectId = $('#gridtable').jfGridValue('Pid');
                if (keyValue == null) {
                    learun.alert.warning("当前项目暂时没有创建合同。");
                } else if (contractStatus != 4) {
                    learun.alert.warning("当前合同未审核通过。");
                }
                /*else if (receivedFlag == 1) {
                    learun.alert.warning("当前合同已经归档。");
                }*/
                else {
                    if (learun.checkrow(keyValue)) {
                        learun.layerForm({
                            id: 'form',
                            title: projectName + '-合同归档',
                            url: top.$.rootUrl + '/LR_CodeDemo/ProjectContract/SendContract?keyValue=' + keyValue + "&ProjectId=" + projectId,
                            width: 600,
                            height: 500,
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
            //双击打开预览
            $("#gridtable").dblclick(function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                var projectName = $('#gridtable').jfGridValue('ProjectName');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: projectName + '-合同(预览)',
                        url: top.$.rootUrl + '/LR_CodeDemo/ProjectContract/PreviewForm?keyValue=' + keyValue,
                        width: 800,
                        height: 700,
                        callBack: function (id) {
                            if (contractStatus != 1) {
                                learun.alert.error("当前合同已经再审核，请勿修改。");
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
            $("#lr_preview").on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                var projectName = $('#gridtable').jfGridValue('ProjectName');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: projectName + '-合同(预览)',
                        url: top.$.rootUrl + '/LR_CodeDemo/ProjectContract/PreviewForm?keyValue=' + keyValue,
                        width: 800,
                        height: 700,
                        callBack: function (id) {
                            if (contractStatus != 1) {
                                learun.alert.error("当前合同已经再审核，请勿修改。");
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

        },
        // 初始化列表
        initGird: function () {
            $('#gridtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/LR_CodeDemo/ProjectContract/GetPageList',
                headData: [

                    {
                        label: "创建日期", name: "CreateTime", width: 90, align: "center",
                        formatter: function (cellvalue) {
                            return learun.formatDate(cellvalue, 'yyyy-MM-dd');
                        }                    },
                    { label: "合同编号", name: "ContractNo", width: 110, align: "center" },
                    {
                        label: "项目名称", name: "ProjectName", width: 300, align: "left",

                    },
                    {
                        label: "委托单位", name: "CustName", width: 250, align: "left",

                    },

                    {
                        label: "合同主体", name: "ContractSubject", width: 80, align: "center",
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
                        label: "营销人员", name: "FollowPerson", width: 80, align: "left",
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
                        label: "项目来源", name: "ProjectSource", width: 70, align: "left",
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

                    //{
                    //    label: "核算1", name: "FollowPersonAmount", width: 100, align: "left", sort: false,
                    //    formatter: function (cellvalue, row, op, $cell) {
                    //        //自主营销
                    //        if (row.ProjectSource == 1) {
                    //            if (row.PaymentAmount == null) {
                    //                cellvalue = row.ContractAmount * 0.02
                    //                return cellvalue.toFixed(2)
                    //            } else if (row.PaymentAmount < (row.ContractAmount * 0.3)) {
                    //                cellvalue = row.ContractAmount * 0.005
                    //                return cellvalue.toFixed(2)
                    //            } else {
                    //                cellvalue = row.ContractAmount * 0.02
                    //                return cellvalue.toFixed(2)
                    //            }
                    //        }
                    //        //渠道营销
                    //        if (row.ProjectSource == 2) {
                    //            if (row.PaymentAmount == null) {
                    //                cellvalue = row.ContractAmount * 0.015
                    //                return cellvalue.toFixed(2)
                    //            } else if (row.PaymentAmount < (row.ContractAmount * 0.3)) {
                    //                cellvalue = row.ContractAmount * 0.002
                    //                return cellvalue.toFixed(2)
                    //            } else {
                    //                cellvalue = row.ContractAmount * 0.001
                    //                return cellvalue.toFixed(2)
                    //            }
                    //        }
                            
                    //    }                    },
                    { label: "合同金额", name: "ContractAmount", width: 100, align: "left" },
                    { label: "有效合同额", name: "EffectiveAmount", width: 100, align: "left" },
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
                    {
                        label: "归档类型", name: "ReceivedFlag", width: 90, align: "left",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'ReceiptType',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        }
                    },
                    {
                        label: "主合同", name: "MainContractName", width: 50, align: "center"
                       
                    },
                    {
                        label: "归档备注", name: "Remark", width: 250, align: "left"
                    }
                ],

                sidx: 'CreateTime',
                sord: 'DESC',
                mainId: 'Id',
                isPage: true
            });
        },
        search: function () {
            var keyword = $('#txt_Keyword').val();
            var param = $('#multiple_condition_query').lrGetFormData();
            var param = param || {};
            param.keyword = keyword;
            param.StartTime = startTime; 
            param.EndTime = endTime;


            $('#gridtable').jfGridSet('reload', { queryJson: JSON.stringify(param) });
            
           
           
        }

    };
    refreshGirdData = function (res, postData) {
        //if (!!res) {
        //    if (res.code == 200) {
        //        // 发起流程
        //        var postData = {
        //            schemeCode: 'ProjectContract',// 填写流程对应模板编号
        //            processId: processId,
        //            level: '1',
        //        };
        //        learun.httpAsync('Post', top.$.rootUrl + '/LR_NewWorkFlow/NWFProcess/CreateFlow', postData, function (data) {
        //            learun.loading(false);
        //        });
        //    }
        //    page.search();
        //}
        //else {

        //}
        page.search();
    };

    page.init();
}
