using Dapper;
using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
    /// 创 建：超级管理员
    /// 日 期：2022-03-16 17:56
    /// 描 述：项目回款管理
    /// </summary>
    public class ProjectPayCollectionService : RepositoryFactory
    {
        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">查询参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectPayCollectionVo> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.id,
                a.ProjectCode,
                a.ProjectName,
                t.ProjectId,
                t.CustName,
                t.ContractNo,
                t.Amount,
                t.ReceiptDate,
                t.PaymentUnit,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                a.FDepartmentId,
                a.PDepartmentId,
                pc.DepartmentId,
                t.UpdateUser,
                a.ProjectSource,
a.CreateUser as BCreateUser,
a.DepartmentId as BDepartmentId,py.PaymentAmount,pc.ContractAmount,pc.EffectiveAmount,lbu.F_RealName as FollowPersonName              
                ");
                // strSql.Append("  FROM ProjectPayCollection t inner join Project a on a.Id=t.ProjectId left join ProjectContract pc on pc.ProjectId=t.ProjectId");
                //strSql.Append(" FROM ProjectPayCollection t inner join Project a on a.Id=t.ProjectId   left join ProjectContract pc on a.Id=pc.ProjectId and pc.MainContract=1 and pc.ContractType=1 and pc.ContractStatus<>7 and pc.ContractStatus<>6 and pc.ContractStatus=4");
                strSql.Append(@"  FROM  ProjectPayCollection t inner join Project a on a.Id=t.ProjectId  
left join (SELECT ContractStatus, ProjectId, ContractType, DepartmentId , ContractNo, max(EffectiveAmount) as EffectiveAmount, sum(ContractAmount) as ContractAmount FROM ProjectContract WHERE ContractStatus = 4 AND ContractType = 1 and MainContract = 1 group by ContractStatus, ProjectId, ContractType, DepartmentId, ContractNo) pc on a.Id=pc.ProjectId ");
                strSql.Append("  left join (select SUM(PaymentAmount) as PaymentAmount,MAX(ProjectId) as ProjectId,MAX(PayType) as PayType from ProjectPayment where PayType<>3 group by ProjectId ) py on a.Id=py.ProjectId AND t.ContractNo = pc.ContractNo  ");
                strSql.Append("  left join adms706.dbo.lr_base_user lbu on lbu.F_UserId = a.FollowPerson ");
                strSql.Append("  where pc.ProjectId is not null ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                //if (!queryParam["ReceiptTimekeyword"].IsEmpty())
                //{
                if (!queryParam["ReceiptDateStartTime"].IsEmpty() && !queryParam["ReceiptDateEndTime"].IsEmpty())
                {
                    DateTime start = DateTime.Parse(queryParam["ReceiptDateStartTime"].ToString());
                    DateTime end = DateTime.Parse(queryParam["ReceiptDateEndTime"].ToString());
                    //dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    //dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.ReceiptDate >= '" + start + "' AND t.ReceiptDate <= '" + end + "' ) ");
                    //strSql.Append(" AND ( t.ReceiptDate >= @startTime AND t.ReceiptDate <= @endTime ) ");
                }


                //}
                //    else
                //{
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                    DateTime end = DateTime.Parse(queryParam["EndTime"].ToString());
                    //dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    //dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.CreateTime >= '" + start + "' AND t.CreateTime <= '" + end + "' ) ");
                    //strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                }
                //}
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or  a.CustName like'%{0}%'or  t.ContractNo like'%{0}%' or  t.Amount like'%{0}%' or  t.ReceiptDate like'%{0}%')", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectId ='{0}' )", queryParam["ProjectId"].ToString()));
                }
                if (!queryParam["ContractNo"].IsEmpty())
                {
                    // repair
                    strSql.Append(string.Format(" AND ( pc.ContractNo = '{0}' )", queryParam["ContractNo"].ToString()));
                    strSql.Append(string.Format(" AND ( t.ContractNo = '{0}' )", queryParam["ContractNo"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like '%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.Amount ='{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ReceiptDate ='{0}' )", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentUnit"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.PaymentUnit like '%{0}%' )", queryParam["PaymentUnit"].ToString()));
                }
                if (!queryParam["DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pc.DepartmentId like'%{0}%')", queryParam["DepartmentId"].ToString()));
                }

                //Debug.Write("-----------Page-----------");
                //Debug.WriteLine(strSql);

                var result = this.BaseRepository("learunOAWFForm").FindList<ProjectPayCollectionVo>(strSql.ToString(), dp, pagination);

                //var resultList = result.ToList();
                //Debug.WriteLine($"-----------查询结果条数：{resultList.Count}-----------");

                //if (resultList.Count > 0)
                //{
                //    for (int i = 0; i < resultList.Count; i++)
                //    {
                //        var item = resultList[i];
                //        Debug.WriteLine($"第{i + 1}条数据：ProjectName={item.ProjectName}, Amount={item.Amount}, CreateUser={item.CreateUser}");
                //    }
                //}

                //Debug.WriteLine(result);

                return result;
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
        /// <param name="pagination">查询参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectPayCollectionVo> GetPageListDepartmentId(Pagination pagination, string queryJson,string dep)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.id,
                a.ProjectCode,
                a.ProjectName,
                t.ProjectId,
                t.CustName,
                pc.ContractNo,
                t.Amount,
                t.ReceiptDate,
                t.PaymentUnit,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                a.FDepartmentId,
                a.PDepartmentId,
                pc.DepartmentId,
                t.UpdateUser,
                a.ProjectSource,
a.CreateUser as BCreateUser,
a.DepartmentId as BDepartmentId,py.PaymentAmount,pc.ContractAmount,pc.EffectiveAmount
      
               
                ");
                // strSql.Append("  FROM ProjectPayCollection t inner join Project a on a.Id=t.ProjectId left join ProjectContract pc on pc.ProjectId=t.ProjectId");
                //strSql.Append(" FROM ProjectPayCollection t inner join Project a on a.Id=t.ProjectId   left join ProjectContract pc on a.Id=pc.ProjectId and pc.MainContract=1 and pc.ContractType=1 and pc.ContractStatus<>7 and pc.ContractStatus<>6 and pc.ContractStatus=4");
                strSql.Append(@"  FROM  ProjectPayCollection t inner join Project a on a.Id=t.ProjectId  
left join (SELECT ContractStatus, ProjectId, ContractType, DepartmentId ,max(ContractNo) as ContractNo,max(EffectiveAmount) as EffectiveAmount, sum(ContractAmount) as ContractAmount FROM ProjectContract WHERE ContractStatus = 4 AND ContractType = 1 and MainContract = 1 group by ContractStatus, ProjectId, ContractType, DepartmentId) pc on a.Id=pc.ProjectId ");
                strSql.Append("  left join (select SUM(PaymentAmount) as PaymentAmount,MAX(ProjectId) as ProjectId,MAX(PayType) as PayType from ProjectPayment where PayType<>3 group by ProjectId ) py on a.Id=py.ProjectId ");
                strSql.Append("  where pc.ProjectId is not null");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                //if (!queryParam["ReceiptTimekeyword"].IsEmpty())
                //{
                if (!queryParam["ReceiptDateStartTime"].IsEmpty() && !queryParam["ReceiptDateEndTime"].IsEmpty())
                {
                    DateTime start = DateTime.Parse(queryParam["ReceiptDateStartTime"].ToString());
                    DateTime end = DateTime.Parse(queryParam["ReceiptDateEndTime"].ToString());
                    //dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    //dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.ReceiptDate >= '" + start + "' AND t.ReceiptDate <= '" + end + "' ) ");
                    //strSql.Append(" AND ( t.ReceiptDate >= @startTime AND t.ReceiptDate <= @endTime ) ");
                }

                //}
                //    else
                //{
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                    DateTime end = DateTime.Parse(queryParam["EndTime"].ToString());
                    //dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    //dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.CreateTime >= '" + start + "' AND t.CreateTime <= '" + end + "' ) ");
                    //strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                }
                //}


                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or  a.CustName like'%{0}%'or  t.ContractNo like'%{0}%' or  t.Amount like'%{0}%' or  t.ReceiptDate like'%{0}%')", queryParam["keyword"].ToString()));
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
                    strSql.Append(string.Format(" AND ( a.ProjectName like '%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.Amount ='{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ReceiptDate ='{0}' )", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentUnit"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.PaymentUnit like '%{0}%' )", queryParam["PaymentUnit"].ToString()));
                }
                strSql.Append(string.Format(" AND ( " + dep + " )"));
                return this.BaseRepository("learunOAWFForm").FindList<ProjectPayCollectionVo>(strSql.ToString(), dp, pagination);
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
        /// 根据报备id查询回款信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<ProjectPayCollectionVo> GetPayCollection(string id)
        {
            try
            {
                var strSql = new StringBuilder();

                strSql.Append("select * FROM ProjectPayCollection t where t.ProjectId='" + id + "'");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectPayCollectionVo>(strSql.ToString());
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
        /// 合计
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProjectPayCollectionVo> GetPageListSUM(string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
//                strSql.Append("SELECT ");
//                strSql.Append(@"
//       t.id,
//                a.ProjectCode,
//                a.ProjectName,
//                t.ProjectId,
//                t.CustName,
//                pc.ContractNo,
//                t.Amount,
//                t.ReceiptDate,
//                t.PaymentUnit,
//                t.CreateTime,
//                t.CreateUser,
//                t.UpdateTime,
//                a.FDepartmentId,
//                a.PDepartmentId,
//                pc.DepartmentId,
//                t.UpdateUser,
//                a.ProjectSource,
//a.CreateUser as BCreateUser,
//a.DepartmentId as BDepartmentId,py.PaymentAmount,pc.ContractAmount
      
//                ");

                //                strSql.Append(@" FROM  ProjectPayCollection t inner join Project a on a.Id=t.ProjectId   
                //                                left join (select  SUM(PaymentAmount) as PaymentAmount,ProjectId from ProjectPayment  GROUP BY ProjectId  ) t1 on t1.ProjectId=a.Id 
                //left join (select ContractStatus,ProjectId,ContractType,DepartmentId from ProjectContract where MainContract=1 and ContractType=1 and ContractStatus<>7 and ContractStatus<>6 and ContractStatus=4 group by ContractStatus, ProjectId, ContractType, DepartmentId ) pc on a.Id=pc.ProjectId 
                //where pc.ProjectId is not null ");
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.id,
                a.ProjectCode,
                a.ProjectName,
                t.ProjectId,
                t.CustName,
                pc.ContractNo,
                t.Amount,
                t.ReceiptDate,
                t.PaymentUnit,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                a.FDepartmentId,
                a.PDepartmentId,
                pc.DepartmentId,
                t.UpdateUser,
                a.ProjectSource,
a.CreateUser as BCreateUser,
a.DepartmentId as BDepartmentId,py.PaymentAmount,pc.ContractAmount,pc.EffectiveAmount, '-1' as ProjectResponsible,lbu.F_RealName as FollowPersonName ");
                // strSql.Append("  FROM ProjectPayCollection t inner join Project a on a.Id=t.ProjectId left join ProjectContract pc on pc.ProjectId=t.ProjectId");
                //strSql.Append(" FROM ProjectPayCollection t inner join Project a on a.Id=t.ProjectId   left join ProjectContract pc on a.Id=pc.ProjectId and pc.MainContract=1 and pc.ContractType=1 and pc.ContractStatus<>7 and pc.ContractStatus<>6 and pc.ContractStatus=4");
                strSql.Append(@"  FROM  ProjectPayCollection t inner join Project a on a.Id=t.ProjectId  
left join (SELECT ContractStatus, ProjectId, ContractType, DepartmentId ,max(ContractNo) as ContractNo,max(EffectiveAmount) as EffectiveAmount, sum(ContractAmount) as ContractAmount FROM ProjectContract WHERE ContractStatus = 4 AND ContractType = 1 and MainContract = 1 group by ContractStatus, ProjectId, ContractType, DepartmentId) pc on a.Id=pc.ProjectId ");
                strSql.Append("  left join (select SUM(PaymentAmount) as PaymentAmount,MAX(ProjectId) as ProjectId,MAX(PayType) as PayType from ProjectPayment where PayType<>3 group by ProjectId ) py on a.Id=py.ProjectId ");
                strSql.Append("  left join adms706.dbo.lr_base_user lbu on lbu.F_UserId = a.FollowPerson ");
                strSql.Append("  where pc.ProjectId is not null");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                //if (!queryParam["ReceiptTimekeyword"].IsEmpty())
                //{
                //    if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                //    {
                //        DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                //        DateTime end = DateTime.Parse(queryParam["EndTime"].ToString());
                //        //dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                //        //dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                //        strSql.Append(" AND ( t.ReceiptDate >= '" + start + "' AND t.ReceiptDate <= '" + end + "' ) ");
                //        //strSql.Append(" AND ( t.ReceiptDate >= @startTime AND t.ReceiptDate <= @endTime ) ");
                //    }
                //}
                //else
                //{
                //    if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                //    {
                //        DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                //        DateTime end = DateTime.Parse(queryParam["EndTime"].ToString()).AddDays(1);
                //        //dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                //        //dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                //        strSql.Append(" AND ( t.CreateTime >= '" + start + "' AND t.CreateTime <= '" + end + "' ) ");
                //        //strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                //    }
                //}
                if (!queryParam["ReceiptDateStartTime"].IsEmpty() && !queryParam["ReceiptDateEndTime"].IsEmpty())
                {
                    DateTime start = DateTime.Parse(queryParam["ReceiptDateStartTime"].ToString());
                    DateTime end = DateTime.Parse(queryParam["ReceiptDateEndTime"].ToString());
                    //dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    //dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND  t.ReceiptDate >= '" + start + "' AND t.ReceiptDate <= '" + end + "'  ");
                    //strSql.Append(" AND ( t.ReceiptDate >= @startTime AND t.ReceiptDate <= @endTime ) ");
                }

                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {

                        DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                        DateTime end = DateTime.Parse(queryParam["EndTime"].ToString());
                        //dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                        //dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                        strSql.Append(" AND  t.CreateTime >= '" + start + "' AND t.CreateTime <= '" + end + "'  ");
                        //strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND  a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or  a.CustName like'%{0}%'or  t.ContractNo like'%{0}%' or  t.Amount like'%{0}%' or  t.ReceiptDate like'%{0}%'", queryParam["keyword"].ToString()));
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
                    strSql.Append(string.Format(" AND ( a.ProjectName like '%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.Amount ='{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ReceiptDate ='{0}' )", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentUnit"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.PaymentUnit like '%{0}%' )", queryParam["PaymentUnit"].ToString()));
                }
                if (!queryParam["DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( pc.DepartmentId like'%{0}%')", queryParam["DepartmentId"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectPayCollectionVo>(strSql.ToString(), dp);
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
        /// 合计多部门
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProjectPayCollectionVo> GetPageListSUMDepartmentId(string queryJson,string dep)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.id,
                a.ProjectCode,
                a.ProjectName,
                t.ProjectId,
                t.CustName,
                pc.ContractNo,
                t.Amount,
                t.ReceiptDate,
                t.PaymentUnit,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                a.FDepartmentId,
                a.PDepartmentId,
                pc.DepartmentId,
                t.UpdateUser,
                a.ProjectSource,
a.CreateUser as BCreateUser,
a.DepartmentId as BDepartmentId,py.PaymentAmount,pc.ContractAmount,pc.EffectiveAmount
      
               
                ");
             
                strSql.Append(@"  FROM  ProjectPayCollection t inner join Project a on a.Id=t.ProjectId  
left join (SELECT ContractStatus, ProjectId, ContractType, DepartmentId ,max(ContractNo) as ContractNo,max(EffectiveAmount) as EffectiveAmount, sum(ContractAmount) as ContractAmount FROM ProjectContract WHERE ContractStatus = 4 AND ContractType = 1 and MainContract = 1 group by ContractStatus, ProjectId, ContractType, DepartmentId) pc on a.Id=pc.ProjectId ");
                strSql.Append("  left join (select SUM(PaymentAmount) as PaymentAmount,MAX(ProjectId) as ProjectId,MAX(PayType) as PayType from ProjectPayment where PayType<>3 group by ProjectId ) py on a.Id=py.ProjectId ");
                strSql.Append("  where pc.ProjectId is not null");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
               
                if (!queryParam["ReceiptDateStartTime"].IsEmpty() && !queryParam["ReceiptDateEndTime"].IsEmpty())
                {
                    DateTime start = DateTime.Parse(queryParam["ReceiptDateStartTime"].ToString());
                    DateTime end = DateTime.Parse(queryParam["ReceiptDateEndTime"].ToString());
                   
                    strSql.Append(" AND  t.ReceiptDate >= '" + start + "' AND t.ReceiptDate <= '" + end + "'  ");
                    
                }
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {

                        DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                        DateTime end = DateTime.Parse(queryParam["EndTime"].ToString());
                        //dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                        //dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                        strSql.Append(" AND  t.CreateTime >= '" + start + "' AND t.CreateTime <= '" + end + "'  ");
                        //strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND  a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or  a.CustName like'%{0}%'or  t.ContractNo like'%{0}%' or  t.Amount like'%{0}%' or  t.ReceiptDate like'%{0}%'", queryParam["keyword"].ToString()));
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
                    strSql.Append(string.Format(" AND ( a.ProjectName like '%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.Amount ='{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ReceiptDate ='{0}' )", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentUnit"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.PaymentUnit like '%{0}%' )", queryParam["PaymentUnit"].ToString()));
                }
                strSql.Append(string.Format(" AND ( " + dep + " )"));
                return this.BaseRepository("learunOAWFForm").FindList<ProjectPayCollectionVo>(strSql.ToString(), dp);
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
        public IEnumerable<ProjectPayCollectionVo> GetPageList(string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                 t.id,
                a.ProjectCode,
                a.ProjectName,
                t.ProjectId,
                t.CustName,
                pc.ContractNo,
                t.Amount,
                t.ReceiptDate,
                t.PaymentUnit,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                a.FDepartmentId,
                a.PDepartmentId,
                pc.DepartmentId,
                t.UpdateUser,
                a.ProjectSource,
                a.CreateUser as BCreateUser,
                a.DepartmentId as BDepartmentId,pc.ContractAmount
                ");
                // strSql.Append("  FROM ProjectPayCollection t inner join Project a on a.Id=t.ProjectId left join ProjectContract pc on pc.ProjectId=t.ProjectId");
                //strSql.Append(" FROM ProjectPayCollection t inner join Project a on a.Id=t.ProjectId   left join ProjectContract pc on a.Id=pc.ProjectId and pc.MainContract=1 and pc.ContractType=1 and pc.ContractStatus<>7 and pc.ContractStatus<>6");
                strSql.Append(@" FROM  ProjectPayCollection t inner join Project a on a.Id=t.ProjectId   
left join (select ContractStatus,ProjectId,ContractType,DepartmentId from ProjectContract where MainContract=1 and ContractType=1 and ContractStatus<>7 and ContractStatus<>6 and ContractStatus=4 group by ContractStatus, ProjectId, ContractType, DepartmentId ) pc on a.Id=pc.ProjectId  ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                //if (!queryParam["ReceiptTimekeyword"].IsEmpty())
                //{
                //    if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                //    {
                //        DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                //        DateTime end = DateTime.Parse(queryParam["EndTime"].ToString());
                //        //dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                //        //dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                //        strSql.Append(" AND ( t.ReceiptDate >= '" + start + "' AND t.ReceiptDate <= '" + end + "' ) ");
                //        //strSql.Append(" AND ( t.ReceiptDate >= @startTime AND t.ReceiptDate <= @endTime ) ");
                //    }
                //}
                //else
                //{
                //    if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                //    {
                //        DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                //        DateTime end = DateTime.Parse(queryParam["EndTime"].ToString()).AddDays(1);
                //        //dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                //        //dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                //        strSql.Append(" AND ( t.CreateTime >= '" + start + "' AND t.CreateTime <= '" + end + "' ) ");
                //        //strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                //    }
                //}

                if (!queryParam["ReceiptDateStartTime"].IsEmpty() && !queryParam["ReceiptDateEndTime"].IsEmpty())
                {
                    DateTime start = DateTime.Parse(queryParam["ReceiptDateStartTime"].ToString());
                    DateTime end = DateTime.Parse(queryParam["ReceiptDateEndTime"].ToString());
                    //dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    //dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.ReceiptDate >= '" + start + "' AND t.ReceiptDate <= '" + end + "' ) ");
                    //strSql.Append(" AND ( t.ReceiptDate >= @startTime AND t.ReceiptDate <= @endTime ) ");
                }
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                    DateTime end = DateTime.Parse(queryParam["EndTime"].ToString()).AddDays(1);
                    //dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    //dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.CreateTime >= '" + start + "' AND t.CreateTime <= '" + end + "' ) ");
                    //strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectName like'%{0}%'   or  a.ProjectCode like'%{0}%' or  a.CustName like'%{0}%'or  t.ContractNo like'%{0}%' or  t.Amount like'%{0}%' or  t.ReceiptDate like'%{0}%')", queryParam["keyword"].ToString()));
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
                    strSql.Append(string.Format(" AND ( a.ProjectName like '%{0}%' )", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.CustName like '%{0}%' )", queryParam["CustName"].ToString()));
                }
                if (!queryParam["Amount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.Amount ='{0}' )", queryParam["Amount"].ToString()));
                }
                if (!queryParam["ReceiptDate"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ReceiptDate ='{0}' )", queryParam["ReceiptDate"].ToString()));
                }
                if (!queryParam["PaymentUnit"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.PaymentUnit like '%{0}%' )", queryParam["PaymentUnit"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectPayCollectionVo>(strSql.ToString(), dp);
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
        /// <param name="pagination">查询参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public ProjectPayCollectionVo GetPreviewProjectPayCollectionById(string keyValue)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.id,
                a.ProjectName,
                t.ProjectId,
                t.CustName,
                pc.ContractNo,
                t.Amount,
                t.ReceiptDate,
                t.PaymentUnit,
                t.CreateTime,
                t.CreateUser,
                t.UpdateTime,
                t.UpdateUser,
                 a.ProjectSource,
                pc.DepartmentId
                ");
                strSql.Append("  FROM ProjectPayCollection t inner join Project a on a.Id=t.ProjectId left join ProjectContract pc on pc.ProjectId=t.ProjectId");
                strSql.Append("  WHERE t.id='" + keyValue + "'");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectPayCollectionVo>(strSql.ToString()).FirstOrDefault();
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
        /// 根据projectId查询对应的回款
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public ProjectPayCollectionVo GetCollectionByIdProjectId(string ProjectId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"      
                   
                sum(t.Amount) as Amount         
                ");
                strSql.Append("  FROM ProjectPayCollection t ");
                strSql.Append("  WHERE t.ProjectId='" + ProjectId + "'");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectPayCollectionVo>(strSql.ToString()).FirstOrDefault();
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
        public ProjectPayCollectionVo GetCollectionByIdProjectIdtIME(string ProjectId,string str)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"                         
                sum(t.Amount) as Amount         
                ");
                strSql.Append("  FROM ProjectPayCollection t  inner join Project a on a.Id=t.ProjectId ");
                strSql.Append("  WHERE a.ProjectSource<>3 and t.ReceiptDate<'" + str+"' and t.ProjectId='" + ProjectId + "'");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectPayCollectionVo>(strSql.ToString()).FirstOrDefault();
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
        public ProjectPayCollectionVo GetPageListsum(string ProjectId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"      
               t.ProjectId,
                t.Amount,
               t.CreateTime,
              t.ReceiptDate,
               t.CreateUser,
               a.FDepartmentId,
                a.PDepartmentId,
             pc.DepartmentId,
 a.ProjectName,
a.CreateUser as BCreateUser,
a.DepartmentId as BDepartmentId,
a.ProjectSource,t1.PaymentAmount      
                ");
                strSql.Append("FROM  ProjectPayCollection t inner join Project a on a.Id=t.ProjectId   left join (select  SUM(PaymentAmount) as PaymentAmount,ProjectId from ProjectPayment  GROUP BY ProjectId  ) t1 on t1.ProjectId=a.Id  left join (select ContractStatus,ProjectId,ContractType,DepartmentId from ProjectContract where MainContract=1 ) pc on a.Id=pc.ProjectId ");
                strSql.Append("  WHERE t.ProjectId='" + ProjectId + "'");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectPayCollectionVo>(strSql.ToString()).FirstOrDefault();
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
        public List<ProjectPayCollectionEntity> GetProjectPayCollectionByProjectId(string projectId)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindList<ProjectPayCollectionEntity>(t => t.ProjectId == projectId).AsList();
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
        /// 获取ProjectPayCollection表实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public ProjectPayCollectionEntity GetProjectPayCollectionEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindEntity<ProjectPayCollectionEntity>(keyValue);
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
                this.BaseRepository("learunOAWFForm").Delete<ProjectPayCollectionEntity>(t => t.id == keyValue);
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
        public void SaveEntity(string keyValue, ProjectPayCollectionEntity entity)
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

        #endregion

    }
}
