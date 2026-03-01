/*
 * 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
 * Copyright (c) 2013-2020 上海力 软信息技术有限公司
 * 创建人：力 软-前端开发组
 * 日 期：2017.03.22
 * 描 述：时间轴方法（降序）
 */
var processId = request('processId');
var nodelist = request('nodelist')
var plINP = request('plINP')
var plList = request('plList')
$.fn.lrtimeline = function (nodelist, fx, fs) {
    var dimension = fx;
    var showComment = fs;
    // title   标题
    // people  审核人
    // content 内容
    // time    时间
    
    var $self = $(this);
    if ($self.length == 0) {
        return $self;
    }
    $self.addClass('lr-timeline');
    var $wrap = $('<div class="lr-timeline-allwrap"></div>');
    var $ul = $('<ul></ul>');


    if (nodelist.length > 0) {
        console.log(nodelist)
        // 开始节点
        var $begin = $('<li class="lr-timeline-header"><div>当前</div></li>')
        $ul.append($begin);
  
        $.each(nodelist, function (_index, _item) {
            var newPL = '';
            // 中间节点
            var $li = $('<li class="lr-timeline-item" ><div class="lr-timeline-wrap" ></div></li>');
            if (_index == 0) {
                $li.find('div').addClass('lr-timeline-current');
            }
            var $itemwrap = $li.find('.lr-timeline-wrap');
            console.log("***********************************",_item)
            var $itemcontent = $('<div class="lr-timeline-content"><span class="arrow"></span></div>');
            $itemcontent.append('<div class="lr-timeline-title">' + _item.title + '</div>');
            $itemcontent.append('<div class="lr-timeline-body"><span>' + _item.people + '</span>' + _item.content + '</div>')

            //for (let item2 of plList) {
            //    console.log('1ID', _item.id)
            //    console.log('2ID', item2.nodIdlisty)
            //    if (item2.nodIdlisty == _item.id) {
            //        newPL = item2.nodeslistNodelisty[0].CreateUserName
            //        console.log('1PL', item2.nodeslistNodelisty[0].CreateUserName)
            //    }
            //    console.log('2PL', newPL)
            //}

            for (let i = 0; i < plList.length; i++) {
                let PL = String(plList[i].nodIdlisty)
                console.log('1ID', _item.id)
                console.log('2ID', PL)
                
                console.log(PL == _item.id)
                if (PL === _item.id) {
                    newPL = plList[i].nodeslistNodelisty[0].CreateUserName
                    console.log('1PL', plList[i].nodeslistNodelisty[0].CreateUserName)
                }
                //console.log('2PL', newPL)
            }

            if (_item.title != '开始' && _item.time != '当前') {

                $itemcontent.append('<div class="pl"><p><span>评论：</span>' + newPL + '</p><span class="showComment"  data-id =' + _item.id + '> 更多>> </span></div>')
                //$itemcontent.append('<div class="pl"><span class="showComment"  data-id =' + _item.id + '> 更多>> </span></div>')
                $itemcontent.append('<div class="lr-timeline-Comment"><button class="test"  data-id =' + _item.id + '>评论</button></div>')
                //$itemcontent.append('<div class="pl"><p><span>评论：</span>' + 2343344444324334324324443434343433432434 + '</p><span onclick="Comment(\'' + _item.id + '\')"> 更多>> </span></div>')
                //$itemcontent.append('<div class="lr-timeline-Comment"><button onclick="plINP(\'' + _item.id + '\')">评论</button></div>')
            }
            $itemcontent.append('<div class="pl_inps" id="plInp"><p onclick="gbSend()">×</p><div class="pl_inp"><input id="inp"></input><button onclick="send()">发送</button></div></div>')
            $itemcontent.append('<div class="lr-timeline-Comments" id="Comments"><div class="gbComments" onclick="gb()"><p>×</p></div><div class="comment" id=""></div></div>')
            $itemwrap.append('<span class="lr-timeline-date">' + _item.time + '</span>');
            $itemwrap.append($itemcontent);
            //_item.CommentsName 评论

           
            var $event = $itemcontent.find('.lr-event');
            if ($event.length > 0) {
                $event[0].lrdata = _item;
                $itemcontent.find('.lr-event').on('click', function () {
                    var data = $(this)[0].lrdata;
                    data.callback && data.callback(data);
                });
            }
          

            $ul.append($li);


        });
        
        // 结束节点
        $ul.append('<li class="lr-timeline-ender"><div>开始</div></li>');

        
    }
    
    $wrap.html($ul);
    $self.html($wrap);
    $(".test").click(function (e) {
        dimension($(this).data("id"));
    });
    $(".showComment").click(function (e) {
        showComment($(this).data("id"));
    });
    //var commentlist = document.getElementById("Comments")
   // console.log(commentlist)
};



var nodeId = '';
function getCommentList(nid) {
    //learun.httpAsync('GET', top.$.rootUrl + '/LR_NewWorkFlow/NWFProcess/GetCommentsList?WorkFlowId=' + processId, null, function (data) {
    //    console.log(data)
    //    //$("#CommentsNameList").val(data.CommentsNameList);
    //});
    //
    $.ajax({
        type: 'POST',
       // url: top.$.rootUrl + '/LR_NewWorkFlow/NWFProcess/GetCommentsListById',
        url: top.$.rootUrl + '/LR_NewWorkFlow/NWFProcess/GetCommentsListById',//根据节点id获取当前节点数据
        //url: top.$.rootUrl + '/LR_NewWorkFlow/NWFProcess/GetCommentsList',//全部数据
        dataType: "json",
        data: {           
           id: nid,
            WorkFlowId: processId
        },
        success: function (res) {
            console.log(res);
            var plList = res.data.nodes;
            if (plList.length > 0) {
                console.log(plList)
                var commentList = plList[0].nodeslistNodelisty;
                var listP = '';
                for (var i = 0; i < commentList.length; i++) {
                    listP += '<div class="comment" id=""><p >' + commentList[i].CreateUserName + '</p><span>' + commentList[i].CommentsName + '</span></div>'
                }
                console.log(listP)
                $(".lr-timeline-content .lr-timeline-Comments .comment").html(listP);
            }
   
        }
    })

    //let a = [{ userName: '123', content: '12323434' }, { userName: '456', content: '54354544' }];
    //let plList = a;
    //let listP = '';
    //for (let i = 0; i < plList.length; i++) {
    //    console.log(plList[i])
    //    listP += '<p >' + plList[i].userName + '</p><span>' + plList[i].content + '</span>'
    //}
    //$(".lr-timeline-content .lr-timeline-Comments .comment").html(listP);
}
function ShowComment(nid) {
    document.getElementById("Comments").style.display = "block";
    this.getCommentList(nid)
}
function gb() {
    document.getElementById("Comments").style.display = "none";
}

function gbSend() {
    
    document.getElementById("plInp").style.display = "none";
}
function send() {
    let a = document.getElementById("inp")
    let b = a.value
    alert(b)
    console.log(this.nodeId)
    
    $.ajax({
        type: 'POST',
        url: top.$.rootUrl + '/LR_NewWorkFlow/NWFProcess/CommentsSave',
        dataType: "json",
        data: {
            WorkFlowId: processId,
            id: this.nodeId,
            CommentsName: b
        },

        success: function (res) {
            console.log(res)
            if (res.code==200) {
                document.getElementById("plInp").style.display = "none";
            }else{
                //alert()
            }
        }
    })
    //document.getElementById("plInp").style.display = "none";
}