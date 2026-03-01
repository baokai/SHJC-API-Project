using Learun.Application.Base.AuthorizeModule;
using Learun.Application.Base.SystemModule;
using Learun.Application.Organization;
using Learun.Util;
using Learun.Util.Operat;
using Nancy;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Learun.Application.WebApi
{
    /// <summary>
    /// 日 期：2017.05.12
    /// 描 述：用户信息
    /// </summary>
    public class UserApi : BaseApi
    {
        /// <summary>
        /// 注册接口
        /// </summary>
        public UserApi()
            : base("/learun/adms/user")
        {
            Post["/login"] = Login;
            Post["/loginByUserCode"] = LoginByUserCode;
            Post["/modifypw"] = ModifyPassword;
            Post["/getUserList"] = GetUserList;
            Post["/syncUserInfo"] = SyncUserInfo;
            Post["/syncDeptInfo"] = SyncDeptInfo;
            Post["/pHandOver"] = PHandOver;


            //Post["/create"] = Create;

            Get["/info"] = Info;
            Get["/map"] = GetMap;
            Get["/img"] = GetImg;
        }
        private UserIBLL userIBLL = new UserBLL();
        private PostIBLL postIBLL = new PostBLL();
        private RoleIBLL roleIBLL = new RoleBLL();
        private DepartmentIBLL deptIBLL = new DepartmentBLL();
        private UserRelationIBLL userRelationIBLL = new UserRelationBLL();

        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private Response Login(dynamic _)
        {
            LoginModel loginModel = this.GetReqData<LoginModel>();
            loginModel.username = loginModel.username.ToUpper();
            #region 内部账户验证

            UserEntity userEntity = userIBLL.GetEntityByAccount(loginModel.username);
            if (userEntity == null)
            {
                //写入日志
                LogEntity logEntity1 = new LogEntity();
                logEntity1.F_ExecuteResult = 0;
                logEntity1.F_ExecuteResultJson =  "登录失败:" + userEntity.LoginMsg;
                logEntity1.WriteLog();
                return Fail(userEntity.LoginMsg);
            }
            #region 写入日志
            LogEntity logEntity = new LogEntity();
            logEntity.F_CategoryId = 1;
            logEntity.F_OperateTypeId = ((int)OperationType.Login).ToString();
            logEntity.F_OperateType = EnumAttribute.GetDescription(OperationType.Login);
            logEntity.F_OperateAccount = loginModel.username + "(" + userEntity.F_RealName + ")";
            logEntity.F_OperateUserId = !string.IsNullOrEmpty(userEntity.F_UserId) ? userEntity.F_UserId : loginModel.username;
            logEntity.F_Module = Config.GetValue("SoftName");
            #endregion

            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = "http://127.0.0.1:8088/appLogin?username=" + loginModel.username + "&password=" + loginModel.password;
                    string str = client.DownloadString(url);
                    var responseData = JsonConvert.DeserializeObject<JavaResModel>(str);
                    //校验失败
                    if (responseData.code != 200)
                    {
                        logEntity.F_ExecuteResult = 0;
                        logEntity.F_ExecuteResultJson = "登录失败:账号或密码不正确";
                        logEntity.WriteLog();
                        return Fail(logEntity.F_ExecuteResultJson);
                    }
                    else
                    {
                        string token = OperatorHelper.Instance.AddLoginUser(userEntity.F_Account, "App", this.loginMark, false);//写入缓存信息
                                                                                                                                //写入日志
                        logEntity.F_ExecuteResult = 1;
                        logEntity.F_ExecuteResultJson = "登录成功";
                        logEntity.WriteLog();

                        OperatorResult res = OperatorHelper.Instance.IsOnLine(token, this.loginMark);
                        res.userInfo.password = null;
                        res.userInfo.secretkey = null;

                        var jsonData = new
                        {
                            baseinfo = res.userInfo,
                            post = postIBLL.GetListByPostIds(res.userInfo.postIds),
                            role = roleIBLL.GetListByRoleIds(res.userInfo.roleIds),
                            HZ = userEntity.F_HZ
                        };
                        return Success(jsonData);
                    }
                }
                catch (WebException e)
                {
                    Console.WriteLine($"请求异常: {e.Message}");
                    logEntity.F_ExecuteResult = 0;
                    logEntity.F_ExecuteResultJson = $"请求异常: {e.Message}";
                    logEntity.WriteLog();
                    return Fail(logEntity.F_ExecuteResultJson);
                }
            }
            #endregion
        }

        private Response LoginByUserCode(dynamic _)
        {
            var account = this.GetReqData();
            UserEntity userEntity = userIBLL.GetEntityByAccount(account);
            if (userEntity == null)
            {
                //写入日志
                LogEntity logEntity = new LogEntity();
                logEntity.F_ExecuteResult = 0;
                logEntity.F_ExecuteResultJson = account + "登录失败:" + userEntity.LoginMsg;
                logEntity.WriteLog();
                return Fail(userEntity.LoginMsg);

            }
            else
            {
                #region 写入日志
                LogEntity logEntity = new LogEntity();
                logEntity.F_CategoryId = 1;
                logEntity.F_OperateTypeId = ((int)OperationType.Login).ToString();
                logEntity.F_OperateType = EnumAttribute.GetDescription(OperationType.Login);
                logEntity.F_OperateAccount = account + "(" + userEntity.F_RealName + ")";
                logEntity.F_OperateUserId = !string.IsNullOrEmpty(userEntity.F_UserId) ? userEntity.F_UserId : account;
                logEntity.F_Module = Config.GetValue("SoftName");
                #endregion
                string loginMark = Guid.NewGuid().ToString();
                string token = OperatorHelper.Instance.AddLoginUser(userEntity.F_Account, "App", loginMark, false);//写入缓存信息
                //写入日志
                logEntity.F_ExecuteResult = 1;
                logEntity.F_ExecuteResultJson = "登录成功";
                logEntity.WriteLog();
                OperatorResult res = OperatorHelper.Instance.IsOnLine(token, loginMark);
                res.userInfo.password = null;
                res.userInfo.secretkey = null;

                var jsonData = new
                {
                    queryJson = userEntity.F_Account,
                    loginMark = loginMark,
                    token = token
                };
                return Success(jsonData);
            }



        }
        private Response LoginByUserCodeFromJava(dynamic _)
        {
            var account = this.GetReqData();
            UserEntity userEntity = userIBLL.GetEntityByAccount(account);
            if (userEntity == null)
            {
                //写入日志
                LogEntity logEntity = new LogEntity();
                logEntity.F_ExecuteResult = 0;
                logEntity.F_ExecuteResultJson = account + "登录失败:" + userEntity.LoginMsg;
                logEntity.WriteLog();
                return Fail(userEntity.LoginMsg);

            }
            else
            {
                #region 写入日志
                LogEntity logEntity = new LogEntity();
                logEntity.F_CategoryId = 1;
                logEntity.F_OperateTypeId = ((int)OperationType.Login).ToString();
                logEntity.F_OperateType = EnumAttribute.GetDescription(OperationType.Login);
                logEntity.F_OperateAccount = account + "(" + userEntity.F_RealName + ")";
                logEntity.F_OperateUserId = !string.IsNullOrEmpty(userEntity.F_UserId) ? userEntity.F_UserId : account;
                logEntity.F_Module = Config.GetValue("SoftName");
                #endregion
                string token = OperatorHelper.Instance.AddLoginUser(userEntity.F_Account, "App", this.loginMark, false);//写入缓存信息
                //写入日志
                logEntity.F_ExecuteResult = 1;
                logEntity.F_ExecuteResultJson = "登录成功";
                logEntity.WriteLog();
                OperatorResult res = OperatorHelper.Instance.IsOnLine(token, this.loginMark);
                res.userInfo.password = null;
                res.userInfo.secretkey = null;

                var jsonData = new
                {
                    loginMark = this.loginMark,
                    token = token
                };
                return Success(jsonData);
            }



        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns> 
        private Response Info(dynamic _)
        {
            var data = userInfo;
            data.password = null;
            data.secretkey = null;

            var jsonData = new
            {
                baseinfo = data,
                post = postIBLL.GetListByPostIds(data.postIds),
                role = roleIBLL.GetListByRoleIds(data.roleIds)
            };

            return Success(jsonData);
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private Response ModifyPassword(dynamic _)
        {
            ModifyModel modifyModel = this.GetReqData<ModifyModel>();
            if (userInfo.isSystem)
            {
                return Fail("当前账户不能修改密码");
            }
            else
            {
                bool res = userIBLL.RevisePassword(modifyModel.newpassword, modifyModel.oldpassword);
                if (!res)
                {
                    return Fail("原密码错误，请重新输入");
                }
                else
                {
                    return Success("密码修改成功");
                }
            }
        }


        /// <summary>
        /// 获取所有员工账号列表
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private Response GetList(dynamic _)
        {
            var data = userInfo;
            data.password = null;
            data.secretkey = null;
            var jsonData = new
            {
                baseinfo = data,
                post = postIBLL.GetListByPostIds(data.postIds),
                role = roleIBLL.GetListByRoleIds(data.roleIds)
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 获取用户映射表
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetMap(dynamic _)
        {
            string ver = this.GetReqData();// 获取模板请求数据
            var data = userIBLL.GetModelMap();
            string md5 = Md5Helper.Encrypt(data.ToJson(), 32);
            if (md5 == ver)
            {
                return Success("no update");
            }
            else
            {
                var jsondata = new
                {
                    data = data,
                    ver = md5
                };
                return Success(jsondata);
            }
        }
        /// <summary>
        /// 获取人员头像图标
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetImg(dynamic _)
        {
            string userId = this.GetReqData();// 获取模板请求数据
            userIBLL.GetImg(userId);
            return Success("获取成功");
        }
        /// <summary>
        /// 获取所有user
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetUserList(dynamic _)
        {
            var data = userInfo;
            data.password = null;
            data.secretkey = null;
            var jsonData = new
            {
                baseinfo = data,
                userList = userIBLL.GetAllList()
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 保存同步userInfo
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response SyncUserInfo(dynamic _)
        {
            UserEntity userInfo = this.GetReqData<UserEntity>();
            var curr_user = userIBLL.GetEntityByUserId(userInfo.F_UserId);
            //更新
            if (curr_user != null)
            {
                curr_user.F_Account = userInfo.F_Account;
                curr_user.F_RealName = userInfo.F_RealName;
                curr_user.F_DepartmentId = userInfo.F_DepartmentId;
                curr_user.F_HZ = userInfo.F_HZ;
                curr_user.F_EnCode = userInfo.F_EnCode;
                curr_user.Modify(curr_user.F_UserId);
                userIBLL.SaveEntity(curr_user.F_UserId, curr_user);
                var role_list = userRelationIBLL.GetUserRoleList(userInfo.F_UserId);
                bool isExisted = role_list.All(i=>i.F_ObjectId == "d865e9b7-f412-4c98-9b6c-ea6c5d449006");
                if (!isExisted)
                {
                    //分配通用权限
                    UserRelationEntity userRelation = new UserRelationEntity();
                    userRelation.Create();
                    userRelation.F_Category = 1;
                    userRelation.F_ObjectId = "d865e9b7-f412-4c98-9b6c-ea6c5d449006";
                    userRelation.F_UserId = userInfo.F_UserId;
                    userRelationIBLL.SaveEntityList(userRelation.F_ObjectId, 1, userRelation.F_UserId);
                }
                return Success(1);
            }
            //新增
            else
            {
                return Success(SyncUserAdd(userInfo));
            }

        }
        private string SyncUserAdd(UserEntity userInfo)
        {
            var curr_user = userIBLL.GetEntityByAccount(userInfo.F_Account);
            if (curr_user == null)
            {
                userInfo.F_CompanyId = "207fa1a9-160c-4943-a89b-8fa4db0547ce";
                userInfo.F_Password = "123456";
                userIBLL.SaveNewEntity(userInfo.F_UserId, userInfo);
                UserRelationEntity userRelation = new UserRelationEntity();
                userRelation.Create();
                userRelation.F_Category = 1;
                //分配通用权限
                userRelation.F_ObjectId = "d865e9b7-f412-4c98-9b6c-ea6c5d449006";
                userRelation.F_UserId = userInfo.F_UserId;
                userRelationIBLL.SaveEntityList(userRelation.F_ObjectId, 1, userRelation.F_UserId);
            }
            else
            {
                //更新
                if (curr_user != null)
                {
                    curr_user.F_Account = userInfo.F_Account;
                    curr_user.F_RealName = userInfo.F_RealName;
                    curr_user.F_DepartmentId = userInfo.F_DepartmentId;
                    curr_user.F_HZ = userInfo.F_HZ;
                    curr_user.F_EnCode = userInfo.F_EnCode;
                    curr_user.Modify(curr_user.F_UserId);
                    userIBLL.SaveEntity(curr_user.F_UserId, curr_user);
                }
            }
            return "1";
        }
        /// <summary>
        /// 保存同步部门信息
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response SyncDeptInfo(dynamic _)
        {
            DepartmentEntity deptInfo = this.GetReqData<DepartmentEntity>();
            var curr_dept = deptIBLL.GetEntity(deptInfo.F_DepartmentId);
            if (curr_dept == null)
            {
                deptInfo.F_FullName = !string.IsNullOrEmpty(deptInfo.F_FullName) ? deptInfo.F_FullName : deptInfo.F_ShortName;
                deptInfo.F_EnCode = DateTime.Now.ToString("yyMMdd");
                deptInfo.F_CompanyId = "207fa1a9-160c-4943-a89b-8fa4db0547ce";
                deptIBLL.SaveEntity("", deptInfo);
            }
            else
            {
                curr_dept.Modify(curr_dept.F_DepartmentId);
                curr_dept.F_FullName = !string.IsNullOrEmpty(deptInfo.F_FullName) ? deptInfo.F_FullName : deptInfo.F_ShortName;
                curr_dept.F_ShortName = deptInfo.F_ShortName;
                curr_dept.F_EnCode = deptInfo.F_EnCode;
                curr_dept.HZ_DepartmentId = deptInfo.HZ_DepartmentId;
                curr_dept.F_Manager = deptInfo.F_Manager;
                deptIBLL.SaveEntity(curr_dept.F_DepartmentId, curr_dept);
            }
            return Success(1);
        }
        /// <summary>
        /// 人员转接
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response PHandOver(dynamic _)
        {
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            string jsonText = string.Empty;

            HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空
            StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
            jsonText = sr.ReadToEnd();
            userIBLL.P_HandOver(jsonText);
            return Success("保存成功！");
        }
    }

    /// <summary>
    /// 登录信息
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
    }
    /// <summary>
    /// 修改密码
    /// </summary>
    public class ModifyModel
    {
        /// <summary>
        /// 新密码
        /// </summary>
        public string newpassword { get; set; }
        /// <summary>
        /// 旧密码
        /// </summary>
        public string oldpassword { get; set; }
    }


}