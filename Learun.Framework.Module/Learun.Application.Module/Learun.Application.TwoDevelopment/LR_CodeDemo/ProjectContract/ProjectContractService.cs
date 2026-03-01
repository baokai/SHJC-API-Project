using Dapper;
using Learun.Application.Base.SystemModule;
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
    /// 日 期：2022-03-10 23:22
    /// 描 述：项目合同申请
    /// </summary>
    public class ProjectContractService : RepositoryFactory
    {
        #region 获取数据
        private NWFProcessIBLL nWFProcessIBLL = new NWFProcessBLL();
        private DepartmentIBLL departmentIBLL = new DepartmentBLL();
        private ProjectManageIBLL projectManageIBLL = new ProjectManageBLL();
        private ProjectPaymentIBLL projectPaymentIBLL = new ProjectPaymentBLL();
        private ProjectPaymentListIBLL projectPaymentListIBLL = new ProjectPaymentListBLL();
        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectContractVo> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {

                var strSql = new StringBuilder();
                strSql.Append("SELECT");
                strSql.Append(@"
                a.ProjectName,
                a.CustName,
                a.ProjectSource,
                a.FollowPerson,
                a.Id as pid,     
                t.id,
                t.ProjectId,
                t.WorkFlowId,
                t.ContractNo,
                t.ContractSubject,
                t.ContractAmount,
                t.ContractFile,
                t.ContractType,
                t.Approver,
                t.Remark,
                t.ContractRemark,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.ContractStatus ,
                t.ReceiptType as ReceivedFlag,
                t.DepartmentId,
  a.DepartmentId as ADepartmentId,
                a.PreparedPerson,
                a.FDepartmentId,
                a.PDepartmentId,
t.ReceivedFlagNo,t.MainContract,t.EffectiveAmount,t.EffectiveAmountShow,t.MainAmount,t.SubAmount,t.PaymentAmountList,t.SubDepartmentId,t.MainDepartmentId
                ");
                strSql.Append("   FROM   ProjectContract t inner join  Project a  on a.Id=t.ProjectId  ");
                //strSql.Append(" left join (select SUM(PaymentAmount) as PaymentAmount,MAX(ProjectId) as ProjectId,MAX(PayType) as PayType from ProjectPayment where PayType<>3 and PaymentStatus=4 group by ProjectId ) pt on a.Id=pt.ProjectId    ");
                strSql.Append("  WHERE t.ContractStatus<>6 and t.ContractStatus<>7");

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
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%'  or  t.ContractNo like'%{0}%'  )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId ='{0}' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like'%{0}%')", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like'%{0}%')", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%')", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like'%{0}%')", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContactName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ContactName like'%{0}%')", queryParam["ContactName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject ='{0}')", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like'%{0}%')", queryParam["DepartmentId"].ToString()));
                }
                if (!queryParam["FollowPerson"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.FollowPerson like'%{0}%')", queryParam["FollowPerson"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource like'%{0}%')", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus ='{0}')", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}')", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractType"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractType like'%{0}%')", queryParam["ContractType"].ToString()));
                }
                if (!queryParam["ReceivedFlag"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(t.ReceivedFlag,0) = '{0}')", queryParam["ReceivedFlag"].ToString()));
                }

                return this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString(), dp, pagination);
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
        /// 营销统计图待回款
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public IEnumerable<ProducTionVo> GetMoneyToBeCollected(string dep)
        {
            try
            {

                var strSql = new StringBuilder();

                strSql.Append(@"
                         
select year(t.CreateTime) Years,
month(t.CreateTime) Month, count(*) Quantity from ProjectContract t left join ProjectPayCollection pc on pc.ProjectId=t.ProjectId  
where  t.ContractStatus=4 and pc.ProjectId is null and   t.CreateTime>=dateadd(year, datediff(year, 0, getdate()), 0) and t.CreateTime<=GETUTCDATE() 
                ");
                if (dep != null)
                {
                    strSql.Append("  and t.DepartmentId='" + dep + "'");
                }

                strSql.Append(" group by year(t.CreateTime),month(t.CreateTime) ");

                // var queryParam = queryJson.ToJObject();
                // 虚拟参数
                /*  var dp = new DynamicParameters(new { });
                  if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                  {
                      dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                      dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                      strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                  }*/

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
        /// 全年项目数量
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public IEnumerable<YearRoundVo> GetyearRoundNumberOfTtems()
        {
            try
            {

                var strSql = new StringBuilder();

                strSql.Append(@" select year(CreateTime) AS Years,count(*) ProjectQuantity from Project ");
                strSql.Append(" group by YEAR(CreateTime)");
                return this.BaseRepository("learunOAWFForm").FindList<YearRoundVo>(strSql.ToString());
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
        /// 全年项目数量 按部门
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public IEnumerable<YearRoundVo> GetyearRoundNumberOfTtemsByDeptids(List<string> deptIds)
        {
            try
            {
                string chosen = " where 1 = 1 and ( ";
                for (int i = 0; i < deptIds.Count; i++)
                {/*
                    if (i == 0)
                    {
                        chosen += " a.DepartmentId = '" + deptIds[i] + "'";
                    }
                    else
                    {
                        chosen += " or a.DepartmentId = '" + deptIds[i] + "'";
                    }*/
                    if (i == 0)
                    {
                        chosen += " (  a.FDepartmentId='" + deptIds[i] + "' or a.PDepartmentId='" + deptIds[i] + "' or a.DepartmentId='" + deptIds[i] + "') ";
                    }
                    else
                    {
                        chosen += " or (  a.FDepartmentId='" + deptIds[i] + "' or a.PDepartmentId='" + deptIds[i] + "' or a.DepartmentId='" + deptIds[i] + "') ";
                    }
                }
                chosen += " ) ";
                var strSql = new StringBuilder();
                strSql.Append(@" select year(a.CreateTime) AS Years,count(*) ProjectQuantity  from Project a  " + chosen);
                strSql.Append(" group by YEAR(a.CreateTime)");
                return this.BaseRepository("learunOAWFForm").FindList<YearRoundVo>(strSql.ToString());
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
        /// 本年度项目数量 按月
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public IEnumerable<YearRoundVo> GetMonthlyRoundNumberOfTtems()
        {
            try
            {
                string currYear = DateTime.Now.ToString("yyyy");
                var strSql = new StringBuilder();
                strSql.Append(@" select month(CreateTime) AS Years,count(*) ProjectQuantity from Project ");
                strSql.Append(" where CreateTime > '" + currYear + "' group by Month(CreateTime) order by month(CreateTime) ");
                return this.BaseRepository("learunOAWFForm").FindList<YearRoundVo>(strSql.ToString());
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
        /// 本年度项目数量 按月 部门ids
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public IEnumerable<YearRoundVo> GetMonthlyRoundNumberOfTtemsByDeptids(List<string> deptIds)
        {
            try
            {
                string chosen = " ( ";
                for (int i = 0; i < deptIds.Count; i++)
                {
                    /*if (i == 0)
                    {
                        chosen += " DepartmentId = '" + deptIds[i] + "'";
                    }
                    else
                    {
                        chosen += " or DepartmentId = '" + deptIds[i] + "'";
                    }*/
                    if (i == 0)
                    {
                        chosen += " (  FDepartmentId='" + deptIds[i] + "' or PDepartmentId='" + deptIds[i] + "' or DepartmentId='" + deptIds[i] + "') ";
                    }
                    else
                    {
                        chosen += " or (   FDepartmentId='" + deptIds[i] + "' or PDepartmentId='" + deptIds[i] + "' or DepartmentId='" + deptIds[i] + "') ";
                    }
                }
                chosen += " ) ";
                string currYear = DateTime.Now.ToString("yyyy");
                var strSql = new StringBuilder();
                strSql.Append(@" select month(CreateTime) AS Years,count(*) ProjectQuantity from Project where " + chosen);
                strSql.Append(" and CreateTime > '" + currYear + "' group by Month(CreateTime) order by month(CreateTime) ");
                return this.BaseRepository("learunOAWFForm").FindList<YearRoundVo>(strSql.ToString());
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
        /// 全年已实施数量
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public IEnumerable<YearRoundVo> GetyearRoundHaveBeenImplemented()
        {
            try
            {

                var strSql = new StringBuilder();

                strSql.Append(@"
            select year(CreateTime)  AS Years,count(*) HasQuantity
     from ProjectTask where TaskStatus=5 
                ");
                strSql.Append(" group by YEAR(CreateTime)");
                return this.BaseRepository("learunOAWFForm").FindList<YearRoundVo>(strSql.ToString());
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
        /// 全年待实施数量/金额
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public IEnumerable<YearRoundVo> GetyearRoundToBeImplemented()
        {
            try
            {

                var strSql = new StringBuilder();

                strSql.Append(@"
             select year(pt.CreateTime)  AS Years,count(*) NotQuantity,sum(t.ContractAmount) NotAmount
    from ProjectContract t inner join ProjectTask pt on t.ProjectId=pt.ProjectId where TaskStatus=1
                ");
                strSql.Append("   group by YEAR(pt.CreateTime)");
                return this.BaseRepository("learunOAWFForm").FindList<YearRoundVo>(strSql.ToString());
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
        /// ②公司全年合同额承揽、承揽金额、开票金额
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public IEnumerable<YearRoundAmountVo> GetyearRoundAmountOfContract()
        {
            try
            {

                var strSql = new StringBuilder();

                strSql.Append(@"
              select year(t.CreateTime)  AS Years,count(*) PromiseQuantity,sum(t.ContractAmount) PromiseAmount,SUM(pb.BillingAmount) as BillingAmount
     from ProjectContract t left join (select SUM(BillingAmount)as BillingAmount,ProjectId from ProjectBilling  group by ProjectId) pb on pb.ProjectId=t.ProjectId
                ");
                strSql.Append(" group by YEAR(t.CreateTime)");
                return this.BaseRepository("learunOAWFForm").FindList<YearRoundAmountVo>(strSql.ToString());
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
        /// ②公司全年合同额承揽、承揽金额、开票金额 通过 部门ids
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public IEnumerable<YearRoundAmountVo> GetyearRoundAmountOfContractByDeptids(List<string> deptIds)
        {
            try
            {
                string chosen = " where 1 = 1 and ( ";
                for (int i = 0; i < deptIds.Count; i++)
                {
                    if (i == 0)
                    {
                        chosen += " ( t.MainDepartmentId='" + deptIds[i] + "' or t.SubDepartmentId='" + deptIds[i] + "' or t.DepartmentId='" + deptIds[i] + "') ";
                    }
                    else
                    {
                        chosen += " or (   t.MainDepartmentId='" + deptIds[i] + "' or t.SubDepartmentId='" + deptIds[i] + "' or t.DepartmentId='" + deptIds[i] + "') ";
                    }
                }
                chosen += " ) ";
                var strSql = new StringBuilder();

                strSql.Append(@"
              select year(t.CreateTime)  AS Years,count(*) PromiseQuantity,sum(t.ContractAmount) PromiseAmount,SUM(pb.BillingAmount) as BillingAmount
     from ProjectContract t left join (select SUM(BillingAmount)as BillingAmount,ProjectId from ProjectBilling  group by ProjectId) pb on pb.ProjectId=t.ProjectId " + chosen);
                strSql.Append(" group by YEAR(t.CreateTime)");
                return this.BaseRepository("learunOAWFForm").FindList<YearRoundAmountVo>(strSql.ToString());
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
        /// ②公司全年合同额承揽、承揽金额、开票金额 月度
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public IEnumerable<YearRoundAmountVo> GetmonthlyRoundAmountOfContract()
        {
            try
            {
                string currYear = DateTime.Now.ToString("yyyy");
                var strSql = new StringBuilder();

                strSql.Append(@"
              select month(t.CreateTime)  AS Years,count(*) PromiseQuantity,sum(t.ContractAmount) PromiseAmount,SUM(pb.BillingAmount) as BillingAmount
     from ProjectContract t left join (select SUM(BillingAmount)as BillingAmount,ProjectId from ProjectBilling  group by ProjectId) pb on pb.ProjectId=t.ProjectId
                where t.CreateTime > '" + currYear + "' ");
                strSql.Append(" group by month(t.CreateTime) order by month(t.CreateTime) ");
                return this.BaseRepository("learunOAWFForm").FindList<YearRoundAmountVo>(strSql.ToString());
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
        /// ②公司全年合同额承揽、承揽金额、开票金额 月度
        /// 通过部门ids
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public IEnumerable<YearRoundAmountVo> GetmonthlyRoundAmountOfContractByDeptids(List<string> deptIds)
        {
            try
            {
                string currYear = DateTime.Now.ToString("yyyy");
                string chosen = " where t.CreateTime > '" + currYear + "' and ( ";
                for (int i = 0; i < deptIds.Count; i++)
                {
                    if (i == 0)
                    {
                        chosen += " (  t.MainDepartmentId='" + deptIds[i] + "' or t.SubDepartmentId='" + deptIds[i] + "' or t.DepartmentId='" + deptIds[i] + "') ";
                    }
                    else
                    {
                        chosen += " or (   t.MainDepartmentId='" + deptIds[i] + "' or t.SubDepartmentId='" + deptIds[i] + "' or t.DepartmentId='" + deptIds[i] + "') ";
                    }
                }
                chosen += " ) ";
                var strSql = new StringBuilder();

                strSql.Append(@"
              select month(t.CreateTime)  AS Years,count(*) PromiseQuantity,sum(t.ContractAmount) PromiseAmount,SUM(pb.BillingAmount) as BillingAmount
     from ProjectContract t left join (select SUM(BillingAmount)as BillingAmount,ProjectId from ProjectBilling  group by ProjectId) pb on pb.ProjectId=t.ProjectId " + chosen);
                strSql.Append(" group by month(t.CreateTime) order by month(t.CreateTime) ");
                return this.BaseRepository("learunOAWFForm").FindList<YearRoundAmountVo>(strSql.ToString());
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
        /// 营销统计图待回款多部门
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public IEnumerable<ProducTionVo> GetMoneyToBeCollectedDepartmentId(string dep)
        {
            try
            {

                var strSql = new StringBuilder();

                strSql.Append(@"
             select year(t.CreateTime) Years,
month(t.CreateTime) Month, count(*) Quantity from ProjectContract t left join ProjectPayCollection pc on pc.ProjectId=t.ProjectId  
where  t.ContractStatus=4 and pc.ProjectId is null and   t.CreateTime>=dateadd(year, datediff(year, 0, getdate()), 0) and t.CreateTime<=GETUTCDATE() 
                ");
                strSql.Append("and (" + dep + ")");

                strSql.Append("group by year(t.CreateTime),month(t.CreateTime) ");

                // var queryParam = queryJson.ToJObject();
                // 虚拟参数
                /*  var dp = new DynamicParameters(new { });
                  if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                  {
                      dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                      dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                      strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                  }*/

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
        /// 营销统计图
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public IEnumerable<ProducTionVo> GetMarkeTingList(string dep)
        {
            try
            {

                var strSql = new StringBuilder();

                strSql.Append(@"
               select year(CreateTime) Years,
month(CreateTime) Month,
sum(ContractAmount) Amount,
count(*) Quantity
from ProjectContract WHERE CreateTime>=dateadd(year, datediff(year, 0, getdate()), 0) and CreateTime<=GETUTCDATE() and ContractType=1

                ");
                if (dep != null)
                {
                    strSql.Append("  and DepartmentId='" + dep + "'");
                }

                strSql.Append(" group by year(CreateTime),month(CreateTime)");

                // var queryParam = queryJson.ToJObject();
                // 虚拟参数
                /*  var dp = new DynamicParameters(new { });
                  if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                  {
                      dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                      dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                      strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                  }*/

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
        /// 营销统计图多部门
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public IEnumerable<ProducTionVo> GetMarkeTingListDepartmentId(string dep)
        {
            try
            {

                var strSql = new StringBuilder();

                strSql.Append(@"
               select year(CreateTime) Years,
            month(CreateTime) Month,
            sum(ContractAmount) Amount,
            count(*) Quantity
            from ProjectContract WHERE  CreateTime>=dateadd(year, datediff(year, 0, getdate()), 0) and CreateTime<=GETUTCDATE() and ContractType=1

                ");
                strSql.Append("and (" + dep + ")");

                strSql.Append(" group by year(CreateTime),month(CreateTime)");

                // var queryParam = queryJson.ToJObject();
                // 虚拟参数
                /*  var dp = new DynamicParameters(new { });
                  if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                  {
                      dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                      dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                      strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                  }*/

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
        /// 多部门获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectContractVo> GetPageListDepartmentId(Pagination pagination, string queryJson, string dep)
        {
            try
            {

                var strSql = new StringBuilder();
                strSql.Append("SELECT");
                strSql.Append(@"
                a.ProjectName,
                a.CustName,
                a.ProjectSource,
                a.FollowPerson,
                a.Id as pid,     
                t.id,
                t.ProjectId,
                t.WorkFlowId,
                t.ContractNo,
                t.ContractSubject,
                t.ContractAmount,
                t.ContractFile,
                t.ContractType,
                t.Approver,
                t.Remark,
                t.ContractRemark,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.ContractStatus ,
                t.ReceiptType as ReceivedFlag,
                t.DepartmentId,
  a.DepartmentId as ADepartmentId,
                a.PreparedPerson,
                a.FDepartmentId,
                a.PDepartmentId,
t.ReceivedFlagNo,t.MainContract,t.EffectiveAmount,t.EffectiveAmountShow,t.MainAmount,t.SubAmount
                ");
                strSql.Append("   FROM   ProjectContract t inner join  Project a  on a.Id=t.ProjectId  ");
                // strSql.Append(" left join (select SUM(PaymentAmount) as PaymentAmount,MAX(ProjectId) as ProjectId,MAX(PayType) as PayType from ProjectPayment where PayType<>3 and PaymentStatus=4 group by ProjectId ) pt on a.Id=pt.ProjectId    ");
                strSql.Append("  WHERE t.ContractStatus<>6 and t.ContractStatus<>7");

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
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%'  or  t.ContractNo like'%{0}%'  )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId ='{0}' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like'%{0}%')", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like'%{0}%')", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%')", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like'%{0}%')", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContactName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ContactName like'%{0}%')", queryParam["ContactName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject ='{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like'%{0}%')", queryParam["DepartmentId"].ToString()));
                }
                if (!queryParam["FollowPerson"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.FollowPerson like'%{0}%')", queryParam["FollowPerson"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource like'%{0}%')", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus ='{0}')", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}')", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractType"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractType like'%{0}%')", queryParam["ContractType"].ToString()));
                }
                if (!queryParam["ReceivedFlag"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(t.ReceivedFlag,0) = '{0}')", queryParam["ReceivedFlag"].ToString()));
                }
                strSql.Append(string.Format(" AND ( " + dep + " )"));

                return this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString(), dp, pagination);
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

        public IEnumerable<ProjectContractVo> GetContractPageList()
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
                t.ContractNo,
                t.ContractSubject,
                t.ContractAmount,
                t.ContractFile,
                t.ContractType,
                t.Approver,
                t.Remark,
                t.ContractRemark,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.ContractStatus ,
                t.ReceiptType as ReceivedFlag,
                t.DepartmentId,
                a.PreparedPerson,
                a.FDepartmentId,
                a.PDepartmentId            
                ");
                strSql.Append("   FROM   ProjectContract t inner join  Project a  on a.Id=t.ProjectId ");
                strSql.Append("  WHERE  t.ContractStatus<>6 and t.ContractStatus<>7");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString());
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
        /// 查看是否有这个项目名
        /// </summary>
        /// <returns></returns>
        public ProjectContractVo GetPageListName(string queryJson)
        {
            try
            {

                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.id,t.ContractNo,t.ContractStatus ");
                strSql.Append("   FROM   ProjectContract t  ");
                strSql.Append("  WHERE t.ContractStatus<>6 and t.ContractNo='" + queryJson + "'");
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
        /// 导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectContractVo> GetPageList(string queryJson)
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
                t.ContractNo,
                t.ContractSubject,
                t.ContractAmount,
                t.ContractFile,
                t.ContractType,
                t.Approver,
                t.Remark,
                t.ContractRemark,
                t.CreateTime,
                t.CreateUser,
                t.ContractStatus ,
                t.ReceiptType as ReceivedFlag,
                t.DepartmentId,
  a.DepartmentId as ADepartmentId,
                a.PreparedPerson,
                a.FDepartmentId,
                a.PDepartmentId,
t.ReceivedFlagNo,t.MainContract,t.EffectiveAmount,t.EffectiveAmountShow, t.SubDepartmentId, t.MainDepartmentId
             
                ");
                strSql.Append("   FROM   ProjectContract t inner join  Project a  on a.Id=t.ProjectId ");
                //strSql.Append(" left join (select SUM(PaymentAmount) as PaymentAmount,MAX(ProjectId) as ProjectId,MAX(PayType) as PayType from ProjectPayment where PayType<>3  group by ProjectId) pt on a.Id=pt.ProjectId    ");
                strSql.Append("  WHERE t.ContractStatus<>6 and t.ContractStatus<>7");

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
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%'  or  t.ContractNo like'%{0}%'  )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId ='{0}' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like'%{0}%')", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like'%{0}%')", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%')", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like'%{0}%')", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContactName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ContactName like'%{0}%')", queryParam["ContactName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject ='{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like'%{0}%')", queryParam["DepartmentId"].ToString()));
                }
                if (!queryParam["FollowPerson"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.FollowPerson like'%{0}%')", queryParam["FollowPerson"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource like'%{0}%')", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus ='{0}')", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}')", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractType"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractType like'%{0}%')", queryParam["ContractType"].ToString()));
                }
                if (!queryParam["ReceivedFlag"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(t.ReceivedFlag,0) = '{0}')", queryParam["ReceivedFlag"].ToString()));
                }


                return this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString(), dp);
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
        /// 查询所有合同
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectContractVo> GetPageListEffectiveAmount()
        {
            try
            {

                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                 a.ProjectName,
     t.id,t.MainContract,t.ContractType,
                a.ProjectSource,
                a.FollowPerson,
                    
                t.ProjectId                            
                ");
                strSql.Append("   FROM   ProjectContract t inner join  Project a  on a.Id=t.ProjectId ");
                strSql.Append("  WHERE t.ContractStatus<>6 and t.ContractStatus<>7 and  t.ContractStatus<>11 and a.ProjectSource<>3");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString());
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
        /// 根据id查找对应的总合同金额和外付
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public ProjectContractVo GetPageListEffectiveAmountProjectId(string ProjectId)
        {
            try
            {

                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
           SUM(t.ContractAmount) as ContractAmount,pt.PaymentAmount from 
 (select sum(ContractAmount) as ContractAmount,ProjectId from ProjectContract where  ContractType=1 and ContractStatus<>6 and ContractStatus<>7 and ContractStatus<>11 group by ProjectId) t 
left join (select sum(PaymentAmount) as PaymentAmount,ProjectId from ProjectPayment where PayType<>3 and PaymentStatus<>11 group by ProjectId ) pt on pt.ProjectId=t.ProjectId                       
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
        public ProjectContractVo GetPageListEffectiveAmountProjectId2(string ProjectId, string id)
        {
            try
            {

                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
            SUM(t.ContractAmount) as ContractAmount,SUM(pt.PaymentAmount) as PaymentAmount from 
  (select sum(ContractAmount) as ContractAmount,ProjectId,id  from ProjectContract where  ContractType=1 and ContractStatus<>6 and ContractStatus<>7 and ContractStatus<>11 group by ProjectId,id) t 
left join (select sum(PaymentAmount) as PaymentAmount,ProjectId from ProjectPayment where PayType<>3 and PaymentStatus=4 and PaymentStatus<>11 group by ProjectId ) pt on pt.ProjectId=t.ProjectId
                          
                ");
                strSql.Append(" where t.ProjectId='" + ProjectId + "' and t.id<>'" + id + "'");
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
        public ProjectContractVo GetPageListContractAmount2(string ProjectId, string id)
        {
            try
            {

                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
            SUM(t.ContractAmount) as ContractAmount from (select sum(ContractAmount) as ContractAmount,ProjectId,id  from ProjectContract 
where  ContractType=1 and ContractStatus<>6 and ContractStatus<>7 and ContractStatus<>11 group by ProjectId,id) t      
                ");
                strSql.Append(" where t.ProjectId='" + ProjectId + "' and t.id<>'" + id + "'");
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
        public ProjectContractVo GetPageListContractAmountfb2(string ProjectId, string id)
        {
            try
            {

                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
            SUM(t.ContractAmount) as ContractAmount from (select sum(ContractAmount) as ContractAmount,ProjectId,id  from ProjectContract 
            where  ContractType=2 and ContractStatus<>1 and ContractStatus<>6 and ContractStatus<>7 and ContractStatus<>11 group by ProjectId,id) t      
                ");
                strSql.Append(" where t.ProjectId='" + ProjectId + "'");
                if (id != null)
                {
                    strSql.Append(" and t.id<>'" + id + "'");
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
        public ProjectContractVo GetPageListContractAmountcn1(string ProjectId, string id)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
            SUM(t.ContractAmount) as ContractAmount from (select sum(ContractAmount) as ContractAmount,ProjectId,id  from ProjectContract 
            where  ContractType=1 and ContractStatus<>1 and ContractStatus<>6 and ContractStatus<>7 and ContractStatus<>11 group by ProjectId,id ) t      
                ");
                strSql.Append(" where t.ProjectId='" + ProjectId + "'");
                if (id != null)
                {
                    strSql.Append(" and t.id<>'" + id + "'");
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
        /// <summary>
        /// 付款
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public ProjectContractVo GetPageListPaymentAmount2(string ProjectId)
        {
            try
            {

                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
           SUM(pt.PaymentAmount) as PaymentAmount from (select sum(PaymentAmount) as PaymentAmount,ProjectId from ProjectPayment 
                 where PayType<>3 and PaymentStatus<>11 and PaymentStatus<>1 group by ProjectId ) pt   
                ");
                strSql.Append(" where pt.ProjectId='" + ProjectId + "'");
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
        public ProjectContractVo GetPaymentAmountByContractId(string ContractId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(@" SELECT SUM(pt.PaymentAmount) as PaymentAmount from (select sum(PaymentAmount) as PaymentAmount,ContractId,ProjectId from ProjectPayment 
                 where PayType<>3 and PaymentStatus<>11 and PaymentStatus<>1 and PayType<>5 group by ContractId,ProjectId ) pt ");
                strSql.Append(" where pt.ContractId='" + ContractId + "'");
                var result = this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString());
                if (result != null && result.ToList().Count > 0)
                {
                    return result.FirstOrDefault();
                }
                else
                {
                    return null;
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
        /// 多部门导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectContractVo> GetPageListDepartmentId(string queryJson, string dep)
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
                t.ContractNo,
                t.ContractSubject,
                t.ContractAmount,
                t.ContractFile,
                t.ContractType,
                t.Approver,
                t.Remark,
                t.ContractRemark,
                t.CreateTime,
                t.CreateUser,
                t.ContractStatus ,
                t.ReceiptType as ReceivedFlag,
                t.DepartmentId,
  a.DepartmentId as ADepartmentId,
                a.PreparedPerson,
                a.FDepartmentId,
                a.PDepartmentId,
t.ReceivedFlagNo,t.MainContract,t.EffectiveAmount,t.EffectiveAmountShow
             
                ");
                strSql.Append("   FROM   ProjectContract t inner join  Project a  on a.Id=t.ProjectId ");
                //strSql.Append(" left join (select SUM(PaymentAmount) as PaymentAmount,MAX(ProjectId) as ProjectId,MAX(PayType) as PayType from ProjectPayment where PayType<>3  group by ProjectId) pt on a.Id=pt.ProjectId    ");
                strSql.Append("  WHERE t.ContractStatus<>6 and t.ContractStatus<>7");

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
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%'  or  t.ContractNo like'%{0}%'  )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId ='{0}' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like'%{0}%')", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like'%{0}%')", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%')", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like'%{0}%')", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContactName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ContactName like'%{0}%')", queryParam["ContactName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject='{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like'%{0}%')", queryParam["DepartmentId"].ToString()));
                }
                if (!queryParam["FollowPerson"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.FollowPerson like'%{0}%')", queryParam["FollowPerson"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource like'%{0}%')", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus='{0}')", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}')", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractType"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractType like'%{0}%')", queryParam["ContractType"].ToString()));
                }
                if (!queryParam["ReceivedFlag"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(t.ReceivedFlag,0) = '{0}')", queryParam["ReceivedFlag"].ToString()));
                }

                strSql.Append(string.Format(" AND ( " + dep + " )"));

                return this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString(), dp);
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
        public IEnumerable<ProjectContractEntity> GetAllContractProject()
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(" SELECT * from ProjectContract ");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectContractEntity>(strSql.ToString());
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
        public IEnumerable<ProjectContractVo> GetContract(string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT t.DepartmentId,COUNT(a.Id) as ProjectCount,SUM(t.ContractAmount) as ContractSum");
                strSql.Append("  from Project a inner join ProjectContract t on a.Id=t.ProjectId");
                if (!queryJson.IsEmpty() && queryJson == "week")
                {
                    strSql.Append(string.Format(" WHERE t.CreateTime>=GETDATE()-7", queryJson));
                }
                else if (!queryJson.IsEmpty() && queryJson == "month")
                {
                    strSql.Append(string.Format(" WHERE t.CreateTime>=dateadd(dd,-day(getdate())+1,getdate())", queryJson));
                }
                strSql.Append(" GROUP BY t.DepartmentId ORDER BY ContractSum desc");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString());
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

        public ProjectContractVo GetFillProjectContractEntity(string keyValue)
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
                t.ContractNo,
                t.ContractSubject,
                t.ContractAmount,
                t.ContractType,
                t.Approver,
                t.Remark,
                t.ContractRemark,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,  
                t.DepartmentId,
                a.PreparedPerson,
                a.PDepartmentId,
t.Approver,
t.ApproverTime
                ");
                strSql.Append("  FROM  ProjectContract t inner join  Project a  on a.Id=t.ProjectId ");
                strSql.Append("  WHERE  t.id='" + keyValue + "'");


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
        public ProjectContractVo GetProjectContractProjectId(string projectId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");

                strSql.Append("  * FROM  ProjectContract t");
                strSql.Append("  WHERE  t.MainContract=1 and t.ProjectId='" + projectId + "'");


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

        public ProjectContractVo GetPreviewFormData(string keyValue)
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
                t.ContractNo,
                t.ContractSubject,
                t.ContractAmount,
                t.ContractFile,
                t.ContractType,
                t.Approver,
                t.Remark,
                t.ContractRemark,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.ContractStatus ,
                t.ReceivedFlag,
                t.DepartmentId,
                a.PreparedPerson,
                a.PDepartmentId,t.ReceiptType,t.ReceivedFlagNo,t.ContractStatus,
t.Approver,
t.ApproverTime,t.EffectiveAmount,t.EffectiveAmountShow,t.MainDepartmentId,t.MainAmount,t.SubDepartmentId,t.SubAmount
                ");
                strSql.Append("  FROM  ProjectContract t inner join  Project a  on a.Id=t.ProjectId ");
                strSql.Append("  WHERE  t.id='" + keyValue + "'");


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
        /// 根据projectId查询对应的合同
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public ProjectContractVo GetProjectContracByprojectId(string projectId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
             t.ProjectId,
t.EffectiveAmount
                ");
                strSql.Append("  FROM  ProjectContract t where t.ProjectId='" + projectId + "'");
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

        internal void update(string keyValue, string strEntity)
        {
            try
            {
                ProjectContractEntity projectContractEntity = GetProjectContractEntity(keyValue);
                ProjectContractEntity entity = strEntity.ToObject<ProjectContractEntity>();
                ProjectUpdateInfoEntity projectUpdateInfoEntity = new ProjectUpdateInfoEntity();
                projectUpdateInfoEntity.Create();
                projectUpdateInfoEntity.OldDepartmentId = projectContractEntity.DepartmentId;
                projectUpdateInfoEntity.NewDepartmentId = entity.DepartmentId;
                projectUpdateInfoEntity.Type = "ProjectContract";
                SaveEntity(keyValue, entity);
                this.BaseRepository("learunOAWFForm").Insert(projectUpdateInfoEntity);

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
        public List<ProjectContractEntity> GetProjectContractByProjectId(string projectId)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindList<ProjectContractEntity>(t => t.ProjectId == projectId).AsList();

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
        public ProjectContractEntity GetProjectContractByDepartmentIdProjectId(string projectId)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindEntity<ProjectContractEntity>(t => t.ProjectId == projectId);
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
        /// 重新提交审批流
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="processId"></param>
        public void UpdateFlowIdTo(string keyValue, string processId)
        {

            try
            {
                ProjectContractEntity projectContractEntity = GetProjectContractEntity(keyValue);
                projectContractEntity.Modify(keyValue);
                projectContractEntity.WorkFlowId = processId;
                projectContractEntity.ContractStatus = 2;

                this.BaseRepository("learunOAWFForm").Update(projectContractEntity);
                //有效合同额合计
                var ct = this.GetPageListEffectiveAmountProjectId(projectContractEntity.ProjectId);
                //分包
                var fb = this.GetPageListContractAmountfb2(projectContractEntity.ProjectId, keyValue);
                if (ct != null)
                {
                    if (fb.ContractAmount != null)
                    {
                        if (ct.PaymentAmount != null && ct.ContractAmount != null)
                        {
                            projectContractEntity.EffectiveAmount = ct.ContractAmount - fb.ContractAmount - ct.PaymentAmount;
                        }

                        if (ct.PaymentAmount == null && ct.ContractAmount != null)
                        {
                            projectContractEntity.EffectiveAmount = ct.ContractAmount - fb.ContractAmount;
                        }
                        if (ct.PaymentAmount == null && ct.ContractAmount == null)
                        {
                            projectContractEntity.EffectiveAmount = 0 - fb.ContractAmount;
                        }
                    }
                    else
                    {
                        if (ct.PaymentAmount != null && ct.ContractAmount != null)
                        {
                            projectContractEntity.EffectiveAmount = ct.ContractAmount - ct.PaymentAmount;
                        }

                        if (ct.PaymentAmount == null && ct.ContractAmount != null)
                        {
                            projectContractEntity.EffectiveAmount = ct.ContractAmount;
                        }
                        if (ct.PaymentAmount == null && ct.ContractAmount == null)
                        {
                            projectContractEntity.EffectiveAmount = 0;
                        }
                    }
                    var ct1 = this.ProjectContract(projectContractEntity.ProjectId);
                    if (ct1.ToList().Count > 0)
                    {
                        foreach (var ina in ct1)
                        {
                            if (ina.ProjectSource.ToInt() != 3)
                            {
                                ProjectContractEntity project1 = new ProjectContractEntity();
                                project1.Modify(ina.id);
                                project1.EffectiveAmount = projectContractEntity.EffectiveAmount;
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
        /// 提交审批流
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="processId"></param>


        public void UpdateFlowId1(string keyValue, string processId)
        {
            try
            {
                var projectContract = this.BaseRepository("learunOAWFForm").FindEntity<ProjectContractEntity>(keyValue);
                UserInfo userInfo = LoginUserInfo.Get();
                ProjectEntity projectEntity = this.BaseRepository("learunOAWFForm").FindEntity<ProjectEntity>(projectContract.ProjectId);
                projectContract.Modify(keyValue);
                projectContract.ContractStatus = 2;
                //设置合同提交人
                projectContract.ContractSubmitter = LoginUserInfo.Get().userId;
                //获取部门code
                var department = departmentIBLL.GetEntity(LoginUserInfo.Get().departmentId);
                if (department != null)
                {
                    projectContract.ContractSubmitterDeptCode = department.F_EnCode;
                }
                bool createFlag = true;
                if (String.IsNullOrEmpty(processId))
                {
                    processId = Guid.NewGuid().ToString();
                    projectContract.WorkFlowId = processId;

                }
                else
                {
                    createFlag = false;
                }
                SaveEntity(projectContract.id, projectContract);
                #region 240509
                //承揽合同
                //var cn = this.GetPageListContractAmountcn1(projectContract.ProjectId, projectContract.id);
                ////分包合同
                //var fb = this.GetPageListContractAmountfb2(projectContract.ProjectId, projectContract.id);
                ////付款金额
                //var fk = this.GetPageListPaymentAmount2(projectContract.ProjectId);
                //// 分包合同不计算有效金额
                //if (projectContract.ContractType != 1)
                //{
                //    projectContract.EffectiveAmount = 0;
                //    projectContract.EffectiveAmountShow = 0;

                //    //if (cn.ContractAmount != null && fb.ContractAmount != null && fk.PaymentAmount != null)
                //    //{
                //    //    projectContract.EffectiveAmount = cn.ContractAmount - fb.ContractAmount - fk.PaymentAmount - projectContract.ContractAmount;
                //    //}
                //    //if (cn.ContractAmount != null && fb.ContractAmount != null && fk.PaymentAmount == null)
                //    //{
                //    //    projectContract.EffectiveAmount = cn.ContractAmount - fb.ContractAmount - projectContract.ContractAmount;
                //    //}
                //    //if (cn.ContractAmount != null && fb.ContractAmount == null && fk.PaymentAmount != null)
                //    //{
                //    //    projectContract.EffectiveAmount = cn.ContractAmount - fk.PaymentAmount - projectContract.ContractAmount;
                //    //}
                //    //if (cn.ContractAmount != null && fb.ContractAmount == null && fk.PaymentAmount == null)
                //    //{
                //    //    projectContract.EffectiveAmount = cn.ContractAmount - projectContract.ContractAmount;
                //    //}
                //    //if (cn.ContractAmount == null && fb.ContractAmount == null && fk.PaymentAmount == null)
                //    //{
                //    //    projectContract.EffectiveAmount = 0 - projectContract.ContractAmount;
                //    //}
                //    //if (cn.ContractAmount == null && fb.ContractAmount != null && fk.PaymentAmount != null)
                //    //{
                //    //    projectContract.EffectiveAmount = 0 - fb.ContractAmount - fk.PaymentAmount - projectContract.ContractAmount;
                //    //}
                //    //if (cn.ContractAmount == null && fb.ContractAmount != null && fk.PaymentAmount == null)
                //    //{
                //    //    projectContract.EffectiveAmount = 0 - projectContract.ContractAmount - fb.ContractAmount;
                //    //}
                //    //if (cn.ContractAmount == null && fb.ContractAmount == null && fk.PaymentAmount != null)
                //    //{
                //    //    projectContract.EffectiveAmount = 0 - fk.PaymentAmount - projectContract.ContractAmount;
                //    //}
                //}
                ////承揽合同
                //else
                //{
                //    if (cn.ContractAmount != null && fb.ContractAmount != null && fk.PaymentAmount != null)
                //    {
                //        projectContract.EffectiveAmount = projectContract.ContractAmount + cn.ContractAmount - fb.ContractAmount - fk.PaymentAmount;
                //    }
                //    if (cn.ContractAmount != null && fb.ContractAmount != null && fk.PaymentAmount == null)
                //    {
                //        projectContract.EffectiveAmount = projectContract.ContractAmount + cn.ContractAmount - fb.ContractAmount;
                //    }
                //    if (cn.ContractAmount != null && fb.ContractAmount == null && fk.PaymentAmount != null)
                //    {
                //        projectContract.EffectiveAmount = projectContract.ContractAmount + cn.ContractAmount - fk.PaymentAmount;
                //    }
                //    if (cn.ContractAmount != null && fb.ContractAmount == null && fk.PaymentAmount == null)
                //    {
                //        projectContract.EffectiveAmount = cn.ContractAmount + projectContract.ContractAmount;
                //    }
                //    if (cn.ContractAmount == null && fb.ContractAmount == null && fk.PaymentAmount == null)
                //    {
                //        projectContract.EffectiveAmount = projectContract.ContractAmount;
                //    }
                //    if (cn.ContractAmount == null && fb.ContractAmount != null && fk.PaymentAmount != null)
                //    {
                //        projectContract.EffectiveAmount = projectContract.ContractAmount - fb.ContractAmount - fk.PaymentAmount;
                //    }
                //    if (cn.ContractAmount == null && fb.ContractAmount != null && fk.PaymentAmount == null)
                //    {
                //        projectContract.EffectiveAmount = projectContract.ContractAmount - fb.ContractAmount;
                //    }
                //    if (cn.ContractAmount == null && fb.ContractAmount == null && fk.PaymentAmount != null)
                //    {
                //        projectContract.EffectiveAmount = projectContract.ContractAmount - fk.PaymentAmount;
                //    }
                //}
                //this.BaseRepository("learunOAWFForm").Update(projectContract);
                //this.SaveEffectiveAmountToContract(projectContract.EffectiveAmount.Value, projectContract.ProjectId);
                #endregion
                #region old修改
                //var ct1 = this.ProjectContract(projectContract.ProjectId);
                //if (ct1.ToList().Count > 0)
                //{
                //    foreach (var ina in ct1)
                //    {
                //        if (ina.ProjectSource.ToInt() != 3)
                //        {
                //            ProjectContractEntity project1 = new ProjectContractEntity();
                //            project1.Modify(ina.id);
                //            if (ina.ContractType.ToInt() == 1 || ina.ContractType.ToInt() == 1)
                //            {
                //                project1.EffectiveAmount = projectContract.EffectiveAmount;
                //            }
                //            else
                //            {
                //                project1.EffectiveAmount = 0;
                //            }

                //            this.BaseRepository("learunOAWFForm").Update(project1);
                //        }
                //        else
                //        {
                //            ProjectContractEntity project1 = new ProjectContractEntity();
                //            project1.Modify(ina.id);
                //            project1.EffectiveAmount = 0;
                //            this.BaseRepository("learunOAWFForm").Update(project1);
                //        }
                //    }
                //}
                #endregion



                var user = LoginUserInfo.Get().userId;
                var followPerson = BaseRepository().FindEntity<UserEntity>(user);
                var dept = BaseRepository().FindEntity<DepartmentEntity>(followPerson.F_DepartmentId);
                string schemeCode = "";
                if (!createFlag)
                {
                    string title = projectEntity.ProjectName;
                    if (dept.HZ_DepartmentId == 1)
                    {
                        schemeCode = "ProjectContract1";
                    }
                    else
                    {
                        schemeCode = "ProjectContract";
                    }


                    int level = 1;
                    nWFProcessIBLL.CreateFlow(schemeCode, processId, title, level, null, userInfo);
                }
                else
                {
                    if (dept.HZ_DepartmentId == 1)
                    {
                        schemeCode = "ProjectContract1";
                    }
                    else
                    {
                        schemeCode = "ProjectContract";
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
        /// 提交审批流
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="processId"></param>
        internal void UpdateFlowId(string keyValue, string processId)
        {
            try
            {
                UserInfo userInfo = LoginUserInfo.Get();
                var projectContract = this.BaseRepository("learunOAWFForm").FindEntity<ProjectContractEntity>(keyValue);
                ProjectEntity projectEntity = this.BaseRepository("learunOAWFForm").FindEntity<ProjectEntity>(projectContract.ProjectId);
                var user = LoginUserInfo.Get().userId;
                var followPerson = BaseRepository().FindEntity<UserEntity>(user);
                var dept = BaseRepository().FindEntity<DepartmentEntity>(followPerson.F_DepartmentId);
                string schemeCode = "";
                if (!"".Equals(processId) && processId != null)
                {
                    string title = projectEntity.ProjectName;


                    if (dept.HZ_DepartmentId == 1)
                    {
                        schemeCode = "ProjectContract1";
                    }
                    else
                    {
                        schemeCode = "ProjectContract";
                    }

                    int level = 1;
                    // nWFProcessIBLL.AgainCreateFlow(processId, userInfo);
                    string auditors = "";
                    nWFProcessIBLL.CreateFlow(schemeCode, processId, title, level, auditors, userInfo);
                }
                else
                {
                    //生成流程id
                    processId = Guid.NewGuid().ToString();
                    projectContract.WorkFlowId = processId;
                    if (dept.HZ_DepartmentId == 1)
                    {
                        schemeCode = "ProjectContract1";
                    }
                    else
                    {
                        schemeCode = "ProjectContract";
                    }
                    int level = 1;
                    string title = projectEntity.ProjectName;
                    string auditors = "";
                    nWFProcessIBLL.CreateFlow(schemeCode, processId, title, level, auditors, userInfo);
                }
                projectContract.Modify(keyValue);
                projectContract.WorkFlowId = processId;
                projectContract.ContractStatus = 2;
                this.BaseRepository("learunOAWFForm").Update(projectContract);
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
        /// 更改审批流
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="processId"></param>
        internal void UpdateContractStatus(string keyValue, string processId)
        {
            try
            {

                var projectContract = this.BaseRepository("learunOAWFForm").FindEntity<ProjectContractEntity>(keyValue);


                if (projectContract.ContractStatus == 4)
                {
                    projectContract.Modify(keyValue);
                    projectContract.WorkFlowId = processId;
                    projectContract.ContractStatus = 6;
                    this.BaseRepository("learunOAWFForm").Update(projectContract);
                    projectContract.ContractStatus = 4;
                    projectContract.WorkFlowId = "";
                    projectContract.Create();
                    this.BaseRepository("learunOAWFForm").Insert(projectContract);
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
        /// 获取ProjectContract表实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public ProjectContractEntity GetProjectContractEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindEntity<ProjectContractEntity>(keyValue);
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
        public IEnumerable<ProjectContractVo> GetPageListCont(string cont)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"t.id ");
                strSql.Append("  FROM ProjectContract t where t.ContractNo='" + cont + "'");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString());
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
        /// 根据流程id获取合同编号
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public ProjectContractEntity GetEntityByContractNoProcessId(string processId)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindEntity<ProjectContractEntity>(t => t.ProjectId == processId);
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
        /// 根据合同编号查找是否有这个编号
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public ProjectContractEntity GetEntityByContractNo(string ContractNo)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindEntity<ProjectContractEntity>(t => t.ContractNo == ContractNo);
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
        public ProjectContractEntity GetContractByProcessId(string ProcessId)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindEntity<ProjectContractEntity>(t => t.WorkFlowId == ProcessId);
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
        public IEnumerable<ProjectContractVo> GetEntityByContractNoList(string ContractNo)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT t.id");
                strSql.Append("  from ProjectContract t where t.ContractNo='" + ContractNo + "'");


                return this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString());
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
        /// 报备根据id查合同
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public IEnumerable<ProjectContractVo> GetProjectContract(string ProjectId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT *");
                strSql.Append("  from ProjectContract t where t.ProjectId='" + ProjectId + "'");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString());

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
        /// 获取ProjectContract表实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public IEnumerable<ProjectContractEntity> GetProjectContractEntityBycNo(string keyValue)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT *");
                strSql.Append("  FROM  ProjectContract");
                strSql.Append(string.Format(" where ( ContractNo ='{0}' and ContractStatus<>6 )", keyValue));
                var vo = this.BaseRepository("learunOAWFForm").FindList<ProjectContractEntity>(strSql.ToString());
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



        public void UpdateReceivedFlag(string keyValue, ProjectContractEntity entity)
        {
            try
            {
                var projectContract = this.BaseRepository("learunOAWFForm").FindEntity<ProjectContractEntity>(keyValue);
                projectContract.Modify(keyValue);
                projectContract.ReceivedFlag = 1;
                projectContract.Remark = entity.Remark;
                projectContract.ReceivedFlagNo = entity.ReceivedFlagNo;
                projectContract.ReceiptType = entity.ReceiptType;
                this.BaseRepository("learunOAWFForm").Update(projectContract);
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
        /// 报告查合同
        /// </summary>

        /// <returns></returns>
        public ProjectContractVo ProjectTaskByContract(string projectId)
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
                t.Remark,
                t.ContractRemark,
                t.ContractNo,
                t.ContractSubject,
                t.ContractAmount,
                t.ContractFile,
                t.ContractType,
                t.Approver,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.ContractStatus ,
                t.ReceivedFlag,
                t.DepartmentId,
                a.PreparedPerson
                ");
                strSql.Append("  FROM  ProjectContract t inner join  Project a  on a.Id=t.ProjectId ");
                strSql.Append("  WHERE   t.MainContract=1 and t.ContractType=1 and t.ContractStatus=4 and  t.ProjectId='" + projectId + "' ");
                var vo = this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString()).FirstOrDefault();
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
        public ProjectContractVo ProjectTaskByContractId(string contractId)
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
                t.Remark,
                t.ContractRemark,
                t.ContractNo,
                t.ContractSubject,
                t.ContractAmount,
                t.ContractFile,
                t.ContractType,
                t.Approver,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.ContractStatus ,
                t.ReceivedFlag,
                t.DepartmentId,
                a.PreparedPerson
                ");
                strSql.Append("  FROM  ProjectContract t inner join  Project a  on a.Id=t.ProjectId ");
                strSql.Append("  WHERE   t.MainContract=1 and t.ContractType=1 and  t.id='" + contractId + "' ");
                var vo = this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString()).FirstOrDefault();
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
        /// <summary>
        /// 获取主表实体数据
        /// </summary>
        /// <param name="processId">流程实例ID</param>
        /// <returns></returns>
        public ProjectContractVo GetEntityByProcessId(string processId)
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
                t.Remark,
                t.ContractRemark,
                t.ContractNo,
                t.ContractSubject,
                t.ContractAmount,
                t.ContractFile,
                t.ContractType,
                t.Approver,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                t.ContractStatus ,
                t.ReceivedFlag,
                t.DepartmentId,
                a.PreparedPerson,
t.EffectiveAmount,t.MainDepartmentId,t.MainAmount,t.SubDepartmentId,t.SubAmount
                ");
                strSql.Append("  FROM  ProjectContract t inner join  Project a  on a.Id=t.ProjectId ");
                strSql.Append("  WHERE 1=1 ");

                strSql.Append(string.Format(" AND ( t.WorkFlowId ='{0}' )", processId));
                var vo = this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString()).FirstOrDefault();
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
        public ProjectContractEntity GetContractEntityByProcessId(string processId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT * ");
                strSql.Append("  FROM  ProjectContract ");
                strSql.Append("  WHERE 1=1 ");
                strSql.Append(string.Format(" AND ( WorkFlowId ='{0}' )", processId));
                var data = this.BaseRepository("learunOAWFForm").FindList<ProjectContractEntity>(strSql.ToString()).FirstOrDefault();
                return data;
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
                ProjectContractEntity entity = new ProjectContractEntity();
                var projectContractEntity = GetPreviewFormData(keyValue);
                entity.ProjectId = projectContractEntity.ProjectId;
                this.BaseRepository("learunOAWFForm").Delete<ProjectContractEntity>(t => t.id == keyValue);
                var ctid = this.ProjectContract(entity.ProjectId);

                if (ctid != null)
                {
                    //承揽合同
                    var cn = this.GetPageListContractAmountcn1(entity.ProjectId, null);
                    //分包合同
                    var fb = this.GetPageListContractAmountfb2(entity.ProjectId, null);
                    //付款金额
                    var fk = this.GetPageListPaymentAmount2(entity.ProjectId);
                    if (entity.ContractType != 1)
                    {
                        if (cn.ContractAmount != null && fb.ContractAmount != null && fk.PaymentAmount != null)
                        {
                            entity.EffectiveAmount = cn.ContractAmount - fb.ContractAmount - fk.PaymentAmount;
                        }
                        if (cn.ContractAmount != null && fb.ContractAmount != null && fk.PaymentAmount == null)
                        {
                            entity.EffectiveAmount = cn.ContractAmount - fb.ContractAmount;
                        }
                        if (cn.ContractAmount != null && fb.ContractAmount == null && fk.PaymentAmount != null)
                        {
                            entity.EffectiveAmount = cn.ContractAmount - fk.PaymentAmount;
                        }
                        if (cn.ContractAmount != null && fb.ContractAmount == null && fk.PaymentAmount == null)
                        {
                            entity.EffectiveAmount = cn.ContractAmount;
                        }
                        if (cn.ContractAmount == null && fb.ContractAmount == null && fk.PaymentAmount == null)
                        {
                            entity.EffectiveAmount = 0 - entity.ContractAmount;
                        }
                        if (cn.ContractAmount == null && fb.ContractAmount != null && fk.PaymentAmount != null)
                        {
                            entity.EffectiveAmount = 0 - fb.ContractAmount - fk.PaymentAmount;
                        }
                        if (cn.ContractAmount == null && fb.ContractAmount != null && fk.PaymentAmount == null)
                        {
                            entity.EffectiveAmount = 0 - entity.ContractAmount - fb.ContractAmount;
                        }
                        if (cn.ContractAmount == null && fb.ContractAmount == null && fk.PaymentAmount != null)
                        {
                            entity.EffectiveAmount = 0 - fk.PaymentAmount;
                        }
                    }
                    else
                    {
                        if (cn.ContractAmount != null && fb.ContractAmount != null && fk.PaymentAmount != null)
                        {
                            entity.EffectiveAmount = cn.ContractAmount - fb.ContractAmount - fk.PaymentAmount;
                        }
                        if (cn.ContractAmount != null && fb.ContractAmount != null && fk.PaymentAmount == null)
                        {
                            entity.EffectiveAmount = cn.ContractAmount - fb.ContractAmount;
                        }
                        if (cn.ContractAmount != null && fb.ContractAmount == null && fk.PaymentAmount != null)
                        {
                            entity.EffectiveAmount = cn.ContractAmount - fk.PaymentAmount;
                        }
                        if (cn.ContractAmount != null && fb.ContractAmount == null && fk.PaymentAmount == null)
                        {
                            entity.EffectiveAmount = cn.ContractAmount;
                        }
                        if (cn.ContractAmount == null && fb.ContractAmount == null && fk.PaymentAmount == null)
                        {
                            entity.EffectiveAmount = 0;
                        }
                        if (cn.ContractAmount == null && fb.ContractAmount != null && fk.PaymentAmount != null)
                        {
                            entity.EffectiveAmount = fb.ContractAmount - fk.PaymentAmount;
                        }
                        if (cn.ContractAmount == null && fb.ContractAmount != null && fk.PaymentAmount == null)
                        {
                            entity.EffectiveAmount = fb.ContractAmount;
                        }
                        if (cn.ContractAmount == null && fb.ContractAmount == null && fk.PaymentAmount != null)
                        {
                            entity.EffectiveAmount = fk.PaymentAmount;
                        }
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
                                if (ina.ContractType.ToInt() == 1 || ina.ContractType.ToInt() == 1)
                                {
                                    project1.EffectiveAmount = entity.EffectiveAmount;
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
        /// 根据id修改指定状态
        /// </summary>
        /// <param name="keyValue"></param>
        public ProjectContractVo UPGetPageListName(string keyValue)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(@"UPDATE ProjectContract set ContractStatus=4,ReceiptType=4 ");
                strSql.Append("  WHERE  ContractStatus<>7 and id='" + keyValue + "'");
                var vo = this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString()).FirstOrDefault();
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
        public ProjectContractVo GetPageListsum(string keyValue)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(@"select      a.FollowPerson,  
                t.CreateUser,             
                t.DepartmentId,
  a.DepartmentId as ADepartmentId,
                a.PreparedPerson,
                a.FDepartmentId,
                a.PDepartmentId, t.ContractAmount
              from (select SUM(ContractAmount) as ContractAmount,MAX(CreateUser) as CreateUser,MAX(DepartmentId) as DepartmentId, ProjectId from ProjectContract where ContractType=1 GROUP BY ProjectId ) t  
                inner join  Project a on a.Id=t.ProjectId   where  t.ProjectId='" + keyValue + "'  ");

                var vo = this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString()).FirstOrDefault();
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
        /// <summary>
        /// 根据projectId查找对应的承揽合同的和
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ProjectContractVo GetPageListsumcl(string projectid)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(@"select  SUM(t.ContractAmount) as ContractAmount     
              from ProjectContract  t  
                 where  t.ProjectId='" + projectid + "' and ContractType=1 and t.ContractStatus=4 and t.ContractStatus<>6 and t.ContractStatus<>7 ");

                var vo = this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString()).FirstOrDefault();
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
        /// <summary>
        /// 根据projectId查找对应的分包合同的和
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ProjectContractVo GetPageListsumfb(string projectid)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(@"select  SUM(t.ContractAmount) as ContractAmount     
              from ProjectContract  t  
                 where  t.ProjectId='" + projectid + "' and t.ContractType=2 and t.ContractStatus=4 and t.ContractStatus<>6 and t.ContractStatus<>7 ");

                var vo = this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString()).FirstOrDefault();
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
        /// <summary>
        /// 根据id修改指定状态
        /// </summary>
        /// <param name="keyValue"></param>
        public ProjectContractVo UPGetPageListNameQX(string keyValue)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(@"UPDATE ProjectContract set ReceiptType=4 ");
                strSql.Append("  WHERE ContractStatus=6  and id='" + keyValue + "'");
                var vo = this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString()).FirstOrDefault();
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
        /// <summary>
        /// 保存实体数据（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public void FillSaveForm(ProjectContractEntity entity)
        {
            try
            {

                entity.MainContract = 0;
                entity.EffectiveAmount = entity.ContractAmount;
                entity.Create();
                this.BaseRepository("learunOAWFForm").Insert(entity);
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
        public void SaveEntityEffectiveAmount(string keyValue, ProjectContractEntity entity)
        {

            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    //修改
                    entity.Modify(keyValue);
                    this.BaseRepository("learunOAWFForm").Update(entity);
                }
                else
                {
                    //添加                 
                    entity.MainContract = 1;
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
        public IEnumerable<ProjectContractEntity> ProjectContractByProjectId(string projectId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"               
                t.*                         
                ");
                strSql.Append("  FROM  ProjectContract t inner join  Project a  on a.Id=t.ProjectId ");
                strSql.Append("  WHERE ( a.ProjectStatus=1 or a.ProjectStatus=3 )and t.ContractStatus<>1 and t.ContractStatus<>6 and  t.ContractStatus<>7 and t.ContractStatus<>11 and  t.ProjectId='" + projectId + "' ");
                var vo = this.BaseRepository("learunOAWFForm").FindList<ProjectContractEntity>(strSql.ToString());
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
        public IEnumerable<ProjectContractEntity> GetAllContractByProjectId(string projectId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"               
                t.*                         
                ");
                strSql.Append("  FROM  ProjectContract t inner join  Project a  on a.Id=t.ProjectId ");
                strSql.Append("  WHERE  t.ProjectId='" + projectId + "' ");
                var vo = this.BaseRepository("learunOAWFForm").FindList<ProjectContractEntity>(strSql.ToString());
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
        public IEnumerable<ProjectContractVo> ProjectContract(string projectId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"               
                t.id,
                t.ProjectId,a.ProjectSource,t.ContractType                          
                ");
                strSql.Append("  FROM  ProjectContract t inner join  Project a  on a.Id=t.ProjectId ");
                strSql.Append("  WHERE a.ProjectStatus=1 and t.MainContract=1  and  t.ContractStatus<>6 and  t.ContractStatus<>7 and t.ContractStatus<>11 and  t.ProjectId='" + projectId + "' ");
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
        public void SaveEntity1(string keyValue, ProjectContractEntity entity)
        {

            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {


                    entity.Modify(keyValue);
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
        public void SaveProjectSettlement(ProjectContractSettlementEntity entity)
        {

            try
            {
                if (entity.id > 0)
                {
                    entity.Modify();
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
        public ProjectContractSettlementEntity GetSettlementByProjectId(string projectId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(" select * from ProjectContractSettlement where projectId = '" + projectId + "' ");
                var list = this.BaseRepository("learunOAWFForm").FindList_NodbWhere<ProjectContractSettlementEntity>(strSql.ToString());
                if (list.ToList().Count > 0)
                {
                    return list.FirstOrDefault();
                }
                else
                {
                    return new ProjectContractSettlementEntity();
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
        public ProjectContractSettlementEntity GetSettlementByContractId(string ContractId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(" select * from ProjectContractSettlement where ContractId = '" + ContractId + "' ");
                var list = this.BaseRepository("learunOAWFForm").FindList_NodbWhere<ProjectContractSettlementEntity>(strSql.ToString());
                if (list.ToList().Count > 0)
                {
                    return list.FirstOrDefault();
                }
                else
                {
                    return new ProjectContractSettlementEntity();
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
        //public void SaveEntity(string keyValue, ProjectContractEntity entity)
        //{
        //    try
        //    {
        //        //ProjectContractEntity project = new ProjectContractEntity();
        //        decimal contractAmount = 0;
        //        //合同金额计算
        //        if (entity.MainAmount != null && entity.SubAmount != null)
        //        {
        //            entity.ContractAmount = entity.MainAmount + entity.SubAmount;
        //        }
        //        else if (entity.MainAmount != null && entity.SubAmount == null)
        //        {
        //            entity.ContractAmount = entity.MainAmount;
        //        }
        //        else
        //        {
        //            entity.ContractAmount = entity.ContractAmount;
        //        }
        //        //非草稿状态才能计入有效合同金额
        //        if (entity.ContractStatus != 1)
        //        {
        //            contractAmount = entity.ContractAmount.Value;
        //        }
        //        if (!string.IsNullOrEmpty(keyValue))
        //        {
        //            //var ct = this.GetPageListEffectiveAmountProjectId(entity.ProjectId);
        //            if (entity.ProjectId == null || entity.ContractStatus == null)
        //            {
        //                var projectContractEntity = GetPreviewFormData(keyValue);
        //                entity.ProjectId = projectContractEntity.ProjectId;
        //                entity.ContractStatus = projectContractEntity.ContractStatus.ToInt();
        //            }
        //            //承揽合同
        //            var cn = this.GetPageListContractAmountcn1(entity.ProjectId, keyValue);
        //            //分包合同
        //            var fb = this.GetPageListContractAmountfb2(entity.ProjectId, keyValue);
        //            //付款金额
        //            var fk = this.GetPageListPaymentAmount2(entity.ProjectId);
        //            if (entity.ContractType != 1)
        //            {
        //                if (cn.ContractAmount != null && fb.ContractAmount != null && fk.PaymentAmount != null)
        //                {
        //                    entity.EffectiveAmount = cn.ContractAmount - fb.ContractAmount - fk.PaymentAmount - contractAmount;
        //                }
        //                if (cn.ContractAmount != null && fb.ContractAmount != null && fk.PaymentAmount == null)
        //                {
        //                    entity.EffectiveAmount = cn.ContractAmount - fb.ContractAmount - contractAmount;
        //                }
        //                if (cn.ContractAmount != null && fb.ContractAmount == null && fk.PaymentAmount != null)
        //                {
        //                    entity.EffectiveAmount = cn.ContractAmount - fk.PaymentAmount - contractAmount;
        //                }
        //                if (cn.ContractAmount != null && fb.ContractAmount == null && fk.PaymentAmount == null)
        //                {
        //                    entity.EffectiveAmount = cn.ContractAmount - contractAmount;
        //                }
        //                if (cn.ContractAmount == null && fb.ContractAmount == null && fk.PaymentAmount == null)
        //                {
        //                    entity.EffectiveAmount = 0 - contractAmount;
        //                }
        //                if (cn.ContractAmount == null && fb.ContractAmount != null && fk.PaymentAmount != null)
        //                {
        //                    entity.EffectiveAmount = 0 - fb.ContractAmount - fk.PaymentAmount - contractAmount;
        //                }
        //                if (cn.ContractAmount == null && fb.ContractAmount != null && fk.PaymentAmount == null)
        //                {
        //                    entity.EffectiveAmount = 0 - contractAmount - fb.ContractAmount;
        //                }
        //                if (cn.ContractAmount == null && fb.ContractAmount == null && fk.PaymentAmount != null)
        //                {
        //                    entity.EffectiveAmount = 0 - fk.PaymentAmount - contractAmount;
        //                }
        //            }
        //            else
        //            {
        //                if (cn.ContractAmount != null && fb.ContractAmount != null && fk.PaymentAmount != null)
        //                {
        //                    entity.EffectiveAmount = contractAmount + cn.ContractAmount - fb.ContractAmount - fk.PaymentAmount;
        //                }
        //                if (cn.ContractAmount != null && fb.ContractAmount != null && fk.PaymentAmount == null)
        //                {
        //                    entity.EffectiveAmount = contractAmount + cn.ContractAmount - fb.ContractAmount;
        //                }
        //                if (cn.ContractAmount != null && fb.ContractAmount == null && fk.PaymentAmount != null)
        //                {
        //                    entity.EffectiveAmount = contractAmount + cn.ContractAmount - fk.PaymentAmount;
        //                }
        //                if (cn.ContractAmount != null && fb.ContractAmount == null && fk.PaymentAmount == null)
        //                {
        //                    entity.EffectiveAmount = cn.ContractAmount + contractAmount;
        //                }
        //                if (cn.ContractAmount == null && fb.ContractAmount == null && fk.PaymentAmount == null)
        //                {
        //                    entity.EffectiveAmount = contractAmount;
        //                }
        //                if (cn.ContractAmount == null && fb.ContractAmount != null && fk.PaymentAmount != null)
        //                {
        //                    entity.EffectiveAmount = contractAmount - fb.ContractAmount - fk.PaymentAmount;
        //                }
        //                if (cn.ContractAmount == null && fb.ContractAmount != null && fk.PaymentAmount == null)
        //                {
        //                    entity.EffectiveAmount = contractAmount - fb.ContractAmount;
        //                }
        //                if (cn.ContractAmount == null && fb.ContractAmount == null && fk.PaymentAmount != null)
        //                {
        //                    entity.EffectiveAmount = contractAmount - fk.PaymentAmount;
        //                }
        //            }
        //            this.BaseRepository("learunOAWFForm").Update(entity);
        //            this.SaveEffectiveAmountToContract(entity.EffectiveAmount.Value, entity.ProjectId);
        //            #region 修改 old
        //            //修改 old
        //            //entity.Modify(keyValue);
        //            //this.BaseRepository("learunOAWFForm").Update(entity);
        //            //var ct1 = this.ProjectContract(entity.ProjectId);
        //            //if (ct1.ToList().Count > 0)
        //            //{
        //            //    foreach (var ina in ct1)
        //            //    {
        //            //        if (ina.ProjectSource.ToInt() != 3)
        //            //        {
        //            //            ProjectContractEntity project1 = new ProjectContractEntity();
        //            //            project1.Modify(ina.id);
        //            //            if (ina.ContractType.ToInt() == 1 || ina.ContractType.ToInt() == 1)
        //            //            {
        //            //                project1.EffectiveAmount = entity.EffectiveAmount;
        //            //            }
        //            //            else
        //            //            {
        //            //                project1.EffectiveAmount = 0;
        //            //            }

        //            //            this.BaseRepository("learunOAWFForm").Update(project1);
        //            //        }
        //            //        else
        //            //        {
        //            //            ProjectContractEntity project1 = new ProjectContractEntity();
        //            //            project1.Modify(ina.id);
        //            //            project1.EffectiveAmount = 0;
        //            //            this.BaseRepository("learunOAWFForm").Update(project1);
        //            //        }
        //            //    }
        //            //}
        //            #endregion
        //        }
        //        else
        //        {
        //            //承揽合同
        //            var cn = this.GetPageListContractAmountcn1(entity.ProjectId, null);
        //            //分包合同
        //            var fb = this.GetPageListContractAmountfb2(entity.ProjectId, null);
        //            //付款金额
        //            var fk = this.GetPageListPaymentAmount2(entity.ProjectId);
        //            if (entity.ContractType != 1)
        //            {
        //                if (cn.ContractAmount != null && fb.ContractAmount != null && fk.PaymentAmount != null)
        //                {
        //                    entity.EffectiveAmount = cn.ContractAmount - fb.ContractAmount - fk.PaymentAmount - entity.ContractAmount;
        //                }
        //                if (cn.ContractAmount != null && fb.ContractAmount != null && fk.PaymentAmount == null)
        //                {
        //                    entity.EffectiveAmount = cn.ContractAmount - fb.ContractAmount - entity.ContractAmount;
        //                }
        //                if (cn.ContractAmount != null && fb.ContractAmount == null && fk.PaymentAmount != null)
        //                {
        //                    entity.EffectiveAmount = cn.ContractAmount - fk.PaymentAmount - entity.ContractAmount;
        //                }
        //                if (cn.ContractAmount != null && fb.ContractAmount == null && fk.PaymentAmount == null)
        //                {
        //                    entity.EffectiveAmount = cn.ContractAmount - entity.ContractAmount;
        //                }
        //                if (cn.ContractAmount == null && fb.ContractAmount == null && fk.PaymentAmount == null)
        //                {
        //                    entity.EffectiveAmount = 0 - entity.ContractAmount;
        //                }
        //                if (cn.ContractAmount == null && fb.ContractAmount != null && fk.PaymentAmount != null)
        //                {
        //                    entity.EffectiveAmount = 0 - fb.ContractAmount - fk.PaymentAmount - entity.ContractAmount;
        //                }
        //                if (cn.ContractAmount == null && fb.ContractAmount != null && fk.PaymentAmount == null)
        //                {
        //                    entity.EffectiveAmount = 0 - entity.ContractAmount - fb.ContractAmount;
        //                }
        //                if (cn.ContractAmount == null && fb.ContractAmount == null && fk.PaymentAmount != null)
        //                {
        //                    entity.EffectiveAmount = 0 - fk.PaymentAmount - entity.ContractAmount;
        //                }
        //            }
        //            else
        //            {
        //                if (cn.ContractAmount != null && fb.ContractAmount != null && fk.PaymentAmount != null)
        //                {
        //                    entity.EffectiveAmount = entity.ContractAmount + cn.ContractAmount - fb.ContractAmount - fk.PaymentAmount;
        //                }
        //                if (cn.ContractAmount != null && fb.ContractAmount != null && fk.PaymentAmount == null)
        //                {
        //                    entity.EffectiveAmount = entity.ContractAmount + cn.ContractAmount - fb.ContractAmount;
        //                }
        //                if (cn.ContractAmount != null && fb.ContractAmount == null && fk.PaymentAmount != null)
        //                {
        //                    entity.EffectiveAmount = entity.ContractAmount + cn.ContractAmount - fk.PaymentAmount;
        //                }
        //                if (cn.ContractAmount != null && fb.ContractAmount == null && fk.PaymentAmount == null)
        //                {
        //                    entity.EffectiveAmount = cn.ContractAmount + entity.ContractAmount;
        //                }
        //                if (cn.ContractAmount == null && fb.ContractAmount == null && fk.PaymentAmount == null)
        //                {
        //                    entity.EffectiveAmount = entity.ContractAmount;
        //                }
        //                if (cn.ContractAmount == null && fb.ContractAmount != null && fk.PaymentAmount != null)
        //                {
        //                    entity.EffectiveAmount = entity.ContractAmount - fb.ContractAmount - fk.PaymentAmount;
        //                }
        //                if (cn.ContractAmount == null && fb.ContractAmount != null && fk.PaymentAmount == null)
        //                {
        //                    entity.EffectiveAmount = entity.ContractAmount - fb.ContractAmount;
        //                }
        //                if (cn.ContractAmount == null && fb.ContractAmount == null && fk.PaymentAmount != null)
        //                {
        //                    entity.EffectiveAmount = entity.ContractAmount - fk.PaymentAmount;
        //                }
        //            }
        //            this.SaveEffectiveAmountToContract(entity.EffectiveAmount.Value, entity.ProjectId);
        //            #region 新增old
        //            //var ct1 = this.ProjectContract(entity.ProjectId);
        //            //if (ct1.ToList().Count > 0)
        //            //{
        //            //    foreach (var ina in ct1)
        //            //    {
        //            //        if (ina.ProjectSource.ToInt() != 3)
        //            //        {
        //            //            ProjectContractEntity project1 = new ProjectContractEntity();
        //            //            project1.Modify(ina.id);
        //            //            /*   if (ina.ContractType.ToInt() == 1)
        //            //               {
        //            //                   project1.EffectiveAmount = entity.EffectiveAmount;
        //            //               }
        //            //               else
        //            //               {
        //            //                   project1.EffectiveAmount = 0;
        //            //               }*/
        //            //            if (ina.ContractType.ToInt() == 1 || ina.ContractType.ToInt() == 1)
        //            //            {
        //            //                project1.EffectiveAmount = entity.EffectiveAmount;
        //            //            }
        //            //            else
        //            //            {
        //            //                project1.EffectiveAmount = 0;
        //            //            }
        //            //            this.BaseRepository("learunOAWFForm").Update(project1);
        //            //        }
        //            //        else
        //            //        {
        //            //            ProjectContractEntity project1 = new ProjectContractEntity();
        //            //            project1.Modify(ina.id);
        //            //            project1.EffectiveAmount = 0;
        //            //            this.BaseRepository("learunOAWFForm").Update(project1);
        //            //        }
        //            //    }
        //            //}
        //            #endregion
        //            //添加                 
        //            entity.MainContract = 1;
        //            entity.Create();
        //            this.BaseRepository("learunOAWFForm").Insert(entity);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex is ExceptionEx)
        //        {
        //            throw;
        //        }
        //        else
        //        {
        //            throw ExceptionEx.ThrowServiceException(ex);
        //        }
        //    }
        //}
        public void SaveEntity(string keyValue, ProjectContractEntity entity)
        {
            try
            {
                decimal contractAmount = 0;
                decimal paymentAmount = 0;
                if (!string.IsNullOrEmpty(keyValue))
                {
                    if (entity.ProjectId == null || entity.ContractStatus == null)
                    {
                        var projectContractEntity = GetPreviewFormData(keyValue);
                        entity.ProjectId = projectContractEntity.ProjectId;
                        entity.ContractStatus = projectContractEntity.ContractStatus.ToInt();
                    }

                    // 分包合同不计算有效金额
                    if (entity.ContractType != 1)
                    {
                        entity.EffectiveAmount = 0;
                        entity.EffectiveAmountShow = 0;
                        this.BaseRepository("learunOAWFForm").Update(entity);
                    }
                    // 承揽合同
                    else
                    {

                        this.BaseRepository("learunOAWFForm").Update(entity);
                        //承揽合同
                        //var cn = this.GetPageListContractAmountcn1(entity.ProjectId, null);
                        //分包合同
                        // var fb = this.GetPageListContractAmountfb2(entity.ProjectId, keyValue);
                        //付款金额
                        var fk = this.GetPaymentAmountByContractId(entity.id);
                        contractAmount = entity.ContractAmount == null ? 0 : entity.ContractAmount.Value;
                        if (fk != null)
                        {
                            paymentAmount = fk.PaymentAmount == null ? 0 : fk.PaymentAmount.Value;
                            entity.EffectiveAmount = contractAmount - paymentAmount;
                            entity.EffectiveAmountShow = entity.EffectiveAmount;
                        }
                        else
                        {
                            entity.EffectiveAmount = contractAmount;
                            entity.EffectiveAmountShow = contractAmount;
                        }
                        this.BaseRepository("learunOAWFForm").Update(entity);
                        //this.SaveEffectiveAmountToContract(entity.EffectiveAmount.Value, entity.ProjectId);
                    }
                }
                else
                {
                    // 分包合同不计算有效金额
                    if (entity.ContractType == 2)
                    {
                        entity.EffectiveAmount = 0;
                        entity.EffectiveAmountShow = 0;
                        //添加                 
                        entity.MainContract = 1;
                        entity.Create();
                        this.BaseRepository("learunOAWFForm").Insert(entity);
                    }
                    else
                    {
                        //添加                 
                        entity.MainContract = 1;
                        entity.Create();
                        this.BaseRepository("learunOAWFForm").Insert(entity);
                        //承揽合同
                        //var cn = this.GetPageListContractAmountcn1(entity.ProjectId, null);
                        //分包合同
                        // var fb = this.GetPageListContractAmountfb2(entity.ProjectId, null);
                        //付款金额
                        var fk = this.GetPaymentAmountByContractId(entity.id);
                        contractAmount = entity.ContractAmount == null ? 0 : entity.ContractAmount.Value;
                        if (fk != null)
                        {
                            paymentAmount = fk.PaymentAmount == null ? 0 : fk.PaymentAmount.Value;
                            entity.EffectiveAmount = contractAmount - paymentAmount;
                            entity.EffectiveAmountShow = entity.EffectiveAmount;
                        }
                        else
                        {
                            entity.EffectiveAmount = contractAmount;
                            entity.EffectiveAmountShow = contractAmount;
                        }
                        this.BaseRepository("learunOAWFForm").Update(entity);
                        //this.SaveEffectiveAmountToContract(entity.EffectiveAmount.Value, entity.ProjectId);
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

        public void RecaculateEffectiveAmountByProjectId(string ProjectId)
        {
            var projectContract = GetProjectContractByProjectId(ProjectId);
            if (projectContract.ToList().Count > 0)
            {
                foreach (var item in projectContract)
                {
                    SaveEntity(item.id, item);
                }
            }
        }
        private void SaveEffectiveAmountToContract(decimal amount, string projectId)
        {
            var list = this.ProjectContractByProjectId(projectId);
            var all_contractt_list = this.GetAllContractByProjectId(projectId);
            list = list.Where(i => i.ContractType == 1).OrderBy(i => i.CreateTime).ToList();
            string selected_contract_id = "";
            if (list.ToList().Count > 0)
            {
                //判断是否有主合同
                var main_list = list.Where(i => i.MainContract == 1).ToList();
                if (main_list.Count >= 1)
                {
                    ProjectContractEntity entity = main_list.FirstOrDefault();
                    entity.Modify(entity.id);
                    entity.EffectiveAmount = amount;
                    entity.EffectiveAmountShow = amount;
                    this.BaseRepository("learunOAWFForm").Update(entity);
                    selected_contract_id = entity.id;
                }
                else
                {
                    ProjectContractEntity entity = list.FirstOrDefault();
                    entity.Modify(entity.id);
                    entity.EffectiveAmount = amount;
                    entity.EffectiveAmountShow = amount;
                    this.BaseRepository("learunOAWFForm").Update(entity);
                    selected_contract_id = entity.id;
                }
            }
            var not_selected_list = all_contractt_list.Where(i => i.id != selected_contract_id).ToList();
            if (not_selected_list.Count > 0)
            {
                foreach (var entity in not_selected_list)
                {
                    //分包合同有效额度都是0
                    if (entity.ContractType == 2)
                    {
                        amount = 0;
                    }
                    entity.Modify(entity.id);
                    entity.EffectiveAmount = amount;
                    entity.EffectiveAmountShow = 0;
                    this.BaseRepository("learunOAWFForm").Update(entity);
                }
            }
        }
        public void RecaculateEffectiveAmount()
        {
            var contract_list = this.GetAllContractProject();
            foreach (var entity in contract_list)
            {
                this.SaveEntity(entity.id, entity);
                //var list = this.ProjectContractByProjectId(project.ProjectId);
                //if (list.ToList().Count > 0)
                //{
                //    //筛选出分包合同
                //    var fb_list = list.Where(i => i.ContractType == 2).ToList();
                //    if (fb_list.Count > 0)
                //    {
                //        foreach (var entity in fb_list)
                //        {
                //            this.SaveEntity(entity.id, entity);
                //        }
                //    }
                //    //承揽合同
                //    var cl_list = list.Where(i => i.ContractType == 1).ToList();
                //    if (cl_list.Count > 0)
                //    {
                //        cl_list = cl_list.OrderBy(i => i.CreateTime).ToList();
                //        var entity = this.GetProjectContractEntity(cl_list.FirstOrDefault().id);
                //        this.SaveEntity(entity.id, entity);
                //    }
                //}
            }
        }
        /// <summary>
        /// 合作伙伴结算台账添加比例
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveFormReportForms(string keyValue, ProjectContractEntity entity)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(@"UPDATE ProjectContract set Proportion='" + entity.Proportion + "'");
                strSql.Append("  WHERE ProjectId='" + keyValue + "'");
                this.BaseRepository("learunOAWFForm").FindList<ProjectContractVo>(strSql.ToString()).FirstOrDefault();

                // entity.ModifyReportForms(keyValue);
                // this.BaseRepository("learunOAWFForm").Update(entity);

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
        public IEnumerable<ProjectVo> GetAllContractFroMatch()
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.CreateTime,
                t.ContactPhone,
                t.CreateUser,
                pc.ContractAmount,
                t.ProjectCode,t.ProjectName,pc.ContractNo,pc.id as ContractId,t.CustName 
                ");
                strSql.Append("  FROM Project t inner join ProjectContract pc on pc.ProjectId=t.Id " +
                    //"  and pc.ContractStatus=4 " +
                    " and pc.ContractType=1 ");
                strSql.Append("  WHERE (t.ProjectStatus = 1 or t.ProjectStatus = 3) AND pc.MainContract=1");
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                return this.BaseRepository("learunOAWFForm").FindList_NodbWhere<ProjectVo>(strSql.ToString());
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
