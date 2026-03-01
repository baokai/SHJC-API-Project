using Learun.Application.Base.SystemModule;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
   

    public class ProjectTaskVo:IEquatable<ProjectTaskVo>
    {

        #region 实体成员
        /// <summary>
        /// TaskRemark
        /// </summary>
        public string TaskRemark { get; set; }
        public string time { get; set; }
        public string CreateTimeyMd { get; set; }
        public string ApproachTimeMd { get; set; }
        public string MainDepartmentName { get; set; }
        public string SubDepartmentName { get; set; }
        public string ActualDepartureTimeMd { get; set; }
        public string ContractSubjectName { get; set; }
        public string PlanTimeMd { get; set; }
        public string PlanFinishTimeMd { get; set; }
        public string PlanApproachTimeMd { get; set; }
        /// <summary>
        /// 创建人部门
        /// </summary>
        public string TaskDepartmentId { get; set; }
        public string ProjectTaskNo { get; set; }
        public string ProjectTaskNoOrigin { get; set; }

        public string DepartmentName { get; set; }

        public string ContractYXDeptId { get; set; }
        public string ContractYXDeptName { get; set; }
        /// <summary>
        /// 主部门
        /// </summary>

        public string MainDepartmentId { get; set; }
        /// <summary>
        /// 主部门金额
        /// </summary>
       
        public decimal? MainAmount { get; set; }
        /// <summary>
        /// 次部门
        /// </summary>
       
        public string SubDepartmentId { get; set; }
        /// <summary>
        /// 次部门金额
        /// </summary>
      
        public decimal? SubAmount { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string F_RealName { get; set; }
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// WorkFlowId
        /// </summary>
        public string WorkFlowId { get; set; }
        /// <summary>
        /// 合同编号
        /// </summary>
        public string ProjectCode { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string ReportApprover { get; set; }
        /// <summary>
        /// 变更记录
        /// </summary>
        public string Change { get; set; }
        /// <summary>
        /// 评级
        /// </summary>
        public string RatingName { get; set; }
        public int index { get; set; }
        public string TaskStatusName { get; set; }
        public string ContractNo { get; set; }
        public string ContractId { get; set; }
        public string ProjectName { get; set; }

        public string ContractSubject { get; set; }
        public string FollowPerson { get; set; }
        public string PreparedPerson { get; set; }
        public string SiteContact { get; set; }
        public string SitePhone { get; set; }
        public string Inspector { get; set; }
        public string Remark { get; set; }
        public string ReportFile { get; set; }

        public string DepartmentId { get; set; }
        /// <summary>
        /// 报告审核人
        /// </summary>
        public string ReportApproverName { get; set; }
        /// <summary>
        /// 报告主体
        /// </summary>
        public string ReportSubjectName { get; set; }
        /// <summary>
        /// 报告评级
        /// </summary>
        public string Rating { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// 项目负责人
        /// </summary>
        public string ProjectResponsible { get; set; }
        /// <summary>
        /// 报告主体
        /// </summary>
        public string ReportSubject { get; set; }
        /// <summary>
        /// 进场时间
        /// </summary>
        public DateTime? ApproachTime { get; set; }
        /// <summary>
        /// 报告计划时间
        /// </summary>
        public DateTime? PlanTime { get; set; }
        public DateTime? PlanFinishTime { get; set; }
        public DateTime? PlanApproachTime { get; set; }
        public DateTime? ArrangeTime { get; set; }
        /// <summary>
        /// 实际离场时间
        /// </summary>
        public DateTime? ActualDepartureTime { get; set; }
        /// <summary>
        /// 盖章完成时间（报告时间）
        /// </summary>
        public DateTime? FlowFinishedTime { get; set; }
        public string FlowFinishedTimeMD { get; set; }
        /// <summary>
        /// 测试内容
        /// </summary>
        public string TestContent { get; set; }
        /// <summary>
        /// 测试目标
        /// </summary>
        public string TestTarget { get; set; }
        /// <summary>
        /// 报告状态
        /// </summary>
        public string TaskStatus { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdateUser { get; set; }


        //委托单位
        public string CustName { get; set; }
        /// <summary>
        /// 审批人
        /// </summary>
        public string Approver { get; set; }
        /// <summary>
        /// 审批时间
        /// </summary> 
        public string ApproverTime { get; set; }
        //联系人
        public string ContactPhone { get; set; }
        /// <summary>
        /// 模板上传
        /// </summary>
        public string ReportTemplateFile { get; set; }
        public string ProjectResponsibleName { get; set; }
        public string InspectorName { get; set; }




        public string PDepartmentId { get; set; }

        public string FDepartmentId { get; set; }

       // public int YJ { get; set; }
        public string YJ { get; set; }
     
        /// 任务附件列表
        /// </summary>
        public IEnumerable<AnnexesFileEntity> annexesFileEntities { get; set; }

        public List<ProjectTaskVo> projectTasks { get; set; }
        public DateTime? ActualApproachTime { get; set; }
       
        public void CreateInsert()
        {
            this.CreateTime = DateTime.Now;
            this.UpdateTime = DateTime.Now;
            this.UpdateUser = LoginUserInfo.Get().userId;
            this.CreateUser = LoginUserInfo.Get().userId;
            this.TaskStatus = "4";
            this.id = Guid.NewGuid().ToString();
        }


        public bool Equals(ProjectTaskVo other)
        {
            return this.id == other.id && this.WorkFlowId == other.WorkFlowId && this.ReportApprover == other.ReportApprover && this.Change == other.Change && this.TaskStatusName == other.TaskStatusName && this.ContractNo == other.ContractNo && this.ProjectName == other.ProjectName && this.ContractSubject == other.ContractSubject && this.PreparedPerson == other.PreparedPerson && this.SiteContact == other.SiteContact && this.SitePhone == other.SitePhone && this.Inspector == other.Inspector && this.Remark == other.Remark && this.ReportFile == other.ReportFile && this.DepartmentId == other.DepartmentId && this.ProjectId == other.ProjectId && this.ProjectResponsible == other.ProjectResponsible && this.ReportSubject == other.ReportSubject && this.ApproachTime == other.ApproachTime && this.PlanTime == other.PlanTime && this.TestContent == other.TestContent && this.TestTarget == other.TestTarget && this.TaskStatus == other.TaskStatus && this.CreateTime == other.CreateTime && this.CreateUser == other.CreateUser && this.UpdateTime == other.UpdateTime && this.UpdateUser == other.UpdateUser && this.CustName == other.CustName && this.ContactPhone == other.ContactPhone && this.annexesFileEntities == other.annexesFileEntities
;
        }

        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {
            this.UpdateTime = DateTime.Now;
            this.UpdateUser = LoginUserInfo.Get().userId;
            this.TaskStatus = "6";
            this.id = keyValue;
        }
        #endregion
    }
    
    }
