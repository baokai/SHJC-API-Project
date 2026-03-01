using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.Base.SystemModule
{
    /// <summary>
    /// 描 述：附件管理
    /// </summary>
    public class AnnexesFileEntity
    {
        #region 实体成员
        /// <summary>
        /// 文件主键
        /// </summary>
        /// <returns></returns>
        [Column("F_ID")]
        public string F_Id { get; set; }
        /// <summary>
        /// 附件夹主键
        /// </summary>
        [Column("F_FOLDERID")]
        public string F_FolderId { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        /// <returns></returns>
        [Column("F_FILENAME")]
        public string F_FileName { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        /// <returns></returns>
        [Column("F_FILEPATH")]
        public string F_FilePath { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        /// <returns></returns>
        [Column("F_FILESIZE")]
        public string F_FileSize { get; set; }
        /// <summary>
        /// 文件后缀
        /// </summary>
        /// <returns></returns>
        [Column("F_FILEEXTENSIONS")]
        public string F_FileExtensions { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        /// <returns></returns>
        [Column("F_FILETYPE")]
        public string F_FileType { get; set; }
        /// <summary>
        /// 下载次数
        /// </summary>
        /// <returns></returns>
        [Column("F_DOWNLOADCOUNT")]
        public int? F_DownloadCount { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        /// <returns></returns>
        [Column("F_CREATEDATE")]
        public DateTime? F_CreateDate { get; set; }
        /// <summary>
        /// 创建用户主键
        /// </summary>
        /// <returns></returns>
        [Column("F_CREATEUSERID")]
        public string F_CreateUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [Column("F_CREATEUSERNAME")]
        public string F_CreateUserName { get; set; }
        /// <summary>
        /// 存储路径
        /// </summary>
        [Column("F_URL")]
        public string F_Url { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public  void Create()
        {
            this.F_CreateDate = DateTime.Now;
        }
        #endregion
    }

}
