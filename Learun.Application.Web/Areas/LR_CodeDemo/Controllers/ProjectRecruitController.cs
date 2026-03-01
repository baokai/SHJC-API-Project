using Learun.Application.Base.SystemModule;
using Learun.Application.Organization;
using Learun.Application.TwoDevelopment.LR_CodeDemo;
using Learun.Util;
using Learun.Util.Operat;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Learun.Application.Web.Areas.LR_CodeDemo.Controllers
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
    /// 创 建：超级管理员
    /// 日 期：2022-03-16 18:06
    /// 描 述：用工申请
    /// </summary>
    public class ProjectRecruitController : MvcControllerBase
    {
        private ProjectRecruitIBLL projectRecruitIBLL = new ProjectRecruitBLL();
        private ProjectContractBLL projectContractBLL = new ProjectContractBLL();
        private UserIBLL userIBLL = new UserBLL();
        private ProjectManageIBLL projectManageIBLL = new ProjectManageBLL();
        private DataItemIBLL dataItemBLL = new DataItemBLL();



        #region 视图功能

        /// <summary>
        /// 主页面
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            if (followPerson.F_MoreDepartmentId != null)
            {
                return View("IndexDepartmentId");
            }
            return View();
        }
        /// <summary>
        /// 用工流程表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Formliuchen()
        {
            return View();
        }
        /// <summary>
        /// 表单页
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }


        ///<summary>
        ///用工页面预览
        /// </summary>
        ///<returns></returns>
        [HttpGet]
        public ActionResult PreviewForm()
        {
            return View();
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageList(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = projectRecruitIBLL.GetPageList(paginationobj, queryJson);
            foreach (var info in data)
            {
                List<ProjectContractEntity> projectContracts = projectContractBLL.GetProjectContractByProjectId(info.ProjectId);
                if (projectContracts.Count > 0)
                {
                    info.ContractNo = projectContracts.FirstOrDefault().ContractNo;
                }
            }
            var jsonData = new
            {
                rows = data,
                total = paginationobj.total,
                page = paginationobj.page,
                records = paginationobj.records
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 二期用工接口列表查询
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageprojectRecruitList(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = projectRecruitIBLL.GetPageList(paginationobj, queryJson);
            foreach (var info in data)
            {
                List<ProjectContractEntity> projectContracts = projectContractBLL.GetProjectContractByProjectId(info.ProjectId);
                if (projectContracts.Count > 0)
                {
                    info.ContractNo = projectContracts.FirstOrDefault().ContractNo;
                }
            }
            var jsonData = new
            {
                rows = data,
                total = paginationobj.total,
                page = paginationobj.page,
                records = paginationobj.records
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageListDepartmentId(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            List<ProjectRecruitVo> listdate = new List<ProjectRecruitVo>();
            if (followPerson.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( a.FDepartmentId='" + strList[i] + "' or t.DepartmentId = '" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or pc.DepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or (  a.FDepartmentId='" + strList[i] + "' or t.DepartmentId = '" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or pc.DepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = projectRecruitIBLL.GetPageListDepartmentId(paginationobj, queryJson, deps);
                if (data.ToList().Count > 0)
                {
                    foreach (var info in data)
                    {
                        List<ProjectContractEntity> projectContracts = projectContractBLL.GetProjectContractByProjectId(info.ProjectId);
                        if (projectContracts.Count > 0)
                        {
                            info.ContractNo = projectContracts.FirstOrDefault().ContractNo;
                        }
                        listdate.Add(info);
                    }
                }

            }
            var jsonData = new
            {
                rows = listdate,
                total = paginationobj.total,
                page = paginationobj.page,
                records = paginationobj.records
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 获取表单数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetFormData(string keyValue)
        {
            var ProjectRecruitData = projectRecruitIBLL.GetProjectRecruitEntity(keyValue);
            var jsonData = new
            {
                ProjectRecruit = ProjectRecruitData,
            };
            return Success(jsonData);
        }
        ///<summary>
        ///根据id获取用工页面预览需要的数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPreviewFormData(string keyValue)
        {
            var ProjectRecruitData = projectRecruitIBLL.GetPreviewProjectRecruit(keyValue);
            var applyPerson = userIBLL.GetFollowPersonNameByUserId(ProjectRecruitData.ApplyPerson);
            DataItemDetailEntity paymentMethod = dataItemBLL.GetDetailItemName(ProjectRecruitData.PaymentMethod, "PaymentMethod");

            if (applyPerson != null)
            {
                ProjectRecruitData.ApplyPerson = applyPerson.F_RealName;

            }
            if (paymentMethod != null)
            {
                ProjectRecruitData.PaymentMethod = paymentMethod.F_ItemName;
            }
            var jsonData = new
            {
                ProjectRecruit = ProjectRecruitData,
            };
            return Success(jsonData);
        }

        /// <summary>
        /// 获取表单数据
        /// </summary>
        /// <param name="processId">流程实例主键</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetFormDataByProcessId(string processId)
        {
            //var ProjectRecruitData = projectRecruitIBLL.GetEntityByProcessId( processId );
            var ProjectRecruitData = projectRecruitIBLL.GetProjectRecruitByProcessId(processId);
            //项目名称
            ProjectRecruitData.ProjectName = ProjectRecruitData.ProjectName;
            //申请人
            var applyPerson = userIBLL.GetFollowPersonNameByUserId(ProjectRecruitData.ApplyPerson);
            if (applyPerson != null)
            {
                ProjectRecruitData.ApplyPersonName = applyPerson.F_RealName;

            }

            var jsonData = new
            {
                ProjectRecruit = ProjectRecruitData,
            };
            return Success(jsonData);
        }
        #endregion

        #region 提交数据

        /// <summary>
        /// 删除实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult DeleteForm(string keyValue)
        {
            projectRecruitIBLL.DeleteEntity(keyValue);
            return Success("删除成功！", "项目用工(删除)", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, keyValue);
        }
        /// <summary>
        /// 保存实体数据（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="strEntity">实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, string strEntity)
        {
            ProjectRecruitEntity entity = strEntity.ToObject<ProjectRecruitEntity>();
            projectRecruitIBLL.SaveEntity(keyValue, entity);
            if (string.IsNullOrEmpty(keyValue))
            {
            }
            return Success("保存成功！", "项目用工", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, entity.ToJson());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult UpdateFlowId(string keyValue, string ProcessId)
        {
            projectRecruitIBLL.UpdateFlowId(keyValue, ProcessId);
            return Success("操作成功！", "项目用工(提交)", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, ProcessId);
        }
        #endregion

    }
}
