using Dapper;
using Learun.DataBase.Repository;
using Learun.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo.ReportForms
{
    public class ReportFormsService : RepositoryFactory
    {

        #region 获取数据


        /// <summary>
        /// 获取营销报表的数据信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<MarketingEntity> GetMarketings(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
				pc.id as PC,
                t.id as Tid,
                CONVERT(varchar(100), t.ApproverTime, 111) as ApproverTime,
                CONVERT(varchar(100), t.CreateTime, 111) as CreateTime,
                t.ContractNo,
	            a.ProjectName,
	            a.CustName,
	            t.ContractSubject,
                t.DepartmentId,
				a.FDepartmentId,
				a.PDepartmentId,
	            u.F_RealName,
	            a.ProjectSource,
	            t.ContractStatus,
                (case t.ReceivedFlag
	            when 1 then '已归档'
	            else '未归档'
	            end) as ReceivedFlag,
	            (case pb.BillingStatus
                when 7 then '已开票'
	            else '未开票'
	            end) as BillingStatus,
	            t.ContractAmount,
                isnull(pc.Amount,0) as Amount,
	            t.ContractAmount-isnull(pc.Amount,0) as NotReceived,
	            pc.ReceiptDate,
	            pt.DepartmentId as J_F_FullName,
	            pt.ApproachTime,
	            pt.ReportSubject,
	            pt.TaskStatus,
	            t.ContractType,
	            u2.F_RealName as P_F_RealName
                ");
                strSql.Append(@"FROM ProjectContract t 
                    inner join  Project a  on a.Id = t.ProjectId and t.ContractStatus<>6 and t.ContractStatus<>7
                    left join adms706.dbo.lr_base_user u on u.F_UserId = a.FollowPerson
                    left join ProjectPayCollection pc on pc.ProjectId = a.Id
                    left join  ProjectTask pt on pt.ProjectId = a.id 
                    left join ProjectPayment pp on pp.ProjectId=a.Id  
                    left join adms706.dbo.lr_base_user u2 on pt.ProjectResponsible=u2.F_UserId
                    left join ProjectBilling pb on pb.ProjectId = a.Id and pb.BillingStatus <>1 and pb.BillingStatus<>8");
                strSql.Append("  WHERE (t.ContractStatus=4 AND t.ContractType=1) and (pt.DeleteFlag=0 or pt.DeleteFlag is NULL)");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.ApproverTime >= @startTime AND t.ApproverTime <= @endTime ) ");
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%'  or  t.ContractNo like'%{0}%'  )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount like '%{0}%' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId = '{0}' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<MarketingEntity>(strSql.ToString(), dp, pagination);
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
        /// 营销台账列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<MarketingEntity> GetMarketings_new(Pagination pagination, string queryJson)
        {
            try
            {

                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                    t.id,
                	CONVERT ( VARCHAR ( 100 ), t.ApproverTime, 111 ) AS ApproverTime,
	                CONVERT ( VARCHAR ( 100 ), t.CreateTime, 111 ) AS CreateTime,
	                t.ContractNo,
	                a.ProjectName,
	                a.CustName,
	                t.ContractSubject,
	                t.DepartmentId,
	                a.FDepartmentId,
	                a.PDepartmentId,
	                py.PayType,
                    pcs.ProjectId as Settlement,
	                a.ProjectSource,
	                t.ContractStatus,
                    ReceivedFlag,
	                ( CASE t.ReceivedFlag WHEN 1 THEN '已归档' ELSE '未归档' END ) AS ReceivedFlagName,
	                cast( pb.BillingAmount as CHAR) AS BillingStatus,
                    pb.BillingAmount,
	                t.ContractAmount,
	                isnull( pc.Amount, 0 ) AS Amount,
	                t.ContractAmount- isnull( pc.Amount, 0 ) AS NotReceived,
	                pc.ReceiptDate,
	                pt.DepartmentId AS J_F_FullName,
	                pt.ApproachTime,
	                pt.ReportSubject,
	                pt.TaskStatus,
	                t.ContractType,
                    pt.ProjectResponsible,
                    a.PreparedPerson,
                    a.FollowPerson,t.EffectiveAmount,py.PaymentAmount,t.MainDepartmentId,t.MainAmount,t.SubDepartmentId,t.SubAmount,task.FinishTime
                ");
                var queryParam = queryJson.ToJObject();
                string start_date = "";
                string end_date = "";
                string approverTime = "";
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    var approve_time = queryParam["ApproverTime"].ToObject<List<string>>();
                    if (approve_time.Count > 0)
                    {
                        start_date = approve_time[0];
                        end_date = approve_time[1].ToDate().AddDays(1).ToString();
                        approverTime = " AND ( ApproverTime >= '" + start_date + "' AND ApproverTime < '" + end_date + "' )";
                    }
                }
                string createTime = "";
                if (!queryParam["CreateTime"].IsEmpty())
                {
                    var create_time = queryParam["CreateTime"].ToObject<List<string>>();
                    if (create_time.Count > 0)
                    {
                        string create_time_start_date = create_time[0];
                        string create_time_end_date = create_time[1].ToDate().AddDays(1).ToString();
                        createTime = " AND ( CreateTime >= '" + create_time_start_date + "' AND CreateTime < '" + create_time_end_date + "' )";
                    }
                }
                string sql = MarketingSql(approverTime, createTime, queryParam);
                strSql.Append(sql);

                // 虚拟参数
                var dp = new DynamicParameters(new { });
                strSql.Append("  WHERE t.ProjectId is not null ");
                if (!start_date.IsEmpty() && !end_date.IsEmpty())
                {
                    //DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                    //DateTime end = DateTime.Parse(queryParam["EndTime"].ToString());
                    //dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    //dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    //strSql.Append(" AND ( t.ApproverTime >= '" + start_date + "' AND t.ApproverTime < '" + end_date + "' ) ");
                }

                if (!queryParam["FinishTime"].IsEmpty())
                {
                    var finish_time = queryParam["FinishTime"].ToObject<List<string>>();
                    if (finish_time.Count > 0)
                    {
                        string finish_start_date = finish_time[0];
                        string finish_end_date = finish_time[1].ToDate().AddDays(1).ToString();
                        createTime = " AND ( task.FinishTime >= '" + finish_start_date + "' AND task.FinishTime < '" + finish_end_date + "' )";
                        strSql.Append(createTime);
                    }
                    //strSql.Append(string.Format(" AND ( task.FinishTime like '%{0}%' )", queryParam["FinishTime"].ToDate().ToString("yyyy-MM-dd")));
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%'  or  t.ContractNo like'%{0}%'  )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                //if (!queryParam["ApproverTime"].IsEmpty())
                //{
                //    var approve_time = queryParam["ApproverTime"].ToObject<List<string>>();
                //    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                //}
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                string receiptDateRange = "";
                if (!queryParam["ReceiptDateRange"].IsEmpty())
                {
                    var receipt_DateRange = queryParam["ReceiptDateRange"].ToObject<List<string>>();
                    if (receipt_DateRange.Count > 0)
                    {
                        string receipt_time_start_date = receipt_DateRange[0];
                        string receipt_time_end_date = receipt_DateRange[1].ToDate().AddDays(1).ToString();
                        receiptDateRange = " AND ( pc.ReceiptDate >= '" + receipt_time_start_date + "' AND pc.ReceiptDate < '" + receipt_time_end_date + "' )";
                        strSql.Append(receiptDateRange);
                    }
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( py.PaymentAmount = '{0}'  )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId = '{0}' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<MarketingEntity>(strSql.ToString(), dp, pagination);
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
        } /// <summary>
          /// 营销台账列表(部门)
          /// </summary>
          /// <param name="pagination"></param>
          /// <param name="queryJson"></param>
          /// <returns></returns>
        public IEnumerable<MarketingEntity> GetMarketings_new1(Pagination pagination, string queryJson, string dep)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
a.Id,
                	CONVERT ( VARCHAR ( 100 ), t.ApproverTime, 111 ) AS ApproverTime,
	                CONVERT ( VARCHAR ( 100 ), t.CreateTime, 111 ) AS CreateTime,
	                t.ContractNo,
	                a.ProjectName,
	                a.CustName,
	                t.ContractSubject,
	                t.DepartmentId,
	                a.FDepartmentId,
	                a.PDepartmentId,
	       
	                a.ProjectSource,
	                t.ContractStatus,
                    ReceivedFlag,
	                ( CASE t.ReceivedFlag WHEN 1 THEN '已归档' ELSE '未归档' END ) AS ReceivedFlagName,
	                cast( pb.BillingAmount as CHAR) AS BillingStatus,
                    pb.BillingAmount,
	                t.ContractAmount,
	                isnull( pc.Amount, 0 ) AS Amount,
	                t.ContractAmount- isnull( pc.Amount, 0 ) AS NotReceived,
	                pc.ReceiptDate,
	                pt.DepartmentId AS J_F_FullName,
	                pt.ApproachTime,
	                pt.ReportSubject,
	                pt.TaskStatus,
	                t.ContractType,
pt.ProjectResponsible,
a.PreparedPerson,
a.FollowPerson,t.EffectiveAmount,py.PaymentAmount,t.MainDepartmentId,t.MainAmount,t.SubDepartmentId,t.SubAmount
                ");
                var queryParam = queryJson.ToJObject();
                string start_date = "";
                string end_date = "";
                string approverTime = "";
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    var approve_time = queryParam["ApproverTime"].ToObject<List<string>>();
                    if (approve_time.Count > 0)
                    {
                        start_date = approve_time[0];
                        end_date = approve_time[1].ToDate().AddDays(1).ToString();
                        approverTime = " AND ( ApproverTime >= '" + start_date + "' AND ApproverTime < '" + end_date + "' )";
                    }
                }
                string createTime = "";
                if (!queryParam["CreateTime"].IsEmpty())
                {
                    var create_time = queryParam["CreateTime"].ToObject<List<string>>();
                    if (create_time.Count > 0)
                    {
                        string create_time_start_date = create_time[0];
                        string create_time_end_date = create_time[1].ToDate().AddDays(1).ToString();
                        createTime = " AND ( CreateTime >= '" + create_time_start_date + "' AND CreateTime < '" + create_time_end_date + "' )";
                    }
                }
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                string sql = MarketingSql(approverTime, createTime, queryParam);
                strSql.Append(sql);

                strSql.Append("  WHERE t.ProjectId is not null ");
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                    DateTime end = DateTime.Parse(queryParam["EndTime"].ToString());
                    strSql.Append(" AND ( t.ApproverTime >= '" + start + "' AND t.ApproverTime <= '" + end + "' ) ");
                }
                string receiptDateRange = "";
                if (!queryParam["ReceiptDateRange"].IsEmpty())
                {
                    var receipt_DateRange = queryParam["ReceiptDateRange"].ToObject<List<string>>();
                    if (receipt_DateRange.Count > 0)
                    {
                        string receipt_time_start_date = receipt_DateRange[0];
                        string receipt_time_end_date = receipt_DateRange[1].ToDate().AddDays(1).ToString();
                        receiptDateRange = " AND ( pc.ReceiptDate >= '" + receipt_time_start_date + "' AND pc.ReceiptDate < '" + receipt_time_end_date + "' )";
                        strSql.Append(receiptDateRange);
                    }
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%'  or  t.ContractNo like'%{0}%'  )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                //if (!queryParam["ApproverTime"].IsEmpty())
                //{
                //    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                //}
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( py.PaymentAmount like '%{0}%' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId = '{0}' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }
                strSql.Append(string.Format(" AND ( " + dep + " )"));
                return this.BaseRepository("learunOAWFForm").FindList<MarketingEntity>(strSql.ToString(), dp, pagination);
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
        /// 成本金额添加修改
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void CapitalAmountSaveForm(decimal costAmount, string yearMonth)
        {
            try
            {
                CapitalAmountEntity entity = getCapitalAmountByYearMonth(yearMonth);
                if (entity != null)
                {
                    if (entity.CostAmount != costAmount)
                    {
                        entity.Modify(entity.Id);
                        entity.CostAmount = costAmount;
                        this.BaseRepository("learunOAWFForm").Update(entity);
                    }
                }
                else
                {
                    CapitalAmountEntity new_entity = new CapitalAmountEntity();
                    new_entity.Create();
                    new_entity.CostAmount = costAmount;
                    new_entity.Yearyear = yearMonth;
                    this.BaseRepository("learunOAWFForm").Insert(new_entity);
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
        } /// <summary>
          /// 成本金额添加修改
          /// </summary>
          /// <param name="keyValue"></param>
          /// <param name="entity"></param>
        public void CapitalAmountSaveForm1(decimal? costAmount, string yearMonth)
        {
            try
            {
                CapitalAmountEntity entity = getCapitalAmountByYearMonth(yearMonth);
                if (entity != null)
                {
                    if (entity.CostAmount != costAmount)
                    {
                        entity.Modify(entity.Id);
                        entity.CostAmount = costAmount;
                        this.BaseRepository("learunOAWFForm").Update(entity);
                    }
                }
                else
                {
                    CapitalAmountEntity new_entity = new CapitalAmountEntity();
                    new_entity.Create();
                    new_entity.CostAmount = costAmount;
                    new_entity.Yearyear = yearMonth;
                    this.BaseRepository("learunOAWFForm").Insert(new_entity);
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
        /// 资金台账列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<CapitalDepartmentId> GetCapitalDepartmentIdList(string start, string end)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
 t.ContractAmount,t.ContractType,t.Id,t.EffectiveAmount,t.DepartmentId,a.ProjectSource,pc.ReceiptDate,pc.Amount,pt.MainAmount,pt.SubAmount,pt.TaskDepartmentId,pt.SubDepartmentId,pt.MainDepartmentId,pt.DepartmentId as TDepartmentId from (SELECT ProjectId,MIN(CreateTime) as CreateTime,DepartmentId,sum(ContractAmount) as ContractAmount,ContractType,MAX(EffectiveAmount) AS EffectiveAmount,MAX(Id) AS Id  FROM ProjectContract   
                 WHERE ContractStatus=4 AND ContractType=1  group by  ProjectId,ContractType, DepartmentId) t
                 INNER JOIN Project a ON a.Id = t.ProjectId 
	             INNER JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
	             LEFT JOIN ( select a.* from ProjectTask a inner join (SELECT ProjectId, MAX(CreateTime) as CreateTime, MAX(MainAmount) as MainAmount, MAX(SubAmount) as SubAmount, MAX(DepartmentId) as DepartmentId, MAX(TaskDepartmentId) as TaskDepartmentId FROM ProjectTask group by ProjectId) b 
                    on a.ProjectId = b.ProjectId and a.CreateTime = b.CreateTime where DeleteFlag=0 or DeleteFlag is NULL ) pt ON pt.ProjectId = a.id               
	                LEFT JOIN (select ProjectId, sum(BillingAmount) as BillingAmount from ProjectBilling where BillingStatus = 7 and BillingType != '3' group by ProjectId) pb ON pb.ProjectId = a.Id 
                    left join (select SUM(PaymentAmount) as PaymentAmount,MAX(ProjectId) as ProjectId,MAX(PayType) as PayType from ProjectPayment where PayType<>3 group by ProjectId ) py on a.Id=py.ProjectId
                ");
                strSql.Append(" where  ( pc.ReceiptDate >= '" + start + "' AND pc.ReceiptDate <'" + end + "' ) ");


                //if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                //{
                //    DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                //    DateTime end = DateTime.Parse(queryParam["EndTime"].ToString());
                //    strSql.Append(@" FROM
                //    (SELECT ProjectId,max(MainContract) as MainContract,max(ApproverTime) as ApproverTime,MIN(CreateTime) as CreateTime,max(ContractNo) as ContractNo,
                //    max(ContractSubject) as ContractSubject,DepartmentId,ContractStatus,MIN(ReceivedFlag) as ReceivedFlag,
                //    sum(ContractAmount) as ContractAmount,ContractType,MAX(EffectiveAmount) AS EffectiveAmount  FROM ProjectContract   
                //    WHERE ContractStatus=4 AND ContractType=1 AND ( ApproverTime >= '" + start + "' AND ApproverTime <= '" + end + "' ) group by ContractStatus, ProjectId, ContractType, DepartmentId) t "
                //    + @" INNER JOIN Project a ON a.Id = t.ProjectId 
                // LEFT JOIN adms706.dbo.lr_base_user u ON u.F_UserId = a.FollowPerson
                // LEFT JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
                // LEFT JOIN ( select a.* from ProjectTask a inner join (SELECT ProjectId, MAX(CreateTime) as CreateTime FROM ProjectTask group by ProjectId) b 
                //    on a.ProjectId = b.ProjectId and a.CreateTime = b.CreateTime where DeleteFlag=0 or DeleteFlag is NULL ) pt ON pt.ProjectId = a.id
                // LEFT JOIN adms706.dbo.lr_base_user u2 ON pt.ProjectResponsible= u2.F_UserId
                // LEFT JOIN (select ProjectId, sum(BillingAmount) as BillingAmount from ProjectBilling where BillingStatus = 7 group by ProjectId) pb ON pb.ProjectId = a.Id 
                //    left join (select SUM(PaymentAmount) as PaymentAmount,MAX(ProjectId) as ProjectId,MAX(PayType) as PayType from ProjectPayment where PayType<>3 group by ProjectId ) py on a.Id=py.ProjectId");

                //}


                return this.BaseRepository("learunOAWFForm").FindList<CapitalDepartmentId>(strSql.ToString());
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
        /// 资金台账列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<CapitalDepartmentId> GetCapitalDepartmentIdListDepartmentId(string cYYYY, string start, string end, string DepartmentId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
t.ContractAmount,t.DepartmentId,a.ProjectSource,t.ContractType,t.EffectiveAmount,t.ContractAmount,pc.Amount,t.ApproverTime,pc.ReceiptDate,a.Id as pid
FROM
                    (SELECT ProjectId,max(MainContract) as MainContract,max(ApproverTime) as ApproverTime,MIN(CreateTime) as CreateTime,
                    max(ContractSubject) as ContractSubject,DepartmentId,ContractStatus,
                    sum(ContractAmount) as ContractAmount,ContractType,MAX(EffectiveAmount) AS EffectiveAmount  FROM ProjectContract   
                    WHERE ContractStatus=4 AND ContractType=1  group by ContractStatus, ProjectId, ContractType, DepartmentId) t 
                     INNER JOIN Project a ON a.Id = t.ProjectId 
	                LEFT JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
                    left join (select SUM(PaymentAmount) as PaymentAmount,MAX(ProjectId) as ProjectId,MAX(PayType) as PayType from ProjectPayment where PayType<>3 and PaymentStatus<>11 group by ProjectId ) py on a.Id=py.ProjectId
                ");
                strSql.Append(" where  a.ProjectSource<>3 and t.ApproverTime >= '" + cYYYY + "' and t.ApproverTime <'" + end + "' and   pc.ReceiptDate >= '" + start + "' AND pc.ReceiptDate < '" + end + "'");
                if (DepartmentId != null)
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId = '" + DepartmentId + "')", DepartmentId.ToString()));
                }



                return this.BaseRepository("learunOAWFForm").FindList<CapitalDepartmentId>(strSql.ToString());
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
        /// 资金台账列表去年
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<CapitalDepartmentId> GetCapitalDepartmentIdListDepartmentId1(string cYYYY, string start, string end, string DepartmentId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
t.ContractAmount,t.DepartmentId,a.ProjectSource,t.ContractType,t.EffectiveAmount,t.ContractAmount,pc.Amount,t.ApproverTime,pc.ReceiptDate,a.Id as pid,py.PaymentAmount
FROM
                    (SELECT ProjectId,max(MainContract) as MainContract,max(ApproverTime) as ApproverTime,MIN(CreateTime) as CreateTime,
                    max(ContractSubject) as ContractSubject,DepartmentId,ContractStatus,
                    sum(ContractAmount) as ContractAmount,ContractType,MAX(EffectiveAmount) AS EffectiveAmount  FROM ProjectContract   
                    WHERE ContractStatus=4 AND ContractType=1  group by ContractStatus, ProjectId, ContractType, DepartmentId) t 
                     INNER JOIN Project a ON a.Id = t.ProjectId 
	                LEFT JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
                    left join (select SUM(PaymentAmount) as PaymentAmount,MAX(ProjectId) as ProjectId,MAX(PayType) as PayType from ProjectPayment where PayType<>3 and PaymentStatus<>11 group by ProjectId ) py on a.Id=py.ProjectId
                ");
                strSql.Append(" where  a.ProjectSource<>3 and   t.ApproverTime < '" + cYYYY + "' AND pc.ReceiptDate >= '" + start + "' AND  pc.ReceiptDate < '" + end + "'");
                //               strSql.Append("SELECT ");
                //               strSql.Append(@"
                //t.ContractAmount,t.CreateTime,t.ContractType,t.ApproverTime,t.EffectiveAmount,t.DepartmentId,a.ProjectSource,pc.ReceiptDate,pc.Amount,pt.MainAmount,pt.SubAmount,pt.TaskDepartmentId,pt.SubDepartmentId,pt.MainDepartmentId,pt.DepartmentId as TDepartmentId from (SELECT ProjectId,MIN(CreateTime) as CreateTime,DepartmentId,sum(ContractAmount) as ContractAmount,ContractType,MAX(EffectiveAmount) AS EffectiveAmount,MAX(ApproverTime) AS ApproverTime  FROM ProjectContract   
                //                WHERE ContractStatus=4 AND ContractType=1  group by  ProjectId,ContractType, DepartmentId) t
                //                INNER JOIN Project a ON a.Id = t.ProjectId 
                //             INNER JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
                //             LEFT JOIN ( select a.* from ProjectTask a inner join (SELECT ProjectId, MAX(CreateTime) as CreateTime, MAX(MainAmount) as MainAmount, MAX(SubAmount) as SubAmount, MAX(DepartmentId) as DepartmentId, MAX(TaskDepartmentId) as TaskDepartmentId FROM ProjectTask group by ProjectId) b 
                //                   on a.ProjectId = b.ProjectId and a.CreateTime = b.CreateTime where DeleteFlag=0 or DeleteFlag is NULL ) pt ON pt.ProjectId = a.id               
                //                LEFT JOIN (select ProjectId, sum(BillingAmount) as BillingAmount from ProjectBilling where BillingStatus = 7 group by ProjectId) pb ON pb.ProjectId = a.Id 
                //                   left join (select SUM(PaymentAmount) as PaymentAmount,MAX(ProjectId) as ProjectId,MAX(PayType) as PayType from ProjectPayment where PayType<>3 group by ProjectId ) py on a.Id=py.ProjectId
                //               ");
                //               strSql.Append(" where a.ProjectSource<>3 and t.CreateTime<'" + cYYYY+ "'and t.ApproverTime >= '" + start + "' AND t.ApproverTime <'" + end + "'");
                if (DepartmentId != null)
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId = '" + DepartmentId + "')", DepartmentId.ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<CapitalDepartmentId>(strSql.ToString());
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
        /// 资金台账列表去年部门
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<CapitalDepartmentId> GetCapitalDepartmentIdListDepartmentIddep1(string cYYYY, string start, string end, string DepartmentId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
t.ContractAmount,t.DepartmentId,a.ProjectSource,t.ContractType,t.EffectiveAmount,t.ContractAmount,pc.Amount,t.ApproverTime,pc.ReceiptDate,a.Id as pid,py.PaymentAmount
FROM
                    (SELECT ProjectId,max(MainContract) as MainContract,max(ApproverTime) as ApproverTime,MIN(CreateTime) as CreateTime,
                    max(ContractSubject) as ContractSubject,DepartmentId,ContractStatus,
                    sum(ContractAmount) as ContractAmount,ContractType,MAX(EffectiveAmount) AS EffectiveAmount  FROM ProjectContract   
                    WHERE ContractStatus=4 AND ContractType=1  group by ContractStatus, ProjectId, ContractType, DepartmentId) t 
                     INNER JOIN Project a ON a.Id = t.ProjectId 
	                LEFT JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
                    left join (select SUM(PaymentAmount) as PaymentAmount,MAX(ProjectId) as ProjectId,MAX(PayType) as PayType from ProjectPayment where PayType<>3 group by ProjectId ) py on a.Id=py.ProjectId
                ");
                strSql.Append(" where  a.ProjectSource<>3 and   t.ApproverTime < '" + cYYYY + "' AND pc.ReceiptDate >= '" + start + "' AND  pc.ReceiptDate < '" + end + "'");
                //               strSql.Append("SELECT ");
                //               strSql.Append(@"
                //t.ContractAmount,t.CreateTime,t.ContractType,t.ApproverTime,t.EffectiveAmount,t.DepartmentId,a.ProjectSource,pc.ReceiptDate,pc.Amount,pt.MainAmount,pt.SubAmount,pt.TaskDepartmentId,pt.SubDepartmentId,pt.MainDepartmentId,pt.DepartmentId as TDepartmentId from (SELECT ProjectId,MIN(CreateTime) as CreateTime,DepartmentId,sum(ContractAmount) as ContractAmount,ContractType,MAX(EffectiveAmount) AS EffectiveAmount,MAX(ApproverTime) AS ApproverTime  FROM ProjectContract   
                //                WHERE ContractStatus=4 AND ContractType=1  group by  ProjectId,ContractType, DepartmentId) t
                //                INNER JOIN Project a ON a.Id = t.ProjectId 
                //             INNER JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
                //             LEFT JOIN ( select a.* from ProjectTask a inner join (SELECT ProjectId, MAX(CreateTime) as CreateTime, MAX(MainAmount) as MainAmount, MAX(SubAmount) as SubAmount, MAX(DepartmentId) as DepartmentId, MAX(TaskDepartmentId) as TaskDepartmentId FROM ProjectTask group by ProjectId) b 
                //                   on a.ProjectId = b.ProjectId and a.CreateTime = b.CreateTime where DeleteFlag=0 or DeleteFlag is NULL ) pt ON pt.ProjectId = a.id               
                //                LEFT JOIN (select ProjectId, sum(BillingAmount) as BillingAmount from ProjectBilling where BillingStatus = 7 group by ProjectId) pb ON pb.ProjectId = a.Id 
                //                   left join (select SUM(PaymentAmount) as PaymentAmount,MAX(ProjectId) as ProjectId,MAX(PayType) as PayType from ProjectPayment where PayType<>3 group by ProjectId ) py on a.Id=py.ProjectId
                //               ");
                //               strSql.Append(" where a.ProjectSource<>3 and t.CreateTime<'" + cYYYY+ "'and t.ApproverTime >= '" + start + "' AND t.ApproverTime <'" + end + "'");
                if (DepartmentId != null)
                {
                    strSql.Append(string.Format(" AND ( " + DepartmentId + " )"));
                }
                return this.BaseRepository("learunOAWFForm").FindList<CapitalDepartmentId>(strSql.ToString());
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
        /// 多部门营销台账列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<MarketingEntity> GetMarketings_newDepartmentId(Pagination pagination, string queryJson, string dep)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                	CONVERT ( VARCHAR ( 100 ), t.ApproverTime, 111 ) AS ApproverTime,
	                CONVERT ( VARCHAR ( 100 ), t.CreateTime, 111 ) AS CreateTime,
	                t.ContractNo,
	                a.ProjectName,
	                a.CustName,
	                t.ContractSubject,
	                t.DepartmentId,
	                a.FDepartmentId,
	                a.PDepartmentId,
	                u.F_RealName,
	                a.ProjectSource,
	                t.ContractStatus,
	                ReceivedFlag,
	                ( CASE t.ReceivedFlag WHEN 1 THEN '已归档' ELSE '未归档' END ) AS ReceivedFlagName,
	                cast( pb.BillingAmount as CHAR) AS BillingStatus,
                    pb.BillingAmount,
	                t.ContractAmount,
	                isnull( pc.Amount, 0 ) AS Amount,
	                t.ContractAmount- isnull( pc.Amount, 0 ) AS NotReceived,
	                pc.ReceiptDate,
	                pt.DepartmentId AS J_F_FullName,
	                pt.ApproachTime,
	                pt.ReportSubject,
	                pt.TaskStatus,
	                t.ContractType,
	                u2.F_RealName AS P_F_RealName,
                    a.PreparedPerson,
                    a.FollowPerson,t.EffectiveAmount,py.PaymentAmount
                ");


                var queryParam = queryJson.ToJObject();
                string start_date = "";
                string end_date = "";
                string approverTime = "";
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    var approve_time = queryParam["ApproverTime"].ToObject<List<string>>();
                    if (approve_time.Count > 0)
                    {
                        start_date = approve_time[0];
                        end_date = approve_time[1].ToDate().AddDays(1).ToString();
                        approverTime = " AND ( ApproverTime >= '" + start_date + "' AND ApproverTime < '" + end_date + "' )";
                    }
                }
                string createTime = "";
                if (!queryParam["CreateTime"].IsEmpty())
                {
                    var create_time = queryParam["CreateTime"].ToObject<List<string>>();
                    if (create_time.Count > 0)
                    {
                        string create_time_start_date = create_time[0];
                        string create_time_end_date = create_time[1].ToDate().AddDays(1).ToString();
                        createTime = " AND ( CreateTime >= '" + create_time_start_date + "' AND CreateTime < '" + create_time_end_date + "' )";
                    }
                }
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                string sql = MarketingSql(approverTime, createTime, queryParam);
                strSql.Append(sql);

                strSql.Append("  WHERE t.ProjectId is not null ");
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                    DateTime end = DateTime.Parse(queryParam["EndTime"].ToString());
                    //dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    //dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.ApproverTime >= '" + start + "' AND t.ApproverTime <= '" + end + "' ) ");
                }

                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%'  or  t.ContractNo like'%{0}%'  )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                //if (!queryParam["ApproverTime"].IsEmpty())
                //{
                //    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                //}
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                string receiptDateRange = "";
                if (!queryParam["ReceiptDateRange"].IsEmpty())
                {
                    var receipt_DateRange = queryParam["ReceiptDateRange"].ToObject<List<string>>();
                    if (receipt_DateRange.Count > 0)
                    {
                        string receipt_time_start_date = receipt_DateRange[0];
                        string receipt_time_end_date = receipt_DateRange[1].ToDate().AddDays(1).ToString();
                        receiptDateRange = " AND ( pc.ReceiptDate >= '" + receipt_time_start_date + "' AND pc.ReceiptDate < '" + receipt_time_end_date + "' )";
                        strSql.Append(receiptDateRange);
                    }
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount like '%{0}%' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId = '{0}' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }
                strSql.Append(string.Format(" AND ( " + dep + " )"));
                return this.BaseRepository("learunOAWFForm").FindList<MarketingEntity>(strSql.ToString(), dp, pagination);
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
        /// 合作伙伴的营销台账
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<MarketingEntity> GetMarketings_newHZ(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                	CONVERT ( VARCHAR ( 100 ), t.ApproverTime, 111 ) AS ApproverTime,
	                CONVERT ( VARCHAR ( 100 ), t.CreateTime, 111 ) AS CreateTime,
	                t.ContractNo,
	                a.ProjectName,
	                a.CustName,
	                t.ContractSubject,
	                t.DepartmentId as DepartmentIdCA,
	                a.FDepartmentId,
	                a.PDepartmentId,
	              
	                a.ProjectSource,
	                t.ContractStatus,
	                ReceivedFlag,
	                ( CASE t.ReceivedFlag WHEN 1 THEN '已归档' ELSE '未归档' END ) AS ReceivedFlagName,
	                cast( pb.BillingAmount as CHAR) AS BillingStatus,
                    pb.BillingAmount,
	                t.ContractAmount,
	                isnull( pc.Amount, 0 ) AS Amount,
	                t.ContractAmount- isnull( pc.Amount, 0 ) AS NotReceived,
	                pc.ReceiptDate,
	                pt.DepartmentId AS J_F_FullName,
	                pt.ApproachTime,
	                pt.ReportSubject,
	                pt.TaskStatus,
	                t.ContractType,
	               pt.ProjectResponsible,
a.PreparedPerson,
a.FollowPerson
                ");
                var queryParam = queryJson.ToJObject();
                string start_date = "";
                string end_date = "";
                string approverTime = "";
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    var approve_time = queryParam["ApproverTime"].ToObject<List<string>>();
                    if (approve_time.Count > 0)
                    {
                        start_date = approve_time[0];
                        end_date = approve_time[1].ToDate().AddDays(1).ToString();
                        approverTime = " AND ( ApproverTime >= '" + start_date + "' AND ApproverTime < '" + end_date + "' )";
                    }
                }
                string createTime = "";
                if (!queryParam["CreateTime"].IsEmpty())
                {
                    var create_time = queryParam["CreateTime"].ToObject<List<string>>();
                    if (create_time.Count > 0)
                    {
                        string create_time_start_date = create_time[0];
                        string create_time_end_date = create_time[1].ToDate().AddDays(1).ToString();
                        createTime = " AND ( CreateTime >= '" + create_time_start_date + "' AND CreateTime < '" + create_time_end_date + "' )";
                    }
                }
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                string sql = MarketingSql(approverTime, createTime, queryParam);
                strSql.Append(sql);

                strSql.Append("  WHERE t.ProjectId is not null ");

                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                    DateTime end = DateTime.Parse(queryParam["EndTime"].ToString());
                    strSql.Append(" AND ( t.ApproverTime >= '" + start + "' AND t.ApproverTime <= '" + end + "' ) ");
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%'  or  t.ContractNo like'%{0}%'  )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName  like  '%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                //if (!queryParam["ApproverTime"].IsEmpty())
                //{
                //    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                //}
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                string receiptDateRange = "";
                if (!queryParam["ReceiptDateRange"].IsEmpty())
                {
                    var receipt_DateRange = queryParam["ReceiptDateRange"].ToObject<List<string>>();
                    if (receipt_DateRange.Count > 0)
                    {
                        string receipt_time_start_date = receipt_DateRange[0];
                        string receipt_time_end_date = receipt_DateRange[1].ToDate().AddDays(1).ToString();
                        receiptDateRange = " AND ( pc.ReceiptDate >= '" + receipt_time_start_date + "' AND pc.ReceiptDate < '" + receipt_time_end_date + "' )";
                        strSql.Append(receiptDateRange);
                    }
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount  = '{0}' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId = '{0}' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<MarketingEntity>(strSql.ToString(), dp, pagination);
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
        /*/// <summary>
        /// 营销报表 
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns>9.3</returns>
        public IEnumerable<MarketingVo> GetMarketingsList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");       
                strSql.Append(@" * FROM  Project a where 1=1"); 
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( a.CreateTime >= @startTime AND a.CreateTime <= @endTime ) ");
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%'  or  t.ContractNo like'%{0}%'  )", queryParam["keyword"].ToString()));
                }             
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                } 
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
               
                
                
                return this.BaseRepository("learunOAWFForm").FindList<MarketingVo>(strSql.ToString(), dp, pagination);
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
        }*/
        public IEnumerable<SettleAccountsEntity> GetSettleAccounts(string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"t.ContractNo,
                    pc.id as PC,
                    t.id as Tid,
                    CONVERT(varchar(100), t.ApproverTime, 111) as ApproverTime,
                    CONVERT(varchar(100), t.CreateTime, 111) as CreateTime,
	                a.ProjectName,
	                a.CustName,
	                t.ContractSubject,
	                t.ContractStatus,
	                a.ProjectSource,
	                u3.F_RealName as P_F_RealName,
	                isnull(t.ContractAmount,0) as ContractAmount,
					isnull(pb.BillingAmount,0) as BillingAmount,
					isnull(pc.Amount,0) as Amount,
                    pc.ReceiptDate,
	                t.ContractAmount-isnull(pc.Amount,0) as NotReceived,
	                t.DepartmentId,
				    a.FDepartmentId,
				    a.PDepartmentId,
	                u.F_RealName as M_F_RealName,
	                pt.DepartmentId as J_F_FullName,
	                u2.F_RealName as J_F_RealName,
	                pt.TaskStatus,
					isnull(pp.PaymentAmount,0) as PaymentAmount,
                    (case t.ReceivedFlag
	            when 1 then '已归档'
	            else '未归档'
	            end) as ReceivedFlag,
	                pt.Remark
                ");
                strSql.Append(@"  FROM ProjectContract t 
	                inner join  Project a  on a.Id=t.ProjectId
	                left join adms706.dbo.lr_base_user u on u.F_UserId=a.FollowPerson
	                left join ProjectPayCollection pc on pc.ProjectId=a.Id
	                left join ProjectTask pt on pt.ProjectId=a.id left join adms706.dbo.lr_base_user u2 on pt.ProjectResponsible=u2.F_UserId
	                left join adms706.dbo.lr_base_user u3 on u3.F_UserId=a.PreparedPerson
	                left join ProjectBilling pb on pb.ProjectId=a.Id
	                left join ProjectContract t2 on t2.ProjectId=a.Id and t2.ContractType=2
	                left join ProjectPayment pp on pp.ProjectId=a.Id and pp.PayType<>3
	                left join adms706.dbo.lr_base_department d on d.F_DepartmentId=a.DepartmentId");
                strSql.Append(" where (t.ContractStatus=4 AND t.ContractType=1) and (pt.DeleteFlag=0 or pt.DeleteFlag is NULL)");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.ApproverTime >= @startTime AND t.ApproverTime <= @endTime ) ");
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
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount like '%{0}%' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }

                strSql.Append(" ORDER BY t.ApproverTime desc");
                return this.BaseRepository("learunOAWFForm").FindList<SettleAccountsEntity>(strSql.ToString(), dp);
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
        public IEnumerable<SettleAccountsEntity> GetSettleAccounts_new(string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"t.ContractNo,
                    CONVERT(varchar(100), t.ApproverTime, 111) as ApproverTime,
                    CONVERT(varchar(100), t.CreateTime, 111) as CreateTime,
	                a.ProjectName,
	                a.CustName,
	                t.ContractSubject,
	                t.ContractStatus,
	                a.ProjectSource,
	                u3.F_RealName as P_F_RealName,
	                isnull(t.ContractAmount,0) as ContractAmount,
					isnull(pb.BillingAmount,0) as BillingAmount,
					isnull(pc.Amount,0) as Amount,
                    pc.ReceiptDate,
	                t.ContractAmount-isnull(pc.Amount,0) as NotReceived,
	                t.DepartmentId,
				    a.FDepartmentId,
				    a.PDepartmentId,
	                u.F_RealName as M_F_RealName,
	                pt.DepartmentId as J_F_FullName,
	                u2.F_RealName as J_F_RealName,
	                pt.TaskStatus,
					isnull(pp.PaymentAmount,0) as PaymentAmount,
                    (case t.ReceivedFlag
	            when 1 then '已归档'
	            else '未归档'
	            end) as ReceivedFlag,
	                pt.Remark,t.EffectiveAmount,py.PaymentAmount,pt.MainAmount,pt.SubAmount
                ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                string sql = SettleAccountSql(queryParam);
                strSql.Append(sql);
                // AND(ApproverTime >= '" + start + "' AND ApproverTime <= '" + end + "')

                strSql.Append(" where t.ProjectId is not null ");


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
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount like '%{0}%' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }

                strSql.Append(" ORDER BY t.ApproverTime desc");
                return this.BaseRepository("learunOAWFForm").FindList<SettleAccountsEntity>(strSql.ToString(), dp);
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
        /// 合作伙伴结算台账导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<SettleAccountsEntity> GetSettleAccounts_newHZ(string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"t.ContractNo,
                    CONVERT(varchar(100), t.ApproverTime, 111) as ApproverTime,
                    CONVERT(varchar(100), t.CreateTime, 111) as CreateTime,
	                a.ProjectName,
	                a.CustName,
	                t.ContractSubject,
	                t.ContractStatus,
	                a.ProjectSource,
	     a.PreparedPerson,
a.FollowPerson,
pt.ProjectResponsible,
	                isnull(t.ContractAmount,0) as ContractAmount,
					isnull(pb.BillingAmount,0) as BillingAmount,
					isnull(pc.Amount,0) as Amount,
                    pc.ReceiptDate,
	                t.ContractAmount-isnull(pc.Amount,0) as NotReceived,
	                t.DepartmentId as DepartmentIdCA,
				    a.FDepartmentId,
				    a.PDepartmentId,
	   
	                pt.DepartmentId as J_F_FullName,

	                pt.TaskStatus,
					isnull(pp.PaymentAmount,0) as PaymentAmount,
                    (case t.ReceivedFlag
	            when 1 then '已归档'
	            else '未归档'
	            end) as ReceivedFlag,
	                pt.Remark,t.ProjectId as cProjectId,t.Proportion
                ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                    DateTime end = DateTime.Parse(queryParam["EndTime"].ToString());
                    strSql.Append(@"  FROM (SELECT ProjectId,max(MainContract) as MainContract,max(ApproverTime) as ApproverTime,MIN(CreateTime) as CreateTime,                    STUFF((
                            SELECT ',' + ContractNo
                            FROM ProjectContract AS pci
                            WHERE pci.ProjectId = pco.ProjectId
                            FOR XML PATH(''), TYPE
                        ).value('.', 'NVARCHAR(MAX)'), 1, 1, '')  AS ContractNo,
                    max(ContractSubject) as ContractSubject,max(DepartmentId) as DepartmentId,max(ContractStatus) as ContractStatus,MIN(ReceivedFlag) as ReceivedFlag,
                    sum(ContractAmount) as ContractAmount,MAX(ContractType) as ContractType,MAX(Proportion) as Proportion FROM ProjectContract  as pco
                    WHERE ContractStatus=4 AND ContractType=1   and ( ApproverTime >= '" + start + "' AND ApproverTime <= '" + end + "' ) group by ProjectId) t "
                    + @" inner join  Project a  on a.Id=t.ProjectId and a.ProjectSource=3
	             
	                LEFT JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
	                LEFT JOIN ( select a.* from ProjectTask a inner join (SELECT ProjectId, MAX(CreateTime) as CreateTime FROM ProjectTask group by ProjectId) b 
                        on a.ProjectId = b.ProjectId and a.CreateTime = b.CreateTime where DeleteFlag=0 or DeleteFlag is NULL ) pt ON pt.ProjectId = a.id
                  
	                LEFT JOIN (select ProjectId, sum(BillingAmount) as BillingAmount from ProjectBilling where BillingStatus = 7 and BillingType != '3' group by ProjectId) pb ON pb.ProjectId = a.Id 
                    left join (select ProjectId,SUM(PaymentAmount) as PaymentAmount from ProjectPayment where PayType<>3 and PaymentStatus<>11 group by ProjectId) pp on pp.ProjectId=a.Id 
	               ");
                }
                else
                {
                    strSql.Append(@"  FROM (SELECT ProjectId,max(MainContract) as MainContract,max(ApproverTime) as ApproverTime,MIN(CreateTime) as CreateTime,                    STUFF((
                            SELECT ',' + ContractNo
                            FROM ProjectContract AS pci
                            WHERE pci.ProjectId = pco.ProjectId
                            FOR XML PATH(''), TYPE
                        ).value('.', 'NVARCHAR(MAX)'), 1, 1, '')  AS ContractNo,
                    max(ContractSubject) as ContractSubject,max(DepartmentId) as DepartmentId,max(ContractStatus) as ContractStatus,MIN(ReceivedFlag) as ReceivedFlag,
                    sum(ContractAmount) as ContractAmount,MAX(ContractType) as ContractType,MAX(Proportion) as Proportion FROM ProjectContract  as pco
                    WHERE ContractStatus=4 AND ContractType=1   group by ProjectId) t 
	                inner join  Project a  on a.Id=t.ProjectId  and a.ProjectSource=3
	                
	                LEFT JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
	                LEFT JOIN ( select a.* from ProjectTask a inner join (SELECT ProjectId, MAX(CreateTime) as CreateTime FROM ProjectTask group by ProjectId) b 
                        on a.ProjectId = b.ProjectId and a.CreateTime = b.CreateTime where DeleteFlag=0 or DeleteFlag is NULL ) pt ON pt.ProjectId = a.id
                   
	                LEFT JOIN (select ProjectId, sum(BillingAmount) as BillingAmount from ProjectBilling where BillingStatus = 7 and BillingType != '3' group by ProjectId) pb ON pb.ProjectId = a.Id 
                    left join (select ProjectId,SUM(PaymentAmount) as PaymentAmount from ProjectPayment where PayType<>3 and PaymentStatus<>11 group by ProjectId) pp on pp.ProjectId=a.Id ");
                }
                // AND(ApproverTime >= '" + start + "' AND ApproverTime <= '" + end + "')

                strSql.Append(" where t.ProjectId is not null ");


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
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName  like  '%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount = '{0}')", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }

                strSql.Append(" ORDER BY t.ApproverTime desc");
                return this.BaseRepository("learunOAWFForm").FindList<SettleAccountsEntity>(strSql.ToString(), dp);
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
        /// 导出结算
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<SettleAccountsEntity> GetSettleAccountsSum(string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"t.ContractNo,
                    CONVERT(varchar(100), t.ApproverTime, 111) as ApproverTime,
                    CONVERT(varchar(100), t.CreateTime, 111) as CreateTime,
	                a.ProjectName,
	                a.CustName,
	                t.ContractSubject,
	                t.ContractStatus,
	                a.ProjectSource,
	                u3.F_RealName as P_F_RealName,
	                isnull(t.ContractAmount,0) as ContractAmount,
					isnull(pb.BillingAmount,0) as BillingAmount,
					isnull(pc.Amount,0) as Amount,
                    pc.ReceiptDate,
	                t.ContractAmount-isnull(pc.Amount,0) as NotReceived,
	                t.DepartmentId,
				    a.FDepartmentId,
				    a.PDepartmentId,
	                u.F_RealName as M_F_RealName,
	                pt.DepartmentId as J_F_FullName,
	                u2.F_RealName as J_F_RealName,
	                pt.TaskStatus,
					isnull(pp.PaymentAmount,0) as PaymentAmount,
                    (case t.ReceivedFlag
	            when 1 then '已归档'
	            else '未归档'
	            end) as ReceivedFlag,
	                pt.Remark,
t.EffectiveAmount
                ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                    DateTime end = DateTime.Parse(queryParam["EndTime"].ToString());
                    strSql.Append(@"  FROM (select ContractSubject,DepartmentId,ContractNo,ReceivedFlag,ApproverTime,CreateTime,ProjectId,ContractStatus,ContractType,ContractAmount,EffectiveAmount FROM ProjectContract  
                    WHERE ContractStatus=4 AND ContractType=1  and ContractStatus<>7 and ContractStatus<>6 and ( ApproverTime >= '" + start + "' AND ApproverTime <= '" + end + "' ) ) t "
                    + @" inner join  Project a  on a.Id=t.ProjectId
	                left join adms706.dbo.lr_base_user u on u.F_UserId=a.FollowPerson
	                LEFT JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
	                LEFT JOIN ( select a.* from ProjectTask a inner join (SELECT ProjectId, MAX(CreateTime) as CreateTime FROM ProjectTask group by ProjectId) b 
                        on a.ProjectId = b.ProjectId and a.CreateTime = b.CreateTime where DeleteFlag=0 or DeleteFlag is NULL ) pt ON pt.ProjectId = a.id
                    left join adms706.dbo.lr_base_user u2 on pt.ProjectResponsible=u2.F_UserId
	                left join adms706.dbo.lr_base_user u3 on u3.F_UserId=a.PreparedPerson
	                LEFT JOIN (select ProjectId, sum(BillingAmount) as BillingAmount from ProjectBilling where BillingStatus = 7 and BillingType != '3' group by ProjectId) pb ON pb.ProjectId = a.Id 
                    left join (select ProjectId,SUM(PaymentAmount) as PaymentAmount from ProjectPayment where PayType<>3 group by ProjectId) pp on pp.ProjectId=a.Id 
	                left join adms706.dbo.lr_base_department d on d.F_DepartmentId=a.DepartmentId");
                }
                else
                {
                    strSql.Append(@"  FROM (select ContractSubject,DepartmentId,ContractNo,ReceivedFlag,ApproverTime,CreateTime,ProjectId,ContractStatus,ContractType,ContractAmount FROM ProjectContract  
                    WHERE ContractStatus=4 AND ContractType=1 and ContractStatus<>7 and ContractStatus<>6 ) t 
	                inner join  Project a  on a.Id=t.ProjectId
	                left join adms706.dbo.lr_base_user u on u.F_UserId=a.FollowPerson
	                LEFT JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
	                LEFT JOIN ( select a.* from ProjectTask a inner join (SELECT ProjectId, MAX(CreateTime) as CreateTime FROM ProjectTask group by ProjectId) b 
                        on a.ProjectId = b.ProjectId and a.CreateTime = b.CreateTime where DeleteFlag=0 or DeleteFlag is NULL ) pt ON pt.ProjectId = a.id
                    left join adms706.dbo.lr_base_user u2 on pt.ProjectResponsible=u2.F_UserId
	                left join adms706.dbo.lr_base_user u3 on u3.F_UserId=a.PreparedPerson
	                LEFT JOIN (select ProjectId, sum(BillingAmount) as BillingAmount from ProjectBilling where BillingStatus = 7 and BillingType != '3' group by ProjectId) pb ON pb.ProjectId = a.Id 
                    left join (select ProjectId,SUM(PaymentAmount) as PaymentAmount from ProjectPayment where PayType<>3 group by ProjectId) pp on pp.ProjectId=a.Id 
	                left join adms706.dbo.lr_base_department d on d.F_DepartmentId=a.DepartmentId");
                }
                // AND(ApproverTime >= '" + start + "' AND ApproverTime <= '" + end + "')

                strSql.Append(" where t.ProjectId is not null ");


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
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount like '%{0}%' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }

                strSql.Append(" ORDER BY t.ApproverTime desc");
                return this.BaseRepository("learunOAWFForm").FindList<SettleAccountsEntity>(strSql.ToString(), dp);
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
        } /// <summary>
          /// 多部门导出结算
          /// </summary>
          /// <param name="queryJson"></param>
          /// <returns></returns>
        public IEnumerable<SettleAccountsEntity> GetSettleAccountsSumDepartmentId(string queryJson, string dep)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"t.ContractNo,
                    CONVERT(varchar(100), t.ApproverTime, 111) as ApproverTime,
                    CONVERT(varchar(100), t.CreateTime, 111) as CreateTime,
	                a.ProjectName,
	                a.CustName,
	                t.ContractSubject,
	                t.ContractStatus,
	                a.ProjectSource,
	                u3.F_RealName as P_F_RealName,
	                isnull(t.ContractAmount,0) as ContractAmount,
					isnull(pb.BillingAmount,0) as BillingAmount,
					isnull(pc.Amount,0) as Amount,
                    pc.ReceiptDate,
	                t.ContractAmount-isnull(pc.Amount,0) as NotReceived,
	                t.DepartmentId,
				    a.FDepartmentId,
				    a.PDepartmentId,
	                u.F_RealName as M_F_RealName,
	                pt.DepartmentId as J_F_FullName,
	                u2.F_RealName as J_F_RealName,
	                pt.TaskStatus,
					isnull(pp.PaymentAmount,0) as PaymentAmount,
                    (case t.ReceivedFlag
	            when 1 then '已归档'
	            else '未归档'
	            end) as ReceivedFlag,
	                pt.Remark,
