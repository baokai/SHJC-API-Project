using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 力软信息技术（苏州）有限公司
    /// 创 建：超级管理员
    /// 日 期：2022-03-16 17:56
    /// 描 述：项目回款管理
    /// </summary>
    public class ProjectPayCollectionEntity 
    {
        #region 实体成员
        /// <summary>
        /// id
        /// </summary>
        [Column("ID")]
        public string id { get; set; }
        /// <summary>
        /// ProjectId
        /// </summary>
        [Column("PROJECTID")]
        public string ProjectId { get; set; }
        [Column("CUSTNAME")]
        public string CustName { get; set; }
        [Column("CONTRACTNO")]
        public string ContractNo { get; set; }
        /// <summary>
        /// 本次到款金额
        /// </summary>
        [Column("AMOUNT")]
        public decimal? Amount { get; set; }
        /// <summary>
        /// 到账日期
        /// </summary>
        [Column("RECEIPTDATE")]
        public DateTime? ReceiptDate { get; set; }
        /// <summary>
        /// PaymentUnit
        /// </summary>
        [Column("PAYMENTUNIT")]
        public string PaymentUnit { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CREATETIME")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CREATEUSER")]
        public string CreateUser { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Column("UPDATETIME")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        [Column("UPDATEUSER")]
        public string UpdateUser { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.CreateTime = DateTime.Now;
            this.UpdateTime = DateTime.Now;
            this.UpdateUser = LoginUserInfo.Get().userId;
            this.CreateUser = LoginUserInfo.Get().userId;
            this.id = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {
            this.UpdateTime = DateTime.Now;
            this.UpdateUser = LoginUserInfo.Get().userId;
            this.id = keyValue;
        }
        #endregion
        #region 扩展字段
        #endregion
    }
}

