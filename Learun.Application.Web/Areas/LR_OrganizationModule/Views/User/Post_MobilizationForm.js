//const { setData } = require("../../../../Content/ueditor/third-party/zeroclipboard/ZeroClipboard");

/*
 * 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
 * Copyright (c) 2013-2020 上海力软信息技术有限公司
 * 创建人：力软-前端开发组
 * 日 期：2017.04.18
 * 描 述：账号添加	
 */
var companyId = request('companyId');


var acceptClick;
var keyValue = '';
var bootstrap = function ($, learun) {
    "use strict";
    var selectedRow = learun.frameTab.currentIframe().selectedRow;
    var page = {
        init: function () {
            page.bind();
            page.initData();

        },
        bind: function () {
            // 部门
            $('#F_DepartmentId').lrDepartmentSelect({ companyId: companyId }).on('change', function () {
                //负责人
                var value = $('#F_DepartmentId').lrselectGet();
                $('#N_FollowPerson').lrselectRefresh({
                    url: top.$.rootUrl + '/LR_OrganizationModule/User/GetListUser',
                    param: { departmentId: value, keyword: keyValue }
                });
            });
            //调岗部门
            $('#P_F_DepartmentId').lrDepartmentSelect({ companyId: companyId })
            $('#N_FollowPerson').lrselect({
                value: 'F_UserId',
                text: 'F_RealName',
                title: 'F_RealName',
                // 展开最大高度
                maxHeight: 110,
                // 是否允许搜索
                allowSearch: true,
                select: function (item) {
                }
            });
        },
        initData: function () { 
            if (!!selectedRow) {
                keyValue = selectedRow.F_UserId;
                selectedRow.F_Password = "******";
                $('#form').lrSetFormData(selectedRow); 
                $('#F_Account').attr('readonly', 'readonly');
                $('#F_RealName').attr('readonly', 'readonly');
                $('#F_DepartmentId').attr('readonly', 'readonly');

                $('#F_Account').attr('unselectable', 'on');
                $('#F_RealName').attr('unselectable', 'on');
                $('#F_DepartmentId').attr('unselectable', 'on');
                

            } 
            else {
                $('#F_CompanyId').val(companyId);
            }
        }
    };
    // 保存数据
    acceptClick = function (callBack) {
        learun.layerConfirm('是否保存', function (res, index) {
            if (res) {
                if (!$("#form").lrValidform()) {
                    return false;
                }
                var formData = $('#form').lrGetFormData(keyValue);
                var dataList = [];
                $('[name ="dataList"]:checked').each(function () {
                    dataList.push($(this).val());
                });
                var postData = {
                    strEntity: JSON.stringify(formData),
                    dataList:dataList
                };
                $.lrSaveForm(top.$.rootUrl + '/LR_OrganizationModule/User/P_SaveForm?keyValue=' + keyValue, postData, function (res) {
                    // 保存成功后才回调
                    if (!!callBack) {
                        callBack();
                    }
                });
                top.layer.close(index); //再执行关闭 
            }
        });



    };
    page.init();
}