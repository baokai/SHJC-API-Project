
using Learun.Application.Organization.User;
using Learun.Util;
using System.Collections.Generic;
namespace Learun.Application.Organization
{
    /// <summary>
    /// 日 期：2017.03.06
    /// 描 述：用户模块业务类(接口)
    /// </summary>
    public interface UserIBLL
    {

        #region 获取数据
        /// <summary>
        /// 用户列表(根据公司主键)
        /// </summary>
        /// <param name="companyId">公司主键</param>
        /// <returns></returns>
        List<UserEntity> GetList(string companyId);
        /// <summary>
        /// 用户列表(根据公司主键,部门主键)
        /// </summary>
        /// <param name="companyId">公司主键</param>
        /// <param name="departmentId">部门主键</param>
        /// <param name="keyword">查询关键词</param>
        /// <returns></returns>
        List<UserEntity> GetList(string companyId, string departmentId, string keyword);
        /// <summary>
        /// 用户列表(全部)
        /// </summary>
        /// <returns></returns>
        List<UserEntity> GetAllList();
        /// <summary>
        /// 用户列表(根据部门主键)
        /// </summary>
        /// <param name="departmentId">部门主键</param>
        /// <returns></returns>
        List<UserEntity> GetListByDepartmentId(string departmentId);
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="companyId">公司主键</param>
        /// <param name="departmentId">部门主键</param>
        /// <param name="pagination">分页参数</param>
        /// <param name="keyword">查询关键词</param>
        /// <returns></returns>
        List<UserEntity> GetPageList(string companyId, string departmentId, Pagination pagination, string keyword);
        /// <summary>
        /// 用户列表（导出Excel）
        /// </summary>
        /// <returns></returns>
        void GetExportList();
         /// <summary>
        /// 获取实体,通过用户账号
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <returns></returns>
        UserEntity GetEntityByAccount(string account);
        /// <summary>
        /// 获取用户数据
        /// </summary>
        /// <param name="userId">用户主键</param>
        /// <returns></returns>
        UserEntity GetEntityByUserId(string userId);
        /// <summary>
        /// 根据用户id获取对于的部门信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        DepartmentEntity GetDepartmentByUserId(string userId);
       
        /// <summary>
        /// 根据用户id获取销售人员名字
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserEntity GetFollowPersonNameByUserId(string userId);

        /// <summary>
        /// 获取用户列表数据
        /// </summary>
        /// <param name="userIds">用户主键串</param>
        /// <returns></returns>
        List<UserEntity> GetListByUserIds(string userIds);
        /// <summary>
        /// 获取用户列表数据，不包括合作伙伴
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        List<UserEntity> GetListByUserIdsHZ(string userIds);
        /// <summary>
        /// 获取映射数据
        /// </summary>
        /// <returns></returns>
        Dictionary<string, UserModel> GetModelMap();
        /// <summary>
        /// 获取超级管理员用户列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserEntity> GetAdminList();
        /// <summary>
        /// 根据账号查用户
        /// </summary>
        /// <param name="Accoun"></param>
        /// <returns></returns>
        UserEntity GetAccountName(string Accoun);
        /// <summary>
        /// 根据名字查id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        UserEntityVo GetInspectorName(string name);
        DepartmentEntity GetDepartmentNameList(string userId);

        #endregion

        #region 提交数据
        /// <summary>
        /// 虚拟删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        void VirtualDelete(string keyValue);
        /// <summary>
        /// 保存用户表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="userEntity">用户实体</param>
        /// <returns></returns>
        void SaveEntity(string keyValue, UserEntity userEntity);

        void SaveNewEntity(string keyValue, UserEntity userEntity);        

        /// <summary>
        /// 多部门添加
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="MDepartmentId"></param>
        void SaveEntityList(string keyValue, string MDepartmentId);
        /// <summary>
        /// 合同主体显示设置
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="F_ItemValue"></param>
        void ContractSubjectList(string keyValue, string F_ItemValue);
         /// <summary>
         /// 修改用户登录密码
         /// </summary>
         /// <param name="newPassword">新密码（MD5 小写）</param>
         /// <param name="oldPassword">旧密码（MD5 小写）</param>
        bool RevisePassword(string newPassword, string oldPassword);
        /// <summary>
        /// 重置密码(000000)
        /// </summary>
        /// <param name="keyValue">账号主键</param>
        void ResetPassword(string keyValue);
        IEnumerable<UserEntity> GetListUser(string departmentId);

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="state">状态：1-启动；0-禁用</param>
        void UpdateState(string keyValue, int state);
        IEnumerable<DepartmentEntity> GetUserDepartmentId(string depId);
        /// <summary>
        /// 通过userId获取咨询类的审核人
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserEntity GetUserId(string userId);
        /// <summary>
        /// 通过userId获取多部门信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserEntityVo DepartmentIdListById(string userId);
        UserEntityVo GetHZUserId(string userId);

        UserEntityVo GetHZUserId_2(string userId);
        
        /// <summary>
        /// 获取查看外验列表的用户信息
        /// </summary>
        /// <returns></returns>
        List<UserEntity> GetProduceUserList();
        /// <summary>
        /// 根据用户名获取用户id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserEntityVo GetHZUserName(string userId);
        #endregion

        #region 验证数据
        /// <summary>
        /// 账户不能重复
        /// </summary>
        /// <param name="account">账户值</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        bool ExistAccount(string account, string keyValue);
        #endregion

        #region 扩展方法
        /// <summary>
        /// 验证登录 F_UserOnLine 0 不成功,1成功
        /// </summary>
        /// <param name="username">账号</param>
        /// <param name="password">密码 MD5 32位 小写</param>
        /// <returns></returns>
        UserEntity CheckLogin(string username, string password);
        UserEntity CheckLoginFromJava(string username);
        /// <summary>
        /// 获取用户头像
        /// </summary>
        /// <param name="userId">用户ID</param>
        void GetImg(string userId);
        void P_SaveForm(string[] dataList, string strEntity);
        void P_HandOver(string strEntity);
        
        #endregion
    }
}
