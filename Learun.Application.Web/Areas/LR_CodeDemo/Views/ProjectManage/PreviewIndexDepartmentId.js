/* * 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
 * Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
 * 创建人：超级管理员
 * 日  期：2022-03-10 22:29
 * 描  述：项目管理
 */
var refreshGirdData;
var createContractWorkFlow;
var bootstrap = function ($, learun) {
    "use strict";
    var startTime;
    var endTime;
    var page = {
        init: function () {
            page.initGird();
            page.bind();
        },
        bind: function () {

            // 查询 
            $('#btn_Search').on('click', function () {
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
            $('#multiple_condition_query').lrMultipleQuery(function (queryJson) {
                page.search(queryJson);
            }, 290, 400);

            $('#ProjectStatus').lrRadioCheckbox({
                type: 'radio',
                code: 'projectStatus'
            });
            $('#F_PurchaseType').lrDataItemSelect({ code: 'menuTrage' });
            $('#FollowPerson').lrDataSourceSelect({ code: 'userdata', value: 'f_userid', text: 'f_realname' });
            $('#PreparedPerson').lrDataSourceSelect({ code: 'userdata', value: 'f_userid', text: 'f_realname' });
            $('#F_Department').lrDataSourceSelect({ code: 'company', value: 'f_companyid', text: 'f_shortname' });
            // 刷新
            $('#lr_refresh').on('click', function () {
                location.reload();
            });


            // 查看     
            $('#lr_view').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('Id');

                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '查看项目信息',
                        url: top.$.rootUrl + '/LR_CodeDemo/ProjectManage/PreviewForm?keyValue=' + keyValue,
                        width: 1400,
                        height: 800,
                        maxmin: true,
                        btn: null
                    });
                }
            });

            // 预览     
            $('#lr_preview').on('click', function () {
                var keyValue = $('#gridtable').jfGridValue('Id');

                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '查看项目信息',
                        url: top.$.rootUrl + '/LR_CodeDemo/ProjectManage/PreviewIndexFrom?keyValue=' + keyValue,
                        width: 600,
                        height: 300,
                        maxmin: true,
                        btn: null
                    });
                }
            });
        },
        // 初始化列表
        initGird: function () {
            $('#gridtable').jfGrid({
                url: top.$.rootUrl + '/LR_CodeDemo/ProjectManage/GetAllPageList',
               // url: top.$.rootUrl + '/LR_CodeDemo/ProjectManage/GetAllPageListDepartmentId',
                headData: [
                    {
                        label: "创建日期", name: "CreateTime", width: 80, align: "center",
                        formatter: function (cellvalue) {
                            return learun.formatDate(cellvalue, 'yyyy-MM-dd');
                        }
                    },
                    { label: "项目编号", name: "ProjectCode", width: 100, align: "center" },
                    { label: "项目名称", name: "ProjectName", width: 300, align: "left" },
                    { label: "委托单位", name: "CustName", width: 250, align: "left" },
                    { label: "地址", name: "Address", width: 200, align: "left" },
                    {
                        label: "报备人", name: "PreparedPerson", width: 80, align: "center",
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
                        label: "跟进人", name: "FollowPerson", width: 80, align: "center",
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
                        label: "项目状态", name: "ProjectStatus", width: 70, align: "left",
                        formatterAsync: function (callback, value, row, op, $cell) {
                            learun.clientdata.getAsync('dataItem', {
                                key: value,
                                code: 'projectStatus',
                                callback: function (_data) {
                                    callback(_data.text);
                                }
                            });
                        }
                    },
                ],
                mainId: 'Id',
                sidx: 'CreateTime',
                sord: 'DESC',
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
