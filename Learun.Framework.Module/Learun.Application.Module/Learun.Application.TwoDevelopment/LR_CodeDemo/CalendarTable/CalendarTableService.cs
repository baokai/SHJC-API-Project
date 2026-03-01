using Dapper;
using Learun.Application.Organization;
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
    /// 日 期：2022-03-11 00:59
    /// 描 述：项目开票
    /// </summary>
    public class CalendarTableService : RepositoryFactory
    {

        #region 获取数据
        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<CalendarTableVo> GetPage(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"u.F_RealName,d.F_FullName,t.PlanTime  from  ProjectTask t
INNER JOIN  adms706.dbo.lr_base_user u ON t.Inspector=u.F_UserId
INNER JOIN adms706.dbo.lr_base_department d 
on d.F_DepartmentId=u.F_DepartmentId 
WHERE 
(d.F_FullName='结构勘测部' or d.F_FullName='房屋检测站' or d.F_FullName='华谨检测部' 
or d.F_FullName='设计事业部' or d.F_FullName='浦东基地' or d.F_FullName='通际公司' 
or d.F_FullName='西安公司' or d.F_FullName='武汉公司' or d.F_FullName='天津公司' 
or d.F_FullName='南京公司' or d.F_FullName='苏州公司' or d.F_FullName='杨浦公司'
or d.F_FullName='创新发展部' or d.F_FullName='咨询部' or d.F_FullName='战略发展部')
and t.TaskStatus<>1 and t.Inspector<>''");
                return this.BaseRepository("learunOAWFForm").FindList<CalendarTableVo>(strSql.ToString());
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
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
        public CalendarTableVo GetPageList(string keyValue)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"u.F_RealName,d.F_FullName from  adms706.dbo.lr_base_user u 
INNER JOIN adms706.dbo.lr_base_department d 
on d.F_DepartmentId=u.F_DepartmentId 
WHERE 
(d.F_FullName='结构勘测部' or d.F_FullName='房屋检测站' or d.F_FullName='华谨检测部' 
or d.F_FullName='设计事业部' or d.F_FullName='浦东基地' or d.F_FullName='通际公司' 
or d.F_FullName='西安公司' or d.F_FullName='武汉公司' or d.F_FullName='天津公司' 
or d.F_FullName='南京公司' or d.F_FullName='苏州公司' or d.F_FullName='杨浦公司'
or d.F_FullName='创新发展部' or d.F_FullName='咨询部' or d.F_FullName='战略发展部') 
and u.F_UserId='"+ keyValue + "'");
                
                return this.BaseRepository("learunOAWFForm").FindList<CalendarTableVo>(strSql.ToString()).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }


        public List <ProjectTaskVo> GetTaskInspectorlist()
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"t.id,t.Inspector,t.TaskStatus,t.PlanTime,t.ActualApproachTime from ProjectTask t where t.TaskStatus<>1 and t.Inspector<>''"); 
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
        public CalendarTableVo GetUserId(string id)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"SELECT Inspector FROM ProjectTask WHERE Inspector LIKE '%,%'");          
                return this.BaseRepository("learunOAWFForm").FindList<CalendarTableVo>(strSql.ToString()).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
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
