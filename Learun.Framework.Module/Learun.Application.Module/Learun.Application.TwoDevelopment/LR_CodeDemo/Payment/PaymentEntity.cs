using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    public class PaymentEntity
    {
        [Column("ID")]
        public string Id { get; set; }


        public string PaymentHeader { get; set; }

        [Column("WORKFLOWID")]
        public string WorkFlowId{ get; set; }

        [Column("PAYTYPE")]
        public string PayType{ get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        [Column("PAYMENTMETHOD")]
        public string PaymentMethod{ get; set; }

        /// <summary>
        /// 支付原因
        /// </summary>
        [Column("PAYMENTREASON")]
        public string PaymentReason{ get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        [Column("PAYMENTAMOUNT")]
        public decimal? PaymentAmount{ get; set; }

        [Column("BANKACCOUNT")]
        public string BankAccount{ get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        [Column("PAYMENTSTATUS")]
        public int? PaymentStatus { get; set; }

        /// <summary>
        /// 银行
        /// </summary>
        [Column("PAYEEBANK")]
        public string PayeeBank{ get; set; }

        [Column("PAYMENTFILE")]
        public string PaymentFile{ get; set; }

        [Column("CREATETIME")]
        public DateTime? CreateTime{ get; set; }

        [Column("CREATEUSER")]
        public string CreateUser{ get; set; }

        [Column("UPDATETIME")]
        public DateTime? UpdateTime{ get; set; }

        [Column("UPDATEUSER")]
        public string UpdateUser{ get; set; }

        /// <summary>
        /// 收款单位名称
        /// </summary>
        [Column("PAYEE")]
        public string Payee{ get; set; }

        /// <summary>
        /// 提审人
        /// </summary>
        [Column("PAYMENTSUBMITTER")]
        public string PaymentSubmitter { get; set; }
        /// <summary>
        /// 创建人部门
        /// </summary>
        [Column("DEPARTMENTID")]
        public string DepartmentId { get; set; }

        /// <summary>
        /// 提审人
        /// </summary>
        [Column("CONTRACTSUBMITTER")]
        public string ContractSubmitter { get; set; }

        //联行号
        [Column("AFFILIATENO")]
        public string AffiliateNo { get; set; }
        /// <summary>
        /// 提审人部门Code
        /// </summary>
        [Column("CONTRACTSUBMITTERDEPTCODE")]
        public string ContractSubmitterDeptCode { get; set; }
        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.CreateTime = DateTime.Now;
            this.UpdateTime = DateTime.Now;
            this.DepartmentId = LoginUserInfo.Get().departmentId;
            this.UpdateUser = LoginUserInfo.Get().userId;
            this.CreateUser = LoginUserInfo.Get().userId;
            this.PaymentStatus = 1;
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

    }
}
