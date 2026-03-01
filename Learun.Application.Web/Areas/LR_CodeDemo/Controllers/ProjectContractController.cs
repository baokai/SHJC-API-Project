using Learun.Application.Base.SystemModule;
using Learun.Application.Excel;
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
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Learun.Application.Web.Areas.LR_CodeDemo.Controllers
{
    /// <summary>
    /// 日 期：2022-03-10 23:22
    /// 描 述：项目合同申请
    /// </summary>
    public class ProjectContractController : MvcControllerBase
    {
        private ProjectContractIBLL projectContractIBLL = new ProjectContractBLL();
        private CodeRuleIBLL codeRuleIBLL = new CodeRuleBLL();
        private DataItemIBLL dataItemBLL = new DataItemBLL();
        private UserIBLL userIBLL = new UserBLL();
        private DepartmentIBLL departmentIBLL = new DepartmentBLL();
        private ProjectManageIBLL projectIBLL = new ProjectManageBLL();
        private NWFTaskIBLL nWFTaskIBLL = new NWFTaskBLL();
        private AnnexesFileIBLL annexesFileIBLL = new AnnexesFileBLL();
        private ExcelImportIBLL excelImportIBLL = new ExcelImportBLL();
        private ICache cache = CacheFactory.CaChe();
        private ProjectPaymentIBLL projectPaymentIBLL = new ProjectPaymentBLL();

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
        /// <summary>
        /// 审核修改
        /// </summary>
        /// <returns></returns>
        public ActionResult EditForm()
        {
            return View();

        }
        public ActionResult EditForm1()
        {
            return View();

        }

        [HttpGet]
        public ActionResult updateForm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PreviewForm()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SendContract()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ImportForm()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ZuofeiForm()
        {
            return View();
        }
        [HttpGet]
        public ActionResult FillForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// ①公司全年项目数量、已实施数量、待实施数量/金额
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetMoneyToBeCollected1()
        {
            List<YearRoundVo> list = new List<YearRoundVo>();
            //公司全年
            var data1 = projectContractIBLL.GetyearRoundNumberOfTtems();
            //已实施数量
            var data2 = projectContractIBLL.GetyearRoundHaveBeenImplemented();
            //待实施数量/对应合同金额
            var data3 = projectContractIBLL.GetyearRoundToBeImplemented();
            DateTime StartTime = DateTime.Now;
            int data = StartTime.Year;

            for (int i = 0; i < 12; i++)
            {

                data = data - i;
                YearRoundVo tionVo = new YearRoundVo();

                foreach (var info1 in data1)
                {

                    if (info1.Years == data.ToString())
                    {
                        tionVo.Years = info1.Years;
                        tionVo.ProjectQuantity = info1.ProjectQuantity;
                    }
                }
                foreach (var info1 in data2)
                {

                    if (info1.Years == data.ToString())
                    {
                        tionVo.Years = info1.Years;
                        tionVo.HasQuantity = info1.HasQuantity;
                    }
                }
                foreach (var info1 in data3)
                {

                    if (info1.Years == data.ToString())
                    {
                        tionVo.Years = info1.Years;
                        tionVo.NotQuantity = info1.NotQuantity;
                        tionVo.NotAmount = info1.NotAmount;
                    }
                }
                if (tionVo.Years != null)
                {
                    list.Add(tionVo);
                }
            }
            return Success(list);
        }
        /// <summary>
        /// ②公司全年合同额承揽、承揽金额、开票金额
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetMoneyToBeCollected11()
        {
            List<YearRoundAmountVo> list = new List<YearRoundAmountVo>();
            var data1 = projectContractIBLL.GetyearRoundAmountOfContract();

            DateTime StartTime = DateTime.Now;
            int data = StartTime.Year;
            for (int i = 0; i < 12; i++)
            {
                data = data - i;
                YearRoundAmountVo tionVo = new YearRoundAmountVo();
                foreach (var info1 in data1)
                {
                    if (info1.Years == data.ToString())
                    {
                        tionVo.Years = info1.Years;
                        //合同承揽
                        tionVo.PromiseQuantity = info1.PromiseQuantity;
                        //承揽金额
                        tionVo.PromiseAmount = info1.PromiseAmount;
                        //开票金额
                        tionVo.BillingAmount = info1.BillingAmount;
                    }
                }
                if (tionVo.Years != null)
                {
                    list.Add(tionVo);
                }
            }
            return Success(list);
        }
        [HttpGet]
        [AjaxOnly]
        //营销统计待回款
        public ActionResult GetMoneyToBeCollected()
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
                        deps += " ( t.DepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = projectContractIBLL.GetMoneyToBeCollectedDepartmentId(deps);
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
                var data = projectContractIBLL.GetMoneyToBeCollected(followPerson.F_DepartmentId);
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
        //营销统计
        public ActionResult GetMarkeTingList()
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
                        deps += " ( DepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( DepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = projectContractIBLL.GetMarkeTingListDepartmentId(deps);
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
                var data = projectContractIBLL.GetMarkeTingList(followPerson.F_DepartmentId);
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
            var data = projectContractIBLL.GetPageList(paginationobj, queryJson);
            foreach (var ina in data)
            {
                /*   ProjectContractEntity entity = new ProjectContractEntity();
                   if (ina.ContractStatus.ToInt() == 11)
                   {
                       entity.ContractStatus = ina.ContractStatus.ToInt();
                       entity.ProjectId = ina.ProjectId;
                       projectContractIBLL.SaveEntity(ina.id, entity);
                   }*/
                if (ina.ProjectSource.ToInt() == 3)
                {
                    ProjectContractEntity entity = new ProjectContractEntity();


                    entity.EffectiveAmount = 0;
                    projectContractIBLL.SaveEntity1(ina.id, entity);
                }
                if (ina.ContractStatus.ToInt() == 11)
                {
                    ProjectContractEntity entity = new ProjectContractEntity();

               
                    entity.EffectiveAmount = 0;
                    projectContractIBLL.SaveEntity1(ina.id, entity);
                }
                /* //根据ProjectId查询付款是否有这个项目
                 if (ina.MainContract == "1" && ina.ContractType == "1" && ina.ProjectSource != "3")
                 {

                     if (ina.PaymentAmount != null)
                     {
                         entity.EffectiveAmount = ina.ContractAmount - ina.PaymentAmount;
                     }
                     else
                     {
                         entity.EffectiveAmount = ina.ContractAmount;
                     }
                     if (ina.PaymentAmountList != null)
                     {
                         entity.EffectiveAmount = entity.EffectiveAmount - ina.PaymentAmountList;
                     }

                 }
                 else
                 {
                     entity.EffectiveAmount = 0;
                 }
                 projectContractIBLL.SaveEntity(ina.id, entity);*/


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
            List<ProjectContractVo> listdate = new List<ProjectContractVo>();
            if (followPerson.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or a.DepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                        //deps += " ( t.DepartmentId='" + strList[i] + "' or a.DepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.DepartmentId='" + followPerson.F_DepartmentId + "' or a.FDepartmentId='" + followPerson.F_DepartmentId + "' or a.PDepartmentId='" + followPerson.F_DepartmentId + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or a.DepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }

                }

                //var data = projectContractIBLL.GetPageList(paginationobj, queryJson);
                var data = projectContractIBLL.GetPageListDepartmentId(paginationobj, queryJson, deps);
                if (data.ToList().Count > 0)
                {
                    foreach (var ina in data)
                    {
                        //    ProjectContractEntity entity = new ProjectContractEntity();

                        //    //根据ProjectId查询付款是否有这个项目
                        //    if (ina.MainContract == "1" && ina.ContractType == "1" && ina.ProjectSource != "3")
                        //    {

                        //        if (ina.PaymentAmount != null)
                        //        {
                        //            entity.EffectiveAmount = ina.ContractAmount - ina.PaymentAmount;
                        //        }
                        //        else
                        //        {
                        //            entity.EffectiveAmount = ina.ContractAmount;
                        //        }
                        //        if (ina.PaymentAmountList != null)
                        //        {
                        //            entity.EffectiveAmount = entity.EffectiveAmount - ina.PaymentAmountList;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        entity.EffectiveAmount = 0;
                        //    }
                        //    projectContractIBLL.SaveEntity(ina.id, entity);
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
        /// 相同合同自动归档
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetContractPageList()
        {
            ///LR_CodeDemo/ProjectContract/GetContractPageList

            var data = projectContractIBLL.GetContractPageList();
            foreach (var contract in data)
            {
                var pcontract = "";
                ProjectContractEntity entity = new ProjectContractEntity();
                if (contract.ReceivedFlagNo != null || contract.Remark != null || contract.ReceiptType != null)
                {
                    pcontract = contract.ContractNo;
                    entity.ReceiptType = contract.ReceiptType;
                    entity.ReceivedFlagNo = contract.ReceivedFlagNo;
                    entity.Remark = contract.Remark;
                }
                var cont = projectContractIBLL.GetPageListCont(pcontract);
                if (cont != null)
                {
                    foreach (var i in cont)
                    {
                        projectContractIBLL.UpdateReceivedFlag(i.id, entity);

                        codeRuleIBLL.UseRuleSeed("1");


                    }
                }
            }
            var jsonData = new
            {
                rows = data,

            };
            return Success(jsonData);
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageListAll(string queryJson)
        {

            var data = projectContractIBLL.GetPageList(queryJson);
            List<ProjectContractVo> list = new List<ProjectContractVo>();
            foreach (var info in data)
            {
                list.Add(info);
            }

            /*   List<ProjectContractVo> marketings = new List<ProjectContractVo>();*/
            /*foreach (var item in data)
            {
                if (!marketings.Contains(item))
                {
                    marketings.Add(item);
                }
            }*/
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
        /// 多部门导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageListAllDepartmentId(string queryJson)
        {
            List<ProjectContractVo> list = new List<ProjectContractVo>();
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
                        deps += " ( t.DepartmentId='" + strList[i] + "' or a.DepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or a.DepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = projectContractIBLL.GetPageListDepartmentId(queryJson, deps);
                if (data.ToList().Count > 0)
                {
                    foreach (var info in data)
                    {
                        list.Add(info);
                    }
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

        [HttpGet]
        [AjaxOnly]
        //获取统计图表
        public ActionResult GetContract(string queryJson)
        {

            var projectContractVo = projectContractIBLL.GetContract(queryJson);
            List<string> DepartmentName = new List<string>();
            List<string> ProjectCount = new List<string>();
            List<string> ContractSum = new List<string>();
            foreach (var item in projectContractVo)
            {
                var department = departmentIBLL.GetEntity(item.DepartmentId);
                if (department != null)
                {
                    item.DepartmentName = department.F_FullName;
                }
                DepartmentName.Add(item.DepartmentName);
                ProjectCount.Add(item.ProjectCount);
                ContractSum.Add(item.ContractSum);
            }
            var jsonData = new
            {
                DepartmentName = DepartmentName,
                ProjectCount = ProjectCount,
                ContractSum = ContractSum
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
            var ProjectContractData = projectContractIBLL.GetProjectContractEntity(keyValue);

            var jsonData = new
            {
                ProjectContract = ProjectContractData,
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 获取增补表单数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetFillFormData(string keyValue)
        {
            var ProjectContractData = projectContractIBLL.GetFillProjectContractEntity(keyValue);

            var jsonData = new
            {
                ProjectContract = ProjectContractData,
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
            var ProjectContractData = projectContractIBLL.GetPreviewFormData(keyValue);
            //编号
            ProjectContractData.ContractNoNane = ProjectContractData.ContractNo;
            //项目来源
            DataItemDetailEntity projectSource = dataItemBLL.GetDetailItemName(ProjectContractData.ProjectSource, "ProjectSource");
            if (projectSource != null)
            {
                ProjectContractData.ProjectSourceName = projectSource.F_ItemName;

            }
            //合同主体
            DataItemDetailEntity contractSubject = dataItemBLL.GetDetailItemName(ProjectContractData.ContractSubject, "ContractSubject");
            if (contractSubject != null)
            {
                ProjectContractData.ContractSubjectName = contractSubject.F_ItemName;
            }
            //合同分类
            DataItemDetailEntity contractType = dataItemBLL.GetDetailItemName(ProjectContractData.ContractType, "ContractType");

            if (contractType != null)
            {
                ProjectContractData.ContractTypeName = contractType.F_ItemName;
            }
            //营销人员
            UserEntity followPerson = userIBLL.GetFollowPersonNameByUserId(ProjectContractData.FollowPerson);
            if (followPerson != null)
            {
                ProjectContractData.FollowPersonName = followPerson.F_RealName;
            }
            //销售部门
            var department = departmentIBLL.GetEntity(ProjectContractData.DepartmentId);
            if (department != null)
            {
                ProjectContractData.DepartmentName = department.F_FullName;
            }


            var jsonData = new
            {
                ProjectContract = ProjectContractData,
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
            var ProjectContractData = projectContractIBLL.GetEntityByProcessId(processId);
            //项目名称
            ProjectContractData.ProjectName = ProjectContractData.ProjectName;
            //编号
            ProjectContractData.ContractNoNane = ProjectContractData.ContractNo;
            //项目来源
            DataItemDetailEntity projectSource = dataItemBLL.GetDetailItemName(ProjectContractData.ProjectSource, "ProjectSource");
            if (projectSource != null)
            {
                ProjectContractData.ProjectSourceName = projectSource.F_ItemName;

            }
            //合同主体
            DataItemDetailEntity contractSubject = dataItemBLL.GetDetailItemName(ProjectContractData.ContractSubject, "ContractSubject");
            if (contractSubject != null)
            {
                ProjectContractData.ContractSubjectName = contractSubject.F_ItemName;
            }
            //合同分类
            DataItemDetailEntity contractType = dataItemBLL.GetDetailItemName(ProjectContractData.ContractType, "ContractType");

            if (contractType != null)
            {
                ProjectContractData.ContractTypeName = contractType.F_ItemName;
            }
            //营销人员
            UserEntity followPerson = userIBLL.GetFollowPersonNameByUserId(ProjectContractData.FollowPerson);
            if (followPerson != null)
            {
                ProjectContractData.FollowPersonName = followPerson.F_RealName;
            }
            //销售部门
            var department = departmentIBLL.GetEntity(ProjectContractData.DepartmentId);
            if (department != null)
            {
                ProjectContractData.DepartmentName = department.F_FullName;
            }
            var jsonData = new
            {
                ProjectContract = ProjectContractData,
            };
            return Success(jsonData);
        }
        public ActionResult ProjectTaskByContract(string keyValue)
        {
            var ProjectContractData = projectContractIBLL.ProjectTaskByContract(keyValue);
            //项目来源
            DataItemDetailEntity projectSource = dataItemBLL.GetDetailItemName(ProjectContractData.ProjectSource, "ProjectSource");
            if (projectSource != null)
            {
                ProjectContractData.ProjectSourceName = projectSource.F_ItemName;

            }
            //合同主体
            DataItemDetailEntity contractSubject = dataItemBLL.GetDetailItemName(ProjectContractData.ContractSubject, "ContractSubject");
            if (contractSubject != null)
            {
                ProjectContractData.ContractSubjectName = contractSubject.F_ItemName;
            }
            //合同分类
            DataItemDetailEntity contractType = dataItemBLL.GetDetailItemName(ProjectContractData.ContractType, "ContractType");

            if (contractType != null)
            {
                ProjectContractData.ContractTypeName = contractType.F_ItemName;
            }
            //营销人员
            UserEntity followPerson = userIBLL.GetFollowPersonNameByUserId(ProjectContractData.FollowPerson);
            if (followPerson != null)
            {
                ProjectContractData.FollowPersonName = followPerson.F_RealName;
            }
            //销售部门
            var department = departmentIBLL.GetEntity(ProjectContractData.DepartmentId);
            if (department != null)
            {
                ProjectContractData.DepartmentName = department.F_FullName;
            }
            var jsonData = new
            {
                ProjectContract = ProjectContractData,
            };
            return Success(jsonData);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// excel文件导入（通用）
        /// </summary>
        /// <param name="templateId">模板Id</param>
        /// <param name="fileId">文件主键</param>
        /// <param name="chunks">分片数</param>
        /// <param name="ext">文件扩展名</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExecuteImportExcel(string templateId, string fileId, int chunks, string ext)
        {
            UserInfo userInfo = LoginUserInfo.Get();
            string path = annexesFileIBLL.SaveAnnexes(fileId, fileId + "." + ext, chunks, userInfo);
            string RedoData = "";
            string nolist = "";
            if (!string.IsNullOrEmpty(path))
            {
                DataTable dt = ExcelHelper.ExcelImport(path);
                foreach (DataRow row in dt.Rows)
                {
                    string no = row.ItemArray[2].ToString();
                    List<ProjectContractEntity> projectContract = (List<ProjectContractEntity>)projectContractIBLL.GetProjectContractEntityBycNo(no);
                    if (projectContract.Count >= 2)
                    {
                        RedoData = RedoData + "," + no;
                        /*ProjectContractEntity projectContractEntity = new ProjectContractEntity();
                        projectContractEntity.CreateTime = DateTime.Parse(row.ItemArray[1].ToString().Replace(".", "-"));
                        projectContractEntity.ContractStatus = 4;
                        projectContractEntity.ReceivedFlag = ("已返".Equals(row.ItemArray[8].ToString())) ? 1 : ("未返".Equals(row.ItemArray[8].ToString())) ? 2 : 0;
                        projectContractIBLL.SaveEntity(projectContract[0].id, projectContractEntity);*/
                    }
                    else if (projectContract.Count == 1 && projectContract[0].ContractAmount == decimal.Parse(row.ItemArray[3].ToString()))
                    {
                        ProjectContractEntity projectContractEntity = new ProjectContractEntity();
                        projectContractEntity.CreateTime = DateTime.Parse(row.ItemArray[1].ToString().Replace(".", "-"));
                        projectContractEntity.ContractStatus = 4;
                        projectContractEntity.ReceivedFlag = ("已返".Equals(row.ItemArray[4].ToString())) ? 1 : ("未返".Equals(row.ItemArray[4].ToString())) ? 2 : 0;
                        projectContractIBLL.SaveEntity(projectContract[0].id, projectContractEntity);
                    }
                    else
                    {
                        nolist = nolist + "," + no;
                    }
                }
                //List<ProjectContractEntity> entity = TableToEntity<ProjectContractEntity>(dt);
                /* string res = excelImportIBLL.ImportTable(templateId, fileId, dt);*/
                return Success("成功");

            }
            else
            {
                return Fail("导入数据失败!");
            }
        }

        public static List<T> TableToEntity<T>(DataTable dt) where T : class, new()
        {
            // 定义集合  
            List<T> ts = new List<T>();

            if (dt != null && dt.Rows.Count > 0)
            {
                // 获得此模型的类型  
                Type type = typeof(T);
                string tempName = "";
                foreach (DataRow dr in dt.Rows)
                {
                    T t = new T();
                    // 获得此模型的公共属性  
                    PropertyInfo[] propertys = t.GetType().GetProperties();
                    foreach (PropertyInfo pi in propertys)
                    {
                        tempName = pi.Name;
                        // 检查DataTable是否包含此列  
                        if (dt.Columns.Contains(tempName))
                        {
                            // 判断此属性是否有Setter  
                            if (!pi.CanWrite)
                                continue;
                            object value = dr[tempName];
                            if (value != DBNull.Value)
                            {
                                //pi.SetValue(t, value, null);  
                                // pi.SetValue(t, Convert.ChangeType(value, pi.PropertyType, CultureInfo.CurrentCulture), null);
                                pi.SetValue(t, ChanageType(value, pi.PropertyType), null);
                            }
                        }
                    }
                    ts.Add(t);
                }
            }

            return ts;
        }
        //转换可空类型 如：DateTime? 
        private static object ChanageType(object value, Type convertsionType)
        {
            //判断convertsionType类型是否为泛型，因为nullable是泛型类,
            if (convertsionType.IsGenericType &&
                //判断convertsionType是否为nullable泛型类
                convertsionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null || value.ToString().Length == 0)
                {
                    return null;
                }

                //如果convertsionType为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                NullableConverter nullableConverter = new NullableConverter(convertsionType);
                //将convertsionType转换为nullable对的基础基元类型
                convertsionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, convertsionType);
        }

        /// <summary>
        /// 删除实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult DeleteForm(string keyValue)
        {
            projectContractIBLL.DeleteEntity(keyValue);
           // var ct = projectContractIBLL.GetPageListEffectiveAmountProjectId(ina.ProjectId);
           
            return Success("删除成功！", "项目合同（删除）", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, keyValue);
        }
        /// <summary>
        /// 保存实体数据（增补合同）
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="strEntity">实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult FillSaveForm(string keyValue, int autoFlag, string strEntity)
        {
            ProjectContractEntity keyValueentity = projectContractIBLL.GetProjectContractEntity(keyValue);
            ProjectContractEntity entity = strEntity.ToObject<ProjectContractEntity>();
            entity.Remark = keyValueentity.Remark;
            entity.ReceiptType = keyValueentity.ReceiptType;
            ////设置报告提交人
            //entity.ContractSubmitter = LoginUserInfo.Get().userId;
            ////获取部门code
            //var department = departmentIBLL.GetEntity(LoginUserInfo.Get().departmentId);
            //if (department != null)
            //{
            //    entity.ContractSubmitterDeptCode = department.F_EnCode;
            //}
            ProjectEntity projectEntity = strEntity.ToObject<ProjectEntity>();
            //projectIBLL.SaveEntity(entity.ProjectId, projectEntity);


            projectContractIBLL.FillSaveForm(entity);
            if (autoFlag == 1)
            {
                if (entity.ContractType == 1)
                {
                    codeRuleIBLL.UseRuleSeed("10010");
                }
                else
                {
                    codeRuleIBLL.UseRuleSeed("10011");
                }

            }

            return Success("保存成功！", "项目合同", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, entity.ToJson());

            //return Success("保存成功！");
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
        public ActionResult SaveForm(string keyValue, int autoFlag, string strEntity)
        {
            ProjectContractEntity entity = strEntity.ToObject<ProjectContractEntity>();
            ////设置报告提交人
            //entity.ContractSubmitter = LoginUserInfo.Get().userId;
            ////获取部门code
            //var department = departmentIBLL.GetEntity(LoginUserInfo.Get().departmentId);
            //if (department != null)
            //{
            //    entity.ContractSubmitterDeptCode = department.F_EnCode;
            //}

            if (entity.DepartmentId == null)
            {
                var user = LoginUserInfo.Get().userId;
                var followPerson = userIBLL.GetHZUserId(LoginUserInfo.Get().userId);
                if (followPerson != null)
                {
                    user = followPerson.F_HZ;
                    if (user == "1" && user.Equals("1"))
                    {
                        entity.DepartmentId = LoginUserInfo.Get().departmentId;
                    }
                }
            }
            ProjectEntity projectEntity = strEntity.ToObject<ProjectEntity>();
            //projectIBLL.SaveEntity(entity.ProjectId,projectEntity);
            //根据ProjectId查询付款是否有这个项目


            // projectContractIBLL.SaveEntity(keyValue, entity);
            //if (keyValue != null && entity.ContractType == 1)
            //{
            //    var ct = projectContractIBLL.GetPageListEffectiveAmountProjectId(entity.ProjectId);
            //    if (ct != null)
            //    {
            //        if (ct.PaymentAmount != null && ct.ContractAmount != null)
            //        {
            //            entity.EffectiveAmount = ct.ContractAmount - ct.PaymentAmount;
            //        }
            //        else
            //        {
            //            entity.EffectiveAmount = ct.ContractAmount;
            //        }
            //    }

            //}
            //else
            //{
            //    entity.EffectiveAmount = 0;
            //}
            projectContractIBLL.SaveEntity(keyValue, entity);





            if (autoFlag == 1)
            {
                if (entity.ContractType == 1)
                {
                    codeRuleIBLL.UseRuleSeed("10010");
                }
                else
                {
                    codeRuleIBLL.UseRuleSeed("10011");
                }

            }
            if (!string.IsNullOrEmpty(keyValue))
            {
                return Success("保存成功！", "项目合同", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, entity.ToJson());
            }
            else
            {
                return Success("保存成功！", "项目合同", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, entity.ToJson());
            }
            //return Success("保存成功！");
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult Zuofei(string keyValue, string strEntity)
        {
            ProjectContractEntity entity1 = strEntity.ToObject<ProjectContractEntity>();
            ProjectContractEntity entity = projectContractIBLL.GetProjectContractEntity(keyValue);
            entity.CancelTheReason = entity1.CancelTheReason;
            entity.ContractStatus = 7;
            projectContractIBLL.SaveEntity(keyValue, entity);
            return Success("保存成功！", "项目合同(取消)", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, keyValue);
        }



        /// <summary>
        /// 归档
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult UpdateReceivedFlag(string keyValue, int autoFlag, string strEntity)
        {
            /* ProjectContractEntity contractNoentity= projectContractIBLL.GetProjectContractEntity(keyValue);
             */

            ProjectContractEntity entity = strEntity.ToObject<ProjectContractEntity>();
            //根据合同编号查询
            var cont = projectContractIBLL.GetPageListCont(entity.ContractNo);
            if (cont != null)
            {
                foreach (var i in cont)
                {
                    projectContractIBLL.UpdateReceivedFlag(i.id, entity);
                    if (autoFlag == 1)
                    {
                        codeRuleIBLL.UseRuleSeed("1");

                    }
                }
            }
            /* for(var i=0;;i++)

             projectContractIBLL.UpdateReceivedFlag(keyValue, entity);*/

            return Success("保存成功！", "项目合同归档", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, entity.ToJson());
            //return Success("操作成功！");
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

            lock (keyValue)
            {
                if ("".Equals(ProcessId) || ProcessId == null)
                {
                    projectContractIBLL.UpdateFlowId(keyValue, ProcessId);
                    return Success("保存成功！", "项目合同(提交)", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, ProcessId);
                }
                else
                {
                    var task = nWFTaskIBLL.GetEntityForProcessId(ProcessId);
                    if (task != null)
                    {
                        projectContractIBLL.UpdateFlowIdTo(keyValue, ProcessId);
                        return Success("已提交！");
                    }
                    else
                    {
                        projectContractIBLL.UpdateFlowId(keyValue, ProcessId);
                        //return Success("操作成功！");
                        return Success("保存成功！", "项目合同(提交)", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, ProcessId);
                    }
                }

            }

        }
        /// <summary>
        /// 更改
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="ProcessId"></param>
        /// <returns></returns>
        public ActionResult UpdateContractStatus(string keyValue, string ProcessId)
        {
            projectContractIBLL.UpdateContractStatus(keyValue, ProcessId);
            return Success("保存成功！", "项目合同", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, ProcessId);
        }
        /// <summary>
        ///有效合同额刷新
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="ProcessId"></param>
        /// <returns></returns>
        public ActionResult UpdateEffectiveAmount()
        {
            //查所有的合同
            var data = projectContractIBLL.GetPageListEffectiveAmount();
            foreach (var ina in data)
            {
                ProjectContractEntity entity = new ProjectContractEntity();

                //根据ProjectId查询付款是否有这个项目
                if (ina.MainContract == "1" && ina.ContractType == "1")
                {
                    var ct = projectContractIBLL.GetPageListEffectiveAmountProjectId(ina.ProjectId);
                    var fb = projectContractIBLL.GetPageListContractAmountfb2(ina.ProjectId,ina.id);
                    if (ct != null)
                    {
                        if (fb.ContractAmount != null)
                        {
                            if (ct.PaymentAmount != null && ct.ContractAmount != null)
                            {
                                entity.EffectiveAmount = ct.ContractAmount - fb.ContractAmount + entity.ContractAmount - ct.PaymentAmount;
                            }
                            if (ct.PaymentAmount != null && ct.ContractAmount == null)
                            {
                                entity.EffectiveAmount = 0 - fb.ContractAmount - ct.PaymentAmount;
                            }
                            if (ct.PaymentAmount == null && ct.ContractAmount != null)
                            {
                                entity.EffectiveAmount = ct.ContractAmount - fb.ContractAmount + entity.ContractAmount;
                            }
                            if (ct.PaymentAmount == null && ct.ContractAmount == null)
                            {
                                entity.EffectiveAmount = 0 - fb.ContractAmount;
                            }
                        }
                        else
                        {
                            if (ct.PaymentAmount != null && ct.ContractAmount != null)
                            {
                                entity.EffectiveAmount = ct.ContractAmount + entity.ContractAmount - ct.PaymentAmount;
                            }
                            if (ct.PaymentAmount == null && ct.ContractAmount != null)
                            {
                                entity.EffectiveAmount = ct.ContractAmount + entity.ContractAmount;
                            }
                            if (ct.PaymentAmount != null && ct.ContractAmount == null)
                            {
                                entity.EffectiveAmount = entity.ContractAmount - ct.PaymentAmount;
                            }
                            if (ct.PaymentAmount == null && ct.ContractAmount == null)
                            {
                                entity.EffectiveAmount = 0;
                            }
                        }
                    }

                }
                else
                {
                    entity.EffectiveAmount = 0;
                }

                projectContractIBLL.SaveEntity1(ina.id, entity);
            }
            var jsonData = new
            {
                ProjectContract = data,
            };
            return Success(jsonData);
        }

        public ActionResult Update(string keyValue, int autoFlag, string strEntity)
        {
            projectContractIBLL.update(keyValue, strEntity);
            return Success("保存成功！", "项目合同", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, strEntity);
        }

        #endregion

    }
}
