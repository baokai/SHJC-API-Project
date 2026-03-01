using Learun.Application.Base.SystemModule;
using Learun.Application.Organization;
using Learun.Application.TwoDevelopment.LR_CodeDemo;
using Learun.Application.TwoDevelopment.LR_CodeDemo.ReportForms;
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
    public class ReportFormsController : MvcControllerBase
    {
        private ProjectPayCollectionIBLL projectPayCollectionIBLL = new ProjectPayCollectionBLL();
        private ProjectContractIBLL projectContractIBLL = new ProjectContractBLL();
        private ProjectPaymentIBLL projectPaymentIBLL = new ProjectPaymentBLL();
        private ReportFormsIBLL reportFormsBLL = new ReportFormsBLL();
        private ProjectContractBLL projectContractBLL = new ProjectContractBLL();
        private DataItemIBLL dataItemBLL = new DataItemBLL();
        private ICache cache = CacheFactory.CaChe();
        private UserIBLL userIBLL = new UserBLL();
        private DepartmentIBLL departmentIBLL = new DepartmentBLL();
        #region 获取数据

        /// <summary>
        /// 获取营销报表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        /// 

        [HttpGet]
        [AjaxOnly]
        public ActionResult GetMarketings(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = reportFormsBLL.GetMarketings(paginationobj, queryJson);
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
        /// 获取营销报表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        /// 

        [HttpGet]
        [AjaxOnly]
        public ActionResult GetMarketingsList(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var u = LoginUserInfo.Get().userId;
            var user = userIBLL.GetHZUserId(u);

            List<MarketingEntity> list = new List<MarketingEntity>();
            //var data = reportFormsBLL.GetMarketings(paginationobj, queryJson);


            string deps = " ( t.DepartmentId='" + user.F_DepartmentId + "' or a.PDepartmentId='" + user.F_DepartmentId + "' or t.SubDepartmentId='" + user.F_DepartmentId + "' or t.MainDepartmentId='" + user.F_DepartmentId + "') ";
            var data = reportFormsBLL.GetMarketings1(paginationobj, queryJson, deps);
            foreach (var cp in data)
            {
                if (cp.SubAmount != null && cp.MainAmount != null)
                {
                    if (user.F_DepartmentId == cp.SubDepartmentId)
                    {

                        cp.ContractAmount = cp.SubAmount;
                        var pay = projectPaymentIBLL.GetProjectPaymentByprojectId(cp.Id);
                        if (pay.PaymentAmount != null)
                        {

                            var p = pay.PaymentAmount.ToInt() / 2;
                            cp.EffectiveAmount = cp.SubAmount - p;

                        }
                        else
                        {
                            cp.EffectiveAmount = cp.SubAmount;
                        }


                    }
                    else if (user.F_DepartmentId == cp.MainDepartmentId)
                    {
                        cp.ContractAmount = cp.MainAmount;
                        var pay = projectPaymentIBLL.GetProjectPaymentByprojectId(cp.Id);
                        if (pay.PaymentAmount != null)
                        {

                            var p = pay.PaymentAmount.ToInt() / 2;
                            cp.EffectiveAmount = cp.MainAmount - p;

                        }
                        else
                        {
                            cp.EffectiveAmount = cp.MainAmount;
                        }
                    }
                }
                else
                {
                    cp.ContractAmount = cp.ContractAmount;
                    cp.EffectiveAmount = cp.EffectiveAmount;
                }
                list.Add(cp);
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
        /// 获取资金报表数据
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns> 
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetCapitalDepartmentIdList(string pagination, string queryJson)
        {
            //获取今年的时间
            DateTime StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-01"));
            //获取历史数据
            DateTime StartTime1 = DateTime.Parse(DateTime.Now.AddYears(-1).ToString("yyyy-01"));
            //接受获取的数据
            List<CapitalDepartmentId> list = new List<CapitalDepartmentId>();
            //公司id
            var companyId = "207fa1a9-160c-4943-a89b-8fa4db0547ce";
            //获取所有部门数据
            var datadepartment = departmentIBLL.GetList(companyId, "");
            //var datadepartment = departmentIBLL.GetPageListHZ();
            //获取部门和年月查询条件
            CapitalDepartmentId q = queryJson.ToObject<CapitalDepartmentId>();
            //部门
            var DepartmentIdT = q.DepartmentId;
            //年
            var DateYYYY = q.YYYYTime;
            var timeYY = StartTime.ToString("yyyy");
            //今年数据
            CapitalDepartmentId xn = new CapitalDepartmentId();
            xn.ContractAmountList = 0;
            xn.EffectiveAmountList = 0;
            xn.ContractAmountSUN = 0;
            xn.sumList = 0;
            xn.ContractAmountSUNList = 0;
            xn.AmountList = 0;
            if (DepartmentIdT != null && DateYYYY != "" && DateYYYY != null)
            {
                if (DateYYYY.ToInt() == timeYY.ToInt())
                {
                    //今年数据
                    StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-01"));
                    ////今年数据
                    xn.yefen = "汇总";
                    //qn.yefen = "历史数据";
                    //上个月小计
                    decimal? sumList1 = 0;
                    //decimal? sumList12 = 0;
                    //循环12个月的数据
                    for (int i = 0; i < 12; i++)
                    {
                        CapitalDepartmentId list1 = new CapitalDepartmentId();
                        //存当前月份
                        list1.yefen = StartTime.AddMonths(i).ToString("yyyy-MM");
                        var department = departmentIBLL.GetEntity(DepartmentIdT);
                        if (department != null)
                        {
                            list1.DepartmentIdName = department.F_FullName;
                        }
                        list1.datayyyyMM = StartTime.AddMonths(i).ToString("yyyy");
                        list1.yefenList = list1.yefen + list1.DepartmentIdName;
                        list1.index = StartTime.AddMonths(i).ToString("MM").ToInt();
                        //初始化
                        //本月合同额
                        list1.ContractAmountList = 0;
                        //本月绩效
                        list1.EffectiveAmountList = 0;
                        //本月资金
                        list1.ContractAmountSUNList = 0;
                        list1.AmountList = 0;
                        decimal? ContractAmountSUNList1 = 0;
                        //小计
                        list1.sumList = 0;
                        var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), DepartmentIdT);
                        if (data1.ToList().Count > 0)
                        {
                            foreach (var ins in data1)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //历史数据
                                //审核时间<当年
                                //本月绩效
                                if (ins.EffectiveAmount != null)
                                {
                                    //自主
                                    if (ins.ProjectSource == "1")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }
                                    //渠道
                                    if (ins.ProjectSource == "2")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }

                                }
                            }
                        }
                        //今年数据
                        var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), DepartmentIdT);
                        if (data.ToList().Count > 0)
                        {
                            //本月成本                 
                            CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                            if (capitalAmount != null)
                            {
                                list1.ContractAmountSUN = capitalAmount.CostAmount;
                            }
                            foreach (var inf in data)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //本月合同额
                                if (inf.EffectiveAmount != null)
                                {
                                    list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                                }

                                //本月绩效                               
                                var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                                if (inf.ContractAmount <= cay.Amount)
                                {
                                    if (inf.EffectiveAmount != null)
                                    {

                                        //自主
                                        if (inf.ProjectSource == "1")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }
                                        }
                                        //渠道
                                        if (inf.ProjectSource == "2")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }

                                        }


                                    }

                                }

                            }
                            //本月资金
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.ContractAmountSUNList != null)
                            {

                                list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;

                            }
                            list.Add(list1);
                            sumList1 = list1.ContractAmountSUNList;
                            xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                            xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                            if (list1.ContractAmountSUN != null)
                            {
                                xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                            }
                            xn.sumList = xn.sumList + list1.sumList;
                        }
                    }

                }
                else
                {
                    var t = timeYY.ToInt() - DateYYYY.ToInt();
                    StartTime1 = DateTime.Parse(DateTime.Now.AddYears(-t).ToString("yyyy-01"));
                    //上个月小计
                    decimal? sumList1 = 0;
                    //今年数据
                    //循环12个月的数据
                    for (int i = 0; i < 12; i++)
                    {
                        CapitalDepartmentId list1 = new CapitalDepartmentId();
                        //存当前月份
                        list1.yefen = StartTime1.AddMonths(i).ToString("yyyy-MM");
                        var department = departmentIBLL.GetEntity(DepartmentIdT);
                        if (department != null)
                        {
                            list1.DepartmentIdName = department.F_FullName;
                        }
                        list1.datayyyyMM = StartTime1.AddMonths(i).ToString("yyyy");
                        list1.yefenList = list1.yefen + list1.DepartmentIdName;
                        list1.index = StartTime1.AddMonths(i).ToString("MM").ToInt();
                        //初始化
                        //本月合同额
                        list1.ContractAmountList = 0;
                        //本月绩效
                        list1.EffectiveAmountList = 0;
                        //本月资金
                        list1.ContractAmountSUNList = 0;
                        list1.AmountList = 0;
                        decimal? ContractAmountSUNList1 = 0;
                        //小计
                        list1.sumList = 0;
                        var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime1.ToString(), StartTime1.AddMonths(i).ToString(), StartTime1.AddMonths(i + 1).ToString(), DepartmentIdT);
                        if (data1.ToList().Count > 0)
                        {
                            foreach (var ins in data1)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //历史数据
                                //审核时间<当年
                                //本月绩效
                                if (ins.EffectiveAmount != null)
                                {
                                    //自主
                                    if (ins.ProjectSource == "1")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }
                                    //渠道
                                    if (ins.ProjectSource == "2")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }

                                }
                            }
                        }
                        //今年数据
                        var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime1.ToString(), StartTime1.AddMonths(i).ToString(), StartTime1.AddMonths(i + 1).ToString(), DepartmentIdT);
                        if (data.ToList().Count > 0)
                        {
                            //本月成本                 
                            CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                            if (capitalAmount != null)
                            {
                                list1.ContractAmountSUN = capitalAmount.CostAmount;
                            }
                            foreach (var inf in data)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //本月合同额
                                if (inf.EffectiveAmount != null)
                                {
                                    list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                                }

                                //本月绩效                               
                                var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                                if (inf.ContractAmount <= cay.Amount)
                                {
                                    if (inf.EffectiveAmount != null)
                                    {

                                        //自主
                                        if (inf.ProjectSource == "1")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }
                                        }
                                        //渠道
                                        if (inf.ProjectSource == "2")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }

                                        }


                                    }

                                }

                            }
                            //本月资金
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.ContractAmountSUNList != null)
                            {

                                list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;

                            }
                            list.Add(list1);
                            sumList1 = list1.ContractAmountSUNList;
                            xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                            xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                            if (list1.ContractAmountSUN != null)
                            {
                                xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                            }
                            xn.sumList = xn.sumList + list1.sumList;
                        }
                    }


                }
                xn.ContractAmountSUNList = xn.EffectiveAmountList - xn.ContractAmountSUN;

                list.Add(xn);

            }

            if (DateYYYY != null && DepartmentIdT == "")
            {
                if (DateYYYY.ToInt() == timeYY.ToInt())
                {
                    foreach (var dep in datadepartment)
                    {
                        ////今年数据
                        xn.yefen = "汇总";
                        //qn.yefen = "历史数据";
                        //上个月小计
                        decimal? sumList1 = 0;
                        //decimal? sumList12 = 0;
                        //循环12个月的数据
                        for (int i = 0; i < 12; i++)
                        {
                            CapitalDepartmentId list1 = new CapitalDepartmentId();
                            //存当前月份
                            list1.yefen = StartTime.AddMonths(i).ToString("yyyy-MM");
                            list1.DepartmentIdName = dep.F_FullName;
                            list1.datayyyyMM = StartTime.AddMonths(i).ToString("yyyy");
                            list1.yefenList = list1.yefen + list1.DepartmentIdName;
                            list1.index = StartTime.AddMonths(i).ToString("MM").ToInt();
                            //初始化
                            //本月合同额
                            list1.ContractAmountList = 0;
                            //本月绩效
                            list1.EffectiveAmountList = 0;
                            //本月资金
                            list1.ContractAmountSUNList = 0;
                            list1.AmountList = 0;
                            decimal? ContractAmountSUNList1 = 0;
                            //小计
                            list1.sumList = 0;
                            var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                            if (data1.ToList().Count > 0)
                            {
                                foreach (var ins in data1)
                                {
                                    decimal? EffectiveAmount1 = 0;
                                    //历史数据
                                    //审核时间<当年
                                    //本月绩效
                                    if (ins.EffectiveAmount != null)
                                    {
                                        //自主
                                        if (ins.ProjectSource == "1")
                                        {
                                            EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.AmountList = list1.AmountList + EffectiveAmount1;
                                            }
                                        }
                                        //渠道
                                        if (ins.ProjectSource == "2")
                                        {
                                            EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.AmountList = list1.AmountList + EffectiveAmount1;
                                            }
                                        }

                                    }
                                }
                            }
                            //今年数据
                            var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                            if (data.ToList().Count > 0)
                            {
                                //本月成本                 
                                CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                                if (capitalAmount != null)
                                {
                                    list1.ContractAmountSUN = capitalAmount.CostAmount;
                                }
                                foreach (var inf in data)
                                {
                                    decimal? EffectiveAmount1 = 0;
                                    //本月合同额
                                    if (inf.EffectiveAmount != null)
                                    {
                                        list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                                    }

                                    //本月绩效                               
                                    var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                                    if (inf.ContractAmount <= cay.Amount)
                                    {
                                        if (inf.EffectiveAmount != null)
                                        {

                                            //自主
                                            if (inf.ProjectSource == "1")
                                            {
                                                EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                                {
                                                    list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                                }
                                            }
                                            //渠道
                                            if (inf.ProjectSource == "2")
                                            {
                                                EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                                {
                                                    list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                                }

                                            }


                                        }

                                    }

                                }
                                //本月资金
                                if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                                {
                                    ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                                }
                                if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                                {
                                    ContractAmountSUNList1 = list1.ContractAmountSUN;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                                }
                                if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                                {
                                    ContractAmountSUNList1 = list1.EffectiveAmountList;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                                }
                                if (list1.ContractAmountSUNList != null)
                                {

                                    list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;

                                }
                                list.Add(list1);
                                sumList1 = list1.ContractAmountSUNList;
                                xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                                xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                                if (list1.ContractAmountSUN != null)
                                {
                                    xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                                }
                                xn.sumList = xn.sumList + list1.sumList;
                            }
                        }
                    }
                }
                else
                {
                    var t = timeYY.ToInt() - DateYYYY.ToInt();
                    StartTime1 = DateTime.Parse(DateTime.Now.AddYears(-t).ToString("yyyy-01"));
                    foreach (var dep in datadepartment)
                    {
                        ////今年数据
                        xn.yefen = "汇总";
                        //qn.yefen = "历史数据";
                        //上个月小计
                        decimal? sumList1 = 0;
                        //decimal? sumList12 = 0;
                        //循环12个月的数据
                        for (int i = 0; i < 12; i++)
                        {
                            CapitalDepartmentId list1 = new CapitalDepartmentId();
                            //存当前月份
                            list1.yefen = StartTime1.AddMonths(i).ToString("yyyy-MM");
                            list1.DepartmentIdName = dep.F_FullName;
                            list1.datayyyyMM = StartTime1.AddMonths(i).ToString("yyyy");
                            list1.yefenList = list1.yefen + list1.DepartmentIdName;
                            list1.index = StartTime1.AddMonths(i).ToString("MM").ToInt();
                            //初始化
                            //本月合同额
                            list1.ContractAmountList = 0;
                            //本月绩效
                            list1.EffectiveAmountList = 0;
                            //本月资金
                            list1.ContractAmountSUNList = 0;
                            list1.AmountList = 0;
                            decimal? ContractAmountSUNList1 = 0;
                            //小计
                            list1.sumList = 0;
                            var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime1.ToString(), StartTime1.AddMonths(i).ToString(), StartTime1.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                            if (data1.ToList().Count > 0)
                            {
                                foreach (var ins in data1)
                                {
                                    decimal? EffectiveAmount1 = 0;
                                    //历史数据
                                    //审核时间<当年
                                    //本月绩效
                                    if (ins.EffectiveAmount != null)
                                    {
                                        //自主
                                        if (ins.ProjectSource == "1")
                                        {
                                            EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.AmountList = list1.AmountList + EffectiveAmount1;
                                            }
                                        }
                                        //渠道
                                        if (ins.ProjectSource == "2")
                                        {
                                            EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.AmountList = list1.AmountList + EffectiveAmount1;
                                            }
                                        }

                                    }
                                }
                            }
                            //今年数据
                            var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime1.ToString(), StartTime1.AddMonths(i).ToString(), StartTime1.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                            if (data.ToList().Count > 0)
                            {
                                //本月成本                 
                                CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                                if (capitalAmount != null)
                                {
                                    list1.ContractAmountSUN = capitalAmount.CostAmount;
                                }
                                foreach (var inf in data)
                                {
                                    decimal? EffectiveAmount1 = 0;
                                    //本月合同额
                                    if (inf.EffectiveAmount != null)
                                    {
                                        list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                                    }

                                    //本月绩效                               
                                    var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                                    if (inf.ContractAmount <= cay.Amount)
                                    {
                                        if (inf.EffectiveAmount != null)
                                        {

                                            //自主
                                            if (inf.ProjectSource == "1")
                                            {
                                                EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                                {
                                                    list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                                }
                                            }
                                            //渠道
                                            if (inf.ProjectSource == "2")
                                            {
                                                EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                                {
                                                    list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                                }

                                            }


                                        }

                                    }

                                }
                                //本月资金
                                if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                                {
                                    ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                                }
                                if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                                {
                                    ContractAmountSUNList1 = list1.ContractAmountSUN;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                                }
                                if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                                {
                                    ContractAmountSUNList1 = list1.EffectiveAmountList;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                                }
                                if (list1.ContractAmountSUNList != null)
                                {

                                    list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;

                                }
                                list.Add(list1);
                                sumList1 = list1.ContractAmountSUNList;
                                xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                                xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                                if (list1.ContractAmountSUN != null)
                                {
                                    xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                                }
                                xn.sumList = xn.sumList + list1.sumList;
                            }
                        }
                    }
                }

                xn.ContractAmountSUNList = xn.EffectiveAmountList - xn.ContractAmountSUN;
                list.Add(xn);
            }

            if (DateYYYY == "" && DepartmentIdT != null)
            {

                ////今年数据
                xn.yefen = "汇总";
                //qn.yefen = "历史数据";
                //上个月小计
                decimal? sumList1 = 0;
                //decimal? sumList12 = 0;
                //循环12个月的数据
                for (int i = 0; i < 12; i++)
                {
                    CapitalDepartmentId list1 = new CapitalDepartmentId();
                    //存当前月份
                    list1.yefen = StartTime.AddMonths(i).ToString("yyyy-MM");
                    var department = departmentIBLL.GetEntity(DepartmentIdT);
                    if (department != null)
                    {
                        list1.DepartmentIdName = department.F_FullName;
                    }
                    list1.datayyyyMM = StartTime.AddMonths(i).ToString("yyyy");
                    list1.yefenList = list1.yefen + list1.DepartmentIdName;
                    list1.index = StartTime.AddMonths(i).ToString("MM").ToInt();
                    //初始化
                    //本月合同额
                    list1.ContractAmountList = 0;
                    //本月绩效
                    list1.EffectiveAmountList = 0;
                    //本月资金
                    list1.ContractAmountSUNList = 0;
                    list1.AmountList = 0;
                    decimal? ContractAmountSUNList1 = 0;
                    //小计
                    list1.sumList = 0;
                    var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), DepartmentIdT);
                    if (data1.ToList().Count > 0)
                    {
                        foreach (var ins in data1)
                        {
                            decimal? EffectiveAmount1 = 0;
                            //历史数据
                            //审核时间<当年
                            //本月绩效
                            if (ins.EffectiveAmount != null)
                            {
                                //自主
                                if (ins.ProjectSource == "1")
                                {
                                    EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                    if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                    {
                                        list1.AmountList = list1.AmountList + EffectiveAmount1;
                                    }
                                }
                                //渠道
                                if (ins.ProjectSource == "2")
                                {
                                    EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                    if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                    {
                                        list1.AmountList = list1.AmountList + EffectiveAmount1;
                                    }
                                }

                            }
                        }
                    }
                    //今年数据
                    var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), DepartmentIdT);
                    if (data.ToList().Count > 0)
                    {
                        //本月成本                 
                        CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                        if (capitalAmount != null)
                        {
                            list1.ContractAmountSUN = capitalAmount.CostAmount;
                        }
                        foreach (var inf in data)
                        {
                            decimal? EffectiveAmount1 = 0;
                            //本月合同额
                            if (inf.EffectiveAmount != null)
                            {
                                list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                            }

                            //本月绩效                               
                            var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                            if (inf.ContractAmount <= cay.Amount)
                            {
                                if (inf.EffectiveAmount != null)
                                {

                                    //自主
                                    if (inf.ProjectSource == "1")
                                    {
                                        EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                        }
                                    }
                                    //渠道
                                    if (inf.ProjectSource == "2")
                                    {
                                        EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                        }

                                    }


                                }

                            }

                        }
                        //本月资金
                        if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                        {
                            ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                            list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                        }
                        if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                        {
                            ContractAmountSUNList1 = list1.ContractAmountSUN;
                            list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                        }
                        if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                        {
                            ContractAmountSUNList1 = list1.EffectiveAmountList;
                            list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                        }
                        if (list1.ContractAmountSUNList != null)
                        {

                            list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;

                        }
                        list.Add(list1);
                        sumList1 = list1.ContractAmountSUNList;
                        xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                        xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                        if (list1.ContractAmountSUN != null)
                        {
                            xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                        }
                        xn.sumList = xn.sumList + list1.sumList;
                    }

                }
                xn.ContractAmountSUNList = xn.EffectiveAmountList - xn.ContractAmountSUN;
                list.Add(xn);
            }


            if (DateYYYY == null && DepartmentIdT == null)
            {
                foreach (var dep in datadepartment)
                {
                    ////今年数据
                    xn.yefen = "汇总";
                    //qn.yefen = "历史数据";
                    //上个月小计
                    decimal? sumList1 = 0;
                    //decimal? sumList12 = 0;
                    //循环12个月的数据
                    for (int i = 0; i < 12; i++)
                    {
                        CapitalDepartmentId list1 = new CapitalDepartmentId();
                        //存当前月份
                        list1.yefen = StartTime.AddMonths(i).ToString("yyyy-MM");
                        list1.DepartmentIdName = dep.F_FullName;
                        list1.datayyyyMM = StartTime.AddMonths(i).ToString("yyyy");
                        list1.yefenList = list1.yefen + list1.DepartmentIdName;
                        list1.index = StartTime.AddMonths(i).ToString("MM").ToInt();

                        //初始化
                        //本月合同额
                        list1.ContractAmountList = 0;
                        //本月绩效
                        list1.EffectiveAmountList = 0;
                        //本月资金
                        list1.ContractAmountSUNList = 0;
                        list1.AmountList = 0;
                        decimal? ContractAmountSUNList1 = 0;
                        //小计
                        list1.sumList = 0;
                        var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                        if (data1.ToList().Count > 0)
                        {
                            foreach (var ins in data1)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //历史数据
                                //审核时间<当年
                                //本月绩效
                                if (ins.EffectiveAmount != null)
                                {
                                    //自主
                                    if (ins.ProjectSource == "1")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }
                                    //渠道
                                    if (ins.ProjectSource == "2")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }

                                }
                            }
                        }
                        //今年数据
                        var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                        if (data.ToList().Count > 0)
                        {
                            //本月成本                 
                            CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                            if (capitalAmount != null)
                            {
                                list1.ContractAmountSUN = capitalAmount.CostAmount;
                            }
                            foreach (var inf in data)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //本月合同额
                                if (inf.EffectiveAmount != null)
                                {
                                    list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                                }

                                //本月绩效                               
                                var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                                if (inf.ContractAmount <= cay.Amount)
                                {
                                    if (inf.EffectiveAmount != null)
                                    {

                                        //自主
                                        if (inf.ProjectSource == "1")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }
                                        }
                                        //渠道
                                        if (inf.ProjectSource == "2")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }

                                        }


                                    }

                                }

                            }
                            //本月资金
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.ContractAmountSUNList != null)
                            {

                                list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;

                            }
                            list.Add(list1);
                            sumList1 = list1.ContractAmountSUNList;
                            xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                            xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                            if (list1.ContractAmountSUN != null)
                            {
                                xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                            }
                            xn.sumList = xn.sumList + list1.sumList;
                            if (list1.AmountList != null)
                            {
                                xn.AmountList = xn.AmountList + list1.AmountList;
                            }
                        }
                    }
                }
                xn.ContractAmountSUNList = xn.EffectiveAmountList - xn.ContractAmountSUN;
                list.Add(xn);
            }

            //降序
            list = list.OrderBy(t => t.index).ThenByDescending(t => t.datayyyyMM).ToList();
            return Success(list);
        }

        /// 获取资金报表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns> 
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetCapitalDepartmentIdListAll(string pagination, string queryJson)
        {
            //获取今年的时间
            DateTime StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-01"));
            //获取历史数据
            DateTime StartTime1 = DateTime.Parse(DateTime.Now.AddYears(-1).ToString("yyyy-01"));
            //接受获取的数据
            List<CapitalDepartmentId> list = new List<CapitalDepartmentId>();
            //公司id
            var companyId = "207fa1a9-160c-4943-a89b-8fa4db0547ce";
            //获取所有部门数据
            var datadepartment = departmentIBLL.GetList(companyId, "");
            //获取部门和年月查询条件
            CapitalDepartmentId q = queryJson.ToObject<CapitalDepartmentId>();
            //部门
            var DepartmentIdT = q.DepartmentId;
            //年
            var DateYYYY = q.YYYYTime;
            var timeYY = StartTime.ToString("yyyy");
            //今年数据
            CapitalDepartmentId xn = new CapitalDepartmentId();
            xn.ContractAmountList = 0;
            xn.EffectiveAmountList = 0;
            xn.ContractAmountSUN = 0;
            xn.sumList = 0;
            xn.ContractAmountSUNList = 0;
            if (DepartmentIdT != null && DateYYYY != "" && DateYYYY != null)
            {
                if (DateYYYY.ToInt() == timeYY.ToInt())
                {
                    //今年数据
                    StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-01"));
                    ////今年数据
                    xn.yefen = "汇总";
                    //qn.yefen = "历史数据";
                    //上个月小计
                    decimal? sumList1 = 0;
                    //decimal? sumList12 = 0;
                    //循环12个月的数据
                    for (int i = 0; i < 12; i++)
                    {
                        CapitalDepartmentId list1 = new CapitalDepartmentId();
                        //存当前月份
                        list1.yefen = StartTime.AddMonths(i).ToString("yyyy-MM");
                        var department = departmentIBLL.GetEntity(DepartmentIdT);
                        if (department != null)
                        {
                            list1.DepartmentIdName = department.F_FullName;
                        }
                        list1.datayyyyMM = StartTime.AddMonths(i).ToString("yyyy");
                        list1.yefenList = list1.yefen + list1.DepartmentIdName;
                        list1.index = StartTime.AddMonths(i).ToString("MM").ToInt();
                        //初始化
                        //本月合同额
                        list1.ContractAmountList = 0;
                        //本月绩效
                        list1.EffectiveAmountList = 0;
                        //本月资金
                        list1.ContractAmountSUNList = 0;
                        list1.AmountList = 0;
                        decimal? ContractAmountSUNList1 = 0;
                        //小计
                        list1.sumList = 0;
                        var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), DepartmentIdT);
                        if (data1.ToList().Count > 0)
                        {
                            foreach (var ins in data1)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //历史数据
                                //审核时间<当年
                                //本月绩效
                                if (ins.EffectiveAmount != null)
                                {
                                    //自主
                                    if (ins.ProjectSource == "1")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }
                                    //渠道
                                    if (ins.ProjectSource == "2")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }

                                }
                            }
                        }
                        //今年数据
                        var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), DepartmentIdT);
                        if (data.ToList().Count > 0)
                        {
                            //本月成本                 
                            CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                            if (capitalAmount != null)
                            {
                                list1.ContractAmountSUN = capitalAmount.CostAmount;
                            }
                            foreach (var inf in data)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //本月合同额
                                if (inf.EffectiveAmount != null)
                                {
                                    list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                                }

                                //本月绩效                               
                                var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                                if (inf.ContractAmount <= cay.Amount)
                                {
                                    if (inf.EffectiveAmount != null)
                                    {

                                        //自主
                                        if (inf.ProjectSource == "1")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }
                                        }
                                        //渠道
                                        if (inf.ProjectSource == "2")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }

                                        }


                                    }

                                }

                            }
                            //本月资金
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.ContractAmountSUNList != null)
                            {

                                list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;

                            }
                            list.Add(list1);
                            sumList1 = list1.ContractAmountSUNList;
                            xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                            xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                            if (list1.ContractAmountSUN != null)
                            {
                                xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                            }
                            xn.sumList = xn.sumList + list1.sumList;
                        }
                    }

                }
                else
                {
                    var t = timeYY.ToInt() - DateYYYY.ToInt();
                    StartTime1 = DateTime.Parse(DateTime.Now.AddYears(-t).ToString("yyyy-01"));
                    //上个月小计
                    decimal? sumList1 = 0;
                    //今年数据
                    //循环12个月的数据
                    for (int i = 0; i < 12; i++)
                    {
                        CapitalDepartmentId list1 = new CapitalDepartmentId();
                        //存当前月份
                        list1.yefen = StartTime1.AddMonths(i).ToString("yyyy-MM");
                        var department = departmentIBLL.GetEntity(DepartmentIdT);
                        if (department != null)
                        {
                            list1.DepartmentIdName = department.F_FullName;
                        }
                        list1.datayyyyMM = StartTime1.AddMonths(i).ToString("yyyy");
                        list1.yefenList = list1.yefen + list1.DepartmentIdName;
                        list1.index = StartTime1.AddMonths(i).ToString("MM").ToInt();
                        //初始化
                        //本月合同额
                        list1.ContractAmountList = 0;
                        //本月绩效
                        list1.EffectiveAmountList = 0;
                        //本月资金
                        list1.ContractAmountSUNList = 0;
                        list1.AmountList = 0;
                        decimal? ContractAmountSUNList1 = 0;
                        //小计
                        list1.sumList = 0;
                        var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime1.ToString(), StartTime1.AddMonths(i).ToString(), StartTime1.AddMonths(i + 1).ToString(), DepartmentIdT);
                        if (data1.ToList().Count > 0)
                        {
                            foreach (var ins in data1)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //历史数据
                                //审核时间<当年
                                //本月绩效
                                if (ins.EffectiveAmount != null)
                                {
                                    //自主
                                    if (ins.ProjectSource == "1")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }
                                    //渠道
                                    if (ins.ProjectSource == "2")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }

                                }
                            }
                        }
                        //今年数据
                        var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime1.ToString(), StartTime1.AddMonths(i).ToString(), StartTime1.AddMonths(i + 1).ToString(), DepartmentIdT);
                        if (data.ToList().Count > 0)
                        {
                            //本月成本                 
                            CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                            if (capitalAmount != null)
                            {
                                list1.ContractAmountSUN = capitalAmount.CostAmount;
                            }
                            foreach (var inf in data)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //本月合同额
                                if (inf.EffectiveAmount != null)
                                {
                                    list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                                }

                                //本月绩效                               
                                var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                                if (inf.ContractAmount <= cay.Amount)
                                {
                                    if (inf.EffectiveAmount != null)
                                    {

                                        //自主
                                        if (inf.ProjectSource == "1")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }
                                        }
                                        //渠道
                                        if (inf.ProjectSource == "2")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }

                                        }


                                    }

                                }

                            }
                            //本月资金
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.ContractAmountSUNList != null)
                            {

                                list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;

                            }
                            list.Add(list1);
                            sumList1 = list1.ContractAmountSUNList;
                            xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                            xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                            if (list1.ContractAmountSUN != null)
                            {
                                xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                            }
                            xn.sumList = xn.sumList + list1.sumList;
                        }
                    }


                }
                xn.ContractAmountSUNList = xn.EffectiveAmountList - xn.ContractAmountSUN;

                list.Add(xn);

            }

            if (DateYYYY != null && DepartmentIdT == "")
            {
                if (DateYYYY.ToInt() == timeYY.ToInt())
                {
                    foreach (var dep in datadepartment)
                    {
                        ////今年数据
                        xn.yefen = "汇总";
                        //qn.yefen = "历史数据";
                        //上个月小计
                        decimal? sumList1 = 0;
                        //decimal? sumList12 = 0;
                        //循环12个月的数据
                        for (int i = 0; i < 12; i++)
                        {
                            CapitalDepartmentId list1 = new CapitalDepartmentId();
                            //存当前月份
                            list1.yefen = StartTime.AddMonths(i).ToString("yyyy-MM");
                            list1.DepartmentIdName = dep.F_FullName;
                            list1.datayyyyMM = StartTime.AddMonths(i).ToString("yyyy");
                            list1.yefenList = list1.yefen + list1.DepartmentIdName;
                            list1.index = StartTime.AddMonths(i).ToString("MM").ToInt();
                            //初始化
                            //本月合同额
                            list1.ContractAmountList = 0;
                            //本月绩效
                            list1.EffectiveAmountList = 0;
                            //本月资金
                            list1.ContractAmountSUNList = 0;
                            list1.AmountList = 0;
                            decimal? ContractAmountSUNList1 = 0;
                            //小计
                            list1.sumList = 0;
                            var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                            if (data1.ToList().Count > 0)
                            {
                                foreach (var ins in data1)
                                {
                                    decimal? EffectiveAmount1 = 0;
                                    //历史数据
                                    //审核时间<当年
                                    //本月绩效
                                    if (ins.EffectiveAmount != null)
                                    {
                                        //自主
                                        if (ins.ProjectSource == "1")
                                        {
                                            EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.AmountList = list1.AmountList + EffectiveAmount1;
                                            }
                                        }
                                        //渠道
                                        if (ins.ProjectSource == "2")
                                        {
                                            EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.AmountList = list1.AmountList + EffectiveAmount1;
                                            }
                                        }

                                    }
                                }
                            }
                            //今年数据
                            var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                            if (data.ToList().Count > 0)
                            {
                                //本月成本                 
                                CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                                if (capitalAmount != null)
                                {
                                    list1.ContractAmountSUN = capitalAmount.CostAmount;
                                }
                                foreach (var inf in data)
                                {
                                    decimal? EffectiveAmount1 = 0;
                                    //本月合同额
                                    if (inf.EffectiveAmount != null)
                                    {
                                        list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                                    }

                                    //本月绩效                               
                                    var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                                    if (inf.ContractAmount <= cay.Amount)
                                    {
                                        if (inf.EffectiveAmount != null)
                                        {

                                            //自主
                                            if (inf.ProjectSource == "1")
                                            {
                                                EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                                {
                                                    list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                                }
                                            }
                                            //渠道
                                            if (inf.ProjectSource == "2")
                                            {
                                                EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                                {
                                                    list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                                }

                                            }


                                        }

                                    }

                                }
                                //本月资金
                                if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                                {
                                    ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                                }
                                if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                                {
                                    ContractAmountSUNList1 = list1.ContractAmountSUN;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                                }
                                if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                                {
                                    ContractAmountSUNList1 = list1.EffectiveAmountList;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                                }
                                if (list1.ContractAmountSUNList != null)
                                {

                                    list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;

                                }
                                list.Add(list1);
                                sumList1 = list1.ContractAmountSUNList;
                                xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                                xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                                if (list1.ContractAmountSUN != null)
                                {
                                    xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                                }
                                xn.sumList = xn.sumList + list1.sumList;
                            }
                        }
                    }
                }
                else
                {
                    var t = timeYY.ToInt() - DateYYYY.ToInt();
                    StartTime1 = DateTime.Parse(DateTime.Now.AddYears(-t).ToString("yyyy-01"));
                    foreach (var dep in datadepartment)
                    {
                        ////今年数据
                        xn.yefen = "汇总";
                        //qn.yefen = "历史数据";
                        //上个月小计
                        decimal? sumList1 = 0;
                        //decimal? sumList12 = 0;
                        //循环12个月的数据
                        for (int i = 0; i < 12; i++)
                        {
                            CapitalDepartmentId list1 = new CapitalDepartmentId();
                            //存当前月份
                            list1.yefen = StartTime1.AddMonths(i).ToString("yyyy-MM");
                            list1.DepartmentIdName = dep.F_FullName;
                            list1.datayyyyMM = StartTime1.AddMonths(i).ToString("yyyy");
                            list1.yefenList = list1.yefen + list1.DepartmentIdName;
                            list1.index = StartTime1.AddMonths(i).ToString("MM").ToInt();
                            //初始化
                            //本月合同额
                            list1.ContractAmountList = 0;
                            //本月绩效
                            list1.EffectiveAmountList = 0;
                            //本月资金
                            list1.ContractAmountSUNList = 0;
                            list1.AmountList = 0;
                            decimal? ContractAmountSUNList1 = 0;
                            //小计
                            list1.sumList = 0;
                            var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime1.ToString(), StartTime1.AddMonths(i).ToString(), StartTime1.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                            if (data1.ToList().Count > 0)
                            {
                                foreach (var ins in data1)
                                {
                                    decimal? EffectiveAmount1 = 0;
                                    //历史数据
                                    //审核时间<当年
                                    //本月绩效
                                    if (ins.EffectiveAmount != null)
                                    {
                                        //自主
                                        if (ins.ProjectSource == "1")
                                        {
                                            EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.AmountList = list1.AmountList + EffectiveAmount1;
                                            }
                                        }
                                        //渠道
                                        if (ins.ProjectSource == "2")
                                        {
                                            EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.AmountList = list1.AmountList + EffectiveAmount1;
                                            }
                                        }

                                    }
                                }
                            }
                            //今年数据
                            var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime1.ToString(), StartTime1.AddMonths(i).ToString(), StartTime1.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                            if (data.ToList().Count > 0)
                            {
                                //本月成本                 
                                CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                                if (capitalAmount != null)
                                {
                                    list1.ContractAmountSUN = capitalAmount.CostAmount;
                                }
                                foreach (var inf in data)
                                {
                                    decimal? EffectiveAmount1 = 0;
                                    //本月合同额
                                    if (inf.EffectiveAmount != null)
                                    {
                                        list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                                    }

                                    //本月绩效                               
                                    var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                                    if (inf.ContractAmount <= cay.Amount)
                                    {
                                        if (inf.EffectiveAmount != null)
                                        {

                                            //自主
                                            if (inf.ProjectSource == "1")
                                            {
                                                EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                                {
                                                    list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                                }
                                            }
                                            //渠道
                                            if (inf.ProjectSource == "2")
                                            {
                                                EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                                {
                                                    list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                                }

                                            }


                                        }

                                    }

                                }
                                //本月资金
                                if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                                {
                                    ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                                }
                                if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                                {
                                    ContractAmountSUNList1 = list1.ContractAmountSUN;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                                }
                                if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                                {
                                    ContractAmountSUNList1 = list1.EffectiveAmountList;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                                }
                                if (list1.ContractAmountSUNList != null)
                                {

                                    list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;

                                }
                                list.Add(list1);
                                sumList1 = list1.ContractAmountSUNList;
                                xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                                xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                                if (list1.ContractAmountSUN != null)
                                {
                                    xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                                }
                                xn.sumList = xn.sumList + list1.sumList;
                            }
                        }
                    }
                }

                xn.ContractAmountSUNList = xn.EffectiveAmountList - xn.ContractAmountSUN;
                list.Add(xn);
            }

            if (DateYYYY == "" && DepartmentIdT != null)
            {

                ////今年数据
                xn.yefen = "汇总";
                //qn.yefen = "历史数据";
                //上个月小计
                decimal? sumList1 = 0;
                //decimal? sumList12 = 0;
                //循环12个月的数据
                for (int i = 0; i < 12; i++)
                {
                    CapitalDepartmentId list1 = new CapitalDepartmentId();
                    //存当前月份
                    list1.yefen = StartTime.AddMonths(i).ToString("yyyy-MM");
                    var department = departmentIBLL.GetEntity(DepartmentIdT);
                    if (department != null)
                    {
                        list1.DepartmentIdName = department.F_FullName;
                    }
                    list1.datayyyyMM = StartTime.AddMonths(i).ToString("yyyy");
                    list1.yefenList = list1.yefen + list1.DepartmentIdName;
                    list1.index = StartTime.AddMonths(i).ToString("MM").ToInt();
                    //初始化
                    //本月合同额
                    list1.ContractAmountList = 0;
                    //本月绩效
                    list1.EffectiveAmountList = 0;
                    //本月资金
                    list1.ContractAmountSUNList = 0;
                    list1.AmountList = 0;
                    decimal? ContractAmountSUNList1 = 0;
                    //小计
                    list1.sumList = 0;
                    var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), DepartmentIdT);
                    if (data1.ToList().Count > 0)
                    {
                        foreach (var ins in data1)
                        {
                            decimal? EffectiveAmount1 = 0;
                            //历史数据
                            //审核时间<当年
                            //本月绩效
                            if (ins.EffectiveAmount != null)
                            {
                                //自主
                                if (ins.ProjectSource == "1")
                                {
                                    EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                    if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                    {
                                        list1.AmountList = list1.AmountList + EffectiveAmount1;
                                    }
                                }
                                //渠道
                                if (ins.ProjectSource == "2")
                                {
                                    EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                    if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                    {
                                        list1.AmountList = list1.AmountList + EffectiveAmount1;
                                    }
                                }

                            }
                        }
                    }
                    //今年数据
                    var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), DepartmentIdT);
                    if (data.ToList().Count > 0)
                    {
                        //本月成本                 
                        CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                        if (capitalAmount != null)
                        {
                            list1.ContractAmountSUN = capitalAmount.CostAmount;
                        }
                        foreach (var inf in data)
                        {
                            decimal? EffectiveAmount1 = 0;
                            //本月合同额
                            if (inf.EffectiveAmount != null)
                            {
                                list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                            }

                            //本月绩效                               
                            var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                            if (inf.ContractAmount <= cay.Amount)
                            {
                                if (inf.EffectiveAmount != null)
                                {

                                    //自主
                                    if (inf.ProjectSource == "1")
                                    {
                                        EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                        }
                                    }
                                    //渠道
                                    if (inf.ProjectSource == "2")
                                    {
                                        EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                        }

                                    }


                                }

                            }

                        }
                        //本月资金
                        if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                        {
                            ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                            list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                        }
                        if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                        {
                            ContractAmountSUNList1 = list1.ContractAmountSUN;
                            list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                        }
                        if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                        {
                            ContractAmountSUNList1 = list1.EffectiveAmountList;
                            list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                        }
                        if (list1.ContractAmountSUNList != null)
                        {

                            list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;

                        }
                        list.Add(list1);
                        sumList1 = list1.ContractAmountSUNList;
                        xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                        xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                        if (list1.ContractAmountSUN != null)
                        {
                            xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                        }
                        xn.sumList = xn.sumList + list1.sumList;
                    }

                }
                xn.ContractAmountSUNList = xn.EffectiveAmountList - xn.ContractAmountSUN;
                list.Add(xn);
            }


            if (DateYYYY == null && DepartmentIdT == null)
            {
                foreach (var dep in datadepartment)
                {
                    ////今年数据
                    xn.yefen = "汇总";
                    //qn.yefen = "历史数据";
                    //上个月小计
                    decimal? sumList1 = 0;
                    //decimal? sumList12 = 0;
                    //循环12个月的数据
                    for (int i = 0; i < 12; i++)
                    {
                        CapitalDepartmentId list1 = new CapitalDepartmentId();
                        //存当前月份
                        list1.yefen = StartTime.AddMonths(i).ToString("yyyy-MM");
                        list1.DepartmentIdName = dep.F_FullName;
                        list1.datayyyyMM = StartTime.AddMonths(i).ToString("yyyy");
                        list1.yefenList = list1.yefen + list1.DepartmentIdName;
                        list1.index = StartTime.AddMonths(i).ToString("MM").ToInt();
                        //初始化
                        //本月合同额
                        list1.ContractAmountList = 0;
                        //本月绩效
                        list1.EffectiveAmountList = 0;
                        //本月资金
                        list1.ContractAmountSUNList = 0;
                        list1.AmountList = 0;
                        decimal? ContractAmountSUNList1 = 0;
                        //小计
                        list1.sumList = 0;
                        var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                        if (data1.ToList().Count > 0)
                        {
                            foreach (var ins in data1)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //历史数据
                                //审核时间<当年
                                //本月绩效
                                if (ins.EffectiveAmount != null)
                                {
                                    //自主
                                    if (ins.ProjectSource == "1")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }
                                    //渠道
                                    if (ins.ProjectSource == "2")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }

                                }
                            }
                        }
                        //今年数据
                        var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                        if (data.ToList().Count > 0)
                        {
                            //本月成本                 
                            CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                            if (capitalAmount != null)
                            {
                                list1.ContractAmountSUN = capitalAmount.CostAmount;
                            }
                            foreach (var inf in data)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //本月合同额
                                if (inf.EffectiveAmount != null)
                                {
                                    list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                                }

                                //本月绩效                               
                                var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                                if (inf.ContractAmount <= cay.Amount)
                                {
                                    if (inf.EffectiveAmount != null)
                                    {

                                        //自主
                                        if (inf.ProjectSource == "1")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }
                                        }
                                        //渠道
                                        if (inf.ProjectSource == "2")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }

                                        }


                                    }

                                }

                            }
                            //本月资金
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.ContractAmountSUNList != null)
                            {

                                list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;

                            }
                            list.Add(list1);
                            sumList1 = list1.ContractAmountSUNList;
                            xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                            xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                            if (list1.ContractAmountSUN != null)
                            {
                                xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                            }
                            xn.sumList = xn.sumList + list1.sumList;
                        }
                    }
                }
                xn.ContractAmountSUNList = xn.EffectiveAmountList - xn.ContractAmountSUN;
                list.Add(xn);
            }

            //降序
            list = list.OrderBy(t => t.index).ThenByDescending(t => t.datayyyyMM).ToList();
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
        /// 获取资金报表数据部门
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns> 
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetCapitalDepartmentId()
        {

            //获取今年的时间
            DateTime StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-01"));
            //接受获取的数据
            List<CapitalDepartmentId> list = new List<CapitalDepartmentId>();
            //今年数据
            CapitalDepartmentId xn = new CapitalDepartmentId();
            xn.ContractAmountList = 0;
            xn.EffectiveAmountList = 0;
            xn.ContractAmountSUN = 0;
            xn.sumList = 0;
            xn.ContractAmountSUNList = 0;
            xn.AmountList = 0;
           
            //获取当前登录人部门
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            if (followPerson.F_MoreDepartmentId != null && followPerson.F_MoreDepartmentId != "")
            {
                string[] strList = followPerson.F_MoreDepartmentId.Split(',');

                for (var y = 0; y < strList.Length; y++)
                {
                    ////今年数据
                    xn.yefen = "汇总";
                    //qn.yefen = "历史数据";
                    //上个月小计
                    decimal? sumList1 = 0;
                    //decimal? sumList12 = 0;
                    //循环12个月的数据
                    for (int i = 0; i < 12; i++)
                    {
                        CapitalDepartmentId list1 = new CapitalDepartmentId();
                        //存当前月份
                        list1.yefen = StartTime.AddMonths(i).ToString("yyyy-MM");
                        var department = departmentIBLL.GetEntity(strList[y]);
                        if (department != null)
                        {
                            list1.DepartmentIdName = department.F_FullName;
                        }
                        list1.datayyyyMM = StartTime.AddMonths(i).ToString("yyyy");
                        list1.yefenList = list1.yefen + list1.DepartmentIdName;
                        list1.index = StartTime.AddMonths(i).ToString("MM").ToInt();

                        //初始化
                        //本月合同额
                        list1.ContractAmountList = 0;
                        //本月绩效
                        list1.EffectiveAmountList = 0;
                        //本月资金
                        list1.ContractAmountSUNList = 0;
                        list1.AmountList = 0;
                    
                        //小计
                        list1.sumList = 0;
                        var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), strList[y]);
                        if (data1.ToList().Count > 0)
                        {
                            foreach (var ins in data1)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //历史数据
                                //审核时间<当年
                                //本月绩效
                                if (ins.EffectiveAmount != null)
                                {
                                    //自主
                                    if (ins.ProjectSource == "1")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }
                                    //渠道
                                    if (ins.ProjectSource == "2")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }

                                }
                            }
                        }
                        //今年数据
                        var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), strList[y]);
                        if (data.ToList().Count > 0)
                        {
                            //本月成本                 
                            CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                            if (capitalAmount != null)
                            {
                                list1.ContractAmountSUN = capitalAmount.CostAmount;
                            }
                            foreach (var inf in data)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //本月合同额
                                if (inf.EffectiveAmount != null)
                                {
                                    list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                                }

                                //本月绩效                               
                                var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                                if (inf.ContractAmount <= cay.Amount)
                                {
                                    if (inf.EffectiveAmount != null)
                                    {

                                        //自主
                                        if (inf.ProjectSource == "1")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }
                                        }
                                        //渠道
                                        if (inf.ProjectSource == "2")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }

                                        }


                                    }

                                }

                            }
                            decimal? ContractAmountSUNList1 = 0;
                            //本月资金
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.ContractAmountSUNList != null)
                            {

                                list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;

                            }
                            list.Add(list1);
                            sumList1 = list1.ContractAmountSUNList;
                            xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                            xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                            if (list1.ContractAmountSUN != null)
                            {
                                xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                            }
                            xn.sumList = xn.sumList + list1.sumList;
                            if (list1.AmountList != null)
                            {
                                xn.AmountList = xn.AmountList + list1.AmountList;
                            }
                        }
                    }

                }

            }
            else
            {
                ////今年数据
                xn.yefen = "汇总";
                //qn.yefen = "历史数据";
                //上个月小计
                decimal? sumList1 = 0;
                //decimal? sumList12 = 0;
                //循环12个月的数据
                for (int i = 0; i < 12; i++)
                {
                    CapitalDepartmentId list1 = new CapitalDepartmentId();
                    //存当前月份
                    list1.yefen = StartTime.AddMonths(i).ToString("yyyy-MM");
                    var department = departmentIBLL.GetEntity(followPerson.F_DepartmentId);
                    if (department != null)
                    {
                        list1.DepartmentIdName = department.F_FullName;
                    }
                    list1.datayyyyMM = StartTime.AddMonths(i).ToString("yyyy");
                    list1.yefenList = list1.yefen + list1.DepartmentIdName;
                    list1.index = StartTime.AddMonths(i).ToString("MM").ToInt();

                    //初始化
                    //本月合同额
                    list1.ContractAmountList = 0;
                    //本月绩效
                    list1.EffectiveAmountList = 0;
                    //本月资金
                    list1.ContractAmountSUNList = 0;
                    list1.AmountList = 0;
                    decimal? ContractAmountSUNList1 = 0;
                    //小计
                    list1.sumList = 0;
                    var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), followPerson.F_DepartmentId);
                    if (data1.ToList().Count > 0)
                    {
                        foreach (var ins in data1)
                        {
                            decimal? EffectiveAmount1 = 0;
                            //历史数据
                            //审核时间<当年
                            //本月绩效
                            if (ins.EffectiveAmount != null)
                            {
                                //自主
                                if (ins.ProjectSource == "1")
                                {
                                    EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                    if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                    {
                                        list1.AmountList = list1.AmountList + EffectiveAmount1;
                                    }
                                }
                                //渠道
                                if (ins.ProjectSource == "2")
                                {
                                    EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                    if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                    {
                                        list1.AmountList = list1.AmountList + EffectiveAmount1;
                                    }
                                }

                            }
                        }
                    }
                    //今年数据
                    var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), followPerson.F_DepartmentId);
                    if (data.ToList().Count > 0)
                    {
                        //本月成本                 
                        CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                        if (capitalAmount != null)
                        {
                            list1.ContractAmountSUN = capitalAmount.CostAmount;
                        }
                        foreach (var inf in data)
                        {
                            decimal? EffectiveAmount1 = 0;
                            //本月合同额
                            if (inf.EffectiveAmount != null)
                            {
                                list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                            }

                            //本月绩效                               
                            var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                            if (inf.ContractAmount <= cay.Amount)
                            {
                                if (inf.EffectiveAmount != null)
                                {

                                    //自主
                                    if (inf.ProjectSource == "1")
                                    {
                                        EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                        }
                                    }
                                    //渠道
                                    if (inf.ProjectSource == "2")
                                    {
                                        EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                        }

                                    }


                                }

                            }

                        }
                        //本月资金
                        if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                        {
                            ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                            list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                        }
                        if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                        {
                            ContractAmountSUNList1 = list1.ContractAmountSUN;
                            list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                        }
                        if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                        {
                            ContractAmountSUNList1 = list1.EffectiveAmountList;
                            list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                        }
                        if (list1.ContractAmountSUNList != null)
                        {

                            list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;

                        }
                        list.Add(list1);
                        sumList1 = list1.ContractAmountSUNList;
                        xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                        xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                        if (list1.ContractAmountSUN != null)
                        {
                            xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                        }
                        xn.sumList = xn.sumList + list1.sumList;
                        if (list1.AmountList != null)
                        {
                            xn.AmountList = xn.AmountList + list1.AmountList;
                        }
                    }
                }
            }
            xn.ContractAmountSUNList = xn.EffectiveAmountList - xn.ContractAmountSUN;
            list.Add(xn);



            return Success(list);
        }
       
        
        
        /// <summary>
        /// 成本添加
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="strEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult CapitalAmountSaveForm(decimal costAmount, string yearMonth)
        {
            reportFormsBLL.CapitalAmountSaveForm(costAmount, yearMonth);
            return Success("保存成功。");
        }

        /// <summary>
        /// 多部门获取营销报表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        /// 

        [HttpGet]
        [AjaxOnly]
        public ActionResult GetMarketingsDepartmentId(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();


            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            List<MarketingEntity> listdate = new List<MarketingEntity>();

            if (followPerson.F_MoreDepartmentId != null)
            {

                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = reportFormsBLL.GetMarketingsDepartmentId(paginationobj, queryJson, deps);
                if (data.ToList().Count > 0)
                {
                    foreach (var info in data)
                    {

                        if (info.SubAmount != null && info.MainAmount != null)
                        {
                            for (var i = 0; i < strList.Length; i++)
                            {
                                if (strList[i] == info.SubDepartmentId)
                                {

                                    info.ContractAmount = info.SubAmount;
                                    var pay = projectPaymentIBLL.GetProjectPaymentByprojectId(info.Id);
                                    if (pay.PaymentAmount != null)
                                    {

                                        var p = pay.PaymentAmount.ToInt() / 2;
                                        info.EffectiveAmount = info.SubAmount - p;

                                    }
                                    else
                                    {
                                        info.EffectiveAmount = info.SubAmount;
                                    }


                                }
                                else if (strList[i] == info.MainDepartmentId)
                                {
                                    info.ContractAmount = info.MainAmount;
                                    var pay = projectPaymentIBLL.GetProjectPaymentByprojectId(info.Id);
                                    if (pay.PaymentAmount != null)
                                    {

                                        var p = pay.PaymentAmount.ToInt() / 2;
                                        info.EffectiveAmount = info.MainAmount - p;

                                    }
                                    else
                                    {
                                        info.EffectiveAmount = info.MainAmount;
                                    }
                                }
                            }
                        }
                        else
                        {
                            info.ContractAmount = info.ContractAmount;
                            info.EffectiveAmount = info.EffectiveAmount;
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
        /// 合作伙伴的营销台账
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetMarketingsHZ(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = reportFormsBLL.GetMarketingsHZ(paginationobj, queryJson);

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
        /// 营销合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetMarketingsSumdc(string queryJson)
        {
            // var dataItem = reportFormsBLL.GetMarketings(queryJson);
            //var dataItem = reportFormsBLL.GetMarketingsSum_new(queryJson);
            List<MarketingEntity> list = new List<MarketingEntity>();
            var u = LoginUserInfo.Get().userId;
            var user = userIBLL.GetHZUserId(u);
            string deps = " ( t.DepartmentId='" + user.F_DepartmentId + "' or a.PDepartmentId='" + user.F_DepartmentId + "' or t.SubDepartmentId='" + user.F_DepartmentId + "' or t.MainDepartmentId='" + user.F_DepartmentId + "') ";
            var dataItem = reportFormsBLL.GetMarketingsdc(queryJson, deps);
            decimal ContractAmountSum = 0;
            decimal AmountSum = 0;
            decimal NotReceivedSum = 0;
            decimal OwnSum = 0;
            decimal DitchSum = 0;
            decimal ConsociationSum = 0;
            foreach (var inf in dataItem)
            {
                if (inf.SubAmount != null && inf.MainAmount != null)
                {
                    if (user.F_DepartmentId == inf.SubDepartmentId)
                    {

                        inf.ContractAmount = inf.SubAmount;
                        var pay = projectPaymentIBLL.GetProjectPaymentByprojectId(inf.Id);
                        if (pay.PaymentAmount != null)
                        {

                            var p = pay.PaymentAmount.ToInt() / 2;
                            inf.EffectiveAmount = inf.SubAmount - p;

                        }
                        else
                        {
                            inf.EffectiveAmount = inf.SubAmount;
                        }


                    }
                    else if (user.F_DepartmentId == inf.MainDepartmentId)
                    {
                        inf.ContractAmount = inf.MainAmount;
                        var pay = projectPaymentIBLL.GetProjectPaymentByprojectId(inf.Id);
                        if (pay.PaymentAmount != null)
                        {

                            var p = pay.PaymentAmount.ToInt() / 2;
                            inf.EffectiveAmount = inf.MainAmount - p;

                        }
                        else
                        {
                            inf.EffectiveAmount = inf.MainAmount;
                        }
                    }
                }
                else
                {
                    inf.ContractAmount = inf.ContractAmount;
                    inf.EffectiveAmount = inf.EffectiveAmount;
                }
                ContractAmountSum = ContractAmountSum + inf.ContractAmount.Value;
                AmountSum = AmountSum + inf.Amount.Value;
                NotReceivedSum = NotReceivedSum + inf.NotReceived.Value;
                if (inf.ProjectSource == 1.ToString())
                {
                    OwnSum = OwnSum + inf.ContractAmount.Value;
                }
                if (inf.ProjectSource == 2.ToString())
                {
                    DitchSum = DitchSum + inf.ContractAmount.Value;
                }
                if (inf.ProjectSource == 3.ToString())
                {
                    ConsociationSum = ConsociationSum + inf.ContractAmount.Value;
                }

            }


            var jsonData = new
            {
                data = dataItem,
                ContractAmountSum = ContractAmountSum,
                AmountSum = AmountSum,
                NotReceivedSum = NotReceivedSum,
                OwnSum = OwnSum,
                DitchSum = DitchSum,
                ConsociationSum = ConsociationSum
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 营销合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetMarketingsSum(string queryJson)
        {
            // var dataItem = reportFormsBLL.GetMarketings(queryJson);
            var dataItem = reportFormsBLL.GetMarketingsSum_new(queryJson);

            decimal ContractAmountSum = 0;
            decimal AmountSum = 0;
            decimal NotReceivedSum = 0;
            decimal OwnSum = 0;
            decimal DitchSum = 0;
            decimal ConsociationSum = 0;
            foreach (var item in dataItem)
            {
                ContractAmountSum = ContractAmountSum + item.ContractAmount.Value;
                AmountSum = AmountSum + item.Amount.Value;
                NotReceivedSum = NotReceivedSum + item.NotReceived.Value;
                if (item.ProjectSource == 1.ToString())
                {
                    OwnSum = OwnSum + item.ContractAmount.Value;
                }
                if (item.ProjectSource == 2.ToString())
                {
                    DitchSum = DitchSum + item.ContractAmount.Value;
                }
                if (item.ProjectSource == 3.ToString())
                {
                    ConsociationSum = ConsociationSum + item.ContractAmount.Value;
                }
            };
            var jsonData = new
            {
                data = dataItem,
                ContractAmountSum = ContractAmountSum,
                AmountSum = AmountSum,
                NotReceivedSum = NotReceivedSum,
                OwnSum = OwnSum,
                DitchSum = DitchSum,
                ConsociationSum = ConsociationSum
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
        public ActionResult GetMarketingsAlldc(string queryJson)
        {

            //var dataItem = reportFormsBLL.GetMarketings1(queryJson);

            List<MarketingEntity> list = new List<MarketingEntity>();
            var u = LoginUserInfo.Get().userId;
            var user = userIBLL.GetHZUserId(u);
            string deps = " ( t.DepartmentId='" + user.F_DepartmentId + "' or a.PDepartmentId='" + user.F_DepartmentId + "' or t.SubDepartmentId='" + user.F_DepartmentId + "' or t.MainDepartmentId='" + user.F_DepartmentId + "') ";
            var dataItem = reportFormsBLL.GetMarketingsdc(queryJson, deps);
            foreach (var inf in dataItem)
            {
                if (inf.SubAmount != null && inf.MainAmount != null)
                {
                    if (user.F_DepartmentId == inf.SubDepartmentId)
                    {

                        inf.ContractAmount = inf.SubAmount;
                        var pay = projectPaymentIBLL.GetProjectPaymentByprojectId(inf.Id);
                        if (pay.PaymentAmount != null)
                        {

                            var p = pay.PaymentAmount.ToInt() / 2;
                            inf.EffectiveAmount = inf.SubAmount - p;

                        }
                        else
                        {
                            inf.EffectiveAmount = inf.SubAmount;
                        }


                    }
                    else if (user.F_DepartmentId == inf.MainDepartmentId)
                    {
                        inf.ContractAmount = inf.MainAmount;
                        var pay = projectPaymentIBLL.GetProjectPaymentByprojectId(inf.Id);
                        if (pay.PaymentAmount != null)
                        {

                            var p = pay.PaymentAmount.ToInt() / 2;
                            inf.EffectiveAmount = inf.MainAmount - p;

                        }
                        else
                        {
                            inf.EffectiveAmount = inf.MainAmount;
                        }
                    }
                }
                else
                {
                    inf.ContractAmount = inf.ContractAmount;
                    inf.EffectiveAmount = inf.EffectiveAmount;
                }

                list.Add(inf);
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
        /// 导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetMarketingsAll(string queryJson)
        {
            var dataItem = reportFormsBLL.GetMarketings(queryJson);

            List<MarketingEntity> list = new List<MarketingEntity>();
            foreach (var inf in dataItem)
            {
                list.Add(inf);
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
        /// 多部门导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetMarketingsDepartmentIdAll(string queryJson)
        {
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            List<MarketingEntity> list = new List<MarketingEntity>();

            if (followPerson.F_MoreDepartmentId != null)
            {

                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = reportFormsBLL.GetMarketingsSum_newDepartmentId(queryJson, deps);
                if (data.ToList().Count > 0)
                {

                    foreach (var inf in data)
                    {
                        list.Add(inf);
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
        /// <summary>
        /// 多部门营销合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetMarketingsSumDepartmentId(string queryJson)
        {
            decimal ContractAmountSum = 0;
            decimal AmountSum = 0;
            decimal NotReceivedSum = 0;
            decimal OwnSum = 0;
            decimal DitchSum = 0;
            decimal ConsociationSum = 0;
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            List<MarketingEntity> listdate = new List<MarketingEntity>();

            if (followPerson.F_MoreDepartmentId != null)
            {

                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = reportFormsBLL.GetMarketingsSum_newDepartmentId(queryJson, deps);
                if (data.ToList().Count > 0)
                {
                    foreach (var item in data)
                    {

                        listdate.Add(item);

                        ContractAmountSum = ContractAmountSum + item.ContractAmount.Value;
                        AmountSum = AmountSum + item.Amount.Value;
                        NotReceivedSum = NotReceivedSum + item.NotReceived.Value;
                        if (item.ProjectSource == 1.ToString())
                        {
                            OwnSum = OwnSum + item.ContractAmount.Value;
                        }
                        if (item.ProjectSource == 2.ToString())
                        {
                            DitchSum = DitchSum + item.ContractAmount.Value;
                        }
                        if (item.ProjectSource == 3.ToString())
                        {
                            ConsociationSum = ConsociationSum + item.ContractAmount.Value;
                        }
                    }
                }
            }






            var jsonData = new
            {
                data = listdate,
                ContractAmountSum = ContractAmountSum,
                AmountSum = AmountSum,
                NotReceivedSum = NotReceivedSum,
                OwnSum = OwnSum,
                DitchSum = DitchSum,
                ConsociationSum = ConsociationSum
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 合作伙伴营销台账合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetMarketingsSumHZ(string queryJson)
        {
            var dataItem = reportFormsBLL.GetMarketingsSumHZ(queryJson);
            decimal ContractAmountSum = 0;
            decimal AmountSum = 0;
            decimal NotReceivedSum = 0;
            decimal OwnSum = 0;
            decimal DitchSum = 0;
            decimal ConsociationSum = 0;
            foreach (var item in dataItem)
            {
                ContractAmountSum = ContractAmountSum + item.ContractAmount.Value;
                AmountSum = AmountSum + item.Amount.Value;
                NotReceivedSum = NotReceivedSum + item.NotReceived.Value;
                if (item.ProjectSource == 1.ToString())
                {
                    OwnSum = OwnSum + item.ContractAmount.Value;
                }
                if (item.ProjectSource == 2.ToString())
                {
                    DitchSum = DitchSum + item.ContractAmount.Value;
                }
                if (item.ProjectSource == 3.ToString())
                {
                    ConsociationSum = ConsociationSum + item.ContractAmount.Value;
                }
            };
            var jsonData = new
            {
                data = dataItem,
                ContractAmountSum = ContractAmountSum,
                AmountSum = AmountSum,
                NotReceivedSum = NotReceivedSum,
                OwnSum = OwnSum,
                DitchSum = DitchSum,
                ConsociationSum = ConsociationSum
            };
            return Success(jsonData);
        }

        /// <summary>
        /// 获取生产报表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>



        [HttpGet]
        [AjaxOnly]
        public ActionResult GetProductions(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = reportFormsBLL.GetProductions(paginationobj, queryJson);


            List<ProductionEntity> marketings = new List<ProductionEntity>();
            foreach (var item in data)
            {
                if (!marketings.Contains(item))
                {
                    marketings.Add(item);
                }
            }
            var jsonData = new
            {
                rows = marketings,
                total = paginationobj.total,
                page = paginationobj.page,
                records = paginationobj.records
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 多部门获取生产报表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>



        [HttpGet]
        [AjaxOnly]
        public ActionResult GetProductionsDepartmentId(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            List<ProductionEntity> marketings = new List<ProductionEntity>();

            if (followPerson.F_MoreDepartmentId != null)
            {

                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = reportFormsBLL.GetProductionsDepartmentId(paginationobj, queryJson, deps);
                if (data.ToList().Count > 0)
                {
                    foreach (var item in data)
                    {

                        marketings.Add(item);


                    }
                }
            }


            var jsonData = new
            {
                rows = marketings,
                total = paginationobj.total,
                page = paginationobj.page,
                records = paginationobj.records
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 生产合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetProductionsSum(string queryJson)
        {
           // var dataItem = reportFormsBLL.GetProductions(queryJson);
         /*   //List<ProductionEntity> marketingsItem = new List<ProductionEntity>();
            //foreach (var item in dataItem)
            //{
            //    if (!marketingsItem.Contains(item))
            //    {
            //        /*var bol = true;
            //        foreach (var list in marketingsItem)
            //        {
            //            if (list.ContractNo == item.ContractNo && list.ProjectName == item.ProjectName)
            //            {
            //                list.Amount += item.Amount;
            //                list.NotReceived -= item.Amount;
            //                bol = false;
            //            }

            //        }
            //        if (bol) marketingsItem.Add(item);
            //        marketingsItem.Add(item);
            //    }
            //}
            decimal ContractAmountSum = 0;
            decimal OwnSum = 0;
            decimal DitchSum = 0;
            decimal ConsociationSum = 0;
            // decimal? SCJXAmountSum = 0;
            //decimal? Sum = 0;
            foreach (var item in dataItem)
            {

                ContractAmountSum = ContractAmountSum + item.ContractAmount.Value;
                if (item.ProjectSource == 1.ToString())
                {
                    OwnSum = OwnSum + item.ContractAmount.Value;
                }
                if (item.ProjectSource == 2.ToString())
                {
                    DitchSum = DitchSum + item.ContractAmount.Value;
                }
                if (item.ProjectSource == 3.ToString())
                {
                    ConsociationSum = ConsociationSum + item.ContractAmount.Value;
                }
*/

                var dataItem = reportFormsBLL.GetProductions(queryJson);
            decimal ContractAmountSum = 0;
            decimal OwnSum = 0;
            decimal DitchSum = 0;
            decimal ConsociationSum = 0;
            foreach (var item in dataItem)
                {
                    ContractAmountSum = ContractAmountSum + item.ContractAmount.Value;
                    if (item.ProjectSource == 1.ToString())
                    {
                        OwnSum = OwnSum + item.ContractAmount.Value;
                    }
                    if (item.ProjectSource == 2.ToString())
                    {
                        DitchSum = DitchSum + item.ContractAmount.Value;
                    }
                    if (item.ProjectSource == 3.ToString())
                    {
                        ConsociationSum = ConsociationSum + item.ContractAmount.Value;
                    }
                };
                var jsonData = new
                {
                    data = dataItem,
                    ContractAmountSum = ContractAmountSum,
                    OwnSum = OwnSum,
                    DitchSum = DitchSum,
                    ConsociationSum = ConsociationSum
                };
                return Success(jsonData);

        /*    };

            var jsonData = new
            {
                data = dataItem,
                ContractAmountSum = ContractAmountSum,
                OwnSum = OwnSum,
                DitchSum = DitchSum,
                ConsociationSum = ConsociationSum,
                //SCJXAmountSum = SCJXAmountSum
            };
            return Success(jsonData);*/
        }
        /// <summary>
        /// 生产导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetProductionsAll(string queryJson)
        {
            List<ProductionEntity> list = new List<ProductionEntity>();
            var dataItem = reportFormsBLL.GetProductions(queryJson);
            foreach (var item in dataItem)
            {
                list.Add(item);
            };
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
        /// 多部门生产导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetProductionsDepartmentIdAll(string queryJson)
        {


            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            List<ProductionEntity> list = new List<ProductionEntity>();

            if (followPerson.F_MoreDepartmentId != null)
            {

                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = reportFormsBLL.GetProductionsDepartmentId(queryJson, deps);
                if (data.ToList().Count > 0)
                {
                    foreach (var item in data)
                    {
                        list.Add(item);
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
        /// <summary>
        /// 多部门生产合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetProductionsSumDepartmentId(string queryJson)
        {

            decimal ContractAmountSum = 0;
            decimal OwnSum = 0;
            decimal DitchSum = 0;
            decimal ConsociationSum = 0;
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            List<ProductionEntity> marketings = new List<ProductionEntity>();

            if (followPerson.F_MoreDepartmentId != null)
            {

                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = reportFormsBLL.GetProductionsDepartmentId(queryJson, deps);
                if (data.ToList().Count > 0)
                {
                    foreach (var item in data)
                    {
                        ContractAmountSum = ContractAmountSum + item.ContractAmount.Value;
                        if (item.ProjectSource == 1.ToString())
                        {
                            OwnSum = OwnSum + item.ContractAmount.Value;
                        }
                        if (item.ProjectSource == 2.ToString())
                        {
                            DitchSum = DitchSum + item.ContractAmount.Value;
                        }
                        if (item.ProjectSource == 3.ToString())
                        {
                            ConsociationSum = ConsociationSum + item.ContractAmount.Value;
                        }
                        marketings.Add(item);


                    }
                }
            }





            var jsonData = new
            {
                data = marketings,
                ContractAmountSum = ContractAmountSum,
                OwnSum = OwnSum,
                DitchSum = DitchSum,
                ConsociationSum = ConsociationSum,
                //SCJXAmountSum = SCJXAmountSum
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 结算导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetSettleAccountsSumAll(string queryJson)
        {
            var dataItem = reportFormsBLL.GetSettleAccountsSum_new(queryJson);
            List<SettleAccountsEntity> list = new List<SettleAccountsEntity>();
            decimal? AmountSum = 0;
            foreach (var info in dataItem)
            {
                AmountSum = AmountSum + info.Amount;
                list.Add(info);
            }

            //放入缓存
            var uuid = Guid.NewGuid().ToString().Replace("-", "");
            cache.Write(uuid, JsonConvert.SerializeObject(list));
            var jsonData = new
            {
                AmountSum = AmountSum,
                rows = uuid
            };

            return Success(jsonData);
        }
        /// <summary>
        /// 多部门结算导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetSettleAccountsSumAllDepartmentId(string queryJson)
        {
            // var dataItem = reportFormsBLL.GetSettleAccountsSum_new(queryJson);

            List<SettleAccountsEntity> list = new List<SettleAccountsEntity>();
            decimal? AmountSum = 0;

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
                        deps += " ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = reportFormsBLL.GetSettleAccountsSum_newDepartmentId(queryJson, deps);
                if (data.ToList().Count > 0)
                {
                    foreach (var info in data)
                    {
                        AmountSum = AmountSum + info.Amount;
                        list.Add(info);


                    }
                }
            }
            //放入缓存
            var uuid = Guid.NewGuid().ToString().Replace("-", "");
            cache.Write(uuid, JsonConvert.SerializeObject(list));
            var jsonData = new
            {
                AmountSum = AmountSum,
                rows = uuid
            };

            return Success(jsonData);
        }
        /// <summary>
        /// 合作伙伴结算台账导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetSettleAccountsSumAllHZ(string queryJson)
        {
            var dataItem = reportFormsBLL.GetSettleAccountsHZ(queryJson);
            List<SettleAccountsEntity> list = new List<SettleAccountsEntity>();
            decimal? AmountSum = 0;
            foreach (var info in dataItem)
            {
                AmountSum = AmountSum + info.Amount;
                list.Add(info);
            }

            //放入缓存
            var uuid = Guid.NewGuid().ToString().Replace("-", "");
            cache.Write(uuid, JsonConvert.SerializeObject(list));
            var jsonData = new
            {
                AmountSum = AmountSum,
                rows = uuid
            };

            return Success(jsonData);
        }
        /// <summary>
        /// 获取结算报表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        /// 

        [HttpGet]
        [AjaxOnly]
        public ActionResult GetSettleAccounts(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = reportFormsBLL.GetSettleAccounts(paginationobj, queryJson);
            decimal? AmountSum = 0;
            foreach (var lr in data)
            {
                AmountSum = AmountSum + lr.Amount;
            }

            var jsonData = new
            {
                AmountSum = AmountSum,
                rows = data,
                total = paginationobj.total,
                page = paginationobj.page,
                records = paginationobj.records
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 获取结算报表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        /// 

        [HttpGet]
        [AjaxOnly]
        public ActionResult GetSettleAccountsDepartmentId(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();

            decimal? AmountSum = 0;

            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            List<SettleAccountsEntity> marketings = new List<SettleAccountsEntity>();

            if (followPerson.F_MoreDepartmentId != null)
            {

                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = reportFormsBLL.GetSettleAccountsDepartmentId(paginationobj, queryJson, deps);
                if (data.ToList().Count > 0)
                {
                    foreach (var item in data)
                    {
                        AmountSum = AmountSum + item.Amount;
                        marketings.Add(item);


                    }
                }
            }
            var jsonData = new
            {
                AmountSum = AmountSum,
                rows = marketings,
                total = paginationobj.total,
                page = paginationobj.page,
                records = paginationobj.records
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 合作伙伴结算部门台账
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetSettleAccountsHZ(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = reportFormsBLL.GetSettleAccountsHZ(paginationobj, queryJson);
            decimal? AmountSum = 0;
            foreach (var lr in data)
            {
                AmountSum = AmountSum + lr.Amount;
            }

            var jsonData = new
            {
                AmountSum = AmountSum,
                rows = data,
                total = paginationobj.total,
                page = paginationobj.page,
                records = paginationobj.records
            };
            return Success(jsonData);
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
        public ActionResult SaveFormReportForms(string keyValue, string strEntity)
        {
            ProjectContractEntity entity = strEntity.ToObject<ProjectContractEntity>();
            projectContractIBLL.SaveFormReportForms(keyValue, entity);

            return Success("结算比例添加成功！", "合作伙伴结算台账", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, entity.ToJson());

        }
        /// <summary>
        /// 结算合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetSettleAccountsSum(string queryJson)
        {
            //var dataItem = reportFormsBLL.GetSettleAccounts(queryJson);
            var dataItem = reportFormsBLL.GetSettleAccountsSum_new(queryJson);
            decimal? ContractAmountSum = 0;
            decimal? BillingAmountSum = 0;
            decimal? AmountSum = 0;
            decimal? NotReceivedSum = 0;
            decimal? PaymentAmountSum = 0;
            decimal? OwnSum = 0;
            decimal? DitchSum = 0;
            decimal? ConsociationSum = 0;
            // decimal? QKSum = 0;
            //decimal? Sum = 0;
            foreach (var item in dataItem)
            {
                ContractAmountSum = ContractAmountSum + item.ContractAmount;
                AmountSum = AmountSum + item.Amount;
                NotReceivedSum = NotReceivedSum + item.NotReceived;
                PaymentAmountSum = PaymentAmountSum + item.PaymentAmount;
                BillingAmountSum = BillingAmountSum + item.BillingAmount;

                if (item.ProjectSource == 1.ToString())
                {
                    OwnSum = OwnSum + item.ContractAmount;
                }
                if (item.ProjectSource == 2.ToString())
                {
                    DitchSum = DitchSum + item.ContractAmount;
                }
                if (item.ProjectSource == 3.ToString())
                {
                    ConsociationSum = ConsociationSum + item.ContractAmount;
                }
                //全款绩效
                //if (item.ProjectSource == "1" && item.EffectiveAmount != null)
                //{

                //    Sum = (decimal)0.3*item.EffectiveAmount;

                //}
                //if (item.ProjectSource == "2" && item.EffectiveAmount != null)
                //{
                //    Sum = ((decimal?)0.03 * item.EffectiveAmount);

                //}
                //QKSum = QKSum + Sum;

            };
            var jsonData = new
            {
                data = dataItem,
                ContractAmountSum = ContractAmountSum,
                BillingAmountSum = BillingAmountSum,
                AmountSum = AmountSum,
                NotReceivedSum = NotReceivedSum,
                PaymentAmountSum = PaymentAmountSum,
                OwnSum = OwnSum,
                DitchSum = DitchSum,
                ConsociationSum = ConsociationSum,
                // QKSum = QKSum
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 多部门结算合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetSettleAccountsSumDepartmentId(string queryJson)
        {

            decimal? ContractAmountSum = 0;
            decimal? BillingAmountSum = 0;
            decimal? AmountSum = 0;
            decimal? NotReceivedSum = 0;
            decimal? PaymentAmountSum = 0;
            decimal? OwnSum = 0;
            decimal? DitchSum = 0;
            decimal? ConsociationSum = 0;


            List<SettleAccountsEntity> list = new List<SettleAccountsEntity>();


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
                        deps += " ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = reportFormsBLL.GetSettleAccountsSum_newDepartmentId(queryJson, deps);
                if (data.ToList().Count > 0)
                {
                    foreach (var item in data)
                    {
                        ContractAmountSum = ContractAmountSum + item.ContractAmount;
                        AmountSum = AmountSum + item.Amount;
                        NotReceivedSum = NotReceivedSum + item.NotReceived;
                        PaymentAmountSum = PaymentAmountSum + item.PaymentAmount;
                        BillingAmountSum = BillingAmountSum + item.BillingAmount;

                        if (item.ProjectSource == 1.ToString())
                        {
                            OwnSum = OwnSum + item.ContractAmount;
                        }
                        if (item.ProjectSource == 2.ToString())
                        {
                            DitchSum = DitchSum + item.ContractAmount;
                        }
                        if (item.ProjectSource == 3.ToString())
                        {
                            ConsociationSum = ConsociationSum + item.ContractAmount;
                        }
                        list.Add(item);


                    }
                }
            }



            var jsonData = new
            {
                data = list,
                ContractAmountSum = ContractAmountSum,
                BillingAmountSum = BillingAmountSum,
                AmountSum = AmountSum,
                NotReceivedSum = NotReceivedSum,
                PaymentAmountSum = PaymentAmountSum,
                OwnSum = OwnSum,
                DitchSum = DitchSum,
                ConsociationSum = ConsociationSum,
                // QKSum = QKSum
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 合作伙伴结算台账合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetSettleAccountsSumHZ(string queryJson)
        {
            var dataItem = reportFormsBLL.GetSettleAccountsSumHZ(queryJson);
            decimal? ContractAmountSum = 0;
            decimal? BillingAmountSum = 0;
            decimal? AmountSum = 0;
            decimal? NotReceivedSum = 0;
            decimal? PaymentAmountSum = 0;
            decimal? OwnSum = 0;
            decimal? DitchSum = 0;
            decimal? ConsociationSum = 0;

            foreach (var item in dataItem)
            {
                ContractAmountSum = ContractAmountSum + item.ContractAmount;
                AmountSum = AmountSum + item.Amount;
                NotReceivedSum = NotReceivedSum + item.NotReceived;
                PaymentAmountSum = PaymentAmountSum + item.PaymentAmount;
                BillingAmountSum = BillingAmountSum + item.BillingAmount;

                if (item.ProjectSource == 1.ToString())
                {
                    OwnSum = OwnSum + item.ContractAmount;
                }
                if (item.ProjectSource == 2.ToString())
                {
                    DitchSum = DitchSum + item.ContractAmount;
                }
                if (item.ProjectSource == 3.ToString())
                {
                    ConsociationSum = ConsociationSum + item.ContractAmount;
                }


            };
            var jsonData = new
            {
                data = dataItem,
                ContractAmountSum = ContractAmountSum,
                BillingAmountSum = BillingAmountSum,
                AmountSum = AmountSum,
                NotReceivedSum = NotReceivedSum,
                PaymentAmountSum = PaymentAmountSum,
                OwnSum = OwnSum,
                DitchSum = DitchSum,
                ConsociationSum = ConsociationSum,

            };
            return Success(jsonData);
        }

        #endregion

        /// <summary>
        /// 合计
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        /// 




        #region 页面
        [HttpGet]
        public ActionResult MarketingReportForms1()
        {
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            if (followPerson.F_MoreDepartmentId != null)
            {
                return View("MarketingReportFormsDepartmentId");
            }
            return View();
        }

        [HttpGet]
        public ActionResult MarketingReportForms()
        {
            /*var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            if (followPerson.F_MoreDepartmentId != null)
            {
                return View("MarketingReportFormsDepartmentId");
            }*/
            return View();
        }
        public ActionResult MarketingReportFormsDepartmentId()
        {
            return View();
        }
        /// <summary>
        /// 合作伙伴营销台账
        /// </summary>
        /// <returns></returns>
        public ActionResult MarketingReportFormsHZ()
        {
            return View();
        }


        /// <summary>
        /// 获取生产报表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ProductionsReportForms()
        {
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            if (followPerson.F_MoreDepartmentId != null)
            {
                return View("ProductionsReportFormsDepartmentId");
            }
            return View();
        }

        /// <summary>
        /// 获取结算报表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SettleAccountsReportForms()
        {
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            if (followPerson.F_MoreDepartmentId != null)
            {
                return View("SettleAccountsReportFormsDepartmentId");
            }
            return View();
        }
        /// <summary>
        /// 获取合作伙伴结算报表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SettleAccountsReportFormsHZ()
        {
            return View();
        }
        /// <summary>
        /// 合作伙伴结算报表数据添加比例
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SettleAccountsAddHZ()
        {
            return View();
        }
        /// <summary>
        /// 资金台账部门
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CapitalDepartmentId()
        {
            return View();
        }
        /// <summary>
        /// 资金台账部门
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CapitalDepartmentIdList()
        {
            return View();
        }
        #endregion

    }
}