t.EffectiveAmount
                ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                    DateTime end = DateTime.Parse(queryParam["EndTime"].ToString());
                    strSql.Append(@"  FROM (select ContractSubject,DepartmentId,ContractNo,ReceivedFlag,ApproverTime,CreateTime,ProjectId,ContractStatus,ContractType,ContractAmount,EffectiveAmount FROM ProjectContract  
                    WHERE ContractStatus=4 AND ContractType=1  and ContractStatus<>7 and ContractStatus<>6 and ( ApproverTime >= '" + start + "' AND ApproverTime <= '" + end + "' ) ) t "
                    + @" inner join  Project a  on a.Id=t.ProjectId
	                left join adms706.dbo.lr_base_user u on u.F_UserId=a.FollowPerson
	                LEFT JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
	                LEFT JOIN ( select a.* from ProjectTask a inner join (SELECT ProjectId, MAX(CreateTime) as CreateTime FROM ProjectTask group by ProjectId) b 
                        on a.ProjectId = b.ProjectId and a.CreateTime = b.CreateTime where DeleteFlag=0 or DeleteFlag is NULL ) pt ON pt.ProjectId = a.id
                    left join adms706.dbo.lr_base_user u2 on pt.ProjectResponsible=u2.F_UserId
	                left join adms706.dbo.lr_base_user u3 on u3.F_UserId=a.PreparedPerson
	                LEFT JOIN (select ProjectId, sum(BillingAmount) as BillingAmount from ProjectBilling where BillingStatus = 7 and BillingType != '3' group by ProjectId) pb ON pb.ProjectId = a.Id 
                    left join (select ProjectId,SUM(PaymentAmount) as PaymentAmount from ProjectPayment where PayType<>3 group by ProjectId) pp on pp.ProjectId=a.Id 
	                left join adms706.dbo.lr_base_department d on d.F_DepartmentId=a.DepartmentId");
                }
                else
                {
                    strSql.Append(@"  FROM (select ContractSubject,DepartmentId,ContractNo,ReceivedFlag,ApproverTime,CreateTime,ProjectId,ContractStatus,ContractType,ContractAmount FROM ProjectContract  
                    WHERE ContractStatus=4 AND ContractType=1 and ContractStatus<>7 and ContractStatus<>6 ) t 
	                inner join  Project a  on a.Id=t.ProjectId
	                left join adms706.dbo.lr_base_user u on u.F_UserId=a.FollowPerson
	                LEFT JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
	                LEFT JOIN ( select a.* from ProjectTask a inner join (SELECT ProjectId, MAX(CreateTime) as CreateTime FROM ProjectTask group by ProjectId) b 
                        on a.ProjectId = b.ProjectId and a.CreateTime = b.CreateTime where DeleteFlag=0 or DeleteFlag is NULL ) pt ON pt.ProjectId = a.id
                    left join adms706.dbo.lr_base_user u2 on pt.ProjectResponsible=u2.F_UserId
	                left join adms706.dbo.lr_base_user u3 on u3.F_UserId=a.PreparedPerson
	                LEFT JOIN (select ProjectId, sum(BillingAmount) as BillingAmount from ProjectBilling where BillingStatus = 7 and BillingType != '3' group by ProjectId) pb ON pb.ProjectId = a.Id 
                    left join (select ProjectId,SUM(PaymentAmount) as PaymentAmount from ProjectPayment where PayType<>3 group by ProjectId) pp on pp.ProjectId=a.Id 
	                left join adms706.dbo.lr_base_department d on d.F_DepartmentId=a.DepartmentId");
                }
                // AND(ApproverTime >= '" + start + "' AND ApproverTime <= '" + end + "')

                strSql.Append(" where t.ProjectId is not null ");


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
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount like '%{0}%' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }

                strSql.Append(" ORDER BY t.ApproverTime desc");
                return this.BaseRepository("learunOAWFForm").FindList<SettleAccountsEntity>(strSql.ToString(), dp);
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
        public IEnumerable<SettleAccountsEntity> GetSettleAccountsSum_new(string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"t.ContractNo,
                    CONVERT(varchar(100), t.ApproverTime, 111) as ApproverTime,
                    CONVERT(varchar(100), t.CreateTime, 111) as CreateTime,
	                a.ProjectName,
	                a.CustName,
	                t.ContractSubject,
	                t.ContractStatus,
	                a.ProjectSource,
	               
	                isnull(t.ContractAmount,0) as ContractAmount,
					isnull(pb.BillingAmount,0) as BillingAmount,
					isnull(pc.Amount,0) as Amount,
                    pc.ReceiptDate,
	                t.ContractAmount-isnull(pc.Amount,0) as NotReceived,
	                t.DepartmentId,
				    a.FDepartmentId,
				    a.PDepartmentId,
	
	                pt.DepartmentId as J_F_FullName,
	             
	                pt.TaskStatus,
					isnull(pp.PaymentAmount,0) as PaymentAmount,
                    (case t.ReceivedFlag
	            when 1 then '已归档'
	            else '未归档'
	            end) as ReceivedFlag,
	                pt.Remark,
t.EffectiveAmount,pt.ProjectResponsible,a.PreparedPerson,a.FollowPerson
                ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                    DateTime end = DateTime.Parse(queryParam["EndTime"].ToString());
                    strSql.Append(@"  FROM (SELECT ProjectId,max(MainContract) as MainContract,max(ApproverTime) as ApproverTime,MIN(CreateTime) as CreateTime,                    STUFF((
                            SELECT ',' + ContractNo
                            FROM ProjectContract AS pci
                            WHERE pci.ProjectId = pco.ProjectId
                            FOR XML PATH(''), TYPE
                        ).value('.', 'NVARCHAR(MAX)'), 1, 1, '')  AS ContractNo,
                    max(ContractSubject) as ContractSubject,DepartmentId,ContractStatus,MIN(ReceivedFlag) as ReceivedFlag,
                    sum(ContractAmount) as ContractAmount,ContractType,MAX(EffectiveAmount) AS EffectiveAmount  FROM ProjectContract   as pco
                    WHERE ContractStatus=4 AND ContractType=1 AND ( ApproverTime >= '" + start + "' AND ApproverTime <= '" + end + "' ) group by ContractStatus, ProjectId, ContractType, DepartmentId) t "
                    + @" inner join  Project a  on a.Id=t.ProjectId
	               
	                LEFT JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
	                LEFT JOIN ( select a.* from ProjectTask a inner join (SELECT ProjectId, MAX(CreateTime) as CreateTime FROM ProjectTask group by ProjectId) b 
                        on a.ProjectId = b.ProjectId and a.CreateTime = b.CreateTime where DeleteFlag=0 or DeleteFlag is NULL ) pt ON pt.ProjectId = a.id
                   
	                LEFT JOIN (select ProjectId, sum(BillingAmount) as BillingAmount from ProjectBilling where BillingStatus = 7 and BillingType != '3' group by ProjectId) pb ON pb.ProjectId = a.Id 
                    left join (select ProjectId,SUM(PaymentAmount) as PaymentAmount from ProjectPayment where PayType<>3 and PaymentStatus<>11 group by ProjectId) pp on pp.ProjectId=a.Id 
	                left join adms706.dbo.lr_base_department d on d.F_DepartmentId=a.DepartmentId
  ");
                }
                else
                {
                    strSql.Append(@"  FROM (SELECT ProjectId,max(MainContract) as MainContract,max(ApproverTime) as ApproverTime,MIN(CreateTime) as CreateTime,                    STUFF((
                            SELECT ',' + ContractNo
                            FROM ProjectContract AS pci
                            WHERE pci.ProjectId = pco.ProjectId
                            FOR XML PATH(''), TYPE
                        ).value('.', 'NVARCHAR(MAX)'), 1, 1, '')  AS ContractNo,
                    max(ContractSubject) as ContractSubject,DepartmentId,ContractStatus,MIN(ReceivedFlag) as ReceivedFlag,
                    sum(ContractAmount) as ContractAmount,ContractType,MAX(EffectiveAmount) AS EffectiveAmount FROM ProjectContract  as pco
                    WHERE ContractStatus=4 AND ContractType=1 group by ContractStatus, ProjectId, ContractType, DepartmentId) t 
	                inner join  Project a  on a.Id=t.ProjectId
	              
	                LEFT JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
	                LEFT JOIN ( select a.* from ProjectTask a inner join (SELECT ProjectId, MAX(CreateTime) as CreateTime FROM ProjectTask group by ProjectId) b 
                        on a.ProjectId = b.ProjectId and a.CreateTime = b.CreateTime where DeleteFlag=0 or DeleteFlag is NULL ) pt ON pt.ProjectId = a.id
 
	               
	                LEFT JOIN (select ProjectId, sum(BillingAmount) as BillingAmount from ProjectBilling where BillingStatus = 7 and BillingType != '3' group by ProjectId) pb ON pb.ProjectId = a.Id 
                    left join (select ProjectId,SUM(PaymentAmount) as PaymentAmount from ProjectPayment where PayType<>3 and PaymentStatus<>11 group by ProjectId) pp on pp.ProjectId=a.Id 
	                left join adms706.dbo.lr_base_department d on d.F_DepartmentId=a.DepartmentId
 ");
                }
                // AND(ApproverTime >= '" + start + "' AND ApproverTime <= '" + end + "')

                strSql.Append(" where t.ProjectId is not null ");


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
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName  like  '%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount= '{0}' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }

                strSql.Append(" ORDER BY t.ApproverTime desc");
                return this.BaseRepository("learunOAWFForm").FindList<SettleAccountsEntity>(strSql.ToString(), dp);
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
        public IEnumerable<SettleAccountsEntity> GetSettleAccountsSum_newDepartmentId(string queryJson, string dep)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"t.ContractNo,
                    CONVERT(varchar(100), t.ApproverTime, 111) as ApproverTime,
                    CONVERT(varchar(100), t.CreateTime, 111) as CreateTime,
	                a.ProjectName,
	                a.CustName,
	                t.ContractSubject,
	                t.ContractStatus,
	                a.ProjectSource,
	  		     a.PreparedPerson,
