
/* * 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
 * Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
 * 创建人：超级管理员
 * 日  期：2022-03-10 22:29
 * 描  述：项目管理
 */
var refreshGirdData;
var createContractWorkFlow;
var select;
var bootstrap = function ($, learun) {
    "use strict";
    var startTime;
    var endTime;
    var processId = '';
    var datelist;
    var select;
    var querselect;
    var processId = request('processId');      // 流程实例主键
    var quer;
    var page = {
        init: function () {
            page.initGird();
            page.bind();
        },
        bind: function () {
            $('#ProjectStatus').lrRadioCheckbox({
                type: 'radio',
                code: 'projectStatus'
            });
            // 查询 
            $('#btn_Search').on('click', function () {
                select = 1;
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
                querselect = 2;
                quer = queryJson;
                page.search(queryJson);
            }, 340, 400);
            $('#F_PurchaseType').lrDataItemSelect({ code: 'menuTrage' });
            //$('#FollowPerson').lrDataSourceSelect({ code: 'userdata', value: 'f_userid', text: 'f_realname' });
            //$('#PreparedPerson').lrDataSourceSelect({ code: 'userdata', value: 'f_userid', text: 'f_realname' });
            $('#F_Department').lrDataSourceSelect({ code: 'company', value: 'f_companyid', text: 'f_shortname' });
            $('#PreparedPerson').lrformselect({
                layerUrl: top.$.rootUrl + '/LR_OrganizationModule/User/SelectForm',
                layerUrlW: 800,
                layerUrlH: 520,
                dataUrl: top.$.rootUrl + '/LR_OrganizationModule/User/GetListByUserIds'
            });
            $('#FollowPerson').lrformselect({
                layerUrl: top.$.rootUrl + '/LR_OrganizationModule/User/SelectForm',
                layerUrlW: 800,
                layerUrlH: 520,
                dataUrl: top.$.rootUrl + '/LR_OrganizationModule/User/GetListByUserIds'
            });
            // 刷新
            $('#lr_refresh').on('click', function () {
                location.reload();
            });
            // 新增
            $('#lr_add').on('click', function () {
                learun.layerForm({
                    id: 'form',
                    title: '新增项目信息',
                    url: top.$.rootUrl + '/LR_CodeDemo/ProjectManage/Form',
                    width: 600,
                    height: 700,
                    btn: ['确认', '关闭'],
                    callBack: function (id) {
                        var res = false;
                        // 验证数据
                        res = top[id].validForm();
                        // 保存数据
                        if (res) {
                            processId = learun.newGuid();
                            res = top[id].save(processId, refreshGirdData);
                        }
                        return res;
                    }
                });
            });
           

          
      

        },
        // 初始化列表
        initGird: function () {
            $('#gridtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/LR_CodeDemo/ProjectManage/GetPageList?processId=' + processId,
                headData: [
                    {
                        label: "创建日期", name: "CreateTime", width: 90, align: "center",
                        formatter: function (cellvalue) {
                            return learun.formatDate(cellvalue, 'yyyy-MM-dd');
                        }
                    },
                    { label: "项目编号", name: "ProjectCode", width: 110, align: "center" },
                    { label: "项目名称", name: "ProjectName", width: 350, align: "left" },
                    { label: "委托单位", name: "CustName", width: 250, align: "left" },
                    { label: "联系人", name: "ContactName", width: 80, align: "left" },
                    { label: "联系电话", name: "ContactPhone", width: 110, align: "left" },
                    /*  { label: "地址", name: "ProvincesAndcities", width: 250, align: "left" },*/
                    { label: "地址", name: "Address", width: 250, align: "left" },
                    { label: "项目来源", name: "ProjectSource", width: 90, align: "center" },
                    {
                        label: "报备人", name: "PreparedPerson", width: 95, align: "center",
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
                        label: "营销人员", name: "FollowPerson", width: 90, align: "center",
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
                    { label: "备注", name: "Remark", width: 80, align: "left" },
                    /* { label: "是否投标", name: "TenderFlg", width: 100, align: "left" },*/

                ],
                sidx: 'CreateTime',
                sord: 'DESC',
                mainId: 'Id',
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
    createContractWorkFlow = function (res, postData) {
        if (!!res) {
            if (res.code == 200) {
                // 发起流程
                var postData = {
                    schemeCode: 'ProjectContract',// 填写流程对应模板编号
                    processId: processId,
                    level: '1',
                };
                learun.httpAsync('Post', top.$.rootUrl + '/LR_NewWorkFlow/NWFProcess/CreateFlow', postData, function (data) {
                    learun.loading(false);
                });
            }
        }
    }

    page.init();
}
