using Learun.Util;
using System.Data;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2022-03-11 00:04
    /// 描 述：合同支付
    /// </summary>
    public interface ProjectPaymentListIBLL
    {
        #region 获取数据
        /// <summary>
        /// 查付款批量添加所有数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         IEnumerable<ProjectPaymentListVo> GetPaymentList(string ProjectId);
       ProjectPaymentListVo GetPaymentListBy(string ProjectId);
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
        /// 付款导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<ProjectPaymentVo> GetPageList(string queryJson);
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
        IEnumerable<ProjectPaymentVo> GetEntityBytID(string processId);
        /// <summary>
        /// 付款预览
        /// </summary>
        /// <param name="keyValue">id</param>
        /// <returns></returns>
        ProjectPaymentVo GetPreviewProjectPayment(string keyValue);
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
        /// <summary>
        /// 保存实体数据（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体</param>
        void SaveEntity(string keyValue, ProjectPaymentEntity entity);
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="entity"></param>
        void SaveEntityList(string Ids, ProjectPaymentEntity entity, List<BatchAuditAddModel> item_list);
        /// <summary>
        /// 提交审批
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="ProcessId"></param>
        void UpdateFlowId(string keyValue, string ProcessId);
       
        #endregion
    }
}
