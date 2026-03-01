using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Learun.Util;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo.Customer
{
  public   class CustomerBLL : CustomerIBLL
    {

        private CustomerService customerService = new CustomerService();

        public Object GetCount(string sql)
        {
            try
            {
                return customerService.GetCount(sql); ;
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        public Object GetCollectionSum(string sql)
        {
            try
            {
                return customerService.GetCollectionSum(sql); ;
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        public Object GetInquiryCount(string sql)
        {
            try
            {
                return customerService.GetInquiryCount(sql); ;
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        public Object GetPaymentSum(string sql)
        {
            try
            {
                return customerService.GetPaymentSum(sql); ;
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        public Object GetSignedSum(string sql)
        {
            try
            {
                return customerService.GetSignedSum(sql); ;
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        public object GetMarketingReport(string sql)
        {
            try
            {
                return customerService.GetMarketingReport(sql); ;
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }
    }
}
