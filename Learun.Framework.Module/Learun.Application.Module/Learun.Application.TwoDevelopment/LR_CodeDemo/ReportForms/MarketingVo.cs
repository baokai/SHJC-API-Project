using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo.ReportForms
{
    public class MarketingVo : IEquatable<MarketingEntity>
    {
        #region 实体成员
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        //序号
        public int index { get; set; }
        //创建时间

        public string Tid { get; set; }
        public string PC { get; set; }
        public string CreateTime { get; set; }

        public string ApproverTime { get; set; }

        //合同编码
        public string ContractNo { get; set; }

        //项目名称
        public string ProjectName { get; set; }

        

        //委托单位
        public string CustName { get; set; }

        //合同主体
        public string ContractSubject { get; set; }


        //营销部门
        public string DepartmentId { get; set; }

        //营销人员
        public string F_RealName { get; set; }

        //项目来源
        public string ProjectSource { get; set; }

        //归档情况
        public string ReceivedFlag { get; set; }

        //开票情况
        public string BillingStatus { get; set; }

        //合同金额
        public decimal? ContractAmount { get; set; }

        //已到金额
        public decimal? Amount { get; set; }

        //未收账款
        public decimal? NotReceived { get; set; }


        //到款日期
        public DateTime? ReceiptDate { get; set; }

        //实施部门
        public string J_F_FullName { get; set; }

        //检测时间
        public DateTime? ApproachTime { get; set; }

        //报告主体
        public string ReportSubject { get; set; }

        //报告状态
        public string TaskStatus { get; set; }

        //项目负责人
        public string P_F_RealName { get; set; }


        //合同状态
        public string ContractStatus { get; set; }

        

        //是否分包
        public string ContractType { get; set; }


        //跟进人部门
        public string FDepartmentId { get; set; }

        //报备人部门
        public string PDepartmentId { get; set; }

        bool IEquatable<MarketingEntity>.Equals(MarketingEntity other)
        {
            return  this.ContractType == other.ContractType && this.ReceivedFlag == other.ReceivedFlag && this.P_F_RealName == other.P_F_RealName   && this.J_F_FullName == other.J_F_FullName && this.ReceiptDate == other.ReceiptDate && this.BillingStatus == other.BillingStatus && this.ContractStatus == other.ContractStatus && this.ProjectSource == other.ProjectSource && this.ContractNo == other.ContractNo && this.ProjectName==other.ProjectName && this.CreateTime == other.CreateTime.ToString() && this.CustName == other.CustName && this.ContractSubject == other.ContractSubject && this.DepartmentId == other.DepartmentId && this.FDepartmentId == other.FDepartmentId && this.PDepartmentId == other.PDepartmentId && this.F_RealName == other.F_RealName;
            //return this.ContractType == other.ContractType && this.ReceivedFlag == other.ReceivedFlag && this.P_F_RealName == other.P_F_RealName && this.TaskStatus == other.TaskStatus && this.ReportSubject == other.ReportSubject && this.ApproachTime == other.ApproachTime && this.J_F_FullName == other.J_F_FullName && this.ReceiptDate == other.ReceiptDate && this.NotReceived == other.NotReceived && this.Amount == other.Amount && this.ContractAmount == other.ContractAmount && this.BillingStatus == other.BillingStatus && this.ContractStatus == other.ContractStatus && this.ProjectSource == other.ProjectSource && this.ContractNo == other.ContractNo && this.ProjectName==other.ProjectName && this.CreateTime == other.CreateTime && this.CustName == other.CustName && this.ContractSubject == other.ContractSubject && this.DepartmentId == other.DepartmentId && this.FDepartmentId == other.FDepartmentId && this.PDepartmentId == other.PDepartmentId && this.F_RealName == other.F_RealName;
        }
        #endregion

    }
}
