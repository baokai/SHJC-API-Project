using Learun.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    public interface PaymentIBLL
    {
        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<PaymentEntity> GetPageList(Pagination pagination, string queryJson);
        IEnumerable<PaymentVo> GetPageList2(Pagination pagination, string queryJson);
        IEnumerable<PaymentVo> GetPageListAPI(Pagination pagination, string queryJson);
        /// <summary>
        /// 多部门获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<PaymentVo> GetPageListDepartmentId(Pagination pagination, string queryJson,string dep);
        /// <summary>
        /// 行政付款导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<PaymentVo> GetPageList(string queryJson,out string sql);
        /// <summary>
        /// 多部门行政付款导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<PaymentVo> GetPageListDepartmentId(string queryJson,string dep);
        /// <summary>
        /// 获取ProjectPayment表实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        PaymentEntity GetProjectPaymentEntity(string keyValue);
        /// <summary>
        /// 获取主表实体数据
        /// </summary>
        /// <param name="processId">流程实例ID</param>
        /// <returns></returns>
        PaymentVo GetEntityByProcessId(string processId);
        PaymentEntity GetPaymentEntityByProcessId(string processId);
        /// <summary>
        /// 付款预览
        /// </summary>
        /// <param name="keyValue">id</param>
        /// <returns></returns>
        PaymentVo GetPreviewProjectPayment(string keyValue);
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
        void SaveEntity(string keyValue, PaymentEntity entity);
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
