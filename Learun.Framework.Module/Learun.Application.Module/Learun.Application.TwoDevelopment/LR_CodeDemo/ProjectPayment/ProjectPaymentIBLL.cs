using Learun.Util;
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
    public interface ProjectPaymentIBLL
    {
        #region 获取数据
        /// <summary>
        /// 根据报备id查询相关付款
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<ProjectPaymentVo> GetPayment(string id);
        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<ProjectPaymentVo> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 多部门列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<ProjectPaymentVo> GetPageListDepartmentId(Pagination pagination, string queryJson,string dep);
        /// <summary>
        /// 付款导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<ProjectPaymentVo> GetPageList(string queryJson); 
        /// <summary>
        /// 多部门付款导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<ProjectPaymentVo> GetPageListDepartmentId(string queryJson,string dep);
        /// <summary>
        /// 获取ProjectPayment表实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        ProjectPaymentEntity GetProjectPaymentEntity(string keyValue); 
        /// <summary>
        /// 获取主表实体数据
        /// </summary>
        /// <param name="processId">流程实例ID</param>
        /// <returns></returns>
        ProjectPaymentVo GetEntityByProcessId(string processId);

        ProjectPaymentEntity GetProjectPaymentEntityByProcessId(string processId);
        /// <summary>
        /// 批量付款获取总金额
        /// </summary>
        /// <param name="tid">tid</param>
        /// <returns></returns>

        ProjectPaymentVo GetAmountSumByTid(string tid);
        
        /// <summary>
        /// 付款预览
        /// </summary>
        /// <param name="keyValue">id</param>
        /// <returns></returns>
        ProjectPaymentVo GetPreviewProjectPayment(string keyValue);
        List<ProjectPaymentVo> GetProjectPaymentByTid(string keyValue);
        /// <summary>
        /// 根据projectId查看有没有对应的项目
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        ProjectPaymentVo GetProjectPaymentByprojectId(string projectId);
        #endregion

        #region 提交数据

        /// <summary>
        /// 删除实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void DeleteEntity(string keyValue);
        void DeleteByTid(string keyValue);
        /// <summary>
        /// 保存实体数据（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体</param>
        void SaveEntity(string keyValue, ProjectPaymentEntity entity);
        void SaveEntity2(string keyValue, ProjectPaymentEntity entity);
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="entity"></param>
        void SaveEntityList(string Ids, ProjectPaymentEntity entity);
        //void ReMatchPaymentFromContract();
        /// <summary>
        /// 提交审批
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="ProcessId"></param>
        void UpdateFlowId(string keyValue, string ProcessId);
        /// <summary>
        /// 变更
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="ProcessId"></param>
        void UpdateFlowIdStatus(string keyValue, string ProcessId);
        #endregion


    }
}
