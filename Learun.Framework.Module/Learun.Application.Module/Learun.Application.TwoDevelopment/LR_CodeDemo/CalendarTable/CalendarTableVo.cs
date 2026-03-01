using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
    /// 创 建：超级管理员
    /// 日 期：2022-03-11 00:59
    /// 描 述：项目开票
    /// </summary>
    public class CalendarTableVo
    {
        #region 实体成员
        /// <summary>
        /// 报告id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string F_FullName { get; set; }
        /// <summary>
        /// 检测员
        /// </summary>
        public string F_RealName { get; set; }
        /// <summary>
        /// 检测员Id
        /// </summary>
        public string Inspector { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int CalendarStatus { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 进场时间
        /// </summary>
        public DateTime? ApproachTime { get; set; }
        /// <summary>
        /// 报告计划时间
        /// </summary> 
        public DateTime? PlanTime { get; set; }
        /// <summary>
        /// 实际进场时间
        /// </summary>
        public DateTime? ActualApproachTime { get; set; }
        /// <summary>
        /// 实际离场时间
        /// </summary>
        public DateTime? ActualDepartureTime { get; set; }
        #endregion

    }
}

