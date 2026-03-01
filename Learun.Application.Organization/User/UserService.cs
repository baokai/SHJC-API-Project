using Learun.Application.Organization.User;
using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Learun.Application.Organization
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 上海力软信息技术有限公司
    /// 创建人：力软-框架开发组
    /// 日 期：2017.03.04
    /// 描 述：用户模块数据操作服务类
    /// </summary>
    public class UserService : RepositoryFactory
    {


        #region 属性 构造函数
        private string fieldSql;
        public UserService()
        {
            fieldSql = @" 
                        t.F_UserId,
                        t.F_EnCode,
                        t.F_Account,
                        t.F_Password,
                        t.F_Secretkey,
                        t.F_RealName,
                        t.F_NickName,
                        t.F_HeadIcon,
                        t.F_QuickQuery,
                        t.F_SimpleSpelling,
                        t.F_Gender,
                        t.F_Birthday,
                        t.F_Mobile,
                        t.F_Telephone,
                        t.F_Email,
                        t.F_OICQ,
                        t.F_WeChat,
                        t.F_MSN,
                        t.F_CompanyId,
                        t.F_DepartmentId,
                        t.F_SecurityLevel,
                        t.F_OpenId,
                        t.F_Question,
                        t.F_AnswerQuestion,
                        t.F_CheckOnLine,
                        t.F_AllowStartTime,
                        t.F_AllowEndTime,
                        t.F_LockStartDate,
                        t.F_LockEndDate,
                        t.F_SortCode,
                        t.F_DeleteMark,
                        t.F_EnabledMark,
                        t.F_Description,
                        t.F_CreateDate,
                        t.F_CreateUserId,
                        t.F_CreateUserName,
                        t.F_ModifyDate,
                        t.F_ModifyUserId,
                        t.F_ModifyUserName,
                        t.F_HZ,
t.F_MoreDepartmentId
                        ";
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取实体,通过用户账号
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <returns></returns>
        public UserEntity GetEntityByAccount(string account)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM LR_Base_User t ");
                strSql.Append(" WHERE t.F_Account = @account AND t.F_DeleteMark = 0  ");
                return this.BaseRepository().FindEntity<UserEntity>(strSql.ToString(), new { account = account });
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        /// <summary>
        /// 用户列表(根据公司主键)
        /// </summary>
        /// <param name="companyId">公司主键</param>
        /// <returns></returns>
        public IEnumerable<UserEntity> GetList(string companyId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql.Replace("t.F_Password,", "").Replace("t.F_Secretkey,", ""));
                strSql.Append(" FROM LR_Base_User t WHERE t.F_DeleteMark = 0 AND t.F_CompanyId = @companyId ORDER BY t.F_DepartmentId,t.F_RealName ");
                return this.BaseRepository().FindList<UserEntity>(strSql.ToString(), new { companyId = companyId });
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }


        /// <summary>
        /// 用户列表(根据公司主键)(分页)
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="departmentId"></param>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public IEnumerable<UserEntity> GetPageList(string companyId, string departmentId, Pagination pagination, string keyword)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql.Replace("t.F_Password,", "").Replace("t.F_Secretkey,", ""));
                strSql.Append(" FROM LR_Base_User t WHERE t.F_DeleteMark = 0 AND t.F_CompanyId = @companyId  ");

                if (!string.IsNullOrEmpty(departmentId))
                {
                    strSql.Append(" AND t.F_DepartmentId = @departmentId ");
                }

                if (!string.IsNullOrEmpty(keyword))
                {
                    keyword = "%" + keyword + "%";
                    strSql.Append(" AND( t.F_Account like @keyword or t.F_RealName like @keyword  or t.F_Mobile like @keyword  ) ");
                }

                return this.BaseRepository().FindList<UserEntity>(strSql.ToString(), new { companyId, departmentId, keyword }, pagination);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        /// <summary>
        /// 用户列表,全部
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserEntity> GetAllList()
        {
            try
            {
                //var strSql = new StringBuilder();
                //strSql.Append("SELECT * ");
                //// strSql.Append(" FROM LR_Base_User t WHERE t.F_DeleteMark = 0 and t.F_EnabledMark=1 ORDER BY t.F_CompanyId,t.F_DepartmentId,t.F_RealName ");
                //strSql.Append(" FROM LR_Base_User t WHERE t.F_DeleteMark = 0 ORDER BY t.F_CompanyId,t.F_DepartmentId,t.F_RealName ");
                //return this.BaseRepository().FindList<UserEntity>(strSql.ToString());
                return this.BaseRepository().FindList<UserEntity>(t => t.F_DeleteMark == 0);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        /// <summary>
        /// 用户列表（导出Excel）
        /// </summary>
        /// <returns></returns>
        public DataTable GetExportList()
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(@"SELECT u.F_Account
                                  ,u.F_RealName
                                  ,CASE WHEN u.F_Gender=1 THEN '男' ELSE '女' END AS F_Gender
                                  ,u.F_Birthday
                                  ,u.F_Mobile
                                  ,u.F_Telephone
                                  ,u.F_Email
                                  ,u.F_WeChat
                                  ,u.F_MSN
                                  ,o.F_FullName AS F_Company
                                  ,d.F_FullName AS F_Department
                                  ,u.F_Description
                                  ,u.F_CreateDate
                                  ,u.F_CreateUserName
                              FROM LR_Base_User u
                              INNER JOIN LR_Base_Department d ON u.F_DepartmentId=d.F_DepartmentId
                              INNER JOIN LR_Base_Company o ON u.F_CompanyId=o.F_CompanyId WHERE u.F_DeleteMark = 0 ");
                return this.BaseRepository().FindTable(strSql.ToString());
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        /// <summary>
        /// 用户实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public UserEntity GetEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository().FindEntity<UserEntity>(t => t.F_UserId == keyValue && t.F_DeleteMark == 0);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        /// <summary>
        /// 用户实体(不包括合作伙伴)
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public UserEntity GetEntityHZ(string keyValue)
        {
            try
            {
                return this.BaseRepository().FindEntity<UserEntity>(t => t.F_UserId == keyValue && t.F_DeleteMark == 0 && t.F_HZ != 1);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        /// <summary>
        /// 获取超级管理员用户列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserEntity> GetAdminList()
        {
            try
            {
                return this.BaseRepository().FindList<UserEntity>(t => t.F_SecurityLevel == 1);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        /// <summary>
        /// 根据账号查用户
        /// </summary>
        /// <param name="Accoun"></param>
        /// <returns></returns>
        public UserEntity GetAccountName(string Accoun)
        {
            try
            {
                return this.BaseRepository().FindEntity<UserEntity>(t => t.F_Account == Accoun && t.F_DeleteMark == 0);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }

        }
        /// <summary>
        /// 根据账号查用户
        /// </summary>
        /// <param name="Accoun"></param>
        /// <returns></returns>
        public UserEntityVo GetInspectorName(string name)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(@"SELECT t.F_UserId");
                strSql.Append(" FROM LR_Base_User t WHERE t.F_DeleteMark = 0 AND t.F_RealName='" + name + "'");
                return this.BaseRepository().FindList<UserEntityVo>(strSql.ToString()).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }

        }
        public UserEntityVo GetHZUserId(string GetUserId)
        {
            try
            {
                string sql = "SELECT t.F_HZ,t.F_MoreDepartmentId,t.F_DepartmentId "
                    + " FROM LR_Base_User t WHERE t.F_DeleteMark = 0 AND t.F_UserId='" + GetUserId + "'";
                //var strSql = new StringBuilder();
                //strSql.Append(@"SELECT t.F_HZ,t.F_MoreDepartmentId,t.F_DepartmentId");           
                //strSql.Append(" FROM LR_Base_User t WHERE t.F_DeleteMark = 0 AND t.F_UserId='" + GetUserId + "'");
                return this.BaseRepository().FindList<UserEntityVo>(sql).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        public UserEntityVo GetHZUserId_2(string GetUserId)
        {
            try
            {
                string sql = "SELECT t.F_HZ,t.F_MoreDepartmentId,t.F_DepartmentId "
                    + " FROM LR_Base_User t WHERE t.F_DeleteMark = 0 AND t.F_UserId='" + GetUserId + "'";
                //var strSql = new StringBuilder();
                //strSql.Append(@"SELECT t.F_HZ,t.F_MoreDepartmentId,t.F_DepartmentId");           
                //strSql.Append(" FROM LR_Base_User t WHERE t.F_DeleteMark = 0 AND t.F_UserId='" + GetUserId + "'");
                return this.BaseRepository().FindList_NodbWhere<UserEntityVo>(sql).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        /// <summary>
        /// 根据用户名获取用户id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserEntityVo GetHZUserName(string userId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(@"SELECT t.F_UserId ,t.F_DepartmentId");
                strSql.Append(" FROM LR_Base_User t WHERE t.F_DeleteMark = 0 AND t.F_UserId like '%" + userId + "%'");
                return this.BaseRepository().FindList<UserEntityVo>(strSql.ToString()).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }

        }

        #endregion

        #region 验证数据
        /// <summary>
        /// 账户不能重复
        /// </summary>
        /// <param name="account">账户值</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistAccount(string account, string keyValue)
        {
            try
            {
                var expression = LinqExtensions.True<UserEntity>();
                expression = expression.And(t => t.F_Account == account);
                if (!string.IsNullOrEmpty(keyValue))
                {
                    expression = expression.And(t => t.F_UserId != keyValue);
                }
                return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 虚拟删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void VirtualDelete(string keyValue)
        {
            try
            {
                UserEntity entity = new UserEntity()
                {
                    F_UserId = keyValue,
                    F_DeleteMark = 1
                };
                this.BaseRepository().Update(entity);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        /// <summary>
        /// 保存用户表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="userEntity">用户实体</param>
        /// <returns></returns>
        public void SaveEntity(string keyValue, UserEntity userEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(keyValue))
                {
                    if (!string.IsNullOrEmpty(userEntity.F_UserId))
                    {
                        userEntity.Create_2();
                    }
                    else
                    {
                        userEntity.Create();
                    }
                    userEntity.F_Secretkey = Md5Helper.Encrypt(CommonHelper.CreateNo(), 16).ToLower();
                    userEntity.F_Password = Md5Helper.Encrypt(DESEncrypt.Encrypt(userEntity.F_Password, userEntity.F_Secretkey).ToLower(), 32).ToLower();
                    this.BaseRepository().Insert(userEntity);
                }
                else
                {
                    userEntity.Modify(keyValue);
                    userEntity.F_Secretkey = null;
                    userEntity.F_Password = null;
                    this.BaseRepository().Update(userEntity);
                }
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        public void SaveNewEntity(string keyValue, UserEntity userEntity)
        {
            try
            {
                if (!string.IsNullOrEmpty(userEntity.F_UserId))
                {
                    userEntity.Create_2();
                }
                else
                {
                    userEntity.Create();
                }
                userEntity.F_Secretkey = Md5Helper.Encrypt(CommonHelper.CreateNo(), 16).ToLower();
                userEntity.F_Password = Md5Helper.Encrypt(DESEncrypt.Encrypt(userEntity.F_Password, userEntity.F_Secretkey).ToLower(), 32).ToLower();
                this.BaseRepository().Insert(userEntity);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        /// <summary>
        /// 多部门保存表单数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public void SaveEntityList(string keyValue, string MDepartmentId)
        {
            try
            {
                if (keyValue != null)
                {
                    UserEntity entity = new UserEntity();
                    entity.F_MoreDepartmentId = MDepartmentId;
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        /// <summary>
        /// 合同主体显示设置
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="F_ItemValue"></param>
        public void ContractSubjectList(string keyValue, string F_ItemValue)
        {
            try
            {
                if (keyValue != null)
                {
                    UserEntity entity = new UserEntity();
                    // entity.F_ItemValue = F_ItemValue;
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        /// <summary>
        /// 修改用户登录密码
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="password">新密码（MD5 小写）</param>
        public void RevisePassword(string keyValue, string password)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity.Modify(keyValue);
                userEntity.F_Secretkey = Md5Helper.Encrypt(CommonHelper.CreateNo(), 16).ToLower();
                userEntity.F_Password = Md5Helper.Encrypt(DESEncrypt.Encrypt(password, userEntity.F_Secretkey).ToLower(), 32).ToLower();
                this.BaseRepository().Update(userEntity);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="state">状态：1-启动；0-禁用</param>
        public void UpdateState(string keyValue, int state)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity.Modify(keyValue);
                userEntity.F_EnabledMark = state;
                this.BaseRepository().Update(userEntity);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="userEntity">实体对象</param>
        public void UpdateEntity(UserEntity userEntity)
        {
            try
            {
                this.BaseRepository().Update(userEntity);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        public void P_SaveForm(string[] dataList, string strEntity)
        {
            var db = this.BaseRepository("learunOAWFForm");
            try
            {
                db.BeginTrans();
                UserEntity userEntity = strEntity.ToObject<UserEntity>();
                UserEntityVo userEntityVo = strEntity.ToObject<UserEntityVo>();
                userEntity.F_DepartmentId = userEntityVo.P_F_DepartmentId;
                UpdateEntity(userEntity);
                foreach (string item in dataList)
                {
                    var strSql = new StringBuilder();
                    strSql.Append("update " + item + " set ");
                    var queryParam = strEntity.ToJObject();
                    if (item.Equals("Project"))
                    {
                        strSql.Append(string.Format("Old_FollowPerson = '{0}'", queryParam["F_UserId"].ToString()));
                        strSql.Append(string.Format(", FollowPerson = '{0}'", queryParam["N_FollowPerson"].ToString()));
                        strSql.Append(string.Format(" where FollowPerson='{0}'", queryParam["F_UserId"].ToString()));
                    }
                    if (item.Equals("ProjectTask"))
                    {
                        strSql.Append(string.Format("ProjectResponsible = '{0}'", queryParam["N_FollowPerson"].ToString()));
                        strSql.Append(string.Format(" where ProjectResponsible='{0}'", queryParam["F_UserId"].ToString()));
                    }
                    if (item.Equals("ProjectRecruit"))
                    {
                        strSql.Append(string.Format("ApplyPerson = '{0}'", queryParam["N_FollowPerson"].ToString()));
                        strSql.Append(string.Format(" where ApplyPerson='{0}'", queryParam["F_UserId"].ToString()));
                    }
                    this.BaseRepository("learunOAWFForm").ExecuteBySql(strSql.ToString());
                }
                db.Commit();
            }
            catch (Exception ex)
            {
                db.Rollback();
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        /// <summary>
        /// 人员转接
        /// </summary>
        /// <param name="strEntity"></param>
        public void P_HandOver(string strEntity)
        {
            var db = this.BaseRepository("learunOAWFForm");
            try
            {
                db.BeginTrans();

                var queryParam = strEntity.ToJObject();
                // Project
                var strSql_1 = new StringBuilder();
                strSql_1.Append("update Project set ");
                strSql_1.Append(string.Format("Old_FollowPerson = '{0}'", queryParam["F_UserId"].ToString()));
                strSql_1.Append(string.Format(", FollowPerson = '{0}'", queryParam["N_FollowPerson"].ToString()));
                strSql_1.Append(string.Format(" where FollowPerson='{0}'", queryParam["F_UserId"].ToString()));
                this.BaseRepository("learunOAWFForm").ExecuteBySql(strSql_1.ToString());
                // ProjectTask
                var strSql_2 = new StringBuilder();
                strSql_2.Append("update ProjectTask set ");
                strSql_2.Append(string.Format("ProjectResponsible = '{0}'", queryParam["N_FollowPerson"].ToString()));
                strSql_2.Append(string.Format(" where ProjectResponsible='{0}'", queryParam["F_UserId"].ToString()));
                this.BaseRepository("learunOAWFForm").ExecuteBySql(strSql_2.ToString());
                // ProjectRecruit
                var strSql_3 = new StringBuilder();
                strSql_3.Append("update ProjectRecruit set ");
                strSql_3.Append(string.Format("ApplyPerson = '{0}'", queryParam["N_FollowPerson"].ToString()));
                strSql_3.Append(string.Format(" where ApplyPerson='{0}'", queryParam["F_UserId"].ToString()));
                this.BaseRepository("learunOAWFForm").ExecuteBySql(strSql_3.ToString());

                db.Commit();
            }
            catch (Exception ex)
            {
                db.Rollback();
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        internal IEnumerable<UserEntity> GetListUser(string departmentId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT *");
                strSql.Append(" FROM adms706.dbo.lr_base_user t WHERE t.F_EnabledMark = 1 AND t.F_DepartmentId = @departmentId  AND t.F_DeleteMark=0");
                return this.BaseRepository().FindList<UserEntity>(strSql.ToString(), new { departmentId = departmentId });
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        public UserEntity GetUserId(string userId)
        {
            try
            {

                var strSql = new StringBuilder();
                strSql.Append(" SELECT * ");
                strSql.Append(" FROM adms706.dbo.lr_base_user t ");
                strSql.Append("where (t.F_RealName='王振华' or t.F_RealName='余加坤' or t.F_RealName='朱卫华' or t.F_RealName ='温东英' or t.F_RealName='杨昆') and t.F_UserId = '" + userId + "'");
                return this.BaseRepository().FindList<UserEntity>(strSql.ToString()).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        /// <summary>
        /// 通过userId获取多部门信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserEntityVo DepartmentIdListById(string userId)
        {
            try
            {

                var strSql = new StringBuilder();
                strSql.Append(" SELECT * ");
                strSql.Append(" FROM adms706.dbo.lr_base_user t ");
                strSql.Append("where  t.F_UserId = '" + userId + "'");
                return this.BaseRepository().FindList<UserEntityVo>(strSql.ToString()).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        public List<UserEntity> GetProduceUserList()
        {
            var strSql = new StringBuilder();
            strSql.Append("SELECT ");
            //strSql.Append(@"u.* FROM LR_Base_User u INNER JOIN lr_base_department d  on d.F_DepartmentId=u.F_DepartmentId WHERE  isnull(d.F_Extrinsic_Flag,0)==1");
            strSql.Append(@"u.* ,isnull(d.F_Extrinsic_Flag,1) FROM LR_Base_User u INNER JOIN lr_base_department d  on d.F_DepartmentId=u.F_DepartmentId  WHERE d.F_FullName='结构勘测部' or d.F_FullName='房屋检测站' or d.F_FullName='华谨检测部' or d.F_FullName='设计事业部' or d.F_FullName='浦东基地' or d.F_FullName='通际公司' or d.F_FullName='西安公司' or d.F_FullName='武汉公司' or d.F_FullName='天津公司' or d.F_FullName='南京公司' or d.F_FullName='苏州公司' or d.F_FullName='杨浦公司'or d.F_FullName='创新发展部' or d.F_FullName='咨询部' or d.F_FullName='战略发展部'
");
            return this.BaseRepository().FindList<UserEntity>(strSql.ToString()).ToList();
        }

        #endregion
    }
}
