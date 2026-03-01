using Learun.Util;
using System.Data;
using System.Collections.Generic;
using System;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2022-03-11 00:18
    /// 描 述：项目任务单
    /// </summary>
    public interface ProjectTaskIBLL
    {
        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<ProjectTaskVo> GetPageList(Pagination pagination, string queryJson);
        IEnumerable<ProjectTaskVo> getTaskForMatchPageList(Pagination pagination, string queryJson);
        IEnumerable<ProjectTaskVo> GetTaskLoadList(string queryJson, string userId, string departmentId, int type);
        IEnumerable<ProjectTaskVo> GetHZPageList(Pagination pagination, string queryJson);
        IEnumerable<ProjectTaskVo> GetHZTaskListAll( string queryJson);
        IEnumerable<ProjectTaskEntity> GetAllFinishedTask();
        void ReMatchTaskAnfContract();

        /// <summary>
        /// 质量技术部统计图
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        IEnumerable<ProducTionVo> GetQualityTechnologyImplement(string categoryId);
        /// <summary>
        /// 报告待检测
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<ProjectTaskVo> GetPageToBeDetectList(Pagination pagination, string queryJson);
        /// <summary>
        /// 报告待检测_首页
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<ProjectTaskVo> GetIndexToBeDetectList(List<string> deptIds, out int countToBeDetect);
        /// <summary>
        /// 待报告_首页
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<ProjectTaskVo> GetIndexToBeReportedList(List<string> deptIds, out int countToBeDetect);
        /// <summary>
        /// 超期项目_首页
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<ProjectTaskVo> GetIndexOverdueList(List<string> deptIds, out int countToBeDetect);
        /// <summary>
        /// 已完成_首页
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<ProjectTaskVo> GetIndexHaveCompletedList(List<string> deptIds, out int countToBeDetect);
        /// <summary>
        /// 生产统计图
        /// </summary>           
        /// <returns></returns>
        IEnumerable<ProducTionVo> GetProducTionList(string dep);
        /// <summary>
        /// 生产超时数据/超时金额
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        IEnumerable<ProducTionVo> GetProducTionTimeoutList(string dep);
        /// <summary>
        /// 生产超时数据/超时金额
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        IEnumerable<ProducTionVo> GetProducTionTimeoutListDepartmentId(string dep);
        /// <summary>
        /// 生产统计图多部门
        /// </summary>           
        /// <returns></returns>
        IEnumerable<ProducTionVo> GetProducTionListDepartmentId(string dep);
        /// <summary>
        /// 待报告
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<ProjectTaskVo> GetPageToBeReportedList(Pagination pagination, string queryJson);
        /// <summary>
        /// 已完成
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>

        IEnumerable<ProjectTaskVo> GetPageHaveCompletedList(Pagination pagination, string queryJson);
        /// <summary>
        /// 超期项目
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<ProjectTaskVo> GetPageOverdueItemList(Pagination pagination, string queryJson);
        /// <summary>
        /// 多部门获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<ProjectTaskVo> GetPageListDepartmentId(Pagination pagination, string queryJson, string dep);
        /// <summary>
        /// 报告导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<ProjectTaskVo> GetPageList(string queryJson);
        /// <summary>
        /// 多部门报告导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<ProjectTaskVo> GetPageListDepartmentId(string queryJson, string dep);
        /// <summary>
        /// 获取ProjectTask表实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        ProjectTaskEntity GetProjectTaskEntity(string keyValue);

       /// <summary>
       /// 获取最新的任务编号
       /// </summary>
       /// <param name="taskNo"></param>
       /// <returns></returns>
        string GetNextProjectTaskNo(string taskNo);
        string GetNextProjectTaskNo_ZBG(string taskNo);

        ///<summary>
        ///根据ID获取任务信息
        /// </summary>
        /// <param name="keyValue"></param>
        ///<remarks></remarks>
        ///ProjectBillingVo GetPriewProjectTask(string keyValue);
        /// <summary>
        /// 获取主表实体数据
        /// </summary>
        /// <param name="processId">流程实例ID</param>
        /// <returns></returns>
        ProjectTaskEntity GetEntityByProcessId(string processId);
        /// <summary>
        /// 获取预览数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        ProjectTaskVo GetPriewProjectTask(string keyValue);

        ///<summary>
        ///根据流程ID获取数据 
        /// </summary>
        /// <param name="processId">流程实例ID</param>
        /// <returns></returns>
        ProjectTaskVo GetTaskByProcessId(string processId);
        ///<summary>
        ///根据流程ID获取数据 
        /// </summary>
        /// <param name="processId">流程实例ID</param>
        /// <returns></returns>
        ProjectTaskVo GetPriewProjectTaskprojectId(string processId);
        /// <summary>
        /// 报告上传
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        void GetProjectTaskSc(string keyValue, ProjectTaskEntity entity);
        /// <summary>
        /// 根据用户id查询对于的任务单（计划时间）
        /// </summary>
        /// <returns></returns>
        List<ProjectTaskVo> GetProjectTaskByInspectorAndPlanTime(string userId, DateTime startTime, DateTime dateTime);
        /// <summary>
        ///  根据用户id查询对于的任务单(实际时间)
        /// </summary>
        /// <returns></returns>
        List<ProjectTaskVo> GetProjectTaskByInspectorAndActualTime(string userId, DateTime startTime, DateTime dateTime);
        ProjectTaskVo GetProjectTaskByTime(string userId);
        #endregion

        #region 提交数据

        /// <summary>
        /// 删除实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void DeleteEntity(string keyValue);
        /// <summary>
        /// 保存实体数据（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体</param>
        void SaveEntity(string keyValue, ProjectTaskEntity entity);
        /// <summary>
        /// 添加子报告
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        void SaveFormTast(ProjectTaskEntity entity);
        void SaveEntityApi(string keyValue, ProjectTaskUserEntity entity);

        /// 提交审批
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="ProcessId"></param>
        void UpdateFlowId(string keyValue, string ProcessId);
        /// <summary>
        /// 变更
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体</param>
        void FormChange(string keyValue, ProjectTaskchangeEntity entity);

        ProjectTaskVo GetFormProjectData(string keyValue);
        /// <summary>
        /// 签到PC端
        /// </summary>
        /// <param name="keyValue"></param>
        void UpdateFielded(string keyValue);
        /// <summary>
        /// 签到移动端
        /// </summary>
        /// <param name="keyValue"></param>
        void UpdateFieldedAPI(string keyValue, ProjectTaskEntity entity);
        /// <summary>
        /// 根据id获取检测员
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        ProjectTaskVo GetProjectTaskById(string keyValue);
        /// <summary>
        /// 外业安排获取项目名
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        ProjectTaskVo GetProjectNameApi(string userId, DateTime dateTime);
        /// <summary>
        /// 根据id修改状态
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ProjectTaskVo GetPageListName(string id);
        /// <summary>
        /// 根据名字获取id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ProjectTaskVo GetProjectName(string name);
        /// <summary>
        /// 根据负责人id查名字
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        ProjectTaskVo GetFollowPersonNameByUserId(string queryJson, string id);
        /// <summary>
        /// 根据报备id查询相关任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<ProjectTaskVo> GetProjectTaskList(string id);
        /// <summary>
        /// 获取排班信息
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="inspetorList"></param>
        /// <returns></returns>
        IEnumerable<TaskScheduleVo> GetScheduleList(ScheduleParam queryJson);

        #endregion

    }
}
