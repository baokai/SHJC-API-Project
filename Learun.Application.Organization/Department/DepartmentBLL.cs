using Learun.Cache.Base;
using Learun.Cache.Factory;
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
    public class DepartmentBLL : DepartmentIBLL
    {
        #region 属性
        private DepartmentService departmentService = new DepartmentService();
        #endregion

        #region 缓存定义
        private ICache cache = CacheFactory.CaChe();
        private string cacheKey = "learun_adms_department_"; // +加公司主键
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取部门列表信息(根据公司Id)
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <returns></returns>
        public List<DepartmentEntity> GetList(string companyId)
        {
            try
            {
                List<DepartmentEntity> list = cache.Read<List<DepartmentEntity>>(cacheKey + companyId, CacheId.department);
                if (list == null)
                {
                    list = (List<DepartmentEntity>)departmentService.GetList(companyId);
                    cache.Write<List<DepartmentEntity>>(cacheKey + companyId, list, CacheId.department);
                }
                return list;
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
        /// 获取部门列表信息(根据公司Id)(不包括合作伙伴)
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <returns></returns>
        public List<DepartmentEntity> GetListHZ(string companyId)
        {
            try
            {
                List<DepartmentEntity> list = cache.Read<List<DepartmentEntity>>(cacheKey + companyId, CacheId.department);
                List<DepartmentEntity> dlist = new List<DepartmentEntity>();
                if (list == null)
                {
                    list = (List<DepartmentEntity>)departmentService.GetListHZ(companyId);
                    cache.Write<List<DepartmentEntity>>(cacheKey + companyId, list, CacheId.department);
                }
                foreach(var q in list)
                {

                    if (q.HZ_DepartmentId != 1)
                    {
                        dlist.Add(q);
                    }
                }
                return dlist;
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
        /// 获取部门列表信息(根据公司Id)
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="keyWord">查询关键字</param>
        /// <returns></returns>
        public List<DepartmentEntity> GetList(string companyId, string keyWord)
        {
            try
            {
                List<DepartmentEntity> list = GetList(companyId);

                if (!string.IsNullOrEmpty(keyWord))
                {
                    list = list.FindAll(t => t.F_FullName.Contains(keyWord) || t.F_EnCode.Contains(keyWord) || t.F_ShortName.Contains(keyWord));
                }
                return list;
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
        /// 获取部门列表信息-资金台账
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public IEnumerable<DepartmentEntity> GetList_zijin(string keyValue)
        {
            try
            {
                return departmentService.GetList_zijin(keyValue);
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
        public List<DepartmentEntity> GetList1(string companyId, string keyWord)
        {
            try
            {
                List<DepartmentEntity> list = GetList(companyId);

                if (keyWord != null && keyWord !="")
                {
                    list = list.FindAll(t => t.F_FullName.Contains(keyWord) || t.F_EnCode.Contains(keyWord) || t.F_ShortName.Contains(keyWord));
                }
                return list;
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
        /// 获取部门数据实体(不包括合作伙伴)
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public DepartmentEntity GetEntityHZ(string keyValue) {
            try
            {
                return departmentService.GetEntityHZ(keyValue);
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
        public DepartmentEntity GetEntity(string keyValue) {
            try
            {
                return departmentService.GetEntity(keyValue);
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
                return departmentService.GetEntityList();
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
        public IEnumerable<DepartmentEntity> GetPageList(Pagination pagination, string queryJson) { 
            try
            {
                return departmentService.GetPageList(pagination, queryJson);
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
        public IEnumerable<DepartmentEntity> GetPageListHZ() { 
            try
            {
                return departmentService.GetPageListHZ();
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
        public IEnumerable<DepartmentEntity> GetPageList2() { 
            try
            {
                return departmentService.GetPageList2();
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
        }  public IEnumerable<DepartmentEntity> GetPageListHZ2() { 
            try
            {
                return departmentService.GetPageListHZ2();
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
        /// <param name="companyId">公司主键</param>
        /// <param name="departmentId">部门主键</param>
        /// <returns></returns>
        public DepartmentEntity GetEntity(string companyId, string departmentId) {
            try
            {
                return GetList(companyId).Find(t => t.F_DepartmentId.Equals(departmentId));
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
        /// 获取树形数据
        /// </summary>
        /// <param name="companyId">公司id</param>
        /// <param name="parentId">父级id</param>
        /// <returns></returns>
        public List<TreeModel> GetTree(string companyId,string parentId)
        {
            try
            {
                if (string.IsNullOrEmpty(companyId)) {// 如果公司主键没有的话，需要加载公司信息
                    return new List<TreeModel>();
                }

                List<DepartmentEntity> list = GetList(companyId);
                list = list.OrderBy(i => i.F_SortCode).ToList();
                List<TreeModel> treeList = new List<TreeModel>();
                foreach (var item in list) {
                    TreeModel node = new TreeModel
                    {
                        id = item.F_DepartmentId,
                        text = item.F_FullName,
                        value = item.F_DepartmentId,
                        showcheck = false,
                        checkstate = 0,
                        isexpand = true,
                        parentId = item.F_ParentId
                    };

                    treeList.Add(node);
                }
                return treeList.ToTree(parentId);
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
        /// 获取树形数据（不包括合作伙伴）
        /// </summary>
        /// <param name="companyId">公司id</param>
        /// <param name="parentId">父级id</param>
        /// <returns></returns>
        public List<TreeModel> GetTreeHZ(string companyId,string parentId)
        {
            try
            {
                if (string.IsNullOrEmpty(companyId)) {// 如果公司主键没有的话，需要加载公司信息
                    return new List<TreeModel>();
                }

                List<DepartmentEntity> list = GetListHZ(companyId);
                list = list.OrderBy(i => i.F_SortCode).ToList();
                List<TreeModel> treeList = new List<TreeModel>();
                foreach (var item in list) {
                    TreeModel node = new TreeModel
                    {
                        id = item.F_DepartmentId,
                        text = item.F_FullName,
                        value = item.F_DepartmentId,
                        showcheck = false,
                        checkstate = 0,
                        isexpand = true,
                        parentId = item.F_ParentId
                    };

                    treeList.Add(node);
                }
                return treeList.ToTree(parentId);
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
        /// 获取树形数据
        /// </summary>
        /// <param name="companylist">公司数据列表</param>
        /// <returns></returns>
        public List<TreeModel> GetTree(List<CompanyEntity> companylist)
        {
            try
            {
                List<TreeModel> treeList = new List<TreeModel>();
                foreach (var companyone in companylist)
                {
                    List<TreeModel> departmentTree = GetTreeHZ(companyone.F_CompanyId, "");
                    if (departmentTree.Count > 0)
                    {
                        TreeModel node = new TreeModel
                        {
                            id = companyone.F_CompanyId,
                            text = companyone.F_FullName,
                            value = companyone.F_CompanyId,
                            showcheck = false,
                            checkstate = 0,
                            isexpand = true,
                            parentId = "0",
                            hasChildren = true,
                            ChildNodes = departmentTree,
                            complete = true
                        };
                        treeList.Add(node);
                    }
                }
                return treeList;
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
        /// 获取树形数据(不包括合作伙伴)
        /// </summary>
        /// <param name="companylist">公司数据列表</param>
        /// <returns></returns>
        public List<TreeModel> GetTreeHZ(List<CompanyEntity> companylist)
        {
            try
            {
                List<TreeModel> treeList = new List<TreeModel>();
                foreach (var companyone in companylist)
                {
                    List<TreeModel> departmentTree = GetTreeHZ(companyone.F_CompanyId, "");
                    if (departmentTree.Count > 0)
                    {
                        TreeModel node = new TreeModel
                        {
                            id = companyone.F_CompanyId,
                            text = companyone.F_FullName,
                            value = companyone.F_CompanyId,
                            showcheck = false,
                            checkstate = 0,
                            isexpand = true,
                            parentId = "0",
                            hasChildren = true,
                            ChildNodes = departmentTree,
                            complete = true
                        };
                        treeList.Add(node);
                    }
                }
                return treeList;
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
        /// 获取部门本身和子部门的id
        /// </summary>
        /// <param name="parentId">父级ID</param>
        /// <returns></returns>
        public List<string> GetSubNodes(string companyId, string parentId)
        {
            try
            {
                if (string.IsNullOrEmpty(parentId) || string.IsNullOrEmpty(companyId))
                {
                    return new List<string>();
                }
                List<string> res = new List<string>();
                res.Add(parentId);
                List<TreeModel> list = GetTree(companyId,parentId);
                GetSubNodes(list, res);
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
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }
        /// <summary>
        /// 遍历树形数据获取全部子节点ID
        /// </summary>
        /// <param name="list">树形数据列表</param>
        /// <param name="ourList">输出数据列表</param>
        private void GetSubNodes(List<TreeModel> list, List<string> ourList)
        {
            foreach (var item in list)
            {
                ourList.Add(item.id);
                if (item.hasChildren)
                {
                    GetSubNodes(item.ChildNodes, ourList);
                }
            }
        }

        /// <summary>
        /// 获取部门映射数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, DepartmentModel> GetModelMap() {
            try
            {
                Dictionary<string, DepartmentModel> dic = cache.Read<Dictionary<string, DepartmentModel>>(cacheKey + "dic", CacheId.department);
                if (dic == null) {
                    dic = new Dictionary<string, DepartmentModel>();
                    var list = departmentService.GetAllList();
                    foreach (var item in list) {
                        DepartmentModel model = new DepartmentModel() {
                            companyId = item.F_CompanyId,
                            parentId = item.F_ParentId,
                            name= item.F_FullName
                        };
                        dic.Add(item.F_DepartmentId,model);
                        cache.Write(cacheKey + "dic", dic, CacheId.department);
                    }
                }
                return dic;
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
        /// 根据id获取部门
        /// </summary>
        /// <param name="depId"></param>
        /// <returns></returns>
            public DepartmentEntity GetDepartmentId(string depId)
        {
            try
            {
                return departmentService.GetDepartmentId(depId);
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
        ///多部门
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public List<DepartmentEntity> GetUserDepartmentId(string depId)
        {
            try
            {
                return departmentService.GetUserDepartmentId(depId);
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
        #endregion

        #region 提交数据
        /// <summary>
        /// 虚拟删除部门信息
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void VirtualDelete(string keyValue)
        {
            try
            {
                DepartmentEntity entity = GetEntity(keyValue);
                cache.Remove(cacheKey + entity.F_CompanyId, CacheId.department);
                cache.Remove(cacheKey + "dic", CacheId.department);

                departmentService.VirtualDelete(keyValue);
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
        /// 保存部门信息（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="departmentEntity">部门实体</param>
        /// <returns></returns>
        public void SaveEntity(string keyValue, DepartmentEntity departmentEntity)
        {
            try
            {
                //if(!keyValue.IsEmpty())
                //{
                //    cache.Remove(cacheKey + departmentEntity.F_CompanyId, CacheId.department);
                //    cache.Remove(cacheKey + "dic", CacheId.department);
                //}
                cache.Remove(cacheKey + departmentEntity.F_CompanyId, CacheId.department);
                cache.Remove(cacheKey + "dic", CacheId.department);
                departmentService.SaveEntity(keyValue, departmentEntity);
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

        public void GetDepartmentNametByUserId(string departmentId)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
