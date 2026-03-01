using Learun.Application.Base.SystemModule;
using Learun.Application.Organization;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.8 亿钜智能敏捷开发框架
    /// Copyright (c) 2013-2020 亿钜智能
    /// 创 建：超级管理员
    /// 日 期：2017-07-12 09:57
    /// 描 述：报表管理
    /// </summary>
    public class ReportTempBLL : ReportTempIBLL
    {
        private ReportTempService reportTempService = new ReportTempService();
        private ProjectContractService projectContractService = new ProjectContractService();
        private ProjectBillingService projectBillingService = new ProjectBillingService();
        private ProjectPayCollectionService projectPayCollectionService = new ProjectPayCollectionService();
        private ProjectTaskService projectTaskService = new ProjectTaskService();
        private UserService userService = new UserService();

        #region 获取数据

        public IEnumerable<MarketingReportModel> GetPDMMarketingReport(Pagination pagination, string keyword)
        {
            try
            {
                IEnumerable<MarketingReportModel> marketingReportModels = reportTempService.GetPDMMarketingReport(pagination, keyword);
                foreach (var marketing in marketingReportModels)
                {
                    List<ProjectContractEntity> projectContractEntities = projectContractService.GetProjectContractByProjectId(marketing.id);
                    if (projectContractEntities.Count > 0)
                    {
                        marketing.ContractTime = projectContractEntities.FirstOrDefault().CreateTime.ToDateString();
                        marketing.ContractStatus = !projectContractEntities.FirstOrDefault().ContractStatus.HasValue ? 0 : projectContractEntities.FirstOrDefault().ContractStatus.Value;
                        marketing.ContractSubject = projectContractEntities.FirstOrDefault().ContractSubject;
                        marketing.ContractAmount = projectContractEntities.Sum(i => i.ContractAmount).HasValue ? projectContractEntities.Sum(i => i.ContractAmount).Value : 0;
                    }
                    List<ProjectBillingEntity> projectBillings = projectBillingService.GetProjectBillingByProjectId(marketing.id);
                    if (projectBillings.Count > 0)
                    {
                        marketing.BillingStatus = !projectBillings.FirstOrDefault().BillingStatus.HasValue ? 0 : projectBillings.FirstOrDefault().BillingStatus.Value;
                    }

                    List<ProjectPayCollectionEntity> payCollectionEntities= projectPayCollectionService.GetProjectPayCollectionByProjectId(marketing.id);
                    if (payCollectionEntities.Count > 0)
                    {
                        marketing.PayCollectionAmount = payCollectionEntities.Sum(i => i.Amount).HasValue ? payCollectionEntities.Sum(i => i.Amount).Value : 0;
                        marketing.PayCollectionTime = payCollectionEntities.LastOrDefault().CreateTime.ToDateString();
                    }
                    List<ProjectTaskEntity>  projectTaskEntities= projectTaskService.GetProjectTaskByProjectId(marketing.id);
                    if (projectTaskEntities.Count > 0)
                    {
                        ProjectTaskEntity projectTask = projectTaskEntities.FirstOrDefault();
                        marketing.TestDateTime = projectTask.ApproachTime.HasValue ? projectTask.ApproachTime.Value.ToDateString() : "";
                        marketing.ReportSubject = projectTask.ReportSubject;
                        marketing.TaskStatus = projectTask.TaskStatus.HasValue ? projectTask.TaskStatus.Value : 0;
                        marketing.ProjectResponsible = projectTask.ProjectResponsible;
                        if (!string.IsNullOrEmpty(projectTask.ProjectResponsible))
                        {
                            UserEntity user= userService.GetEntity(projectTask.ProjectResponsible);
                            if (user != null)
                            {
                                marketing.CarryDept = user.F_DepartmentId;
                            }
                        }
                       
                    }

                }
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
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }
        /// <summary>
        /// 获得报表数据
        /// </summary>
        /// <param name="dataSourceId">数据库id</param>
        /// <param name="strSql">SQL语句</param>
        /// <returns></returns>
        public DataTable GetReportData(string dataSourceId, string strSql)
        {
            try
            {
                if (!string.IsNullOrEmpty(strSql))
                    return new DatabaseLinkBLL().FindTable(dataSourceId, strSql);
                else
                    return null;
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
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ReportTempEntity GetEntity(string keyValue)
        {
            try
            {
                return reportTempService.GetEntity(keyValue);
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
                reportTempService.DeleteEntity(keyValue);
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
        /// 保存（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveEntity(string keyValue, ReportTempEntity entity)
        {
            try
            {
                reportTempService.SaveEntity(keyValue, entity);
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
        #endregion
    }
}
