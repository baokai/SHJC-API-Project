using Learun.Application.Base.AuthorizeModule;
using Learun.Application.Base.SystemModule;
using Learun.Application.Organization;
using Learun.Application.TwoDevelopment.LR_CodeDemo;
using Learun.Application.WorkFlow;
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
    /// 日 期：2022-03-11 00:04
    /// 描 述：合同支付
    /// </summary>
    public class ProjectPaymentController : MvcControllerBase
    {
        private ProjectPaymentIBLL projectPaymentIBLL = new ProjectPaymentBLL();
        private CodeRuleIBLL codeRuleIBLL = new CodeRuleBLL();
        private ProjectManageIBLL projectManageIBLL = new ProjectManageBLL();
        private DataItemIBLL dataItemBLL = new DataItemBLL();
        private NWFProcessIBLL nWFProcessIBLL = new NWFProcessBLL();
        private UserIBLL userIBLL = new UserBLL();
        private UserRelationIBLL userRelationIBLL = new UserRelationBLL();
        private NWFTaskIBLL nWFTaskIBLL = new NWFTaskBLL();
        private ICache cache = CacheFactory.CaChe();


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
        /// 多部门主页面
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IndexDepartmentId()
        {
            return View();
        }
        [HttpGet]
        public ActionResult BatchAuditAdd()
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
        public ActionResult FormAdd()
        {
            return View();
        }
        [HttpGet]
        public ActionResult FormHZ()
        {
            return View();
        }
        public ActionResult Form1()
        {
            return View();
        }
        ///<summary>
        ///付款预览
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PreviewForm()
        {
            return View();
        }
        public ActionResult PrintForm()
        {
            return View();
        }
        public ActionResult PrintFormId()
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
            var data = projectPaymentIBLL.GetPageList(paginationobj, queryJson);
         /*   foreach(var info in data)
            {
                if (info.PaymentStatus.ToInt() == 11)
                {
                    ProjectPaymentEntity entity = new ProjectPaymentEntity();
                    entity.ProjectId = info.ProjectId;
                    entity.PaymentStatus = info.PaymentStatus.ToInt();

                    projectPaymentIBLL.SaveEntity2(info.ProjectId, entity);
                }
            }*/
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
            List<ProjectPaymentVo> listdate = new List<ProjectPaymentVo>();
            if (followPerson.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " (  a.FDepartmentId='" + strList[i] + "' or t.CreateUser='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( a.FDepartmentId='" + strList[i] + "' or t.CreateUser='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = projectPaymentIBLL.GetPageListDepartmentId(paginationobj, queryJson, deps);
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
        /// 多部门付款导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageListAllDepartmentId(string queryJson)
        {
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            List<ProjectPaymentVo> list = new List<ProjectPaymentVo>();
            if (followPerson.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " (  a.FDepartmentId='" + strList[i] + "' or t.CreateUser='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( a.FDepartmentId='" + strList[i] + "' or t.CreateUser='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = projectPaymentIBLL.GetPageListDepartmentId(queryJson, deps);

                foreach (var info in data)
                {
                    list.Add(info);
                }
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
        /// 付款导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageListAll(string queryJson)
        {

            var data = projectPaymentIBLL.GetPageList(queryJson);
            List<ProjectPaymentVo> list = new List<ProjectPaymentVo>();
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
        /// 获取表单数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetFormData(string keyValue)
        {
            var ProjectPaymentData = projectPaymentIBLL.GetProjectPaymentEntity(keyValue);

            var jsonData = new
            {
                ProjectPayment = ProjectPaymentData,
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 获取表单数据打印
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetFormDataPrintForm(string keyValue)
        {
            var ProjectPaymentData = projectPaymentIBLL.GetProjectPaymentEntity(keyValue);
            var ProjectData = projectManageIBLL.GetProjectEntity(ProjectPaymentData.ProjectId);
            if (ProjectData != null)
            {
                ProjectPaymentData.ProjectId = ProjectData.ProjectName;
            }
            var jsonData = new
            {
                ProjectPayment = ProjectPaymentData,
            };
            return Success(jsonData);
        }
        ///<summary>
        ///付款预览
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPreviewFormData(string keyValue)
        {
            var ProjectPaymentData = projectPaymentIBLL.GetPreviewProjectPayment(keyValue);

            DataItemDetailEntity payType = dataItemBLL.GetDetailItemName(ProjectPaymentData.PayType, "PayType");
            DataItemDetailEntity paymentHeader = dataItemBLL.GetDetailItemName(ProjectPaymentData.PaymentHeader, "PaymentHeader");
            DataItemDetailEntity paymentMethod = dataItemBLL.GetDetailItemName(ProjectPaymentData.PaymentMethod, "Client_PaymentMode");

            UserEntity createUser = userIBLL.GetFollowPersonNameByUserId(ProjectPaymentData.CreateUser);
            if (createUser != null)
            {
                ProjectPaymentData.CreateUserName = createUser.F_RealName;
            }
            if (payType != null)
            {
                ProjectPaymentData.PayType = payType.F_ItemName;
            }
            if (paymentHeader != null)
            {
                ProjectPaymentData.PaymentHeader = paymentHeader.F_ItemName;
            }
            if (paymentMethod != null)
            {
                ProjectPaymentData.PaymentMethod = paymentMethod.F_ItemName;
            }
            var jsonData = new
            {
                ProjectPayment = ProjectPaymentData,
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
            var ProjectPaymentData = projectPaymentIBLL.GetEntityByProcessId(processId);
            var jsonData = new
            {
                ProjectPayment = ProjectPaymentData,
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
        public ActionResult GetFormDataByProcessIdPrint(string processId)//94775c0d-4ce1-4e9a-9532-b78020aff0ee
        {
            var ProjectPaymentData = projectPaymentIBLL.GetEntityByProcessId(processId);
            if (ProjectPaymentData == null)
            {
                var processid = nWFTaskIBLL.GetEntityChildProcessId(processId);
                if (processid.F_ProcessId != null)
                {
                    ProjectPaymentData = projectPaymentIBLL.GetEntityByProcessId(processid.F_ProcessId);
                }
            }
            var payType = dataItemBLL.GetDetailItemName(ProjectPaymentData.PayType, "PayType");
            var paymentMethod = dataItemBLL.GetDetailItemName(ProjectPaymentData.PaymentMethod, "Client_PaymentMode");
            var paymentHeader = dataItemBLL.GetDetailItemName(ProjectPaymentData.PaymentHeader, "PaymentHeader");

            var createUser = userIBLL.GetFollowPersonNameByUserId(ProjectPaymentData.CreateUser);
            if (createUser != null)
            {
                ProjectPaymentData.CreateUserName = createUser.F_RealName;
            }
            if (payType != null)
            {
                ProjectPaymentData.PayTypeName = payType.F_ItemName;
            }
            if (paymentHeader != null)
            {
                ProjectPaymentData.PaymentHeaderName = paymentHeader.F_ItemName;
            }
            if (paymentMethod != null)
            {
                ProjectPaymentData.PaymentMethodName = paymentMethod.F_ItemName;
            }
            var jsonData = new
            {
                ProjectPayment = ProjectPaymentData,
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
            projectPaymentIBLL.DeleteEntity(keyValue);
            return Success("删除成功！", "项目付款(删除）", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, keyValue);
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
            ProjectPaymentEntity entity = strEntity.ToObject<ProjectPaymentEntity>();
            projectPaymentIBLL.SaveEntity(keyValue, entity);
            if (string.IsNullOrEmpty(keyValue))
            {
            }
            return Success("保存成功！", "项目付款", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, entity.ToJson());
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="Ids">主键</param>
        /// <param name="strEntity">实体</param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AjaxOnly]
        /*public ActionResult SaveFormList(string Ids, string PayType, string Payee, string PayeeBank, string BankAccount, string PaymentAmount, string PaymentMethod, string PaymentHeader, string PaymentReason, string PaymentFile)
        {*/
        public ActionResult SaveFormList(string Ids, string strEntity)
        {
            ProjectPaymentEntity entity = strEntity.ToObject<ProjectPaymentEntity>();
            projectPaymentIBLL.SaveEntityList(Ids, entity);
            //if (string.IsNullOrEmpty(Ids))
            //{
            //}
            return Success("保存成功！", "项目付款", string.IsNullOrEmpty(Ids) ? OperationType.Create : OperationType.Update, Ids, entity.ToJson());
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
            var entity = projectPaymentIBLL.GetProjectPaymentEntity(keyValue);
            if (entity.PayType.Equals("1"))
            {
                var user = LoginUserInfo.Get().userId;
                var users = userRelationIBLL.GetUserId(user);
                if (users == null)
                {
                    return Fail("你没有权限提审！");
                }
            }
            projectPaymentIBLL.UpdateFlowId(keyValue, ProcessId);
            return Success("操作成功！", "项目付款(提审)", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, ProcessId);


        }
        /// <summary>
        /// 变更
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="ProcessId"></param>
        /// <returns></returns>
        public ActionResult UpdateFlowIdStatus(string keyValue, string ProcessId)
        {
            projectPaymentIBLL.UpdateFlowIdStatus(keyValue, ProcessId);
            return Success("操作成功！", "项目付款(变更)", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, ProcessId);
        }
        #endregion

    }
}
