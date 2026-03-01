using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Learun.DataBase.Repository;
using Learun.Util;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo.Customer
{
    internal class CustomerService : RepositoryFactory
    {
        #region 获取数据
        /// <summary>
        /// 获取客户数量
        /// <summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>

        public Object GetCount(string sql)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindTable(sql);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        public Object GetInquiryCount(string sql)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindTable(sql);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        public Object GetCollectionSum(string sql)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindTable(sql);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        public Object GetPaymentSum(string sql)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindTable(sql);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        public Object GetSignedSum(string sql)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindTable(sql);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        public Object GetMarketingReport(string sql)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindTable(sql);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        #endregion
    }
}
