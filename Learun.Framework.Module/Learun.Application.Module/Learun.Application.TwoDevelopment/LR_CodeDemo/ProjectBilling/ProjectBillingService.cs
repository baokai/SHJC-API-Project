using Dapper;
using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using Learun.Application.WorkFlow;
using Learun.Application.Organization;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 日 期：2022-03-11 00:59
    /// 描 述：项目开票
    /// </summary>
    public class ProjectBillingService : RepositoryFactory
    {
        #region 获取数据
        private NWFProcessIBLL nWFProcessIBLL = new NWFProcessBLL();
        private DepartmentIBLL departmentIBLL = new DepartmentBLL();
        private ProjectManageService projectManageService = new ProjectManageService();
        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectBillingVo> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                a.ProjectName,
                a.CustName,
                a.ProjectSource,
                a.Id as pid,     
                t.id,
                t.ProjectId,
                t.ContractId,
                pc.ContractNo,   
                t.WorkFlowId,
                t.BillingType,
                t.BillingAmount,
                t.BillingContent,
                t.BillingInformation,
                t.BillingUnit,
                 t.Remark,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.BillingStatus,
                a.PreparedPerson,
                t.B_Remark,
                a.FDepartmentId,
                a.PDepartmentId,
                a.DepartmentId,
                a.FollowPerson,
                task.FinishTime,
t.BillingTitle,t.TaxNo,t.BankName,t.BankAccount
                ");
                // strSql.Append("  from ProjectBilling t  inner join   Project a on a.Id=t.ProjectId and  t.BillingStatus<>6 and t.BillingStatus<>8 ");
                strSql.Append("  from ProjectBilling t" +
                       " left join(select ContractNo, id as ContractId, ProjectId from ProjectContract " +
                       " where ContractStatus<>1 and ContractStatus<>6 and ContractType=1 and ContractStatus<>7 and ContractStatus<>11 and MainContract=1 group by ContractNo,id, ProjectId) pc " +
                       " on pc.ContractId= t.ContractId " +
                    " left join  Project a on a.Id=t.ProjectId " +
                    " left join (select F_ProcessId, max(F_CreateDate) as FinishTime from adms706.dbo.lr_nwf_task where F_IsFinished = 1 group by F_ProcessId) task on task.F_ProcessId = t.WorkFlowId");
                strSql.Append("  WHERE t.BillingStatus<>6 and t.BillingStatus<>8 ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                }
                if (!queryParam["FinishTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.BillingStatus = 7 and task.FinishTime like '%{0}%' )", queryParam["FinishTime"].ToDate().ToString("yyyy-MM-dd")));
                }
                
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or a.CustName like'%{0}%' or a.ContactName like'%{0}%' or a.ContactPhone like'%{0}%' or a.Address like'%{0}%' or t.BillingAmount like'%{0}%' or t.Remark like'%{0}%' or t.BillingInformation like'%{0}%' or t.B_Remark like'%{0}%')", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    string projectKeySql = string.Format(" ( select id as ProjectId from Project where ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString());
                    strSql.Append(" and t.ProjectId in " + projectKeySql);
                    //strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString()));
                }

                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pc.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId ='%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["BillingTitle"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.BillingTitle like '%{0}%' )", queryParam["BillingTitle"].ToString()));
                }
                /*    if (!queryParam["ContractNo"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( pc.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                    }*/
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.id like '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["BillingAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.BillingAmount = '{0}' )", queryParam["BillingAmount"].ToString()));
                }
                if (!queryParam["BillingUnit"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.BillingUnit = '{0}' )", queryParam["BillingUnit"].ToString()));
                }
                if (!queryParam["BillingStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.BillingStatus = '{0}' )", queryParam["BillingStatus"].ToString()));
                }
                if (!queryParam["DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.DepartmentId = '{0}' )", queryParam["DepartmentId"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectBillingVo>(strSql.ToString(), dp, pagination);
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
        public IEnumerable<ProjectBillingVo> GetPageListDepartmentId(Pagination pagination, string queryJson, string dep)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                a.ProjectName,
                a.CustName,
                a.ProjectSource,
                a.Id as pid,     
                t.id,
                t.ProjectId,
                t.WorkFlowId,
                t.BillingType,
                t.BillingAmount,
                t.BillingContent,
                t.BillingInformation,
                t.BillingUnit,
                 t.Remark,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.BillingStatus,
                a.PreparedPerson,
                t.B_Remark,
                a.FDepartmentId,
                a.PDepartmentId,
                a.DepartmentId,
                a.FollowPerson
                ");
                strSql.Append("  from ProjectBilling t  inner join   Project a on a.Id=t.ProjectId ");
                strSql.Append("  WHERE  t.BillingStatus<>6 and t.BillingStatus<>8   ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or a.CustName like'%{0}%' or a.ContactName like'%{0}%' or a.ContactPhone like'%{0}%' or a.Address like'%{0}%' or t.BillingAmount like'%{0}%' or t.Remark like'%{0}%' or t.BillingInformation like'%{0}%' or t.B_Remark like'%{0}%')", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId ='%{0}%' )", queryParam["ProjectId"].ToString()));
                }

                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pc.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like '%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.id like '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["BillingAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.BillingAmount = '{0}' )", queryParam["BillingAmount"].ToString()));
                }
                if (!queryParam["BillingUnit"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.BillingUnit = '{0}' )", queryParam["BillingUnit"].ToString()));
                }
                if (!queryParam["BillingStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.BillingStatus = '{0}' )", queryParam["BillingStatus"].ToString()));
                }
                if (!queryParam["DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.DepartmentId = '{0}' )", queryParam["DepartmentId"].ToString()));
                }
                strSql.Append(string.Format(" AND ( " + dep + " )"));
                return this.BaseRepository("learunOAWFForm").FindList<ProjectBillingVo>(strSql.ToString(), dp, pagination);
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
        /// 根据报备id查询相关发票数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<ProjectBillingVo> GetBilling(string id)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("select *  from ProjectBilling t  where  t.ProjectId='" + id + "' ");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectBillingVo>(strSql.ToString());
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
        /// 导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectBillingVo> GetPageList(string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                a.ProjectName,
                a.CustName,
                a.ProjectSource,
                a.Id as pid,     
                t.id,
                t.ProjectId,
                t.ContractId,
                pc.ContractNo,     
                t.WorkFlowId,
                t.BillingType,
                t.BillingAmount,
                t.BillingContent,
                t.BillingInformation,
                t.BillingUnit,
                 t.Remark,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.BillingStatus,
                a.PreparedPerson,
                t.B_Remark,
                a.FDepartmentId,
                a.PDepartmentId,
                a.DepartmentId,
                a.FollowPerson,
