/* * 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
 * Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
 * 创建人：超级管理员
 * 日  期：2022-03-16 17:56
 * 描  述：项目回款管理
 */
var refreshGirdData;
var bootstrap = function ($, learun) {
    "use strict";
    var startTime;
    var endTime;
    var receiptDateStartTime;
    var receiptDateEndTime;
    var datelist;
    var select;
    var querselect;
    var quer;
    var page = {
        init: function () {
            page.initGird();
            page.bind();  
            //page.sum();
           
        },
        sum: function () {
           learun.loading(true);            
                $.QueryJson = $.QueryJson || {};
                $.QueryJson.ReceiptDateStartTime = receiptDateStartTime;
                $.QueryJson.ReceiptDateEndTime = receiptDateEndTime;
                $.QueryJson.StartTime = startTime;
            $.QueryJson.EndTime = endTime;
            if (select == 1) {
                var keyword = $('#txt_Keyword').val();
                $.QueryJson.keyword = keyword;
            }
            if (querselect == 2) {
                $.QueryJson.ReceiptDateStartTime = quer.ReceiptDateStartTime;
                $.QueryJson.ReceiptDateEndTime = quer.ReceiptDateEndTime;
                $.QueryJson.ContractNo = quer.ContractNo;
                $.QueryJson.ProjectName = quer.ProjectName;
                $.QueryJson.CustName = quer.CustName;
                $.QueryJson.Amount = quer.Amount;
                $.QueryJson.ReceiptDate = quer.ReceiptDate;
                $.QueryJson.PaymentUnit = quer.PaymentUnit;
            }
            learun.httpAsync('Get', top.$.rootUrl + '/LR_CodeDemo/ProjectPayCollection/GetPageListSUM', { queryJson: JSON.stringify($.QueryJson) }, function (data) {
                $("#AmountSUM").val(data.AmountSUM.toFixed(2));
               // $("#JXAmountSUM").val(data.JXAmountSUM.toFixed(2));
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
            // 创建时间搜索框
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
                    if (startTime != null || endTime != null) {
                        receiptDateStartTime = "";
                        receiptDateEndTime = "";
                    }
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
             
               
            }, 310, 400);
            $('#F_PurchaseType').lrDataItemSelect({ code: 'menuTrage' });
            $('#F_Appler').lrDataSourceSelect({ code: 'userdata', value: 'f_userid', text: 'f_realname' });
            $('#F_Department').lrDataSourceSelect({ code: 'company', value: 'f_companyid', text: 'f_shortname' });
            //$('#PaymentUnit').lrDataItemSelect({ code: 'PaymentUnit', value: 'f_userid', text: 'f_realname' });
            // 刷新
            $('#lr_refresh').on('click', function () {
                location.reload();
                page.sum();
            });
            // 新增
            $('#lr_add').on('click', function () {
                learun.layerForm({
                    id: 'form',
                    title: '新增',
                    url: top.$.rootUrl + '/LR_CodeDemo/ProjectPayCollection/Form',
                    width: 600,
                    height: 400,
                    callBack: function (id) {
                        return top[id].acceptClick(refreshGirdData);
                    }
                });
            });
            // 编辑
            $('#lr_edit').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '编辑',
                        url: top.$.rootUrl + '/LR_CodeDemo/ProjectPayCollection/Form?keyValue=' + keyValue,
                        width: 600,
                        height: 400,
                        callBack: function (id) {
                            return top[id].acceptClick(refreshGirdData);
                        }
                    });
                }
            });
            //双击打开预览
            $("#gridtable").dblclick(function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '预览',
                        url: top.$.rootUrl + '/LR_CodeDemo/ProjectPayCollection/PreviewForm?keyValue=' + keyValue,
                        width: 600,
                        height: 500,
                        callBack: function (id) {
                            return top[id].acceptClick(refreshGirdData);
                        }
                    });
                }
            });// 预览
            $('#lr_preview').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '预览',
                        url: top.$.rootUrl + '/LR_CodeDemo/ProjectPayCollection/PreviewForm?keyValue=' + keyValue,
                        width: 600,
                        height: 500,
                        callBack: function (id) {
                            return top[id].acceptClick(refreshGirdData);
                        }
                    });
                }
            });
            // 删除
            $('#lr_delete').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('id');
                if (learun.checkrow(keyValue)) {
                    learun.layerConfirm('是否确认删除该项！', function (res) {
                        if (res) {
                            learun.deleteForm(top.$.rootUrl + '/LR_CodeDemo/ProjectPayCollection/DeleteForm', { keyValue: keyValue }, function () {
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
            //导出
            $('#lr-export').on('click', function () {
                learun.loading(true);
                $.QueryJson = $.QueryJson || {};
                $.QueryJson.ReceiptDateStartTime = receiptDateStartTime;
                $.QueryJson.ReceiptDateEndTime = receiptDateEndTime;
                $.QueryJson.StartTime = startTime;
                $.QueryJson.EndTime = endTime;
                if (select == 1) {
                    var keyword = $('#txt_Keyword').val();
                    $.QueryJson.keyword = keyword;
                }
                if (querselect == 2) {
                    $.QueryJson.ReceiptDateStartTime = quer.ReceiptDateStartTime;
                    $.QueryJson.ReceiptDateEndTime = quer.ReceiptDateEndTime;
                    $.QueryJson.ContractNo = quer.ContractNo;
                    $.QueryJson.ProjectName = quer.ProjectName;
                    $.QueryJson.CustName = quer.CustName;
                    $.QueryJson.Amount = quer.Amount;
                    $.QueryJson.ReceiptDate = quer.ReceiptDate;
                    $.QueryJson.PaymentUnit = quer.PaymentUnit;
                }
                learun.httpAsync('Get', top.$.rootUrl + '/LR_CodeDemo/ProjectPayCollection/GetPageListAll', { queryJson: JSON.stringify($.QueryJson) }, function (data) {
                    // $("#AmountSUM").val(data.AmountSUM.toFixed(2));
                   var uuid = data.rows;
                    learun.download({
                        method: "POST",
                        url: '/Utility/ExportExcelPayCol',
                        param: {
                            fileName: "导出回款",
                            columnJson: JSON.stringify($('#gridtable').jfGridGet('settingInfo').headData),
                            dataJson: uuid
                        }
                    });
                    learun.loading(false);
                });
               
            });
            from
           /* $('#lr-export').on('click', function () {
               
                $.QueryJson = $.QueryJson || {};
              
                learun.download({
                    method: "POST",
                    url: '/Utility/ExportExcelPayCol',
                    param: {
                        fileName: "导出回款",
                        columnJson: JSON.stringify($('#gridtable').jfGridGet('settingInfo').headData),
                        dataJson: JSON.stringify($.datelist)
                        
                    }
                   
                });
             
                
            });*/
        },
        // 初始化列表
        initGird: function () {
            $('#gridtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/LR_CodeDemo/ProjectPayCollection/GetPageList',
                headData: [
                    {
                        label: "创建日期", name: "CreateTime", width: 90, align: "center",
                        formatter: function (cellvalue) {
                            return learun.formatDate(cellvalue, 'yyyy-MM-dd');
                        }                      },
                    { label: "项目名称", name: "ProjectName", width: 250, align: "left" },
                    { label: "委托单位", name: "CustName", width: 200, align: "left" },
                    { label: "合同编号", name: "ContractNo", width: 120, align: "center" },
                    { label: "本次到款", name: "Amount", width: 100, align: "left" },
                    {
                        label: "到账日期", name: "ReceiptDate", width: 90, align: "center",
                        formatter: function (cellvalue) {
                            return learun.formatDate(cellvalue, 'yyyy-MM-dd');
                        }
                    },
                    //{
                    //    label: "核算1", name: "FollowPersonAmount", width: 80, align: "left", sort: false,
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

                    //    }
                    //},
                    {
                        label: "款绩", name: "PayCollectionAmount", width: 80, align: "left", sort: false,
                        formatter: function (cellvalue, row, op, $cell) {
                            //自主营销
                            if (row.ProjectSource == 1) {
                                if (row.PaymentAmount == null) {
                                    cellvalue = row.Amount * 0.02
                                   
                                    return cellvalue.toFixed(2)
                                } else if (row.PaymentAmount < (row.Amount * 0.3)) {
                                    cellvalue = row.Amount * 0.005
                                   
                                    return cellvalue.toFixed(2)
                                } else {
                                    cellvalue = row.Amount * 0.02
                                  
                                    return cellvalue.toFixed(2)
                                }
                            }
                            //渠道营销
                            if (row.ProjectSource == 2) {
                                if (row.PaymentAmount == null) {
                                    cellvalue = row.Amount * 0.015
                                    
                                    return cellvalue.toFixed(2)
                                } else if (row.PaymentAmount < (row.Amount * 0.3)) {
                                    cellvalue = row.Amount * 0.002
                                   
                                    return cellvalue.toFixed(2)
                                } else {
                                    cellvalue = row.Amount * 0.001
                                  
                                    return cellvalue.toFixed(2)
                                }
                            }

                        }
                    },
                    { label: "有效合同额", name: "EffectiveAmount", width: 100, align: "left" },
                    { label: "到款单位", name: "PaymentUnit", width: 90, align: "center" },

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
                        label: "项目来源", name: "ProjectSource", width: 90, align: "left",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'ProjectSource',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        }

                    }
                ],
                sidx: 'CreateTime',
                sord: 'DESC',
                mainId: 'id',
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
    refreshGirdData = function () {
        page.search();
    };
    page.init();
}
