using Learun.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2022-03-10 22:29
    /// 描 述：项目管理
    /// </summary>
    public class Tast
    {
        public string categoryId { get; set; }
        public string keyword { get; set; }
    }
    /*    public class TastProcess
        {
            public string processId { get; set; }
            public string taskId { get; set; }
        }*/

    public class ProjectVo
    {
        #region 实体成员
        /// <summary>
        /// 省市县
        /// </summary>
        public string F_AreaName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string F_RealName { get; set; }
        public string F_UserId { get; set; }
        //营销人员名字
        public string FollowPersonName { get; set; }
        //报备人员名字
        public string PreparedPersonName { get; set; }
        //报备人员部门
        public string PreparedPersonDeptName { get; set; }
        //项目状态名字
        public string ProjectStatusName { get; set; }
        //项目来源名字
        public string ProjectSourceName { get; set; }


        public string NPC { get; set; }
        /// <summary>
        /// 全款绩效
        /// </summary>
        public double quanAmountSum { get; set; }
        /// <summary>
        /// 到款绩效
        /// </summary>
        public double daoAmountSum { get; set; }
        public decimal? ContractAmount { get; set; }
        public int index { get; set; }
        public string Id { get; set; }

        public string WorkFlowId { get; set; }

        public string ProjectCode { get; set; }
        public string F_MoreDepartmentId { get; set; }

        public string ProjectSource { get; set; }

        public string ProjectName { get; set; }

        public string CustName { get; set; }

        public string ProvinceId { get; set; }

        public string CityId { get; set; }

        public string CountyId { get; set; }

        public string Address { get; set; }

        public string ProjectSituation { get; set; }

        public string Remark { get; set; }
        /// <summary>
        /// 合同备注
        /// </summary>
        public string ContractRemark { get; set; }


        public string FollowPerson { get; set; }

        public string FollowPersonAvatar { get; set; }

        public string PreparedPerson { get; set; }

        public string PreparedPersonAvatar { get; set; }

        public string UserId { get; set; }

        public string ProjectStatus { get; set; }

        public string ContactPhone { get; set; }

        public string ContactName { get; set; }

        public string CreateTimeyMd { get; set; }
        public DateTime? CreateTime { get; set; }

        public string CreateUser { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string UpdateUser { get; set; }
        /// <summary>
        /// 是否投标
        /// </summary>
        public string TenderFlg { get; set; }
        public string TenderFlgName { get; set; }
        /// <summary>
        /// 合同编号
        /// </summary>
        public string ContractNo { get; set; }
        public string ContractId { get; set; }

        public string FCompanyId { get; set; }
        public string FDepartmentId { get; set; }
        public string PCompanyId { get; set; }
        public string PDepartmentId { get; set; }
        public string DepartmentId { get; set; }
        public string CompanyId { get; set; }
        /// <summary>
        /// 审批人
        /// </summary>   
        public string Approver { get; set; }
        /// <summary>
        /// 审批时间
        /// </summary>
        public string ApproverTime { get; set; }
        /// <summary>
        /// 省市
        /// </summary>
        public string ProvincesAndcities { get; set; }
        /// <summary>
        /// 主合同
        /// </summary>
        public string MainContract { get; set; }
        public List<ProjectEntity> contractEntities { get; set; }
        /// <summary>
        /// 合同分类
        /// </summary>
        public string ContractType { get; set; }
        /// <summary>
        /// 合同主体
        /// </summary>
        public string ContractSubject { get; set; }
        /// <summary>
        /// 合同
        /// </summary>
        public ProjectContractVo clist { get; set; }
        public List<ProjectTaskVo> tlist { get; set; }
        /// <summary>
        /// 现场负责人
        /// </summary>
        public string SiteContact { get; set; }
        /// <summary>
        /// 现场联系电话
        /// </summary>
        public string SitePhone { get; set; }
        /// <summary>
        /// 项目负责人
        /// </summary>
        public string ProjectResponsible { get; set; }
        /// <summary>
        /// 检测员
        /// </summary>
        public string Inspector { get; set; }
        /// <summary>
        /// 报告备注
        /// </summary>
        public string TaskRemark { get; set; }
        /// <summary>
        /// 进场时间
        /// </summary>
        public DateTime? ApproachTime { get; set; }
        /// <summary>
        /// 报告计划时间
        /// </summary>
        public DateTime? PlanTime { get; set; }
        /// <summary>
        /// 检测内容
        /// </summary>
        public string TestContent { get; set; }
        /// <summary>
        /// 真实检测目的
        /// </summary>
        public string TestTarget { get; set; }
        /// <summary>
        /// 当前状态
        /// </summary>
        public string TaskStatus { get; set; }
        #endregion

    }
    public class ProjectDto
    {
        public ProjectEntity form { get; set; }

        public List<ProjectFollowListEntity> followList { get; set; }
    }
}

