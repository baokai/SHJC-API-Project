using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.Extention.TaskScheduling
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2019-01-09 16:07
    /// 描 述：任务执行日志
    /// </summary>
    public class TSLogEntity
    {
        #region 实体成员 
        /// <summary> 
        /// 主键 
        /// </summary> 
        [Column("F_ID")]
        public string F_Id { get; set; }
        /// <summary> 
        /// 任务进程主键 
        /// </summary> 
        [Column("F_PROCESSID")]
        public string F_ProcessId { get; set; }
        /// <summary> 
        /// 执行结果 1 :成功  2： 异常 
        /// </summary> 
        [Column("F_EXECUTERESULT")]
        public int? F_ExecuteResult { get; set; }
        /// <summary> 
        /// 创建时间 
        /// </summary> 
        [Column("F_CREATEDATE")]
        public DateTime? F_CreateDate { get; set; }
        /// <summary> 
        /// 描述 
        /// </summary> 
        [Column("F_DES")]
        public string F_Des { get; set; }
        #endregion

        #region 扩展操作 
        /// <summary> 
        /// 新增调用 
        /// </summary> 
        public void Create()
        {
            this.F_Id = Guid.NewGuid().ToString();
            this.F_CreateDate = DateTime.Now;
        }
        /// <summary> 
        /// 编辑调用 
        /// </summary> 
        /// <param name="keyValue"></param> 
        public void Modify(string keyValue)
        {
            this.F_Id = keyValue;
        }
        #endregion
        #region 扩展字段 
        /// <summary>
        /// 任务名称
        /// </summary>
        [NotMapped]
        public string F_Name { get; set; }
        #endregion
    }
}