a.FollowPerson,
pt.ProjectResponsible,                
	                isnull(t.ContractAmount,0) as ContractAmount,
					isnull(pb.BillingAmount,0) as BillingAmount,
					isnull(pc.Amount,0) as Amount,
                    pc.ReceiptDate,
	                t.ContractAmount-isnull(pc.Amount,0) as NotReceived,
	                t.DepartmentId,
				    a.FDepartmentId,
				    a.PDepartmentId,
	               
	                pt.DepartmentId as J_F_FullName,
	                
	                pt.TaskStatus,
					isnull(pp.PaymentAmount,0) as PaymentAmount,
                    (case t.ReceivedFlag
	            when 1 then '已归档'
	            else '未归档'
	            end) as ReceivedFlag,
	                pt.Remark,
t.EffectiveAmount
                ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                    DateTime end = DateTime.Parse(queryParam["EndTime"].ToString());
                    strSql.Append(@"  FROM (SELECT ProjectId,max(MainContract) as MainContract,max(ApproverTime) as ApproverTime,MIN(CreateTime) as CreateTime,                    STUFF((
                            SELECT ',' + ContractNo
                            FROM ProjectContract AS pci
                            WHERE pci.ProjectId = pco.ProjectId
                            FOR XML PATH(''), TYPE
                        ).value('.', 'NVARCHAR(MAX)'), 1, 1, '')  AS ContractNo,
                    max(ContractSubject) as ContractSubject,DepartmentId,ContractStatus,MIN(ReceivedFlag) as ReceivedFlag,
                    sum(ContractAmount) as ContractAmount,ContractType,MAX(EffectiveAmount) AS EffectiveAmount  FROM ProjectContract   as pco
                    WHERE ContractStatus=4 AND ContractType=1 AND ( ApproverTime >= '" + start + "' AND ApproverTime <= '" + end + "' ) group by ContractStatus, ProjectId, ContractType, DepartmentId) t "
                    + @" inner join  Project a  on a.Id=t.ProjectId
	              
	                LEFT JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
	                LEFT JOIN ( select a.* from ProjectTask a inner join (SELECT ProjectId, MAX(CreateTime) as CreateTime FROM ProjectTask group by ProjectId) b 
                        on a.ProjectId = b.ProjectId and a.CreateTime = b.CreateTime where DeleteFlag=0 or DeleteFlag is NULL ) pt ON pt.ProjectId = a.id
                 
	                LEFT JOIN (select ProjectId, sum(BillingAmount) as BillingAmount from ProjectBilling where BillingStatus = 7 and BillingType != '3' group by ProjectId) pb ON pb.ProjectId = a.Id 
                    left join (select ProjectId,SUM(PaymentAmount) as PaymentAmount from ProjectPayment where PayType<>3 and PaymentStatus<>11 group by ProjectId) pp on pp.ProjectId=a.Id 
	                
  ");
                }
                else
                {
                    strSql.Append(@"  FROM (SELECT ProjectId,max(MainContract) as MainContract,max(ApproverTime) as ApproverTime,MIN(CreateTime) as CreateTime,                    STUFF((
                            SELECT ',' + ContractNo
                            FROM ProjectContract AS pci
                            WHERE pci.ProjectId = pco.ProjectId
                            FOR XML PATH(''), TYPE
                        ).value('.', 'NVARCHAR(MAX)'), 1, 1, '')  AS ContractNo,
                    max(ContractSubject) as ContractSubject,DepartmentId,ContractStatus,MIN(ReceivedFlag) as ReceivedFlag,
                    sum(ContractAmount) as ContractAmount,ContractType,MAX(EffectiveAmount) AS EffectiveAmount FROM ProjectContract as pco 
                    WHERE ContractStatus=4 AND ContractType=1 group by ContractStatus, ProjectId, ContractType, DepartmentId) t 
	                inner join  Project a  on a.Id=t.ProjectId
	             
	                LEFT JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
	                LEFT JOIN ( select a.* from ProjectTask a inner join (SELECT ProjectId, MAX(CreateTime) as CreateTime FROM ProjectTask group by ProjectId) b 
                        on a.ProjectId = b.ProjectId and a.CreateTime = b.CreateTime where DeleteFlag=0 or DeleteFlag is NULL ) pt ON pt.ProjectId = a.id
                  
	                LEFT JOIN (select ProjectId, sum(BillingAmount) as BillingAmount from ProjectBilling where BillingStatus = 7 and BillingType != '3' group by ProjectId) pb ON pb.ProjectId = a.Id 
                    left join (select ProjectId,SUM(PaymentAmount) as PaymentAmount from ProjectPayment where PayType<>3 and PaymentStatus<>11 group by ProjectId) pp on pp.ProjectId=a.Id 
	                
 ");
                }
                // AND(ApproverTime >= '" + start + "' AND ApproverTime <= '" + end + "')

                strSql.Append(" where t.ProjectId is not null ");


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
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount like '%{0}%' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }
                strSql.Append(string.Format(" AND ( " + dep + " )"));
                strSql.Append(" ORDER BY t.ApproverTime desc");
                return this.BaseRepository("learunOAWFForm").FindList<SettleAccountsEntity>(strSql.ToString(), dp);
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
        /// 合作伙伴结算台账合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<SettleAccountsEntity> GetSettleAccountsSumHZ(string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"t.ContractNo,
                    CONVERT(varchar(100), t.ApproverTime, 111) as ApproverTime,
                    CONVERT(varchar(100), t.CreateTime, 111) as CreateTime,
	                a.ProjectName,
	                a.CustName,
	                t.ContractSubject,
	                t.ContractStatus,
	                a.ProjectSource,
	                u3.F_RealName as P_F_RealName,
	                isnull(t.ContractAmount,0) as ContractAmount,
					isnull(pb.BillingAmount,0) as BillingAmount,
					isnull(pc.Amount,0) as Amount,
                    pc.ReceiptDate,
	                t.ContractAmount-isnull(pc.Amount,0) as NotReceived,
	                t.DepartmentId as DepartmentIdCA ,
				    a.FDepartmentId,
				    a.PDepartmentId,
	                u.F_RealName as M_F_RealName,
	                pt.DepartmentId as J_F_FullName,
	                u2.F_RealName as J_F_RealName,
	                pt.TaskStatus,
					isnull(pp.PaymentAmount,0) as PaymentAmount,
                    (case t.ReceivedFlag
	            when 1 then '已归档'
	            else '未归档'
	            end) as ReceivedFlag,
	                pt.Remark

                ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                    DateTime end = DateTime.Parse(queryParam["EndTime"].ToString());
                    strSql.Append(@"  FROM (SELECT ProjectId,max(MainContract) as MainContract,max(ApproverTime) as ApproverTime,MIN(CreateTime) as CreateTime,		    
                    STUFF((
                            SELECT ',' + ContractNo
                            FROM ProjectContract AS pci
                            WHERE pci.ProjectId = pco.ProjectId
                            FOR XML PATH(''), TYPE
                        ).value('.', 'NVARCHAR(MAX)'), 1, 1, '')  AS ContractNo,
                    max(ContractSubject) as ContractSubject,DepartmentId,ContractStatus,MIN(ReceivedFlag) as ReceivedFlag,
                    sum(ContractAmount) as ContractAmount,ContractType,MAX(Proportion) AS Proportion  FROM ProjectContract  as pco 
                    WHERE ContractStatus=4 AND ContractType=1 AND ( ApproverTime >= '" + start + "' AND ApproverTime <= '" + end + "' ) group by ContractStatus, ProjectId, ContractType, DepartmentId) t "
                    + @" inner join  Project a  on a.Id=t.ProjectId and a.ProjectSource=3
	                left join adms706.dbo.lr_base_user u on u.F_UserId=a.FollowPerson
	                LEFT JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
	                LEFT JOIN ( select a.* from ProjectTask a inner join (SELECT ProjectId, MAX(CreateTime) as CreateTime FROM ProjectTask group by ProjectId) b 
                        on a.ProjectId = b.ProjectId and a.CreateTime = b.CreateTime where DeleteFlag=0 or DeleteFlag is NULL ) pt ON pt.ProjectId = a.id
                    left join adms706.dbo.lr_base_user u2 on pt.ProjectResponsible=u2.F_UserId
	                left join adms706.dbo.lr_base_user u3 on u3.F_UserId=a.PreparedPerson
	                LEFT JOIN (select ProjectId, sum(BillingAmount) as BillingAmount from ProjectBilling where BillingStatus = 7 and BillingType != '3' group by ProjectId) pb ON pb.ProjectId = a.Id 
                    left join (select ProjectId,SUM(PaymentAmount) as PaymentAmount from ProjectPayment where PayType<>3 and PaymentStatus<>11 group by ProjectId) pp on pp.ProjectId=a.Id 
	                left join adms706.dbo.lr_base_department d on d.F_DepartmentId=a.DepartmentId
  ");
                }
                else
                {
                    strSql.Append(@"  FROM (SELECT ProjectId,max(MainContract) as MainContract,max(ApproverTime) as ApproverTime,MIN(CreateTime) as CreateTime,                    
                    STUFF((
                            SELECT ',' + ContractNo
                            FROM ProjectContract AS pci
                            WHERE pci.ProjectId = pco.ProjectId
                            FOR XML PATH(''), TYPE
                        ).value('.', 'NVARCHAR(MAX)'), 1, 1, '')  AS ContractNo,
                    max(ContractSubject) as ContractSubject,DepartmentId,ContractStatus,MIN(ReceivedFlag) as ReceivedFlag,
                    sum(ContractAmount) as ContractAmount,ContractType,MAX(Proportion) AS Proportion FROM ProjectContract  as pco
                    WHERE ContractStatus=4 AND ContractType=1 group by ContractStatus, ProjectId, ContractType, DepartmentId) t 
	                inner join  Project a  on a.Id=t.ProjectId and a.ProjectSource=3
	                left join adms706.dbo.lr_base_user u on u.F_UserId=a.FollowPerson
	                LEFT JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
	                LEFT JOIN ( select a.* from ProjectTask a inner join (SELECT ProjectId, MAX(CreateTime) as CreateTime FROM ProjectTask group by ProjectId) b 
                        on a.ProjectId = b.ProjectId and a.CreateTime = b.CreateTime where DeleteFlag=0 or DeleteFlag is NULL ) pt ON pt.ProjectId = a.id
                    left join adms706.dbo.lr_base_user u2 on pt.ProjectResponsible=u2.F_UserId
	                left join adms706.dbo.lr_base_user u3 on u3.F_UserId=a.PreparedPerson
	                LEFT JOIN (select ProjectId, sum(BillingAmount) as BillingAmount from ProjectBilling where BillingStatus = 7 and BillingType != '3' group by ProjectId) pb ON pb.ProjectId = a.Id 
                    left join (select ProjectId,SUM(PaymentAmount) as PaymentAmount from ProjectPayment where PayType<>3 and PaymentStatus<>11 group by ProjectId) pp on pp.ProjectId=a.Id 
	                left join adms706.dbo.lr_base_department d on d.F_DepartmentId=a.DepartmentId ");
                }
                // AND(ApproverTime >= '" + start + "' AND ApproverTime <= '" + end + "')

                strSql.Append(" where t.ProjectId is not null ");


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
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName  like  '%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount = '{0}' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }

                strSql.Append(" ORDER BY t.ApproverTime desc");
                return this.BaseRepository("learunOAWFForm").FindList<SettleAccountsEntity>(strSql.ToString(), dp);
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

        public IEnumerable<ProductionEntity> GetProductions(string queryJsons)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"t.ContractNo,
	                a.ProjectName,
                    CONVERT(varchar(100), pt.CreateTime, 111) as CreateTime,
	                a.CustName,
	                t.ContractSubject,
	                pt.ReportSubject,
	                u.F_RealName,
	                t.DepartmentId,
				    a.FDepartmentId,
				    a.PDepartmentId,
	                t.ContractStatus,
	                t.ContractAmount,
	                pt.DepartmentId as J_F_FullName,
	                u2.F_RealName as P_F_RealName,
	                pt.ApproachTime,
	                pt.id,
	                pt.TaskStatus,
	                (case t.ReceivedFlag
	                when 1 then '已归档'
	                else '未归档'
	                end) as ReceivedFlag,
                    pt.FlowFinishedTime,
	                t.ContractType,
                    a.ProjectSource,
	                pt.Remark,a.FollowPerson
                ");
                strSql.Append(@"FROM ProjectContract t 
	                inner join  Project a  on a.Id=t.ProjectId
	        
	                left join ProjectPayCollection pc on pc.ProjectId=a.Id
	                left join ProjectTask pt on pt.ProjectId=a.id left join adms706.dbo.lr_base_user u2 on pt.ProjectResponsible=u2.F_UserId
	          ");
                strSql.Append("  where t.ContractType=1 and (pt.DeleteFlag=0 or pt.DeleteFlag is NULL) and pt.TaskStatus<>1");
                var queryParam = queryJsons.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( pt.CreateTime >= @startTime AND pt.CreateTime <= @endTime ) ");
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
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["CreateTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pt.CreateTime)=0 )", queryParam["CreateTime"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount like '%{0}%' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }
                strSql.Append(" ORDER BY t.ApproverTime desc");
                return this.BaseRepository("learunOAWFForm").FindList<ProductionEntity>(strSql.ToString(), dp);
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
        /// 生产台账合计
        /// </summary>
        /// <param name="queryJsons"></param>
        /// <returns></returns>
        public IEnumerable<ProductionEntity> GetProductions_new(string queryJsons)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"t.ContractNo,
	                a.ProjectName,
                    CONVERT(varchar(100), pt.CreateTime, 111) as CreateTime,
	                a.CustName,
	                t.ContractSubject,
	                pt.ReportSubject,
	               
	                t.DepartmentId,
				    a.FDepartmentId,
				    a.PDepartmentId,
	                t.ContractStatus,
	                t.ContractAmount,
	                pt.DepartmentId as J_F_FullName,
	              
	                pt.ApproachTime,
	                pt.id,
	                pt.TaskStatus,
	                (case t.ReceivedFlag
	                    when 1 then '已归档'
	                    else '未归档'
	                    end) as ReceivedFlag,
                  pt.FlowFinishedTime,
	                t.ContractType,
                    a.ProjectSource,
	                pt.Remark,
                    pt.ProjectResponsible,
                    a.DepartmentId as XDepartmentId,
                    pc.Amount,
                    t.EffectiveAmount,
                    py.PaymentAmount,
                    pt.SubAmount,
                    pt.MainAmount,a.FollowPerson,pc.ReceiptDate
                ");
                string sql = ProductionsSql();
                strSql.Append(sql);
                strSql.Append(" where t.ProjectId is not null and lbd.HZ_DepartmentId != 1 and ( a.ProjectSource = 1 or a.ProjectSource = 2 ) ");
                var queryParam = queryJsons.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate().AddDays(1), DbType.DateTime);
                    strSql.Append(" AND ( pt.FlowFinishedTime >= @startTime AND pt.FlowFinishedTime < @endTime ) ");
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
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["CreateTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pt.CreateTime)=0 )", queryParam["CreateTime"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                /*                if (!queryParam["ReceiptDate"].IsEmpty())
                                {
                                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                                }*/
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    var receipt_time = queryParam["ReceiptDate"].ToObject<List<string>>();
                    if (receipt_time.Count > 0)
                    {
                        string start_date = receipt_time[0];
                        string end_date = receipt_time[1].ToDate().AddDays(1).ToString();
                        string ReceiptDate = " AND ( ReceiptDate >= '" + start_date + "' AND ReceiptDate < '" + end_date + "' )";
                        strSql.Append(ReceiptDate);
                    }
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( py.PaymentAmount = '{0}' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }
                strSql.Append(" ORDER BY t.ApproverTime desc");
                return this.BaseRepository("learunOAWFForm").FindList<ProductionEntity>(strSql.ToString(), dp);
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
        /// 多部门生产合计
        /// </summary>
        /// <param name="queryJsons"></param>
        /// <returns></returns>
        public IEnumerable<ProductionEntity> GetProductions_newDepartmentId(string queryJsons, string dep)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"t.ContractNo,
	                a.ProjectName,
                    CONVERT(varchar(100), pt.CreateTime, 111) as CreateTime,
	                a.CustName,
	                t.ContractSubject,
	                pt.ReportSubject,
	             
	                t.DepartmentId,
				    a.FDepartmentId,
				    a.PDepartmentId,
	                t.ContractStatus,
	                t.ContractAmount,
	                pt.DepartmentId as J_F_FullName,
	               
	                pt.ApproachTime,
	                pt.id,
	                pt.TaskStatus,
	                (case t.ReceivedFlag
	            when 1 then '已归档'
	            else '未归档'
	            end) as ReceivedFlag,
                    pt.FlowFinishedTime,
	                t.ContractType,
                    a.ProjectSource,
	                pt.Remark,
                    pt.ProjectResponsible,
                    a.DepartmentId as XDepartmentId,
                    pc.Amount,pc.ReceiptDate,
                    t.EffectiveAmount,
                    py.PaymentAmount,
                    pt.SubAmount,
                    pt.MainAmount,a.FollowPerson
                ");
                strSql.Append(@"FROM (SELECT ProjectId,max(MainContract) as MainContract,max(ApproverTime) as ApproverTime,MIN(CreateTime) as CreateTime,                    STUFF((
                            SELECT ',' + ContractNo
                            FROM ProjectContract AS pci
                            WHERE pci.ProjectId = pco.ProjectId
                            FOR XML PATH(''), TYPE
                        ).value('.', 'NVARCHAR(MAX)'), 1, 1, '')  AS ContractNo,
                    max(ContractSubject) as ContractSubject,DepartmentId,ContractStatus,MIN(ReceivedFlag) as ReceivedFlag,
                    sum(ContractAmount) as ContractAmount,ContractType,MAX(EffectiveAmount) AS EffectiveAmount FROM ProjectContract  as pco
                    WHERE ContractStatus=4 AND ContractType=1 group by ContractStatus, ProjectId, ContractType, DepartmentId) t 
	                inner join  Project a  on a.Id=t.ProjectId 
	                
	                LEFT JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
	                LEFT JOIN ( select a.* from ProjectTask a inner join (SELECT ProjectId, MAX(CreateTime) as CreateTime FROM ProjectTask group by ProjectId) b 
                    on a.ProjectId = b.ProjectId and a.CreateTime = b.CreateTime where ( DeleteFlag= 0 or DeleteFlag is NULL) ) pt ON pt.ProjectId = a.id
                   
                    left join (select SUM(PaymentAmount) as PaymentAmount,MAX(ProjectId) as ProjectId,MAX(PayType) as PayType from ProjectPayment where PayType<>3 and PaymentStatus<>11 group by ProjectId ) py on a.Id=py.ProjectId");
                strSql.Append("  where t.ProjectId is not null ");
                var queryParam = queryJsons.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( pt.CreateTime >= @startTime AND pt.CreateTime <= @endTime ) ");
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
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["CreateTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pt.CreateTime)=0 )", queryParam["CreateTime"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount like '%{0}%' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }
                strSql.Append(string.Format(" AND ( " + dep + " )"));
                strSql.Append(" ORDER BY t.ApproverTime desc");
                return this.BaseRepository("learunOAWFForm").FindList<ProductionEntity>(strSql.ToString(), dp);
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

        public IEnumerable<MarketingEntity> GetMarketings(string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                pc.id as PC,
                t.id as Tid,
                CONVERT(varchar(100), t.ApproverTime, 111) as ApproverTime,
                CONVERT(varchar(100), t.CreateTime, 111) as CreateTime,
                t.ContractNo,
	            a.ProjectName,
	            a.CustName,
	            t.ContractSubject,
                t.DepartmentId,
				a.FDepartmentId,
				a.PDepartmentId,
	            u.F_RealName,
	            a.ProjectSource,
	            t.ContractStatus,
                (case t.ReceivedFlag
	            when 1 then '已归档'
	            else '未归档'
	            end) as ReceivedFlag,
	            (case pb.BillingStatus
                when 7 then '已开票'
	            else '未开票'
	            end) as BillingStatus,
	            t.ContractAmount,
                isnull(pc.Amount,0) as Amount,
	            t.ContractAmount-isnull(pc.Amount,0) as NotReceived,
	            pc.ReceiptDate,
	            pt.DepartmentId as J_F_FullName,
	            pt.ApproachTime,
	            pt.ReportSubject,
	            pt.TaskStatus,
	            t.ContractType,
                pt.ProjectResponsible,
	            u2.F_RealName as P_F_RealName
                ");
                strSql.Append(@"FROM ProjectContract t 
                    inner join  Project a  on a.Id = t.ProjectId and t.ContractStatus<>7
                    left join adms706.dbo.lr_base_user u on u.F_UserId = a.FollowPerson
                    left join ProjectPayCollection pc on pc.ProjectId = a.Id
                    left join (SELECT top 1 ProjectId,DeleteFlag,ProjectResponsible,DepartmentId,ApproachTime,ReportSubject,TaskStatus from  ProjectTask)  pt on pt.ProjectId = a.Id 
                    left join ProjectPayment pp on pp.ProjectId=a.Id
                    
                    left join adms706.dbo.lr_base_user u2 on pt.ProjectResponsible=u2.F_UserId
                    left join ProjectBilling pb on pb.ProjectId = a.Id and pb.BillingStatus<>8");
                strSql.Append("  WHERE (t.ContractStatus=4 AND t.ContractType=1) and (pt.DeleteFlag=0 or pt.DeleteFlag is NULL)");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.ApproverTime >= @startTime AND t.ApproverTime <= @endTime ) ");
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%'  or  t.ContractNo like'%{0}%'  )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount like '%{0}%' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }
                strSql.Append(" ORDER BY t.ApproverTime desc");
                return this.BaseRepository("learunOAWFForm").FindList<MarketingEntity>(strSql.ToString(), dp);
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
        //营销台账合计
        public IEnumerable<MarketingEntity> GetMarketings_new(string queryJson)
        {
            try
            {


                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                    t.id,
                	 CONVERT ( VARCHAR ( 100 ), t.ApproverTime, 111 ) AS ApproverTime,
	                CONVERT ( VARCHAR ( 100 ), t.CreateTime, 111 ) AS CreateTime,
	                t.ContractNo,
	                a.ProjectName,
	                a.CustName,
	                t.ContractSubject,
	                t.DepartmentId,
	                a.FDepartmentId,
	                a.PDepartmentId,
	              	  pt.ProjectResponsible,
	                a.ProjectSource,
	                t.ContractStatus,
	                ReceivedFlag,
	                ( CASE t.ReceivedFlag WHEN 1 THEN '已归档' ELSE '未归档' END ) AS ReceivedFlagName,
	                cast( BillingAmount as CHAR) AS BillingStatus,
pb.BillingAmount,
	                t.ContractAmount,
	                isnull( pc.Amount, 0 ) AS Amount,
	                t.ContractAmount- isnull( pc.Amount, 0 ) AS NotReceived,
	                pc.ReceiptDate,
	                pt.DepartmentId AS J_F_FullName,
	                pt.ApproachTime,
	                pt.ReportSubject,
	                pt.TaskStatus,
	                t.ContractType,
	                pcs.ProjectId as Settlement,
                    a.PreparedPerson,
                    a.FollowPerson,t.MainDepartmentId,t.MainAmount,t.SubDepartmentId,t.SubAmount,t.EffectiveAmount,task.FinishTime


                ");

                var queryParam = queryJson.ToJObject();
                string start_date = "";
                string end_date = "";
                string approverTime = "";
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    var approve_time = queryParam["ApproverTime"].ToObject<List<string>>();
                    if (approve_time.Count > 0)
                    {
                        start_date = approve_time[0];
                        end_date = approve_time[1].ToDate().AddDays(1).ToString();
                        approverTime = " AND ( ApproverTime >= '" + start_date + "' AND ApproverTime < '" + end_date + "' )";
                    }
                }
                string createTime = "";
                if (!queryParam["CreateTime"].IsEmpty())
                {
                    var create_time = queryParam["CreateTime"].ToObject<List<string>>();
                    if (create_time.Count > 0)
                    {
                        string create_time_start_date = create_time[0];
                        string create_time_end_date = create_time[1].ToDate().AddDays(1).ToString();
                        createTime = " AND ( CreateTime >= '" + create_time_start_date + "' AND CreateTime < '" + create_time_end_date + "' )";
                    }
                }
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                string sql = MarketingSql(approverTime, createTime, queryParam);
                strSql.Append(sql);

                strSql.Append("  WHERE t.ProjectId is not null ");

                //if (!start_date.IsEmpty() && !end_date.IsEmpty())
                //{
                //    strSql.Append(" AND ( t.ApproverTime >= '" + start_date + "' AND t.ApproverTime < '" + end_date + "' ) ");
                //}

                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%'  or  t.ContractNo like'%{0}%'  )", queryParam["keyword"].ToString()));
                }

                if (!queryParam["FinishTime"].IsEmpty())
                {
                    var finish_time = queryParam["FinishTime"].ToObject<List<string>>();
                    if (finish_time.Count > 0)
                    {
                        string finish_start_date = finish_time[0];
                        string finish_end_date = finish_time[1].ToDate().AddDays(1).ToString();
                        createTime = " AND ( task.FinishTime >= '" + finish_start_date + "' AND task.FinishTime < '" + finish_end_date + "' )";
                        strSql.Append(createTime);
                    }
                    //strSql.Append(string.Format(" AND ( task.FinishTime like '%{0}%' )", queryParam["FinishTime"].ToDate().ToString("yyyy-MM-dd")));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                //if (!queryParam["ApproverTime"].IsEmpty())
                //{
                //    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                //}
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                string receiptDateRange = "";
                if (!queryParam["ReceiptDateRange"].IsEmpty())
                {
                    var receipt_DateRange = queryParam["ReceiptDateRange"].ToObject<List<string>>();
                    if (receipt_DateRange.Count > 0)
                    {
                        string receipt_time_start_date = receipt_DateRange[0];
                        string receipt_time_end_date = receipt_DateRange[1].ToDate().AddDays(1).ToString();
                        receiptDateRange = " AND ( pc.ReceiptDate >= '" + receipt_time_start_date + "' AND pc.ReceiptDate < '" + receipt_time_end_date + "' )";
                        strSql.Append(receiptDateRange);
                    }
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( py.PaymentAmount = '{0}' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }
                strSql.Append(" ORDER BY t.ApproverTime desc");
                return this.BaseRepository("learunOAWFForm").FindList<MarketingEntity>(strSql.ToString(), dp);
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
        public IEnumerable<MarketingEntity> GetMarketings_newdc(string queryJson, string dep)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
a.Id,
                	CONVERT ( VARCHAR ( 100 ), t.ApproverTime, 111 ) AS ApproverTime,
	                CONVERT ( VARCHAR ( 100 ), t.CreateTime, 111 ) AS CreateTime,
	                t.ContractNo,
	                a.ProjectName,
	                a.CustName,
	                t.ContractSubject,
	                t.DepartmentId,
	                a.FDepartmentId,
	                a.PDepartmentId,
 
	                a.ProjectSource,
	                t.ContractStatus,
	                ReceivedFlag,
	                ( CASE t.ReceivedFlag WHEN 1 THEN '已归档' ELSE '未归档' END ) AS ReceivedFlagName,
	                cast( pb.BillingAmount as CHAR) AS BillingStatus,
                    pb.BillingAmount,
	                t.ContractAmount,
	                isnull( pc.Amount, 0 ) AS Amount,
	                t.ContractAmount- isnull( pc.Amount, 0 ) AS NotReceived,
	                pc.ReceiptDate,
	                pt.DepartmentId AS J_F_FullName,
	                pt.ApproachTime,
	                pt.ReportSubject,
	                pt.TaskStatus,
	                t.ContractType,
pt.ProjectResponsible,
a.PreparedPerson,
a.FollowPerson,t.EffectiveAmount,py.PaymentAmount,t.MainDepartmentId,t.MainAmount,t.SubDepartmentId,t.SubAmount
                ");
                var queryParam = queryJson.ToJObject();
                string start_date = "";
                string end_date = "";
                string approverTime = "";
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    var approve_time = queryParam["ApproverTime"].ToObject<List<string>>();
                    if (approve_time.Count > 0)
                    {
                        start_date = approve_time[0];
                        end_date = approve_time[1].ToDate().AddDays(1).ToString();
                        approverTime = " AND ( ApproverTime >= '" + start_date + "' AND ApproverTime < '" + end_date + "' )";
                    }
                }
                string createTime = "";
                if (!queryParam["CreateTime"].IsEmpty())
                {
                    var create_time = queryParam["CreateTime"].ToObject<List<string>>();
                    if (create_time.Count > 0)
                    {
                        string create_time_start_date = create_time[0];
                        string create_time_end_date = create_time[1].ToDate().AddDays(1).ToString();
                        createTime = " AND ( CreateTime >= '" + create_time_start_date + "' AND CreateTime < '" + create_time_end_date + "' )";
                    }
                }
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                string sql = MarketingSql(approverTime, createTime, queryParam);
                strSql.Append(sql);

                strSql.Append("  WHERE t.ProjectId is not null ");
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                    DateTime end = DateTime.Parse(queryParam["EndTime"].ToString());
                    strSql.Append(" AND ( t.ApproverTime >= '" + start + "' AND t.ApproverTime <= '" + end + "' ) ");
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%'  or  t.ContractNo like'%{0}%'  )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                //if (!queryParam["ApproverTime"].IsEmpty())
                //{
                //    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                //}
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                string receiptDateRange = "";
                if (!queryParam["ReceiptDateRange"].IsEmpty())
                {
                    var receipt_DateRange = queryParam["ReceiptDateRange"].ToObject<List<string>>();
                    if (receipt_DateRange.Count > 0)
                    {
                        string receipt_time_start_date = receipt_DateRange[0];
                        string receipt_time_end_date = receipt_DateRange[1].ToDate().AddDays(1).ToString();
                        receiptDateRange = " AND ( pc.ReceiptDate >= '" + receipt_time_start_date + "' AND pc.ReceiptDate < '" + receipt_time_end_date + "' )";
                        strSql.Append(receiptDateRange);
                    }
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount like '%{0}%' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId = '{0}' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }
                strSql.Append(string.Format(" AND ( " + dep + " )"));
                return this.BaseRepository("learunOAWFForm").FindList<MarketingEntity>(strSql.ToString(), dp);
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
        /// 营销合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<MarketingEntity> GetMarketingsSum(string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                	 CONVERT ( VARCHAR ( 100 ), t.ApproverTime, 111 ) AS ApproverTime,
	                CONVERT ( VARCHAR ( 100 ), t.CreateTime, 111 ) AS CreateTime,
	                t.ContractNo,
	                a.ProjectName,
	                a.CustName,
	                t.ContractSubject,
	                t.DepartmentId,
	                a.FDepartmentId,
	                a.PDepartmentId,
	                u.F_RealName,
	                a.ProjectSource,
	                t.ContractStatus,
	                ReceivedFlag,
	                ( CASE t.ReceivedFlag WHEN 1 THEN '已归档' ELSE '未归档' END ) AS ReceivedFlagName,
	                cast( BillingAmount as CHAR) AS BillingStatus,
pb.BillingAmount,
	                t.ContractAmount,
	                isnull( pc.Amount, 0 ) AS Amount,
	                t.ContractAmount- isnull( pc.Amount, 0 ) AS NotReceived,
	                pc.ReceiptDate,
	                pt.DepartmentId AS J_F_FullName,
	                pt.ApproachTime,
	                pt.ReportSubject,
	                pt.TaskStatus,
	                t.ContractType,
	                u2.F_RealName AS P_F_RealName,
                    a.PreparedPerson,
                    a.FollowPerson,
                    t.EffectiveAmount,
                    py.PaymentAmount
                ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });

                string start_date = "";
                string end_date = "";
                string approverTime = "";
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    var approve_time = queryParam["ApproverTime"].ToObject<List<string>>();
                    if (approve_time.Count > 0)
                    {
                        start_date = approve_time[0];
                        end_date = approve_time[1].ToDate().AddDays(1).ToString();
                        approverTime = " AND ( ApproverTime >= '" + start_date + "' AND ApproverTime < '" + end_date + "' )";
                    }
                }
                string createTime = "";
                if (!queryParam["CreateTime"].IsEmpty())
                {
                    var create_time = queryParam["CreateTime"].ToObject<List<string>>();
                    if (create_time.Count > 0)
                    {
                        string create_time_start_date = create_time[0];
                        string create_time_end_date = create_time[1].ToDate().AddDays(1).ToString();
                        createTime = " AND ( CreateTime >= '" + create_time_start_date + "' AND CreateTime < '" + create_time_end_date + "' )";
                    }
                }
                string sql = MarketingSql(approverTime, createTime, queryParam);
                strSql.Append(sql);
                strSql.Append("  WHERE t.ProjectId is not null ");
                /*if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.ApproverTime >= @startTime AND t.ApproverTime <= @endTime ) ");
                }*/

                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%'  or  t.ContractNo like'%{0}%'  )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                //if (!queryParam["ApproverTime"].IsEmpty())
                //{
                //    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                //}
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount like '%{0}%' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }
                strSql.Append(" ORDER BY t.ApproverTime desc");
                return this.BaseRepository("learunOAWFForm").FindList<MarketingEntity>(strSql.ToString(), dp);
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
        /// 营销合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<MarketingEntity> GetMarketingsSum_new(string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                	 CONVERT ( VARCHAR ( 100 ), t.ApproverTime, 111 ) AS ApproverTime,
	                CONVERT ( VARCHAR ( 100 ), t.CreateTime, 111 ) AS CreateTime,
	                t.ContractNo,
	                a.ProjectName,
	                a.CustName,
	                t.ContractSubject,
	                t.DepartmentId,
	                a.FDepartmentId,
	                a.PDepartmentId,
	               
	                a.ProjectSource,
	                t.ContractStatus,
	                ReceivedFlag,
	                ( CASE t.ReceivedFlag WHEN 1 THEN '已归档' ELSE '未归档' END ) AS ReceivedFlagName,
	                cast( BillingAmount as CHAR) AS BillingStatus,
