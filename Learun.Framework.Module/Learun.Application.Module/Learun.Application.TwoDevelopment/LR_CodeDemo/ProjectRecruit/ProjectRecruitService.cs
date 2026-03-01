using Dapper;
using Learun.Application.WorkFlow;
using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
    /// 创 建：超级管理员
    /// 日 期：2022-03-16 18:06
    /// 描 述：用工申请
    /// </summary>
    public class ProjectRecruitService : RepositoryFactory
    {
        #region 获取数据
        private NWFProcessIBLL nWFProcessIBLL = new NWFProcessBLL();
        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectRecruitVo> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                a.ProjectName,
                a.CustName,
                a.ProjectSource,
                a.FollowPerson,
                a.Id as pid,     
                t.id,
                t.ProjectId,
                t.WorkingTime,
                t.Remark,
                t.WorkFlowId,
                t.ApplyPerson,
                t.WageSource,
                t.JobType,
                t.PersonQty,
                t.Price,
                t.Amount,
                t.PayeeUnit,
                t.PayeeBank,
                t.PayeeAccount,
                t.PaymentMethod,
                t.RecruitStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                a.FDepartmentId,
                a.PDepartmentId,
                pc.DepartmentId,
                a.PreparedPerson,pc.ContractNo
                ");
                // strSql.Append("   from Project a left join ProjectRecruit t on a.Id=t.ProjectId left join ProjectContract pc on pc.ProjectId=a.Id");
                //strSql.Append("     from Project a left join ProjectRecruit t on a.Id=t.ProjectId left join (select ProjectId,ContractNo,DepartmentId  from ProjectContract where ContractType=1 and ContractStatus<>7 and ContractStatus<>6 and ContractStatus=4 GROUP BY ProjectId,ContractNo,DepartmentId) pc on pc.ProjectId=a.Id");
                strSql.Append("     from Project a inner join ProjectRecruit t on a.Id=t.ProjectId inner join (select * from ProjectContract  where ContractStatus<>6 and ContractType=1 and ContractStatus<>7 and MainContract=1) pc on pc.ProjectId=a.Id");
                strSql.Append("  where 1=1 ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                }
                string createTime = "";
                if (!queryParam["CreateTime"].IsEmpty())
                {
                    var create_time = queryParam["CreateTime"].ToObject<List<string>>();
                    if (create_time.Count > 0)
                    {
                        string create_time_start_date = create_time[0];
                        string create_time_end_date = create_time[1].ToDate().AddDays(1).ToString();
                        createTime = " AND ( t.CreateTime >= '" + create_time_start_date + "' AND t.CreateTime < '" + create_time_end_date + "' )";
                        strSql.Append(createTime);
                    }
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or  t.WageSource like'%{0}%' or  t.WorkingTime like'%{0}%' or  t.Price like'%{0}%' or  t.Amount like'%{0}%' or  t.PayeeBank like'%{0}%' or  t.PayeeAccount like'%{0}%' or  t.Remark like'%{0}%' )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId ='{0}' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pc.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like '%{0}%')", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["ApplyPerson"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ApplyPerson like '%{0}%' )", queryParam["ApplyPerson"].ToString()));
                }
                if (!queryParam["WageSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.WageSource like '%{0}%' )", queryParam["WageSource"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.Amount = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["PayeeUnit"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.PayeeUnit like '%{0}%' )", queryParam["PayeeUnit"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectRecruitVo>(strSql.ToString(), dp, pagination);
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
        /// 多部门获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectRecruitVo> GetPageListDepartmentId(Pagination pagination, string queryJson,string dep)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                a.ProjectName,
                a.CustName,
                a.ProjectSource,
                a.FollowPerson,
                a.Id as pid,     
                t.id,
                t.ProjectId,
                t.WorkingTime,
                t.Remark,
                t.WorkFlowId,
                t.ApplyPerson,
                t.WageSource,
                t.JobType,
                t.PersonQty,
                t.Price,
                t.Amount,
                t.PayeeUnit,
                t.PayeeBank,
                t.PayeeAccount,
                t.PaymentMethod,
                t.RecruitStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                a.FDepartmentId,
                a.PDepartmentId,
                pc.DepartmentId,
                a.PreparedPerson,pc.ContractNo
                ");
                // strSql.Append("   from Project a left join ProjectRecruit t on a.Id=t.ProjectId left join ProjectContract pc on pc.ProjectId=a.Id");
                //strSql.Append("     from Project a left join ProjectRecruit t on a.Id=t.ProjectId left join (select ProjectId,ContractNo,DepartmentId  from ProjectContract where ContractType=1 and ContractStatus<>7 and ContractStatus<>6 and ContractStatus=4 GROUP BY ProjectId,ContractNo,DepartmentId) pc on pc.ProjectId=a.Id");
                strSql.Append("    from Project a inner join ProjectRecruit t on a.Id=t.ProjectId inner join (select * from ProjectContract  where ContractStatus<>6 and ContractType=1 and ContractStatus<>7 and MainContract=1) pc on pc.ProjectId=a.Id");
                strSql.Append("  where a.ProjectStatus=1 ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                }
                string createTime = "";
                if (!queryParam["CreateTime"].IsEmpty())
                {
                    var create_time = queryParam["CreateTime"].ToObject<List<string>>();
                    if (create_time.Count > 0)
                    {
                        string create_time_start_date = create_time[0];
                        string create_time_end_date = create_time[1].ToDate().AddDays(1).ToString();
                        createTime = " AND ( t.CreateTime >= '" + create_time_start_date + "' AND t.CreateTime < '" + create_time_end_date + "' )";
                        strSql.Append(createTime);
                    }
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or  t.WageSource like'%{0}%' or  t.WorkingTime like'%{0}%' or  t.Price like'%{0}%' or  t.Amount like'%{0}%' or  t.PayeeBank like'%{0}%' or  t.PayeeAccount like'%{0}%' or  t.Remark like'%{0}%' )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId ='{0}' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pc.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like '%{0}%')", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["ApplyPerson"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ApplyPerson like '%{0}%' )", queryParam["ApplyPerson"].ToString()));
                }
                if (!queryParam["WageSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.WageSource like '%{0}%' )", queryParam["WageSource"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.Amount = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["PayeeUnit"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.PayeeUnit like '%{0}%' )", queryParam["PayeeUnit"].ToString()));
                }
                strSql.Append(string.Format(" AND ( " + dep + " )"));
                return this.BaseRepository("learunOAWFForm").FindList<ProjectRecruitVo>(strSql.ToString(), dp, pagination);
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

        ///<summary>
        ///根据id获取用工页面预览需要的数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public ProjectRecruitVo GetPreviewProjectRecruitById(string keyValue)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                a.ProjectName,
                a.CustName,
                a.ProjectSource,
                a.FollowPerson,
                a.Id as pid,     
                t.id,
t.WorkingTime,
                t.Remark,
                t.ProjectId,
                t.WorkFlowId,
t.WorkingTime,
                t.ApplyPerson,
                t.WageSource,
                t.JobType,
                t.PersonQty,
                t.Price,
                t.Amount,
                t.PayeeUnit,
                t.PayeeBank,
                t.PayeeAccount,
                t.PaymentMethod,
                t.RecruitStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                a.PreparedPerson
                ");
                strSql.Append("   from Project a left join ProjectRecruit t on a.Id=t.ProjectId ");
                strSql.Append("  where  t.id='" + keyValue + "'");


                return this.BaseRepository("learunOAWFForm").FindList<ProjectRecruitVo>(strSql.ToString()).FirstOrDefault();
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
        /// 用工流程表示数据
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public ProjectRecruitVo GetProjectRecruitByProcessId(string processId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                a.ProjectName,
                a.CustName,
                a.ProjectSource,
                a.FollowPerson,
                a.Id as pid,     
                t.id,
t.WorkingTime,
                t.Remark,
                t.ProjectId,
                t.WorkFlowId,
                t.ApplyPerson,
                t.WageSource,
                t.JobType,
                t.PersonQty,
                t.Price,
                t.Amount,
                t.PayeeUnit,
                t.PayeeBank,
                t.PayeeAccount,
                t.PaymentMethod,
                t.RecruitStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                a.PreparedPerson
                ");
                strSql.Append("   from Project a left join ProjectRecruit t on a.Id=t.ProjectId ");
                strSql.Append("  where  t.WorkFlowId='" + processId + "'");


                return this.BaseRepository("learunOAWFForm").FindList<ProjectRecruitVo>(strSql.ToString()).FirstOrDefault();
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
        /// 获取ProjectRecruit表实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public ProjectRecruitEntity GetProjectRecruitEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindEntity<ProjectRecruitEntity>(keyValue);
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
        /// 获取主表实体数据
        /// </summary>
        /// <param name="processId">流程实例ID</param>
        /// <returns></returns>
        public ProjectRecruitEntity GetEntityByProcessId(string processId)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindEntity<ProjectRecruitEntity>(t => t.WorkFlowId == processId);
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
        /// 根据流程id查询
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public ProjectRecruitVo GetRecruitByProcessId(string processId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                a.ProjectName,
                a.CustName,
                a.ProjectSource,
                a.FollowPerson,
                a.Id as pid,     
                t.id,
                t.ProjectId,
                t.WorkFlowId,
                t.ApplyPerson,
                t.WageSource,
                t.JobType,
                t.PersonQty,
                t.Price,
                t.Amount,
                t.PayeeUnit,
                t.PayeeBank,
                t.PayeeAccount,
                t.PaymentMethod,
                t.RecruitStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                a.PreparedPerson,t.Remark,t.WorkingTime
                ");
                strSql.Append("   from Project a left join ProjectRecruit t on a.Id=t.ProjectId ");
                strSql.Append("  where t.WorkFlowId='" + processId + "'");
                //strSql.Append("  where a.ProjectStatus=1 and t.WorkFlowId='" + processId + "'");


                return this.BaseRepository("learunOAWFForm").FindList<ProjectRecruitVo>(strSql.ToString()).FirstOrDefault();
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
        /// 提交审批流
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="processId"></param>
        internal void UpdateFlowId(string keyValue, string processId)
        {
            var db = this.BaseRepository("learunOAWFForm").BeginTrans();
            try
            {
                var projectContract = this.BaseRepository("learunOAWFForm").FindEntity<ProjectRecruitEntity>(keyValue);
                UserInfo userInfo = LoginUserInfo.Get();
                ProjectEntity projectEntity = this.BaseRepository("learunOAWFForm").FindEntity<ProjectEntity>(projectContract.ProjectId);
                if (!"".Equals(processId) && processId != null)
                {
                    string schemeCode = "ProjectRecruit";
                    int level = 1;
                    string title = projectEntity.ProjectName;
                    string auditors = "";
                    nWFProcessIBLL.CreateFlow(schemeCode, processId, title, level, auditors, userInfo);
                    // nWFProcessIBLL.AgainCreateFlow(processId, userInfo);
                }
                else
                {
                    processId = Guid.NewGuid().ToString();
                    string schemeCode = "ProjectRecruit";
                    int level = 1;
                    string title = projectEntity.ProjectName;
                    string auditors = "";
                    nWFProcessIBLL.CreateFlow(schemeCode, processId, title, level, auditors, userInfo);
                }
                projectContract.Modify(keyValue);
                projectContract.WorkFlowId = processId;
                projectContract.RecruitStatus = 2;
                this.BaseRepository("learunOAWFForm").Update(projectContract);
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
                this.BaseRepository("learunOAWFForm").Delete<ProjectRecruitEntity>(t => t.id == keyValue);
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
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public void SaveEntity(string keyValue, ProjectRecruitEntity entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    this.BaseRepository("learunOAWFForm").Update(entity);
                }
                else
                {
                    entity.DepartmentId = LoginUserInfo.Get().departmentId;
                    entity.Create();
                    this.BaseRepository("learunOAWFForm").Insert(entity);
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
