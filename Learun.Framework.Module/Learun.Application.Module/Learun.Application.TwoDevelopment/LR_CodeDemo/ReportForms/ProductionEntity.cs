using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo.ReportForms
{
    public class ProductionEntity : IEquatable<ProductionEntity>
    {
        #region 生产报表实体成员

        //序号
        public int index { get; set; }

        //创建时间
        public DateTime? CreateTime { get; set; }
        public string CreateTimeyMd { get; set; }
        public string DepartmentIdAmountName1 { get; set; }
        public string ContractSubjectName { get; set; }
        public DateTime? ApproverTime { get; set; }
        public string ApproverTimeMd { get; set; }
        public string ReceiptDateMd { get; set; }
        public string ApproachTimeMd { get; set; }
        public string ProjectResponsibleName { get; set; }
        public string DepartmentName { get; set; }
        public string ProjectSourceName { get; set; }
        public string ReportSubjectName { get; set; }
        public string TaskStatusName { get; set; }
        public string ReceivedFlagName { get; set; }
        public string BillingStatusName { get; set; }
        public string MainDepartmentName { get; set; }
        public string MainDepartmentId { get; set; }
        public string SubDepartmentId { get; set; }
        public string SubDepartmentName { get; set; }
        public string BillingStatus { get; set; }
        public string FollowPersonName { get; set; }
        public DateTime? ReceiptDate { get; set; }
        //合同编码
        public string ContractNo { get; set; }
        //项目名称
        public string ProjectName { get; set; }
        public decimal? PaymentAmount { get; set; }
        public decimal? FollowPersonAmount { get; set; }
        public decimal? DepartmentIdAmount { get; set; }
        public decimal? PayCollectionAmount { get; set; }
        public decimal? MainAmount { get; set; }
        public decimal? SubAmount { get; set; }
        //委托单位
        public string CustName { get; set; }
        //合同主体
        public string ContractSubject { get; set; }
        //报告主体
        public string ReportSubject { get; set; }
        //营销人员
        public string F_RealName { get; set; }
        public string FollowPerson { get; set; }
        //项目负责人
        public string ProjectResponsible { get; set; }

        //创建营销部门
        public string XDepartmentId { get; set; }
        //营销部门
        public string DepartmentId { get; set; }

        //跟进人部门
        public string FDepartmentId { get; set; }

        //报备人部门
        public string PDepartmentId { get; set; }

        //合同状态
        public string ContractStatus { get; set; }


        //合同金额
        public decimal? ContractAmount { get; set; }

        //实施部门
        public string J_F_FullName { get; set; }

        //项目负责人
        public string P_F_RealName { get; set; }

        //检测时间
        public DateTime? ApproachTime { get; set; }
        public DateTime? FlowFinishedTime { get; set; }
        public string FlowFinishedTimeMd { get; set; }
        //报告编号
        public string id { get; set; }

        //报告状态
        public string TaskStatus { get; set; }


        //归档情况
        public string ReceivedFlag { get; set; }

        //是否分包
        public string ContractType { get; set; }

        //备注
        public string Remark { get; set; }

        //项目来源
        public string ProjectSource { get; set; }
        /// <summary>
        /// 回款金额
        /// </summary>
        public string Amount { get; set; }
        public decimal? EffectiveAmount { get; set; }

        bool IEquatable<ProductionEntity>.Equals(ProductionEntity other)
        {
            return  this.ContractNo == other.ContractNo  && this.ProjectName == other.ProjectName && this.CreateTime == other.CreateTime && this.CustName == other.CustName  && this.ContractSubject == other.ContractSubject && this.ReportSubject == other.ReportSubject  && this.F_RealName == other.F_RealName  && this.DepartmentId == other.DepartmentId && this.FDepartmentId == other.FDepartmentId  && this.PDepartmentId == other.PDepartmentId  && this.ContractStatus == other.ContractStatus && this.ContractAmount == other.ContractAmount && this.J_F_FullName == other.J_F_FullName && this.P_F_RealName == other.P_F_RealName && this.ApproachTime == other.ApproachTime && this.id == other.id  && this.TaskStatus == other.TaskStatus  && this.ReceivedFlag == other.ReceivedFlag && this.ContractType == other.ContractType && this.Remark == other.Remark  && this.ProjectSource == other.ProjectSource;
        }


        #endregion
    }
}
