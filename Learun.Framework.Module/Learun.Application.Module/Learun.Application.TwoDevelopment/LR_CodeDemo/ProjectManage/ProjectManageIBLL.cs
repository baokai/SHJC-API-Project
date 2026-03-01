using Learun.Util;
using System.Data;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
    /// 创 建：超级管理员
    /// 日 期：2022-03-10 22:29
    /// 描 述：项目管理
    /// </summary>
    public interface ProjectManageIBLL
    {
        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<ProjectEntity> GetPageList(Pagination pagination, string queryJson);
        IEnumerable<ProjectVo> GetProjectList2(Pagination pagination, string queryJson);
        IEnumerable<ProjectVo> GetPageListAPI(Pagination pagination, string queryJson);
        /// <summary>
        /// 多部门列表历史信息
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <param name="dep"></param>
        /// <returns></returns>
        IEnumerable<ProjectVo> GetPageListDepartmentId(Pagination pagination, string queryJson, string dep);
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<ProjectVo> GetPageListAddress(Pagination pagination, string queryJson);
        /// <summary>
        /// 多部门列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<ProjectVo> GetPageListAddressDepartmentId(Pagination pagination, string queryJson, string dep);
        IEnumerable<ProjectVo> GetPageListAddressDepartmentIds(Pagination pagination, string queryJson, string dep);



        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<ProjectVo> GetPageListAddress(string queryJson);
        IEnumerable<ProjectVo> GetPageListAddress2(string queryJson);
        /// <summary>
        /// 多部门导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="dep"></param>
        /// <returns></returns>
        IEnumerable<ProjectVo> GetPageListAddressDepartmentId(string queryJson, string dep);
        /// <summary>
        /// 显示所有数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<ProjectEntity> GetList(string queryJson);
        /// <summary>
        /// 获取可以选择的项目信息
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<ProjectEntity> GetSelectedProjectList(Pagination pagination, string queryJson);
        IEnumerable<ProjectVo> GetSelectedProjectListWithoutContract(Pagination pagination, string queryJson);
        
        IEnumerable<ProjectVo> GetSelectedProjectList1(Pagination pagination, string queryJson);
        IEnumerable<ProjectEntity> GetSelectedProjectListT(Pagination pagination, string queryJson);
        IEnumerable<ProjectEntity> GetSelectedProjectListTi(Pagination pagination, string queryJson);
        IEnumerable<ProjectVo> GetSelectedProjectByContractList(Pagination pagination, string queryJson);

        IEnumerable<ProjectVo> GetSelectedProjectByContractList_2(Pagination pagination, string queryJson, string dept);
        /// <summary>
        /// 开票添加项目
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<ProjectVo> GetSelectedProjectByContractListBilling(Pagination pagination, string queryJson);
        /// <summary>
        /// 多部门开票添加项目
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<ProjectVo> GetSelectedProjectByContractListBillingDepartmentId(Pagination pagination, string queryJson, string dep);

        /// <summary>
        /// 判断当前的合同是否重复
        /// </summary>
        /// <param name="ContactPhone"></param>
        /// <param name="CustName"></param>
        /// <returns></returns>
        int JudgePepeatProject(string ContactPhone, string CustName, string keyValue);
        int JudgePepeatProjectBy(string ContactPhone, string CustName);

        /// <summary>
        /// 根据项目名称获取历史项目信息的数量
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        int JudgePepeatProjectName(string projectName, string keyValue);
        int JudgePepeatProjectNameBy(string projectName);
        /// <summary>
        /// 合同预览
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        ProjectEntity GetProjectEntity(string keyValue);
        /// <summary>
        /// 获取主表实体数据
        /// </summary>
        /// <param name="processId">流程实例ID</param>
        /// <returns></returns>
        ProjectEntity GetEntityByProcessId(string processId);
        /// <summary>
        /// 预览
        /// </summary>
        /// <param name="keyValue">id</param>
        /// <returns></returns>
        ProjectEntity GetPreviewFormData(string keyValue);
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        ProjectVo GetPreviewFormDataById(string keyValue);
        IEnumerable<ProjectFollowListEntity> GetProjectFollowList(string keyVale);
        ProjectVo GetPreviewFormDataBy(string keyValue);
        /// <summary>
        /// 获取月度数量
        /// </summary>
        /// <returns></returns>
        List<ProjectDpVo> GetProjectMonthCount();

        List<ProjectSourceVo> GetProjectCountBySource();

        List<ProvinceCount> GetProjectCountByProvince();
        List<ProjectConversionVo> GetProjectConversion();
        List<ProjectPaymentBackVo> GetBackProjectRate();
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
        void SaveEntity(string keyValue, ProjectEntity entity);
        void SaveProjectFollowEntity(string keyValue, ProjectFollowListEntity entity);
        void AddEntity(ProjectEntity entity);
        ProjectVo PreviewIndexFrom(string keyValue);
        /// <summary>
        /// 报备查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ProjectVo GetToviewListProject(string id);
        /// <summary>
        /// 市
        /// </summary>
        /// <param name="ProvinceId"></param>
        /// <param name="CityId"></param>
        /// <returns></returns>
        ProjectVo ProvinceIdS(string Id);
        /// <summary>
        /// 县/区
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        ProjectVo ProvinceIdX(string Id);
        #endregion

    }
}