pb.BillingAmount,
	                t.ContractAmount,
	                isnull( pc.Amount, 0 ) AS Amount,
	                t.ContractAmount- isnull( pc.Amount, 0 ) AS NotReceived,
	                pc.ReceiptDate,
	                pt.DepartmentId AS J_F_FullName,
	                pt.ApproachTime,
	                pt.ReportSubject,
	                pt.TaskStatus,
	                t.ContractType,
	               
                    a.PreparedPerson,
                    a.FollowPerson,
                    t.EffectiveAmount,
                    py.PaymentAmount,pt.ProjectResponsible
                ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });

                string start_date = "";
                string end_date = "";
                string approverTime = "";
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    var approve_time = queryParam["ApproverTime"].ToObject<List<string>>();
                    if (approve_time.Count > 0)
                    {
                        start_date = approve_time[0];
                        end_date = approve_time[1].ToDate().AddDays(1).ToString();
                        approverTime = " AND ( ApproverTime >= '" + start_date + "' AND ApproverTime < '" + end_date + "' )";
                    }
                }
                string createTime = "";
                if (!queryParam["CreateTime"].IsEmpty())
                {
                    var create_time = queryParam["CreateTime"].ToObject<List<string>>();
                    if (create_time.Count > 0)
                    {
                        string create_time_start_date = create_time[0];
                        string create_time_end_date = create_time[1].ToDate().AddDays(1).ToString();
                        createTime = " AND ( CreateTime >= '" + create_time_start_date + "' AND CreateTime < '" + create_time_end_date + "' )";
                    }
                }
                string sql = MarketingSql(approverTime, createTime, queryParam);
                strSql.Append(sql);

                strSql.Append("  WHERE t.ProjectId is not null ");
                /*if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
               {
                   dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                   dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                   strSql.Append(" AND ( t.ApproverTime >= @startTime AND t.ApproverTime <= @endTime ) ");
               }*/

                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%'  or  t.ContractNo like'%{0}%'  )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                //if (!queryParam["ApproverTime"].IsEmpty())
                //{
                //    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                //}
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount like '%{0}%' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }
                strSql.Append(" ORDER BY t.ApproverTime desc");
                return this.BaseRepository("learunOAWFForm").FindList<MarketingEntity>(strSql.ToString(), dp);
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
        /// 多部门营销合计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<MarketingEntity> GetMarketingsSum_newDepartmentId(string queryJson, string dep)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                	 CONVERT ( VARCHAR ( 100 ), t.ApproverTime, 111 ) AS ApproverTime,
	                CONVERT ( VARCHAR ( 100 ), t.CreateTime, 111 ) AS CreateTime,
	                t.ContractNo,
	                a.ProjectName,
	                a.CustName,
	                t.ContractSubject,
	                t.DepartmentId,
	                a.FDepartmentId,
	                a.PDepartmentId,
	                u.F_RealName,
	                a.ProjectSource,
	                t.ContractStatus,
	                ReceivedFlag,
	                ( CASE t.ReceivedFlag WHEN 1 THEN '已归档' ELSE '未归档' END ) AS ReceivedFlagName,
	                cast( BillingAmount as CHAR) AS BillingStatus,
