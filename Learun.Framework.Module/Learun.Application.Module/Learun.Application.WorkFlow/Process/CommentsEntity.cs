using Learun.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.WorkFlow
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 上海力软信息技术有限公司
    /// 创建人：力软-框架开发组
    /// 日 期：2018.12.06
    /// 描 述：工作流进程(新)
    /// </summary>
  /*  public class FlowComment
    {
       public string Id { get; set; }
        public string CreateUserName { get; set; }
        public string CreateUserId { get; set; }
        public string CommentsName { get; set; }
        public DateTime? CreateTime { get; set; }

    }
       
    public class ComNodeList
    {
        public string nodIdlisty { get; set; }
      
        public List<FlowComment> nodeslistNodelisty { get; set; }


    }*/
    public class CommentsEntity
    {
        #region 实体成员 
        /// <summary> 
        /// 主键 
        /// </summary> 
        /// <returns></returns> 
        [Column("F_ID")]
        public string F_Id { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
         [Column("COMMENTSNAME")]
        public string CommentsName { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CREATEUSER")]
        public string CreateUser { get; set; }
        /// <summary>
        /// 创建人名字
        /// </summary>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CREATETIME")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// nodId
        /// </summary>
        [Column("NODID")]
        public string  nodId { get; set; }
       
        /// <summary>
        /// 流程id
        /// </summary>
        [Column("WORKFLOWID")]
        public string WorkFlowId { get; set; } 
      
        #endregion

        #region 扩展操作 
       
        /// <summary> 
        /// 新增调用 
        /// </summary> 
        public void Create()
        {
            this.CreateTime = DateTime.Now;
            this.CreateUser = LoginUserInfo.Get().userId;
            this.F_Id = Guid.NewGuid().ToString();
           
        }
        /// <summary> 
        /// 编辑调用 
        /// </summary> 
        public void Modify(string keyValue)
        {
            
            this.F_Id = keyValue;
        }
        #endregion

       
       
    }
}
