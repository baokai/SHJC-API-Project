var dfopid = request('dfopid');
var selectValue = request('selectValue');

var acceptClick;
var bootstrap = function ($, learun) {
    "use strict";

    var page = {
        init: function () {
            page.bind();
        },
        bind: function () {

            // 查询
            $('#btn_Search').on('click', function () {
                var keyword = $('#txt_Keyword').val();
                var param = {};
                param.keyword = keyword;
                param.ProjectStatus = 1;
                page.search({ queryJson: JSON.stringify(param) });
            });
  
            $('#gridtable').jfGrid({
               // url: top.$.rootUrl + '/LR_CodeDemo/ProjectManage/GetSelectedProjectListT',
                url: top.$.rootUrl + '/LR_CodeDemo/ProjectManage/GetSelectedProjectByContractListBilling',
                headData: [
                    { label: "项目编号", name: "ProjectCode", width: 90, align: "center" },
                    { label: "项目名称", name: "ProjectName", width: 350, align: "left" },
                    { label: "合同金额", name: "ContractAmount", width: 100, align: "left" },
                    { label: "合同编号", name: "ContractNo", width: 110, align: "center" },
                    { label: "委托单位", name: "CustName", width: 100, align: "left" }
                ],
                mainId: 'Id',
                sidx: 'CreateTime',
                sord: 'DESC',
                isPage: true
            });

            page.search();
        }
        , search: function (param) {
            var data = {};
            data.ProjectStatus = 1;
            $.extend(param, data);
            $('#gridtable').jfGridSet('reload', param);
        }
    };
    // 保存数据
    acceptClick = function (callBack) {
        var selectedRow = $('#gridtable').jfGridGet('rowdata');
        selectedRow.value = selectedRow.Id;
        selectedRow.text = selectedRow.ProjectName;

        if (!selectedRow.value) {
            learun.alert.warning("请选择");
            return false;
        }

        callBack(selectedRow, dfopid);
        return true;
    };
    page.init();
}

