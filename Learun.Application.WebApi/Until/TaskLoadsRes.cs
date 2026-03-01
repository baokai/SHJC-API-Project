using System.Collections.Generic;

namespace Learun.Application.WebApi
{
    /// <summary>
    /// 提交实体数据
    /// </summary>
    public class TaskLoadsRes
    {
        public string departmentName { get; set; }
        public string departmentId { get; set; }
        public int totalCount { get; set; }
        public List<TaskLoadsItemRes> userLoads { get; set; }
    }
    public class TaskLoadsItemRes
    {
        public string userName { get; set; }
        public string userId { get; set; }
        public int totalCount { get; set; }
        public int taskCount1 { get; set; }
        public int taskCount2 { get; set; }
        public int taskCount3 { get; set; }
        public int taskCount4 { get; set; }
    }
}