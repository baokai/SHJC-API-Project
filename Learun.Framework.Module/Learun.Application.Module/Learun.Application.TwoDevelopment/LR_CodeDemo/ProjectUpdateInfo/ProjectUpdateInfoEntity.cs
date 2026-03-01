using Learun.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    public class ProjectUpdateInfoEntity
    {
        #region 实体成员

        public string id { get; set; }

        public DateTime CreateTime { get; set; }

        public string CreateUser { get; set; }

        public string OldDepartmentId { get; set; }

        public string NewDepartmentId { get; set; }

        public string Type { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {

            this.CreateTime = DateTime.Now;
            this.CreateUser = LoginUserInfo.Get().userId;
            this.id = Guid.NewGuid().ToString();
        }
        #endregion
    }
}
