/*
 * 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
 * Copyright (c) 2013-2020 上海力软信息技术有限公司
 * 创建人：力软-前端开发组
 * 日 期：2017.08.04
 * 描 述：流程（我的任务）	
 */
var refreshGirdData;
var CapitalAmountSaveForm;
var PayType;
var Payee;
var PayeeBank;
var BankAccount;
var PaymentAmount;
var PaymentMethod;
var PaymentHeader;
var PaymentReason;
var PaymentFile;
var data;
var dataName;
var Idlist;
var selectedRows;
var val;
var vall;
var valp;
var List;
var selectList;
var keyValue = request('keyValue');
var bootstrap = function ($, learun) {
    "use strict";
    var logbegin = '';
    var logend = '';

    refreshGirdData = function () {
        $('#gridtable').jfGridSet('reload');
    }
    //CapitalAmountSaveForm = function (data) {
    //    console.log(data);
    //    $.ajax({
    //        url: '/LR_CodeDemo/ReportForms/CapitalAmountSaveForm?costAmount=' + data.ContractAmountSUN + '&yearMonth=' + data.ProjectId,
    //        type: "POST",
    //        dataType: "json",
    //        async: true,
    //        cache: false,
    //        success: function (res) {
    //            console.log(res);
    //            if (res.code == 200) {
    //                 //refreshGirdData();
    //            }
    //            else {
    //                learun.alert.error(res.info);
    //                learun.loading(false);
    //            }
    //        },
    //        error: function (XMLHttpRequest, textStatus, errorThrown) {
    //            learun.httpErrorLog(textStatus);
    //            learun.loading(false);
    //            callback(exres);
    //        },
    //        beforeSend: function () {
    //        },
    //        complete: function () {
    //        }
    //    });

    //}
    var page = {
        init: function () {
            selectedRows = [];
            List = [];
            page.initGrid();
            page.bind();

        },
        bind: function () {
            $('#datesearch').lrdate({
                dfdata: [
                    { name: '今天', begin: function () { return learun.getDate('yyyy-MM-dd 00:00:00') }, end: function () { return learun.getDate('yyyy-MM-dd 23:59:59') } },
                    { name: '近7天', begin: function () { return learun.getDate('yyyy-MM-dd 00:00:00', 'd', -6) }, end: function () { return learun.getDate('yyyy-MM-dd 23:59:59') } },
                    { name: '近1个月', begin: function () { return learun.getDate('yyyy-MM-dd 00:00:00', 'm', -1) }, end: function () { return learun.getDate('yyyy-MM-dd 23:59:59') } },
                    { name: '近3个月', begin: function () { return learun.getDate('yyyy-MM-dd 00:00:00', 'm', -3) }, end: function () { return learun.getDate('yyyy-MM-dd 23:59:59') } },
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
                selectfn: function (begin, end) {
                    logbegin = begin;
                    logend = end;

                    page.search();
                }
            });
           
            $('#PaymentMethod').lrDataItemSelect({ code: 'Client_PaymentMode', maxHeight: 230 });
            $('#PayType').lrDataItemSelect({ code: 'PayType', maxHeight: 230 });
            $('#PaymentHeader').lrDataItemSelect({ code: 'PaymentHeader', maxHeight: 230 });
            $('#PaymentFile').lrUploader();

            $('#lr_agreList1').on('click', function () {
                var formData = $('body').lrGetFormData();
             
               
                if (learun.checkrow(keyValue)) {
                        learun.layerConfirm('是否批量添加？', function (res, _index) {
                            if (res) {
                                learun.loading(true, '批量审核...');
                                var formData = $('body').lrGetFormData();
                                var item_list = $('#gridtable').jfGridGet('rowdatas');
                                var postData = {
                                    strEntity: JSON.stringify(formData),
                                    itemList: JSON.stringify(item_list)
                                };
                                
                                $.lrSaveForm(top.$.rootUrl + '/LR_CodeDemo/ProjectPaymentList/SaveFormList?Ids=' + keyValue, postData, function (res) {
                                    refreshGirdData();
                                    top.layer.closeAll(_index);
                                });

                                //learun.httpAsync('Post', top.$.rootUrl + '/LR_CodeDemo/ProjectPayment/SaveFormList?Ids=' + data, postData, function (data) {
                                //    learun.loading(false);
                                //    refreshGirdData();
                                //    learun.frameTab.currentIframe().refreshGirdData && learun.frameTab.currentIframe().refreshGirdData();
                                //});
                                top.layer.close(_index);

                            }
                        });
                    }

                

            });
        },

        initGrid: function () {

            $('#gridtable').jfGrid({
                //url: top.$.rootUrl + '/LR_CodeDemo/ProjectManage/GetSelectedProjectListTi',
                url: top.$.rootUrl + '/LR_CodeDemo/ProjectPaymentList/GetPageListById',
                headData: [
                    {
                        label: "项目名称", name: "ProjectName", width: 350, align: "left" ,
                       
                         
                                            },
                    {
                        name: "ContractAmountSUN", label: "支付金额", width: 150, align: "left", statistics: true,
                        edit: {
                            type: 'input',
                            change: function (data, rownum) {// 行数据和行号,弹层选择行的数据，如果是自定义实现弹窗方式则该方法无效
                                //CapitalAmountSaveForm(data);
                            }
                        },

                    },
                ],
                mainId: 'ProjectId',
                isPage: true,
                
                isMultiselect: false,
              
            });
            page.search();
        },
        search: function (param) {
            param = param || {};
            param.Id = keyValue;

            $('#gridtable').jfGridSet('reload', { queryJson: JSON.stringify(param) });
        }

    };

    page.init();
}


