using Learun.Util;
using System;
using System.Data;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
    /// 创 建：超级管理员
    /// 日 期：2022-03-11 00:04
    /// 描 述：合同支付
    /// </summary>
    public class ProjectPaymentListBLL : ProjectPaymentListIBLL
    {
        private ProjectPaymentListService projectPaymentListService = new ProjectPaymentListService();

        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectPaymentVo> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return projectPaymentListService.GetPageList(pagination, queryJson);
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
        /// 根据报备id查询相关付款
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<ProjectPaymentVo> GetPayment(string id)
        {
            try
            {
                return projectPaymentListService.GetPayment(id);
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
        /// 付款导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectPaymentVo> GetPageList(string queryJson)
        {
            try
            {
                return projectPaymentListService.GetPageList(queryJson);
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
        /// 查付款批量添加所有数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProjectPaymentListVo> GetPaymentList(string ProjectId)
        {
            try
            {
                return projectPaymentListService.GetPaymentList(ProjectId);
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
        /// 查付款批量添加所有数据
        /// </summary>
        /// <returns></returns>
        public ProjectPaymentListVo GetPaymentListBy(string ProjectId)
        {
            try
            {
                return projectPaymentListService.GetPaymentListBy(ProjectId);
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
        /// 获取ProjectPayment表实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public ProjectPaymentEntity GetProjectPaymentEntity(string keyValue)
        {
            try
            {
                return projectPaymentListService.GetProjectPaymentEntity(keyValue);
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
        /// 页面预览信息
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public ProjectPaymentVo GetPreviewProjectPayment(string keyValue)
        {
            try
            {
                return projectPaymentListService.GetPreviewProjectPayment(keyValue);
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
        /// 根据projectId查询对应的付款
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ProjectPaymentVo GetProjectPaymentByprojectId(string keyValue)
        {
            try
            {
                return projectPaymentListService.GetProjectPaymentByprojectId(keyValue);
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
        /// 获取主表实体数据
        /// </summary>
        /// <param name="processId">流程实例ID</param>
        /// <returns></returns>
        public ProjectPaymentVo GetEntityByProcessId(string processId)
        {
            try
            {
                return projectPaymentListService.GetEntityByProcessId(processId);
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
        public IEnumerable<ProjectPaymentVo> GetEntityBytID(string processId)
        {
            try
            {
                return projectPaymentListService.GetEntityBytID(processId);
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
        /// 删除实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void DeleteEntity(string keyValue)
        {
            try
            {
                projectPaymentListService.DeleteEntity(keyValue);
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
        /// 保存实体数据（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体</param>
        public void SaveEntity(string keyValue, ProjectPaymentEntity entity)
        {
            try
            {
                projectPaymentListService.SaveEntity(keyValue, entity);
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
        /// 批量添加
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveEntityList(string Ids, ProjectPaymentEntity entity, List<BatchAuditAddModel> item_list)
        {
            try
            {
                projectPaymentListService.SaveEntityList(Ids, entity,item_list);
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
        public void UpdateFlowId(string keyValue, string ProcessId)
        {
            try
            {
                projectPaymentListService.UpdateFlowId(keyValue, ProcessId);
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
