using Dapper;
using Learun.Application.Organization;
using Learun.Application.WorkFlow;
using Learun.DataBase.Repository;
using Learun.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 日 期：2022-03-11 00:18
    /// 描 述：项目任务单
    /// </summary>
    public class ProjectTaskService : RepositoryFactory
    {
        private UserIBLL userIBLL = new UserBLL();
        private DepartmentIBLL departmentIBLL = new DepartmentBLL();
        private CompanyIBLL companyIBLL = new CompanyBLL();
        #region 获取数据
        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" 
                a.ProjectName,
                t.id,
                t.ProjectId,
                t.ProjectResponsible,
                t.SiteContact,
                t.SitePhone,
                t.Inspector,
                t.ReportFile,
                t.ReportSubject,
                t.ApproachTime,
                t.PlanTime,
                t.PlanFinishTime,
                t.PlanApproachTime,
                t.TestContent,
                t.TestTarget,
                t.TaskStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.DepartmentId,
                t.WorkFlowId,
                a.PreparedPerson,   
                a.FollowPerson,
                a.CustName,
                a.FDepartmentId,
                a.PDepartmentId,
                t.TaskDepartmentId,
                t.ReportTemplateFile,
                t.FlowFinishedTime,
                t.ContractId,
                pc.ContractNo,
                pc.DepartmentId as ContractYXDeptId,
             CASE
                   WHEN PlanTime-1<GETDATE() THEN 111
                   WHEN PlanTime-3<GETDATE()  THEN 999
               ELSE DATEDIFF(DAY,GETDATE(),PlanTime) END as YJ,t.ProjectTaskNo,t.ActualDepartureTime,t.ActualApproachTime,t.SubDepartmentId, t.MainDepartmentId
                ");
                /*(case ISNULL(ActualApproachTime,0)
              when 0 then  DATEDIFF(day,GETDATE(),ApproachTime)
              else 999
              end) as YJ*/
                // strSql.Append("  FROM ProjectTask t inner join  Project a on a.Id=t.ProjectId  left join ProjectContract pc on a.id=pc.ProjectId and pc.ContractStatus<>6 and pc.ContractType=1 and pc.ContractStatus<>7 and pc.MainContract=1");
                strSql.Append("FROM ProjectTask t  " +
                    " left join (select ContractNo,id as ContractId, ProjectId,DepartmentId,ContractStatus from ProjectContract " +
                    " where  MainContract=1 and ContractType=1 ) pc " +
                    " on pc.ContractId= t.ContractId" +
                    " left join Project a on a.Id= pc.ProjectId ");
                strSql.Append("  WHERE 1=1 and pc.ContractStatus<>1 and pc.ContractStatus<>6  and ContractStatus<>7 and ContractStatus<>11 ");

                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });

                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate().ToString("yyyy-MM-dd"), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate().AddDays(1).ToString("yyyy-MM-dd"), DbType.DateTime);
                    strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime < @endTime ) ");
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
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or  a.CustName like'%{0}%' or  t.ApproachTime like'%{0}%' or  t.PlanTime like'%{0}%' or  t.Change like'%{0}%' )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["YCS"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (t.TaskStatus<>5 and GETDATE()>t.PlanTime )"));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like'%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pc.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    string projectKeySql = string.Format(" ( select id as ProjectId from Project where ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString());
                    strSql.Append(" and t.ProjectId in " + projectKeySql);
                    //strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like'%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContactName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ContactName like'%{0}%' )", queryParam["ContactName"].ToString()));
                }
                if (!queryParam["ProjectResponsible"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectResponsible like'%{0}%' )", queryParam["ProjectResponsible"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["PlanTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.PlanTime)=0 )", queryParam["PlanTime"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.TaskStatus = '{0}')", queryParam["TaskStatus"].ToString()));
                }
                if (!queryParam["IsOverdue"].IsEmpty())
                {
                    if (queryParam["IsOverdue"].ToString() == "1")
                    {
                        strSql.Append(string.Format(" AND t.TaskStatus<> 5 AND GETDATE() > t.PlanTime ").ToString());
                    }
                }
                if (!queryParam["IsToBeDetect"].IsEmpty())
                {
                    if (queryParam["IsToBeDetect"].ToString() == "1")
                    {
                        strSql.Append(string.Format(" AND (t.TaskStatus=1 or t.TaskStatus=2) ").ToString());
                    }
                }
                if (!queryParam["IsToBeReported"].IsEmpty())
                {
                    if (queryParam["IsToBeReported"].ToString() == "1")
                    {
                        strSql.Append(string.Format(" AND  (t.TaskStatus=8 or t.TaskStatus=9 or t.TaskStatus=11) ").ToString());
                    }
                }
                if (!queryParam["IsHaveCompleted"].IsEmpty())
                {
                    if (queryParam["IsHaveCompleted"].ToString() == "1")
                    {
                        strSql.Append(string.Format(" AND t.TaskStatus=5 ").ToString());
                    }
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString(), dp, pagination);

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
        public IEnumerable<ProjectTaskVo> getTaskForMatchPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" 
                a.ProjectName,
                t.id,
                t.ProjectId,
                t.ProjectResponsible,
                t.SiteContact,
                t.SitePhone,
                t.Inspector,
                t.ReportFile,
                t.ReportSubject,
                t.ApproachTime,
                t.PlanTime,
                t.PlanFinishTime,
                t.PlanApproachTime,
                t.TestContent,
                t.TestTarget,
                t.TaskStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.DepartmentId,
                t.WorkFlowId,
                a.PreparedPerson,   
                a.FollowPerson,
                a.CustName,
                a.FDepartmentId,
                a.PDepartmentId,
                t.TaskDepartmentId,
                t.ReportTemplateFile,
                t.FlowFinishedTime,
                t.ContractId,
                pc.ContractNo,
                pc.DepartmentId as ContractYXDeptId,
             CASE
                   WHEN PlanTime-1<GETDATE() THEN 111
                   WHEN PlanTime-3<GETDATE()  THEN 999
               ELSE DATEDIFF(DAY,GETDATE(),PlanTime) END as YJ,t.ProjectTaskNo,t.ActualDepartureTime,t.ActualApproachTime,t.SubDepartmentId, t.MainDepartmentId
                ");
                /*(case ISNULL(ActualApproachTime,0)
              when 0 then  DATEDIFF(day,GETDATE(),ApproachTime)
              else 999
              end) as YJ*/
                // strSql.Append("  FROM ProjectTask t inner join  Project a on a.Id=t.ProjectId  left join ProjectContract pc on a.id=pc.ProjectId and pc.ContractStatus<>6 and pc.ContractType=1 and pc.ContractStatus<>7 and pc.MainContract=1");
                strSql.Append("FROM ProjectTask t  " +
                    " left join (select ContractNo,id as ContractId, ProjectId,DepartmentId,ContractStatus from ProjectContract " +
                    " where  MainContract=1 and ContractType=1 ) pc " +
                    " on pc.ContractId= t.ContractId" +
                    " left join Project a on a.Id= pc.ProjectId ");
                strSql.Append("  WHERE 1=1  ");

                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });

                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate().ToString("yyyy-MM-dd"), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate().AddDays(1).ToString("yyyy-MM-dd"), DbType.DateTime);
                    strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime < @endTime ) ");
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
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or  a.CustName like'%{0}%' or  t.ApproachTime like'%{0}%' or  t.PlanTime like'%{0}%' or  t.Change like'%{0}%' )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["YCS"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (t.TaskStatus<>5 and GETDATE()>t.PlanTime )"));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like'%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pc.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    string projectKeySql = string.Format(" ( select id as ProjectId from Project where ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString());
                    strSql.Append(" and t.ProjectId in " + projectKeySql);
                    //strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like'%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContactName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ContactName like'%{0}%' )", queryParam["ContactName"].ToString()));
                }
                if (!queryParam["ProjectResponsible"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectResponsible like'%{0}%' )", queryParam["ProjectResponsible"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["PlanTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.PlanTime)=0 )", queryParam["PlanTime"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.TaskStatus = '{0}')", queryParam["TaskStatus"].ToString()));
                }
                if (!queryParam["IsOverdue"].IsEmpty())
                {
                    if (queryParam["IsOverdue"].ToString() == "1")
                    {
                        strSql.Append(string.Format(" AND t.TaskStatus<> 5 AND GETDATE() > t.PlanTime ").ToString());
                    }
                }
                if (!queryParam["IsToBeDetect"].IsEmpty())
                {
                    if (queryParam["IsToBeDetect"].ToString() == "1")
                    {
                        strSql.Append(string.Format(" AND (t.TaskStatus=1 or t.TaskStatus=2) ").ToString());
                    }
                }
                if (!queryParam["IsToBeReported"].IsEmpty())
                {
                    if (queryParam["IsToBeReported"].ToString() == "1")
                    {
                        strSql.Append(string.Format(" AND  (t.TaskStatus=8 or t.TaskStatus=9 or t.TaskStatus=11) ").ToString());
                    }
                }
                if (!queryParam["IsHaveCompleted"].IsEmpty())
                {
                    if (queryParam["IsHaveCompleted"].ToString() == "1")
                    {
                        strSql.Append(string.Format(" AND t.TaskStatus=5 ").ToString());
                    }
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString(), dp, pagination);

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
        public IEnumerable<ProjectTaskVo> GetTaskLoadList(string queryJson, string userId, string departmentId, int type)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" 
                a.ProjectName,
                t.id,
                t.ProjectId,
                t.ProjectResponsible,
                t.SiteContact,
                t.SitePhone,
                t.Inspector,
                t.ReportFile,
                t.ReportSubject,
                t.ApproachTime,
                t.PlanTime,
                t.PlanFinishTime,
                t.PlanApproachTime,
                t.TestContent,
                t.TestTarget,
                t.TaskStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.WorkFlowId,
                a.PreparedPerson,   
                a.FollowPerson,
                a.CustName,
                a.FDepartmentId,
                a.PDepartmentId,
               t.TaskDepartmentId,
               t.ReportTemplateFile,
                t.FlowFinishedTime,
             CASE
 WHEN PlanTime-1<GETDATE() THEN 111
WHEN PlanTime-3<GETDATE()  THEN 999
    
    ELSE DATEDIFF(DAY,GETDATE(),PlanTime) END as YJ,t.ProjectTaskNo,t.ActualDepartureTime,t.SubDepartmentId, t.MainDepartmentId,
	lbu.F_RealName as  ProjectResponsibleName,
	lbu.F_DepartmentId as DepartmentId,
	lbd.F_FullName as DepartmentName

                ");
                strSql.Append("FROM ProjectTask t " +
                    " INNER JOIN adms706.dbo.lr_base_user lbu on t.ProjectResponsible = lbu.F_UserId " +
                    " inner join adms706.dbo.lr_base_department lbd on lbu.F_DepartmentId = lbd.F_DepartmentId " +
                    " inner join (select ProjectId,id as ContractId from ProjectContract " +
                    " where ContractStatus<>1 and ContractStatus<>6 and ContractType=1 and ContractStatus<>7 and ContractStatus<>11 and MainContract=1 group by ProjectId) pc " +
                    " on pc.ContractId=T.ContractId " +
                    " inner join  Project a on a.Id=pc.ProjectId");
                strSql.Append("  WHERE lbd.HZ_DepartmentId != 1 ");
                if (type == 1)
                {
                    strSql.Append("  and  lbu.F_UserId = '" + userId + "'");
                }
                if (type == 2)
                {
                    strSql.Append("  and  lbd.F_DepartmentId = '" + departmentId + "'");
                }
                //var queryParam = queryJson.ToJObject();
                //// 虚拟参数
                //var dp = new DynamicParameters(new { });

                //if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                //{
                //    dp.Add("startTime", queryParam["StartTime"].ToDate().ToString("yyyy-MM-dd"), DbType.DateTime);
                //    dp.Add("endTime", queryParam["EndTime"].ToDate().AddDays(1).ToString("yyyy-MM-dd"), DbType.DateTime);
                //    strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime < @endTime ) ");
                //}
                //string createTime = "";
                //if (!queryParam["CreateTime"].IsEmpty())
                //{
                //    var create_time = queryParam["CreateTime"].ToObject<List<string>>();
                //    if (create_time.Count > 0)
                //    {
                //        string create_time_start_date = create_time[0];
                //        string create_time_end_date = create_time[1].ToDate().AddDays(1).ToString();
                //        createTime = " AND ( t.CreateTime >= '" + create_time_start_date + "' AND t.CreateTime < '" + create_time_end_date + "' )";
                //        strSql.Append(createTime);
                //    }
                //}
                return this.BaseRepository("learunOAWFForm").FindList_NodbWhere<ProjectTaskVo>(strSql.ToString());

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
        public IEnumerable<ProjectTaskVo> GetHZPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" 
                a.ProjectName,
                t.id,
                t.ProjectId,
                t.ProjectResponsible,
                t.SiteContact,
                t.SitePhone,
                t.Inspector,
                t.ReportFile,
                t.ReportSubject,
                t.ApproachTime,
                t.PlanTime,
                t.PlanFinishTime,
                t.PlanApproachTime,
                t.TestContent,
                t.TestTarget,
                t.TaskStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.DepartmentId,
                t.WorkFlowId,
                a.PreparedPerson,   
                a.FollowPerson,
                a.CustName,
                a.FDepartmentId,
                a.PDepartmentId,
               t.TaskDepartmentId,
               t.ReportTemplateFile,
                t.FlowFinishedTime,
                pc.ContractNo,
                t.ContractId,
             CASE
 WHEN PlanTime-1<GETDATE() THEN 111