pb.BillingAmount,
	                t.ContractAmount,
	                isnull( pc.Amount, 0 ) AS Amount,
	                t.ContractAmount- isnull( pc.Amount, 0 ) AS NotReceived,
	                pc.ReceiptDate,
	                pt.DepartmentId AS J_F_FullName,
	                pt.ApproachTime,
	                pt.ReportSubject,
	                pt.TaskStatus,
	                t.ContractType,
	                u2.F_RealName AS P_F_RealName,
                    a.PreparedPerson,
                    a.FollowPerson,
                    t.EffectiveAmount,
                    py.PaymentAmount
                ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });

                string start_date = "";
                string end_date = "";
                string approverTime = "";
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    var approve_time = queryParam["ApproverTime"].ToObject<List<string>>();
                    if (approve_time.Count > 0)
                    {
                        start_date = approve_time[0];
                        end_date = approve_time[1].ToDate().AddDays(1).ToString();
                        approverTime = " AND ( ApproverTime >= '" + start_date + "' AND ApproverTime < '" + end_date + "' )";
                    }
                }
                string createTime = "";
                if (!queryParam["CreateTime"].IsEmpty())
                {
                    var create_time = queryParam["CreateTime"].ToObject<List<string>>();
                    if (create_time.Count > 0)
                    {
                        string create_time_start_date = create_time[0];
                        string create_time_end_date = create_time[1].ToDate().AddDays(1).ToString();
                        createTime = " AND ( CreateTime >= '" + create_time_start_date + "' AND CreateTime < '" + create_time_end_date + "' )";
                    }
                }
                string sql = MarketingSql(approverTime, createTime, queryParam);
                strSql.Append(sql);

                strSql.Append("  WHERE t.ProjectId is not null ");
                /*if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.ApproverTime >= @startTime AND t.ApproverTime <= @endTime ) ");
                }*/

                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%'  or  t.ContractNo like'%{0}%'  )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                //if (!queryParam["ApproverTime"].IsEmpty())
                //{
                //    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                //}
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                string receiptDateRange = "";
                if (!queryParam["ReceiptDateRange"].IsEmpty())
                {
                    var receipt_DateRange = queryParam["ReceiptDateRange"].ToObject<List<string>>();
                    if (receipt_DateRange.Count > 0)
                    {
                        string receipt_time_start_date = receipt_DateRange[0];
                        string receipt_time_end_date = receipt_DateRange[1].ToDate().AddDays(1).ToString();
                        receiptDateRange = " AND ( pc.ReceiptDate >= '" + receipt_time_start_date + "' AND pc.ReceiptDate < '" + receipt_time_end_date + "' )";
                        strSql.Append(receiptDateRange);
                    }
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount like '%{0}%' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }
                strSql.Append(string.Format(" AND ( " + dep + " )"));
                strSql.Append(" ORDER BY t.ApproverTime desc");
                return this.BaseRepository("learunOAWFForm").FindList<MarketingEntity>(strSql.ToString(), dp);
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
        /// 合作伙伴营销台账
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<MarketingEntity> GetMarketingsSumHZ(string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                	

                    CONVERT(VARCHAR(100), t.ApproverTime, 111) AS ApproverTime,
                    CONVERT(VARCHAR(100), t.CreateTime, 111) AS CreateTime,
                    t.ContractNo,
                    a.ProjectName,
                    a.CustName,
                    t.ContractSubject,
                    t.DepartmentId as DepartmentIdCA,
                    a.FDepartmentId,
                    a.PDepartmentId,
                    
                    a.ProjectSource,
                    t.ContractStatus,
                    (CASE t.ReceivedFlag WHEN 1 THEN '已归档' ELSE '未归档' END) AS ReceivedFlag,
                  cast( pb.BillingAmount as CHAR) AS BillingStatus,
                pb.BillingAmount,
                  t.ContractAmount,
	                isnull(pc.Amount, 0) AS Amount,
                  t.ContractAmount - isnull(pc.Amount, 0) AS NotReceived,
                 pc.ReceiptDate,
	                pt.DepartmentId AS J_F_FullName,
	                pt.ApproachTime,
	                pt.ReportSubject,
	                pt.TaskStatus,
	                t.ContractType,
	               pt.ProjectResponsible,
                    a.PreparedPerson,
                    a.FollowPerson
                ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });

                string start_date = "";
                string end_date = "";
                string approverTime = "";
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    var approve_time = queryParam["ApproverTime"].ToObject<List<string>>();
                    if (approve_time.Count > 0)
                    {
                        start_date = approve_time[0];
                        end_date = approve_time[1].ToDate().AddDays(1).ToString();
                        approverTime = " AND ( ApproverTime >= '" + start_date + "' AND ApproverTime < '" + end_date + "' )";
                    }
                }
                string createTime = "";
                if (!queryParam["CreateTime"].IsEmpty())
                {
                    var create_time = queryParam["CreateTime"].ToObject<List<string>>();
                    if (create_time.Count > 0)
                    {
                        string create_time_start_date = create_time[0];
                        string create_time_end_date = create_time[1].ToDate().AddDays(1).ToString();
                        createTime = " AND ( CreateTime >= '" + create_time_start_date + "' AND CreateTime < '" + create_time_end_date + "' )";
                    }
                }
                // 虚拟参数
                string sql = MarketingSql(approverTime, createTime, queryParam);
                strSql.Append(sql);

                strSql.Append("  WHERE t.ProjectId is not null ");

                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%'  or  t.ContractNo like'%{0}%'  )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName  like  '%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                //if (!queryParam["ApproverTime"].IsEmpty())
                //{
                //    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                //}
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                string receiptDateRange = "";
                if (!queryParam["ReceiptDateRange"].IsEmpty())
                {
                    var receipt_DateRange = queryParam["ReceiptDateRange"].ToObject<List<string>>();
                    if (receipt_DateRange.Count > 0)
                    {
                        string receipt_time_start_date = receipt_DateRange[0];
                        string receipt_time_end_date = receipt_DateRange[1].ToDate().AddDays(1).ToString();
                        receiptDateRange = " AND ( pc.ReceiptDate >= '" + receipt_time_start_date + "' AND pc.ReceiptDate < '" + receipt_time_end_date + "' )";
                        strSql.Append(receiptDateRange);
                    }
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount ='{0}' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }
                strSql.Append(" ORDER BY t.ApproverTime desc");
                return this.BaseRepository("learunOAWFForm").FindList<MarketingEntity>(strSql.ToString(), dp);
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
        /// 获取生产报表的数据信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProductionEntity> GetProductions(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"t.ContractNo,
	                a.ProjectName,
                    CONVERT(varchar(100), pt.CreateTime, 111) as CreateTime,
	                a.CustName,
	                t.ContractSubject,
	                pt.ReportSubject,
	                u.F_RealName,
	                t.DepartmentId,
				    a.FDepartmentId,
				    a.PDepartmentId,
	                t.ContractStatus,
	                t.ContractAmount,
	                pt.DepartmentId as J_F_FullName,
	                u2.F_RealName as P_F_RealName,
	                pt.ApproachTime,
	                pt.id,
	                pt.TaskStatus,
	                (case t.ReceivedFlag
	            when 1 then '已归档'
	            else '未归档'
	            end) as ReceivedFlag,
                    pt.FlowFinishedTime,
	                t.ContractType,
                    a.ProjectSource,
	                pt.Remark,
