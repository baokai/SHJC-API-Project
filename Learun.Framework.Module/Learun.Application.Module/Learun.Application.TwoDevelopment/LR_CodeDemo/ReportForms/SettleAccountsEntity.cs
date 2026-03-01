using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo.ReportForms
{
    public class SettleAccountsEntity: IEquatable<SettleAccountsEntity>
    {
        #region 结算报表实体成员


        //序号
        public int index { get; set; }

        public string PC { get; set; }
        public string CreateTimeyMd { get; set; }
        public string DepartmentIdAmountName { get; set; }
        public string DepartmentIdAmountName1 { get; set; }
        public string ApproverTimeMd { get; set; }
        public string FollowPersonAmount1 { get; set; }
        public string ReceivedFlagName { get; set; }
        public string ProjectSourceName { get; set; }
        public string DepartmentName { get; set; }
        public string ProjectResponsibleName { get; set; }
        public string FollowPersonName { get; set; }
        public string ContractSubjectName { get; set; }
        public string TaskStatusName { get; set; }
        public string ReceiptDateMd { get; set; }
        public string ApproachTimeMd { get; set; }
        /// <summary>
        /// 有效合同额
        /// </summary>
        public decimal? EffectiveAmount { get; set; }
        public decimal? Proportion { get; set; }
        public decimal? ProportionAmount { get; set; }

        public string Tid { get; set; }
        public string cProjectId { get; set; }
        //创建时间
        public DateTime CreateTime { get; set; }

        public DateTime ApproverTime { get; set; }
        public string DepartmentIdCA { get; set; }

        //合同编号
        public string ContractNo { get; set; }

        //项目名称
        public string ProjectName { get; set; }
        public string PreparedPerson { get; set; }
        public string FollowPerson { get; set; }
        public string ProjectResponsible { get; set; }

  

        //委托单位
        public string CustName { get; set; }

        //合同主体
        public string ContractSubject { get; set; }

        //合同状态
        public string ContractStatus { get; set; }

        //项目来源
        public string ProjectSource { get; set; }

        //报备人
        public string P_F_RealName { get; set; }

        //合作伙伴

        //合同金额
        public decimal? ContractAmount { get; set; }

        //开票金额
        public decimal? BillingAmount { get; set; }

        //已到金额
        public decimal? Amount { get; set; }
        public decimal? DepartmentIdAmount { get; set; }
        public decimal? DepartmentIdAmount1 { get; set; }
        public decimal? SubAmount { get; set; }
        public decimal? MainAmount { get; set; }
        public decimal? FollowPersonAmount { get; set; }

        //未收账款
        public decimal? NotReceived { get; set; }

        //营销部门
        public string DepartmentId { get; set; }

        //跟进人部门
        public string FDepartmentId { get; set; }

        //报备人部门
        public string PDepartmentId { get; set; }

        //营销人员
        public string M_F_RealName { get; set; }

        //实施部门
        public string J_F_FullName { get; set; }

        //项目负责人
        public string J_F_RealName { get; set; }

        //报告状态
        public string TaskStatus { get; set; }

        //营销外付
        public decimal? PaymentAmount { get; set; }

        //分包金额
        public decimal?  F_ContractAmount { get; set; }

        //归档
        public string ReceivedFlag { get; set; }

        //备注
        public string Remark { get; set; }


        public DateTime? ReceiptDate { get; set; }

        public bool Equals(SettleAccountsEntity other)
        {
            return this.ContractNo == other.ContractNo && this.ProjectName == other.ProjectName && this.CreateTime == other.CreateTime && this.CustName == other.CustName && this.ContractSubject == other.ContractSubject && this.ContractStatus == other.ContractStatus && this.ProjectSource == other.ProjectSource && this.P_F_RealName == other.P_F_RealName  && this.DepartmentId == other.DepartmentId && this.FDepartmentId == other.FDepartmentId && this.PDepartmentId == other.PDepartmentId && this.M_F_RealName == other.M_F_RealName && this.J_F_FullName == other.J_F_FullName && this.J_F_RealName == other.J_F_RealName && this.TaskStatus == other.TaskStatus  && this.ReceivedFlag == other.ReceivedFlag && this.Remark == other.Remark;
            //return this.ReceiptDate == other.ReceiptDate && this.ContractNo == other.ContractNo && this.ProjectName == other.ProjectName && this.CreateTime == other.CreateTime && this.CustName == other.CustName && this.ContractSubject == other.ContractSubject && this.ContractStatus == other.ContractStatus && this.ProjectSource == other.ProjectSource && this.P_F_RealName == other.P_F_RealName && this.NotReceived == other.NotReceived && this.DepartmentId == other.DepartmentId && this.FDepartmentId == other.FDepartmentId && this.PDepartmentId == other.PDepartmentId && this.M_F_RealName == other.M_F_RealName && this.J_F_FullName == other.J_F_FullName && this.J_F_RealName == other.J_F_RealName && this.TaskStatus == other.TaskStatus  && this.ReceivedFlag == other.ReceivedFlag && this.Remark == other.Remark;
        }
        #endregion

    }
}
