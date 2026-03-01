using Learun.Util;
using System.Data;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
    /// 创 建：超级管理员
    /// 日 期：2022-03-11 00:59
    /// 描 述：项目开票
    /// </summary>
    public interface CalendarTableIBLL
    {

        #region 获取数据
        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<CalendarTableVo> GetPage(Pagination pagination, string queryJson);
        List<ProjectTaskVo> GetTaskInspectorlist();

        /// <summary>
        /// 获取页面显示列表数据检测员
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
       CalendarTableVo GetPageList(string keyvalue);
        /* CalendarTableVo GetId(string id);*/
        #endregion
    }
}
