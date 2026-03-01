using Learun.Application.Base.SystemModule;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{


    public class TaskScheduleVo
    {
        public string id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectId { get; set; }
        public string ProjectResponsible { get; set; }
        public List<string> ProjectResponsibleName { get; set; }
        public string Inspector { get; set; }
        public List<string> InspectorName { get; set; }
        public DateTime? ApproachTime { get; set; }
        public DateTime? PlanTime { get; set; }
        public DateTime? PlanFinishTime { get; set; }
        public DateTime? PlanApproachTime { get; set; }
        public DateTime? ActualDepartureTime { get; set; }
        public string YJ { get; set; }
        public string DepartmentId { get; set; }
        public string WorkFlowId { get; set; }
        public string ProjectTaskNo { get; set; }
        public int TaskStatus { get; set; }
        public int PlanDateDiff { get; set; }
        public int ApproachDateDiff { get; set; }
    }
    public class ScheduleParam
    {
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string inspector { get; set; }
    }
    public class TaskScheduleRes
    {
        public string userName { get; set; }
        public string userId { get; set; }

        public List<TaskScheduleVo> taskList { get; set; }
    }
}
