using Learun.Application.TwoDevelopment.LR_CodeDemo;
using System.Data.Entity.ModelConfiguration;

namespace  Learun.Application.Mapping
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
    /// 创 建：超级管理员
    /// 日 期：2022-03-16 17:56
    /// 描 述：项目回款管理
    /// </summary>
    public class ProjectPayCollectionMap : EntityTypeConfiguration<ProjectPayCollectionEntity>
    {
        public ProjectPayCollectionMap()
        {
            #region 表、主键
            //表
            this.ToTable("PROJECTPAYCOLLECTION");
            //主键
            this.HasKey(t => t.id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}

