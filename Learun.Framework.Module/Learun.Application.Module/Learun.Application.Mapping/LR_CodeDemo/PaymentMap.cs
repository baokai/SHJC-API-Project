using Learun.Application.TwoDevelopment.LR_CodeDemo;
using System.Data.Entity.ModelConfiguration;

namespace Learun.Application.Mapping.LR_CodeDemo
{
    public class PaymentMap : EntityTypeConfiguration<PaymentEntity>
    {
        public PaymentMap()
        {
            #region 表、主键
            //表
            this.ToTable("PAYMENT");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
