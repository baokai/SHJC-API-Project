using Dapper;
using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Learun.Application.Base.SystemModule
{
    /// <summary>
    /// 日 期：2017.03.08
    /// 描 述：附件管理
    /// </summary>
    public class AnnexesFileService : RepositoryFactory
    {
        #region 属性 构造函数
        private string fieldSql;
        /// <summary>
        /// 
        /// </summary>
        public AnnexesFileService()
        {
            fieldSql = @" 
                   t.F_Id,
                   t.F_FolderId,
                   t.F_FileName,
                   t.F_FilePath,
                   t.F_FileSize,
                   t.F_FileExtensions,
                   t.F_FileType,
                   t.F_DownloadCount,
                   t.F_CreateDate,
                   t.F_CreateUserId,
                   t.F_CreateUserName,
t.F_Url
                    ";
        }
        #endregion


        #region 获取数据
        /// <summary>
        /// 获取实体列表
        /// </summary>
        /// <param name="folderId">主键值串</param>
        /// <returns></returns>
        public IEnumerable<AnnexesFileEntity> GetList(string folderId)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT " + fieldSql + " FROM LR_Base_AnnexesFile t WHERE t.F_FolderId = (@folderId) Order By t.F_CreateDate desc ");
                return this.BaseRepository().FindList<AnnexesFileEntity>(strSql.ToString(), new { folderId = folderId });
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
        /// 获取附件名称集合
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public string GetFileNames(string keyValue)
        {
            try
            {
                string res = "";
                IEnumerable<AnnexesFileEntity> list = GetList(keyValue);
                foreach (var item in list)
                {
                    if (!string.IsNullOrEmpty(res))
                    {
                        res += ",";
                    }
                    res += item.F_FileName;
                }
                return res;
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
        /// 获取附件实体
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public AnnexesFileEntity GetEntity(string keyValue)
        {
            try
            {
                var fileEntity = this.BaseRepository().FindEntity<AnnexesFileEntity>(keyValue);
                if (fileEntity == null)
                {
                    IEnumerable<AnnexesFileEntity> fileList = GetList(keyValue);
                    foreach (var item in fileList)
                    {
                        return item;
                    }
                }
                return fileEntity;
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
        /// 获取Sharefilelist 实体
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public SharefilelistEntity GetSharefilelistEntity(int keyValue)
        {
            try
            {
                var fileEntity = this.BaseRepository().FindEntity<SharefilelistEntity>(keyValue);
                return fileEntity;
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
        /// 获取Sharefilelist数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<SharefilelistVo> GetSharefilelistPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(@" SELECT
                     lbs.*,lbu.F_DepartmentId AS DepartmentId,lbd.F_FullName as DepartmentName
                ");
                strSql.Append("  FROM	lr_base_sharefilelist lbs ");
                strSql.Append("  JOIN lr_base_user lbu ON lbs.CreateUserId = lbu.F_UserId ");
                strSql.Append("  join lr_base_department lbd on lbu.F_DepartmentId = lbd.F_DepartmentId ");
                strSql.Append("  WHERE 1=1   ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["CreateTime"].IsEmpty())
                {
                    var create_time = queryParam["CreateTime"].ToObject<List<string>>();
                    if (create_time.Count > 0)
                    {
                        string create_time_start_date = create_time[0];
                        string create_time_end_date = create_time[1].ToDate().AddDays(1).ToString();
                        string createTime = " AND ( lbs.F_CreateDate >= '" + create_time_start_date + "' AND lbs.F_CreateDate < '" + create_time_end_date + "' )";
                        strSql.Append(createTime);
                    }
                }
                if (!queryParam["FileName"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( lbs.F_FileName like '%{0}%' )", queryParam["FileName"].ToString()));
                }
                if (!queryParam["F_DepartmentId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( lbu.F_DepartmentId ='{0}')", queryParam["F_DepartmentId"].ToString()));
                }
                if (!queryParam["UserId"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( lbs.F_CreateUserId ='{0}' )", queryParam["UserId"].ToString()));
                }
                return this.BaseRepository().FindList_NodbWhere<SharefilelistVo>(strSql.ToString(), pagination);
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
        /// 保存数据实体
        /// </summary>
        /// <param name="folderId">附件夹主键</param>
        /// <param name="annexesFileEntity">附件实体数据</param>
        public void SaveEntity(string folderId, AnnexesFileEntity annexesFileEntity)
        {
            try
            {
                annexesFileEntity.Create();
                annexesFileEntity.F_FolderId = folderId;
                //annexesFileEntity.F_Url = "http://114.215.185.6:83/filePath/"+ annexesFileEntity.F_FilePath.Replace("C:/fileAnnexes", "");
                string fileUrl = Config.GetValue("JCUrl");
                  annexesFileEntity.F_Url = fileUrl;
                this.BaseRepository().Insert(annexesFileEntity);
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
        /// 保存Sharefilelist
        /// </summary>
        /// <param name="entity">附件实体数据</param>
        public void SaveSharefilelist(SharefilelistEntity entity)
        {
            try
            {
                if(entity.Id > 0)
                {
                    this.BaseRepository().Update(entity);
                }
                else
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
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
        /// 删除附件
        /// </summary>
        /// <param name="fileId">文件主键</param>
        public void DeleteEntity(string fileId)
        {
            try
            {
                this.BaseRepository().Delete(new AnnexesFileEntity() { F_Id = fileId });
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
        /// 删除Sharefilelist
        /// </summary>
        /// <param name="Id">文件主键</param>
        public void DeleteSharefilelist(int Id)
        {
            try
            {
                this.BaseRepository().Delete(new SharefilelistEntity() { Id = Id });
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
