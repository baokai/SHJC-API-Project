using Learun.Util;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 描 述：项目合同申请
    /// </summary>
    public class ProjectContractBLL : ProjectContractIBLL
    {
        private ProjectContractService projectContractService = new ProjectContractService();

        private ProjectPaymentService projectPaymentService = new ProjectPaymentService();

        private ProjectPaymentListService projectPaymentListService = new ProjectPaymentListService();

        private ProjectBillingService projectBillingService = new ProjectBillingService();
        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectContractVo> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return projectContractService.GetPageList(pagination, queryJson);
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
        /// 二期营销统计图待回款
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public IEnumerable<ProducTionVo> GetMoneyToBeCollected(string dep)
        {
            try
            {
                return projectContractService.GetMoneyToBeCollected(dep);
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
        /// 二期营销统计图待回款多部门
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public IEnumerable<ProducTionVo> GetMoneyToBeCollectedDepartmentId(string dep)
        {
            try
            {
                return projectContractService.GetMoneyToBeCollectedDepartmentId(dep);
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
        /// 全年项目数量
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public IEnumerable<YearRoundVo> GetyearRoundNumberOfTtems()
        {
            try
            {
                return projectContractService.GetyearRoundNumberOfTtems();
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
        public IEnumerable<YearRoundVo> GetyearRoundNumberOfTtemsByDeptids(List<string> deptIds)
        {
            try
            {
                return projectContractService.GetyearRoundNumberOfTtemsByDeptids(deptIds);
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
        public IEnumerable<YearRoundVo> GetMonthlyRoundNumberOfTtems()
        {
            try
            {
                return projectContractService.GetMonthlyRoundNumberOfTtems();
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
        public IEnumerable<YearRoundVo> GetMonthlyRoundNumberOfTtemsByDeptids(List<string> deptIds)
        {
            try
            {
                return projectContractService.GetMonthlyRoundNumberOfTtemsByDeptids(deptIds);
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
        /// 全年已实施数量
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public IEnumerable<YearRoundVo> GetyearRoundHaveBeenImplemented()
        {
            try
            {
                return projectContractService.GetyearRoundHaveBeenImplemented();
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
        /// 全年待实施数量/金额
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public IEnumerable<YearRoundVo> GetyearRoundToBeImplemented()
        {
            try
            {
                return projectContractService.GetyearRoundToBeImplemented();
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
        /// ②公司全年合同额承揽、承揽金额、开票金额
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public IEnumerable<YearRoundAmountVo> GetyearRoundAmountOfContract()
        {
            try
            {
                return projectContractService.GetyearRoundAmountOfContract();
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
        public IEnumerable<YearRoundAmountVo> GetyearRoundAmountOfContractByDeptids(List<string> deptIds)
        {
            try
            {
                return projectContractService.GetyearRoundAmountOfContractByDeptids(deptIds);
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
        public IEnumerable<YearRoundAmountVo> GetmonthlyRoundAmountOfContract()
        {
            try
            {
                return projectContractService.GetmonthlyRoundAmountOfContract();
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
        public IEnumerable<YearRoundAmountVo> GetmonthlyRoundAmountOfContractByDeptids(List<string> deptIds)
        {
            try
            {
                return projectContractService.GetmonthlyRoundAmountOfContractByDeptids(deptIds);
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
        /// 二期营销统计图
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public IEnumerable<ProducTionVo> GetMarkeTingList(string dep)
        {
            try
            {
                return projectContractService.GetMarkeTingList(dep);
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
        /// 多部门二期营销统计图
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public IEnumerable<ProducTionVo> GetMarkeTingListDepartmentId(string dep)
        {
            try
            {
                return projectContractService.GetMarkeTingListDepartmentId(dep);
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
        public IEnumerable<ProjectContractVo> GetPageListDepartmentId(Pagination pagination, string queryJson, string dep)
        {
            try
            {
                return projectContractService.GetPageListDepartmentId(pagination, queryJson, dep);
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
        /// 根据id修改状态
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ProjectContractVo UPGetPageListName(string queryJson)
        {
            try
            {
                return projectContractService.UPGetPageListName(queryJson);
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
        /// 根据projectId查找对应的承揽合同的和
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ProjectContractVo GetPageListsumcl(string projectid)
        {
            try
            {
                return projectContractService.GetPageListsumcl(projectid);
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
        /// 根据projectId查找对应的分包合同的和
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ProjectContractVo GetPageListsumfb(string projectid)
        {
            try
            {
                return projectContractService.GetPageListsumfb(projectid);
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
        /// 根据id求和
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ProjectContractVo GetPageListsum(string queryJson)
        {
            try
            {
                return projectContractService.GetPageListsum(queryJson);
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
        /// 根据id修改状态
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ProjectContractVo UPGetPageListNameQX(string queryJson)
        {
            try
            {
                return projectContractService.UPGetPageListNameQX(queryJson);
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
        /// 查看是否有这个项目名
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ProjectContractVo GetPageListName(string queryJson)
        {
            try
            {
                return projectContractService.GetPageListName(queryJson);
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
        /// 相同合同编号自动归档
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectContractVo> GetContractPageList()
        {
            try
            {
                return projectContractService.GetContractPageList();
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
        public IEnumerable<ProjectContractVo> GetPageList(string queryJson)
        {
            try
            {
                return projectContractService.GetPageList(queryJson);
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
        /// 查询所有合同
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectContractVo> GetPageListEffectiveAmount()
        {
            try
            {
                return projectContractService.GetPageListEffectiveAmount();
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
        /// 根据id查找对应的总合同金额和外付
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public ProjectContractVo GetPageListEffectiveAmountProjectId(string ProjectId)
        {
            try
            {
                return projectContractService.GetPageListEffectiveAmountProjectId(ProjectId);
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
        /// 多部门导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectContractVo> GetPageListDepartmentId(string queryJson, string dep)
        {
            try
            {
                return projectContractService.GetPageListDepartmentId(queryJson, dep);
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
        /// 根据ID获取
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<ProjectContractEntity> GetProjectContractByProjectId(string projectId)
        {
            try
            {
                return projectContractService.GetProjectContractByProjectId(projectId);
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
        /// 根据projectId获取合同部门
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public ProjectContractEntity GetProjectContractByDepartmentIdProjectId(string projectId)
        {
            try
            {
                return projectContractService.GetProjectContractByDepartmentIdProjectId(projectId);
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

        public ProjectContractVo GetProjectContractProjectId(string projectId)
        {
            try
            {
                return projectContractService.GetProjectContractProjectId(projectId);
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
        /// 合同增补查询
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ProjectContractVo GetFillProjectContractEntity(string keyValue)
        {
            try
            {
                return projectContractService.GetFillProjectContractEntity(keyValue);
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
        public ProjectContractVo GetPreviewFormData(string keyValue)
        {
            try
            {
                return projectContractService.GetPreviewFormData(keyValue);
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
        /// 根据projectId查询对应的合同
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public ProjectContractVo GetProjectContracByprojectId(string projectId)
        {
            try
            {
                return projectContractService.GetProjectContracByprojectId(projectId);
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
        /// 获取ProjectContract表实体数据(分页)
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public ProjectContractEntity GetProjectContractEntity(string keyValue)
        {
            try
            {
                return projectContractService.GetProjectContractEntity(keyValue);
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
        /// 获取ProjectContract表实体数据(分页)
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public IEnumerable<ProjectContractEntity> GetProjectContractEntityBycNo(string keyValue)
        {
            try
            {
                return projectContractService.GetProjectContractEntityBycNo(keyValue);
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

        public void UpdateReceivedFlag(string keyValue, ProjectContractEntity entity)
        {
            try
            {
                projectContractService.UpdateReceivedFlag(keyValue, entity);
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
                projectContractService.UpdateFlowId1(keyValue, ProcessId);
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
        /// 更改
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="ProcessId"></param>
        public void UpdateContractStatus(string keyValue, string ProcessId)
        {
            try
            {
                projectContractService.UpdateContractStatus(keyValue, ProcessId);
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
        /// 重新提交审批流
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="processId"></param>
        public void UpdateFlowIdTo(string keyValue, string processId)
        {
            try
            {
                projectContractService.UpdateFlowIdTo(keyValue, processId);
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
        /// 报告查合同
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public ProjectContractVo ProjectTaskByContract(string projectId)
        {
            try
            {
                return projectContractService.ProjectTaskByContract(projectId);
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
        public ProjectContractVo ProjectTaskByContractId(string contractId)
        {
            try
            {
                return projectContractService.ProjectTaskByContractId(contractId);
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
        /// 根据报备id查相关合同
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public IEnumerable<ProjectContractVo> GetProjectContract(string ProjectId)
        {
            try
            {
                return projectContractService.GetProjectContract(ProjectId);
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
        public ProjectContractVo GetEntityByProcessId(string processId)
        {
            try
            {
                return projectContractService.GetEntityByProcessId(processId);
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
        public ProjectContractEntity GetContractEntityByProcessId(string processId)
        {
            try
            {
                return projectContractService.GetContractEntityByProcessId(processId);
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
        /// 根据流程id获取合同编号
        /// </summary>
        /// <param name="processId">流程实例ID</param>
        /// <returns></returns>
        public ProjectContractEntity GetEntityByContractNo(string ContractNo)
        {
            try
            {
                return projectContractService.GetEntityByContractNo(ContractNo);
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
        public ProjectContractEntity GetContractByProcessId(string ProcessId)
        {
            try
            {
                return projectContractService.GetContractByProcessId(ProcessId);
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
        /// 根据编号查询
        /// </summary>
        /// <param name="ContractNo"></param>
        /// <returns></returns>
        public IEnumerable<ProjectContractVo> GetEntityByContractNoList(string ContractNo)
        {
            try
            {
                return projectContractService.GetEntityByContractNoList(ContractNo);
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
        public IEnumerable<ProjectContractVo> GetPageListCont(string ContractNo)
        {
            try
            {
                return projectContractService.GetPageListCont(ContractNo);
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
        /// 根据流程id获取合同编号
        /// </summary>
        /// <param name="processId">流程实例ID</param>
        /// <returns></returns>
        public ProjectContractEntity GetEntityByContractNoProcessId(string processId)
        {
            try
            {
                return projectContractService.GetEntityByContractNoProcessId(processId);
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
        public ProjectContractVo GetPageListContractAmountfb2(string ProjectId, string id)
        {
            try
            {
                return projectContractService.GetPageListContractAmountfb2(ProjectId, id);
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
                projectContractService.DeleteEntity(keyValue);
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
        /// 保存实体数据（增补）
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体</param>
        public void FillSaveForm(ProjectContractEntity entity)
        {
            try
            {
                projectContractService.FillSaveForm(entity);
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
        public void SaveProjectSettlement(ProjectContractSettlementEntity entity)
        {
            try
            {
                projectContractService.SaveProjectSettlement(entity);
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
        public ProjectContractSettlementEntity GetSettlementByProjectId(string projectId)
        {
            try
            {
                return projectContractService.GetSettlementByProjectId(projectId);
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
        public ProjectContractSettlementEntity GetSettlementByContractId(string ContractId)
        {
            try
            {
                return projectContractService.GetSettlementByContractId(ContractId);
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
        public void SaveEntity(string keyValue, ProjectContractEntity entity)
        {
            try
            {
                if (string.IsNullOrEmpty(entity.ProjectSource))
                {
                    entity.ProjectSource = "1";
                }
                projectContractService.SaveEntity(keyValue, entity);
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
        public void RecaculateEffectiveAmount()
        {
            try
            {
                projectContractService.RecaculateEffectiveAmount();
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
        public void RecaculateEffectiveAmountByProjectId(string projectId)
        {
            try
            {
                projectContractService.RecaculateEffectiveAmountByProjectId(projectId);
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
        public void SaveEntity1(string keyValue, ProjectContractEntity entity)
        {
            try
            {
                projectContractService.SaveEntity1(keyValue, entity);
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
        public void SaveEntityEffectiveAmount(string keyValue, ProjectContractEntity entity)
        {
            try
            {
                projectContractService.SaveEntity(keyValue, entity);
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
        /// 合作伙伴结算台账添加比例
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveFormReportForms(string keyValue, ProjectContractEntity entity)
        {
            try
            {
                projectContractService.SaveFormReportForms(keyValue, entity);
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

        public void update(string keyValue, string strEntity)
        {
            try
            {
                projectContractService.update(keyValue, strEntity);
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
        public IEnumerable<ProjectContractVo> ProjectContract(string projectId)
        {
            try
            {
                return projectContractService.ProjectContract(projectId);
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

        public IEnumerable<ProjectContractVo> GetContract(string queryJson)
        {
            try
            {
                return projectContractService.GetContract(queryJson);
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

        public void ReMatchPaymentFromContract()
        {
            var paymentList = projectPaymentService.GetAllPaymentToMatchContract();
            var contractList = projectContractService.GetAllContractFroMatch();
            foreach (var item in paymentList)
            {
                var matcedContract = contractList.Where(i => i.Id == item.ProjectId).ToList();
                if (matcedContract.Count == 1)
                {
                    item.ContractId = matcedContract.FirstOrDefault().ContractId;
                    if (item.type == 1)
                    {
                        projectPaymentListService.SaveEntity(item.id, item);
                    }
                    else if (item.type == 0)
                    {
                        projectPaymentService.SaveEntity(item.id, item);
                    }
                }
            }
        }
        public void ReMatchBillingFromContract()
        {
            var billingList = projectBillingService.GetAllBillingToMatchContract();
            var contractList = projectContractService.GetAllContractFroMatch();
            foreach (var item in billingList)
            {
                var matcedContract = contractList.Where(i => i.Id == item.ProjectId).ToList();
                if (matcedContract.Count == 1)
                {
                    item.ContractId = matcedContract.FirstOrDefault().ContractId;
                    projectBillingService.SaveEntity(item.Id, item);
                }
            }
        }

    }
}
