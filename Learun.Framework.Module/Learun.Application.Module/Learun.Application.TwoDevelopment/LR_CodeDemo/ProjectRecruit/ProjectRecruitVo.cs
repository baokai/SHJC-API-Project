using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    public class ProjectRecruitVo
    {
        public string Pid { get; set; }
        public string ProjectName { get; set; }
        public string CreateTimeyMd { get; set; }
        public string CustName { get; set; }
        public string ProjectSource { get; set; }
        public string FollowPerson { get; set; }
        public string PreparedPerson { get; set; }
        public string ContractNo { get; set; }
        public string DepartmentId { get; set; }

        public string PDepartmentId { get; set; }
        public string FDepartmentId { get; set; }
        public string WorkingTime { get; set; }
        public string Remark { get; set; }
        public string RecruitStatusName { get; set; }
        public string PaymentMethodName { get; set; }

        #region 实体成员
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// WorkFlowId
        /// </summary>
        public string WorkFlowId { get; set; }
        /// <summary>
        /// ProjectId
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public string ApplyPerson { get; set; }
        public string ApplyPersonName { get; set; }
        /// <summary>
        /// 用工资源
        /// </summary>
        public string WageSource { get; set; }
        /// <summary>
        /// JobType
        /// </summary>
        public string JobType { get; set; }
        /// <summary>
        /// PersonQty
        /// </summary>
        public int? PersonQty { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        public decimal? Price { get; set; }
        /// <summary>
        /// Amount
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// PayeeUnit
        /// </summary>
        public string PayeeUnit { get; set; }
        /// <summary>
        /// PayeeBank
        /// </summary>
        public string PayeeBank { get; set; }
        /// <summary>
        /// PayeeAccount
        /// </summary>
        public string PayeeAccount { get; set; }
        /// <summary>
        /// PaymentMethod
        /// </summary>
        public string PaymentMethod { get; set; }
        /// <summary>
        /// RecruitStatus
        /// </summary>
        public string RecruitStatus { get; set; }
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
        /// <summary>
        /// 审批人
        /// </summary>

        public string Approver { get; set; }
        /// <summary>
        /// 审批时间
        /// </summary>

        public string ApproverTime { get; set; }
        #endregion
    }
}
