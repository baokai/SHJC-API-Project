using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learun.Application.Extention.TaskScheduling
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2019-01-09 16:07
    /// 描 述：任务执行日志
    /// </summary>
    public class TSLogService : RepositoryFactory
    {
        #region 获取数据 

        /// <summary> 
        /// 获取页面显示列表数据 
        /// <summary> 
        /// <param name="queryJson">查询参数</param> 
        /// <returns></returns> 
        public IEnumerable<TSLogEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(@" 
                            SELECT
	                            t.F_Id,
	                            t.F_ProcessId,
	                            t.F_ExecuteResult,
	                            t.F_CreateDate,
	                            t.F_Des,
	                            s.F_Name
                            FROM
	                            LR_TS_Log t
                            LEFT JOIN LR_TS_Process p ON p.F_Id = t.F_ProcessId
                            LEFT JOIN LR_TS_SchemeInfo s ON s.F_Id = p.F_SchemeInfoId
                ");
                strSql.Append("  WHERE 1=1 ");
                var queryParam = queryJson.ToJObject();

                DateTime startTime = DateTime.Now, endTime = DateTime.Now;
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    startTime = queryParam["StartTime"].ToDate();
                    endTime = queryParam["EndTime"].ToDate();
                    strSql.Append(" AND ( t.F_CreateDate >= @startTime AND  t.F_CreateDate <= @endTime ) ");
                }
                string keyword = "";
                if (!queryParam["keyword"].IsEmpty())
                {
                    keyword = "%" + queryParam["keyword"].ToString() + "%";
                    strSql.Append(" AND ( s.F_Name like @keyword ) ");
                }

                int executeResult = 1;
                if (!queryParam["executeResult"].IsEmpty())
                {
                    executeResult = Convert.ToInt32(queryParam["executeResult"].ToString());
                    strSql.Append(" AND t.F_ExecuteResult = @executeResult ");
                }


                return this.BaseRepository().FindList<TSLogEntity>(strSql.ToString(), new { startTime, endTime, keyword , executeResult }, pagination);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        /// <summary> 
        /// 获取LR_TS_Log表实体数据 
        /// <param name="keyValue">主键</param> 
        /// <summary> 
        /// <returns></returns> 
        public TSLogEntity GetLogEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository().FindEntity<TSLogEntity>(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        #endregion

        #region 提交数据 

        /// <summary> 
        /// 删除实体数据 
        /// <param name="keyValue">主键</param> 
        /// <summary> 
        /// <returns></returns> 
        public void DeleteEntity(string keyValue)
        {
            try
            {
                this.BaseRepository().Delete<TSLogEntity>(t => t.F_Id == keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        /// <summary> 
        /// 保存实体数据（新增、修改） 
        /// <param name="keyValue">主键</param> 
        /// <summary> 
        /// <returns></returns> 
        public void SaveEntity(string keyValue, TSLogEntity entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
                else
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        #endregion
    }
}
