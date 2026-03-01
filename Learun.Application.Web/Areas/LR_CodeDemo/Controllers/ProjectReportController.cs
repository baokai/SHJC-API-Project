using Learun.Application.TwoDevelopment.LR_CodeDemo;
using Learun.Util;
using System.Web.Mvc;

namespace Learun.Application.Web.Areas.LR_CodeDemo.Controllers
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
    /// 创 建：超级管理员
    /// 日 期：2022-03-10 22:29
    /// 描 述：项目管理
    /// </summary>
    public class ProjectReportController : MvcControllerBase
    {
        private ReportTempIBLL reportTempIBLL = new ReportTempBLL();

        #region 视图功能
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MarketingReport()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TechnologyReport()
        {
            return View();
        }


        [HttpGet]
        public ActionResult SettlementReport()
        {
            return View();
        }
        /// <summary>
        /// 销售报表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SalesReport()
        {
            return View();
        }
        /// <summary>
        /// 仓库报表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult StockReport()
        {
            return View();
        }
        /// <summary>
        /// 收支报表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FinanceReport()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        public ActionResult GetPDMMarketingReport(string pagination, string keyword)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = reportTempIBLL.GetPDMMarketingReport(paginationobj, keyword);

            var jsonData = new
            {
                rows = data,
                total = 0,
                page = 0,
                records = 0
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 获取采购报表数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPurchaseReportList()
        {
            var data = ExcelHelper.ExcelImport(Server.MapPath("~/Areas/LR_ReportModule/Views/ReportTemplate/ReportData/PurchaseReport.xlsx"));
            return Success(data);
        }
        /// <summary>
        /// 获取销售报表数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSalesReportList()
        {
            var data = ExcelHelper.ExcelImport(Server.MapPath("~/Areas/LR_ReportModule/Views/ReportTemplate/ReportData/SalesReport.xlsx"));
            return Success(data);
        }
        /// <summary>
        /// 获取仓库报表数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetStockReportList()
        {
            var data = ExcelHelper.ExcelImport(Server.MapPath("~/Areas/LR_ReportModule/Views/ReportTemplate/ReportData/StockReport.xlsx"));
            return Success(data);
        }
        /// <summary>
        /// 获取收支报表数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetFinanceReportList()
        {
            var data = ExcelHelper.ExcelImport(Server.MapPath("~/Areas/LR_ReportModule/Views/ReportTemplate/ReportData/FinanceReport.xlsx"));
            return Success(data);
        }
        #endregion

    }
}
