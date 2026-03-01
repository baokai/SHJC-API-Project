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
    /// 日 期：2022-03-11 00:59
    /// 描 述：项目开票
    /// </summary>
    public class ProjectBillingController : MvcControllerBase
    {
        private ProjectContractIBLL projectContractIBLL = new ProjectContractBLL();
        private ProjectBillingIBLL projectBillingIBLL = new ProjectBillingBLL();
        private ProjectTaskIBLL projectTaskIBLL = new ProjectTaskBLL();
        private ProjectManageIBLL projectManageIBLL = new ProjectManageBLL();
        private CompanyIBLL companyIBLL = new CompanyBLL();
        private DataItemIBLL dataItemBLL = new DataItemBLL();
        private ICache cache = CacheFactory.CaChe();
        private UserIBLL userIBLL = new UserBLL();
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
        [HttpGet]
        public ActionResult Formliuchen()
        {
            return View();
        }
        [HttpGet]
        public ActionResult FormDepartmentId()
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
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            if (followPerson != null)
            {
                user = followPerson.F_HZ;
                if (user == "1" && user.Equals("1"))
                {

                    return View("FormHZ");

                }
            }
            return View();

        }
        [HttpGet]
        public ActionResult FormHZ()
        {
            return View();
        }
        /// 预览
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FormId()
        {
            return View();
        }
        /// 取消
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ZuofeiForm()
        {
            return View();
        }

        #endregion

        #region 获取数据

        /// <summary>
        /// 开票导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageListAll(string queryJson)
        {
            var data = projectBillingIBLL.GetPageList(queryJson);
            List<ProjectBillingVo> list = new List<ProjectBillingVo>();
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
        /// 多部门开票导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageListAllDepartmentId(string queryJson)
        {
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            List<ProjectBillingVo> listdate = new List<ProjectBillingVo>();

            if (followPerson.F_MoreDepartmentId != null)
            {

                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = projectBillingIBLL.GetPageListDepartmentId(queryJson, deps);
                if (data.ToList().Count > 0)
                {
                    foreach (var info in data)
                    {

                        listdate.Add(info);

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
        /// 开票合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetPageListAllSUM(string queryJson)
        {
            var data = projectBillingIBLL.GetPageList(queryJson);
            decimal? BillingAmountSUM = 0;
            foreach (var item in data)
            {
                BillingAmountSUM = BillingAmountSUM + item.BillingAmount;
            }
            var jsonData = new
            {
                BillingAmountSUM = BillingAmountSUM
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 开票合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetPageListAllSUMDepartmentId(string queryJson)
        {

            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            List<ProjectBillingVo> listdate = new List<ProjectBillingVo>();
            decimal? BillingAmountSUM = 0;
            if (followPerson.F_MoreDepartmentId != null)
            {

                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = projectBillingIBLL.GetPageListDepartmentId(queryJson, deps);
                if (data.ToList().Count > 0)
                {
                    foreach (var info in data)
                    {
                        BillingAmountSUM = BillingAmountSUM + info.BillingAmount;
                        listdate.Add(info);
                    }
                }
            }



            var jsonData = new
            {
                BillingAmountSUM = BillingAmountSUM
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
        public ActionResult GetPageList(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = projectBillingIBLL.GetPageList(paginationobj, queryJson);
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
            List<ProjectBillingVo> listdate = new List<ProjectBillingVo>();

            if (followPerson.F_MoreDepartmentId != null)
            {

                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = projectBillingIBLL.GetPageListDepartmentId(paginationobj, queryJson, deps);
                if (data.ToList().Count > 0)
                {
                    foreach (var info in data)
                    {

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
        /// 获取发票预览信息
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPriewFormData(string keyValue)
        {
            var data = projectBillingIBLL.GetPriewFormBilling(keyValue);

            //项目名称
            var name = projectManageIBLL.GetPreviewFormData(data.ProjectId);
            if (name != null)
            {
                data.ProjectName = name.ProjectName;
            }
            //项目来源
            DataItemDetailEntity projectSource = dataItemBLL.GetDetailItemName(data.ProjectSource, "ProjectSource");
            if (projectSource != null)
            {
                data.ProjectSourceName = projectSource.F_ItemName;
            }
            //开票类型
            DataItemDetailEntity billingType = dataItemBLL.GetDetailItemName(data.BillingType, "BillingType");
            if (billingType != null)
            {
                data.BillingTypeName = billingType.F_ItemName;
            }
            //开票内容
            DataItemDetailEntity billingContent = dataItemBLL.GetDetailItemName(data.BillingContent, "BillingContent");
            if (billingContent != null)
            {
                data.BillingContentName = billingContent.F_ItemName;
            }
            //开票单位
            var billingUnit = companyIBLL.GetBillingUnitName(data.BillingUnit);
            if (billingUnit != null)
            {
                data.BillingUnitName = billingUnit.F_ShortName;
            }

            var jsonData = new
            {
                ProjectBilling = data,
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
            var ProjectBillingData = projectBillingIBLL.GetProjectBillingEntity(keyValue);
            var jsonData = new
            {
                ProjectBilling = ProjectBillingData
            };
            return Success(jsonData);
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult Zuofei(string keyValue, string strEntity)
        {
            ProjectBillingEntity entity1 = strEntity.ToObject<ProjectBillingEntity>();
            ProjectBillingEntity entity = projectBillingIBLL.GetProjectBillingEntity(keyValue);
            entity.CancelTheReason = entity1.CancelTheReason;
            entity.BillingStatus = 8;
            projectBillingIBLL.SaveEntity(keyValue, entity);
            return Success("保存成功！", "项目开票(作废)", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, keyValue);
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
            //string ProjectBillingData = projectBillingIBLL.GetEntityByProcessId( processId ).ToJson();
            var ProjectBillingData = projectBillingIBLL.GetBillingByProcessId(processId);

            //ProjectBillingVo projectBillingVo = ProjectBillingData.ToObject<ProjectBillingVo>();
            // ProjectEntity projectEntity = projectManageIBLL.GetProjectEntity(projectBillingVo.ProjectId);

           // ProjectBillingData.ProjectSource = ProjectBillingData.ProjectSource;
            //项目名称
            var name = projectManageIBLL.GetPreviewFormData(ProjectBillingData.ProjectId);
            if (name != null)
            {
                ProjectBillingData.ProjectName = name.ProjectName;
            }
            //项目来源
            DataItemDetailEntity projectSource = dataItemBLL.GetDetailItemName(ProjectBillingData.ProjectSource, "ProjectSource");
            if (projectSource != null)
            {
                ProjectBillingData.ProjectSourceName = projectSource.F_ItemName;
            }
            //开票类型
            DataItemDetailEntity billingType = dataItemBLL.GetDetailItemName(ProjectBillingData.BillingType, "BillingType");
            if (billingType != null)
            {
                ProjectBillingData.BillingTypeName = billingType.F_ItemName;
            }
            //开票内容
            DataItemDetailEntity billingContent = dataItemBLL.GetDetailItemName(ProjectBillingData.BillingContent, "BillingContent");
            if (billingContent != null)
            {
                ProjectBillingData.BillingContentName = billingContent.F_ItemName;
            }
            //开票单位
            var billingUnit = companyIBLL.GetBillingUnitName(ProjectBillingData.BillingUnit);
            if (billingUnit != null)
            {
                ProjectBillingData.BillingUnitName = billingUnit.F_ShortName;
            }
            var jsonData = new
            {
                ProjectBilling = ProjectBillingData,
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
            projectBillingIBLL.DeleteEntity(keyValue);
            return Success("删除成功！", "项目开票(删除)", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, keyValue);
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
            ProjectBillingEntity entity = strEntity.ToObject<ProjectBillingEntity>();
            projectBillingIBLL.SaveEntity(keyValue, entity);
            if (string.IsNullOrEmpty(keyValue))
            {
            }
            return Success("保存成功！", "项目开票", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, entity.ToJson());
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
            projectBillingIBLL.UpdateFlowId(keyValue, ProcessId);
            return Success("操作成功！", "项目开票(提交)", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, ProcessId);
        }
        /// <summary>
        /// 变更
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="ProcessId"></param>
        /// <returns></returns>
        public ActionResult UpdateContractStatus(string keyValue, string ProcessId)
        {
            projectBillingIBLL.UpdateContractStatus(keyValue, ProcessId);
            return Success("操作成功！", "项目开票", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, ProcessId);
        }
        #endregion

    }
}
