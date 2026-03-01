var bootstrap = function ($, learun) {
    "use strict";
    var startTime;
    var endTime;
    var processId = '';
    var QueryJson;
    var datelist;
    var select;
    var querselect;
    var quer;
    var page = {
        init: function () {
            page.initGrid();
            page.bind();
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
                $.QueryJson.ApproverTime = quer.ApproverTime;
                $.QueryJson.ProjectId = quer.ProjectId;
                $.QueryJson.ContractNo = quer.ContractNo;
                $.QueryJson.ContractSubject = quer.ContractSubject;
                $.QueryJson.CustName = quer.CustName;
                $.QueryJson.ProjectSource = quer.ProjectSource;
                $.QueryJson.ContractAmount = quer.ContractAmount;
                $.QueryJson.ContractStatus = quer.ContractStatus;
                $.QueryJson.Y_DepartmentId = quer.Y_DepartmentId;
                $.QueryJson.Amount = quer.Amount;
                $.QueryJson.ReceiptDate = quer.ReceiptDate;
                $.QueryJson.PaymentAmount = quer.PaymentAmount;
                $.QueryJson.S_DepartmentId = quer.S_DepartmentId;
                $.QueryJson.ApproachTime = quer.ApproachTime;
                $.QueryJson.ReportSubject = quer.ReportSubject;
                $.QueryJson.TaskStatus = quer.TaskStatus;
            }
            learun.httpAsync('Get', top.$.rootUrl + '/LR_CodeDemo/ReportForms/GetSettleAccountsSum', { queryJson: JSON.stringify($.QueryJson)  }, function (data) {
               $("#ContractAmountSum").val(data.ContractAmountSum.toFixed(2));
                $("#BillingAmountSum").val(data.BillingAmountSum.toFixed(2));
                $("#AmountSum").val(data.AmountSum.toFixed(2));
                $("#NotReceivedSum").val(data.NotReceivedSum.toFixed(2));
                $("#PaymentAmountSum").val(data.PaymentAmountSum.toFixed(2));
                $("#OwnSum").val(data.OwnSum.toFixed(2));
                $("#DitchSum").val(data.DitchSum.toFixed(2));
                $("#ConsociationSum").val(data.ConsociationSum.toFixed(2));
               // $("#QKSum").val(data.QKSum.toFixed(2));
            });
            learun.loading(false);
        },
        bind: function () {
            $('#btn_Search').on('click', function () {
                select = 1;
                var keyword = $('#txt_Keyword').val();
                $.QueryJson = ({ keyword: keyword });
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
                $.QueryJson = queryJson;
                page.search(queryJson);
                page.sum();
            }, 710, 400);
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
            $("#Y_DepartmentId").lrDepartmentSelect();
            $('#S_DepartmentId').lrDepartmentSelect();
            $('#ContractStatus').lrDataItemSelect({ code: 'ContractStatus', maxHeight: 230 });
            $('#ContractSubject').lrDataItemSelect({ code: 'ContractSubject', maxHeight: 230 });
            $('#ReportSubject').lrDataItemSelect({ code: 'ContractSubject', maxHeight: 230 });
            $('#TaskStatus').lrDataItemSelect({ code: 'TaskStatus', maxHeight: 230 });
            // 刷新
            $('#lr-replace').on('click', function () {
                location.reload();
            });
            // 计算
          /*  $('#lr-sum').on('click', function () {
                var ContractAmountSum = 0;
                var BillingAmountSum = 0;
                var AmountSum = 0;
                var NotReceivedSum = 0;
                var PaymentAmountSum = 0;
                var OwnSum = 0;
                var DitchSum = 0;
                var ConsociationSum = 0;
                var arr = $('#gridtable').jfGridGet('settingInfo').rowdatas;
                for (var i = 0; i < arr.length; i++) {
                    ContractAmountSum = ContractAmountSum + arr[i].ContractAmount;
                    BillingAmountSum = BillingAmountSum + arr[i].BillingAmount;
                    AmountSum = AmountSum + arr[i].Amount;
                    NotReceivedSum = NotReceivedSum + arr[i].NotReceived;
                    PaymentAmountSum = PaymentAmountSum + arr[i].PaymentAmount;
                    if (arr[i].ProjectSource == 1) {
                        OwnSum = OwnSum + arr[i].ContractAmount;
                    }
                    if (arr[i].ProjectSource == 2) {
                        DitchSum = DitchSum + arr[i].ContractAmount;
                    }
                    if (arr[i].ProjectSource == 3) {
                        ConsociationSum = ConsociationSum + arr[i].ContractAmount;
                    }
                };
                $("#ContractAmountSum").val(ContractAmountSum.toFixed(2));
                $("#BillingAmountSum").val(BillingAmountSum.toFixed(2));
                $("#AmountSum").val(AmountSum.toFixed(2));
                $("#NotReceivedSum").val(NotReceivedSum.toFixed(2));
                $("#PaymentAmountSum").val(PaymentAmountSum.toFixed(2));
                $("#OwnSum").val(OwnSum.toFixed(2));
                $("#DitchSum").val(DitchSum.toFixed(2));
                $("#ConsociationSum").val(ConsociationSum.toFixed(2));
            });*/
            //打印
            $('#lr-print').on('click', function () {
                $("#gridtable").jqprintTable({ title: '营销报表' });
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
                    $.QueryJson.ApproverTime = quer.ApproverTime;
                    $.QueryJson.ProjectId = quer.ProjectId;
                    $.QueryJson.ContractNo = quer.ContractNo;
                    $.QueryJson.ContractSubject = quer.ContractSubject;
                    $.QueryJson.CustName = quer.CustName;
                    $.QueryJson.ProjectSource = quer.ProjectSource;
                    $.QueryJson.ContractAmount = quer.ContractAmount;
                    $.QueryJson.ContractStatus = quer.ContractStatus;
                    $.QueryJson.Y_DepartmentId = quer.Y_DepartmentId;
                    $.QueryJson.Amount = quer.Amount;
                    $.QueryJson.ReceiptDate = quer.ReceiptDate;
                    $.QueryJson.PaymentAmount = quer.PaymentAmount;
                    $.QueryJson.S_DepartmentId = quer.S_DepartmentId;
                    $.QueryJson.ApproachTime = quer.ApproachTime;
                    $.QueryJson.ReportSubject = quer.ReportSubject;
                    $.QueryJson.TaskStatus = quer.TaskStatus;
                }
                //learun.download({
                //    method: "POST",
                //    url: '/Utility/ExportExcelSetReportForms',
                //    param: {
                //        fileName: "导出结算报表",
                //        columnJson: JSON.stringify($('#gridtable').jfGridGet('settingInfo').headData),
                //        dataJson: JSON.stringify($.datelist)
                //    }
                //});
                learun.httpAsync('Get', top.$.rootUrl + '/LR_CodeDemo/ReportForms/GetSettleAccountsSumAll', { queryJson: JSON.stringify($.QueryJson)  }, function (data) {
                    var uuid = data.rows;

                    learun.download({
                        method: "POST",
                        url: '/Utility/ExportExcelSetReportForms',
                        param: {
                            fileName: "导出结算报表",
                            columnJson: JSON.stringify($('#gridtable').jfGridGet('settingInfo').headData),
                            dataJson: uuid
                        }
                    });
                    learun.loading(false);
                });

            });
        },
        initGrid: function () {
            $("#gridtable").height($(window).height() - 170);
            $('#gridtable').jfGrid({
                url: top.$.rootUrl + '/LR_CodeDemo/ReportForms/GetSettleAccounts',
                headData: [
                    { name: "CreateTime", label: "创建日期", width: 80, align: "center" },
                    { name: "ApproverTime", label: "审核日期", width: 80, align: "center" },
                    { name: "ContractNo", label: "合同编码", width: 110, align: "center" },
                    { name: "ProjectName", label: "项目名称", width: 350, align: "left" },
                    { name: "CustName", label: "委托单位", width: 250, align: "left" },
                    {
                       
                        name: "ContractSubject", label: "合同主体", width: 70, align: "center",
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
                        label: "报备人", name: "FollowPerson", width: 80, align: "left",
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
                        name: "ContractAmount", label: "合同金额", width: 80, align: "right",
                        formatter: function (cellvalue) {
                            return learun.toDecimal(cellvalue);
                        }
                    },
                    {
                        name: "BillingAmount", label: "开票金额", width: 80, align: "right",
                        formatter: function (cellvalue) {
                            return learun.toDecimal(cellvalue);
                        }
                    },
                    {
                        name: "Amount", label: "已到金额", width: 80, align: "right",
                        formatter: function (cellvalue) {
                            return learun.toDecimal(cellvalue);
                        }
                    },
                    {
                        name: "NotReceived", label: "未收账款", width: 80, align: "right",
                        formatter: function (cellvalue) {
                            return learun.toDecimal(cellvalue);
                        }
                    },
                    {
                        label: "营销核算", name: "DepartmentIdAmount", width: 80, align: "left", sort: false,
                        formatter: function (cellvalue, row, op, $cell) {
                           
                            //自主营销
                            if (row.ProjectSource == 1) {
                                if (row.PaymentAmount == null) {
                                    cellvalue = row.ContractAmount * 0.3
                                    return cellvalue
                                } else if (row.PaymentAmount < (row.ContractAmount * 0.3)) {
                                    cellvalue = (row.ContractAmount * 0.3) - row.PaymentAmount
                                    return cellvalue.toFixed(2)
                                } else {
                                    cellvalue = row.ContractAmount * 0.03
                                    return cellvalue.toFixed(2)
                                }
                            }
                            //渠道营销
                            if (row.ProjectSource == 2) {
                                cellvalue = row.ContractAmount * 0.03
                                return cellvalue.toFixed(2)
                            }
                        }

                    },
                    {
                        label: "生产核算", name: "DepartmentIdAmount1", width: 80, align: "left", sort: false,
                        formatter: function (cellvalue, row, op, $cell) {
                            if (row.ProjectSource==1 || row.ProjectSource==2) {
                                if (row.TaskStatus == 9 || row.TaskStatus == 3 || row.TaskStatus == 11 || row.TaskStatus == 4 || row.TaskStatus == 5) {
                                    //自主营销

                                    if (row.PaymentAmount >= (row.ContractAmount * 0.3)) {
                                        cellvalue = (row.ContractAmount - row.PaymentAmount - row.MainAmount - row.SubAmount) * 0.3
                                        return cellvalue.toFixed(2)
                                    } else {
                                        cellvalue = row.ContractAmount * 0.2
                                        return cellvalue.toFixed(2)
                                    }
                                }
                            }


                        }

                    },

                    {
                        label: "核算1", name: "FollowPersonAmount", width: 80, align: "left", sort: false,
                        formatter: function (cellvalue, row, op, $cell) {
                            //自主营销
                            if (row.ProjectSource == 1) {
                                if (row.PaymentAmount == null) {
                                    cellvalue = row.ContractAmount * 0.02
                                    return cellvalue.toFixed(2)
                                } else if (row.PaymentAmount < (row.ContractAmount * 0.3)) {
                                    cellvalue = row.ContractAmount * 0.005
                                    return cellvalue.toFixed(2)
                                } else {
                                    cellvalue = row.ContractAmount * 0.02
                                    return cellvalue.toFixed(2)
                                }
                            }
                            //渠道营销
                            if (row.ProjectSource == 2) {
                                if (row.PaymentAmount == null) {
                                    cellvalue = row.ContractAmount * 0.015
                                    return cellvalue.toFixed(2)
                                } else if (row.PaymentAmount < (row.ContractAmount * 0.3)) {
                                    cellvalue = row.ContractAmount * 0.002
                                    return cellvalue.toFixed(2)
                                } else {
                                    cellvalue = row.ContractAmount * 0.001
                                    return cellvalue.toFixed(2)
                                }
                            }

                        }
                    },
                    { label: "有效合同额", name: "EffectiveAmount", width: 80, align: "left" },
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
                        name: "ProjectSource", label: "项目来源", width: 70, align: "center",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'ProjectSource',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        },
                    },
                    { name: "ReceivedFlag", label: "合同归档情况", width: 65, align: "center" },
                    {
                        name: "J_F_FullName", label: "实施部门", width: 70, align: "center",
                        formatterAsync: function (callback, value, row) {
                            learun.clientdata.getAsync('department', {
                                key: value,
                                callback: function (item) {
                                    callback(item.name);
                                }
                            });
                        } },
                    {
                        name: "ProjectResponsible", label: "项目负责人", width: 70, align: "center",
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
                        name: "TaskStatus", label: "报告状态", width: 70, align: "center",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'TaskStatus',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        },
                    },
                    { name: "PaymentAmount", label: "营销外付", width: 100, align: "left" },
                    
                    { name: "Remark", label: "备注", width: 150, align: "left" }
                ],
                reloadSelected: true,
                mainId: 'ContractNo',
                sidx: 'ApproverTime',
                sord: 'DESC',
                isPage: true
            });

            //page.search();
        },
        search: function (param) {
            param = param || {};
            param.StartTime = startTime;
            param.EndTime = endTime;
            $('#gridtable').jfGridSet('reload', { queryJson: JSON.stringify(param) });
//            learun.httpAsync('Get', top.$.rootUrl + '/LR_CodeDemo/ReportForms/GetSettleAccountsSum', { queryJson: JSON.stringify(param) }, function (data) {
//                //$("#ContractAmountSum").val(data.ContractAmountSum.toFixed(2));
//                //$("#BillingAmountSum").val(data.BillingAmountSum.toFixed(2));
//                //$("#AmountSum").val(data.AmountSum.toFixed(2));
//                //$("#NotReceivedSum").val(data.NotReceivedSum.toFixed(2));
//                //$("#PaymentAmountSum").val(data.PaymentAmountSum.toFixed(2));
//                //$("#OwnSum").val(data.OwnSum.toFixed(2));
//                //$("#DitchSum").val(data.DitchSum.toFixed(2));
//                //$("#ConsociationSum").val(data.ConsociationSum.toFixed(2));
///*                $.datelist = data.data;*/
//                //$.ajax({
//                //    url: top.$.rootUrl + '/LR_CodeDemo/ReportForms/GetSettleAccountsSum',
//                //    type: 'get',
//                //    dataType: "json",
//                //    date: { queryJson: JSON.stringify(param) },
//                //    async: false,
//                //    success: function (item) {
//                //        $.datelist = data.data;
//                //    }
//                //});
//            });
        }
    };
    page.init();
}