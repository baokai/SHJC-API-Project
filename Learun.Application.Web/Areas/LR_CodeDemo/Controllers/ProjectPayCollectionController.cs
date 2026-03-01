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
    /// 日 期：2022-03-16 17:56
    /// 描 述：项目回款管理
    /// </summary>
    public class ProjectPayCollectionController : MvcControllerBase
    {
        private ProjectPayCollectionIBLL projectPayCollectionIBLL = new ProjectPayCollectionBLL();
        private ProjectPaymentIBLL projectPaymentIBLL = new ProjectPaymentBLL();
        private DepartmentIBLL departmentIBLL = new DepartmentBLL();
        private DataItemIBLL dataItemBLL = new DataItemBLL();
        private ProjectContractBLL projectContractBLL = new ProjectContractBLL();
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
        /// <summary>
        ///预览
        /// <summary>
        /// <returns></returns>
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
            var data = projectPayCollectionIBLL.GetPageList(paginationobj, queryJson);


            var jsonData = new
            {

                rows = data,
                total = paginationobj.total,
                page = paginationobj.page,
                records = paginationobj.records,

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
            List<ProjectPayCollectionVo> listdate = new List<ProjectPayCollectionVo>();

            if (followPerson.F_MoreDepartmentId != null)
            {

                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( pc.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( pc.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = projectPayCollectionIBLL.GetPageListDepartmentId(paginationobj, queryJson, deps);
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
                records = paginationobj.records,

            };

            return Success(jsonData);
        }
        /// <summary>
        /// 多部门合计
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPageListSUMDepartmentId(string queryJson)
        {
            decimal? AmountSUM = 0;
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            List<ProjectPayCollectionVo> listdate = new List<ProjectPayCollectionVo>();

            if (followPerson.F_MoreDepartmentId != null)
            {

                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( pc.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( pc.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = projectPayCollectionIBLL.GetPageListSUMDepartmentId(queryJson, deps);
                if (data.ToList().Count > 0)
                {
                    foreach (var info in data)
                    {
                        AmountSUM = AmountSUM + info.Amount;
                        listdate.Add(info);


                    }
                }
            }

            var jsonData = new
            {
                AmountSUM = AmountSUM,

            };
            return Success(jsonData);
        }        /// <summary>
                 /// 合计
                 /// </summary>
                 /// <returns></returns>
        public ActionResult GetPageListSUM(string queryJson)
        {
            var data = projectPayCollectionIBLL.GetPageListSUM(queryJson);
            decimal? AmountSUM = 0;
            // decimal? JXAmountSUM = 0;
            // decimal? SUM = 0;
            foreach (var item in data)
            {
                AmountSUM = AmountSUM + item.Amount;
                // var payment=projectPaymentIBLL.GetProjectPaymentByprojectId(item.ProjectId);

                //var datasum = projectPayCollectionIBLL.GetPageListsum(item.ProjectId);
                //if (item.ProjectSource == "1"&& datasum.Amount != null)
                //{                                      
                //    if (item.PaymentAmount != null)
                //    {
                //        decimal? sum = datasum.Amount- item.PaymentAmount;
                //        SUM = ((decimal)0.3*sum);
                //    }
                //    else
                //    {
                //        SUM = ((decimal?)0.3 * datasum.Amount);
                //    }

                //}
                //if (item.ProjectSource == "2" && datasum.Amount != null)
                //{
                //    if (item.PaymentAmount != null)
                //    {
                //        decimal? sum = datasum.Amount - item.PaymentAmount;
                //        SUM = ((decimal)0.03 * sum);
                //    }
                //    else
                //    {
                //        SUM = ((decimal?)0.03 * datasum.Amount);
                //    }

                //}
                //JXAmountSUM = JXAmountSUM + SUM;

            }
            var jsonData = new
            {
                AmountSUM = AmountSUM,
                // JXAmountSUM = JXAmountSUM

            };
            return Success(jsonData);
        }
        /// <summary>
        /// 回款导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageListAll(string queryJson)
        {
            var data = projectPayCollectionIBLL.GetPageListSUM(queryJson);
            List<ProjectPayCollectionVo> list = new List<ProjectPayCollectionVo>();
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
        /// 多部门回款导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageListAllDepartmentId(string queryJson)
        {
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            List<ProjectPayCollectionVo> listdate = new List<ProjectPayCollectionVo>();

            if (followPerson.F_MoreDepartmentId != null)
            {

                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( pc.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( pc.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = projectPayCollectionIBLL.GetPageListSUMDepartmentId(queryJson, deps);
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
            var ProjectPayCollectionData = projectPayCollectionIBLL.GetProjectPayCollectionEntity(keyValue);
            var jsonData = new
            {
                ProjectPayCollection = ProjectPayCollectionData,
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 预览
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPreviewFormData(string keyValue)
        {
            var ProjectPayCollectionData = projectPayCollectionIBLL.GetPreviewProjectPayCollectionById(keyValue);
            var department = departmentIBLL.GetEntity(ProjectPayCollectionData.DepartmentId);
            if (department != null)
            {
                ProjectPayCollectionData.DepartmentId = department.F_FullName;
            }
            DataItemDetailEntity projectSource = dataItemBLL.GetDetailItemName(ProjectPayCollectionData.ProjectSource, "ProjectSource");
            if (projectSource != null)
            {
                ProjectPayCollectionData.ProjectSource = projectSource.F_ItemName;

            }
            var jsonData = new
            {
                ProjectPayCollection = ProjectPayCollectionData,
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
        //   [AjaxOnly]
        public ActionResult DeleteForm(string keyValue)
        {
            projectPayCollectionIBLL.DeleteEntity(keyValue);
            return Success("删除成功！", "项目回款(删除)", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, keyValue);
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
            ProjectPayCollectionEntity entity = strEntity.ToObject<ProjectPayCollectionEntity>();
            projectPayCollectionIBLL.SaveEntity(keyValue, entity);
            if (string.IsNullOrEmpty(keyValue))
            {
            }
            return Success("保存成功！", "项目回款", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, entity.ToJson());
        }
    }
    #endregion
}
