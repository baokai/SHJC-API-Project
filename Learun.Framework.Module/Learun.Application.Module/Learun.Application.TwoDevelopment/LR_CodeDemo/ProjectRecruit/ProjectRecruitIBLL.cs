using Learun.Util;
using System.Data;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
    /// 创 建：超级管理员
    /// 日 期：2022-03-16 18:06
    /// 描 述：用工申请
    /// </summary>
    public interface ProjectRecruitIBLL
    {
        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<ProjectRecruitVo> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 多部门获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<ProjectRecruitVo> GetPageListDepartmentId(Pagination pagination, string queryJson,string dep);
        /// <summary>
        /// 获取ProjectRecruit表实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        ProjectRecruitEntity GetProjectRecruitEntity(string keyValue);
        /// <summary>
        /// 获取主表实体数据
        /// </summary>
        /// <param name="processId">流程实例ID</param>
        /// <returns></returns>
        ProjectRecruitEntity GetEntityByProcessId(string processId);
        /// <summary>
        /// 获取流程表单数据
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        ProjectRecruitVo GetProjectRecruitByProcessId(string processId);
        /// <summary>
        /// 根据流程id查询
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        ProjectRecruitVo GetRecruitByProcessId(string processId);
        ///<summary>
        ///根据id获取用工页面预览需要的数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        ProjectRecruitVo GetPreviewProjectRecruit(string keyValue);
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
        void SaveEntity(string keyValue, ProjectRecruitEntity entity);
        /// <summary>
        /// 提交审批
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="ProcessId"></param>
        void UpdateFlowId(string keyValue, string ProcessId);
        #endregion

    }
}
