using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.Base.SystemModule
{
    /// <summary>
    /// 日 期：2024-08-06
    /// 描 述：Sharefilelist
    /// </summary>
    public class SharefilelistEntity
    {
        #region 实体成员
        /// <summary>
        /// 文件主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public int Id { get; set; }
        /// <summary>
        /// 附件夹主键
        /// </summary>
        [Column("FOLDERID")]
        public string FolderId { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        /// <returns></returns>
        [Column("FILENAME")]
        public string FileName { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        /// <returns></returns>
        [Column("CREATETIME")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建用户主键
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.CreateTime = DateTime.Now;
            this.CreateUserId = LoginUserInfo.Get().userId;
            this.CreateUserName = LoginUserInfo.Get().realName;
        }
        #endregion
    }
}
