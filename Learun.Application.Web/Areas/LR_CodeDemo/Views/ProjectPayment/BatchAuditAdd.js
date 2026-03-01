/*
 * 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
 * Copyright (c) 2013-2020 上海力软信息技术有限公司
 * 创建人：力软-前端开发组
 * 日 期：2017.08.04
 * 描 述：流程（我的任务）	
 */
var refreshGirdData;
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
var bootstrap = function ($, learun) {
    "use strict";
    var logbegin = '';
    var logend = '';

    refreshGirdData = function () {
        $('#gridtable').jfGridSet('reload');
    }
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
            $('#multiple').lrAddQuery(function (queryJson) {
                PayType = queryJson.PayType;
                Payee = queryJson.Payee;
                PayeeBank = queryJson.PayeeBank;
                BankAccount = queryJson.BankAccount;
                PaymentAmount = queryJson.PaymentAmount;
                PaymentMethod = queryJson.PaymentMethod;
                PaymentHeader = queryJson.PaymentHeader;
                PaymentReason = queryJson.PaymentReason;
                PaymentFile = queryJson.PaymentFile;
                //page.search(queryJson);
            }, 500, 400);
            $('#PaymentMethod').lrDataItemSelect({ code: 'Client_PaymentMode', maxHeight: 230 });
            $('#PayType').lrDataItemSelect({ code: 'PayType', maxHeight: 230 });
            $('#PaymentHeader').lrDataItemSelect({ code: 'PaymentHeader', maxHeight: 230 });
            $('#PaymentFile').lrUploader();
            // 查询
            $('#btn_Search').on('click', function () {
                selectList = 1;
                var keyword = $('#txt_Keyword').val();
                page.search({ keyword: keyword });


            });
            // 刷新
            $('#lr_refresh').on('click', function () {
                location.reload();
            });
            //批量添加
            $('#lr_agreList').on('click', function () {
                if (window.confirm("请您确认添加的项目名称以及信息是否正确,如有错误请刷新重新选择项目名称!!!")) {                
                    alert(dataName)
                } else {
                    alert("取消");
                    return false;
                }

               // alert("请确认添加的项目名称以及信息是否正确,如有错误请刷新重新选择项目名称!!!")
               // alert(dataName)              
                //alert(data)
                //var Ids = $('#gridtable').jfGridValue('Id');
               // alert("贵" + Ids)
                if (PayType != null & Payee != null & PayeeBank != null & BankAccount != null & PaymentAmount != null & PaymentMethod != null & PaymentHeader != null & PaymentReason != null) {
                    if (learun.checkrow(data)) {
                        learun.layerConfirm('是否批量添加？', function (res, _index) {
                            if (res) {
                                learun.loading(true, '批量审核...');
                                var formData = $('body').lrGetFormData();
                                if (!!data) {
                                    formData.PayType = PayType,
                                        formData.Payee = Payee,
                                        formData.PayeeBank = PayeeBank,
                                        formData.BankAccount = BankAccount,
                                        formData.PaymentAmount = PaymentAmount,
                                        formData.PaymentMethod = PaymentMethod,
                                        formData.PaymentHeader = PaymentHeader,
                                        formData.PaymentReason = PaymentReason,
                                        formData.PaymentFile = PaymentFile
                                }
                                var postData = {
                                    strEntity: JSON.stringify(formData)

                                };
                                $.lrSaveForm(top.$.rootUrl + '/LR_CodeDemo/ProjectPayment/SaveFormList?Ids=' + data, postData, function (res) {
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

                } else {
                    learun.alert.warning("请选择要添加的项目名称以及填写信息完整");
                    return;
                }

            });
        },

        initGrid: function () {

            $('#gridtable').jfGrid({
                url: top.$.rootUrl + '/LR_CodeDemo/ProjectManage/GetSelectedProjectListT',
                headData: [
                    { label: "项目编号", name: "ProjectCode", width: 90, align: "center" },
                    { label: "项目名称", name: "ProjectName", width: 400, align: "left" },

                    { label: "委托单位", name: "CustName", width: 250, align: "left" },
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
                isPage: true,
                sidx: 'CreateTime DESC',
                isMultiselect: true,
                onSelectRow: function (rowdata) {
                    selectedRows.push(rowdata);

                    if (data != null) {
                        if (rowdata != null) {                     
                            ////indexOf方法来确定字符串中是否包含了另一个字符串：
                            //val = data.indexOf(rowdata.Id + ",") == -1 ? false : true
                            //valp = data.indexOf("," + rowdata.Id) == -1 ? false : true
                            //if (val) {
                            //    data = data.replace(rowdata.Id + ",", "")
                            //}
                            //else if (valp) {
                            //    data = data.replace("," + rowdata.Id, "")
                            //} else {
                            //    data = data + "," + rowdata.Id;
                            //}
                            //val = dataName.indexOf(rowdata.ProjectName + ",") == -1 ? false : true
                            //valp = dataName.indexOf("," + rowdata.ProjectName) == -1 ? false : true
                            //if (val) {
                            //    dataName = dataName.replace(rowdata.ProjectName + ",", "")
                            //}
                            //else if (valp) {
                            //    dataName = dataName.replace("," + rowdata.ProjectName, "")
                            //} else {
                            //    dataName = dataName + "," + rowdata.ProjectName;
                            //}
                            data = data + "," + rowdata.Id;
                            dataName = dataName + "," + rowdata.ProjectName;


                        }
                    } else {
                        data = rowdata.Id;
                        dataName = rowdata.ProjectName;
                    }

                },
                onRenderComplete: function (rowdatas) {
                    console.log(selectedRows);
                    if (selectedRows.length == 0) {
                        selectedRows = $('#gridtable').jfGridGet('rowdata');

                    }
                    else {
                        let selectedRow = $('#gridtable').jfGridGet('rowdata');
                        for (let i = 0; i < selectedRow.length; i++) {
                            selectedRows.push(selectedRow[i]);

                        }
                    }
                    $('#gridtable').jfGridSet('setcheck_2', selectedRows);


                }
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


