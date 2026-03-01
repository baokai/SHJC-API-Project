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

namespace Learun.Application.Web.Areas.LR_CodeDemo
{
    public class PaymentController : MvcControllerBase
    {
        private PaymentIBLL paymentIBLL = new PaymentBLL();
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
            var data = paymentIBLL.GetPageList(paginationobj, queryJson);
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
            List<PaymentVo> listdate = new List<PaymentVo>();

            if (followPerson.F_MoreDepartmentId != null)
            {

                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( a.DepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or (a.DepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = paymentIBLL.GetPageListDepartmentId(paginationobj, queryJson, deps);
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
        /// 行政付款导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageListAll(string queryJson)
        {
            string sql = "";
            var list = paymentIBLL.GetPageList(queryJson, out sql);
            //List<PaymentVo> list = new List<PaymentVo>();
            //foreach (var info in data)
            //{
            //    list.Add(info);
            //}
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
        /// 多部门行政付款导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageListAllDepartmentId(string queryJson)
        {


            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            List<PaymentVo> listdate = new List<PaymentVo>();

            if (followPerson.F_MoreDepartmentId != null)
            {

                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( a.DepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or (a.DepartmentId='" + strList[i] + "') ";
                    }

                }

                var data = paymentIBLL.GetPageListDepartmentId(queryJson, deps);
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
        /// 获取表单数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetFormData(string keyValue)
        {
            var PaymentData = paymentIBLL.GetProjectPaymentEntity(keyValue);
            var jsonData = new
            {
                Payment = PaymentData,
            };
            return Success(jsonData);
        }
        ///<summary>
        ///行政付款预览
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPriewFormData(string keyValue)
        {
            var PaymentData = paymentIBLL.GetPreviewProjectPayment(keyValue);
            //付款类型
            var payType = dataItemBLL.GetDetailItemName(PaymentData.PayType, "PaymentType");
            if (payType != null)
            {
                PaymentData.PayTypeName = payType.F_ItemName;
            }
            //支付方式
            var paymentMethod = dataItemBLL.GetDetailItemName(PaymentData.PaymentMethod, "Client_PaymentMode");
            if (paymentMethod != null)
            {
                PaymentData.PaymentMethodName = paymentMethod.F_ItemName;
            }
            //我司支付
            var paymentHeader = dataItemBLL.GetDetailItemName(PaymentData.PaymentHeader, "PaymentHeader");
            if (paymentHeader != null)
            {
                PaymentData.PaymentHeaderName = paymentHeader.F_ItemName;
            }

            var jsonData = new
            {
                Payment = PaymentData,
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
            var PaymentData = paymentIBLL.GetEntityByProcessId(processId);

            //支付方式
            var paymentMethod = dataItemBLL.GetDetailItemName(PaymentData.PaymentMethod, "Client_PaymentMode");
            if (paymentMethod != null)
            {
                PaymentData.PaymentMethodName = paymentMethod.F_ItemName;
            }
            //我司支付
            var paymentHeader = dataItemBLL.GetDetailItemName(PaymentData.PaymentHeader, "PaymentHeader");
            if (paymentHeader != null)
            {
                PaymentData.PaymentHeaderName = paymentHeader.F_ItemName;
            }
            //付款类型
            var payType = dataItemBLL.GetDetailItemName(PaymentData.PayType, "PaymentType");
            if (payType != null)
            {
                PaymentData.PayTypeName = payType.F_ItemName;
            }
            var jsonData = new
            {
                Payment = PaymentData,
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
            paymentIBLL.DeleteEntity(keyValue);
            return Success("删除成功！", "行政付款（删除）", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, keyValue);
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
            PaymentEntity entity = strEntity.ToObject<PaymentEntity>();
            entity.PaymentSubmitter = LoginUserInfo.Get().userId;
            paymentIBLL.SaveEntity(keyValue, entity);
            if (string.IsNullOrEmpty(keyValue))
            {
            }
            return Success("保存成功！", "行政付款", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, entity.ToJson());
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
            paymentIBLL.UpdateFlowId(keyValue, ProcessId);
            return Success("操作成功！", "行政付款(提交)", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, ProcessId);

        }
        /// <summary>
        /// 变更
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="ProcessId"></param>
        /// <returns></returns>
        public ActionResult UpdateFlowIdStatus(string keyValue, string ProcessId)
        {
            paymentIBLL.UpdateFlowIdStatus(keyValue, ProcessId);
            return Success("操作成功！", "行政付款(提交)", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, ProcessId);
        }
        #endregion
    }
}
