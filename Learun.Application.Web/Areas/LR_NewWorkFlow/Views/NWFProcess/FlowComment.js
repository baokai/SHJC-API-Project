var keyVaule = request('keyVaule');
var bootstrap = function ($, learun) {
    "use strict";
    var processId = request('processId');      // 流程进程主键
    var taskId = request('taskId');            // 流程任务主键
    $.ajax({
        type: 'POST',
        url: top.$.rootUrl + '/LR_NewWorkFlow/NWFProcess/GetCommentsListById',//根据节点id获取当前节点数据
        dataType: "json",
        data: {
            id: taskId,
            WorkFlowId: processId
        },
        success: function (res) {
            console.log("123456789", res);
            var plList = res.data.nodes;
            if (plList.length > 0) {
                var data = plList[0].nodeslistNodelisty;
                for (var i = 0, l = data.length; i < l; i++) {
                    $('#lr_form_file_queue .lr-form-file-queue-bg').hide();
                    var item = data[i];
                    var $item = $('<div class="lr-form-file-queue-item" id="lr_filequeue_' + item.Id + '" ></div>');
                    $item.append('<div class="lr-file-image"><img src="/LR_OrganizationModule/User/GetImg?userId=' + item.CreateUserId + '"></div>');
                    //$item.append('<span class="lr-file-name">' + item.CreateUserName + '</span>');
                    //$item.append('<div><span>创建人：' + item.CommentsName + '</span><span style="padding-left: 20px;">创建时间：' + item.CreateTime + '</span></div>');
                    $item.append('<span class="lr-file-name">' + item.CommentsName + '</span>');
                    $item.append('<div><span>创建人：' + item.CreateUserName + '</span><span style="padding-left: 20px;">创建时间：' + item.CreateTime + '</span></div>');
                    // $item.append('<div class="lr-tool-bar"><i class="fa fa-eye" title="预览"  data-value="' + item.F_FilePath + '" ></i><i class="fa fa-cloud-download" title="下载"  data-value="' + item.F_FilePath + '" ></i></div>');
                    $('#lr_form_file_queue_list').append($item);
                }
            }
        }
    })
    //$.lrSetForm(top.$.rootUrl + '/LR_NewWorkFlow/NWFProcess/GetCommentsListById?id=' + taskId + "&processId=" + processId, function (data) {

    //});
    // 下载文件

    $('#lr_form_file_queue').lrscroll();
}
