using Learun.Application.Base.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
   public class ProjectPaymentListVo
    {
        public int index { get; set; }
        /// <summary>
        /// 付款类型
        /// </summary>
        public string PayTypeName { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 付款方式
        /// </summary>
        public string PaymentMethodName { get; set; }
        /// <summary>
        /// 我司支付
        /// </summary>
        public string PaymentHeaderName { get; set; }
        public string tid { get; set; }
        
        public string ProjectName { get; set; }
        public string Id { get; set; }
        public string tId { get; set; }
        public string CustName { get; set; }
        public string ProjectSource { get; set; }
        public string FollowPerson { get; set; }
        public string PreparedPerson { get; set; }

        public string DepartmentId { get; set; }

        public string PDepartmentId { get; set; }
        public string FDepartmentId { get; set; }
        public string Pid { get; set; }
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

        public string PayType { get; set; }

        /// <summary>
        /// 支付公司
        /// </summary>
        public string Payee { get; set; }
        /// <summary>
        /// 银行
        /// </summary>
        public string PayeeBank { get; set; }

        public string BankAccount { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal? PaymentAmount { get; set; }
        public decimal? PaymentAmountsum { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string PaymentMethod { get; set; }
        /// <summary>
        /// 支付抬头
        /// </summary>
        public string PaymentHeader { get; set; }
        /// <summary>
        /// 支付原因
        /// </summary>
        public string PaymentReason { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public string PaymentStatus { get; set; }

        public string PaymentFile { get; set; }
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
        public List<ProjectContractEntity> contractEntities { get; set; }
        /// <summary>
        /// 付款附件列表
        /// </summary>
        public IEnumerable<AnnexesFileEntity> annexesFileEntities { get; set; }
    }
}
