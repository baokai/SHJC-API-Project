using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
    /// 创 建：超级管理员
    /// 日 期：2022-03-11 00:04
    /// 描 述：合同支付
    /// </summary>
    public class ProjectPaymentListEntity
    {
        #region 实体成员
        /// <summary>
        /// id
        /// </summary>
        [Column("ID")]
        public string id { get; set; }
        /// <summary>
        /// tid
        /// </summary>
        [Column("TID")]
        public string tid { get; set; }
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
        [Column("PAYTYPE")]
        public string PayType { get; set; }

        /// <summary>
        /// 支付公司
        /// </summary>
        [Column("PAYEE")]
        public string Payee { get; set; }
        /// <summary>
        /// 银行
        /// </summary>
        [Column("PAYEEBANK")]
        public string PayeeBank { get; set; }
        [Column("BANKACCOUNT")]
        public string BankAccount { get; set; }
        /// <summary>
        /// 支付金额比例
        /// </summary>
        [Column("PAYMENTAMOUNTSUM")]
        public decimal? PaymentAmountsum { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        [Column("PAYMENTAMOUNT")]
        public decimal? PaymentAmount { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        [Column("PAYMENTMETHOD")]
        public string PaymentMethod { get; set; }
        /// <summary>
        /// 支付抬头
        /// </summary>
        [Column("PAYMENTHEADER")]
        public string PaymentHeader { get; set; }
        /// <summary>
        /// 支付原因
        /// </summary>
        [Column("PAYMENTREASON")]
        public string PaymentReason { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        [Column("PAYMENTSTATUS")]
        public int? PaymentStatus { get; set; }
        [Column("PAYMENTFILE")]
        public string PaymentFile { get; set; }
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

        /// <summary>
        /// 提审人
        /// </summary>
        [Column("CONTRACTSUBMITTER")]
        public string ContractSubmitter { get; set; }

        /// <summary>
        /// 提审人部门Code
        /// </summary>
        [Column("CONTRACTSUBMITTERDEPTCODE")]
        public string ContractSubmitterDeptCode { get; set; }
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
            this.PaymentStatus =1;
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

