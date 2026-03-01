using Learun.Util;
using System;
using System.Data;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 日 期：2022-03-16 17:56
    /// 描 述：项目回款管理
    /// </summary>
    public class ProjectPayCollectionBLL : ProjectPayCollectionIBLL
    {
        private ProjectPayCollectionService projectPayCollectionService = new ProjectPayCollectionService();

        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectPayCollectionVo> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                //IEnumerable <ProjectPayCollectionVo> list=projectPayCollectionService.GetPageList(pagination, queryJson);
                return projectPayCollectionService.GetPageList(pagination, queryJson);
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
        /// 多部门获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectPayCollectionVo> GetPageListDepartmentId(Pagination pagination, string queryJson,string dep)
        {
            try
            {
                //IEnumerable <ProjectPayCollectionVo> list=projectPayCollectionService.GetPageListDepartmentId(pagination, queryJson,dep);
                return projectPayCollectionService.GetPageListDepartmentId(pagination, queryJson,dep);
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
        /// 根据报备id查询回款信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
     public IEnumerable<ProjectPayCollectionVo> GetPayCollection(string id)
        {
            try
            {
                return projectPayCollectionService.GetPayCollection(id);
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
        /// 合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectPayCollectionVo> GetPageListSUM(string queryJson)
        {
            try {
                
                return projectPayCollectionService.GetPageListSUM(queryJson);
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
        /// 合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectPayCollectionVo> GetPageListSUMDepartmentId(string queryJson,string dep)
        {
            try {
                
                return projectPayCollectionService.GetPageListSUMDepartmentId(queryJson,dep);
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
        /// 导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectPayCollectionVo> GetPageList( string queryJson)
        {
            try
            {
                //IEnumerable<ProjectPayCollectionVo> list = projectPayCollectionService.GetPageList(queryJson);
                return projectPayCollectionService.GetPageList(queryJson);
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
        /// 获取ProjectPayCollection表实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public ProjectPayCollectionEntity GetProjectPayCollectionEntity(string keyValue)
        {
            try
            {
                return projectPayCollectionService.GetProjectPayCollectionEntity(keyValue);
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
        /// 根据projectId查询对应的回款
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public ProjectPayCollectionVo GetCollectionByIdProjectId(string ProjectId)
        {
            try
            {
                return projectPayCollectionService.GetCollectionByIdProjectId(ProjectId);
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
        }  public ProjectPayCollectionVo GetCollectionByIdProjectIdtIME(string ProjectId,string str)
        {
            try
            {
                return projectPayCollectionService.GetCollectionByIdProjectIdtIME(ProjectId,str);
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
        public ProjectPayCollectionVo GetPageListsum(string ProjectId)
        {
            try
            {
                return projectPayCollectionService.GetPageListsum(ProjectId);
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
        /// 预览
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public ProjectPayCollectionVo GetPreviewProjectPayCollectionById(string keyValue)
        {
            try
            {
                return projectPayCollectionService.GetPreviewProjectPayCollectionById(keyValue);
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
                projectPayCollectionService.DeleteEntity(keyValue);
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
        /// <returns></returns>
        public void SaveEntity(string keyValue, ProjectPayCollectionEntity entity)
        {
            try
            {
                projectPayCollectionService.SaveEntity(keyValue, entity);
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
