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
           
            $('#F_HZ').lrRadioCheckbox({
                type: 'radio',
                code: 'F_HZ',
                fristSelected: true
            });

            // 部门
            $('#F_DepartmentId').lrDepartmentSelect({ companyId: companyId });


            // 性别
            $('#F_Gender').lrselect();
        },
        initData: function () {

            if (!!selectedRow) {
                console.log(selectedRow);
                keyValue = selectedRow.F_UserId;
                selectedRow.F_Password = "******";
                $('#form').lrSetFormData(selectedRow);
                $('#F_Password').attr('readonly', 'readonly');
                $('#F_Account').attr('readonly', 'readonly');

                $('#F_Password').attr('unselectable', 'on');
                $('#F_Account').attr('unselectable', 'on');
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

                if (!$('#form').lrValidform()) {
                    return false;
                }
                var postData = $('#form').lrGetFormData(keyValue);
                if (!keyValue) {
                    postData.F_Password = $.md5(postData.F_Password);
                }
                console.log(postData)
                $.lrSaveForm(top.$.rootUrl + '/LR_OrganizationModule/User/SaveForm?keyValue=' + keyValue, postData, function (res) {
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