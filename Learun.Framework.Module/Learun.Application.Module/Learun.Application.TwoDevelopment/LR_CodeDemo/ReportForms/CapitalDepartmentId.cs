using Learun.Application.Base.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{

    public class CapitalDepartmentId : IEquatable<CapitalDepartmentId>

    {

   
        /// <summary>
        /// 月份
        /// </summary>
        public string yefen { get; set; }
        public string pid { get; set; }
        public string yefenList { get; set; }
        public string datayyyyMM { get; set; }
        public DateTime? ApproverTime { get; set; }
        public string DepartmentIdName { get; set; }
        /// <summary>
        /// 主部门金额
        /// </summary>

        public decimal? MainAmount { get; set; }
        public decimal? PaymentAmount { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AmountList { get; set; }
        public decimal? sumList { get; set; }
        public decimal? ContractAmountSUNList { get; set; }

  
        public string ProjectSource { get; set; }
        /// <summary>
        /// 次部门金额
        /// </summary>

        public decimal? SubAmount { get; set; }
        public int index { get; set; }
        public int index1 { get; set; }


    
        /// <summary>
        /// 金额
        /// </summary>
        public decimal? ContractAmount { get; set; }
        public decimal? ContractAmountList { get; set; }
        public decimal? EffectiveAmountList { get; set; }
        public decimal? AmountList1 { get; set; }

        public decimal? ContractAmountSUN { get; set; }
        public decimal? EffectiveAmount { get; set; }
        //部门
        public string DepartmentId { get; set; }
        //年
        public string YYYYTime { get; set; }

        bool IEquatable<CapitalDepartmentId>.Equals(CapitalDepartmentId other)
        {
            return this.yefen == other.yefen && this.ContractAmountSUNList == other.ContractAmountSUNList && this.DepartmentIdName == other.DepartmentIdName && this.EffectiveAmountList == other.EffectiveAmountList && this.ContractAmountList == other.ContractAmountList && this.ContractAmountSUN == other.ContractAmountSUN && this.ContractAmountSUNList == other.ContractAmountSUNList && this.sumList == other.sumList;
        }
    }
}
