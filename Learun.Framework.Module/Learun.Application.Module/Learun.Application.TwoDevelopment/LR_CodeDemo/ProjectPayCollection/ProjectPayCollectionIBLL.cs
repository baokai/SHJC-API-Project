using Learun.Util;
using System.Data;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
    /// 创 建：超级管理员
    /// 日 期：2022-03-16 17:56
    /// 描 述：项目回款管理
    /// </summary>
    public interface ProjectPayCollectionIBLL
    {
        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<ProjectPayCollectionVo> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 多部门获取页面显示列表数据
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<ProjectPayCollectionVo> GetPageListDepartmentId(Pagination pagination, string queryJson,string dep);
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<ProjectPayCollectionVo> GetPageList(string queryJson);
        /// <summary>
        /// 合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<ProjectPayCollectionVo> GetPageListSUM(string queryJson); 
        /// <summary>
        /// 合计多部门
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<ProjectPayCollectionVo> GetPageListSUMDepartmentId(string queryJson,string dep); 
        /// <summary>
        /// 获取ProjectPayCollection表实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        ProjectPayCollectionEntity GetProjectPayCollectionEntity(string keyValue);
        /// <summary>
        /// 预览
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
       ProjectPayCollectionVo GetPreviewProjectPayCollectionById(string keyValue);
        /// <summary>
        /// 根据报备id查询回款信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<ProjectPayCollectionVo> GetPayCollection(string id);
        /// <summary>
        /// 根据projectId查询对应的回款
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
         ProjectPayCollectionVo GetCollectionByIdProjectId(string ProjectId);
         ProjectPayCollectionVo GetCollectionByIdProjectIdtIME(string ProjectId, string str);
        ProjectPayCollectionVo GetPageListsum(string ProjectId);
        #endregion

        #region 提交数据

        /// <summary>
        /// 删除实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void DeleteEntity(string keyValue);
        /// <summary>
        /// 保存实体数据（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体</param>
        void SaveEntity(string keyValue, ProjectPayCollectionEntity entity);
        #endregion

    }
}
