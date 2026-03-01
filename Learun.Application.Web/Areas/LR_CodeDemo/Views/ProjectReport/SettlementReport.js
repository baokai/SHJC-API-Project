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
            $("#gridtable").height($(window).height() - 170);
            $('#gridtable').jfGrid({
                url: top.$.rootUrl + '/LR_ReportModule/PdmReport/GetPDMMarketingReport',
                headData: [
                    { name: "PRJ_PROJECTNO", label: "项目编号", width: 120, align: "center" },
                    { name: "PRJ_PROJECTNAME", label: "项目名称", width: 150, align: "left" },
                    { name: "PRJ_CLIENTNAME", label: "委托单位", width: 80, align: "left" },
                    { name: "CONTRACT", label: "合同主体", width: 150, align: "left" },
                    { name: "CONTRACT", label: "合同状态 ", width: 80, align: "left" },
                    { name: "CONTRACT", label: "项目来源", width: 80, align: "center" },
                    { name: "CONTRACT", label: "项目来源人", width: 80, align: "center" },
                    { name: "CONTRACT", label: "合作伙伴", width: 80, align: "right" },
                    { name: "CONTRACT_AMOUNT", label: "合同额（元）", width: 150, align: "left" },
                    { name: "CONTRACT", label: "开票金额", width: 150, align: "left" },
                    { name: "PAYMENT_AMOUNT", label: "已到金额", width: 150, align: "left" },
                    { name: "REMAINING_AMOUNT", label: "未到金额", width: 150, align: "left" },
                    { name: "CONTRACT", label: "营销部门", width: 150, align: "left" },
                    { name: "CONTRACT", label: "营销人员 ", width: 150, align: "left" },
                    { name: "PT_BEGINDATE", label: "项目实施", width: 150, align: "left" },
                    { name: "REPORT_SUBJECT", label: "实施人员", width: 150, align: "left" },
                    { name: "CONTRACT", label: "报告状态", width: 150, align: "left" },
                    { name: "CONTRACT", label: "营销外付", width: 150, align: "left" },
                    { name: "CONTRACT", label: "外付比例", width: 150, align: "left" }, 
                    { name: "CONTRACT", label: "分包金额", width: 150, align: "left" }, 
                    { name: "CONTRACT", label: "生产成本", width: 150, align: "left" }, 
                    { name: "CONTRACT", label: "钧测税金", width: 150, align: "left" }, 
                    { name: "CONTRACT", label: "钧测管理费", width: 150, align: "left" }, 
                    { name: "CONTRACT", label: "平台成本", width: 150, align: "left" }, 
                    { name: "CONTRACT", label: "其他扣款", width: 150, align: "left" }, 
                    { name: "CONTRACT", label: "部门结余", width: 150, align: "left" }, 
                    { name: "CONTRACT", label: "划拨网络推广", width: 150, align: "left" }, 
                    { name: "CONTRACT", label: "部门净结余", width: 150, align: "left" },
                    { name: "CONTRACT", label: "部门净结余", width: 150, align: "left" },
                    { name: "CONTRACT", label: "结算状态", width: 150, align: "left" },
                    { name: "CONTRACT", label: "备注", width: 150, align: "left" }
                ],
                isPage: true,
                mainId: 'PRJ_PROJECTNO'
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


