using Learun.Application.Organization;
using Learun.Application.TwoDevelopment.LR_CodeDemo;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
namespace Learun.Application.Web.Areas.LR_CodeDemo.Controllers
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
    /// 创 建：超级管理员
    /// 日 期：2022-03-11 00:59
    /// 描 述：项目开票
    /// </summary>
    public class CalendarTableController : MvcControllerBase
    {

        private ProjectTaskIBLL projectTaskIBLL = new ProjectTaskBLL();

        private UserIBLL userIBLL = new UserBLL();

        #region 视图功能

        /// <summary>
        /// 主页面
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        #endregion
        #region 获取数据

        /// <summary>
        /// 获取生产人员状态接口
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]

        public ActionResult GetProduceUserStatusInfo(string keyword)
        {
            List<object> list = new List<object>();
            List<UserEntity> produceUserList = userIBLL.GetProduceUserList();
            string Status = "";
            foreach (var userItem in produceUserList)
            {
                DateTime StartTime = DateTime.Now;
                DateTime EndTime = StartTime.AddDays(+1);
                List<object> timeList = new List<object>();
                //计算
                //计划时间
                var planInfoList = projectTaskIBLL.GetProjectTaskByInspectorAndPlanTime(userItem.F_UserId, StartTime, EndTime);
                //实际时间
                var ActualList = projectTaskIBLL.GetProjectTaskByInspectorAndActualTime(userItem.F_UserId, StartTime, EndTime);

                if (planInfoList.Count > 0 && ActualList.Count <= 0)
                {
                    Status = "1";//计划忙
                }
                else if (ActualList.Count > 0)
                {
                    Status = "2";//进场
                }
                else
                {
                    Status = " ";//空闲
                }

                timeList.Add(new
                {

                    beginTime = StartTime.ToString(),
                    endTime = EndTime.ToString(),
                    color = string.IsNullOrEmpty(Status) ? "#1bb99a" : Status,
                    overtime = false,
                    text = userItem.F_RealName

                }); ;

                var data = new
                {
                    id = userItem.F_UserId,
                    text = userItem.F_RealName,
                    isexpand = false,
                    complete = false,
                    timeList = timeList,
                    hasChildren = true
                };
                if (!string.IsNullOrEmpty(keyword))
                {
                    if (data.text.IndexOf(keyword) != -1)
                    {
                        list.Add(data);
                    }
                }
                else
                {
                    list.Add(data);
                };
            }
            return Success(list);
        }
        #endregion

    }
}
