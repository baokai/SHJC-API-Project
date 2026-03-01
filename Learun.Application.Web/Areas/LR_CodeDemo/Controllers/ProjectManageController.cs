using Learun.Application.Base.AuthorizeModule;
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
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Learun.Application.Web.Areas.LR_CodeDemo.Controllers
{
    public class ProjectManageController : MvcControllerBase
    {
        private ProjectContractIBLL projectContractIBLL = new ProjectContractBLL();
        private ProjectManageIBLL projectManageIBLL = new ProjectManageBLL();
        private ProjectContractBLL projectContractBLL = new ProjectContractBLL();
        private CodeRuleIBLL codeRuleIBLL = new CodeRuleBLL();
        private UserIBLL userIBLL = new UserBLL();
        private DataItemIBLL dataItemBLL = new DataItemBLL();
        private AreaIBLL areaIBLL = new AreaBLL();
        private ICache cache = CacheFactory.CaChe();
        private DepartmentIBLL departmentIBLL = new DepartmentBLL();
        private ProjectTaskIBLL projectTaskIBLL = new ProjectTaskBLL();
        private ProjectPayCollectionIBLL projectPayCollectionIBLL = new ProjectPayCollectionBLL();
        private ProjectBillingIBLL projectBillingIBLL = new ProjectBillingBLL();
        private ProjectPaymentIBLL projectPaymentIBLL = new ProjectPaymentBLL();
        private UserRelationBLL userRelationIBLL = new UserRelationBLL();

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
        /// 详情
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PreviewFormById()
        {
            return View();
        }
        public ActionResult SelectProjectForm()
        {
            return View();
        }
        public ActionResult SelectProjectFormT()
        {
            return View();
        }
        public ActionResult SelectProjectFormPT()
        {
            return View();
        }
        public ActionResult PreviewForm()
        {
            return View();
        }
        public ActionResult PreviewIndex()
        {
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            if (followPerson.F_MoreDepartmentId != null)
            {
                return View("PreviewIndexDepartmentId");
            }
            return View();
        }
        public ActionResult PreviewIndexDepartmentId()
        {
            return View();

        }
        public ActionResult PreviewIndexFrom()
        {
            return View();

        }
        /// <summary>
        /// 开票添加
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectProjectFormTBilling()
        {
            return View();
        }
        public ActionResult SelectProjectFormTBillingDepartmentId()
        {
            return View();
        }
        /// <summary>
        /// 报备查看
        /// </summary>
        /// <returns></returns>
        public ActionResult Toview()
        {
            return View();
        }
        #endregion

        #region 获取数据

        [HttpGet]
        [AjaxOnly]
        public ActionResult ToviewList(string id)
        {
            ProjectVo lv = new ProjectVo();
            List<ProjectContractVo> lclist = new List<ProjectContractVo>();
            List<ProjectTaskVo> ltlist = new List<ProjectTaskVo>();
            List<ProjectPayCollectionVo> lpclist = new List<ProjectPayCollectionVo>();
            List<ProjectBillingVo> lblist = new List<ProjectBillingVo>();
            List<ProjectPaymentVo> lplist = new List<ProjectPaymentVo>();
            //报备查询
            var dataProject = projectManageIBLL.GetToviewListProject(id);
            var dataContract = projectContractIBLL.GetProjectContract(dataProject.Id);
            var dataTask = projectTaskIBLL.GetProjectTaskList(dataProject.Id);
            var dataPayCollection = projectPayCollectionIBLL.GetPayCollection(dataProject.Id);
            var dataBilling = projectBillingIBLL.GetBilling(dataProject.Id);
            var dataPayment = projectPaymentIBLL.GetPayment(dataProject.Id);

            if (dataProject != null || dataContract != null || dataTask != null || dataPayCollection != null || dataBilling != null || dataPayment != null)
            {
                //项目信息
                //项目名称
                lv.ProjectName = dataProject.ProjectName;
                //委托单位
                lv.CustName = dataProject.CustName;
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(dataProject.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    lv.ProjectSource = projectSource.F_ItemName;
                }
                //营销部门
                var department = departmentIBLL.GetEntity(dataProject.DepartmentId);
                if (department != null)
                {
                    lv.DepartmentId = department.F_FullName;
                }
                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(dataProject.FollowPerson);
                if (followPerson != null)
                {
                    lv.FollowPerson = followPerson.F_RealName;
                }
                //合同信息
                foreach (var cinfo in dataContract)
                {
                    ProjectContractVo lc = new ProjectContractVo();
                    if (cinfo.MasterContract == "0")
                    {
                        lc.MasterContract = "增补合同";
                    }
                    else
                    {
                        lc.MasterContract = "主合同";
                    }
                    //合同分类
                    var contractType = dataItemBLL.GetDetailItemName(cinfo.ContractType, "ContractType");
                    if (contractType != null)
                    {
                        lc.ContractType = contractType.F_ItemName;
                    }
                    //合同编号
                    lc.ContractNo = cinfo.ContractNo;
                    //合同主体
                    var contractSubject = dataItemBLL.GetDetailItemName(cinfo.ContractSubject, "ContractSubject");
                    if (contractSubject != null)
                    {
                        lc.ContractSubject = contractSubject.F_ItemName;
                    }
                    //合同金额
                    lc.ContractAmount = cinfo.ContractAmount;
                    //备注
                    lc.ContractRemark = cinfo.ContractRemark;
                    lclist.Add(lc);
                }
                //任务信息
                foreach (var tinfo in dataTask)
                {
                    ProjectTaskVo lt = new ProjectTaskVo();
                    //现场负责人
                    lt.SiteContact = tinfo.SiteContact;
                    //现场联系电话
                    lt.SitePhone = tinfo.SitePhone;
                    //项目负责人
                    if (tinfo.ProjectResponsible != null)
                    {
                        string[] strList = tinfo.ProjectResponsible.Split(',');
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
                        lt.ProjectResponsible = projectResponsible;
                    }
                    //检测员
                    if (tinfo.Inspector != null)
                    {
                        string[] strList = tinfo.Inspector.Split(',');
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
                        lt.Inspector = inspectorName;
                    }
                    //备注
                    lt.TaskRemark = tinfo.Remark;
                    //进场时间
                    lt.ApproachTime = tinfo.ApproachTime;
                    //报告计划时间
                    lt.PlanTime = tinfo.PlanTime;
                    //检测内容
                    lt.TestContent = tinfo.TestContent;
                    //真实检测目的
                    lt.TestTarget = tinfo.TestTarget;
                    //当前状态
                    lt.TaskStatus = tinfo.TaskStatus;
                    if (lt != null)
                    {
                        ltlist.Add(lt);
                    }
                }
                //回款信息
                foreach (var cyinfo in dataPayCollection)
                {
                    ProjectPayCollectionVo lpc = new ProjectPayCollectionVo();
                    //回款时间
                    lpc.ReceiptDate = cyinfo.ReceiptDate;
                    //回款金额
                    lpc.Amount = cyinfo.Amount;
                    if (lpc != null)
                    {
                        lpclist.Add(lpc);
                    }
                }
                //发票信息
                foreach (var pbinfo in dataBilling)
                {
                    ProjectBillingVo lb = new ProjectBillingVo();
                    //创建时间
                    lb.CreateTime = pbinfo.CreateTime;
                    //开票金额
                    lb.BillingAmount = pbinfo.BillingAmount;
                    lblist.Add(lb);
                }
                //付款信息
                foreach (var pinfo in dataPayment)
                {
                    ProjectPaymentVo lp = new ProjectPaymentVo();
                    //创建时间
                    lp.CreateTime = pinfo.CreateTime;
                    //支付金额
                    lp.PaymentAmount = pinfo.PaymentAmount;
                    if (lp != null)
                    {
                        lplist.Add(lp);
                    }
                }

            }
            var jsonData = new
            {
                rows = lv,
                Contract = lclist,
                Task = ltlist,
                PayCollection = lpclist,
                Billing = lblist,
                Payment = lplist
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
            var data = projectManageIBLL.GetPageListAddress(paginationobj, queryJson);

            //foreach (var info in data)
            //{

            //    var tenderFlg = dataItemBLL.GetDetailItemName(info.TenderFlg, "TenderFlg");
            //    var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
            //    if (tenderFlg != null)
            //    {
            //        info.TenderFlg = tenderFlg.F_ItemName;
            //    }
            //    if (projectSource != null)
            //    {
            //        info.ProjectSource = projectSource.F_ItemName;
            //    }

            //}
            //  data = data.OrderBy(t => t.CreateTime).ToList();
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
            List<ProjectVo> listdate = new List<ProjectVo>();

            if (followPerson.F_MoreDepartmentId != null)
            {

                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or t.FDepartmentId='" + strList[i] + "' or t.PDepartmentId='" + strList[i] + "') ";
                        //deps += " ( t.DepartmentId='" + strList[i] + "' or t.FDepartmentId='" + strList[i] + "' or t.PDepartmentId='" + strList[i] + "' or t.DepartmentId='" + followPerson.F_DepartmentId + "' or t.FDepartmentId='" + followPerson.F_DepartmentId + "' or t.PDepartmentId='" + followPerson.F_DepartmentId + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or t.FDepartmentId='" + strList[i] + "' or t.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = projectManageIBLL.GetPageListAddressDepartmentIds(paginationobj, queryJson, deps);
                if (data.ToList().Count > 0)
                {
                    listdate = data.ToList();
                }
            }
            // listdate = listdate.OrderBy(t => t.CreateTime).ThenByDescending(t => t.Id).ToList();

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
        /// 获取页面导出显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageListAll(string queryJson)
        {

            var data1 = projectManageIBLL.GetPageListAddress(queryJson);
            List<ProjectVo> list = new List<ProjectVo>();
            foreach (var info in data1)
            {

                var tenderFlg = dataItemBLL.GetDetailItemName(info.TenderFlg, "TenderFlg");
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (tenderFlg != null)
                {
                    info.TenderFlg = tenderFlg.F_ItemName;
                }
                if (projectSource != null)
                {
                    info.ProjectSource = projectSource.F_ItemName;
                }
                list.Add(info);
            }
            list = list.OrderBy(t => t.CreateTime).ToList();
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
        /// 多部门获取页面导出显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageListAllDepartmentId(string queryJson)
        {
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            List<ProjectVo> listdate = new List<ProjectVo>();

            if (followPerson.F_MoreDepartmentId != null)
            {

                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or t.FDepartmentId='" + strList[i] + "' or t.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or t.FDepartmentId='" + strList[i] + "' or t.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = projectManageIBLL.GetPageListAddressDepartmentId(queryJson, deps);
                if (data.ToList().Count > 0)
                {
                    foreach (var info in data)
                    {

                        listdate.Add(info);


                    }
                }
            }
            //listdate = listdate.OrderBy(t => t.CreateTime).ToList();
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
        /// 多部门历史信息
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetAllPageListDepartmentId(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            List<ProjectVo> listdate = new List<ProjectVo>();
            if (followPerson.F_MoreDepartmentId != null)
            {

                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or t.FDepartmentId='" + strList[i] + "' or t.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or t.FDepartmentId='" + strList[i] + "' or t.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = projectManageIBLL.GetPageListDepartmentId(paginationobj, queryJson, deps);
                if (data.ToList().Count > 0)
                {
                    foreach (var info in data)
                    {
                        List<ProjectContractEntity> projectContracts = projectContractBLL.GetProjectContractByProjectId(info.Id);
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
        }/// <summary>
         /// 历史信息
         /// </summary>
         /// <param name="pagination"></param>
         /// <param name="queryJson"></param>
         /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetAllPageList(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = projectManageIBLL.GetPageList(paginationobj, queryJson);
            foreach (var info in data)
            {
                List<ProjectContractEntity> projectContracts = projectContractBLL.GetProjectContractByProjectId(info.Id);
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
        public ActionResult PreviewIndexFromById(string keyValue)
        {
            var data = projectManageIBLL.PreviewIndexFrom(keyValue);

            var followPerson = userIBLL.GetEntityByUserId(data.FollowPerson);
            var preparedPerson = userIBLL.GetEntityByUserId(data.PreparedPerson);

            var projectStatus = dataItemBLL.GetDetailItemName(data.ProjectStatus, "ProjectStatus");
            if (preparedPerson != null)
            {
                data.PreparedPerson = preparedPerson.F_RealName;
            }
            if (followPerson != null)
            {
                data.FollowPerson = followPerson.F_RealName;
            }
            if (projectStatus != null)
            {
                data.ProjectStatus = projectStatus.F_ItemName;
            }
            var jsonData = new
            {
                Project = data
            };
            return Success(jsonData);
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult GetSelectedProjectList(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();

            var data = projectManageIBLL.GetSelectedProjectList(paginationobj, queryJson);
            /*   foreach (var info in data)
               {
                   List<ProjectContractEntity> projectContracts = projectContractBLL.GetProjectContractByProjectId(info.Id);
                   if (projectContracts.Count > 0)
                   {
                       if (projectContracts.FirstOrDefault().MainContract == 1)
                       {
                           info.ContractNo = projectContracts.FirstOrDefault().ContractNo;
                       }
                   }
                   if (projectContracts.Count > 1)
                   {
                       if (projectContracts.FirstOrDefault().MainContract == 1)
                       {
                           foreach (var contract in projectContracts)
                           {
                               if (contract.ContractType == 1)
                               {
                                   info.ContractNo = contract.ContractNo;
                               }
                           }
                       }

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
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetSelectedProjectByContractList(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var user = LoginUserInfo.Get();
            var userId = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId_2(userId);
            List<ProjectVo> listdate = new List<ProjectVo>();
            var roleList = userRelationIBLL.GetUserRoleList(userId);
            string deps = "";
            int isFinance = 0;
            if(roleList.Count > 0)
            {
                var financeRole = roleList.Where(i => i.F_ObjectId == "c38d935c-c364-41fb-8d36-304076009949").ToList();
                if(financeRole.Count > 0)
                {
                    isFinance = 1;
                }
            }
            //是否管理员
            //是否角色位财务
            if (userId == "1e5dfa6a-6f0c-454c-b1ac-aeafef95aea5" || userId == "fae74e8a-3dcc-45f2-b6e1-b6800654eaf9" || userId == "System" 
                || isFinance == 1)
            {
                deps = "";
            }
            else
            {
                deps = " and ( ";
                if (followPerson.F_MoreDepartmentId != null)
                {

                    string[] strList = followPerson.F_MoreDepartmentId.Split(',');

                    for (var i = 0; i < strList.Length; i++)
                    {
                        if (i == 0)
                        {
                            deps += " ( t.DepartmentId='" + strList[i] + "' or t.FDepartmentId='" + strList[i] + "' or t.PDepartmentId='" + strList[i] + "') ";
                        }
                        else
                        {
                            deps += " or ( t.DepartmentId='" + strList[i] + "' or t.FDepartmentId='" + strList[i] + "' or t.PDepartmentId='" + strList[i] + "') ";
                        }

                    }
                }
                else
                {
                    deps += " ( t.DepartmentId='" + user.departmentId + "' or t.FDepartmentId='" + user.departmentId + "' or t.PDepartmentId='" + user.departmentId + "') ";
                }
                deps += " or pt.DepartmentId='" + user.departmentId + "' ) ";

            }

            var data = projectManageIBLL.GetSelectedProjectByContractList_2(paginationobj, queryJson, deps);
            //var data = projectManageIBLL.GetSelectedProjectList(paginationobj, queryJson);
            //foreach (var info in data)
            //{
            //    List<ProjectContractEntity> projectContracts = projectContractBLL.GetProjectContractByProjectId(info.Id);
            //    if (projectContracts.Count > 0)
            //    {
            //        info.ContractNo = projectContracts.FirstOrDefault().ContractNo;
            //    }
            //    if (projectContracts.Count > 1)
            //    {
            //        foreach (var contract in projectContracts)
            //        {
            //            if (contract.ContractType == 1)
            //            {
            //                info.ContractNo = contract.ContractNo;
            //            }
            //        }
            //    }
            //}
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
        /// 开票添加项目查询
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetSelectedProjectByContractListBilling(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = projectManageIBLL.GetSelectedProjectByContractListBilling(paginationobj, queryJson);
            //var data = projectManageIBLL.GetSelectedProjectList(paginationobj, queryJson);
            //foreach (var info in data)
            //{
            //    List<ProjectContractEntity> projectContracts = projectContractBLL.GetProjectContractByProjectId(info.Id);
            //    if (projectContracts.Count > 0)
            //    {
            //        info.ContractNo = projectContracts.FirstOrDefault().ContractNo;
            //    }
            //    if (projectContracts.Count > 1)
            //    {
            //        foreach (var contract in projectContracts)
            //        {
            //            if (contract.ContractType == 1)
            //            {
            //                info.ContractNo = contract.ContractNo;
            //            }
            //        }
            //    }
            //}
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
        /// 多部门开票添加项目查询
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetSelectedProjectByContractListBillingDepartmentId(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            List<ProjectVo> list = new List<ProjectVo>();

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
                        deps += " ( pc.DepartmentId='" + strList[i] + "' or t.DepartmentId='" + strList[i] + "' or t.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( pc.DepartmentId='" + strList[i] + "' or t.DepartmentId='" + strList[i] + "' or t.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = projectManageIBLL.GetSelectedProjectByContractListBillingDepartmentId(paginationobj, queryJson, deps);
                if (data.ToList().Count > 0)
                {
                    foreach (var info in data)
                    {
                        list.Add(info);
                    }
                }
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

        [HttpGet]
        [AjaxOnly]
        public ActionResult GetSelectedProjectListT(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = projectManageIBLL.GetSelectedProjectListT(paginationobj, queryJson);
            /* foreach (var info in data)
             {
                 List<ProjectContractEntity> projectContracts = projectContractBLL.GetProjectContractByProjectId(info.Id);
                 if (projectContracts.Count > 0)
                 {
                     info.ContractNo = projectContracts.FirstOrDefault().ContractNo;
                 }
                 if (projectContracts.Count > 1)
                 {
                     foreach (var contract in projectContracts)
                     {
                         if (contract.ContractType == 1)
                         {
                             info.ContractNo = contract.ContractNo;
                         }
                     }
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
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetSelectedProjectListTi(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = projectManageIBLL.GetSelectedProjectListTi(paginationobj, queryJson);
            //foreach (var info in data)
            //{
            //    List<ProjectContractEntity> projectContracts = projectContractBLL.GetProjectContractByProjectId(info.Id);
            //    if (projectContracts.Count > 0)
            //    {
            //        info.ContractNo = projectContracts.FirstOrDefault().ContractNo;
            //    }
            //    if (projectContracts.Count > 1)
            //    {
            //        foreach (var contract in projectContracts)
            //        {
            //            if (contract.ContractType == 1)
            //            {
            //                info.ContractNo = contract.ContractNo;
            //            }
            //        }
            //    }
            //}
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
        /// 获取表单数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetFormData(string keyValue)
        {
            // var ProjectData = projectManageIBLL.GetProjectEntity(keyValue);
            var ProjectData = projectManageIBLL.GetPreviewFormDataById(keyValue);
            //var contractdata = projectContractIBLL.GetProjectContracByprojectId(ProjectData.Id);
            //var paycollection = projectPayCollectionIBLL.GetCollectionByIdProjectId(ProjectData.Id);
            //var payment = projectPaymentIBLL.GetProjectPaymentByprojectId(ProjectData.Id);
            //decimal? Sum = 0;
            ////全款绩效
            //decimal? quanAmountSum = 0;
            ////到款绩效
            //decimal? daoAmountSum = 0;

            //if (contractdata != null)
            //{
            //    if (ProjectData.ProjectSource == "1")
            //    {
            //        quanAmountSum = (decimal)0.3 * contractdata.EffectiveAmount;
            //        ProjectData.quanAmountSum = (double)quanAmountSum;
            //        if (paycollection != null)
            //        {
            //            if (payment.PaymentAmount != null)
            //            {
            //                decimal? sum = paycollection.Amount - payment.PaymentAmount;
            //                daoAmountSum = ((decimal)0.3 * sum);
            //            }
            //            else
            //            {
            //                daoAmountSum = ((decimal?)0.3 * paycollection.Amount);
            //            }
            //            ProjectData.daoAmountSum = (double)daoAmountSum;
            //        }

            //    }
            //    if (ProjectData.ProjectSource == "2")
            //    {
            //        quanAmountSum = (decimal)0.03 * contractdata.EffectiveAmount;
            //        ProjectData.quanAmountSum = (double)quanAmountSum;
            //        if (paycollection != null)
            //        {
            //            if (payment.PaymentAmount != null)
            //            {
            //                decimal? sum = paycollection.Amount - payment.PaymentAmount;
            //                daoAmountSum = ((decimal)0.03 * sum);
            //            }
            //            else
            //            {
            //                daoAmountSum = ((decimal?)0.03 * paycollection.Amount);
            //            }
            //            ProjectData.daoAmountSum = (double)daoAmountSum;
            //        }
            //    }
            //}





            var jsonData = new
            {
                Project = ProjectData,
                //quanAmountSum= quanAmountSum,
                //daoAmountSum= daoAmountSum
            };
            return Success(jsonData);
        }

        /// <summary>
        /// 合同预览
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>

        public ActionResult GetPreviewFormData(string keyValue)
        {
            var ProjectData = projectManageIBLL.GetPreviewFormData(keyValue);
            List<ProjectContractEntity> projectContracts = projectContractBLL.GetProjectContractByProjectId(ProjectData.Id);
            var tenderFlg = dataItemBLL.GetDetailItemName(ProjectData.TenderFlg, "TenderFlg");
            if (projectContracts.Count > 0)
            {
                ProjectData.ContractNo = projectContracts.FirstOrDefault().ContractNo;
            }
            if (ProjectData != null)
            {

            }
            var jsonData = new
            {
                Project = ProjectData,
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>

        public ActionResult GetPreviewFormDataById(string keyValue)
        {
            var ProjectData = projectManageIBLL.GetPreviewFormDataById(keyValue);
            var preparedPerson = userIBLL.GetEntityByUserId(ProjectData.PreparedPerson);
            var followPerson = userIBLL.GetEntityByUserId(ProjectData.FollowPerson);
            var projectStatus = dataItemBLL.GetDetailItemName(ProjectData.ProjectStatus, "ProjectStatus");
            var projectSource = dataItemBLL.GetDetailItemName(ProjectData.ProjectSource, "ProjectSource");
            var tenderFlg = dataItemBLL.GetDetailItemName(ProjectData.TenderFlg, "TenderFlg");
            if (ProjectData.ProvinceId != null || ProjectData.CityId != null || ProjectData.CountyId != null)
            {

                var provinceId = "";
                var cityId = "";
                var countyId = "";

                var shen = areaIBLL.GetList(ProjectData.ProvinceId);
                if (shen.Count() > 0)
                {
                    provinceId = shen.FirstOrDefault().F_AreaName;
                }

                var shi = areaIBLL.GetList(ProjectData.CityId);

                if (shi.Count() > 0)
                {
                    cityId = shi.FirstOrDefault().F_AreaName;
                }

                var diz = areaIBLL.GetList(ProjectData.CountyId);

                if (diz.Count() > 0)
                {
                    countyId = diz.FirstOrDefault().F_AreaName;
                }
                ProjectData.ProvincesAndcities = provinceId + "" + cityId + "" + countyId + ProjectData.Address;
            }

            if (tenderFlg != null)
            {
                ProjectData.TenderFlg = tenderFlg.F_ItemName;
            }

            if (preparedPerson != null)
            {
                ProjectData.PreparedPerson = preparedPerson.F_RealName;
            }

            if (followPerson != null)
            {
                ProjectData.FollowPerson = followPerson.F_RealName;
            }
            if (projectStatus != null)
            {
                ProjectData.ProjectStatus = projectStatus.F_ItemName;
            }
            if (projectSource != null)
            {
                ProjectData.ProjectSource = projectSource.F_ItemName;
            }
            var jsonData = new
            {
                Project = ProjectData,
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 编辑查询
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>

        public ActionResult GetPreviewFormDataBy(string keyValue)
        {
            var ProjectData = projectManageIBLL.GetPreviewFormDataBy(keyValue);
            /*var preparedPerson = userIBLL.GetEntityByUserId(ProjectData.PreparedPerson);
            var followPerson = userIBLL.GetEntityByUserId(ProjectData.FollowPerson);
            var projectStatus = dataItemBLL.GetDetailItemName(ProjectData.ProjectStatus, "ProjectStatus");
            var projectSource = dataItemBLL.GetDetailItemName(ProjectData.ProjectSource, "ProjectSource");

            if (preparedPerson != null)
            {
                ProjectData.PreparedPerson = preparedPerson.F_RealName;
            }
            if (followPerson != null)
            {
                ProjectData.FollowPerson = followPerson.F_RealName;
            }
            if (projectStatus != null)
            {
                ProjectData.ProjectStatus = projectStatus.F_ItemName;
            }
            if (projectSource != null)
            {
                ProjectData.ProjectSource = projectSource.F_ItemName;
            }*/

            var jsonData = new
            {
                Project = ProjectData,
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 获取项目名称
        /// </summary>
        /// <param name="keyValue">项目主键</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetEntityName(string keyValue)
        {
            if (keyValue == "0")
            {
                return SuccessString("");
            }
            var data = projectManageIBLL.GetProjectEntity(keyValue);
            return SuccessString(data.ProjectName);
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
            var ProjectData = projectManageIBLL.GetEntityByProcessId(processId);


            var jsonData = new
            {
                Project = ProjectData,
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
            projectManageIBLL.DeleteEntity(keyValue);
            return Success("删除成功！", "项目报备(删除)", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, keyValue);
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

            ProjectEntity entity = strEntity.ToObject<ProjectEntity>();
            //if (!string.IsNullOrEmpty(entity.ProjectSource) && entity.ProjectSource == "1")
            if (!string.IsNullOrEmpty(entity.ProjectSource))
            {
                if (entity.TenderFlg == "1")
                {
                    int count = projectManageIBLL.JudgePepeatProjectName(entity.ProjectName, keyValue);
                    if (count > 0)
                    {
                        return Fail("该项目系统已存在！");
                    }
                }
                else
                {
                    int count = projectManageIBLL.JudgePepeatProject(entity.ContactPhone, entity.CustName, keyValue);
                    if (count > 0)
                    {
                        if (count == 1)
                        {
                            return Fail("该手机号码系统已存在！");
                        }
                        else if (count == 2)
                        {
                            return Fail("该委托单位系统已存在！");
                        }
                    }
                }

                //根绝营销人id 获取对应的营销人员的部门/和公司信息
                if (!string.IsNullOrEmpty(entity.FollowPerson))
                {
                    UserEntity user = userIBLL.GetEntityByUserId(entity.FollowPerson);
                    if (user != null)
                    {
                        entity.FCompanyId = user.F_CompanyId;
                        entity.FDepartmentId = user.F_DepartmentId;
                    }
                }
                else
                {
                    entity.FCompanyId = "";
                    entity.FDepartmentId = "";
                }
                if (!string.IsNullOrEmpty(entity.PreparedPerson))
                {
                    UserEntity user = userIBLL.GetEntityByUserId(entity.PreparedPerson);
                    if (user != null)
                    {
                        entity.PCompanyId = user.F_CompanyId;
                        entity.PDepartmentId = user.F_DepartmentId;
                    }
                }
                else
                {
                    entity.PCompanyId = "";
                    entity.PDepartmentId = "";
                }
                projectManageIBLL.SaveEntity(keyValue, entity);
                if (autoFlag == 1)
                {
                    codeRuleIBLL.UseRuleSeed("10002");
                }
            }
            else
            {
                //根绝营销人id 获取对应的营销人员的部门/和公司信息
                if (!string.IsNullOrEmpty(entity.FollowPerson))
                {
                    UserEntity user = userIBLL.GetEntityByUserId(entity.FollowPerson);
                    if (user != null)
                    {
                        entity.FCompanyId = user.F_CompanyId;
                        entity.FDepartmentId = user.F_DepartmentId;
                    }
                }
                else
                {
                    entity.FCompanyId = "";
                    entity.FDepartmentId = "";
                }
                if (!string.IsNullOrEmpty(entity.PreparedPerson))
                {
                    UserEntity user = userIBLL.GetEntityByUserId(entity.PreparedPerson);
                    if (user != null)
                    {
                        entity.PCompanyId = user.F_CompanyId;
                        entity.PDepartmentId = user.F_DepartmentId;
                    }
                }
                else
                {
                    entity.PCompanyId = "";
                    entity.PDepartmentId = "";
                }
                projectManageIBLL.SaveEntity(keyValue, entity);
                if (autoFlag == 1)
                {
                    codeRuleIBLL.UseRuleSeed("10002");
                }
            }



            return Success("保存成功！", "项目报备", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, entity.ToJson());
        }


        #endregion
    }
}
