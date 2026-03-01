using Learun.Application.Base.SystemModule;
using Learun.Application.Organization;
using Learun.Application.TwoDevelopment.LR_CodeDemo;
using Learun.Cache.Base;
using Learun.Cache.Factory;
using Learun.Util;
using Learun.Util.Operat;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Learun.Application.Web.Areas.LR_CodeDemo.Controllers
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
    /// 创 建：超级管理员
    /// 日 期：2022-03-11 00:18
    /// 描 述：项目任务单
    /// </summary>
    public class ProjectTaskController : MvcControllerBase
    {

        private ProjectTaskIBLL projectTaskIBLL = new ProjectTaskBLL();
        private ProjectContractBLL projectContractBLL = new ProjectContractBLL();
        private DepartmentIBLL departmentIBLL = new DepartmentBLL();
        private CompanyIBLL companyIBLL = new CompanyBLL();
        private UserIBLL userIBLL = new UserBLL();
        private ProjectManageIBLL projectManageIBLL = new ProjectManageBLL();
        private DataItemIBLL dataItemBLL = new DataItemBLL();
        private ProjectContractIBLL projectContractIBLL = new ProjectContractBLL();
        private ICache cache = CacheFactory.CaChe();
        #region 视图功能

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
        /// 多部门主页面
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IndexDepartmentId()
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
        [HttpGet]
        public ActionResult FormAdd()
        {

            return View();
        }

        [HttpGet]
        public ActionResult PreviewForm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PreviewReportForm()
        {
            return View();
        }
        [HttpGet]
        public ActionResult UpdateNameForm()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ReportForm()
        {
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            if (followPerson != null)
            {
                user = followPerson.F_HZ;
                if (user == "1" && user.Equals("1"))
                {

                    return View("ReportFormHZ");

                }
            }

            return View();
        }

        public ActionResult FormChange()
        {
            return View();
        }

        public ActionResult CreateQRCode()
        {
            return View();
        }
        /// <summary>
        /// 报告查合同数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ContractPreviewForm()
        {
            return View();
        }
        public ActionResult TaskForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        //二期生产统计图
        public ActionResult GetProducTionList()
        {
            List<ProducTionVo> list = new List<ProducTionVo>();
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            if (followPerson.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or t.TaskDepartmentId like '%" + strList[i] + "%' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or pc.DepartmentId='" + strList[i] + "' or t.SubDepartmentId='" + strList[i] + "' or t.MainDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or t.TaskDepartmentId like '%" + strList[i] + "%' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or pc.DepartmentId='" + strList[i] + "' or t.SubDepartmentId='" + strList[i] + "' or t.MainDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = projectTaskIBLL.GetProducTionListDepartmentId(deps);
                foreach (var info in data)
                {
                    ProducTionVo tionVo = new ProducTionVo();
                    tionVo.DepartmentId = LoginUserInfo.Get().departmentId;
                    for (int i = 0; i < 12; i++)
                    {
                        if (info.Month.ToInt() == i)
                        {
                            tionVo.Month = i.ToString();
                            tionVo.Quantity = info.Quantity;
                            tionVo.Amount = info.Amount;
                        }
                        list.Add(tionVo);
                    }
                }
            }
            else
            {
                var data = projectTaskIBLL.GetProducTionList(followPerson.F_DepartmentId);
                foreach (var info in data)
                {
                    ProducTionVo tionVo = new ProducTionVo();
                    tionVo.DepartmentId = LoginUserInfo.Get().departmentId;
                    for (int i = 0; i < 12; i++)
                    {
                        if (info.Month.ToInt() == i)
                        {
                            tionVo.Month = i.ToString();
                            tionVo.Quantity = info.Quantity;
                            tionVo.Amount = info.Amount;
                        }
                        list.Add(tionVo);
                    }
                }
            }
            return Success(list);
        }


        //二期质量技术部显示
        public ActionResult GetQualityTechnology(string categoryId)
        {
            List<ProducTionVo> list = new List<ProducTionVo>();
            switch (categoryId)
            {
                case "1":
                    //1.全公司项目待实施数量/待实施金额
                    var date1 = projectTaskIBLL.GetQualityTechnologyImplement(categoryId);
                    for (int i = 0; i < 12; i++)
                    {
                        ProducTionVo tionVo = new ProducTionVo();
                        var department = departmentIBLL.GetEntity(LoginUserInfo.Get().departmentId);

                        if (department != null)
                        {
                            tionVo.DepartmentId = department.F_FullName;
                        }

                        foreach (var info in date1)
                        {
                            if (info.Month.ToInt() == i)
                            {
                                tionVo.Years = info.Years;
                                tionVo.Month = i.ToString();
                                tionVo.Quantity = info.Quantity;
                                tionVo.Amount = info.Amount;
                            }

                        }
                        if (tionVo.Month != null)
                        {
                            list.Add(tionVo);
                        }
                    }

                    break;
                case "2":

                    //2.全公司项目实施数量/实施金额
                    var date2 = projectTaskIBLL.GetQualityTechnologyImplement(categoryId);
                    for (int i = 0; i < 12; i++)
                    {
                        ProducTionVo tionVo = new ProducTionVo();
                        var department = departmentIBLL.GetEntity(LoginUserInfo.Get().departmentId);

                        if (department != null)
                        {
                            tionVo.DepartmentId = department.F_FullName;
                        }

                        foreach (var info in date2)
                        {
                            if (info.Month.ToInt() == i)
                            {
                                tionVo.Years = info.Years;
                                tionVo.Month = i.ToString();
                                tionVo.Quantity = info.Quantity;
                                tionVo.Amount = info.Amount;
                            }

                        }
                        if (tionVo.Month != null)
                        {
                            list.Add(tionVo);
                        }
                    }
                    break;
                case "3":
                    //3.全公司项目超期数量/超期金额
                    var date3 = projectTaskIBLL.GetQualityTechnologyImplement(categoryId);
                    for (int i = 0; i < 12; i++)
                    {
                        ProducTionVo tionVo = new ProducTionVo();
                        var department = departmentIBLL.GetEntity(LoginUserInfo.Get().departmentId);

                        if (department != null)
                        {
                            tionVo.DepartmentId = department.F_FullName;
                        }

                        foreach (var info in date3)
                        {
                            if (info.Month.ToInt() == i)
                            {
                                tionVo.Years = info.Years;
                                tionVo.Month = i.ToString();
                                tionVo.Quantity = info.Quantity;
                                tionVo.Amount = info.Amount;
                            }

                        }
                        if (tionVo.Month != null)
                        {
                            list.Add(tionVo);
                        }
                    }
                    break;
            }
            return Success(list);
        }


        [HttpGet]
        [AjaxOnly]
        public ActionResult GetProducTionTimeoutList()
        {
            List<ProducTionVo> list = new List<ProducTionVo>();
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            if (followPerson.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or pt.TaskDepartmentId like '%" + strList[i] + "' or pt.SubDepartmentId='" + strList[i] + "' or pt.MainDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or pt.TaskDepartmentId like '%" + strList[i] + "' or pt.SubDepartmentId='" + strList[i] + "' or pt.MainDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = projectTaskIBLL.GetProducTionTimeoutListDepartmentId(deps);
                foreach (var info in data)
                {
                    ProducTionVo tionVo = new ProducTionVo();
                    tionVo.DepartmentId = LoginUserInfo.Get().departmentId;
                    for (int i = 0; i < 12; i++)
                    {
                        if (info.Month.ToInt() == i)
                        {
                            tionVo.Month = i.ToString();
                            tionVo.Quantity = info.Quantity;
                            tionVo.Amount = info.Amount;
                        }
                        list.Add(tionVo);
                    }
                }
            }
            else
            {
                var data = projectTaskIBLL.GetProducTionTimeoutList(followPerson.F_DepartmentId);
                foreach (var info in data)
                {
                    ProducTionVo tionVo = new ProducTionVo();
                    tionVo.DepartmentId = LoginUserInfo.Get().departmentId;
                    for (int i = 0; i < 12; i++)
                    {
                        if (info.Month.ToInt() == i)
                        {
                            tionVo.Month = i.ToString();
                            tionVo.Quantity = info.Quantity;
                            tionVo.Amount = info.Amount;
                        }
                        list.Add(tionVo);
                    }
                }
            }
            return Success(list);
        }



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
            var data = projectTaskIBLL.GetPageList(paginationobj, queryJson);
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
        /// 报告显示
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageToBeDetectList(string pagination, string queryJson, string categoryId)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            IEnumerable<ProjectTaskVo> list = new List<ProjectTaskVo>();
            switch (categoryId)
            {
                //待检测
                case "1":
                    list = projectTaskIBLL.GetPageToBeDetectList(paginationobj, queryJson);
                    break;
                //待报告
                case "2":
                    list = projectTaskIBLL.GetPageToBeReportedList(paginationobj, queryJson);
                    break;
                //已完成
                case "3":
                    list = projectTaskIBLL.GetPageHaveCompletedList(paginationobj, queryJson);
                    break;
                //超期项目
                case "4":
                    list = projectTaskIBLL.GetPageOverdueItemList(paginationobj, queryJson);
                    break;

            }

            var jsonData = new
            {
                rows = list,
                total = paginationobj.total,
                page = paginationobj.page,
                records = paginationobj.records
            };
            return Success(jsonData);
        }

        /// <summary>
        /// 多部门获取页面显示列表数据
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
            List<ProjectTaskVo> listdate = new List<ProjectTaskVo>();
            if (followPerson.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or t.TaskDepartmentId like '%" + strList[i] + "%' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or pc.DepartmentId='" + strList[i] + "' or t.SubDepartmentId='" + strList[i] + "' or t.MainDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or t.TaskDepartmentId like '%" + strList[i] + "%' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or pc.DepartmentId='" + strList[i] + "' or t.SubDepartmentId='" + strList[i] + "' or t.MainDepartmentId='" + strList[i] + "') ";
                    }

                }

                var data = projectTaskIBLL.GetPageListDepartmentId(paginationobj, queryJson, deps);
                if (data.ToList().Count > 0)
                {
                    foreach (var ina in data)
                    {
                        listdate.Add(ina);
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
        /// 报告导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>     
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageListAll(string queryJson)
        {

            var data = projectTaskIBLL.GetPageList(queryJson);
            List<ProjectTaskVo> list = new List<ProjectTaskVo>();
            foreach (var info in data)
            {
                list.Add(info);
            }
            //放入缓存
            var uuid = Guid.NewGuid().ToString().Replace("-", "");
            cache.Write(uuid, JsonConvert.SerializeObject(list));
            var jsonData = new
            {
                rows = uuid
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 多部门报告导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>     
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageListAllDepartmentId(string queryJson)
        {
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            List<ProjectTaskVo> listdate = new List<ProjectTaskVo>();
            if (followPerson.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or t.TaskDepartmentId like '%" + strList[i] + "%' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or pc.DepartmentId='" + strList[i] + "' or t.SubDepartmentId='" + strList[i] + "' or t.MainDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or t.TaskDepartmentId like '%" + strList[i] + "%' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or pc.DepartmentId='" + strList[i] + "' or t.SubDepartmentId='" + strList[i] + "' or t.MainDepartmentId='" + strList[i] + "') ";
                    }

                }

                var data = projectTaskIBLL.GetPageListDepartmentId(queryJson, deps);
                if (data.ToList().Count > 0)
                {
                    foreach (var ina in data)
                    {
                        listdate.Add(ina);
                    }
                }
            }



            //放入缓存
            var uuid = Guid.NewGuid().ToString().Replace("-", "");
            cache.Write(uuid, JsonConvert.SerializeObject(listdate));
            var jsonData = new
            {
                rows = uuid
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
            var ProjectTaskData = projectTaskIBLL.GetProjectTaskEntity(keyValue);

            //if (ProjectTaskData.ProjectResponsible != null)
            //{
            //    string[] strList = ProjectTaskData.ProjectResponsible.Split(',');

            //    string projectResponsible = "";
            //    for (var i = 0; i < strList.Length; i++)
            //    {
            //        var Responsible = userIBLL.GetFollowPersonNameByUserId(strList[i]);
            //        if (Responsible != null)
            //        {
            //            if (string.IsNullOrWhiteSpace(projectResponsible))
            //            {
            //                projectResponsible = Responsible.F_RealName;
            //            }
            //            else
            //            {
            //                projectResponsible = projectResponsible + "," + Responsible.F_RealName;
            //            }
            //        }
            //    }
            //    ProjectTaskData.ProjectResponsible = projectResponsible;
            //}


            //if (ProjectTaskData.Inspector != null)
            //{
            //    string[] strList = ProjectTaskData.Inspector.Split(',');

            //    string inspectorName = "";
            //    for (var i = 0; i < strList.Length; i++)
            //    {
            //        var inspectorInfo = userIBLL.GetFollowPersonNameByUserId(strList[i]);
            //        if (inspectorInfo != null)
            //        {
            //            if (string.IsNullOrWhiteSpace(inspectorName))
            //            {
            //                inspectorName = inspectorInfo.F_RealName;
            //            }
            //            else
            //            {
            //                inspectorName = inspectorName + "," + inspectorInfo.F_RealName;
            //            }
            //        }
            //    }
            //    ProjectTaskData.Inspector = inspectorName;
            //}
            var jsonData = new
            {
                ProjectTask = ProjectTaskData,
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 获取预览数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPreviewFormData(string keyValue)
        {
            var ProjectTaskData = projectTaskIBLL.GetPriewProjectTask(keyValue);
            DataItemDetailEntity taskStatus = dataItemBLL.GetDetailItemName(ProjectTaskData.TaskStatus, "TaskStatus");

            if (ProjectTaskData.ProjectResponsible != null)
            {
                string[] strList = ProjectTaskData.ProjectResponsible.Split(',');

                string projectResponsible = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    var Responsible = userIBLL.GetFollowPersonNameByUserId(strList[i]);
                    if (Responsible != null)
                    {
                        if (string.IsNullOrWhiteSpace(projectResponsible))
                        {
                            projectResponsible = Responsible.F_RealName;
                        }
                        else
                        {
                            projectResponsible = projectResponsible + "," + Responsible.F_RealName;
                        }
                    }
                }
                ProjectTaskData.ProjectResponsible = projectResponsible;
            }


            if (ProjectTaskData.Inspector != null)
            {
                string[] strList = ProjectTaskData.Inspector.Split(',');

                string inspectorName = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    var inspectorInfo = userIBLL.GetFollowPersonNameByUserId(strList[i]);
                    if (inspectorInfo != null)
                    {
                        if (string.IsNullOrWhiteSpace(inspectorName))
                        {
                            inspectorName = inspectorInfo.F_RealName;
                        }
                        else
                        {
                            inspectorName = inspectorName + "," + inspectorInfo.F_RealName;
                        }
                    }
                }
                ProjectTaskData.Inspector = inspectorName;
            }

            if (taskStatus != null)
            {
                ProjectTaskData.TaskStatusName = taskStatus.F_ItemName.ToString();
            }
            //评级
            var Rating = dataItemBLL.GetDetailItemName(ProjectTaskData.Rating, "Rating");
            if (Rating != null)
            {
                ProjectTaskData.RatingName = Rating.F_ItemName;
            }
            var jsonData = new
            {
                ProjectTask = ProjectTaskData,
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
            //var ProjectTaskData = projectTaskIBLL.GetEntityByProcessId(processId);
            var ProjectTaskData = projectTaskIBLL.GetPriewProjectTaskprojectId(processId);

            var reportApprover = dataItemBLL.GetDetailItemName(ProjectTaskData.ReportApprover, "ReportApprover");
            if (reportApprover != null)
            {
                ProjectTaskData.ReportApproverName = reportApprover.F_ItemName;
            }
            var reportSubject = dataItemBLL.GetDetailItemName(ProjectTaskData.ReportSubject, "ContractSubject");
            if (reportApprover != null)
            {
                ProjectTaskData.ReportSubjectName = reportSubject.F_ItemName;
            }

            if (ProjectTaskData.ProjectResponsible != null)
            {
                string[] strList = ProjectTaskData.ProjectResponsible.Split(',');

                string projectResponsible = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    var Responsible = userIBLL.GetFollowPersonNameByUserId(strList[i]);
                    if (Responsible != null)
                    {
                        if (string.IsNullOrWhiteSpace(projectResponsible))
                        {
                            projectResponsible = Responsible.F_RealName;
                        }
                        else
                        {
                            projectResponsible = projectResponsible + "," + Responsible.F_RealName;
                        }
                    }
                }
                ProjectTaskData.ProjectResponsibleName = projectResponsible;
            }


            if (ProjectTaskData.Inspector != null)
            {
                string[] strList = ProjectTaskData.Inspector.Split(',');

                string inspectorName = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    var inspectorInfo = userIBLL.GetFollowPersonNameByUserId(strList[i]);
                    if (inspectorInfo != null)
                    {
                        if (string.IsNullOrWhiteSpace(inspectorName))
                        {
                            inspectorName = inspectorInfo.F_RealName;
                        }
                        else
                        {
                            inspectorName = inspectorName + "," + inspectorInfo.F_RealName;
                        }
                    }
                }
                ProjectTaskData.InspectorName = inspectorName;
            }
            var jsonData = new
            {
                ProjectTask = ProjectTaskData,
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
            projectTaskIBLL.DeleteEntity(keyValue);
            return Success("删除成功！", "任务报告(删除)", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, keyValue);
        }
        /// <summary>
        /// 保存实体数据（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="strEntity">实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, string report, string strEntity)
        {
            ProjectTaskEntity entity = strEntity.ToObject<ProjectTaskEntity>();


            if (entity.ProjectId != null)
            {

                if (!string.IsNullOrEmpty(report) && report == "1")
                {

                }
                if (!string.IsNullOrEmpty(entity.ProjectResponsible))
                {
                    string[] userIds = entity.ProjectResponsible.Split(',');
                    string dept = "";
                    for (var i = 0; i < userIds.Length; i++)
                    {
                        var user = userIBLL.GetEntityByUserId(userIds[i]);
                        if (user != null)
                        {
                            if (!string.IsNullOrEmpty(user.F_DepartmentId))
                            {
                                if (string.IsNullOrEmpty(dept))
                                {
                                    dept = user.F_DepartmentId;
                                }
                                else
                                {
                                    dept = dept + "," + user.F_DepartmentId;
                                }
                            }
                        }
                    }

                    entity.DepartmentId = dept;
                }
                else
                {
                    entity.DepartmentId = "";
                }
                if (!string.IsNullOrEmpty(entity.Inspector))
                {
                    string[] userIds = entity.Inspector.Split(',');
                    string dept = "";
                    for (var i = 0; i < userIds.Length; i++)
                    {
                        var user = userIBLL.GetEntityByUserId(userIds[i]);
                        if (user != null)
                        {
                            if (!string.IsNullOrEmpty(user.F_DepartmentId))
                            {
                                if (string.IsNullOrEmpty(dept))
                                {
                                    dept = user.F_DepartmentId;
                                }
                                else
                                {
                                    dept = dept + "," + user.F_DepartmentId;
                                }
                            }
                        }
                    }

                    entity.TaskDepartmentId = dept;
                }
                else
                {
                    entity.TaskDepartmentId = "";
                }
            }

            projectTaskIBLL.SaveEntity(keyValue, entity);
            return Success("保存成功！", "任务报告", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, entity.ToJson());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveFormTast(string keyValue, string strEntity)
        {
            ProjectTaskEntity entity = strEntity.ToObject<ProjectTaskEntity>();
            if (!string.IsNullOrEmpty(entity.ProjectResponsible))
            {
                string[] userIds = entity.ProjectResponsible.Split(',');
                string dept = "";
                for (var i = 0; i < userIds.Length; i++)
                {
                    var user = userIBLL.GetEntityByUserId(userIds[i]);
                    if (user != null)
                    {
                        if (!string.IsNullOrEmpty(user.F_DepartmentId))
                        {
                            if (string.IsNullOrEmpty(dept))
                            {
                                dept = user.F_DepartmentId;
                            }
                            else
                            {
                                dept = dept + "," + user.F_DepartmentId;
                            }
                        }
                    }
                }

                entity.DepartmentId = dept;
            }
            else
            {
                entity.DepartmentId = "";
            }
            projectTaskIBLL.SaveFormTast(entity);
            return Success("保存成功！", "任务报告（人员修改）", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, entity.ToJson());
        }
        /// <summary>
        /// 变更
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="strEntity">实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult getFormChange(string keyValue, string strEntity)
        {
            ProjectTaskchangeEntity entity = strEntity.ToObject<ProjectTaskchangeEntity>();
            projectTaskIBLL.FormChange(keyValue, entity);
            return Success("保存成功！", "任务报告(变更)", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, entity.ToJson());
        }
        public ActionResult UpdateFlowId(string keyValue, string ProcessId)
        {
            projectTaskIBLL.UpdateFlowId(keyValue, ProcessId);
            return Success("操作成功！", "任务报告(提交)", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, ProcessId);
        }
        /// <summary>
        /// 签到PC端
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns> 
        public ActionResult UpdateFielded(string keyValue)
        {
            projectTaskIBLL.UpdateFielded(keyValue);
            return Success("签到成功！", "任务报告(签到)", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, keyValue);
        }
        #endregion

    }
}
