/* * 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
 * Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
 * 创建人：超级管理员
 * 日  期：2022-03-11 00:18
 * 描  述：项目任务单
 */
var acceptClick;
var keyValue = request('keyValue');
// 设置权限
var setAuthorize;
// 设置表单数据
var setFormData;
// 验证数据是否填写完整
var validForm;
// 保存数据
var save;
var bootstrap = function ($, learun) {
    var page = {
        init: function () {
            $('.lr-form-wrap').lrscroll();
            page.bind();
            page.initData();
        },
        bind: function () {
            $('#MainDepartmentId').lrDepartmentSelect();
            $('#SubDepartmentId').lrDepartmentSelect();
            //$('#ReportTemplateFile').lrUploader({ isDown: true, isView: true, isUpload: false });
            $('#ReportFile').lrUploader({ isDown: true, isView: true, isUpload: false });
          
        },
        initData: function () {
            if (!!keyValue) {
                $.lrSetForm(top.$.rootUrl + '/LR_CodeDemo/ProjectTask/GetPreviewFormData?keyValue=' + keyValue + "&report=0", function (data) {
                    for (var id in data) {
                        if (!!data[id].length && data[id].length > 0) {
                            $('#' + id).jfGridSet('refreshdata', data[id]);
                        }
                        else {
                            $('[data-table="' + id + '"]').lrSetFormData(data[id]);
                        }
                        console.log(data[id].ProjectId)

                    }
                });
            }
        }

    }
    page.init();
}
