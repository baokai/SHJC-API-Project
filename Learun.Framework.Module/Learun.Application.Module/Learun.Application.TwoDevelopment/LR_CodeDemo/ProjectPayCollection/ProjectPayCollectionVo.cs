using Learun.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 日 期：2022-06-16 17:56
    /// 描 述：项目回款管理
    /// </summary>
    public class ProjectPayCollectionVo 
    {
        #region 实体成员
        public int index { get; set; }
        public string id { get; set; }
        public string CreateTimeyMd { get; set; }
        public string ProjectSourceName { get; set; }
        public string ReceiptDateMd { get; set; }
        public string CreateUserName { get; set; }
        public string FollowPersonName { get; set; }
        
        public string DepartmentName { get; set; }

        public DateTime? CreateTime { get; set; }

        public string ProjectName { get; set; }

        public string CustName { get; set; }

        public string ContractNo { get; set; }

        public decimal? Amount { get; set; }
        public decimal? ContractAmount { get; set; }
        public decimal? FollowPersonAmount { get; set; }
        public decimal? PayCollectionAmount { get; set; }
        public string PayCollectionAmount1 { get; set; }
        /// <summary>
        /// 有效合同额
        /// </summary>
        public decimal? EffectiveAmount { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        public decimal? PaymentAmount { get; set; }

        public DateTime? ReceiptDate { get; set; }

        public string PaymentUnit { get; set; }
        /// <summary>
        /// 项目创建人
        /// </summary>
        public string BCreateUser { get; set; }
        /// <summary>
        /// 项目创建人部门
        /// </summary>
        public string BDepartmentId { get; set; }
        public string DepartmentId { get; set; }
        /// <summary>
        /// 销售部门
        /// </summary>
        public string ProjectSource { get; set; }
        public string ProjectId { get; set; }
        



        public string ProjectCode { get; set; }
        

        
        public decimal? NotReceived { get; set; }
        

        

        

        public string CreateUser { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string UpdateUser { get; set; }

        

        public string PDepartmentId { get; set; }
        public string FDepartmentId { get; set; }
        public List<ProjectPayCollectionEntity> contractEntities { get; set; }

        /*public bool Equals(ProjectPayCollectionVo other)
        {
            return this.id == other.id;
            //return this.id == other.id && this.ProjectId == other.ProjectId && this.ProjectName == other.ProjectName && this.CustName == other.CustName  && this.Amount == other.Amount && this.ReceiptDate == other.ReceiptDate && this.PaymentUnit == other.PaymentUnit && this.CreateTime == other.CreateTime && this.CreateUser == other.CreateUser && this.UpdateTime == other.UpdateTime && this.UpdateUser == other.UpdateUser && this.DepartmentId == other.DepartmentId && this.PDepartmentId == other.PDepartmentId && this.FDepartmentId == other.FDepartmentId && this.contractEntities == other.contractEntities && this.ProjectCode == other.ProjectCode;
        }*/
        #endregion


    }
}

