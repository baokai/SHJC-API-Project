using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learun.Application.Organization
{
    /// <summary>
    /// 日 期：2017.04.17
    /// 描 述：部门管理
    /// </summary>
    public class DepartmentService : RepositoryFactory
    {
        #region 构造函数和属性
        private string fieldSql;
        public DepartmentService()
        {
            fieldSql = @"
                            t.F_DepartmentId,
                            t.F_CompanyId,
                            t.F_ParentId,
                            t.F_EnCode,
                            t.F_FullName,
                            t.F_ShortName,
                            t.F_Nature,
                            t.F_Manager,
                            t.F_OuterPhone,
                            t.F_InnerPhone,
                            t.F_Email,
                            t.F_Fax,
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
        t.HZ_DepartmentId
                            ";
        }
      
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取部门列表信息(根据公司Id)
        /// </summary>
        /// <param name="companyId">公司主键</param>
        /// <returns></returns>
        public IEnumerable<DepartmentEntity> GetList(string companyId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM LR_Base_Department t WHERE t.F_EnabledMark = 1 AND t.F_DeleteMark = 0 AND F_CompanyId = @companyId ORDER BY t.F_ParentId,t.F_FullName ");
                return this.BaseRepository().FindList<DepartmentEntity>(strSql.ToString(), new { companyId = companyId });
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
        /// 获取部门列表信息(根据公司Id)(不包括合作伙伴)
        /// </summary>
        /// <param name="companyId">公司主键</param>
        /// <returns></returns>
        public IEnumerable<DepartmentEntity> GetListHZ(string companyId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM LR_Base_Department t WHERE t.F_EnabledMark = 1 AND t.F_DeleteMark = 0 AND F_CompanyId = @companyId and t.HZ_DepartmentId<>1 ORDER BY t.F_ParentId,t.F_FullName ");
                
                return this.BaseRepository().FindList<DepartmentEntity>(strSql.ToString(), new { companyId = companyId });
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
        /// 获取部门列表信息(根据公司Id)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DepartmentEntity> GetAllList()
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM LR_Base_Department t WHERE t.F_EnabledMark = 1 AND t.F_DeleteMark = 0 ");
                return this.BaseRepository().FindList<DepartmentEntity>(strSql.ToString());
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
        /// 获取部门数据实体
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public DepartmentEntity GetEntity(string keyValue) {
            try
            {
                return this.BaseRepository().FindEntity<DepartmentEntity>(keyValue);
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
        /// 获取部门列表信息-资金台账
        /// </summary>
        /// <param name="keyWord">查询关键字</param>
        /// <returns></returns>
        public IEnumerable<DepartmentEntity> GetList_zijin(string keyWord)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("select tt.* from lr_base_department_add t inner join lr_base_department tt on t.F_DepartmentId = tt.F_DepartmentId");
                if (!string.IsNullOrEmpty(keyWord))
                {
                    strSql.Append(" where tt.F_FullName like '" + keyWord + "' or tt.F_EnCode like '" + keyWord + "' or tt.F_ShortName like '" + keyWord + "' ");
                }
                return this.BaseRepository().FindList<DepartmentEntity>(strSql.ToString());
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }
        /// <summary>
        /// 获取部门数据实体
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public List<DepartmentEntity> GetEntityList() {
            try
            {
                return this.BaseRepository().FindList<DepartmentEntity>().ToList();
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
        /// 获取部门数据实体
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public IEnumerable<DepartmentEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(" t.* from  LR_Base_Department t where t.F_EnabledMark = 1 AND t.F_DeleteMark = 0 ");
                var queryParam = queryJson.ToJObject();           
                return this.BaseRepository().FindList<DepartmentEntity>(strSql.ToString(), pagination);
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
        /// 获取部门数据实体不包括合作伙伴
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public IEnumerable<DepartmentEntity> GetPageListHZ()
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(" t.* from  LR_Base_Department t where t.F_EnabledMark = 1 AND t.F_DeleteMark = 0 AND t.HZ_DepartmentId<>1");
                return this.BaseRepository().FindList<DepartmentEntity>(strSql.ToString());
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
        public IEnumerable<DepartmentEntity> GetPageList2()
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(" t.* from  LR_Base_Department t where t.F_EnabledMark = 1 AND t.F_DeleteMark = 0 ");
                //var queryParam = queryJson.ToJObject();
                return this.BaseRepository().FindList<DepartmentEntity>(strSql.ToString());
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
        public IEnumerable<DepartmentEntity> GetPageListHZ2()
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(" t.* from  LR_Base_Department t where t.F_EnabledMark = 1  AND t.F_DeleteMark = 0  AND (t.HZ_DepartmentId is null or t.HZ_DepartmentId<>1)");
                //var queryParam = queryJson.ToJObject();
                return this.BaseRepository().FindList<DepartmentEntity>(strSql.ToString());
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
        /// 获取部门数据实体
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public DepartmentEntity GetEntityHZ(string keyValue) {
            try
            {
                return this.BaseRepository().FindEntity<DepartmentEntity>(t=>t.F_DepartmentId== keyValue&&t.HZ_DepartmentId!=1);
              
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
        /// 根据id获取部门
        /// </summary>
        /// <param name="depId"></param>
        /// <returns></returns>
            public DepartmentEntity GetDepartmentId(string depId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(@"SELECT F_FullName,F_DepartmentId");
                strSql.Append(" FROM LR_Base_Department where F_DepartmentId='"+depId+"'");
                return this.BaseRepository().FindList<DepartmentEntity>(strSql.ToString()).FirstOrDefault();
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
        /// 多部门
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public List<DepartmentEntity> GetUserDepartmentId(string depId)
        {
            try
            {
                return this.BaseRepository("adms706").FindList<DepartmentEntity>(t => t.F_DepartmentId == depId).ToList();
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
        /// 虚拟删除部门
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void VirtualDelete(string keyValue)
        {
            try
            {
                DepartmentEntity entity = new DepartmentEntity()
                {
                    F_DepartmentId = keyValue,
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
        /// 保存部门表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="departmentEntity">部门实体</param>
        /// <returns></returns>
        public void SaveEntity(string keyValue, DepartmentEntity departmentEntity)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    departmentEntity.Modify(keyValue);
                    this.BaseRepository().Update(departmentEntity);
                }
                else
                {
                    if (!string.IsNullOrEmpty(departmentEntity.F_DepartmentId))
                    {
                        departmentEntity.Create_2();
                    }
                    else
                    {
                        departmentEntity.Create();
                    }
                    
                    this.BaseRepository().Insert(departmentEntity);
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
        #endregion
    }
}
