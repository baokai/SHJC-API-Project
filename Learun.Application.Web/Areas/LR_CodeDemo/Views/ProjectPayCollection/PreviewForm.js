/* * 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
 * Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
 * 创建人：超级管理员
 * 日  期：2022-03-11 00:59
 * 描  述：回款管理
 */
var keyValue = request('keyValue');
// 设置表单数据
var setFormData;
var bootstrap = function ($, learun) {
    var page = {

        init: function () {
            $('.lr-form-wrap').lrscroll();
            page.initData();
        },
        initData: function () {
            ``
            if (!!keyValue) {
                $.lrSetForm(top.$.rootUrl + '/LR_CodeDemo/ProjectPayCollection/GetPreviewFormData?keyValue=' + keyValue, function (data) {
                    for (var id in data) {
                        if (!!data[id].length && data[id].length > 0) {
                            $('#' + id).jfGridSet('refreshdata', data[id]);
                        }
                        else {
                            $('[data-table="' + id + '"]').lrSetFormData(data[id]);
                        }
                    }
                });
            }
        }
    };
    // 验证数据是否填写完整

    page.init();
}
