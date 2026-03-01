using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    public class ProjectContractSettlementEntity
    {
        /// <summary>
        /// id
        /// </summary>
        [Column("ID")]
        public int id { get; set; }
        /// <summary>
        /// WorkFlowId
        /// </summary>
        [Column("ProjectId")]
        public string ProjectId { get; set; }
        [Column("ContractId")]
        public string ContractId { get; set; }
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
        [Column("EnabledMark")]
        public int EnabledMark { get; set; }

        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.CreateTime = DateTime.Now;
            this.UpdateTime = DateTime.Now;
            this.UpdateUser = LoginUserInfo.Get().userId;
            this.CreateUser = LoginUserInfo.Get().userId;
            this.EnabledMark = 1;
        }
        public void Modify()
        {
            this.UpdateTime = DateTime.Now;
            this.UpdateUser = LoginUserInfo.Get().userId;
        }
    }
}
