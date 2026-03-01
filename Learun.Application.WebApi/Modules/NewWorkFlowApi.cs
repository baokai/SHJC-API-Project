using Learun.Application.Form;
using Learun.Application.Organization;
using Learun.Application.TwoDevelopment.LR_CodeDemo;
using Learun.Application.WorkFlow;
using Learun.Util;
using Nancy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Learun.Application.WebApi.Modules
{
    /// <summary>
    /// 日 期：2019.01.10
    /// 描 述：新的工作流接口
    /// </summary>
    public class NewWorkFlowApi : BaseApi
    {
        /// <summary>
        /// 注册接口
        /// </summary>
        public NewWorkFlowApi()
            : base("/learun/adms/newwf")
        {
            Get["/schemelist"] = GetSchemePageList;
            Get["/scheme"] = GetSchemeByCode;
            //我的流程
            Get["/mylist"] = GetMyProcess;
            //待办
            Get["/mytask"] = GetMyTaskList;
            //已办
            Get["/mytaskmaked"] = GetMyMakeTaskList;

            Get["/auditer"] = GetNextAuditors;
            Get["/processinfo"] = GetProcessDetails;

            Post["/create"] = Create;
            Post["/againcreate"] = AgainCreateFlow;
            Post["/childcreate"] = CreateChildFlow;

            Post["/draft"] = SaveDraft;
            Post["/deldraft"] = DeleteDraft;

            Get["/delProcess"] = DeleteProcess;

            Get["/resetProcess"] = ResetProcess;

            Post["/audit"] = AuditFlow;
            Post["/sign"] = SignFlow;
            Post["/signaudit"] = SignAuditFlow;

            Post["/urge"] = UrgeFlow;
            Post["/revoke"] = RevokeFlow;

            Post["/refer"] = ReferFlow;
            //获取流程任务信息
            Get["/getTaskPageList"] = GetTaskPageList;
        }
        private NWFSchemeIBLL nWFSchemeIBLL = new NWFSchemeBLL();
        private NWFProcessIBLL nWFProcessIBLL = new NWFProcessBLL();
        private ProjectPaymentIBLL projectPaymentIBLL = new ProjectPaymentBLL();
        private PaymentIBLL paymentIBLL = new PaymentBLL();
        private ProjectTaskIBLL projectTaskIBLL = new ProjectTaskBLL();
        private ProjectContractIBLL projectContractIBLL = new ProjectContractBLL();
        private UserIBLL userIBLL = new UserBLL();
        private FormSchemeIBLL formSchemeIBLL = new FormSchemeBLL();
        private NWFTaskIBLL nWFTaskIBLL = new NWFTaskBLL();

        private GantProjectIBLL gantProjectIBLL = new GantProjectBLL();
        private ProjectManageIBLL projectIBLL = new ProjectManageBLL();
        private VersionIBLL versionBLL = new VersionBLL();
        private ProjectRecruitIBLL projectRecruitIBLL = new ProjectRecruitBLL();
        private ProjectBillingIBLL projectBillingIBLL = new ProjectBillingBLL();



        /// <summary>
        /// 获取我的流程实例信息
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private Response GetMyProcess(dynamic _)
        {
            QueryModel parameter = this.GetReqData<QueryModel>();

            IEnumerable<NWFProcessEntity> list = new List<NWFProcessEntity>();
            list = nWFProcessIBLL.GetMyPageList(userInfo.userId, parameter.pagination, parameter.queryJson);
            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records,
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 流程任务
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private Response GetTaskPageList(dynamic _)
        {
            QueryModel parameter = this.GetReqData<QueryModel>();

            IEnumerable<NWFProcessEntity> list = new List<NWFProcessEntity>();
            Tast q = parameter.queryJson.ToObject<Tast>();
            UserInfo userInfo = LoginUserInfo.Get();
            switch (q.categoryId)
            {
                case "1"://我的流程
                    list = nWFProcessIBLL.GetMyPageList(userInfo.userId, parameter.pagination, parameter.queryJson);
                    foreach (var info in list)
                    {
                        var departments = userIBLL.GetDepartmentNameList(info.F_CreateUserId);
                        if (departments != null)
                        {

                            info.DepartmentName = departments.F_FullName;
                        }
                    }
                    break;
                case "2"://待办流程
                    list = nWFProcessIBLL.GetMyTaskPageList(userInfo, parameter.pagination, parameter.queryJson);
                    foreach (var info in list)
                    {
                        DepartmentEntity user = userIBLL.GetDepartmentNameList(info.F_CreateUserId);

                        if (user != null)
                        {
                            info.DepartmentName = user.F_FullName;
                        }

                    }
                    break;
                case "3"://已办流程
                    list = nWFProcessIBLL.GetMyFinishTaskPageList(userInfo, parameter.pagination, parameter.queryJson);
                    foreach (var info in list)
                    {
                        DepartmentEntity user = userIBLL.GetDepartmentNameList(info.F_CreateUserId);

                        if (user != null)
                        {
                            info.DepartmentName = user.F_FullName;
                        }
                    }
                    break;
            }
            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records,
            };
            return Success(jsonData);
        }

        /*  public Response GetProcessDetails1(dynamic _)
          {
              UserInfo userInfo = LoginUserInfo.Get();
              QueryModel parameter = this.GetReqData<QueryModel>();
              //IEnumerable<NWFProcessEntity> list = new List<NWFProcessEntity>();
              TastProcess q = parameter.queryJson.ToObject<TastProcess>();
              var data = nWFProcessIBLL.GetProcessDetails(q.processId, q.taskId, userInfo);
              if (!string.IsNullOrEmpty(data.childProcessId))
              {
                  q.processId = data.childProcessId;
              }

              var taskNode = nWFProcessIBLL.GetTaskUserList(q.processId);

              var jsonData = new
              {
                  info = data,
                  task = taskNode
              };

              return Success(jsonData);
          }
  */

        /// <summary>
        /// 获取我的任务列表
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private Response GetMyTaskList(dynamic _)
        {
            QueryModel parameter = this.GetReqData<QueryModel>();

            IEnumerable<NWFProcessEntity> list = new List<NWFProcessEntity>();
            list = nWFProcessIBLL.GetMyTaskPageList(userInfo, parameter.pagination, parameter.queryJson);
            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records,
            };
            return Success(jsonData);
        }

        /// <summary>
        /// 获取我已处理的任务列表
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private Response GetMyMakeTaskList(dynamic _)
        {
            QueryModel parameter = this.GetReqData<QueryModel>();

            IEnumerable<NWFProcessEntity> list = new List<NWFProcessEntity>();
            list = nWFProcessIBLL.GetMyFinishTaskPageList(userInfo, parameter.pagination, parameter.queryJson);
            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records,
            };
            return Success(jsonData);
        }


        /// <summary>
        /// 获取流程模板
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private Response GetSchemePageList(dynamic _)
        {
            QueryModel parameter = this.GetReqData<QueryModel>();

            IEnumerable<NWFSchemeInfoEntity> list = new List<NWFSchemeInfoEntity>();
            list = nWFSchemeIBLL.GetAppInfoPageList(parameter.pagination, this.userInfo, parameter.queryJson);
            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records,
            };
            return Success(jsonData);
        }

        /// <summary>
        /// 获取流程模板
        /// </summary>
        /// <param name="code">流程编码</param>
        /// <returns></returns>
        public Response GetSchemeByCode(dynamic _)
        {
            string code = this.GetReqData();
            var schemeInfo = nWFSchemeIBLL.GetInfoEntityByCode(code);
            if (schemeInfo != null)
            {
                var data = nWFSchemeIBLL.GetSchemeEntity(schemeInfo.F_SchemeId);
                return Success(data);
            }
            return Fail("找不到该流程模板");
        }


        /// <summary>
        /// 获取流程下一节点审核
        /// </summary>
        /// <returns></returns>
        public Response GetNextAuditors(dynamic _)
        {
            flowParam parameter = this.GetReqData<flowParam>();
            UserInfo userInfo = LoginUserInfo.Get();
            var data = nWFProcessIBLL.GetNextAuditors(parameter.code, parameter.processId, parameter.taskId, parameter.nodeId, parameter.operationCode, userInfo);
            return Success(data);
        }
        /// <summary>
        /// 获取流程进程信息
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetProcessDetails(dynamic _)
        {
            UserInfo userInfo = LoginUserInfo.Get();
            flowParam parameter = this.GetReqData<flowParam>();

            var data = nWFProcessIBLL.GetProcessDetails(parameter.processId, parameter.taskId, userInfo);
            if (!string.IsNullOrEmpty(data.childProcessId))
            {
                parameter.processId = data.childProcessId;
            }

            var taskNode = nWFProcessIBLL.GetTaskUserList(parameter.processId);

            var jsonData = new
            {
                info = data,
                task = taskNode
            };

            return Success(jsonData);
        }

        /// <summary>
        /// 创建流程实例
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private Response Create(dynamic _)
        {
            flowParam parameter = this.GetReqData<flowParam>();

            List<FormParam> req = parameter.formreq.ToObject<List<FormParam>>();// 获取模板请求数据
            foreach (var item in req)
            {
                formSchemeIBLL.SaveInstanceForm(item.schemeInfoId, item.processIdName, item.keyValue, item.formData);
            }

            nWFProcessIBLL.CreateFlow(parameter.code, parameter.processId, parameter.title, parameter.level, parameter.auditors, userInfo);
            return this.Success("创建成功");
        }

        /// <summary>
        /// 重新创建流程
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response AgainCreateFlow(dynamic _)
        {
            flowParam parameter = this.GetReqData<flowParam>();
            if (parameter == null)
            {
                HttpContext.Current.Response.ContentType = "application/json";
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

                string jsonText = string.Empty;

                HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空

                StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
                jsonText = sr.ReadToEnd();

                //ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
                parameter = jsonText.ToObject<flowParam>();
            }
            // 保存自定义表单
            List<FormParam> req = parameter.formreq.ToObject<List<FormParam>>();// 获取模板请求数据
            foreach (var item in req)
            {
                formSchemeIBLL.SaveInstanceForm(item.schemeInfoId, item.processIdName, item.keyValue, item.formData);
            }
            nWFProcessIBLL.AgainCreateFlow(parameter.processId, userInfo, parameter.des);
            try
            {
                afterAuditTask(parameter.processId);
            }
            catch { }
            return Success("重新创建成功");
        }

        /// <summary>
        /// 创建流程(子流程)
        /// </summary>
        public Response CreateChildFlow(dynamic _)
        {
            flowParam parameter = this.GetReqData<flowParam>();
            // 保存自定义表单
            List<FormParam> req = parameter.formreq.ToObject<List<FormParam>>();// 获取模板请求数据
            foreach (var item in req)
            {
                formSchemeIBLL.SaveInstanceForm(item.schemeInfoId, item.processIdName, item.keyValue, item.formData);
            }
            nWFProcessIBLL.CreateChildFlow(parameter.code, parameter.processId, parameter.parentProcessId, parameter.parentTaskId, userInfo);
            return Success("子流程创建成功");
        }

        /// <summary>
        /// 保存草稿(流程)
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response SaveDraft(dynamic _)
        {
            flowParam parameter = this.GetReqData<flowParam>();

            // 保存自定义表单
            List<FormParam> req = parameter.formreq.ToObject<List<FormParam>>();// 获取模板请求数据
            foreach (var item in req)
            {
                formSchemeIBLL.SaveInstanceForm(item.schemeInfoId, item.processIdName, item.keyValue, item.formData);
            }
            if (!string.IsNullOrEmpty(parameter.processId))
            {
                nWFProcessIBLL.SaveDraft(parameter.processId, parameter.code, userInfo);
            }
            return Success("保存成功");
        }

        /// <summary>
        /// 删除草稿
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response DeleteDraft(dynamic _)
        {
            string processId = this.GetReqData();
            nWFProcessIBLL.DeleteDraft(processId, userInfo);
            return Success("草稿删除成功");
        }

        public Response DeleteProcess(dynamic _)
        {
            string processId = this.GetReqData();
            var processInfo = nWFProcessIBLL.GetEntity(processId);
            var taskList = nWFTaskIBLL.GetALLTaskList(processId);
            if (taskList != null)
            {
                nWFProcessIBLL.deleteTaskList(taskList, processId);
            }
            nWFProcessIBLL.DeleteEntity(processId);
            return Success("流程删除成功");
        }
        public Response ResetProcess(dynamic _)
        {
            string processId = this.GetReqData();
            var processInfo = nWFProcessIBLL.GetEntity(processId);
            var taskList = nWFTaskIBLL.GetALLTaskList(processId);
            if (taskList != null)
            {
                nWFProcessIBLL.deleteTaskList(taskList, processId);
            }
            nWFProcessIBLL.DeleteEntity(processId);
            string schemeCode = processInfo.F_SchemeCode.Replace("1", "");
            if (schemeCode == "ProjectContract")
            {
                var data = projectContractIBLL.GetContractEntityByProcessId(processId);
                if (data != null)
                {
                    data.ContractStatus = 1;
                    data.WorkFlowId = "";
                    projectContractIBLL.SaveEntity(data.id, data);
                }
            }
            else if (schemeCode == "ProjectTask")
            {
                var data = projectTaskIBLL.GetEntityByProcessId(processId);
                if (data != null)
                {
                    data.TaskStatus = 9;
                    data.WorkFlowId = "";
                    projectTaskIBLL.SaveEntity(data.id, data);
                }
            }
            else if (schemeCode == "ProjectPayment")
            {

                var data = projectPaymentIBLL.GetProjectPaymentEntityByProcessId(processId);
                if (data != null)
                {
                    data.PaymentStatus = 1;
                    data.WorkFlowId = "";
                    projectPaymentIBLL.SaveEntity(data.id, data);
                }
            }
            else if (schemeCode == "ProjectBilling")
            {

                var data = projectBillingIBLL.GetEntityByProcessId(processId);
                if (data != null)
                {
                    data.BillingStatus = 1;
                    data.WorkFlowId = "";
                    projectBillingIBLL.SaveEntity(data.Id, data);
                }
            }
            else if (schemeCode == "ProjectRecruit")
            {

                var data = projectRecruitIBLL.GetEntityByProcessId(processId);
                if (data != null)
                {
                    data.RecruitStatus = 1;
                    data.WorkFlowId = "";
                    projectRecruitIBLL.SaveEntity(data.id, data);
                }
            }
            else if (schemeCode == "Payment")
            {

                var data = paymentIBLL.GetPaymentEntityByProcessId(processId);
                if (data != null)
                {
                    data.PaymentStatus = 1;
                    data.WorkFlowId = "";
                    paymentIBLL.SaveEntity(data.Id, data);
                }
            }
            return Success("流程重置成功，请重新提交提审");
        }


        /// <summary>
        /// 审批流程
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response AuditFlow(dynamic _)
        {
            flowParam parameter = this.GetReqData<flowParam>();
            if (parameter == null)
            {
                HttpContext.Current.Response.ContentType = "application/json";
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

                string jsonText = string.Empty;

                HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空

                StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
                jsonText = sr.ReadToEnd();

                //ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
                parameter = jsonText.ToObject<flowParam>();
            }



            //报告状态
            int taskStatus = -1;
            var projectTask = projectTaskIBLL.GetEntityByProcessId(parameter.processId);
            if (projectTask != null)
            {
                taskStatus = projectTask.TaskStatus.Value;
            }
            // 保存自定义表单
            List<FormParam> req = parameter.formreq.ToObject<List<FormParam>>();// 获取模板请求数据
            foreach (var item in req)
            {
                formSchemeIBLL.SaveInstanceForm(item.schemeInfoId, item.processIdName, item.keyValue, item.formData);
            }
            nWFProcessIBLL.AuditFlow(parameter.operationCode, parameter.operationName, parameter.processId, parameter.taskId, parameter.des, parameter.auditors, "", parameter.signUrl, userInfo);
            //如果是项目付款的拒绝流程，需要重新计算项目合同的有效合同额，把拒绝的金额加上去
            try
            {
                if (parameter.operationCode == "disagree")
                {
                    afterAuditTask(parameter.processId);
                }
                //判断报告是否完成
                if (projectTask != null)
                {
                    projectTask = projectTaskIBLL.GetEntityByProcessId(parameter.processId);
                    if (taskStatus != projectTask.TaskStatus && projectTask.TaskStatus == 5)
                    {
                        projectTask.FlowFinishedTime = DateTime.Now;
                        projectTaskIBLL.SaveEntity(projectTask.id, projectTask);
                    }
                }
            }
            catch { }
            return Success("审批成功");
        }

        /// <summary>
        /// 流程转交
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response SignFlow(dynamic _)
        {
            flowParam parameter = this.GetReqData<flowParam>();
            if (parameter == null)
            {
                HttpContext.Current.Response.ContentType = "application/json";
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

                string jsonText = string.Empty;

                HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空

                StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
                jsonText = sr.ReadToEnd();

                //ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
                parameter = jsonText.ToObject<flowParam>();
            }

            // 保存自定义表单
            List<FormParam> req = parameter.formreq.ToObject<List<FormParam>>();// 获取模板请求数据
            foreach (var item in req)
            {
                formSchemeIBLL.SaveInstanceForm(item.schemeInfoId, item.processIdName, item.keyValue, item.formData);
            }

            nWFProcessIBLL.SignFlow(parameter.processId, parameter.taskId, parameter.userId, parameter.des, userInfo);
            return Success("转交成功");
        }

        /// <summary>
        /// 流程转交审核
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response SignAuditFlow(dynamic _)
        {
            flowParam parameter = this.GetReqData<flowParam>();

            // 保存自定义表单
            List<FormParam> req = parameter.formreq.ToObject<List<FormParam>>();// 获取模板请求数据
            foreach (var item in req)
            {
                formSchemeIBLL.SaveInstanceForm(item.schemeInfoId, item.processIdName, item.keyValue, item.formData);
            }

            nWFProcessIBLL.SignAuditFlow(parameter.operationCode, parameter.processId, parameter.taskId, parameter.des, userInfo);
            return Success("转交审批成功");
        }


        /// <summary>
        /// 催办流程
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response UrgeFlow(dynamic _)
        {
            string processId = this.GetReqData();
            FormParam1 q = new FormParam1();
            if (string.IsNullOrEmpty(processId))
            {
                HttpContext.Current.Response.ContentType = "application/json";
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

                string jsonText = string.Empty;

                HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空

                StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
                jsonText = sr.ReadToEnd();

                //ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
                q = jsonText.ToObject<FormParam1>();
            }
            else
            {
                q = processId.ToObject<FormParam1>();
            }
            UserInfo userInfo = LoginUserInfo.Get();
            nWFProcessIBLL.UrgeFlow(q.processId, userInfo);
            return Success("催办成功");
        }

        /// <summary>
        /// 撤销流程（只有在该流程未被处理的情况下）
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response RevokeFlow(dynamic _)
        {
            string str = this.GetReqData();
            flowParam flowParam = new flowParam();
            if (str == null)
            {
                HttpContext.Current.Response.ContentType = "application/json";
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

                string jsonText = string.Empty;

                HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空

                StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
                jsonText = sr.ReadToEnd();

                //ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
                flowParam = jsonText.ToObject<flowParam>();
            }
            else
            {
                flowParam = str.ToObject<flowParam>();
            }

            //nWFProcessIBLL.RevokeFlow(processId, userInfo);
            //return Success("撤销成功");
            var res = nWFProcessIBLL.RevokeAudit(flowParam.processId, flowParam.taskId, userInfo);
            if (res)
            {
                return Success("撤销成功");
            }
            else
            {
                return Fail("撤销失败，当前不允许撤销！");
            }
        }

        /// <summary>
        /// 确认阅读
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response ReferFlow(dynamic _)
        {
            flowParam parameter = this.GetReqData<flowParam>();
            nWFProcessIBLL.ReferFlow(parameter.processId, parameter.taskId, userInfo);
            return Success("确认成功");
        }
        private void afterAuditTask(string processId)
        {
            var projectPayment = projectPaymentIBLL.GetEntityByProcessId(processId);
            if (projectPayment != null)
            {
                if (!string.IsNullOrEmpty(projectPayment.ProjectId))
                {
                    var projectContract = projectContractIBLL.GetProjectContractByProjectId(projectPayment.ProjectId);
                    if (projectContract.ToList().Count > 0)
                    {
                        foreach (var item in projectContract)
                        {
                            projectContractIBLL.SaveEntity(item.id, item);
                        }
                    }
                }
            }
            else
            {
                var contract = projectContractIBLL.GetContractByProcessId(processId);
                if (contract != null)
                {
                    if (!string.IsNullOrEmpty(contract.id))
                    {
                        projectContractIBLL.SaveEntity(contract.id, contract);

                    }
                }
            }
        }
        private class flowParam
        {
            /// <summary>
            /// 流程模板编码
            /// </summary>
            public string code { get; set; }
            /// <summary>
            /// 流程进程主键
            /// </summary>
            public string processId { get; set; }
            /// <summary>
            /// 流程任务主键
            /// </summary>
            public string taskId { get; set; }
            /// <summary>
            /// 流程节点Id
            /// </summary>
            public string nodeId { get; set; }
            /// <summary>
            /// 审核操作码
            /// </summary>
            public string operationCode { get; set; }
            /// <summary>
            /// 审核操作名称
            /// </summary>
            public string operationName { get; set; }

            /// <summary>
            /// 流程自定义标题
            /// </summary>
            public string title { get; set; }
            /// <summary>
            /// 流程等级
            /// </summary>
            public int level { get; set; }
            /// <summary>
            /// 流程审核用户
            /// </summary>
            public string auditors { get; set; }
            /// <summary>
            /// 表单信息
            /// </summary>
            public string formreq { get; set; }
            /// <summary>
            /// 描述
            /// </summary>
            public string des { get; set; }
            /// <summary>
            /// 转交人员主键
            /// </summary>
            public string userId { get; set; }
            /// <summary>
            /// 父流程进程主键
            /// </summary>
            public string parentProcessId { get; set; }
            /// <summary>
            /// 父流程任务主键
            /// </summary>
            public string parentTaskId { get; set; }

            /// <summary>
            /// 签字图片信息
            /// </summary>
            public string signUrl { get; set; }
        }

        /// <summary>
        /// 查询条件对象
        /// </summary>
        private class QueryModel
        {
            public Pagination pagination { get; set; }
            public string queryJson { get; set; }
        }
        /// <summary>
        /// 自定义表单提交参数
        /// </summary>
        private class FormParam
        {
            /// <summary>
            /// 流程模板id
            /// </summary>
            public string schemeInfoId { get; set; }
            /// <summary>
            /// 关联字段名称
            /// </summary>
            public string processIdName { get; set; }
            /// <summary>
            /// 数据主键值
            /// </summary>
            public string keyValue { get; set; }
            /// <summary>
            /// 表单数据
            /// </summary>
            public string formData { get; set; }
        }
        private class FormParam1
        {
            /// <summary>
            /// 流程模板id
            /// </summary>
            public string processId { get; set; }

        }
    }
}