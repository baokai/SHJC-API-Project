using Learun.Util;
using System.Collections.Generic;

namespace Learun.Application.WebApi
{
    /// <summary>
    /// 分页请求参数
    /// </summary>
    public class ReqPageParam
    {
        public Pagination pagination { get; set; }
        public string queryJson { get; set; }
    }
    public class QueryTaskLoadParam
    {
        public string queryJson { get; set; }
        /// <summary>
        /// 类别 1 个人 2 部门 3 领导
        /// </summary>
        public int type { get; set; }
        public string userId { get; set; }
        public string departmentId { get; set; }
    }

    public class JavaResModel
    {
        public string msg { get; set; }
        public int code { get; set; }
        public string token { get; set; }
    }
}