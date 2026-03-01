using Learun.Application.Base.SystemModule;
using Learun.Util;
using Nancy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Learun.Application.WebApi.Modules
{
    public class AnnexesApi : BaseApi
    {
        public AnnexesApi()
            : base("/learun/adms/annexes")
        {
            Get["/list"] = GetList;
            Get["/down"] = DownAnnexesFile;

            Post["/upload"] = Upload;
            Post["/delete"] = DeleteFile;
            Post["/sharefilelistSave"] = SharefilelistSave;

            Get["/getSharefilelistPageList"] = GetSharefilelistPageList;
            Get["/deleteSharefilelist"] = DeleteSharefilelist;
            Get["/getSharefile"] = GetSharefile;

        }
        private AnnexesFileIBLL annexesFileIBLL = new AnnexesFileBLL();
        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetList(dynamic _)
        {
            var keyValue = this.GetReqData();
            var list = annexesFileIBLL.GetList(keyValue);
            if (list.ToList().Count > 0)
            {
                string systemPath = Config.GetValue("AnnexesFile");
                foreach (var item in list)
                {
                    string f_url = item.F_FilePath;
                    if (f_url.Contains(systemPath))
                    {
                        f_url = item.F_FilePath.Replace(systemPath, item.F_Url);
                    }
                    else if (f_url.Contains("F:/fileAnnexes"))
                    {
                        f_url = item.F_FilePath.Replace("F:/fileAnnexes", item.F_Url);
                    }
                    item.F_Url = f_url;
                }
            }
            var jsonData = new
            {
                rows = list
            };
            return Success(jsonData);
        }

        /// <summary>
        /// 上传附件图片文件
        /// <summary>
        /// <returns></returns>
        public Response Upload(dynamic _)
        {
            //FileInfo DownloadFile = new FileInfo("D:/aidex/uploadPath/upload/2023/06/28/1ec6dea7-bb32-4a11-94cc-af9f947ead5b.xlsx");
            var files = (List<HttpFile>)this.Context.Request.Files;
            //var folderId = this.GetReqData();
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            string jsonText = string.Empty;

            HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空

            StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
            jsonText = sr.ReadToEnd();

            ReqUploadFile entity = jsonText.ToObject<ReqUploadFile>();
            FileInfo DownloadFile = new FileInfo(entity.path + "/" + entity.fileName);
            string filePath = Config.GetValue("AnnexesFile");
            string fileUrl = Config.GetValue("JCUrl");
            string uploadDate = DateTime.Now.ToString("yyyyMMdd");
            string FileEextension = DownloadFile.Extension.ToLower();
            string fileGuid = Guid.NewGuid().ToString();
            string folderId = entity.folderId;
            string virtualPath = string.Format("{0}/{1}/{2}/{3}{4}", filePath, userInfo.userId, uploadDate, fileGuid, FileEextension);

            //创建文件夹
            string path = Path.GetDirectoryName(virtualPath);
            Directory.CreateDirectory(path);

            File.Move(entity.path, virtualPath);
            AnnexesFileEntity fileAnnexesEntity = new AnnexesFileEntity();
            if (System.IO.File.Exists(virtualPath))
            {
                //byte[] bytes = new byte[files[0].Value.Length];
                //files[0].Value.Read(bytes, 0, bytes.Length);
                FileInfo file = new FileInfo(virtualPath);
                //FileStream fs = file.Create();
                //fs.Write(bytes, 0, bytes.Length);
                //fs.Close();

                //文件信息写入数据库
                fileAnnexesEntity.F_Id = fileGuid;
                fileAnnexesEntity.F_FileName = DownloadFile.Name;
                fileAnnexesEntity.F_FilePath = virtualPath;
                fileAnnexesEntity.F_FileSize = file.Length.ToString();
                fileAnnexesEntity.F_FileExtensions = FileEextension;
                fileAnnexesEntity.F_FileType = FileEextension.Replace(".", "");
                fileAnnexesEntity.F_CreateUserId = userInfo.userId;
                fileAnnexesEntity.F_CreateUserName = userInfo.realName;

                annexesFileIBLL.SaveEntity(folderId, fileAnnexesEntity);
            }

            return SuccessString(fileGuid);
        }
        public Response SharefilelistSave(dynamic _)
        {
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            string jsonText = string.Empty;

            HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空

            StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
            jsonText = sr.ReadToEnd();

            SharefilelistEntity entity = jsonText.ToObject<SharefilelistEntity>();
            annexesFileIBLL.SaveSharefilelist(entity);
            return Success("保存成功！");
        }
        public Response GetSharefilelistPageList(dynamic _)
        {
            var dataJson = this.GetReqData();
            string userId = LoginUserInfo.Get().userId;
            ReqPageParam parameter = JsonConvert.DeserializeObject<ReqPageParam>(dataJson);
            var list = annexesFileIBLL.GetSharefilelistPageList(parameter.pagination, parameter.queryJson);
            var jsonData = new
            {
                rows = list,
                userId = userId,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };
            return Success(jsonData);
        }
        public Response GetSharefile(dynamic _)
        {
            string keyValue = this.GetReqData();
            int Id = int.Parse(keyValue);
            var entity = annexesFileIBLL.GetSharefilelistEntity(Id);
            var jsonData = new
            {
                entity = entity,
            };
            return Success(jsonData);
        }
        public Response DeleteSharefilelist(dynamic _)
        {
            string keyValue = this.GetReqData();
            int id = int.Parse(keyValue);
            var entity = annexesFileIBLL.GetSharefilelistEntity(id);
            var filelist = annexesFileIBLL.GetList(entity.FolderId);
            if (filelist.ToList().Count > 0)
            {
                foreach(var fileInfoEntity in filelist)
                {
                    annexesFileIBLL.DeleteEntity(fileInfoEntity.F_Id);
                    //删除文件
                    if (System.IO.File.Exists(fileInfoEntity.F_FilePath))
                    {
                        System.IO.File.Delete(fileInfoEntity.F_FilePath);
                    }
                }
            }
            annexesFileIBLL.DeleteSharefilelist(id);
            return Success("删除成功");
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response DeleteFile(dynamic _)
        {
            var fileId = this.GetReqData();
            AnnexesFileEntity fileInfoEntity = annexesFileIBLL.GetEntity(fileId);
            annexesFileIBLL.DeleteEntity(fileId);
            //删除文件
            if (System.IO.File.Exists(fileInfoEntity.F_FilePath))
            {
                System.IO.File.Delete(fileInfoEntity.F_FilePath);
            }

            return Success("删除成功");
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response DownAnnexesFile(dynamic _)
        {
            string name = this.GetReqData();
            string fileId = name.Split('.')[0];
            var data = annexesFileIBLL.GetEntity(fileId);
            string filepath = data.F_FilePath;
            if (FileDownHelper.FileExists(filepath))
            {
                FileDownHelper.DownLoadnew(filepath);
            }
            return Success("");
        }
        /// <summary>
        /// 上传文件数据
        /// </summary>
        private class ReqUploadFile
        {
            public string folderId { get; set; }
            public string path { get; set; }
            public string fileName { get; set; }
        }
    }
}