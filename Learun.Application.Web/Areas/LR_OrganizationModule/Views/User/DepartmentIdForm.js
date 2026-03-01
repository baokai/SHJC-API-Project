/* * 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
 * Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
 * 创建人：超级管理员
 * 日  期：2022-03-11 00:59
 * 描  述：项目开票
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
            page.bind();
        },
        bind: function () {
            learun.httpAsync('Get', top.$.rootUrl + '/LR_OrganizationModule/User/DepartmentIdListById', { userId: keyValue }, function (data) {

                $("#F_MoreDepartmentName").val(data.F_MoreDepartmentName);                
            });
            // 删除
            $('#lr_deleteDepartmentNameList').on('click', function () {
              
                
                if (learun.checkrow(keyValue)) {
                    learun.layerConfirm('是否清除？', function (res, _index) {
                        if (res) {
                            learun.loading(true, '清除中...');
                            var formData = $('body').lrGetFormData();
                            if (!!keyValue) {
                                formData.F_MoreDepartmentId = ""

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
            });
        }       
    };
    
    // 验证数据是否填写完整
    validForm = function () {
        if (!$('body').lrValidform()) {
            return false;
        }
        return true;
    };
        
    page.init();
}
