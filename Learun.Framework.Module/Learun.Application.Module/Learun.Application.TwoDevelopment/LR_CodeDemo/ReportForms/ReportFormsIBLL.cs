using Learun.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo.ReportForms
{
    public interface ReportFormsIBLL
    {

        #region 获取数据

        /// <summary>
        /// 获取营销报表的数据信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<MarketingEntity> GetMarketings(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取营销报表的数据信息(部门)
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<MarketingEntity> GetMarketings1(Pagination pagination, string queryJson,string dep);
        /// <summary>
        /// 资金台账列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<CapitalDepartmentId> GetCapitalDepartmentIdList(string start, string end); 
        /// <summary>
        /// 资金台账列表部门去年
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<CapitalDepartmentId> GetCapitalDepartmentIdListDepartmentId1(string cYYYY, string start, string end,string DepartmentId);
        /// <summary>
        /// 资金台账列表部门去年部门
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<CapitalDepartmentId> GetCapitalDepartmentIdListDepartmentIddep1(string cYYYY, string start, string end,string DepartmentId);
        /// <summary>
        /// 资金台账列表部门
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<CapitalDepartmentId> GetCapitalDepartmentIdListDepartmentId(string cYYYY, string start, string end,string DepartmentId);
        /// <summary>
        /// 成本金额添加修改
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
         void CapitalAmountSaveForm(decimal costAmount, string yearMonth);
         void CapitalAmountSaveForm1(decimal? costAmount, string yearMonth);
        /// <summary>
        /// 获取当月成本金额
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        CapitalAmountEntity getCapitalAmountByYearMonth(string yearMonth);
        /// <summary>
        /// 多部门获取营销报表的数据信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<MarketingEntity> GetMarketingsDepartmentId(Pagination pagination, string queryJson,string dep);
        /// <summary>
        /// 合作伙伴的营销台账
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<MarketingEntity> GetMarketingsHZ(Pagination pagination, string queryJson);
      
       
        IEnumerable<MarketingEntity> GetMarketings(string queryJson);
        IEnumerable<MarketingEntity> GetMarketingsdc(string queryJson,string dep);

        /// <summary>
        /// 获取生产报表的数据信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<ProductionEntity> GetProductions(Pagination pagination, string queryJson);
        /// <summary>
        /// 多部门获取生产报表的数据信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<ProductionEntity> GetProductionsDepartmentId(Pagination pagination, string queryJson,string dep);
       

        /// <summary>
        /// 获取结算报表的数据信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<SettleAccountsEntity> GetSettleAccounts(Pagination pagination, string queryJson);
        /// <summary>  
        /// 
        /// 多部门获取结算报表的数据信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<SettleAccountsEntity> GetSettleAccountsDepartmentId(Pagination pagination, string queryJson,string  dep);
        /// <summary>s
        /// 合作伙伴结算台账
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<SettleAccountsEntity> GetSettleAccountsHZ(Pagination pagination, string queryJson);
        /// <summary>
        /// 生产合计
        /// </summary>
        /// <param name="queryJsons"></param>
        /// <returns></returns>
        IEnumerable<ProductionEntity> GetProductions(string queryJsons);
        /// <summary>
        /// 多部门生产合计
        /// </summary>
        /// <param name="queryJsons"></param>
        /// <returns></returns>
        IEnumerable<ProductionEntity> GetProductionsDepartmentId(string queryJsons,string dep);
        IEnumerable<SettleAccountsEntity> GetSettleAccounts(string queryJson);
        /// <summary>
        /// 合作伙伴结算台账导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<SettleAccountsEntity> GetSettleAccountsHZ(string queryJson);
        IEnumerable<ProjectContractEntity> getChangeList();
        /// <summary>
        /// 结算合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<SettleAccountsEntity> GetSettleAccountsSum(string queryJson); 
        /// <summary>
        /// 多部门结算合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        //IEnumerable<SettleAccountsEntity> GetSettleAccountsSumDepartmentId(string queryJson,string dep);
        IEnumerable<SettleAccountsEntity> GetSettleAccountsSum_newDepartmentId(string queryJson,string dep);
        IEnumerable<SettleAccountsEntity> GetSettleAccountsSum_new(string queryJson);
        /// <summary>
        /// 合作伙伴结算合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<SettleAccountsEntity> GetSettleAccountsSumHZ(string queryJson);
        /// <summary>
        /// 营销合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<MarketingEntity> GetMarketingsSum(string queryJson);
        /// <summary>
        /// 营销合计导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<MarketingEntity> GetMarketingsSum_new(string queryJson); 
        /// <summary>
        /// 多部门营销合计导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<MarketingEntity> GetMarketingsSum_newDepartmentId(string queryJson,string dep);
        /// <summary>
        /// 合作伙伴的营销台账合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<MarketingEntity> GetMarketingsSumHZ(string queryJson);


        #endregion
    }
}
