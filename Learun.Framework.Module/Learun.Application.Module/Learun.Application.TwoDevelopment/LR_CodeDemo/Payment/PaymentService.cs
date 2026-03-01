using Dapper;
using Learun.Application.Organization;
using Learun.Application.WorkFlow;
using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    public class PaymentService : RepositoryFactory
    {
        private NWFProcessIBLL nWFProcessIBLL = new NWFProcessBLL();
        private DepartmentIBLL departmentIBLL = new DepartmentBLL();
        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<PaymentEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT *");
                strSql.Append("  from  Payment a  WHERE 1=1");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( a.CreateTime >= @startTime AND a.CreateTime <= @endTime ) ");
                }


                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.Payee like '%{0}%' or a.PayType like '%{0}%' or a.ContractSubmitter like '%{0}%' or a.PaymentReason like '%{0}%' )", queryParam["keyword"].ToString()));
                }

                if (!queryParam["Payee"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.Payee like '%{0}%')", queryParam["Payee"].ToString()));
                }
                if (!queryParam["PayType"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (a.PayType = '{0}' )", queryParam["PayType"].ToString()));
                }
                if (!queryParam["ContractSubmitter"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ContractSubmitter ='{0}' )", queryParam["ContractSubmitter"].ToString()));
                }
                if (!queryParam["PaymentReason"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.PaymentReason like '%{0}%' )", queryParam["PaymentReason"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.PaymentAmount ='{0}' )", queryParam["PaymentAmount"].ToString()));
                }

                return this.BaseRepository("learunOAWFForm").FindList<PaymentEntity>(strSql.ToString(), dp, pagination);
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

        public IEnumerable<PaymentVo> GetPageList2(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT *");
                strSql.Append("  from  Payment a  WHERE 1=1");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( a.CreateTime >= @startTime AND a.CreateTime <= @endTime ) ");
                }


                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.Payee like '%{0}%' or a.PayType like '%{0}%' or a.ContractSubmitter like '%{0}%' or a.PaymentReason like '%{0}%' )", queryParam["keyword"].ToString()));
                }

                if (!queryParam["Payee"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.Payee like '%{0}%')", queryParam["Payee"].ToString()));
                }
                if (!queryParam["PayType"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.PayType = '{0}' )", queryParam["PayType"].ToString()));
                }
                if (!queryParam["ContractSubmitter"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ContractSubmitter ='{0}' )", queryParam["ContractSubmitter"].ToString()));
                }
                if (!queryParam["PaymentReason"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.PaymentReason like '%{0}%' )", queryParam["PaymentReason"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.PaymentAmount ='{0}' )", queryParam["PaymentAmount"].ToString()));
                }

                return this.BaseRepository("learunOAWFForm").FindList<PaymentVo>(strSql.ToString(), dp, pagination);
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


        public IEnumerable<PaymentVo> GetPageListAPI(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT a.*");
                strSql.Append("  from  Payment a  WHERE 1=1");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    DateTime start = DateTime.Parse(queryParam["StartTime"].ToString());
                    DateTime end = DateTime.Parse(queryParam["EndTime"].ToString());
                    strSql.Append(" AND ( a.CreateTime >= '" + start + "' AND a.CreateTime <= '" + end + "' ) ");

                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.Payee like '%{0}%' or a.PayType like '%{0}%' or a.ContractSubmitter like '%{0}%' or a.PaymentReason like '%{0}%' )", queryParam["keyword"].ToString()));
                }

                if (!queryParam["Payee"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.Payee like '%{0}%' )", queryParam["Payee"].ToString()));
                }
                if (!queryParam["PayType"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.PayType ='{0}' )", queryParam["PayType"].ToString()));
                }
                if (!queryParam["ContractSubmitter"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ContractSubmitter ='{0}' )", queryParam["ContractSubmitter"].ToString()));
                }
                if (!queryParam["PaymentReason"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.PaymentReason  like '%{0}%'  )", queryParam["PaymentReason"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.PaymentAmount ='{0}' )", queryParam["PaymentAmount"].ToString()));
                }

                return this.BaseRepository("learunOAWFForm").FindList<PaymentVo>(strSql.ToString(), dp, pagination);
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
        /// 多部门获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<PaymentVo> GetPageListDepartmentId(Pagination pagination, string queryJson, string dep)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT *");
                strSql.Append("  from  Payment a  WHERE 1=1");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( a.CreateTime >= @startTime AND a.CreateTime <= @endTime ) ");
                }

                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.Payee like '%{0}%' or a.PayType like '%{0}%' or a.ContractSubmitter like '%{0}%' or a.PaymentReason like '%{0}%' )", queryParam["keyword"].ToString()));
                }

                if (!queryParam["Payee"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.Payee like '%{0}%' )", queryParam["Payee"].ToString()));
                }
                if (!queryParam["PayType"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (a.PayType = '{0}' )", queryParam["PayType"].ToString()));
                }
                if (!queryParam["ContractSubmitter"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ContractSubmitter ='{0}' )", queryParam["ContractSubmitter"].ToString()));
                }
                if (!queryParam["PaymentReason"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.PaymentReason like '%{0}%'  )", queryParam["PaymentReason"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.PaymentAmount ='{0}' )", queryParam["PaymentAmount"].ToString()));
                }
                strSql.Append(string.Format(" AND ( " + dep + " )"));
                return this.BaseRepository("learunOAWFForm").FindList<PaymentVo>(strSql.ToString(), dp, pagination);
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
        /// 行政付款导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<PaymentVo> GetPageList(string queryJson, out string sql)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(" SELECT cs.F_RealName AS ContractSubmitter, ps.F_RealName AS PaymentSubmitter, a.CreateUser AS CreateUser, "
                    + " cs.F_RealName AS ContractSubmitterName, ps.F_RealName AS PaymentSubmitterName, cu.F_RealName AS CreateUserName, dept.F_FullName AS DepartmentName,  "
                    + " a.DepartmentId, a.CreateTime, a.Payee, a.PayeeBank, a.BankAccount, a.PaymentAmount, a.PayType, "
                    + "a.PaymentReason, a.PaymentStatus, a.PaymentMethod , a.WorkFlowId ");
                strSql.Append(" from  Payment a "
                        + " LEFT JOIN [adms706].[dbo].[lr_base_user] cs ON a.ContractSubmitter = cs.F_UserId "
                        + " left join [adms706].[dbo].[lr_base_user] ps on a.PaymentSubmitter = ps.F_UserId "
                        + " left join [adms706].[dbo].[lr_base_user] cu on a.CreateUser = cu.F_UserId "
                        + " left join [adms706].[dbo].[lr_base_department] dept on a.DepartmentId = dept.F_DepartmentId "
                    + " WHERE 1=1 ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    strSql.Append(" AND ( a.CreateTime >= '"+ queryParam["StartTime"] + "' AND a.CreateTime <= '"+ queryParam["EndTime"] + "' ) ");
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.Payee like '%{0}%' or a.PayType like '%{0}%' or a.ContractSubmitter like '%{0}%' or a.PaymentReason like '%{0}%' )", queryParam["keyword"].ToString()));
                }

                if (!queryParam["Payee"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.Payee like '%{0}%')", queryParam["Payee"].ToString()));
                }
                if (!queryParam["PayType"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND (a.PayType = '{0}' )", queryParam["PayType"].ToString()));
                }
                if (!queryParam["ContractSubmitter"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.ContractSubmitter ='{0}' )", queryParam["ContractSubmitter"].ToString()));
                }
                if (!queryParam["PaymentReason"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.PaymentReason like '%{0}%' )", queryParam["PaymentReason"].ToString()));
                }
                if (!queryParam["PaymentAmount"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.PaymentAmount ='{0}' )", queryParam["PaymentAmount"].ToString()));
                }
                //return this.BaseRepository("learunOAWFForm").FindList<PaymentVo>(strSql.ToString(), dp);
                //return this.BaseRepository("learunOAWFForm").FindList<PaymentVo>(strSql.ToString());
                return this.BaseRepository("learunOAWFForm").FindList_Export<PaymentVo>(strSql.ToString(),out sql);
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
        /// 多部门行政付款导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<PaymentVo> GetPageListDepartmentId(string queryJson, string dep)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT *");
                strSql.Append("  from  Payment a   WHERE 1=1");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    dp.Add("endTime", queryParam["EndTime"].ToDate(), DbType.DateTime);
                    strSql.Append(" AND ( a.CreateTime >= @startTime AND a.CreateTime <= @endTime ) ");
                }
                if (!queryParam["PaymentStatus"].IsEmpty())
                {
                    strSql.Append(string.Format(" AND ( a.PaymentStatus ='{0}' )", queryParam["PaymentStatus"].ToString()));
                }
                strSql.Append(string.Format(" AND ( " + dep + " )"));
                return this.BaseRepository("learunOAWFForm").FindList<PaymentVo>(strSql.ToString(), dp);
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
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public ProjectPaymentVo GetPreviewPayment(string keyValue)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                *
                ");
                strSql.Append("  from  Payment t ");

                strSql.Append("  and t.id='" + keyValue + "'");


                return this.BaseRepository("learunOAWFForm").FindList<ProjectPaymentVo>(strSql.ToString()).FirstOrDefault();

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
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public PaymentVo GetPreviewProjectPayment(string keyValue)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"t.* from Payment t ");
                strSql.Append("  where t.Id='" + keyValue + "'");


                return this.BaseRepository("learunOAWFForm").FindList<PaymentVo>(strSql.ToString()).FirstOrDefault();

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
        /// 获取ProjectPayment表实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public PaymentEntity GetProjectPaymentEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository("learunOAWFForm").FindEntity<PaymentEntity>(keyValue);
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
        /// 获取主表实体数据
        /// </summary>
        /// <param name="processId">流程实例ID</param>
        /// <returns></returns>
        public PaymentVo GetEntityByProcessId(string processId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT *");
                strSql.Append("  from  Payment ");
                strSql.Append("  WHERE  WorkFlowId='" + processId + "'");
                return this.BaseRepository("learunOAWFForm").FindList<PaymentVo>(strSql.ToString()).FirstOrDefault();
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
        public PaymentEntity GetPaymentEntityByProcessId(string processId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT *");
                strSql.Append("  from  Payment ");
                strSql.Append("  WHERE  WorkFlowId='" + processId + "'");
                return this.BaseRepository("learunOAWFForm").FindList<PaymentEntity>(strSql.ToString()).FirstOrDefault();
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
        

        /*/// <summary>
        /// 提交审批流
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="processId"></param>
        internal void UpdateFlowId(string keyValue, string processId)
        {
            try
            {
                var projectContract = this.BaseRepository("learunOAWFForm").FindEntity<PaymentEntity>(keyValue);
                projectContract.Modify(keyValue);
                projectContract.WorkFlowId = processId;
                projectContract.PaymentStatus = 2;
                this.BaseRepository("learunOAWFForm").Update(projectContract);
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
        }*/
        /*/// <summary>
        /// 变更
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="processId"></param>
        public void UpdateFlowIdStatus(string keyValue, string processId)
        {
            try
            {
                var projectPayment = this.BaseRepository("learunOAWFForm").FindEntity<PaymentEntity>(keyValue);
                projectPayment.PaymentStatus = 1;
                projectPayment.WorkFlowId = "";
                projectPayment.Create();
                this.BaseRepository("learunOAWFForm").Insert(projectPayment);
                projectPayment.Modify(keyValue);
                projectPayment.WorkFlowId = processId;
                projectPayment.PaymentStatus = 6;
                this.BaseRepository("learunOAWFForm").Update(projectPayment);

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
        }*/
        #endregion


        #region 提交数据
        /// <summary>
        /// 删除实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void DeleteEntity(string keyValue)
        {
            try
            {
                this.BaseRepository("learunOAWFForm").Delete<PaymentEntity>(t => t.Id == keyValue);
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
        /// 保存实体数据（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public void SaveEntity(string keyValue, PaymentEntity entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    this.BaseRepository("learunOAWFForm").Update(entity);
                }
                else
                {
                    entity.Create();
                    this.BaseRepository("learunOAWFForm").Insert(entity);
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
        /// 变更
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="processId"></param>
        public void UpdateFlowIdStatus(string keyValue, string processId)
        {
            try
            {
                var projectPayment = this.BaseRepository("learunOAWFForm").FindEntity<PaymentEntity>(keyValue);
                projectPayment.PaymentStatus = 1;
                projectPayment.WorkFlowId = "";
                projectPayment.Create();
                this.BaseRepository("learunOAWFForm").Insert(projectPayment);
                projectPayment.Modify(keyValue);
                projectPayment.WorkFlowId = processId;
                projectPayment.PaymentStatus = 6;
                this.BaseRepository("learunOAWFForm").Update(projectPayment);

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
        /// 提交审批流
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="processId"></param>
        internal void UpdateFlowId(string keyValue, string processId)
        {
            //var db = this.BaseRepository("learunOAWFForm").BeginTrans();
            try
            {
                var projectPayment = this.BaseRepository("learunOAWFForm").FindEntity<PaymentEntity>(keyValue);
                projectPayment.PaymentSubmitter = LoginUserInfo.Get().userId;

                UserInfo userInfo = LoginUserInfo.Get();
                projectPayment.Modify(keyValue);
                projectPayment.PaymentStatus = 2;

                //设置报告提交人
                projectPayment.ContractSubmitter = LoginUserInfo.Get().userId;

                //获取部门code
                var department = departmentIBLL.GetEntity(LoginUserInfo.Get().departmentId);
                if (department != null)
                {
                    projectPayment.ContractSubmitterDeptCode = department.F_EnCode;
                }


                bool createFlag = true;
                if (String.IsNullOrEmpty(processId))
                {
                    processId = Guid.NewGuid().ToString();
                    projectPayment.WorkFlowId = processId;
                }
                else
                {
                    createFlag = false;
                }
                this.BaseRepository("learunOAWFForm").Update(projectPayment);
                // db.Update(projectPayment);
                // db.Commit();
                var user = LoginUserInfo.Get().userId;
                var followPerson = BaseRepository().FindEntity<UserEntity>(user);
                var dept = BaseRepository().FindEntity<DepartmentEntity>(followPerson.F_DepartmentId);
                string schemeCode = "";
                if (!createFlag)
                {
                    string title = projectPayment.PaymentReason;
                    if (dept.HZ_DepartmentId == 1)
                    {
                        schemeCode = "Payment1";
                    }
                    else
                    {
                        schemeCode = "Payment";
                    }

                    int level = 1;
                    nWFProcessIBLL.CreateFlow(schemeCode, processId, title, level, null, userInfo);
                }
                else
                {
                    if (dept.HZ_DepartmentId == 1)
                    {
                        schemeCode = "Payment1";
                    }
                    else
                    {
                        schemeCode = "Payment";
                    }
                    int level = 1;
                    string title = projectPayment.PaymentReason;
                    string auditors = "";
                    nWFProcessIBLL.CreateFlow(schemeCode, processId, title, level, auditors, userInfo);
                }
            }
            catch (Exception ex)
            {
                //db.Rollback();
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
