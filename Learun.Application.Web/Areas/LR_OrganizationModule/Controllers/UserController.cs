using Learun.Application.Base.AuthorizeModule;
using Learun.Application.Base.SystemModule;
using Learun.Application.Organization;
using Learun.Util;
using Learun.Util.Operat;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Learun.Application.Web.Areas.LR_OrganizationModule.Controllers
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.6 力软敏捷开发框架
    /// Copyright (c) 2013-2020 上海力软信息技术有限公司
    /// 创建人：力软-框架开发组
    /// 日 期：2017.03.09
    /// 描 述：用户管理控制器
    /// </summary>
    public class UserController : MvcControllerBase
    {
        private UserIBLL userIBLL = new UserBLL();
        private DepartmentIBLL departmentIBLL = new DepartmentBLL();
        private DataItemIBLL dataItemBLL = new DataItemBLL();
        private UserRelationIBLL userRelationIBLL = new UserRelationBLL();

        #region 获取视图
        /// <summary>
        /// 用户管理主页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 多部门添加页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MoreDepartmentIdAdd()
        {
            return View();
        }
        /// <summary>
        /// 合同主体已添加显示
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ContractSubjectForm()
        {
            return View();
        }

        /// <summary>
        /// 用户管理表单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form(string userId)
        {

            return View();
        }
        /// <summary>
        /// 合同主体显示
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DetailIndex()
        {

            return View();
        }
        /// <summary>
        /// 查询已存在多部门信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DepartmentIdForm(string userId)
        {

            return View();
        }

        /// <summary>
        /// 人员选择
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelectForm()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SelectFormHZ()
        {
            return View();
        }
        /// <summary>
        /// 人员选择
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelectOnlyForm()
        {
            return View();
        }



        /// <summary>
        /// 调岗
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Post_MobilizationForm()
        {
            return View();
        }

        #endregion

        #region 获取数据
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="keyword">关键字</param>
        /// <param name="companyId">公司主键</param>
        /// <param name="departmentId">部门主键</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageList(string pagination, string keyword, string companyId, string departmentId)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = userIBLL.GetPageList(companyId, departmentId, paginationobj, keyword);
            var jsonData = new
            {
                rows = data,
                total = paginationobj.total,
                page = paginationobj.page,
                records = paginationobj.records,
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="companyId">公司主键</param>
        /// <param name="departmentId">部门主键</param>
        /// <param name="keyword">查询关键词</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetList(string companyId, string departmentId, string keyword)
        {
            if (string.IsNullOrEmpty(companyId))
            {
                var department = departmentIBLL.GetEntity(departmentId);
                if (department != null)
                {
                    var data = userIBLL.GetList(department.F_CompanyId, departmentId, keyword);
                    foreach (var item in data)
                    {
                        item.F_DepartmentId = departmentId;
                    }
                    return Success(data);
                }
                else
                {
                    return Success(new List<string>());
                }
            }
            else
            {
                var data = userIBLL.GetList(companyId, departmentId, keyword);
                return Success(data);
            }
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="companyId">公司主键</param>
        /// <param name="departmentId">部门主键</param>
        /// <param name="keyword">查询关键词</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetListHZ(string companyId, string departmentId, string keyword)
        {
            if (string.IsNullOrEmpty(companyId))
            {
                var department = departmentIBLL.GetEntityHZ(departmentId);
                if (department != null)
                {
                    var data = userIBLL.GetList(department.F_CompanyId, departmentId, keyword);
                    foreach (var item in data)
                    {
                        item.F_DepartmentId = departmentId;
                    }
                    return Success(data);
                }
                else
                {
                    return Success(new List<string>());
                }
            }
            else
            {
                var data = userIBLL.GetList(companyId, departmentId, keyword);
                return Success(data);
            }
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="companyId">公司主键</param>
        /// <param name="departmentId">部门主键</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetListUser(string departmentId, string keyword)
        {
            var data = this.userIBLL.GetListUser(departmentId);
            List<UserEntity> listdata = new List<UserEntity>();
            foreach (var item in data)
            {
                if (item.F_UserId != keyword)
                {
                    listdata.Add(item);
                }
            }
            return Success(listdata);
        }
        /// <summary>
        /// 获取本部门的人员
        /// </summary>
        /// <param name="companyId">公司主键</param>
        /// <param name="departmentId">部门主键</param>
        /// <param name="keyword">查询关键词</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetMyDepartmentList()
        {
            UserInfo userinfo = LoginUserInfo.Get();
            var data = userIBLL.GetList(userinfo.companyId, userinfo.departmentId, "");
            return Success(data);
        }
        /// <summary>
        /// 获取用户信息列表
        /// </summary>
        /// <param name="userIds">用户主键串</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetListByUserIds(string keyValue)
        {
            var list = userIBLL.GetListByUserIds(keyValue);
            string text = "";
            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    text += ",";
                }
                text += item.F_RealName;
            }
            return SuccessString(text);
        }
        /// <summary>
        /// 获取用户信息列表
        /// </summary>
        /// <param name="userIds">用户主键串</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetEntityListByUserIds(string keyValue)
        {
            var list = userIBLL.GetListByUserIds(keyValue);
            return Success(list);
        }
        /// <summary>
        /// 获取用户信息列表(不包括合作伙伴)
        /// </summary>
        /// <param name="userIds">用户主键串</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetEntityListByUserIdsHZ(string keyValue)
        {
            var list = userIBLL.GetListByUserIdsHZ(keyValue);
            return Success(list);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userIds">用户主键</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetUserEntity(string userId)
        {
            var data = userIBLL.GetEntityByUserId(userId);
            return Success(data);
        }

        /// <summary>
        /// 获取用户部门信息
        /// </summary>
        /// <param name="userIds">用户主键</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetDepartmentEntity(string userId)
        {
            var data = userIBLL.GetDepartmentByUserId(userId);
            return Success(data);
        }

        /// <summary>
        /// 获取映射数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetMap(string ver)
        {
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
        /// 获取头像
        /// </summary>
        /// <param name="userId">用户主键</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetImg(string userId)
        {
            userIBLL.GetImg(userId);
            return Success("获取成功。");
        }

        /// <summary>
        /// 通过userId获取咨询类的审核人
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult GetUserId(string userId)
        {
            var data = userIBLL.GetUserId(userId);
            return Success(data);
        }
        /// <summary>
        /// 通过userId获取多部门信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DepartmentIdListById(string userId)
        {
            var data = userIBLL.DepartmentIdListById(userId);
            if (data.F_MoreDepartmentId != null)
            {
                string[] strList = data.F_MoreDepartmentId.Split(',');

                string projectId = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    var dep = departmentIBLL.GetEntity(strList[i]);
                    if (dep != null)
                    {
                        if (string.IsNullOrWhiteSpace(projectId))
                        {
                            projectId = dep.F_FullName;
                        }
                        else
                        {
                            projectId = projectId + "," + dep.F_FullName;
                        }
                    }
                    data.F_MoreDepartmentName = projectId;
                }
            }
            else
            {
                data.F_MoreDepartmentName = "暂时没有添加多部门!!!";
            }
            var jsonData = new
            {
                F_MoreDepartmentName = data.F_MoreDepartmentName

            };
            return Success(jsonData);
        }
        /// <summary>
        /// 通过userId获取合同主体
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ContractSubjectListById(string userId)
        {
            var data = userIBLL.DepartmentIdListById(userId);
            if (data.F_ItemValue != null)
            {
                string[] strList = data.F_ItemValue.Split(',');

                string projectId = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    //合同主体
                    var dep = dataItemBLL.GetDetailItemName(strList[i], "ContractSubject");


                    if (dep != null)
                    {
                        if (string.IsNullOrWhiteSpace(projectId))
                        {
                            projectId = dep.F_ItemName;
                        }
                        else
                        {
                            projectId = projectId + "," + dep.F_ItemName;
                        }
                    }
                    data.F_ItemValue = projectId;
                }
            }
            else
            {
                data.F_ItemValue = "暂时没有添加显示合同主体!!!";
            }
            var jsonData = new
            {
                F_ItemValue = data.F_ItemValue

            };
            return Success(jsonData);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 保存表单数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, UserEntity entity)
        {
            try
            {
                var name = userIBLL.GetAccountName(entity.F_Account);
                if (name != null && keyValue == "")
                {
                    return Fail("账号重复");
                }
                userIBLL.SaveEntity(keyValue, entity);
                return Success("保存成功！", "用户管理", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, entity.F_UserId, entity.ToJson());
            }
            catch (System.Exception)
            {
                return Fail("账号不能重复");
            }

        }
        /// <summary>
        /// 合同主体显示设置
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>

        public ActionResult ContractSubjectList(string keyValue, string F_ItemValue)
        {

            userIBLL.ContractSubjectList(keyValue, F_ItemValue);


            if (F_ItemValue == "")
            {
                return Success("清除成功！", "合同主体显示设置", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, F_ItemValue);
            }
            return Success("保存成功！", "合同主体显示设置", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, F_ItemValue);


        }

        /// <summary>
        /// 多部门保存表单数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveFormList(string keyValue, string strEntity)
        {
            UserEntity entity = strEntity.ToObject<UserEntity>();


            userIBLL.SaveEntityList(keyValue, entity.F_MoreDepartmentId);


            if (entity.F_MoreDepartmentId == "")
            {
                return Success("清除成功！", "用户多部门", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, strEntity);
            }
            return Success("保存成功！", "用户多部门", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, keyValue);


        }


        /// <summary>
        /// 调岗
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult P_SaveForm(string keyValue, string strEntity, string[] dataList)
        {
            try
            {
                /*UserEntity userEntity = strEntity.ToObject<UserEntity>();
                UserEntityVo userEntityVo=strEntity.ToObject<UserEntityVo>();
                //保存
                userEntity.F_DepartmentId = userEntityVo.P_F_DepartmentId;
                userIBLL.SaveEntity(keyValue, userEntity);

                foreach(string item in dataList)
                {
                    userIBLL.P_SaveForm(item,strEntity);
                }*/
                userIBLL.P_SaveForm(dataList, strEntity);
                return Success("保存成功！", "调岗", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, keyValue);
            }
            catch (System.Exception)
            {
                return Fail("账号不能重复");
            }

        }


        /// <summary>
        /// 获取用户主键列表信息
        /// </summary>
        /// <param name="objectId">用户主键</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetUserIdList(string objectId)
        {
            var data = userRelationIBLL.GetUserIdList(objectId);
            string userIds = "";
            foreach (var item in data)
            {
                if (userIds != "")
                {
                    userIds += ",";
                }
                userIds += item.F_UserId;
            }
            var userList = userIBLL.GetListByUserIds(userIds);
            var datajson = new
            {
                userIds = userIds,
                userInfoList = userList
            };
            return Success(datajson);
        }


        /// <summary>
        /// 删除表单数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult DeleteForm(string keyValue)
        {
            userIBLL.VirtualDelete(keyValue);
            return Success("删除成功！", "用户管理", OperationType.Delete, keyValue, keyValue);
        }
        /// <summary>
        /// 启用禁用账号
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult UpdateState(string keyValue, int state)
        {
            userIBLL.UpdateState(keyValue, state);
            return Success("保存成功！", "启用禁用账号", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, keyValue);
        }
        /// <summary>
        /// 重置用户账号密码
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult ResetPassword(string keyValue)
        {
            userIBLL.ResetPassword(keyValue);
            return Success("保存成功！", "重置用户账号密码", string.IsNullOrEmpty(keyValue) ? OperationType.Create : OperationType.Update, keyValue, keyValue);
        }
        #endregion

        #region 数据导出
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExportUserList()
        {
            userIBLL.GetExportList();
            return Success("导出成功。");
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 账号不能重复
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="F_Account">账号</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult ExistAccount(string keyValue, string F_Account)
        {
            bool res = userIBLL.ExistAccount(F_Account, keyValue);
            return Success(res);
        }
        #endregion
    }
}
