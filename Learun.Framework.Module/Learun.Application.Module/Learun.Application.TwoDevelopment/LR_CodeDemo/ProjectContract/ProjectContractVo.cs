using Learun.Application.Base.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
   
    public class ProjectContractVo:IEquatable<ProjectContractVo>

    {
        /// <summary>
        /// 主部门
        /// </summary>
       
        public string MainDepartmentId { get; set; }
        public string MainDepartmentName { get; set; }
        public string SubDepartmentName { get; set; }
        public string CreateTimeyMd { get; set; }
        public string ContractStatusName { get; set; }
        public string ReceivedFlagName { get; set; }

        public string TaskId { get; set; }
        /// <summary>
        /// 主部门金额
        /// </summary>

        public decimal? MainAmount { get; set; }
       

        /// <summary>
        /// 次部门
        /// </summary>
       
        public string SubDepartmentId { get; set; }
        /// <summary>
        /// 次部门金额
        /// </summary>
    
        public decimal? SubAmount { get; set; }
        public int index { get; set; }
        /// <summary>
        /// 合同编号
        /// </summary>
        public string ContractNo { get; set; }
        public decimal? Proportion { get; set; }
        public string ContractNoNane { get; set; }
        /// <summary>
        /// 归档编号
        /// </summary>
        public string ReceivedFlagNo { get; set; }
        /// <summary>
        /// 归档类型
        /// </summary>
        public string ReceiptType { get; set; }
        /// <summary>
        /// 销售部门名字
        /// </summary>
        public string DepartmentName { get; set; }
        ///  /// <summary>
        /// 销售人员名字
        /// </summary>
        public string FollowPersonName { get; set; }
        /// <summary>
        /// 项目来源名字
        /// </summary>
        public string ProjectSourceName { get; set; }
        /// <summary>
        /// 合同主体名字
        /// </summary>
        public string ContractSubjectName { get; set; }
        /// /// <summary>
        /// 合同分类名字
        /// </summary>
        public string ProjectCount { get; set; }
        /// <summary>
        /// 是否主合同
        /// </summary>
        public string MasterContract { get; set; }
        public string MainContract { get; set; }
        public string MainContractName { get; set; }
        public string ContractSum { get; set; }
        public string ProjectResponsibleDept { get; set; }
        public string PDepartmentId { get; set; }
        public string FDepartmentId { get; set; }
        public string ProjectName { get; set; }
        public string CustName { get; set; }
        public string ProjectSource { get; set; }
        public string FollowPerson { get; set; }
        public string PreparedPerson { get; set; }
        public string  Pid { get; set; }

        public string id { get; set; }
        /// <summary>
        /// WorkFlowId
        /// </summary>
        public string WorkFlowId { get; set; }
        public string DepartmentId { get; set; }
        /// <summary>
        /// ProjectId
        /// </summary>
        public string ProjectId { get; set; }
        
        /// <summary>
        /// 合同主体
        /// </summary>
        public string ContractSubject { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal? ContractAmount { get; set; }
       
        /// <summary>
        /// 合同金额合计
        /// </summary>
        public decimal? ContractAmountSUN { get; set; }
        /// <summary>
        /// ContractType
        /// </summary>
        public string ContractType { get; set; }
        public string ContractTypeName { get; set; }
        /// <summary>
        /// ContractStatus
        /// </summary>
        public string ContractStatus { get; set; }
        /// <summary>
        /// 合同文件
        /// </summary>
        public string ContractFile { get; set; }
        /// <summary>
        /// 审批人
        /// </summary>
        public string Approver { get; set; }
        ///<summary>
        /// 审批时间
        /// </summary>
        public string ApproverTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
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
        public string ReceivedFlag { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        public decimal? PaymentAmount { get; set; }
        /// <summary>
        /// 批量付款金额
        /// </summary>
        public decimal? PaymentAmountList { get; set; }
        /// <summary>
        /// 比例
        /// </summary>
        public decimal? FollowPersonAmount { get; set; }
        public string ContractRemark { get; set; }
        public string Remark { get; set; }

        public decimal? EffectiveAmount { get; set; }
        public decimal? EffectiveAmountShow { get; set; }


        /// 合同附件列表
        /// </summary>
        public IEnumerable<AnnexesFileEntity> annexesFileEntities { get; set; }

        bool IEquatable<ProjectContractVo>.Equals(ProjectContractVo other)
        {
            return this.DepartmentName == other.DepartmentName && this.FollowPersonName == other.FollowPersonName && this.ProjectSourceName == other.ProjectSourceName && this.ContractSubjectName == other.ContractSubjectName  && this.PDepartmentId == other.PDepartmentId && this.FDepartmentId == other.FDepartmentId && this.ProjectName == other.ProjectName  && this.CustName == other.CustName && this.ProjectSource == other.ProjectSource && this.FollowPerson == other.FollowPerson  && this.PreparedPerson == other.PreparedPerson && this.Pid == other.Pid  && this.id == other.id  && this.WorkFlowId == other.WorkFlowId && this.DepartmentId == other.DepartmentId  && this.ProjectId == other.ProjectId  && this.ContractNo == other.ContractNo && this.ContractSubject == other.ContractSubject && this.ContractAmount == other.ContractAmount  && this.ContractType == other.ContractType && this.ContractTypeName == other.ContractTypeName && this.ContractStatus == other.ContractStatus && this.ContractFile == other.ContractFile && this.Approver == other.Approver && this.CreateTime == other.CreateTime && this.CreateUser == other.CreateUser && this.UpdateTime == other.UpdateTime && this.UpdateUser == other.UpdateUser && this.ReceivedFlag == other.ReceivedFlag && this.ContractRemark == other.ContractRemark && this.Remark == other.Remark && this.annexesFileEntities == other.annexesFileEntities;
        }
    }
}
