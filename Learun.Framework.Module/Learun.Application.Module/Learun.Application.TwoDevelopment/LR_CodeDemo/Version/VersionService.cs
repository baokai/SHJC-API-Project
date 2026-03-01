using Dapper;
using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
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
    public class VersionService : RepositoryFactory
    {
        #region 获取数据
        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<VersionVo> GetPageList()
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT");
                strSql.Append(@"* from lr_base_version");
                ////return this.BaseRepository().FindList<VersionEntity>(strSql.ToString());
                return this.BaseRepository().FindList<VersionVo>(strSql.ToString());
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
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
