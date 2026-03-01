using Learun.Util;
using System.Collections.Generic;

namespace Learun.Application.Organization
{
    /// <summary>
    /// 日 期：2017.04.17
    /// 描 述：部门管理
    /// </summary>
    public interface DepartmentIBLL
    {
        #region 获取数据
        /// <summary>
        /// 获取部门列表信息(根据公司Id)
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <returns></returns>
        List<DepartmentEntity> GetList(string companyId);
        /// <summary>
        /// 获取部门列表信息(根据公司Id)
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="keyWord">查询关键字</param>
        /// <returns></returns>
        List<DepartmentEntity> GetList(string companyId, string keyWord);
        List<DepartmentEntity> GetList1(string companyId, string keyWord);
        IEnumerable<DepartmentEntity> GetList_zijin(string keyValue);

        /// <summary>
        /// 获取部门数据实体
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        DepartmentEntity GetEntity(string keyValue);
        /// <summary>
        /// 部门数据实体
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        List<DepartmentEntity> GetEntityList();
        /// <summary>
        /// 获取部门数据实体
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        IEnumerable<DepartmentEntity> GetPageList(Pagination pagination, string queryJson);
        IEnumerable<DepartmentEntity> GetPageListHZ();
        IEnumerable<DepartmentEntity> GetPageList2();
        IEnumerable<DepartmentEntity> GetPageListHZ2();

        /// <summary>
        /// 获取部门数据实体(不包含合作伙伴)
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        DepartmentEntity GetEntityHZ(string keyValue);
        /// <summary>
        /// 获取部门数据实体
        /// </summary>
        /// <param name="companyId">公司主键</param>
        /// <param name="departmentId">部门主键</param>
        /// <returns></returns>
        DepartmentEntity GetEntity(string companyId, string departmentId);
        /// <summary>
        /// 获取树形数据
        /// </summary>
        /// <param name="companyId">公司id</param>
        /// <param name="parentId">父级id</param>
        /// <returns></returns>
        List<TreeModel> GetTree(string companyId, string parentId);
        /// <summary>
        /// 获取树形数据(不包含合作伙伴)
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        List<TreeModel> GetTreeHZ(string companyId, string parentId);
        /// <summary>
        /// 获取树形数据
        /// </summary>
        /// <param name="companyId">公司id</param>
        /// <param name="parentId">父级id</param>
        /// <returns></returns>
        List<TreeModel> GetTree(List<CompanyEntity> companylist);
        /// <summary>
        /// 获取树形数据(不包含合作伙伴)
        /// </summary>
        /// <param name="companylist"></param>
        /// <returns></returns>
        List<TreeModel> GetTreeHZ(List<CompanyEntity> companylist);
        /// <summary>
        /// 获取部门本身和子部门的id
        /// </summary>
        /// <param name="parentId">父级ID</param>
        /// <returns></returns>
        List<string> GetSubNodes(string companyId, string parentId);
        /// <summary>
        /// 获取部门映射数据
        /// </summary>
        /// <returns></returns>
        Dictionary<string, DepartmentModel> GetModelMap();
        /// <summary>
        /// 多部门
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<DepartmentEntity> GetUserDepartmentId(string userId);
        /// <summary>
        /// 根据id获取部门
        /// </summary>
        /// <param name="depId"></param>
        /// <returns></returns>
        DepartmentEntity GetDepartmentId(string depId);
        #endregion

        #region 提交数据
        /// <summary>
        /// 虚拟删除部门信息
        /// </summary>
        /// <param name="keyValue">主键</param>
        void VirtualDelete(string keyValue);
        /// <summary>
        /// 保存部门信息（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="departmentEntity">部门实体</param>
        /// <returns></returns>
        void SaveEntity(string keyValue, DepartmentEntity departmentEntity);
        void GetDepartmentNametByUserId(string departmentId);
        #endregion
    }
}
