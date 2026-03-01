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
    /// 日 期：2022-03-10 22:29
    /// 描 述：项目管理
    /// </summary>
    public class ProjectManageService : RepositoryFactory
    {

        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.ProjectCode,
                t.ProjectName,
                t.CustName,
                t.ContactName,
                t.ContactPhone,
                t.Address,
                t.ProvinceId,
                t.CityId,
                t.CountyId,
                t.ProjectSituation,
                t.Remark,
                t.PreparedPerson,
                t.FollowPerson,
                t.FCompanyId,
                t.FDepartmentId,
                t.PCompanyId,
                t.PDepartmentId,
                t.CompanyId,
                t.DepartmentId,
                t.ProjectStatus,
                t.CreateTime,
                t.TenderFlg,
                t.ProjectSource,
                t.CreateUser
             ");
                strSql.Append("  FROM Project t ");
                strSql.Append("  WHERE 1=1 ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (queryParam["DepartmentId"].IsEmpty())
                {
                    if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                    {
                        dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                        dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                        strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                    }
                    if (!queryParam["keyword"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%'   or  t.ProjectCode like'%{0}%'  or  t.CustName like'%{0}%'  or  t.ContactName like'%{0}%' or  t.ContactPhone like'%{0}%' or t.Address like'%{0}%')", queryParam["keyword"].ToString()));
                    }
                    if (!queryParam["ProjectStatus"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectStatus = {0})", queryParam["ProjectStatus"].ToString()));
                    }
                    if (!queryParam["ProjectName"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%')", queryParam["ProjectName"].ToString()));
                    }
                    if (!queryParam["ProjectCode"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectCode like'%{0}%')", queryParam["ProjectCode"].ToString()));
                    }
                    if (!queryParam["CustName"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.CustName like'%{0}%')", queryParam["CustName"].ToString()));
                    }
                    if (!queryParam["ContactPhone"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ContactPhone like'%{0}%')", queryParam["ContactPhone"].ToString()));
                    }
                    if (!queryParam["PreparedPerson"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.PreparedPerson like'%{0}%')", queryParam["PreparedPerson"].ToString()));
                    }
                    if (!queryParam["FollowPerson"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.FollowPerson like'%{0}%')", queryParam["FollowPerson"].ToString()));
                    }
                    if (!queryParam["ProjectStatus"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectStatus like'%{0}%')", queryParam["ProjectStatus"].ToString()));
                    }
                }
                else
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId ='')"));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectEntity>(strSql.ToString(), dp, pagination);
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
        public IEnumerable<ProjectVo> GetPageListAPI(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.ProjectCode,
                t.ProjectName,
                t.CustName,
                t.ContactName,
                t.ContactPhone,
                t.Address,
                t.ProvinceId,
                t.CityId,
                t.CountyId,
                t.ProjectSituation,
                t.Remark,
                t.PreparedPerson,
                t.FollowPerson,
                t.FCompanyId,
                t.FDepartmentId,
                t.PCompanyId,
                t.PDepartmentId,
                t.CompanyId,
                t.DepartmentId,
                t.ProjectStatus,
                t.CreateTime,
                t.TenderFlg,
                t.ProjectSource,
                t.CreateUser
             ");
                strSql.Append("  FROM Project t ");
                strSql.Append("  WHERE 1=1 ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                /* if (queryParam["DepartmentId"].IsEmpty())
                 {*/
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%'   or  t.ProjectCode like'%{0}%'  or  t.CustName like'%{0}%'  or  t.ContactName like'%{0}%' or  t.ContactPhone like'%{0}%' or t.Address like'%{0}%')", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectStatus = {0})", queryParam["ProjectStatus"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%')", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["ProjectCode"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectCode like'%{0}%')", queryParam["ProjectCode"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.CustName like'%{0}%')", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContactPhone"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContactPhone like'%{0}%')", queryParam["ContactPhone"].ToString()));
                }
                if (!queryParam["PreparedPerson"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.PreparedPerson like'%{0}%')", queryParam["PreparedPerson"].ToString()));
                }
                if (!queryParam["FollowPerson"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.FollowPerson like'%{0}%')", queryParam["FollowPerson"].ToString()));
                }
                if (!queryParam["ProjectStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectStatus like'%{0}%')", queryParam["ProjectStatus"].ToString()));
                }
                /*   }
                   else
                   {
                       strSql.Append(string.Format(" AND ( t.DepartmentId ='')"));
                   }*/
                return this.BaseRepository("learunOAWFForm").FindList<ProjectVo>(strSql.ToString(), dp, pagination);
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
        /// 多部门获取页面显示列表数据历史信息
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectVo> GetPageListDepartmentId(Pagination pagination, string queryJson, string dep)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.ProjectCode,
                t.ProjectName,
                t.CustName,
                t.ContactName,
                t.ContactPhone,
                t.Address,
                t.ProvinceId,
                t.CityId,
                t.CountyId,
                t.ProjectSituation,
                t.Remark,
                t.PreparedPerson,
                t.FollowPerson,
                t.FCompanyId,
                t.FDepartmentId,
                t.PCompanyId,
                t.PDepartmentId,
                t.CompanyId,
                t.DepartmentId,
                t.ProjectStatus,
                t.CreateTime,
                t.TenderFlg,
                t.ProjectSource,
                t.CreateUser
             ");
                strSql.Append("  FROM Project t ");
                strSql.Append("  WHERE 1=1 ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (queryParam["DepartmentId"].IsEmpty())
                {
                    if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                    {
                        dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                        dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                        strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                    }
                    if (!queryParam["keyword"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%'   or  t.ProjectCode like'%{0}%'  or  t.CustName like'%{0}%'  or  t.ContactName like'%{0}%' or  t.ContactPhone like'%{0}%' or t.Address like'%{0}%')", queryParam["keyword"].ToString()));
                    }
                    if (!queryParam["ProjectStatus"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectStatus = {0})", queryParam["ProjectStatus"].ToString()));
                    }
                    if (!queryParam["ProjectName"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%')", queryParam["ProjectName"].ToString()));
                    }
                    if (!queryParam["ProjectCode"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectCode like'%{0}%')", queryParam["ProjectCode"].ToString()));
                    }
                    if (!queryParam["CustName"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.CustName like'%{0}%')", queryParam["CustName"].ToString()));
                    }
                    if (!queryParam["ContactPhone"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ContactPhone like'%{0}%')", queryParam["ContactPhone"].ToString()));
                    }
                    if (!queryParam["PreparedPerson"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.PreparedPerson like'%{0}%')", queryParam["PreparedPerson"].ToString()));
                    }
                    if (!queryParam["FollowPerson"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.FollowPerson like'%{0}%')", queryParam["FollowPerson"].ToString()));
                    }
                    if (!queryParam["ProjectStatus"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectStatus like'%{0}%')", queryParam["ProjectStatus"].ToString()));
                    }
                    strSql.Append(string.Format(" AND ( " + dep + " )"));
                }
                else
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId ='')"));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectVo>(strSql.ToString(), dp, pagination);
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
        /// 获取月份的数据
        /// </summary>
        /// <returns></returns>
        public List<ProjectDpVo> GetProjectMonthCount()
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                 ProjectStatus,convert(varchar(20),year(CreateTime)) +'-'+ convert(varchar(20),month(CreateTime)) as ProjectMonth,COUNT(ProjectStatus) as ProjectCount  
                from Project where ProjectStatus<>3 group by ProjectStatus,convert(varchar(20),year(CreateTime)) +'-'+ convert(varchar(20),month(CreateTime)) 
                order by convert(varchar(20),year(CreateTime)) +'-'+ convert(varchar(20),month(CreateTime)),ProjectStatus ");

                return this.BaseRepository("learunOAWFForm").FindList<ProjectDpVo>(strSql.ToString()).ToList();
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
        /// 根据项目来源来统计项目数量
        /// </summary>
        /// <returns></returns>
        public List<ProjectSourceVo> GetProjectCountBySource()
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" ProjectSource,COUNT(ProjectStatus) as ProjectCount  
                  from Project where ProjectSource is not null group by ProjectSource order by ProjectSource");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectSourceVo>(strSql.ToString()).ToList();
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
        /// 根据省份获取项目统计数量
        /// </summary>
        /// <returns></returns>
        public List<ProjectProvinceVo> GetProjectCountByProvince()
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"  ProvinceId,COUNT(ProvinceId) as ProjectCount from Project where ISNULL(ProvinceId,'')<>'' group by ProvinceId order  by  COUNT(ProvinceId)  desc");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectProvinceVo>(strSql.ToString()).ToList();
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

        public List<ProjectConversionVo> GetProjectConversion()
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"  CONVERT(varchar(100), CreateTime, 23) ProjectDay,COUNT(id) TotalCount,sum(case when ProjectStatus=1 then 1 else 0 end ) as SucessCount 
from Project  group by CONVERT(varchar(100), CreateTime, 23)");
                List<ProjectConversionVo> projectConversionVos = this.BaseRepository("learunOAWFForm").FindList<ProjectConversionVo>(strSql.ToString()).ToList();
                foreach (var item in projectConversionVos)
                {
                    if (item.TotalCount > 0)
                    {
                        item.ConversionRate = Math.Round((item.SucessCount / (item.TotalCount * 1.00)) * 100, 2);
                    }
                }
                return projectConversionVos;
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

        public List<ProjectPaymentBackVo> GetBackProjectRate()
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"  a.DepartmentId,SUM(ISNUll(a.ContractAmount,0)) as ContractAmount, SUM(ISNUll(b.Amount,0)) as CollectionAmount from ProjectContract a
                             left join ProjectPayCollection b on a.ProjectId =b.ProjectId where a.ContractStatus=4 group by DepartmentId order by SUM(ISNUll(a.ContractAmount,0))  desc");
                List<ProjectPaymentBackVo> projectConversionVos = this.BaseRepository("learunOAWFForm").FindList<ProjectPaymentBackVo>(strSql.ToString()).ToList();
                foreach (var item in projectConversionVos)
                {
                    if (item.CollectionAmount > 0)
                    {
                        item.CollectionAmount = Math.Round(item.CollectionAmount, 2);
                        item.ContractAmount = Math.Round(item.ContractAmount, 2);
                        item.CollectionRate = Math.Round((item.CollectionAmount / (item.ContractAmount * 1.00)) * 100, 2);
                    }
                }
                return projectConversionVos;
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
        public IEnumerable<ProjectVo> GetPageListAddress(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.ProjectCode,
                t.ProjectName,
                t.CustName,
                t.ContactName,
                t.ContactPhone,
                t.Address,
                t.ProvinceId,
                t.CityId,
                t.CountyId,
                t.ProjectSituation,
                t.Remark,
                t.PreparedPerson,
                t.FollowPerson,
                t.FCompanyId,
                t.FDepartmentId,
                t.PCompanyId,
                t.PDepartmentId,
                t.CompanyId,
                t.DepartmentId,
                t.ProjectStatus,
                t.CreateTime,
                t.TenderFlg,
                t.ProjectSource,
                t.CreateUser

             ");
                strSql.Append("  FROM Project t left join  (select ProjectId from ProjectContract group by ProjectId) a  on t.Id=a.ProjectId  ");
                strSql.Append("  WHERE 1=1 ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (queryParam["DepartmentId"].IsEmpty())
                {
                    if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                    {
                        dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                        dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                        strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                    }
                    if (!queryParam["keyword"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%'   or  t.ProjectCode like'%{0}%'  or  t.CustName like'%{0}%'  or  t.ContactName like'%{0}%' or  t.ContactPhone like'%{0}%' or t.Address like'%{0}%')", queryParam["keyword"].ToString()));
                    }
                    if (!queryParam["cs"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( a.ProjectId is null)", queryParam["cs"].ToString()));
                    }
                    if (!queryParam["ProjectStatus"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectStatus = {0})", queryParam["ProjectStatus"].ToString()));
                    }
                    if (!queryParam["ProjectName"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%')", queryParam["ProjectName"].ToString()));
                    }
                    if (!queryParam["ProjectCode"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectCode like'%{0}%')", queryParam["ProjectCode"].ToString()));
                    }
                    if (!queryParam["CustName"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.CustName like'%{0}%')", queryParam["CustName"].ToString()));
                    }
                    if (!queryParam["ContactPhone"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ContactPhone like'%{0}%')", queryParam["ContactPhone"].ToString()));
                    }
                    if (!queryParam["PreparedPerson"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.PreparedPerson like'%{0}%')", queryParam["PreparedPerson"].ToString()));
                    }
                    if (!queryParam["FollowPerson"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.FollowPerson like'%{0}%')", queryParam["FollowPerson"].ToString()));
                    }
                    if (!queryParam["ProjectStatus"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectStatus like'%{0}%')", queryParam["ProjectStatus"].ToString()));
                    }
                }
                else
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId ='')"));
                }
                //strSql.Append(string.Format("  order by t.CreateTime desc "));
                return this.BaseRepository("learunOAWFForm").FindList<ProjectVo>(strSql.ToString(), dp, pagination);
                //return this.BaseRepository("learunOAWFForm").FindList<ProjectVo>(strSql.ToString(), dp);
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
        public IEnumerable<ProjectVo> GetPageListAddressDepartmentId(Pagination pagination, string queryJson, string dep)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.ProjectCode,
                t.ProjectName,
                t.CustName,
                t.ContactName,
                t.ContactPhone,
                t.Address,
                t.ProvinceId,
                t.CityId,
                t.CountyId,
                t.ProjectSituation,
                t.Remark,
                t.PreparedPerson,
                t.FollowPerson,
                t.FCompanyId,
                t.FDepartmentId,
                t.PCompanyId,
                t.PDepartmentId,
                t.CompanyId,
                t.DepartmentId,
                t.ProjectStatus,
                t.CreateTime,
                t.TenderFlg,
                t.ProjectSource,
                t.CreateUser,
u.F_HZ,u.F_DepartmentId
             ");
                strSql.Append("  FROM Project t inner join adms706.dbo.lr_base_user u on u.F_UserId=t.CreateUser ");
                strSql.Append("  WHERE 1=1");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (queryParam["DepartmentId"].IsEmpty())
                {
                    if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                    {
                        dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                        dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                        strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                    }
                    if (!queryParam["keyword"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%'   or  t.ProjectCode like'%{0}%'  or  t.CustName like'%{0}%'  or  t.ContactName like'%{0}%' or  t.ContactPhone like'%{0}%' or t.Address like'%{0}%')", queryParam["keyword"].ToString()));
                    }
                    if (!queryParam["ProjectStatus"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectStatus = {0})", queryParam["ProjectStatus"].ToString()));
                    }
                    if (!queryParam["ProjectName"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%')", queryParam["ProjectName"].ToString()));
                    }
                    if (!queryParam["ProjectCode"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectCode like'%{0}%')", queryParam["ProjectCode"].ToString()));
                    }
                    if (!queryParam["CustName"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.CustName like'%{0}%')", queryParam["CustName"].ToString()));
                    }
                    if (!queryParam["ContactPhone"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ContactPhone like'%{0}%')", queryParam["ContactPhone"].ToString()));
                    }
                    if (!queryParam["PreparedPerson"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.PreparedPerson like'%{0}%')", queryParam["PreparedPerson"].ToString()));
                    }
                    if (!queryParam["FollowPerson"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.FollowPerson like'%{0}%')", queryParam["FollowPerson"].ToString()));
                    }
                    if (!queryParam["ProjectStatus"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectStatus like'%{0}%')", queryParam["ProjectStatus"].ToString()));
                    }
                    strSql.Append(string.Format(" AND ( t.DepartmentId='" + dep + "' or t.FDepartmentId='" + dep + "' or t.PDepartmentId='" + dep + "')"));

                }
                else
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId ='')"));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectVo>(strSql.ToString(), dp, pagination);
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
        public IEnumerable<ProjectVo> GetPageListAddressDepartmentIds(Pagination pagination, string queryJson, string dep)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.ProjectCode,
                t.ProjectName,
                t.CustName,
                t.ContactName,
                t.ContactPhone,
                t.Address,
                t.ProvinceId,
                t.CityId,
                t.CountyId,
                t.ProjectSituation,
                t.Remark,
                t.PreparedPerson,
                t.FollowPerson,
                t.FCompanyId,
                t.FDepartmentId,
                t.PCompanyId,
                t.PDepartmentId,
                t.CompanyId,
                t.DepartmentId,
                t.ProjectStatus,
                t.CreateTime,
                t.TenderFlg,
                t.ProjectSource,
                t.CreateUser
             ");
                //  strSql.Append("  FROM Project t inner join adms706.dbo.lr_base_user u on u.F_UserId=t.CreateUser ");
                strSql.Append("  FROM  Project t left join  (select ProjectId from ProjectContract group by ProjectId) a  on t.Id=a.ProjectId  ");
                strSql.Append("  WHERE 1=1");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (queryParam["DepartmentId"].IsEmpty())
                {
                    if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                    {
                        dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                        dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                        strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                    }
                    if (!queryParam["keyword"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%'   or  t.ProjectCode like'%{0}%'  or  t.CustName like'%{0}%'  or  t.ContactName like'%{0}%' or  t.ContactPhone like'%{0}%' or t.Address like'%{0}%')", queryParam["keyword"].ToString()));
                    }
                    if (!queryParam["ProjectStatus"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectStatus = {0})", queryParam["ProjectStatus"].ToString()));
                    }
                    if (!queryParam["cs"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( a.ProjectId is null)", queryParam["cs"].ToString()));
                    }
                    if (!queryParam["ProjectName"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%')", queryParam["ProjectName"].ToString()));
                    }
                    if (!queryParam["ProjectCode"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectCode like'%{0}%')", queryParam["ProjectCode"].ToString()));
                    }
                    if (!queryParam["CustName"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.CustName like'%{0}%')", queryParam["CustName"].ToString()));
                    }
                    if (!queryParam["ContactPhone"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ContactPhone like'%{0}%')", queryParam["ContactPhone"].ToString()));
                    }
                    if (!queryParam["PreparedPerson"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.PreparedPerson like'%{0}%')", queryParam["PreparedPerson"].ToString()));
                    }
                    if (!queryParam["FollowPerson"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.FollowPerson like'%{0}%')", queryParam["FollowPerson"].ToString()));
                    }
                    if (!queryParam["ProjectStatus"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectStatus like'%{0}%')", queryParam["ProjectStatus"].ToString()));
                    }


                }
                else
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId ='')"));
                }
                strSql.Append(string.Format(" AND ( " + dep + " )"));
                return this.BaseRepository("learunOAWFForm").FindList<ProjectVo>(strSql.ToString(), dp, pagination);
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
        /// 获取页面导出显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectVo> GetPageListAddress(string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.ProjectCode,
                t.ProjectName,
                t.CustName,
                t.ContactName,
                t.ContactPhone,
                t.Address,
                t.ProvinceId,
                t.CityId,
                t.CountyId,
                t.ProjectSituation,
                t.Remark,
                t.PreparedPerson,
                t.FollowPerson,
                t.FCompanyId,
                t.FDepartmentId,
                t.PCompanyId,
                t.PDepartmentId,
                t.CompanyId,
                t.DepartmentId,
                t.ProjectStatus,
                t.CreateTime,
                t.TenderFlg,
                t.ProjectSource,
                t.CreateUser
             ");
                strSql.Append("  FROM Project t left join  (select ProjectId from ProjectContract group by ProjectId) a  on t.Id=a.ProjectId  ");
                strSql.Append("  WHERE 1=1 ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (queryParam["DepartmentId"].IsEmpty())
                {
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
                        strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%'   or  t.ProjectCode like'%{0}%'  or  t.CustName like'%{0}%'  or  t.ContactName like'%{0}%' or  t.ContactPhone like'%{0}%' or t.Address like'%{0}%')", queryParam["keyword"].ToString()));
                    }
                    if (!queryParam["cs"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( a.ProjectId is null)", queryParam["cs"].ToString()));
                    }
                    if (!queryParam["ProjectStatus"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectStatus = {0})", queryParam["ProjectStatus"].ToString()));
                    }
                    if (!queryParam["ProjectName"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%')", queryParam["ProjectName"].ToString()));
                    }
                    if (!queryParam["ProjectCode"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectCode like'%{0}%')", queryParam["ProjectCode"].ToString()));
                    }
                    if (!queryParam["CustName"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.CustName like'%{0}%')", queryParam["CustName"].ToString()));
                    }
                    if (!queryParam["ContactPhone"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ContactPhone like'%{0}%')", queryParam["ContactPhone"].ToString()));
                    }
                    if (!queryParam["PreparedPerson"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.PreparedPerson like'%{0}%')", queryParam["PreparedPerson"].ToString()));
                    }
                    if (!queryParam["FollowPerson"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.FollowPerson like'%{0}%')", queryParam["FollowPerson"].ToString()));
                    }
                    if (!queryParam["ProjectStatus"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectStatus like'%{0}%')", queryParam["ProjectStatus"].ToString()));
                    }
                }
                else
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId ='')"));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectVo>(strSql.ToString(), dp);
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
        }   /// <summary>
            /// 获取页面导出显示列表数据
            /// </summary>
            /// <param name="pagination">分页参数</param>
            /// <param name="queryJson">查询参数</param>
            /// <returns></returns>
        public IEnumerable<ProjectVo> GetPageListAddress2(string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.ProjectCode,
                t.ProjectName,
                t.CustName,
                t.ContactName,
                t.ContactPhone,
                t.Address,
                t.ProvinceId,
                t.CityId,
                t.CountyId,
                t.ProjectSituation,
                t.Remark,
                t.PreparedPerson,
                t.FollowPerson,
                t.FCompanyId,
                t.FDepartmentId,
                t.PCompanyId,
                t.PDepartmentId,
                t.CompanyId,
                t.DepartmentId,
                t.ProjectStatus,
                t.CreateTime,
                t.TenderFlg,
                t.ProjectSource,
                t.CreateUser
             ");
                strSql.Append("  FROM  Project t left join  (select ProjectId from ProjectContract group by ProjectId) a  on t.Id=a.ProjectId  ");
                strSql.Append(" WHERE 1=1");

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
                if (!queryParam["StartTime"].IsEmpty() && queryParam["EndTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.CreateTime >='{0}' and t.CreateTime< (select dateadd(day,1,'{0}')))", queryParam["StartTime"].ToString()));
                }
                if (queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.CreateTime >= '{0}' and t.CreateTime< (select dateadd(day,1,'{0}')))", queryParam["EndTime"].ToString()));
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%'   or  t.ProjectCode like'%{0}%'  or  t.CustName like'%{0}%'   or  t.ContactPhone like'%{0}%')", queryParam["keyword"].ToString()));
                }
                if (!queryParam["cs"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectId is null)", queryParam["cs"].ToString()));
                }
                if (!queryParam["ProjectStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectStatus = {0})", queryParam["ProjectStatus"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%')", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["ProjectCode"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectCode like'%{0}%')", queryParam["ProjectCode"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.CustName like'%{0}%')", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContactPhone"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContactPhone like'%{0}%')", queryParam["ContactPhone"].ToString()));
                }
                if (!queryParam["PreparedPerson"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.PreparedPerson like'%{0}%')", queryParam["PreparedPerson"].ToString()));
                }
                if (!queryParam["FollowPerson"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.FollowPerson like'%{0}%')", queryParam["FollowPerson"].ToString()));
                }
                if (!queryParam["ProjectStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectStatus like'%{0}%')", queryParam["ProjectStatus"].ToString()));
                }
                if (!queryParam["Remark"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.Remark like'%{0}%')", queryParam["Remark"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectVo>(strSql.ToString(), dp);
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
        /// 多部门获取页面导出显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectVo> GetPageListAddressDepartmentId(string queryJson, string dep)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.ProjectCode,
                t.ProjectName,
                t.CustName,
                t.ContactName,
                t.ContactPhone,
                t.Address,
                t.ProvinceId,
                t.CityId,
                t.CountyId,
                t.ProjectSituation,
                t.Remark,
                t.PreparedPerson,
                t.FollowPerson,
                t.FCompanyId,
                t.FDepartmentId,
                t.PCompanyId,
                t.PDepartmentId,
                t.CompanyId,
                t.DepartmentId,
                t.ProjectStatus,
                t.CreateTime,
                t.TenderFlg,
                t.ProjectSource,
                t.CreateUser
             ");
                //strSql.Append("  FROM Project t inner join adms706.dbo.lr_base_user u on u.F_UserId=t.CreateUser ");
                strSql.Append("  FROM  Project t left join  (select ProjectId from ProjectContract group by ProjectId) a  on t.Id=a.ProjectId  ");
                strSql.Append("  WHERE 1=1");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (queryParam["DepartmentId"].IsEmpty())
                {
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
                        strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%'   or  t.ProjectCode like'%{0}%'  or  t.CustName like'%{0}%'  or  t.ContactName like'%{0}%' or  t.ContactPhone like'%{0}%' or t.Address like'%{0}%')", queryParam["keyword"].ToString()));
                    }
                    if (!queryParam["ProjectStatus"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectStatus = {0})", queryParam["ProjectStatus"].ToString()));
                    }
                    if (!queryParam["cs"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( a.ProjectId is null)", queryParam["cs"].ToString()));
                    }
                    if (!queryParam["ProjectName"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%')", queryParam["ProjectName"].ToString()));
                    }
                    if (!queryParam["ProjectCode"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectCode like'%{0}%')", queryParam["ProjectCode"].ToString()));
                    }
                    if (!queryParam["CustName"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.CustName like'%{0}%')", queryParam["CustName"].ToString()));
                    }
                    if (!queryParam["ContactPhone"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ContactPhone like'%{0}%')", queryParam["ContactPhone"].ToString()));
                    }
                    if (!queryParam["PreparedPerson"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.PreparedPerson like'%{0}%')", queryParam["PreparedPerson"].ToString()));
                    }
                    if (!queryParam["FollowPerson"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.FollowPerson like'%{0}%')", queryParam["FollowPerson"].ToString()));
                    }
                    if (!queryParam["ProjectStatus"].IsEmpty())
                    {
                        strSql.Append(string.Format(" AND ( t.ProjectStatus like'%{0}%')", queryParam["ProjectStatus"].ToString()));
                    }


                }
                else
                {
                    strSql.Append(string.Format(" AND ( t.DepartmentId ='')"));
                }
                strSql.Append(string.Format(" AND ( " + dep + " )"));
                return this.BaseRepository("learunOAWFForm").FindList<ProjectVo>(strSql.ToString(), dp);
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
        public IEnumerable<ProjectVo> GetProjectList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.ProjectCode,
                t.ProjectName,
                t.CustName,

                t.ContactName,
                t.ContactPhone,
                t.Address,
                t.ProvinceId,
                t.CityId,
                t.CountyId,
                t.ProjectSituation,
                t.Remark,
                t.PreparedPerson,
                t.FollowPerson,
                t.FCompanyId,
                t.FDepartmentId,
                t.PCompanyId,
                t.PDepartmentId,
                t.CompanyId,
                t.DepartmentId,
                t.ProjectStatus,
                t.CreateTime,

t.ProjectSource

                ");
                strSql.Append("  FROM  Project t inner join  (select ProjectId from ProjectContract group by ProjectId) a  on t.Id=a.ProjectId  ");
                strSql.Append(" WHERE 1=1");

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
                    strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%'   or  t.ProjectCode like'%{0}%'  or  t.CustName like'%{0}%'   or  t.ContactPhone like'%{0}%')", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectStatus = {0})", queryParam["ProjectStatus"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%')", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["ProjectCode"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectCode like'%{0}%')", queryParam["ProjectCode"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.CustName like'%{0}%')", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContactPhone"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContactPhone like'%{0}%')", queryParam["ContactPhone"].ToString()));
                }
                if (!queryParam["PreparedPerson"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.PreparedPerson like'%{0}%')", queryParam["PreparedPerson"].ToString()));
                }
                if (!queryParam["FollowPerson"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.FollowPerson like'%{0}%')", queryParam["FollowPerson"].ToString()));
                }
                if (!queryParam["ProjectStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectStatus like'%{0}%')", queryParam["ProjectStatus"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectVo>(strSql.ToString(), dp, pagination);
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
        public IEnumerable<ProjectVo> GetProjectList2(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.ProjectCode,
                t.ProjectName,
                t.CustName,

                t.ContactName,
                t.ContactPhone,
                t.Address,
                t.ProvinceId,
                t.CityId,
                t.CountyId,
                t.ProjectSituation,
                t.Remark,
                t.PreparedPerson,
                t.FollowPerson,
                t.FCompanyId,
                t.FDepartmentId,
                t.PCompanyId,
                t.PDepartmentId,
                t.CompanyId,
                t.DepartmentId,
                t.ProjectStatus,
                t.CreateTime,
t.CreateUser,
t.ProjectSource

                ");
                strSql.Append("  FROM  Project t left join  (select ProjectId from ProjectContract group by ProjectId) a  on t.Id=a.ProjectId  ");
                strSql.Append(" WHERE 1=1");

                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                //if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                //{
                //    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                //    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                //    strSql.Append(" AND ( t.CreateTime >= @startTime AND t.CreateTime <= @endTime ) ");
                //}
                //if (!queryParam["StartTime"].IsEmpty() && queryParam["EndTime"].IsEmpty())
                //{
                //    strSql.Append(string.Format(" AND ( t.CreateTime >='{0}' and t.CreateTime< (select dateadd(day,1,'{0}')))", queryParam["StartTime"].ToString()));
                //}
                //if (queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                //{
                //    strSql.Append(string.Format(" AND ( t.CreateTime >= '{0}' and t.CreateTime< (select dateadd(day,1,'{0}')))", queryParam["EndTime"].ToString()));                 
                //}
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
                    strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%'   or  t.ProjectCode like'%{0}%'  or  t.CustName like'%{0}%'   or  t.ContactPhone like'%{0}%')", queryParam["keyword"].ToString()));
                }
                if (!queryParam["cs"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ProjectId is null)", queryParam["cs"].ToString()));
                }
                if (!queryParam["ProjectStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectStatus = {0})", queryParam["ProjectStatus"].ToString()));
                }
                if (!queryParam["ProjectName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%')", queryParam["ProjectName"].ToString()));
                }
                if (!queryParam["ProjectCode"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectCode like'%{0}%')", queryParam["ProjectCode"].ToString()));
                }
                if (!queryParam["CustName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.CustName like'%{0}%')", queryParam["CustName"].ToString()));
                }
                if (!queryParam["ContactPhone"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ContactPhone like'%{0}%')", queryParam["ContactPhone"].ToString()));
                }
                if (!queryParam["PreparedPerson"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.PreparedPerson like'%{0}%')", queryParam["PreparedPerson"].ToString()));
                }
                if (!queryParam["FollowPerson"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.FollowPerson like'%{0}%')", queryParam["FollowPerson"].ToString()));
                }
                if (!queryParam["ProjectStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectStatus like'%{0}%')", queryParam["ProjectStatus"].ToString()));
                }
                if (!queryParam["Remark"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.Remark like'%{0}%')", queryParam["Remark"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectVo>(strSql.ToString(), dp, pagination);
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

        public ProjectVo GetToviewListProject(string id)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.ProjectName,
                t.CustName, 
                t.Remark,
                t.PreparedPerson,
                t.FollowPerson,
                t.DepartmentId,
                t.ProjectSource
                ");
                strSql.Append("  FROM Project t ");
                strSql.Append(" WHERE t.Id='" + id + "'");

                return this.BaseRepository("learunOAWFForm").FindList<ProjectVo>(strSql.ToString()).FirstOrDefault();
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
        public ProjectVo PreviewIndexFrom(string keyValue)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.ProjectCode,
                t.ProjectName,
                t.CustName,
                t.ContactName,
                t.ContactPhone,
                t.Address,
                t.ProvinceId,
                t.CityId,
                t.CountyId,
                t.ProjectSituation,
                t.Remark,
                t.PreparedPerson,
                t.FollowPerson,
                t.FCompanyId,
                t.FDepartmentId,
                t.PCompanyId,
                t.PDepartmentId,
                t.CompanyId,
                t.DepartmentId,
                t.ProjectStatus,
                t.CreateTime,
                t.CreateUser
                ");
                strSql.Append("  FROM Project t ");
                strSql.Append(" WHERE t.Id='" + keyValue + "'");

                return this.BaseRepository("learunOAWFForm").FindList<ProjectVo>(strSql.ToString()).FirstOrDefault();
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
        public IEnumerable<ProjectEntity> GetList(string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.ProjectCode,
                t.ProjectName,
                t.CustName,
                t.ContactName,
                t.ContactPhone,
                t.Address,
                t.ProvinceId,
                t.CityId,
                t.CountyId,
                t.ProjectSituation,
                t.Remark,
                t.PreparedPerson,
                t.FollowPerson,
                t.FCompanyId,
                t.FDepartmentId,
                t.PCompanyId,
                t.PDepartmentId,
                t.CompanyId,
                t.DepartmentId,
                t.ProjectStatus,
                t.CreateTime,
                t.CreateUser
                ");
                strSql.Append("  FROM Project t ");
                strSql.Append("  WHERE 1=1 ");
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
                    strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%'   or  t.ProjectCode like'%{0}%'  or  t.CustName like'%{0}%'   or  t.ContactPhone like'%{0}%')", queryParam["keyword"].ToString()));
                }
                if (!queryParam["ProjectStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( t.ProjectStatus = {0})", queryParam["ProjectStatus"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectEntity>(strSql.ToString(), dp);
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

        public IEnumerable<ProjectVo> GetSelectedProjectByContractList(Pagination pagination, string queryJson)

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
                t.FDepartmentId,
                t.DepartmentId,
                pt.DepartmentId as PDepartmentId,
               t.ProjectCode,t.ProjectName,pc.ContractNo,pc.id as ContractId,t.CustName 
                ");
                strSql.Append("  FROM Project t left join ProjectContract pc on pc.ProjectId=t.Id LEFT JOIN projectTask pt ON pt.ProjectId = pc.ProjectId ");
                strSql.Append("  WHERE (t.ProjectStatus = 1 or t.ProjectStatus = 3) ");
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
                    strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%'   or  t.ProjectCode like'%{0}%'  or  t.CustName like'%{0}%'   or  t.ContactPhone like'%{0}%' or  pc.ContractNo like'%{0}%' )",
                        queryParam["keyword"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectVo>(strSql.ToString(), dp, pagination);
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
        public IEnumerable<ProjectVo> GetSelectedProjectByContractList_2(Pagination pagination, string queryJson, string dept)
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
                strSql.Append("  FROM Project t inner join ProjectContract pc on pc.ProjectId=t.Id and pc.ContractStatus=4 and pc.ContractType=1 left join projectTask pt on pt.ProjectId=t.Id ");
                strSql.Append("  WHERE (t.ProjectStatus = 1 or t.ProjectStatus = 3) AND pc.MainContract=1");
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
                    strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%'   or  t.ProjectCode like'%{0}%'  or  t.CustName like'%{0}%'   or  t.ContactPhone like'%{0}%' or pc.ContractNo like '%{0}%' )", queryParam["keyword"].ToString()));
                }
                strSql.Append(dept + " group by t.Id, t.CreateTime, t.ContactPhone, t.CreateUser, pc.ContractAmount, t.ProjectCode, t.ProjectName, pc.ContractNo, pc.id, t.CustName ");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectVo>(strSql.ToString(), dp, pagination);
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
        /// 开票添加项目
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectVo> GetSelectedProjectByContractListBilling(Pagination pagination, string queryJson)

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
               t.ProjectCode,t.ProjectName,pc.ContractNo,t.CustName,t.DepartmentId,t.FDepartmentId
                ");
                strSql.Append("  FROM Project t inner join ProjectContract pc on pc.ProjectId=t.Id and pc.ContractStatus=4 and pc.ContractType=1");
                strSql.Append("  WHERE (t.ProjectStatus = 1 or t.ProjectStatus = 3) AND pc.MainContract=1");
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
                    strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%'   or  t.ProjectCode like'%{0}%'  or  t.CustName like'%{0}%'   or  t.ContactPhone like'%{0}%')", queryParam["keyword"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectVo>(strSql.ToString(), dp, pagination);
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
        /// 多部门开票添加项目
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectVo> GetSelectedProjectByContractListBillingDepartmentId(Pagination pagination, string queryJson, string dep)

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
               t.ProjectCode,t.ProjectName,pc.ContractNo,t.CustName,t.DepartmentId,t.FDepartmentId
                ");
                strSql.Append("  FROM Project t inner join ProjectContract pc on pc.ProjectId=t.Id and pc.ContractStatus=4 and pc.ContractType=1");
                strSql.Append("  WHERE (t.ProjectStatus = 1 or t.ProjectStatus = 3) AND pc.MainContract=1");
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
                    strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%'   or  t.ProjectCode like'%{0}%'  or  t.CustName like'%{0}%'   or  t.ContactPhone like'%{0}%')", queryParam["keyword"].ToString()));
                }
                strSql.Append(string.Format(" AND ( " + dep + " )"));
                return this.BaseRepository("learunOAWFForm").FindList<ProjectVo>(strSql.ToString(), dp, pagination);
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
        public IEnumerable<ProjectEntity> GetSelectedProjectList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.ProjectCode,
                t.ProjectName,
                t.CustName,
                t.ContactName,
                t.ContactPhone,
                t.Address,
                t.ProvinceId,
                t.CityId,
                t.CountyId,
                t.ProjectSituation,
                t.Remark,
                t.PreparedPerson,
                t.FollowPerson,
                t.FCompanyId,
                t.FDepartmentId,
                t.PCompanyId,
                t.PDepartmentId,
                t.CompanyId,
                t.DepartmentId,
                t.ProjectStatus,
                t.CreateTime,
                t.CreateUser,
                t.TenderFlg,a.ContractNo
                ");
                strSql.Append("   FROM Project t left join (select ProjectId,MAX(ContractNo) as ContractNo from ProjectContract where ContractType=1 and MainContract=1 group by ProjectId,ContractNo ) a on a.ProjectId=t.Id ");
                strSql.Append(" WHERE (t.ProjectStatus = 1 or t.ProjectStatus = 3)  ");
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
                    strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%'   or  t.ProjectCode like'%{0}%'  or  t.CustName like'%{0}%'   or  t.ContactPhone like'%{0}%')", queryParam["keyword"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectEntity>(strSql.ToString(), dp, pagination);
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
        public IEnumerable<ProjectVo> GetSelectedProjectList1(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.ProjectCode,
                t.ProjectName,
                t.CustName,
                t.ContactName,
                t.ContactPhone,
                t.Address,
                t.ProvinceId,
                t.CityId,
                t.CountyId,
                t.ProjectSituation,
                t.Remark,
                t.PreparedPerson,
                t.FollowPerson,
                t.FCompanyId,
                t.FDepartmentId,
                t.PCompanyId,
                t.PDepartmentId,
                t.CompanyId,
                t.DepartmentId,
                t.ProjectStatus,
                t.CreateTime,
                t.CreateUser,
                t.TenderFlg,a.ContractNo,a.ContractId,t.ProjectSource
                ");
                strSql.Append("  FROM Project t inner join (select ProjectId,ContractNo,id as ContractId from ProjectContract where ContractType=1 and MainContract=1 and ContractStatus = 4 group by ProjectId,ContractNo,id ) a on a.ProjectId=t.Id");
                strSql.Append("  WHERE (t.ProjectStatus = 1 or t.ProjectStatus = 3) ");
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
                    strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%'   or  t.ProjectCode like'%{0}%'  or  t.CustName like'%{0}%'   or  t.ContactPhone like'%{0}%' or a.ContractNo like '%{0}%')", queryParam["keyword"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectVo>(strSql.ToString(), dp, pagination);
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
        public IEnumerable<ProjectVo> GetSelectedProjectListWithoutContract(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.ProjectCode,
                t.ProjectName,
                t.CustName,
                t.ContactName,
                t.ContactPhone,
                t.Address,
                t.ProvinceId,
                t.CityId,
                t.CountyId,
                t.ProjectSituation,
                t.Remark,
                t.PreparedPerson,
                t.FollowPerson,
                t.FCompanyId,
                t.FDepartmentId,
                t.PCompanyId,
                t.PDepartmentId,
                t.CompanyId,
                t.DepartmentId,
                t.ProjectStatus,
                t.CreateTime,
                t.CreateUser,
                t.TenderFlg,
                t.ProjectSource
                ");
                strSql.Append("  FROM Project t");
                strSql.Append("  WHERE (t.ProjectStatus = 1 or t.ProjectStatus = 3) ");
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
                    strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%'   or  t.ProjectCode like'%{0}%'  or  t.CustName like'%{0}%'   or  t.ContactPhone like'%{0}%')", queryParam["keyword"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectVo>(strSql.ToString(), dp, pagination);
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

        public IEnumerable<ProjectEntity> GetSelectedProjectListT(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.ProjectCode,
                t.ProjectName,
                t.CustName,
                t.ContactName,
                t.ContactPhone,
                t.Address,
                t.ProvinceId,
                t.CityId,
                t.CountyId,
                t.ProjectSituation,
                t.Remark,
                t.PreparedPerson,
                t.FollowPerson,
                t.FCompanyId,
                t.FDepartmentId,
                t.PCompanyId,
                t.PDepartmentId,
                t.CompanyId,
                t.DepartmentId,
                t.ProjectStatus,
                t.CreateTime,
                t.CreateUser,
                t.TenderFlg,
t.ProjectSource
                ");
                strSql.Append("  FROM Project t");
                strSql.Append("  WHERE (t.ProjectStatus = 1 or t.ProjectStatus = 3) ");
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
                    strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%'   or  t.ProjectCode like'%{0}%'  or  t.CustName like'%{0}%'   or  t.ContactPhone like'%{0}%')", queryParam["keyword"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectEntity>(strSql.ToString(), dp, pagination);
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
        public IEnumerable<ProjectEntity> GetSelectedProjectListTi(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.ProjectCode,
                t.ProjectName,
                t.CustName,
                t.ContactName,
                t.ContactPhone,
                t.Address,
                t.ProvinceId,
                t.CityId,
                t.CountyId,
                t.ProjectSituation,
                t.Remark,
                t.PreparedPerson,
                t.FollowPerson,
                t.FCompanyId,
                t.FDepartmentId,
                t.PCompanyId,
                t.PDepartmentId,
                t.CompanyId,
                t.DepartmentId,
                t.ProjectStatus,
                t.CreateTime,
                t.CreateUser,
                t.TenderFlg,
t.ProjectSource,ct.ContractNo
                ");
                strSql.Append("  FROM Project t inner join ProjectContract ct on ct.ProjectId=t.Id");
                strSql.Append("  WHERE (t.ProjectStatus = 1 or t.ProjectStatus = 3) and ct.MainContract=1 and ct.ContractStatus=4");
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
                    strSql.Append(string.Format(" AND ( t.ProjectName like'%{0}%'   or  t.ProjectCode like'%{0}%'  or  t.CustName like'%{0}%'   or  t.ContactPhone like'%{0}%')", queryParam["keyword"].ToString()));
                }
                return this.BaseRepository("learunOAWFForm").FindList<ProjectEntity>(strSql.ToString(), dp, pagination);
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
        /// 合同预览
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public ProjectEntity GetPreviewFormData(string keyValue)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.ProjectCode,
                t.ProjectName,
                t.CustName,
                t.ContactName,
                t.ContactPhone,
                t.Address,
                t.ProvinceId,
                t.CityId,
                t.CountyId,
                t.ProjectSituation,
                t.Remark,
                t.PreparedPerson,
                t.FollowPerson,
                t.FCompanyId,
                t.FDepartmentId,
                t.PCompanyId,
                t.PDepartmentId,
                t.CompanyId,
                t.DepartmentId,
                t.ProjectStatus,
                t.CreateTime,
                t.CreateUser,
                t.ProjectSource,
                t.TenderFlg,
                t.UpdateTime,
                t.UpdateUser
                ");
                strSql.Append("  FROM Project t ");
                strSql.Append("  WHERE 1=1");
                strSql.Append(" and t.Id='" + keyValue + "'");


                return this.BaseRepository("learunOAWFForm").FindList<ProjectEntity>(strSql.ToString()).FirstOrDefault();
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
        public ProjectVo GetPreviewFormDataById(string keyValue)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.ProjectCode,
                t.ProjectName,
                t.CustName,
                t.ContactName,
                t.ProjectSource,
                t.Remark,
                t.ContactPhone,
                t.Address,
                t.ProvinceId,
                t.CityId,
                t.CountyId,
                t.ProjectSituation,
                t.Remark,
                t.PreparedPerson,
                t.FollowPerson,
                t.FCompanyId,
                t.FDepartmentId,
                t.PCompanyId,
                t.PDepartmentId,
                t.CompanyId,
                t.DepartmentId,
                t.ProjectStatus,
                t.TenderFlg,
                t.CreateTime,
                t.CreateUser,
                t.TenderFlg
             

                ");
                strSql.Append("  FROM  Project t ");
                strSql.Append(" WHERE t.Id='" + keyValue + "'");


                return this.BaseRepository("learunOAWFForm").FindList<ProjectVo>(strSql.ToString()).FirstOrDefault();

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
        public IEnumerable<ProjectFollowListEntity> GetProjectFollowList(string keyValue)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.Id,
                t.ProjectId,
                t.FollowContent,
                t.FollowDate,
                t.CreateTime,
                t.CreateUser,             
                t.UpdateTime,
                t.UpdateUser,
                t.UpdateTime,
                t.UpdateUser
                ");
                strSql.Append("  FROM  ProjectFollowList t ");
                strSql.Append(" WHERE t.ProjectId='" + keyValue + "'");


                return this.BaseRepository("learunOAWFForm").FindList_NodbWhere<ProjectFollowListEntity>(strSql.ToString());

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
        /// 省市
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ProjectVo ProvinceIdS(string id)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" a.F_AreaName,t.PreparedPerson,t.FollowPerson,t.CreateUser FROM adms706.dbo.lr_base_area a inner join Project t on t.ProvinceId=a.F_ParentId and t.CityId=a.F_AreaCode");
                strSql.Append(" WHERE a.F_EnabledMark = 1 AND a.F_DeleteMark = 0 and t.id='" + id + "'");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectVo>(strSql.ToString()).FirstOrDefault();

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
        /// 省市
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ProjectVo ProvinceIdX(string id)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@" a.F_AreaName,t.PreparedPerson,t.FollowPerson,t.CreateUser FROM adms706.dbo.lr_base_area a inner join  Project t on  a.F_ParentId=t.CityId  and a.F_AreaCode=t.CompanyId ");
                strSql.Append(" WHERE a.F_EnabledMark = 1 AND a.F_DeleteMark = 0 and t.id='" + id + "'");
                return this.BaseRepository("learunOAWFForm").FindList<ProjectVo>(strSql.ToString()).FirstOrDefault();

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
        /// 获取Project表实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public ProjectEntity GetProjectEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindEntity<ProjectEntity>(keyValue);
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
        public ProjectEntity GetEntityByProcessId(string processId)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindEntity<ProjectEntity>(t => t.WorkFlowId == processId);
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
                this.BaseRepository("learunOAWFForm").Delete<ProjectEntity>(t => t.Id == keyValue);
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
        ///  判断当前的合同是否重复
        /// </summary>
        /// <param name="ContactPhone"></param>
        /// <param name="CustName"></param>
        /// <returns></returns>
        public int JudgePepeatProject(string ContactPhone, string CustName, string keyValue)
        {
            int count = 0;
            try
            {

                if (string.IsNullOrEmpty(keyValue))
                {
                    List<ProjectEntity> projects = this.BaseRepository("learunOAWFForm").FindList<ProjectEntity>(t => t.ContactPhone == ContactPhone).AsList();
                    if (projects.Count > 0)
                    {
                        count = 1;
                    }
                    else
                    {
                        projects = this.BaseRepository("learunOAWFForm").FindList<ProjectEntity>(t => t.CustName == CustName).AsList();
                        if (projects.Count > 0)
                        {
                            count = 2;
                        }
                    }
                }
                else
                {
                    List<ProjectEntity> projects = this.BaseRepository("learunOAWFForm").FindList<ProjectEntity>(t => t.ContactPhone == ContactPhone && t.Id != keyValue).AsList();
                    if (projects.Count > 0)
                    {
                        count = 1;
                    }
                    else
                    {
                        projects = this.BaseRepository("learunOAWFForm").FindList<ProjectEntity>(t => t.CustName == CustName && t.Id != keyValue).AsList();
                        if (projects.Count > 0)
                        {
                            count = 2;
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
            return count;
        }

        /// <summary>
        ///  判断当前的合同是否重复
        /// </summary>
        /// <param name="ContactPhone"></param>
        /// <param name="CustName"></param>
        /// <returns></returns>
        public int JudgePepeatProjectBy(string ContactPhone, string CustName)
        {
            int count = 0;
            try
            {
                List<ProjectEntity> projects = this.BaseRepository("learunOAWFForm").FindList<ProjectEntity>(t => t.ContactPhone == ContactPhone).AsList();
                if (projects.Count > 0)
                {
                    count = 1;
                }
                else
                {
                    projects = this.BaseRepository("learunOAWFForm").FindList<ProjectEntity>(t => t.CustName == CustName).AsList();
                    if (projects.Count > 0)
                    {
                        count = 2;
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
            return count;
        }
        /// <summary>
        /// 判断当前合同是否存在一样的项目合同名称
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public int JudgePepeatProjectName(string projectName, string keyValue)
        {
            int count = 0;
            try
            {

                if (string.IsNullOrEmpty(keyValue))
                {
                    List<ProjectEntity> projects = this.BaseRepository("learunOAWFForm").FindList<ProjectEntity>(t => t.ProjectName == projectName).AsList();
                    if (projects.Count > 0)
                    {
                        count = 1;
                    }
                }
                else
                {
                    List<ProjectEntity> projects = this.BaseRepository("learunOAWFForm").FindList<ProjectEntity>(t => t.ProjectName == projectName && t.Id != keyValue).AsList();
                    if (projects.Count > 0)
                    {
                        count = 1;
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
            return count;
        }
        /// <summary>
        /// 判断当前合同是否存在一样的项目合同名称
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public int JudgePepeatProjectNameBy(string projectName)
        {
            int count = 0;
            try
            {
                List<ProjectEntity> projects = this.BaseRepository("learunOAWFForm").FindList<ProjectEntity>(t => t.ProjectName == projectName).AsList();
                if (projects.Count > 0)
                {
                    count = 1;
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
            return count;
        }
        /// <summary>
        /// 保存实体数据（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public void SaveEntity(string keyValue, ProjectEntity entity)
        {
            try
            {

                if (!string.IsNullOrEmpty(keyValue))
                {
                    //1.根据报备id去查询每个流程对应的项目   
                    //合同
                    var projectContract = this.BaseRepository("learunOAWFForm").FindEntity<ProjectContractEntity>(t => t.ProjectId == keyValue);
                    //报告
                    var projectTask = this.BaseRepository("learunOAWFForm").FindEntity<ProjectTaskEntity>(t => t.ProjectId == keyValue);
                    //用工
                    var projectRecruit = this.BaseRepository("learunOAWFForm").FindEntity<ProjectRecruitEntity>(t => t.ProjectId == keyValue);
                    //开票
                    var projectBilling = this.BaseRepository("learunOAWFForm").FindEntity<ProjectBillingEntity>(t => t.ProjectId == keyValue);
                    //付款
                    var projectPayment = this.BaseRepository("learunOAWFForm").FindEntity<ProjectPaymentEntity>(t => t.ProjectId == keyValue);
                    //2.用查到的流程id去流程表里面找对应的流程
                    //3.根据原来的流程修改流程里面的标题
                    //合同
                    if (projectContract != null && projectContract.WorkFlowId != null)
                    {
                        NWFProcessEntity processEntity = new NWFProcessEntity();
                        var nWFProcessEntityTmp = this.BaseRepository().FindEntity<NWFProcessEntity>(t => t.F_Id == projectContract.WorkFlowId);
                        if (nWFProcessEntityTmp != null && nWFProcessEntityTmp.F_Title != entity.ProjectName)
                        {
                            processEntity.F_Title = entity.ProjectName;
                            processEntity.Modify(projectContract.WorkFlowId);
                            this.BaseRepository().Update(processEntity);
                        }
                    }
                    //报告
                    if (projectTask != null && projectTask.WorkFlowId != null)
                    {
                        NWFProcessEntity processEntity = new NWFProcessEntity();
                        var nWFProcessEntityTmp = this.BaseRepository().FindEntity<NWFProcessEntity>(t => t.F_Id == projectTask.WorkFlowId);
                        if (nWFProcessEntityTmp != null && nWFProcessEntityTmp.F_Title != entity.ProjectName)
                        {
                            processEntity.F_Title = entity.ProjectName;
                            processEntity.Modify(projectTask.WorkFlowId);
                            this.BaseRepository().Update(processEntity);
                        }
                    }
                    //用工
                    if (projectRecruit != null && projectRecruit.WorkFlowId != null)
                    {
                        NWFProcessEntity processEntity = new NWFProcessEntity();
                        var nWFProcessEntityTmp = this.BaseRepository().FindEntity<NWFProcessEntity>(t => t.F_Id == projectRecruit.WorkFlowId);
                        if (nWFProcessEntityTmp != null && nWFProcessEntityTmp.F_Title != entity.ProjectName)
                        {
                            processEntity.F_Title = entity.ProjectName;
                            processEntity.Modify(projectRecruit.WorkFlowId);
                            this.BaseRepository().Update(processEntity);
                        }
                    }
                    //开票
                    if (projectBilling != null && projectBilling.WorkFlowId != null)
                    {
                        NWFProcessEntity processEntity = new NWFProcessEntity();
                        var nWFProcessEntityTmp = this.BaseRepository().FindEntity<NWFProcessEntity>(t => t.F_Id == projectBilling.WorkFlowId);
                        if (nWFProcessEntityTmp != null && nWFProcessEntityTmp.F_Title != entity.ProjectName)
                        {
                            processEntity.F_Title = entity.ProjectName;
                            processEntity.Modify(projectBilling.WorkFlowId);
                            this.BaseRepository().Update(processEntity);
                        }
                    }
                    //付款
                    if (projectPayment != null && projectPayment.WorkFlowId != null)
                    {
                        NWFProcessEntity processEntity = new NWFProcessEntity();
                        var nWFProcessEntityTmp = this.BaseRepository().FindEntity<NWFProcessEntity>(t => t.F_Id == projectPayment.WorkFlowId);
                        if (nWFProcessEntityTmp != null && nWFProcessEntityTmp.F_Title != entity.ProjectName)
                        {
                            processEntity.F_Title = entity.ProjectName;
                            processEntity.Modify(projectPayment.WorkFlowId);
                            this.BaseRepository().Update(processEntity);
                        }
                    }
                    //4.修改报备的项目名
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
        public void SaveProjectFollowEntity(string keyValue, ProjectFollowListEntity entity)
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
        /// 新增
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public void AddEntity(ProjectEntity entity)
        {
            try
            {
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

        #endregion

    }
}
