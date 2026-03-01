using Learun.Util;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>

    /// 描 述：项目任务单
    /// </summary>
    public class ProjectTaskBLL : ProjectTaskIBLL
    {
        private ProjectTaskService projectTaskService = new ProjectTaskService();
        private ProjectContractService projectContractService = new ProjectContractService();

        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return projectTaskService.GetPageList(pagination, queryJson);
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
        public IEnumerable<ProjectTaskVo> getTaskForMatchPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return projectTaskService.getTaskForMatchPageList(pagination, queryJson);
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
        
        public IEnumerable<ProjectTaskVo> GetTaskLoadList(string queryJson, string userId, string departmentId, int type)
        {
            try
            {
                return projectTaskService.GetTaskLoadList(queryJson, userId, departmentId, type);
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
        public IEnumerable<ProjectTaskVo> GetHZPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return projectTaskService.GetHZPageList(pagination, queryJson);
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

        public IEnumerable<ProjectTaskEntity> GetAllFinishedTask()
        {
            return projectTaskService.GetAllFinishedTask();
        }
        public void ReMatchTaskAnfContract()
        {
            var taskList = projectTaskService.GetAllTaskToMatchContract();
            var contractList = projectContractService.GetAllContractFroMatch();
            foreach (var task in taskList)
            {
                var matcedContract = contractList.Where(i => i.Id == task.ProjectId).ToList();
                if(matcedContract.Count == 1)
                {
                    task.ContractId = matcedContract.FirstOrDefault().ContractId;
                    projectTaskService.SaveEntity(task.id, task);
                }
            }
        }

        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProducTionVo> GetQualityTechnologyImplement(string categoryId)
        {
            try
            {
                return projectTaskService.GetQualityTechnologyImplement(categoryId);
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
        /// 报告待检测
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetPageToBeDetectList(Pagination pagination, string queryJson)
        {
            try
            {
                return projectTaskService.GetPageToBeDetectList(pagination, queryJson);
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
        /// 报告待检测_首页
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetIndexToBeDetectList(List<string> deptIds, out int count)
        {
            try
            {
                return projectTaskService.GetIndexToBeDetectList(deptIds, out count);
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
        /// 待报告_首页
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetIndexToBeReportedList(List<string> deptIds, out int count)
        {
            try
            {
                return projectTaskService.GetIndexToBeReportedList(deptIds, out count);
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
        /// 超期项目_首页
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetIndexOverdueList(List<string> deptIds, out int count)
        {
            try
            {
                return projectTaskService.GetIndexOverdueList(deptIds, out count);
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
        /// 已完成_首页
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetIndexHaveCompletedList(List<string> deptIds, out int count)
        {
            try
            {
                return projectTaskService.GetIndexHaveCompletedList(deptIds, out count);
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
        /// 生产统计图
        /// </summary>           
        /// <returns></returns>
        public IEnumerable<ProducTionVo> GetProducTionList(string dep)
        {
            try
            {
                return projectTaskService.GetProducTionList(dep);
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
        /// 生产统计图超时数据/超时金额
        /// </summary>           
        /// <returns></returns>
        public IEnumerable<ProducTionVo> GetProducTionTimeoutList(string dep)
        {
            try
            {
                return projectTaskService.GetProducTionTimeoutList(dep);
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
        /// 多部门生产统计图超时数据/超时金额
        /// </summary>           
        /// <returns></returns>
        public IEnumerable<ProducTionVo> GetProducTionTimeoutListDepartmentId(string dep)
        {
            try
            {
                return projectTaskService.GetProducTionTimeoutListDepartmentId(dep);
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
        /// 多部门生产统计图
        /// </summary>           
        /// <returns></returns>
        public IEnumerable<ProducTionVo> GetProducTionListDepartmentId(string dep)
        {
            try
            {
                return projectTaskService.GetProducTionListDepartmentId(dep);
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
        /// 待报告
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetPageToBeReportedList(Pagination pagination, string queryJson)
        {
            try
            {
                return projectTaskService.GetPageToBeReportedList(pagination, queryJson);
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
        /// 已完成
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetPageHaveCompletedList(Pagination pagination, string queryJson)
        {
            try
            {
                return projectTaskService.GetPageHaveCompletedList(pagination, queryJson);
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
        /// 超期项目
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetPageOverdueItemList(Pagination pagination, string queryJson)
        {
            try
            {
                return projectTaskService.GetPageOverdueItemList(pagination, queryJson);
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
        /// 多部门获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetPageListDepartmentId(Pagination pagination, string queryJson, string dep)
        {
            try
            {
                return projectTaskService.GetPageListDepartmentId(pagination, queryJson, dep);
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
        /// 报告导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetPageList(string queryJson)
        {
            try
            {
                return projectTaskService.GetPageList(queryJson);
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
        /// 合作报告导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetHZTaskListAll(string queryJson)
        {
            try
            {
                return projectTaskService.GetHZPageList(queryJson);
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
        /// 多部门报告导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetPageListDepartmentId(string queryJson, string dep)
        {
            try
            {
                return projectTaskService.GetPageListDepartmentId(queryJson, dep);
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
        /// 根据负责人id查名字
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ProjectTaskVo GetFollowPersonNameByUserId(string queryJson, string id)
        {
            try
            {
                return projectTaskService.GetFollowPersonNameByUserId(queryJson, id);
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
        /// 根据报备id查询相关任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<ProjectTaskVo> GetProjectTaskList(string id)
        {
            try
            {
                return projectTaskService.GetProjectTaskList(id);
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
        public IEnumerable<TaskScheduleVo> GetScheduleList(ScheduleParam queryJson)
        {
            try
            {
                return projectTaskService.GetScheduleList(queryJson);
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
        /// 获取ProjectTask表实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public ProjectTaskEntity GetProjectTaskEntity(string keyValue)
        {
            try
            {
                return projectTaskService.GetProjectTaskEntity(keyValue);
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
        /// 获取最新的任务编号
        /// </summary>
        /// <param name="taskNo"></param>
        /// <returns></returns>
        public string GetNextProjectTaskNo(string taskNo)
        {
            try
            {
                return projectTaskService.GetNextProjectTaskNo(taskNo);
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
        public string GetNextProjectTaskNo_ZBG(string taskNo)
        {
            try
            {
                return projectTaskService.GetNextProjectTaskNo_ZBG(taskNo);
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
        /// 根据名字获取id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ProjectTaskVo GetProjectName(string name)
        {
            try
            {
                return projectTaskService.GetProjectName(name);
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
        /// 根据id修改状态
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ProjectTaskVo GetPageListName(string id)
        {
            try
            {
                return projectTaskService.GetPageListName(id);
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
        /// 根据id获取检测员
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ProjectTaskVo GetProjectTaskById(string keyValue)
        {
            try
            {
                return projectTaskService.GetProjectTaskById(keyValue);
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
        /// 获取预览数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public ProjectTaskVo GetPriewProjectTask(string keyValue)
        {
            try
            {
                return projectTaskService.GetPriewProjectTask(keyValue);
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
        /// 获取流程数据
        /// </summary>
        /// <param name="processId">主键</param>
        /// <returns></returns>
        public ProjectTaskVo GetPriewProjectTaskprojectId(string processId)
        {
            try
            {
                return projectTaskService.GetPriewProjectTaskprojectId(processId);
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
        /// 获取主表实体数据
        /// </summary>
        /// <param name="processId">流程实例ID</param>
        /// <returns></returns>
        public ProjectTaskEntity GetEntityByProcessId(string processId)
        {
            try
            {
                return projectTaskService.GetEntityByProcessId(processId);
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
        ///<summary>
        ///根据流程ID获取数据 
        /// </summary>
        /// <param name="processId">流程实例ID</param>
        /// <returns></returns>
        public ProjectTaskVo GetTaskByProcessId(string processId)
        {
            try
            {
                return projectTaskService.GetTaskByProcessId(processId);
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
        /// 报告上传
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void GetProjectTaskSc(string keyValue, ProjectTaskEntity entity)
        {
            try
            {
                projectTaskService.GetProjectTaskSc(keyValue, entity);
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
        /// 删除实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void DeleteEntity(string keyValue)
        {
            try
            {
                projectTaskService.DeleteEntity(keyValue);
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
        /// 根据用户id查询对于的任务单（计划时间）
        /// </summary>
        /// <returns></returns>
        public List<ProjectTaskVo> GetProjectTaskByInspectorAndPlanTime(string userId, DateTime startTime, DateTime dateTime)
        {
            try
            {
                return projectTaskService.GetProjectTaskByInspectorAndPlanTime(userId, startTime, dateTime);
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
        ///  根据用户id查询对于的任务单(实际时间)
        /// </summary>
        /// <returns></returns>
        public ProjectTaskVo GetProjectTaskByTime(string userId)
        {
            try
            {
                return projectTaskService.GetProjectTaskByTime(userId);
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
        ///  根据用户id查询对于的任务单(实际时间)
        /// </summary>
        /// <returns></returns>
        public List<ProjectTaskVo> GetProjectTaskByInspectorAndActualTime(string userId, DateTime startTime, DateTime dateTime)
        {
            try
            {
                return projectTaskService.GetProjectTaskByInspectorAndActualTime(userId, startTime, dateTime);
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
        /// 外业查询人员信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<ProjectTaskVo> GetProjectTask(string userId)
        {
            try
            {
                return projectTaskService.GetProjectTask(userId);
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
        /// 移动端保存实体数据（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体</param>
        public void SaveEntityApi(string keyValue, ProjectTaskUserEntity entity)
        {
            try
            {
                projectTaskService.SaveEntityApi(keyValue, entity);
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
        /// 保存实体数据（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体</param>
        public void SaveEntity(string keyValue, ProjectTaskEntity entity)
        {
            try
            {
                projectTaskService.SaveEntity(keyValue, entity);
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
        /// 添加子报告
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveFormTast(ProjectTaskEntity entity)
        {
            try
            {
                projectTaskService.SaveFormTast(entity);
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
        public void FormChange(string keyValue, ProjectTaskchangeEntity entity)
        {
            try
            {
                projectTaskService.FormChange(keyValue, entity);
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
        /// /提交审批
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="ProcessId"></param>
        public void UpdateFlowId(string keyValue, string ProcessId)
        {
            try
            {
                projectTaskService.UpdateFlowId(keyValue, ProcessId);
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
        /// 签到PC端
        /// </summary>
        /// <param name="keyValue"></param>
        public void UpdateFielded(string keyValue)
        {
            try
            {
                projectTaskService.UpdateFielded(keyValue);
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
        /// 签到移动端
        /// </summary>
        /// <param name="keyValue"></param>
        public void UpdateFieldedAPI(string keyValue, ProjectTaskEntity entity)
        {
            try
            {
                projectTaskService.UpdateFieldedAPI(keyValue, entity);
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

        public ProjectTaskVo GetFormProjectData(string keyValue)
        {
            try
            {
                return projectTaskService.GetFormProjectData(keyValue);
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
        /// 外业安排获取项目名
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public ProjectTaskVo GetProjectNameApi(string userId, DateTime dateTime)
        {
            try
            {
                return projectTaskService.GetProjectNameApi(userId, dateTime);
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

    }
}
