using Learun.Application.Organization;
using Learun.Application.TwoDevelopment.LR_CodeDemo;
using Learun.Util;
using Nancy;

namespace Learun.Application.WebApi.Modules
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 上海力软信息技术有限公司
    /// 创建人：力软-框架开发组
    /// 日 期：2018.01.04
    /// 描 述：部门管理
    /// </summary>
    public class DepartmentApi: BaseApi
    {
        public DepartmentApi()
          : base("/learun/adms/department")
        {
            Get["/map"] = GetMap;// 获取组织机构数据映射表
            Get["/getContractAmountLeaderboard"] = GetContractAmountLeaderboard;// 
            Get["/getTaskFinishedRateLeaderboard"] = GetTaskFinishedRateLeaderboard;// 
            Get["/getCollectionAmountLeaderboard"] = GetCollectionAmountLeaderboard;// 
            Post["/syncDepartment"] = SyncDepartment;// 同步部门数据
        }
        private DepartmentIBLL departmentIBLL = new DepartmentBLL();
        private GantProjectIBLL gantProjectIBLL = new GantProjectBLL();
        /// <summary>
        /// 获取部门映射表
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetMap(dynamic _)
        {
            string ver = this.GetReqData();// 获取模板请求数据
            var data = departmentIBLL.GetModelMap();
            string md5 = Md5Helper.Encrypt(data.ToJson(), 32);
            if (md5 == ver)
            {
                return Success("no update");
            }
            else
            {
                var jsondata = new
                {
                    data = data,
                    ver = md5
                };
                return Success(jsondata);
            }
        }
        public Response SyncDepartment(dynamic _)
        {
            string ver = this.GetReqData();// 获取模板请求数据
            var data = departmentIBLL.GetModelMap();
            string md5 = Md5Helper.Encrypt(data.ToJson(), 32);
            if (md5 == ver)
            {
                return Success("no update");
            }
            else
            {
                var jsondata = new
                {
                    data = data,
                    ver = md5
                };
                return Success(jsondata);
            }
        }
        public Response GetContractAmountLeaderboard(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = gantProjectIBLL.GetContractAmountLeaderboard(parameter.queryJson);
            var jsonData = new
            {
                rows = data
            };
            return Success(jsonData);
        }
        public Response GetTaskFinishedRateLeaderboard(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = gantProjectIBLL.GetTaskFinishedRateLeaderboard(parameter.queryJson);
            var jsonData = new
            {
                rows = data
            };
            return Success(jsonData);
        }
        public Response GetCollectionAmountLeaderboard(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = gantProjectIBLL.GetCollectionAmountLeaderboard(parameter.queryJson);
            var jsonData = new
            {
                rows = data
            };
            return Success(jsonData);
        }
    }
}