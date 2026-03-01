using Learun.Util;
using System;
using System.Data;
using System.Collections.Generic;
using Learun.Application.Base.SystemModule;
using Learun.Application.Organization;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
    /// 创 建：超级管理员
    /// 日 期：2022-03-10 22:29
    /// 描 述：项目管理
    /// </summary>
    public class ProjectManageBLL : ProjectManageIBLL
    {
        private ProjectManageService projectManageService = new ProjectManageService();

        private AreaService areaService = new AreaService();
        private DepartmentService departmentService = new DepartmentService();

        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return projectManageService.GetPageList(pagination, queryJson);
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
        public IEnumerable<ProjectVo> GetPageListAPI(Pagination pagination, string queryJson)
        {
            try
            {
                return projectManageService.GetPageListAPI(pagination, queryJson);
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
        /// 多部门获取历史信息页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectVo> GetPageListDepartmentId(Pagination pagination, string queryJson,string dep)
        {
            try
            {
                return projectManageService.GetPageListDepartmentId(pagination, queryJson,dep);
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
        public List<ProjectDpVo> GetProjectMonthCount()
        {
            try
            {
                return projectManageService.GetProjectMonthCount();
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
        /// 根绝
        /// </summary>
        /// <returns></returns>
        public List<ProjectSourceVo> GetProjectCountBySource()
        {
            try
            {
                return projectManageService.GetProjectCountBySource();
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

        public List<ProvinceCount> GetProjectCountByProvince()
        {
            try
            {
                List<ProjectProvinceVo> projectProvinces = projectManageService.GetProjectCountByProvince();
                List<ProvinceCount> ProvinceCountList = new List<ProvinceCount>();
                foreach (var item in projectProvinces)
                {
                    AreaEntity area = areaService.GetEntity(item.ProvinceId);
                    ProvinceCount provinceCount = new ProvinceCount();
                    if (area != null)
                    {
                        provinceCount.name = area.F_AreaName.Replace("省", "");
                        provinceCount.value = item.ProjectCount;
                        ProvinceCountList.Add(provinceCount);
                    }
                }
                return ProvinceCountList;
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

        public List<ProjectConversionVo> GetProjectConversion()
        {
            try
            {
                return projectManageService.GetProjectConversion();
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

        public List<ProjectPaymentBackVo> GetBackProjectRate()
        {
            try
            {
                List<ProjectPaymentBackVo> backProjectList = projectManageService.GetBackProjectRate();
                foreach (var item in backProjectList)
                {
                    DepartmentEntity department = departmentService.GetEntity(item.DepartmentId);
                    if (department != null)
                    {
                        item.DepartmentName = department.F_FullName;
                    }
                }
                return backProjectList;
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
        /// 省市
        /// </summary>
        /// <param name="ProvinceId"></param>
        /// <param name="CityId"></param>
        /// <returns></returns>
             public ProjectVo ProvinceIdS(string id)
        {
            try
            {
                return projectManageService.ProvinceIdS(id);
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
        /// 县/区
        /// </summary>
        /// <param name="ProvinceId"></param>
        /// <param name="CityId"></param>
        /// <returns></returns>
        public ProjectVo ProvinceIdX(string Id)
        {
            try
            {
                return projectManageService.ProvinceIdX(Id);
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
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectVo> GetPageListAddress(Pagination pagination, string queryJson)
        {
            try
            {
                return projectManageService.GetPageListAddress(pagination, queryJson);
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
        public IEnumerable<ProjectVo> GetPageListAddressDepartmentId(Pagination pagination, string queryJsons,string dep)
        {
            try
            {
                return projectManageService.GetPageListAddressDepartmentId(pagination, queryJsons,dep);
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
        public IEnumerable<ProjectVo> GetPageListAddressDepartmentIds(Pagination pagination, string queryJsons, string dep)
        {
            try
            {
                return projectManageService.GetPageListAddressDepartmentIds(pagination, queryJsons, dep);
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
        /// 获取页面导出显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectVo> GetPageListAddress(string queryJson)
        {
            try
            {

                return projectManageService.GetPageListAddress(queryJson);
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
        /// 获取页面导出显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectVo> GetPageListAddress2(string queryJson)
        {
            try
            {

                return projectManageService.GetPageListAddress2(queryJson);
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
        /// 多部门获取页面导出显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectVo> GetPageListAddressDepartmentId(string queryJson,string dep)
        {
            try
            {

                return projectManageService.GetPageListAddressDepartmentId(queryJson,dep);
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
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectVo> GetProjectList(Pagination pagination, string queryJson)
        {
            try
            {
                return projectManageService.GetProjectList(pagination, queryJson);
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
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectVo> GetProjectList2(Pagination pagination, string queryJson)
        {
            try
            {
                return projectManageService.GetProjectList2(pagination, queryJson);
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
        /// 报备查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
             public ProjectVo GetToviewListProject(string id)
        {

            try
            {
                return projectManageService.GetToviewListProject(id);
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
        public ProjectVo PreviewIndexFrom(string keyValue)
        {

            try
            {
                return projectManageService.PreviewIndexFrom(keyValue);
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
        /// 
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectEntity> GetList(string queryJson)
        {
            try
            {
                return projectManageService.GetList(queryJson);
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
        /// 获取可以选择的项目信息
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectVo> GetSelectedProjectByContractList(Pagination pagination, string queryJson)
        {
            try
            {
                return projectManageService.GetSelectedProjectByContractList(pagination, queryJson);
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
        public IEnumerable<ProjectVo> GetSelectedProjectByContractList_2(Pagination pagination, string queryJson,string dept)
        {
            try
            {
                return projectManageService.GetSelectedProjectByContractList_2(pagination, queryJson, dept);
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
        /// 获取可以选择的项目信息开票添加
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectVo> GetSelectedProjectByContractListBilling(Pagination pagination, string queryJson)
        {
            try
            {
                return projectManageService.GetSelectedProjectByContractListBilling(pagination, queryJson);
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
        /// 多部门获取可以选择的项目信息开票添加
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectVo> GetSelectedProjectByContractListBillingDepartmentId(Pagination pagination, string queryJson,string dep)
        {
            try
            {
                return projectManageService.GetSelectedProjectByContractListBillingDepartmentId(pagination, queryJson,dep);
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
        /// 获取可以选择的项目信息
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectEntity> GetSelectedProjectList(Pagination pagination, string queryJson)
        {
            try
            {
                return projectManageService.GetSelectedProjectList(pagination, queryJson);
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
        }   /// <summary>
        /// 获取可以选择的项目信息
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectVo> GetSelectedProjectList1(Pagination pagination, string queryJson)
        {
            try
            {
                return projectManageService.GetSelectedProjectList1(pagination, queryJson);
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
        public IEnumerable<ProjectVo> GetSelectedProjectListWithoutContract(Pagination pagination, string queryJson)
        {
            try
            {
                return projectManageService.GetSelectedProjectListWithoutContract(pagination, queryJson);
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
        

        public IEnumerable<ProjectEntity> GetSelectedProjectListT(Pagination pagination, string queryJson)
        {
            try
            {
                return projectManageService.GetSelectedProjectListT(pagination, queryJson);
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
        public IEnumerable<ProjectEntity> GetSelectedProjectListTi(Pagination pagination, string queryJson)
        {
            try
            {
                return projectManageService.GetSelectedProjectListTi(pagination, queryJson);
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
        /// 获取Project表实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public ProjectEntity GetProjectEntity(string keyValue)
        {
            try
            {
                return projectManageService.GetProjectEntity(keyValue);
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
        }/// <summary>
         /// 获取Project表实体数据
         /// </summary>
         /// <param name="keyValue">主键</param>
         /// <returns></returns>
        public ProjectEntity GetFormDataByProcessIdBilling(string keyValue)
        {
            try
            {
                return projectManageService.GetProjectEntity(keyValue);
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
        /// 合同预览
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public ProjectEntity GetPreviewFormData(string keyValue)
        {
            try
            {
                return projectManageService.GetPreviewFormData(keyValue);
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
        public ProjectVo GetPreviewFormDataById(string keyValue)
        {
            try
            {
                return projectManageService.GetPreviewFormDataById(keyValue);
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
        public IEnumerable<ProjectFollowListEntity> GetProjectFollowList(string keyValue)
        {
            try
            {
                return projectManageService.GetProjectFollowList(keyValue);
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
        
        public ProjectVo GetPreviewFormDataBy(string keyValue)
        {
            try
            {
                return projectManageService.GetPreviewFormDataById(keyValue);
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
        /// 获取Project表实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public ProjectEntity GetProjectById(string keyValue)
        {
            try
            {
                return projectManageService.GetProjectEntity(keyValue);
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
        public ProjectEntity GetEntityByProcessId(string processId)
        {
            try
            {
                return projectManageService.GetEntityByProcessId(processId);
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
                projectManageService.DeleteEntity(keyValue);
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
        /// 判断当前的合同是否重复
        /// </summary>
        /// <param name="ContactPhone"></param>
        /// <param name="CustName"></param>
        /// <returns></returns>
        public int JudgePepeatProject(string ContactPhone, string CustName, string keyValue)
        {
            try
            {
                return projectManageService.JudgePepeatProject(ContactPhone, CustName, keyValue);
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
        /// 判断当前的合同是否重复
        /// </summary>
        /// <param name="ContactPhone"></param>
        /// <param name="CustName"></param>
        /// <returns></returns>
        public int JudgePepeatProjectBy(string ContactPhone, string CustName)
        {
            try
            {
                return projectManageService.JudgePepeatProjectBy(ContactPhone, CustName);
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
        public int JudgePepeatProjectName(string projectName, string keyValue)
        {
            try
            {
                return projectManageService.JudgePepeatProjectName(projectName, keyValue);
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
        public int JudgePepeatProjectNameBy(string projectName)
        {
            try
            {
                return projectManageService.JudgePepeatProjectNameBy(projectName);
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
        public void SaveEntity(string keyValue, ProjectEntity entity)
        {
            try
            {
                projectManageService.SaveEntity(keyValue, entity);
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
        public void SaveProjectFollowEntity(string keyValue, ProjectFollowListEntity entity)
        {
            try
            {
                projectManageService.SaveProjectFollowEntity(keyValue, entity);
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
        /// 新增
        /// </summary>
        /// <param name="entity">实体</param>
        public void AddEntity(ProjectEntity entity)
        {
            try
            {
                projectManageService.AddEntity(entity);
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
