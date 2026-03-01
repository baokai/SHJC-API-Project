using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
    /// 创 建：超级管理员
    /// 日 期：2022-03-16 18:06
    /// 描 述：用工申请
    /// </summary>
    public class ProjectRecruitEntity 
    {
        #region 实体成员
        /// <summary>
        /// id
        /// </summary>
        [Column("ID")]
        public string id { get; set; }
        /// <summary>
        /// WorkFlowId
        /// </summary>
        [Column("WORKFLOWID")]
        public string WorkFlowId { get; set; }
        /// <summary>
        /// ProjectId
        /// </summary>
        [Column("PROJECTID")]
        public string ProjectId { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        [Column("APPLYPERSON")]
        public string ApplyPerson { get; set; }
        /// <summary>
        /// 用工资源
        /// </summary>
        [Column("WAGESOURCE")]
        public string WageSource { get; set; }
        /// <summary>
        /// JobType
        /// </summary>
        [Column("JOBTYPE")]
        public string JobType { get; set; }
        /// <summary>
        /// PersonQty
        /// </summary>
        [Column("PERSONQTY")]
        public int? PersonQty { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [Column("PRICE")]
        public decimal? Price { get; set; }
        /// <summary>
        /// Amount
        /// </summary>
        [Column("AMOUNT")]
        public decimal? Amount { get; set; }
        /// <summary>
        /// PayeeUnit
        /// </summary>
        [Column("PAYEEUNIT")]
        public string PayeeUnit { get; set; }
        /// <summary>
        /// PayeeBank
        /// </summary>
        [Column("PAYEEBANK")]
        public string PayeeBank { get; set; }
        /// <summary>
        /// PayeeAccount
        /// </summary>
        [Column("PAYEEACCOUNT")]
        public string PayeeAccount { get; set; }
        /// <summary>
        /// PaymentMethod
        /// </summary>
        [Column("PAYMENTMETHOD")]
        public string PaymentMethod { get; set; }
        /// <summary>
        /// RecruitStatus
        /// </summary>
        [Column("RECRUITSTATUS")]
        public int? RecruitStatus { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CREATETIME")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CREATEUSER")]
        public string CreateUser { get; set; }
        /// <summary>
        /// 创建人部门
        /// </summary>
        [Column("DEPARTMENTID")]
        public string DepartmentId { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Column("UPDATETIME")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        [Column("UPDATEUSER")]
        public string UpdateUser { get; set; }
        /// <summary>
        /// 审批人
        /// </summary>
        [Column("APPROVER")]
        public string Approver { get; set; }
        /// <summary>
        /// 审批时间
        /// </summary>
        [Column("APPROVERTIME")]
        public string ApproverTime { get; set; }


        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 现场用工时间
        /// </summary>
        [Column("WORKINGTIME")]
        public DateTime? WorkingTime { get; set; }



        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.CreateTime = DateTime.Now;
            this.UpdateTime = DateTime.Now;
            this.UpdateUser = LoginUserInfo.Get().userId;
            this.CreateUser = LoginUserInfo.Get().userId;
            this.DepartmentId = LoginUserInfo.Get().departmentId;
            this.RecruitStatus = 1;
            this.id = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {
            this.UpdateTime = DateTime.Now;
            this.UpdateUser = LoginUserInfo.Get().userId;
            this.id = keyValue;
        }
        #endregion
        #region 扩展字段
        #endregion
    }
}

