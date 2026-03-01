using Learun.Application.Base.AuthorizeModule;
using Learun.Application.Base.SystemModule;
using Learun.Application.Organization;
using Learun.Application.TwoDevelopment.LR_CodeDemo;
using Learun.Application.TwoDevelopment.LR_CodeDemo.ReportForms;
using Learun.Application.WorkFlow;
using Learun.Cache.Base;
using Learun.Cache.Factory;
using Learun.Util;
using Learun.Util.Operat;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public class ProjectPaymentListController : MvcControllerBase
    {
        private ReportFormsIBLL reportFormsBLL = new ReportFormsBLL();
        private ProjectPaymentListIBLL projectPaymentListIBLL = new ProjectPaymentListBLL();
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
            return View();
        }
        [HttpGet]
        public ActionResult BatchAuditAdd()
        {
            return View();
        }
        [HttpGet]
        public ActionResult IndexAdd()
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
        public ActionResult Form()
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
            List<ProjectPaymentVo> list = new List<ProjectPaymentVo>();
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = projectPaymentListIBLL.GetPageList(paginationobj, queryJson);
          /* 
            if (data != null)
            {
                foreach (var info in data)
                {



                    if (info.ProjectId != null)
                    {
                        list.Add(info);
                    }
                    ProjectPaymentVo t = new ProjectPaymentVo();
                    t.WorkFlowId = info.WorkFlowId;
                    t.tid = info.tid;

                    if (info.WorkFlowId != null)
                    {
                        var ProjectPaymentData = projectPaymentListIBLL.GetEntityByProcessId(info.WorkFlowId);
                        if (ProjectPaymentData.tid != null)
                        {
                            var DataT = projectPaymentListIBLL.GetEntityBytID(ProjectPaymentData.tid);
                            foreach (var f in DataT)
                            {
                                ProjectPaymentEntity list1 = new ProjectPaymentEntity();
                                list1.Modify(f.id);
                                list1.PaymentStatus = ProjectPaymentData.PaymentStatus.ToInt();
                                projectPaymentListIBLL.SaveEntity(f.id, list1);
                            }
                        }
                    }

                }

            }

           */
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
        /// 付款导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageListAll(string queryJson)
        {

            var data = projectPaymentListIBLL.GetPageList(queryJson);
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
            var ProjectPaymentData = projectPaymentListIBLL.GetPreviewProjectPayment(keyValue);
            // var ProjectPaymentData = projectPaymentListIBLL.GetProjectPaymentEntity(keyValue);
            string projectId = "";
            if (ProjectPaymentData.ProjectId != null)
            {
                string[] strList = ProjectPaymentData.ProjectId.Split(',');


                for (var i = 0; i < strList.Length; i++)
                {
                    var Responsible = projectManageIBLL.GetProjectEntity(strList[i]);
                    //var Responsible = userIBLL.GetFollowPersonNameByUserId(strList[i]);
                    if (Responsible != null)
                    {
                        if (string.IsNullOrWhiteSpace(projectId))
                        {
                            projectId = Responsible.ProjectName;
                        }
                        else
                        {
                            projectId = projectId + "。" + Responsible.ProjectName;
                        }
                    }
                }
            }
            ProjectPaymentData.ProjectName = projectId;
            var jsonData = new
            {
                ProjectPayment = ProjectPaymentData,
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
        public ActionResult GetPageListById(string pagination, string queryJson)
        {
            // Pagination paginationobj = pagination.ToObject<Pagination>();
            ProjectVo q = queryJson.ToObject<ProjectVo>();
            List<BatchAuditAddModel> list = new List<BatchAuditAddModel>();
            if (q.Id != null)
            {

                string[] strList = q.Id.Split(',');


                for (var i = 0; i < strList.Length; i++)
                {
                    var Responsible = projectManageIBLL.GetProjectEntity(strList[i]);
                    BatchAuditAddModel ProjectPaymentData = new BatchAuditAddModel();
                    if (Responsible != null)
                    {

                        ProjectPaymentData.ProjectName = Responsible.ProjectName;
                        ProjectPaymentData.ProjectId = Responsible.Id;

                    }
                    list.Add(ProjectPaymentData);

                }

            }

            var jsonData = new
            {
                rows = list,
                total = 1,
                page = 1,
                records = list.Count
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
            var ProjectPaymentData = projectPaymentListIBLL.GetProjectPaymentEntity(keyValue);
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
            var ProjectPaymentData = projectPaymentListIBLL.GetPreviewProjectPayment(keyValue);
            string projectId = "";
            if (ProjectPaymentData.ProjectId != null)
            {
                string[] strList = ProjectPaymentData.ProjectId.Split(',');


                for (var i = 0; i < strList.Length; i++)
                {
                    var Responsible = projectManageIBLL.GetProjectEntity(strList[i]);
                    //var Responsible = userIBLL.GetFollowPersonNameByUserId(strList[i]);
                    if (Responsible != null)
                    {
                        if (string.IsNullOrWhiteSpace(projectId))
                        {
                            projectId = Responsible.ProjectName;
                        }
                        else
                        {
                            projectId = projectId + "。" + Responsible.ProjectName;
                        }
                    }
                }
            }
            ProjectPaymentData.ProjectName = projectId;
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
            var ProjectPaymentData = projectPaymentListIBLL.GetEntityByProcessId(processId);

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
            var ProjectPaymentData = projectPaymentListIBLL.GetEntityByProcessId(processId);
            if (ProjectPaymentData == null)
            {
                var processid = nWFTaskIBLL.GetEntityChildProcessId(processId);
                if (processid.F_ProcessId != null)
                {
                    ProjectPaymentData = projectPaymentListIBLL.GetEntityByProcessId(processid.F_ProcessId);
                }
            }
            string projectId = "";
            if (ProjectPaymentData.ProjectId != null)
            {
                string[] strList = ProjectPaymentData.ProjectId.Split(',');


                for (var i = 0; i < strList.Length; i++)
                {
                    var Responsible = projectManageIBLL.GetProjectEntity(strList[i]);
                    //var Responsible = userIBLL.GetFollowPersonNameByUserId(strList[i]);
                    if (Responsible != null)
                    {
                        if (string.IsNullOrWhiteSpace(projectId))
                        {
                            projectId = Responsible.ProjectName;
                        }
                        else
                        {
                            projectId = projectId + "。" + Responsible.ProjectName;
                        }
                    }
                }
            }
            ProjectPaymentData.ProjectName = projectId;
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
            projectPaymentListIBLL.DeleteEntity(keyValue);
            return Success("删除成功！", "项目付款(删除）", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, keyValue);
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
        public ActionResult SaveFormList(string Ids, string strEntity, string itemList)
        {
            ProjectPaymentEntity entity = strEntity.ToObject<ProjectPaymentEntity>();
            List<BatchAuditAddModel> item_list = itemList.ToObject<List<BatchAuditAddModel>>();        
            projectPaymentListIBLL.SaveEntityList(Ids, entity, item_list);         
            return Success("保存成功！", "项目付款", string.IsNullOrEmpty(Ids) ? OperationType.Create : OperationType.Update, Ids, entity.ToJson());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]

        public ActionResult SaveForm(string Ids, string strEntity)
        {
            ProjectPaymentEntity entity = strEntity.ToObject<ProjectPaymentEntity>();

            projectPaymentListIBLL.SaveEntity(Ids, entity);
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
            var entity = projectPaymentListIBLL.GetProjectPaymentEntity(keyValue);
            if (entity.PayType.Equals("1"))
            {
                var user = LoginUserInfo.Get().userId;
                var users = userRelationIBLL.GetUserId(user);
                if (users == null)
                {
                    return Fail("你没有权限提审！");
                }
            }
            projectPaymentListIBLL.UpdateFlowId(keyValue, ProcessId);
            return Success("操作成功！", "项目付款(提审)", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, ProcessId);


        }

        #endregion

    }
}
