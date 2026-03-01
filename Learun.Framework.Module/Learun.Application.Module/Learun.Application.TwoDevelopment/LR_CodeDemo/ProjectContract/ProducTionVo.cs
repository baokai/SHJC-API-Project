using Learun.Application.Base.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{

    public class ProducTionVo
    {
        //部门
        public string DepartmentId { get; set; }
        //月份
        public string Month { get; set; }
        //年
        public string Years { get; set; }
        //数据
        public string Quantity { get; set; }
        //金额
        public string Amount { get; set; }
        //开票金额
        public string BillingAmount { get; set; }

    }
}
