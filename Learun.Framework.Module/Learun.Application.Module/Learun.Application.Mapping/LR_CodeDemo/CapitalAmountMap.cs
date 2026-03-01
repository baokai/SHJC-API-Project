using Learun.Application.TwoDevelopment.LR_CodeDemo;
using System.Data.Entity.ModelConfiguration;

namespace Learun.Application.Mapping.LR_CodeDemo
{
    public class CapitalAmountMap : EntityTypeConfiguration<CapitalAmountEntity>
    {
        public CapitalAmountMap()
        {
            #region 表、主键
            //表
            this.ToTable("CAPITALAMOUNT");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
