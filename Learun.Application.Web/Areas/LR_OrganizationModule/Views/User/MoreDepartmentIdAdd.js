/*
 * 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
 * Copyright (c) 2013-2020 上海力软信息技术有限公司
 * 创建人：力软-前端开发组
 * 日 期：2017.08.04
 * 描 述：流程（我的任务）	
 */
var refreshGirdData;
var keyValue = request('keyValue');
var bootstrap = function ($, learun) {
    "use strict";
    var logbegin = '';
    var logend = '';

    refreshGirdData = function () {
        $('#gridtable').jfGridSet('reload');
    }
    var page = {
        init: function () {

            page.initGrid();
            page.bind();

        },
        bind: function () {

            // 刷新
            $('#lr_refresh').on('click', function () {
                location.reload();
            });
            //批量添加
            $('#lr_UserList').on('click', function () {
                var F_MoreDepartmentId = $('#gridtable').jfGridValue('F_DepartmentId');
                if (F_MoreDepartmentId != null) {
                    if (learun.checkrow(keyValue)) {
                        learun.layerConfirm('是否批量添加？', function (res, _index) {
                            if (res) {
                                learun.loading(true, '批量审核...');
                                var formData = $('body').lrGetFormData();
                                if (!!keyValue) {
                                    formData.F_MoreDepartmentId = F_MoreDepartmentId
                                      
                                }
                                var postData = {
                                    strEntity: JSON.stringify(formData)

                                };
                                $.lrSaveForm(top.$.rootUrl + '/LR_OrganizationModule/User/SaveFormList?keyValue=' + keyValue, postData, function (res) {
                                    top.layer.closeAll(_index);
                                });
                                top.layer.close(_index);

                            }
                        });
                    }

                } else {
                    learun.alert.warning("请选择要添加的项目名称以及填写信息完整");
                    return;
                }
              
            });
            //清除多部门
            $('#lr_List1').on('click', function () {
          
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '已添加多部门',
                        url: top.$.rootUrl + '/LR_OrganizationModule/User/DepartmentIdForm?keyValue=' + keyValue,
                        width: 750,
                        height: 250,
                        btn: null,
                        callBack: function (id) {
                            return top[id].acceptClick(refreshGirdData);
                        }
                    });
                }                                                        
            });
        },

        initGrid: function () {

            $('#gridtable').jfGrid({
                url: top.$.rootUrl + '/LR_OrganizationModule/Department/GetEntityList',
                headData: [
                    { label: "部门名称", name: "F_FullName", width: 100, align: "left" }
                ],
                mainId: 'F_DepartmentId',
                isPage: true,
                sidx: 'F_CreateDate DESC',
                isMultiselect: true,
                
            });
            page.search();
        },
        search: function (param) {
            param = param || {};
            param.StartTime = logbegin;
            param.EndTime = logend;

            $('#gridtable').jfGridSet('reload', { queryJson: JSON.stringify(param) });
        }

    };

    page.init();
}


