using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 日 期：2022-03-10 22:29
    /// 描 述：项目管理
    /// </summary>
    public class ProjectEntity
    {
        #region 实体成员
        /// <summary>
        /// Id
        /// </summary>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// WorkFlowId
        /// </summary>
        [Column("WORKFLOWID")]
        public string WorkFlowId { get; set; }
        /// <summary>
        /// ProjectCode
        /// </summary>
        [Column("PROJECTCODE")]
        public string ProjectCode { get; set; }
        [Column("ProjectSource")]
        public string ProjectSource { get; set; }
        /// <summary>
        /// ProjectName
        /// </summary>
        [Column("PROJECTNAME")]
        public string ProjectName { get; set; }
        /// <summary>
        /// 委托单位
        /// </summary>
        [Column("CUSTNAME")]
        public string CustName { get; set; }
        [Column("PROVINCEID")]
        public string ProvinceId { get; set; }
        [Column("CITYID")]
        public string CityId { get; set; }
        [Column("COUNTYID")]
        public string CountyId { get; set; }
        /// <summary>
        /// 项目地址
        /// </summary>
        [Column("ADDRESS")]
        public string Address { get; set; }
        [Column("PROJECTSITUATION")]
        public string ProjectSituation { get; set; }
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// FollowPerson
        /// </summary>
        [Column("FOLLOWPERSON")]
        public string FollowPerson { get; set; }
        /// <summary>
        /// 跟进头像
        /// </summary>
        [Column("FOLLOWPERSONAVATAR")]
        public string FollowPersonAvatar { get; set; }
        /// <summary>
        /// PreparedPerson
        /// </summary>
        [Column("PREPAREDPERSON")]
        public string PreparedPerson { get; set; }
        /// <summary>
        /// PreparedPersonAvatar
        /// </summary>
        [Column("PREPAREDPERSONAVATAR")]
        public string PreparedPersonAvatar { get; set; }
        /// <summary>
        /// 钉钉用户code
        /// </summary>
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// 合同状态
        /// </summary>
        [Column("PROJECTSTATUS")]
        public string ProjectStatus { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [Column("CONTACTPHONE")]
        public string ContactPhone { get; set; }
        /// <summary>
        /// ContactName
        /// </summary>
        [Column("CONTACTNAME")]
        public string ContactName { get; set; }
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
        [Column("TENDERFLG")]
        public string TenderFlg { get; set; }

        public string ContractNo { get; set; }

        public string FCompanyId { get; set; }
        public string FDepartmentId { get; set; }
        public string PCompanyId { get; set; }
        public string PDepartmentId { get; set; }
        public string DepartmentId { get; set; }
        public string CompanyId { get; set; }



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
            this.DepartmentId = LoginUserInfo.Get().departmentId;
            this.CompanyId = LoginUserInfo.Get().companyId;
            this.Id = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {
            this.UpdateTime = DateTime.Now;
            this.UpdateUser = LoginUserInfo.Get().userId;
            this.Id = keyValue;
        }
        #endregion
        #region 扩展字段
       

        #endregion
    }
}