WHEN PlanTime-3<GETDATE()  THEN 999
    
    ELSE DATEDIFF(DAY,GETDATE(),PlanTime) END as YJ,t.ProjectTaskNo,t.ActualDepartureTime,t.SubDepartmentId, t.MainDepartmentId

                ");
                /*(case ISNULL(ActualApproachTime,0)
              when 0 then  DATEDIFF(day,GETDATE(),ApproachTime)
              else 999
              end) as YJ*/
                // strSql.Append("  FROM ProjectTask t inner join  Project a on a.Id=t.ProjectId  left join ProjectContract pc on a.id=pc.ProjectId and pc.ContractStatus<>6 and pc.ContractType=1 and pc.ContractStatus<>7 and pc.MainContract=1");
                strSql.Append("FROM ProjectTask t  " +
                    " inner join adms706.dbo.lr_base_user lbu on t.CreateUser = lbu.F_UserId " +
                    " inner join adms706.dbo.lr_base_department lbd on lbu.F_DepartmentId = lbd.F_DepartmentId " +
                    " left join (select ContractNo,id as ContractId, ProjectId,DepartmentId,ContractStatus from ProjectContract " +
                    " where  MainContract=1 and ContractType=1 ) pc " +
                    " on pc.ContractId= t.ContractId " +
                    " left join Project a on a.Id= pc.ProjectId ");
                strSql.Append("  WHERE lbd.HZ_DepartmentId = 1 and pc.ContractStatus<>1 and pc.ContractStatus<>6  and ContractStatus<>7 and ContractStatus<>11");

                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });

                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate().ToString("yyyy-MM-dd"), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate().AddDays(1).ToString("yyyy-MM-dd"), DbType.DateTime);
                    strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime < @endTime ) ");
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
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or  a.CustName like'%{0}%' or  t.ApproachTime like'%{0}%' or  t.PlanTime like'%{0}%' or  t.Change like'%{0}%' )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["YCS"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (t.TaskStatus<>5 and GETDATE()>t.PlanTime )"));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like'%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pc.ContractNo ='{0}' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    string projectKeySql = string.Format(" ( select id as ProjectId from Project where ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString());
                    strSql.Append(" and t.ProjectId in " + projectKeySql);
                    //strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like'%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContactName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ContactName like'%{0}%' )", queryParam["ContactName"].ToString()));
                }
                if (!queryParam["ProjectResponsible"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectResponsible like'%{0}%' )", queryParam["ProjectResponsible"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["PlanTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.PlanTime)=0 )", queryParam["PlanTime"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.TaskStatus = '{0}')", queryParam["TaskStatus"].ToString()));
                }
                if (!queryParam["IsOverdue"].IsEmpty())
                {
                    if (queryParam["IsOverdue"].ToString() == "1")
                    {
                        strSql.Append(string.Format(" AND t.TaskStatus<> 5 AND GETDATE() > t.PlanTime ").ToString());
                    }
                }
                if (!queryParam["IsToBeDetect"].IsEmpty())
                {
                    if (queryParam["IsToBeDetect"].ToString() == "1")
                    {
                        strSql.Append(string.Format(" AND (t.TaskStatus=1 or t.TaskStatus=2) ").ToString());
                    }
                }
                if (!queryParam["IsToBeReported"].IsEmpty())
                {
                    if (queryParam["IsToBeReported"].ToString() == "1")
                    {
                        strSql.Append(string.Format(" AND  (t.TaskStatus=8 or t.TaskStatus=9 or t.TaskStatus=11) ").ToString());
                    }
                }
                if (!queryParam["IsHaveCompleted"].IsEmpty())
                {
                    if (queryParam["IsHaveCompleted"].ToString() == "1")
                    {
                        strSql.Append(string.Format(" AND t.TaskStatus=5 ").ToString());
                    }
                }
                return this.BaseRepository("learunOAWFForm").FindList_NodbWhere<ProjectTaskVo>(strSql.ToString() + " order by t.CreateTime desc");

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

        public IEnumerable<ProjectTaskEntity> GetAllFinishedTask()
        {
            try
            {
                string sql = "select * from ProjectTask where TaskStatus = 5";
                return this.BaseRepository("learunOAWFForm").FindList_NodbWhere<ProjectTaskEntity>(sql);
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
        public IEnumerable<ProjectTaskEntity> GetAllTaskToMatchContract()
        {
            try
            {
                string sql = "select * from ProjectTask";
                return this.BaseRepository("learunOAWFForm").FindList_NodbWhere<ProjectTaskEntity>(sql);
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

        public IEnumerable<ProducTionVo> GetQualityTechnologyImplement(string categoryId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(@" 
             select year(pt.CreateTime) Years,
month(pt.CreateTime) Month,
sum(t.ContractAmount) Amount,
count(*) Quantity
from ProjectContract t inner join ProjectTask pt on t.ProjectId=pt.ProjectId WHERE  pt.CreateTime>=dateadd(year, datediff(year, 0, getdate()), 0) and pt.CreateTime<=GETUTCDATE() ");
                if (categoryId == "1" && categoryId != null)
                {
                    strSql.Append(" and pt.TaskStatus=1");
                }
                if (categoryId == "2" && categoryId != null)
                {
                    strSql.Append(" and pt.TaskStatus=2");
                }
                if (categoryId == "3" && categoryId != null)
                {
                    strSql.Append("AND (pt.TaskStatus=1 and GETDATE()>pt.ApproachTime )");
                }

                strSql.Append("group by year(pt.CreateTime),month(pt.CreateTime)");

                // 虚拟参数
                var dp = new DynamicParameters(new { });
                return this.BaseRepository("learunOAWFForm").FindList<ProducTionVo>(strSql.ToString());
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
        /// 生产统计
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProducTionVo> GetProducTionList(string dep)
        {
            try
            {
                var strSql = new StringBuilder();

                strSql.Append(@" 
select year(t.CreateTime) Years,
month(t.CreateTime) Month,
sum(t.ContractAmount) Amount,
count(*) Quantity
from ProjectContract t inner join ProjectTask pt on t.ProjectId=pt.ProjectId WHERE t.CreateTime>=dateadd(year, datediff(year, 0, getdate()), 0) and t.CreateTime<=GETUTCDATE()
                ");
                if (dep != null)
                {
                    strSql.Append("and t.DepartmentId='" + dep + "'");
                }
                strSql.Append("group by year(t.CreateTime),month(t.CreateTime)");
                //var queryParam = queryJson.ToJObject();
                // 虚拟参数
                //var dp = new DynamicParameters(new { });

                /*if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                }
                */
                return this.BaseRepository("learunOAWFForm").FindList<ProducTionVo>(strSql.ToString());
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
        /// 生产统计报告超时/金额
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProducTionVo> GetProducTionTimeoutList(string dep)
        {
            try
            {
                var strSql = new StringBuilder();

                strSql.Append(@" 
select year(pt.CreateTime) Years,
month(pt.CreateTime) Month,
sum(t.ContractAmount) Amount,
count(*) Quantity
from ProjectContract t inner join ProjectTask pt on t.ProjectId=pt.ProjectId WHERE pt.CreateTime>=dateadd(year, datediff(year, 0, getdate()), 0) and pt.CreateTime<=GETUTCDATE() and pt.TaskStatus=1 and GETDATE()>pt.ApproachTime 
                ");
                if (dep != null)
                {
                    strSql.Append("and t.DepartmentId='" + dep + "' or  pt.SubDepartmentId='" + dep + "' and pt.MainDepartmentId='" + dep + "' ");
                }
                strSql.Append("group by year(pt.CreateTime),month(pt.CreateTime)");
                //var queryParam = queryJson.ToJObject();
                // 虚拟参数
                //var dp = new DynamicParameters(new { });

                /*if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                }
                */
                return this.BaseRepository("learunOAWFForm").FindList<ProducTionVo>(strSql.ToString());
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
        /// 多部门生产统计报告超时/金额
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProducTionVo> GetProducTionTimeoutListDepartmentId(string dep)
        {
            try
            {
                var strSql = new StringBuilder();

                strSql.Append(@" 
select year(pt.CreateTime) Years,
month(pt.CreateTime) Month,
sum(t.ContractAmount) Amount,
count(*) Quantity
from ProjectContract t inner join ProjectTask pt on t.ProjectId=pt.ProjectId WHERE pt.CreateTime>=dateadd(year, datediff(year, 0, getdate()), 0) and pt.CreateTime<=GETUTCDATE() and pt.TaskStatus=1 and GETDATE()>pt.ApproachTime 
                ");
                strSql.Append("and (" + dep + ")");
                strSql.Append("group by year(pt.CreateTime),month(pt.CreateTime)");
                //var queryParam = queryJson.ToJObject();
                // 虚拟参数
                //var dp = new DynamicParameters(new { });

                /*if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                }
                */
                return this.BaseRepository("learunOAWFForm").FindList<ProducTionVo>(strSql.ToString());
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
        /// 多部门生产统计
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProducTionVo> GetProducTionListDepartmentId(string dep)
        {
            try
            {
                var strSql = new StringBuilder();

                strSql.Append(@" 
select year(t.CreateTime) Years,
month(t.CreateTime) Month,
sum(t.ContractAmount) Amount,
count(*) Quantity
from ProjectContract t inner join ProjectTask pt on t.ProjectId=pt.ProjectId WHERE t.CreateTime>=dateadd(year, datediff(year, 0, getdate()), 0) and t.CreateTime<=GETUTCDATE()
                ");
                strSql.Append("and (" + dep + ")");
                strSql.Append("group by year(t.CreateTime),month(t.CreateTime)");
                //var queryParam = queryJson.ToJObject();
                // 虚拟参数
                //var dp = new DynamicParameters(new { });

                /*if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                }
                */
                return this.BaseRepository("learunOAWFForm").FindList<ProducTionVo>(strSql.ToString());
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
        /// 报告待检测
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetPageToBeDetectList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" 
                a.ProjectName,
                t.id,
                t.ProjectId,
                t.ProjectResponsible,
                t.SiteContact,
                t.SitePhone,
                t.Inspector,
                t.ReportFile,
                t.ReportSubject,
                t.ApproachTime,
                t.PlanTime,
                t.PlanFinishTime,
                t.PlanApproachTime,
                t.TestContent,
                t.TestTarget,
                t.TaskStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.DepartmentId,
                 t.WorkFlowId,
                a.PreparedPerson,   
                a.FollowPerson,
                a.CustName,
                a.FDepartmentId,
                a.PDepartmentId,
               t.TaskDepartmentId,
                pc.ContractNo,
             (case (case TaskStatus
              when 1 then 1000
              else ISNULL(ActualApproachTime,0)
              end)
              when 0 then  DATEDIFF(day,GETDATE(),ApproachTime)
              when 1000 then  DATEDIFF(day,GETDATE(),ApproachTime)
              else 999
              end) as YJ,t.ProjectTaskNo,t.ActualDepartureTime,t.SubDepartmentId, t.MainDepartmentId

                ");
                /*(case ISNULL(ActualApproachTime,0)
              when 0 then  DATEDIFF(day,GETDATE(),ApproachTime)
              else 999
              end) as YJ*/

                strSql.Append("  FROM ProjectTask t   " +
                    " inner join (select ContractNo,ProjectId,id as ContractId from ProjectContract  " +
                    " where ContractStatus<>1 and ContractStatus<>6 and ContractType=1 and ContractStatus<>7 and ContractStatus<>11 and MainContract=1 group by ContractNo, ProjectId) pc on pc.ContractId=t.ContractId" +
                    " inner join Project a on a.Id=pc.ProjectId ");
                strSql.Append("WHERE t.TaskStatus=1 or t.TaskStatus=2");

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
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or  a.CustName like'%{0}%' or  t.ApproachTime like'%{0}%' or  t.PlanTime like'%{0}%' or  t.Change like'%{0}%' )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["YCS"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (t.TaskStatus<>5 and GETDATE()>t.PlanTime )"));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like'%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pc.ContractNo ='{0}' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like'%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContactName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ContactName like'%{0}%' )", queryParam["ContactName"].ToString()));
                }
                if (!queryParam["ProjectResponsible"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectResponsible like'%{0}%' )", queryParam["ProjectResponsible"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["PlanTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.PlanTime)=0 )", queryParam["PlanTime"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.TaskStatus = '{0}')", queryParam["TaskStatus"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString(), dp, pagination);
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
        /// 待报告
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetPageToBeReportedList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" 
                a.ProjectName,
                t.id,
                t.ProjectId,
                t.ProjectResponsible,
                t.SiteContact,
                t.SitePhone,
                t.Inspector,
                t.ReportFile,
                t.ReportSubject,
                t.ApproachTime,
                t.PlanTime,
                t.PlanFinishTime,
                t.PlanApproachTime,
                t.TestContent,
                t.TestTarget,
                t.TaskStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.DepartmentId,
                 t.WorkFlowId,
                a.PreparedPerson,   
                a.FollowPerson,
                a.CustName,
                a.FDepartmentId,
                a.PDepartmentId,
               t.TaskDepartmentId,
                pc.ContractNo,
             (case (case TaskStatus
              when 1 then 1000
              else ISNULL(ActualApproachTime,0)
              end)
              when 0 then  DATEDIFF(day,GETDATE(),ApproachTime)
              when 1000 then  DATEDIFF(day,GETDATE(),ApproachTime)
              else 999
              end) as YJ,t.ProjectTaskNo,t.ActualDepartureTime,t.SubDepartmentId, t.MainDepartmentId

                ");
                /*(case ISNULL(ActualApproachTime,0)
              when 0 then  DATEDIFF(day,GETDATE(),ApproachTime)
              else 999
              end) as YJ*/
                strSql.Append("  FROM ProjectTask t  " +
                               " inner join (select ContractNo,id as ContractId, ProjectId from ProjectContract " +
                               " where ContractStatus<>1 and ContractStatus<>6 and ContractType=1 and ContractStatus<>7 and MainContract=1 and ContractStatus<>11 group by ContractNo,id, ProjectId) pc on pc.ContractId= t.ContractId" +
                               " inner join  Project a on a.Id= pc.ProjectId ");
                strSql.Append("WHERE t.TaskStatus=8 or t.TaskStatus=9 or t.TaskStatus=11");

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
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or  a.CustName like'%{0}%' or  t.ApproachTime like'%{0}%' or  t.PlanTime like'%{0}%' or  t.Change like'%{0}%' )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["YCS"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (t.TaskStatus<>5 and GETDATE()>t.PlanTime )"));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like'%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pc.ContractNo ='{0}' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like'%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContactName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ContactName like'%{0}%' )", queryParam["ContactName"].ToString()));
                }
                if (!queryParam["ProjectResponsible"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectResponsible like'%{0}%' )", queryParam["ProjectResponsible"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["PlanTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.PlanTime)=0 )", queryParam["PlanTime"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.TaskStatus = '{0}')", queryParam["TaskStatus"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString(), dp, pagination);
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
        /// 超期项目
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetPageOverdueItemList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" 
                a.ProjectName,
                t.id,
                t.ProjectId,
                t.ProjectResponsible,
                t.SiteContact,
                t.SitePhone,
                t.Inspector,
                t.ReportFile,
                t.ReportSubject,
                t.ApproachTime,
                t.PlanTime,
                t.PlanFinishTime,
                t.PlanApproachTime,
                t.TestContent,
                t.TestTarget,
                t.TaskStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.DepartmentId,
                 t.WorkFlowId,
                a.PreparedPerson,   
                a.FollowPerson,
                a.CustName,
                a.FDepartmentId,
                a.PDepartmentId,
               t.TaskDepartmentId,
                pc.ContractNo,
             (case (case TaskStatus
              when 1 then 1000
              else ISNULL(ActualApproachTime,0)
              end)
              when 0 then  DATEDIFF(day,GETDATE(),ApproachTime)
              when 1000 then  DATEDIFF(day,GETDATE(),ApproachTime)
              else 999
              end) as YJ,t.ProjectTaskNo,t.ActualDepartureTime,t.SubDepartmentId, t.MainDepartmentId

                ");
                /*(case ISNULL(ActualApproachTime,0)
              when 0 then  DATEDIFF(day,GETDATE(),ApproachTime)
              else 999
              end) as YJ*/
                strSql.Append("  FROM ProjectTask t " +
                               " inner join (select ContractNo,id as ContractId, ProjectId from ProjectContract " +
                               " where ContractStatus<>1 and ContractStatus<>6 and ContractType=1 and ContractStatus<>7 and MainContract=1 and ContractStatus<>11 group by ContractNo,id, ProjectId) pc on pc.ContractId= t.ContractId" +
                               " inner join  Project a on a.Id= pc.ProjectId ");
                // strSql.Append(" where (t.TaskStatus=1 and GETDATE()>t.ApproachTime ) ");
                strSql.Append(" where (t.TaskStatus<>5 and GETDATE()>t.PlanTime ) ");

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
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or  a.CustName like'%{0}%' or  t.ApproachTime like'%{0}%' or  t.PlanTime like'%{0}%' or  t.Change like'%{0}%' )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["YCS"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (t.TaskStatus<>5 and GETDATE()>t.PlanTime )"));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like'%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pc.ContractNo ='{0}' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like'%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContactName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ContactName like'%{0}%' )", queryParam["ContactName"].ToString()));
                }
                if (!queryParam["ProjectResponsible"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectResponsible like'%{0}%' )", queryParam["ProjectResponsible"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["PlanTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.PlanTime)=0 )", queryParam["PlanTime"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.TaskStatus = '{0}')", queryParam["TaskStatus"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString(), dp, pagination);
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
        /// 已完成
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetPageHaveCompletedList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" 
                a.ProjectName,
                t.id,
                t.ProjectId,
                t.ProjectResponsible,
                t.SiteContact,
                t.SitePhone,
                t.Inspector,
                t.ReportFile,
                t.ReportSubject,
                t.ApproachTime,
                t.PlanTime,
                t.PlanFinishTime,
                t.PlanApproachTime,
                t.TestContent,
                t.TestTarget,
                t.TaskStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.DepartmentId,
                 t.WorkFlowId,
                a.PreparedPerson,   
                a.FollowPerson,
                a.CustName,
                a.FDepartmentId,
                a.PDepartmentId,
                t.TaskDepartmentId,
                pc.ContractNo,
                (case (case TaskStatus
                when 1 then 1000
                else ISNULL(ActualApproachTime,0)
                end)
                when 0 then  DATEDIFF(day,GETDATE(),ApproachTime)
                when 1000 then  DATEDIFF(day,GETDATE(),ApproachTime)
                else 999
                end) as YJ,t.ProjectTaskNo,t.ActualDepartureTime,t.SubDepartmentId, t.MainDepartmentId

                ");
                /*(case ISNULL(ActualApproachTime,0)
              when 0 then  DATEDIFF(day,GETDATE(),ApproachTime)
              else 999
              end) as YJ*/
                strSql.Append("  FROM ProjectTask t " +
               " inner join (select ContractNo,id as ContractId, ProjectId from ProjectContract " +
               " where ContractStatus<>1 and ContractStatus<>6 and ContractType=1 and ContractStatus<>7 and MainContract=1 and ContractStatus<>11 group by ContractNo,id, ProjectId) pc on pc.ContractId= t.ContractId" +
               " inner join  Project a on a.Id= pc.ProjectId ");
                strSql.Append("WHERE t.TaskStatus=5");

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
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or  a.CustName like'%{0}%' or  t.ApproachTime like'%{0}%' or  t.PlanTime like'%{0}%' or  t.Change like'%{0}%' )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["YCS"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (t.TaskStatus<>5 and GETDATE()>t.PlanTime )"));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like'%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pc.ContractNo ='{0}' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like'%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContactName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ContactName like'%{0}%' )", queryParam["ContactName"].ToString()));
                }
                if (!queryParam["ProjectResponsible"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectResponsible like'%{0}%' )", queryParam["ProjectResponsible"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["PlanTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.PlanTime)=0 )", queryParam["PlanTime"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.TaskStatus = '{0}')", queryParam["TaskStatus"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString(), dp, pagination);
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
        /// 报告待检测_首页
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetIndexToBeDetectList(List<string> deptIds, out int count)
        {
            try
            {
                var strSql = new StringBuilder();
                var strSql_index = new StringBuilder();
                strSql_index.Append("SELECT TOP 5 ");
                strSql_index.Append(@" 
                a.ProjectName,
                t.id,
                t.ProjectId,
                t.ProjectResponsible,
                t.SiteContact,
                t.SitePhone,
                t.Inspector,
                t.ReportFile,
                t.ReportSubject,
                t.ApproachTime,
                t.PlanTime,
                t.PlanFinishTime,
                t.PlanApproachTime,
                t.TestContent,
                t.TestTarget,
                t.TaskStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.DepartmentId,
                 t.WorkFlowId,
                a.PreparedPerson,   
                a.FollowPerson,
                a.CustName,
                a.FDepartmentId,
                a.PDepartmentId,
               t.TaskDepartmentId,
                pc.ContractNo,
             (case (case TaskStatus
              when 1 then 1000
              else ISNULL(ActualApproachTime,0)
              end)
              when 0 then  DATEDIFF(day,GETDATE(),ApproachTime)
              when 1000 then  DATEDIFF(day,GETDATE(),ApproachTime)
              else 999
              end) as YJ,t.ProjectTaskNo,t.ActualDepartureTime,t.SubDepartmentId, t.MainDepartmentId

                ");
                var strSql_count = new StringBuilder();
                if (deptIds.Count > 1)
                {
                    strSql_count.Append(" SELECT count(t.id) AS YJ ");
                    strSql.Append("  FROM ProjectTask t " +
                        " inner join (select ContractNo,id as ContractId, ProjectId,DepartmentId from ProjectContract  " +
                        " where ContractStatus<>1 and ContractStatus<>6 and ContractType=1 and ContractStatus<>7 and MainContract=1 and ContractStatus<>11 group by ContractNo,id, ProjectId,DepartmentId) pc " +
                        " on pc.ContractId=t.ContractId " +
                        " inner join  Project a on a.Id=pc.ProjectId ");
                    strSql.Append(" WHERE (t.TaskStatus=1 or t.TaskStatus=2) ");
                }
                else if (deptIds.Count > 0)
                {
                    strSql_count.Append(" SELECT count(t.id) AS YJ ");
                    strSql.Append("  FROM (ProjectTask t   " +
                        " inner join (select ContractNo,id as ContractId, ProjectId,DepartmentId from ProjectContract  " +
                         "where ContractStatus<>1 and ContractStatus<>6 and ContractType=1 and ContractStatus<>7 and MainContract=1 and ContractStatus<>11 group by ContractNo,id, ProjectId,DepartmentId) pc " +
                        " on pc.ContractId=t.ContractId " +
                        " inner join  Project a on a.Id=pc.ProjectId ");
                    strSql.Append(" and (t.TaskStatus=1 or t.TaskStatus=2))");
                }
                else if (deptIds.Count == 0)
                {
                    strSql_count.Append(" SELECT count(t.id) AS YJ ");
                    strSql.Append(" FROM (ProjectTask t   " +
                        " inner join (select ContractNo,id as ContractId, ProjectId,DepartmentId from ProjectContract  " +
                        " where ContractStatus<>1 and ContractStatus<>6 and ContractType=1 and ContractStatus<>7 and MainContract=1 and ContractStatus<>11 group by ContractNo,id, ProjectId,DepartmentId) pc " +
                        " on pc.ContractId=t.ContractId " +
                        " inner join  Project a on a.Id=pc.ProjectId ");
                    strSql.Append(" and (t.TaskStatus=1 or t.TaskStatus=2)) ");
                }


                /* if (deptIds.Count > 0)
                 {
                     string chosen = "and t.DepartmentId in ( ";
                     for (int i = 0; i < deptIds.Count; i++)
                     {
                         if (i == 0)
                         {
                             chosen += " '" + deptIds[i] + "'";
                         }
                         else
                         {
                             chosen += " , '" + deptIds[i] + "' ";
                         }
                     }
                     chosen += " ) ";
                     strSql.Append(chosen);
                 }*/

                if (deptIds.Count > 1)
                {
                    string chosen = "";
                    for (var i = 0; i < deptIds.Count; i++)
                    {

                        if (i == 0)
                        {
                            chosen += " ( t.DepartmentId='" + deptIds[i] + "' or t.TaskDepartmentId like '%" + deptIds[i] + "%' or a.FDepartmentId='" + deptIds[i] + "' or a.PDepartmentId='" + deptIds[i] + "' or pc.DepartmentId='" + deptIds[i] + "' or t.SubDepartmentId='" + deptIds[i] + "' or t.MainDepartmentId='" + deptIds[i] + "') ";
                        }
                        else
                        {
                            chosen += " or ( t.DepartmentId='" + deptIds[i] + "' or t.TaskDepartmentId like '%" + deptIds[i] + "%' or a.FDepartmentId='" + deptIds[i] + "' or a.PDepartmentId='" + deptIds[i] + "' or pc.DepartmentId='" + deptIds[i] + "' or t.SubDepartmentId='" + deptIds[i] + "' or t.MainDepartmentId='" + deptIds[i] + "') ";
                        }

                    }

                    strSql.Append("and (" + chosen + ")");
                }
                else

                if (deptIds.Count > 0)
                {

                    string chosen = " Where ( t.DepartmentId = ";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += "   or  t.CreateUser = ";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += "   or  t.ProjectResponsible = ";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += "   or  t.Inspector like";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += "  or  t.TaskDepartmentId = ";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += "  or a.FDepartmentId = ";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += " ) ";
                    strSql.Append(chosen);
                }
                string count_sql = strSql_count.ToString() + strSql.ToString();
                string index_sql = strSql_index.ToString() + strSql.ToString() + " order by t.CreateTime desc ";
                count = 0;
                string count_result = this.BaseRepository("learunOAWFForm").FindObject(count_sql).ToString();
                count = int.Parse(count_result);
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(index_sql);
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
        /// 待报告_首页
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetIndexToBeReportedList(List<string> deptIds, out int count)
        {
            try
            {
                var strSql = new StringBuilder();
                var strSql_index = new StringBuilder();
                strSql_index.Append("SELECT TOP 5 ");
                strSql_index.Append(@" 
                a.ProjectName,
                t.id,
                t.ProjectId,
                t.ProjectResponsible,
                t.SiteContact,
                t.SitePhone,
                t.Inspector,
                t.ReportFile,
                t.ReportSubject,
                t.ApproachTime,
                t.PlanTime,
                t.PlanFinishTime,
                t.PlanApproachTime,
                t.TestContent,
                t.TestTarget,
                t.TaskStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.DepartmentId,
                 t.WorkFlowId,
                a.PreparedPerson,   
                a.FollowPerson,
                a.CustName,
                a.FDepartmentId,
                a.PDepartmentId,
               t.TaskDepartmentId,
                pc.ContractNo,
             (case (case TaskStatus
              when 1 then 1000
              else ISNULL(ActualApproachTime,0)
              end)
              when 0 then  DATEDIFF(day,GETDATE(),ApproachTime)
              when 1000 then  DATEDIFF(day,GETDATE(),ApproachTime)
              else 999
              end) as YJ,t.ProjectTaskNo,t.ActualDepartureTime,t.SubDepartmentId, t.MainDepartmentId

                ");

                var strSql_count = new StringBuilder();
                if (deptIds.Count > 1)
                {
                    strSql_count.Append(" SELECT count(t.id) AS YJ ");
                    strSql.Append("  FROM ProjectTask t   " +
                        "inner join (select ContractNo,id as ContractId, ProjectId,DepartmentId from ProjectContract  " +
                        "where ContractStatus<>1 and ContractStatus<>6 and ContractType=1 and ContractStatus<>7 and MainContract=1 and ContractStatus<>11 group by ContractNo,id, ProjectId,DepartmentId) pc " +
                        "on pc.ContractId=t.ContractId " +
                        "inner join  Project a on a.Id=pc.ProjectId");
                    strSql.Append(" WHERE (t.TaskStatus=8 or t.TaskStatus=9 or t.TaskStatus=11) ");
                }
                else if (deptIds.Count > 0)
                {
                    strSql_count.Append(" SELECT count(t.id) AS YJ ");
                    strSql.Append("  FROM (ProjectTask t  " +
                        "inner join (select ContractNo,id as ContractId, ProjectId,DepartmentId from ProjectContract  " +
                        "where ContractStatus<>1 and ContractStatus<>6 and ContractType=1 and ContractStatus<>7 and MainContract=1 and ContractStatus<>11 group by ContractNo,id, ProjectId,DepartmentId) pc " +
                        "on pc.ContractId=t.ContractId " +
                        "inner join  Project a on a.Id=pc.ProjectId");
                    strSql.Append(" and (t.TaskStatus=8 or t.TaskStatus=9 or t.TaskStatus=11)) ");
                }
                else if (deptIds.Count == 0)
                {
                    strSql_count.Append(" SELECT count(t.id) AS YJ ");
                    strSql.Append("  FROM (ProjectTask t  " +
                        "inner join (select ContractNo,id as ContractId, ProjectId,DepartmentId from ProjectContract  " +
                        "where ContractStatus<>1 and ContractStatus<>6 and ContractType=1 and ContractStatus<>7 and MainContract=1 and ContractStatus<>11 group by ContractNo,id, ProjectId,DepartmentId) pc " +
                        "on pc.ContractId=t.ContractId " +
                        "inner join  Project a on a.Id=pc.ProjectId");
                    strSql.Append(" and (t.TaskStatus=8 or t.TaskStatus=9 or t.TaskStatus=11)) ");
                }
                /* if (deptIds.Count > 0)
                 {
                     string chosen = " and t.DepartmentId in ( ";
                     for (int i = 0; i < deptIds.Count; i++)
                     {
                         if (i == 0)
                         {
                             chosen += " '" + deptIds[i] + "'";
                         }
                         else
                         {
                             chosen += " , '" + deptIds[i] + "' ";
                         }
                     }
                     chosen += " ) ";
                     strSql.Append(chosen);
                 }*/
                if (deptIds.Count > 1)
                {
                    string chosen = "";
                    for (var i = 0; i < deptIds.Count; i++)
                    {

                        if (i == 0)
                        {
                            chosen += " ( t.DepartmentId='" + deptIds[i] + "' or t.TaskDepartmentId like '%" + deptIds[i] + "%' or a.FDepartmentId='" + deptIds[i] + "' or a.PDepartmentId='" + deptIds[i] + "' or pc.DepartmentId='" + deptIds[i] + "' or t.SubDepartmentId='" + deptIds[i] + "' or t.MainDepartmentId='" + deptIds[i] + "') ";
                        }
                        else
                        {
                            chosen += " or ( t.DepartmentId='" + deptIds[i] + "' or t.TaskDepartmentId like '%" + deptIds[i] + "%' or a.FDepartmentId='" + deptIds[i] + "' or a.PDepartmentId='" + deptIds[i] + "' or pc.DepartmentId='" + deptIds[i] + "' or t.SubDepartmentId='" + deptIds[i] + "' or t.MainDepartmentId='" + deptIds[i] + "') ";
                        }

                    }

                    strSql.Append("and (" + chosen + ")");
                }

                else
                if (deptIds.Count > 0)
                {

                    string chosen = " Where ( t.DepartmentId = ";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += "   or  t.CreateUser = ";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += "   or  t.ProjectResponsible = ";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += "   or  t.Inspector like";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += "  or  t.TaskDepartmentId = ";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += "  or a.FDepartmentId = ";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += " ) ";
                    strSql.Append(chosen);
                }



                string count_sql = strSql_count.ToString() + strSql.ToString();
                string index_sql = strSql_index.ToString() + strSql.ToString() + " order by t.CreateTime desc ";
                count = 0;
                string count_result = this.BaseRepository("learunOAWFForm").FindObject(count_sql).ToString();
                count = int.Parse(count_result);
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(index_sql);
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
        /// 超期项目_首页
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetIndexOverdueList(List<string> deptIds, out int count)
        {
            try
            {
                var strSql = new StringBuilder();
                var strSql_index = new StringBuilder();
                strSql_index.Append("SELECT TOP 5 ");
                strSql_index.Append(@" 
                a.ProjectName,
                t.id,
                t.ProjectId,
                t.ProjectResponsible,
                t.SiteContact,
                t.SitePhone,
                t.Inspector,
                t.ReportFile,
                t.ReportSubject,
                t.ApproachTime,
                t.PlanTime,
                t.PlanFinishTime,
                t.PlanApproachTime,
                t.TestContent,
                t.TestTarget,
                t.TaskStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.DepartmentId,
                 t.WorkFlowId,
                a.PreparedPerson,   
                a.FollowPerson,
                a.CustName,
                a.FDepartmentId,
                a.PDepartmentId,
               t.TaskDepartmentId,
                pc.ContractNo,
             (case (case TaskStatus
              when 1 then 1000
              else ISNULL(ActualApproachTime,0)
              end)
              when 0 then  DATEDIFF(day,GETDATE(),ApproachTime)
              when 1000 then  DATEDIFF(day,GETDATE(),ApproachTime)
              else 999
              end) as YJ,t.ProjectTaskNo,t.ActualDepartureTime,t.SubDepartmentId, t.MainDepartmentId

                ");
                var strSql_count = new StringBuilder();

                if (deptIds.Count > 1)
                {
                    strSql_count.Append(" SELECT count(t.id) AS YJ ");
                    strSql.Append("  FROM ProjectTask t   " +
                        " inner join (select ContractNo,id as ContractId, ProjectId,DepartmentId from ProjectContract  " +
                        "where ContractStatus<>1 and ContractStatus<>6 and ContractType=1 and ContractStatus<>7 and MainContract=1 and ContractStatus<>11 group by ContractNo,id, ProjectId,DepartmentId) pc " +
                        "on pc.ContractId=t.ContractId " +
                        "inner join  Project a on a.Id=t.ProjectId");
                    strSql.Append(" WHERE t.TaskStatus<>5 and GETDATE()>t.PlanTime ");
                }
                else if (deptIds.Count > 0)
                {
                    strSql_count.Append(" SELECT count(t.id) AS YJ ");
                    strSql.Append("  FROM (ProjectTask t   " +
                        "inner join (select ContractNo,id as ContractId, ProjectId,DepartmentId from ProjectContract  " +
                        "where ContractStatus<>1 and ContractStatus<>6 and ContractType=1 and ContractStatus<>7 and MainContract=1 and ContractStatus<>11 group by ContractNo,id, ProjectId,DepartmentId) pc " +
                        "on pc.ContractId=t.ContractId " +
                        "inner join  Project a on a.Id=t.ProjectId");
                    strSql.Append("  and t.TaskStatus<>5 and GETDATE()>t.PlanTime ) ");
                }
                else if (deptIds.Count == 0)
                {
                    strSql_count.Append(" SELECT count(t.id) AS YJ ");
                    strSql.Append("  FROM (ProjectTask t " +
                        "inner join (select ContractNo,id as ContractId, ProjectId,DepartmentId from ProjectContract  " +
                        "where ContractStatus<>1 and ContractStatus<>6 and ContractType=1 and ContractStatus<>7 and MainContract=1 and ContractStatus<>11 group by ContractNo,id, ProjectId,DepartmentId) pc " +
                        "on pc.ContractId=t.ContractId " +
                        "inner join  Project a on a.Id=t.ProjectId");
                    strSql.Append("  and t.TaskStatus<>5 and GETDATE()>t.PlanTime ) ");
                }
                if (deptIds.Count > 1)
                {

                    string chosen = "";
                    for (var i = 0; i < deptIds.Count; i++)
                    {

                        if (i == 0)
                        {
                            chosen += " ( t.DepartmentId='" + deptIds[i] + "' or t.TaskDepartmentId like '%" + deptIds[i] + "%' or a.FDepartmentId='" + deptIds[i] + "' or a.PDepartmentId='" + deptIds[i] + "' or pc.DepartmentId='" + deptIds[i] + "' or t.SubDepartmentId='" + deptIds[i] + "' or t.MainDepartmentId='" + deptIds[i] + "') ";
                        }
                        else
                        {
                            chosen += " or ( t.DepartmentId='" + deptIds[i] + "' or t.TaskDepartmentId like '%" + deptIds[i] + "%' or a.FDepartmentId='" + deptIds[i] + "' or a.PDepartmentId='" + deptIds[i] + "' or pc.DepartmentId='" + deptIds[i] + "' or t.SubDepartmentId='" + deptIds[i] + "' or t.MainDepartmentId='" + deptIds[i] + "') ";
                        }

                    }

                    strSql.Append("and (" + chosen + ")");
                }
                else
                if (deptIds.Count > 0)
                {

                    string chosen = " Where ( t.DepartmentId = ";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += "   or  t.CreateUser = ";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += "   or  t.ProjectResponsible = ";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += "   or  t.Inspector like";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += "  or  t.TaskDepartmentId = ";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += "  or a.FDepartmentId = ";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += " ) ";
                    strSql.Append(chosen);
                }




                string count_sql = strSql_count.ToString() + strSql.ToString();
                string index_sql = strSql_index.ToString() + strSql.ToString() + " order by t.CreateTime desc ";
                count = 0;
                string count_result = this.BaseRepository("learunOAWFForm").FindObject(count_sql).ToString();
                count = int.Parse(count_result);
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(index_sql);
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
        /// 已完成_首页
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetIndexHaveCompletedList(List<string> deptIds, out int count)
        {
            try
            {
                var strSql = new StringBuilder();
                var strSql_index = new StringBuilder();
                strSql_index.Append("SELECT TOP 5 ");
                strSql_index.Append(@" 
                a.ProjectName,
                t.id,
                t.ProjectId,
                t.ProjectResponsible,
                t.SiteContact,
                t.SitePhone,
                t.Inspector,
                t.ReportFile,
                t.ReportSubject,
                t.ApproachTime,
                t.PlanTime,
                t.PlanFinishTime,
                t.PlanApproachTime,
                t.TestContent,
                t.TestTarget,
                t.TaskStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.DepartmentId,
                 t.WorkFlowId,
                a.PreparedPerson,   
                a.FollowPerson,
                a.CustName,
                a.FDepartmentId,
                a.PDepartmentId,
               t.TaskDepartmentId,
                pc.ContractNo,
             (case (case TaskStatus
              when 1 then 1000
              else ISNULL(ActualApproachTime,0)
              end)
              when 0 then  DATEDIFF(day,GETDATE(),ApproachTime)
              when 1000 then  DATEDIFF(day,GETDATE(),ApproachTime)
              else 999
              end) as YJ,t.ProjectTaskNo,t.ActualDepartureTime,t.SubDepartmentId, t.MainDepartmentId

                ");
                var strSql_count = new StringBuilder();

                if (deptIds.Count > 1)
                {
                    strSql_count.Append(" SELECT count(t.id) AS YJ ");
                    strSql.Append("  FROM ProjectTask t  " +
                        " inner join (select ContractNo,id as ContractId, ProjectId,DepartmentId from ProjectContract  " +
                        "where ContractStatus<>1 and ContractStatus<>6 and ContractType=1 and ContractStatus<>7 and MainContract=1 and ContractStatus<>11 group by ContractNo,id, ProjectId,DepartmentId ) pc " +
                        "on pc.ContractId=t.ContractId " +
                        "inner join  Project a on a.Id=pc.ProjectId");
                    strSql.Append(" WHERE (t.TaskStatus=5) ");
                }
                else if (deptIds.Count > 0)
                {
                    strSql_count.Append(" SELECT count(t.id) AS YJ ");
                    strSql.Append("  FROM (ProjectTask t " +
                        " inner join (select ContractNo,id as ContractId, ProjectId,DepartmentId from ProjectContract  " +
                        "where ContractStatus<>1 and ContractStatus<>6 and ContractType=1 and ContractStatus<>7 and MainContract=1 and ContractStatus<>11 group by ContractNo,id, ProjectId,DepartmentId ) pc " +
                        "on pc.ContractId=t.ContractId " +
                        "inner join  Project a on a.Id=pc.ProjectId");
                    strSql.Append(" and t.TaskStatus=5) ");
                }
                else if (deptIds.Count == 0)
                {
                    strSql_count.Append(" SELECT count(t.id) AS YJ ");
                    strSql.Append("  FROM (ProjectTask t " +
                        " inner join (select ContractNo,id as ContractId, ProjectId,DepartmentId from ProjectContract  " +
                        "where ContractStatus<>1 and ContractStatus<>6 and ContractType=1 and ContractStatus<>7 and MainContract=1 and ContractStatus<>11 group by ContractNo,id, ProjectId,DepartmentId ) pc " +
                        "on pc.ContractId=t.ContractId " +
                        "inner join  Project a on a.Id=pc.ProjectId");
                    strSql.Append(" and t.TaskStatus=5) ");
                }
                /*  if (deptIds.Count > 0)
                  {
                      string chosen = " and t.DepartmentId in ( ";
                      for (int i = 0; i < deptIds.Count; i++)
                      {
                          if (i == 0)
                          {
                              chosen += " '" + deptIds[i] + "'";
                          }
                          else
                          {
                              chosen += " , '" + deptIds[i] + "' ";
                          }
                      }
                      chosen += " ) ";
                      strSql.Append(chosen);
                  }*/
                if (deptIds.Count > 1)
                {

                    string chosen = "";
                    for (var i = 0; i < deptIds.Count; i++)
                    {

                        if (i == 0)
                        {
                            chosen += " ( t.DepartmentId='" + deptIds[i] + "' or t.TaskDepartmentId like '%" + deptIds[i] + "%' or a.FDepartmentId='" + deptIds[i] + "' or a.PDepartmentId='" + deptIds[i] + "' or pc.DepartmentId='" + deptIds[i] + "' or t.SubDepartmentId='" + deptIds[i] + "' or t.MainDepartmentId='" + deptIds[i] + "') ";
                        }
                        else
                        {
                            chosen += " or ( t.DepartmentId='" + deptIds[i] + "' or t.TaskDepartmentId like '%" + deptIds[i] + "%' or a.FDepartmentId='" + deptIds[i] + "' or a.PDepartmentId='" + deptIds[i] + "' or pc.DepartmentId='" + deptIds[i] + "' or t.SubDepartmentId='" + deptIds[i] + "' or t.MainDepartmentId='" + deptIds[i] + "') ";
                        }

                    }

                    strSql.Append("and (" + chosen + ")");
                }

                else
                if (deptIds.Count > 0)
                {

                    string chosen = " Where ( t.DepartmentId = ";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += "   or  t.CreateUser = ";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += "   or  t.ProjectResponsible = ";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += "   or  t.Inspector like";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += "  or  t.TaskDepartmentId = ";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += "  or a.FDepartmentId = ";
                    for (int i = 0; i < deptIds.Count; i++)
                    {
                        if (i == 0)
                        {
                            chosen += " '" + deptIds[i] + "'";
                        }
                        else
                        {
                            chosen += " , '" + deptIds[i] + "' ";
                        }
                    }
                    chosen += " ) ";
                    strSql.Append(chosen);
                }



                string count_sql = strSql_count.ToString() + strSql.ToString();
                string index_sql = strSql_index.ToString() + strSql.ToString() + " order by t.CreateTime desc ";
                count = 0;
                string count_result = this.BaseRepository("learunOAWFForm").FindObject(count_sql).ToString();
                count = int.Parse(count_result);
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(index_sql);
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
        public IEnumerable<ProjectTaskVo> GetPageListDepartmentId(Pagination pagination, string queryJson, string dep)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" 
                a.ProjectName,
                t.id,
                t.ProjectId,
                t.ProjectResponsible,
                t.SiteContact,
                t.SitePhone,
                t.Inspector,
                t.ReportFile,
                t.ReportSubject,
                t.ApproachTime,
                t.PlanTime,
                t.PlanFinishTime,
                t.PlanApproachTime,
                t.TestContent,
                t.TestTarget,
                t.TaskStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.DepartmentId,
                 t.WorkFlowId,
                a.PreparedPerson,   
                a.FollowPerson,
                a.CustName,
                a.FDepartmentId,
                a.PDepartmentId,
               t.TaskDepartmentId,
               t.FlowFinishedTime,
                pc.ContractNo,
                t.ContractId,
               CASE
 WHEN PlanTime-1<GETDATE() THEN 111
WHEN PlanTime-3<GETDATE()  THEN 999
    
    ELSE DATEDIFF(DAY,GETDATE(),PlanTime) END as YJ,t.ProjectTaskNo,t.ActualDepartureTime

                ");
                /*(case ISNULL(ActualApproachTime,0)
              when 0 then  DATEDIFF(day,GETDATE(),ApproachTime)
              else 999
              end) as YJ*/
                //strSql.Append("  FROM ProjectTask t inner join  Project a on a.Id=t.ProjectId  left join ProjectContract pc on a.id=pc.ProjectId and pc.ContractStatus<>6 and pc.ContractType=1 and pc.ContractStatus<>7 and pc.MainContract=1");
                strSql.Append("FROM ProjectTask t " +
                    " left join (select ContractNo,id as ContractId, ProjectId,DepartmentId,ContractStatus from ProjectContract " +
                    " where  MainContract=1 and ContractType=1 ) pc " +
                    " on pc.ContractId=t.ContractId" +
                    " left join  Project a on a.Id=pc.ProjectId  ");
                strSql.Append("  WHERE pc.ContractStatus<>1 and pc.ContractStatus<>6  and ContractStatus<>7 and ContractStatus<>11");

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
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or  a.CustName like'%{0}%' or  t.ApproachTime like'%{0}%' or  t.PlanTime like'%{0}%' or  t.Change like'%{0}%' )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["YCS"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (t.TaskStatus<>5 and GETDATE()>t.PlanTime )"));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like'%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pc.ContractNo ='{0}' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    string projectKeySql = string.Format(" ( select id as ProjectId from Project where ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString());
                    strSql.Append(" and t.ProjectId in " + projectKeySql);
                    //strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like'%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContactName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ContactName like'%{0}%' )", queryParam["ContactName"].ToString()));
                }
                if (!queryParam["ProjectResponsible"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectResponsible like'%{0}%' )", queryParam["ProjectResponsible"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["PlanTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.PlanTime)=0 )", queryParam["PlanTime"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.TaskStatus = '{0}')", queryParam["TaskStatus"].ToString()));
                }
                strSql.Append(string.Format(" AND ( " + dep + " )"));
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString(), dp, pagination);
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
        /// 报告导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetPageList(string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" 
                a.ProjectName,
                t.id,
                t.ProjectId,
                t.ProjectResponsible,
                t.SiteContact,
                t.SitePhone,
                t.Inspector,
                t.ReportFile,
                t.ReportSubject,
                t.ApproachTime,
                t.PlanTime,
                t.PlanFinishTime,
                t.PlanApproachTime,
                t.TestContent,
                t.TestTarget,
                t.TaskStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.DepartmentId,
                 t.WorkFlowId,
                a.PreparedPerson,   
                a.FollowPerson,
                a.CustName,
                a.FDepartmentId,
                a.PDepartmentId,
               t.TaskDepartmentId,
                pc.ContractNo,
                t.FlowFinishedTime,
                pc.DepartmentId as ContractYXDeptId,
             CASE
                 WHEN PlanTime-1<GETDATE() THEN 111
                 WHEN PlanTime-3<GETDATE()  THEN 999
    
    ELSE DATEDIFF(DAY,GETDATE(),PlanTime) END as YJ,t.ProjectTaskNo,t.ActualDepartureTime,t.ActualApproachTime,t.SubDepartmentId, t.MainDepartmentId

                ");
                /*(case ISNULL(ActualApproachTime,0)
              when 0 then  DATEDIFF(day,GETDATE(),ApproachTime)
              else 999
              end) as YJ*/
                // strSql.Append("  FROM ProjectTask t inner join  Project a on a.Id=t.ProjectId  left join ProjectContract pc on a.id=pc.ProjectId and pc.ContractStatus<>6 and pc.ContractType=1 and pc.ContractStatus<>7 and pc.MainContract=1");
                strSql.Append("FROM ProjectTask t  " +
                    " left join (select ContractNo,id as ContractId, ProjectId,DepartmentId,ContractStatus from ProjectContract " +
                    " where  MainContract=1 and ContractType=1 ) pc " +
                    " on pc.ContractId= t.ContractId" +
               " left join  Project a on a.Id= pc.ProjectId ");
                strSql.Append("  WHERE 1=1 and pc.ContractStatus<>1 and pc.ContractStatus<>6  and ContractStatus<>7 and ContractStatus<>11");

                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });

                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate().ToString("yyyy-MM-dd"), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate().AddDays(1).ToString("yyyy-MM-dd"), DbType.DateTime);
                    strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime < @endTime ) ");
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
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or  a.CustName like'%{0}%' or  t.ApproachTime like'%{0}%' or  t.PlanTime like'%{0}%' or  t.Change like'%{0}%' )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["YCS"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (t.TaskStatus<>5 and GETDATE()>t.PlanTime )"));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like'%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pc.ContractNo ='{0}' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    string projectKeySql = string.Format(" ( select id as ProjectId from Project where ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString());
                    strSql.Append(" and t.ProjectId in " + projectKeySql);
                    //strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like'%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContactName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ContactName like'%{0}%' )", queryParam["ContactName"].ToString()));
                }
                if (!queryParam["ProjectResponsible"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectResponsible like'%{0}%' )", queryParam["ProjectResponsible"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["PlanTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.PlanTime)=0 )", queryParam["PlanTime"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.TaskStatus = '{0}')", queryParam["TaskStatus"].ToString()));
                }
                if (!queryParam["IsOverdue"].IsEmpty())
                {
                    if (queryParam["IsOverdue"].ToString() == "1")
                    {
                        strSql.Append(string.Format(" AND t.TaskStatus<> 5 AND GETDATE() > t.PlanTime ").ToString());
                    }
                }
                if (!queryParam["IsToBeDetect"].IsEmpty())
                {
                    if (queryParam["IsToBeDetect"].ToString() == "1")
                    {
                        strSql.Append(string.Format(" AND (t.TaskStatus=1 or t.TaskStatus=2) ").ToString());
                    }
                }
                if (!queryParam["IsToBeReported"].IsEmpty())
                {
                    if (queryParam["IsToBeReported"].ToString() == "1")
                    {
                        strSql.Append(string.Format(" AND  (t.TaskStatus=8 or t.TaskStatus=9 or t.TaskStatus=11) ").ToString());
                    }
                }
                if (!queryParam["IsHaveCompleted"].IsEmpty())
                {
                    if (queryParam["IsHaveCompleted"].ToString() == "1")
                    {
                        strSql.Append(string.Format(" AND t.TaskStatus=5 ").ToString());
                    }
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString(), dp);
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
        /// 合作报告导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetHZPageList(string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" 
                a.ProjectName,
                t.id,
                t.ProjectId,
                t.ProjectResponsible,
                t.SiteContact,
                t.SitePhone,
                t.Inspector,
                t.ReportFile,
                t.ReportSubject,
                t.ApproachTime,
                t.PlanTime,
                t.PlanFinishTime,
                t.PlanApproachTime,
                t.TestContent,
                t.TestTarget,
                t.TaskStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.DepartmentId,
                 t.WorkFlowId,
                a.PreparedPerson,   
                a.FollowPerson,
                a.CustName,
                a.FDepartmentId,
                a.PDepartmentId,
               t.TaskDepartmentId,
                pc.ContractNo,
                t.CotractId,
                t.FlowFinishedTime,
             CASE
 WHEN PlanTime-1<GETDATE() THEN 111
