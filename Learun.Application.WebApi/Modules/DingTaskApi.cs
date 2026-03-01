using Learun.Application.Base.SystemModule;
using Learun.Util;
using Nancy;
using System;
using System.Collections.Generic;
using System.IO;

namespace Learun.Application.WebApi.Modules
{
    public class DingTaskApi : BaseApi
    {
        public DingTaskApi()
            : base("/learun/adms/dingTask")
        {
          
        }
        
    }
}