using Learun.Application.Base.SystemModule;
using System.Data.Entity.ModelConfiguration;

namespace Learun.Application.Mapping
{
    /// <summary>
    /// 日 期：2024.08.05
    /// 描 述：Share附件管理
    /// </summary>
    public class SharefilelistMap : EntityTypeConfiguration<SharefilelistEntity>
    {
        public SharefilelistMap()
        {
            #region 表、主键
            //表
            this.ToTable("lr_base_sharefilelist");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
