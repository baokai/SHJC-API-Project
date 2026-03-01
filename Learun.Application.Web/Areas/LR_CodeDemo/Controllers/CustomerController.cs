
using Learun.Application.TwoDevelopment.LR_CodeDemo.Customer;
using System.Web.Mvc;

namespace Learun.Application.Web.Areas.LR_CodeDemo.Controllers

{
    /*敏捷*/
    public class CustomerController : MvcControllerBase
    {
        private CustomerIBLL customerIBBL = new CustomerBLL();
        //获取新增客户量
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetCount()
        {
            string sql = "select COUNT(*) from ProjectContract where DATEDIFF(DD ,ProjectContract.CreateTime,GETDATE())=0";
            object jsonData = customerIBBL.GetCount(sql);
            return Success(jsonData);
        }

        //获取新增商机
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetInquiryCount()
        {
            string sql = "select COUNT(*) from ProjectPayCollection where DATEDIFF(DD ,ProjectPayCollection.CreateTime,GETDATE())>0";
            object jsonData = customerIBBL.GetInquiryCount(sql);
            return Success(jsonData);
        }

        //获取今日签约
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetSignedSum()
        {
            string sql = "select sum(ContractAmount) from ProjectContract where ProjectContract.ContractStatus=3";
            var jsData = customerIBBL.GetSignedSum(sql);
            return Success(jsData);
        }

        //获取应收账款
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetCollectionSum()
        {
            string sql = "select SUM(ContractAmount) from ProjectContract where ContractType=1 and datediff(month,ProjectContract.CreateTime,getdate())=0";
            object jsData = customerIBBL.GetCollectionSum(sql);
            return Success(jsData);
        }

        //获取应付账款
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPaymentSum()
        {
            // string sql = "select SUM(PaymentAmount) from ProjectPayment where ProjectPayment.PaymentStatus=3 and datediff(month,ProjectPayment.CreateTime,getdate())=0;";
            string sql = "select ISNULL(SUM(PaymentAmount),0) from ProjectPayment where ProjectPayment.PaymentStatus=3 and datediff(month,ProjectPayment.CreateTime,getdate())=0;";
            object jsData = customerIBBL.GetPaymentSum(sql);
            return Success(jsData);
        }


        //获取营销报表
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetMarketingReport()
        {
            string sql = "select SUM(PaymentAmount) from ProjectPayment where ProjectPayment.PaymentStatus=3 and datediff(month,ProjectPayment.CreateTime,getdate())=0;";
            object jsData = customerIBBL.GetMarketingReport(sql);
            return Success(jsData);
        }
    }
}
