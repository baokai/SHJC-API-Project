using Learun.Application.TwoDevelopment.LR_CodeDemo;
using System.Data.Entity.ModelConfiguration;

namespace  Learun.Application.Mapping
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2019-06-12 18:49
    /// 描 述：库存
    /// </summary>
    public class LR_Demo_StockMap : EntityTypeConfiguration<LR_Demo_StockEntity>
    {
        public LR_Demo_StockMap()
        {
            #region 表、主键
            //表
            this.ToTable("LR_DEMO_STOCK");
            //主键
            this.HasKey(t => t.F_Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}

