using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2022-03-11 00:18
    /// 描 述：项目任务单
    /// </summary>
    public class ProjectTaskEntity
    {
        #region 实体成员

        /// <summary>
        /// id
        /// </summary>
        [Column("ID")]
        public string id { get; set; }
        /// <summary>
        /// WorkFlowId
        /// </summary>
        [Column("WORKFLOWID")]
        public string WorkFlowId { get; set; }
        /// <summary>
        /// 报告编号
        /// </summary>
        [Column("PROJECTTASKNO")]
        public string ProjectTaskNo { get; set; }
        /// <summary>
        /// 报告编号
        /// </summary>
        [Column("PROJECTTASKNOORIGIN")]
        public string ProjectTaskNoOrigin { get; set; }
        
        /// <summary>
        /// 主部门
        /// </summary>
        [Column("MAINDEPARTMENTID")]
        public string MainDepartmentId { get; set; }
        /// <summary>
        /// 主部门金额
        /// </summary>
        [Column("MAINAMOUNT")]
        public decimal? MainAmount { get; set; } 
        /// <summary>
        /// 次部门
        /// </summary>
        [Column("SUBDEPARTMENTID")]
        public string SubDepartmentId { get; set; }
        /// <summary>
        /// 次部门金额
        /// </summary>
        [Column("SUBAMOUNT")]
        public decimal? SubAmount { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        [Column("PROJECTID")]
        public string ProjectId { get; set; }
        [Column("CONTRACTID")]
        public string ContractId { get; set; }

        [Column("SITECONTACT")]
        public string SiteContact { get; set; }
        [Column("SITEPHONE")]
        public string SitePhone { get; set; }

        [Column("REPORTAPPROVER")]
        public string ReportApprover { get; set; }
        /// <summary>
        /// 检测员
        /// </summary>
        [Column("INSPECTOR")]
        public string Inspector { get; set; }
        [Column("REPORTFILE")]
        public string ReportFile { get; set; }
        /// <summary>
        /// 项目负责人
        /// </summary>
        [Column("PROJECTRESPONSIBLE")]
        public string ProjectResponsible { get; set; }
        /// <summary>
        /// 报告主体
        /// </summary>
        [Column("REPORTSUBJECT")]
        public string ReportSubject { get; set; }
        /// <summary>
        /// 进场时间
        /// </summary>
        [Column("APPROACHTIME")]
        public DateTime? ApproachTime { get; set; }
        /// <summary>
        /// 报告计划时间
        /// </summary>
        [Column("PLANTIME")]
        public DateTime? PlanTime { get; set; }
        /// <summary>
        /// 合同委托时间
        /// </summary>
        [Column("ARRANGETIME")]
        public DateTime? ArrangeTime { get; set; }
        /// <summary>
        /// 测试内容
        /// </summary>
        [Column("TESTCONTENT")]
        public string TestContent { get; set; }
        /// <summary>
        /// 测试目标
        /// </summary>
        [Column("TESTTARGET")]
        public string TestTarget { get; set; }
        [Column("REMARK")]
        public string Remark { get; set; }

        /// <summary>
        /// 报告状态
        /// </summary>
        [Column("TASKSTATUS")]
        public int? TaskStatus { get; set; }
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

        [Column("REPORTSUBMITTER")]
        public string ReportSubmitter { get; set; }

        [Column("REPORTSUBMITTERDEPTCODE")]
        public string ReportSubmitterDeptCode { get; set; }

        [Column("REPORTSUBMITTERCOMPANYCODE")]
        public string ReportSubmitterCompanyCode { get; set; }
        [Column("DEPARTMENTID")]
        public string DepartmentId { get; set; }
        /// <summary>
        /// 创建人部门
        /// </summary>
        [Column("TASKDEPARTMENTID")]
        public string TaskDepartmentId { get; set; }
        /// <summary>
        /// 审批人
        /// </summary>
        [Column("APPROVER")]
        public string Approver { get; set; }
        /// <summary>
        /// 审批时间
        /// </summary>
        [Column("APPROVERTIME")]
        public string ApproverTime { get; set; }
        /// <summary>
        /// 模板上传
        /// </summary>
        [Column("REPORTTEMPLATEFILE")]
        public string ReportTemplateFile { get; set; }

        /// <summary>
        /// 实际进场时间
        /// </summary>
        [Column("ACTUALAPPROACHTIME")]
        public DateTime? ActualApproachTime { get; set; }
        /// <summary>
        /// 实际离场时间
        /// </summary>
        [Column("ACTUALDEPARTURETIME")]
        public DateTime? ActualDepartureTime{get;set;}
        /// <summary>
        /// 盖章完成时间（报告时间）
        /// </summary>
        [Column("FLOWFINISHEDTIME")]
        public DateTime? FlowFinishedTime { get; set; }
        /// <summary>
        /// 报告评级
        /// </summary>
        [Column("RATING")]
        public int? Rating { get; set; }
        [Column("PLANFINISHTIME")]
        public DateTime? PlanFinishTime { get; set; }
        [Column("PLANAPPROACHTIME")]
        public DateTime? PlanApproachTime { get; set; }
        #endregion
        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void CreateTask()
        {
            this.CreateTime = DateTime.Now;
            this.UpdateTime = DateTime.Now; 
            this.TaskStatus = 1;
            this.TaskDepartmentId= LoginUserInfo.Get().departmentId; 
            this.id = Guid.NewGuid().ToString();
        }
         /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.CreateTime = DateTime.Now;
            this.UpdateTime = DateTime.Now;
            this.UpdateUser = LoginUserInfo.Get().userId;
            this.CreateUser = LoginUserInfo.Get().userId;
           // this.TaskDepartmentId = LoginUserInfo.Get().departmentId;
            this.TaskStatus = 1;
            this.id = Guid.NewGuid().ToString();
        }
        public void CreateTast()
        {
            this.CreateTime = DateTime.Now;
            this.UpdateTime = DateTime.Now;
            this.UpdateUser = LoginUserInfo.Get().userId;
            this.CreateUser = LoginUserInfo.Get().userId;
           // this.TaskDepartmentId = LoginUserInfo.Get().departmentId;
            this.TaskStatus = 1;
            this.id = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 新增调用（变更）
        /// </summary>
        public void CreateIn()
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
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void ModifyTest(string keyValue)
        {
            this.UpdateTime = DateTime.Now;

            this.id = keyValue;
        }
        /// <summary>
        /// 编辑调用（变更）
        /// </summary>
        /// <param name="keyValue"></param>
        public void ModifyUp(string keyValue)
        {
            this.UpdateTime = DateTime.Now;
            this.UpdateUser = LoginUserInfo.Get().userId;
            this.id = keyValue;
        }
        /// <summary>
        /// 编辑调用(报告上传)
        /// </summary>
        /// <param name="keyValue"></param>
        public void ModifySC(string keyValue)
        {

            this.id = keyValue;
        }
        #endregion
       
    }
}

