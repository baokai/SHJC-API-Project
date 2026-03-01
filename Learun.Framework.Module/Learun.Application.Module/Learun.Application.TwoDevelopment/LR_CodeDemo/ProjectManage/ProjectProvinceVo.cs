using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    public class ProjectProvinceVo
    {
        public string ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public string ProjectCount { get; set; }
    }

    public class ProvinceCount
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class ProjectConversionVo
    {
        public string ProjectDay { get; set; }
        public int TotalCount { get; set; }
        public int SucessCount { get; set; }

        public double ConversionRate { get; set; }
    }

    public class ProjectPaymentBackVo
    {
        public string DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public double ContractAmount { get; set; }
        public double CollectionAmount { get; set; }

        public double CollectionRate { get; set; }

    }
}
