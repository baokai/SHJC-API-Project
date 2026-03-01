using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// 描 述：项目任务单
    /// </summary>
    public class ProjectTaskUserEntity
    {
        #region 实体成员
        
        /// <summary>
        /// id
        /// </summary>
        [Column("ID")]
        public string id { get; set; }
        /// <summary>
        /// userId
        /// </summary>
        [Column("USERID")]
        public string userId { get; set; }
        /// <summary>
        /// WorkFlowId
        /// </summary>
        [Column("WORKFLOWID")]
        public string WorkFlowId { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        [Column("PROJECTID")]
        public string ProjectId { get; set; }
        [Column("SITECONTACT")]
        public string SiteContact { get; set; }
        [Column("SITEPHONE")]
        public string SitePhone { get; set; }

        [Column("REPORTAPPROVER")]
        public string ReportApprover { get; set; }
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
            this.TaskStatus = 1;
            this.id = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 新增调用
        /// </summary>
        public void CreateTask()
        {
            this.CreateTime = DateTime.Now;
            this.UpdateTime = DateTime.Now;
            this.TaskStatus = 1;
            this.id = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {

            this.UpdateTime = DateTime.Now;
            this.id = keyValue;
        }
       
        #endregion
        #region 扩展字段
        #endregion
    }
}

