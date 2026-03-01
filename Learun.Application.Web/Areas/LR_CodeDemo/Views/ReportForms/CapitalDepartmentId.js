/*
 * 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
 * Copyright (c) 2013-2020 上海力软信息技术有限公司
 * 创建人：力软-前端开发组
 * 日 期：2017.08.04
 * 描 述：流程（我的任务）	
 */
var refreshGirdData;
var CapitalAmountSaveForm;

var bootstrap = function ($, learun) {
    "use strict";
    var logbegin = '';
    var logend = '';
    var quer;
    var qq;
    refreshGirdData = function () {
        $('#gridtable').jfGridSet('reload');
    }
    CapitalAmountSaveForm = function (data) {
        console.log(data);
        $.ajax({
            url: '/LR_CodeDemo/ReportForms/CapitalAmountSaveForm?costAmount=' + data.ContractAmountSUN + '&yearMonth=' + data.yefenList,
            type: "POST",
            dataType: "json",
            async: true,
            cache: false,
            success: function (res) {
                console.log(res);
                if (res.code == 200) {
                    // refreshGirdData();
                }
                else {
                    learun.alert.error(res.info);
                    learun.loading(false);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                learun.httpErrorLog(textStatus);
                learun.loading(false);
                callback(exres);
            },
            beforeSend: function () {
            },
            complete: function () {
            }
        });

    }
    var page = {
        init: function () {

            page.initGrid();
            page.bind();

        },
        bind: function () {

            $('#multiple_condition_query').lrMultipleQuery(function (queryJson) {
                $.QueryJson = queryJson;
                qq = 1;
                quer = queryJson;
                page.search(queryJson);
            }, 150, 400);
            $("#DepartmentId").lrDepartmentSelect();
            // 刷新
            $('#lr-replace').on('click', function () {
                location.reload();
            });
            // 打印
            $('#lr_print').on('click', function () {
                $('#gridtable').jqprintTable();
            });
            //导出

            $('#lr-export').on('click', function () {
                learun.loading(true);
                $.QueryJson = $.QueryJson || {};
                if (qq == 1) {
                    $.QueryJson.YYYYTime = quer.YYYYTime;
                    $.QueryJson.DepartmentId = quer.DepartmentId;
                } else {
                    $.QueryJson.YYYYTime = null;
                    $.QueryJson.DepartmentId = null;
                }

                learun.httpAsync('Get', top.$.rootUrl + '/LR_CodeDemo/ReportForms/GetCapitalDepartmentIdListAll', { queryJson: JSON.stringify($.QueryJson) }, function (data) {
                    var uuid = data.rows;
                    learun.download({
                        method: "POST",
                        url: '/Utility/CapitalDepartmentIdForms',
                        param: {
                            fileName: "资金台账",
                            columnJson: JSON.stringify($('#gridtable').jfGridGet('settingInfo').headData),
                            dataJson: uuid
                        }
                    });
                    learun.loading(false);
                });
            });

        },

        initGrid: function () {

            $('#gridtable').jfGrid({
                url: top.$.rootUrl + '/LR_CodeDemo/ReportForms/GetCapitalDepartmentId',
                headData: [
                    { name: "yefen", label: "月份", width: 100, align: "center", },
                    { name: "DepartmentIdName", label: "部门", width: 120, align: "center" },
                    {
                        name: "AmountList", label: "历史数据", width: 170, align: "center",
                        formatter: function (cellvalue, row, op, $cell) {

                            return cellvalue.toFixed(2)

                        }
                    },
                    {
                        name: "ContractAmountList", label: "有效合同额", width: 170, align: "center",
                        formatter: function (cellvalue, row, op, $cell) {

                            return cellvalue.toFixed(2)

                        }
                    },
                    {
                        name: "EffectiveAmountList", label: "本月绩效", width: 150, align: "center",
                        formatter: function (cellvalue, row, op, $cell) {

                            return cellvalue.toFixed(2)

                        }
                    },
                    //{
                    //    name: "ContractAmountSUN", label: "本月成本", width: 350, align: "left",
                    //    edit: {
                    //        type: 'input',
                    //        change: function (data, rownum) {// 行数据和行号,弹层选择行的数据，如果是自定义实现弹窗方式则该方法无效
                    //            CapitalAmountSaveForm(data);
                    //        }
                    //    },

                    //},
                    {
                        name: "ContractAmountSUNList", label: "本月资金", width: 250, align: "left",
                        formatter: function (cellvalue, row, op, $cell) {

                            return cellvalue.toFixed(2)

                        }
                    },
                    {
                        name: "sumList", label: "小计", width: 70, align: "center",
                        formatter: function (cellvalue, row, op, $cell) {

                            return cellvalue.toFixed(2)

                        }
                    },

                ],
                mainId: 'yefen',

            });
            page.search();
        },
        search: function (param) {
            param = param || {};
            //param.StartTime = logbegin;
            //param.EndTime = logend;

            $('#gridtable').jfGridSet('reload', { queryJson: JSON.stringify(param) });
        }

    };

    page.init();
}


