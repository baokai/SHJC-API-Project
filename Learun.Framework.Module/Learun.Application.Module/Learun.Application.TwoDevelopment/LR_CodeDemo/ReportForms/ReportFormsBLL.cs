using Learun.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo.ReportForms
{
    public class ReportFormsBLL : ReportFormsIBLL
    {
        private ReportFormsService reportFormsService = new ReportFormsService();

        public IEnumerable<ProjectContractEntity> getChangeList()
        {
            try
            {
                return reportFormsService.getChangeList();
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
        /// <summary>
        /// 营销台账列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<MarketingEntity> GetMarketings(Pagination pagination, string queryJson)
        {
            try
            {
                return reportFormsService.GetMarketings_new(pagination, queryJson);
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
        /// <summary>
        /// 营销台账列表(部门)
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<MarketingEntity> GetMarketings1(Pagination pagination, string queryJson,string dep)
        {
            try
            {
                return reportFormsService.GetMarketings_new1(pagination, queryJson,dep);
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
        /// <summary>
        /// 资金台账列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<CapitalDepartmentId> GetCapitalDepartmentIdList(string start, string end)
        {
            try
            {
                return reportFormsService.GetCapitalDepartmentIdList(start, end);
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
        /// <summary>
        /// 资金台账列表部门今年
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<CapitalDepartmentId> GetCapitalDepartmentIdListDepartmentId(string cYYYY, string start, string end,string DepartmentId)
        {
            try
            {
                return reportFormsService.GetCapitalDepartmentIdListDepartmentId(cYYYY,start, end, DepartmentId);
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
        /// <summary>
        /// 资金台账列表部门去年
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<CapitalDepartmentId> GetCapitalDepartmentIdListDepartmentId1(string cYYYY, string start, string end,string DepartmentId)
        {
            try
            {
                return reportFormsService.GetCapitalDepartmentIdListDepartmentId1(cYYYY,start, end, DepartmentId);
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
        /// <summary>
        /// 资金台账列表部门去年部门
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<CapitalDepartmentId> GetCapitalDepartmentIdListDepartmentIddep1(string cYYYY, string start, string end,string DepartmentId)
        {
            try
            {
                return reportFormsService.GetCapitalDepartmentIdListDepartmentIddep1(cYYYY,start, end, DepartmentId);
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
        /// <summary>
        /// 成本金额添加修改
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void CapitalAmountSaveForm(decimal costAmount, string yearMonth)
        {
            try
            {
                reportFormsService.CapitalAmountSaveForm(costAmount, yearMonth);
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
         /// <summary>
        /// 成本金额添加修改
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void CapitalAmountSaveForm1(decimal? costAmount, string yearMonth)
        {
            try
            {
                reportFormsService.CapitalAmountSaveForm1(costAmount, yearMonth);
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
        /// <summary>
        /// 获取当月成本金额
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        public CapitalAmountEntity getCapitalAmountByYearMonth(string yearMonth)
        {
            try
            {
                return reportFormsService.getCapitalAmountByYearMonth(yearMonth);
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
        /// <summary>
        /// 多部门营销台账列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<MarketingEntity> GetMarketingsDepartmentId(Pagination pagination, string queryJson, string dep)
        {
            try
            {
                return reportFormsService.GetMarketings_newDepartmentId(pagination, queryJson, dep);
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
        /// <summary>
        /// 合作伙伴的营销台账
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<MarketingEntity> GetMarketingsHZ(Pagination pagination, string queryJson)
        {
            try
            {
                return reportFormsService.GetMarketings_newHZ(pagination, queryJson);
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

        public IEnumerable<MarketingEntity> GetMarketings(string queryJson)
        {
            try
            {
                return reportFormsService.GetMarketings_new(queryJson);
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
        public IEnumerable<MarketingEntity> GetMarketingsdc(string queryJson,string dep)
        {
            try
            {
                return reportFormsService.GetMarketings_newdc(queryJson,dep);
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
        /// <summary>
        /// 营销合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<MarketingEntity> GetMarketingsSum(string queryJson)
        {
            try
            {
                return reportFormsService.GetMarketingsSum(queryJson);
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
        /// <summary>
        /// 营销合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<MarketingEntity> GetMarketingsSum_new(string queryJson)
        {
            try
            {
                return reportFormsService.GetMarketingsSum_new(queryJson);
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
        /// <summary>
        /// 多部门营销合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<MarketingEntity> GetMarketingsSum_newDepartmentId(string queryJson, string dep)
        {
            try
            {
                return reportFormsService.GetMarketingsSum_newDepartmentId(queryJson, dep);
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
        /// <summary>
        /// 合作伙伴营销台账
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<MarketingEntity> GetMarketingsSumHZ(string queryJson)
        {
            try
            {
                return reportFormsService.GetMarketingsSumHZ(queryJson);
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


        /// <summary>
        /// 生产报表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProductionEntity> GetProductions(Pagination pagination, string queryJson)
        {
            try
            {
                return reportFormsService.GetProductions_new(pagination, queryJson);
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

        /// <summary>
        /// 多部门生产报表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProductionEntity> GetProductionsDepartmentId(Pagination pagination, string queryJson, string dep)
        {
            try
            {
                return reportFormsService.GetProductions_newDepartmentId(pagination, queryJson, dep);
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
        /// <summary>
        /// 生产合计
        /// </summary>
        /// <param name="queryJsons"></param>
        /// <returns></returns>
        public IEnumerable<ProductionEntity> GetProductions(string queryJsons)
        {
            try
            {
                return reportFormsService.GetProductions_new(queryJsons);
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
        /// <summary>
        /// 多部门生产合计
        /// </summary>
        /// <param name="queryJsons"></param>
        /// <returns></returns>
        public IEnumerable<ProductionEntity> GetProductionsDepartmentId(string queryJsons, string dep)
        {
            try
            {
                return reportFormsService.GetProductions_newDepartmentId(queryJsons, dep);
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
        /// <summary>
        /// 结算列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<SettleAccountsEntity> GetSettleAccounts(Pagination pagination, string queryJson)
        {
            try
            {
                return reportFormsService.GetSettleAccounts_new(pagination, queryJson);
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
        /// <summary>
        /// 多部门结算列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<SettleAccountsEntity> GetSettleAccountsDepartmentId(Pagination pagination, string queryJson, string dep)
        {
            try
            {
                return reportFormsService.GetSettleAccounts_newDepartmentId(pagination, queryJson, dep);
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
        /// <summary>
        /// 合作伙伴结算台账
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<SettleAccountsEntity> GetSettleAccountsHZ(Pagination pagination, string queryJson)
        {
            try
            {
                return reportFormsService.GetSettleAccounts_newHZ(pagination, queryJson);
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

        public IEnumerable<SettleAccountsEntity> GetSettleAccounts(string queryJson)
        {
            try
            {
                return reportFormsService.GetSettleAccounts_new(queryJson);
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
        /// <summary>
        /// 合作伙伴结算台账导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<SettleAccountsEntity> GetSettleAccountsHZ(string queryJson)
        {
            try
            {
                return reportFormsService.GetSettleAccounts_newHZ(queryJson);
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
        /// <summary>
        /// 结算合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<SettleAccountsEntity> GetSettleAccountsSum(string queryJson)
        {
            try
            {
                return reportFormsService.GetSettleAccountsSum(queryJson);
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
        /// <summary>
        /// 多部门结算导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<SettleAccountsEntity> GetSettleAccountsSum_newDepartmentId(string queryJson, string dep)
        {
            try
            {
                return reportFormsService.GetSettleAccountsSum_newDepartmentId(queryJson, dep);
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
        } /// <summary>
          /// 结算导出
          /// </summary>
          /// <param name="queryJson"></param>
          /// <returns></returns>
        public IEnumerable<SettleAccountsEntity> GetSettleAccountsSum_new(string queryJson)
        {
            try
            {
                return reportFormsService.GetSettleAccountsSum_new(queryJson);
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
        /// <summary>
        /// 合作伙伴结算台账合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<SettleAccountsEntity> GetSettleAccountsSumHZ(string queryJson)
        {
            try
            {
                return reportFormsService.GetSettleAccountsSumHZ(queryJson);
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
