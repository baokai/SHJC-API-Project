using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo.Customer
{
    public  interface CustomerIBLL
    {
        /// <summary>
        /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
        /// Copyright (c) 2013-2020 上海力软信息技术有限公司
        /// 创 建：超级管理员
        /// 日 期：2019-06-04 10:38
        /// 描 述：订单信息
        /// </summary>

        #region
        Object GetCount(string sql);
        Object GetInquiryCount(string sql);
        Object GetSignedSum(string sql);
        Object GetCollectionSum(string sql);
        Object GetPaymentSum(string sql);
        Object GetMarketingReport(string sql);
        #endregion
    }
}
