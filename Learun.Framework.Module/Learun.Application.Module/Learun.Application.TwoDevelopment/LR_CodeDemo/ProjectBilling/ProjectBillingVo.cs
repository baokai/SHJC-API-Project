using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
  public  class ProjectBillingVo
    {
        public int index { get; set; }
       
        public string ContractNo { get; set; }
        public string ContractId { get; set; }
        public string CreateTimeyMd { get; set; }
        public string BillingContentName { get; set; }
        //开票状态
        public string BillingStatusName { get; set; }
        public string DepartmentName { get; set; }
        //public string DepartmentName { get; set; }
       
       
        public string BillingTypeName { get; set; }
        
        public string ProjectSourceName { get; set; }


        public string ProjectSource { get; set; }
        public string FollowPerson { get; set; }
        public string PreparedPerson { get; set; }
        public string Pid { get; set; }

        #region 实体成员
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        public DateTime? FinishTime { get; set; }
        
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        /// <summary>
        /// 开票金额
        /// </summary>
        public decimal? BillingAmount { get; set; }

        /// <summary>
        /// 开票内容
        /// </summary>
        public string BillingContent { get; set; }
        /// <summary>
        /// 开票信息
        /// </summary>
        public string BillingInformation { get; set; }
        /// <summary>
        /// 开票类型
        /// </summary>
        public string BillingType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public string BillingUnitName { get; set; }

        /// <summary>
        /// 开票单位
        /// </summary>
        public string BillingUnit { get; set; }
        public string BillingStatus { get; set; }
        /// <summary>
        /// BillingStatus
        /// </summary>

        public string CustName { get; set; }
        /// <summary>
        /// WorkFlowId
        /// </summary>
        public string WorkFlowId { get; set; }





        /// <summary>
        /// 提审人
        /// </summary>
        public string ContractSubmitter { get; set; }

        /// <summary>
        /// 提审人部门Code
        /// </summary>
        public string ContractSubmitterDeptCode { get; set; }

        /// <summary>
        /// 开票项目名称
        /// </summary>
        public string BillingName { get; set; }
        
        
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdateUser { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string B_Remark { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public string ReportFile { get; set; }
        /// <summary>
        /// 审批人
        /// </summary>  
        public string Approver { get; set; }
        /// <summary>
        /// 审批时间
        /// </summary>
        public string ApproverTime { get; set; }


        public string DepartmentId { get; set; }

        public string PDepartmentId { get; set; }
        public string FDepartmentId { get; set; }
        /// <summary>
        /// 报备id
        /// </summary>
        public string pid { get; set; }

        public string BillingTitle { get; set; }

        public string TaxNo { get; set; }

        public string BankName { get; set; }

        public string BankAccount { get; set; }
        /*public bool Equals(ProjectBillingVo other)
        {
            return this.Id == other.Id && this.CreateTime == other.CreateTime && this.ProjectId == other.ProjectId && this.BillingType == other.BillingType && this.BillingUnit == other.BillingUnit && this.CreateUser == other.CreateUser && this.DepartmentId == other.DepartmentId;
        }*/
        #endregion
    }
}