t.ReportFile,
t.BillingTitle,t.TaxNo,t.BankName,t.BankAccount
                ");
                //strSql.Append("  from ProjectBilling t  inner join   Project a on a.Id=t.ProjectId  and t.BillingStatus<>6 and t.BillingStatus<>8 left join ProjectContract pc on pc.ProjectId=t.Id");
                //strSql.Append("  from ProjectBilling t  inner join   Project a on a.Id=t.ProjectId ");
                strSql.Append("   from ProjectBilling t " +
                              " left join (select ContractNo,id as ContractId, ProjectId from ProjectContract " +
                               " where ContractStatus<>1 and ContractStatus<>6 and ContractType=1 and ContractStatus<>7 and ContractStatus<>11 and MainContract=1 group by ContractNo,id, ProjectId) pc " +
                               " on pc.ContractId= t.ContractId " +
                            " left join  Project a on a.Id=t.ProjectId " +
                             " left join (select F_ProcessId, max(F_CreateDate) as FinishTime from adms706.dbo.lr_nwf_task where F_IsFinished = 1 group by F_ProcessId) task on task.F_ProcessId = t.WorkFlowId");
                strSql.Append(" WHERE  t.BillingStatus <> 6 and t.BillingStatus <> 8 ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                }
                if (!queryParam["FinishTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.BillingStatus = 7 and task.FinishTime like '%{0}%' )", queryParam["FinishTime"].ToDate().ToString("yyyy-MM-dd")));
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or a.CustName like'%{0}%' or a.ContactName like'%{0}%' or a.ContactPhone like'%{0}%' or a.Address like'%{0}%' or t.BillingAmount like'%{0}%' or t.Remark like'%{0}%' or t.BillingInformation like'%{0}%' or t.B_Remark like'%{0}%')", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    string projectKeySql = string.Format(" ( select id as ProjectId from Project where ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString());
                    strSql.Append(" and t.ProjectId in " + projectKeySql);
                    //strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pc.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId ='%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["BillingTitle"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.BillingTitle like '%{0}%' )", queryParam["BillingTitle"].ToString()));
                }
                
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.id like '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["BillingAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.BillingAmount = '{0}' )", queryParam["BillingAmount"].ToString()));
                }
                if (!queryParam["BillingUnit"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.BillingUnit = '{0}' )", queryParam["BillingUnit"].ToString()));
                }
                if (!queryParam["BillingStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.BillingStatus = '{0}' )", queryParam["BillingStatus"].ToString()));
                }
                if (!queryParam["DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.DepartmentId = '{0}' )", queryParam["DepartmentId"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectBillingVo>(strSql.ToString(), dp);
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
        /// 多部门导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectBillingVo> GetPageListDepartmentId(string queryJson, string dep)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                a.ProjectName,
                a.CustName,
                a.ProjectSource,
                a.Id as pid,     
                t.id,
                t.ProjectId,
                t.WorkFlowId,
                t.BillingType,
                t.BillingAmount,
                t.BillingContent,
                t.BillingInformation,
                t.BillingUnit,
                 t.Remark,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.BillingStatus,
                a.PreparedPerson,
                t.B_Remark,
                a.FDepartmentId,
                a.PDepartmentId,
                a.DepartmentId,
                a.FollowPerson,
t.ReportFile
                ");
                //strSql.Append("  from ProjectBilling t  inner join   Project a on a.Id=t.ProjectId left join ProjectContract pc on pc.ProjectId=t.Id");
                strSql.Append("  from ProjectBilling t  inner join   Project a on a.Id=t.ProjectId ");
                strSql.Append(" WHERE  t.BillingStatus<>6 and t.BillingStatus<>8     ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%'  )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId ='%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like '%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pc.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.id like '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["BillingAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.BillingAmount = '{0}' )", queryParam["BillingAmount"].ToString()));
                }
                if (!queryParam["BillingUnit"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.BillingUnit = '{0}' )", queryParam["BillingUnit"].ToString()));
                }
                if (!queryParam["BillingStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.BillingStatus = '{0}' )", queryParam["BillingStatus"].ToString()));
                }
                strSql.Append(string.Format(" AND ( " + dep + " )"));
                return this.BaseRepository("learunOAWFForm").FindList<ProjectBillingVo>(strSql.ToString(), dp);
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
        /// 获取预览信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ProjectBillingVo GetPriewFormBilling(string keyValue)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                a.ProjectName,
                a.CustName,
                a.ProjectSource,
                a.Id as pid,     
                t.id,
                t.ProjectId,
                pc.ContractNo,
                t.ContractId,
                t.WorkFlowId,
                t.BillingType,
                t.BillingAmount,
                t.BillingContent,
                t.BillingInformation,
                t.BillingUnit,
                 t.Remark,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.BillingStatus,
                a.PreparedPerson,
                a.FollowPerson,
                t.ReportFile,
                t.Approver,
                t.ApproverTime,
                t.B_Remark,
                t.BillingTitle,
                t.TaxNo,
                t.BankName,
                t.BankAccount
                ");
                strSql.Append("  from ProjectBilling t " +
                    " left join Project a on a.Id=t.ProjectId " +
                    " left join ProjectContract pc on pc.Id = t.ContractId ");
                strSql.Append("  WHERE  t.id='" + keyValue + "'");

                return this.BaseRepository("learunOAWFForm").FindList<ProjectBillingVo>(strSql.ToString()).FirstOrDefault();
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
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<ProjectBillingEntity> GetProjectBillingByProjectId(string projectId)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindList<ProjectBillingEntity>(t => t.ProjectId == projectId).AsList();
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
        /// 获取ProjectBilling表实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public ProjectBillingEntity GetProjectBillingEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindEntity<ProjectBillingEntity>(keyValue);
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
        public ProjectBillingEntity GetEntityByProcessId(string processId)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindEntity<ProjectBillingEntity>(t => t.WorkFlowId == processId);
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
        public ProjectBillingVo GetBillingByProcessId(string processId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                a.ProjectName,
                a.CustName,
                a.ProjectSource,
                a.Id as pid,     
                t.id,
                t.ProjectId,
                t.WorkFlowId,
                t.BillingType,
                t.BillingAmount,
                t.BillingContent,
                t.BillingInformation,
                t.BillingUnit,
                 t.Remark,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.BillingStatus,
                a.PreparedPerson,
                a.FollowPerson,
                t.B_Remark,
                t.ReportFile,
                t.BillingTitle,
                t.TaxNo,
                t.BankName,
                t.BankAccount
                ");
                strSql.Append("  from ProjectBilling t  inner join   Project a on a.Id=t.ProjectId ");
                strSql.Append("  WHERE t.WorkFlowId='" + processId + "'");

                return this.BaseRepository("learunOAWFForm").FindList<ProjectBillingVo>(strSql.ToString()).FirstOrDefault();
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
            // var db = this.BaseRepository("learunOAWFForm").BeginTrans();
            try
            {
                var projectContract = this.BaseRepository("learunOAWFForm").FindEntity<ProjectBillingEntity>(keyValue);
                ProjectContractEntity contract = new ProjectContractEntity();
                var projectInfo = projectManageService.GetProjectEntity(projectContract.ProjectId);

                UserInfo userInfo = LoginUserInfo.Get();
                ProjectEntity projectEntity = this.BaseRepository("learunOAWFForm").FindEntity<ProjectEntity>(projectContract.ProjectId);
                projectContract.Modify(keyValue);

                //设置报告提交人
                projectContract.ContractSubmitter = LoginUserInfo.Get().userId;

                //获取部门code
                var department = departmentIBLL.GetEntity(LoginUserInfo.Get().departmentId);
                if (department != null)
                {
                    projectContract.ContractSubmitterDeptCode = department.F_EnCode;
                }
                if (!"".Equals(processId) && processId != null)
                {
                    projectContract.WorkFlowId = processId;

                }
                else
                {
                    projectContract.WorkFlowId = processId = Guid.NewGuid().ToString();
                }
                projectContract.WorkFlowId = processId;
                projectContract.BillingStatus = 2;
                this.BaseRepository("learunOAWFForm").Update(projectContract);
                var user = LoginUserInfo.Get().userId;
                var followPerson = BaseRepository().FindEntity<UserEntity>(user);
                string schemeCode = "";
                if (!"".Equals(processId) && processId != null)
                {
                    string title = projectEntity.ProjectName;
                    //项目来源是否为合作伙伴
                    if (projectInfo.ProjectSource == "3")
                    {
                        schemeCode = "ProjectBilling1";
                    }
                    else
                    {
                        schemeCode = "ProjectBilling";
                        if (followPerson.F_HZ == 1)
                        {
                            schemeCode = "ProjectBilling1";
                        }
                        //else
                        //{
                        //    schemeCode = "ProjectBilling";
                        //}
                    }
                    int level = 1;
                    nWFProcessIBLL.CreateFlow(schemeCode, projectContract.WorkFlowId, title, level, null, userInfo);
                    //nWFProcessIBLL.AgainCreateFlow(processId, userInfo);
                }
                else
                {
                    //项目来源是否为合作伙伴
                    if (projectInfo.ProjectSource == "3")
                    {
                        schemeCode = "ProjectBilling1";
                    }
                    else
                    {
                        schemeCode = "ProjectBilling";
                    }
                    //if (followPerson.F_HZ == 1)
                    //{
                    //    schemeCode = "ProjectBilling1";
                    //}
                    //else
                    //{
                    //    schemeCode = "ProjectBilling";
                    //}
                    int level = 1;
                    string title = projectEntity.ProjectName;
                    string auditors = "";
                    nWFProcessIBLL.CreateFlow(schemeCode, projectContract.WorkFlowId, title, level, auditors, userInfo);
                }

                //db.Commit();
            }
            catch (Exception ex)
            {
                // db.Rollback();
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
        /// 发票变更
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="processId"></param>
        public void UpdateContractStatus(string keyValue, string processId)
        {
            try
            {
                var projectContract = this.BaseRepository("learunOAWFForm").FindEntity<ProjectBillingEntity>(keyValue);

                projectContract.Modify(keyValue);
                projectContract.WorkFlowId = processId;
                projectContract.BillingStatus = 6;
                this.BaseRepository("learunOAWFForm").Update(projectContract);
                projectContract.BillingStatus = 1;
                projectContract.WorkFlowId = "";
                projectContract.Create();
                this.BaseRepository("learunOAWFForm").Insert(projectContract);

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
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void DeleteEntity(string keyValue)
        {
            try
            {
                this.BaseRepository("learunOAWFForm").Delete<ProjectBillingEntity>(t => t.Id == keyValue);
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
        public void SaveEntity(string keyValue, ProjectBillingEntity entity)
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

        public IEnumerable<ProjectBillingEntity> GetAllBillingToMatchContract()
        {
            try
            {
                string sql = "select * from ProjectBilling ";
                return this.BaseRepository("learunOAWFForm").FindList_NodbWhere<ProjectBillingEntity>(sql);
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
    }
}
