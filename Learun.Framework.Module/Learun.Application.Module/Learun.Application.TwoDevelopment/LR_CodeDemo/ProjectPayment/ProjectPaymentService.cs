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
    /// 日 期：2022-03-11 00:04
    /// 描 述：合同支付
    /// </summary>
    public class ProjectPaymentService : RepositoryFactory
    {
        private NWFProcessIBLL nWFProcessIBLL = new NWFProcessBLL();
        private DepartmentIBLL departmentIBLL = new DepartmentBLL();
        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectPaymentVo> GetPageList(Pagination pagination, string queryJson)
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
                t.PayType,
                t.Payee,
                t.PayeeBank,
                t.BankAccount,
                t.PaymentAmount,
                t.PaymentMethod,
                t.PaymentHeader,
                t.PaymentReason,
                t.PaymentFile,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.PaymentStatus ,
                a.PreparedPerson,
                a.FDepartmentId,
                a.PDepartmentId,
                b.F_DepartmentId as DepartmentId,
                a.FollowPerson 
                ");
                // strSql.Append("  from  ProjectPayment t inner join  Project a   on a.Id=t.ProjectId  left join ProjectContract pc on pc.ProjectId=a.Id and pc.ContractStatus<>7 and pc.ContractStatus<>6");
                // strSql.Append("  from  ProjectPayment t inner join  Project a   on a.Id=t.ProjectId  "); --20231012 brian
                strSql.Append("  from  ProjectPayment t " +
                    " Inner join adms706.dbo.lr_base_user b on t.CreateUser = b.F_UserId " +
                    " left join (select ContractNo,id as ContractId, ProjectId from ProjectContract " +
                       " group by ContractNo,id, ProjectId) pc " +
                       " on pc.ContractId= t.ContractId " +
                    " left join  Project a on a.Id=t.ProjectId " +
                //    "left join (select ProjectId,DepartmentId from ProjectContract group by ProjectId,DepartmentId) pc on t.ProjectId = pc.ProjectId "); --20240127 brian
                    "  WHERE (a.ProjectStatus=1 or a.ProjectStatus=3) and t.type=0");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND  t.CreateTime >= @startTime  ");
                }
                if (!queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("endTime", queryParam["EndTime"].ToDate().AddDays(1).ToString());
                    strSql.Append(" AND  t.CreateTime <= @endTime  ");
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or  a.CustName like'%{0}%' or  t.Payee like'%{0}%' or  t.PayeeBank like'%{0}%' or  t.PaymentAmount like'%{0}%' or  t.PaymentReason like'%{0}%')", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId ='{0}' )", queryParam["ProjectId"].ToString()));
                }
                /*    if (!queryParam["ContractNo"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( pc.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                    }*/
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
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource ='{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["Payee"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.Payee like '%{0}%' )", queryParam["Payee"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.PaymentAmount ='{0}' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["PaymentStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.PaymentStatus ='{0}' )", queryParam["PaymentStatus"].ToString()));
                }
                if (!queryParam["PayType"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.PayType ='{0}' )", queryParam["PayType"].ToString()));
                }

                return this.BaseRepository("learunOAWFForm").FindList<ProjectPaymentVo>(strSql.ToString(), dp, pagination);
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
        public IEnumerable<ProjectPaymentVo> GetPageListDepartmentId(Pagination pagination, string queryJson, string dep)
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
                t.PayType,
                t.Payee,
                t.PayeeBank,
                t.BankAccount,
                t.PaymentAmount,
                t.PaymentMethod,
                 t.PaymentHeader,
                 t.PaymentReason,
                 t.PaymentFile,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.PaymentStatus ,
                a.PreparedPerson,
                a.FDepartmentId,
                a.PDepartmentId,
                a.DepartmentId,
                a.FollowPerson 
                ");
                //strSql.Append("  from  ProjectPayment t inner join  Project a   on a.Id=t.ProjectId  inner join adms706.dbo.lr_base_user u on u.F_UserId=t.CreateUser left join ProjectContract pc on pc.ProjectId=t.id and pc.ContractStatus<>7 and pc.ContractStatus<>6");
                strSql.Append("  from  ProjectPayment t inner join  Project a   on a.Id=t.ProjectId  ");
                strSql.Append("  WHERE (a.ProjectStatus=1 or a.ProjectStatus=3) and t.type=0");
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
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or  a.CustName like'%{0}%' or  t.Payee like'%{0}%' or  t.PayeeBank like'%{0}%' or  t.PaymentAmount like'%{0}%' or  t.PaymentReason like'%{0}%')", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId ='{0}' )", queryParam["ProjectId"].ToString()));
                }
                /*  if (!queryParam["ContractNo"].IsEmpty())
                  {
                      strSql.Append(string.Format(" AND ( pc.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                  }*/
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like '%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource ='{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["Payee"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.Payee like '%{0}%' )", queryParam["Payee"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.PaymentAmount ='{0}' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["PaymentStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.PaymentStatus ='{0}' )", queryParam["PaymentStatus"].ToString()));
                }
                if (!queryParam["PayType"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.PayType ='{0}' )", queryParam["PayType"].ToString()));
                }
                strSql.Append(string.Format(" AND ( " + dep + " )"));
                return this.BaseRepository("learunOAWFForm").FindList<ProjectPaymentVo>(strSql.ToString(), dp, pagination);
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
        /// 根据报备id查询相关付款
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<ProjectPaymentVo> GetPayment(string id)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("select *  from  ProjectPayment t where t.ProjectId='" + id + "' and t.PaymentStatus=4");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectPaymentVo>(strSql.ToString());
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
        /// 付款导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectPaymentVo> GetPageList(string queryJson)
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
                t.PayType,
                t.Payee,
                t.PayeeBank,
                t.BankAccount,
                t.PaymentAmount,
                t.PaymentMethod,
                t.PaymentHeader,
                t.PaymentReason,
                t.PaymentFile,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.PaymentStatus ,
                a.PreparedPerson,
                a.FDepartmentId,
                a.PDepartmentId,
                b.F_DepartmentId as DepartmentId,
                a.FollowPerson 
                ");
                //strSql.Append("  from  ProjectPayment t inner join  Project a   on a.Id=t.ProjectId  left join ProjectContract pc on pc.ProjectId=a.Id and pc.ContractStatus<>7 and pc.ContractStatus<>6");
                strSql.Append(" from  ProjectPayment t " +
                    " Inner join adms706.dbo.lr_base_user b on t.CreateUser = b.F_UserId " +
                    " left join (select ContractNo,id as ContractId, ProjectId from ProjectContract " +
                       " where ContractStatus<>1 and ContractStatus<>6 and ContractType=1 and ContractStatus<>7 and ContractStatus<>11 and MainContract=1 group by ContractNo,id, ProjectId) pc " +
                       " on pc.ContractId= t.ContractId " +
                    " left join  Project a on a.Id=t.ProjectId ");
                strSql.Append("  WHERE (a.ProjectStatus=1 or a.ProjectStatus=3) and t.type=0");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND  t.CreateTime >= @startTime  ");
                }
                if (!queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("endTime", queryParam["EndTime"].ToDate().AddDays(1).ToString("yyyy-MM-dd"));
                    strSql.Append(" AND  t.CreateTime <= @endTime  ");
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or  a.CustName like'%{0}%' or  t.Payee like'%{0}%' or  t.PayeeBank like'%{0}%' or  t.PaymentAmount like'%{0}%' or  t.PaymentReason like'%{0}%')", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId ='{0}' )", queryParam["ProjectId"].ToString()));
                }
                /*      if (!queryParam["ContractNo"].IsEmpty())
                      {
                          strSql.Append(string.Format(" AND ( pc.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                      }*/
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
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource ='{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["Payee"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.Payee like '%{0}%' )", queryParam["Payee"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.PaymentAmount ='{0}' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["PaymentStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.PaymentStatus ='{0}' )", queryParam["PaymentStatus"].ToString()));
                }
                if (!queryParam["PayType"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.PayType ='{0}' )", queryParam["PayType"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectPaymentVo>(strSql.ToString(), dp);
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
        /// 多部门付款导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectPaymentVo> GetPageListDepartmentId(string queryJson, string dep)
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
                t.PayType,
                t.Payee,
                t.PayeeBank,
                t.BankAccount,
                t.PaymentAmount,
                t.PaymentMethod,
                 t.PaymentHeader,
                 t.PaymentReason,
                 t.PaymentFile,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.PaymentStatus ,
                a.PreparedPerson,
                a.FDepartmentId,
                a.PDepartmentId,
                a.DepartmentId,
                a.FollowPerson 
                ");
                //strSql.Append("  from  ProjectPayment t inner join  Project a   on a.Id=t.ProjectId inner join adms706.dbo.lr_base_user u on u.F_UserId=t.CreateUser left join ProjectContract pc on pc.ProjectId=t.id and pc.ContractStatus<>7 and pc.ContractStatus<>6");
                strSql.Append("  from  ProjectPayment t inner join  Project a   on a.Id=t.ProjectId ");
                strSql.Append("  WHERE (a.ProjectStatus=1 or a.ProjectStatus=3) and t.type=0");
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
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or  a.CustName like'%{0}%' or  t.Payee like'%{0}%' or  t.PayeeBank like'%{0}%' or  t.PaymentAmount like'%{0}%' or  t.PaymentReason like'%{0}%')", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId ='{0}' )", queryParam["ProjectId"].ToString()));
                }
                /*     if (!queryParam["ContractNo"].IsEmpty())
                     {
                         strSql.Append(string.Format(" AND ( pc.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                     }*/
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like '%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource ='{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["Payee"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.Payee like '%{0}%' )", queryParam["Payee"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.PaymentAmount ='{0}' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["PaymentStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.PaymentStatus ='{0}' )", queryParam["PaymentStatus"].ToString()));
                }
                if (!queryParam["PayType"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.PayType ='{0}' )", queryParam["PayType"].ToString()));
                }
                strSql.Append(string.Format(" AND ( " + dep + " )"));
                return this.BaseRepository("learunOAWFForm").FindList<ProjectPaymentVo>(strSql.ToString(), dp);
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
        public ProjectPaymentVo GetPreviewProjectPayment(string keyValue)
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
                t.tid,
                t.ProjectId,
                pc.ContractNo,
                t.WorkFlowId,
                t.PayType,
                t.Payee,
                t.PayeeBank,
                t.BankAccount,
                t.PaymentAmount,
                t.PaymentMethod,
                 t.PaymentHeader,
                 t.PaymentReason,
                 t.PaymentFile,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.PaymentStatus ,
                a.PreparedPerson,
                a.FollowPerson,
                t.Approver,
                t.ApproverTime
                ");
                strSql.Append("  from  ProjectPayment t " +
                    " left join  Project a   on a.Id=t.ProjectId " +
                    " left join ProjectContract pc on t.ContractId = pc.id ");
                strSql.Append("  WHERE (a.ProjectStatus=1 or a.ProjectStatus=3 )");
                strSql.Append("  and t.id='" + keyValue + "'");


                return this.BaseRepository("learunOAWFForm").FindList<ProjectPaymentVo>(strSql.ToString()).FirstOrDefault();

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
        public List<ProjectPaymentVo> GetProjectPaymentByTid(string keyValue)
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
                t.tid,
                t.ProjectId,
                t.ContractId,
                pc.ContractNo,
                t.WorkFlowId,
                t.PayType,
                t.Payee,
                t.PayeeBank,
                t.BankAccount,
                t.PaymentAmount,
                t.PaymentMethod,
                t.PaymentHeader,
                t.PaymentReason,
                t.PaymentFile,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.PaymentStatus ,
                a.PreparedPerson,
                a.FollowPerson,
                t.ApproverTime
                ");
                strSql.Append("  from  ProjectPayment t " +
                    " inner join  Project a on a.Id=t.ProjectId " +
                    " inner join ProjectContract pc on pc.id = t.ContractId ");
                strSql.Append("  WHERE (a.ProjectStatus=1 or a.ProjectStatus=3 )");
                strSql.Append("  and t.tid='" + keyValue + "'");


                return this.BaseRepository("learunOAWFForm").FindList<ProjectPaymentVo>(strSql.ToString()).ToList();

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
        /// 获取ProjectPayment表实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public ProjectPaymentEntity GetProjectPaymentEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindEntity<ProjectPaymentEntity>(keyValue);
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
        public ProjectPaymentVo GetEntityByProcessId(string processId)
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
                t.PayType,
                t.Payee,
                t.PayeeBank,
                t.BankAccount,
                t.PaymentAmount,
                t.PaymentMethod,
                 t.PaymentHeader,
                 t.PaymentReason,
                 t.PaymentFile,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.PaymentStatus ,
                a.PreparedPerson,
                a.FollowPerson,
                t.tid
                ");
                strSql.Append("  from  ProjectPayment t inner join  Project a   on a.Id=t.ProjectId ");
                strSql.Append("  WHERE  t.WorkFlowId='" + processId + "'  ");
                var list = this.BaseRepository("learunOAWFForm").FindList<ProjectPaymentVo>(strSql.ToString());
                return list.FirstOrDefault();
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

        public ProjectPaymentEntity GetProjectPaymentEntityByProcessId(string processId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT *");
                strSql.Append("  from  ProjectPayment t  ");
                strSql.Append("  WHERE  t.WorkFlowId='" + processId + "'  ");
                var list = this.BaseRepository("learunOAWFForm").FindList<ProjectPaymentEntity>(strSql.ToString());
                return list.FirstOrDefault();
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
        /// 批量付款获取总金额
        /// </summary>
        /// <returns></returns>
        public ProjectPaymentVo GetAmountSumByTid(string tid)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" sum(PaymentAmount) as PaymentAmountsum ");
                strSql.Append("  from  ProjectPayment ");
                strSql.Append("  WHERE  tid ='" + tid + "'  ");
                var list = this.BaseRepository("learunOAWFForm").FindList<ProjectPaymentVo>(strSql.ToString());
                return list.FirstOrDefault();
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

        public ProjectPaymentVo GetProjectPaymentByprojectId(string projectId)
        {
            try
            {
                var strSql = new StringBuilder();

                strSql.Append(@"SELECT SUM(t.PaymentAmount) as PaymentAmount   from  ProjectPayment t where t.ProjectId='" + projectId + "'");
                var list = this.BaseRepository("learunOAWFForm").FindList<ProjectPaymentVo>(strSql.ToString()).FirstOrDefault();
                return list;
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
        public ProjectContractVo GetPageListEffectiveAmountProjectId(string ProjectId)
        {
            try
            {

                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
            t.ContractAmount,pt.PaymentAmount from 
 (select sum(ContractAmount) as ContractAmount,ProjectId from ProjectContract where  ContractType=1 and ContractStatus<>6 and ContractStatus<>7 and ContractStatus<>11 group by ProjectId) t 
left join (select sum(PaymentAmount) as PaymentAmount,ProjectId from ProjectPayment where PayType<>3 and PaymentStatus<>11 group by ProjectId ) pt on pt.ProjectId=t.ProjectId
                          
                ");
                strSql.Append(" where t.ProjectId='" + ProjectId + "'    ");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString()).FirstOrDefault();
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
                var projectPayment = this.BaseRepository("learunOAWFForm").FindEntity<ProjectPaymentEntity>(keyValue);
                UserInfo userInfo = LoginUserInfo.Get();
                ProjectEntity projectEntity = this.BaseRepository("learunOAWFForm").FindEntity<ProjectEntity>(projectPayment.ProjectId);
                //projectPayment.Modify(keyValue);
                //projectPayment.PaymentStatus = 2;

                //设置报告提交人
                projectPayment.ContractSubmitter = LoginUserInfo.Get().userId;

                //获取部门code
                var department = departmentIBLL.GetEntity(LoginUserInfo.Get().departmentId);
                if (department != null)
                {
                    projectPayment.ContractSubmitterDeptCode = department.F_EnCode;
                }
                //this.BaseRepository("learunOAWFForm").Update(projectPayment);
                bool createFlag = true;
                if (String.IsNullOrEmpty(processId))
                {
                    processId = Guid.NewGuid().ToString();
                    projectPayment.WorkFlowId = processId;
                }
                else
                {
                    createFlag = false;
                }
                projectPayment.Modify(keyValue);
                projectPayment.PaymentStatus = 2;
                ProjectPaymentEntity entity1 = new ProjectPaymentEntity();
                entity1.PaymentStatus = projectPayment.PaymentStatus;
                entity1.ProjectId = projectPayment.ProjectId;

                this.BaseRepository("learunOAWFForm").Update(projectPayment);
                SaveEntity2(entity1.ProjectId, entity1);
                //db.Update(projectPayment);
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
                        schemeCode = "ProjectPayment1";
                    }
                    else
                    {
                        schemeCode = "ProjectPayment";
                    }

                    int level = 1;
                    nWFProcessIBLL.CreateFlow(schemeCode, processId, title, level, null, userInfo);
                }
                else
                {

                    if (dept.HZ_DepartmentId == 1)
                    {
                        schemeCode = "ProjectPayment1";
                    }
                    else
                    {
                        schemeCode = "ProjectPayment";
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
        /// 变更
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="processId"></param>
        public void UpdateFlowIdStatus(string keyValue, string processId)
        {
            try
            {
                var projectPayment = this.BaseRepository("learunOAWFForm").FindEntity<ProjectPaymentEntity>(keyValue);
                projectPayment.Modify(keyValue);
                projectPayment.WorkFlowId = processId;
                projectPayment.PaymentStatus = 6;
                this.BaseRepository("learunOAWFForm").Update(projectPayment);
                projectPayment.PaymentStatus = 1;
                projectPayment.WorkFlowId = "";
                projectPayment.Create();
                this.BaseRepository("learunOAWFForm").Insert(projectPayment);


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
                ProjectPaymentEntity entity = new ProjectPaymentEntity();
                var projectPayment = this.BaseRepository("learunOAWFForm").FindEntity<ProjectPaymentEntity>(keyValue);
                entity.ProjectId = projectPayment.ProjectId;
                entity.PaymentStatus = projectPayment.PaymentStatus.ToInt();
                this.BaseRepository("learunOAWFForm").Delete<ProjectPaymentEntity>(t => t.id == keyValue);
                SaveEntity2(entity.ProjectId, entity);
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
        public void DeleteByTid(string keyValue)
        {
            try
            {
                this.BaseRepository("learunOAWFForm").Delete<ProjectPaymentEntity>(t => t.tid == keyValue);
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
        //有效合同额找合同
        public IEnumerable<ProjectContractVo> ProjectContract(string projectId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"               
                t.id,
                t.ProjectId,t.ContractStatus,t.ContractType                          
                ");
                strSql.Append("  FROM  ProjectContract t inner join  Project a  on a.Id=t.ProjectId ");
                strSql.Append("  WHERE a.ProjectStatus=1 and t.MainContract=1  and t.ContractStatus<>6 and  t.ContractStatus<>7 and  t.ProjectId='" + projectId + "' ");


                var vo = this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString());
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
        //取金额
        public ProjectContractVo GetPageListEffectiveAmountProjectId2(string ProjectId, string id)
        {
            try
            {

                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
            SUM(t.ContractAmount) as ContractAmount,SUM(pt.PaymentAmount) as PaymentAmount from 
  (select sum(ContractAmount) as ContractAmount,ProjectId  from ProjectContract where  ContractType=1 and ContractStatus<>6 and ContractStatus<>7 and ContractStatus<>11 group by ProjectId) t 
left join (select sum(PaymentAmount) as PaymentAmount,ProjectId,id from ProjectPayment where PayType<>3 and PaymentStatus=4 and PaymentStatus<>11 group by ProjectId,id ) pt on pt.ProjectId=t.ProjectId
                          
                ");
                strSql.Append(" where t.ProjectId='" + ProjectId + "' and pt.id<>'" + id + "'");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString()).FirstOrDefault();
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
        //合同金额
        public ProjectContractVo GetPageListContractAmount2(string ProjectId)
        {
            try
            {

                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
            SUM(t.ContractAmount) as ContractAmount from (select sum(ContractAmount) as ContractAmount,ProjectId,id  from ProjectContract 
where  ContractType=1 and ContractStatus<>6 and ContractStatus<>7 and ContractStatus<>11 group by ProjectId,id) t      
                ");
                strSql.Append(" where t.ProjectId='" + ProjectId + "'");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString()).FirstOrDefault();
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
        public ProjectContractVo GetPageListPaymentAmount22(string ProjectId, string id)
        {
            try
            {

                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
           SUM(pt.PaymentAmount) as PaymentAmount from (select sum(PaymentAmount) as PaymentAmount,ProjectId,id from ProjectPayment 
where PayType<>3 and PaymentStatus<>11 group by ProjectId,id ) pt   
                ");
                strSql.Append(" where pt.ProjectId='" + ProjectId + "' and pt.id<>'" + id + "'");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString()).FirstOrDefault();
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
        public ProjectContractVo GetPageListContractAmountcn1(string ProjectId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
            SUM(t.ContractAmount) as ContractAmount from (select sum(ContractAmount) as ContractAmount,ProjectId,id  from ProjectContract 
where  ContractType=1 and ContractStatus<>6 and ContractStatus<>7 and ContractStatus<>11 group by ProjectId,id) t      
                ");
                strSql.Append(" where t.ProjectId='" + ProjectId + "'");

                return this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString()).FirstOrDefault();
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
        /// 付款
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public ProjectContractVo GetPageListPaymentAmount2(string ProjectId, string id)
        {
            try
            {

                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
           SUM(pt.PaymentAmount) as PaymentAmount from (select sum(PaymentAmount) as PaymentAmount,ProjectId,id from ProjectPayment 
where PayType<>3 and PaymentStatus<>11 group by ProjectId,id ) pt   
                ");
                strSql.Append(" where pt.ProjectId='" + ProjectId + "'");
                if (id != null)
                {
                    strSql.Append(" and pt.id<>'" + id + "'");
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString()).FirstOrDefault();
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
        public ProjectContractVo GetPageListContractAmountfb2(string ProjectId)
        {
            try
            {

                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
            SUM(t.ContractAmount) as ContractAmount from (select sum(ContractAmount) as ContractAmount,ProjectId,id  from ProjectContract 
where  ContractType=2 and ContractStatus<>6 and ContractStatus<>7 and ContractStatus<>11 group by ProjectId,id) t      
                ");
                strSql.Append(" where t.ProjectId='" + ProjectId + "'");

                return this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString()).FirstOrDefault();
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
        /// 付款
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>



        public ProjectContractVo GetPageListPaymentAmountID2(string ProjectId, string id)
        {
            try
            {

                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
           SUM(pt.PaymentAmount) as PaymentAmount from (select sum(PaymentAmount) as PaymentAmount,ProjectId,id from ProjectPayment 
where PayType<>3 and PaymentStatus<>11 group by ProjectId,id ) pt   
                ");
                strSql.Append(" where pt.ProjectId='" + ProjectId + "'");
                if (id != null)
                {
                    strSql.Append(" and pt.id<>'" + id + "'");
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString()).FirstOrDefault();
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
        public void SaveEntity2(string keyValue, ProjectPaymentEntity entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    ProjectContractEntity entity1 = new ProjectContractEntity();
                    //承揽合同
                    var cn = this.GetPageListContractAmountcn1(entity.ProjectId);
                    //分包合同
                    var fb = this.GetPageListContractAmountfb2(entity.ProjectId);
                    //付款金额
                    var fk = this.GetPageListPaymentAmount2(entity.ProjectId, null);

                    if (cn.ContractAmount != null && fb.ContractAmount != null && fk.PaymentAmount != null)
                    {
                        entity1.EffectiveAmount = cn.ContractAmount - fb.ContractAmount - fk.PaymentAmount;
                    }
                    if (cn.ContractAmount != null && fb.ContractAmount != null && fk.PaymentAmount == null)
                    {
                        entity1.EffectiveAmount = cn.ContractAmount - fb.ContractAmount;
                    }
                    if (cn.ContractAmount != null && fb.ContractAmount == null && fk.PaymentAmount != null)
                    {
                        entity1.EffectiveAmount = cn.ContractAmount - fk.PaymentAmount;
                    }
                    if (cn.ContractAmount != null && fb.ContractAmount == null && fk.PaymentAmount == null)
                    {
                        entity1.EffectiveAmount = cn.ContractAmount;
                    }

                    if (cn.ContractAmount == null && fb.ContractAmount != null && fk.PaymentAmount != null)
                    {
                        entity1.EffectiveAmount = 0 - fb.ContractAmount - fk.PaymentAmount;
                    }
                    if (cn.ContractAmount == null && fb.ContractAmount != null && fk.PaymentAmount == null)
                    {
                        entity1.EffectiveAmount = 0 - fb.ContractAmount;
                    }
                    if (cn.ContractAmount == null && fb.ContractAmount == null && fk.PaymentAmount != null)
                    {
                        entity1.EffectiveAmount = 0 - fk.PaymentAmount;
                    }



                    var ct1 = this.ProjectContract(entity.ProjectId);
                    if (ct1.ToList().Count > 0)
                    {
                        foreach (var ina in ct1)
                        {
                            if (ina.ProjectSource.ToInt() != 3)
                            {
                                ProjectContractEntity project1 = new ProjectContractEntity();
                                project1.Modify(ina.id);
                                if (ina.ContractType.ToInt() == 1)
                                {
                                    project1.EffectiveAmount = entity1.EffectiveAmount;
                                }
                                else
                                {
                                    project1.EffectiveAmount = 0;
                                }

                                this.BaseRepository("learunOAWFForm").Update(project1);
                            }
                            else
                            {
                                ProjectContractEntity project1 = new ProjectContractEntity();
                                project1.Modify(ina.id);
                                project1.EffectiveAmount = 0;
                                this.BaseRepository("learunOAWFForm").Update(project1);
                            }
                        }
                    }
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
        /// 保存实体数据（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public void SaveEntity(string keyValue, ProjectPaymentEntity entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.type = 0;
                    entity.Modify(keyValue);
                    this.BaseRepository("learunOAWFForm").Update(entity);
                }
                else
                {
                    entity.type = 0;
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
        /// 批量添加
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveEntityList(string Ids, ProjectPaymentEntity entity)
        {
            try
            {
                entity.ProjectId = Ids;
                entity.Create();
                this.BaseRepository("learunOAWFForm").Insert(entity);
                //    string[] IdList = Ids.Split(',');
                //    foreach (var taskId in IdList)
                //    {
                //        ProjectPaymentEntity entitylist = new ProjectPaymentEntity();
                //        entitylist.ProjectId = taskId;
                //        entitylist.PayType = entity.PayType;
                //        entitylist.Payee = entity.Payee;
                //        entitylist.PayeeBank = entity.PayeeBank;
                //        entitylist.BankAccount = entity.BankAccount;
                //        entitylist.PaymentAmount = entity.PaymentAmount;
                //        entitylist.PaymentMethod = entity.PaymentMethod;
                //        entitylist.PaymentHeader = entity.PaymentHeader;
                //        entitylist.PaymentReason = entity.PaymentReason;
                //        entitylist.PaymentFile = entity.PaymentFile;
                //        entitylist.CreateTime = DateTime.Now;
                //        entitylist.CreateUser = LoginUserInfo.Get().userId;
                //        entitylist.PaymentStatus = 1;
                //        entitylist.id = Guid.NewGuid().ToString();
                //        this.BaseRepository("learunOAWFForm").Insert(entitylist);
                //}
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
        public IEnumerable<ProjectPaymentEntity> GetAllPaymentToMatchContract()
        {
            try
            {
                string sql = "select * from ProjectPayment";
                return this.BaseRepository("learunOAWFForm").FindList_NodbWhere<ProjectPaymentEntity>(sql);
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
