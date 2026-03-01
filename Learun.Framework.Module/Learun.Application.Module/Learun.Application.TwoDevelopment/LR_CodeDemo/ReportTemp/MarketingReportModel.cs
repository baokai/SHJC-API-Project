using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    public class MarketingReportModel
    {
        public string id { get; set; }
        /// <summary>
        /// 合同日期
        /// </summary>
        public string ContractTime { get; set; }
        /// <summary>
        /// 项目编号
        /// </summary>
        public string ProjectCode { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目客户
        /// </summary>
        public string CustName { get; set; }
        /// <summary>
        /// 营销人员
        /// </summary>
        public string FollowPerson { get; set; }
        /// <summary>
        /// 项目来源
        /// </summary>
        public string ProjectSource { get; set; }
        /// <summary>
        /// 合同主体
        /// </summary>
        public string ContractSubject { get; set; }
        /// <summary>
        /// 合同状态
        /// </summary>
        public int ContractStatus { get; set; }
        /// <summary>
        /// 开票情况
        /// </summary>
        public int BillingStatus { get; set; }
        /// <summary>
        /// 合同额
        /// </summary>
        public decimal ContractAmount { get; set; }
        /// <summary>
        /// 已到金额
        /// </summary>
        public decimal PayCollectionAmount { get; set; }
        /// <summary>
        /// 未到金额
        /// </summary>
        public decimal NoPayAmount { get; set; }
        /// <summary>
        /// 到款日期
        /// </summary>
        public string PayCollectionTime { get; set; }
        /// <summary>
        /// 实施部门
        /// </summary>
        public string CarryDept { get; set; }
        /// <summary>
        /// TestDateTime
        /// </summary>
        public string TestDateTime { get; set; }
        /// <summary>
        /// 报告主体
        /// </summary>
        public string ReportSubject { get; set; }
        /// <summary>
        /// 报告状态
        /// </summary>
        public int TaskStatus { get; set; }
        /// <summary>
        /// 项目负责人
        /// </summary>
        public string ProjectResponsible { get; set; }
    }
}
