using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.Base.SystemModule
{
    /// <summary>
    /// 
    /// </summary>
    public class SharefilelistVo
    {

        public int Id { get; set; }

        public string FolderId { get; set; }

        public string FileName { get; set; }

        public DateTime? CreateTime { get; set; }

        public string CreateUserId { get; set; }

        public string CreateUserName { get; set; }
        public string DepartmentId { get; set; }

        public string DepartmentName { get; set; }

    }
}
