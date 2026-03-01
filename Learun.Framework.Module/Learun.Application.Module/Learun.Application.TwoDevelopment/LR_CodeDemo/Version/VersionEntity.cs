using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
    /// 创 建：超级管理员
    /// 日 期：2022-03-11 00:59
    /// 描 述：项目开票
    /// </summary>
    public class VersionEntity
    {
        #region 实体成员

        /// <summary>
        /// Id
        /// </summary>
        [Column("ID")]
        public string Id { get; set; }    
       
        /// <summary>
        /// 版本名字
        /// </summary>
        [Column("VERSIONNAME")]
        public string VersionName { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        [Column("VERSIONNUMBER")]
        public string Versionnumber { get; set; }
        #endregion


        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            
            this.Id = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {
           
            this.Id = keyValue;
        }
        #endregion
        #region 扩展字段
        #endregion
    }
}

