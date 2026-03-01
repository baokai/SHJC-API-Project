using Learun.Application.Base.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    public class PaymentVo
    {
        public int index { get; set; }


        public string PaymentStatusName { get; set; }
        public string CreateTimeyMd { get; set; }
        public string CreateUserName { get; set; }
        public string BillingStatusName { get; set; }
        public string DepartmentName { get; set; }
        public string ContractSubmitterName { get; set; }
        public string PaymentSubmitterName { get; set; }
        public string ContractSubmitter { get; set; }
        public string PaymentSubmitter { get; set; }

        public string Id { get; set; }

        public string PayTypeName { get; set; }
        public string PaymentHeader { get; set; }
        public string PaymentHeaderName { get; set; }



        public string WorkFlowId { get; set; }

       
        public string PayType { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
      
        public string PaymentMethod { get; set; }
        public string PaymentMethodName { get; set; }

        /// <summary>
        /// 支付原因
        /// </summary>

        public string PaymentReason { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
      
        public decimal? PaymentAmount { get; set; }

      
        public string BankAccount { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
      
        public string PaymentStatus { get; set; }

        /// <summary>
        /// 银行
        /// </summary>
     
        public string PayeeBank { get; set; }

   
        public string PaymentFile { get; set; }
        public string DepartmentId { get; set; }


        public DateTime? CreateTime { get; set; }

       
        public string CreateUser { get; set; }

        
        public DateTime? UpdateTime { get; set; }

       
        public string UpdateUser { get; set; }

        /// <summary>
        /// 收款单位名称
        /// </summary>
       
        public string Payee { get; set; }
        /// <summary>
        /// 上传附件
        /// </summary>

        public IEnumerable<AnnexesFileEntity> annexesFileEntities { get; set; }
    }
}
