/*
 * 日 期：208.11.22
 * 描 述：甘特图
 */
var bootstrap = function ($, learun) {
    "use strict";
    var page = {
        init: function () {
            page.initGantt();
            page.bind();
        },
        bind: function () {
            // 查询
            $('#btn_Search').on('click', function () {
                var keyword = $('#txt_Keyword').val();
                page.search({ keyword: keyword });
            });
            // 刷新
            $('#lr_refresh').on('click', function () {
                location.reload();
            });
        },
        initGantt: function () {
            $('#gridtable').lrGantt({
                url: top.$.rootUrl + '/LR_CodeDemo/GanttDemo/GetTimeList',
                childUrl: top.$.rootUrl + '/LR_CodeDemo/GanttDemo/GetTimeList',
                timebtns: ['month', 'week', 'day'],//'month', 'week', 'day', 'hour'
            }).lrGanttSet('reload');
        },
        search: function (param) {
            $('#gridtable').lrGanttSet('reload', param || {});
        }
    };
    page.init();
}


