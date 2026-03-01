using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// 创 建：超级管理员
    /// 日 期：2022-03-10 23:22
    /// 描 述：项目合同申请
    /// </summary>
    public class ProjectContractEntity 
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
        /// 项目来源
        /// </summary>
        [Column("PROJECTSOURCE")]
        public string ProjectSource { get; set; }
        /// <summary>
        /// ProjectId
        /// </summary>
        [Column("PROJECTID")]
        public string ProjectId { get; set; }
        [Column("DEPARTMENTID")]
        public string DepartmentId { get; set; }
        /// <summary>
        /// 合同编号
        /// </summary>
        [Column("CONTRACTNO")]
        public string ContractNo { get; set; }
        /// <summary>
        /// 合同主体
        /// </summary>
        [Column("CONTRACTSUBJECT")]
        public string ContractSubject { get; set; }
        /// <summary>
        /// 主部门
        /// </summary>
        [Column("MAINDEPARTMENTID")]
        public string MainDepartmentId { get; set; }
        /// <summary>
        /// 主部门金额
        /// </summary>
        [Column("MAINAMOUNT")]
        public decimal? MainAmount { get; set; } 
        /// <summary>
        /// 资金成本金额
        /// </summary>
        [Column("CAPITALAMOUNT")]
        public decimal? CapitalAmount { get; set; }
        /// <summary>
        /// 次部门
        /// </summary>
        [Column("SUBDEPARTMENTID")]
        public string SubDepartmentId { get; set; }
        /// <summary>
        /// 次部门金额
        /// </summary>
        [Column("SUBAMOUNT")]
        public decimal? SubAmount { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        [Column("CONTRACTAMOUNT")]
        public decimal? ContractAmount { get; set; }
        /// <summary>
        /// 比例(合作伙伴结算台账输入的比例)
        /// </summary>
        [Column("PROPORTION")]
        public decimal? Proportion { get; set; }
        /// <summary>
        /// 批量付款添加金额
        /// </summary>
        [Column("PAYMENTAMOUNTLIST")]
        public decimal? PaymentAmountList { get; set; }
        /// <summary>
        /// ContractType
        /// </summary>
        [Column("CONTRACTTYPE")]
        public int? ContractType { get; set; }
        /// <summary>
        /// ContractStatus
        /// </summary>
        [Column("CONTRACTSTATUS")]
        public int? ContractStatus { get; set; }
        [Column("CONTRACTFILE")]
        public string ContractFile { get; set; }
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
        [Column("RECEIVEDFLAG")]
        public int? ReceivedFlag { get; set; }
        [Column("RECEIPTTYPE")]
        public string ReceiptType { get; set; }
        [Column("REMARK")]
        public string Remark { get; set; }
        [Column("CONTRACTREMARK")]
        public string ContractRemark { get; set; }
        /// <summary>
        /// 审批人
        /// </summary>
        [Column("APPROVER")]
        public string Approver { get; set; }
        /// <summary>
        /// 审批时间
        /// </summary>
        [Column("APPROVERTIME")]
        public DateTime? ApproverTime { get; set; }


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

        [Column("RECEIVEDFLAGNO")]
        public string ReceivedFlagNo { get; set; }
        /// <summary>
        ///// 主合同
        /// </summary>
        [Column("MAINCONTRACT")]
        public int? MainContract { get; set; }
        /// <summary>
        /// 取消理由
        /// </summary>
        [Column("CANCELTHEREASON")]
        public string CancelTheReason { get; set; } 
        /// <summary>
        /// 有效合同额
        /// </summary>
        [Column("EFFECTIVEAMOUNT")]
        public decimal? EffectiveAmount { get; set; }
        /// <summary>
        /// 有效合同额
        /// </summary>
        [Column("EFFECTIVEAMOUNTSHOW")]
        public decimal? EffectiveAmountShow { get; set; }
        
        ///// <summary>
        ///// 人员绩效
        ///// </summary>
        //[Column("FOLLOWPERSONAMOUNT")]
        //public decimal? FollowPersonAmount { get; set; }
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
            this.ContractStatus = 1;
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
        /// <summary>
        /// 编辑调用(合作伙伴结算台账添加比例)
        /// </summary>
        /// <param name="keyValue"></param>
        public void ModifyReportForms(string keyValue)
        {
            this.UpdateTime = DateTime.Now;
            this.UpdateUser = LoginUserInfo.Get().userId;
            //this.ProjectId = keyValue;
            this.id = keyValue;
        }

        #endregion
        #region 扩展字段
      
        #endregion
    }
}

