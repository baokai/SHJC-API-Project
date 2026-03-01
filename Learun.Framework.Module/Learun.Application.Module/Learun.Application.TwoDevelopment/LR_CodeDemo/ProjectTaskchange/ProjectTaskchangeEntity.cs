using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
    /// 创 建：超级管理员
    /// 日 期：2022-03-11 00:18
    /// 描 述：项目任务单
    /// </summary>
    public class ProjectTaskchangeEntity
    {
        #region 实体成员
        /// <summary>
        /// Id
        /// </summary>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 变更id
        /// </summary>
        [Column("PROJECTID")]
        public string ProjectId { get; set; }
        /// <summary>
        /// 变更原因
        /// </summary>
        [Column("CHANGERECORD")]
        public string ChangeRecord { get; set; }
        /// <summary>
        /// 变更人
        /// </summary>
        [Column("UPDATEUSER")]
        public string UpdateUser { get; set; }
       /// <summary>
       /// 变更时间
       /// </summary>
       [Column("UPDATETIME")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// WorkFlowId
        /// </summary>
        [Column("WORKFLOWID")]
        public string WorkFlowId { get; set; }

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.UpdateTime = DateTime.Now;
            this.UpdateUser = LoginUserInfo.Get().userId;
            this.Id = Guid.NewGuid().ToString();
        }
       

        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {
            this.UpdateTime = DateTime.Now;
            this.UpdateUser = LoginUserInfo.Get().userId;
            this.Id = keyValue;
        }
        #endregion
        #region 扩展字段
        #endregion
    }
}