WHEN PlanTime-3<GETDATE()  THEN 999
    
    ELSE DATEDIFF(DAY,GETDATE(),PlanTime) END as YJ,t.ProjectTaskNo,t.ActualDepartureTime,t.SubDepartmentId, t.MainDepartmentId

                ");
                /*(case ISNULL(ActualApproachTime,0)
              when 0 then  DATEDIFF(day,GETDATE(),ApproachTime)
              else 999
              end) as YJ*/
                // strSql.Append("  FROM ProjectTask t inner join  Project a on a.Id=t.ProjectId  left join ProjectContract pc on a.id=pc.ProjectId and pc.ContractStatus<>6 and pc.ContractType=1 and pc.ContractStatus<>7 and pc.MainContract=1");
                strSql.Append("FROM ProjectTask t " +
                    " inner join adms706.dbo.lr_base_user lbu on t.CreateUser = lbu.F_UserId " +
                    " inner join adms706.dbo.lr_base_department lbd on lbu.F_DepartmentId = lbd.F_DepartmentId " +
                    " left join (select ContractNo,id as ContractId, ProjectId,DepartmentId,ContractStatus from ProjectContract " +
                    " where  MainContract=1 and ContractType=1 ) pc " +
                    " on pc.ContractId= t.ContractId " +
                    " left join  Project a on a.Id= pc.ProjectId ");
                strSql.Append("  WHERE 1=1 and lbd.HZ_DepartmentId = 1 and pc.ContractStatus<>1 and pc.ContractStatus<>6  and ContractStatus<>7 and ContractStatus<>11 ");

                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });

                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate().ToString("yyyy-MM-dd"), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate().AddDays(1).ToString("yyyy-MM-dd"), DbType.DateTime);
                    strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime < @endTime ) ");
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
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or  a.CustName like'%{0}%' or  t.ApproachTime like'%{0}%' or  t.PlanTime like'%{0}%' or  t.Change like'%{0}%' )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["YCS"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (t.TaskStatus<>5 and GETDATE()>t.PlanTime )"));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like'%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pc.ContractNo ='{0}' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    string projectKeySql = string.Format(" ( select id as ProjectId from Project where ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString());
                    strSql.Append(" and t.ProjectId in " + projectKeySql);
                    //strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like'%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContactName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ContactName like'%{0}%' )", queryParam["ContactName"].ToString()));
                }
                if (!queryParam["ProjectResponsible"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectResponsible like'%{0}%' )", queryParam["ProjectResponsible"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["PlanTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.PlanTime)=0 )", queryParam["PlanTime"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.TaskStatus = '{0}')", queryParam["TaskStatus"].ToString()));
                }
                if (!queryParam["IsOverdue"].IsEmpty())
                {
                    if (queryParam["IsOverdue"].ToString() == "1")
                    {
                        strSql.Append(string.Format(" AND t.TaskStatus<> 5 AND GETDATE() > t.PlanTime ").ToString());
                    }
                }
                if (!queryParam["IsToBeDetect"].IsEmpty())
                {
                    if (queryParam["IsToBeDetect"].ToString() == "1")
                    {
                        strSql.Append(string.Format(" AND (t.TaskStatus=1 or t.TaskStatus=2) ").ToString());
                    }
                }
                if (!queryParam["IsToBeReported"].IsEmpty())
                {
                    if (queryParam["IsToBeReported"].ToString() == "1")
                    {
                        strSql.Append(string.Format(" AND  (t.TaskStatus=8 or t.TaskStatus=9 or t.TaskStatus=11) ").ToString());
                    }
                }
                if (!queryParam["IsHaveCompleted"].IsEmpty())
                {
                    if (queryParam["IsHaveCompleted"].ToString() == "1")
                    {
                        strSql.Append(string.Format(" AND t.TaskStatus=5 ").ToString());
                    }
                }
                return this.BaseRepository("learunOAWFForm").FindList_NodbWhere<ProjectTaskVo>(strSql.ToString());
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
        /// 多部门报告导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetPageListDepartmentId(string queryJson, string dep)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" 
                a.ProjectName,
                t.id,
                t.ProjectId,
                t.ProjectResponsible,
                t.SiteContact,
                t.SitePhone,
                t.Inspector,
                t.ReportFile,
                t.ReportSubject,
                t.ApproachTime,
                t.PlanTime,
                t.PlanFinishTime,
                t.PlanApproachTime,
                t.TestContent,
                t.TestTarget,
                t.TaskStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.DepartmentId,
                 t.WorkFlowId,
                a.PreparedPerson,   
                a.FollowPerson,
                a.CustName,
                a.FDepartmentId,
                a.PDepartmentId,
               t.TaskDepartmentId,
               t.FlowFinishedTime,
               pc.ContractNo,
               t.ContractId,
             CASE
 WHEN PlanTime-1<GETDATE() THEN 111
