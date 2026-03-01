using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.Base.SystemModule
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 上海力软信息技术有限公司
    /// 创建人：力软-框架开发组
    /// 日 期：2017.03.08
    /// 描 述：数据字典明细
    /// </summary>
    public class DataItemDetailByDataItemVo
    {
        #region 实体成员
      
      
        public string F_ItemName { get; set; }

        public string F_ItemId { get; set; }
        
        public string F_ParentId { get; set; }
       
        public string F_ItemCode { get; set; }
      
       
        public int? F_IsTree { get; set; }
       
        public int? F_IsNav { get; set; }
        
        public int? F_SortCode { get; set; }
       
        public int? F_DeleteMark { get; set; }
       
        public int? F_EnabledMark { get; set; }
       
        public string F_Description { get; set; }
        
        public DateTime? F_CreateDate { get; set; }
       
        public string F_CreateUserId { get; set; }
       
        public string F_CreateUserName { get; set; }
       
        public DateTime? F_ModifyDate { get; set; }
       
        public string F_ModifyUserId { get; set; }
       
        public string F_ModifyUserName { get; set; }


        #endregion


    }
}
