using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.8 亿钜智能敏捷开发框架
    /// Copyright (c) 2013-2020 亿钜智能
    /// 创 建：超级管理员
    /// 日 期：2017-07-12 09:57
    /// 描 述：报表管理
    /// </summary>
    public class ReportTempService : RepositoryFactory
    {
        #region 获取数据
        public IEnumerable<MarketingReportModel> GetPDMMarketingReport(Pagination pagination, string keyword)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.ProjectCode,
                t.ProjectName,
                t.CustName,
                t.ContactName,
                t.ContactPhone,
                t.Address,
                t.ProvinceId,
                t.CityId,
                t.CountyId,
                t.ProjectSituation,
                t.Remark,
                t.PreparedPerson,
                t.FollowPerson,
                t.FCompanyId,
                t.FDepartmentId,
                t.PCompanyId,
                t.PDepartmentId,
                t.CompanyId,
                t.DepartmentId,
                t.ProjectStatus,
                t.CreateTime,
                t.CreateUser
                ");
                strSql.Append("  FROM Project t where t.ProjectStatus=1");
                IEnumerable<MarketingReportModel> marketingReportModels = BaseRepository("learunOAWFForm").FindList<MarketingReportModel>(strSql.ToString(), pagination);
                return marketingReportModels;
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

     
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ReportTempEntity GetEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository().FindEntity<ReportTempEntity>(keyValue);
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

        #region 提交数据
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void DeleteEntity(string keyValue)
        {
            try
            {
                ReportTempEntity entity = new ReportTempEntity()
                {
                    F_TempId = keyValue,
                };
                this.BaseRepository().Delete(entity);
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
        /// <summary>
        /// 保存（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveEntity(string keyValue, ReportTempEntity entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
                else
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
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
