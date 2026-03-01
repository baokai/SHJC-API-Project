using Learun.Util;
using System.Data;
using System.Collections.Generic;
using System;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2022-03-10 23:22
    /// 描 述：项目合同申请
    /// </summary>
    public interface ProjectContractIBLL
    {
        #region 获取数据 
        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<ProjectContractVo> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 营销统计图
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        IEnumerable<ProducTionVo> GetMarkeTingList(string dep);
        /// <summary>
        /// 营销统计图待回款
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        IEnumerable<ProducTionVo> GetMoneyToBeCollected(string dep);  
        /// <summary>
        /// 营销统计图待回款多部门
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        IEnumerable<ProducTionVo> GetMoneyToBeCollectedDepartmentId(string dep);
        /// <summary>
        /// 全年项目数量
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        IEnumerable<YearRoundVo> GetyearRoundNumberOfTtems();
        IEnumerable<YearRoundVo> GetyearRoundNumberOfTtemsByDeptids(List<string> deptIds);
        IEnumerable<YearRoundVo> GetMonthlyRoundNumberOfTtems();
        IEnumerable<YearRoundVo> GetMonthlyRoundNumberOfTtemsByDeptids(List<string> deptIds);
        /// <summary>
        /// 全年已实施数量
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        IEnumerable<YearRoundVo> GetyearRoundHaveBeenImplemented();
        /// <summary>
        /// 全年待实施数量/金额
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
         IEnumerable<YearRoundVo> GetyearRoundToBeImplemented();
        /// <summary>
        /// ②公司全年合同额承揽、承揽金额、开票金额
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        IEnumerable<YearRoundAmountVo> GetyearRoundAmountOfContract();
        IEnumerable<YearRoundAmountVo> GetmonthlyRoundAmountOfContract();
        IEnumerable<YearRoundAmountVo> GetmonthlyRoundAmountOfContractByDeptids(List<string> deptIds);
        IEnumerable<YearRoundAmountVo> GetyearRoundAmountOfContractByDeptids(List<string> deptIds);
        /// <summary>
        /// 营销统计图多部门
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        IEnumerable<ProducTionVo> GetMarkeTingListDepartmentId(string dep);
        /// <summary>
        /// 多部门获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<ProjectContractVo> GetPageListDepartmentId(Pagination pagination, string queryJson,string dep);
        /// <summary>
        /// 多部门导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="dep"></param>
        /// <returns></returns>
        IEnumerable<ProjectContractVo> GetPageListDepartmentId(string queryJson,string dep);
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<ProjectContractVo> GetPageList(string queryJson);
        /// <summary>
        /// 查询所有合同
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
         IEnumerable<ProjectContractVo> GetPageListEffectiveAmount();
        /// <summary>
        /// 
        /// 根据id查找对应的总合同金额和外付
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        ProjectContractVo GetPageListEffectiveAmountProjectId(string ProjectId);
        /// <summary>
        /// 相同合同编号自动归档
        /// </summary>
        /// <returns></returns>
        IEnumerable<ProjectContractVo> GetContractPageList(); 
        /// <summary>
        /// 根据项目id获取对于的合同信息
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        List<ProjectContractEntity> GetProjectContractByProjectId(string projectId);
        /// <summary>
        /// 根据projectId获取合同部门
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        ProjectContractEntity GetProjectContractByDepartmentIdProjectId(string projectId);
        ProjectContractVo GetProjectContractProjectId(string projectId);
        /// <summary>
        /// 根据项目id获取对于的合同信息
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        ///ProjectContractVo GetProjectContractById(string projectId);
        /// <summary>
        /// 获取ProjectContract表实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        ProjectContractEntity GetProjectContractEntity(string keyValue);
        /// <summary>
        /// 根据报备id查合同
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        IEnumerable<ProjectContractVo> GetProjectContract(string ProjectId);
        IEnumerable<ProjectContractEntity> GetProjectContractEntityBycNo(string keyValue);
        /// <summary>
        /// 合同预览
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        ProjectContractVo GetPreviewFormData(string keyValue); 
        ProjectContractVo GetFillProjectContractEntity(string keyValue);
       
        /// <summary>
        /// 更新是否收到合同状态
        /// </summary>
        /// <param name="keyValue"></param>
        void UpdateReceivedFlag(string keyValue, ProjectContractEntity entity);
        /// <summary>
        /// 提交审批
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="ProcessId"></param>
        void UpdateFlowId(string keyValue, string ProcessId);
        /// <summary>
        /// 更改审批
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="ProcessId"></param>
        void UpdateContractStatus(string keyValue, string ProcessId); 
        /// <summary>
        /// 报告查合同
        /// </summary>
        
        /// <returns></returns>
        ProjectContractVo ProjectTaskByContract(string projectId);
        ProjectContractVo ProjectTaskByContractId(string contractId);
        /// <summary>
        /// 获取主表实体数据
        /// </summary>
        /// <param name="processId">流程实例ID</param>
        /// <returns></returns>
        ProjectContractVo GetEntityByProcessId(string processId);
        ProjectContractEntity GetContractEntityByProcessId(string processId);
        /// <summary>
        /// 根据流程id获取合同编号
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        ProjectContractEntity GetEntityByContractNoProcessId(string processId);
        /// <summary>
        /// 重新提交审批流
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="processId"></param>
        void UpdateFlowIdTo(string keyValue, string processId);
        /// <summary>
        /// 根据projectId查询对应的合同
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        ProjectContractVo GetProjectContracByprojectId(string projectId);
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
        void SaveEntity(string keyValue, ProjectContractEntity entity);
        void RecaculateEffectiveAmount();
        void RecaculateEffectiveAmountByProjectId(string projectId);
        void SaveEntity1(string keyValue, ProjectContractEntity entity);
        void SaveEntityEffectiveAmount(string keyValue, ProjectContractEntity entity);
        /// <summary>
        /// 合作伙伴结算添加比例
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        void SaveFormReportForms(string keyValue, ProjectContractEntity entity);
        /// <summary>
        /// 增补合同
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        void FillSaveForm(ProjectContractEntity entity);
        void update(string keyValue, string strEntity);
        IEnumerable<ProjectContractVo> GetContract(string queryJson);
        IEnumerable<ProjectContractVo> ProjectContract(string projectId);

        /// <summary>
        /// 根据合同编号查询是否有这个编号
        /// </summary>
        /// <param name="ContractNo"></param>
        /// <returns></returns>
        ProjectContractEntity GetEntityByContractNo(string ContractNo);

        ProjectContractEntity GetContractByProcessId(string ProcessId);
        
        IEnumerable<ProjectContractVo> GetPageListCont(string cont);
        /// <summary>
        /// 查看是否有这个项目名
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
       ProjectContractVo GetPageListName(string queryJson);
        /// <summary>
        /// 根据id修改状态
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        ProjectContractVo UPGetPageListName(string queryJson);
        /// <summary>
        /// 根据id修改状态
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        ProjectContractVo UPGetPageListNameQX(string queryJson);
        /// <summary>
        /// 根据id求和
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        ProjectContractVo GetPageListsum(string queryJson);
        /// <summary>
        /// 根据projectId查找对应的承揽合同的和
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
         ProjectContractVo GetPageListsumcl(string projectid);  /// <summary>
        /// 根据projectId查找对应的分包合同的和
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
         ProjectContractVo GetPageListsumfb(string projectid);
         ProjectContractVo GetPageListContractAmountfb2(string ProjectId, string id);

        #endregion

        void ReMatchPaymentFromContract();

        void ReMatchBillingFromContract();

    }
}
