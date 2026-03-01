using Dapper;
using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2019-05-08 18:30
    /// 描 述：甘特图应用
    /// </summary>
    public class GantProjectService : RepositoryFactory
    {
        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// <summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<LR_OA_ProjectEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.F_Id,
                t.F_ProjectName,
                t.F_StartTime,
                t.F_EndTime,
                t.F_Remark,
                t.F_Status
                ");
                strSql.Append("  FROM LR_OA_Project t ");
                strSql.Append("  WHERE 1=1 ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["F_ProjectName"].IsEmpty())
                {
                    dp.Add("F_ProjectName", "%" + queryParam["F_ProjectName"].ToString() + "%", DbType.String);
                    strSql.Append(" AND t.F_ProjectName Like @F_ProjectName ");
                }
                return this.BaseRepository().FindList<LR_OA_ProjectEntity>(strSql.ToString(), dp, pagination);
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
        /// 获取LR_OA_ProjectDetail表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<LR_OA_ProjectDetailEntity> GetLR_OA_ProjectDetailList(string parentId)
        {
            try
            {
                return this.BaseRepository().FindList<LR_OA_ProjectDetailEntity>("select * from LR_OA_ProjectDetail where F_ParentId='" + parentId + "' order by F_StartTime");
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
        /// 获取LR_OA_Project表实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public LR_OA_ProjectEntity GetLR_OA_ProjectEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository().FindEntity<LR_OA_ProjectEntity>(keyValue);
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
        /// 获取LR_OA_ProjectDetail表实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public LR_OA_ProjectDetailEntity GetLR_OA_ProjectDetailEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository().FindEntity<LR_OA_ProjectDetailEntity>(t => t.F_Id == keyValue);
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
        /// 获取项目列表
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public IEnumerable<LR_OA_ProjectEntity> GetList(string keyValue)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                return this.BaseRepository().FindList<LR_OA_ProjectEntity>();
            }
            else
            {
                return this.BaseRepository().FindList<LR_OA_ProjectEntity>(t => t.F_ProjectName.Contains(keyValue));
            }
        }
        /// <summary>
        /// 获取项目明细列表
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public IEnumerable<LR_OA_ProjectDetailEntity> GetDetailList(string parentId)
        {
            return this.BaseRepository().FindList<LR_OA_ProjectDetailEntity>(t => t.F_ParentId == parentId);
        }
        public IEnumerable<LeaderboardVo> GetContractAmountLeaderboard(string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT top 5 ");
                strSql.Append(@"               
                t.DepartmentId,
                lbd.F_FullName as DepartmentName,
	            SUM(t.ContractAmount ) as TotalAmount                         
                ");
                strSql.Append(@"FROM ProjectContract t
                                INNER JOIN Project a ON a.Id = t.ProjectId
                                inner join adms706.dbo.lr_base_user lbu on t.CreateUser = lbu.F_UserId
                                inner join adms706.dbo.lr_base_department lbd on lbu.F_DepartmentId = lbd.F_DepartmentId");
                strSql.Append(" WHERE a.ProjectStatus = 1 AND t.ContractStatus <> 1 AND t.ContractStatus <> 6 AND t.ContractStatus <> 7 AND t.ContractStatus <> 11 and lbd.HZ_DepartmentId != 1");

                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                string createTime = "";
                if (!queryParam["datetime"].IsEmpty())
                {
                    var create_time = queryParam["datetime"].ToObject<List<string>>();
                    if (create_time.Count > 0)
                    {
                        string create_time_start_date = create_time[0];
                        string create_time_end_date = create_time[1].ToDate().AddDays(1).ToString();
                        createTime = " AND ( t.CreateTime >= '" + create_time_start_date + "' AND t.CreateTime < '" + create_time_end_date + "' )";
                        strSql.Append(createTime);
                    }
                }
                strSql.Append("group by t.DepartmentId,lbd.F_FullName ORDER BY TotalAmount desc ");
                var vo = this.BaseRepository("learunOAWFForm").FindList_NodbWhere<LeaderboardVo>(strSql.ToString());
                return vo;
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
        public IEnumerable<LeaderboardVo> GetTaskFinishedRateLeaderboard(string queryJson)
        {
            try
            {
                string query1 = "WITH SplitInspector AS (SELECT id, ProjectId, TaskStatus, CreateTime, PlanTime, ActualApproachTime, Inspector, CAST('<X>' + REPLACE(Inspector, ',', '</X><X>') + '</X>' AS XML) AS InspectorXML FROM projectTask WHERE TaskStatus <> 11), ";
                string query2 = "ParsedInspector AS (SELECT id, ProjectId, TaskStatus, CreateTime, PlanTime, ActualApproachTime, Inspector, InspectorXML, InspectorXML.value('(/X[1]/text())[1]', 'NVARCHAR(MAX)') AS userid, 1 AS POSITION FROM SplitInspector " +
                    "UNION ALL SELECT p.id, p.ProjectId, p.TaskStatus, p.CreateTime, p.PlanTime, p.ActualApproachTime, p.Inspector, p.InspectorXML, p.InspectorXML.value('(/X[sql:column(\"Position\") + 1]/text())[1]', 'NVARCHAR(MAX)') AS userid, POSITION + 1 FROM ParsedInspector p WHERE p.userid IS NOT NULL) " +
                    "SELECT TOP 5 lbu.F_DepartmentId AS DepartmentId, lbd.F_FullName AS DepartmentName, (SUM(CASE WHEN CONVERT(DATE, ActualApproachTime) <= CONVERT(DATE, PlanTime) THEN 1 ELSE 0 END) * 100) AS finishedCount, (SUM(CASE WHEN CONVERT(DATE, ActualApproachTime) <= CONVERT(DATE, PlanTime) THEN 1 ELSE 0 END) * 100 / COUNT(*)) AS finishedCountRate, COUNT(*) AS TotalCount " +
                    "FROM ParsedInspector t INNER JOIN adms706.dbo.lr_base_user lbu ON t.userid = lbu.F_UserId INNER JOIN adms706.dbo.lr_base_department lbd ON lbd.F_DepartmentId = lbu.F_DepartmentId WHERE userid IS NOT NULL AND lbd.HZ_DepartmentId != 1";
                    



                var strSql = new StringBuilder();
                strSql.Append(query1);

                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                string createTime = "";
                if (!queryParam["datetime"].IsEmpty())
                {
                    var create_time = queryParam["datetime"].ToObject<List<string>>();
                    if (create_time.Count > 0)
                    {
                        string create_time_start_date = create_time[0];
                        string create_time_end_date = create_time[1].ToDate().AddDays(1).ToString();
                        createTime = " AND ( CreateTime >= '" + create_time_start_date + "' AND CreateTime < '" + create_time_end_date + "' )";
                    }
                }
                strSql.Append(query2 + createTime + " GROUP BY lbu.F_DepartmentId, lbd.F_FullName ORDER BY finishedCountRate DESC;");
                var vo = this.BaseRepository("learunOAWFForm").FindList_NodbWhere<LeaderboardVo>(strSql.ToString());
                return vo;
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
        public IEnumerable<LeaderboardVo> GetCollectionAmountLeaderboard(string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT top 5 ");
                strSql.Append(@"               
                lbu.F_DepartmentId AS DepartmentId,
                lbd.F_FullName as DepartmentName,
	            SUM(t.Amount ) as TotalAmount                         
                ");
                strSql.Append(@"FROM
	                            ProjectPayCollection t
	                            INNER JOIN Project p ON t.ProjectId = p.Id
	                            INNER JOIN adms706.dbo.lr_base_user lbu ON p.CreateUser = lbu.F_UserId
	                            INNER JOIN adms706.dbo.lr_base_department lbd ON lbu.F_DepartmentId = lbd.F_DepartmentId");
                strSql.Append(" WHERE p.ProjectStatus = 1 and lbd.HZ_DepartmentId != 1");

                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                string createTime = "";
                if (!queryParam["datetime"].IsEmpty())
                {
                    var create_time = queryParam["datetime"].ToObject<List<string>>();
                    if (create_time.Count > 0)
                    {
                        string create_time_start_date = create_time[0];
                        string create_time_end_date = create_time[1].ToDate().AddDays(1).ToString();
                        createTime = " AND ( t.CreateTime >= '" + create_time_start_date + "' AND t.CreateTime < '" + create_time_end_date + "' )";
                        strSql.Append(createTime);
                    }
                }
                strSql.Append(" group by lbu.F_DepartmentId,lbd.F_FullName ORDER BY TotalAmount desc ");
                var vo = this.BaseRepository("learunOAWFForm").FindList_NodbWhere<LeaderboardVo>(strSql.ToString());
                return vo;
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
            var db = this.BaseRepository().BeginTrans();
            try
            {
                var lR_OA_ProjectEntity = GetLR_OA_ProjectEntity(keyValue);
                db.Delete<LR_OA_ProjectEntity>(t => t.F_Id == keyValue);
                db.Delete<LR_OA_ProjectDetailEntity>(t => t.F_ParentId == lR_OA_ProjectEntity.F_Id);
                db.Commit();
            }
            catch (Exception ex)
            {
                db.Rollback();
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
        /// 删除明细数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public void DeleteDetail(string keyValue)
        {
            try
            {
                this.BaseRepository().Delete<LR_OA_ProjectDetailEntity>(t => t.F_Id == keyValue);
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
        public void SaveEntity(string keyValue, LR_OA_ProjectEntity entity, List<LR_OA_ProjectDetailEntity> lR_OA_ProjectDetailList)
        {
            var db = this.BaseRepository().BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    var lR_OA_ProjectEntityTmp = GetLR_OA_ProjectEntity(keyValue);
                    entity.Modify(keyValue);
                    db.Update(entity);
                    db.Delete<LR_OA_ProjectDetailEntity>(t => t.F_ParentId == lR_OA_ProjectEntityTmp.F_Id);
                    foreach (LR_OA_ProjectDetailEntity item in lR_OA_ProjectDetailList)
                    {
                        item.Create();
                        item.F_ParentId = lR_OA_ProjectEntityTmp.F_Id;
                        db.Insert(item);
                    }
                }
                else
                {
                    entity.Create();
                    db.Insert(entity);
                    foreach (LR_OA_ProjectDetailEntity item in lR_OA_ProjectDetailList)
                    {
                        item.Create();
                        item.F_ParentId = entity.F_Id;
                        db.Insert(item);
                    }
                }
                db.Commit();
            }
            catch (Exception ex)
            {
                db.Rollback();
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
        /// 保存表头实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public void SaveGant(string keyValue, LR_OA_ProjectEntity entity)
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
        /// <summary>
        /// 保存明细实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public void SaveDetail(string keyValue, LR_OA_ProjectDetailEntity entity)
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
