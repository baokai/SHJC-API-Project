using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learun.Application.Base.AuthorizeModule
{
    /// <summary>
    /// 日 期：2017.04.17
    /// 描 述：用户关联对象
    /// </summary>
    public class UserRelationService : RepositoryFactory
    {
        #region 构造函数和属性
        private string fieldSql;
        /// <summary>
        /// 
        /// </summary>
        public UserRelationService()
        {
            fieldSql = @"
                t.F_UserRelationId,
                t.F_UserId,
                t.F_Category,
                t.F_ObjectId,
                t.F_CreateDate,
                t.F_CreateUserId,
                t.F_CreateUserName
            ";
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取对象主键列表信息
        /// </summary>
        /// <param name="userId">用户主键</param>
        /// <param name="category">分类:1-角色2-岗位</param>
        /// <returns></returns>
        public IEnumerable<UserRelationEntity> GetObjectIdList(string userId, int category)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(" SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM LR_Base_UserRelation t WHERE t.F_UserId = @userId AND t.F_Category =  @category ");
                return this.BaseRepository().FindList<UserRelationEntity>(strSql.ToString(), new { userId = userId,category=category });
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
        /// 获取用户角色
        /// </summary>
        /// <param name="userId">用户主键</param>
        /// 分类:1-角色2-岗位</param>
        /// <returns></returns>
        public IEnumerable<UserRelationEntity> GetUserRoleList(string userId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(" SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM LR_Base_UserRelation t WHERE  t.F_Category =  1 and t.F_UserId = '"+ userId +"'");
                return this.BaseRepository().FindList_NodbWhere<UserRelationEntity>(strSql.ToString());
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
        /// 获取用户主键列表信息
        /// </summary>
        /// <param name="objectId">关联角色或岗位组件</param>
        /// <returns></returns>
        public IEnumerable<UserRelationEntity> GetUserIdList(string objectId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(" SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM LR_Base_UserRelation t WHERE t.F_ObjectId = @objectId");
                return this.BaseRepository().FindList<UserRelationEntity>(strSql.ToString(), new { objectId = objectId });
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
        /// 获取用户主键列表信息
        /// </summary>
        /// <param name="objectIdList">关联或角色主键集合</param>
        /// <returns></returns>
        public IEnumerable<UserRelationEntity> GetUserIdList(List<string>objectIdList)
        {
            try
            {
                return this.BaseRepository().FindList<UserRelationEntity>(t => objectIdList.Contains(t.F_ObjectId));
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
        /// 通过用户id查询营销系统部门负责人
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserRelationEntity GetUserId(string userId)
        {
            try
            {

                var strSql = new StringBuilder();
                strSql.Append(" SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM LR_Base_UserRelation t WHERE t.F_ObjectId = '52513c34-bad1-4030-87b2-fa1200a3496d' and t.F_UserId='"+userId+"'");
                return this.BaseRepository().FindList<UserRelationEntity>(strSql.ToString()).FirstOrDefault();
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
        /// 保存用户对应对象数据
        /// </summary>
        /// <param name="objectId">对应ID</param>
        /// <param name="userRelationEntityList">列表</param>
        public void SaveEntityList(string objectId, IEnumerable<UserRelationEntity> userRelationEntityList)
        {
            var db = this.BaseRepository().BeginTrans();
            try
            {
                foreach (UserRelationEntity userRelationEntity in userRelationEntityList)
                {
                    db.Insert(userRelationEntity);
                    db.Delete<UserRelationEntity>(t => t.F_ObjectId.Equals(objectId) && t.F_UserId.Equals(userRelationEntity.F_UserId));
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
        #endregion
    }
}