WHEN PlanTime-3<GETDATE()  THEN 999
    
    ELSE DATEDIFF(DAY,GETDATE(),PlanTime) END as YJ,t.ProjectTaskNo,t.ActualDepartureTime

                ");
                /*(case ISNULL(ActualApproachTime,0)
              when 0 then  DATEDIFF(day,GETDATE(),ApproachTime)
              else 999
              end) as YJ*/
                // strSql.Append("  FROM ProjectTask t inner join  Project a on a.Id=t.ProjectId  left join ProjectContract pc on a.id=pc.ProjectId and pc.ContractStatus<>6 and pc.ContractType=1 and pc.ContractStatus<>7 and pc.MainContract=1");
                strSql.Append("FROM ProjectTask t " +
                    " left join (select ContractNo,id as ContractId, ProjectId,DepartmentId,ContractStatus from ProjectContract " +
                    " where  MainContract=1 and ContractType=1 ) pc " +
                    " on pc.ContractId= t.ContractId" +
                    " left join Project a on a.Id= pc.ProjectId ");
                strSql.Append("  WHERE 1=1 and pc.ContractStatus<>1 and pc.ContractStatus<>6  and ContractStatus<>7 and ContractStatus<>11");

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
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or  a.CustName like'%{0}%' or  t.ApproachTime like'%{0}%' or  t.PlanTime like'%{0}%' or  t.Change like'%{0}%' )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["YCS"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (t.TaskStatus<>5 and GETDATE()>t.PlanTime )"));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like'%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pc.ContractNo ='{0}' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    string projectKeySql = string.Format(" ( select id as ProjectId from Project where ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString());
                    strSql.Append(" and t.ProjectId in " + projectKeySql);
                    //strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like'%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContactName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ContactName like'%{0}%' )", queryParam["ContactName"].ToString()));
                }
                if (!queryParam["ProjectResponsible"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectResponsible like'%{0}%' )", queryParam["ProjectResponsible"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["PlanTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.PlanTime)=0 )", queryParam["PlanTime"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.TaskStatus = '{0}')", queryParam["TaskStatus"].ToString()));
                }
                strSql.Append(string.Format(" AND ( " + dep + " )"));
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString(), dp);
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
        /// 根据报备id查询相关任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetProjectTaskList(string id)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("select *  FROM ProjectTask t where t.ProjectId='" + id + "'");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString());
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
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetPageListApi(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" 
                a.ProjectName,
                t.id,
                t.ProjectId,
                t.ProjectResponsible,
                t.SiteContact,
                t.SitePhone,
                t.Inspector,
                t.ReportFile,
                t.ReportSubject,
                t.ApproachTime,
                t.PlanTime,
                t.PlanFinishTime,
                t.PlanApproachTime,
                t.TestContent,
                t.TestTarget,
                t.TaskStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.DepartmentId,
                a.PreparedPerson,   
                a.FollowPerson,
                a.CustName,
                pc.ContractNo,t.ActualDepartureTime
                ");

                strSql.Append("  FROM ProjectTask t inner join  Project a on a.Id=t.ProjectId  left join ProjectContract pc on a.id=pc.ProjectId and pc.ContractStatus<>6 and pc.ContractType=1");
                strSql.Append("WHERE 1=1");

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
                    strSql.Append(string.Format(" AND ( t.ProjectId like'%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pc.ContractNo ='{0}' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like'%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContactName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ContactName like'%{0}%' )", queryParam["ContactName"].ToString()));
                }
                if (!queryParam["ProjectResponsible"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectResponsible like'%{0}%' )", queryParam["ProjectResponsible"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["PlanTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.PlanTime)=0 )", queryParam["PlanTime"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.TaskStatus = '{0}')", queryParam["TaskStatus"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString(), dp, pagination);
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
        /// 获取排班信息
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<TaskScheduleVo> GetScheduleList(ScheduleParam queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" 
                a.ProjectName,
                t.id,
                t.ProjectId,
                t.ProjectResponsible,
                t.Inspector,
                t.PlanTime,
                t.ActualApproachTime as ApproachTime,
                t.PlanFinishTime,
                t.PlanApproachTime,
                t.ActualDepartureTime,
                t.DepartmentId,
                t.WorkFlowId,
                t.TaskStatus,
             CASE
                    WHEN PlanTime-1<GETDATE() THEN 111
                WHEN PlanTime-3<GETDATE()  THEN 999
                    ELSE DATEDIFF(DAY,GETDATE(),PlanTime) END as YJ
                ");
                strSql.Append(" FROM ProjectTask t  " +
                    " left join adms706.dbo.lr_base_user lbu on t.CreateUser = lbu.F_UserId " +
                    " left join adms706.dbo.lr_base_department lbd on lbu.F_DepartmentId = lbd.F_DepartmentId " +
                    " inner join (select ContractNo,id as ContractId, ProjectId from ProjectContract " +
                    " where ContractStatus<>1 and ContractStatus<>6 and ContractType=1 and ContractStatus<>7 and MainContract=1 and ContractStatus<>11 group by ContractNo,id, ProjectId) pc on pc.ContractId= t.ContractId " +
                    " inner join  Project a on a.Id= pc.ProjectId ");
                strSql.Append("  WHERE t.DeleteFlag = 0 and t.TaskStatus != 5 and t.TaskStatus != 11 and ( lbd.HZ_DepartmentId is NULL or lbd.HZ_DepartmentId <> 1) ");

                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!string.IsNullOrEmpty(queryJson.startTime) && !string.IsNullOrEmpty(queryJson.endTime))
                {
                    string start_date = queryJson.startTime;
                    string end_date = queryJson.endTime.ToDate().AddDays(1).ToString();
                    string time = " AND (( t.ApproachTime >= '" + start_date + "' AND t.ApproachTime < '" + end_date + "' ) " +
                                  " or ( t.PlanFinishTime >= '" + start_date + "' AND t.PlanFinishTime < '" + end_date + "' ) " +
                                  //" or ( t.PlanApproachTime >= '" + start_date + "' AND t.PlanApproachTime < '" + end_date + "' ) " +
                                  " or ( t.ActualDepartureTime >= '" + start_date + "' AND t.ActualDepartureTime < '" + end_date + "' )) ";
                    strSql.Append(time);
                }
                if (!string.IsNullOrEmpty(queryJson.inspector))
                {
                    var inspetorList = JsonConvert.DeserializeObject<List<string>>(queryJson.inspector);
                    if (inspetorList.Count > 0)
                    {
                        string inspector = " and ( ";
                        for (int i = 0; i < inspetorList.Count; i++)
                        {
                            if (i == 0)
                            {
                                inspector = inspector + " t.Inspector like '%" + inspetorList[i] + "%' or t.ProjectResponsible like '%" + inspetorList[i] + "%'";
                            }
                            else
                            {
                                inspector = inspector + " or t.Inspector like '%" + inspetorList[i] + "%' or t.ProjectResponsible like '%" + inspetorList[i] + "%'";
                            }
                        }
                        inspector = inspector + " ) ";
                        strSql.Append(inspector);
                    }
                }
                return this.BaseRepository("learunOAWFForm").FindList_NodbWhere<TaskScheduleVo>(strSql.ToString());
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
        /// 计划时间和用户id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public List<ProjectTaskVo> GetProjectTaskByInspectorAndPlanTime(string userId, DateTime startTime, DateTime endTime)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(" SELECT pt.* from  ProjectTask pt inner join  Project a on a.Id=pt.ProjectId  WHERE  pt.Inspector like '%" + userId + "%'" + "and pt.PlanTime>='" + startTime.ToString() + "'and pt.PlanTime<= '" + endTime.ToString() + "'");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString()).ToList();
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
        public ProjectTaskVo GetProjectTaskByTime(string userId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(" SELECT pt.* from  ProjectTask pt  WHERE  pt.Inspector like '%" + userId + "%'");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString()).FirstOrDefault();
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
        /// 实际时间和用户id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public List<ProjectTaskVo> GetProjectTaskByInspectorAndActualTime(string userId, DateTime startTime, DateTime endTime)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(" SELECT pt.* from  ProjectTask pt inner join  Project a on a.Id=pt.ProjectId  WHERE  pt.Inspector like '%" + userId + "%'and pt.ActualApproachTime>='" + startTime.ToString() + "' and pt.ActualApproachTime<= '" + endTime.ToString() + "'and pt.TaskStatus=2");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString()).ToList();
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
        public List<ProjectTaskVo> GetProjectTask(string userId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(" SELECT pt.* from  ProjectTask pt  WHERE  pt.Inspector like '%" + userId + "'");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString()).ToList();
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
        public ProjectTaskVo GetFormProjectData(string keyValue)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(" SELECT a.projectName,a.CustName,pt.* FROM Project a left join ProjectTask pt on a.id=pt.projectid WHERE pt.id=@keyValue ");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString(), new { keyValue = keyValue }).FirstOrDefault();
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
        /// 获取预览数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public ProjectTaskVo GetPriewProjectTask(string keyValue)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" 
                a.ProjectName,
                t.id,
                t.ContractId,
                t.WorkFlowId,
                t.ProjectId,
                t.ProjectResponsible,
                t.SiteContact,
                t.SitePhone,
                t.Inspector,
                t.ReportFile,
                t.ReportSubject,
                t.ApproachTime,
                t.PlanTime,
                t.PlanFinishTime,
                t.PlanApproachTime,
                t.ArrangeTime,
                t.TestContent,
                t.TestTarget,
                t.TaskStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.DepartmentId,
                a.PreparedPerson,
                a.FollowPerson,
                a.CustName,
                pc.ContractNo,
                t.ReportTemplateFile,
                t.Remark,
                t.Approver,
                t.ApproverTime,t.ReportApprover,
                t.Rating,t.ProjectTaskNo,t.MainDepartmentId,t.MainAmount,t.SubDepartmentId,t.SubAmount,t.ReportFile
                ");
                strSql.Append("  FROM ProjectTask t left join ProjectContract pc on t.ContractId=pc.Id left join Project a on a.Id=pc.ProjectId");
                strSql.Append("  WHERE  t.id='" + keyValue + "'");

                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString()).FirstOrDefault();
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
        public ProjectTaskVo GetPriewProjectTaskprojectId(string projectId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" 
                a.ProjectName,
                t.id,
                t.ProjectId,
                t.ProjectResponsible,
                t.SiteContact,
                t.SitePhone,
                t.Inspector,
                t.ReportFile,
                t.ReportSubject,
                t.ApproachTime,
                t.PlanTime,
                t.PlanFinishTime,
                t.PlanApproachTime,
                t.TestContent,
                t.TestTarget,
                t.TaskStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.DepartmentId,
                a.PreparedPerson,
                a.FollowPerson,
                a.CustName,
                pc.ContractNo,
t.ReportTemplateFile,
t.Remark,
t.Approver,
t.ApproverTime,
t.Rating,
t.ReportApprover,
t.ReportSubject,t.ProjectTaskNo,t.MainDepartmentId,t.MainAmount,t.SubDepartmentId,t.SubAmount
                ");
                strSql.Append("  FROM ProjectTask t left join ProjectContract pc on a.ContractId=pc.Id inner join  Project a on a.Id=pc.ProjectId ");
                strSql.Append("  WHERE  t.WorkFlowId='" + projectId + "'");

                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString()).FirstOrDefault();
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
        public List<ProjectTaskEntity> GetProjectTaskByProjectId(string projectId)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskEntity>(t => t.ProjectId == projectId).AsList();
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
        /// 根据负责人id查名字
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ProjectTaskVo GetFollowPersonNameByUserId(string queryJson, string keyValue)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" u.F_RealName,u.F_UserId,t.* ,pt.* from  ProjectTask t inner join adms706.dbo.lr_base_user u on t.ProjectResponsible like '%'+u.F_UserId+'%'");
                strSql.Append("where t.id='" + keyValue + "' and  u.F_UserId='" + queryJson + "'");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString()).FirstOrDefault();
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
        /// 根据id获取检测员
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ProjectTaskVo GetProjectTaskById(string keyValue)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"t.Inspector FROM ProjectTask t ");
                strSql.Append("WHERE t.id='" + keyValue + "'");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString()).FirstOrDefault();
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
        /// 获取ProjectTask表实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public ProjectTaskEntity GetProjectTaskEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindEntity<ProjectTaskEntity>(keyValue);
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
        /// 获取最新的任务编号
        /// </summary>
        /// <param name="taskNo"></param>
        /// <returns></returns>
        public string GetNextProjectTaskNo(string taskNo)
        {
            try
            {
                //taskNo = "JC24" + taskNo;
                // 定义正则表达式，匹配所有中文字符
                string pattern = "[\u4e00-\u9fa5]";
                var strSql = new StringBuilder();
                strSql.Append("SELECT REPLACE(LEFT(LTRIM(RTRIM(REPLACE(ProjectTaskNo, '沪房检', ''))), 10),'" + taskNo + "','')  as ProjectTaskNo ");
                strSql.Append(@"FROM ProjectTask WHERE ProjectTaskNo !=  'JC24CP887001' and ProjectTaskNo != 'JC24CP884001' and ProjectTaskNo != 'JC24CP882001' 
                                and LEFT(LTRIM(RTRIM(ProjectTaskNo)), 10) LIKE '%" + taskNo + "%';");
                List<ProjectTaskEntity> projectTasks = this.BaseRepository("learunOAWFForm").FindList<ProjectTaskEntity>(strSql.ToString()).ToList();
                int newNo = 0;
                if (projectTasks.Count > 0)
                {
                    newNo = projectTasks.Select(s => ExtractNumber(s.ProjectTaskNo.Replace("-", ""))).Max();
                    int maxIndex = newNo;
                    maxIndex += 1;
                    if (maxIndex >= 999)
                    {
                        string newNoResult = taskNo + maxIndex.ToString().PadLeft(4, '0');
                        return newNoResult;
                    }
                    else
                    {
                        string newNoResult = taskNo + maxIndex.ToString().PadLeft(3, '0');
                        return newNoResult;
                    }
                }
                else
                {
                    return taskNo + "001";
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
        public string GetNextProjectTaskNo_ZBG(string taskNo)
        {
            try
            {
                string pattern = "[\u4e00-\u9fa5]";
                var strSql = new StringBuilder();
                var len = "LEN('" + taskNo + " ') + 4";
                strSql.Append("SELECT REPLACE(LEFT(LTRIM(RTRIM(REPLACE(ProjectTaskNo, '沪房检', ''))), " + len + "),'" + taskNo + "','')  as ProjectTaskNo ");
                strSql.Append(@"FROM ProjectTask WHERE ProjectTaskNo !=  'JC24CP887001' and ProjectTaskNo != 'JC24CP884001' and ProjectTaskNo != 'JC24CP882001' 
                                and ProjectTaskNo LIKE '" + taskNo + "%';");
                List<ProjectTaskEntity> projectTasks = this.BaseRepository("learunOAWFForm").FindList<ProjectTaskEntity>(strSql.ToString()).ToList();
                int newNo = 0;
                if (projectTasks.Count > 0)
                {
                    newNo = projectTasks.Select(s => ExtractNumber(s.ProjectTaskNo.Replace("-", ""))).Max();
                    int maxIndex = newNo;
                    maxIndex += 1;
                    if (maxIndex >= 999)
                    {
                        string newNoResult = taskNo + maxIndex.ToString().PadLeft(4, '0');
                        return newNoResult;
                    }
                    else
                    {
                        string newNoResult = taskNo + maxIndex.ToString().PadLeft(3, '0');
                        return newNoResult;
                    }
                }
                else
                {
                    return taskNo + "001";
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
        static int ExtractNumber(string input)
        {
            // 提取字符串中的数字
            var match = Regex.Match(input, @"\d+");
            return match.Success ? int.Parse(match.Value) : 0;
        }
        public ProjectTaskVo GetProjectName(string name)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" t.TaskStatus,t.id FROM ProjectTask t inner join  Project a on a.Id=t.ProjectId  ");
                strSql.Append("WHERE a.ProjectName='" + name + "'");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString()).FirstOrDefault();
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
        /// 根据id修改状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProjectTaskVo GetPageListName(string id)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(@"UPDATE ProjectTask set TaskStatus=5");
                strSql.Append("WHERE id='" + id + "'");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString()).FirstOrDefault();
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
        public ProjectTaskEntity GetEntityByProcessId(string processId)
        {
            try
            {

                return this.BaseRepository("learunOAWFForm").FindEntity<ProjectTaskEntity>(t => t.WorkFlowId == processId);
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
        ///根据流程ID获取数据
        /// </summary>
        /// <param name="processId">流程实例ID</param>
        /// <returns></returns>
        public ProjectTaskVo GetTaskByProcessId(string processId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" 
                a.ProjectName,
                t.WorkFlowId,
                t.id,
                t.ProjectId,
                t.ProjectResponsible,
                t.SiteContact,
                t.SitePhone,
                t.Inspector,
                t.ReportFile,
                t.ReportSubject,
                t.ApproachTime,
                t.PlanTime,
                t.PlanFinishTime,
                t.PlanApproachTime,
                t.TestContent,
                t.TestTarget,
                t.TaskStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                a.DepartmentId,
                a.PreparedPerson,
                a.FollowPerson,t.ProjectTaskNo,t.MainDepartmentId,t.MainAmount,t.SubDepartmentId,t.SubAmount,t.Remark,t.Rating,t.ProjectTaskNo
                ");
                strSql.Append("  FROM ProjectTask t inner join  Project a on a.Id=t.ProjectId  ");
                strSql.Append("  WHERE  t.WorkFlowId='" + processId + "'");

                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString()).FirstOrDefault();
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
        /// 报告上传
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param> 

        public void GetProjectTaskSc(string keyValue, ProjectTaskEntity entity)
        {
            try
            {

                entity.ModifySC(keyValue);
                var projectTaskEntity = this.BaseRepository("learunOAWFForm").FindEntity<ProjectTaskEntity>(keyValue);
                projectTaskEntity.ReportFile = entity.ReportFile;
                this.BaseRepository("learunOAWFForm").Update(entity);

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
        ///根据流程ID获取数据
        /// </summary>
        /// <param name="processId">流程实例ID</param>
        /// <returns></returns>
        public ProjectTaskVo GetProjectNameApi(string userId, DateTime dateTime)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" 
                   a.ProjectName,
                t.WorkFlowId,
                t.id,
                t.ProjectId,
                t.ProjectResponsible,
                t.SiteContact,
                t.SitePhone,
                t.Inspector,
                t.ReportFile,
                t.ReportSubject,
                t.ApproachTime,
                t.PlanTime,
                t.PlanFinishTime,
                t.PlanApproachTime,
                t.TestContent,
                t.TestTarget,
                t.TaskStatus,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                a.DepartmentId,
                a.PreparedPerson,
                a.FollowPerson 
                ");
                strSql.Append("  FROM ProjectTask t inner join  Project a on a.Id=t.ProjectId  ");
                strSql.Append("  WHERE t.Inspector like '%" + userId + "%' and t.PlanTime>='" + dateTime.ToString() + "' and t.PlanTime<= '" + dateTime.AddDays(1).ToString() + "'");

                return this.BaseRepository("learunOAWFForm").FindList<ProjectTaskVo>(strSql.ToString()).FirstOrDefault();
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
        private NWFProcessIBLL nWFProcessIBLL = new NWFProcessBLL();
        #region 提交数据

        /// <summary>
        /// 删除实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void DeleteEntity(string keyValue)
        {
            try
            {
                this.BaseRepository("learunOAWFForm").Delete<ProjectTaskEntity>(t => t.id == keyValue);
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
        /// 签到PC端
        /// </summary>
        /// <param name="keyValue"></param>
        public void UpdateFielded(string keyValue)
        {
            try
            {
                var projectTask = this.BaseRepository("learunOAWFForm").FindEntity<ProjectTaskEntity>(keyValue);

                if (projectTask.TaskStatus == 1)
                {
                    projectTask.ActualApproachTime = DateTime.Now;
                    projectTask.TaskStatus = 2;
                }
                else
                {
                    projectTask.ActualDepartureTime = DateTime.Now;
                    projectTask.TaskStatus = 8;
                }

                projectTask.Modify(keyValue);
                projectTask.ActualApproachTime = DateTime.Now;

                this.BaseRepository("learunOAWFForm").Update(projectTask);

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
        /// 签到移动端
        /// </summary>
        /// <param name="keyValue"></param>
        public void UpdateFieldedAPI(string keyValue, ProjectTaskEntity entity)
        {
            try
            {
                var projectTask = this.BaseRepository("learunOAWFForm").FindEntity<ProjectTaskEntity>(keyValue);
                if (projectTask.TaskStatus == 1)
                {
                    projectTask.ActualApproachTime = DateTime.Now;
                    projectTask.TaskStatus = 2;
                }
                else if (projectTask.TaskStatus == 4)
                {
                    projectTask.ActualDepartureTime = DateTime.Now;
                    projectTask.TaskStatus = 8;
                }
                projectTask.UpdateUser = entity.UpdateUser;
                projectTask.ModifyTest(keyValue);
                this.BaseRepository("learunOAWFForm").Update(projectTask);

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
        public void UpdateFlowId(string keyValue, string processId)
        {
            //var db = this.BaseRepository("learunOAWFForm").BeginTrans();
            try
            {
                var projectTask = this.BaseRepository("learunOAWFForm").FindEntity<ProjectTaskEntity>(keyValue);
                UserInfo userInfo = LoginUserInfo.Get();
                ProjectEntity projectEntity = this.BaseRepository("learunOAWFForm").FindEntity<ProjectEntity>(projectTask.ProjectId);
                projectTask.Modify(keyValue);
                projectTask.TaskStatus = 3;

                //设置报告提交人
                projectTask.ReportSubmitter = LoginUserInfo.Get().userId;
                //获取部门code
                var department = departmentIBLL.GetEntity(LoginUserInfo.Get().departmentId);
                if (department != null)
                {
                    projectTask.ReportSubmitterDeptCode = department.F_EnCode;
                }
                var company = companyIBLL.GetEntity(LoginUserInfo.Get().companyId);
                if (company != null)
                {
                    projectTask.ReportSubmitterCompanyCode = company.F_EnCode;
                }


                //原来在这边
                //this.BaseRepository("learunOAWFForm").Update(projectContract);
                bool createFlag = true;
                if (String.IsNullOrEmpty(processId))
                {
                    processId = Guid.NewGuid().ToString();
                    projectTask.WorkFlowId = processId;

                }
                else
                {
                    createFlag = false;
                }
                this.BaseRepository("learunOAWFForm").Update(projectTask);
                //db.Update(projectContract);
                //db.Commit();
                var user = LoginUserInfo.Get().userId;
                var followPerson = BaseRepository().FindEntity<UserEntity>(user);
                var dept = BaseRepository().FindEntity<DepartmentEntity>(followPerson.F_DepartmentId);
                string schemeCode = "";
                if (!createFlag)
                {
                    string title = projectEntity.ProjectName;
                    if (dept.HZ_DepartmentId == 1)
                    {
                        schemeCode = "ProjectTask1";
                    }
                    else
                    {
                        schemeCode = "ProjectTask";
                    }


                    int level = 1;
                    nWFProcessIBLL.CreateFlow(schemeCode, processId, title, level, null, userInfo);
                }
                else
                {
                    if (dept.HZ_DepartmentId == 1)
                    {
                        schemeCode = "ProjectTask1";
                    }
                    else
                    {
                        schemeCode = "ProjectTask";
                    }
                    int level = 1;
                    string title = projectEntity.ProjectName;
                    string auditors = "";
                    nWFProcessIBLL.CreateFlow(schemeCode, processId, title, level, auditors, userInfo);
                }
            }
            catch (Exception ex)
            {
                //db.Rollback();
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
        public void SaveEntity(string keyValue, ProjectTaskEntity entity)
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
        /// <summary>
        /// 添加子报告
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveFormTast(ProjectTaskEntity entity)
        {
            try
            {
                //entity.TaskStatus = 9;
                if (string.IsNullOrEmpty(entity.id))
                {
                    entity.CreateTast();
                    this.BaseRepository("learunOAWFForm").Insert(entity);
                }
                else
                {
                    entity.Modify(entity.id);
                    this.BaseRepository("learunOAWFForm").Update(entity);
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
        /// 移动端保存实体数据（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public void SaveEntityApi(string keyValue, ProjectTaskUserEntity entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    ProjectTaskEntity project = new ProjectTaskEntity();
                    /* if (entity.Inspector != null)
                     {
                         string[] strList = entity.Inspector.Split(',');

                         string inspectorName = "";
                         for (var i = 0; i < strList.Length; i++)
                         {

                             var inspectorInfo = userIBLL.GetInspectorName(strList[i]);
                             if (inspectorInfo != null)
                             {
                                 if (string.IsNullOrWhiteSpace(inspectorName))
                                 {
                                     inspectorName = inspectorInfo.F_RealName;
                                 }
                                 else
                                 {
                                     inspectorName = inspectorName + "," + inspectorInfo.F_RealName;
                                 }
                             }
                         }
                         project.Inspector = inspectorName;
                     }*/

                    project.ProjectId = entity.ProjectId;
                    project.UpdateUser = entity.userId;
                    project.SiteContact = entity.SiteContact;
                    project.SitePhone = entity.SitePhone;
                    project.ProjectResponsible = entity.ProjectResponsible;
                    project.ReportSubject = entity.ReportSubject;
                    project.ApproachTime = entity.ApproachTime;
                    project.PlanTime = entity.PlanTime;
                    project.TestTarget = entity.TestTarget;
                    project.TestContent = entity.TestContent;
                    project.TaskStatus = entity.TaskStatus;
                    project.Inspector = entity.Inspector;
                    project.Remark = entity.Remark;
                    project.ReportFile = entity.ReportFile;
                    project.TaskStatus = entity.TaskStatus;
                    project.CreateTime = entity.CreateTime;
                    project.CreateUser = project.CreateUser;
                    project.UpdateTime = entity.UpdateTime;
                    project.ReportApprover = entity.ReportApprover;
                    project.ReportSubmitter = entity.ReportSubmitter;
                    project.ReportSubmitterCompanyCode = entity.ReportSubmitterCompanyCode;
                    project.ReportSubmitterDeptCode = entity.ReportSubmitterDeptCode;
                    project.DepartmentId = entity.DepartmentId;
                    project.WorkFlowId = entity.WorkFlowId;

                    project.ModifyTest(keyValue);
                    this.BaseRepository("learunOAWFForm").Update(project);
                }
                else
                {
                    ProjectTaskEntity project = new ProjectTaskEntity();
                    project.UpdateUser = entity.userId;
                    project.ModifyTest(keyValue);
                    project.ProjectId = entity.ProjectId;
                    project.SiteContact = entity.SiteContact;
                    project.SitePhone = entity.SitePhone;
                    project.ProjectResponsible = entity.ProjectResponsible;
                    project.ReportSubject = entity.ReportSubject;
                    project.ApproachTime = entity.ApproachTime;
                    project.PlanTime = entity.PlanTime;
                    project.TestContent = entity.TestContent;
                    project.TaskStatus = entity.TaskStatus;
                    project.Inspector = entity.Inspector;
                    project.TestTarget = entity.TestTarget;
                    project.Remark = entity.Remark;
                    project.ReportFile = entity.ReportFile;
                    project.TaskStatus = entity.TaskStatus;
                    project.CreateTime = entity.CreateTime;
                    project.CreateUser = project.CreateUser;
                    project.UpdateTime = entity.UpdateTime;
                    project.ReportApprover = entity.ReportApprover;
                    project.ReportSubmitter = entity.ReportSubmitter;
                    project.ReportSubmitterCompanyCode = entity.ReportSubmitterCompanyCode;
                    project.ReportSubmitterDeptCode = entity.ReportSubmitterDeptCode;
                    project.DepartmentId = entity.DepartmentId;
                    project.WorkFlowId = entity.WorkFlowId;

                    project.CreateTask();
                    this.BaseRepository("learunOAWFForm").Insert(project);
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

        public void FormChange(string keyValue, ProjectTaskchangeEntity entity)
        {
            try
            {
                var projectTask = this.BaseRepository("learunOAWFForm").FindEntity<ProjectTaskEntity>(keyValue);
                entity.ProjectId = projectTask.id;
                entity.WorkFlowId = projectTask.WorkFlowId;
                entity.Create();

                var strSql = "insert into ProjectTaskchange (Id,ProjectId,ChangeRecord,UpdateUser,UpdateTime,WorkFlowId)values('" + entity.Id + "','" + entity.ProjectId + "','" + entity.ChangeRecord + "','" + entity.UpdateUser + "','" + entity.UpdateTime + "','" + entity.WorkFlowId + "')";

                this.BaseRepository("learunOAWFForm").ExecuteBySql(strSql.ToString());
                projectTask.WorkFlowId = "";
                projectTask.TaskStatus = 6;
                projectTask.ModifyUp(keyValue);
                this.BaseRepository("learunOAWFForm").Update(projectTask);
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
