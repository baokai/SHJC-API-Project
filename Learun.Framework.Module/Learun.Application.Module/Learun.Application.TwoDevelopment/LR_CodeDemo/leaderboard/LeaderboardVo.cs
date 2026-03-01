using Learun.Application.Base.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{

    public class LeaderboardVo
    {
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? finishedCount { get; set; }
        public decimal? finishedCountRate { get; set; }
        public decimal? TotalCount { get; set; }
    }
}
