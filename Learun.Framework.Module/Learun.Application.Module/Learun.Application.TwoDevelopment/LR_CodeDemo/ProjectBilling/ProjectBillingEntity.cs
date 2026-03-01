using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 日 期：2022-03-11 00:59
    /// 描 述：项目开票
    /// </summary>
    public class ProjectBillingEntity 
    {
        #region 实体成员

        /// <summary>
        /// Id
        /// </summary>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        [Column("PROJECTID")]
        public string ProjectId { get; set; }
        /// <summary>
        /// WorkFlowId
        /// </summary>
        [Column("WORKFLOWID")]
        public string WorkFlowId { get; set; }
        /// <summary>
        /// 开票金额
        /// </summary>
        [Column("BILLINGAMOUNT")]
        public decimal? BillingAmount { get; set; }
        /// <summary>
        /// 开票内容
        /// </summary>
        [Column("BILLINGCONTENT")]
        public string BillingContent { get; set; }
        /// <summary>
        /// 开票信息
        /// </summary>
        [Column("BILLINGINFORMATION")]
        public string BillingInformation { get; set; }
        /// <summary>
        /// 开票类型
        /// </summary>
        [Column("BILLINGTYPE")]
        public string BillingType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 开票单位
        /// </summary>
        [Column("BILLINGUNIT")]
        public string BillingUnit { get; set; }
        /// <summary>
        /// 开票项目名称
        /// </summary>
        [Column("BILLINGNAME")]
        public string BillingName { get; set; }
        /// <summary>
        /// BillingStatus
        /// </summary>
        [Column("BILLINGSTATUS")]
        public int? BillingStatus { get; set; }
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
        /// 更新人
        /// </summary>
        [Column("B_REMARK")]
        public string B_Remark { get; set; }
        /// <summary>
        /// 开票附件
        /// </summary>
        [Column("REPORTFILE")]
        public string ReportFile { get; set; }
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
        /// 取消备注
        /// </summary>
        [Column("CANCELTHEREASON")]
        public string CancelTheReason { get; set; }

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

        [Column("CONTRACTID")]
        public string ContractId { get; set; }

        [Column("BILLINGTITLE")]
        public string BillingTitle { get; set; }

        [Column("TAXNO")]
        public string TaxNo { get; set; }

        [Column("BANKNAME")]
        public string BankName { get; set; }

        [Column("BANKACCOUNT")]
        public string BankAccount { get; set; }
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
            this.BillingStatus = 1;
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

