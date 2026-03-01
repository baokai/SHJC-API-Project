using Learun.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    public class PaymentBLL:PaymentIBLL
    {
        private PaymentService paymentService=new PaymentService();
         #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<PaymentEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return paymentService.GetPageList(pagination, queryJson);
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
        public IEnumerable<PaymentVo> GetPageList2(Pagination pagination, string queryJson)
        {
            try
            {
                return paymentService.GetPageList2(pagination, queryJson);
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
        public IEnumerable<PaymentVo> GetPageListAPI(Pagination pagination, string queryJson)
        {
            try
            {
                return paymentService.GetPageListAPI(pagination, queryJson);
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
        public IEnumerable<PaymentVo> GetPageListDepartmentId(Pagination pagination, string queryJson,string dep )
        {
            try
            {
                return paymentService.GetPageListDepartmentId(pagination, queryJson,dep);
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
        /// 行政付款导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<PaymentVo> GetPageList(string queryJson, out string sql)
        {
            try
            {
               
                return paymentService.GetPageList(queryJson,out sql);
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
        /// 行政付款导出多部门
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<PaymentVo> GetPageListDepartmentId(string queryJson,string dep)
        {
            try
            {
                return paymentService.GetPageListDepartmentId(queryJson,dep);
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
        /// 获取ProjectPayment表实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public PaymentEntity GetProjectPaymentEntity(string keyValue)
        {
            try
            {
                return paymentService.GetProjectPaymentEntity(keyValue);
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
        /// 页面预览信息
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public PaymentVo GetPreviewProjectPayment(string keyValue)
        {
            try
            {
                return paymentService.GetPreviewProjectPayment(keyValue);
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
        public PaymentVo GetEntityByProcessId(string processId)
        {
            try
            {
                return paymentService.GetEntityByProcessId(processId);
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
        public PaymentEntity GetPaymentEntityByProcessId(string processId)
        {
            try
            {
                return paymentService.GetPaymentEntityByProcessId(processId);
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
                paymentService.DeleteEntity(keyValue);
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
        public void SaveEntity(string keyValue, PaymentEntity entity)
        {
            try
            {
                paymentService.SaveEntity(keyValue, entity);
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
                paymentService.UpdateFlowId(keyValue, ProcessId);
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
        /// 变更
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="ProcessId"></param>
        public void UpdateFlowIdStatus(string keyValue, string ProcessId)
        {
            try
            {
                paymentService.UpdateFlowIdStatus(keyValue, ProcessId);
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

        /*/// <summary>
        /// 删除实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void DeleteEntity(string keyValue)
        {
            try
            {
                paymentService.DeleteEntity(keyValue);
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
*/
        /* /// <summary>
         /// 保存实体数据（新增、修改）
         /// </summary>
         /// <param name="keyValue">主键</param>
         /// <param name="entity">实体</param>
         public void SaveEntity(string keyValue, PaymentEntity entity)
         {
             try
             {
                 paymentService.SaveEntity(keyValue, entity);
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
                 paymentService.UpdateFlowId(keyValue, ProcessId);
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
         /// 变更
         /// </summary>
         /// <param name="keyValue"></param>
         /// <param name="ProcessId"></param>
         public void UpdateFlowIdStatus(string keyValue, string ProcessId)
         {
             try
             {
                 paymentService.UpdateFlowIdStatus(keyValue, ProcessId);
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
         }*/
        #endregion

    }
}
