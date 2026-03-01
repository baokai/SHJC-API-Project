using Learun.Application.Base.SystemModule;
using Learun.Application.Organization;
using Learun.Application.TwoDevelopment.LR_CodeDemo;
using Learun.Application.TwoDevelopment.LR_CodeDemo.ReportForms;
using Learun.Cache.Base;
using Learun.Cache.Factory;
using Learun.Util;
using Learun.Util.Ueditor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;


namespace Learun.Application.Web.Controllers
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 上海力软信息技术有限公司
    /// 创建人：力软-框架开发组
    /// 日 期：2017.03.07
    /// 描 述：通用控制器,处理通用的接口
    /// </summary>
    [HandlerLogin(FilterMode.Ignore)]
    public class UtilityController : MvcControllerBase
    {
        private ProjectManageIBLL projectManageIBLL = new ProjectManageBLL();
        private ReportFormsIBLL reportFormsBLL = new ReportFormsBLL();
        private UserIBLL userIBLL = new UserBLL();
        private ICache cache = CacheFactory.CaChe();
        private ProjectTaskIBLL projectTaskIBLL = new ProjectTaskBLL();
        #region 选择图标
        /// <summary>
        /// 图标的选择
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerLogin(FilterMode.Enforce)]
        public ActionResult Icon()
        {
            return View();
        }
        /// <summary>
        /// 移动图标的选择
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerLogin(FilterMode.Enforce)]
        public ActionResult AppIcon()
        {
            return View();
        }
        #endregion

        private DataItemIBLL dataItemBLL = new DataItemBLL();
        private DepartmentIBLL departmentIBLL = new DepartmentBLL();

        #region 百度编辑器的后端接口
        /// <summary>
        /// 百度编辑器的后端接口
        /// </summary>
        /// <param name="action">执行动作</param>
        /// <returns></returns>
        public ActionResult UEditor()
        {
            string action = Request["action"];

            switch (action)
            {
                case "config":
                    return Content(UeditorConfig.Items.ToJson());
                case "uploadimage":
                    return UEditorUpload(new UeditorUploadConfig()
                    {
                        AllowExtensions = UeditorConfig.GetStringList("imageAllowFiles"),
                        PathFormat = UeditorConfig.GetString("imagePathFormat"),
                        SizeLimit = UeditorConfig.GetInt("imageMaxSize"),
                        UploadFieldName = UeditorConfig.GetString("imageFieldName")
                    });
                case "uploadscrawl":
                    return UEditorUpload(new UeditorUploadConfig()
                    {
                        AllowExtensions = new string[] { ".png" },
                        PathFormat = UeditorConfig.GetString("scrawlPathFormat"),
                        SizeLimit = UeditorConfig.GetInt("scrawlMaxSize"),
                        UploadFieldName = UeditorConfig.GetString("scrawlFieldName"),
                        Base64 = true,
                        Base64Filename = "scrawl.png"
                    });
                case "uploadvideo":
                    return UEditorUpload(new UeditorUploadConfig()
                    {
                        AllowExtensions = UeditorConfig.GetStringList("videoAllowFiles"),
                        PathFormat = UeditorConfig.GetString("videoPathFormat"),
                        SizeLimit = UeditorConfig.GetInt("videoMaxSize"),
                        UploadFieldName = UeditorConfig.GetString("videoFieldName")
                    });
                case "uploadfile":
                    return UEditorUpload(new UeditorUploadConfig()
                    {
                        AllowExtensions = UeditorConfig.GetStringList("fileAllowFiles"),
                        PathFormat = UeditorConfig.GetString("filePathFormat"),
                        SizeLimit = UeditorConfig.GetInt("fileMaxSize"),
                        UploadFieldName = UeditorConfig.GetString("fileFieldName")
                    });
                case "listimage":
                    return ListFileManager(UeditorConfig.GetString("imageManagerListPath"), UeditorConfig.GetStringList("imageManagerAllowFiles"));
                case "listfile":
                    return ListFileManager(UeditorConfig.GetString("fileManagerListPath"), UeditorConfig.GetStringList("fileManagerAllowFiles"));
                case "catchimage":
                    return CrawlerHandler();
                default:
                    return Content(new
                    {
                        state = "action 参数为空或者 action 不被支持。"
                    }.ToJson());
            }
        }
        /// <summary>
        /// 百度编辑器的文件上传
        /// </summary>
        /// <param name="uploadConfig">上传配置信息</param>
        /// <returns></returns>
        public ActionResult UEditorUpload(UeditorUploadConfig uploadConfig)
        {
            UeditorUploadResult result = new UeditorUploadResult() { State = UeditorUploadState.Unknown };

            byte[] uploadFileBytes = null;
            string uploadFileName = null;

            if (uploadConfig.Base64)
            {
                uploadFileName = uploadConfig.Base64Filename;
                uploadFileBytes = Convert.FromBase64String(Request[uploadConfig.UploadFieldName]);
            }
            else
            {
                var file = Request.Files[uploadConfig.UploadFieldName];
                uploadFileName = file.FileName;

                if (!CheckFileType(uploadConfig, uploadFileName))
                {
                    return Content(new
                    {
                        state = GetStateMessage(UeditorUploadState.TypeNotAllow)
                    }.ToJson());
                }
                if (!CheckFileSize(uploadConfig, file.ContentLength))
                {
                    return Content(new
                    {
                        state = GetStateMessage(UeditorUploadState.SizeLimitExceed)
                    }.ToJson());
                }

                uploadFileBytes = new byte[file.ContentLength];
                try
                {
                    file.InputStream.Read(uploadFileBytes, 0, file.ContentLength);
                }
                catch (Exception)
                {
                    return Content(new
                    {
                        state = GetStateMessage(UeditorUploadState.NetworkError)
                    }.ToJson());
                }
            }

            result.OriginFileName = uploadFileName;

            var savePath = UeditorPathFormatter.Format(uploadFileName, uploadConfig.PathFormat);
            var localPath = Server.MapPath(savePath).Replace("\\Utility\\", "\\ueditor\\");// +"/ueditor/net";
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(localPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(localPath));
                }
                System.IO.File.WriteAllBytes(localPath, uploadFileBytes);
                result.Url = savePath;
                result.State = UeditorUploadState.Success;
            }
            catch (Exception e)
            {
                result.State = UeditorUploadState.FileAccessError;
                result.ErrorMessage = e.Message;
            }

            return Content(new
            {
                state = GetStateMessage(result.State),
                url = result.Url,
                title = result.OriginFileName,
                original = result.OriginFileName,
                error = result.ErrorMessage
            }.ToJson());
        }
        /// <summary>
        /// 百度编辑器的文件列表管理
        /// </summary>
        /// <param name="pathToList">文件列表目录</param>
        /// <param name="searchExtensions">扩展名</param>
        /// <returns></returns>
        public ActionResult ListFileManager(string pathToList, string[] searchExtensions)
        {
            int Start;
            int Size;
            int Total;
            String[] FileList;
            String[] SearchExtensions;
            SearchExtensions = searchExtensions.Select(x => x.ToLower()).ToArray();
            try
            {
                Start = String.IsNullOrEmpty(Request["start"]) ? 0 : Convert.ToInt32(Request["start"]);
                Size = String.IsNullOrEmpty(Request["size"]) ? UeditorConfig.GetInt("imageManagerListSize") : Convert.ToInt32(Request["size"]);
            }
            catch (FormatException)
            {
                return Content(new
                {
                    state = "参数不正确",
                    start = 0,
                    size = 0,
                    total = 0
                }.ToJson());
            }
            var buildingList = new List<String>();
            try
            {
                var localPath = Server.MapPath(pathToList).Replace("\\Utility\\", "\\ueditor\\");
                buildingList.AddRange(Directory.GetFiles(localPath, "*", SearchOption.AllDirectories)
                    .Where(x => SearchExtensions.Contains(Path.GetExtension(x).ToLower()))
                    .Select(x => pathToList + x.Substring(localPath.Length).Replace("\\", "/")));
                Total = buildingList.Count;
                FileList = buildingList.OrderBy(x => x).Skip(Start).Take(Size).ToArray();
            }
            catch (UnauthorizedAccessException)
            {
                return Content(new
                {
                    state = "文件系统权限不足",
                    start = 0,
                    size = 0,
                    total = 0
                }.ToJson());
            }
            catch (DirectoryNotFoundException)
            {
                return Content(new
                {
                    state = "路径不存在",
                    start = 0,
                    size = 0,
                    total = 0
                }.ToJson());
            }
            catch (IOException)
            {
                return Content(new
                {
                    state = "文件系统读取错误",
                    start = 0,
                    size = 0,
                    total = 0
                }.ToJson());
            }

            return Content(new
            {
                state = "SUCCESS",
                list = FileList == null ? null : FileList.Select(x => new { url = x }),
                start = Start,
                size = Size,
                total = Total
            }.ToJson());
        }

        public ActionResult CrawlerHandler()
        {
            string[] sources = Request.Form.GetValues("source[]");
            if (sources == null || sources.Length == 0)
            {
                return Content(new
                {
                    state = "参数错误：没有指定抓取源"
                }.ToJson());
            }

            UeditorCrawler[] crawlers = sources.Select(x => new UeditorCrawler(x).Fetch()).ToArray();
            return Content(new
            {
                state = "SUCCESS",
                list = crawlers.Select(x => new
                {
                    state = x.State,
                    source = x.SourceUrl,
                    url = x.ServerUrl
                })
            }.ToJson());
        }
        private string GetStateMessage(UeditorUploadState state)
        {
            switch (state)
            {
                case UeditorUploadState.Success:
                    return "SUCCESS";
                case UeditorUploadState.FileAccessError:
                    return "文件访问出错，请检查写入权限";
                case UeditorUploadState.SizeLimitExceed:
                    return "文件大小超出服务器限制";
                case UeditorUploadState.TypeNotAllow:
                    return "不允许的文件格式";
                case UeditorUploadState.NetworkError:
                    return "网络错误";
            }
            return "未知错误";
        }
        /// <summary>
        /// 检测是否符合上传文件格式
        /// </summary>
        /// <param name="uploadConfig">配置信息</param>
        /// <param name="filename">文件名字</param>
        /// <returns></returns>
        private bool CheckFileType(UeditorUploadConfig uploadConfig, string filename)
        {
            var fileExtension = Path.GetExtension(filename).ToLower();
            var res = false;
            foreach (var item in uploadConfig.AllowExtensions)
            {
                if (item == fileExtension)
                {
                    res = true;
                    break;
                }
            }
            return res;
        }
        /// <summary>
        /// 检测是否符合上传文件大小
        /// </summary>
        /// <param name="uploadConfig">配置信息</param>
        /// <param name="size">文件大小</param>
        /// <returns></returns>
        private bool CheckFileSize(UeditorUploadConfig uploadConfig, int size)
        {
            return size < uploadConfig.SizeLimit;
        }


        #endregion

        #region 导出Excel
        /// <summary>
        /// 请选择要导出的字段页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerLogin(FilterMode.Enforce)]
        public ActionResult ExcelExportForm()
        {
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public void ExportExcel(string fileName, string columnJson, string dataJson, string exportField)
        {
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = Server.UrlDecode(fileName);
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 15;
            excelconfig.FileName = Server.UrlDecode(fileName) + ".xls";
            excelconfig.IsAllSizeColumn = true;
            excelconfig.ColumnEntity = new List<ColumnModel>();
            //表头
            List<jfGridModel> columnList = columnJson.ToList<jfGridModel>();
            //行数据
            DataTable rowData = dataJson.ToTable();
            //写入Excel表头
            Dictionary<string, string> exportFieldMap = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(exportField))
            {
                string[] exportFields = exportField.Split(',');
                foreach (var field in exportFields)
                {
                    if (!exportFieldMap.ContainsKey(field))
                    {
                        exportFieldMap.Add(field, "1");
                    }
                }
            }

            foreach (jfGridModel columnModel in columnList)
            {
                if (exportFieldMap.ContainsKey(columnModel.name) || string.IsNullOrEmpty(exportField))
                {
                    excelconfig.ColumnEntity.Add(new ColumnModel()
                    {
                        Column = columnModel.name,
                        ExcelColumn = columnModel.label,
                        Alignment = columnModel.align,
                        Width = columnModel.name.Length
                    });
                }
            }
            ExcelHelper.ExcelDownload(rowData, excelconfig);
        }

        [HttpPost, ValidateInput(false)]
        public void ExportExcelContract(string fileName, string columnJson, string dataJson, string exportField)
        {
            int indexs = 1;
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = Server.UrlDecode(fileName);
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 15;
            excelconfig.FileName = Server.UrlDecode(fileName) + ".xls";
            excelconfig.IsAllSizeColumn = true;
            excelconfig.ColumnEntity = new List<ColumnModel>();
            string json = cache.Read<string>(dataJson);
            //表头
            List<jfGridModel> columnList = columnJson.ToList<jfGridModel>();
            columnList.Add(new jfGridModel { name = "index", label = "序号", align = "center" });

            //行数据
            DataTable rowData = json.ToTable();
            List<ProjectContractVo> data = TableToEntity<ProjectContractVo>(rowData);
            List<ProjectContractVo> marketings = new List<ProjectContractVo>();
            foreach (var item in data)
            {
                //if (!marketings.Contains(item))
                //{
                //    marketings.Add(item);
                //}
                marketings.Add(item);
            }
            foreach (var item in marketings)
            {
                item.index = indexs++;
                if (item.MainContract == "1")
                {
                    item.MainContractName = "是";
                }
                ////核算1
                ////自主营销
                //if (item.ProjectSource == "1")
                //{
                //    if (item.PaymentAmount == null)
                //    {

                //        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.02;

                //        item.FollowPersonAmount=Math.Round((decimal)item.FollowPersonAmount * 100) / 100;

                //    }
                //    else if (item.PaymentAmount < (item.ContractAmount * (decimal?)0.3))
                //    {

                //        item.FollowPersonAmount = (item.ContractAmount * (decimal?)0.005);
                //        item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;
                //    }
                //    else
                //    {

                //        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.02;
                //        item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;

                //    }
                //}
                ////渠道营销
                //if (item.ProjectSource == "2")
                //{
                //    if (item.PaymentAmount == null)
                //    {
                //        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.015;
                //        item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;
                //    }
                //    else if (item.PaymentAmount < (item.ContractAmount * (decimal?)0.3))
                //    {
                //        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.002;
                //        item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;

                //    }
                //    else
                //    {
                //        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.001;
                //        item.FollowPersonAmount=Math.Round((decimal)item.FollowPersonAmount * 100) / 100;
                //    }
                //}

                DataItemDetailEntity contractSubject = dataItemBLL.GetDetailItemName(item.ContractSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    item.ContractSubject = contractSubject.F_ItemName;
                }

                DataItemDetailEntity contractReceiptType = dataItemBLL.GetDetailItemName(item.ReceivedFlag, "ReceiptType");
                if (contractReceiptType != null)
                {
                    item.ReceivedFlag = contractReceiptType.F_ItemName;
                }

                DataItemDetailEntity contractType = dataItemBLL.GetDetailItemName(item.ContractType, "ContractType");
                if (contractType != null)
                {
                    item.ContractType = contractType.F_ItemName;
                }

                DataItemDetailEntity contractStatus = dataItemBLL.GetDetailItemName(item.ContractStatus, "ContractStatus");
                if (contractStatus != null)
                {
                    item.ContractStatus = contractStatus.F_ItemName;
                }

                UserEntity followPerson = userIBLL.GetFollowPersonNameByUserId(item.FollowPerson);
                if (followPerson != null)
                {
                    item.FollowPerson = followPerson.F_RealName;
                }
                else
                {
                    item.FollowPerson = null;
                }

                DataItemDetailEntity projectSource = dataItemBLL.GetDetailItemName(item.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    item.ProjectSource = projectSource.F_ItemName;
                }
                if (item.DepartmentId != null && item.DepartmentId.IndexOf(",") == -1)
                {
                    var department = departmentIBLL.GetEntity(item.DepartmentId);
                    if (department != null)
                    {
                        item.DepartmentId = department.F_FullName;
                    }
                    else
                    {
                        item.DepartmentId = null;
                    }
                }
                else
                {
                    if (item.DepartmentId != null)
                    {
                        string[] array = item.DepartmentId.Split(new char[] { ',' });
                        foreach (var dept in array)
                        {
                            var department = departmentIBLL.GetEntity(dept);
                            if (department != null)
                            {
                                item.DepartmentId = department.F_FullName;
                            }
                            else
                            {
                                item.DepartmentId = null;
                            }
                        }
                    }
                    else
                    {
                        item.DepartmentId = null;
                    }

                }

            }
            rowData = marketings.ToJson().ToTable();
            //写入Excel表头
            Dictionary<string, string> exportFieldMap = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(exportField))
            {
                string[] exportFields = exportField.Split(',');
                foreach (var field in exportFields)
                {
                    if (!exportFieldMap.ContainsKey(field))
                    {
                        exportFieldMap.Add(field, "1");
                    }
                }
            }

            foreach (jfGridModel columnModel in columnList)
            {
                if (exportFieldMap.ContainsKey(columnModel.name) || string.IsNullOrEmpty(exportField))
                {
                    excelconfig.ColumnEntity.Add(new ColumnModel()
                    {
                        Column = columnModel.name,
                        ExcelColumn = columnModel.label,
                        Alignment = columnModel.align,
                        Width = columnModel.name.Length
                    });
                }
            }
            ExcelHelper.ExcelDownload(rowData, excelconfig);
        }
        [HttpPost, ValidateInput(false)]
        public void ExportExcelTask(string fileName, string columnJson, string dataJson, string exportField)
        {
            int indexs = 1;
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = Server.UrlDecode(fileName);
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 15;
            excelconfig.FileName = Server.UrlDecode(fileName) + ".xls";
            excelconfig.IsAllSizeColumn = true;
            excelconfig.ColumnEntity = new List<ColumnModel>();
            string json = cache.Read<string>(dataJson);
            //表头
            List<jfGridModel> columnList = columnJson.ToList<jfGridModel>();
            columnList.Add(new jfGridModel { name = "index", label = "序号", align = "center" });

            //行数据
            DataTable rowData = json.ToTable();
            List<ProjectTaskVo> data = TableToEntity<ProjectTaskVo>(rowData);
            List<ProjectTaskVo> marketings = new List<ProjectTaskVo>();
            foreach (var item in data)
            {
                //if (!marketings.Contains(item))
                //{
                //    marketings.Add(item);
                //}
                marketings.Add(item);
            }
            foreach (var item in marketings)
            {
                item.index = indexs++;

                if (item.ProjectResponsible != null)
                {
                    string[] strList = item.ProjectResponsible.Split(',');

                    string projectResponsible = "";
                    for (var i = 0; i < strList.Length; i++)
                    {
                        var Responsible = userIBLL.GetFollowPersonNameByUserId(strList[i]);
                        if (Responsible != null)
                        {
                            if (string.IsNullOrWhiteSpace(projectResponsible))
                            {
                                projectResponsible = Responsible.F_RealName;
                            }
                            else
                            {
                                projectResponsible = projectResponsible + "," + Responsible.F_RealName;
                            }
                        }
                    }
                    item.ProjectResponsible = projectResponsible;
                }

                //营销部门
                if (item.DepartmentId != null)
                {
                    string[] strList = item.DepartmentId.Split(',');
                    string departmentId = "";
                    for (var i = 0; i < strList.Length; i++)
                    {
                        var department = departmentIBLL.GetDepartmentId(strList[i]);
                        if (department != null)
                        {
                            if (string.IsNullOrWhiteSpace(departmentId))
                            {
                                departmentId = department.F_FullName;
                            }
                            else
                            {
                                departmentId = departmentId + "," + department.F_FullName;
                            }
                        }
                    }
                    item.DepartmentId = departmentId;
                }

                //预警
                if (item.TaskStatus.ToInt() == 5)
                {
                    item.YJ = "已完成";
                }

                else
                if (item.YJ.ToInt() == 999)
                {

                    item.YJ = "超时";
                }
                else

                if (item.TaskStatus.ToInt() != 5 && item.YJ.ToInt() != 999 && item.YJ.ToInt() != 111)
                {

                    item.YJ = "剩余时间:" + item.YJ.ToInt();
                }
                else
                if (item.YJ.ToInt() == 111)
                {

                    item.YJ = "严重超时";
                }

                //状态
                DataItemDetailEntity taskStatus = dataItemBLL.GetDetailItemName(item.TaskStatus, "TaskStatus");
                if (taskStatus != null)
                {
                    item.TaskStatus = taskStatus.F_ItemName;
                }
            }
            rowData = marketings.ToJson().ToTable();
            //写入Excel表头
            Dictionary<string, string> exportFieldMap = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(exportField))
            {
                string[] exportFields = exportField.Split(',');
                foreach (var field in exportFields)
                {
                    if (!exportFieldMap.ContainsKey(field))
                    {
                        exportFieldMap.Add(field, "1");
                    }
                }
            }

            foreach (jfGridModel columnModel in columnList)
            {
                if (exportFieldMap.ContainsKey(columnModel.name) || string.IsNullOrEmpty(exportField))
                {
                    excelconfig.ColumnEntity.Add(new ColumnModel()
                    {
                        Column = columnModel.name,
                        ExcelColumn = columnModel.label,
                        Alignment = columnModel.align,
                        Width = columnModel.name.Length
                    });
                }
            }
            ExcelHelper.ExcelDownload(rowData, excelconfig);
        }


        [HttpPost, ValidateInput(false)]
        public void ExportExcelPayment(string fileName, string columnJson, string dataJson, string exportField)
        {
            int indexs = 1;
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = Server.UrlDecode(fileName);
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 15;
            excelconfig.FileName = Server.UrlDecode(fileName) + ".xls";
            excelconfig.IsAllSizeColumn = true;
            excelconfig.ColumnEntity = new List<ColumnModel>();
            string json = cache.Read<string>(dataJson);
            //表头
            List<jfGridModel> columnList = columnJson.ToList<jfGridModel>();
            columnList.Add(new jfGridModel { name = "index", label = "序号", align = "center" });
            //行数据
            DataTable rowData = json.ToTable();
            List<PaymentVo> data = TableToEntity<PaymentVo>(rowData);
            List<PaymentVo> marketings = new List<PaymentVo>();
            foreach (var item in data)
            {
                if (!marketings.Contains(item))
                {
                    marketings.Add(item);
                }
            }
            foreach (var item in marketings)
            {
                item.index = indexs++;


                //付款类型
                var payType = dataItemBLL.GetDetailItemName(item.PayType, "PaymentType");
                if (payType != null)
                {
                    item.PayType = payType.F_ItemName;
                }
                //支付方式
                var paymentMethod = dataItemBLL.GetDetailItemName(item.PaymentMethod, "Client_PaymentMode");
                if (paymentMethod != null)
                {
                    item.PaymentMethod = paymentMethod.F_ItemName;
                }
                //我司支付
                var paymentHeader = dataItemBLL.GetDetailItemName(item.PaymentHeader, "PaymentHeader");
                if (paymentHeader != null)
                {
                    item.PaymentHeader = paymentHeader.F_ItemName;
                }
                //状态             
                var paymentStatus = dataItemBLL.GetDetailItemName(item.PaymentStatus, "PaymentStatus");
                if (paymentStatus != null)
                {
                    item.PaymentStatus = paymentStatus.F_ItemName;
                }

            }
            rowData = marketings.ToJson().ToTable();
            //写入Excel表头
            Dictionary<string, string> exportFieldMap = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(exportField))
            {
                string[] exportFields = exportField.Split(',');
                foreach (var field in exportFields)
                {
                    if (!exportFieldMap.ContainsKey(field))
                    {
                        exportFieldMap.Add(field, "1");
                    }
                }
            }

            foreach (jfGridModel columnModel in columnList)
            {
                if (exportFieldMap.ContainsKey(columnModel.name) || string.IsNullOrEmpty(exportField))
                {
                    excelconfig.ColumnEntity.Add(new ColumnModel()
                    {
                        Column = columnModel.name,
                        ExcelColumn = columnModel.label,
                        Alignment = columnModel.align,
                        Width = columnModel.name.Length
                    });
                }
            }
            ExcelHelper.ExcelDownload(rowData, excelconfig);
        }
        [HttpPost, ValidateInput(false)]
        public void ExportExcelBilling(string fileName, string columnJson, string dataJson, string exportField)
        {
            int indexs = 1;
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = Server.UrlDecode(fileName);
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 15;
            excelconfig.FileName = Server.UrlDecode(fileName) + ".xls";
            excelconfig.IsAllSizeColumn = true;
            excelconfig.ColumnEntity = new List<ColumnModel>();
            string json = cache.Read<string>(dataJson);
            //表头
            List<jfGridModel> columnList = columnJson.ToList<jfGridModel>();
            columnList.Add(new jfGridModel { name = "index", label = "序号", align = "center" });
            //行数据
            DataTable rowData = json.ToTable();
            List<ProjectBillingVo> data = TableToEntity<ProjectBillingVo>(rowData);
            List<ProjectBillingVo> marketings = new List<ProjectBillingVo>();
            foreach (var item in data)
            {
                if (!marketings.Contains(item))
                {
                    marketings.Add(item);
                }
            }
            foreach (var item in marketings)
            {
                item.index = indexs++;


                var department = departmentIBLL.GetEntity(item.FDepartmentId);
                if (department != null)
                {
                    item.FDepartmentId = department.F_FullName;
                }
                DataItemDetailEntity billingContent = dataItemBLL.GetDetailItemName(item.BillingContent, "BillingContent");
                if (billingContent != null)
                {
                    item.BillingContent = billingContent.F_ItemName;
                }

                DataItemDetailEntity contractSubject = dataItemBLL.GetDetailItemName(item.BillingUnit, "ContractSubject");
                if (contractSubject != null)
                {
                    item.BillingUnit = contractSubject.F_ItemName;
                }

                DataItemDetailEntity uillingType = dataItemBLL.GetDetailItemName(item.BillingType, "BillingType");
                if (billingContent != null)
                {
                    item.BillingType = uillingType.F_ItemName;
                }

                DataItemDetailEntity billingStatus = dataItemBLL.GetDetailItemName(item.BillingStatus, "BillingStatus");
                if (billingStatus != null)
                {
                    item.BillingStatus = billingStatus.F_ItemName;
                }

            }
            rowData = marketings.ToJson().ToTable();
            //写入Excel表头
            Dictionary<string, string> exportFieldMap = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(exportField))
            {
                string[] exportFields = exportField.Split(',');
                foreach (var field in exportFields)
                {
                    if (!exportFieldMap.ContainsKey(field))
                    {
                        exportFieldMap.Add(field, "1");
                    }
                }
            }

            foreach (jfGridModel columnModel in columnList)
            {
                if (exportFieldMap.ContainsKey(columnModel.name) || string.IsNullOrEmpty(exportField))
                {
                    excelconfig.ColumnEntity.Add(new ColumnModel()
                    {
                        Column = columnModel.name,
                        ExcelColumn = columnModel.label,
                        Alignment = columnModel.align,
                        Width = columnModel.name.Length
                    });
                }
            }
            ExcelHelper.ExcelDownload(rowData, excelconfig);
        }
        /// <summary>
        /// 导出付款
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="columnJson"></param>
        /// <param name="dataJson"></param>
        /// <param name="exportField"></param>
        [HttpPost, ValidateInput(false)]
        public void ExportExcelProjectPayment(string fileName, string columnJson, string dataJson, string exportField)
        {
            int indexs = 1;
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = Server.UrlDecode(fileName);
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 15;
            excelconfig.FileName = Server.UrlDecode(fileName) + ".xls";
            excelconfig.IsAllSizeColumn = true;
            excelconfig.ColumnEntity = new List<ColumnModel>();
            string json = cache.Read<string>(dataJson);
            //表头
            List<jfGridModel> columnList = columnJson.ToList<jfGridModel>();
            columnList.Add(new jfGridModel { name = "index", label = "序号", align = "center" });
            //行数据
            DataTable rowData = json.ToTable();
            List<ProjectPaymentVo> data = TableToEntity<ProjectPaymentVo>(rowData);
            List<ProjectPaymentVo> list = new List<ProjectPaymentVo>();
            foreach (var item in data)
            {
                if (!list.Contains(item))
                {
                    list.Add(item);
                }
            }
            foreach (var item in list)
            {
                item.index = indexs++;
                DataItemDetailEntity payType = dataItemBLL.GetDetailItemName(item.PayType, "PayType");
                DataItemDetailEntity paymentHeader = dataItemBLL.GetDetailItemName(item.PaymentHeader, "PaymentHeader");
                DataItemDetailEntity paymentMethod = dataItemBLL.GetDetailItemName(item.PaymentMethod, "Client_PaymentMode");
                if (payType != null)
                {
                    item.PayType = payType.F_ItemName;
                }
                if (paymentHeader != null)
                {
                    item.PaymentHeader = paymentHeader.F_ItemName;
                }
                if (paymentMethod != null)
                {
                    item.PaymentMethod = paymentMethod.F_ItemName;
                }
                //状态             
                var paymentStatus = dataItemBLL.GetDetailItemName(item.PaymentStatus, "PaymentStatus");
                if (paymentStatus != null)
                {
                    item.PaymentStatus = paymentStatus.F_ItemName;
                }
                var projectSource = dataItemBLL.GetDetailItemName(item.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    item.ProjectSource = projectSource.F_ItemName;
                }
                var department = departmentIBLL.GetEntity(item.DepartmentId);
                if (department != null)
                {
                    item.DepartmentId = department.F_FullName;
                }
            }
            rowData = list.ToJson().ToTable();
            //写入Excel表头
            Dictionary<string, string> exportFieldMap = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(exportField))
            {
                string[] exportFields = exportField.Split(',');
                foreach (var field in exportFields)
                {
                    if (!exportFieldMap.ContainsKey(field))
                    {
                        exportFieldMap.Add(field, "1");
                    }
                }
            }

            foreach (jfGridModel columnModel in columnList)
            {
                if (exportFieldMap.ContainsKey(columnModel.name) || string.IsNullOrEmpty(exportField))
                {
                    excelconfig.ColumnEntity.Add(new ColumnModel()
                    {
                        Column = columnModel.name,
                        ExcelColumn = columnModel.label,
                        Alignment = columnModel.align,
                        Width = columnModel.name.Length
                    });
                }
            }
            ExcelHelper.ExcelDownload(rowData, excelconfig);
        }
        /// <summary>
        /// 导出批量付款
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="columnJson"></param>
        /// <param name="dataJson"></param>
        /// <param name="exportField"></param>
        [HttpPost, ValidateInput(false)]
        public void ExportExcelProjectPaymentList(string fileName, string columnJson, string dataJson, string exportField)
        {
            int indexs = 1;
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = Server.UrlDecode(fileName);
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 15;
            excelconfig.FileName = Server.UrlDecode(fileName) + ".xls";
            excelconfig.IsAllSizeColumn = true;
            excelconfig.ColumnEntity = new List<ColumnModel>();
            string json = cache.Read<string>(dataJson);
            //表头
            List<jfGridModel> columnList = columnJson.ToList<jfGridModel>();
            columnList.Add(new jfGridModel { name = "index", label = "序号", align = "center" });
            //行数据
            DataTable rowData = json.ToTable();
            //List<ProjectPaymentListVo> data = TableToEntity<ProjectPaymentListVo>(rowData);
            //List<ProjectPaymentListVo> list = new List<ProjectPaymentListVo>();
            //foreach (var item in data)
            //{
            //    if (!list.Contains(item))
            //    {
            //        list.Add(item);
            //    }
            //}
            //foreach (var item in list)
            //{
            //    item.index = indexs++;

            //    if (item.ProjectId != null)
            //    {
            //        var projectId = "";
            //        string[] strList = item.ProjectId.Split(',');

            //        for (var i = 0; i < strList.Length; i++)
            //        {
            //            var Responsible = projectManageIBLL.GetProjectEntity(strList[i]);
            //            if (Responsible.ProjectName != null)
            //            {
            //                if (projectId == "")
            //                {
            //                    projectId = Responsible.ProjectName;
            //                }
            //                else
            //                {

            //                    projectId = projectId + "。" + Responsible.ProjectName;
            //                }
            //            }
            //        }
            //        item.ProjectName = projectId;
            //    }
            //    DataItemDetailEntity payType = dataItemBLL.GetDetailItemName(item.PayType, "PayType");
            //        DataItemDetailEntity paymentHeader = dataItemBLL.GetDetailItemName(item.PaymentHeader, "PaymentHeader");
            //        DataItemDetailEntity paymentMethod = dataItemBLL.GetDetailItemName(item.PaymentMethod, "Client_PaymentMode");
            //        if (payType != null)
            //        {
            //            item.PayType = payType.F_ItemName;
            //        }
            //        if (paymentHeader != null)
            //        {
            //            item.PaymentHeader = paymentHeader.F_ItemName;
            //        }
            //        if (paymentMethod != null)
            //        {
            //            item.PaymentMethod = paymentMethod.F_ItemName;
            //        }
            //        //状态             
            //        var paymentStatus = dataItemBLL.GetDetailItemName(item.PaymentStatus, "PaymentStatus");
            //        if (paymentStatus != null)
            //        {
            //            item.PaymentStatus = paymentStatus.F_ItemName;
            //        }
            //        var projectSource = dataItemBLL.GetDetailItemName(item.ProjectSource, "ProjectSource");
            //        if (projectSource != null)
            //        {
            //            item.ProjectSource = projectSource.F_ItemName;
            //        }
            //        var department = departmentIBLL.GetEntity(item.DepartmentId);
            //        if (department != null)
            //        {
            //            item.DepartmentId = department.F_FullName;
            //        }
            //    }
            //    rowData = list.ToJson().ToTable();
            //写入Excel表头
            Dictionary<string, string> exportFieldMap = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(exportField))
            {
                string[] exportFields = exportField.Split(',');
                foreach (var field in exportFields)
                {
                    if (!exportFieldMap.ContainsKey(field))
                    {
                        exportFieldMap.Add(field, "1");
                    }
                }
            }

            foreach (jfGridModel columnModel in columnList)
            {
                if (exportFieldMap.ContainsKey(columnModel.name) || string.IsNullOrEmpty(exportField))
                {
                    excelconfig.ColumnEntity.Add(new ColumnModel()
                    {
                        Column = columnModel.name,
                        ExcelColumn = columnModel.label,
                        Alignment = columnModel.align,
                        Width = columnModel.name.Length
                    });
                }
            }
            ExcelHelper.ExcelDownload(rowData, excelconfig);
        }
        /// <summary>
        /// 导出报备
        /// </summary>
        [HttpPost, ValidateInput(false)]
        public void ExportExcelProject(string fileName, string columnJson, string dataJson, string exportField)
        {
            int indexs = 1;
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = Server.UrlDecode(fileName);
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 15;
            excelconfig.FileName = Server.UrlDecode(fileName) + ".xls";
            excelconfig.IsAllSizeColumn = true;
            excelconfig.ColumnEntity = new List<ColumnModel>();
            string json = cache.Read<string>(dataJson);
            //表头
            List<jfGridModel> columnList = columnJson.ToList<jfGridModel>();
            columnList.Add(new jfGridModel { name = "index", label = "序号", align = "center" });
            //行数据
            DataTable rowData = json.ToTable();
            List<ProjectVo> data = TableToEntity<ProjectVo>(rowData);
            List<ProjectVo> list = new List<ProjectVo>();
            foreach (var item in data)
            {
                if (!list.Contains(item))
                {
                    list.Add(item);
                }
            }
            foreach (var item in list)
            {
                item.index = indexs++;


                var preparedPerson = userIBLL.GetEntityByUserId(item.PreparedPerson);
                if (preparedPerson != null)
                {
                    item.PreparedPerson = preparedPerson.F_RealName;
                }
                var followPerson = userIBLL.GetEntityByUserId(item.FollowPerson);
                if (followPerson != null)
                {
                    item.FollowPerson = followPerson.F_RealName;
                }
                var projectStatus = dataItemBLL.GetDetailItemName(item.ProjectStatus, "ProjectStatus");
                if (projectStatus != null)
                {
                    item.ProjectStatus = projectStatus.F_ItemName;
                }
                var projectSource = dataItemBLL.GetDetailItemName(item.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    item.ProjectSource = projectSource.F_ItemName;
                }

            }
            rowData = list.ToJson().ToTable();
            //写入Excel表头
            Dictionary<string, string> exportFieldMap = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(exportField))
            {
                string[] exportFields = exportField.Split(',');
                foreach (var field in exportFields)
                {
                    if (!exportFieldMap.ContainsKey(field))
                    {
                        exportFieldMap.Add(field, "1");
                    }
                }
            }

            foreach (jfGridModel columnModel in columnList)
            {
                if (exportFieldMap.ContainsKey(columnModel.name) || string.IsNullOrEmpty(exportField))
                {
                    excelconfig.ColumnEntity.Add(new ColumnModel()
                    {
                        Column = columnModel.name,
                        ExcelColumn = columnModel.label,
                        Alignment = columnModel.align,
                        Width = columnModel.name.Length
                    });
                }
            }
            ExcelHelper.ExcelDownload(rowData, excelconfig);
        }
        [HttpPost, ValidateInput(false)]
        public void ExportExcelPayCol(string fileName, string columnJson, string dataJson, string exportField)
        {
            int indexs = 1;
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = Server.UrlDecode(fileName);
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 15;
            excelconfig.FileName = Server.UrlDecode(fileName) + ".xls";
            excelconfig.IsAllSizeColumn = true;
            excelconfig.ColumnEntity = new List<ColumnModel>();
            string json = cache.Read<string>(dataJson);
            //表头
            List<jfGridModel> columnList = columnJson.ToList<jfGridModel>();
            columnList.Add(new jfGridModel { name = "index", label = "序号", align = "center" });
            //行数据
            DataTable rowData = json.ToTable();


            List<ProjectPayCollectionVo> data = TableToEntity<ProjectPayCollectionVo>(rowData);
            List<ProjectPayCollectionVo> list = new List<ProjectPayCollectionVo>();
            foreach (var item in data)
            {
                if (!list.Contains(item))
                {
                    list.Add(item);
                }
            }

            foreach (var item in list)
            {
                item.index = indexs++;
                ////核算1
                ////自主营销
                //if (item.ProjectSource == "1")
                //{
                //    if (item.PaymentAmount == null)
                //    {

                //        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.02;
                //        item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;
                //    }
                //    else if (item.PaymentAmount < (item.ContractAmount * (decimal?)0.3))
                //    {
                //        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.005;
                //        item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;
                //    }
                //    else
                //    {

                //        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.02;
                //        item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;

                //    }
                //}
                ////自主营销
                //if (item.ProjectSource == "2")
                //{
                //    if (item.PaymentAmount == null)
                //    {

                //        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.015;
                //        item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;
                //    }
                //    else if (item.PaymentAmount < (item.ContractAmount * (decimal?)0.3))
                //    {


                //        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.002;
                //        item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;

                //    }
                //    else
                //    {

                //        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.001;
                //        item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;

                //    }
                //}
                //款绩
                //自主营销
                if (item.ProjectSource == "1")
                {
                    if (item.PaymentAmount == null)
                    {

                        item.PayCollectionAmount = item.Amount * (decimal?)0.02;
                        item.PayCollectionAmount = Math.Round((decimal)item.PayCollectionAmount * 100) / 100;
                    }
                    else if (item.PaymentAmount < (item.Amount * (decimal?)0.3))
                    {
                        item.PayCollectionAmount = item.Amount * (decimal?)0.005;
                        item.PayCollectionAmount = Math.Round((decimal)item.PayCollectionAmount * 100) / 100;
                    }
                    else
                    {
                        item.PayCollectionAmount = item.Amount * (decimal?)0.02;
                        item.PayCollectionAmount = Math.Round((decimal)item.PayCollectionAmount * 100) / 100;
                    }
                }
                //自主营销
                if (item.ProjectSource == "2")
                {
                    if (item.PaymentAmount == null)
                    {
                        item.PayCollectionAmount = item.Amount * (decimal?)0.015;
                        item.PayCollectionAmount = Math.Round((decimal)item.PayCollectionAmount * 100) / 100;
                    }
                    else if (item.PaymentAmount < (item.Amount * (decimal?)0.3))
                    {
                        item.PayCollectionAmount = (item.Amount * (decimal?)0.002) - item.PaymentAmount;
                        item.PayCollectionAmount = Math.Round((decimal)item.PayCollectionAmount * 100) / 100;
                    }
                    else
                    {

                        item.PayCollectionAmount = item.Amount * (decimal?)0.001;
                        item.PayCollectionAmount = Math.Round((decimal)item.PayCollectionAmount * 100) / 100;
                    }
                }

                var department = departmentIBLL.GetEntity(item.DepartmentId);
                if (department != null)
                {
                    item.DepartmentId = department.F_FullName;
                }

                DataItemDetailEntity projectSource = dataItemBLL.GetDetailItemName(item.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    item.ProjectSource = projectSource.F_ItemName;
                }

            }


            rowData = list.ToJson().ToTable();

            //写入Excel表头
            Dictionary<string, string> exportFieldMap = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(exportField))
            {
                string[] exportFields = exportField.Split(',');
                foreach (var field in exportFields)
                {
                    if (!exportFieldMap.ContainsKey(field))
                    {
                        exportFieldMap.Add(field, "1");
                    }
                }
            }

            foreach (jfGridModel columnModel in columnList)
            {
                if (exportFieldMap.ContainsKey(columnModel.name) || string.IsNullOrEmpty(exportField))
                {
                    excelconfig.ColumnEntity.Add(new ColumnModel()
                    {
                        Column = columnModel.name,
                        ExcelColumn = columnModel.label,
                        Alignment = columnModel.align,
                        Width = columnModel.name.Length
                    });
                }
            }
            ExcelHelper.ExcelDownload(rowData, excelconfig);
        }

        [HttpPost, ValidateInput(false)]
        public void ExportExcelProReportForms(string fileName, string columnJson, string dataJson, string exportField)
        {
            int indexs = 0;
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = Server.UrlDecode(fileName);
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 15;
            excelconfig.FileName = Server.UrlDecode(fileName) + ".xls";
            excelconfig.IsAllSizeColumn = true;
            excelconfig.ColumnEntity = new List<ColumnModel>();
            string json = cache.Read<string>(dataJson);
            //表头
            List<jfGridModel> columnList = columnJson.ToList<jfGridModel>();
            columnList.Add(new jfGridModel { name = "index", label = "序号", align = "center" });

            //行数据
            DataTable rowData = json.ToTable();
            List<ProductionEntity> data = TableToEntity<ProductionEntity>(rowData);
            List<ProductionEntity> marketings = new List<ProductionEntity>();

            foreach (var item in data)
            {
                if (!marketings.Contains(item))
                {
                    marketings.Add(item);
                }
            }
            foreach (var item in marketings)
            {

                item.index = ++indexs;
                //营销人员
                UserEntity followPerson = userIBLL.GetFollowPersonNameByUserId(item.FollowPerson);
                if (followPerson != null)
                {
                    item.FollowPerson = followPerson.F_RealName;
                }
                //项目负责人

                if (item.ProjectResponsible != null)
                {
                    string[] strList = item.ProjectResponsible.Split(',');

                    string projectResponsible = "";
                    for (var i = 0; i < strList.Length; i++)
                    {
                        var Responsible = userIBLL.GetFollowPersonNameByUserId(strList[i]);
                        if (Responsible != null)
                        {
                            if (string.IsNullOrWhiteSpace(projectResponsible))
                            {
                                projectResponsible = Responsible.F_RealName;
                            }
                            else
                            {
                                projectResponsible = projectResponsible + "," + Responsible.F_RealName;
                            }
                        }
                    }
                    item.ProjectResponsible = projectResponsible;
                }
                ////核算1
                ////自主营销
                //if (item.ProjectSource == "1")
                //{
                //    if (item.PaymentAmount == null)
                //    {

                //        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.02;
                //        item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;
                //    }
                //    else if (item.PaymentAmount < (item.ContractAmount * (decimal?)0.3))
                //    {
                //        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.005;
                //        item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;
                //    }
                //    else
                //    {

                //        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.02;
                //        item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;
                //    }
                //}
                ////渠道营销
                //if (item.ProjectSource == "2")
                //{
                //    if (item.PaymentAmount == null)
                //    {
                //        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.015;
                //        item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;
                //    }
                //    else if (item.PaymentAmount < (item.ContractAmount * (decimal?)0.3))
                //    {
                //        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.002;
                //        item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;
                //    }
                //    else
                //    {
                //        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.001;
                //        item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;
                //    }
                //}
                //核算
                if (item.ProjectSource == "1" || item.ProjectSource == "2")
                {
                    if (item.TaskStatus == "3" || item.TaskStatus == "4" || item.TaskStatus == "9" || item.TaskStatus == "11" || item.TaskStatus == "5")
                    {
                        if (item.PaymentAmount != null)
                        {
                            if (item.PaymentAmount >= (item.ContractAmount * (decimal?)0.3))
                            {
                                if (item.SubAmount != null && item.MainAmount == null)
                                {
                                    item.DepartmentIdAmount = (item.ContractAmount - item.PaymentAmount - item.SubAmount) * (decimal?)0.3;
                                    item.DepartmentIdAmount = Math.Round((decimal)item.DepartmentIdAmount * 100) / 100;
                                    // item.DepartmentIdAmount = decimal.Round(decimal.Parse(((item.ContractAmount - item.PaymentAmount - item.SubAmount) * (decimal?)0.3).ToString()), 2);
                                }
                                else if (item.SubAmount == null && item.MainAmount != null)
                                {
                                    item.DepartmentIdAmount = (item.ContractAmount - item.PaymentAmount - item.MainAmount) * (decimal?)0.3;
                                    item.DepartmentIdAmount = Math.Round((decimal)item.DepartmentIdAmount * 100) / 100;
                                    //item.DepartmentIdAmount = decimal.Round(decimal.Parse(((item.ContractAmount - item.PaymentAmount - item.MainAmount) * (decimal?)0.3).ToString()),2);
                                }
                                else
                                {
                                    item.DepartmentIdAmount = (item.ContractAmount - item.PaymentAmount) * (decimal?)0.3;
                                    item.DepartmentIdAmount = Math.Round((decimal)item.DepartmentIdAmount * 100) / 100;
                                    //item.DepartmentIdAmount = decimal.Round(decimal.Parse(((item.ContractAmount - item.PaymentAmount) * (decimal?)0.3).ToString()), 2);
                                }
                            }
                            else if (item.PaymentAmount < (item.ContractAmount * (decimal?)0.3))
                            {
                                item.DepartmentIdAmount = item.ContractAmount * (decimal?)0.2;
                                item.DepartmentIdAmount = Math.Round((decimal)item.DepartmentIdAmount * 100) / 100;
                                //item.DepartmentIdAmount = decimal.Round(decimal.Parse((item.ContractAmount * (decimal?)0.2).ToString()), 2);
                            }
                        }
                        else
                        {
                            if (item.ContractAmount != null)
                            {
                                item.DepartmentIdAmount = (item.ContractAmount * (decimal?)0.2);
                                item.DepartmentIdAmount = Math.Round((decimal)item.DepartmentIdAmount * 100) / 100;
                                //item.DepartmentIdAmount = decimal.Round(decimal.Parse((item.ContractAmount * (decimal?)0.2).ToString()), 2);
                            }
                        }


                    }






                }

                DataItemDetailEntity contractSubject = dataItemBLL.GetDetailItemName(item.ContractSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    item.ContractSubject = contractSubject.F_ItemName;
                }

                DataItemDetailEntity projectSource = dataItemBLL.GetDetailItemName(item.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    item.ProjectSource = projectSource.F_ItemName;
                }

                DataItemDetailEntity reportSubject = dataItemBLL.GetDetailItemName(item.ReportSubject, "ContractSubject");
                if (reportSubject != null)
                {
                    item.ReportSubject = reportSubject.F_ItemName;
                }

                DataItemDetailEntity taskStatus = dataItemBLL.GetDetailItemName(item.TaskStatus, "TaskStatus");
                if (taskStatus != null)
                {
                    item.TaskStatus = taskStatus.F_ItemName;
                }
                if (item.DepartmentId != null && item.DepartmentId.IndexOf(",") == -1)
                {
                    var department = departmentIBLL.GetEntity(item.DepartmentId);
                    if (department != null)
                    {
                        item.DepartmentId = department.F_FullName;
                    }
                    else
                    {
                        item.DepartmentId = null;
                    }
                }
                else
                {
                    if (item.DepartmentId != null)
                    {
                        string[] array = item.DepartmentId.Split(new char[] { ',' });
                        foreach (var dept in array)
                        {
                            var department = departmentIBLL.GetEntity(dept);
                            if (department != null)
                            {
                                item.DepartmentId = department.F_FullName;
                            }
                            else
                            {
                                item.DepartmentId = null;
                            }
                        }
                    }
                    else
                    {
                        item.DepartmentId = null;
                    }

                }
                if (item.J_F_FullName != null && item.J_F_FullName.IndexOf(",") == -1)
                {
                    var jfdepartment = departmentIBLL.GetEntity(item.J_F_FullName);
                    if (jfdepartment != null)
                    {
                        item.J_F_FullName = jfdepartment.F_FullName;
                    }
                    else
                    {
                        item.J_F_FullName = null;
                    }
                }
                else
                {
                    if (item.J_F_FullName != null)
                    {
                        string[] array = item.J_F_FullName.Split(new char[] { ',' });
                        foreach (var dept in array)
                        {
                            var department = departmentIBLL.GetEntity(dept);
                            if (department != null)
                            {
                                item.J_F_FullName = department.F_FullName;
                            }
                            else
                            {
                                item.J_F_FullName = null;
                            }
                        }

                    }
                    else
                    {
                        item.J_F_FullName = null;
                    }



                }

            }
            rowData = marketings.ToJson().ToTable();
            //写入Excel表头
            Dictionary<string, string> exportFieldMap = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(exportField))
            {
                string[] exportFields = exportField.Split(',');
                foreach (var field in exportFields)
                {
                    if (!exportFieldMap.ContainsKey(field))
                    {
                        exportFieldMap.Add(field, "1");
                    }
                }
            }

            foreach (jfGridModel columnModel in columnList)
            {
                if (exportFieldMap.ContainsKey(columnModel.name) || string.IsNullOrEmpty(exportField))
                {
                    excelconfig.ColumnEntity.Add(new ColumnModel()
                    {
                        Column = columnModel.name,
                        ExcelColumn = columnModel.label,
                        Alignment = columnModel.align,
                        Width = columnModel.name.Length
                    });
                }
            }
            ExcelHelper.ExcelDownload(rowData, excelconfig);
        }
        [HttpPost, ValidateInput(false)]
        public void ExportExcelMarReportForms(string fileName, string columnJson, string dataJson, string exportField)
        {
            int indexs = 0;
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = Server.UrlDecode(fileName);
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 15;
            excelconfig.FileName = Server.UrlDecode(fileName) + ".xls";
            excelconfig.IsAllSizeColumn = true;
            excelconfig.ColumnEntity = new List<ColumnModel>();
            string json = cache.Read<string>(dataJson);
            //表头
            List<jfGridModel> columnList = columnJson.ToList<jfGridModel>();
            columnList.Add(new jfGridModel { name = "index", label = "序号", align = "center" });
            //行数据
            DataTable rowData = json.ToTable();
            List<MarketingEntity> data = TableToEntity<MarketingEntity>(rowData);
            List<MarketingEntity> marketings = new List<MarketingEntity>();

            foreach (var item in data)
            {



                if (!marketings.Contains(item))
                {
                    marketings.Add(item);
                }
            }


            foreach (var item in marketings)
            {
                item.index = ++indexs;
                //营销人员
                UserEntity followPerson = userIBLL.GetFollowPersonNameByUserId(item.FollowPerson);
                if (followPerson != null)
                {
                    item.FollowPerson = followPerson.F_RealName;
                }
                //项目负责人
                /* UserEntity projectResponsible = userIBLL.GetFollowPersonNameByUserId(item.ProjectResponsible);
                 if (projectResponsible != null)
                 {
                     item.ProjectResponsible = projectResponsible.F_RealName;
                 }*/
                if (item.ProjectResponsible != null)
                {
                    string[] strList = item.ProjectResponsible.Split(',');

                    string projectResponsible = "";
                    for (var i = 0; i < strList.Length; i++)
                    {
                        var Responsible = userIBLL.GetFollowPersonNameByUserId(strList[i]);
                        if (Responsible != null)
                        {
                            if (string.IsNullOrWhiteSpace(projectResponsible))
                            {
                                projectResponsible = Responsible.F_RealName;
                            }
                            else
                            {
                                projectResponsible = projectResponsible + "," + Responsible.F_RealName;
                            }
                        }
                    }
                    item.ProjectResponsible = projectResponsible;
                }

                ////核算1
                ////自主营销
                //if (item.ProjectSource == "1")
                //{
                //    if (item.PaymentAmount == null)
                //    {

                //        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.02;
                //        item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;
                //    }
                //    else if (item.PaymentAmount < (item.ContractAmount * (decimal?)0.3))
                //    {


                //        item.FollowPersonAmount = (item.ContractAmount * (decimal?)0.005) - item.PaymentAmount;
                //        item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;

                //    }
                //    else
                //    {

                //        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.02;
                //        item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;

                //    }
                //}
                ////渠道营销
                //if (item.ProjectSource == "2")
                //{
                //    if (item.PaymentAmount == null)
                //    {
                //        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.015;
                //        item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;
                //    }
                //    else if (item.PaymentAmount < (item.ContractAmount * (decimal?)0.3))
                //    {
                //        item.FollowPersonAmount = (item.ContractAmount * (decimal?)0.002) - item.PaymentAmount;
                //        item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;
                //    }
                //    else
                //    {
                //        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.001;
                //        item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;
                //    }
                //}
                //核算
                //自主营销

                if (item.ProjectSource == "1")
                {
                    if (item.PaymentAmount == null)
                    {
                        item.DepartmentIdAmount = item.ContractAmount * (decimal?)0.3;
                        if (item.DepartmentIdAmount != null)
                        {
                            item.DepartmentIdAmount = Math.Round((decimal)item.DepartmentIdAmount * 100) / 100;
                        }
                    }
                    else if (item.PaymentAmount < (item.ContractAmount * (decimal?)0.3))
                    {

                        item.DepartmentIdAmount = (item.ContractAmount * (decimal?)0.3) - item.PaymentAmount;
                        if (item.DepartmentIdAmount != null)
                        {
                            item.DepartmentIdAmount = Math.Round((decimal)item.DepartmentIdAmount * 100) / 100;
                        }
                    }
                    else
                    {
                        item.DepartmentIdAmount = item.ContractAmount * (decimal?)0.03;
                        if (item.DepartmentIdAmount != null)
                        {
                            item.DepartmentIdAmount = Math.Round((decimal)item.DepartmentIdAmount * 100) / 100;
                        }
                    }

                    //渠道营销
                    if (item.ProjectSource == "2")
                    {
                        item.DepartmentIdAmount = item.ContractAmount * (decimal?)0.03;
                        if (item.DepartmentIdAmount != null)
                        {
                            item.DepartmentIdAmount = Math.Round((decimal)item.DepartmentIdAmount * 100) / 100;
                        }
                    }
                }
                DataItemDetailEntity contractSubject = dataItemBLL.GetDetailItemName(item.ContractSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    item.ContractSubject = contractSubject.F_ItemName;
                }

                //DataItemDetailEntity billingStatus = dataItemBLL.GetDetailItemName(item.BillingStatus, "BillingStatus");
                //if (billingStatus != null && (item.BillingStatus == "4" || item.BillingStatus == "7"))
                //{
                //    item.BillingStatus ="已开票";
                //}
                //else
                //{
                //    item.BillingStatus = "未开票";
                //}
                if (item.BillingStatus == null)
                {
                    item.BillingStatus = "未开票";
                }
                else
                {
                    item.BillingStatus = "已开票（" + decimal.Round(decimal.Parse(item.BillingStatus.Trim()), 2) + "）";
                }

                DataItemDetailEntity projectSource = dataItemBLL.GetDetailItemName(item.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    item.ProjectSource = projectSource.F_ItemName;
                }

                DataItemDetailEntity reportSubject = dataItemBLL.GetDetailItemName(item.ReportSubject, "ContractSubject");
                if (reportSubject != null)
                {
                    item.ReportSubject = reportSubject.F_ItemName;
                }

                DataItemDetailEntity taskStatus = dataItemBLL.GetDetailItemName(item.TaskStatus, "TaskStatus");
                if (taskStatus != null)
                {
                    item.TaskStatus = taskStatus.F_ItemName;
                }
                var mdepartment = departmentIBLL.GetEntity(item.MainDepartmentId);
                if (mdepartment != null)
                {
                    item.MainDepartmentId = mdepartment.F_FullName;
                }

                var sdepartment = departmentIBLL.GetEntity(item.SubDepartmentId);
                if (sdepartment != null)
                {
                    item.SubDepartmentId = sdepartment.F_FullName;
                }

                if (item.DepartmentId != null && item.DepartmentId.IndexOf(",") == -1)
                {
                    var department = departmentIBLL.GetEntity(item.DepartmentId);
                    if (department != null)
                    {
                        item.DepartmentId = department.F_FullName;
                    }
                    else
                    {
                        item.DepartmentId = null;
                    }
                }
                else
                {
                    if (item.DepartmentId != null)
                    {
                        string[] array = item.DepartmentId.Split(new char[] { ',' });
                        foreach (var dept in array)
                        {
                            var department = departmentIBLL.GetEntity(dept);
                            if (department != null)
                            {
                                item.DepartmentId = department.F_FullName;
                            }
                            else
                            {
                                item.DepartmentId = null;
                            }
                        }
                    }
                    else
                    {
                        item.DepartmentId = null;
                    }

                }
                if (item.J_F_FullName != null && item.J_F_FullName.IndexOf(",") == -1)
                {
                    var jfdepartment = departmentIBLL.GetEntity(item.J_F_FullName);
                    if (jfdepartment != null)
                    {
                        item.J_F_FullName = jfdepartment.F_FullName;
                    }
                    else
                    {
                        item.J_F_FullName = null;
                    }
                }
                else
                {
                    if (item.J_F_FullName != null)
                    {
                        string[] array = item.J_F_FullName.Split(new char[] { ',' });
                        foreach (var dept in array)
                        {
                            var department = departmentIBLL.GetEntity(dept);
                            if (department != null)
                            {
                                item.J_F_FullName = department.F_FullName;
                            }
                            else
                            {
                                item.J_F_FullName = null;
                            }
                        }

                    }
                    else
                    {
                        item.J_F_FullName = null;
                    }



                }

            }
            rowData = marketings.ToJson().ToTable();
            //写入Excel表头
            Dictionary<string, string> exportFieldMap = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(exportField))
            {
                string[] exportFields = exportField.Split(',');
                foreach (var field in exportFields)
                {
                    if (!exportFieldMap.ContainsKey(field))
                    {
                        exportFieldMap.Add(field, "1");
                    }
                }
            }

            foreach (jfGridModel columnModel in columnList)
            {
                if (exportFieldMap.ContainsKey(columnModel.name) || string.IsNullOrEmpty(exportField))
                {
                    excelconfig.ColumnEntity.Add(new ColumnModel()
                    {
                        Column = columnModel.name,
                        ExcelColumn = columnModel.label,
                        Alignment = columnModel.align,
                        Width = columnModel.name.Length
                    });
                }
            }
            ExcelHelper.ExcelDownload(rowData, excelconfig);
        }
        /// <summary>
        /// 合作伙伴营销台账
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="columnJson"></param>
        /// <param name="dataJson"></param>
        /// <param name="exportField"></param>
        public void ExportExcelMarReportFormsHZ(string fileName, string columnJson, string dataJson, string exportField)
        {
            int indexs = 0;
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = Server.UrlDecode(fileName);
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 15;
            excelconfig.FileName = Server.UrlDecode(fileName) + ".xls";
            excelconfig.IsAllSizeColumn = true;
            excelconfig.ColumnEntity = new List<ColumnModel>();
            //表头
            List<jfGridModel> columnList = columnJson.ToList<jfGridModel>();
            columnList.Add(new jfGridModel { name = "index", label = "序号", align = "center" });
            //行数据
            //DataTable rowData = dataJson.ToTable();
            List<MarketingEntity> data = JsonConvert.DeserializeObject<List<MarketingEntity>>(dataJson);
            List<MarketingEntity> marketings = new List<MarketingEntity>();
            foreach (var item in data)
            {

                if (!marketings.Contains(item))
                {
                    marketings.Add(item);
                }
            }


            foreach (var item in marketings)
            {
                item.index = ++indexs;
                //营销人员
                UserEntity followPerson = userIBLL.GetFollowPersonNameByUserId(item.FollowPerson);
                if (followPerson != null)
                {
                    item.FollowPerson = followPerson.F_RealName;
                }
                //项目负责人

                if (item.ProjectResponsible != null)
                {
                    string[] strList = item.ProjectResponsible.Split(',');

                    string projectResponsible = "";
                    for (var i = 0; i < strList.Length; i++)
                    {
                        var Responsible = userIBLL.GetFollowPersonNameByUserId(strList[i]);
                        if (Responsible != null)
                        {
                            if (string.IsNullOrWhiteSpace(projectResponsible))
                            {
                                projectResponsible = Responsible.F_RealName;
                            }
                            else
                            {
                                projectResponsible = projectResponsible + "," + Responsible.F_RealName;
                            }
                        }
                    }
                    item.ProjectResponsible = projectResponsible;
                }

                DataItemDetailEntity contractSubject = dataItemBLL.GetDetailItemName(item.ContractSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    item.ContractSubject = contractSubject.F_ItemName;
                }

                if (item.BillingStatus == null)
                {
                    item.BillingStatus = "未开票";
                }
                else
                {
                    item.BillingStatus = "已开票（" + decimal.Round(decimal.Parse(item.BillingStatus.Trim()), 2) + "）";
                }

                DataItemDetailEntity projectSource = dataItemBLL.GetDetailItemName(item.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    item.ProjectSource = projectSource.F_ItemName;
                }

                DataItemDetailEntity reportSubject = dataItemBLL.GetDetailItemName(item.ReportSubject, "ContractSubject");
                if (reportSubject != null)
                {
                    item.ReportSubject = reportSubject.F_ItemName;
                }

                DataItemDetailEntity taskStatus = dataItemBLL.GetDetailItemName(item.TaskStatus, "TaskStatus");
                if (taskStatus != null)
                {
                    item.TaskStatus = taskStatus.F_ItemName;
                }
                if (item.DepartmentIdCA != null && item.DepartmentIdCA.IndexOf(",") == -1)
                {
                    var department = departmentIBLL.GetEntity(item.DepartmentIdCA);
                    if (department != null)
                    {
                        item.DepartmentIdCA = department.F_FullName;
                    }
                    else
                    {
                        item.DepartmentIdCA = null;
                    }
                }
                else
                {
                    if (item.DepartmentIdCA != null)
                    {
                        string[] array = item.DepartmentIdCA.Split(new char[] { ',' });
                        foreach (var dept in array)
                        {
                            var department = departmentIBLL.GetEntity(dept);
                            if (department != null)
                            {
                                item.DepartmentIdCA = department.F_FullName;
                            }
                            else
                            {
                                item.DepartmentIdCA = null;
                            }
                        }
                    }
                    else
                    {
                        item.DepartmentIdCA = null;
                    }

                }
                if (item.J_F_FullName != null && item.J_F_FullName.IndexOf(",") == -1)
                {
                    var jfdepartment = departmentIBLL.GetEntity(item.J_F_FullName);
                    if (jfdepartment != null)
                    {
                        item.J_F_FullName = jfdepartment.F_FullName;
                    }
                    else
                    {
                        item.J_F_FullName = null;
                    }
                }
                else
                {
                    if (item.J_F_FullName != null)
                    {
                        string[] array = item.J_F_FullName.Split(new char[] { ',' });
                        foreach (var dept in array)
                        {
                            var department = departmentIBLL.GetEntity(dept);
                            if (department != null)
                            {
                                item.J_F_FullName = department.F_FullName;
                            }
                            else
                            {
                                item.J_F_FullName = null;
                            }
                        }

                    }
                    else
                    {
                        item.J_F_FullName = null;
                    }



                }

            }
            DataTable rowData = marketings.ToJson().ToTable();
            //写入Excel表头
            Dictionary<string, string> exportFieldMap = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(exportField))
            {
                string[] exportFields = exportField.Split(',');
                foreach (var field in exportFields)
                {
                    if (!exportFieldMap.ContainsKey(field))
                    {
                        exportFieldMap.Add(field, "1");
                    }
                }
            }

            foreach (jfGridModel columnModel in columnList)
            {
                if (exportFieldMap.ContainsKey(columnModel.name) || string.IsNullOrEmpty(exportField))
                {
                    excelconfig.ColumnEntity.Add(new ColumnModel()
                    {
                        Column = columnModel.name,
                        ExcelColumn = columnModel.label,
                        Alignment = columnModel.align,
                        Width = columnModel.name.Length
                    });
                }
            }
            ExcelHelper.ExcelDownload(rowData, excelconfig);
        }
        /// <summary>
        /// 资金台账
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="columnJson"></param>
        /// <param name="dataJson"></param>
        /// <param name="exportField"></param>
        public void CapitalDepartmentIdForms(string fileName, string columnJson, string dataJson, string exportField)
        {

            int indexs = 0;
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = Server.UrlDecode(fileName);
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 15;
            excelconfig.FileName = Server.UrlDecode(fileName) + ".xls";
            excelconfig.IsAllSizeColumn = true;
            excelconfig.ColumnEntity = new List<ColumnModel>();
            string json = cache.Read<string>(dataJson);
            //表头
            List<jfGridModel> columnList = columnJson.ToList<jfGridModel>();
            columnList.Add(new jfGridModel { name = "index", label = "序号", align = "center" });
            //行数据
            DataTable rowData = json.ToTable();
            List<CapitalDepartmentId> data = TableToEntity<CapitalDepartmentId>(rowData);
            List<CapitalDepartmentId> marketings = new List<CapitalDepartmentId>();
            foreach (var item in data)
            {

                if (!marketings.Contains(item))
                {
                    marketings.Add(item);
                }
            }
            foreach (var item in marketings)
            {

                item.index = ++indexs;
                if (item.AmountList != null)
                {
                    item.AmountList = Math.Round((decimal)item.AmountList * 100) / 100;
                }
                if (item.ContractAmountList != null)
                {
                    item.ContractAmountList = Math.Round((decimal)item.ContractAmountList * 100) / 100;
                }
                if (item.EffectiveAmountList != null)
                {
                    item.EffectiveAmountList = Math.Round((decimal)item.EffectiveAmountList * 100) / 100;
                }
                if (item.ContractAmountSUN != null)
                {
                    item.ContractAmountSUN = Math.Round((decimal)item.ContractAmountSUN * 100) / 100;
                }
                if (item.ContractAmountSUNList != null)
                {
                    item.ContractAmountSUNList = Math.Round((decimal)item.ContractAmountSUNList * 100) / 100;
                }
                if (item.sumList != null)
                {
                    item.sumList = Math.Round((decimal)item.sumList * 100) / 100;
                }



            }
            rowData = marketings.ToJson().ToTable();
            //写入Excel表头
            Dictionary<string, string> exportFieldMap = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(exportField))
            {
                string[] exportFields = exportField.Split(',');
                foreach (var field in exportFields)
                {
                    if (!exportFieldMap.ContainsKey(field))
                    {
                        exportFieldMap.Add(field, "1");
                    }
                }
            }

            foreach (jfGridModel columnModel in columnList)
            {
                if (exportFieldMap.ContainsKey(columnModel.name) || string.IsNullOrEmpty(exportField))
                {
                    excelconfig.ColumnEntity.Add(new ColumnModel()
                    {
                        Column = columnModel.name,
                        ExcelColumn = columnModel.label,
                        Alignment = columnModel.align,
                        Width = columnModel.name.Length
                    });
                }
            }
            ExcelHelper.ExcelDownload(rowData, excelconfig);
        }

        [HttpPost, ValidateInput(false)]
        public void ExportExcelSetReportForms(string fileName, string columnJson, string dataJson, string exportField)
        {

            int indexs = 0;
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = Server.UrlDecode(fileName);
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 15;
            excelconfig.FileName = Server.UrlDecode(fileName) + ".xls";
            excelconfig.IsAllSizeColumn = true;
            excelconfig.ColumnEntity = new List<ColumnModel>();
            string json = cache.Read<string>(dataJson);
            //表头
            List<jfGridModel> columnList = columnJson.ToList<jfGridModel>();
            columnList.Add(new jfGridModel { name = "index", label = "序号", align = "center" });
            //行数据
            DataTable rowData = json.ToTable();
            List<SettleAccountsEntity> data = TableToEntity<SettleAccountsEntity>(rowData);
            List<SettleAccountsEntity> marketings = new List<SettleAccountsEntity>();
            foreach (var item in data)
            {
                //if (!marketings.Contains(item))
                //{
                //    var bol = true;
                //    foreach (var list in marketings)
                //    {
                //        if (list.ContractNo == item.ContractNo && list.ProjectName == item.ProjectName)
                //        {
                //            list.Amount += item.Amount;
                //            list.NotReceived -= item.Amount;
                //            bol = false;
                //        }

                //    }
                //    if (bol) marketings.Add(item);
                //    //marketings.Add(item);
                //}
                if (!marketings.Contains(item))
                {
                    marketings.Add(item);
                }
            }
            foreach (var item in marketings)
            {

                item.index = ++indexs;
                //营销部门
                var departmentcd = departmentIBLL.GetEntity(item.DepartmentIdCA);
                if (departmentcd != null)
                {
                    item.DepartmentIdCA = departmentcd.F_FullName;
                }



                //报备人
                UserEntity preparedPerson = userIBLL.GetFollowPersonNameByUserId(item.PreparedPerson);
                if (preparedPerson != null)
                {
                    item.PreparedPerson = preparedPerson.F_RealName;
                }
                //营销人员
                UserEntity followPerson = userIBLL.GetFollowPersonNameByUserId(item.FollowPerson);
                if (followPerson != null)
                {
                    item.FollowPerson = followPerson.F_RealName;
                }
                //项目负责人

                if (item.ProjectResponsible != null)
                {
                    string[] strList = item.ProjectResponsible.Split(',');

                    string projectResponsible = "";
                    for (var i = 0; i < strList.Length; i++)
                    {
                        var Responsible = userIBLL.GetFollowPersonNameByUserId(strList[i]);
                        if (Responsible != null)
                        {
                            if (string.IsNullOrWhiteSpace(projectResponsible))
                            {
                                projectResponsible = Responsible.F_RealName;
                            }
                            else
                            {
                                projectResponsible = projectResponsible + "," + Responsible.F_RealName;
                            }
                        }
                    }
                    item.ProjectResponsible = projectResponsible;
                }

                //核算1
                //自主营销
                if (item.ProjectSource == "1")
                {
                    if (item.PaymentAmount == null)
                    {

                        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.02;
                        if (item.FollowPersonAmount != null)
                        {
                            item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;
                        }
                    }
                    else if (item.PaymentAmount < (item.ContractAmount * (decimal?)0.3))
                    {
                        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.005;
                        if (item.FollowPersonAmount != null)
                        {
                            item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;
                        }
                    }
                    else
                    {
                        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.02;
                        if (item.FollowPersonAmount != null)
                        {
                            item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;
                        }
                    }
                }
                //渠道营销
                if (item.ProjectSource == "2")
                {
                    if (item.PaymentAmount == null)
                    {

                        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.015;
                        if (item.FollowPersonAmount != null)
                        {
                            item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;
                        }
                    }
                    else if (item.PaymentAmount < (item.ContractAmount * (decimal?)0.3))
                    {
                        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.002;
                        if (item.FollowPersonAmount != null)
                        {
                            item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;
                        }
                    }
                    else
                    {
                        item.FollowPersonAmount = item.ContractAmount * (decimal?)0.001;
                        if (item.FollowPersonAmount != null)
                        {
                            item.FollowPersonAmount = Math.Round((decimal)item.FollowPersonAmount * 100) / 100;
                        }
                    }
                }
                //营销核算

                //自主营销
                if (item.ProjectSource == "1")
                {
                    if (item.PaymentAmount == null)
                    {
                        item.DepartmentIdAmount = item.ContractAmount * (decimal?)0.3;
                        if (item.DepartmentIdAmount != null)
                        {
                            item.DepartmentIdAmount = Math.Round((decimal)item.DepartmentIdAmount * 100) / 100;
                        }
                    }
                    else if (item.PaymentAmount < (item.ContractAmount * (decimal?)0.3))
                    {
                        item.DepartmentIdAmount = (item.ContractAmount * (decimal?)0.3) - item.PaymentAmount;
                        if (item.DepartmentIdAmount != null)
                        {
                            item.DepartmentIdAmount = Math.Round((decimal)item.DepartmentIdAmount * 100) / 100;
                        }
                    }
                    else
                    {
                        item.DepartmentIdAmount = item.ContractAmount * (decimal?)0.03;
                        if (item.DepartmentIdAmount != null)
                        {
                            item.DepartmentIdAmount = Math.Round((decimal)item.DepartmentIdAmount * 100) / 100;
                        }
                    }
                }
                //渠道营销
                if (item.ProjectSource == "2")
                {
                    item.DepartmentIdAmount = item.ContractAmount * (decimal?)0.03;
                    if (item.DepartmentIdAmount != null)
                    {
                        item.DepartmentIdAmount = Math.Round((decimal)item.DepartmentIdAmount * 100) / 100;
                    }
                }
                //生产核算        
                if (item.ProjectSource == "1" || item.ProjectSource == "2")
                {
                    if (item.TaskStatus == "3" || item.TaskStatus == "4" || item.TaskStatus == "9" || item.TaskStatus == "11" || item.TaskStatus == "5")
                    {
                        if (item.PaymentAmount != null)
                        {
                            if (item.PaymentAmount >= (item.ContractAmount * (decimal?)0.3))
                            {
                                if (item.SubAmount != null && item.MainAmount == null)
                                {
                                    item.DepartmentIdAmount1 = (item.ContractAmount - item.PaymentAmount - item.SubAmount) * (decimal?)0.3;
                                    item.DepartmentIdAmount1 = Math.Round((decimal)item.DepartmentIdAmount1 * 100) / 100;
                                    // item.DepartmentIdAmount = decimal.Round(decimal.Parse(((item.ContractAmount - item.PaymentAmount - item.SubAmount) * (decimal?)0.3).ToString()), 2);
                                }
                                else if (item.SubAmount == null && item.MainAmount != null)
                                {
                                    item.DepartmentIdAmount1 = (item.ContractAmount - item.PaymentAmount - item.MainAmount) * (decimal?)0.3;
                                    item.DepartmentIdAmount1 = Math.Round((decimal)item.DepartmentIdAmount1 * 100) / 100;
                                    //item.DepartmentIdAmount = decimal.Round(decimal.Parse(((item.ContractAmount - item.PaymentAmount - item.MainAmount) * (decimal?)0.3).ToString()),2);
                                }
                                else
                                {
                                    item.DepartmentIdAmount1 = (item.ContractAmount - item.PaymentAmount) * (decimal?)0.3;
                                    item.DepartmentIdAmount1 = Math.Round((decimal)item.DepartmentIdAmount1 * 100) / 100;
                                    //item.DepartmentIdAmount = decimal.Round(decimal.Parse(((item.ContractAmount - item.PaymentAmount) * (decimal?)0.3).ToString()), 2);
                                }
                            }
                            else if (item.PaymentAmount < (item.ContractAmount * (decimal?)0.3))
                            {
                                item.DepartmentIdAmount1 = item.ContractAmount * (decimal?)0.2;
                                item.DepartmentIdAmount1 = Math.Round((decimal)item.DepartmentIdAmount1 * 100) / 100;
                                //item.DepartmentIdAmount = decimal.Round(decimal.Parse((item.ContractAmount * (decimal?)0.2).ToString()), 2);
                            }
                        }
                        else
                        {
                            if (item.ContractAmount != null)
                            {
                                item.DepartmentIdAmount1 = (item.ContractAmount * (decimal?)0.2);
                                item.DepartmentIdAmount1 = Math.Round((decimal)item.DepartmentIdAmount1 * 100) / 100;
                                //item.DepartmentIdAmount = decimal.Round(decimal.Parse((item.ContractAmount * (decimal?)0.2).ToString()), 2);
                            }
                        }


                    }






                }
                //if (item.ProjectSource == "1" || item.ProjectSource == "2")
                //{
                //    if (item.TaskStatus == "3" || item.TaskStatus == "4" || item.TaskStatus == "9" || item.TaskStatus == "11" || item.TaskStatus == "5")
                //    {
                //        if (item.PaymentAmount >= (item.ContractAmount * (decimal?)0.3))
                //        {
                //            item.DepartmentIdAmount1 = (item.ContractAmount - item.PaymentAmount - item.SubAmount - item.MainAmount) * (decimal?)0.3;
                //            if (item.DepartmentIdAmount1 != null)
                //            {
                //                item.DepartmentIdAmount1 = Math.Round((decimal)item.DepartmentIdAmount1 * 100) / 100;
                //            }
                //        }
                //        else
                //        {
                //            item.DepartmentIdAmount1 = item.ContractAmount * (decimal?)0.2;
                //            if (item.DepartmentIdAmount1 != null)
                //            {
                //                item.DepartmentIdAmount1 = Math.Round((decimal)item.DepartmentIdAmount1 * 100) / 100;
                //            }
                //        }


                //    }
                //}
                DataItemDetailEntity contractSubject = dataItemBLL.GetDetailItemName(item.ContractSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    item.ContractSubject = contractSubject.F_ItemName;
                }

                DataItemDetailEntity projectSource = dataItemBLL.GetDetailItemName(item.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    item.ProjectSource = projectSource.F_ItemName;
                }

                DataItemDetailEntity taskStatus = dataItemBLL.GetDetailItemName(item.TaskStatus, "TaskStatus");
                if (taskStatus != null)
                {
                    item.TaskStatus = taskStatus.F_ItemName;
                }
                if (item.DepartmentId != null && item.DepartmentId.IndexOf(",") == -1)
                {
                    var department = departmentIBLL.GetEntity(item.DepartmentId);
                    if (department != null)
                    {
                        item.DepartmentId = department.F_FullName;
                    }
                    else
                    {
                        item.DepartmentId = null;
                    }
                }
                else
                {
                    if (item.DepartmentId != null)
                    {
                        string[] array = item.DepartmentId.Split(new char[] { ',' });
                        foreach (var dept in array)
                        {
                            var department = departmentIBLL.GetEntity(dept);
                            if (department != null)
                            {
                                item.DepartmentId = department.F_FullName;
                            }
                            else
                            {
                                item.DepartmentId = null;
                            }
                        }
                    }
                    else
                    {
                        item.DepartmentId = null;
                    }

                }
                if (item.J_F_FullName != null && item.J_F_FullName.IndexOf(",") == -1)
                {
                    var jfdepartment = departmentIBLL.GetEntity(item.J_F_FullName);
                    if (jfdepartment != null)
                    {
                        item.J_F_FullName = jfdepartment.F_FullName;
                    }
                    else
                    {
                        item.J_F_FullName = null;
                    }
                }
                else
                {
                    if (item.J_F_FullName != null)
                    {
                        string[] array = item.J_F_FullName.Split(new char[] { ',' });
                        foreach (var dept in array)
                        {
                            var department = departmentIBLL.GetEntity(dept);
                            if (department != null)
                            {
                                item.J_F_FullName = department.F_FullName;
                            }
                            else
                            {
                                item.J_F_FullName = null;
                            }
                        }

                    }
                    else
                    {
                        item.J_F_FullName = null;
                    }



                }

            }
            rowData = marketings.ToJson().ToTable();
            //写入Excel表头
            Dictionary<string, string> exportFieldMap = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(exportField))
            {
                string[] exportFields = exportField.Split(',');
                foreach (var field in exportFields)
                {
                    if (!exportFieldMap.ContainsKey(field))
                    {
                        exportFieldMap.Add(field, "1");
                    }
                }
            }

            foreach (jfGridModel columnModel in columnList)
            {
                if (exportFieldMap.ContainsKey(columnModel.name) || string.IsNullOrEmpty(exportField))
                {
                    excelconfig.ColumnEntity.Add(new ColumnModel()
                    {
                        Column = columnModel.name,
                        ExcelColumn = columnModel.label,
                        Alignment = columnModel.align,
                        Width = columnModel.name.Length
                    });
                }
            }
            ExcelHelper.ExcelDownload(rowData, excelconfig);
        }

        public static List<T> TableToEntity<T>(DataTable dt) where T : class, new()
        {
            // 定义集合  
            List<T> ts = new List<T>();

            if (dt != null && dt.Rows.Count > 0)
            {
                // 获得此模型的类型  
                Type type = typeof(T);
                string tempName = "";
                foreach (DataRow dr in dt.Rows)
                {
                    T t = new T();
                    // 获得此模型的公共属性  
                    PropertyInfo[] propertys = t.GetType().GetProperties();
                    foreach (PropertyInfo pi in propertys)
                    {
                        tempName = pi.Name;
                        // 检查DataTable是否包含此列  
                        if (dt.Columns.Contains(tempName))
                        {
                            // 判断此属性是否有Setter  
                            if (!pi.CanWrite)
                                continue;
                            object value = dr[tempName];
                            if (value != DBNull.Value)
                            {
                                //pi.SetValue(t, value, null);  
                                // pi.SetValue(t, Convert.ChangeType(value, pi.PropertyType, CultureInfo.CurrentCulture), null);
                                pi.SetValue(t, ChanageType(value, pi.PropertyType), null);
                            }
                        }
                    }
                    ts.Add(t);
                }
            }

            return ts;
        }
        //转换可空类型 如：DateTime? 
        private static object ChanageType(object value, Type convertsionType)
        {
            //判断convertsionType类型是否为泛型，因为nullable是泛型类,
            if (convertsionType.IsGenericType &&
                //判断convertsionType是否为nullable泛型类
                convertsionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null || value.ToString().Length == 0)
                {
                    return null;
                }

                //如果convertsionType为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                NullableConverter nullableConverter = new NullableConverter(convertsionType);
                //将convertsionType转换为nullable对的基础基元类型
                convertsionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, convertsionType);
        }
        #endregion

        #region 列表选择弹层
        /// <summary>
        /// 列表选择弹层
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerLogin(FilterMode.Enforce)]
        public ActionResult GirdSelectIndex()
        {
            return View();
        }
        #endregion

        #region 树形选择弹层
        /// <summary>
        /// 列表选择弹层
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerLogin(FilterMode.Enforce)]
        public ActionResult TreeSelectIndex()
        {
            return View();
        }
        #endregion

        #region 加载js和css文件
        /// <summary>
        /// 列表选择弹层
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult JsCss(string plugins)
        {
            Dictionary<string, JssCssModel> list = new Dictionary<string, JssCssModel>();
            string[] pluginArray = plugins.Split(',');
            foreach (var item in pluginArray)
            {
                GetJssCss(item, list);
            }
            return Success(list);
        }
        /// <summary>
        /// 获取js和css文件值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="list"></param>
        private void GetJssCss(string name, Dictionary<string, JssCssModel> list)
        {
            JssCssModel model = new JssCssModel();
            switch (name)
            {
                case "jquery":
                    model.js = JsCssHelper.Read("/Content/jquery/jquery-1.10.2.min.js");
                    break;
                case "cookie":
                    model.js = JsCssHelper.Read("/Content/jquery/plugin/jquery.cookie.min.js");
                    break;
                case "md5":
                    model.js = JsCssHelper.Read("/Content/jquery/jquery.md5.min.js");
                    break;
                case "scrollbar":
                    model.css = JsCssHelper.Read("/Content/jquery/plugin/scrollbar/jquery.mCustomScrollbar.min.css");
                    model.js = JsCssHelper.Read("/Content/jquery/plugin/scrollbar/jquery.mCustomScrollbar.concat.min.js");
                    break;
                case "toastr":
                    model.css = JsCssHelper.Read("/Content/jquery/plugin/toastr/toastr.css");
                    model.js = JsCssHelper.Read("/Content/jquery/plugin/toastr/toastr.min.js");
                    break;
                case "bootstrap":
                    model.css = JsCssHelper.Read("/Content/bootstrap/bootstrap.min.css");
                    model.js = JsCssHelper.Read("/Content/bootstrap/bootstrap.min.js");
                    break;
                case "layer":
                    model.css = JsCssHelper.Read("/Content/bootstrap/bootstrap.min.css");
                    model.js = JsCssHelper.Read("/Content/bootstrap/bootstrap.min.js");
                    break;
                case "jqprint":
                    break;
                case "wdatePicker":
                    break;
                case "syntaxhighlighter":
                    break;

                case "fontAwesome":
                    break;
                case "iconfont":
                    break;

                case "common":
                    break;
                case "base":
                    break;
                case "tabs":
                    break;
                case "date":
                    break;
                case "validator-helper":
                    break;
                case "lrlayer":
                    break;
                case "ajax":
                    break;
                case "clientdata":
                    break;
                case "iframe":
                    break;
                case "validator":
                    break;
                case "layout":
                    break;
                case "tree":
                    break;
                case "select":
                    break;
                case "formselect":
                    break;
                case "layerselect":
                    break;
                case "jfgrid":
                    break;
                case "wizard":
                    break;
                case "timeline":
                    break;
                case "datepicker":
                    break;
                case "uploader":
                    break;
                case "excel":
                    break;
                case "authorize":
                    break;
                case "custmerform":
                    break;
                case "workflow":
                    break;
                case "form":
                    break;

            }
        }
        private class JssCssModel
        {
            /// <summary>
            /// js 代码
            /// </summary>
            public string js { get; set; }
            /// <summary>
            /// css 代码
            /// </summary>
            public string css { get; set; }
            /// <summary>
            /// 版本号
            /// </summary>
            public string ver { get; set; }
        }

        #endregion

        #region 自定义表单
        /// <summary>
        /// 表单预览
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PreviewForm()
        {
            return View();
        }
        /// <summary>
        /// 编辑表格
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditGridForm()
        {
            return View();
        }

        #endregion

        #region jfgrid弹层选择
        /// <summary>
        /// 列表选择弹层
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerLogin(FilterMode.Enforce)]
        public ActionResult JfGirdLayerForm()
        {
            return View();
        }
        #endregion

        #region 桌面消息列表详情查看
        /// <summary>
        /// 桌面消息列表详情查看
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ListContentIndex()
        {
            return View();
        }
        #endregion

        #region 开发向导
        /// <summary>
        /// pc端开发向导
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerLogin(FilterMode.Enforce)]
        public ActionResult PCDevGuideIndex()
        {
            return View();
        }

        /// <summary>
        /// 移动端开发向导
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerLogin(FilterMode.Enforce)]
        public ActionResult AppDevGuideIndex()
        {
            return View();
        }
        #endregion

        #region 单点登录
        // 
        #endregion


        /// <summary>
        /// pc端开发向导
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerLogin(FilterMode.Ignore)]
        public ActionResult TopIndex()
        {
            return View();
        }

    }
}