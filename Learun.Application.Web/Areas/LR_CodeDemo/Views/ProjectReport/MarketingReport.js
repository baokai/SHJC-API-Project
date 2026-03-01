var bootstrap = function ($, learun) {
    "use strict";

    var page = {
        init: function () {
            page.initGrid();
            page.bind();
        },
        bind: function () {

        },
        initGrid: function () {
            console.log(111)
            $("#gridtable").height($(window).height() - 170);
            $('#gridtable').jfGrid({
                url: top.$.rootUrl + '/LR_CodeDemo/ProjectReport/GetPDMMarketingReport',
                headData: [
                    { name: "ContractTime", label: "合同日期", width: 80, align: "center" },
                    { name: "ProjectCode", label: "项目编号", width: 120, align: "center" },
                    { name: "ProjectName", label: "项目名称", width: 250, align: "left" },
                    { name: "CustName", label: "项目客户", width: 250, align: "left" },
                    {
                        name: "ContractSubject", label: "合同主体", width: 150, align: "left",
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
                        name: "FollowPerson", label: "营销人员", width: 80, align: "center",
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
                        name: "ProjectSource", label: "项目来源", width: 80, align: "left",
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
                        name: "ContractStatus", label: "合同状态", width: 80, align: "right",
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
                    { name: "BillingStatus", label: "开票情况", width: 150, align: "left"},
                    { name: "ContractAmount", label: "合同额（元）", width: 150, align: "left" },
                    { name: "PayCollectionAmount", label: "已到金额（元）", width: 150, align: "left" },
                    { name: "REMAINING_AMOUNT", label: "未到金额（元）", width: 150, align: "left" },
                    { name: "PayCollectionTime", label: "到款日期", width: 150, align: "left" },
                    {
                        name: "CarryDept", label: "实施部门", width: 150, align: "left",
                        formatterAsync: function (callback, value, row) {
                            learun.clientdata.getAsync('department', {
                                key: value,
                                callback: function (item) {
                                    callback(item.name);
                                }
                            });
                        }},
                    { name: "TestDateTime", label: "检测日期", width: 150, align: "left" },
                    {
                        name: "ReportSubject", label: "报告主体", width: 150, align: "left",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'ReportSubject',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        }
                    },
                    {
                        name: "TaskStatus", label: "报告状态", width: 150, align: "left",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'TaskStatus',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        } },
                    {
                        name: "ProjectResponsible", label: "项目负责人", width: 150, align: "left",
                        formatterAsync: function (callback, value, row) {
                            learun.clientdata.getAsync('user', {
                                key: value,
                                callback: function (item) {
                                    callback(item.name);
                                }
                            });
                        }}
                ],
                isPage: true,
                mainId: 'id',
                sidx: 'CreateTime',
                sord: 'DESC',
            });

            page.search();
        },
        search: function (param) {
            param = param || {};
            $('#gridtable').jfGridSet('reload', param);
        }
    };
    page.init();
}