pt.ProjectResponsible,a.FollowPerson
                ");
                strSql.Append(@"FROM ProjectContract t 
	                inner join  Project a  on a.Id=t.ProjectId and t.ContractStatus<>7 and t.ContractStatus<>6
	                left join adms706.dbo.lr_base_user u on u.F_UserId=a.FollowPerson
	                left join ProjectPayCollection pc on pc.ProjectId=a.Id
	                left join ProjectTask pt on pt.ProjectId=a.id 
                    left join adms706.dbo.lr_base_user u2 on pt.ProjectResponsible=u2.F_UserId
	                left join adms706.dbo.lr_base_department d on d.F_DepartmentId=a.DepartmentId");
                strSql.Append("  where t.ContractType=1 and (pt.DeleteFlag=0 or pt.DeleteFlag is NULL) and pt.TaskStatus<>1");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( pt.CreateTime >= @startTime AND pt.CreateTime <= @endTime ) ");
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
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["CreateTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pt.CreateTime)=0 )", queryParam["CreateTime"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount like '%{0}%' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProductionEntity>(strSql.ToString(), dp, pagination);
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
        /// 生产台账
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProductionEntity> GetProductions_new(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"t.ContractNo,
	                a.ProjectName,
                    CONVERT(varchar(100), pt.CreateTime, 111) as CreateTime,
	                a.CustName,
	                t.ContractSubject,
	                pt.ReportSubject,
	                a.FollowPerson,
	                t.DepartmentId,
				    a.FDepartmentId,
				    a.PDepartmentId,
	                t.ContractStatus,
	                t.ContractAmount,
	                pt.DepartmentId as J_F_FullName,
	                pt.ApproachTime,
	                pt.id,
	                pt.TaskStatus,
                    t.ReceivedFlag,
	                (case t.ReceivedFlag
	                    when 1 then '已归档'
	                    else '未归档'
	                    end) as ReceivedFlagName,
                    pt.FlowFinishedTime,
	                t.ContractType,
                    a.ProjectSource,
	                pt.Remark,
                    pt.ProjectResponsible,
                    a.DepartmentId as XDepartmentId,
                    pc.Amount,pc.ReceiptDate,t.EffectiveAmount,py.PaymentAmount,pt.SubAmount,pt.MainAmount
                ");
                string sql = ProductionsSql();
                strSql.Append(sql);
                strSql.Append("  where t.ProjectId is not null and lbd.HZ_DepartmentId != 1 and ( a.ProjectSource = 1 or a.ProjectSource = 2 ) ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });

                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate().AddDays(1), DbType.DateTime);
                    strSql.Append(" AND ( pt.FlowFinishedTime >= @startTime AND pt.FlowFinishedTime < @endTime ) ");
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%'  or  t.ContractNo like'%{0}%'  )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId ='{0}' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND pt.ApproachTime ='{0}'", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["CreateTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pt.CreateTime)=0 )", queryParam["CreateTime"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                /*                if (!queryParam["ReceiptDate"].IsEmpty())
                                {
                                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                                }*/
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    var receipt_time = queryParam["ReceiptDate"].ToObject<List<string>>();
                    if (receipt_time.Count > 0)
                    {
                        string start_date = receipt_time[0];
                        string end_date = receipt_time[1].ToDate().AddDays(1).ToString();
                        string ReceiptDate = " AND ( ReceiptDate >= '" + start_date + "' AND ReceiptDate < '" + end_date + "' )";
                        strSql.Append(ReceiptDate);
                    }
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( py.PaymentAmount = '{0}' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                /*  if (!queryParam["ApproachTime"].IsEmpty())
                  {
                      strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                  }*/
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProductionEntity>(strSql.ToString(), dp, pagination);
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
        } /// <summary>
          /// 多部门生产台账
          /// </summary>
          /// <param name="pagination"></param>
          /// <param name="queryJson"></param>
          /// <returns></returns>
        public IEnumerable<ProductionEntity> GetProductions_newDepartmentId(Pagination pagination, string queryJson, string dep)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"t.ContractNo,
	                a.ProjectName,
                    CONVERT(varchar(100), pt.CreateTime, 111) as CreateTime,
	                a.CustName,
	                t.ContractSubject,
	                pt.ReportSubject,
	             
	                t.DepartmentId,
				    a.FDepartmentId,
				    a.PDepartmentId,
	                t.ContractStatus,
	                t.ContractAmount,
	                pt.DepartmentId as J_F_FullName,
	              
	                pt.ApproachTime,
	                pt.id,
	                pt.TaskStatus,
	                (case t.ReceivedFlag
	            when 1 then '已归档'
	            else '未归档'
	            end) as ReceivedFlag,
                    pt.FlowFinishedTime,
	                t.ContractType,
                    a.ProjectSource,
	                pt.Remark,
pt.ProjectResponsible,
a.DepartmentId as XDepartmentId,
pc.Amount,pc.ReceiptDate,t.EffectiveAmount,py.PaymentAmount,pt.SubAmount,pt.MainAmount,a.FollowPerson
                ");
                strSql.Append(@"FROM (SELECT ProjectId,max(MainContract) as MainContract,max(ApproverTime) as ApproverTime,MIN(CreateTime) as CreateTime,                    STUFF((
                            SELECT ',' + ContractNo
                            FROM ProjectContract AS pci
                            WHERE pci.ProjectId = pco.ProjectId
                            FOR XML PATH(''), TYPE
                        ).value('.', 'NVARCHAR(MAX)'), 1, 1, '')  AS ContractNo,
                    max(ContractSubject) as ContractSubject,DepartmentId,ContractStatus,MIN(ReceivedFlag) as ReceivedFlag,
                    sum(ContractAmount) as ContractAmount,ContractType,MAX(EffectiveAmount) AS EffectiveAmount FROM ProjectContract  as pco
                    WHERE ContractStatus=4 AND ContractType=1 group by ContractStatus, ProjectId, ContractType, DepartmentId) t 
	                inner join  Project a  on a.Id=t.ProjectId 
	               
	                LEFT JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
	                LEFT JOIN ( select a.* from ProjectTask a inner join (SELECT ProjectId, MAX(CreateTime) as CreateTime, MAX(ApproachTime) as ApproachTime FROM ProjectTask group by ProjectId) b 
                    on a.ProjectId = b.ProjectId and a.CreateTime = b.CreateTime where ( DeleteFlag= 0 or DeleteFlag is NULL) ) pt ON pt.ProjectId = a.id
                  
left join (select SUM(PaymentAmount) as PaymentAmount,MAX(ProjectId) as ProjectId,MAX(PayType) as PayType from ProjectPayment where PayType<>3 and PaymentStatus<>11 group by ProjectId ) py on a.Id=py.ProjectId  ");
                strSql.Append("  where t.ProjectId is not null ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });

                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( pt.CreateTime >= @startTime AND pt.CreateTime <= @endTime ) ");
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
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["CreateTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pt.CreateTime)=0 )", queryParam["CreateTime"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount like '%{0}%' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }
                strSql.Append(string.Format(" AND ( " + dep + " )"));
                return this.BaseRepository("learunOAWFForm").FindList<ProductionEntity>(strSql.ToString(), dp, pagination);
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
        /// 获取结算报表的数据信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<SettleAccountsEntity> GetSettleAccounts(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"t.ContractNo,
                    pc.id as PC,
                    t.id as Tid,
                    CONVERT(varchar(100), t.ApproverTime, 111) as ApproverTime,
                    CONVERT(varchar(100), t.CreateTime, 111) as CreateTime,
	                a.ProjectName,
	                a.CustName,
	                t.ContractSubject,
	                t.ContractStatus,
	                a.ProjectSource,
	                u3.F_RealName as P_F_RealName,
	                isnull(t.ContractAmount,0) as ContractAmount,
					isnull(pb.BillingAmount,0) as BillingAmount,
					isnull(pc.Amount,0) as Amount,
                    pc.ReceiptDate,
	                t.ContractAmount-isnull(pc.Amount,0) as NotReceived,
	                t.DepartmentId,
				    a.FDepartmentId,
				    a.PDepartmentId,
	                u.F_RealName as M_F_RealName,
	                pt.DepartmentId as J_F_FullName,
	                u2.F_RealName as J_F_RealName,
	                pt.TaskStatus,
					isnull(pp.PaymentAmount,0) as PaymentAmount,
                    (case t.ReceivedFlag
	            when 1 then '已归档'
	            else '未归档'
	            end) as ReceivedFlag,
	                pt.Remark
                ");
                strSql.Append(@"  FROM ProjectContract t 
	                inner join  Project a  on a.Id=t.ProjectId and t.ContractStatus<>7 and t.ContractStatus<>6
	                left join adms706.dbo.lr_base_user u on u.F_UserId=a.FollowPerson
	                left join ProjectPayCollection pc on pc.ProjectId=a.Id
	                left join ProjectTask pt on pt.ProjectId=a.id left join adms706.dbo.lr_base_user u2 on pt.ProjectResponsible=u2.F_UserId
	                left join adms706.dbo.lr_base_user u3 on u3.F_UserId=a.PreparedPerson
	                left join ProjectBilling pb on pb.ProjectId=a.Id and pb.BillingStatus<>8
	                left join ProjectContract t2 on t2.ProjectId=a.Id and t2.ContractType=2
	                left join ProjectPayment pp on pp.ProjectId=a.Id and pp.PayType<>3
	                left join adms706.dbo.lr_base_department d on d.F_DepartmentId=a.DepartmentId");
                strSql.Append(" where (t.ContractStatus=4 AND t.ContractType=1) and (pt.DeleteFlag=0 or pt.DeleteFlag is NULL)");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.ApproverTime >= @startTime AND t.ApproverTime <= @endTime ) ");
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
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount like '%{0}%' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<SettleAccountsEntity>(strSql.ToString(), dp, pagination);
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
        /// 结算列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<SettleAccountsEntity> GetSettleAccounts_new(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"t.ContractNo,
                    CONVERT(varchar(100), t.ApproverTime, 111) as ApproverTime,
                    CONVERT(varchar(100), t.CreateTime, 111) as CreateTime,
	                a.ProjectName,
	                a.CustName,
	                t.ContractSubject,
	                t.ContractStatus,
	                a.ProjectSource,
	               a.FollowPerson,
                   a.PreparedPerson,
	                isnull(t.ContractAmount,0) as ContractAmount,
					isnull(pb.BillingAmount,0) as BillingAmount,
					isnull(pc.Amount,0) as Amount,
                    pc.ReceiptDate,
	                t.ContractAmount-isnull(pc.Amount,0) as NotReceived,
	                t.DepartmentId,
				    a.FDepartmentId,
				    a.PDepartmentId,
	              
	                pt.DepartmentId as J_F_FullName,
	               
	                pt.TaskStatus,
					isnull(pp.PaymentAmount,0) as PaymentAmount,
                    (case t.ReceivedFlag
	            when 1 then '已归档'
	            else '未归档'
	            end) as ReceivedFlag,
	                pt.Remark,t.EffectiveAmount,pt.SubAmount,pt.MainAmount,pt.ProjectResponsible
                ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                string sql = SettleAccountSql(queryParam);
                strSql.Append(sql);
                strSql.Append(" where t.ProjectId is not null ");
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%'  or  t.ContractNo like'%{0}%'  )", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId ='{0}' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName  like  '%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount = '{0}' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<SettleAccountsEntity>(strSql.ToString(), dp, pagination);
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
        } /// <summary>
          /// 多部门结算列表
          /// </summary>
          /// <param name="pagination"></param>
          /// <param name="queryJson"></param>
          /// <returns></returns>
        public IEnumerable<SettleAccountsEntity> GetSettleAccounts_newDepartmentId(Pagination pagination, string queryJson, string dep)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"t.ContractNo,
                    CONVERT(varchar(100), t.ApproverTime, 111) as ApproverTime,
                    CONVERT(varchar(100), t.CreateTime, 111) as CreateTime,
	                a.ProjectName,
	                a.CustName,
	                t.ContractSubject,
	                t.ContractStatus,
	                a.ProjectSource,
		     a.PreparedPerson,
a.FollowPerson,
pt.ProjectResponsible,             
	                isnull(t.ContractAmount,0) as ContractAmount,
					isnull(pb.BillingAmount,0) as BillingAmount,
					isnull(pc.Amount,0) as Amount,
                    pc.ReceiptDate,
	                t.ContractAmount-isnull(pc.Amount,0) as NotReceived,
	                t.DepartmentId,
				    a.FDepartmentId,
				    a.PDepartmentId,

	                pt.DepartmentId as J_F_FullName,
	      
	                pt.TaskStatus,
					isnull(pp.PaymentAmount,0) as PaymentAmount,
                    (case t.ReceivedFlag
	            when 1 then '已归档'
	            else '未归档'
	            end) as ReceivedFlag,
	                pt.Remark,t.EffectiveAmount,pt.SubAmount,pt.MainAmount
                ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                    DateTime end = DateTime.Parse(queryParam["EndTime"].ToString());
                    strSql.Append(@"  FROM (SELECT ProjectId,max(MainContract) as MainContract,max(ApproverTime) as ApproverTime,MIN(CreateTime) as CreateTime,                    STUFF((
                            SELECT ',' + ContractNo
                            FROM ProjectContract AS pci
                            WHERE pci.ProjectId = pco.ProjectId
                            FOR XML PATH(''), TYPE
                        ).value('.', 'NVARCHAR(MAX)'), 1, 1, '')  AS ContractNo,
                    max(ContractSubject) as ContractSubject,DepartmentId,ContractStatus,MIN(ReceivedFlag) as ReceivedFlag,
                    sum(ContractAmount) as ContractAmount,ContractType,MAX(EffectiveAmount) AS EffectiveAmount  FROM ProjectContract   as pco
                    WHERE ContractStatus=4 AND ContractType=1 AND ( ApproverTime >= '" + start + "' AND ApproverTime <= '" + end + "' ) group by ContractStatus, ProjectId, ContractType, DepartmentId) t "
                    + @" inner join  Project a  on a.Id=t.ProjectId

	                LEFT JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
	                LEFT JOIN ( select a.* from ProjectTask a inner join (SELECT ProjectId, MAX(CreateTime) as CreateTime FROM ProjectTask group by ProjectId) b 
                        on a.ProjectId = b.ProjectId and a.CreateTime = b.CreateTime where DeleteFlag=0 or DeleteFlag is NULL ) pt ON pt.ProjectId = a.id
            
	                LEFT JOIN (select ProjectId, sum(BillingAmount) as BillingAmount from ProjectBilling where BillingStatus = 7 and BillingType != '3' group by ProjectId) pb ON pb.ProjectId = a.Id 
                    left join (select ProjectId,SUM(PaymentAmount) as PaymentAmount from ProjectPayment where PayType<>3 group and PaymentStatus<>11 by ProjectId) pp on pp.ProjectId=a.Id 
	                
  ");
                }
                else
                {
                    strSql.Append(@"  FROM (SELECT ProjectId,max(MainContract) as MainContract,max(ApproverTime) as ApproverTime,MIN(CreateTime) as CreateTime,                    STUFF((
                            SELECT ',' + ContractNo
                            FROM ProjectContract AS pci
                            WHERE pci.ProjectId = pco.ProjectId
                            FOR XML PATH(''), TYPE
                        ).value('.', 'NVARCHAR(MAX)'), 1, 1, '')  AS ContractNo,
                    max(ContractSubject) as ContractSubject,DepartmentId,ContractStatus,MIN(ReceivedFlag) as ReceivedFlag,
                    sum(ContractAmount) as ContractAmount,ContractType,MAX(EffectiveAmount) AS EffectiveAmount FROM ProjectContract  as pco
                    WHERE ContractStatus=4 AND ContractType=1 group by ContractStatus, ProjectId, ContractType, DepartmentId) t 
	                inner join  Project a  on a.Id=t.ProjectId

	                LEFT JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
	                LEFT JOIN ( select a.* from ProjectTask a inner join (SELECT ProjectId, MAX(CreateTime) as CreateTime FROM ProjectTask group by ProjectId) b 
                        on a.ProjectId = b.ProjectId and a.CreateTime = b.CreateTime where DeleteFlag=0 or DeleteFlag is NULL ) pt ON pt.ProjectId = a.id
           
	                LEFT JOIN (select ProjectId, sum(BillingAmount) as BillingAmount from ProjectBilling where BillingStatus = 7 and BillingType != '3' group by ProjectId) pb ON pb.ProjectId = a.Id 
                    left join (select ProjectId,SUM(PaymentAmount) as PaymentAmount from ProjectPayment where PayType<>3 and PaymentStatus<>11 group by ProjectId) pp on pp.ProjectId=a.Id 
	               
 ");
                }
                // AND(ApproverTime >= '" + start + "' AND ApproverTime <= '" + end + "')

                strSql.Append(" where t.ProjectId is not null ");


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
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount like '%{0}%' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }
                strSql.Append(string.Format(" AND ( " + dep + " )"));
                return this.BaseRepository("learunOAWFForm").FindList<SettleAccountsEntity>(strSql.ToString(), dp, pagination);
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
        /// 合作伙伴结算台账
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<SettleAccountsEntity> GetSettleAccounts_newHZ(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"t.ContractNo,
                    CONVERT(varchar(100), t.ApproverTime, 111) as ApproverTime,
                    CONVERT(varchar(100), t.CreateTime, 111) as CreateTime,
	                a.ProjectName,
	                a.CustName,
	                t.ContractSubject,
	                t.ContractStatus,
	                a.ProjectSource,
	     a.PreparedPerson,
a.FollowPerson,
pt.ProjectResponsible,
	                isnull(t.ContractAmount,0) as ContractAmount,
					isnull(pb.BillingAmount,0) as BillingAmount,
					isnull(pc.Amount,0) as Amount,
                    pc.ReceiptDate,
	                t.ContractAmount-isnull(pc.Amount,0) as NotReceived,
	                t.DepartmentId as DepartmentIdCA,
				    a.FDepartmentId,
				    a.PDepartmentId,
	   
	                pt.DepartmentId as J_F_FullName,

	                pt.TaskStatus,
					isnull(pp.PaymentAmount,0) as PaymentAmount,
                    (case t.ReceivedFlag
	            when 1 then '已归档'
	            else '未归档'
	            end) as ReceivedFlag,
	                pt.Remark,t.ProjectId as cProjectId,t.Proportion
                ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                    DateTime end = DateTime.Parse(queryParam["EndTime"].ToString());
                    strSql.Append(@"  FROM (SELECT ProjectId,max(MainContract) as MainContract,max(ApproverTime) as ApproverTime,MIN(CreateTime) as CreateTime,                    STUFF((
                            SELECT ',' + ContractNo
                            FROM ProjectContract AS pci
                            WHERE pci.ProjectId = pco.ProjectId
                            FOR XML PATH(''), TYPE
                        ).value('.', 'NVARCHAR(MAX)'), 1, 1, '')  AS ContractNo,
                    max(ContractSubject) as ContractSubject,max(DepartmentId) as DepartmentId,max(ContractStatus) as ContractStatus,MIN(ReceivedFlag) as ReceivedFlag,
                    sum(ContractAmount) as ContractAmount,MAX(ContractType) as ContractType,MAX(Proportion) as Proportion FROM ProjectContract  as pco
                    WHERE ContractStatus=4 AND ContractType=1   and ( ApproverTime >= '" + start + "' AND ApproverTime <= '" + end + "' ) group by ProjectId) t "
                    + @" inner join  Project a  on a.Id=t.ProjectId and a.ProjectSource=3
	             
	                LEFT JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
	                LEFT JOIN ( select a.* from ProjectTask a inner join (SELECT ProjectId, MAX(CreateTime) as CreateTime FROM ProjectTask group by ProjectId) b 
                        on a.ProjectId = b.ProjectId and a.CreateTime = b.CreateTime where DeleteFlag=0 or DeleteFlag is NULL ) pt ON pt.ProjectId = a.id
                  
	                LEFT JOIN (select ProjectId, sum(BillingAmount) as BillingAmount from ProjectBilling where BillingStatus = 7 and BillingType != '3' group by ProjectId) pb ON pb.ProjectId = a.Id 
                    left join (select ProjectId,SUM(PaymentAmount) as PaymentAmount from ProjectPayment where PayType<>3 and PaymentStatus<>11 group by ProjectId) pp on pp.ProjectId=a.Id 
	               ");
                }
                else
                {
                    strSql.Append(@"  FROM (SELECT ProjectId,max(MainContract) as MainContract,max(ApproverTime) as ApproverTime,MIN(CreateTime) as CreateTime,                    STUFF((
                            SELECT ',' + ContractNo
                            FROM ProjectContract AS pci
                            WHERE pci.ProjectId = pco.ProjectId
                            FOR XML PATH(''), TYPE
                        ).value('.', 'NVARCHAR(MAX)'), 1, 1, '')  AS ContractNo,
                    max(ContractSubject) as ContractSubject,max(DepartmentId) as DepartmentId,max(ContractStatus) as ContractStatus,MIN(ReceivedFlag) as ReceivedFlag,
                    sum(ContractAmount) as ContractAmount,MAX(ContractType) as ContractType,MAX(Proportion) as Proportion FROM ProjectContract  as pco
                    WHERE ContractStatus=4 AND ContractType=1   group by ProjectId) t 
	                inner join  Project a  on a.Id=t.ProjectId  and a.ProjectSource=3
	                
	                LEFT JOIN ( SELECT ProjectId, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ProjectId ) pc ON pc.ProjectId = a.Id
	                LEFT JOIN ( select a.* from ProjectTask a inner join (SELECT ProjectId, MAX(CreateTime) as CreateTime FROM ProjectTask group by ProjectId) b 
                        on a.ProjectId = b.ProjectId and a.CreateTime = b.CreateTime where DeleteFlag=0 or DeleteFlag is NULL ) pt ON pt.ProjectId = a.id
                   
	                LEFT JOIN (select ProjectId, sum(BillingAmount) as BillingAmount from ProjectBilling where BillingStatus = 7 and BillingType != '3' group by ProjectId) pb ON pb.ProjectId = a.Id 
                    left join (select ProjectId,SUM(PaymentAmount) as PaymentAmount from ProjectPayment where PayType<>3 and PaymentStatus<>11 group by ProjectId) pp on pp.ProjectId=a.Id ");
                }
                // AND(ApproverTime >= '" + start + "' AND ApproverTime <= '" + end + "')

                strSql.Append(" where t.ProjectId is not null ");


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
                    strSql.Append(string.Format(" AND ( t.ProjectId like  '%{0}%' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName  like  '%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["ApproverTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',t.ApproverTime)=0 )", queryParam["ApproverTime"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractNo like '%{0}%' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContractSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractSubject = '{0}' )", queryParam["ContractSubject"].ToString()));
                }
                if (!queryParam["ProjectSource"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectSource = '{0}' )", queryParam["ProjectSource"].ToString()));
                }
                if (!queryParam["ContractAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractAmount = '{0}' )", queryParam["ContractAmount"].ToString()));
                }
                if (!queryParam["ContractStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContractStatus = '{0}' )", queryParam["ContractStatus"].ToString()));
                }
                if (!queryParam["Y_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId like '%{0}%' )", queryParam["Y_DepartmentId"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( isnull(pc.Amount,0) = '{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( DateDiff(DD,'{0}',pc.ReceiptDate)=0)", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pp.PaymentAmount = '{0}' )", queryParam["PaymentAmount"].ToString()));
                }
                if (!queryParam["S_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.DepartmentId like '%{0}%' )", queryParam["S_DepartmentId"].ToString()));
                }
                if (!queryParam["ApproachTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (DateDiff(DD,'{0}',pt.ApproachTime)=0 )", queryParam["ApproachTime"].ToString()));
                }
                if (!queryParam["ReportSubject"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.ReportSubject = '{0}' )", queryParam["ReportSubject"].ToString()));
                }
                if (!queryParam["TaskStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pt.TaskStatus = '{0}' )", queryParam["TaskStatus"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<SettleAccountsEntity>(strSql.ToString(), dp, pagination);
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

        public IEnumerable<ProjectContractEntity> getChangeList()
        {
            int? status = 6;
            return this.BaseRepository("learunOAWFForm").FindList<ProjectContractEntity>(t => t.ContractStatus == 6);
        }
        public CapitalAmountEntity getCapitalAmountByYearMonth(string yearMonth)
        {
            var list = this.BaseRepository("learunOAWFForm").FindList<CapitalAmountEntity>(t => t.Yearyear == yearMonth).ToList();
            if (list.Count > 0)
            {
                return list.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        #endregion
        private string MarketingSql(string approverTime, string createTime, JObject queryParam)
        {
            string chosen = "";
            if (!queryParam["ApproachTime"].IsEmpty())
            {
                chosen += " AND CAST(ApproachTime AS DATE) = '" + queryParam["ApproachTime"].ToString() + "'";
            }
            var taskSql = new StringBuilder();
            //taskSql.Append(@"SELECT 
            //            ContractId,
            //            STUFF((SELECT ',' + CAST(TaskStatus AS VARCHAR(max)) 
            //                    FROM ProjectTask AS PT 
            //                    WHERE PT.ContractId = T.ContractId
            //                    AND (PT.DeleteFlag = 0 OR PT.DeleteFlag IS NULL)
            //                    FOR XML PATH('')), 1, 1, '') AS TaskStatus,
            //            STUFF((SELECT ',' + CAST(DepartmentId AS VARCHAR(max)) 
            //                    FROM ProjectTask AS PT 
            //                    WHERE PT.ContractId = T.ContractId
            //                    AND (PT.DeleteFlag = 0 OR PT.DeleteFlag IS NULL)
            //                    FOR XML PATH('')), 1, 1, '') AS DepartmentId,
            //            STUFF((SELECT ',' + CAST(ApproachTime AS VARCHAR(max)) 
            //                    FROM ProjectTask AS PT 
            //                    WHERE PT.ContractId = T.ContractId
            //                    AND (PT.DeleteFlag = 0 OR PT.DeleteFlag IS NULL)
            //                    FOR XML PATH('')), 1, 1, '') AS ApproachTime,
            //            STUFF((SELECT ',' + CAST(ReportSubject AS VARCHAR(max)) 
            //                    FROM ProjectTask AS PT 
            //                    WHERE PT.ContractId = T.ContractId
            //                    AND (PT.DeleteFlag = 0 OR PT.DeleteFlag IS NULL)
            //                    FOR XML PATH('')), 1, 1, '') AS ReportSubject,
            //            STUFF((SELECT ',' + CAST(ProjectResponsible AS VARCHAR(max)) 
            //                    FROM ProjectTask AS PT 
            //                    WHERE PT.ContractId = T.ContractId
            //                    AND (PT.DeleteFlag = 0 OR PT.DeleteFlag IS NULL)
            //                    FOR XML PATH('')), 1, 1, '') AS ProjectResponsible
            //        FROM 
            //            ProjectTask AS T
            //        WHERE 
            //            (T.DeleteFlag = 0 OR T.DeleteFlag IS NULL) and T.ContractId is not null ");
            taskSql.Append(@"SELECT 
                        ContractId,
                        5 AS TaskStatus,
                        MAX(DepartmentId) as DepartmentId,
                        Max( ApproachTime) as ApproachTime,
                        Max( ReportSubject) as ReportSubject,
                        Max( ProjectResponsible) as ProjectResponsible
                    FROM 
                        ProjectTask AS T
                    WHERE 
                        (T.DeleteFlag = 0 OR T.DeleteFlag IS NULL) and T.ContractId is not null and T.TaskStatus = 5 ");
            taskSql.Append(chosen);
            taskSql.Append("GROUP BY ContractId ");
            var strSql = new StringBuilder();
            strSql.Append(@" FROM
                    (SELECT * FROM ProjectContract as pco WHERE ContractStatus=4 AND ContractType=1 " +
                    approverTime + createTime + " ) t "
        + @" INNER JOIN Project a ON a.Id = t.ProjectId 
	        LEFT JOIN ( SELECT ContractNo, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ContractNo ) pc ON pc.ContractNo = t.ContractNo
	        LEFT JOIN ( ");
            strSql.Append(taskSql.ToString());
            strSql.Append(@" ) pt ON pt.ContractId = t.id
	        LEFT JOIN (            SELECT
                ContractId,
                SUM(BillingAmount) AS BillingAmount,
                (SELECT TOP 1 WorkFlowId FROM projectbilling AS pb2 WHERE pb2.ContractId = pb1.ContractId) AS WorkFlowId 
            FROM
                projectbilling AS pb1 
            WHERE
                pb1.BillingStatus = 7 
                AND pb1.BillingType != '3' 
            GROUP BY
                ContractId) pb ON pb.ContractId = t.id 
            left join (select SUM(PaymentAmount) as PaymentAmount,ContractId,MAX(PayType) as PayType from ProjectPayment where PayType<>3 and PaymentStatus<>11 group by ContractId ) py on t.id=py.ContractId 
            LEFT JOIN adms706.dbo.lr_base_user u ON u.F_UserId = a.FollowPerson
            left join adms706.dbo.lr_base_user u2 on pt.ProjectResponsible=u2.F_UserId
            left join ProjectContractSettlement pcs on t.id = pcs.ContractId 
            left join (select F_ProcessId, max(F_CreateDate) as FinishTime from adms706.dbo.lr_nwf_task where F_IsFinished = 1 group by F_ProcessId) task on task.F_ProcessId = pb.WorkFlowId");
            return strSql.ToString();
        }
        private string ProductionsSql()
        {
            var strSql = new StringBuilder();
            strSql.Append(@"FROM (SELECT * from ProjectContract as pco WHERE ContractStatus=4 AND ContractType=1) t 
	                inner join  Project a  on a.Id=t.ProjectId 
	                LEFT JOIN ( SELECT ContractNo, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ContractNo ) pc ON pc.ContractNo = t.ContractNo
	                LEFT JOIN ( select a.* from ProjectTask a where DeleteFlag= 0 or DeleteFlag is NULL ) pt ON pt.ContractId = t.id     
	                left join adms706.dbo.lr_base_user lbu on pt.ProjectResponsible = lbu.F_UserId
	                left join adms706.dbo.lr_base_department lbd on lbu.F_DepartmentId = lbd.F_DepartmentId
                  left join (select SUM(PaymentAmount) as PaymentAmount,ContractId,MAX(PayType) as PayType from ProjectPayment where PayType<>3 and PaymentStatus<>11 group by ContractId ) py on t.id=py.ContractId  ");
            return strSql.ToString();
        }
        private string SettleAccountSql(JObject queryParam)
        {
            var strSql = new StringBuilder();
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                DateTime end = DateTime.Parse(queryParam["EndTime"].ToString());
                strSql.Append(@"  FROM (SELECT * FROM ProjectContract as pco
                    WHERE ContractStatus=4 AND ContractType=1 AND ( ApproverTime >= '" + start + "' AND ApproverTime <= '" + end + "' ) ) t "
                            + @" inner join  Project a  on a.Id=t.ProjectId
	                LEFT JOIN ( SELECT ContractNo, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ContractNo ) pc ON pc.ContractNo = a.ContractNo
	                LEFT JOIN ( select a.* from ProjectTask a where DeleteFlag=0 or DeleteFlag is NULL ) pt ON pt.ContractId = t.id
	                LEFT JOIN (select ContractId, sum(BillingAmount) as BillingAmount from ProjectBilling where BillingStatus = 7 and BillingType != '3' group by ContractId) pb ON pb.ContractId = t.id 
                    left join (select ContractId,SUM(PaymentAmount) as PaymentAmount from ProjectPayment where PayType<>3 and PaymentStatus<>11 group by ContractId) pp on pp.ContractId=t.id 
	                left join adms706.dbo.lr_base_department d on d.F_DepartmentId=a.DepartmentId ");
            }
            else
            {
                strSql.Append(@"  FROM (SELECT * FROM ProjectContract  as pco
                    WHERE ContractStatus=4 AND ContractType=1 ) t 
	                inner join  Project a  on a.Id=t.ProjectId
	                LEFT JOIN ( SELECT ContractNo, SUM ( Amount ) AS Amount, MAX ( ReceiptDate ) AS ReceiptDate FROM ProjectPayCollection GROUP BY ContractNo ) pc ON pc.ContractNo = a.ContractNo
	                LEFT JOIN ( select a.* from ProjectTask a where DeleteFlag=0 or DeleteFlag is NULL ) pt ON pt.ContractId = t.id
	                LEFT JOIN (select ContractId, sum(BillingAmount) as BillingAmount from ProjectBilling where BillingStatus = 7 and BillingType != '3' group by ContractId) pb ON pb.ContractId = t.Id 
                    left join (select ContractId,SUM(PaymentAmount) as PaymentAmount from ProjectPayment where PayType<>3 and PaymentStatus<>11 group by ContractId) pp on pp.ContractId=t.Id ");
            }
            return strSql.ToString();
        }
    }
}
