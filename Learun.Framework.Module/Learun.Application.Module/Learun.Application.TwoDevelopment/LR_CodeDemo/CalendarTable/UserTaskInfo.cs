
using Learun.Util;

using System;
using System.Collections.Generic;
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
   

    public class UserTaskInfo
    {

        public string UserId { get; set; }
        /// <summary>
        /// 人员
        /// </summary>
        public string UserName { get; set; }
        public List<UserStatusInfo> UserStatusInfos { get; set; }
    }

    public class UserStatusInfo
    {
        public int Status { get; set; }
    }
    public class UserTimeInfo
    {
        public string StatusDateTime { get; set; }

    }
    public class UserProjectName
    {
        public string ProjectName { get; set; }
    }
    public class UserTimeYear
    {
        public string DateTimeYear { get; set; }
    }



}
