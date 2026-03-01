using System.Collections.Generic;
using Learun.Application.TwoDevelopment.LR_CodeDemo;
using System.Linq;
using Learun.Application.Base.SystemModule;
using Learun.Application.Organization;
using Nancy;
using Learun.Util;
using System;
using Learun.Application.TwoDevelopment.LR_CodeDemo.ReportForms;
using Learun.Application.Base.AuthorizeModule;
using Learun.Application.WorkFlow;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Learun.Application.WebApi
{
    public class ProjectApi : BaseApi
    {
        private GantProjectIBLL gantProjectIBLL = new GantProjectBLL();
        private ProjectManageIBLL projectIBLL = new ProjectManageBLL();
        private ProjectContractIBLL projectContractIBLL = new ProjectContractBLL();
        private VersionIBLL versionBLL = new VersionBLL();
        private ProjectTaskIBLL projectTaskIBLL = new ProjectTaskBLL();
        private ProjectRecruitIBLL projectRecruitIBLL = new ProjectRecruitBLL();
        private ProjectBillingIBLL projectBillingIBLL = new ProjectBillingBLL();
        private ProjectPaymentIBLL projectPaymentIBLL = new ProjectPaymentBLL();
        private ProjectPayCollectionIBLL projectPayCollectionIBLL = new ProjectPayCollectionBLL();
        private DataItemIBLL dataItemBLL = new DataItemBLL();
        private CompanyIBLL companyIBLL = new CompanyBLL();
        private UserIBLL userIBLL = new UserBLL();
        private DepartmentIBLL departmentIBLL = new DepartmentBLL();
        private ProjectManageIBLL projectManageIBLL = new ProjectManageBLL();
        private ProjectContractBLL projectContractBLL = new ProjectContractBLL();
        CodeRuleIBLL codeRuleIBLL = new CodeRuleBLL();
        private AnnexesFileIBLL annexesFileIBLL = new AnnexesFileBLL();
        private PaymentIBLL paymentIBLL = new PaymentBLL();
        private ReportFormsIBLL reportFormsBLL = new ReportFormsBLL();
        private DataItemIBLL dataItemIBLL = new DataItemBLL();
        private UserRelationIBLL userRelationIBLL = new UserRelationBLL();
        private NWFProcessIBLL nWFProcessIBLL = new NWFProcessBLL();
        private ProjectPaymentListIBLL projectPaymentListIBLL = new ProjectPaymentListBLL();
        private NWFTaskIBLL nWFTaskIBLL = new NWFTaskBLL();
        private AreaIBLL areaIBLL = new AreaBLL();
        /// <summary>
        /// 注册接口
        /// <summary>
        public ProjectApi()
            : base("/learun/adms/LR_CodeDemo/Project")
        {

            Get["/pagelist"] = GetPageList;
            Get["/list"] = GetList;
            Get["/form"] = GetForm;
            Post["/delete"] = DeleteForm;
            Post["/save"] = SaveForm;
            //外业安排日历
            Get["/getTableList"] = GetProduceUserStatusInfo;
            Get["/getProjectName"] = GetProjectName;
            //组件获取项目名称接口
            //合同添加项目名称/付款
            Get["/getProjectPageList"] = GetProjectPageList;
            Get["/getProjectContractPageList"] = GetProjectContractPageList;
            //回款添加项目名称/用工/任务
            Get["/getProjectPageListT"] = GetProjectPageListT;
            //开票添加
            Get["/getProjectPageListBilling"] = GetProjectPageListBilling;
            //组件获取部门和人员的接口
            //选择部门
            Get["/getDepList"] = GetdepList;
            //添加人员
            Get["/getUserList"] = GetUserList;
            //合作伙伴添加人员
            Get["/getUserListHZ2"] = GetUserListHZ2;
            //根据部门id查找部门名称
            Get["/getDepById"] = GetdepById;
            //根据人员id查找人员名称
            Get["/getUserById"] = GetUserById;
            Get["/getUser"] = GetUser;
            //合作伙伴合同主体
            Get["/getDetailTreeHZ"] = GetDetailTreeHZ;
            //项目报备
            //列表显示
            Get["/getProjectList"] = GetProjectList;
            //根据id查询对应数据
            Get["/getProjectById"] = GetProjectById;
            Get["/getProjectById2"] = GetProjectById2;
            //新增/修改
            Get["/getProjectSave"] = GetProjectSave;
            Post["/ProjectSave2"] = ProjectSave2;
            //删除
            Get["/getProjectDelete"] = getProjectDelete;
            Get["/getProjectDelete2"] = getProjectDelete2;
            //导出
            Get["/getProjectAll"] = GetProjectAll;



            //项目合同管理
            //合同有效额
            Get["/getUpdateEffectiveAmount"] = GetUpdateEffectiveAmount;
            //重算有效合同额
            Get["/RecaculateEffectiveAmount"] = RecaculateEffectiveAmount;
            Get["/ReMatchTaskWithContractId"] = ReMatchTaskWithContractId;
            Get["/ReMatchPaymentWithContractId"] = ReMatchPaymentWithContractId;
            Get["/ReMatchBillingWithContractId"] = ReMatchBillingWithContractId;

            //列表显示
            Post["/getContractList_2"] = GetContractPageList;
            Get["/getContractList"] = GetContractPageList;
            Get["/getContractById"] = GetContractById;
            Get["/getContractById2"] = GetContractById2;
            //根据流程id显示数据
            Get["/getContractByProcessId"] = GetContractByProcessId;
            //添加合同/编辑合同
            Post["/ContractSave"] = ContractSave;
            Post["/ContractSave2"] = ContractSave2;
            //合同归档
            Post["/UpdateReceivedFlagSave2"] = UpdateReceivedFlagSave2;
            //提审
            Get["/getContractSubmitter2"] = GetContractSubmitter2;
            //取消
            Post["/Zuofei"] = Zuofei;
            //合同编号自动生成
            Get["/GetContractCode"] = GetContractCode;
            //删除
            Get["/getContractDelete2"] = GetContractDelete2;
            //全部
            Get["/getContractListAll"] = GetContractPageListAll;
            //导出
            Get["/getProjectContractAll"] = GetProjectContractAll;
            //统计图
            Get["/getContract"] = GetContract;



            //任务管理
            Get["/getFormProjectData"] = GetFormProjectData;
            //列表显示
            Get["/getTaskPageList"] = GetTaskPageList;
            Get["/getTaskForMatchPageList"] = GetTaskForMatchPageList;


            //生产负荷
            Get["/getTaskLoadList"] = GetTaskLoadList;

            Get["/getHZTaskPageList"] = GetHZTaskPageList;

            Get["/reCaculateFinishTime"] = ReCaculateFinishTime;

            //排期List
            Post["/getTaskScheduleList"] = GetTaskScheduleList;

            //排行榜
            Post["/getLeaderboardData"] = GetLeaderboardData;

            //id查询
            Get["/getTaskById"] = GetTaskById;
            Get["/getTaskById2"] = GetTaskById2;
            //合同查看
            Get["/getTaskByContractId"] = GetTaskByContractId;
            Get["/getTaskByProcessId"] = GetTaskByProcessId;
            Post["/TaskSave"] = TaskSave;
            //子报告
            Post["/TaskSaveFormTast"] = TaskSaveFormTast;
            //新增/编辑
            Post["/TaskSave2"] = TaskSave2;
            //上传报告
            Post["/TaskSaveUpdate2"] = TaskSaveUpdate2;
            //更新报告编号
            Get["/UpdateProjectTaskNo"] = UpdateProjectTaskNo;

            //报告上传
            Post["/getProjectTaskSc"] = GetProjectTaskSc;
            Post["/ProjectTaskSc2"] = ProjectTaskS2c;
            Post["/ProjectTaskFielded"] = GetProjectTaskFielded;
            //签到/离场
            Get["/getProjectTaskFielded2"] = GetProjectTaskFielded2;
            //删除
            Get["/getProjectTaskDelete2"] = GetProjectTaskDelete2;
            //提审
            Get["/geTaskSubmitter2"] = GeTaskSubmitter2;
            //报告导出
            Get["/getProjectTaskAll"] = GetProjectTaskAll;

            Get["/getHZProjectTaskAll"] = GetHZProjectTaskAll;


            //用工管理
            Get["/getRecruitPageList"] = GetRecruitPageList;
            //新增/编辑
            Post["/RecruitSaveForm"] = RecruitSaveForm;
            //根据id查询
            Get["/getRecruitById"] = GetRecruitById;
            Get["/getRecruitById2"] = GetRecruitById2;
            Get["/getRecruitByProcessId"] = GetRecruitByProcessId;
            //提审
            Get["/getRecruitSubmitter"] = GetRecruitSubmitter;
            //删除
            Get["/getRecruitDelete2"] = GetRecruitDelete2;


            //任务管理选择项目接口
            Get["/getProjectName2"] = GetProjectName2;
            //合同管理选择项目接口
            Get["/getProjectName1"] = GetProjectName1;
            //用工管理选择项目接口
            Get["/getProjectPageListYG"] = getProjectPageListYG;



            //开票管理            
            Get["/getBillingPageList"] = GetBillingPageList;
            Get["/getBillingSum"] = GetBillingSum;
            //提审
            Get["/getBillingSubmitter"] = GetBillingSubmitter;
            //根据id查询
            Get["/getBillingById"] = GetBillingById;
            Get["/getBillingById2"] = GetBillingById2;
            Get["/getBillingByProcessId"] = GetBillingByProcessId;
            //保存
            Post["/BillingSave"] = BillingSave;
            Post["/BillingSave2"] = BillingSave2;
            //取消
            Post["/BillingZuofei"] = BillingZuofei;
            //删除
            Get["/getBillingDelete2"] = GetBillingDelete2;
            //导出
            Get["/getProjectBillingAll"] = GetProjectBillingAll;


            //回款管理
            Get["/getPayCollectionPageList1"] = GetPayCollectionPageList;
            Get["/getPayCollectionSum"] = GetPayCollectionSum;
            Get["/getPayCollectioById"] = GetPayCollectioById;
            //根据id查询
            Get["/getPayCollectioById2"] = GetPayCollectioById2;
            Post["/PayCollectioSave"] = PayCollectioSave;
            //回款添加/编辑
            Post["/PayCollectioSave2"] = PayCollectioSave2;
            //删除
            Get["/getPayCollectioDeleteId2"] = GetPayCollectioDeleteId2;
            //导出
            Get["/getProjectPayCollectionAll"] = GetProjectPayCollectionAll;


            //付款
            Get["/getPaymentPageList"] = GetPaymentPageList;
            //付款批量
            Get["/GetPaymentPageList2"] = GetPaymentPageList2;
            //id查询
            Get["/getPaymentById"] = GetPaymentById;
            Get["/getPaymentById2"] = GetPaymentById2;
            Get["/getPaymentByProcessId"] = GetPaymentByProcessId;
            Post["/PaymentSave"] = PaymentSave;
            //付款批量新增
            Post["/PaymentSaveList2"] = PaymentSaveList2;
            //付款批量编辑
            Post["/PaymentUpdateList2"] = PaymentUpdateList2;
            //付款批量提审
            Get["/getPaymentSubmitterList2"] = GetPaymentSubmitterList2;
            //新增/编辑
            Post["/PaymentSave2"] = PaymentSave2;
            //删除
            Get["/getPaymentDeleteId2"] = GetPaymentDeleteId2;

            Get["/getPaymentDeleteByTid"] = GetPaymentDeleteByTid;
            //提审
            Get["/getPaymentSubmitter2"] = GetPaymentSubmitter2;
            //导出
            Get["/getProjectPaymentAll"] = GetProjectPaymentAll;
            Get["/getProjectPaymentAll2"] = GetProjectPaymentAll2;


            //行政付款
            Get["/getAdministrativePaymentPageList"] = GetAdministrativePaymenteList;
            Post["/AdministrativePaymentSave"] = AdministrativePaymentSave;
            Get["/getAdministrativePaymentById"] = GetAdministrativePaymentById;
            Get["/getAdministrativePaymentDeleteId2"] = GetAdministrativePaymentDeleteId2;
            //导出
            Get["/getPaymentExport"] = GetPaymentExport;
            //根据id查询
            Get["/getAdministrativePaymentById2"] = GetAdministrativePaymentById2;
            Get["/getAdministrativePaymentSubmitter"] = GetAdministrativePaymentSubmitter;
            //行政付款新增/保存
            Post["/AdministrativePaymentSave"] = AdministrativePaymentSave;
            Get["/getAdministrativePaymentByProcessId"] = GetAdministrativePaymentByProcessId;
            //营销报表
            Get["/getMarketings"] = GetMarketings;
            Get["/getMarketingsSum"] = GetMarketingsSum;
            //生产报表
            Get["/getProductions"] = GetProductions;
            Get["/getGetProductionsSum"] = GetProductionsSum;
            //结算报表
            Get["/getSettleAccounts"] = GetSettleAccounts;
            Get["/getSettleAccountsSum"] = GetSettleAccountsSum;
            //获取大屏信息
            Get["/getProjectMonthCount"] = GetProjectMonthCount;
            Get["/getProjectCountBySource"] = GetProjectCountBySource;
            Get["/getProjectCountByProvince"] = GetProjectCountByProvince;
            Get["/getProjectConversion"] = GetProjectConversion;
            Get["/getBackProjectRate"] = GetBackProjectRate;
            //版本更新
            Get["/getVersionList"] = GetVersionList;
            //生产统计图
            Get["/getProducTionList"] = GetProducTionList;
            //生产超时报告
            Get["/getProducTionTimeoutList"] = GetProducTionTimeoutList;
            //营销统计图
            Get["/getMarkeTingList"] = GetMarkeTingList;
            //营销待回款
            Get["/getMoneyToBeCollected"] = GetMoneyToBeCollected;
            //质量技术部
            Get["/getQualityTechnology"] = GetQualityTechnology;
            //①公司全年项目数量、已实施数量、待实施数量/金额
            Get["/getMoneyToBeCollected1"] = GetMoneyToBeCollected1;
            Get["/getMoneyToBeCollected11"] = GetMoneyToBeCollected11;

            //首页
            Get["/getIndexData"] = GetIndexData;

            Get["/getIndexData2"] = GetIndexData2;
            //二期任务单显示
            Get["/getTaskPageToBeDetectList"] = GetTaskPageToBeDetectList;
            //财务管理
            //资金台账
            Get["/getFundStatementList"] = GetFundStatementList;
            //成本新增/编辑
            Post["/CapitalAmountSaveForm"] = CapitalAmountSaveForm;
            //资金台账导出
            Get["/getFundStatementListAll"] = GetFundStatementListAll;
            //营销台账
            Get["/getMarketingList"] = GetMarketingList;
            Get["/getMarketingtAll"] = GetMarketingAll;
            Get["/getMarketingtSUM"] = GetMarketingSUM;
            Get["/SaveSettlement"] = SaveSettlement;


            //生产台账
            Get["/getProductionsList"] = GetProductionsList;
            Get["/getProductionsAll"] = GetProductionsAll;
            Get["/getProductionsSUM"] = GetProductionsSUM;
            //结算台账
            Get["/getSettleAccountsList"] = GetSettleAccountsList;
            Get["/getSettleAccountsAll"] = GetSettleAccountsAll;
            Get["/getSettleAccountsSUM2"] = GetSettleAccountsSUM2;
            //伙伴营销台账
            Get["/getMarketingListHZ"] = GetMarketingListHZ;
            Get["/getMarketingtAllHZ"] = GetMarketingAllHZ;
            Get["/getMarketingtSUMHZ"] = GetMarketingSUMHZ;
            //伙伴结算台账
            Get["/getSettleAccountsListHZ"] = GetSettleAccountsListHZ;
            Get["/getSettleAccountsAllHZ"] = GetSettleAccountsAllHZ;
            Get["/getSettleAccountsSUMHZ"] = GetSettleAccountsSUMHZ;
            Post["/getSettleAccountsbili"] = GetSettleAccountsbili;
            //流程任务
            //根据流程id获取流程信息
            Get["/getprocessId"] = GetprocessId;
            //根据流程id获取节点信息
            Get["/getTaskList"] = GetTaskList;
            Get["/getcTaskList"] = GetcTaskList;

            //多部门数据显示
            //项目报备
            Get["/getProjectListDepartmentId"] = GetProjectListDepartmentId;
            //导出
            Get["/getProjectAllDepartmentId"] = GetProjectAllDepartmentId;
            //合同管理
            Get["/getProjectContractListDepartmentId"] = GetProjectContractListDepartmentId;
            //导出
            Get["/getProjectContractAllDepartmentId"] = GetProjectContractAllDepartmentId;
            //任务管理
            Get["/getProjectTaskListDepartmentId"] = GetProjectTaskListDepartmentId;
            //导出
            Get["/getProjectTaskAllDepartmentId"] = GetProjectTaskAllDepartmentId;
            //用工管理
            Get["/getProjectRecruiListDepartmentId"] = GetProjectRecruiListDepartmentId;

            //开票管理
            Get["/getProjectBillingListDepartmentId"] = GetProjectBillingListDepartmentId;
            Get["/getProjectBillingSumDepartmentId"] = GetProjectBillingSumDepartmentId;
            //导出
            Get["/getProjectBillingAllDepartmentId"] = GetProjectBillingAllDepartmentId;

            //回款管理
            Get["/getProjectPayCollectionListDepartmentId"] = GetProjectPayCollectionListDepartmentId;
            Get["/getProjectPayCollectionSumDepartmentId"] = GetProjectPayCollectionSumDepartmentId;
            //导出
            Get["/getProjectPayCollectionAllDepartmentId"] = GetProjectPayCollectionAllDepartmentId;
            //付款管理

            Get["/getProjectPaymentListDepartmentId"] = GetProjectPaymentListDepartmentId;
            //导出
            Get["/getProjectPaymentAllDepartmentId"] = GetProjectPaymentAllDepartmentId;
            //行政付款
            Get["/getPaymentListDepartmentId"] = GetPaymentListDepartmentId;
            //导出
            Get["/getPaymentAllDepartmentId"] = GetPaymentAllDepartmentId;
            //营销台账
            Get["/getMarketingsDepartmentId"] = GetMarketingsDepartmentId;
            //导出
            Get["/getMarketingsDepartmentIdAll"] = GetMarketingsDepartmentIdAll;
            //合计
            Get["/getMarketingsSumDepartmentId"] = GetMarketingsSumDepartmentId;

            //生产台账
            Get["/getProductionsDepartmentId"] = GetProductionsDepartmentId;
            //导出
            Get["/getProductionsDepartmentIdAll"] = GetProductionsDepartmentIdAll;
            //合计
            Get["/getProductionsSumDepartmentId"] = GetProductionsSumDepartmentId;

            //结算台账
            Get["/getSettleAccountsDepartmentId"] = GetSettleAccountsDepartmentId;
            //导出
            Get["/getSettleAccountsSumAllDepartmentId"] = GetSettleAccountsSumAllDepartmentId;
            //合计 
            Get["/getSettleAccountsSumDepartmentId"] = GetSettleAccountsSumDepartmentId;

        }

        #region 多部门接口
        //报备列表

        public Response GetProjectListDepartmentId(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
            List<ProjectVo> list = new List<ProjectVo>();

            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or t.FDepartmentId='" + strList[i] + "' or t.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or t.FDepartmentId='" + strList[i] + "' or t.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";
                    }

                }
                var data = projectManageIBLL.GetPageListAddressDepartmentIds(parameter.pagination, parameter.queryJson, deps);
                foreach (var info in data)
                {
                    //创建时间
                    DateTime time = (DateTime)info.CreateTime;
                    info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                    //营销人员
                    var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                    //报备人员
                    var preparedPerson = userIBLL.GetEntityByUserId(info.PreparedPerson);
                    //项目状态
                    var projectStatus = dataItemBLL.GetDetailItemName(info.ProjectStatus, "projectStatus");
                    //项目来源
                    var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                    if (projectSource != null)
                    {
                        info.ProjectSourceName = projectSource.F_ItemName;
                    }
                    if (preparedPerson != null)
                    {
                        info.PreparedPersonName = preparedPerson.F_RealName;
                    }
                    if (followPerson != null)
                    {
                        info.FollowPersonName = followPerson.F_RealName;
                    }
                    if (projectStatus != null)
                    {
                        info.ProjectStatusName = projectStatus.F_ItemName;
                    }
                    list.Add(info);
                }

            }


            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };

            return Success(jsonData);
        }
        //多部门报备导出
        public Response GetProjectAllDepartmentId(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
            List<ProjectVo> list = new List<ProjectVo>();

            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or t.FDepartmentId='" + strList[i] + "' or t.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or t.FDepartmentId='" + strList[i] + "' or t.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";
                    }

                }
                var data = projectManageIBLL.GetPageListAddressDepartmentId(parameter.queryJson, deps);
                foreach (var info in data)
                {
                    //创建时间
                    DateTime time = (DateTime)info.CreateTime;
                    info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                    //营销人员
                    var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                    //报备人员
                    var preparedPerson = userIBLL.GetEntityByUserId(info.PreparedPerson);
                    //项目状态
                    var projectStatus = dataItemBLL.GetDetailItemName(info.ProjectStatus, "projectStatus");
                    //项目来源
                    var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                    if (projectSource != null)
                    {
                        info.ProjectSourceName = projectSource.F_ItemName;
                    }
                    if (preparedPerson != null)
                    {
                        info.PreparedPersonName = preparedPerson.F_RealName;
                    }
                    if (followPerson != null)
                    {
                        info.FollowPersonName = followPerson.F_RealName;
                    }
                    if (projectStatus != null)
                    {
                        info.ProjectStatusName = projectStatus.F_ItemName;
                    }
                    list.Add(info);
                }

            }




            list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);


        }
        //合同管理

        public Response GetProjectContractListDepartmentId(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
            List<ProjectContractVo> list = new List<ProjectContractVo>();

            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or a.DepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";

                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or a.DepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";
                    }

                }
                var data = projectContractIBLL.GetPageListDepartmentId(parameter.pagination, parameter.queryJson, deps);
                foreach (var info in data)
                {

                    //if (info.ProjectSource.ToInt() == 3)
                    //{
                    //    ProjectContractEntity entity = new ProjectContractEntity();


                    //    entity.EffectiveAmount = 0;
                    //    projectContractIBLL.SaveEntity1(info.id, entity);
                    //}
                    //if (info.ContractStatus.ToInt() == 11)
                    //{
                    //    ProjectContractEntity entity = new ProjectContractEntity();


                    //    entity.EffectiveAmountShow = 0;info.ContractStatus.ToInt() == 11
                    //    projectContractIBLL.SaveEntity1(info.id, entity);
                    //}
                    //创建时间
                    if (info.CreateTime != null)
                    {
                        DateTime time = (DateTime)info.CreateTime;
                        info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                    }
                    //项目来源
                    var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                    if (projectSource != null)
                    {
                        info.ProjectSourceName = projectSource.F_ItemName;
                    }
                    //营销人员
                    var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                    if (followPerson != null)
                    {
                        info.FollowPersonName = followPerson.F_RealName;
                    }
                    //营销部门
                    var department = departmentIBLL.GetEntity(info.DepartmentId);
                    if (department != null)
                    {
                        info.DepartmentName = department.F_FullName;
                    }
                    //合同主体
                    var contractSubject = dataItemBLL.GetDetailItemName(info.ContractSubject, "ContractSubject");
                    if (contractSubject != null)
                    {
                        info.ContractSubjectName = contractSubject.F_ItemName;
                    }
                    //合同状态
                    var contractStatus = dataItemBLL.GetDetailItemName(info.ContractStatus, "ContractStatus");
                    if (contractStatus != null)
                    {
                        info.ContractStatusName = contractStatus.F_ItemName;
                    }
                    //合同分类
                    var contractType = dataItemBLL.GetDetailItemName(info.ContractType, "ContractType");
                    if (contractType != null)
                    {
                        info.ContractTypeName = contractType.F_ItemName;
                    }
                    //归档类型
                    var receivedFlag = dataItemBLL.GetDetailItemName(info.ReceivedFlag, "ReceiptType");
                    if (receivedFlag != null)
                    {
                        info.ReceivedFlagName = receivedFlag.F_ItemName;
                    }

                    //if (info.ContractStatus.ToInt() == 11 && info.ProjectSource.ToInt() != 3)
                    //{
                    //    ProjectContractEntity entity = new ProjectContractEntity();
                    //    entity.EffectiveAmount = 0;
                    //    projectContractIBLL.SaveEntity1(info.id, entity);
                    //}
                    list.Add(info);
                }

            }


            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };

            return Success(jsonData);
        }
        //多部门合同导出
        public Response GetProjectContractAllDepartmentId(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
            List<ProjectContractVo> list = new List<ProjectContractVo>();

            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or a.DepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";

                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or a.DepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";
                    }

                }
                var data = projectContractIBLL.GetPageListDepartmentId(parameter.queryJson, deps);
                foreach (var info in data)
                {

                    //创建时间
                    if (info.CreateTime != null)
                    {
                        DateTime time = (DateTime)info.CreateTime;
                        info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                    }
                    //项目来源
                    var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                    if (projectSource != null)
                    {
                        info.ProjectSourceName = projectSource.F_ItemName;
                    }
                    //营销人员
                    var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                    if (followPerson != null)
                    {
                        info.FollowPersonName = followPerson.F_RealName;
                    }
                    //营销部门
                    var department = departmentIBLL.GetEntity(info.DepartmentId);
                    if (department != null)
                    {
                        info.DepartmentName = department.F_FullName;
                    }
                    //合同主体
                    var contractSubject = dataItemBLL.GetDetailItemName(info.ContractSubject, "ContractSubject");
                    if (contractSubject != null)
                    {
                        info.ContractSubjectName = contractSubject.F_ItemName;
                    }
                    //合同状态
                    var contractStatus = dataItemBLL.GetDetailItemName(info.ContractStatus, "ContractStatus");
                    if (contractStatus != null)
                    {
                        info.ContractStatusName = contractStatus.F_ItemName;
                    }
                    //合同分类
                    var contractType = dataItemBLL.GetDetailItemName(info.ContractType, "ContractType");
                    if (contractType != null)
                    {
                        info.ContractTypeName = contractType.F_ItemName;
                    }
                    //归档类型
                    var receivedFlag = dataItemBLL.GetDetailItemName(info.ReceivedFlag, "ReceiptType");
                    if (receivedFlag != null)
                    {
                        info.ReceivedFlagName = receivedFlag.F_ItemName;
                    }

                    //if (info.ContractStatus.ToInt() == 11 && info.ProjectSource.ToInt() != 3)
                    //{
                    //    ProjectContractEntity entity = new ProjectContractEntity();
                    //    entity.EffectiveAmount = 0;
                    //    projectContractIBLL.SaveEntity1(info.id, entity);
                    //}
                    list.Add(info);
                }

            }


            list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);


        }
        //任务管理
        public Response GetProjectTaskListDepartmentId(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
            List<ProjectTaskVo> list = new List<ProjectTaskVo>();

            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or t.TaskDepartmentId like '%" + strList[i] + "%' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or pc.DepartmentId='" + strList[i] + "' or t.SubDepartmentId='" + strList[i] + "' or t.MainDepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or t.TaskDepartmentId like '%" + strList[i] + "%' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or pc.DepartmentId='" + strList[i] + "' or t.SubDepartmentId='" + strList[i] + "' or t.MainDepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";
                    }

                }
                var data = projectTaskIBLL.GetPageListDepartmentId(parameter.pagination, parameter.queryJson, deps);
                foreach (var info in data)
                {
                    if (string.IsNullOrEmpty(info.ProjectName))
                    {
                        var projectInfo = projectIBLL.GetProjectEntity(info.ProjectId);
                        if (projectInfo != null)
                        {
                            info.ProjectName = projectInfo.ProjectName;
                        }
                    }
                    //创建时间
                    DateTime time = (DateTime)info.CreateTime;
                    info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                    //报告时间
                    if (info.FlowFinishedTime != null)
                    {
                        DateTime time1 = (DateTime)info.FlowFinishedTime;
                        info.FlowFinishedTimeMD = time1.ToString("yyyy-MM-dd");
                    }
                    //进场时间
                    if (info.ApproachTime != null)
                    {
                        DateTime time1 = (DateTime)info.ApproachTime;
                        info.ApproachTimeMd = time1.ToString("yyyy-MM-dd");
                    }

                    //离场时间
                    if (info.ActualDepartureTime != null)
                    {
                        DateTime time2 = (DateTime)info.ActualDepartureTime;
                        info.ActualDepartureTimeMd = time2.ToString("yyyy-MM-dd");
                    }
                    //报告计划时间
                    if (info.PlanTime != null)
                    {
                        DateTime time3 = (DateTime)info.PlanTime;
                        info.PlanTimeMd = time3.ToString("yyyy-MM-dd");
                    }


                    //项目负责人
                    var projectResponsible = userIBLL.GetEntityByUserId(info.ProjectResponsible);
                    if (projectResponsible != null)
                    {
                        info.ProjectResponsibleName = projectResponsible.F_RealName;
                    }
                    //所属部门
                    var department = departmentIBLL.GetEntity(info.DepartmentId);
                    if (department != null)
                    {
                        info.DepartmentName = department.F_FullName;
                    }

                    //报告状态
                    var taskStatus = dataItemBLL.GetDetailItemName(info.TaskStatus, "TaskStatus");
                    if (taskStatus != null)
                    {
                        info.TaskStatusName = taskStatus.F_ItemName;
                    }
                    //合同主体
                    var contractSubject = dataItemBLL.GetDetailItemName(info.ReportSubject, "ContractSubject");
                    if (contractSubject != null)
                    {
                        info.ReportSubjectName = contractSubject.F_ItemName;
                    }
                    /*List<ProjectContractEntity> projectContracts = projectContractIBLL.GetProjectContractByProjectId(info.ProjectId);
                    if (projectContracts.Count > 0)
                    {
                        info.ContractNo = projectContracts.FirstOrDefault().ContractNo;

                        //合同主体
                        var contractSubject = dataItemBLL.GetDetailItemName(projectContracts.FirstOrDefault().ContractSubject, "ContractSubject");
                        if (contractSubject != null)
                        {
                            info.ContractSubjectName = contractSubject.F_ItemName;
                        }
                    }*/
                    list.Add(info);
                }



            }


            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };

            return Success(jsonData);
        }
        //多部门任务导出
        public Response GetProjectTaskAllDepartmentId(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
            List<ProjectTaskVo> list = new List<ProjectTaskVo>();

            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or t.TaskDepartmentId like '%" + strList[i] + "%' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or pc.DepartmentId='" + strList[i] + "' or t.SubDepartmentId='" + strList[i] + "' or t.MainDepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or t.TaskDepartmentId like '%" + strList[i] + "%' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or pc.DepartmentId='" + strList[i] + "' or t.SubDepartmentId='" + strList[i] + "' or t.MainDepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";
                    }

                }
                var data = projectTaskIBLL.GetPageListDepartmentId(parameter.queryJson, deps);
                foreach (var info in data)
                {
                    if (string.IsNullOrEmpty(info.ProjectName))
                    {
                        var projectInfo = projectIBLL.GetProjectEntity(info.ProjectId);
                        if (projectInfo != null)
                        {
                            info.ProjectName = projectInfo.ProjectName;
                        }
                    }
                    //创建时间
                    DateTime time = (DateTime)info.CreateTime;
                    info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                    //报告时间
                    if (info.FlowFinishedTime != null)
                    {
                        DateTime time1 = (DateTime)info.FlowFinishedTime;
                        info.FlowFinishedTimeMD = time1.ToString("yyyy-MM-dd");
                    }
                    //进场时间
                    if (info.ApproachTime != null)
                    {
                        DateTime time1 = (DateTime)info.ApproachTime;
                        info.ApproachTimeMd = time1.ToString("yyyy-MM-dd");
                    }

                    //离场时间
                    if (info.ActualDepartureTime != null)
                    {
                        DateTime time2 = (DateTime)info.ActualDepartureTime;
                        info.ActualDepartureTimeMd = time2.ToString("yyyy-MM-dd");
                    }
                    //报告计划时间
                    if (info.PlanTime != null)
                    {
                        DateTime time3 = (DateTime)info.PlanTime;
                        info.PlanTimeMd = time3.ToString("yyyy-MM-dd");
                    }


                    //项目负责人
                    var projectResponsible = userIBLL.GetEntityByUserId(info.ProjectResponsible);
                    if (projectResponsible != null)
                    {
                        info.ProjectResponsibleName = projectResponsible.F_RealName;
                    }
                    //所属部门
                    var department = departmentIBLL.GetEntity(info.DepartmentId);
                    if (department != null)
                    {
                        info.DepartmentName = department.F_FullName;
                    }

                    //报告状态
                    var taskStatus = dataItemBLL.GetDetailItemName(info.TaskStatus, "TaskStatus");
                    if (taskStatus != null)
                    {
                        info.TaskStatusName = taskStatus.F_ItemName;
                    }
                    //合同主体
                    var contractSubject = dataItemBLL.GetDetailItemName(info.ReportSubject, "ContractSubject");
                    if (contractSubject != null)
                    {
                        info.ReportSubjectName = contractSubject.F_ItemName;
                    }
                    //预警

                    if (info.TaskStatus.ToInt() == 5)
                    {
                        info.YJ = "已完成";
                    }
                    else if (info.YJ.ToInt() == 999)
                    {
                        info.YJ = "超时";
                    }
                    else if (info.YJ.ToInt() != 999 && info.TaskStatus.ToInt() != 5 && info.YJ.ToInt() != 111)
                    {
                        info.YJ = "剩余时间" + info.YJ;
                    }
                    else if (info.YJ.ToInt() == 111)
                    {
                        info.YJ = "严重超时";
                    }
                    /* List<ProjectContractEntity> projectContracts = projectContractIBLL.GetProjectContractByProjectId(info.ProjectId);
                     if (projectContracts.Count > 0)
                     {
                         info.ContractNo = projectContracts.FirstOrDefault().ContractNo;

                         //合同主体
                         var contractSubject = dataItemBLL.GetDetailItemName(projectContracts.FirstOrDefault().ContractSubject, "ContractSubject");
                         if (contractSubject != null)
                         {
                             info.ContractSubjectName = contractSubject.F_ItemName;
                         }
                     }*/
                    list.Add(info);
                }



            }


            list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);


        }
        //用工管理
        public Response GetProjectRecruiListDepartmentId(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
            List<ProjectRecruitVo> list = new List<ProjectRecruitVo>();

            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( a.FDepartmentId='" + strList[i] + "' or t.DepartmentId = '" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or pc.DepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";
                    }
                    else
                    {
                        deps += " or (  a.FDepartmentId='" + strList[i] + "' or t.DepartmentId = '" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or pc.DepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";
                    }

                }
                var data = projectRecruitIBLL.GetPageListDepartmentId(parameter.pagination, parameter.queryJson, deps);
                foreach (var info in data)
                {
                    //创建时间
                    DateTime time = (DateTime)info.CreateTime;
                    info.CreateTimeyMd = time.ToString("yyyy-MM-dd");

                    //申请人
                    var applyPerson = userIBLL.GetEntityByUserId(info.ApplyPerson);
                    if (applyPerson != null)
                    {
                        info.ApplyPersonName = applyPerson.F_RealName;
                    }
                    /*    //合同编号
                        List<ProjectContractEntity> projectContracts = projectContractIBLL.GetProjectContractByProjectId(info.ProjectId);
                        if (projectContracts.Count > 0)
                        {
                            info.ContractNo = projectContracts.FirstOrDefault().ContractNo;
                        }*/
                    //支付类型
                    var paymentMethod = dataItemBLL.GetDetailItemName(info.PaymentMethod, "PaymentMethod");
                    if (paymentMethod != null)
                    {
                        info.PaymentMethodName = paymentMethod.F_ItemName;
                    }
                    //报告状态
                    var recruitStatus = dataItemBLL.GetDetailItemName(info.RecruitStatus, "RecruitStatus");
                    if (recruitStatus != null)
                    {
                        info.RecruitStatusName = recruitStatus.F_ItemName;
                    }
                    list.Add(info);
                }


            }


            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };

            return Success(jsonData);
        }
        /// <summary>
        /// 多部门开票管理
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetProjectBillingListDepartmentId(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
            List<ProjectBillingVo> list = new List<ProjectBillingVo>();

            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";
                    }

                }
                var data = projectBillingIBLL.GetPageListDepartmentId(parameter.pagination, parameter.queryJson, deps);
                foreach (var info in data)
                {
                    //创建时间
                    DateTime time = (DateTime)info.CreateTime;
                    info.CreateTimeyMd = time.ToString("yyyy-MM-dd");

                    //开票内容
                    var billingContent = dataItemBLL.GetDetailItemName(info.BillingContent, "BillingContent");
                    if (billingContent != null)
                    {
                        info.BillingContentName = billingContent.F_ItemName;
                    }
                    //营销人员
                    var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                    if (followPerson != null)
                    {
                        info.FollowPerson = followPerson.F_RealName;
                    }
                    //营销部门
                    var department = departmentIBLL.GetEntity(info.DepartmentId);
                    if (department != null)
                    {
                        info.DepartmentName = department.F_FullName;
                    }
                    //开票类型
                    var billingType = dataItemBLL.GetDetailItemName(info.BillingType, "BillingType");
                    if (billingType != null)
                    {
                        info.BillingTypeName = billingType.F_ItemName;
                    }
                    //开票状态
                    var billingStatus = dataItemBLL.GetDetailItemName(info.BillingStatus, "BillingStatus");
                    if (billingStatus != null)
                    {
                        info.BillingStatusName = billingStatus.F_ItemName;
                    }
                    //开票单位
                    var billingUnit = dataItemBLL.GetDetailItemName(info.BillingUnit, "ContractSubject");
                    if (billingUnit != null)
                    {
                        info.BillingUnitName = billingUnit.F_ItemName;
                    }

                    list.Add(info);
                }

            }


            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };

            return Success(jsonData);
        }
        //开票合计
        public Response GetProjectBillingSumDepartmentId(dynamic _)
        {

            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            List<ProjectBillingVo> listdate = new List<ProjectBillingVo>();
            decimal? BillingAmountSUM = 0;
            if (followPerson.F_MoreDepartmentId != null)
            {

                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";
                    }

                }
                var data = projectBillingIBLL.GetPageListDepartmentId(parameter.queryJson, deps);
                if (data.ToList().Count > 0)
                {
                    foreach (var info in data)
                    {
                        BillingAmountSUM = BillingAmountSUM + info.BillingAmount;
                        listdate.Add(info);
                    }
                }
            }

            var result = new
            {
                BillingAmountSum = BillingAmountSUM,

            };
            var jsonData = new
            {
                rows = result
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetProjectBillingAllDepartmentId(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
            List<ProjectBillingVo> list = new List<ProjectBillingVo>();

            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";
                    }

                }
                var data = projectBillingIBLL.GetPageListDepartmentId(parameter.queryJson, deps);
                foreach (var info in data)
                {
                    //创建时间
                    DateTime time = (DateTime)info.CreateTime;
                    info.CreateTimeyMd = time.ToString("yyyy-MM-dd");

                    //开票内容
                    var billingContent = dataItemBLL.GetDetailItemName(info.BillingContent, "BillingContent");
                    if (billingContent != null)
                    {
                        info.BillingContentName = billingContent.F_ItemName;
                    }
                    //营销人员
                    var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                    if (followPerson != null)
                    {
                        info.FollowPerson = followPerson.F_RealName;
                    }
                    //营销部门
                    var department = departmentIBLL.GetEntity(info.DepartmentId);
                    if (department != null)
                    {
                        info.DepartmentName = department.F_FullName;
                    }
                    //开票类型
                    var billingType = dataItemBLL.GetDetailItemName(info.BillingType, "BillingType");
                    if (billingType != null)
                    {
                        info.BillingTypeName = billingType.F_ItemName;
                    }
                    //开票状态
                    var billingStatus = dataItemBLL.GetDetailItemName(info.BillingStatus, "BillingStatus");
                    if (billingStatus != null)
                    {
                        info.BillingStatusName = billingStatus.F_ItemName;
                    }
                    //开票单位
                    var billingUnit = dataItemBLL.GetDetailItemName(info.BillingUnit, "ContractSubject");
                    if (billingUnit != null)
                    {
                        info.BillingUnitName = billingUnit.F_ItemName;
                    }

                    list.Add(info);
                }

            }


            list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 多部门回款
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetProjectPayCollectionListDepartmentId(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
            List<ProjectPayCollectionVo> list = new List<ProjectPayCollectionVo>();

            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( pc.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( pc.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = projectPayCollectionIBLL.GetPageListDepartmentId(parameter.pagination, parameter.queryJson, deps);

                foreach (var info in data)
                {

                    //创建时间
                    DateTime time = (DateTime)info.CreateTime;
                    info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                    //到款时间
                    DateTime time1 = (DateTime)info.ReceiptDate;
                    info.ReceiptDateMd = time1.ToString("yyyy-MM-dd");
                    //项目来源
                    var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                    if (projectSource != null)
                    {
                        info.ProjectSourceName = projectSource.F_ItemName;
                    }
                    //营销部门
                    var department = departmentIBLL.GetEntity(info.DepartmentId);
                    if (department != null)
                    {
                        info.DepartmentName = department.F_FullName;
                    }
                    //创建人
                    var createUser = userIBLL.GetFollowPersonNameByUserId(info.CreateUser);
                    if (createUser != null)
                    {
                        info.CreateUserName = createUser.F_RealName;
                    }
                    list.Add(info);
                }
            }


            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };

            return Success(jsonData);
        }
        /// <summary>
        /// 回款合计
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetProjectPayCollectionSumDepartmentId(dynamic _)
        {

            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            decimal? AmountSUM = 0;
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            if (followPerson.F_MoreDepartmentId != null)
            {

                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( pc.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( pc.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = projectPayCollectionIBLL.GetPageListSUMDepartmentId(parameter.queryJson, deps);
                if (data.ToList().Count > 0)
                {
                    foreach (var info in data)
                    {
                        AmountSUM = AmountSUM + info.Amount;
                    }
                }
            }

            var result = new
            {
                AmountSum = AmountSUM,

            };
            var jsonData = new
            {
                rows = result
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetProjectPayCollectionAllDepartmentId(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
            List<ProjectPayCollectionVo> list = new List<ProjectPayCollectionVo>();

            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( pc.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( pc.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = projectPayCollectionIBLL.GetPageListSUMDepartmentId(parameter.queryJson, deps);

                foreach (var info in data)
                {
                    //创建时间
                    DateTime time = (DateTime)info.CreateTime;
                    info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                    //到款时间
                    DateTime time1 = (DateTime)info.ReceiptDate;
                    info.ReceiptDateMd = time1.ToString("yyyy-MM-dd");
                    //项目来源
                    var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                    if (projectSource != null)
                    {
                        info.ProjectSourceName = projectSource.F_ItemName;
                    }
                    //创建人
                    var createUser = userIBLL.GetFollowPersonNameByUserId(info.CreateUser);
                    if (createUser != null)
                    {
                        info.CreateUserName = createUser.F_RealName;
                    }
                    //营销部门
                    var department = departmentIBLL.GetEntity(info.DepartmentId);
                    if (department != null)
                    {
                        info.DepartmentName = department.F_FullName;
                    }
                    list.Add(info);
                }
            }
            list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 付款管理多部门
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetProjectPaymentListDepartmentId(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
            List<ProjectPaymentVo> list = new List<ProjectPaymentVo>();

            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( a.FDepartmentId='" + strList[i] + "' or a.DepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";
                    }
                    else
                    {
                        deps += " or ( a.FDepartmentId='" + strList[i] + "' or a.DepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";
                    }

                }
                var data = projectPaymentIBLL.GetPageListDepartmentId(parameter.pagination, parameter.queryJson, deps);
                foreach (var info in data)
                {
                    //创建时间
                    DateTime time = (DateTime)info.CreateTime;
                    info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                    //项目来源
                    DataItemDetailEntity projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                    if (projectSource != null)
                    {
                        info.ProjectSourceName = projectSource.F_ItemName;

                    }
                    //创建人
                    var createUser = userIBLL.GetFollowPersonNameByUserId(info.CreateUser);
                    if (createUser != null)
                    {
                        info.CreateUserName = createUser.F_RealName;
                    }
                    //付款类型
                    var payType = dataItemBLL.GetDetailItemName(info.PayType, "PayType");
                    if (payType != null)
                    {
                        info.PayTypeName = payType.F_ItemName;
                    }

                    //所属部门
                    var department = departmentIBLL.GetEntity(info.DepartmentId);
                    if (department != null)
                    {
                        info.DepartmentName = department.F_FullName;
                    }
                    //支付方式
                    //var paymentMethod = dataItemBLL.GetDetailItemName(info.PaymentMethod, "Client_PaymentMode");
                    //if (paymentMethod != null)
                    //{
                    //    info.PaymentMethodName = paymentMethod.F_ItemName;
                    //}
                    ////支付状态
                    //var billingStatus = dataItemBLL.GetDetailItemName(info.PaymentStatus, "PaymentStatus");
                    //if (billingStatus != null)
                    //{
                    //    info.PaymentStatusName = billingStatus.F_ItemName;
                    //}
                    ////支付抬头
                    //var paymentHeader = dataItemBLL.GetDetailItemName(info.PaymentHeader, "PaymentHeader");
                    //if (paymentHeader != null)
                    //{
                    //    info.PaymentHeaderName = paymentHeader.F_ItemName;
                    //}

                    list.Add(info);
                }

            }


            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };

            return Success(jsonData);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetProjectPaymentAllDepartmentId(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
            List<ProjectPaymentVo> list = new List<ProjectPaymentVo>();

            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " (  a.FDepartmentId='" + strList[i] + "' or a.DepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";
                    }
                    else
                    {
                        deps += " or ( a.FDepartmentId='" + strList[i] + "' or a.DepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + userInfo.userId + "') ";
                    }

                }
                var data = projectPaymentIBLL.GetPageListDepartmentId(parameter.queryJson, deps);
                foreach (var info in data)
                {
                    //创建时间
                    DateTime time = (DateTime)info.CreateTime;
                    info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                    //项目来源
                    DataItemDetailEntity projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                    if (projectSource != null)
                    {
                        info.ProjectSourceName = projectSource.F_ItemName;

                    }
                    //创建人
                    var createUser = userIBLL.GetFollowPersonNameByUserId(info.CreateUser);
                    if (createUser != null)
                    {
                        info.CreateUserName = createUser.F_RealName;
                    }
                    //付款类型
                    var payType = dataItemBLL.GetDetailItemName(info.PayType, "PayType");
                    if (payType != null)
                    {
                        info.PayTypeName = payType.F_ItemName;
                    }

                    //所属部门
                    var department = departmentIBLL.GetEntity(info.DepartmentId);
                    if (department != null)
                    {
                        info.DepartmentName = department.F_FullName;
                    }
                    //支付方式
                    //var paymentMethod = dataItemBLL.GetDetailItemName(info.PaymentMethod, "Client_PaymentMode");
                    //if (paymentMethod != null)
                    //{
                    //    info.PaymentMethodName = paymentMethod.F_ItemName;
                    //}
                    ////支付状态
                    //var billingStatus = dataItemBLL.GetDetailItemName(info.PaymentStatus, "PaymentStatus");
                    //if (billingStatus != null)
                    //{
                    //    info.PaymentStatusName = billingStatus.F_ItemName;
                    //}
                    ////支付抬头
                    //var paymentHeader = dataItemBLL.GetDetailItemName(info.PaymentHeader, "PaymentHeader");
                    //if (paymentHeader != null)
                    //{
                    //    info.PaymentHeaderName = paymentHeader.F_ItemName;
                    //}

                    list.Add(info);
                }

            }



            list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 多部门行政付款
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetPaymentListDepartmentId(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
            List<PaymentVo> list = new List<PaymentVo>();

            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( a.DepartmentId='" + strList[i] + "' or a.CreateUser='" + userInfo.userId + "') ";
                    }
                    else
                    {
                        deps += " or (a.DepartmentId='" + strList[i] + "' or a.CreateUser='" + userInfo.userId + "') ";
                    }

                }
                var data = paymentIBLL.GetPageListDepartmentId(parameter.pagination, parameter.queryJson, deps);
                foreach (var info in data)
                {
                    //创建时间
                    DateTime time = (DateTime)info.CreateTime;
                    info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                    //创建人
                    var createUser = userIBLL.GetFollowPersonNameByUserId(info.CreateUser);
                    if (createUser != null)
                    {
                        info.CreateUserName = createUser.F_RealName;
                    }
                    //提审人
                    var contractSubmitter = userIBLL.GetFollowPersonNameByUserId(info.ContractSubmitter);
                    if (contractSubmitter != null)
                    {
                        info.ContractSubmitterName = contractSubmitter.F_RealName;
                    }
                    //当前提审人
                    info.PaymentSubmitterName = "";
                    var taskNode = nWFProcessIBLL.GetTaskUserList_NodbWhere(info.WorkFlowId);
                    if (taskNode != null)
                    {
                        if (taskNode.ToList().Count > 0)
                        {
                            var taskInfo = taskNode.ToList().OrderByDescending(i => i.F_CreateDate).FirstOrDefault();

                            NWFTaskEntity t = new NWFTaskEntity();
                            if (taskInfo.nWFUserInfoList[0].Id != null)
                            {
                                UserEntity followPerson = userIBLL.GetFollowPersonNameByUserId(taskInfo.nWFUserInfoList[0].Id);
                                if (followPerson != null)
                                {
                                    info.PaymentSubmitterName = followPerson.F_RealName;
                                }
                            }
                        }
                    }
                    //var paymentSubmitter = userIBLL.GetFollowPersonNameByUserId(info.PaymentSubmitter);
                    //if (paymentSubmitter != null)
                    //{
                    //    info.PaymentSubmitterName = paymentSubmitter.F_RealName;
                    //}
                    //开票内容
                    var payType = dataItemBLL.GetDetailItemName(info.PayType, "PaymentType");
                    if (payType != null)
                    {
                        info.PayTypeName = payType.F_ItemName;
                    }

                    //所属部门
                    var department = departmentIBLL.GetEntity(info.DepartmentId);
                    if (department != null)
                    {
                        info.DepartmentName = department.F_FullName;
                    }
                    //支付类型
                    //var paymentMethod = dataItemBLL.GetDetailItemName(info.PaymentMethod, "Client_PaymentMode");
                    //if (paymentMethod != null)
                    //{
                    //    info.PaymentMethodName = paymentMethod.F_ItemName;
                    //}
                    ////支付状态
                    //var billingStatus = dataItemBLL.GetDetailItemName(info.PaymentStatus, "PaymentStatus");
                    //if (billingStatus != null)
                    //{
                    //    info.PaymentStatusName = billingStatus.F_ItemName;
                    //}
                    list.Add(info);
                }

            }


            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };

            return Success(jsonData);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetPaymentAllDepartmentId(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
            List<PaymentVo> list = new List<PaymentVo>();

            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( a.DepartmentId='" + strList[i] + "' or a.CreateUser='" + userInfo.userId + "') ";
                    }
                    else
                    {
                        deps += " or (a.DepartmentId='" + strList[i] + "' or a.CreateUser='" + userInfo.userId + "') ";
                    }

                }
                var data = paymentIBLL.GetPageListDepartmentId(parameter.pagination, parameter.queryJson, deps);
                foreach (var info in data)
                {
                    //创建时间
                    DateTime time = (DateTime)info.CreateTime;
                    info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                    //创建人
                    var createUser = userIBLL.GetFollowPersonNameByUserId(info.CreateUser);
                    if (createUser != null)
                    {
                        info.CreateUserName = createUser.F_RealName;
                    }
                    //提审人
                    var contractSubmitter = userIBLL.GetFollowPersonNameByUserId(info.ContractSubmitter);
                    if (contractSubmitter != null)
                    {
                        info.ContractSubmitterName = contractSubmitter.F_RealName;
                    }
                    //审核人
                    //var paymentSubmitter = userIBLL.GetFollowPersonNameByUserId(info.PaymentSubmitter);
                    //if (paymentSubmitter != null)
                    //{
                    //    info.PaymentSubmitterName = paymentSubmitter.F_RealName;
                    //}
                    //当前提审人
                    info.PaymentSubmitterName = "";
                    var taskNode = nWFProcessIBLL.GetTaskUserList_NodbWhere(info.WorkFlowId);
                    if (taskNode != null)
                    {
                        if (taskNode.ToList().Count > 0)
                        {
                            var taskInfo = taskNode.ToList().OrderByDescending(i => i.F_CreateDate).FirstOrDefault();

                            NWFTaskEntity t = new NWFTaskEntity();
                            if (taskInfo.nWFUserInfoList[0].Id != null)
                            {
                                UserEntity followPerson = userIBLL.GetFollowPersonNameByUserId(taskInfo.nWFUserInfoList[0].Id);
                                if (followPerson != null)
                                {
                                    info.PaymentSubmitterName = followPerson.F_RealName;
                                }
                            }
                        }
                    }
                    //开票内容
                    var payType = dataItemBLL.GetDetailItemName(info.PayType, "PaymentType");
                    if (payType != null)
                    {
                        info.PayTypeName = payType.F_ItemName;
                    }

                    //所属部门
                    var department = departmentIBLL.GetEntity(info.DepartmentId);
                    if (department != null)
                    {
                        info.DepartmentName = department.F_FullName;
                    }
                    //支付类型
                    //var paymentMethod = dataItemBLL.GetDetailItemName(info.PaymentMethod, "Client_PaymentMode");
                    //if (paymentMethod != null)
                    //{
                    //    info.PaymentMethodName = paymentMethod.F_ItemName;
                    //}
                    ////支付状态
                    //var billingStatus = dataItemBLL.GetDetailItemName(info.PaymentStatus, "PaymentStatus");
                    //if (billingStatus != null)
                    //{
                    //    info.PaymentStatusName = billingStatus.F_ItemName;
                    //}
                    list.Add(info);
                }

            }


            list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);
        }
        ///营销台账多部门
        public Response GetMarketingsDepartmentId(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
            List<MarketingEntity> data = new List<MarketingEntity>();
            List<MarketingEntity> list = new List<MarketingEntity>();
            var departmentList = departmentIBLL.GetEntityList();
            var userList = userIBLL.GetAllList();
            string deps = " 1 = 1 ";
            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + strList[i] + "') ";
                    }

                }
                data = reportFormsBLL.GetMarketingsDepartmentId(parameter.pagination, parameter.queryJson, deps).ToList();
            }
            else if (userInfo.userId == "1e5dfa6a-6f0c-454c-b1ac-aeafef95aea5" || userInfo.userId == "System")
            {
                data = reportFormsBLL.GetMarketingsDepartmentId(parameter.pagination, parameter.queryJson, deps).ToList();
            }
            foreach (var info in data)
            {
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //审核时间               
                if (info.ApproverTime != null)
                {
                    DateTime time1 = (DateTime)info.ApproverTime;
                    info.ApproverTimeMd = time1.ToString("yyyy-MM-dd");
                }
                //到款日期               
                if (info.ReceiptDate != null)
                {
                    DateTime time1 = (DateTime)info.ReceiptDate;
                    info.ReceiptDateMd = time1.ToString("yyyy-MM-dd");
                }
                //检测时间               
                //if (info.ApproachTime != null)
                //{
                //    DateTime time1 = (DateTime)info.ApproachTime;
                //    info.ApproachTimeMd = time1.ToString("yyyy-MM-dd");
                //}
                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                if (followPerson != null)
                {
                    info.FollowPersonName = followPerson.F_RealName;

                }
                //项目负责人
                if (!string.IsNullOrEmpty(info.ProjectResponsible))
                {
                    var item_list = info.ProjectResponsible.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (item_list.Count > 0)
                    {
                        for (int i = 0; i < item_list.Count; i++)
                        {
                            var Items = userList.Where(ii => ii.F_UserId == item_list[i]).ToList();
                            if (Items.Count > 0)
                            {
                                info.ProjectResponsibleName = info.ProjectResponsibleName + Items[0].F_RealName;
                                if (i < item_list.Count - 1)
                                {
                                    info.ProjectResponsibleName += ";";
                                }
                            }
                        }
                    }
                }
                //部门
                if (!string.IsNullOrEmpty(info.DepartmentId))
                {
                    var item_list = info.DepartmentId.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (item_list.Count > 0)
                    {
                        for (int i = 0; i < item_list.Count; i++)
                        {
                            var Items = departmentList.Where(ii => ii.F_DepartmentId == item_list[i]).ToList();
                            if (Items.Count > 0)
                            {
                                info.DepartmentName = info.DepartmentName + Items[0].F_FullName;
                                if (i < item_list.Count - 1)
                                {
                                    info.DepartmentName += ";";
                                }
                            }
                        }
                    }
                }
                //实施部门
                var department1 = departmentIBLL.GetEntity(info.J_F_FullName);
                if (department1 != null)
                {
                    info.J_F_FullName = department1.F_FullName;
                }
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }
                //报告主体
                if (!string.IsNullOrEmpty(info.ReportSubject))
                {
                    var item_list = info.ReportSubject.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (item_list.Count > 0)
                    {
                        for (int i = 0; i < item_list.Count; i++)
                        {
                            var Item = dataItemBLL.GetDetailItemName(item_list[i], "ContractSubject");
                            if (Item != null)
                            {
                                info.ReportSubjectName = info.ReportSubjectName + Item.F_ItemName;
                                if (i < item_list.Count - 1)
                                {
                                    info.ReportSubjectName += ";";
                                }
                            }
                        }
                    }
                }
                //报告状态
                if (!string.IsNullOrEmpty(info.TaskStatus))
                {
                    var TaskStatusName_list = info.TaskStatus.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (TaskStatusName_list.Count > 0)
                    {
                        for (int i = 0; i < TaskStatusName_list.Count; i++)
                        {
                            var TaskStatusName = dataItemBLL.GetDetailItemName(TaskStatusName_list[i], "TaskStatus");
                            if (TaskStatusName != null)
                            {
                                info.TaskStatusName = info.TaskStatusName + TaskStatusName.F_ItemName;
                                if (i < TaskStatusName_list.Count - 1)
                                {
                                    info.TaskStatusName += ";";
                                }
                            }
                        }
                    }
                }
                //归档情况
                if (info.ReceivedFlag.ToInt() != 0)
                {
                    info.ReceivedFlagName = "已归档";
                }
                else
                {
                    info.ReceivedFlagName = "未归档";
                }
                //开票情况
                if (info.BillingStatus.ToInt() != 0)
                {
                    info.BillingStatusName = "已开票";
                }
                else
                {
                    info.BillingStatusName = "未开票";
                }
                //营销部门
                if (info.MainDepartmentId != null)
                {
                    var department2 = departmentIBLL.GetEntity(info.MainDepartmentId);
                    if (department2 != null)
                    {
                        info.MainDepartmentName = department2.F_FullName;
                    }
                }
                //营销部门
                if (info.SubDepartmentId != null)
                {
                    var department3 = departmentIBLL.GetEntity(info.SubDepartmentId);
                    if (department3 != null)
                    {
                        info.SubDepartmentName = department3.F_FullName;
                    }
                }
                list.Add(info);
            }
            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };

            return Success(jsonData);
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetMarketingsDepartmentIdAll(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
            List<MarketingEntity> data = new List<MarketingEntity>();
            List<MarketingEntity> list = new List<MarketingEntity>();
            var departmentList = departmentIBLL.GetEntityList();
            var userList = userIBLL.GetAllList();
            string deps = " 1 = 1 ";
            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + strList[i] + "') ";
                    }

                }
                data = reportFormsBLL.GetMarketingsSum_newDepartmentId(parameter.queryJson, deps).ToList();
            }
            else if (userInfo.userId == "1e5dfa6a-6f0c-454c-b1ac-aeafef95aea5" || userInfo.userId == "System")
            {
                data = reportFormsBLL.GetMarketingsSum_newDepartmentId(parameter.queryJson, deps).ToList();
            }
            foreach (var info in data)
            {
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //审核时间               
                if (info.ApproverTime != null)
                {
                    DateTime time1 = (DateTime)info.ApproverTime;
                    info.ApproverTimeMd = time1.ToString("yyyy-MM-dd");
                }
                //到款日期               
                if (info.ReceiptDate != null)
                {
                    DateTime time1 = (DateTime)info.ReceiptDate;
                    info.ReceiptDateMd = time1.ToString("yyyy-MM-dd");
                }
                //检测时间               
                //if (info.ApproachTime != null)
                //{
                //    DateTime time1 = (DateTime)info.ApproachTime;
                //    info.ApproachTimeMd = time1.ToString("yyyy-MM-dd");
                //}
                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                if (followPerson != null)
                {
                    info.FollowPersonName = followPerson.F_RealName;

                }
                //项目负责人
                if (!string.IsNullOrEmpty(info.ProjectResponsible))
                {
                    var item_list = info.ProjectResponsible.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (item_list.Count > 0)
                    {
                        for (int i = 0; i < item_list.Count; i++)
                        {
                            var Items = userList.Where(ii => ii.F_UserId == item_list[i]).ToList();
                            if (Items.Count > 0)
                            {
                                info.ProjectResponsibleName = info.ProjectResponsibleName + Items[0].F_RealName;
                                if (i < item_list.Count - 1)
                                {
                                    info.ProjectResponsibleName += ";";
                                }
                            }
                        }
                    }
                }
                //部门
                if (!string.IsNullOrEmpty(info.DepartmentId))
                {
                    var item_list = info.DepartmentId.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (item_list.Count > 0)
                    {
                        for (int i = 0; i < item_list.Count; i++)
                        {
                            var Items = departmentList.Where(ii => ii.F_DepartmentId == item_list[i]).ToList();
                            if (Items.Count > 0)
                            {
                                info.DepartmentName = info.DepartmentName + Items[0].F_FullName;
                                if (i < item_list.Count - 1)
                                {
                                    info.DepartmentName += ";";
                                }
                            }
                        }
                    }
                }
                //实施部门
                var department1 = departmentIBLL.GetEntity(info.J_F_FullName);
                if (department1 != null)
                {
                    info.J_F_FullName = department1.F_FullName;
                }
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }
                //报告主体
                if (!string.IsNullOrEmpty(info.ReportSubject))
                {
                    var item_list = info.ReportSubject.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (item_list.Count > 0)
                    {
                        for (int i = 0; i < item_list.Count; i++)
                        {
                            var Item = dataItemBLL.GetDetailItemName(item_list[i], "ContractSubject");
                            if (Item != null)
                            {
                                info.ReportSubjectName = info.ReportSubjectName + Item.F_ItemName;
                                if (i < item_list.Count - 1)
                                {
                                    info.ReportSubjectName += ";";
                                }
                            }
                        }
                    }
                }
                //报告状态
                if (!string.IsNullOrEmpty(info.TaskStatus))
                {
                    var TaskStatusName_list = info.TaskStatus.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (TaskStatusName_list.Count > 0)
                    {
                        for (int i = 0; i < TaskStatusName_list.Count; i++)
                        {
                            var TaskStatusName = dataItemBLL.GetDetailItemName(TaskStatusName_list[i], "TaskStatus");
                            if (TaskStatusName != null)
                            {
                                info.TaskStatusName = info.TaskStatusName + TaskStatusName.F_ItemName;
                                if (i < TaskStatusName_list.Count - 1)
                                {
                                    info.TaskStatusName += ";";
                                }
                            }
                        }
                    }
                }
                //归档情况
                if (info.ReceivedFlag.ToInt() != 0)
                {
                    info.ReceivedFlagName = "已归档";
                }
                else
                {
                    info.ReceivedFlagName = "未归档";
                }
                //开票情况
                if (info.BillingStatus.ToInt() != 0)
                {
                    info.BillingStatusName = "已开票";
                }
                else
                {
                    info.BillingStatusName = "未开票";
                }
                //营销部门
                if (info.MainDepartmentId != null)
                {
                    var department2 = departmentIBLL.GetEntity(info.MainDepartmentId);
                    if (department2 != null)
                    {
                        info.MainDepartmentName = department2.F_FullName;
                    }
                }
                //营销部门
                if (info.SubDepartmentId != null)
                {
                    var department3 = departmentIBLL.GetEntity(info.SubDepartmentId);
                    if (department3 != null)
                    {
                        info.SubDepartmentName = department3.F_FullName;
                    }
                }
                list.Add(info);
            }
            list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 合计
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetMarketingsSumDepartmentId(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
            MarketingEntity q = new MarketingEntity();
            q.ContractAmountSum = 0;
            q.AmountSum = 0;
            q.NotReceivedSum = 0;
            q.OwnSum = 0;
            q.DitchSum = 0;
            q.ConsociationSum = 0;
            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + strList[i] + "') ";
                    }

                }
                var data = reportFormsBLL.GetMarketingsSum_newDepartmentId(parameter.queryJson, deps);
                foreach (var item in data)
                {

                    if (item.ContractAmount != null)
                    {
                        q.ContractAmountSum = q.ContractAmountSum + item.ContractAmount;
                    }
                    if (item.Amount != null)
                    {
                        q.AmountSum = q.AmountSum + item.Amount;
                    }
                    if (item.NotReceived != null)
                    {
                        q.NotReceivedSum = q.NotReceivedSum + item.NotReceived;
                    }

                    if (item.ProjectSource == 1.ToString() && item.ContractAmount.HasValue)
                    {
                        q.OwnSum = q.OwnSum + item.ContractAmount;
                    }
                    if (item.ProjectSource == 2.ToString() && item.ContractAmount.HasValue)
                    {
                        q.DitchSum = q.DitchSum + item.ContractAmount;
                    }
                    if (item.ProjectSource == 3.ToString() && item.ContractAmount.HasValue)
                    {
                        q.ConsociationSum = q.ConsociationSum + item.ContractAmount;
                    }
                }

            }


            var result = new
            {
                ContractAmountSum = q.ContractAmountSum,
                AmountSum = q.AmountSum,
                NotReceivedSum = q.NotReceivedSum,
                OwnSum = q.OwnSum,
                DitchSum = q.DitchSum,
                ConsociationSum = q.ConsociationSum
            };
            var jsonData = new
            {
                rows = result
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 生产台账
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetProductionsDepartmentId(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
            List<ProductionEntity> data = new List<ProductionEntity>();
            List<ProductionEntity> list = new List<ProductionEntity>();
            string deps = " 1 = 1 ";
            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + strList[i] + "') ";
                    }

                }
                data = reportFormsBLL.GetProductionsDepartmentId(parameter.pagination, parameter.queryJson, deps).ToList();
            }
            else if (userInfo.userId == "1e5dfa6a-6f0c-454c-b1ac-aeafef95aea5" || userInfo.userId == "System")
            {
                data = reportFormsBLL.GetProductionsDepartmentId(parameter.pagination, parameter.queryJson, deps).ToList();
            }
            foreach (var info in data)
            {
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //到款日期               
                if (info.ReceiptDate != null)
                {
                    DateTime time1 = (DateTime)info.ReceiptDate;
                    info.ReceiptDateMd = time1.ToString("yyyy-MM-dd");
                }
                //检测时间               
                if (info.ApproachTime != null)
                {
                    DateTime time1 = (DateTime)info.ApproachTime;
                    info.ApproachTimeMd = time1.ToString("yyyy-MM-dd");
                }
                //报告时间               
                if (info.FlowFinishedTime != null)
                {
                    DateTime time1 = (DateTime)info.FlowFinishedTime;
                    info.FlowFinishedTimeMd = time1.ToString("yyyy-MM-dd");
                }
                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                if (followPerson != null)
                {
                    info.FollowPersonName = followPerson.F_RealName;

                }
                //项目负责人
                var projectResponsible = userIBLL.GetEntityByUserId(info.ProjectResponsible);
                if (projectResponsible != null)
                {
                    info.ProjectResponsibleName = projectResponsible.F_RealName;
                }
                //部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }
                //实施部门
                var department1 = departmentIBLL.GetEntity(info.J_F_FullName);
                if (department1 != null)
                {
                    info.J_F_FullName = department1.F_FullName;
                }
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }
                //报告主体
                var reportSubject = dataItemBLL.GetDetailItemName(info.ReportSubject, "ContractSubject");
                if (reportSubject != null)
                {
                    info.ReportSubjectName = reportSubject.F_ItemName;
                }
                //合同主体
                var contractSubject = dataItemBLL.GetDetailItemName(info.ContractSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    info.ContractSubjectName = contractSubject.F_ItemName;
                }
                //报告状态
                var taskStatus = dataItemBLL.GetDetailItemName(info.TaskStatus, "TaskStatus");
                if (taskStatus != null)
                {
                    info.TaskStatusName = taskStatus.F_ItemName;
                }
                //归档情况
                if (info.ReceivedFlag.ToInt() != 0)
                {
                    info.ReceivedFlagName = "已归档";
                }
                else
                {
                    info.ReceivedFlagName = "未归档";
                }
                //开票情况
                if (info.BillingStatus.ToInt() != 0)
                {
                    info.BillingStatusName = "已开票";
                }
                else
                {
                    info.BillingStatusName = "未开票";
                }
                //营销部门
                if (info.MainDepartmentId != null)
                {
                    var department2 = departmentIBLL.GetEntity(info.MainDepartmentId);
                    if (department2 != null)
                    {
                        info.MainDepartmentName = department2.F_FullName;
                    }
                }
                //营销部门
                if (info.SubDepartmentId != null)
                {
                    var department3 = departmentIBLL.GetEntity(info.SubDepartmentId);
                    if (department3 != null)
                    {
                        info.SubDepartmentName = department3.F_FullName;
                    }
                }
                list.Add(info);
            }
            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };

            return Success(jsonData);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetProductionsDepartmentIdAll(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
            List<ProductionEntity> data = new List<ProductionEntity>();
            List<ProductionEntity> list = new List<ProductionEntity>();
            string deps = " 1 = 1 ";

            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + strList[i] + "') ";
                    }

                }
                data = reportFormsBLL.GetProductionsDepartmentId(parameter.queryJson, deps).ToList();

            }
            else if (userInfo.userId == "1e5dfa6a-6f0c-454c-b1ac-aeafef95aea5" || userInfo.userId == "System")
            {
                data = reportFormsBLL.GetProductionsDepartmentId(parameter.queryJson, deps).ToList();
            }
            foreach (var info in data)
            {
                if (info.CreateTime != null)
                {
                    //创建时间
                    DateTime time = (DateTime)info.CreateTime;
                    info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                }
                //到款日期               
                if (info.ReceiptDate != null)
                {
                    DateTime time1 = (DateTime)info.ReceiptDate;
                    info.ReceiptDateMd = time1.ToString("yyyy-MM-dd");
                }
                //检测时间               
                if (info.ApproachTime != null)
                {
                    DateTime time1 = (DateTime)info.ApproachTime;
                    info.ApproachTimeMd = time1.ToString("yyyy-MM-dd");
                }
                //报告时间               
                if (info.FlowFinishedTime != null)
                {
                    DateTime time1 = (DateTime)info.FlowFinishedTime;
                    info.FlowFinishedTimeMd = time1.ToString("yyyy-MM-dd");
                }
                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                if (followPerson != null)
                {
                    info.FollowPersonName = followPerson.F_RealName;

                }
                //项目负责人
                var projectResponsible = userIBLL.GetEntityByUserId(info.ProjectResponsible);
                if (projectResponsible != null)
                {
                    info.ProjectResponsibleName = projectResponsible.F_RealName;
                }
                //部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }
                //实施部门
                var department1 = departmentIBLL.GetEntity(info.J_F_FullName);
                if (department1 != null)
                {
                    info.J_F_FullName = department1.F_FullName;
                }
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }
                //报告主体
                var reportSubject = dataItemBLL.GetDetailItemName(info.ReportSubject, "ContractSubject");
                if (reportSubject != null)
                {
                    info.ReportSubjectName = reportSubject.F_ItemName;
                }
                //合同主体
                var contractSubject = dataItemBLL.GetDetailItemName(info.ContractSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    info.ContractSubjectName = contractSubject.F_ItemName;
                }
                //报告状态
                var taskStatus = dataItemBLL.GetDetailItemName(info.TaskStatus, "TaskStatus");
                if (taskStatus != null)
                {
                    info.TaskStatusName = taskStatus.F_ItemName;
                }
                //归档情况
                if (info.ReceivedFlag.ToInt() != 0)
                {
                    info.ReceivedFlagName = "已归档";
                }
                else
                {
                    info.ReceivedFlagName = "未归档";
                }
                //开票情况
                if (info.BillingStatus.ToInt() != 0)
                {
                    info.BillingStatusName = "已开票";
                }
                else
                {
                    info.BillingStatusName = "未开票";
                }
                //营销部门
                if (info.MainDepartmentId != null)
                {
                    var department2 = departmentIBLL.GetEntity(info.MainDepartmentId);
                    if (department2 != null)
                    {
                        info.MainDepartmentName = department2.F_FullName;
                    }
                }
                //营销部门
                if (info.SubDepartmentId != null)
                {
                    var department3 = departmentIBLL.GetEntity(info.SubDepartmentId);
                    if (department3 != null)
                    {
                        info.SubDepartmentName = department3.F_FullName;
                    }
                }
                list.Add(info);
            }

            list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 合计
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetProductionsSumDepartmentId(dynamic _)
        {
            decimal ContractAmountSum = 0;
            decimal OwnSum = 0;
            decimal DitchSum = 0;
            decimal ConsociationSum = 0;


            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);

            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + strList[i] + "') ";
                    }

                }
                var data = reportFormsBLL.GetProductionsDepartmentId(parameter.queryJson, deps);
                foreach (var item in data)
                {

                    ContractAmountSum = ContractAmountSum + item.ContractAmount.Value;
                    if (item.ProjectSource == 1.ToString() && item.ContractAmount.HasValue)
                    {
                        OwnSum = OwnSum + item.ContractAmount.Value;
                    }
                    if (item.ProjectSource == 2.ToString() && item.ContractAmount.HasValue)
                    {
                        DitchSum = DitchSum + item.ContractAmount.Value;
                    }
                    if (item.ProjectSource == 3.ToString() && item.ContractAmount.HasValue)
                    {
                        ConsociationSum = ConsociationSum + item.ContractAmount.Value;
                    }
                }

            }

            var result = new
            {
                ContractAmountSum = ContractAmountSum,
                OwnSum = OwnSum,
                DitchSum = DitchSum,
                ConsociationSum = ConsociationSum
            };
            var jsonData = new
            {
                rows = result
            };
            return Success(jsonData);

        }
        //结算台账多部门
        public Response GetSettleAccountsDepartmentId(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
            List<SettleAccountsEntity> data = new List<SettleAccountsEntity>();
            List<SettleAccountsEntity> list = new List<SettleAccountsEntity>();
            string deps = " 1 = 1 ";
            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + strList[i] + "') ";
                    }
                }
                data = reportFormsBLL.GetSettleAccountsDepartmentId(parameter.pagination, parameter.queryJson, deps).ToList();
            }
            else if (userInfo.userId == "1e5dfa6a-6f0c-454c-b1ac-aeafef95aea5" || userInfo.userId == "System")
            {
                data = reportFormsBLL.GetSettleAccountsDepartmentId(parameter.pagination, parameter.queryJson, deps).ToList();
            }
            foreach (var info in data)
            {
                //创建时间
                if (info.CreateTime != null)
                {
                    DateTime time = (DateTime)info.CreateTime;
                    info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                }
                //审核时间               
                if (info.ApproverTime != null)
                {
                    DateTime time1 = (DateTime)info.ApproverTime;
                    info.ApproverTimeMd = time1.ToString("yyyy-MM-dd");
                }
                //到款日期               
                if (info.ReceiptDate != null)
                {
                    DateTime time1 = (DateTime)info.ReceiptDate;
                    info.ReceiptDateMd = time1.ToString("yyyy-MM-dd");
                }

                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                if (followPerson != null)
                {
                    info.FollowPersonName = followPerson.F_RealName;

                }
                //项目负责人
                var projectResponsible = userIBLL.GetEntityByUserId(info.ProjectResponsible);
                if (projectResponsible != null)
                {
                    info.ProjectResponsibleName = projectResponsible.F_RealName;
                }
                //部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }
                //实施部门
                var department1 = departmentIBLL.GetEntity(info.J_F_FullName);
                if (department1 != null)
                {
                    info.J_F_FullName = department1.F_FullName;
                }
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }

                //合同主体
                var contractSubject = dataItemBLL.GetDetailItemName(info.ContractSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    info.ContractSubjectName = contractSubject.F_ItemName;
                }
                //报告状态
                var taskStatus = dataItemBLL.GetDetailItemName(info.TaskStatus, "TaskStatus");
                if (taskStatus != null)
                {
                    info.TaskStatusName = taskStatus.F_ItemName;
                }

                //核算1
                //自主营销
                if (info.ProjectSource == "1")
                {
                    if (info.PaymentAmount == null)
                    {

                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.02;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                    else if (info.PaymentAmount < (info.ContractAmount * (decimal?)0.3))
                    {
                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.005;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                    else
                    {
                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.02;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                }
                //渠道营销
                if (info.ProjectSource == "2")
                {
                    if (info.PaymentAmount == null)
                    {

                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.015;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                    else if (info.PaymentAmount < (info.ContractAmount * (decimal?)0.3))
                    {
                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.002;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                    else
                    {
                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.001;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                }
                //营销核算

                //自主营销
                if (info.ProjectSource == "1")
                {
                    if (info.PaymentAmount == null)
                    {
                        info.DepartmentIdAmount = info.ContractAmount * (decimal?)0.3;
                        if (info.DepartmentIdAmount != null)
                        {
                            //info.DepartmentIdAmountName = Math.Round((decimal)info.DepartmentIdAmount * 100) / 100;
                            info.DepartmentIdAmountName = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                        }
                    }
                    else if (info.PaymentAmount < (info.ContractAmount * (decimal?)0.3))
                    {
                        info.DepartmentIdAmount = (info.ContractAmount * (decimal?)0.3) - info.PaymentAmount;
                        if (info.DepartmentIdAmount != null)
                        {
                            info.DepartmentIdAmountName = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                        }
                    }
                    else
                    {
                        info.DepartmentIdAmount = info.ContractAmount * (decimal?)0.03;
                        if (info.DepartmentIdAmount != null)
                        {
                            info.DepartmentIdAmountName = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                        }
                    }
                }
                //生产核算
                if (info.ProjectSource == "1" || info.ProjectSource == "2")
                {
                    if (info.TaskStatus == "3" || info.TaskStatus == "4" || info.TaskStatus == "9" || info.TaskStatus == "5")
                    {
                        if (info.PaymentAmount != null)
                        {
                            if (info.PaymentAmount >= (info.ContractAmount * (decimal?)0.3))
                            {
                                if (info.SubAmount != null && info.MainAmount == null)
                                {
                                    info.DepartmentIdAmount = (info.ContractAmount - info.PaymentAmount - info.SubAmount) * (decimal?)0.3;
                                    if (info.DepartmentIdAmount != null)
                                    {
                                        info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                    }
                                }
                                else if (info.SubAmount == null && info.MainAmount != null)
                                {
                                    info.DepartmentIdAmount = (info.ContractAmount - info.PaymentAmount - info.MainAmount) * (decimal?)0.3;
                                    if (info.DepartmentIdAmount != null)
                                    {
                                        info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                    }
                                }
                                else
                                {
                                    info.DepartmentIdAmount = (info.ContractAmount - info.PaymentAmount) * (decimal?)0.3;
                                    if (info.DepartmentIdAmount != null)
                                    {
                                        info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                    }
                                }
                            }
                            else if (info.PaymentAmount < (info.ContractAmount * (decimal?)0.3))
                            {
                                info.DepartmentIdAmount = info.ContractAmount * (decimal?)0.2;
                                if (info.DepartmentIdAmount != null)
                                {
                                    info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                }
                                //item.DepartmentIdAmount = decimal.Round(decimal.Parse((item.ContractAmount * (decimal?)0.2).ToString()), 2);
                            }
                        }
                        else
                        {
                            if (info.ContractAmount != null)
                            {
                                info.DepartmentIdAmount = (info.ContractAmount * (decimal?)0.2);
                                if (info.DepartmentIdAmount != null)
                                {
                                    info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                }
                            }
                        }


                    }

                }
                //归档情况
                if (info.ReceivedFlag.ToInt() != 0)
                {
                    info.ReceivedFlagName = "已归档";
                }
                else
                {
                    info.ReceivedFlagName = "未归档";
                }

                list.Add(info);
            }
            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };

            return Success(jsonData);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetSettleAccountsSumAllDepartmentId(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
            List<SettleAccountsEntity> data = new List<SettleAccountsEntity>();
            List<SettleAccountsEntity> list = new List<SettleAccountsEntity>();
            string deps = " 1 = 1 ";

            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + strList[i] + "') ";
                    }

                }
                data = reportFormsBLL.GetSettleAccountsSum_newDepartmentId(parameter.queryJson, deps).ToList();

            }
            else if (userInfo.userId == "1e5dfa6a-6f0c-454c-b1ac-aeafef95aea5" || userInfo.userId == "System")
            {
                data = reportFormsBLL.GetSettleAccountsSum_newDepartmentId(parameter.queryJson, deps).ToList();
            }
            foreach (var info in data)
            {
                //创建时间
                if (info.CreateTime != null)
                {
                    DateTime time = (DateTime)info.CreateTime;
                    info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                }
                //审核时间               
                if (info.ApproverTime != null)
                {
                    DateTime time1 = (DateTime)info.ApproverTime;
                    info.ApproverTimeMd = time1.ToString("yyyy-MM-dd");
                }
                //到款日期               
                if (info.ReceiptDate != null)
                {
                    DateTime time1 = (DateTime)info.ReceiptDate;
                    info.ReceiptDateMd = time1.ToString("yyyy-MM-dd");
                }

                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                if (followPerson != null)
                {
                    info.FollowPersonName = followPerson.F_RealName;

                }
                //项目负责人
                var projectResponsible = userIBLL.GetEntityByUserId(info.ProjectResponsible);
                if (projectResponsible != null)
                {
                    info.ProjectResponsibleName = projectResponsible.F_RealName;
                }
                //部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }
                //实施部门
                var department1 = departmentIBLL.GetEntity(info.J_F_FullName);
                if (department1 != null)
                {
                    info.J_F_FullName = department1.F_FullName;
                }
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }

                //合同主体
                var contractSubject = dataItemBLL.GetDetailItemName(info.ContractSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    info.ContractSubjectName = contractSubject.F_ItemName;
                }
                //报告状态
                var taskStatus = dataItemBLL.GetDetailItemName(info.TaskStatus, "TaskStatus");
                if (taskStatus != null)
                {
                    info.TaskStatusName = taskStatus.F_ItemName;
                }

                //核算1
                //自主营销
                if (info.ProjectSource == "1")
                {
                    if (info.PaymentAmount == null)
                    {

                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.02;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                    else if (info.PaymentAmount < (info.ContractAmount * (decimal?)0.3))
                    {
                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.005;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                    else
                    {
                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.02;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                }
                //渠道营销
                if (info.ProjectSource == "2")
                {
                    if (info.PaymentAmount == null)
                    {

                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.015;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                    else if (info.PaymentAmount < (info.ContractAmount * (decimal?)0.3))
                    {
                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.002;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                    else
                    {
                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.001;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                }
                //营销核算

                //自主营销
                if (info.ProjectSource == "1")
                {
                    if (info.PaymentAmount == null)
                    {
                        info.DepartmentIdAmount = info.ContractAmount * (decimal?)0.3;
                        if (info.DepartmentIdAmount != null)
                        {
                            //info.DepartmentIdAmountName = Math.Round((decimal)info.DepartmentIdAmount * 100) / 100;
                            info.DepartmentIdAmountName = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                        }
                    }
                    else if (info.PaymentAmount < (info.ContractAmount * (decimal?)0.3))
                    {
                        info.DepartmentIdAmount = (info.ContractAmount * (decimal?)0.3) - info.PaymentAmount;
                        if (info.DepartmentIdAmount != null)
                        {
                            info.DepartmentIdAmountName = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                        }
                    }
                    else
                    {
                        info.DepartmentIdAmount = info.ContractAmount * (decimal?)0.03;
                        if (info.DepartmentIdAmount != null)
                        {
                            info.DepartmentIdAmountName = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                        }
                    }
                }
                //生产核算
                if (info.ProjectSource == "1" || info.ProjectSource == "2")
                {
                    if (info.TaskStatus == "3" || info.TaskStatus == "4" || info.TaskStatus == "9" || info.TaskStatus == "5")
                    {
                        if (info.PaymentAmount != null)
                        {
                            if (info.PaymentAmount >= (info.ContractAmount * (decimal?)0.3))
                            {
                                if (info.SubAmount != null && info.MainAmount == null)
                                {
                                    info.DepartmentIdAmount = (info.ContractAmount - info.PaymentAmount - info.SubAmount) * (decimal?)0.3;
                                    if (info.DepartmentIdAmount != null)
                                    {
                                        info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                    }
                                }
                                else if (info.SubAmount == null && info.MainAmount != null)
                                {
                                    info.DepartmentIdAmount = (info.ContractAmount - info.PaymentAmount - info.MainAmount) * (decimal?)0.3;
                                    if (info.DepartmentIdAmount != null)
                                    {
                                        info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                    }
                                }
                                else
                                {
                                    info.DepartmentIdAmount = (info.ContractAmount - info.PaymentAmount) * (decimal?)0.3;
                                    if (info.DepartmentIdAmount != null)
                                    {
                                        info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                    }
                                }
                            }
                            else if (info.PaymentAmount < (info.ContractAmount * (decimal?)0.3))
                            {
                                info.DepartmentIdAmount = info.ContractAmount * (decimal?)0.2;
                                if (info.DepartmentIdAmount != null)
                                {
                                    info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                }
                                //item.DepartmentIdAmount = decimal.Round(decimal.Parse((item.ContractAmount * (decimal?)0.2).ToString()), 2);
                            }
                        }
                        else
                        {
                            if (info.ContractAmount != null)
                            {
                                info.DepartmentIdAmount = (info.ContractAmount * (decimal?)0.2);
                                if (info.DepartmentIdAmount != null)
                                {
                                    info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                }
                            }
                        }
                    }

                }
                //归档情况
                if (info.ReceivedFlag.ToInt() != 0)
                {
                    info.ReceivedFlagName = "已归档";
                }
                else
                {
                    info.ReceivedFlagName = "未归档";
                }

                list.Add(info);
            }
            list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 合计
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetSettleAccountsSumDepartmentId(dynamic _)
        {
            decimal? ContractAmountSum = 0;
            decimal? AmountSum = 0;
            decimal? NotReceivedSum = 0;
            decimal? OwnSum = 0;
            decimal? DitchSum = 0;
            decimal? ConsociationSum = 0;


            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var userInfo = LoginUserInfo.Get();
            var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);

            if (followPerson1.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson1.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or pt.DepartmentId='" + strList[i] + "' or a.FDepartmentId='" + strList[i] + "' or a.PDepartmentId='" + strList[i] + "' or t.CreateUser='" + strList[i] + "') ";
                    }

                }
                var data = reportFormsBLL.GetSettleAccountsSum_newDepartmentId(parameter.queryJson, deps);
                foreach (var item in data)
                {

                    ContractAmountSum = ContractAmountSum + item.ContractAmount;
                    AmountSum = AmountSum + item.Amount;
                    NotReceivedSum = NotReceivedSum + item.NotReceived;

                    if (item.ProjectSource == 1.ToString() && item.ContractAmount.HasValue)
                    {
                        OwnSum = OwnSum + item.ContractAmount;
                    }
                    if (item.ProjectSource == 2.ToString() && item.ContractAmount.HasValue)
                    {
                        DitchSum = DitchSum + item.ContractAmount;
                    }
                    if (item.ProjectSource == 3.ToString() && item.ContractAmount.HasValue)
                    {
                        ConsociationSum = ConsociationSum + item.ContractAmount;
                    }
                }

            }

            var result = new
            {
                ContractAmountSum = ContractAmountSum,
                AmountSum = AmountSum,
                NotReceivedSum = NotReceivedSum,
                OwnSum = OwnSum,
                DitchSum = DitchSum,
                ConsociationSum = ConsociationSum,
            };
            var jsonData = new
            {
                rows = result
            };
            return Success(jsonData);

        }


        #endregion
        #region 组件接口
        //合作伙伴合同主体
        public Response GetDetailTreeHZ(dynamic _)
        {
            var data = dataItemIBLL.GetDetailTreeHZ("ContractSubject");
            var jsonData = new
            {
                rows = data
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 合同添加项目名称
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetProjectPageList(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();

            var data = projectManageIBLL.GetSelectedProjectListWithoutContract(parameter.pagination, parameter.queryJson);
            foreach (var info in data)
            {
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                if (followPerson != null)
                {
                    info.FollowPersonName = followPerson.F_RealName;

                }
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }
                /* var projectContracts = projectContractBLL.GetProjectContractProjectId(info.Id);
                 if(projectContracts.ContractNo != null)
                 {
                     info.ContractNo = projectContracts.ContractNo;
                 }*/
                /*   List<ProjectContractEntity> projectContracts = projectContractBLL.GetProjectContractByProjectId(info.Id);
                   if (projectContracts.Count > 0)
                   {
                       if (projectContracts.FirstOrDefault().MainContract == 1)
                       {
                           info.ContractNo = projectContracts.FirstOrDefault().ContractNo;
                       }
                   }
                   if (projectContracts.Count > 1)
                   {
                       if (projectContracts.FirstOrDefault().MainContract == 1)
                       {
                           foreach (var contract in projectContracts)
                           {
                               if (contract.ContractType == 1)
                               {
                                   info.ContractNo = contract.ContractNo;
                               }
                           }
                       }

                   }*/
            }
            var jsonData = new
            {
                rows = data,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };
            return Success(jsonData);
        }
        public Response GetProjectContractPageList(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();

            var data = projectManageIBLL.GetSelectedProjectByContractList(parameter.pagination, parameter.queryJson);
            foreach (var info in data)
            {
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                if (followPerson != null)
                {
                    info.FollowPersonName = followPerson.F_RealName;
                }
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }
            }
            var jsonData = new
            {
                rows = data,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 回款添加项目名称
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetProjectPageListT(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = projectManageIBLL.GetSelectedProjectByContractList(parameter.pagination, parameter.queryJson);
            foreach (var info in data)
            {
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }
                /*   var projectContracts = projectContractBLL.GetProjectContractProjectId(info.Id);
                   if (projectContracts.ContractNo != null)
                   {
                       info.ContractNo = projectContracts.ContractNo;
                   }*/
            }
            var jsonData = new
            {
                rows = data,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };
            return Success(jsonData);
        }
        public Response GetProjectPageListBilling(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = projectManageIBLL.GetSelectedProjectByContractListBilling(parameter.pagination, parameter.queryJson);
            foreach (var info in data)
            {
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }
                /*   var projectContracts = projectContractBLL.GetProjectContractProjectId(info.Id);
                   if (projectContracts.ContractNo != null)
                   {
                       info.ContractNo = projectContracts.ContractNo;
                   }*/
            }
            var jsonData = new
            {
                rows = data,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };
            return Success(jsonData);
        }
        //部门选择
        public Response GetdepList(dynamic _)
        {
            var companylist = companyIBLL.GetList("");
            var data = departmentIBLL.GetTree(companylist);
            return Success(data);

        }
        public Response GetUserList(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            List<DepartmentUser> list = new List<DepartmentUser>();
            if (string.IsNullOrEmpty(""))
            {
                var data = departmentIBLL.GetPageList2();
                foreach (var i in data)
                {
                    DepartmentUser department = new DepartmentUser();
                    department.F_DepartmentId = i.F_DepartmentId;
                    department.F_FullName = i.F_FullName;
                    var user = userIBLL.GetListUser(i.F_DepartmentId);
                    if (user.Count() > 0)
                    {
                        List<DepUser> u1 = new List<DepUser>();
                        foreach (var w in user)
                        {
                            DepUser u = new DepUser();
                            u.F_UserId = w.F_UserId;
                            u.F_RealName = w.F_RealName;
                            u.F_CompanyId = w.F_CompanyId;
                            u.F_DepartmentId = i.F_DepartmentId;
                            u1.Add(u);
                        }
                        department.depuser = u1;
                    }
                    list.Add(department);
                }
            }
            return Success(list);
        }
        public Response GetUserById(dynamic _)
        {
            string keyValue = this.GetReqData();
            var ProjectData = userIBLL.GetFollowPersonNameByUserId(keyValue);

            return Success(ProjectData);
        }
        public Response GetUser(dynamic _)
        {
            string keyValue = this.GetReqData();
            var userInfo = LoginUserInfo.Get();
            //var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
            ProjectVo vo = new ProjectVo();
            if (keyValue == "10002")
            {
                vo.F_UserId = userInfo.userId;
                var data = codeRuleIBLL.GetBillCode(keyValue);
                vo.ProjectCode = data;
            }
            if (vo.F_UserId != null)
            {
                var ProjectData = userIBLL.GetFollowPersonNameByUserId(vo.F_UserId);
                vo.F_RealName = ProjectData.F_RealName;
                vo.F_UserId = ProjectData.F_UserId;
            }

            return Success(vo);
        }
        public Response GetdepById(dynamic _)
        {
            string keyValue = this.GetReqData();
            var ProjectData = departmentIBLL.GetEntity(keyValue);

            return Success(ProjectData);
        }
        public Response GetUserListHZ2(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            List<DepartmentUser> list = new List<DepartmentUser>();
            if (string.IsNullOrEmpty(""))
            {
                var data = departmentIBLL.GetPageListHZ2();
                foreach (var i in data)
                {
                    DepartmentUser department = new DepartmentUser();
                    department.F_DepartmentId = i.F_DepartmentId;
                    department.F_FullName = i.F_FullName;
                    var user = userIBLL.GetListUser(i.F_DepartmentId);
                    if (user.Count() > 0)
                    {
                        List<DepUser> u1 = new List<DepUser>();
                        foreach (var w in user)
                        {
                            DepUser u = new DepUser();
                            u.F_UserId = w.F_UserId;
                            u.F_RealName = w.F_RealName;
                            u.F_CompanyId = w.F_CompanyId;
                            u.F_DepartmentId = i.F_DepartmentId;
                            u1.Add(u);
                        }
                        department.depuser = u1;
                    }
                    list.Add(department);
                }
            }
            return Success(list);
        }
        #endregion
        #region 流程任务
        public Response GetTaskList(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();

            ProjectContractVo q = parameter.queryJson.ToObject<ProjectContractVo>();
            UserInfo userInfo = LoginUserInfo.Get();

            var data = nWFProcessIBLL.GetProcessDetails(q.WorkFlowId, q.TaskId, userInfo);
            if (data.TaskLogList != null)
            {
                data.TaskLogList = data.TaskLogList.OrderByDescending(i => i.F_CreateDate).ToList();
                foreach (var item in data.TaskLogList)
                {
                    UserEntity followPerson = userIBLL.GetFollowPersonNameByUserId(item.F_CreateUserId);
                    var department = departmentIBLL.GetEntity(followPerson.F_DepartmentId);
                    if (department != null)
                    {
                        item.F_CreateUserName = item.F_CreateUserName + "（" + department.F_FullName + "）";
                    }
                }
            }

            if (!string.IsNullOrEmpty(data.childProcessId))
            {
                q.WorkFlowId = data.childProcessId;
            }
            var taskNode = nWFProcessIBLL.GetTaskUserList(q.WorkFlowId);
            int is_Operator = 0;
            List<NWFTaskEntity> list = new List<NWFTaskEntity>();
            foreach (var i in taskNode)
            {
                NWFTaskEntity t = new NWFTaskEntity();
                if (i.nWFUserInfoList[0].Id != null)
                {
                    UserEntity followPerson = userIBLL.GetFollowPersonNameByUserId(i.nWFUserInfoList[0].Id);
                    if (followPerson.F_UserId == userInfo.userId)
                    {
                        is_Operator = 1;
                    }
                    if (followPerson != null)
                    {
                        t.F_CreateUserName = followPerson.F_RealName;
                    }
                    var department = departmentIBLL.GetEntity(followPerson.F_DepartmentId);
                    if (department != null)
                    {
                        t.F_CreateUserName = t.F_CreateUserName + "（" + department.F_FullName + "）";
                    }
                    t.F_NodeName = i.F_NodeName;
                    t.F_NodeId = i.F_NodeId;
                    list.Add(t);
                    /*  //部门
                      var department = departmentIBLL.GetEntity(followPerson.F_DepartmentId);
                      if (department != null)
                      {
                          t.DepartmentId = department.F_EnCode;
                      }*/
                }
            }
            //var cTaskList = nWFTaskIBLL.GetALLTaskList(q.WorkFlowId);
            var jsonData = new
            {
                rows = data,
                task = list,
                isOperator = is_Operator
            };
            return Success(jsonData);
        }
        public Response GetcTaskList(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();

            ProjectContractVo q = parameter.queryJson.ToObject<ProjectContractVo>();
            var cTaskList = nWFTaskIBLL.GetALLTaskList(q.WorkFlowId);
            cTaskList = cTaskList.OrderBy(t => t.F_CreateDate).ToList();
            var a = cTaskList.Count();
            var w = 0;
            List<NWFTaskEntity> list = new List<NWFTaskEntity>();
            foreach (var i in cTaskList)
            {
                w++;
                if (a.ToInt() == w)
                {
                    //已拒绝
                    NWFTaskEntity t = new NWFTaskEntity();
                    if (i.F_ModifyUserName == null && i.F_IsFinished == 0 && i.F_IsBatchAudit == null)
                    {

                        var tname = nWFTaskIBLL.GetEntityF_PrevNodeIdbyProcessId(i.F_ProcessId, i.F_NodeId);
                        t.F_CreateUserName = tname.F_CreateUserName;
                        t.F_IsFinished = i.F_IsFinished;
                        list.Add(t);
                    }
                    //下一审核人
                    if (i.F_ModifyUserName == null && i.F_IsFinished == 0 && i.F_IsBatchAudit == 1)
                    {
                        t.F_CreateUserName = i.F_NodeName;
                        t.F_IsFinished = i.F_IsFinished;
                        t.F_IsBatchAudit = i.F_IsBatchAudit;
                        list.Add(t);
                    }

                    //流程结束
                    if (i.F_ModifyUserName != null && i.F_IsFinished == 1 && i.F_IsBatchAudit == 1)
                    {
                        // NWFTaskEntity nWFTaskEntity = nWFTaskIBLL.GetEntity(i.F_Id);
                        t.F_CreateUserName = i.F_ModifyUserName;
                        t.F_IsFinished = i.F_IsFinished;
                        t.F_IsBatchAudit = i.F_IsBatchAudit;
                        t.F_CreateDate = i.F_ModifyDate;
                        list.Add(t);
                    }

                }

                list.Add(i);
            }
            list = list.OrderBy(t => t.F_CreateDate).ToList();
            var jsonData = new
            {
                rows = list,
                // task = list
            };
            return Success(jsonData);
        }
        public Response GetprocessId(dynamic _)
        {
            string keyValue = this.GetReqData();
            var processEntity = nWFProcessIBLL.GetEntity(keyValue);

            return Success(processEntity);
        }


        #endregion
        #region 台账
        #region 资金台账-旧
        //资金台账全部
        //public Response GetFundStatementList(dynamic _)
        //{
        //    ReqPageParam parameter = this.GetReqData<ReqPageParam>();
        //    //获取今年的时间
        //    DateTime StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-01"));
        //    //获取历史数据
        //    DateTime StartTime1 = DateTime.Parse(DateTime.Now.AddYears(-1).ToString("yyyy-01"));
        //    //接受获取的数据
        //    List<CapitalDepartmentId> list = new List<CapitalDepartmentId>();
        //    //公司id
        //    var companyId = "207fa1a9-160c-4943-a89b-8fa4db0547ce";
        //    //获取所有部门数据
        //    var datadepartment = departmentIBLL.GetList(companyId, "");
        //    //获取部门和年月查询条件
        //    CapitalDepartmentId q = parameter.queryJson.ToObject<CapitalDepartmentId>();
        //    //部门
        //    var DepartmentIdT = q.DepartmentId;
        //    //年
        //    var DateYYYY = q.YYYYTime;
        //    var timeYY = StartTime.ToString("yyyy");
        //    //今年数据
        //    CapitalDepartmentId xn = new CapitalDepartmentId();
        //    xn.ContractAmountList = 0;
        //    xn.EffectiveAmountList = 0;
        //    xn.ContractAmountSUN = 0;
        //    xn.sumList = 0;
        //    xn.ContractAmountSUNList = 0;
        //    xn.AmountList = 0;
        //    if (DepartmentIdT != null && DateYYYY != "" && DateYYYY != null)
        //    {
        //        if (DateYYYY.ToInt() == timeYY.ToInt())
        //        {
        //            //今年数据
        //            StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-01"));
        //            ////今年数据
        //            xn.yefen = "汇总";
        //            //qn.yefen = "历史数据";
        //            //上个月小计
        //            decimal? sumList1 = 0;
        //            //decimal? sumList12 = 0;
        //            //循环12个月的数据
        //            for (int i = 0; i < 12; i++)
        //            {
        //                CapitalDepartmentId list1 = new CapitalDepartmentId();
        //                //存当前月份
        //                list1.yefen = StartTime.AddMonths(i).ToString("yyyy-MM");
        //                var department = departmentIBLL.GetEntity(DepartmentIdT);
        //                if (department != null)
        //                {
        //                    list1.DepartmentIdName = department.F_FullName;
        //                }
        //                list1.datayyyyMM = StartTime.AddMonths(i).ToString("yyyy");
        //                list1.yefenList = list1.yefen + list1.DepartmentIdName;
        //                list1.index = StartTime.AddMonths(i).ToString("MM").ToInt();
        //                //初始化
        //                //本月合同额
        //                list1.ContractAmountList = 0;
        //                //本月绩效
        //                list1.EffectiveAmountList = 0;
        //                //本月资金
        //                list1.ContractAmountSUNList = 0;
        //                list1.AmountList = 0;
        //                decimal? ContractAmountSUNList1 = 0;
        //                //小计
        //                list1.sumList = 0;
        //                var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), DepartmentIdT);
        //                if (data1.ToList().Count > 0)
        //                {
        //                    foreach (var ins in data1)
        //                    {
        //                        decimal? EffectiveAmount1 = 0;
        //                        //历史数据
        //                        //审核时间<当年
        //                        //本月绩效
        //                        if (ins.EffectiveAmount != null)
        //                        {
        //                            //自主
        //                            if (ins.ProjectSource == "1")
        //                            {
        //                                EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
        //                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                                {
        //                                    list1.AmountList = list1.AmountList + EffectiveAmount1;
        //                                }
        //                            }
        //                            //渠道
        //                            if (ins.ProjectSource == "2")
        //                            {
        //                                EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
        //                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                                {
        //                                    list1.AmountList = list1.AmountList + EffectiveAmount1;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                //今年数据
        //                var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), DepartmentIdT);
        //                if (data.ToList().Count > 0)
        //                {
        //                    //本月成本                 
        //                    CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
        //                    if (capitalAmount != null)
        //                    {
        //                        list1.ContractAmountSUN = capitalAmount.CostAmount;
        //                    }
        //                    foreach (var inf in data)
        //                    {
        //                        decimal? EffectiveAmount1 = 0;
        //                        //本月合同额
        //                        if (inf.EffectiveAmount != null)
        //                        {
        //                            list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
        //                        }
        //                        //本月绩效                               
        //                        var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
        //                        if (inf.ContractAmount <= cay.Amount)
        //                        {
        //                            if (inf.EffectiveAmount != null)
        //                            {
        //                                //自主
        //                                if (inf.ProjectSource == "1")
        //                                {
        //                                    EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
        //                                    if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                                    {
        //                                        list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
        //                                    }
        //                                }
        //                                //渠道
        //                                if (inf.ProjectSource == "2")
        //                                {
        //                                    EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
        //                                    if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                                    {
        //                                        list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    //本月资金
        //                    if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
        //                    {
        //                        ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
        //                        list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
        //                    }
        //                    if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
        //                    {
        //                        ContractAmountSUNList1 = list1.ContractAmountSUN;
        //                        list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
        //                    }
        //                    if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
        //                    {
        //                        ContractAmountSUNList1 = list1.EffectiveAmountList;
        //                        list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
        //                    }
        //                    if (list1.ContractAmountSUNList != null)
        //                    {

        //                        list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;
        //                    }
        //                    list.Add(list1);
        //                    sumList1 = list1.ContractAmountSUNList;
        //                    xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
        //                    xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
        //                    if (list1.ContractAmountSUN != null)
        //                    {
        //                        xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
        //                    }
        //                    xn.sumList = xn.sumList + list1.sumList;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            var t = timeYY.ToInt() - DateYYYY.ToInt();
        //            StartTime1 = DateTime.Parse(DateTime.Now.AddYears(-t).ToString("yyyy-01"));
        //            //上个月小计
        //            decimal? sumList1 = 0;
        //            //今年数据
        //            //循环12个月的数据
        //            for (int i = 0; i < 12; i++)
        //            {
        //                CapitalDepartmentId list1 = new CapitalDepartmentId();
        //                //存当前月份
        //                list1.yefen = StartTime1.AddMonths(i).ToString("yyyy-MM");
        //                var department = departmentIBLL.GetEntity(DepartmentIdT);
        //                if (department != null)
        //                {
        //                    list1.DepartmentIdName = department.F_FullName;
        //                }
        //                list1.datayyyyMM = StartTime1.AddMonths(i).ToString("yyyy");
        //                list1.yefenList = list1.yefen + list1.DepartmentIdName;
        //                list1.index = StartTime1.AddMonths(i).ToString("MM").ToInt();
        //                //初始化
        //                //本月合同额
        //                list1.ContractAmountList = 0;
        //                //本月绩效
        //                list1.EffectiveAmountList = 0;
        //                //本月资金
        //                list1.ContractAmountSUNList = 0;
        //                list1.AmountList = 0;
        //                decimal? ContractAmountSUNList1 = 0;
        //                //小计
        //                list1.sumList = 0;
        //                var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime1.ToString(), StartTime1.AddMonths(i).ToString(), StartTime1.AddMonths(i + 1).ToString(), DepartmentIdT);
        //                if (data1.ToList().Count > 0)
        //                {
        //                    foreach (var ins in data1)
        //                    {
        //                        decimal? EffectiveAmount1 = 0;
        //                        //历史数据
        //                        //审核时间<当年
        //                        //本月绩效
        //                        if (ins.EffectiveAmount != null)
        //                        {
        //                            //自主
        //                            if (ins.ProjectSource == "1")
        //                            {
        //                                EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
        //                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                                {
        //                                    list1.AmountList = list1.AmountList + EffectiveAmount1;
        //                                }
        //                            }
        //                            //渠道
        //                            if (ins.ProjectSource == "2")
        //                            {
        //                                EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
        //                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                                {
        //                                    list1.AmountList = list1.AmountList + EffectiveAmount1;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                //今年数据
        //                var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime1.ToString(), StartTime1.AddMonths(i).ToString(), StartTime1.AddMonths(i + 1).ToString(), DepartmentIdT);
        //                if (data.ToList().Count > 0)
        //                {
        //                    //本月成本                 
        //                    CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
        //                    if (capitalAmount != null)
        //                    {
        //                        list1.ContractAmountSUN = capitalAmount.CostAmount;
        //                    }
        //                    foreach (var inf in data)
        //                    {
        //                        decimal? EffectiveAmount1 = 0;
        //                        //本月合同额
        //                        if (inf.EffectiveAmount != null)
        //                        {
        //                            list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
        //                        }
        //                        //本月绩效                               
        //                        var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
        //                        if (inf.ContractAmount <= cay.Amount)
        //                        {
        //                            if (inf.EffectiveAmount != null)
        //                            {
        //                                //自主
        //                                if (inf.ProjectSource == "1")
        //                                {
        //                                    EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
        //                                    if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                                    {
        //                                        list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
        //                                    }
        //                                }
        //                                //渠道
        //                                if (inf.ProjectSource == "2")
        //                                {
        //                                    EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
        //                                    if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                                    {
        //                                        list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    //本月资金
        //                    if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
        //                    {
        //                        ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
        //                        list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
        //                    }
        //                    if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
        //                    {
        //                        ContractAmountSUNList1 = list1.ContractAmountSUN;
        //                        list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
        //                    }
        //                    if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
        //                    {
        //                        ContractAmountSUNList1 = list1.EffectiveAmountList;
        //                        list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
        //                    }
        //                    if (list1.ContractAmountSUNList != null)
        //                    {
        //                        list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;
        //                    }
        //                    list.Add(list1);
        //                    sumList1 = list1.ContractAmountSUNList;
        //                    xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
        //                    xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
        //                    if (list1.ContractAmountSUN != null)
        //                    {
        //                        xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
        //                    }
        //                    xn.sumList = xn.sumList + list1.sumList;
        //                }
        //            }
        //        }
        //        xn.ContractAmountSUNList = xn.EffectiveAmountList - xn.ContractAmountSUN;
        //        list.Add(xn);
        //    }
        //    if (DateYYYY != null && (DepartmentIdT == "" || DepartmentIdT == null))
        //    {
        //        if (DateYYYY.ToInt() == timeYY.ToInt())
        //        {
        //            foreach (var dep in datadepartment)
        //            {
        //                ////今年数据
        //                xn.yefen = "汇总";
        //                //qn.yefen = "历史数据";
        //                //上个月小计
        //                decimal? sumList1 = 0;
        //                //decimal? sumList12 = 0;
        //                //循环12个月的数据
        //                for (int i = 0; i < 12; i++)
        //                {
        //                    CapitalDepartmentId list1 = new CapitalDepartmentId();
        //                    //存当前月份
        //                    list1.yefen = StartTime.AddMonths(i).ToString("yyyy-MM");
        //                    list1.DepartmentIdName = dep.F_FullName;
        //                    list1.datayyyyMM = StartTime.AddMonths(i).ToString("yyyy");
        //                    list1.yefenList = list1.yefen + list1.DepartmentIdName;
        //                    list1.index = StartTime.AddMonths(i).ToString("MM").ToInt();
        //                    //初始化
        //                    //本月合同额
        //                    list1.ContractAmountList = 0;
        //                    //本月绩效
        //                    list1.EffectiveAmountList = 0;
        //                    //本月资金
        //                    list1.ContractAmountSUNList = 0;
        //                    list1.AmountList = 0;
        //                    decimal? ContractAmountSUNList1 = 0;
        //                    //小计
        //                    list1.sumList = 0;
        //                    var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
        //                    if (data1.ToList().Count > 0)
        //                    {
        //                        foreach (var ins in data1)
        //                        {
        //                            decimal? EffectiveAmount1 = 0;
        //                            //历史数据
        //                            //审核时间<当年
        //                            //本月绩效
        //                            if (ins.EffectiveAmount != null)
        //                            {
        //                                //自主
        //                                if (ins.ProjectSource == "1")
        //                                {
        //                                    EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
        //                                    if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                                    {
        //                                        list1.AmountList = list1.AmountList + EffectiveAmount1;
        //                                    }
        //                                }
        //                                //渠道
        //                                if (ins.ProjectSource == "2")
        //                                {
        //                                    EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
        //                                    if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                                    {
        //                                        list1.AmountList = list1.AmountList + EffectiveAmount1;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    //今年数据
        //                    var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
        //                    if (data.ToList().Count > 0)
        //                    {
        //                        //本月成本                 
        //                        CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
        //                        if (capitalAmount != null)
        //                        {
        //                            list1.ContractAmountSUN = capitalAmount.CostAmount;
        //                        }
        //                        foreach (var inf in data)
        //                        {
        //                            decimal? EffectiveAmount1 = 0;
        //                            //本月合同额
        //                            if (inf.EffectiveAmount != null)
        //                            {
        //                                list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
        //                            }
        //                            //本月绩效                               
        //                            var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
        //                            if (inf.ContractAmount <= cay.Amount)
        //                            {
        //                                if (inf.EffectiveAmount != null)
        //                                {
        //                                    //自主
        //                                    if (inf.ProjectSource == "1")
        //                                    {
        //                                        EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
        //                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                                        {
        //                                            list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
        //                                        }
        //                                    }
        //                                    //渠道
        //                                    if (inf.ProjectSource == "2")
        //                                    {
        //                                        EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
        //                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                                        {
        //                                            list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
        //                                        }
        //                                    }
        //                                }

        //                            }

        //                        }
        //                        //本月资金
        //                        if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
        //                        {
        //                            ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
        //                            list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
        //                        }
        //                        if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
        //                        {
        //                            ContractAmountSUNList1 = list1.ContractAmountSUN;
        //                            list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
        //                        }
        //                        if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
        //                        {
        //                            ContractAmountSUNList1 = list1.EffectiveAmountList;
        //                            list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
        //                        }
        //                        if (list1.ContractAmountSUNList != null)
        //                        {

        //                            list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;

        //                        }
        //                        list.Add(list1);
        //                        sumList1 = list1.ContractAmountSUNList;
        //                        xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
        //                        xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
        //                        if (list1.ContractAmountSUN != null)
        //                        {
        //                            xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
        //                        }
        //                        xn.sumList = xn.sumList + list1.sumList;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            var t = timeYY.ToInt() - DateYYYY.ToInt();
        //            StartTime1 = DateTime.Parse(DateTime.Now.AddYears(-t).ToString("yyyy-01"));
        //            foreach (var dep in datadepartment)
        //            {
        //                ////今年数据
        //                xn.yefen = "汇总";
        //                //qn.yefen = "历史数据";
        //                //上个月小计
        //                decimal? sumList1 = 0;
        //                //decimal? sumList12 = 0;
        //                //循环12个月的数据
        //                for (int i = 0; i < 12; i++)
        //                {
        //                    CapitalDepartmentId list1 = new CapitalDepartmentId();
        //                    //存当前月份
        //                    list1.yefen = StartTime1.AddMonths(i).ToString("yyyy-MM");
        //                    list1.DepartmentIdName = dep.F_FullName;
        //                    list1.datayyyyMM = StartTime1.AddMonths(i).ToString("yyyy");
        //                    list1.yefenList = list1.yefen + list1.DepartmentIdName;
        //                    list1.index = StartTime1.AddMonths(i).ToString("MM").ToInt();
        //                    //初始化
        //                    //本月合同额
        //                    list1.ContractAmountList = 0;
        //                    //本月绩效
        //                    list1.EffectiveAmountList = 0;
        //                    //本月资金
        //                    list1.ContractAmountSUNList = 0;
        //                    list1.AmountList = 0;
        //                    decimal? ContractAmountSUNList1 = 0;
        //                    //小计
        //                    list1.sumList = 0;
        //                    var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime1.ToString(), StartTime1.AddMonths(i).ToString(), StartTime1.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
        //                    if (data1.ToList().Count > 0)
        //                    {
        //                        foreach (var ins in data1)
        //                        {
        //                            decimal? EffectiveAmount1 = 0;
        //                            //历史数据
        //                            //审核时间<当年
        //                            //本月绩效
        //                            if (ins.EffectiveAmount != null)
        //                            {
        //                                //自主
        //                                if (ins.ProjectSource == "1")
        //                                {
        //                                    EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
        //                                    if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                                    {
        //                                        list1.AmountList = list1.AmountList + EffectiveAmount1;
        //                                    }
        //                                }
        //                                //渠道
        //                                if (ins.ProjectSource == "2")
        //                                {
        //                                    EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
        //                                    if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                                    {
        //                                        list1.AmountList = list1.AmountList + EffectiveAmount1;
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                    //今年数据
        //                    var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime1.ToString(), StartTime1.AddMonths(i).ToString(), StartTime1.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
        //                    if (data.ToList().Count > 0)
        //                    {
        //                        //本月成本                 
        //                        CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
        //                        if (capitalAmount != null)
        //                        {
        //                            list1.ContractAmountSUN = capitalAmount.CostAmount;
        //                        }
        //                        foreach (var inf in data)
        //                        {
        //                            decimal? EffectiveAmount1 = 0;
        //                            //本月合同额
        //                            if (inf.EffectiveAmount != null)
        //                            {
        //                                list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
        //                            }

        //                            //本月绩效                               
        //                            var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
        //                            if (inf.ContractAmount <= cay.Amount)
        //                            {
        //                                if (inf.EffectiveAmount != null)
        //                                {

        //                                    //自主
        //                                    if (inf.ProjectSource == "1")
        //                                    {
        //                                        EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
        //                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                                        {
        //                                            list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
        //                                        }
        //                                    }
        //                                    //渠道
        //                                    if (inf.ProjectSource == "2")
        //                                    {
        //                                        EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
        //                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                                        {
        //                                            list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
        //                                        }

        //                                    }
        //                                }
        //                            }
        //                        }
        //                        //本月资金
        //                        if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
        //                        {
        //                            ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
        //                            list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
        //                        }
        //                        if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
        //                        {
        //                            ContractAmountSUNList1 = list1.ContractAmountSUN;
        //                            list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
        //                        }
        //                        if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
        //                        {
        //                            ContractAmountSUNList1 = list1.EffectiveAmountList;
        //                            list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
        //                        }
        //                        if (list1.ContractAmountSUNList != null)
        //                        {

        //                            list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;

        //                        }
        //                        list.Add(list1);
        //                        sumList1 = list1.ContractAmountSUNList;
        //                        xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
        //                        xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
        //                        if (list1.ContractAmountSUN != null)
        //                        {
        //                            xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
        //                        }
        //                        xn.sumList = xn.sumList + list1.sumList;
        //                    }
        //                }
        //            }
        //        }
        //        xn.ContractAmountSUNList = xn.EffectiveAmountList - xn.ContractAmountSUN;
        //        list.Add(xn);
        //    }
        //    if (DateYYYY == "" && DepartmentIdT != null)
        //    {
        //        ////今年数据
        //        xn.yefen = "汇总";
        //        //qn.yefen = "历史数据";
        //        //上个月小计
        //        decimal? sumList1 = 0;
        //        //decimal? sumList12 = 0;
        //        //循环12个月的数据
        //        for (int i = 0; i < 12; i++)
        //        {
        //            CapitalDepartmentId list1 = new CapitalDepartmentId();
        //            //存当前月份
        //            list1.yefen = StartTime.AddMonths(i).ToString("yyyy-MM");
        //            var department = departmentIBLL.GetEntity(DepartmentIdT);
        //            if (department != null)
        //            {
        //                list1.DepartmentIdName = department.F_FullName;
        //            }
        //            list1.datayyyyMM = StartTime.AddMonths(i).ToString("yyyy");
        //            list1.yefenList = list1.yefen + list1.DepartmentIdName;
        //            list1.index = StartTime.AddMonths(i).ToString("MM").ToInt();
        //            //初始化
        //            //本月合同额
        //            list1.ContractAmountList = 0;
        //            //本月绩效
        //            list1.EffectiveAmountList = 0;
        //            //本月资金
        //            list1.ContractAmountSUNList = 0;
        //            list1.AmountList = 0;
        //            decimal? ContractAmountSUNList1 = 0;
        //            //小计
        //            list1.sumList = 0;
        //            var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), DepartmentIdT);
        //            if (data1.ToList().Count > 0)
        //            {
        //                foreach (var ins in data1)
        //                {
        //                    decimal? EffectiveAmount1 = 0;
        //                    //历史数据
        //                    //审核时间<当年
        //                    //本月绩效
        //                    if (ins.EffectiveAmount != null)
        //                    {
        //                        //自主
        //                        if (ins.ProjectSource == "1")
        //                        {
        //                            EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
        //                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                            {
        //                                list1.AmountList = list1.AmountList + EffectiveAmount1;
        //                            }
        //                        }
        //                        //渠道
        //                        if (ins.ProjectSource == "2")
        //                        {
        //                            EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
        //                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                            {
        //                                list1.AmountList = list1.AmountList + EffectiveAmount1;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            //今年数据
        //            var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), DepartmentIdT);
        //            if (data.ToList().Count > 0)
        //            {
        //                //本月成本                 
        //                CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
        //                if (capitalAmount != null)
        //                {
        //                    list1.ContractAmountSUN = capitalAmount.CostAmount;
        //                }
        //                foreach (var inf in data)
        //                {
        //                    decimal? EffectiveAmount1 = 0;
        //                    //本月合同额
        //                    if (inf.EffectiveAmount != null)
        //                    {
        //                        list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
        //                    }
        //                    //本月绩效                               
        //                    var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
        //                    if (inf.ContractAmount <= cay.Amount)
        //                    {
        //                        if (inf.EffectiveAmount != null)
        //                        {

        //                            //自主
        //                            if (inf.ProjectSource == "1")
        //                            {
        //                                EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
        //                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                                {
        //                                    list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
        //                                }
        //                            }
        //                            //渠道
        //                            if (inf.ProjectSource == "2")
        //                            {
        //                                EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
        //                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                                {
        //                                    list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                //本月资金
        //                if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
        //                {
        //                    ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
        //                    list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
        //                }
        //                if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
        //                {
        //                    ContractAmountSUNList1 = list1.ContractAmountSUN;
        //                    list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
        //                }
        //                if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
        //                {
        //                    ContractAmountSUNList1 = list1.EffectiveAmountList;
        //                    list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
        //                }
        //                if (list1.ContractAmountSUNList != null)
        //                {
        //                    list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;
        //                }
        //                list.Add(list1);
        //                sumList1 = list1.ContractAmountSUNList;
        //                xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
        //                xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
        //                if (list1.ContractAmountSUN != null)
        //                {
        //                    xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
        //                }
        //                xn.sumList = xn.sumList + list1.sumList;
        //            }
        //        }
        //        xn.ContractAmountSUNList = xn.EffectiveAmountList - xn.ContractAmountSUN;
        //        list.Add(xn);
        //    }
        //    if ((DateYYYY == null || DateYYYY == "") && DepartmentIdT == null)
        //    {
        //        foreach (var dep in datadepartment)
        //        {
        //            ////今年数据
        //            xn.yefen = "汇总";
        //            //qn.yefen = "历史数据";
        //            //上个月小计
        //            decimal? sumList1 = 0;
        //            //decimal? sumList12 = 0;
        //            //循环12个月的数据
        //            for (int i = 0; i < 12; i++)
        //            {
        //                CapitalDepartmentId list1 = new CapitalDepartmentId();
        //                //存当前月份
        //                list1.yefen = StartTime.AddMonths(i).ToString("yyyy-MM");
        //                list1.DepartmentIdName = dep.F_FullName;
        //                list1.datayyyyMM = StartTime.AddMonths(i).ToString("yyyy");
        //                list1.yefenList = list1.yefen + list1.DepartmentIdName;
        //                list1.index = StartTime.AddMonths(i).ToString("MM").ToInt();

        //                //初始化
        //                //本月合同额
        //                list1.ContractAmountList = 0;
        //                //本月绩效
        //                list1.EffectiveAmountList = 0;
        //                //本月资金
        //                list1.ContractAmountSUNList = 0;
        //                list1.AmountList = 0;
        //                decimal? ContractAmountSUNList1 = 0;
        //                //小计
        //                list1.sumList = 0;
        //                var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
        //                if (data1.ToList().Count > 0)
        //                {
        //                    foreach (var ins in data1)
        //                    {
        //                        decimal? EffectiveAmount1 = 0;
        //                        //历史数据
        //                        //审核时间<当年
        //                        //本月绩效
        //                        if (ins.EffectiveAmount != null)
        //                        {
        //                            //自主
        //                            if (ins.ProjectSource == "1")
        //                            {
        //                                EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
        //                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                                {
        //                                    list1.AmountList = list1.AmountList + EffectiveAmount1;
        //                                }
        //                            }
        //                            //渠道
        //                            if (ins.ProjectSource == "2")
        //                            {
        //                                EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
        //                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                                {
        //                                    list1.AmountList = list1.AmountList + EffectiveAmount1;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                //今年数据
        //                var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
        //                if (data.ToList().Count > 0)
        //                {
        //                    //本月成本                 
        //                    CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
        //                    if (capitalAmount != null)
        //                    {
        //                        list1.ContractAmountSUN = capitalAmount.CostAmount;
        //                    }
        //                    foreach (var inf in data)
        //                    {
        //                        decimal? EffectiveAmount1 = 0;
        //                        //本月合同额
        //                        if (inf.EffectiveAmount != null)
        //                        {
        //                            list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
        //                        }
        //                        //本月绩效                               
        //                        var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
        //                        if (inf.ContractAmount <= cay.Amount)
        //                        {
        //                            if (inf.EffectiveAmount != null)
        //                            {
        //                                //自主
        //                                if (inf.ProjectSource == "1")
        //                                {
        //                                    EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
        //                                    if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                                    {
        //                                        list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
        //                                    }
        //                                }
        //                                //渠道
        //                                if (inf.ProjectSource == "2")
        //                                {
        //                                    EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
        //                                    if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
        //                                    {
        //                                        list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    //本月资金
        //                    if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
        //                    {
        //                        ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
        //                        list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
        //                    }
        //                    if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
        //                    {
        //                        ContractAmountSUNList1 = list1.ContractAmountSUN;
        //                        list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
        //                    }
        //                    if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
        //                    {
        //                        ContractAmountSUNList1 = list1.EffectiveAmountList;
        //                        list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
        //                    }
        //                    if (list1.ContractAmountSUNList != null)
        //                    {
        //                        list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;
        //                    }
        //                    list.Add(list1);
        //                    sumList1 = list1.ContractAmountSUNList;
        //                    xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
        //                    xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
        //                    if (list1.ContractAmountSUN != null)
        //                    {
        //                        xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
        //                    }
        //                    xn.sumList = xn.sumList + list1.sumList;
        //                    if (list1.AmountList != null)
        //                    {
        //                        xn.AmountList = xn.AmountList + list1.AmountList;
        //                    }
        //                }
        //            }
        //        }
        //        xn.ContractAmountSUNList = xn.EffectiveAmountList - xn.ContractAmountSUN;
        //        list.Add(xn);
        //    }
        //    //降序
        //    list = list.OrderByDescending(t => t.index).ThenByDescending(t => t.datayyyyMM).ToList();
        //    var jsonData = new
        //    {
        //        rows = list,
        //        total = parameter.pagination.total,
        //        page = parameter.pagination.page,
        //        records = parameter.pagination.records
        //    };
        //    return Success(jsonData);
        //}
        #endregion
        //资金台账全部 2023-12-11
        public Response GetFundStatementList(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            //获取今年的时间
            DateTime StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-01"));
            //获取历史数据
            DateTime StartTime1 = DateTime.Parse(DateTime.Now.AddYears(-1).ToString("yyyy-01"));
            //接受获取的数据
            List<CapitalDepartmentId> list = new List<CapitalDepartmentId>();
            //公司id
            var companyId = "207fa1a9-160c-4943-a89b-8fa4db0547ce";
            //获取所有部门数据
            var datadepartment = departmentIBLL.GetList_zijin("");
            //获取部门和年月查询条件
            CapitalDepartmentId q = parameter.queryJson.ToObject<CapitalDepartmentId>();
            //部门
            var DepartmentIdT = q.DepartmentId;
            //年
            var DateYYYY = q.YYYYTime;
            var timeYY = StartTime.ToString("yyyy");
            //今年数据
            CapitalDepartmentId xn = new CapitalDepartmentId();
            xn.ContractAmountList = 0;
            xn.EffectiveAmountList = 0;
            xn.ContractAmountSUN = 0;
            xn.sumList = 0;
            xn.ContractAmountSUNList = 0;
            xn.AmountList = 0;
            if (DepartmentIdT != null && DateYYYY != "" && DateYYYY != null)
            {
                if (DateYYYY.ToInt() == timeYY.ToInt())
                {
                    //今年数据
                    StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-01"));
                    ////今年数据
                    xn.yefen = "汇总";
                    //qn.yefen = "历史数据";
                    //上个月小计
                    decimal? sumList1 = 0;
                    //decimal? sumList12 = 0;
                    //循环12个月的数据
                    for (int i = 0; i < 12; i++)
                    {
                        if (StartTime.AddMonths(i) > DateTime.Now)
                        {
                            break;
                        }
                        CapitalDepartmentId list1 = new CapitalDepartmentId();
                        //存当前月份
                        list1.yefen = StartTime.AddMonths(i).ToString("yyyy-MM");
                        var department = departmentIBLL.GetEntity(DepartmentIdT);
                        if (department != null)
                        {
                            list1.DepartmentIdName = department.F_FullName;
                        }
                        list1.datayyyyMM = StartTime.AddMonths(i).ToString("yyyy");
                        list1.yefenList = list1.yefen + list1.DepartmentIdName;
                        list1.index = StartTime.AddMonths(i).ToString("MM").ToInt();
                        //初始化
                        //本月合同额
                        list1.ContractAmountList = 0;
                        //本月绩效
                        list1.EffectiveAmountList = 0;
                        //本月资金
                        list1.ContractAmountSUNList = 0;
                        list1.AmountList = 0;
                        decimal? ContractAmountSUNList1 = 0;
                        //小计
                        list1.sumList = 0;
                        var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), DepartmentIdT);
                        if (data1.ToList().Count > 0)
                        {
                            foreach (var ins in data1)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //历史数据
                                //审核时间<当年
                                //本月绩效
                                if (ins.EffectiveAmount != null)
                                {
                                    //自主
                                    if (ins.ProjectSource == "1")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }
                                    //渠道
                                    if (ins.ProjectSource == "2")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }
                                }
                            }
                        }
                        //今年数据
                        var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), DepartmentIdT);
                        if (data.ToList().Count > 0)
                        {
                            //本月成本                 
                            CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                            if (capitalAmount != null)
                            {
                                list1.ContractAmountSUN = capitalAmount.CostAmount;
                            }
                            foreach (var inf in data)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //本月合同额
                                if (inf.EffectiveAmount != null)
                                {
                                    list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                                }
                                //本月绩效                               
                                var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                                if (inf.ContractAmount <= cay.Amount)
                                {
                                    if (inf.EffectiveAmount != null)
                                    {
                                        //自主
                                        if (inf.ProjectSource == "1")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }
                                        }
                                        //渠道
                                        if (inf.ProjectSource == "2")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }
                                        }
                                    }
                                }
                            }
                            //本月资金
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.ContractAmountSUNList != null)
                            {

                                list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;
                            }
                            list.Add(list1);
                            sumList1 = list1.ContractAmountSUNList;
                            xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                            xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                            if (list1.ContractAmountSUN != null)
                            {
                                xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                            }
                            xn.sumList = xn.sumList + list1.sumList;
                        }
                        else
                        {
                            list.Add(list1);
                        }
                    }
                }
                else
                {
                    var t = timeYY.ToInt() - DateYYYY.ToInt();
                    StartTime1 = DateTime.Parse(DateTime.Now.AddYears(-t).ToString("yyyy-01"));
                    //上个月小计
                    decimal? sumList1 = 0;
                    //今年数据
                    //循环12个月的数据
                    for (int i = 0; i < 12; i++)
                    {
                        if (StartTime.AddMonths(i) > DateTime.Now)
                        {
                            break;
                        }
                        CapitalDepartmentId list1 = new CapitalDepartmentId();
                        //存当前月份
                        list1.yefen = StartTime1.AddMonths(i).ToString("yyyy-MM");
                        var department = departmentIBLL.GetEntity(DepartmentIdT);
                        if (department != null)
                        {
                            list1.DepartmentIdName = department.F_FullName;
                        }
                        list1.datayyyyMM = StartTime1.AddMonths(i).ToString("yyyy");
                        list1.yefenList = list1.yefen + list1.DepartmentIdName;
                        list1.index = StartTime1.AddMonths(i).ToString("MM").ToInt();
                        //初始化
                        //本月合同额
                        list1.ContractAmountList = 0;
                        //本月绩效
                        list1.EffectiveAmountList = 0;
                        //本月资金
                        list1.ContractAmountSUNList = 0;
                        list1.AmountList = 0;
                        decimal? ContractAmountSUNList1 = 0;
                        //小计
                        list1.sumList = 0;
                        var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime1.ToString(), StartTime1.AddMonths(i).ToString(), StartTime1.AddMonths(i + 1).ToString(), DepartmentIdT);
                        if (data1.ToList().Count > 0)
                        {
                            foreach (var ins in data1)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //历史数据
                                //审核时间<当年
                                //本月绩效
                                if (ins.EffectiveAmount != null)
                                {
                                    //自主
                                    if (ins.ProjectSource == "1")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }
                                    //渠道
                                    if (ins.ProjectSource == "2")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }
                                }
                            }
                        }
                        //今年数据
                        var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime1.ToString(), StartTime1.AddMonths(i).ToString(), StartTime1.AddMonths(i + 1).ToString(), DepartmentIdT);
                        if (data.ToList().Count > 0)
                        {
                            //本月成本                 
                            CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                            if (capitalAmount != null)
                            {
                                list1.ContractAmountSUN = capitalAmount.CostAmount;
                            }
                            foreach (var inf in data)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //本月合同额
                                if (inf.EffectiveAmount != null)
                                {
                                    list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                                }
                                //本月绩效                               
                                var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                                if (inf.ContractAmount <= cay.Amount)
                                {
                                    if (inf.EffectiveAmount != null)
                                    {
                                        //自主
                                        if (inf.ProjectSource == "1")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }
                                        }
                                        //渠道
                                        if (inf.ProjectSource == "2")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }
                                        }
                                    }
                                }
                            }
                            //本月资金
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.ContractAmountSUNList != null)
                            {
                                list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;
                            }
                            list.Add(list1);
                            sumList1 = list1.ContractAmountSUNList;
                            xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                            xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                            if (list1.ContractAmountSUN != null)
                            {
                                xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                            }
                            xn.sumList = xn.sumList + list1.sumList;
                        }
                        else
                        {
                            list.Add(list1);
                        }
                    }
                }
                xn.ContractAmountSUNList = xn.EffectiveAmountList - xn.ContractAmountSUN;
                list.Add(xn);
            }
            if (DateYYYY != null && (DepartmentIdT == "" || DepartmentIdT == null))
            {
                if (DateYYYY.ToInt() == timeYY.ToInt())
                {
                    foreach (var dep in datadepartment)
                    {
                        ////今年数据
                        xn.yefen = "汇总";
                        //qn.yefen = "历史数据";
                        //上个月小计
                        decimal? sumList1 = 0;
                        //decimal? sumList12 = 0;
                        //循环12个月的数据
                        for (int i = 0; i < 12; i++)
                        {
                            if (StartTime.AddMonths(i) > DateTime.Now)
                            {
                                break;
                            }
                            CapitalDepartmentId list1 = new CapitalDepartmentId();
                            //存当前月份
                            list1.yefen = StartTime.AddMonths(i).ToString("yyyy-MM");
                            list1.DepartmentIdName = dep.F_FullName;
                            list1.datayyyyMM = StartTime.AddMonths(i).ToString("yyyy");
                            list1.yefenList = list1.yefen + list1.DepartmentIdName;
                            list1.index = StartTime.AddMonths(i).ToString("MM").ToInt();
                            //初始化
                            //本月合同额
                            list1.ContractAmountList = 0;
                            //本月绩效
                            list1.EffectiveAmountList = 0;
                            //本月资金
                            list1.ContractAmountSUNList = 0;
                            list1.AmountList = 0;
                            decimal? ContractAmountSUNList1 = 0;
                            //小计
                            list1.sumList = 0;
                            var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                            if (data1.ToList().Count > 0)
                            {
                                foreach (var ins in data1)
                                {
                                    decimal? EffectiveAmount1 = 0;
                                    //历史数据
                                    //审核时间<当年
                                    //本月绩效
                                    if (ins.EffectiveAmount != null)
                                    {
                                        //自主
                                        if (ins.ProjectSource == "1")
                                        {
                                            EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.AmountList = list1.AmountList + EffectiveAmount1;
                                            }
                                        }
                                        //渠道
                                        if (ins.ProjectSource == "2")
                                        {
                                            EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.AmountList = list1.AmountList + EffectiveAmount1;
                                            }
                                        }
                                    }
                                }
                            }
                            //今年数据
                            var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                            if (data.ToList().Count > 0)
                            {
                                //本月成本                 
                                CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                                if (capitalAmount != null)
                                {
                                    list1.ContractAmountSUN = capitalAmount.CostAmount;
                                }
                                foreach (var inf in data)
                                {
                                    decimal? EffectiveAmount1 = 0;
                                    //本月合同额
                                    if (inf.EffectiveAmount != null)
                                    {
                                        list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                                    }
                                    //本月绩效                               
                                    var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                                    if (inf.ContractAmount <= cay.Amount)
                                    {
                                        if (inf.EffectiveAmount != null)
                                        {
                                            //自主
                                            if (inf.ProjectSource == "1")
                                            {
                                                EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                                {
                                                    list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                                }
                                            }
                                            //渠道
                                            if (inf.ProjectSource == "2")
                                            {
                                                EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                                {
                                                    list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                                }
                                            }
                                        }

                                    }

                                }
                                //本月资金
                                if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                                {
                                    ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                                }
                                if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                                {
                                    ContractAmountSUNList1 = list1.ContractAmountSUN;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                                }
                                if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                                {
                                    ContractAmountSUNList1 = list1.EffectiveAmountList;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                                }
                                if (list1.ContractAmountSUNList != null)
                                {

                                    list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;

                                }
                                list.Add(list1);
                                sumList1 = list1.ContractAmountSUNList;
                                xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                                xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                                if (list1.ContractAmountSUN != null)
                                {
                                    xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                                }
                                xn.sumList = xn.sumList + list1.sumList;
                            }
                            else
                            {
                                list.Add(list1);
                            }
                        }
                    }
                }
                else
                {
                    var t = timeYY.ToInt() - DateYYYY.ToInt();
                    StartTime1 = DateTime.Parse(DateTime.Now.AddYears(-t).ToString("yyyy-01"));
                    foreach (var dep in datadepartment)
                    {
                        ////今年数据
                        xn.yefen = "汇总";
                        //qn.yefen = "历史数据";
                        //上个月小计
                        decimal? sumList1 = 0;
                        //decimal? sumList12 = 0;
                        //循环12个月的数据
                        for (int i = 0; i < 12; i++)
                        {
                            if (StartTime.AddMonths(i) > DateTime.Now)
                            {
                                break;
                            }
                            CapitalDepartmentId list1 = new CapitalDepartmentId();
                            //存当前月份
                            list1.yefen = StartTime1.AddMonths(i).ToString("yyyy-MM");
                            list1.DepartmentIdName = dep.F_FullName;
                            list1.datayyyyMM = StartTime1.AddMonths(i).ToString("yyyy");
                            list1.yefenList = list1.yefen + list1.DepartmentIdName;
                            list1.index = StartTime1.AddMonths(i).ToString("MM").ToInt();
                            //初始化
                            //本月合同额
                            list1.ContractAmountList = 0;
                            //本月绩效
                            list1.EffectiveAmountList = 0;
                            //本月资金
                            list1.ContractAmountSUNList = 0;
                            list1.AmountList = 0;
                            decimal? ContractAmountSUNList1 = 0;
                            //小计
                            list1.sumList = 0;
                            var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime1.ToString(), StartTime1.AddMonths(i).ToString(), StartTime1.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                            if (data1.ToList().Count > 0)
                            {
                                foreach (var ins in data1)
                                {
                                    decimal? EffectiveAmount1 = 0;
                                    //历史数据
                                    //审核时间<当年
                                    //本月绩效
                                    if (ins.EffectiveAmount != null)
                                    {
                                        //自主
                                        if (ins.ProjectSource == "1")
                                        {
                                            EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.AmountList = list1.AmountList + EffectiveAmount1;
                                            }
                                        }
                                        //渠道
                                        if (ins.ProjectSource == "2")
                                        {
                                            EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.AmountList = list1.AmountList + EffectiveAmount1;
                                            }
                                        }

                                    }
                                }
                            }
                            //今年数据
                            var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime1.ToString(), StartTime1.AddMonths(i).ToString(), StartTime1.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                            if (data.ToList().Count > 0)
                            {
                                //本月成本                 
                                CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                                if (capitalAmount != null)
                                {
                                    list1.ContractAmountSUN = capitalAmount.CostAmount;
                                }
                                foreach (var inf in data)
                                {
                                    decimal? EffectiveAmount1 = 0;
                                    //本月合同额
                                    if (inf.EffectiveAmount != null)
                                    {
                                        list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                                    }

                                    //本月绩效                               
                                    var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                                    if (inf.ContractAmount <= cay.Amount)
                                    {
                                        if (inf.EffectiveAmount != null)
                                        {

                                            //自主
                                            if (inf.ProjectSource == "1")
                                            {
                                                EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                                {
                                                    list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                                }
                                            }
                                            //渠道
                                            if (inf.ProjectSource == "2")
                                            {
                                                EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                                {
                                                    list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                                }

                                            }
                                        }
                                    }
                                }
                                //本月资金
                                if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                                {
                                    ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                                }
                                if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                                {
                                    ContractAmountSUNList1 = list1.ContractAmountSUN;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                                }
                                if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                                {
                                    ContractAmountSUNList1 = list1.EffectiveAmountList;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                                }
                                if (list1.ContractAmountSUNList != null)
                                {

                                    list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;

                                }
                                list.Add(list1);
                                sumList1 = list1.ContractAmountSUNList;
                                xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                                xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                                if (list1.ContractAmountSUN != null)
                                {
                                    xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                                }
                                xn.sumList = xn.sumList + list1.sumList;
                            }
                            else
                            {
                                list.Add(list1);
                            }
                        }
                    }
                }
                xn.ContractAmountSUNList = xn.EffectiveAmountList - xn.ContractAmountSUN;
                list.Add(xn);
            }
            if (DateYYYY == "" && DepartmentIdT != null)
            {
                ////今年数据
                xn.yefen = "汇总";
                //qn.yefen = "历史数据";
                //上个月小计
                decimal? sumList1 = 0;
                //decimal? sumList12 = 0;
                //循环12个月的数据
                for (int i = 0; i < 12; i++)
                {
                    if (StartTime.AddMonths(i) > DateTime.Now)
                    {
                        break;
                    }
                    CapitalDepartmentId list1 = new CapitalDepartmentId();
                    //存当前月份
                    list1.yefen = StartTime.AddMonths(i).ToString("yyyy-MM");
                    var department = departmentIBLL.GetEntity(DepartmentIdT);
                    if (department != null)
                    {
                        list1.DepartmentIdName = department.F_FullName;
                    }
                    list1.datayyyyMM = StartTime.AddMonths(i).ToString("yyyy");
                    list1.yefenList = list1.yefen + list1.DepartmentIdName;
                    list1.index = StartTime.AddMonths(i).ToString("MM").ToInt();
                    //初始化
                    //本月合同额
                    list1.ContractAmountList = 0;
                    //本月绩效
                    list1.EffectiveAmountList = 0;
                    //本月资金
                    list1.ContractAmountSUNList = 0;
                    list1.AmountList = 0;
                    decimal? ContractAmountSUNList1 = 0;
                    //小计
                    list1.sumList = 0;
                    var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), DepartmentIdT);
                    if (data1.ToList().Count > 0)
                    {
                        foreach (var ins in data1)
                        {
                            decimal? EffectiveAmount1 = 0;
                            //历史数据
                            //审核时间<当年
                            //本月绩效
                            if (ins.EffectiveAmount != null)
                            {
                                //自主
                                if (ins.ProjectSource == "1")
                                {
                                    EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                    if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                    {
                                        list1.AmountList = list1.AmountList + EffectiveAmount1;
                                    }
                                }
                                //渠道
                                if (ins.ProjectSource == "2")
                                {
                                    EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                    if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                    {
                                        list1.AmountList = list1.AmountList + EffectiveAmount1;
                                    }
                                }
                            }
                        }
                    }
                    //今年数据
                    var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), DepartmentIdT);
                    if (data.ToList().Count > 0)
                    {
                        //本月成本                 
                        CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                        if (capitalAmount != null)
                        {
                            list1.ContractAmountSUN = capitalAmount.CostAmount;
                        }
                        foreach (var inf in data)
                        {
                            decimal? EffectiveAmount1 = 0;
                            //本月合同额
                            if (inf.EffectiveAmount != null)
                            {
                                list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                            }
                            //本月绩效                               
                            var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                            if (inf.ContractAmount <= cay.Amount)
                            {
                                if (inf.EffectiveAmount != null)
                                {

                                    //自主
                                    if (inf.ProjectSource == "1")
                                    {
                                        EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                        }
                                    }
                                    //渠道
                                    if (inf.ProjectSource == "2")
                                    {
                                        EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                        }
                                    }
                                }
                            }
                        }
                        //本月资金
                        if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                        {
                            ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                            list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                        }
                        if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                        {
                            ContractAmountSUNList1 = list1.ContractAmountSUN;
                            list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                        }
                        if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                        {
                            ContractAmountSUNList1 = list1.EffectiveAmountList;
                            list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                        }
                        if (list1.ContractAmountSUNList != null)
                        {
                            list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;
                        }
                        list.Add(list1);
                        sumList1 = list1.ContractAmountSUNList;
                        xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                        xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                        if (list1.ContractAmountSUN != null)
                        {
                            xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                        }
                        xn.sumList = xn.sumList + list1.sumList;
                    }
                    else
                    {
                        list.Add(list1);
                    }
                }
                xn.ContractAmountSUNList = xn.EffectiveAmountList - xn.ContractAmountSUN;
                list.Add(xn);
            }
            if ((DateYYYY == null || DateYYYY == "") && DepartmentIdT == null)
            {
                foreach (var dep in datadepartment)
                {
                    ////今年数据
                    xn.yefen = "汇总";
                    //qn.yefen = "历史数据";
                    //上个月小计
                    decimal? sumList1 = 0;
                    //decimal? sumList12 = 0;
                    //循环12个月的数据
                    for (int i = 0; i < 12; i++)
                    {
                        if (StartTime.AddMonths(i) > DateTime.Now)
                        {
                            break;
                        }
                        CapitalDepartmentId list1 = new CapitalDepartmentId();
                        //存当前月份
                        list1.yefen = StartTime.AddMonths(i).ToString("yyyy-MM");
                        list1.DepartmentIdName = dep.F_FullName;
                        list1.datayyyyMM = StartTime.AddMonths(i).ToString("yyyy");
                        list1.yefenList = list1.yefen + list1.DepartmentIdName;
                        list1.index = StartTime.AddMonths(i).ToString("MM").ToInt();

                        //初始化
                        //本月合同额
                        list1.ContractAmountList = 0;
                        //本月绩效
                        list1.EffectiveAmountList = 0;
                        //本月资金
                        list1.ContractAmountSUNList = 0;
                        list1.AmountList = 0;
                        decimal? ContractAmountSUNList1 = 0;
                        //小计
                        list1.sumList = 0;
                        var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                        if (data1.ToList().Count > 0)
                        {
                            foreach (var ins in data1)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //历史数据
                                //审核时间<当年
                                //本月绩效
                                if (ins.EffectiveAmount != null)
                                {
                                    //自主
                                    if (ins.ProjectSource == "1")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }
                                    //渠道
                                    if (ins.ProjectSource == "2")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }
                                }
                            }
                        }
                        //今年数据
                        var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                        if (data.ToList().Count > 0)
                        {
                            //本月成本                 
                            CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                            if (capitalAmount != null)
                            {
                                list1.ContractAmountSUN = capitalAmount.CostAmount;
                            }
                            foreach (var inf in data)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //本月合同额
                                if (inf.EffectiveAmount != null)
                                {
                                    list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                                }
                                //本月绩效                               
                                var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                                if (inf.ContractAmount <= cay.Amount)
                                {
                                    if (inf.EffectiveAmount != null)
                                    {
                                        //自主
                                        if (inf.ProjectSource == "1")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }
                                        }
                                        //渠道
                                        if (inf.ProjectSource == "2")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }
                                        }
                                    }
                                }
                            }
                            //本月资金
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.ContractAmountSUNList != null)
                            {
                                list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;
                            }
                            list.Add(list1);
                            sumList1 = list1.ContractAmountSUNList;
                            xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                            xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                            if (list1.ContractAmountSUN != null)
                            {
                                xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                            }
                            xn.sumList = xn.sumList + list1.sumList;
                            if (list1.AmountList != null)
                            {
                                xn.AmountList = xn.AmountList + list1.AmountList;
                            }
                        }
                        else
                        {
                            list.Add(list1);
                        }
                    }
                }
                xn.ContractAmountSUNList = xn.EffectiveAmountList - xn.ContractAmountSUN;
                list.Add(xn);
            }
            //降序
            list = list.OrderByDescending(t => t.index).ThenByDescending(t => t.datayyyyMM).ToList();
            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };
            return Success(jsonData);
        }
        //资金台账成本添加
        public Response CapitalAmountSaveForm(dynamic _)
        {
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            string jsonText = string.Empty;

            HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空
            StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
            jsonText = sr.ReadToEnd();

            //ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();

            // CapitalAmountEntity entity = parameter.strEntity.ToObject<CapitalAmountEntity>();
            CapitalAmountEntity entity = jsonText.ToObject<CapitalAmountEntity>();
            //成本金额/年月
            reportFormsBLL.CapitalAmountSaveForm1(entity.CostAmount, entity.Yearyear);
            return Success("保存成功！");
        }
        //资金台账导出
        public Response GetFundStatementListAll(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            //获取今年的时间
            DateTime StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-01"));
            //获取历史数据
            DateTime StartTime1 = DateTime.Parse(DateTime.Now.AddYears(-1).ToString("yyyy-01"));
            //接受获取的数据
            List<CapitalDepartmentId> list = new List<CapitalDepartmentId>();
            //公司id
            var companyId = "207fa1a9-160c-4943-a89b-8fa4db0547ce";
            //获取所有部门数据
            var datadepartment = departmentIBLL.GetList(companyId, "");
            //获取部门和年月查询条件
            CapitalDepartmentId q = parameter.queryJson.ToObject<CapitalDepartmentId>();
            //部门
            var DepartmentIdT = q.DepartmentId;
            //年
            var DateYYYY = q.YYYYTime;
            var timeYY = StartTime.ToString("yyyy");
            //今年数据
            CapitalDepartmentId xn = new CapitalDepartmentId();
            xn.ContractAmountList = 0;
            xn.EffectiveAmountList = 0;
            xn.ContractAmountSUN = 0;
            xn.sumList = 0;
            xn.ContractAmountSUNList = 0;
            xn.AmountList = 0;
            if (DepartmentIdT != null && DateYYYY != "" && DateYYYY != null)
            {
                if (DateYYYY.ToInt() == timeYY.ToInt())
                {
                    //今年数据
                    StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-01"));
                    ////今年数据
                    xn.yefen = "汇总";
                    //qn.yefen = "历史数据";
                    //上个月小计
                    decimal? sumList1 = 0;
                    //decimal? sumList12 = 0;
                    //循环12个月的数据
                    for (int i = 0; i < 12; i++)
                    {
                        if (StartTime.AddMonths(i) > DateTime.Now)
                        {
                            break;
                        }
                        CapitalDepartmentId list1 = new CapitalDepartmentId();
                        //存当前月份
                        list1.yefen = StartTime.AddMonths(i).ToString("yyyy-MM");
                        var department = departmentIBLL.GetEntity(DepartmentIdT);
                        if (department != null)
                        {
                            list1.DepartmentIdName = department.F_FullName;
                        }
                        list1.datayyyyMM = StartTime.AddMonths(i).ToString("yyyy");
                        list1.yefenList = list1.yefen + list1.DepartmentIdName;
                        list1.index = StartTime.AddMonths(i).ToString("MM").ToInt();
                        //初始化
                        //本月合同额
                        list1.ContractAmountList = 0;
                        //本月绩效
                        list1.EffectiveAmountList = 0;
                        //本月资金
                        list1.ContractAmountSUNList = 0;
                        list1.AmountList = 0;
                        decimal? ContractAmountSUNList1 = 0;
                        //小计
                        list1.sumList = 0;
                        var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), DepartmentIdT);
                        if (data1.ToList().Count > 0)
                        {
                            foreach (var ins in data1)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //历史数据
                                //审核时间<当年
                                //本月绩效
                                if (ins.EffectiveAmount != null)
                                {
                                    //自主
                                    if (ins.ProjectSource == "1")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }
                                    //渠道
                                    if (ins.ProjectSource == "2")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }
                                }
                            }
                        }
                        //今年数据
                        var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), DepartmentIdT);
                        if (data.ToList().Count > 0)
                        {
                            //本月成本                 
                            CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                            if (capitalAmount != null)
                            {
                                list1.ContractAmountSUN = capitalAmount.CostAmount;
                            }
                            foreach (var inf in data)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //本月合同额
                                if (inf.EffectiveAmount != null)
                                {
                                    list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                                }
                                //本月绩效                               
                                var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                                if (inf.ContractAmount <= cay.Amount)
                                {
                                    if (inf.EffectiveAmount != null)
                                    {
                                        //自主
                                        if (inf.ProjectSource == "1")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }
                                        }
                                        //渠道
                                        if (inf.ProjectSource == "2")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }
                                        }
                                    }
                                }
                            }
                            //本月资金
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.ContractAmountSUNList != null)
                            {

                                list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;
                            }
                            list.Add(list1);
                            sumList1 = list1.ContractAmountSUNList;
                            xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                            xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                            if (list1.ContractAmountSUN != null)
                            {
                                xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                            }
                            xn.sumList = xn.sumList + list1.sumList;
                        }
                    }
                }
                else
                {
                    var t = timeYY.ToInt() - DateYYYY.ToInt();
                    StartTime1 = DateTime.Parse(DateTime.Now.AddYears(-t).ToString("yyyy-01"));
                    //上个月小计
                    decimal? sumList1 = 0;
                    //今年数据
                    //循环12个月的数据
                    for (int i = 0; i < 12; i++)
                    {
                        if (StartTime.AddMonths(i) > DateTime.Now)
                        {
                            break;
                        }
                        CapitalDepartmentId list1 = new CapitalDepartmentId();
                        //存当前月份
                        list1.yefen = StartTime1.AddMonths(i).ToString("yyyy-MM");
                        var department = departmentIBLL.GetEntity(DepartmentIdT);
                        if (department != null)
                        {
                            list1.DepartmentIdName = department.F_FullName;
                        }
                        list1.datayyyyMM = StartTime1.AddMonths(i).ToString("yyyy");
                        list1.yefenList = list1.yefen + list1.DepartmentIdName;
                        list1.index = StartTime1.AddMonths(i).ToString("MM").ToInt();
                        //初始化
                        //本月合同额
                        list1.ContractAmountList = 0;
                        //本月绩效
                        list1.EffectiveAmountList = 0;
                        //本月资金
                        list1.ContractAmountSUNList = 0;
                        list1.AmountList = 0;
                        decimal? ContractAmountSUNList1 = 0;
                        //小计
                        list1.sumList = 0;
                        var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime1.ToString(), StartTime1.AddMonths(i).ToString(), StartTime1.AddMonths(i + 1).ToString(), DepartmentIdT);
                        if (data1.ToList().Count > 0)
                        {
                            foreach (var ins in data1)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //历史数据
                                //审核时间<当年
                                //本月绩效
                                if (ins.EffectiveAmount != null)
                                {
                                    //自主
                                    if (ins.ProjectSource == "1")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }
                                    //渠道
                                    if (ins.ProjectSource == "2")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }
                                }
                            }
                        }
                        //今年数据
                        var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime1.ToString(), StartTime1.AddMonths(i).ToString(), StartTime1.AddMonths(i + 1).ToString(), DepartmentIdT);
                        if (data.ToList().Count > 0)
                        {
                            //本月成本                 
                            CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                            if (capitalAmount != null)
                            {
                                list1.ContractAmountSUN = capitalAmount.CostAmount;
                            }
                            foreach (var inf in data)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //本月合同额
                                if (inf.EffectiveAmount != null)
                                {
                                    list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                                }
                                //本月绩效                               
                                var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                                if (inf.ContractAmount <= cay.Amount)
                                {
                                    if (inf.EffectiveAmount != null)
                                    {
                                        //自主
                                        if (inf.ProjectSource == "1")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }
                                        }
                                        //渠道
                                        if (inf.ProjectSource == "2")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }
                                        }
                                    }
                                }
                            }
                            //本月资金
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.ContractAmountSUNList != null)
                            {
                                list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;
                            }
                            list.Add(list1);
                            sumList1 = list1.ContractAmountSUNList;
                            xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                            xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                            if (list1.ContractAmountSUN != null)
                            {
                                xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                            }
                            xn.sumList = xn.sumList + list1.sumList;
                        }
                    }
                }
                xn.ContractAmountSUNList = xn.EffectiveAmountList - xn.ContractAmountSUN;
                list.Add(xn);
            }
            if (DateYYYY != null && DepartmentIdT == "")
            {
                if (DateYYYY.ToInt() == timeYY.ToInt())
                {
                    foreach (var dep in datadepartment)
                    {
                        ////今年数据
                        xn.yefen = "汇总";
                        //qn.yefen = "历史数据";
                        //上个月小计
                        decimal? sumList1 = 0;
                        //decimal? sumList12 = 0;
                        //循环12个月的数据
                        for (int i = 0; i < 12; i++)
                        {
                            if (StartTime.AddMonths(i) > DateTime.Now)
                            {
                                break;
                            }
                            CapitalDepartmentId list1 = new CapitalDepartmentId();
                            //存当前月份
                            list1.yefen = StartTime.AddMonths(i).ToString("yyyy-MM");
                            list1.DepartmentIdName = dep.F_FullName;
                            list1.datayyyyMM = StartTime.AddMonths(i).ToString("yyyy");
                            list1.yefenList = list1.yefen + list1.DepartmentIdName;
                            list1.index = StartTime.AddMonths(i).ToString("MM").ToInt();
                            //初始化
                            //本月合同额
                            list1.ContractAmountList = 0;
                            //本月绩效
                            list1.EffectiveAmountList = 0;
                            //本月资金
                            list1.ContractAmountSUNList = 0;
                            list1.AmountList = 0;
                            decimal? ContractAmountSUNList1 = 0;
                            //小计
                            list1.sumList = 0;
                            var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                            if (data1.ToList().Count > 0)
                            {
                                foreach (var ins in data1)
                                {
                                    decimal? EffectiveAmount1 = 0;
                                    //历史数据
                                    //审核时间<当年
                                    //本月绩效
                                    if (ins.EffectiveAmount != null)
                                    {
                                        //自主
                                        if (ins.ProjectSource == "1")
                                        {
                                            EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.AmountList = list1.AmountList + EffectiveAmount1;
                                            }
                                        }
                                        //渠道
                                        if (ins.ProjectSource == "2")
                                        {
                                            EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.AmountList = list1.AmountList + EffectiveAmount1;
                                            }
                                        }
                                    }
                                }
                            }
                            //今年数据
                            var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                            if (data.ToList().Count > 0)
                            {
                                //本月成本                 
                                CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                                if (capitalAmount != null)
                                {
                                    list1.ContractAmountSUN = capitalAmount.CostAmount;
                                }
                                foreach (var inf in data)
                                {
                                    decimal? EffectiveAmount1 = 0;
                                    //本月合同额
                                    if (inf.EffectiveAmount != null)
                                    {
                                        list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                                    }
                                    //本月绩效                               
                                    var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                                    if (inf.ContractAmount <= cay.Amount)
                                    {
                                        if (inf.EffectiveAmount != null)
                                        {
                                            //自主
                                            if (inf.ProjectSource == "1")
                                            {
                                                EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                                {
                                                    list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                                }
                                            }
                                            //渠道
                                            if (inf.ProjectSource == "2")
                                            {
                                                EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                                {
                                                    list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                                }
                                            }
                                        }

                                    }

                                }
                                //本月资金
                                if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                                {
                                    ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                                }
                                if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                                {
                                    ContractAmountSUNList1 = list1.ContractAmountSUN;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                                }
                                if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                                {
                                    ContractAmountSUNList1 = list1.EffectiveAmountList;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                                }
                                if (list1.ContractAmountSUNList != null)
                                {

                                    list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;

                                }
                                list.Add(list1);
                                sumList1 = list1.ContractAmountSUNList;
                                xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                                xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                                if (list1.ContractAmountSUN != null)
                                {
                                    xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                                }
                                xn.sumList = xn.sumList + list1.sumList;
                            }
                        }
                    }
                }
                else
                {
                    var t = timeYY.ToInt() - DateYYYY.ToInt();
                    StartTime1 = DateTime.Parse(DateTime.Now.AddYears(-t).ToString("yyyy-01"));
                    foreach (var dep in datadepartment)
                    {
                        ////今年数据
                        xn.yefen = "汇总";
                        //qn.yefen = "历史数据";
                        //上个月小计
                        decimal? sumList1 = 0;
                        //decimal? sumList12 = 0;
                        //循环12个月的数据
                        for (int i = 0; i < 12; i++)
                        {
                            if (StartTime.AddMonths(i) > DateTime.Now)
                            {
                                break;
                            }
                            CapitalDepartmentId list1 = new CapitalDepartmentId();
                            //存当前月份
                            list1.yefen = StartTime1.AddMonths(i).ToString("yyyy-MM");
                            list1.DepartmentIdName = dep.F_FullName;
                            list1.datayyyyMM = StartTime1.AddMonths(i).ToString("yyyy");
                            list1.yefenList = list1.yefen + list1.DepartmentIdName;
                            list1.index = StartTime1.AddMonths(i).ToString("MM").ToInt();
                            //初始化
                            //本月合同额
                            list1.ContractAmountList = 0;
                            //本月绩效
                            list1.EffectiveAmountList = 0;
                            //本月资金
                            list1.ContractAmountSUNList = 0;
                            list1.AmountList = 0;
                            decimal? ContractAmountSUNList1 = 0;
                            //小计
                            list1.sumList = 0;
                            var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime1.ToString(), StartTime1.AddMonths(i).ToString(), StartTime1.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                            if (data1.ToList().Count > 0)
                            {
                                foreach (var ins in data1)
                                {
                                    decimal? EffectiveAmount1 = 0;
                                    //历史数据
                                    //审核时间<当年
                                    //本月绩效
                                    if (ins.EffectiveAmount != null)
                                    {
                                        //自主
                                        if (ins.ProjectSource == "1")
                                        {
                                            EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.AmountList = list1.AmountList + EffectiveAmount1;
                                            }
                                        }
                                        //渠道
                                        if (ins.ProjectSource == "2")
                                        {
                                            EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.AmountList = list1.AmountList + EffectiveAmount1;
                                            }
                                        }

                                    }
                                }
                            }
                            //今年数据
                            var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime1.ToString(), StartTime1.AddMonths(i).ToString(), StartTime1.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                            if (data.ToList().Count > 0)
                            {
                                //本月成本                 
                                CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                                if (capitalAmount != null)
                                {
                                    list1.ContractAmountSUN = capitalAmount.CostAmount;
                                }
                                foreach (var inf in data)
                                {
                                    decimal? EffectiveAmount1 = 0;
                                    //本月合同额
                                    if (inf.EffectiveAmount != null)
                                    {
                                        list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                                    }

                                    //本月绩效                               
                                    var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                                    if (inf.ContractAmount <= cay.Amount)
                                    {
                                        if (inf.EffectiveAmount != null)
                                        {

                                            //自主
                                            if (inf.ProjectSource == "1")
                                            {
                                                EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                                {
                                                    list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                                }
                                            }
                                            //渠道
                                            if (inf.ProjectSource == "2")
                                            {
                                                EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                                if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                                {
                                                    list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                                }

                                            }
                                        }
                                    }
                                }
                                //本月资金
                                if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                                {
                                    ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                                }
                                if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                                {
                                    ContractAmountSUNList1 = list1.ContractAmountSUN;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                                }
                                if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                                {
                                    ContractAmountSUNList1 = list1.EffectiveAmountList;
                                    list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                                }
                                if (list1.ContractAmountSUNList != null)
                                {

                                    list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;

                                }
                                list.Add(list1);
                                sumList1 = list1.ContractAmountSUNList;
                                xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                                xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                                if (list1.ContractAmountSUN != null)
                                {
                                    xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                                }
                                xn.sumList = xn.sumList + list1.sumList;
                            }
                        }
                    }
                }
                xn.ContractAmountSUNList = xn.EffectiveAmountList - xn.ContractAmountSUN;
                list.Add(xn);
            }
            if (DateYYYY == "" && DepartmentIdT != null)
            {
                ////今年数据
                xn.yefen = "汇总";
                //qn.yefen = "历史数据";
                //上个月小计
                decimal? sumList1 = 0;
                //decimal? sumList12 = 0;
                //循环12个月的数据
                for (int i = 0; i < 12; i++)
                {
                    if (StartTime.AddMonths(i) > DateTime.Now)
                    {
                        break;
                    }
                    CapitalDepartmentId list1 = new CapitalDepartmentId();
                    //存当前月份
                    list1.yefen = StartTime.AddMonths(i).ToString("yyyy-MM");
                    var department = departmentIBLL.GetEntity(DepartmentIdT);
                    if (department != null)
                    {
                        list1.DepartmentIdName = department.F_FullName;
                    }
                    list1.datayyyyMM = StartTime.AddMonths(i).ToString("yyyy");
                    list1.yefenList = list1.yefen + list1.DepartmentIdName;
                    list1.index = StartTime.AddMonths(i).ToString("MM").ToInt();
                    //初始化
                    //本月合同额
                    list1.ContractAmountList = 0;
                    //本月绩效
                    list1.EffectiveAmountList = 0;
                    //本月资金
                    list1.ContractAmountSUNList = 0;
                    list1.AmountList = 0;
                    decimal? ContractAmountSUNList1 = 0;
                    //小计
                    list1.sumList = 0;
                    var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), DepartmentIdT);
                    if (data1.ToList().Count > 0)
                    {
                        foreach (var ins in data1)
                        {
                            decimal? EffectiveAmount1 = 0;
                            //历史数据
                            //审核时间<当年
                            //本月绩效
                            if (ins.EffectiveAmount != null)
                            {
                                //自主
                                if (ins.ProjectSource == "1")
                                {
                                    EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                    if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                    {
                                        list1.AmountList = list1.AmountList + EffectiveAmount1;
                                    }
                                }
                                //渠道
                                if (ins.ProjectSource == "2")
                                {
                                    EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                    if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                    {
                                        list1.AmountList = list1.AmountList + EffectiveAmount1;
                                    }
                                }
                            }
                        }
                    }
                    //今年数据
                    var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), DepartmentIdT);
                    if (data.ToList().Count > 0)
                    {
                        //本月成本                 
                        CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                        if (capitalAmount != null)
                        {
                            list1.ContractAmountSUN = capitalAmount.CostAmount;
                        }
                        foreach (var inf in data)
                        {
                            decimal? EffectiveAmount1 = 0;
                            //本月合同额
                            if (inf.EffectiveAmount != null)
                            {
                                list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                            }
                            //本月绩效                               
                            var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                            if (inf.ContractAmount <= cay.Amount)
                            {
                                if (inf.EffectiveAmount != null)
                                {

                                    //自主
                                    if (inf.ProjectSource == "1")
                                    {
                                        EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                        }
                                    }
                                    //渠道
                                    if (inf.ProjectSource == "2")
                                    {
                                        EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                        }
                                    }
                                }
                            }
                        }
                        //本月资金
                        if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                        {
                            ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                            list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                        }
                        if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                        {
                            ContractAmountSUNList1 = list1.ContractAmountSUN;
                            list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                        }
                        if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                        {
                            ContractAmountSUNList1 = list1.EffectiveAmountList;
                            list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                        }
                        if (list1.ContractAmountSUNList != null)
                        {
                            list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;
                        }
                        list.Add(list1);
                        sumList1 = list1.ContractAmountSUNList;
                        xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                        xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                        if (list1.ContractAmountSUN != null)
                        {
                            xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                        }
                        xn.sumList = xn.sumList + list1.sumList;
                    }
                }
                xn.ContractAmountSUNList = xn.EffectiveAmountList - xn.ContractAmountSUN;
                list.Add(xn);
            }
            if (DateYYYY == null && DepartmentIdT == null)
            {
                foreach (var dep in datadepartment)
                {
                    ////今年数据
                    xn.yefen = "汇总";
                    //qn.yefen = "历史数据";
                    //上个月小计
                    decimal? sumList1 = 0;
                    //decimal? sumList12 = 0;
                    //循环12个月的数据
                    for (int i = 0; i < 12; i++)
                    {
                        if (StartTime.AddMonths(i) > DateTime.Now)
                        {
                            break;
                        }
                        CapitalDepartmentId list1 = new CapitalDepartmentId();
                        //存当前月份
                        list1.yefen = StartTime.AddMonths(i).ToString("yyyy-MM");
                        list1.DepartmentIdName = dep.F_FullName;
                        list1.datayyyyMM = StartTime.AddMonths(i).ToString("yyyy");
                        list1.yefenList = list1.yefen + list1.DepartmentIdName;
                        list1.index = StartTime.AddMonths(i).ToString("MM").ToInt();

                        //初始化
                        //本月合同额
                        list1.ContractAmountList = 0;
                        //本月绩效
                        list1.EffectiveAmountList = 0;
                        //本月资金
                        list1.ContractAmountSUNList = 0;
                        list1.AmountList = 0;
                        decimal? ContractAmountSUNList1 = 0;
                        //小计
                        list1.sumList = 0;
                        var data1 = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId1(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                        if (data1.ToList().Count > 0)
                        {
                            foreach (var ins in data1)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //历史数据
                                //审核时间<当年
                                //本月绩效
                                if (ins.EffectiveAmount != null)
                                {
                                    //自主
                                    if (ins.ProjectSource == "1")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.3) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }
                                    //渠道
                                    if (ins.ProjectSource == "2")
                                    {
                                        EffectiveAmount1 = (ins.EffectiveAmount * (decimal)0.03) + (ins.EffectiveAmount * (decimal)0.2);
                                        if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                        {
                                            list1.AmountList = list1.AmountList + EffectiveAmount1;
                                        }
                                    }
                                }
                            }
                        }
                        //今年数据
                        var data = reportFormsBLL.GetCapitalDepartmentIdListDepartmentId(StartTime.ToString(), StartTime.AddMonths(i).ToString(), StartTime.AddMonths(i + 1).ToString(), dep.F_DepartmentId);
                        if (data.ToList().Count > 0)
                        {
                            //本月成本                 
                            CapitalAmountEntity capitalAmount = reportFormsBLL.getCapitalAmountByYearMonth(list1.yefenList);
                            if (capitalAmount != null)
                            {
                                list1.ContractAmountSUN = capitalAmount.CostAmount;
                            }
                            foreach (var inf in data)
                            {
                                decimal? EffectiveAmount1 = 0;
                                //本月合同额
                                if (inf.EffectiveAmount != null)
                                {
                                    list1.ContractAmountList = list1.ContractAmountList + inf.EffectiveAmount;
                                }
                                //本月绩效                               
                                var cay = projectPayCollectionIBLL.GetCollectionByIdProjectIdtIME(inf.pid, StartTime.AddMonths(i + 1).ToString());
                                if (inf.ContractAmount <= cay.Amount)
                                {
                                    if (inf.EffectiveAmount != null)
                                    {
                                        //自主
                                        if (inf.ProjectSource == "1")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.3) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }
                                        }
                                        //渠道
                                        if (inf.ProjectSource == "2")
                                        {
                                            EffectiveAmount1 = (inf.EffectiveAmount * (decimal)0.03) + (inf.EffectiveAmount * (decimal)0.2);
                                            if (EffectiveAmount1 != 0 && EffectiveAmount1 != null)
                                            {
                                                list1.EffectiveAmountList = list1.EffectiveAmountList + EffectiveAmount1;
                                            }
                                        }
                                    }
                                }
                            }
                            //本月资金
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList - list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList == null && list1.ContractAmountSUN != null)
                            {
                                ContractAmountSUNList1 = list1.ContractAmountSUN;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList - ContractAmountSUNList1;
                            }
                            if (list1.EffectiveAmountList != null && list1.ContractAmountSUN == null)
                            {
                                ContractAmountSUNList1 = list1.EffectiveAmountList;
                                list1.ContractAmountSUNList = list1.ContractAmountSUNList + ContractAmountSUNList1;
                            }
                            if (list1.ContractAmountSUNList != null)
                            {
                                list1.sumList = list1.sumList + list1.ContractAmountSUNList + sumList1;
                            }
                            list.Add(list1);
                            sumList1 = list1.ContractAmountSUNList;
                            xn.ContractAmountList = xn.ContractAmountList + list1.ContractAmountList;
                            xn.EffectiveAmountList = xn.EffectiveAmountList + list1.EffectiveAmountList;
                            if (list1.ContractAmountSUN != null)
                            {
                                xn.ContractAmountSUN = xn.ContractAmountSUN + list1.ContractAmountSUN;
                            }
                            xn.sumList = xn.sumList + list1.sumList;
                            if (list1.AmountList != null)
                            {
                                xn.AmountList = xn.AmountList + list1.AmountList;
                            }
                        }
                    }
                }
                xn.ContractAmountSUNList = xn.EffectiveAmountList - xn.ContractAmountSUN;
                list.Add(xn);
            }
            //降序
            list = list.OrderByDescending(t => t.index).ThenByDescending(t => t.datayyyyMM).ToList();
            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)

            };
            return Success(jsonData);
        }

        //营销台账合计
        public Response GetMarketingSUM(dynamic _)
        {

            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = reportFormsBLL.GetMarketings(parameter.queryJson);
            MarketingEntity q = new MarketingEntity();
            q.ContractAmountSum = 0;
            q.AmountSum = 0;
            q.NotReceivedSum = 0;
            q.OwnSum = 0;
            q.DitchSum = 0;
            q.ConsociationSum = 0;
            foreach (var item in data)
            {
                //MarketingEntity q = new MarketingEntity();

                if (item.ContractAmount != null)
                {
                    q.ContractAmountSum = q.ContractAmountSum + item.ContractAmount;
                }
                if (item.Amount != null)
                {
                    q.AmountSum = q.AmountSum + item.Amount;
                }
                if (item.NotReceived != null)
                {
                    q.NotReceivedSum = q.NotReceivedSum + item.NotReceived;
                }

                if (item.ProjectSource == 1.ToString() && item.ContractAmount.HasValue)
                {
                    q.OwnSum = q.OwnSum + item.ContractAmount;
                }
                if (item.ProjectSource == 2.ToString() && item.ContractAmount.HasValue)
                {
                    q.DitchSum = q.DitchSum + item.ContractAmount;
                }
                if (item.ProjectSource == 3.ToString() && item.ContractAmount.HasValue)
                {
                    q.ConsociationSum = q.ConsociationSum + item.ContractAmount;
                }
            }
            var result = new
            {
                ContractAmountSum = q.ContractAmountSum,
                AmountSum = q.AmountSum,
                NotReceivedSum = q.NotReceivedSum,
                OwnSum = q.OwnSum,
                DitchSum = q.DitchSum,
                ConsociationSum = q.ConsociationSum
            };
            var jsonData = new
            {
                rows = result
            };
            return Success(jsonData);
        }

        //营销台账
        public Response GetMarketingList(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = reportFormsBLL.GetMarketings(parameter.pagination, parameter.queryJson);
            List<MarketingEntity> list = new List<MarketingEntity>();
            var departmentList = departmentIBLL.GetEntityList();
            var userList = userIBLL.GetAllList();
            foreach (var info in data)
            {
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //审核时间               
                if (info.ApproverTime != null)
                {
                    DateTime time1 = (DateTime)info.ApproverTime;
                    info.ApproverTimeMd = time1.ToString("yyyy-MM-dd");
                }
                //到款日期               
                if (info.ReceiptDate != null)
                {
                    DateTime time1 = (DateTime)info.ReceiptDate;
                    info.ReceiptDateMd = time1.ToString("yyyy-MM-dd");
                }
                //检测时间               
                if (!string.IsNullOrEmpty(info.ApproachTime))
                {
                    var item_list = info.ApproachTime.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (item_list.Count > 0)
                    {
                        for (int i = 0; i < item_list.Count; i++)
                        {
                            DateTime time1 = DateTime.Parse(item_list[i]);
                            var ApproachTimeMd = time1.ToString("yyyy-MM-dd");
                            info.ApproachTimeMd += ApproachTimeMd;
                            if (i < item_list.Count - 1)
                            {
                                info.ApproachTimeMd += ";";
                            }
                        }
                    }
                }
                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                if (followPerson != null)
                {
                    info.FollowPersonName = followPerson.F_RealName;

                }
                //项目负责人
                //var projectResponsible = userIBLL.GetEntityByUserId(info.ProjectResponsible);
                //if (projectResponsible != null)
                //{
                //    info.ProjectResponsibleName = projectResponsible.F_RealName;
                //}
                if (!string.IsNullOrEmpty(info.ProjectResponsible))
                {
                    var item_list = info.ProjectResponsible.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (item_list.Count > 0)
                    {
                        for (int i = 0; i < item_list.Count; i++)
                        {
                            var Items = userList.Where(ii => ii.F_UserId == item_list[i]).ToList();
                            if (Items.Count > 0)
                            {
                                info.ProjectResponsibleName = info.ProjectResponsibleName + Items[0].F_RealName;
                                if (i < item_list.Count - 1)
                                {
                                    info.ProjectResponsibleName += ";";
                                }
                            }
                        }
                    }
                }
                //部门
                if (!string.IsNullOrEmpty(info.DepartmentId))
                {
                    var item_list = info.DepartmentId.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (item_list.Count > 0)
                    {
                        for (int i = 0; i < item_list.Count; i++)
                        {
                            var Items = departmentList.Where(ii => ii.F_DepartmentId == item_list[i]).ToList();
                            if (Items.Count > 0)
                            {
                                info.DepartmentName = info.DepartmentName + Items[0].F_FullName;
                                if (i < item_list.Count - 1)
                                {
                                    info.DepartmentName += ";";
                                }
                            }
                        }
                    }
                }
                //实施部门
                if (!string.IsNullOrEmpty(info.J_F_FullName))
                {
                    string full_name = "";
                    var J_F_FullName_list = info.J_F_FullName.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (J_F_FullName_list.Count > 0)
                    {
                        for (int i = 0; i < J_F_FullName_list.Count; i++)
                        {
                            var department1 = departmentIBLL.GetEntity(J_F_FullName_list[i]);
                            if (department1 != null)
                            {
                                full_name = full_name + department1.F_FullName + "；";
                            }
                        }
                    }
                    info.J_F_FullName = full_name;
                }
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }
                //报告主体
                //var reportSubject = dataItemBLL.GetDetailItemName(info.ReportSubject, "ContractSubject");
                //if (reportSubject != null)
                //{
                //    info.ReportSubjectName = reportSubject.F_ItemName;
                //}
                if (!string.IsNullOrEmpty(info.ReportSubject))
                {
                    var item_list = info.ReportSubject.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (item_list.Count > 0)
                    {
                        for (int i = 0; i < item_list.Count; i++)
                        {
                            var Item = dataItemBLL.GetDetailItemName(item_list[i], "ContractSubject");
                            if (Item != null)
                            {
                                info.ReportSubjectName = info.ReportSubjectName + Item.F_ItemName;
                                if (i < item_list.Count - 1)
                                {
                                    info.ReportSubjectName += ";";
                                }
                            }
                        }
                    }
                }
                //报告状态
                if (!string.IsNullOrEmpty(info.TaskStatus))
                {
                    var TaskStatusName_list = info.TaskStatus.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (TaskStatusName_list.Count > 0)
                    {
                        for (int i = 0; i < TaskStatusName_list.Count; i++)
                        {
                            var TaskStatusName = dataItemBLL.GetDetailItemName(TaskStatusName_list[i], "TaskStatus");
                            if (TaskStatusName != null)
                            {
                                info.TaskStatusName = info.TaskStatusName + TaskStatusName.F_ItemName;
                                if (i < TaskStatusName_list.Count - 1)
                                {
                                    info.TaskStatusName += ";";
                                }
                            }
                        }
                    }
                }
                //合同主体
                var contractSubject = dataItemBLL.GetDetailItemName(info.ContractSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    info.ContractSubjectName = contractSubject.F_ItemName;
                }
                //归档情况
                if (info.ReceivedFlag.ToInt() != 0)
                {
                    info.ReceivedFlagName = "已归档";
                }
                else
                {
                    info.ReceivedFlagName = "未归档";
                }
                //开票情况
                if (info.BillingStatus.ToInt() != 0)
                {
                    info.BillingStatusName = "已开票";
                }
                else
                {
                    info.BillingStatusName = "未开票";
                }
                //营销部门
                if (info.MainDepartmentId != null)
                {
                    var department2 = departmentIBLL.GetEntity(info.MainDepartmentId);
                    if (department2 != null)
                    {
                        info.MainDepartmentName = department2.F_FullName;
                    }
                }
                //次部门
                if (info.SubDepartmentId != null)
                {
                    var department3 = departmentIBLL.GetEntity(info.SubDepartmentId);
                    if (department3 != null)
                    {
                        info.SubDepartmentName = department3.F_FullName;
                    }
                }
                list.Add(info);
            }

            var jsonData = new
            {

                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records,
                /* ContractAmountSum = ContractAmountSum,
                 AmountSum = AmountSum,
                 NotReceivedSum = NotReceivedSum,
                 OwnSum = OwnSum,
                 DitchSum = DitchSum,
                 ConsociationSum = ConsociationSum,*/
            };

            return Success(jsonData);


        }
        //营销台账导出
        public Response GetMarketingAll(dynamic _)
        {

            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = reportFormsBLL.GetMarketings(parameter.queryJson);
            List<MarketingEntity> list = new List<MarketingEntity>();
            var departmentList = departmentIBLL.GetEntityList();
            var userList = userIBLL.GetAllList();
            foreach (var info in data)
            {
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                if (info.FinishTime != null)
                {
                    DateTime time1 = (DateTime)info.FinishTime;
                    info.FinishTimeMd = time1.ToString("yyyy-MM-dd");
                }
                //审核时间               
                if (info.ApproverTime != null)
                {
                    DateTime time1 = (DateTime)info.ApproverTime;
                    info.ApproverTimeMd = time1.ToString("yyyy-MM-dd");
                }
                //到款日期               
                if (info.ReceiptDate != null)
                {
                    DateTime time1 = (DateTime)info.ReceiptDate;
                    info.ReceiptDateMd = time1.ToString("yyyy-MM-dd");
                }
                //检测时间               
                //if (info.ApproachTime != null)
                //{
                //    DateTime time1 = (DateTime)info.ApproachTime;
                //    info.ApproachTimeMd = time1.ToString("yyyy-MM-dd");
                //}
                //营销人员
                //var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                var followPerson = userList.Where(ii => ii.F_UserId == info.FollowPerson).ToList();
                if (followPerson.Count > 0)
                {
                    info.FollowPersonName = followPerson[0].F_RealName;

                }
                //项目负责人
                if (!string.IsNullOrEmpty(info.ProjectResponsible))
                {
                    var item_list = info.ProjectResponsible.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (item_list.Count > 0)
                    {
                        for (int i = 0; i < item_list.Count; i++)
                        {
                            var Items = userList.Where(ii => ii.F_UserId == item_list[i]).ToList();
                            if (Items.Count > 0)
                            {
                                info.ProjectResponsibleName = info.ProjectResponsibleName + Items[0].F_RealName;
                                if (i < item_list.Count - 1)
                                {
                                    info.ProjectResponsibleName += ";";
                                }
                            }
                        }
                    }
                }
                //部门
                if (!string.IsNullOrEmpty(info.DepartmentId))
                {
                    var item_list = info.DepartmentId.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (item_list.Count > 0)
                    {
                        for (int i = 0; i < item_list.Count; i++)
                        {
                            var Items = departmentList.Where(ii => ii.F_DepartmentId == item_list[i]).ToList();
                            if (Items.Count > 0)
                            {
                                info.DepartmentName = info.DepartmentName + Items[0].F_FullName;
                                if (i < item_list.Count - 1)
                                {
                                    info.DepartmentName += ";";
                                }
                            }
                        }
                    }
                }
                //核算
                //营销核算

                //自主营销
                if (info.ProjectSource == "1")
                {
                    if (info.PaymentAmount == null)
                    {
                        info.DepartmentIdAmount = info.ContractAmount * (decimal?)0.3;
                        if (info.DepartmentIdAmount != null)
                        {
                            //info.DepartmentIdAmountName = Math.Round((decimal)info.DepartmentIdAmount * 100) / 100;
                            info.DepartmentIdAmountName = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                        }
                    }
                    else if (info.PaymentAmount < (info.ContractAmount * (decimal?)0.3))
                    {
                        info.DepartmentIdAmount = (info.ContractAmount * (decimal?)0.3) - info.PaymentAmount;
                        if (info.DepartmentIdAmount != null)
                        {
                            info.DepartmentIdAmountName = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                        }
                    }
                    else
                    {
                        info.DepartmentIdAmount = info.ContractAmount * (decimal?)0.03;
                        if (info.DepartmentIdAmount != null)
                        {
                            info.DepartmentIdAmountName = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                        }
                    }
                }

                //实施部门
                if (!string.IsNullOrEmpty(info.J_F_FullName))
                {
                    string full_name = "";
                    var J_F_FullName_list = info.J_F_FullName.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (J_F_FullName_list.Count > 0)
                    {
                        for (int i = 0; i < J_F_FullName_list.Count; i++)
                        {
                            //var department1 = departmentIBLL.GetEntity(J_F_FullName_list[i]);
                            var department1 = departmentList.Where(ii => ii.F_DepartmentId == J_F_FullName_list[i]).ToList();
                            if (department1.Count > 0)
                            {
                                full_name = full_name + department1[0].F_FullName ;
                            }
                            if (i < J_F_FullName_list.Count - 1)
                            {
                                full_name += ";";
                            }
                        }
                    }
                    info.J_F_FullName = full_name;
                }
                //var department1 = departmentIBLL.GetEntity(info.J_F_FullName);
                //if (department1 != null)
                //{
                //    info.J_F_FullName = department1.F_FullName;
                //}
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }
                //报告主体
                if (!string.IsNullOrEmpty(info.ReportSubject))
                {
                    var item_list = info.ReportSubject.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (item_list.Count > 0)
                    {
                        for (int i = 0; i < item_list.Count; i++)
                        {
                            var Item = dataItemBLL.GetDetailItemName(item_list[i], "ContractSubject");
                            if (Item != null)
                            {
                                info.ReportSubjectName = info.ReportSubjectName + Item.F_ItemName;
                                if (i < item_list.Count - 1)
                                {
                                    info.ReportSubjectName += ";";
                                }
                            }
                        }
                    }
                }
                //报告状态
                if (!string.IsNullOrEmpty(info.TaskStatus))
                {
                    var TaskStatusName_list = info.TaskStatus.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (TaskStatusName_list.Count > 0)
                    {
                        for (int i = 0; i < TaskStatusName_list.Count; i++)
                        {
                            var TaskStatusName = dataItemBLL.GetDetailItemName(TaskStatusName_list[i], "TaskStatus");
                            if (TaskStatusName != null)
                            {
                                info.TaskStatusName = info.TaskStatusName + TaskStatusName.F_ItemName;
                                if (i < TaskStatusName_list.Count - 1)
                                {
                                    info.TaskStatusName += ";";
                                }
                            }
                        }
                    }
                }
                //合同主体
                var contractSubject = dataItemBLL.GetDetailItemName(info.ContractSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    info.ContractSubjectName = contractSubject.F_ItemName;
                }
                //归档情况
                if (info.ReceivedFlag.ToInt() != 0)
                {
                    info.ReceivedFlagName = "已归档";
                }
                else
                {
                    info.ReceivedFlagName = "未归档";
                }
                //开票情况
                if (info.BillingStatus.ToInt() != 0)
                {
                    info.BillingStatusName = "已开票";
                }
                else
                {
                    info.BillingStatusName = "未开票";
                }
                //营销部门
                if (info.MainDepartmentId != null)
                {
                    //var department2 = departmentIBLL.GetEntity(info.MainDepartmentId);
                    var department2 = departmentList.Where(ii => ii.F_DepartmentId == info.MainDepartmentId).ToList();
                    if (department2.Count > 0)
                    {
                        info.MainDepartmentName = department2[0].F_FullName;
                    }
                }
                //营销部门
                if (info.SubDepartmentId != null)
                {
                    //var department3 = departmentIBLL.GetEntity(info.SubDepartmentId);
                    var department3 = departmentList.Where(ii => ii.F_DepartmentId == info.SubDepartmentId).ToList();
                    if (department3.Count > 0)
                    {
                        info.SubDepartmentName = department3[0].F_FullName;
                    }
                }

                list.Add(info);
            }
            list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);
        }

        //伙伴营销台账合计
        public Response GetMarketingSUMHZ(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = reportFormsBLL.GetMarketingsSumHZ(parameter.queryJson);
            MarketingEntity q = new MarketingEntity();
            q.ContractAmountSum = 0;
            q.AmountSum = 0;
            q.NotReceivedSum = 0;
            q.OwnSum = 0;
            q.DitchSum = 0;
            q.ConsociationSum = 0;
            foreach (var item in data)
            {
                //MarketingEntity q = new MarketingEntity();

                if (item.ContractAmount != null)
                {
                    q.ContractAmountSum = q.ContractAmountSum + item.ContractAmount;
                }
                if (item.Amount != null)
                {
                    q.AmountSum = q.AmountSum + item.Amount;
                }
                if (item.NotReceived != null)
                {
                    q.NotReceivedSum = q.NotReceivedSum + item.NotReceived;
                }

                if (item.ProjectSource == 1.ToString() && item.ContractAmount.HasValue)
                {
                    q.OwnSum = q.OwnSum + item.ContractAmount;
                }
                if (item.ProjectSource == 2.ToString() && item.ContractAmount.HasValue)
                {
                    q.DitchSum = q.DitchSum + item.ContractAmount;
                }
                if (item.ProjectSource == 3.ToString() && item.ContractAmount.HasValue)
                {
                    q.ConsociationSum = q.ConsociationSum + item.ContractAmount;
                }
            }

            var result = new
            {
                ContractAmountSum = q.ContractAmountSum,
                AmountSum = q.AmountSum,
                NotReceivedSum = q.NotReceivedSum,
                OwnSum = q.OwnSum,
                DitchSum = q.DitchSum,
                ConsociationSum = q.ConsociationSum
            };
            var jsonData = new
            {
                rows = result
            };
            return Success(jsonData);
        }

        //伙伴营销台账
        public Response GetMarketingListHZ(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = reportFormsBLL.GetMarketingsHZ(parameter.pagination, parameter.queryJson);
            List<MarketingEntity> list = new List<MarketingEntity>();
            var departmentList = departmentIBLL.GetEntityList();
            var userList = userIBLL.GetAllList();
            foreach (var info in data)
            {
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //审核时间               
                if (info.ApproverTime != null)
                {
                    DateTime time1 = (DateTime)info.ApproverTime;
                    info.ApproverTimeMd = time1.ToString("yyyy-MM-dd");
                }
                //到款日期               
                if (info.ReceiptDate != null)
                {
                    DateTime time1 = (DateTime)info.ReceiptDate;
                    info.ReceiptDateMd = time1.ToString("yyyy-MM-dd");
                }
                //检测时间               
                //if (info.ApproachTime != null)
                //{
                //    DateTime time1 = (DateTime)info.ApproachTime;
                //    info.ApproachTimeMd = time1.ToString("yyyy-MM-dd");
                //}
                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                if (followPerson != null)
                {
                    info.FollowPersonName = followPerson.F_RealName;

                }
                //项目负责人
                if (!string.IsNullOrEmpty(info.ProjectResponsible))
                {
                    var item_list = info.ProjectResponsible.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (item_list.Count > 0)
                    {
                        for (int i = 0; i < item_list.Count; i++)
                        {
                            var Items = userList.Where(ii => ii.F_UserId == item_list[i]).ToList();
                            if (Items.Count > 0)
                            {
                                info.ProjectResponsibleName = info.ProjectResponsibleName + Items[0].F_RealName;
                                if (i < item_list.Count - 1)
                                {
                                    info.ProjectResponsibleName += ";";
                                }
                            }
                        }
                    }
                }
                //部门
                if (!string.IsNullOrEmpty(info.DepartmentId))
                {
                    var item_list = info.DepartmentId.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (item_list.Count > 0)
                    {
                        for (int i = 0; i < item_list.Count; i++)
                        {
                            var Items = departmentList.Where(ii => ii.F_DepartmentId == item_list[i]).ToList();
                            if (Items.Count > 0)
                            {
                                info.DepartmentName = info.DepartmentName + Items[0].F_FullName;
                                if (i < item_list.Count - 1)
                                {
                                    info.DepartmentName += ";";
                                }
                            }
                        }
                    }
                }
                //实施部门
                var department1 = departmentIBLL.GetEntity(info.J_F_FullName);
                if (department1 != null)
                {
                    info.J_F_FullName = department1.F_FullName;
                }
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }
                //报告主体
                if (!string.IsNullOrEmpty(info.ReportSubject))
                {
                    var item_list = info.ReportSubject.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (item_list.Count > 0)
                    {
                        for (int i = 0; i < item_list.Count; i++)
                        {
                            var Item = dataItemBLL.GetDetailItemName(item_list[i], "ContractSubject");
                            if (Item != null)
                            {
                                info.ReportSubjectName = info.ReportSubjectName + Item.F_ItemName;
                                if (i < item_list.Count - 1)
                                {
                                    info.ReportSubjectName += ";";
                                }
                            }
                        }
                    }
                }
                //报告状态
                if (!string.IsNullOrEmpty(info.TaskStatus))
                {
                    var TaskStatusName_list = info.TaskStatus.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (TaskStatusName_list.Count > 0)
                    {
                        for (int i = 0; i < TaskStatusName_list.Count; i++)
                        {
                            var TaskStatusName = dataItemBLL.GetDetailItemName(TaskStatusName_list[i], "TaskStatus");
                            if (TaskStatusName != null)
                            {
                                info.TaskStatusName = info.TaskStatusName + TaskStatusName.F_ItemName;
                                if (i < TaskStatusName_list.Count - 1)
                                {
                                    info.TaskStatusName += ";";
                                }
                            }
                        }
                    }
                }
                //归档情况
                if (info.ReceivedFlag.ToInt() != 0)
                {
                    info.ReceivedFlagName = "已归档";
                }
                else
                {
                    info.ReceivedFlagName = "未归档";
                }
                //开票情况
                if (info.BillingStatus.ToInt() != 0)
                {
                    info.BillingStatusName = "已开票";
                }
                else
                {
                    info.BillingStatusName = "未开票";
                }
                //营销部门
                if (info.MainDepartmentId != null)
                {
                    var department2 = departmentIBLL.GetEntity(info.MainDepartmentId);
                    if (department2 != null)
                    {
                        info.MainDepartmentName = department2.F_FullName;
                    }
                }
                //营销部门
                if (info.SubDepartmentId != null)
                {
                    var department3 = departmentIBLL.GetEntity(info.SubDepartmentId);
                    if (department3 != null)
                    {
                        info.SubDepartmentName = department3.F_FullName;
                    }
                }
                list.Add(info);
            }

            var jsonData = new
            {

                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records,
                /* ContractAmountSum = ContractAmountSum,
                 AmountSum = AmountSum,
                 NotReceivedSum = NotReceivedSum,
                 OwnSum = OwnSum,
                 DitchSum = DitchSum,
                 ConsociationSum = ConsociationSum,*/
            };

            return Success(jsonData);


        }
        //伙伴营销台账导出
        public Response GetMarketingAllHZ(dynamic _)
        {

            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = reportFormsBLL.GetMarketingsSumHZ(parameter.queryJson);
            List<MarketingEntity> list = new List<MarketingEntity>();
            var departmentList = departmentIBLL.GetEntityList();
            var userList = userIBLL.GetAllList();
            foreach (var info in data)
            {
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //审核时间               
                if (info.ApproverTime != null)
                {
                    DateTime time1 = (DateTime)info.ApproverTime;
                    info.ApproverTimeMd = time1.ToString("yyyy-MM-dd");
                }
                //到款日期               
                if (info.ReceiptDate != null)
                {
                    DateTime time1 = (DateTime)info.ReceiptDate;
                    info.ReceiptDateMd = time1.ToString("yyyy-MM-dd");
                }
                //检测时间               
                //if (info.ApproachTime != null)
                //{
                //    DateTime time1 = (DateTime)info.ApproachTime;
                //    info.ApproachTimeMd = time1.ToString("yyyy-MM-dd");
                //}
                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                if (followPerson != null)
                {
                    info.FollowPersonName = followPerson.F_RealName;

                }
                //项目负责人
                if (!string.IsNullOrEmpty(info.ProjectResponsible))
                {
                    var item_list = info.ProjectResponsible.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (item_list.Count > 0)
                    {
                        for (int i = 0; i < item_list.Count; i++)
                        {
                            var Items = userList.Where(ii => ii.F_UserId == item_list[i]).ToList();
                            if (Items.Count > 0)
                            {
                                info.ProjectResponsibleName = info.ProjectResponsibleName + Items[0].F_RealName;
                                if (i < item_list.Count - 1)
                                {
                                    info.ProjectResponsibleName += ";";
                                }
                            }
                        }
                    }
                }
                //部门
                if (!string.IsNullOrEmpty(info.DepartmentId))
                {
                    var item_list = info.DepartmentId.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (item_list.Count > 0)
                    {
                        for (int i = 0; i < item_list.Count; i++)
                        {
                            var Items = departmentList.Where(ii => ii.F_DepartmentId == item_list[i]).ToList();
                            if (Items.Count > 0)
                            {
                                info.DepartmentName = info.DepartmentName + Items[0].F_FullName;
                                if (i < item_list.Count - 1)
                                {
                                    info.DepartmentName += ";";
                                }
                            }
                        }
                    }
                }
                //核算
                //营销核算

                //自主营销
                if (info.ProjectSource == "1")
                {
                    if (info.PaymentAmount == null)
                    {
                        info.DepartmentIdAmount = info.ContractAmount * (decimal?)0.3;
                        if (info.DepartmentIdAmount != null)
                        {
                            //info.DepartmentIdAmountName = Math.Round((decimal)info.DepartmentIdAmount * 100) / 100;
                            info.DepartmentIdAmountName = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                        }
                    }
                    else if (info.PaymentAmount < (info.ContractAmount * (decimal?)0.3))
                    {
                        info.DepartmentIdAmount = (info.ContractAmount * (decimal?)0.3) - info.PaymentAmount;
                        if (info.DepartmentIdAmount != null)
                        {
                            info.DepartmentIdAmountName = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                        }
                    }
                    else
                    {
                        info.DepartmentIdAmount = info.ContractAmount * (decimal?)0.03;
                        if (info.DepartmentIdAmount != null)
                        {
                            info.DepartmentIdAmountName = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                        }
                    }
                }

                //实施部门
                var department1 = departmentIBLL.GetEntity(info.J_F_FullName);
                if (department1 != null)
                {
                    info.J_F_FullName = department1.F_FullName;
                }
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }
                //报告主体
                if (!string.IsNullOrEmpty(info.ReportSubject))
                {
                    var item_list = info.ReportSubject.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (item_list.Count > 0)
                    {
                        for (int i = 0; i < item_list.Count; i++)
                        {
                            var Item = dataItemBLL.GetDetailItemName(item_list[i], "ContractSubject");
                            if (Item != null)
                            {
                                info.ReportSubjectName = info.ReportSubjectName + Item.F_ItemName;
                                if (i < item_list.Count - 1)
                                {
                                    info.ReportSubjectName += ";";
                                }
                            }
                        }
                    }
                }
                //报告状态
                if (!string.IsNullOrEmpty(info.TaskStatus))
                {
                    var TaskStatusName_list = info.TaskStatus.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    if (TaskStatusName_list.Count > 0)
                    {
                        for (int i = 0; i < TaskStatusName_list.Count; i++)
                        {
                            var TaskStatusName = dataItemBLL.GetDetailItemName(TaskStatusName_list[i], "TaskStatus");
                            if (TaskStatusName != null)
                            {
                                info.TaskStatusName = info.TaskStatusName + TaskStatusName.F_ItemName;
                                if (i < TaskStatusName_list.Count - 1)
                                {
                                    info.TaskStatusName += ";";
                                }
                            }
                        }
                    }
                }
                //归档情况
                if (info.ReceivedFlag.ToInt() != 0)
                {
                    info.ReceivedFlagName = "已归档";
                }
                else
                {
                    info.ReceivedFlagName = "未归档";
                }
                //开票情况
                if (info.BillingStatus.ToInt() != 0)
                {
                    info.BillingStatusName = "已开票";
                }
                else
                {
                    info.BillingStatusName = "未开票";
                }
                //营销部门
                if (info.MainDepartmentId != null)
                {
                    var department2 = departmentIBLL.GetEntity(info.MainDepartmentId);
                    if (department2 != null)
                    {
                        info.MainDepartmentName = department2.F_FullName;
                    }
                }
                //营销部门
                if (info.SubDepartmentId != null)
                {
                    var department3 = departmentIBLL.GetEntity(info.SubDepartmentId);
                    if (department3 != null)
                    {
                        info.SubDepartmentName = department3.F_FullName;
                    }
                }

                list.Add(info);
            }
            list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);
        }


        //生产台账
        public Response GetProductionsList(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = reportFormsBLL.GetProductions(parameter.pagination, parameter.queryJson);
            List<ProductionEntity> list = new List<ProductionEntity>();
            foreach (var info in data)
            {
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //到款日期               
                if (info.ReceiptDate != null)
                {
                    DateTime time1 = (DateTime)info.ReceiptDate;
                    info.ReceiptDateMd = time1.ToString("yyyy-MM-dd");
                }
                //检测时间               
                if (info.ApproachTime != null)
                {
                    DateTime time1 = (DateTime)info.ApproachTime;
                    info.ApproachTimeMd = time1.ToString("yyyy-MM-dd");
                }
                //报告时间               
                if (info.FlowFinishedTime != null)
                {
                    DateTime time1 = (DateTime)info.FlowFinishedTime;
                    info.FlowFinishedTimeMd = time1.ToString("yyyy-MM-dd");
                }

                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                if (followPerson != null)
                {
                    info.FollowPersonName = followPerson.F_RealName;

                }
                //项目负责人
                var projectResponsible = userIBLL.GetEntityByUserId(info.ProjectResponsible);
                if (projectResponsible != null)
                {
                    info.ProjectResponsibleName = projectResponsible.F_RealName;
                }
                //部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }
                //实施部门
                var department1 = departmentIBLL.GetEntity(info.J_F_FullName);
                if (department1 != null)
                {
                    info.J_F_FullName = department1.F_FullName;
                }
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }
                //报告主体
                var reportSubject = dataItemBLL.GetDetailItemName(info.ReportSubject, "ContractSubject");
                if (reportSubject != null)
                {
                    info.ReportSubjectName = reportSubject.F_ItemName;
                }
                //合同主体
                var contractSubject = dataItemBLL.GetDetailItemName(info.ContractSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    info.ContractSubjectName = contractSubject.F_ItemName;
                }
                //报告状态
                var taskStatus = dataItemBLL.GetDetailItemName(info.TaskStatus, "TaskStatus");
                if (taskStatus != null)
                {
                    info.TaskStatusName = taskStatus.F_ItemName;
                }
                //归档情况
                if (info.ReceivedFlag.ToInt() != 0)
                {
                    info.ReceivedFlagName = "已归档";
                }
                else
                {
                    info.ReceivedFlagName = "未归档";
                }
                //开票情况
                if (info.BillingStatus.ToInt() != 0)
                {
                    info.BillingStatusName = "已开票";
                }
                else
                {
                    info.BillingStatusName = "未开票";
                }
                //营销部门
                if (info.MainDepartmentId != null)
                {
                    var department2 = departmentIBLL.GetEntity(info.MainDepartmentId);
                    if (department2 != null)
                    {
                        info.MainDepartmentName = department2.F_FullName;
                    }
                }
                //营销部门
                if (info.SubDepartmentId != null)
                {
                    var department3 = departmentIBLL.GetEntity(info.SubDepartmentId);
                    if (department3 != null)
                    {
                        info.SubDepartmentName = department3.F_FullName;
                    }
                }
                list.Add(info);
            }

            var jsonData = new
            {

                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records,
            };

            return Success(jsonData);


        }

        //生产导出
        public Response GetProductionsSUM(dynamic _)
        {

            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            /*    var data = reportFormsBLL.GetMarketings(parameter.queryJson);
                MarketingEntity q = new MarketingEntity();
                q.ContractAmountSum = 0;
                q.AmountSum = 0;
                q.NotReceivedSum = 0;
                q.OwnSum = 0;
                q.DitchSum = 0;
                q.ConsociationSum = 0;*/
            var dataItem = reportFormsBLL.GetProductions(parameter.queryJson);
            decimal ContractAmountSum = 0;
            decimal OwnSum = 0;
            decimal DitchSum = 0;
            decimal ConsociationSum = 0;
            foreach (var item in dataItem)
            {
                decimal contractAmount = item.ContractAmount.HasValue ? item.ContractAmount.Value : 0;
                ContractAmountSum = ContractAmountSum + contractAmount;
                if (item.ProjectSource == 1.ToString() && item.ContractAmount.HasValue)
                {
                    OwnSum = OwnSum + item.ContractAmount.Value;
                }
                if (item.ProjectSource == 2.ToString() && item.ContractAmount.HasValue)
                {
                    DitchSum = DitchSum + item.ContractAmount.Value;
                }
                if (item.ProjectSource == 3.ToString() && item.ContractAmount.HasValue)
                {
                    ConsociationSum = ConsociationSum + item.ContractAmount.Value;
                }
            };
            var result = new
            {
                data = dataItem,
                ContractAmountSum = ContractAmountSum,
                OwnSum = OwnSum,
                DitchSum = DitchSum,
                ConsociationSum = ConsociationSum
            };
            var jsonData = new
            {
                rows = result
            };
            return Success(jsonData);
        }
        //生产台账导出
        public Response GetProductionsAll(dynamic _)
        {

            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = reportFormsBLL.GetProductions(parameter.queryJson);
            List<ProductionEntity> list = new List<ProductionEntity>();
            foreach (var info in data)
            {
                //创建时间
                if (info.CreateTime != null)
                {
                    DateTime time = (DateTime)info.CreateTime;
                    info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                }
                //审核时间               
                if (info.ApproverTime != null)
                {
                    DateTime time1 = (DateTime)info.ApproverTime;
                    info.ApproverTimeMd = time1.ToString("yyyy-MM-dd");
                }
                //到款日期               
                if (info.ReceiptDate != null)
                {
                    DateTime time1 = (DateTime)info.ReceiptDate;
                    info.ReceiptDateMd = time1.ToString("yyyy-MM-dd");
                }
                //检测时间               
                if (info.ApproachTime != null)
                {
                    DateTime time1 = (DateTime)info.ApproachTime;
                    info.ApproachTimeMd = time1.ToString("yyyy-MM-dd");
                }
                //报告时间               
                if (info.FlowFinishedTime != null)
                {
                    DateTime time1 = (DateTime)info.FlowFinishedTime;
                    info.FlowFinishedTimeMd = time1.ToString("yyyy-MM-dd");
                }
                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                if (followPerson != null)
                {
                    info.FollowPersonName = followPerson.F_RealName;

                }
                //项目负责人
                var projectResponsible = userIBLL.GetEntityByUserId(info.ProjectResponsible);
                if (projectResponsible != null)
                {
                    info.ProjectResponsibleName = projectResponsible.F_RealName;
                }
                //部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }
                //实施部门
                var department1 = departmentIBLL.GetEntity(info.J_F_FullName);
                if (department1 != null)
                {
                    info.J_F_FullName = department1.F_FullName;
                }
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }
                //报告主体
                var reportSubject = dataItemBLL.GetDetailItemName(info.ReportSubject, "ContractSubject");
                if (reportSubject != null)
                {
                    info.ReportSubjectName = reportSubject.F_ItemName;
                }
                //合同主体
                var contractSubject = dataItemBLL.GetDetailItemName(info.ContractSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    info.ContractSubjectName = contractSubject.F_ItemName;
                }
                //报告状态
                var taskStatus = dataItemBLL.GetDetailItemName(info.TaskStatus, "TaskStatus");
                if (taskStatus != null)
                {
                    info.TaskStatusName = taskStatus.F_ItemName;
                }
                if (info.ProjectSource == "1" || info.ProjectSource == "2")
                {
                    if (info.TaskStatus == "3" || info.TaskStatus == "4" || info.TaskStatus == "9" || info.TaskStatus == "5")
                    {
                        if (info.PaymentAmount != null)
                        {
                            if (info.PaymentAmount >= (info.ContractAmount * (decimal?)0.3))
                            {
                                if (info.SubAmount != null && info.MainAmount == null)
                                {
                                    info.DepartmentIdAmount = (info.ContractAmount - info.PaymentAmount - info.SubAmount) * (decimal?)0.3;
                                    if (info.DepartmentIdAmount != null)
                                    {
                                        info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                    }
                                }
                                else if (info.SubAmount == null && info.MainAmount != null)
                                {
                                    info.DepartmentIdAmount = (info.ContractAmount - info.PaymentAmount - info.MainAmount) * (decimal?)0.3;
                                    if (info.DepartmentIdAmount != null)
                                    {
                                        info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                    }
                                }
                                else
                                {
                                    info.DepartmentIdAmount = (info.ContractAmount - info.PaymentAmount) * (decimal?)0.3;
                                    if (info.DepartmentIdAmount != null)
                                    {
                                        info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                    }
                                }
                            }
                            else if (info.PaymentAmount < (info.ContractAmount * (decimal?)0.3))
                            {
                                info.DepartmentIdAmount = info.ContractAmount * (decimal?)0.2;
                                if (info.DepartmentIdAmount != null)
                                {
                                    info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                }
                                //item.DepartmentIdAmount = decimal.Round(decimal.Parse((item.ContractAmount * (decimal?)0.2).ToString()), 2);
                            }
                        }
                        else
                        {
                            if (info.ContractAmount != null)
                            {
                                info.DepartmentIdAmount = (info.ContractAmount * (decimal?)0.2);
                                if (info.DepartmentIdAmount != null)
                                {
                                    info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                }
                            }
                        }


                    }

                }
                //归档情况
                if (info.ReceivedFlag.ToInt() != 0)
                {
                    info.ReceivedFlagName = "已归档";
                }
                else
                {
                    info.ReceivedFlagName = "未归档";
                }
                //开票情况
                if (info.BillingStatus.ToInt() != 0)
                {
                    info.BillingStatusName = "已开票";
                }
                else
                {
                    info.BillingStatusName = "未开票";
                }

                list.Add(info);
            }
            list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);
        }
        //结算台账
        public Response GetSettleAccountsList(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = reportFormsBLL.GetSettleAccounts(parameter.pagination, parameter.queryJson);
            List<SettleAccountsEntity> list = new List<SettleAccountsEntity>();
            foreach (var info in data)
            {
                //创建时间
                if (info.CreateTime != null)
                {
                    DateTime time = (DateTime)info.CreateTime;
                    info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                }
                //审核时间               
                if (info.ApproverTime != null)
                {
                    DateTime time1 = (DateTime)info.ApproverTime;
                    info.ApproverTimeMd = time1.ToString("yyyy-MM-dd");
                }
                //到款日期               
                if (info.ReceiptDate != null)
                {
                    DateTime time1 = (DateTime)info.ReceiptDate;
                    info.ReceiptDateMd = time1.ToString("yyyy-MM-dd");
                }

                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                if (followPerson != null)
                {
                    info.FollowPersonName = followPerson.F_RealName;

                }
                //项目负责人
                var projectResponsible = userIBLL.GetEntityByUserId(info.ProjectResponsible);
                if (projectResponsible != null)
                {
                    info.ProjectResponsibleName = projectResponsible.F_RealName;
                }
                //部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }
                //实施部门
                var department1 = departmentIBLL.GetEntity(info.J_F_FullName);
                if (department1 != null)
                {
                    info.J_F_FullName = department1.F_FullName;
                }
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }

                //合同主体
                var contractSubject = dataItemBLL.GetDetailItemName(info.ContractSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    info.ContractSubjectName = contractSubject.F_ItemName;
                }
                //报告状态
                var taskStatus = dataItemBLL.GetDetailItemName(info.TaskStatus, "TaskStatus");
                if (taskStatus != null)
                {
                    info.TaskStatusName = taskStatus.F_ItemName;
                }
                //归档情况
                if (info.ReceivedFlag.ToInt() != 0)
                {
                    info.ReceivedFlagName = "已归档";
                }
                else
                {
                    info.ReceivedFlagName = "未归档";
                }

                list.Add(info);
            }

            var jsonData = new
            {

                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records,
            };

            return Success(jsonData);


        }

        //结算台账导出
        public Response GetSettleAccountsAll(dynamic _)
        {

            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = reportFormsBLL.GetSettleAccountsSum_new(parameter.queryJson);
            List<SettleAccountsEntity> list = new List<SettleAccountsEntity>();
            foreach (var info in data)
            {
                //创建时间
                if (info.CreateTime != null)
                {
                    DateTime time = (DateTime)info.CreateTime;
                    info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                }
                //审核时间               
                if (info.ApproverTime != null)
                {
                    DateTime time1 = (DateTime)info.ApproverTime;
                    info.ApproverTimeMd = time1.ToString("yyyy-MM-dd");
                }
                //到款日期               
                if (info.ReceiptDate != null)
                {
                    DateTime time1 = (DateTime)info.ReceiptDate;
                    info.ReceiptDateMd = time1.ToString("yyyy-MM-dd");
                }

                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                if (followPerson != null)
                {
                    info.FollowPersonName = followPerson.F_RealName;

                }
                //项目负责人
                var projectResponsible = userIBLL.GetEntityByUserId(info.ProjectResponsible);
                if (projectResponsible != null)
                {
                    info.ProjectResponsibleName = projectResponsible.F_RealName;
                }
                //部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }
                //实施部门
                var department1 = departmentIBLL.GetEntity(info.J_F_FullName);
                if (department1 != null)
                {
                    info.J_F_FullName = department1.F_FullName;
                }
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }

                //合同主体
                var contractSubject = dataItemBLL.GetDetailItemName(info.ContractSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    info.ContractSubjectName = contractSubject.F_ItemName;
                }
                //报告状态
                var taskStatus = dataItemBLL.GetDetailItemName(info.TaskStatus, "TaskStatus");
                if (taskStatus != null)
                {
                    info.TaskStatusName = taskStatus.F_ItemName;
                }

                //核算1
                //自主营销
                if (info.ProjectSource == "1")
                {
                    if (info.PaymentAmount == null)
                    {

                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.02;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                    else if (info.PaymentAmount < (info.ContractAmount * (decimal?)0.3))
                    {
                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.005;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                    else
                    {
                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.02;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                }
                //渠道营销
                if (info.ProjectSource == "2")
                {
                    if (info.PaymentAmount == null)
                    {

                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.015;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                    else if (info.PaymentAmount < (info.ContractAmount * (decimal?)0.3))
                    {
                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.002;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                    else
                    {
                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.001;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                }
                //营销核算

                //自主营销
                if (info.ProjectSource == "1")
                {
                    if (info.PaymentAmount == null)
                    {
                        info.DepartmentIdAmount = info.ContractAmount * (decimal?)0.3;
                        if (info.DepartmentIdAmount != null)
                        {
                            //info.DepartmentIdAmountName = Math.Round((decimal)info.DepartmentIdAmount * 100) / 100;
                            info.DepartmentIdAmountName = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                        }
                    }
                    else if (info.PaymentAmount < (info.ContractAmount * (decimal?)0.3))
                    {
                        info.DepartmentIdAmount = (info.ContractAmount * (decimal?)0.3) - info.PaymentAmount;
                        if (info.DepartmentIdAmount != null)
                        {
                            info.DepartmentIdAmountName = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                        }
                    }
                    else
                    {
                        info.DepartmentIdAmount = info.ContractAmount * (decimal?)0.03;
                        if (info.DepartmentIdAmount != null)
                        {
                            info.DepartmentIdAmountName = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                        }
                    }
                }
                //生产核算
                if (info.ProjectSource == "1" || info.ProjectSource == "2")
                {
                    if (info.TaskStatus == "3" || info.TaskStatus == "4" || info.TaskStatus == "9" || info.TaskStatus == "5")
                    {
                        if (info.PaymentAmount != null)
                        {
                            if (info.PaymentAmount >= (info.ContractAmount * (decimal?)0.3))
                            {
                                if (info.SubAmount != null && info.MainAmount == null)
                                {
                                    info.DepartmentIdAmount = (info.ContractAmount - info.PaymentAmount - info.SubAmount) * (decimal?)0.3;
                                    if (info.DepartmentIdAmount != null)
                                    {
                                        info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                    }
                                }
                                else if (info.SubAmount == null && info.MainAmount != null)
                                {
                                    info.DepartmentIdAmount = (info.ContractAmount - info.PaymentAmount - info.MainAmount) * (decimal?)0.3;
                                    if (info.DepartmentIdAmount != null)
                                    {
                                        info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                    }
                                }
                                else
                                {
                                    info.DepartmentIdAmount = (info.ContractAmount - info.PaymentAmount) * (decimal?)0.3;
                                    if (info.DepartmentIdAmount != null)
                                    {
                                        info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                    }
                                }
                            }
                            else if (info.PaymentAmount < (info.ContractAmount * (decimal?)0.3))
                            {
                                info.DepartmentIdAmount = info.ContractAmount * (decimal?)0.2;
                                if (info.DepartmentIdAmount != null)
                                {
                                    info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                }
                                //item.DepartmentIdAmount = decimal.Round(decimal.Parse((item.ContractAmount * (decimal?)0.2).ToString()), 2);
                            }
                        }
                        else
                        {
                            if (info.ContractAmount != null)
                            {
                                info.DepartmentIdAmount = (info.ContractAmount * (decimal?)0.2);
                                if (info.DepartmentIdAmount != null)
                                {
                                    info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                }
                            }
                        }


                    }

                }
                //归档情况
                if (info.ReceivedFlag.ToInt() != 0)
                {
                    info.ReceivedFlagName = "已归档";
                }
                else
                {
                    info.ReceivedFlagName = "未归档";
                }

                list.Add(info);
            }

            list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);
        }
        //结算台账合计
        public Response GetSettleAccountsSUM2(dynamic _)
        {

            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var dataItem = reportFormsBLL.GetSettleAccountsSum_new(parameter.queryJson);
            decimal? ContractAmountSum = 0;
            // decimal? BillingAmountSum = 0;
            decimal? AmountSum = 0;
            decimal? NotReceivedSum = 0;
            // decimal? PaymentAmountSum = 0;
            decimal? OwnSum = 0;
            decimal? DitchSum = 0;
            decimal? ConsociationSum = 0;
            // decimal? QKSum = 0;
            //decimal? Sum = 0;
            foreach (var item in dataItem)
            {
                ContractAmountSum = ContractAmountSum + item.ContractAmount;
                AmountSum = AmountSum + item.Amount;
                NotReceivedSum = NotReceivedSum + item.NotReceived;
                //PaymentAmountSum = PaymentAmountSum + item.PaymentAmount;
                // BillingAmountSum = BillingAmountSum + item.BillingAmount;

                if (item.ProjectSource == 1.ToString() && item.ContractAmount.HasValue)
                {
                    OwnSum = OwnSum + item.ContractAmount;
                }
                if (item.ProjectSource == 2.ToString() && item.ContractAmount.HasValue)
                {
                    DitchSum = DitchSum + item.ContractAmount;
                }
                if (item.ProjectSource == 3.ToString() && item.ContractAmount.HasValue)
                {
                    ConsociationSum = ConsociationSum + item.ContractAmount;
                }
            }
            var result = new
            {
                ContractAmountSum = ContractAmountSum,
                AmountSum = AmountSum,
                NotReceivedSum = NotReceivedSum,
                OwnSum = OwnSum,
                DitchSum = DitchSum,
                ConsociationSum = ConsociationSum,
                // QKSum = QKSum
            };
            var jsonData = new
            {
                rows = result
            };
            return Success(jsonData);
        }

        //结算台账
        public Response GetSettleAccountsListHZ(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = reportFormsBLL.GetSettleAccountsHZ(parameter.pagination, parameter.queryJson);
            List<SettleAccountsEntity> list = new List<SettleAccountsEntity>();
            foreach (var info in data)
            {
                //创建时间
                if (info.CreateTime != null)
                {
                    DateTime time = (DateTime)info.CreateTime;
                    info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                }
                //审核时间               
                if (info.ApproverTime != null)
                {
                    DateTime time1 = (DateTime)info.ApproverTime;
                    info.ApproverTimeMd = time1.ToString("yyyy-MM-dd");
                }
                //到款日期               
                if (info.ReceiptDate != null)
                {
                    DateTime time1 = (DateTime)info.ReceiptDate;
                    info.ReceiptDateMd = time1.ToString("yyyy-MM-dd");
                }

                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                if (followPerson != null)
                {
                    info.FollowPersonName = followPerson.F_RealName;

                }
                //项目负责人
                var projectResponsible = userIBLL.GetEntityByUserId(info.ProjectResponsible);
                if (projectResponsible != null)
                {
                    info.ProjectResponsibleName = projectResponsible.F_RealName;
                }
                //部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }
                //实施部门
                var department1 = departmentIBLL.GetEntity(info.J_F_FullName);
                if (department1 != null)
                {
                    info.J_F_FullName = department1.F_FullName;
                }
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }

                //合同主体
                var contractSubject = dataItemBLL.GetDetailItemName(info.ContractSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    info.ContractSubjectName = contractSubject.F_ItemName;
                }
                //报告状态
                var taskStatus = dataItemBLL.GetDetailItemName(info.TaskStatus, "TaskStatus");
                if (taskStatus != null)
                {
                    info.TaskStatusName = taskStatus.F_ItemName;
                }
                //归档情况
                if (info.ReceivedFlag.ToInt() != 0)
                {
                    info.ReceivedFlagName = "已归档";
                }
                else
                {
                    info.ReceivedFlagName = "未归档";
                }

                list.Add(info);
            }

            var jsonData = new
            {

                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records,
            };

            return Success(jsonData);


        }

        //伙伴结算台账导出
        public Response GetSettleAccountsAllHZ(dynamic _)
        {

            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = reportFormsBLL.GetSettleAccountsHZ(parameter.queryJson);
            List<SettleAccountsEntity> list = new List<SettleAccountsEntity>();
            foreach (var info in data)
            {
                //创建时间
                if (info.CreateTime != null)
                {
                    DateTime time = (DateTime)info.CreateTime;
                    info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                }
                //审核时间               
                if (info.ApproverTime != null)
                {
                    DateTime time1 = (DateTime)info.ApproverTime;
                    info.ApproverTimeMd = time1.ToString("yyyy-MM-dd");
                }
                //到款日期               
                if (info.ReceiptDate != null)
                {
                    DateTime time1 = (DateTime)info.ReceiptDate;
                    info.ReceiptDateMd = time1.ToString("yyyy-MM-dd");
                }

                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                if (followPerson != null)
                {
                    info.FollowPersonName = followPerson.F_RealName;

                }
                //项目负责人
                var projectResponsible = userIBLL.GetEntityByUserId(info.ProjectResponsible);
                if (projectResponsible != null)
                {
                    info.ProjectResponsibleName = projectResponsible.F_RealName;
                }
                //部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }
                //实施部门
                var department1 = departmentIBLL.GetEntity(info.J_F_FullName);
                if (department1 != null)
                {
                    info.J_F_FullName = department1.F_FullName;
                }
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }

                //合同主体
                var contractSubject = dataItemBLL.GetDetailItemName(info.ContractSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    info.ContractSubjectName = contractSubject.F_ItemName;
                }
                //报告状态
                var taskStatus = dataItemBLL.GetDetailItemName(info.TaskStatus, "TaskStatus");
                if (taskStatus != null)
                {
                    info.TaskStatusName = taskStatus.F_ItemName;
                }
                //结算金额
                if (info.Proportion != null)
                {
                    info.ProportionAmount = info.ContractAmount * info.Proportion;
                }
                else
                {
                    info.ProportionAmount = 0;
                }
                //核算1
                //自主营销
                if (info.ProjectSource == "1")
                {
                    if (info.PaymentAmount == null)
                    {

                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.02;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                    else if (info.PaymentAmount < (info.ContractAmount * (decimal?)0.3))
                    {
                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.005;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                    else
                    {
                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.02;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                }
                //渠道营销
                if (info.ProjectSource == "2")
                {
                    if (info.PaymentAmount == null)
                    {

                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.015;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                    else if (info.PaymentAmount < (info.ContractAmount * (decimal?)0.3))
                    {
                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.002;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                    else
                    {
                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.001;
                        if (info.FollowPersonAmount != null)
                        {
                            info.FollowPersonAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                        }
                    }
                }
                //营销核算

                //自主营销
                if (info.ProjectSource == "1")
                {
                    if (info.PaymentAmount == null)
                    {
                        info.DepartmentIdAmount = info.ContractAmount * (decimal?)0.3;
                        if (info.DepartmentIdAmount != null)
                        {
                            //info.DepartmentIdAmountName = Math.Round((decimal)info.DepartmentIdAmount * 100) / 100;
                            info.DepartmentIdAmountName = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                        }
                    }
                    else if (info.PaymentAmount < (info.ContractAmount * (decimal?)0.3))
                    {
                        info.DepartmentIdAmount = (info.ContractAmount * (decimal?)0.3) - info.PaymentAmount;
                        if (info.DepartmentIdAmount != null)
                        {
                            info.DepartmentIdAmountName = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                        }
                    }
                    else
                    {
                        info.DepartmentIdAmount = info.ContractAmount * (decimal?)0.03;
                        if (info.DepartmentIdAmount != null)
                        {
                            info.DepartmentIdAmountName = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                        }
                    }
                }
                //生产核算
                if (info.ProjectSource == "1" || info.ProjectSource == "2")
                {
                    if (info.TaskStatus == "3" || info.TaskStatus == "4" || info.TaskStatus == "9" || info.TaskStatus == "5")
                    {
                        if (info.PaymentAmount != null)
                        {
                            if (info.PaymentAmount >= (info.ContractAmount * (decimal?)0.3))
                            {
                                if (info.SubAmount != null && info.MainAmount == null)
                                {
                                    info.DepartmentIdAmount = (info.ContractAmount - info.PaymentAmount - info.SubAmount) * (decimal?)0.3;
                                    if (info.DepartmentIdAmount != null)
                                    {
                                        info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                    }
                                }
                                else if (info.SubAmount == null && info.MainAmount != null)
                                {
                                    info.DepartmentIdAmount = (info.ContractAmount - info.PaymentAmount - info.MainAmount) * (decimal?)0.3;
                                    if (info.DepartmentIdAmount != null)
                                    {
                                        info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                    }
                                }
                                else
                                {
                                    info.DepartmentIdAmount = (info.ContractAmount - info.PaymentAmount) * (decimal?)0.3;
                                    if (info.DepartmentIdAmount != null)
                                    {
                                        info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                    }
                                }
                            }
                            else if (info.PaymentAmount < (info.ContractAmount * (decimal?)0.3))
                            {
                                info.DepartmentIdAmount = info.ContractAmount * (decimal?)0.2;
                                if (info.DepartmentIdAmount != null)
                                {
                                    info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                }
                                //item.DepartmentIdAmount = decimal.Round(decimal.Parse((item.ContractAmount * (decimal?)0.2).ToString()), 2);
                            }
                        }
                        else
                        {
                            if (info.ContractAmount != null)
                            {
                                info.DepartmentIdAmount = (info.ContractAmount * (decimal?)0.2);
                                if (info.DepartmentIdAmount != null)
                                {
                                    info.DepartmentIdAmountName1 = Math.Round((double)info.DepartmentIdAmount, 2).ToString();
                                }
                            }
                        }


                    }

                }
                //归档情况
                if (info.ReceivedFlag.ToInt() != 0)
                {
                    info.ReceivedFlagName = "已归档";
                }
                else
                {
                    info.ReceivedFlagName = "未归档";
                }

                list.Add(info);
            }
            list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);
        }
        //伙伴结算台账合计
        public Response GetSettleAccountsSUMHZ(dynamic _)
        {

            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var dataItem = reportFormsBLL.GetSettleAccountsSumHZ(parameter.queryJson);
            decimal? ContractAmountSum = 0;
            // decimal? BillingAmountSum = 0;
            decimal? AmountSum = 0;
            decimal? NotReceivedSum = 0;
            // decimal? PaymentAmountSum = 0;item.ProjectSource == 3
            decimal? OwnSum = 0;
            decimal? DitchSum = 0;
            decimal? ConsociationSum = 0;
            // decimal? QKSum = 0;
            //decimal? Sum = 0;
            foreach (var item in dataItem)
            {
                ContractAmountSum = ContractAmountSum + item.ContractAmount;
                AmountSum = AmountSum + item.Amount;
                NotReceivedSum = NotReceivedSum + item.NotReceived;
                //PaymentAmountSum = PaymentAmountSum + item.PaymentAmount;
                // BillingAmountSum = BillingAmountSum + item.BillingAmount;

                if (item.ProjectSource == 1.ToString() && item.ContractAmount.HasValue)
                {
                    OwnSum = OwnSum + item.ContractAmount;
                }
                if (item.ProjectSource == 2.ToString() && item.ContractAmount.HasValue)
                {
                    DitchSum = DitchSum + item.ContractAmount;
                }
                if (item.ProjectSource == 3.ToString() && item.ContractAmount.HasValue)
                {
                    ConsociationSum = ConsociationSum + item.ContractAmount;
                }
            }
            var result = new
            {
                ContractAmountSum = ContractAmountSum,
                AmountSum = AmountSum,
                NotReceivedSum = NotReceivedSum,
                OwnSum = OwnSum,
                DitchSum = DitchSum,
                ConsociationSum = ConsociationSum,
                // QKSum = QKSum
            };
            var jsonData = new
            {
                rows = result
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 伙伴比例添加
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetSettleAccountsbili(dynamic _)
        {

            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            string jsonText = string.Empty;

            HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空
            StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
            jsonText = sr.ReadToEnd();

            //ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            ProjectContractEntity entity = jsonText.ToObject<ProjectContractEntity>();

            projectContractIBLL.SaveFormReportForms(entity.ProjectId, entity);
            return Success("保存成功！");
        }
        #endregion





        #region 二期任务单接口
        public Response GetTaskPageToBeDetectList(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            string categoryId = this.GetReqData();
            IEnumerable<ProjectTaskVo> list = new List<ProjectTaskVo>();
            switch (categoryId)
            {
                //待检测
                case "1":
                    list = projectTaskIBLL.GetPageToBeDetectList(parameter.pagination, parameter.queryJson);
                    break;
                //待报告
                case "2":
                    list = projectTaskIBLL.GetPageToBeReportedList(parameter.pagination, parameter.queryJson);
                    break;
                //已完成
                case "3":
                    list = projectTaskIBLL.GetPageHaveCompletedList(parameter.pagination, parameter.queryJson);
                    break;
                //超期项目
                case "4":
                    list = projectTaskIBLL.GetPageOverdueItemList(parameter.pagination, parameter.queryJson);
                    break;
            }
            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };
            return Success(jsonData);
        }


        #endregion

        #region 统计图
        //②公司全年合同额承揽、承揽金额、开票金额
        public Response GetMoneyToBeCollected11(dynamic _)
        {
            List<YearRoundAmountVo> list = new List<YearRoundAmountVo>();
            var data1 = projectContractIBLL.GetyearRoundAmountOfContract();

            List<string> yearList = new List<string>();
            foreach (var info1 in data1)
            {
                YearRoundAmountVo tionVo = new YearRoundAmountVo();

                tionVo.Years = info1.Years;
                //合同承揽
                tionVo.PromiseQuantity = info1.PromiseQuantity;
                //承揽金额
                tionVo.PromiseAmount = info1.PromiseAmount;
                //开票金额
                tionVo.BillingAmount = info1.BillingAmount;
                if (tionVo.Years != null)
                {
                    yearList.Add(tionVo.Years);
                    list.Add(tionVo);
                }
            }
            var response = new
            {
                titles = yearList,
                rows = list
            };
            var jsonData = new
            {
                rows = response
            };
            return Success(jsonData);
        }
        //①公司全年项目数量、已实施数量、待实施数量/金额
        public Response GetMoneyToBeCollected1(dynamic _)
        {
            List<YearRoundVo> list = new List<YearRoundVo>();
            //公司全年
            var data1 = projectContractIBLL.GetyearRoundNumberOfTtems();
            //已实施数量
            var data2 = projectContractIBLL.GetyearRoundHaveBeenImplemented();
            //待实施数量/对应合同金额
            var data3 = projectContractIBLL.GetyearRoundToBeImplemented();
            //DateTime StartTime = DateTime.Now;
            //int data = StartTime.Year;
            List<string> yearList = data1.ToList().Select(i => i.Years).Distinct().ToList();

            for (int i = 0; i < yearList.Count; i++)
            {
                string data = yearList[i];
                YearRoundVo tionVo = new YearRoundVo();

                foreach (var info1 in data1)
                {

                    if (info1.Years == data)
                    {
                        tionVo.Years = info1.Years;
                        tionVo.ProjectQuantity = info1.ProjectQuantity;
                    }
                }
                foreach (var info1 in data2)
                {

                    if (info1.Years == data)
                    {
                        tionVo.Years = info1.Years;
                        tionVo.HasQuantity = info1.HasQuantity;
                    }
                }
                foreach (var info1 in data3)
                {

                    if (info1.Years == data)
                    {
                        tionVo.Years = info1.Years;
                        tionVo.NotQuantity = info1.NotQuantity;
                        tionVo.NotAmount = info1.NotAmount;
                    }
                }
                if (tionVo.Years != null)
                {
                    list.Add(tionVo);
                }
            }
            var response = new
            {
                titles = yearList,
                rows = list
            };
            var jsonData = new
            {
                rows = response
            };
            return Success(jsonData);
        }

        public Response GetIndexData(dynamic _)
        {
            var userInfo = LoginUserInfo.Get();
            var roles = userInfo.roleIds.Split(new string[] { "," }, StringSplitOptions.None);
            var all_right = roles.Where(i => i == "df168dd3-1e2f-49be-ba1d-e421012bb0d4").ToList();
            UserEntity userEntity = userIBLL.GetEntityByUserId(userInfo.userId);
            //管理员 所有权限看到所有公司的
            //质量技术部看所有的
            if (all_right.Count > 0 || userInfo.departmentId == "4abfdc80-b91f-48ed-8aa7-bbcd7948ac84")
            {
                List<YearRoundVo> list = new List<YearRoundVo>();
                //公司全年项目数量
                var data1_Mearly = projectContractIBLL.GetyearRoundNumberOfTtems();
                //公司当年按月项目数量
                var data_Monthly = projectContractIBLL.GetMonthlyRoundNumberOfTtems();
                //公司项目总金额 年度
                var data2_Yearly = projectContractIBLL.GetyearRoundAmountOfContract();
                //公司当年按月项目总金额
                var data2_monthly = projectContractIBLL.GetmonthlyRoundAmountOfContract();

                //DateTime StartTime = DateTime.Now;
                //int data = StartTime.Year;
                List<string> yearList = data1_Mearly.ToList().Select(i => i.Years).ToList();
                List<string> yearData = data1_Mearly.ToList().Select(i => i.ProjectQuantity).ToList();
                List<string> monthList = data_Monthly.ToList().Select(i => i.Years).ToList();
                List<string> monthData = data_Monthly.ToList().Select(i => i.ProjectQuantity).ToList();
                List<string> yearList2 = data2_Yearly.ToList().Select(i => i.Years).ToList();
                List<string> yearData2 = data2_Yearly.ToList().Select(i => i.PromiseAmount).ToList();
                List<string> monthList2 = data2_monthly.ToList().Select(i => i.Years).ToList();
                List<string> monthData2 = data2_monthly.ToList().Select(i => i.PromiseAmount).ToList();
                var jsonData = new
                {
                    code = 100,
                    yearList = yearList,
                    monthList = monthList,
                    yearList2 = yearList2,
                    monthList2 = monthList2,
                    yearData = yearData,
                    monthData = monthData,
                    yearData2 = yearData2,
                    monthData2 = monthData2
                };
                return Success(jsonData);
            }
            //非合作商看到自己部门下面的 包括多部门的
            else if (userEntity.F_HZ != 1)
            {
                List<YearRoundVo> list = new List<YearRoundVo>();
                var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
                List<string> deptIds = new List<string>();
                if (!string.IsNullOrEmpty(followPerson1.F_MoreDepartmentId))
                {
                    deptIds = followPerson1.F_MoreDepartmentId.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                }
                else
                {
                    deptIds.Add(userInfo.departmentId);
                }
                //var test = deptIds.Where(i => i == userInfo.departmentId).ToList();
                //公司全年项目数量
                var data1_Mearly = projectContractIBLL.GetyearRoundNumberOfTtemsByDeptids(deptIds);
                //公司当年按月项目数量
                var data_Monthly = projectContractIBLL.GetMonthlyRoundNumberOfTtemsByDeptids(deptIds);
                //公司项目总金额 年度
                var data2_Yearly = projectContractIBLL.GetyearRoundAmountOfContractByDeptids(deptIds);
                //公司当年按月项目总金额
                var data2_monthly = projectContractIBLL.GetmonthlyRoundAmountOfContractByDeptids(deptIds);

                //DateTime StartTime = DateTime.Now;
                //int data = StartTime.Year;
                List<string> yearList = data1_Mearly.ToList().Select(i => i.Years).ToList();
                List<string> yearData = data1_Mearly.ToList().Select(i => i.ProjectQuantity).ToList();
                List<string> monthList = data_Monthly.ToList().Select(i => i.Years).ToList();
                List<string> monthData = data_Monthly.ToList().Select(i => i.ProjectQuantity).ToList();
                List<string> yearList2 = data2_Yearly.ToList().Select(i => i.Years).ToList();
                List<string> yearData2 = data2_Yearly.ToList().Select(i => i.PromiseAmount).ToList();
                List<string> monthList2 = data2_monthly.ToList().Select(i => i.Years).ToList();
                List<string> monthData2 = data2_monthly.ToList().Select(i => i.PromiseAmount).ToList();
                var jsonData = new
                {
                    code = 100,
                    yearList = yearList,
                    monthList = monthList,
                    yearList2 = yearList2,
                    monthList2 = monthList2,
                    yearData = yearData,
                    monthData = monthData,
                    yearData2 = yearData2,
                    monthData2 = monthData2
                };
                return Success(jsonData);
            }
            //合作商什么都看不到
            else
            {
                var jsonData = new
                {
                    code = -1
                };
                return Success(jsonData);
            }
        }
        public Response GetIndexData2(dynamic _)
        {
            var userInfo = LoginUserInfo.Get();
            var roles = userInfo.roleIds.Split(new string[] { "," }, StringSplitOptions.None);
            var all_right = roles.Where(i => i == "df168dd3-1e2f-49be-ba1d-e421012bb0d4").ToList();
            UserEntity userEntity = userIBLL.GetEntityByUserId(userInfo.userId);
            //管理员 所有权限看到所有公司的
            //质量技术部看所有的
            if (all_right.Count > 0 || userInfo.departmentId == "4abfdc80-b91f-48ed-8aa7-bbcd7948ac84")
            {
                //待检测
                int countToBeDetect = 0;
                var list_1 = projectTaskIBLL.GetIndexToBeDetectList(new List<string>(), out countToBeDetect);
                ////待报告
                int countToBeReported = 0;
                var list_2 = projectTaskIBLL.GetIndexToBeReportedList(new List<string>(), out countToBeReported);
                ////已完成
                int countHaveCompleted = 0;
                var list_3 = projectTaskIBLL.GetIndexHaveCompletedList(new List<string>(), out countHaveCompleted);
                ////超期项目
                int countOverdueItem = 0;
                var list_4 = projectTaskIBLL.GetIndexOverdueList(new List<string>(), out countOverdueItem);

                List<string> listToBeDetect = list_1.ToList().Select(i => i.ProjectName).ToList();
                List<string> listToBeReported = list_2.ToList().Select(i => i.ProjectName).ToList();
                List<string> listHaveCompleted = list_3.ToList().Select(i => i.ProjectName).ToList();
                List<string> listOverdueItem = list_4.ToList().Select(i => i.ProjectName).ToList();
                var jsonData = new
                {
                    code = 100,
                    countToBeDetect = countToBeDetect,
                    countToBeReported = countToBeReported,
                    countHaveCompleted = countHaveCompleted,
                    countOverdueItem = countOverdueItem,
                    listToBeDetect = listToBeDetect,
                    listToBeReported = listToBeReported,
                    listHaveCompleted = listHaveCompleted,
                    listOverdueItem = listOverdueItem
                };
                return Success(jsonData);
            }
            //非合作商看到自己部门下面的 包括多部门的
            else if (userEntity.F_HZ != 1)
            {
                List<YearRoundVo> list = new List<YearRoundVo>();
                var followPerson1 = userIBLL.GetEntityByUserId(userInfo.userId);
                List<string> deptIds = new List<string>();
                if (!string.IsNullOrEmpty(followPerson1.F_MoreDepartmentId))
                {
                    deptIds = followPerson1.F_MoreDepartmentId.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                }
                else
                {
                    deptIds.Add(userInfo.departmentId);
                }
                //待检测
                int countToBeDetect = 0;
                var list_1 = projectTaskIBLL.GetIndexToBeDetectList(deptIds, out countToBeDetect);
                ////待报告
                int countToBeReported = 0;
                var list_2 = projectTaskIBLL.GetIndexToBeReportedList(deptIds, out countToBeReported);
                ////已完成
                int countHaveCompleted = 0;
                var list_3 = projectTaskIBLL.GetIndexHaveCompletedList(deptIds, out countHaveCompleted);
                ////超期项目
                int countOverdueItem = 0;
                var list_4 = projectTaskIBLL.GetIndexOverdueList(deptIds, out countOverdueItem);

                List<string> listToBeDetect = list_1.ToList().Select(i => i.ProjectName).ToList();
                List<string> listToBeReported = list_2.ToList().Select(i => i.ProjectName).ToList();
                List<string> listHaveCompleted = list_3.ToList().Select(i => i.ProjectName).ToList();
                List<string> listOverdueItem = list_4.ToList().Select(i => i.ProjectName).ToList();
                var jsonData = new
                {
                    code = 100,
                    countToBeDetect = countToBeDetect,
                    countToBeReported = countToBeReported,
                    countHaveCompleted = countHaveCompleted,
                    countOverdueItem = countOverdueItem,
                    listToBeDetect = listToBeDetect,
                    listToBeReported = listToBeReported,
                    listHaveCompleted = listHaveCompleted,
                    listOverdueItem = listOverdueItem
                };
                return Success(jsonData);
            }
            //合作商什么都看不到
            else
            {
                var jsonData = new
                {
                    code = -1
                };
                return Success(jsonData);
            }
        }
        //生产超时报告/金额
        public Response GetProducTionTimeoutList(dynamic _)
        {
            List<ProducTionVo> list = new List<ProducTionVo>();
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            List<string> yearList = new List<string>();
            if (followPerson.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += "  ( t.DepartmentId='" + strList[i] + "' or pt.TaskDepartmentId like '%" + strList[i] + "' or pt.SubDepartmentId='" + strList[i] + "' or pt.MainDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or pt.TaskDepartmentId like '%" + strList[i] + "' or pt.SubDepartmentId='" + strList[i] + "' or pt.MainDepartmentId='" + strList[i] + "') ";
                    }

                }
                var data = projectTaskIBLL.GetProducTionTimeoutListDepartmentId(deps);
                for (int i = 0; i < 12; i++)
                {
                    ProducTionVo tionVo = new ProducTionVo();
                    var department = departmentIBLL.GetEntity(LoginUserInfo.Get().departmentId);

                    if (department != null)
                    {
                        tionVo.DepartmentId = department.F_FullName;
                    }
                    foreach (var info in data)
                    {
                        if (info.Month.ToInt() == i)
                        {
                            tionVo.Years = info.Years;
                            tionVo.Month = i.ToString();
                            tionVo.Quantity = info.Quantity;
                            tionVo.Amount = info.Amount;
                        }
                    }
                    if (tionVo.Month != null)
                    {
                        list.Add(tionVo);
                    }
                }
            }
            else
            {
                var data = projectTaskIBLL.GetProducTionTimeoutList(followPerson.F_DepartmentId);
                for (int i = 0; i < 12; i++)
                {
                    ProducTionVo tionVo = new ProducTionVo();
                    var department = departmentIBLL.GetEntity(LoginUserInfo.Get().departmentId);

                    if (department != null)
                    {
                        tionVo.DepartmentId = department.F_FullName;
                    }


                    foreach (var info in data)
                    {
                        if (info.Month.ToInt() == i)
                        {
                            tionVo.Years = info.Years;
                            tionVo.Month = i.ToString();
                            tionVo.Quantity = info.Quantity;
                            tionVo.Amount = info.Amount;
                        }

                    }
                    if (tionVo.Month != null)
                    {
                        list.Add(tionVo);
                    }
                }
            }
            return Success(list);
        }
        //生产统计图
        public Response GetProducTionList(dynamic _)
        {
            List<ProducTionVo> list = new List<ProducTionVo>();
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            if (followPerson.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "' or pt.TaskDepartmentId like '%" + strList[i] + "' or pt.SubDepartmentId='" + strList[i] + "' or pt.MainDepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "' or pt.TaskDepartmentId like '%" + strList[i] + "' or pt.SubDepartmentId='" + strList[i] + "' or pt.MainDepartmentId='" + strList[i] + "') ";
                    }
                }
                var data = projectTaskIBLL.GetProducTionListDepartmentId(deps);
                for (int i = 0; i < 12; i++)
                {
                    ProducTionVo tionVo = new ProducTionVo();
                    var department = departmentIBLL.GetEntity(LoginUserInfo.Get().departmentId);

                    if (department != null)
                    {
                        tionVo.DepartmentId = department.F_FullName;
                    }
                    foreach (var info in data)
                    {
                        if (info.Month.ToInt() == i)
                        {
                            tionVo.Years = info.Years;
                            tionVo.Month = i.ToString();
                            tionVo.Quantity = info.Quantity;
                            tionVo.Amount = info.Amount;
                        }
                    }
                    if (tionVo.Month != null)
                    {
                        list.Add(tionVo);
                    }
                }
            }
            else
            {
                var data = projectTaskIBLL.GetProducTionList(followPerson.F_DepartmentId);
                for (int i = 0; i < 12; i++)
                {
                    ProducTionVo tionVo = new ProducTionVo();
                    var department = departmentIBLL.GetEntity(LoginUserInfo.Get().departmentId);

                    if (department != null)
                    {
                        tionVo.DepartmentId = department.F_FullName;
                    }
                    foreach (var info in data)
                    {
                        if (info.Month.ToInt() == i)
                        {
                            tionVo.Years = info.Years;
                            tionVo.Month = i.ToString();
                            tionVo.Quantity = info.Quantity;
                            tionVo.Amount = info.Amount;
                        }
                    }
                    if (tionVo.Month != null)
                    {
                        list.Add(tionVo);
                    }
                }
            }
            return Success(list);
        }
        public Response GetMarkeTingList(dynamic _)
        {
            List<ProducTionVo> list = new List<ProducTionVo>();
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            if (followPerson.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( DepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( DepartmentId='" + strList[i] + "') ";
                    }
                }
                var data = projectContractIBLL.GetMarkeTingListDepartmentId(deps);
                for (int i = 0; i < 12; i++)
                {
                    ProducTionVo tionVo = new ProducTionVo();
                    var department = departmentIBLL.GetEntity(LoginUserInfo.Get().departmentId);

                    if (department != null)
                    {
                        tionVo.DepartmentId = department.F_FullName;
                    }

                    foreach (var info in data)
                    {
                        if (info.Month.ToInt() == i)
                        {
                            tionVo.Years = info.Years;
                            tionVo.Month = i.ToString();
                            tionVo.Quantity = info.Quantity;
                            tionVo.Amount = info.Amount;
                        }

                    }
                    if (tionVo.Month != null)
                    {
                        list.Add(tionVo);
                    }
                }
            }
            else
            {
                var data = projectContractIBLL.GetMarkeTingList(followPerson.F_DepartmentId);
                for (int i = 0; i < 12; i++)
                {
                    ProducTionVo tionVo = new ProducTionVo();
                    var department = departmentIBLL.GetEntity(LoginUserInfo.Get().departmentId);

                    if (department != null)
                    {
                        tionVo.DepartmentId = department.F_FullName;
                    }

                    foreach (var info in data)
                    {
                        if (info.Month.ToInt() == i)
                        {
                            tionVo.Years = info.Years;
                            tionVo.Month = i.ToString();
                            tionVo.Quantity = info.Quantity;
                            tionVo.Amount = info.Amount;
                        }
                    }
                    if (tionVo.Month != null)
                    {
                        list.Add(tionVo);
                    }
                }
            }
            return Success(list);
        }
        //营造待回款
        public Response GetMoneyToBeCollected(dynamic _)
        {
            List<ProducTionVo> list = new List<ProducTionVo>();
            var user = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId(user);
            if (followPerson.F_MoreDepartmentId != null)
            {
                string[] strList = followPerson.F_MoreDepartmentId.Split(',');
                string deps = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    if (i == 0)
                    {
                        deps += " ( t.DepartmentId='" + strList[i] + "') ";
                    }
                    else
                    {
                        deps += " or ( t.DepartmentId='" + strList[i] + "') ";
                    }
                }
                var data = projectContractIBLL.GetMoneyToBeCollectedDepartmentId(deps);
                for (int i = 0; i < 12; i++)
                {
                    ProducTionVo tionVo = new ProducTionVo();
                    var department = departmentIBLL.GetEntity(LoginUserInfo.Get().departmentId);

                    if (department != null)
                    {
                        tionVo.DepartmentId = department.F_FullName;
                    }

                    foreach (var info in data)
                    {
                        if (info.Month.ToInt() == i)
                        {
                            tionVo.Years = info.Years;
                            tionVo.Month = i.ToString();
                            tionVo.Quantity = info.Quantity;
                            tionVo.Amount = info.Amount;
                        }
                    }
                    if (tionVo.Month != null)
                    {
                        list.Add(tionVo);
                    }
                }
            }
            else
            {
                var data = projectContractIBLL.GetMoneyToBeCollected(followPerson.F_DepartmentId);
                for (int i = 0; i < 12; i++)
                {
                    ProducTionVo tionVo = new ProducTionVo();
                    var department = departmentIBLL.GetEntity(LoginUserInfo.Get().departmentId);

                    if (department != null)
                    {
                        tionVo.DepartmentId = department.F_FullName;
                    }

                    foreach (var info in data)
                    {
                        if (info.Month.ToInt() == i)
                        {
                            tionVo.Years = info.Years;
                            tionVo.Month = i.ToString();
                            tionVo.Quantity = info.Quantity;
                            tionVo.Amount = info.Amount;
                        }
                    }
                    if (tionVo.Month != null)
                    {
                        list.Add(tionVo);
                    }
                }
            }
            return Success(list);
        }
        //质量技术部统计图
        public Response GetQualityTechnology(dynamic _)
        {
            string categoryId = this.GetReqData();
            List<ProducTionVo> list = new List<ProducTionVo>();
            switch (categoryId)
            {
                case "1":
                    //1.全公司项目待实施数量/待实施金额
                    var date1 = projectTaskIBLL.GetQualityTechnologyImplement(categoryId);
                    for (int i = 0; i < 12; i++)
                    {
                        ProducTionVo tionVo = new ProducTionVo();
                        var department = departmentIBLL.GetEntity(LoginUserInfo.Get().departmentId);

                        if (department != null)
                        {
                            tionVo.DepartmentId = department.F_FullName;
                        }
                        foreach (var info in date1)
                        {
                            if (info.Month.ToInt() == i)
                            {
                                tionVo.Years = info.Years;
                                tionVo.Month = i.ToString();
                                tionVo.Quantity = info.Quantity;
                                tionVo.Amount = info.Amount;
                            }
                        }
                        if (tionVo.Month != null)
                        {
                            list.Add(tionVo);
                        }
                    }
                    break;
                case "2":
                    //2.全公司项目实施数量/实施金额
                    var date2 = projectTaskIBLL.GetQualityTechnologyImplement(categoryId);
                    for (int i = 0; i < 12; i++)
                    {
                        ProducTionVo tionVo = new ProducTionVo();
                        var department = departmentIBLL.GetEntity(LoginUserInfo.Get().departmentId);

                        if (department != null)
                        {
                            tionVo.DepartmentId = department.F_FullName;
                        }

                        foreach (var info in date2)
                        {
                            if (info.Month.ToInt() == i)
                            {
                                tionVo.Years = info.Years;
                                tionVo.Month = i.ToString();
                                tionVo.Quantity = info.Quantity;
                                tionVo.Amount = info.Amount;
                            }

                        }
                        if (tionVo.Month != null)
                        {
                            list.Add(tionVo);
                        }
                    }
                    break;
                case "3":
                    //3.全公司项目超期数量/超期金额
                    var date3 = projectTaskIBLL.GetQualityTechnologyImplement(categoryId);
                    for (int i = 0; i < 12; i++)
                    {
                        ProducTionVo tionVo = new ProducTionVo();
                        var department = departmentIBLL.GetEntity(LoginUserInfo.Get().departmentId);

                        if (department != null)
                        {
                            tionVo.DepartmentId = department.F_FullName;
                        }

                        foreach (var info in date3)
                        {
                            if (info.Month.ToInt() == i)
                            {
                                tionVo.Years = info.Years;
                                tionVo.Month = i.ToString();
                                tionVo.Quantity = info.Quantity;
                                tionVo.Amount = info.Amount;
                            }
                        }
                        if (tionVo.Month != null)
                        {
                            list.Add(tionVo);
                        }
                    }
                    break;
            }
            return Success(list);
        }
        #endregion
        #region 版本更新
        public Response GetVersionList(dynamic _)
        {
            var data = versionBLL.GetPageList();


            var jsonData = new
            {
                VersionName = data.FirstOrDefault().VersionName,
                Versionnumber = data.FirstOrDefault().Versionnumber,
            };
            return Success(jsonData);
        }
        #endregion
        #region 报表
        //营销报表
        public Response GetMarketings(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = reportFormsBLL.GetMarketings(parameter.pagination, parameter.queryJson);
            List<MarketingEntity> marketings = new List<MarketingEntity>();
            List<MarketingEntity> listdata = new List<MarketingEntity>();
            //foreach (var item in data)
            //{
            //    if (!marketings.Contains(item))
            //    {
            //        if (!listdata.Contains(item))
            //        {
            //            var bol = true;
            //            foreach (var list in marketings)
            //            {
            //                if (list.ContractNo == item.ContractNo && list.ProjectName == item.ProjectName && list.PC == item.PC && list.Tid != item.Tid)
            //                {
            //                    list.ContractAmount += item.ContractAmount;
            //                    list.NotReceived = list.ContractAmount - list.Amount;
            //                    bol = false;
            //                }
            //                else if (list.ContractNo == item.ContractNo && list.PC != item.PC && list.Tid == item.Tid)
            //                {
            //                    list.Amount += item.Amount;
            //                    list.NotReceived = list.ContractAmount - list.Amount;
            //                    bol = false;
            //                }
            //                else if (list.ContractNo == item.ContractNo && list.PC != item.PC)
            //                {
            //                    bol = false;
            //                }
            //            }
            //            if (bol) marketings.Add(item);
            //            listdata.Add(item);
            //        }
            //    }
            //}
            var jsonData = new
            {
                rows = data,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };
            return Success(jsonData);
        }
        //营销报表合计
        public Response GetMarketingsSum(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var dataItem = reportFormsBLL.GetMarketings(parameter.queryJson);
            List<MarketingEntity> marketingsItem = new List<MarketingEntity>();
            List<MarketingEntity> listdata = new List<MarketingEntity>();
            foreach (var item in dataItem)
            {
                if (!marketingsItem.Contains(item))
                {
                    if (!listdata.Contains(item))
                    {
                        var bol = true;
                        foreach (var list in marketingsItem)
                        {
                            if (list.ContractNo == item.ContractNo && list.ProjectName == item.ProjectName && list.PC == item.PC && list.Tid != item.Tid)
                            {
                                list.ContractAmount += item.ContractAmount;
                                list.NotReceived = list.ContractAmount - list.Amount;
                                bol = false;
                            }
                            else if (list.ContractNo == item.ContractNo && list.PC != item.PC && list.Tid == item.Tid)
                            {
                                list.Amount += item.Amount;
                                list.NotReceived = list.ContractAmount - list.Amount;
                                bol = false;
                            }
                            else if (list.ContractNo == item.ContractNo && list.PC != item.PC)
                            {
                                bol = false;
                            }

                        }
                        if (bol) marketingsItem.Add(item);
                        listdata.Add(item);
                    }
                }
            }
            decimal? ContractAmountSum = 0;
            decimal? AmountSum = 0;
            decimal? NotReceivedSum = 0;
            decimal? OwnSum = 0;
            decimal? DitchSum = 0;
            decimal? ConsociationSum = 0;
            foreach (var item in marketingsItem)
            {
                ContractAmountSum = ContractAmountSum + item.ContractAmount;
                AmountSum = AmountSum + item.Amount;
                NotReceivedSum = NotReceivedSum + item.NotReceived;
                if (item.ProjectSource == 1.ToString() && item.ContractAmount.HasValue)
                {
                    OwnSum = OwnSum + item.ContractAmount;
                }
                if (item.ProjectSource == 2.ToString() && item.ContractAmount.HasValue)
                {
                    DitchSum = DitchSum + item.ContractAmount;
                }
                if (item.ProjectSource == 3.ToString() && item.ContractAmount.HasValue)
                {
                    ConsociationSum = ConsociationSum + item.ContractAmount;
                }
            };
            var jsonData = new
            {
                //data = marketingsItem,
                ContractAmountSum = ContractAmountSum,
                AmountSum = AmountSum,
                NotReceivedSum = NotReceivedSum,
                OwnSum = OwnSum,
                DitchSum = DitchSum,
                ConsociationSum = ConsociationSum
            };
            return Success(jsonData);
        }
        //生产报表
        public Response GetProductions(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = reportFormsBLL.GetProductions(parameter.pagination, parameter.queryJson);
            List<ProductionEntity> marketings = new List<ProductionEntity>();
            foreach (var item in data)
            {
                if (!marketings.Contains(item))
                {
                    marketings.Add(item);
                }
            }
            var jsonData = new
            {
                rows = marketings,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };
            return Success(jsonData);
        }
        //生产报表计算
        public Response GetProductionsSum(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var dataItem = reportFormsBLL.GetProductions(parameter.queryJson);
            List<ProductionEntity> marketingsItem = new List<ProductionEntity>();
            foreach (var item in dataItem)
            {
                if (!marketingsItem.Contains(item))
                {
                    /*var bol = true;
                    foreach (var list in marketingsItem)
                    {
                        if (list.ContractNo == item.ContractNo && list.ProjectName == item.ProjectName)
                        {
                            list.Amount += item.Amount;
                            list.NotReceived -= item.Amount;
                            bol = false;
                        }

                    }
                    if (bol) marketingsItem.Add(item);*/
                    marketingsItem.Add(item);
                }
            }
            var ContractAmountSum = 0;
            var OwnSum = 0;
            var DitchSum = 0;
            var ConsociationSum = 0;
            foreach (var item in marketingsItem)
            {
                ContractAmountSum = ContractAmountSum + (int)item.ContractAmount;
                if (item.ProjectSource == 1.ToString() && item.ContractAmount.HasValue)
                {
                    OwnSum = OwnSum + (int)item.ContractAmount;
                }
                if (item.ProjectSource == 2.ToString() && item.ContractAmount.HasValue)
                {
                    DitchSum = DitchSum + (int)item.ContractAmount;
                }
                if (item.ProjectSource == 3.ToString() && item.ContractAmount.HasValue)
                {
                    ConsociationSum = ConsociationSum + (int)item.ContractAmount;
                }
            };
            var jsonData = new
            {
                //data = marketingsItem,
                ContractAmountSum = ContractAmountSum,
                OwnSum = OwnSum,
                DitchSum = DitchSum,
                ConsociationSum = ConsociationSum
            };
            return Success(jsonData);
        }
        public Response GetSettleAccounts(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = reportFormsBLL.GetSettleAccounts(parameter.pagination, parameter.queryJson);
            List<SettleAccountsEntity> marketings = new List<SettleAccountsEntity>();
            List<SettleAccountsEntity> listdata = new List<SettleAccountsEntity>();
            foreach (var item in data)
            {
                if (!marketings.Contains(item))
                {
                    if (!listdata.Contains(item))
                    {
                        var bol = true;
                        foreach (var list in marketings)
                        {
                            if (list.ContractNo == item.ContractNo && list.ProjectName == item.ProjectName && list.PC == item.PC && list.Tid != item.Tid)
                            {
                                list.ContractAmount += item.ContractAmount;
                                list.NotReceived = list.ContractAmount - list.Amount;
                                bol = false;
                            }
                            else if (list.ContractNo == item.ContractNo && list.PC != item.PC && list.Tid == item.Tid)
                            {
                                list.Amount += item.Amount;
                                list.NotReceived = list.ContractAmount - list.Amount;
                                bol = false;
                            }
                            else if (list.ContractNo == item.ContractNo && list.PC != item.PC)
                            {
                                bol = false;
                            }

                        }
                        if (bol) marketings.Add(item);
                        listdata.Add(item);
                    }
                }
            }
            var jsonData = new
            {
                rows = marketings,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };
            return Success(jsonData);
        }
        public Response GetSettleAccountsSum(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var dataItem = reportFormsBLL.GetSettleAccounts(parameter.queryJson);
            List<SettleAccountsEntity> marketingsItem = new List<SettleAccountsEntity>();
            List<SettleAccountsEntity> listdata = new List<SettleAccountsEntity>();
            foreach (var item in dataItem)
            {
                if (!marketingsItem.Contains(item))
                {
                    if (!listdata.Contains(item))
                    {
                        var bol = true;
                        foreach (var list in marketingsItem)
                        {
                            if (list.ContractNo == item.ContractNo && list.ProjectName == item.ProjectName && list.PC == item.PC && list.Tid != item.Tid)
                            {
                                list.ContractAmount += item.ContractAmount;
                                list.NotReceived = list.ContractAmount - list.Amount;
                                bol = false;
                            }
                            else if (list.ContractNo == item.ContractNo && list.PC != item.PC && list.Tid == item.Tid)
                            {
                                list.Amount += item.Amount;
                                list.NotReceived = list.ContractAmount - list.Amount;
                                bol = false;
                            }
                            else if (list.ContractNo == item.ContractNo && list.PC != item.PC)
                            {
                                bol = false;
                            }
                        }
                        if (bol) marketingsItem.Add(item);
                        listdata.Add(item);
                    }
                }
            }
            decimal? ContractAmountSum = 0;
            decimal? BillingAmountSum = 0;
            decimal? AmountSum = 0;
            decimal? NotReceivedSum = 0;
            decimal? PaymentAmountSum = 0;
            decimal? OwnSum = 0;
            decimal? DitchSum = 0;
            decimal? ConsociationSum = 0;
            foreach (var item in marketingsItem)
            {
                if (item.ContractAmount.ToString() != "") ContractAmountSum = ContractAmountSum + decimal.Parse(item.ContractAmount.ToString());
                if (item.Amount.ToString() != "") AmountSum = AmountSum + decimal.Parse(item.Amount.ToString());
                if (item.NotReceived.ToString() != "") NotReceivedSum = NotReceivedSum + decimal.Parse(item.NotReceived.ToString());
                if (item.PaymentAmount.ToString() != "") PaymentAmountSum = PaymentAmountSum + decimal.Parse(item.PaymentAmount.ToString());
                if (item.BillingAmount.ToString() != "") BillingAmountSum = BillingAmountSum + decimal.Parse(item.BillingAmount.ToString());
                if (item.ProjectSource == 1.ToString() && item.ContractAmount.HasValue)
                {
                    OwnSum = OwnSum + (int)item.ContractAmount;
                }
                if (item.ProjectSource == 2.ToString() && item.ContractAmount.HasValue)
                {
                    DitchSum = DitchSum + (int)item.ContractAmount;
                }
                if (item.ProjectSource == 3.ToString() && item.ContractAmount.HasValue)
                {
                    ConsociationSum = ConsociationSum + (int)item.ContractAmount;
                }
            };
            var jsonData = new
            {
                //data = marketingsItem,
                ContractAmountSum = ContractAmountSum,
                BillingAmountSum = BillingAmountSum,
                AmountSum = AmountSum,
                NotReceivedSum = NotReceivedSum,
                PaymentAmountSum = PaymentAmountSum,
                OwnSum = OwnSum,
                DitchSum = DitchSum,
                ConsociationSum = ConsociationSum
            };
            return Success(jsonData);
        }

        #endregion

        #region 获取数据

        //获取统计图表
        public Response GetContract(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var projectContractVo = projectContractIBLL.GetContract(parameter.queryJson);
            List<string> DepartmentName = new List<string>();
            List<string> ProjectCount = new List<string>();
            List<string> ContractSum = new List<string>();
            foreach (var item in projectContractVo)
            {
                var department = departmentIBLL.GetEntity(item.DepartmentId);
                if (department != null)
                {
                    item.DepartmentName = department.F_FullName;
                }
                DepartmentName.Add(item.DepartmentName);
                ProjectCount.Add(item.ProjectCount);
                ContractSum.Add(item.ContractSum);
            }
            var jsonData = new
            {
                DepartmentName = DepartmentName,
                ProjectCount = ProjectCount,
                ContractSum = ContractSum
            };
            return Success(jsonData);
        }


        //获取统计图表
        public Response GetProjectMonthCount(dynamic _)
        {
            var projectContractVo = projectIBLL.GetProjectMonthCount();
            return Success(projectContractVo);
        }
        public Response GetProjectCountBySource(dynamic _)
        {
            var projectContractVo = projectIBLL.GetProjectCountBySource();
            return Success(projectContractVo);
        }
        public Response GetProjectCountByProvince(dynamic _)
        {
            var projectContractVo = projectIBLL.GetProjectCountByProvince();
            return Success(projectContractVo);
        }
        public Response GetProjectConversion(dynamic _)
        {
            var projectContractVo = projectIBLL.GetProjectConversion();
            return Success(projectContractVo);
        }

        public Response GetBackProjectRate(dynamic _)
        {
            var projectContractVo = projectIBLL.GetBackProjectRate();
            return Success(projectContractVo);
        }

        /// <summary>
        /// 获取页面显示列表分页数据
        /// <summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetPageList(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = projectIBLL.GetPageListAPI(parameter.pagination, parameter.queryJson);
            List<ProjectVo> list = new List<ProjectVo>();
            foreach (var info in data)
            {
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                //报备人员
                var preparedPerson = userIBLL.GetEntityByUserId(info.PreparedPerson);
                //项目状态
                var projectStatus = dataItemBLL.GetDetailItemName(info.ProjectStatus, "projectStatus");
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }
                if (preparedPerson != null)
                {
                    info.PreparedPersonName = preparedPerson.F_RealName;
                }
                if (followPerson != null)
                {
                    info.FollowPersonName = followPerson.F_RealName;
                }
                if (projectStatus != null)
                {
                    info.ProjectStatusName = projectStatus.F_ItemName;
                }
                list.Add(info);
            }
            var jsonData = new
            {
                rows = data,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };
            return Success(jsonData);
        }


        #region 合同接口
        /// <summary>
        /// 获取合同信息
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetContractPageList(dynamic _)
        {
            var dataJson = this.GetReqData();
            ReqPageParam parameter = JsonConvert.DeserializeObject<ReqPageParam>(dataJson);

            List<ProjectContractVo> list = new List<ProjectContractVo>();
            var data = projectContractIBLL.GetPageList(parameter.pagination, parameter.queryJson);
            foreach (var info in data)
            {
                //if (info.ProjectSource.ToInt() == 3)
                //{
                //    ProjectContractEntity entity = new ProjectContractEntity();


                //    entity.EffectiveAmount = 0;
                //    entity.EffectiveAmountShow = 0;
                //    projectContractIBLL.SaveEntity1(info.id, entity);
                //    info.EffectiveAmount = 0;
                //    info.EffectiveAmountShow = 0;
                //}
                //if (info.ContractStatus.ToInt() == 11)
                //{
                //    ProjectContractEntity entity = new ProjectContractEntity();


                //    entity.EffectiveAmountShow = 0;
                //    projectContractIBLL.SaveEntity1(info.id, entity);
                //}
                //创建时间
                if (info.CreateTime != null)
                {
                    DateTime time = (DateTime)info.CreateTime;
                    info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                }
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }
                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                if (followPerson != null)
                {
                    info.FollowPersonName = followPerson.F_RealName;
                }
                //营销部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }
                //合同主体
                var contractSubject = dataItemBLL.GetDetailItemName(info.ContractSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    info.ContractSubjectName = contractSubject.F_ItemName;
                }
                //合同状态
                var contractStatus = dataItemBLL.GetDetailItemName(info.ContractStatus, "ContractStatus");
                if (contractStatus != null)
                {
                    info.ContractStatusName = contractStatus.F_ItemName;
                }
                //合同分类
                var contractType = dataItemBLL.GetDetailItemName(info.ContractType, "ContractType");
                if (contractType != null)
                {
                    info.ContractTypeName = contractType.F_ItemName;
                }
                //归档类型
                var receivedFlag = dataItemBLL.GetDetailItemName(info.ReceivedFlag, "ReceiptType");
                if (receivedFlag != null)
                {
                    info.ReceivedFlagName = receivedFlag.F_ItemName;
                }

                //if (info.ContractStatus.ToInt() == 11 && info.ProjectSource.ToInt() != 3)
                //{
                //    ProjectContractEntity entity = new ProjectContractEntity();
                //    entity.EffectiveAmountShow = 0;
                //    projectContractIBLL.SaveEntity1(info.id, entity);
                //}
                list.Add(info);
            }

            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };
            return Success(jsonData);
        }
        public Response GetContractPageListAll(dynamic _)
        {
            var dataJson = this.GetReqData();

            ReqPageParam parameter = JsonConvert.DeserializeObject<ReqPageParam>(dataJson);
            List<ProjectContractVo> list = new List<ProjectContractVo>();
            var data = projectContractIBLL.GetPageList(parameter.queryJson);
            foreach (var info in data)
            {
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");

                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }
                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                if (followPerson != null)
                {
                    info.FollowPersonName = followPerson.F_RealName;
                }
                //营销部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }
                //合同主体
                var contractSubject = dataItemBLL.GetDetailItemName(info.ContractSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    info.ContractSubjectName = contractSubject.F_ItemName;
                }
                //合同状态
                var contractStatus = dataItemBLL.GetDetailItemName(info.ContractStatus, "ContractStatus");
                if (contractStatus != null)
                {
                    info.ContractStatusName = contractStatus.F_ItemName;
                }
                //合同分类
                var contractType = dataItemBLL.GetDetailItemName(info.ContractType, "ContractType");
                if (contractType != null)
                {
                    info.ContractTypeName = contractType.F_ItemName;
                }
                //归档类型
                var receivedFlag = dataItemBLL.GetDetailItemName(info.ReceivedFlag, "ReceiptType");
                if (receivedFlag != null)
                {
                    info.ReceivedFlagName = receivedFlag.F_ItemName;
                }
                list.Add(info);
            }

            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);
        }

        /// 获取表单数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public Response GetFormProjectData(dynamic _)
        {
            string keyValue = this.GetReqData();
            var ProjectTaskData = projectTaskIBLL.GetFormProjectData(keyValue);
            var jsonData = new
            {
                ProjectTask = ProjectTaskData,
            };
            return Success(jsonData);
        }
        public Response RecaculateEffectiveAmount(dynamic _)
        {
            projectContractIBLL.RecaculateEffectiveAmount();
            var jsonData = new
            {
                msg = "成功",
            };
            return Success(jsonData);
        }
        public Response ReMatchTaskWithContractId(dynamic _)
        {
            projectTaskIBLL.ReMatchTaskAnfContract();
            var jsonData = new
            {
                msg = "成功",
            };
            return Success(jsonData);
        }
        public Response ReMatchPaymentWithContractId(dynamic _)
        {
            projectContractIBLL.ReMatchPaymentFromContract();
            var jsonData = new
            {
                msg = "成功",
            };
            return Success(jsonData);
        }
        public Response ReMatchBillingWithContractId(dynamic _)
        {
            projectContractIBLL.ReMatchBillingFromContract();
            var jsonData = new
            {
                msg = "成功",
            };
            return Success(jsonData);
        }
        public Response GetUpdateEffectiveAmount(dynamic _)
        {

            //查所有的合同
            var data = projectContractIBLL.GetPageListEffectiveAmount();
            foreach (var ina in data)
            {
                ProjectContractEntity entity = new ProjectContractEntity();

                //根据ProjectId查询付款是否有这个项目
                if (ina.MainContract == "1" && ina.ContractType == "1")
                {
                    var ct = projectContractIBLL.GetPageListEffectiveAmountProjectId(ina.ProjectId);
                    if (ct != null)
                    {
                        if (ct.PaymentAmount != null && ct.ContractAmount != null)
                        {
                            entity.EffectiveAmount = ct.ContractAmount - ct.PaymentAmount;
                        }
                        else
                        {
                            entity.EffectiveAmount = ct.ContractAmount;
                        }
                    }

                }
                else
                {
                    entity.EffectiveAmount = 0;
                }

                projectContractIBLL.SaveEntity(ina.id, entity);
            }
            var jsonData = new
            {
                ProjectContract = data,
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 根据id获取合同详情
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public Response GetContractById(dynamic _)
        {
            string keyValue = this.GetReqData();
            var ProjectContractData = projectContractIBLL.GetPreviewFormData(keyValue);
            /*var contractFileList = annexesFileIBLL.GetList(ProjectContractData.ContractFile);
            if (contractFileList != null)
            {
                ProjectContractData.annexesFileEntities = contractFileList;

            }*/
            var contractFileList = annexesFileIBLL.GetList(ProjectContractData.ContractFile);
            if (contractFileList != null)
            {
                foreach (var item in contractFileList)
                {
                    item.F_FileType = item.F_FileType.ToLower();
                }
                ProjectContractData.annexesFileEntities = contractFileList;
            }
            List<ProjectContractEntity> projectContracts = projectContractIBLL.GetProjectContractByProjectId(ProjectContractData.ProjectId);
            if (projectContracts.Count > 0)
            {
                ProjectContractData.ContractNo = projectContracts.FirstOrDefault().ContractNo;
                ProjectContractData.ContractSubject = projectContracts.FirstOrDefault().ContractSubject;
            }

            var projectSource = dataItemBLL.GetDetailItemName(ProjectContractData.ProjectSource, "ProjectSource");
            var contractSubject = dataItemBLL.GetDetailItemName(ProjectContractData.ContractSubject, "ContractSubject");
            var followPerson = userIBLL.GetFollowPersonNameByUserId(ProjectContractData.FollowPerson);
            var department = departmentIBLL.GetEntity(ProjectContractData.DepartmentId);
            var contractStatus = dataItemBLL.GetDetailItemName(ProjectContractData.ContractStatus, "ContractStatus");
            var receivedFlag = dataItemBLL.GetDetailItemName(ProjectContractData.ReceivedFlag, "ReceivedFlag");
            var contractType = dataItemBLL.GetDetailItemName(ProjectContractData.ContractType, "ContractType");
            if (receivedFlag != null)
            {
                ProjectContractData.ReceivedFlag = receivedFlag.F_ItemName;
            }
            /*if (contractStatus != null)
            {
                ProjectContractData.ContractStatus = contractStatus.F_ItemName;
            }*/
            if (projectSource != null)
            {
                ProjectContractData.ProjectSource = projectSource.F_ItemName;

            }
            if (contractSubject != null)
            {
                ProjectContractData.ContractSubjectName = contractSubject.F_ItemName;
            }
            if (contractType != null)
            {
                ProjectContractData.ContractTypeName = contractType.F_ItemName;
            }
            if (followPerson != null)
            {
                ProjectContractData.FollowPersonName = followPerson.F_RealName;
            }
            if (department != null)
            {
                ProjectContractData.DepartmentName = department.F_FullName;
            }
            var jsonData = new
            {
                ProjectContract = ProjectContractData,
            };
            return Success(jsonData);
        }

        public Response GetContractById2(dynamic _)
        {
            string keyValue = this.GetReqData();
            var ProjectContractData = projectContractIBLL.GetPreviewFormData(keyValue);
            //创建时间
            DateTime time = (DateTime)ProjectContractData.CreateTime;
            ProjectContractData.CreateTimeyMd = time.ToString("yyyy-MM-dd");

            //项目来源
            var projectSource = dataItemBLL.GetDetailItemName(ProjectContractData.ProjectSource, "ProjectSource");
            if (projectSource != null)
            {
                ProjectContractData.ProjectSourceName = projectSource.F_ItemName;
            }
            //营销人员
            var followPerson = userIBLL.GetEntityByUserId(ProjectContractData.FollowPerson);
            if (followPerson != null)
            {
                ProjectContractData.FollowPersonName = followPerson.F_RealName;
            }
            //营销部门
            if (ProjectContractData.DepartmentId != null)
            {
                var department = departmentIBLL.GetEntity(ProjectContractData.DepartmentId);
                if (department != null)
                {
                    ProjectContractData.DepartmentName = department.F_FullName;
                }
            }
            //主部门
            if (ProjectContractData.MainDepartmentId != null)
            {
                var department1 = departmentIBLL.GetEntity(ProjectContractData.MainDepartmentId);
                if (department1 != null)
                {
                    ProjectContractData.MainDepartmentName = department1.F_FullName;
                }
            }
            //次部门
            if (ProjectContractData.SubDepartmentId != null)
            {
                var department2 = departmentIBLL.GetEntity(ProjectContractData.SubDepartmentId);
                if (department2 != null)
                {
                    ProjectContractData.SubDepartmentName = department2.F_FullName;
                }
            }
            //合同主体
            var contractSubject = dataItemBLL.GetDetailItemName(ProjectContractData.ContractSubject, "ContractSubject");
            if (contractSubject != null)
            {
                ProjectContractData.ContractSubjectName = contractSubject.F_ItemName;
            }

            //合同分类
            var contractType = dataItemBLL.GetDetailItemName(ProjectContractData.ContractType, "ContractType");
            if (contractType != null)
            {
                ProjectContractData.ContractTypeName = contractType.F_ItemName;
            }
            return Success(ProjectContractData);
        }
        /// <summary>
        /// 根据流程id获取合同详情
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public Response GetContractByProcessId(dynamic _)
        {
            string keyValue = this.GetReqData();
            var ProjectContractData = projectContractIBLL.GetEntityByProcessId(keyValue);
            /*      var projectSource = dataItemBLL.GetDetailItemName(ProjectContractData.ProjectSource, "ProjectSource");
                  var contractStatus = dataItemBLL.GetDetailItemName(ProjectContractData.ContractStatus, "ContractStatus");*/

            /*var contractFileList = annexesFileIBLL.GetList(ProjectContractData.ContractFile);
            if (contractFileList != null)
            {
                ProjectContractData.annexesFileEntities = contractFileList;
            }*/
            //创建时间
            DateTime time = (DateTime)ProjectContractData.CreateTime;
            ProjectContractData.CreateTimeyMd = time.ToString("yyyy-MM-dd");

            //项目来源
            var projectSource = dataItemBLL.GetDetailItemName(ProjectContractData.ProjectSource, "ProjectSource");
            if (projectSource != null)
            {
                ProjectContractData.ProjectSourceName = projectSource.F_ItemName;
            }
            //营销人员
            var followPerson = userIBLL.GetEntityByUserId(ProjectContractData.FollowPerson);
            if (followPerson != null)
            {
                ProjectContractData.FollowPersonName = followPerson.F_RealName;
            }
            //营销部门
            if (ProjectContractData.DepartmentId != null)
            {
                var department = departmentIBLL.GetEntity(ProjectContractData.DepartmentId);
                if (department != null)
                {
                    ProjectContractData.DepartmentName = department.F_FullName;
                }
            }
            //主部门
            if (ProjectContractData.MainDepartmentId != null)
            {
                var department1 = departmentIBLL.GetEntity(ProjectContractData.MainDepartmentId);
                if (department1 != null)
                {
                    ProjectContractData.MainDepartmentName = department1.F_FullName;
                }
            }
            //次部门
            if (ProjectContractData.SubDepartmentId != null)
            {
                var department2 = departmentIBLL.GetEntity(ProjectContractData.SubDepartmentId);
                if (department2 != null)
                {
                    ProjectContractData.SubDepartmentName = department2.F_FullName;
                }
            }
            //合同主体
            var contractSubject = dataItemBLL.GetDetailItemName(ProjectContractData.ContractSubject, "ContractSubject");
            //if (contractSubject != null)
            //{
            //    ProjectContractData.ContractSubjectName = contractSubject.F_ItemName;
            //}

            //合同分类
            var contractType = dataItemBLL.GetDetailItemName(ProjectContractData.ContractType, "ContractType");
            if (contractType != null)
            {
                ProjectContractData.ContractTypeName = contractType.F_ItemName;
            }
            var jsonData = new
            {
                ProjectContract = ProjectContractData,
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 合同的新增，修改
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response ContractSave(dynamic _)
        {
            ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            ProjectContractEntity entity = parameter.strEntity.ToObject<ProjectContractEntity>();
            if (string.IsNullOrEmpty(entity.ProjectSource))
            {
                entity.ProjectSource = "1";
            }
            projectContractIBLL.SaveEntity(parameter.keyValue, entity);
            return Success("保存成功！");
        }
        //提审
        public Response GetContractSubmitter2(dynamic _)
        {
            string keyValue = this.GetReqData();
            // var ProjectContractData = projectContractIBLL.GetPreviewFormData(keyValue);
            var ProjectContractData = projectContractIBLL.GetProjectContractEntity(keyValue);
            if (ProjectContractData.ContractStatus.ToInt() == 11 && ProjectContractData.WorkFlowId != null)
            {
                projectContractIBLL.UpdateFlowId(ProjectContractData.id, ProjectContractData.WorkFlowId);
                UserInfo userInfo = LoginUserInfo.Get();
                nWFProcessIBLL.AgainCreateFlow(ProjectContractData.WorkFlowId, userInfo, "");
            }
            else
            {
                projectContractIBLL.UpdateFlowId(ProjectContractData.id, ProjectContractData.WorkFlowId);
            }
            ProjectContractData = projectContractIBLL.GetProjectContractEntity(keyValue);
            projectContractIBLL.SaveEntity(keyValue, ProjectContractData);
            return Success("提审成功！");
        }
        //提审
        public Response GeTaskSubmitter2(dynamic _)
        {
            string keyValue = this.GetReqData();
            var ProjectTaskData = projectTaskIBLL.GetPriewProjectTask(keyValue);
            //if (ProjectTaskData.TaskStatus.ToInt() == 11 && ProjectTaskData.WorkFlowId != null)
            if (!string.IsNullOrEmpty(ProjectTaskData.WorkFlowId))
            {
                projectTaskIBLL.UpdateFlowId(ProjectTaskData.id, ProjectTaskData.WorkFlowId);
                UserInfo userInfo = LoginUserInfo.Get();
                nWFProcessIBLL.AgainCreateFlow(ProjectTaskData.WorkFlowId, userInfo, "");
            }
            else
            {
                projectTaskIBLL.UpdateFlowId(ProjectTaskData.id, ProjectTaskData.WorkFlowId);
            }

            return Success("提审成功！");
        }
        public Response ContractSave2(dynamic _)
        {
            //string result = string.Empty;
            //using (StreamReader stream = new StreamReader(HttpContext.Current.Request.InputStream))
            //{
            //    result = HttpUtility.UrlDecode(stream.ReadToEnd(), Encoding.UTF8);
            //    //todo
            //}
            //HttpContext.Current.Response.ContentType = "application/json";
            //HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //using (var reader = new StreamReader(HttpContext.Current.Request.InputStream))
            //{
            //    result = reader.ReadToEnd();//data是post传过来的数据
            //}
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            string jsonText = string.Empty;

            HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空
            StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
            jsonText = sr.ReadToEnd();

            //ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            ProjectContractEntity entity = jsonText.ToObject<ProjectContractEntity>();
            if (entity.ProjectId == null || entity.ProjectId == "")
            {
                return Fail("项目名称不能为空");
            }
            if (entity.ContractType == null)
            {
                return Fail("合同分类不能为空");
            }
            if (entity.ContractNo == null || entity.ContractNo == "")
            {
                return Fail("合同编号不能为空");
            }
            projectContractIBLL.SaveEntity(entity.id, entity);
            if (entity.ContractType > 0)
            {
                if (entity.ContractType == 1)
                {
                    codeRuleIBLL.UseRuleSeed("10010");
                }
                else
                {
                    codeRuleIBLL.UseRuleSeed("10011");
                }
            }
            return Success("保存成功！");
        }

        public Response UpdateReceivedFlagSave2(dynamic _)
        {
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            string jsonText = string.Empty;

            HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空

            StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
            jsonText = sr.ReadToEnd();

            //ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            ProjectContractEntity entity = jsonText.ToObject<ProjectContractEntity>();
            //根据合同编号查询
            var cont = projectContractIBLL.GetPageListCont(entity.ContractNo);
            if (cont.Count().ToInt() > 0)
            {
                foreach (var i in cont)
                {
                    projectContractIBLL.UpdateReceivedFlag(i.id, entity);
                    if (entity.ReceivedFlagNo != null)
                    {
                        if (entity.ReceivedFlagNo.Contains("TJ"))
                        {
                            codeRuleIBLL.UseRuleSeed("1TJ");
                        }
                        else if (entity.ReceivedFlagNo.Contains("FB"))
                        {
                            codeRuleIBLL.UseRuleSeed("1FB");
                        }
                        else
                        {
                            codeRuleIBLL.UseRuleSeed("1");
                        }
                    }
                }
            }
            projectContractIBLL.SaveEntity(entity.id, entity);
            return Success("保存成功！");
        }
        public Response Zuofei(dynamic _)
        {
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            string jsonText = string.Empty;

            HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空

            StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
            jsonText = sr.ReadToEnd();

            //ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            ProjectContractEntity entity1 = jsonText.ToObject<ProjectContractEntity>();
            ProjectContractEntity entity = projectContractIBLL.GetProjectContractEntity(entity1.id);
            entity.CancelTheReason = entity1.CancelTheReason;
            entity.ContractStatus = 7;
            if (entity.WorkFlowId != null)
            {
                var processEntity = nWFProcessIBLL.GetEntity(entity.WorkFlowId);
                if (processEntity.F_IsFinished != 1)
                {
                    NWFProcessEntity n = new NWFProcessEntity();
                    n.F_Id = entity.WorkFlowId;
                    n.F_IsFinished = 1;
                    nWFProcessIBLL.GetEntityByWorkFlowId(entity.WorkFlowId, n);
                }
            }
            projectContractIBLL.SaveEntity1(entity1.id, entity);

            return Success("取消成功！");
        }

        #endregion

        #region 任务接口

        public Response GetTaskForMatchPageList(dynamic _)
        {
            var dataJson = this.GetReqData();

            ReqPageParam parameter = JsonConvert.DeserializeObject<ReqPageParam>(dataJson);
            List<ProjectTaskVo> list = new List<ProjectTaskVo>();
            var data = projectTaskIBLL.getTaskForMatchPageList(parameter.pagination, parameter.queryJson);
            foreach (var info in data)
            {
                if (string.IsNullOrEmpty(info.ProjectName))
                {
                    var projectInfo = projectIBLL.GetProjectEntity(info.ProjectId);
                    if (projectInfo != null)
                    {
                        info.ProjectName = projectInfo.ProjectName;
                    }
                }
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //报告时间
                if (info.FlowFinishedTime != null)
                {
                    DateTime time1 = (DateTime)info.FlowFinishedTime;
                    info.FlowFinishedTimeMD = time1.ToString("yyyy-MM-dd");
                }
                //进场时间
                if (info.ActualApproachTime != null)
                {
                    DateTime time1 = (DateTime)info.ActualApproachTime;
                    info.ApproachTimeMd = time1.ToString("yyyy-MM-dd");
                }

                //离场时间
                if (info.ActualDepartureTime != null)
                {
                    DateTime time2 = (DateTime)info.ActualDepartureTime;
                    info.ActualDepartureTimeMd = time2.ToString("yyyy-MM-dd");
                }
                //报告计划时间
                if (info.PlanTime != null)
                {
                    DateTime time3 = (DateTime)info.PlanTime;
                    info.PlanTimeMd = time3.ToString("yyyy-MM-dd");
                }
                //项目负责人
                var projectResponsible = userIBLL.GetEntityByUserId(info.ProjectResponsible);
                if (projectResponsible != null)
                {
                    info.ProjectResponsibleName = projectResponsible.F_RealName;
                }
                //所属部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }

                //报告状态
                var taskStatus = dataItemBLL.GetDetailItemName(info.TaskStatus, "TaskStatus");
                if (taskStatus != null)
                {
                    info.TaskStatusName = taskStatus.F_ItemName;
                }

                //报告主体
                var contractSubject = dataItemBLL.GetDetailItemName(info.ReportSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    info.ReportSubjectName = contractSubject.F_ItemName;
                }
                list.Add(info);
            }


            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };
            return Success(jsonData);
        }

        public Response GetTaskPageList(dynamic _)
        {
            var dataJson = this.GetReqData();

            ReqPageParam parameter = JsonConvert.DeserializeObject<ReqPageParam>(dataJson);
            List<ProjectTaskVo> list = new List<ProjectTaskVo>();
            var data = projectTaskIBLL.GetPageList(parameter.pagination, parameter.queryJson);
            foreach (var info in data)
            {
                if (string.IsNullOrEmpty(info.ProjectName))
                {
                    var projectInfo = projectIBLL.GetProjectEntity(info.ProjectId);
                    if (projectInfo != null)
                    {
                        info.ProjectName = projectInfo.ProjectName;
                    }
                }
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //报告时间
                if (info.FlowFinishedTime != null)
                {
                    DateTime time1 = (DateTime)info.FlowFinishedTime;
                    info.FlowFinishedTimeMD = time1.ToString("yyyy-MM-dd");
                }
                //进场时间
                if (info.ActualApproachTime != null)
                {
                    DateTime time1 = (DateTime)info.ActualApproachTime;
                    info.ApproachTimeMd = time1.ToString("yyyy-MM-dd");
                }

                //离场时间
                if (info.ActualDepartureTime != null)
                {
                    DateTime time2 = (DateTime)info.ActualDepartureTime;
                    info.ActualDepartureTimeMd = time2.ToString("yyyy-MM-dd");
                }
                //报告计划时间
                if (info.PlanTime != null)
                {
                    DateTime time3 = (DateTime)info.PlanTime;
                    info.PlanTimeMd = time3.ToString("yyyy-MM-dd");
                }
                //项目负责人
                var projectResponsible = userIBLL.GetEntityByUserId(info.ProjectResponsible);
                if (projectResponsible != null)
                {
                    info.ProjectResponsibleName = projectResponsible.F_RealName;
                }
                //所属部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }

                //报告状态
                var taskStatus = dataItemBLL.GetDetailItemName(info.TaskStatus, "TaskStatus");
                if (taskStatus != null)
                {
                    info.TaskStatusName = taskStatus.F_ItemName;
                }

                //报告主体
                var contractSubject = dataItemBLL.GetDetailItemName(info.ReportSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    info.ReportSubjectName = contractSubject.F_ItemName;
                }
                list.Add(info);
            }


            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };
            return Success(jsonData);
        }

        public Response GetTaskLoadList(dynamic _)
        {
            var dataJson = this.GetReqData();
            QueryTaskLoadParam parameter = JsonConvert.DeserializeObject<QueryTaskLoadParam>(dataJson);
            //List<ProjectTaskVo> list = new List<ProjectTaskVo>();
            var list = projectTaskIBLL.GetTaskLoadList(parameter.queryJson, parameter.userId, parameter.departmentId, parameter.type);
            List<TaskLoadsRes> loadList = new List<TaskLoadsRes>();
            if (list.ToList().Count > 0)
            {
                var deptList = list.Select(i => i.DepartmentId).Distinct().ToList();

                foreach (var dept in deptList)
                {
                    TaskLoadsRes load = new TaskLoadsRes();
                    load.userLoads = new List<TaskLoadsItemRes>();
                    var taskList = list.Where(i => i.DepartmentId == dept).ToList();
                    load.totalCount = taskList.Count();
                    load.departmentId = taskList[0].DepartmentId;
                    load.departmentName = taskList[0].DepartmentName;
                    var userList = taskList.Select(i => i.ProjectResponsible).Distinct().ToList();
                    foreach (var userId in userList)
                    {
                        var userTaskList = list.Where(i => i.ProjectResponsible == userId).ToList();
                        TaskLoadsItemRes item = new TaskLoadsItemRes();
                        item.userId = userId;
                        item.userName = userTaskList[0].ProjectResponsibleName;
                        item.totalCount = userTaskList.Count();
                        //现场中
                        var task1 = userTaskList.Where(i => i.TaskStatus == "2").ToList();
                        //待进场
                        var task2 = userTaskList.Where(i => i.TaskStatus == "1").ToList();
                        //超期未进场
                        DateTime now = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(1);
                        var task3 = userTaskList.Where(i => i.TaskStatus == "1" && i.ApproachTime < now).ToList();
                        //已完成
                        var task4 = userTaskList.Where(i => i.TaskStatus == "5").ToList();
                        item.taskCount1 = task1.Count();
                        item.taskCount2 = task2.Count();
                        item.taskCount3 = task3.Count();
                        item.taskCount4 = task4.Count();
                        load.userLoads.Add(item);
                    }
                    loadList.Add(load);
                }
            }
            return Success(JsonConvert.SerializeObject(loadList));
        }


        public Response GetHZTaskPageList(dynamic _)
        {
            var dataJson = this.GetReqData();

            ReqPageParam parameter = JsonConvert.DeserializeObject<ReqPageParam>(dataJson);
            List<ProjectTaskVo> list = new List<ProjectTaskVo>();
            var data = projectTaskIBLL.GetHZPageList(parameter.pagination, parameter.queryJson);
            foreach (var info in data)
            {
                if (string.IsNullOrEmpty(info.ProjectName))
                {
                    var projectInfo = projectIBLL.GetProjectEntity(info.ProjectId);
                    if (projectInfo != null)
                    {
                        info.ProjectName = projectInfo.ProjectName;
                    }
                }
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //报告时间
                if (info.FlowFinishedTime != null)
                {
                    DateTime time1 = (DateTime)info.FlowFinishedTime;
                    info.FlowFinishedTimeMD = time1.ToString("yyyy-MM-dd");
                }
                //进场时间
                if (info.ApproachTime != null)
                {
                    DateTime time1 = (DateTime)info.ApproachTime;
                    info.ApproachTimeMd = time1.ToString("yyyy-MM-dd");
                }

                //离场时间
                if (info.ActualDepartureTime != null)
                {
                    DateTime time2 = (DateTime)info.ActualDepartureTime;
                    info.ActualDepartureTimeMd = time2.ToString("yyyy-MM-dd");
                }
                //报告计划时间
                if (info.PlanTime != null)
                {
                    DateTime time3 = (DateTime)info.PlanTime;
                    info.PlanTimeMd = time3.ToString("yyyy-MM-dd");
                }
                //项目负责人
                var projectResponsible = userIBLL.GetEntityByUserId(info.ProjectResponsible);
                if (projectResponsible != null)
                {
                    info.ProjectResponsibleName = projectResponsible.F_RealName;
                }
                //所属部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }

                //报告状态
                var taskStatus = dataItemBLL.GetDetailItemName(info.TaskStatus, "TaskStatus");
                if (taskStatus != null)
                {
                    info.TaskStatusName = taskStatus.F_ItemName;
                }

                //报告主体
                var contractSubject = dataItemBLL.GetDetailItemName(info.ReportSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    info.ReportSubjectName = contractSubject.F_ItemName;
                }
                list.Add(info);
            }


            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };
            return Success(jsonData);
        }
        public Response GetTaskScheduleList(dynamic _)
        {

            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            string jsonText = string.Empty;

            HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空
            StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
            jsonText = sr.ReadToEnd();
            ScheduleParam parameter = JsonConvert.DeserializeObject<ScheduleParam>(jsonText);
            DateTime startDate = DateTime.Parse(parameter.startTime);
            List<TaskScheduleVo> list = projectTaskIBLL.GetScheduleList(parameter).ToList();
            List<string> userIdList = new List<string>();
            List<TaskScheduleRes> resultList = new List<TaskScheduleRes>();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    item.InspectorName = new List<string>();
                    item.ProjectResponsibleName = new List<string>();
                    string[] strList = item.Inspector.Split(',');
                    string[] strList2 = item.ProjectResponsible.Split(',');
                    for (var i = 0; i < strList2.Length; i++)
                    {
                        var responsibleNameInfo = userIBLL.GetFollowPersonNameByUserId(strList2[i]);
                        if (responsibleNameInfo != null)
                        {
                            item.ProjectResponsibleName.Add(responsibleNameInfo.F_RealName);
                        }
                    }
                    //日期间隔
                    if (item.PlanTime.HasValue)
                    {
                        DateTime planDate = DateTime.Parse(item.PlanTime.Value.ToString("yyyy-MM-dd"));
                        TimeSpan difference = planDate.Subtract(startDate);
                        int diffDays = difference.Days;
                        item.PlanDateDiff = diffDays;
                    }
                    if (item.ApproachTime.HasValue)
                    {
                        DateTime planDate = DateTime.Parse(item.ApproachTime.Value.ToString("yyyy-MM-dd"));
                        TimeSpan difference = planDate.Subtract(startDate);
                        int diffDays = difference.Days;
                        item.ApproachDateDiff = diffDays;
                    }
                    for (var i = 0; i < strList.Length; i++)
                    {
                        var inspectorInfo = userIBLL.GetFollowPersonNameByUserId(strList[i]);
                        if (inspectorInfo != null)
                        {
                            item.InspectorName.Add(inspectorInfo.F_RealName);
                            var chesckUserId = resultList.Where(ii => ii.userId == inspectorInfo.F_UserId).ToList();
                            if (chesckUserId.Count == 0)
                            {
                                TaskScheduleRes model = new TaskScheduleRes()
                                {
                                    userId = inspectorInfo.F_UserId,
                                    userName = inspectorInfo.F_RealName,
                                    taskList = new List<TaskScheduleVo>()
                                };
                                model.taskList.Add(item);
                                resultList.Add(model);
                            }
                            else
                            {
                                foreach (var taskRes in resultList)
                                {
                                    if (taskRes.userId == inspectorInfo.F_UserId)
                                    {
                                        taskRes.taskList.Add(item);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            var jsonData = new
            {
                rows = list,
                task = resultList
            };
            return Success(jsonData);
        }


        public Response GetLeaderboardData(dynamic _)
        {
            var dataJson = this.GetReqData();
            ReqPageParam parameter = JsonConvert.DeserializeObject<ReqPageParam>(dataJson);

            List<ProjectContractVo> list = new List<ProjectContractVo>();
            var contractLeader = gantProjectIBLL.GetContractAmountLeaderboard(parameter.queryJson);
            var jsonData = new
            {
                contractLeader = list,
            };
            return Success(jsonData);
        }


        public Response ReCaculateFinishTime(dynamic _)
        {
            var list = projectTaskIBLL.GetAllFinishedTask();
            if (list.ToList().Count > 0)
            {
                foreach (var item in list)
                {
                    var task_list = nWFProcessIBLL.GetAllTaskList(item.WorkFlowId);
                    if (task_list.ToList().Count > 0)
                    {
                        var latest_task = task_list.ToList().OrderByDescending(i => i.F_ModifyDate).FirstOrDefault();
                        item.FlowFinishedTime = latest_task.F_ModifyDate;
                        projectTaskIBLL.SaveEntity(item.id, item);
                    }
                }
            }
            return Success(1);
        }
        public Response GetTaskById(dynamic _)
        {
            string keyValue = this.GetReqData();
            var ProjectTaskData = projectTaskIBLL.GetPriewProjectTask(keyValue);
            if (ProjectTaskData.Inspector != null)
            {
                string[] strList = ProjectTaskData.Inspector.Split(',');

                string inspectorName = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    /*string inspectorInfo = (strList[i]);*/
                    var inspectorInfo = userIBLL.GetFollowPersonNameByUserId(strList[i]);
                    if (inspectorInfo != null)
                    {
                        if (string.IsNullOrWhiteSpace(inspectorName))
                        {
                            inspectorName = inspectorInfo.F_RealName;
                        }
                        else
                        {
                            inspectorName = inspectorName + "," + inspectorInfo.F_RealName;
                        }
                    }
                }
                ProjectTaskData.Inspector = inspectorName;
            }
            var reportApprover = userIBLL.GetAccountName(ProjectTaskData.ReportApprover);
            if (reportApprover != null)
            {
                ProjectTaskData.ReportApprover = reportApprover.F_RealName;
            }
            var reportFile = annexesFileIBLL.GetList(ProjectTaskData.ReportFile);
            if (reportFile != null)
            {
                foreach (var item in reportFile)
                {
                    item.F_FileType = item.F_FileType.ToLower();
                }
                ProjectTaskData.annexesFileEntities = reportFile;
            }

            var projectResponsible = userIBLL.GetFollowPersonNameByUserId(ProjectTaskData.ProjectResponsible);

            var department = departmentIBLL.GetEntity(ProjectTaskData.DepartmentId);

            if (department != null)
            {
                ProjectTaskData.DepartmentId = department.F_FullName;
            }
            if (projectResponsible != null)
            {
                ProjectTaskData.ProjectResponsible = projectResponsible.F_RealName;
            }

            var jsonData = new
            {
                ProjectTask = ProjectTaskData,
            };
            return Success(jsonData);
        }
        public Response GetTaskById2(dynamic _)
        {
            string keyValue = this.GetReqData();
            var ProjectTaskData = projectTaskIBLL.GetPriewProjectTask(keyValue);
            if (ProjectTaskData.Inspector != null)
            {
                string[] strList = ProjectTaskData.Inspector.Split(',');

                string inspectorName = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    /*string inspectorInfo = (strList[i]);*/
                    var inspectorInfo = userIBLL.GetFollowPersonNameByUserId(strList[i]);
                    if (inspectorInfo != null)
                    {
                        if (string.IsNullOrWhiteSpace(inspectorName))
                        {
                            inspectorName = inspectorInfo.F_RealName;
                        }
                        else
                        {
                            inspectorName = inspectorName + "," + inspectorInfo.F_RealName;
                        }
                    }
                }
                ProjectTaskData.InspectorName = inspectorName;
            }
            var reportApprover = userIBLL.GetAccountName(ProjectTaskData.ReportApprover);
            if (reportApprover != null)
            {
                ProjectTaskData.ReportApproverName = reportApprover.F_RealName;
            }
            var reportFile = annexesFileIBLL.GetList(ProjectTaskData.ReportFile);
            if (reportFile != null)
            {
                foreach (var item in reportFile)
                {
                    item.F_FileType = item.F_FileType.ToLower();
                }
                ProjectTaskData.annexesFileEntities = reportFile;
            }
            //营销部门
            if (ProjectTaskData.MainDepartmentId != null)
            {
                var department2 = departmentIBLL.GetEntity(ProjectTaskData.MainDepartmentId);
                if (department2 != null)
                {
                    ProjectTaskData.MainDepartmentName = department2.F_FullName;
                }
            }
            //次部门
            if (ProjectTaskData.SubDepartmentId != null)
            {
                var department3 = departmentIBLL.GetEntity(ProjectTaskData.SubDepartmentId);
                if (department3 != null)
                {
                    ProjectTaskData.SubDepartmentName = department3.F_FullName;
                }
            }
            var projectResponsible = userIBLL.GetFollowPersonNameByUserId(ProjectTaskData.ProjectResponsible);

            var department = departmentIBLL.GetEntity(ProjectTaskData.DepartmentId);

            if (department != null)
            {
                ProjectTaskData.DepartmentId = department.F_FullName;
            }
            if (projectResponsible != null)
            {
                ProjectTaskData.ProjectResponsibleName = projectResponsible.F_RealName;
            }

            ProjectTaskData.ProjectTaskNoOrigin = ProjectTaskData.ProjectTaskNo;
            return Success(ProjectTaskData);
        }
        public Response GetTaskByContractId(dynamic _)
        {
            string keyValue = this.GetReqData();
            var ProjectContractData = projectContractIBLL.ProjectTaskByContractId(keyValue);
            //项目来源
            DataItemDetailEntity projectSource = dataItemBLL.GetDetailItemName(ProjectContractData.ProjectSource, "ProjectSource");
            if (projectSource != null)
            {
                ProjectContractData.ProjectSourceName = projectSource.F_ItemName;

            }
            //合同主体
            DataItemDetailEntity contractSubject = dataItemBLL.GetDetailItemName(ProjectContractData.ContractSubject, "ContractSubject");
            if (contractSubject != null)
            {
                ProjectContractData.ContractSubjectName = contractSubject.F_ItemName;
            }
            //合同分类
            DataItemDetailEntity contractType = dataItemBLL.GetDetailItemName(ProjectContractData.ContractType, "ContractType");

            if (contractType != null)
            {
                ProjectContractData.ContractTypeName = contractType.F_ItemName;
            }
            //营销人员
            UserEntity followPerson = userIBLL.GetFollowPersonNameByUserId(ProjectContractData.FollowPerson);
            if (followPerson != null)
            {
                ProjectContractData.FollowPersonName = followPerson.F_RealName;
            }
            //销售部门
            var department = departmentIBLL.GetEntity(ProjectContractData.DepartmentId);
            if (department != null)
            {
                ProjectContractData.DepartmentName = department.F_FullName;
            }

            return Success(ProjectContractData);
        }

        public Response GetTaskByProcessId(dynamic _)
        {
            string keyValue = this.GetReqData();
            var ProjectTaskData = projectTaskIBLL.GetTaskByProcessId(keyValue);

            /*var reportFile = annexesFileIBLL.GetList(ProjectTaskData.ReportFile);
            if (reportFile != null)
            {
                foreach (var item in reportFile)
                {
                    item.F_FileType = item.F_FileType.ToLower();
                }
                ProjectTaskData.annexesFileEntities = reportFile;
            }

            var reportSubject = dataItemBLL.GetDetailItemName(ProjectTaskData.ReportSubject, "ContractSubject");

            var taskStatus = dataItemBLL.GetDetailItemName(ProjectTaskData.TaskStatus, "TaskStatus");
            if (reportSubject != null)
            {
                ProjectTaskData.ReportSubject = reportSubject.F_ItemName;
            }

            if (taskStatus != null)
            {
                ProjectTaskData.TaskStatus = taskStatus.F_ItemName;
            }*/
            //营销部门
            if (ProjectTaskData.MainDepartmentId != null)
            {
                var department2 = departmentIBLL.GetEntity(ProjectTaskData.MainDepartmentId);
                if (department2 != null)
                {
                    ProjectTaskData.MainDepartmentName = department2.F_FullName;
                }
            }
            //营销部门
            if (ProjectTaskData.SubDepartmentId != null)
            {
                var department3 = departmentIBLL.GetEntity(ProjectTaskData.SubDepartmentId);
                if (department3 != null)
                {
                    ProjectTaskData.SubDepartmentName = department3.F_FullName;
                }
            }
            if (ProjectTaskData.Inspector != null)
            {
                string[] strList = ProjectTaskData.Inspector.Split(',');

                string inspectorName = "";
                for (var i = 0; i < strList.Length; i++)
                {
                    /*string inspectorInfo = (strList[i]);*/
                    var inspectorInfo = userIBLL.GetFollowPersonNameByUserId(strList[i]);
                    if (inspectorInfo != null)
                    {
                        if (string.IsNullOrWhiteSpace(inspectorName))
                        {
                            inspectorName = inspectorInfo.F_RealName;
                        }
                        else
                        {
                            inspectorName = inspectorName + "," + inspectorInfo.F_RealName;
                        }
                    }
                }
                ProjectTaskData.InspectorName = inspectorName;
            }
            var reportApprover = userIBLL.GetAccountName(ProjectTaskData.ReportApprover);
            if (reportApprover != null)
            {
                ProjectTaskData.ReportApprover = reportApprover.F_RealName;
            }
            var reportFile = annexesFileIBLL.GetList(ProjectTaskData.ReportFile);
            if (reportFile != null)
            {
                foreach (var item in reportFile)
                {
                    item.F_FileType = item.F_FileType.ToLower();
                }
                ProjectTaskData.annexesFileEntities = reportFile;
            }

            var projectResponsible = userIBLL.GetFollowPersonNameByUserId(ProjectTaskData.ProjectResponsible);

            var department = departmentIBLL.GetEntity(ProjectTaskData.DepartmentId);

            if (department != null)
            {
                ProjectTaskData.DepartmentId = department.F_FullName;
            }
            if (projectResponsible != null)
            {
                ProjectTaskData.ProjectResponsibleName = projectResponsible.F_RealName;
            }
            //评级
            var Rating = dataItemBLL.GetDetailItemName(ProjectTaskData.Rating, "Rating");
            if (Rating != null)
            {
                ProjectTaskData.RatingName = Rating.F_ItemName;
            }
            var jsonData = new
            {
                ProjectTask = ProjectTaskData,
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 报告上传
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetProjectTaskSc(dynamic _)
        {
            ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            ProjectTaskEntity entity = parameter.strEntity.ToObject<ProjectTaskEntity>();
            projectTaskIBLL.GetProjectTaskSc(parameter.keyValue, entity);
            return Success("上传成功");
        }
        public Response ProjectTaskS2c(dynamic _)
        {
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            string jsonText = string.Empty;
            HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空
            StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
            jsonText = sr.ReadToEnd();
            ProjectTaskEntity entity = jsonText.ToObject<ProjectTaskEntity>();
            projectTaskIBLL.GetProjectTaskSc(entity.id, entity);
            return Success("上传成功");
        }
        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetProjectTaskFielded(dynamic _)
        {
            ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            ProjectTaskEntity entity = parameter.strEntity.ToObject<ProjectTaskEntity>();
            projectTaskIBLL.UpdateFieldedAPI(parameter.keyValue, entity);
            return Success("签到成功");
        }
        /// <summary>
        /// 签到/离场
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetProjectTaskFielded2(dynamic _)
        {
            string keyValue = this.GetReqData();
            projectTaskIBLL.UpdateFielded(keyValue);
            return Success("签到成功");
        }
        public Response GetProjectTaskDelete2(dynamic _)
        {
            string keyValue = this.GetReqData();
            projectTaskIBLL.DeleteEntity(keyValue);
            return Success("删除成功");
        }


        /// <summary>
        /// 报告的新增和修改
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response TaskSave(dynamic _)
        {
            ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            ProjectTaskUserEntity entity = parameter.strEntity.ToObject<ProjectTaskUserEntity>();
            projectTaskIBLL.SaveEntityApi(parameter.keyValue, entity);
            return Success("保存成功");

        }
        //子报告添加
        public Response TaskSaveFormTast(dynamic _)
        {
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            string jsonText = string.Empty;
            HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空
            StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
            jsonText = sr.ReadToEnd();
            ProjectTaskEntity entity = jsonText.ToObject<ProjectTaskEntity>();
            if (string.IsNullOrEmpty(entity.ProjectTaskNo))
            {
                entity.ProjectTaskNo = projectTaskIBLL.GetNextProjectTaskNo_ZBG(entity.ProjectTaskNo + "-");
            }
            entity.id = null;
            if (!string.IsNullOrEmpty(entity.ProjectResponsible))
            {
                string[] userIds = entity.ProjectResponsible.Split(',');
                string dept = "";
                for (var i = 0; i < userIds.Length; i++)
                {
                    var user = userIBLL.GetEntityByUserId(userIds[i]);
                    if (user != null)
                    {
                        if (!string.IsNullOrEmpty(user.F_DepartmentId))
                        {
                            if (string.IsNullOrEmpty(dept))
                            {
                                dept = user.F_DepartmentId;
                            }
                            else
                            {
                                dept = dept + "," + user.F_DepartmentId;
                            }
                        }
                    }
                }

                entity.DepartmentId = dept;
            }
            else
            {
                entity.DepartmentId = "";
            }
            projectTaskIBLL.SaveFormTast(entity);
            return Success("保存成功");

        }
        public Response TaskSave2(dynamic _)
        {
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            string jsonText = string.Empty;

            HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空
            StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
            jsonText = sr.ReadToEnd();

            //ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            ProjectTaskEntity entity = jsonText.ToObject<ProjectTaskEntity>();
            if (string.IsNullOrEmpty(entity.ProjectId))
            {
                return Fail("项目名称不能为空");
            }
            if (string.IsNullOrEmpty(entity.ProjectResponsible))
            {
                return Fail("项目负责人不能为空");
            }
            if (entity.ProjectId != null)
            {
                if (!string.IsNullOrEmpty(entity.ProjectResponsible))
                {
                    string[] userIds = entity.ProjectResponsible.Split(',');
                    string dept = "";
                    for (var i = 0; i < userIds.Length; i++)
                    {
                        var user = userIBLL.GetEntityByUserId(userIds[i]);
                        if (user != null)
                        {
                            if (!string.IsNullOrEmpty(user.F_DepartmentId))
                            {
                                if (string.IsNullOrEmpty(dept))
                                {
                                    dept = user.F_DepartmentId;
                                }
                                else
                                {
                                    dept = dept + "," + user.F_DepartmentId;
                                }
                            }
                        }
                    }

                    entity.DepartmentId = dept;
                }
                else
                {
                    entity.DepartmentId = "";
                }
                if (!string.IsNullOrEmpty(entity.Inspector))
                {
                    string[] userIds = entity.Inspector.Split(',');
                    string dept = "";
                    for (var i = 0; i < userIds.Length; i++)
                    {
                        var user = userIBLL.GetEntityByUserId(userIds[i]);
                        if (user != null)
                        {
                            if (!string.IsNullOrEmpty(user.F_DepartmentId))
                            {
                                if (string.IsNullOrEmpty(dept))
                                {
                                    dept = user.F_DepartmentId;
                                }
                                else
                                {
                                    dept = dept + "," + user.F_DepartmentId;
                                }
                            }
                        }
                    }

                    entity.TaskDepartmentId = dept;
                }
                else
                {
                    entity.TaskDepartmentId = "";
                }
            }


            projectTaskIBLL.SaveEntity(entity.id, entity);
            return Success("保存成功");

        }
        public Response TaskSaveUpdate2(dynamic _)
        {
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            string jsonText = string.Empty;

            HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空
            StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
            jsonText = sr.ReadToEnd();

            //ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            ProjectTaskEntity entity = jsonText.ToObject<ProjectTaskEntity>();
            if (entity.ReportApprover == null || entity.ReportApprover == "")
            {
                return Fail("报告审核人不能为空");
            }
            if (entity.ReportSubject == null)
            {
                return Fail("报告主体不能为空");
            }
            if (entity.ProjectId != null)
            {
                if (!string.IsNullOrEmpty(entity.ProjectResponsible))
                {
                    string[] userIds = entity.ProjectResponsible.Split(',');
                    string dept = "";
                    for (var i = 0; i < userIds.Length; i++)
                    {
                        var user = userIBLL.GetEntityByUserId(userIds[i]);
                        if (user != null)
                        {
                            if (!string.IsNullOrEmpty(user.F_DepartmentId))
                            {
                                if (string.IsNullOrEmpty(dept))
                                {
                                    dept = user.F_DepartmentId;
                                }
                                else
                                {
                                    dept = dept + "," + user.F_DepartmentId;
                                }
                            }
                        }
                    }

                    entity.DepartmentId = dept;
                }
                else
                {
                    entity.DepartmentId = "";
                }
                if (!string.IsNullOrEmpty(entity.Inspector))
                {
                    string[] userIds = entity.Inspector.Split(',');
                    string dept = "";
                    for (var i = 0; i < userIds.Length; i++)
                    {
                        var user = userIBLL.GetEntityByUserId(userIds[i]);
                        if (user != null)
                        {
                            if (!string.IsNullOrEmpty(user.F_DepartmentId))
                            {
                                if (string.IsNullOrEmpty(dept))
                                {
                                    dept = user.F_DepartmentId;
                                }
                                else
                                {
                                    dept = dept + "," + user.F_DepartmentId;
                                }
                            }
                        }
                    }

                    entity.TaskDepartmentId = dept;
                }
                else
                {
                    entity.TaskDepartmentId = "";
                }
            }
            projectTaskIBLL.SaveEntity(entity.id, entity);
            return Success("保存成功");

        }
        /// <summary>
        /// 更新task的报告编号
        /// <param name="_"></param>
        /// <summary>
        /// <returns></returns>
        public Response UpdateProjectTaskNo(dynamic _)
        {
            ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            var entity = parameter.strEntity.ToObject<ProjectTaskEntity>();
            var projectTask = projectTaskIBLL.GetProjectTaskEntity(entity.id);
            var project = projectIBLL.GetProjectEntity(projectTask.ProjectId);
            var userInfo = userIBLL.GetEntityByUserId(projectTask.ProjectResponsible);
            var dept = departmentIBLL.GetEntity(userInfo.F_DepartmentId);
            string year = DateTime.Now.ToString("yy");
            if (project.ProjectSource == "3" || dept.HZ_DepartmentId == 1)
            {
                projectTask.ProjectTaskNo = "H" + year + "TW";
            }
            else
            {
                //房屋检测站
                if (userInfo.F_DepartmentId == "230062b2-04c9-4214-ba85-b943c7a5574f")
                {
                    projectTask.ProjectTaskNo = "H" + year + "FJ";
                }
                //桥隧检测部
                else if (userInfo.F_DepartmentId == "80282924-2ba9-45b9-8315-0b4a1607f4b8")
                {
                    projectTask.ProjectTaskNo = "H" + year + "QS";
                }
                //通际公司
                else if (userInfo.F_DepartmentId == "1b666e23-78b0-4f43-b8e8-9565602455f3")
                {
                    projectTask.ProjectTaskNo = "H" + year + "TG";
                }
                //南京公司
                else if (userInfo.F_DepartmentId == "b818c95b-88e8-442d-abed-bc463b8794af")
                {
                    projectTask.ProjectTaskNo = "H" + year + "NJ";
                }
                //武汉公司
                else if (userInfo.F_DepartmentId == "81213b3a-1010-43e0-877b-00985c9d470b")
                {
                    projectTask.ProjectTaskNo = "H" + year + "WH";
                }
                //西安公司
                else if (userInfo.F_DepartmentId == "b8574c5a-1d11-4fc6-8911-2d8103fc470e")
                {
                    projectTask.ProjectTaskNo = "H" + year + "XA";
                }
                //天津公司
                else if (userInfo.F_DepartmentId == "7519724c-dbae-4ba9-bff4-00da3840e7ec")
                {
                    projectTask.ProjectTaskNo = "H" + year + "TJ";
                }
                //华中公司
                else if (userInfo.F_DepartmentId == "1bc1cc84-68f4-41b3-b78f-bef7cc0ef8c9")
                {
                    projectTask.ProjectTaskNo = "H" + year + "HZ";
                }
                //广州公司
                else if (userInfo.F_DepartmentId == "3e5f3746-13ec-4aea-b35f-beea192a7a0b")
                {
                    projectTask.ProjectTaskNo = "H" + year + "GZ";
                }
                //苏州公司
                else if (userInfo.F_DepartmentId == "343d89b0-27d0-4268-9d23-06d0f6ef314b")
                {
                    projectTask.ProjectTaskNo = "H" + year + "SZ";
                }
                //合肥公司
                else if (userInfo.F_DepartmentId == "ca3b8faf-9ba1-4473-aef7-0547fdbbc419")
                {
                    projectTask.ProjectTaskNo = "H" + year + "HF";
                }
                //杨浦公司
                else if (userInfo.F_DepartmentId == "95115eca-c380-425a-be11-3428a11612ca")
                {
                    projectTask.ProjectTaskNo = "H" + year + "YP";
                }
                //海外公司
                else if (userInfo.F_DepartmentId == "fac629e63b564dfc968fbbc707fb5730")
                {
                    projectTask.ProjectTaskNo = "H" + year + "HW";
                }
                else
                {
                    projectTask.ProjectTaskNo = "H" + year + "QT";
                }
            }
            projectTask.ProjectTaskNo = projectTaskIBLL.GetNextProjectTaskNo(projectTask.ProjectTaskNo);
            projectTaskIBLL.SaveEntity(projectTask.id, projectTask);
            return Success(projectTask.ProjectTaskNo, "保存成功！");
        }
        #endregion
        #region 外业安排
        public Response GetProduceUserStatusInfo(dynamic _)
        {
            QueryUserStatusReq statusInfo = this.GetReqData<QueryUserStatusReq>();
            //1.判断传入的数据是否为空
            if (statusInfo.StartTime.Equals(""))
            {
                DateTime dt = DateTime.Now;
                statusInfo.StartTime = dt.Date.ToString();
            }
            if (statusInfo.days.Equals(""))
            {
                statusInfo.days = "7";
            }
            //2.赋值
            DateTime StartTime = Convert.ToDateTime(statusInfo.StartTime);
            int days = Convert.ToInt32(statusInfo.days);
            //3.查（用户信息）
            List<UserEntity> produceUserList = userIBLL.GetProduceUserList();
            //5.定义一个list来接收名字和状态
            List<UserTaskInfo> userTaskInfos = new List<UserTaskInfo>();
            //6定义一个list来接收时间
            List<UserTimeInfo> userTimeInfos = new List<UserTimeInfo>();
            List<UserTimeYear> userTimeYears = new List<UserTimeYear>();
            int d = days;
            //8.查时间
            for (var j = 0; j < d; j++)
            {
                //7.定义一个对象来存时间
                UserTimeInfo time = new UserTimeInfo();
                UserTimeYear timeYear = new UserTimeYear();
                DateTime startTime = StartTime.AddDays(j);
                string YMd = startTime.ToString("yy-MM-dd HH:mm:ss");
                string Md = startTime.ToString("MM-dd");
                timeYear.DateTimeYear = YMd;
                time.StatusDateTime = Md;
                userTimeYears.Add(timeYear);
                userTimeInfos.Add(time);
            }
            //9.查人员和状态
            foreach (var userItem in produceUserList)
            {
                //10.定义一个对象来放人员和状态集合
                UserTaskInfo userTaskInfo = new UserTaskInfo();
                //11.定义一个list来放状态
                List<UserStatusInfo> userStatusInfos = new List<UserStatusInfo>();
                //根据人员id，开始时间，结束时间到计划时间和实际时间查是否符合条件
                //开始时间,days
                for (var i = 0; i < days; i++)
                {
                    DateTime startTime = StartTime.AddDays(i);
                    DateTime endTime = StartTime.AddDays(i + 1);
                    //计算
                    //计划时间
                    var planInfoList = projectTaskIBLL.GetProjectTaskByInspectorAndPlanTime(userItem.F_UserId, startTime, endTime);
                    //实际时间
                    var ActualList = projectTaskIBLL.GetProjectTaskByInspectorAndActualTime(userItem.F_UserId, startTime, endTime);
                    UserStatusInfo userStatusInfo = new UserStatusInfo();
                    if (planInfoList.Count > 0 && ActualList.Count <= 0)
                    {
                        userStatusInfo.Status = 1;//计划忙
                    }
                    else if (ActualList.Count > 0)
                    {
                        userStatusInfo.Status = 2;//进场
                    }
                    else
                    {
                        userStatusInfo.Status = 0;//空闲
                    }
                    userStatusInfos.Add(userStatusInfo);


                }
                userTaskInfo.UserId = userItem.F_UserId;
                userTaskInfo.UserName = userItem.F_RealName;
                userTaskInfo.UserStatusInfos = userStatusInfos;
                userTaskInfos.Add(userTaskInfo);
            }
            var jsonData = new
            {
                yearTime = userTimeYears,
                time = userTimeInfos,
                rows = userTaskInfos
            };
            return Success(jsonData);
        }
        public Response GetProjectName(dynamic _)
        {
            ReqProjectName data = this.GetReqData<ReqProjectName>();
            //用户id
            string username = data.userId;
            //时间
            DateTime usertime = Convert.ToDateTime(data.time);
            var projectNameDate = projectTaskIBLL.GetProjectNameApi(username, usertime);

            var jsonData = new
            {
                ProjectTask = projectNameDate,
            };
            return Success(jsonData);
        }
        #endregion
        #region 用工接口

        public Response GetRecruitPageList(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = projectRecruitIBLL.GetPageList(parameter.pagination, parameter.queryJson);
            List<ProjectRecruitVo> list = new List<ProjectRecruitVo>();
            foreach (var info in data)
            {
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");

                //申请人
                var applyPerson = userIBLL.GetEntityByUserId(info.ApplyPerson);
                if (applyPerson != null)
                {
                    info.ApplyPersonName = applyPerson.F_RealName;
                }
                /*   //合同编号
                   List<ProjectContractEntity> projectContracts = projectContractIBLL.GetProjectContractByProjectId(info.ProjectId);
                   if (projectContracts.Count > 0)
                   {
                       info.ContractNo = projectContracts.FirstOrDefault().ContractNo;
                   }*/
                //支付类型
                var paymentMethod = dataItemBLL.GetDetailItemName(info.PaymentMethod, "PaymentMethod");
                if (paymentMethod != null)
                {
                    info.PaymentMethodName = paymentMethod.F_ItemName;
                }

                var recruitStatus = dataItemBLL.GetDetailItemName(info.RecruitStatus, "RecruitStatus");
                if (recruitStatus != null)
                {
                    info.RecruitStatusName = recruitStatus.F_ItemName;
                }
                list.Add(info);
            }



            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 新增/编辑
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response RecruitSaveForm(dynamic _)
        {
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            string jsonText = string.Empty;

            HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空

            StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
            jsonText = sr.ReadToEnd();

            //ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            ProjectRecruitEntity entity = jsonText.ToObject<ProjectRecruitEntity>();
            if (entity.ProjectId == null || entity.ProjectId == "")
            {
                return Fail("项目名称不能为空");
            }
            projectRecruitIBLL.SaveEntity(entity.id, entity);
            return Success("保存成功");
        }
        /// <summary>
        /// 获取表单数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>

        public Response GetRecruitById(dynamic _)
        {
            string keyValue = this.GetReqData();
            var ProjectRecruitData = projectRecruitIBLL.GetPreviewProjectRecruit(keyValue);
            var applyPerson = userIBLL.GetFollowPersonNameByUserId(ProjectRecruitData.ApplyPerson);
            var paymentMethod = dataItemBLL.GetDetailItemName(ProjectRecruitData.PaymentMethod, "PaymentMethod");

            if (applyPerson != null)
            {
                ProjectRecruitData.ApplyPersonName = applyPerson.F_RealName;

            }
            if (paymentMethod != null)
            {
                ProjectRecruitData.PaymentMethod = paymentMethod.F_ItemName;
            }
            var jsonData = new
            {
                ProjectRecruit = ProjectRecruitData,
            };
            return Success(jsonData);
        }
        public Response GetRecruitById2(dynamic _)
        {
            string keyValue = this.GetReqData();
            var ProjectRecruitData = projectRecruitIBLL.GetPreviewProjectRecruit(keyValue);
            var applyPerson = userIBLL.GetFollowPersonNameByUserId(ProjectRecruitData.ApplyPerson);
            var paymentMethod = dataItemBLL.GetDetailItemName(ProjectRecruitData.PaymentMethod, "PaymentMethod");

            if (applyPerson != null)
            {
                ProjectRecruitData.ApplyPersonName = applyPerson.F_RealName;

            }
            if (paymentMethod != null)
            {
                ProjectRecruitData.PaymentMethodName = paymentMethod.F_ItemName;
            }

            return Success(ProjectRecruitData);
        }

        public Response GetRecruitByProcessId(dynamic _)
        {
            string keyValue = this.GetReqData();
            var ProjectRecruitData = projectRecruitIBLL.GetRecruitByProcessId(keyValue);
            /* var applyPerson = userIBLL.GetFollowPersonNameByUserId(ProjectRecruitData.ApplyPerson);
             var paymentMethod = dataItemBLL.GetDetailItemName(ProjectRecruitData.PaymentMethod, "PaymentMethod");
             var recruitStatus = dataItemBLL.GetDetailItemName(ProjectRecruitData.RecruitStatus, "RecruitStatus");
             var projectSource = dataItemBLL.GetDetailItemName(ProjectRecruitData.ProjectSource, "ProjectSource");
             if (projectSource != null)
             {
                 ProjectRecruitData.ProjectSource = recruitStatus.F_ItemName;
             }
             if (recruitStatus != null)
             {
                 ProjectRecruitData.RecruitStatus = recruitStatus.F_ItemName;
             }

             if (applyPerson != null)
             {
                 ProjectRecruitData.ApplyPerson = applyPerson.F_RealName;

             }
             if (paymentMethod != null)
             {
                 ProjectRecruitData.PaymentMethod = paymentMethod.F_ItemName;
             }*/
            var applyPerson = userIBLL.GetFollowPersonNameByUserId(ProjectRecruitData.ApplyPerson);
            var paymentMethod = dataItemBLL.GetDetailItemName(ProjectRecruitData.PaymentMethod, "PaymentMethod");

            if (applyPerson != null)
            {
                ProjectRecruitData.ApplyPersonName = applyPerson.F_RealName;

            }
            if (paymentMethod != null)
            {
                ProjectRecruitData.PaymentMethodName = paymentMethod.F_ItemName;
            }

            var jsonData = new
            {
                ProjectRecruit = ProjectRecruitData,
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 提审
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetRecruitSubmitter(dynamic _)
        {
            string keyValue = this.GetReqData();
            var entity = projectRecruitIBLL.GetPreviewProjectRecruit(keyValue);
            //ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            //ProjectRecruitEntity entity = parameter.strEntity.ToObject<ProjectRecruitEntity>();
            if (entity.RecruitStatus.ToInt() == 11 && entity.RecruitStatus != null)
            {

                projectRecruitIBLL.UpdateFlowId(entity.id, entity.WorkFlowId);
                UserInfo userInfo = LoginUserInfo.Get();
                nWFProcessIBLL.AgainCreateFlow(entity.WorkFlowId, userInfo, "");
            }
            else
            {
                projectRecruitIBLL.UpdateFlowId(entity.id, entity.WorkFlowId);
            }

            return Success("提审成功！");
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetRecruitDelete2(dynamic _)
        {
            string keyValue = this.GetReqData();
            projectRecruitIBLL.DeleteEntity(keyValue);
            return Success("删除成功");
        }

        #endregion

        #region 发票管理
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetProjectBillingAll(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = projectBillingIBLL.GetPageList(parameter.queryJson);
            List<ProjectBillingVo> list = new List<ProjectBillingVo>();
            foreach (var info in data)
            {
                if (string.IsNullOrEmpty(info.ProjectName))
                {
                    var projectData = projectIBLL.GetProjectEntity(info.ProjectId);
                    if (projectData != null)
                    {
                        info.ProjectName = projectData.ProjectName;
                    }
                }
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                List<ProjectContractEntity> projectContracts = projectContractIBLL.GetProjectContractByProjectId(info.ProjectId);
                if (projectContracts.Count > 0)
                {
                    info.ContractNo = projectContracts.FirstOrDefault().ContractNo;
                }
                //开票内容
                var billingContent = dataItemBLL.GetDetailItemName(info.BillingContent, "BillingContent");
                if (billingContent != null)
                {
                    info.BillingContentName = billingContent.F_ItemName;
                }
                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                if (followPerson != null)
                {
                    info.FollowPerson = followPerson.F_RealName;
                }
                //营销部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }
                //开票类型
                var billingType = dataItemBLL.GetDetailItemName(info.BillingType, "BillingType");
                if (billingType != null)
                {
                    info.BillingTypeName = billingType.F_ItemName;
                }
                //开票状态
                var billingStatus = dataItemBLL.GetDetailItemName(info.BillingStatus, "BillingStatus");
                if (billingStatus != null)
                {
                    info.BillingStatusName = billingStatus.F_ItemName;
                }
                //开票单位
                var billingUnit = dataItemBLL.GetDetailItemName(info.BillingUnit, "ContractSubject");
                if (billingUnit != null)
                {
                    info.BillingUnitName = billingUnit.F_ItemName;
                }
                list.Add(info);
            }
            list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);


        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetProjectPayCollectionAll(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = projectPayCollectionIBLL.GetPageListSUM(parameter.queryJson);
            List<ProjectPayCollectionVo> list = new List<ProjectPayCollectionVo>();

            foreach (var info in data)
            {
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //到款时间
                DateTime time1 = (DateTime)info.ReceiptDate;
                info.ReceiptDateMd = time1.ToString("yyyy-MM-dd");
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }
                //营销部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }
                //操作人
                var createUser = userIBLL.GetEntityByUserId(info.CreateUser);
                if (createUser != null)
                {
                    info.CreateUserName = createUser.F_RealName;
                }
                if (info.ProjectSource.ToInt() == 1)
                {
                    if (info.PaymentAmount == null)
                    {
                        info.FollowPersonAmount = info.Amount * (decimal?)0.02;
                        info.PayCollectionAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                    }
                    else if (info.PaymentAmount < info.Amount * (decimal?)0.3)
                    {
                        info.FollowPersonAmount = info.Amount * (decimal?)0.005;
                        info.PayCollectionAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                    }
                    else
                    {
                        info.FollowPersonAmount = info.ContractAmount * (decimal?)0.02;
                        info.PayCollectionAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                    }
                }
                else if (info.ProjectSource.ToInt() == 2)
                {
                    if (info.PaymentAmount == null)
                    {
                        info.FollowPersonAmount = info.Amount * (decimal?)0.015;
                        info.PayCollectionAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();


                    }
                }
                else if (info.PaymentAmount < info.Amount * (decimal?)0.3)
                {
                    info.FollowPersonAmount = info.Amount * (decimal?)0.002;
                    info.PayCollectionAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                }
                else
                {
                    info.FollowPersonAmount = info.Amount * (decimal?)0.001;
                    info.PayCollectionAmount1 = Math.Round((double)info.FollowPersonAmount, 2).ToString();
                }
                list.Add(info);
            }
            list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);


        }
        public Response GetBillingPageList(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = projectBillingIBLL.GetPageList(parameter.pagination, parameter.queryJson);
            List<ProjectBillingVo> list = new List<ProjectBillingVo>();
            foreach (var info in data)
            {
                if (string.IsNullOrEmpty(info.ProjectName))
                {
                    var projectData = projectIBLL.GetProjectEntity(info.ProjectId);
                    if (projectData != null)
                    {
                        info.ProjectName = projectData.ProjectName;
                    }
                }
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");

                //开票内容
                var billingContent = dataItemBLL.GetDetailItemName(info.BillingContent, "BillingContent");
                if (billingContent != null)
                {
                    info.BillingContentName = billingContent.F_ItemName;
                }
                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                if (followPerson != null)
                {
                    info.FollowPerson = followPerson.F_RealName;
                }
                //营销部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }
                //开票类型
                var billingType = dataItemBLL.GetDetailItemName(info.BillingType, "BillingType");
                if (billingType != null)
                {
                    info.BillingTypeName = billingType.F_ItemName;
                }
                //开票状态
                var billingStatus = dataItemBLL.GetDetailItemName(info.BillingStatus, "BillingStatus");
                if (billingStatus != null)
                {
                    info.BillingStatusName = billingStatus.F_ItemName;
                }
                //开票单位
                var billingUnit = dataItemBLL.GetDetailItemName(info.BillingUnit, "ContractSubject");
                if (billingUnit != null)
                {
                    info.BillingUnitName = billingUnit.F_ItemName;
                }
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }

                list.Add(info);
            }

            var jsonData = new
            {
                rows = data,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };
            return Success(jsonData);
        }
        //开票合计
        public Response GetBillingSum(dynamic _)
        {

            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = projectBillingIBLL.GetPageList(parameter.queryJson);
            decimal? BillingAmountSUM = 0;
            foreach (var item in data)
            {
                BillingAmountSUM = BillingAmountSUM + item.BillingAmount;
            }


            var result = new
            {
                BillingAmountSum = BillingAmountSUM,

            };
            var jsonData = new
            {
                rows = result
            };
            return Success(jsonData);
        }
        //提审
        public Response GetBillingSubmitter(dynamic _)
        {
            /*ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            ProjectBillingEntity entity = parameter.strEntity.ToObject<ProjectBillingEntity>();*/
            string keyValue = this.GetReqData();

            var data = projectBillingIBLL.GetPriewFormBilling(keyValue);
            if (data.BillingStatus.ToInt() == 11 && data.WorkFlowId != null)
            {

                projectBillingIBLL.UpdateFlowId(data.Id, data.WorkFlowId);
                UserInfo userInfo = LoginUserInfo.Get();
                nWFProcessIBLL.AgainCreateFlow(data.WorkFlowId, userInfo, "");
            }
            else
            {
                projectBillingIBLL.UpdateFlowId(data.Id, data.WorkFlowId);
            }

            return Success("提审成功！");
        }
        public Response GetBillingById(dynamic _)
        {
            string keyValue = this.GetReqData();

            var data = projectBillingIBLL.GetPriewFormBilling(keyValue);

            var billingType = dataItemBLL.GetDetailItemName(data.BillingType, "BillingType");
            var billingContent = dataItemBLL.GetDetailItemName(data.BillingContent, "BillingContent");
            var billingUnit = companyIBLL.GetBillingUnitName(data.BillingUnit);

            if (billingType != null)
            {
                data.BillingTypeName = billingType.F_ItemName;
            }
            if (billingContent != null)
            {
                data.BillingContentName = billingContent.F_ItemName;
            }
            if (billingUnit != null)
            {
                data.BillingUnitName = billingUnit.F_ShortName;
            }
            if (billingContent != null)
            {
                data.BillingTypeName = billingType.F_ItemName;
            }
            if (billingUnit != null)
            {
                data.BillingContentName = billingContent.F_ItemName;

            }

            if (billingType != null)
            {
                data.BillingContentName = billingType.F_ItemName;
            }

            var jsonData = new
            {
                ProjectBilling = data,
            };
            return Success(jsonData);
        }
        public Response GetBillingById2(dynamic _)
        {
            string keyValue = this.GetReqData();

            var data = projectBillingIBLL.GetPriewFormBilling(keyValue);

            var billingType = dataItemBLL.GetDetailItemName(data.BillingType, "BillingType");
            var billingContent = dataItemBLL.GetDetailItemName(data.BillingContent, "BillingContent");
            //var billingUnit = companyIBLL.GetBillingUnitName(data.BillingUnit);
            //开票类型
            if (billingType != null)
            {
                data.BillingTypeName = billingType.F_ItemName;
            }
            //开票内容
            if (billingContent != null)
            {
                data.BillingContentName = billingContent.F_ItemName;
            }
            //开票单位
            var billingUnit = dataItemBLL.GetDetailItemName(data.BillingUnit, "ContractSubject");
            if (billingUnit != null)
            {
                data.BillingUnitName = billingUnit.F_ItemName;
            }
            //项目来源
            var projectSource = dataItemBLL.GetDetailItemName(data.ProjectSource, "ProjectSource");
            if (projectSource != null)
            {
                data.ProjectSourceName = projectSource.F_ItemName;
            }



            return Success(data);
        }



        public Response GetBillingByProcessId(dynamic _)
        {
            string keyValue = this.GetReqData();
            var ProjectBillingData = projectBillingIBLL.GetBillingByProcessId(keyValue);


            /*   var billingType = dataItemBLL.GetDetailItemName(ProjectBillingData.BillingType, "BillingType");
               var billingContent = dataItemBLL.GetDetailItemName(ProjectBillingData.BillingContent, "BillingContent");
               var billingUnit = companyIBLL.GetBillingUnitName(ProjectBillingData.BillingUnit);
               var billingStatus = dataItemBLL.GetDetailItemName(ProjectBillingData.BillingStatus, "BillingStatus");

               if (billingType != null)
               {
                   ProjectBillingData.BillingType = billingType.F_ItemName;
               }
               if (billingContent != null)
               {
                   ProjectBillingData.BillingContent = billingContent.F_ItemName;
               }
               if (billingUnit != null)
               {
                   ProjectBillingData.BillingUnit = billingUnit.F_ShortName;
               }
               if (billingStatus != null)
               {
                   ProjectBillingData.BillingStatus = billingStatus.F_ItemName;

               }*/
            var billingType = dataItemBLL.GetDetailItemName(ProjectBillingData.BillingType, "BillingType");
            var billingContent = dataItemBLL.GetDetailItemName(ProjectBillingData.BillingContent, "BillingContent");
            // var billingUnit = companyIBLL.GetBillingUnitName(ProjectBillingData.BillingUnit);
            //开票类型
            if (billingType != null)
            {
                ProjectBillingData.BillingTypeName = billingType.F_ItemName;
            }
            //开票内容
            if (billingContent != null)
            {
                ProjectBillingData.BillingContentName = billingContent.F_ItemName;
            }

            //开票单位
            var billingUnit = dataItemBLL.GetDetailItemName(ProjectBillingData.BillingUnit, "ContractSubject");
            if (billingUnit != null)
            {
                ProjectBillingData.BillingUnitName = billingUnit.F_ItemName;
            }
            /* if (billingUnit != null)
             {
                 ProjectBillingData.BillingUnitName = billingUnit.F_ShortName;
             }*/
            //项目来源
            var projectSource = dataItemBLL.GetDetailItemName(ProjectBillingData.ProjectSource, "ProjectSource");
            if (projectSource != null)
            {
                ProjectBillingData.ProjectSourceName = projectSource.F_ItemName;
            }
            var jsonData = new
            {
                ProjectBilling = ProjectBillingData,
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 开票的新增和修改
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response BillingSave(dynamic _)
        {
            ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            ProjectBillingEntity entity = parameter.strEntity.ToObject<ProjectBillingEntity>();
            projectBillingIBLL.SaveEntity(parameter.keyValue, entity);
            return Success("保存成功！");
        }
        /// <summary>
        /// 开票的新增和修改
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response BillingSave2(dynamic _)
        {
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            string jsonText = string.Empty;

            HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空
            StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
            jsonText = sr.ReadToEnd();
            ProjectBillingEntity entity = jsonText.ToObject<ProjectBillingEntity>();
            if (entity.ProjectId == null || entity.ProjectId == "")
            {
                return Fail("项目名称不能为空");
            }
            projectBillingIBLL.SaveEntity(entity.Id, entity);
            return Success("保存成功！");
        }
        public Response BillingZuofei(dynamic _)
        {
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            string jsonText = string.Empty;

            HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空
            StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
            jsonText = sr.ReadToEnd();
            ProjectBillingEntity entity1 = jsonText.ToObject<ProjectBillingEntity>();
            ProjectBillingEntity entity = projectBillingIBLL.GetProjectBillingEntity(entity1.Id);
            entity.CancelTheReason = entity1.CancelTheReason;
            entity.BillingStatus = 8;
            if (entity.WorkFlowId != null)
            {
                var processEntity = nWFProcessIBLL.GetEntity(entity.WorkFlowId);
                if (processEntity.F_IsFinished != 1)
                {
                    NWFProcessEntity n = new NWFProcessEntity();
                    n.F_Id = entity.WorkFlowId;
                    n.F_IsFinished = 1;
                    nWFProcessIBLL.GetEntityByWorkFlowId(entity.WorkFlowId, n);
                }
            }
            projectBillingIBLL.SaveEntity(entity1.Id, entity);
            return Success("保存成功！");
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetBillingDelete2(dynamic _)
        {
            string keyValue = this.GetReqData();
            projectBillingIBLL.DeleteEntity(keyValue);
            return Success("删除成功");
        }
        #endregion

        #region 付款接口

        public Response GetPaymentPageList(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = projectPaymentIBLL.GetPageList(parameter.pagination, parameter.queryJson);
            List<ProjectPaymentVo> list = new List<ProjectPaymentVo>();

            foreach (var info in data)
            {
                if (string.IsNullOrEmpty(info.ProjectName))
                {
                    var projectData = projectIBLL.GetProjectEntity(info.ProjectId);
                    if (projectData != null)
                    {
                        info.ProjectName = projectData.ProjectName;
                    }
                }
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //项目来源
                DataItemDetailEntity projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }
                //创建人
                var createUser = userIBLL.GetFollowPersonNameByUserId(info.CreateUser);
                if (createUser != null)
                {
                    info.CreateUserName = createUser.F_RealName;
                }
                //付款类型
                var payType = dataItemBLL.GetDetailItemName(info.PayType, "PayType");
                if (payType != null)
                {
                    info.PayTypeName = payType.F_ItemName;
                }
                var projectInfo = projectIBLL.GetProjectEntity(info.ProjectId);
                if (projectInfo != null)
                {
                    if (projectInfo.ProjectStatus == "3" || projectInfo.TenderFlg == "1")
                    {
                        var followPerson = userIBLL.GetEntityByUserId(projectInfo.FollowPerson);
                        if (followPerson != null)
                        {
                            info.DepartmentId = followPerson.F_DepartmentId;
                        }
                    }
                }
                //所属部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }
                ////支付状态
                //var billingStatus = dataItemBLL.GetDetailItemName(info.PaymentStatus, "PaymentStatus");
                //if (billingStatus != null)
                //{
                //    info.PaymentStatusName = billingStatus.F_ItemName;
                //}
                ////支付方式
                //var paymentMethod = dataItemBLL.GetDetailItemName(info.PaymentMethod, "Client_PaymentMode");
                //if (paymentMethod != null)
                //{
                //    info.PaymentMethodName = paymentMethod.F_ItemName;
                //}
                //支付抬头
                //var paymentHeader = dataItemBLL.GetDetailItemName(info.PaymentHeader, "PaymentHeader");
                //if (paymentHeader != null)
                //{
                //    info.PaymentHeaderName = paymentHeader.F_ItemName;
                //}
                /*   if (info.PaymentStatus.ToInt() == 11)
                   {
                       ProjectPaymentEntity entity = new ProjectPaymentEntity();
                       entity.ProjectId = info.ProjectId;
                       entity.PaymentStatus = info.PaymentStatus.ToInt();

                       projectPaymentIBLL.SaveEntity2(info.ProjectId, entity);
                   }*/
                list.Add(info);
            }

            var jsonData = new
            {
                rows = data,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };
            return Success(jsonData);
        }
        public Response GetPaymentPageList2(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = projectPaymentListIBLL.GetPageList(parameter.pagination, parameter.queryJson);
            List<ProjectPaymentVo> list = new List<ProjectPaymentVo>();


            foreach (var info in data)
            {
                ProjectPaymentVo t = new ProjectPaymentVo();
                t.WorkFlowId = info.WorkFlowId;
                t.tid = info.tid;

                if (info.WorkFlowId != null)
                {
                    var ProjectPaymentData = projectPaymentListIBLL.GetEntityByProcessId(info.WorkFlowId);
                    if (ProjectPaymentData.tid != null)
                    {
                        var DataT = projectPaymentListIBLL.GetEntityBytID(ProjectPaymentData.tid);
                        foreach (var f in DataT)
                        {
                            ProjectPaymentEntity list1 = new ProjectPaymentEntity();
                            list1.Modify(f.id);
                            list1.PaymentStatus = ProjectPaymentData.PaymentStatus.ToInt();
                            projectPaymentListIBLL.SaveEntity(f.id, list1);
                        }
                    }
                }
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //项目来源
                DataItemDetailEntity projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;

                }
                //创建人
                var createUser = userIBLL.GetFollowPersonNameByUserId(info.CreateUser);
                if (createUser != null)
                {
                    info.CreateUserName = createUser.F_RealName;
                }
                //付款类型
                var payType = dataItemBLL.GetDetailItemName(info.PayType, "PayType");
                if (payType != null)
                {
                    info.PayTypeName = payType.F_ItemName;
                }
                var projectInfo = projectIBLL.GetProjectEntity(info.ProjectId);
                if (projectInfo != null)
                {
                    if (projectInfo.ProjectStatus == "3" || projectInfo.TenderFlg == "1")
                    {
                        var followPerson = userIBLL.GetEntityByUserId(projectInfo.FollowPerson);
                        if (followPerson != null)
                        {
                            info.DepartmentId = followPerson.F_DepartmentId;
                        }
                    }
                }
                //所属部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }
                ////支付方式
                //var paymentMethod = dataItemBLL.GetDetailItemName(info.PaymentMethod, "Client_PaymentMode");
                //if (paymentMethod != null)
                //{
                //    info.PaymentMethodName = paymentMethod.F_ItemName;
                //}
                ////支付状态
                //var billingStatus = dataItemBLL.GetDetailItemName(info.PaymentStatus, "PaymentStatus");
                //if (billingStatus != null)
                //{
                //    info.PaymentStatusName = billingStatus.F_ItemName;
                //}
                //支付抬头
                //var paymentHeader = dataItemBLL.GetDetailItemName(info.PaymentHeader, "PaymentHeader");
                //if (paymentHeader != null)
                //{
                //    info.PaymentHeaderName = paymentHeader.F_ItemName;
                //}

                list.Add(info);
            }
            var jsonData = new
            {
                rows = data,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };
            return Success(jsonData);
        }
        public Response GetPaymentById(dynamic _)
        {
            string keyValue = this.GetReqData();
            var ProjectPaymentData = projectPaymentIBLL.GetPreviewProjectPayment(keyValue);

            var paymentFile = annexesFileIBLL.GetList(ProjectPaymentData.PaymentFile);
            if (paymentFile != null)
            {
                foreach (var item in paymentFile)
                {
                    item.F_FileType = item.F_FileType.ToLower();
                }
                ProjectPaymentData.annexesFileEntities = paymentFile;
            }

            var jsonData = new
            {
                ProjectPayment = ProjectPaymentData,
            };
            return Success(jsonData);
        }
        public Response GetPaymentById2(dynamic _)
        {
            string keyValue = this.GetReqData();
            var ProjectPaymentData = projectPaymentIBLL.GetPreviewProjectPayment(keyValue);
            //创建时间
            DateTime time = (DateTime)ProjectPaymentData.CreateTime;
            ProjectPaymentData.CreateTimeyMd = time.ToString("yyyy-MM-dd");
            //项目来源
            DataItemDetailEntity projectSource = dataItemBLL.GetDetailItemName(ProjectPaymentData.ProjectSource, "ProjectSource");
            if (projectSource != null)
            {
                ProjectPaymentData.ProjectSourceName = projectSource.F_ItemName;

            }
            //创建人
            var createUser = userIBLL.GetFollowPersonNameByUserId(ProjectPaymentData.CreateUser);
            if (createUser != null)
            {
                ProjectPaymentData.CreateUserName = createUser.F_RealName;
            }
            //付款类型
            var payType = dataItemBLL.GetDetailItemName(ProjectPaymentData.PayType, "PayType");
            if (payType != null)
            {
                ProjectPaymentData.PayTypeName = payType.F_ItemName;
            }

            //所属部门
            var department = departmentIBLL.GetEntity(ProjectPaymentData.DepartmentId);
            if (department != null)
            {
                ProjectPaymentData.DepartmentName = department.F_FullName;
            }
            //支付方式
            //var paymentMethod = dataItemBLL.GetDetailItemName(ProjectPaymentData.PaymentMethod, "Client_PaymentMode");
            //if (paymentMethod != null)
            //{
            //    ProjectPaymentData.PaymentMethodName = paymentMethod.F_ItemName;
            //}
            ////支付状态
            //var billingStatus = dataItemBLL.GetDetailItemName(ProjectPaymentData.PaymentStatus, "PaymentStatus");
            //if (billingStatus != null)
            //{
            //    ProjectPaymentData.PaymentStatusName = billingStatus.F_ItemName;
            //}
            //支付抬头
            //var paymentHeader = dataItemBLL.GetDetailItemName(ProjectPaymentData.PaymentHeader, "PaymentHeader");
            //if (paymentHeader != null)
            //{
            //    ProjectPaymentData.PaymentHeaderName = paymentHeader.F_ItemName;
            //}

            var paymentFile = annexesFileIBLL.GetList(ProjectPaymentData.PaymentFile);
            if (paymentFile != null)
            {
                foreach (var item in paymentFile)
                {
                    item.F_FileType = item.F_FileType.ToLower();
                }
                ProjectPaymentData.annexesFileEntities = paymentFile;
            }
            List<ProjectPaymentVo> subProjectPayments = new List<ProjectPaymentVo>();
            if (!string.IsNullOrEmpty(ProjectPaymentData.tid))
            {
                subProjectPayments = projectPaymentIBLL.GetProjectPaymentByTid(ProjectPaymentData.tid);
                if (subProjectPayments.Count > 0)
                {
                    foreach (var item in subProjectPayments)
                    {
                        //创建时间
                        item.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                        //项目来源
                        DataItemDetailEntity projectSourceItem = dataItemBLL.GetDetailItemName(item.ProjectSource, "ProjectSource");
                        if (projectSourceItem != null)
                        {
                            item.ProjectSourceName = projectSourceItem.F_ItemName;

                        }
                        //创建人
                        var createUserItem = userIBLL.GetFollowPersonNameByUserId(ProjectPaymentData.CreateUser);
                        if (createUserItem != null)
                        {
                            item.CreateUserName = createUserItem.F_RealName;
                        }
                        //付款类型
                        var payTypeItem = dataItemBLL.GetDetailItemName(item.PayType, "PayType");
                        if (payTypeItem != null)
                        {
                            item.PayTypeName = payTypeItem.F_ItemName;
                        }

                        //所属部门
                        var departmentItem = departmentIBLL.GetEntity(item.DepartmentId);
                        if (departmentItem != null)
                        {
                            item.DepartmentName = departmentItem.F_FullName;
                        }
                        //支付方式
                        var paymentMethodItem = dataItemBLL.GetDetailItemName(item.PaymentMethod, "Client_PaymentMode");
                        if (paymentMethodItem != null)
                        {
                            item.PaymentMethodName = paymentMethodItem.F_ItemName;
                        }
                        //支付状态
                        var billingStatusItem = dataItemBLL.GetDetailItemName(item.PaymentStatus, "PaymentStatus");
                        if (billingStatusItem != null)
                        {
                            item.PaymentStatusName = billingStatusItem.F_ItemName;
                        }
                    }
                }
            }
            var jsonData = new
            {
                ProjectPayment = ProjectPaymentData,
                SubProjectPaymentList = subProjectPayments
            };
            return Success(jsonData);
        }


        public Response GetPaymentByProcessId(dynamic _)
        {
            string keyValue = this.GetReqData();
            var ProjectPaymentData = projectPaymentIBLL.GetEntityByProcessId(keyValue);

            /* var paymentFile = annexesFileIBLL.GetList(ProjectPaymentData.PaymentFile);

             if (paymentFile != null)
             {
                 ProjectPaymentData.annexesFileEntities = paymentFile;
             }*/
            /*   var paymentFile = annexesFileIBLL.GetList(ProjectPaymentData.PaymentFile);
               if (paymentFile != null)
               {
                   foreach (var item in paymentFile)
                   {
                       item.F_FileType = item.F_FileType.ToLower();
                   }
                   ProjectPaymentData.annexesFileEntities = paymentFile;
               }
               var projectId = projectManageIBLL.GetProjectEntity(ProjectPaymentData.ProjectId);
               var payType = dataItemBLL.GetDetailItemName(ProjectPaymentData.PayType, "PayType");
               var paymentHeader = dataItemBLL.GetDetailItemName(ProjectPaymentData.PaymentHeader, "PaymentHeader");
               var paymentMethod = dataItemBLL.GetDetailItemName(ProjectPaymentData.PaymentMethod, "Client_PaymentMode");
               var projectSource = dataItemBLL.GetDetailItemName(ProjectPaymentData.ProjectSource, "ProjectSource");
               var paymentStatus = dataItemBLL.GetDetailItemName(ProjectPaymentData.PaymentStatus, "PaymentStatus");
               if (paymentStatus != null)
               {
                   ProjectPaymentData.PaymentStatus = paymentStatus.F_ItemName;
               }
               *//* if (projectSource != null)
                {
                    ProjectPaymentData.ProjectSource = projectSource.F_ItemName;
                }*//*
               if (projectId != null)
               {
                   ProjectPaymentData.ProjectId = projectId.ProjectName;
               }
               if (payType != null)
               {
                   ProjectPaymentData.PayType = payType.F_ItemName;
               }
               if (paymentHeader != null)
               {
                   ProjectPaymentData.PaymentHeader = paymentHeader.F_ItemName;
               }
               if (paymentMethod != null)
               {
                   ProjectPaymentData.PaymentMethod = paymentMethod.F_ItemName;
               }*/
            //创建时间
            DateTime time = (DateTime)ProjectPaymentData.CreateTime;
            ProjectPaymentData.CreateTimeyMd = time.ToString("yyyy-MM-dd");
            //项目来源
            DataItemDetailEntity projectSource = dataItemBLL.GetDetailItemName(ProjectPaymentData.ProjectSource, "ProjectSource");
            if (projectSource != null)
            {
                ProjectPaymentData.ProjectSourceName = projectSource.F_ItemName;

            }
            //创建人
            var createUser = userIBLL.GetFollowPersonNameByUserId(ProjectPaymentData.CreateUser);
            if (createUser != null)
            {
                ProjectPaymentData.CreateUserName = createUser.F_RealName;
            }
            //付款类型
            var payType = dataItemBLL.GetDetailItemName(ProjectPaymentData.PayType, "PayType");
            if (payType != null)
            {
                ProjectPaymentData.PayTypeName = payType.F_ItemName;
            }

            //所属部门
            var department = departmentIBLL.GetEntity(ProjectPaymentData.DepartmentId);
            if (department != null)
            {
                ProjectPaymentData.DepartmentName = department.F_FullName;
            }
            //支付方式
            var paymentMethod = dataItemBLL.GetDetailItemName(ProjectPaymentData.PaymentMethod, "Client_PaymentMode");
            if (paymentMethod != null)
            {
                ProjectPaymentData.PaymentMethodName = paymentMethod.F_ItemName;
            }
            //支付状态
            var billingStatus = dataItemBLL.GetDetailItemName(ProjectPaymentData.PaymentStatus, "PaymentStatus");
            if (billingStatus != null)
            {
                ProjectPaymentData.PaymentStatusName = billingStatus.F_ItemName;
            }
            //支付抬头
            //var paymentHeader = dataItemBLL.GetDetailItemName(ProjectPaymentData.PaymentHeader, "PaymentHeader");
            //if (paymentHeader != null)
            //{
            //    ProjectPaymentData.PaymentHeaderName = paymentHeader.F_ItemName;
            //}
            //批量付款总金额
            if (!string.IsNullOrEmpty(ProjectPaymentData.tid))
            {
                var AmountSum = projectPaymentIBLL.GetAmountSumByTid(ProjectPaymentData.tid);
                if (AmountSum.PaymentAmountsum != null)
                {
                    ProjectPaymentData.PaymentAmountsum = AmountSum.PaymentAmountsum;
                }
            }

            var paymentFile = annexesFileIBLL.GetList(ProjectPaymentData.PaymentFile);
            if (paymentFile != null)
            {
                foreach (var item in paymentFile)
                {
                    item.F_FileType = item.F_FileType.ToLower();
                }
                ProjectPaymentData.annexesFileEntities = paymentFile;
            }
            var jsonData = new
            {
                ProjectPayment = ProjectPaymentData,
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 付款的新增和修改
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response PaymentSave(dynamic _)
        {
            ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            ProjectPaymentEntity entity = parameter.strEntity.ToObject<ProjectPaymentEntity>();
            projectPaymentIBLL.SaveEntity(parameter.keyValue, entity);
            return Success("保存成功！");
        }
        public Response PaymentSave2(dynamic _)
        {
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            string jsonText = string.Empty;

            HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空

            StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
            jsonText = sr.ReadToEnd();

            //ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            ProjectPaymentEntity entity = jsonText.ToObject<ProjectPaymentEntity>();
            projectPaymentIBLL.SaveEntity(entity.id, entity);
            return Success("保存成功！");
        }
        public Response PaymentUpdateList2(dynamic _)
        {
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            string jsonText = string.Empty;

            HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空

            StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
            jsonText = sr.ReadToEnd();

            //ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            ProjectPaymentEntity entity = jsonText.ToObject<ProjectPaymentEntity>();
            projectPaymentListIBLL.SaveEntity(entity.id, entity);
            return Success("保存成功！");
        }
        public Response GetPaymentSubmitterList2(dynamic _)
        {
            string keyValue = this.GetReqData();
            var entity = projectPaymentListIBLL.GetPreviewProjectPayment(keyValue);
            //ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            //ProjectPaymentEntity entity = parameter.strEntity.ToObject<ProjectPaymentEntity>();
            if (entity.PaymentStatus.ToInt() == 11 && entity.WorkFlowId != null)
            {
                projectPaymentListIBLL.UpdateFlowId(entity.id, entity.WorkFlowId);
                UserInfo userInfo = LoginUserInfo.Get();
                nWFProcessIBLL.AgainCreateFlow(entity.WorkFlowId, userInfo, "");
            }
            else
            {
                projectPaymentListIBLL.UpdateFlowId(entity.id, entity.WorkFlowId);
            }
            var tid_list = projectPaymentListIBLL.GetEntityBytID(entity.tid);
            if (tid_list != null & tid_list.ToList().Count > 0)
            {
                foreach (var item in tid_list)
                {
                    projectContractBLL.RecaculateEffectiveAmountByProjectId(item.ProjectId);
                }
            }
            return Success("提审成功！");
        }
        public Response PaymentSaveList2(dynamic _)
        {
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            string jsonText = string.Empty;
            HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空
            StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
            jsonText = sr.ReadToEnd();
            ProjectPaymentList2 entity = jsonText.ToObject<ProjectPaymentList2>();
            ProjectPaymentEntity entity1 = entity.ProjectPayment;
            List<BatchAuditAddModel> item_list = entity.PaymentAmountList;
            projectPaymentListIBLL.SaveEntityList("1", entity1, item_list);
            return Success("保存成功！");
        }
        public Response GetPaymentDeleteId2(dynamic _)
        {
            string keyValue = this.GetReqData();
            projectPaymentIBLL.DeleteEntity(keyValue);
            return Success("删除成功");
        }
        public Response GetPaymentDeleteByTid(dynamic _)
        {
            string keyValue = this.GetReqData();
            projectPaymentIBLL.DeleteByTid(keyValue);
            return Success("删除成功");
        }
        public Response GetPaymentSubmitter2(dynamic _)
        {
            string keyValue = this.GetReqData();
            var entity = projectPaymentIBLL.GetPreviewProjectPayment(keyValue);
            //ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            //ProjectPaymentEntity entity = parameter.strEntity.ToObject<ProjectPaymentEntity>();
            if (entity.PaymentStatus.ToInt() == 11 && entity.WorkFlowId != null)
            {
                projectPaymentIBLL.UpdateFlowId(entity.id, entity.WorkFlowId);
                UserInfo userInfo = LoginUserInfo.Get();
                nWFProcessIBLL.AgainCreateFlow(entity.WorkFlowId, userInfo, "");
            }
            else
            {
                projectPaymentIBLL.UpdateFlowId(entity.id, entity.WorkFlowId);
            }
            projectContractBLL.RecaculateEffectiveAmountByProjectId(entity.ProjectId);
            return Success("提审成功！");
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetProjectPaymentAll(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = projectPaymentIBLL.GetPageList(parameter.queryJson);
            List<ProjectPaymentVo> list = new List<ProjectPaymentVo>();
            foreach (var info in data)
            {
                if (string.IsNullOrEmpty(info.ProjectName))
                {
                    var projectData = projectIBLL.GetProjectEntity(info.ProjectId);
                    if (projectData != null)
                    {
                        info.ProjectName = projectData.ProjectName;
                    }
                }
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //项目来源
                DataItemDetailEntity projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;

                }
                //创建人
                var createUser = userIBLL.GetFollowPersonNameByUserId(info.CreateUser);
                if (createUser != null)
                {
                    info.CreateUserName = createUser.F_RealName;
                }
                //付款类型
                var payType = dataItemBLL.GetDetailItemName(info.PayType, "PayType");
                if (payType != null)
                {
                    info.PayTypeName = payType.F_ItemName;
                }
                var projectInfo = projectIBLL.GetProjectEntity(info.ProjectId);
                if (projectInfo != null)
                {
                    if (projectInfo.ProjectStatus == "3" || projectInfo.TenderFlg == "1")
                    {
                        var followPerson = userIBLL.GetEntityByUserId(projectInfo.FollowPerson);
                        if (followPerson != null)
                        {
                            info.DepartmentId = followPerson.F_DepartmentId;
                        }
                    }
                }
                //所属部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }
                //支付状态
                //var billingStatus = dataItemBLL.GetDetailItemName(info.PaymentStatus, "PaymentStatus");
                //if (billingStatus != null)
                //{
                //    info.PaymentStatusName = billingStatus.F_ItemName;
                //}
                //支付方式
                //var paymentMethod = dataItemBLL.GetDetailItemName(info.PaymentMethod, "Client_PaymentMode");
                //if (paymentMethod != null)
                //{
                //    info.PaymentMethodName = paymentMethod.F_ItemName;
                //}
                //支付抬头
                //var paymentHeader = dataItemBLL.GetDetailItemName(info.PaymentHeader, "PaymentHeader");
                //if (paymentHeader != null)
                //{
                //    info.PaymentHeaderName = paymentHeader.F_ItemName;
                //}
                list.Add(info);
            }
            list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);


        } /// <summary>


        /// 导出
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetProjectPaymentAll2(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = projectPaymentListIBLL.GetPageList(parameter.queryJson);
            List<ProjectPaymentVo> list = new List<ProjectPaymentVo>();
            foreach (var info in data)
            {
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //项目来源
                DataItemDetailEntity projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;

                }
                //创建人
                var createUser = userIBLL.GetFollowPersonNameByUserId(info.CreateUser);
                if (createUser != null)
                {
                    info.CreateUserName = createUser.F_RealName;
                }
                //付款类型
                var payType = dataItemBLL.GetDetailItemName(info.PayType, "PayType");
                if (payType != null)
                {
                    info.PayTypeName = payType.F_ItemName;
                }
                var projectInfo = projectIBLL.GetProjectEntity(info.ProjectId);
                if (projectInfo != null)
                {
                    if (projectInfo.ProjectStatus == "3" || projectInfo.TenderFlg == "1")
                    {
                        var followPerson = userIBLL.GetEntityByUserId(projectInfo.FollowPerson);
                        if (followPerson != null)
                        {
                            info.DepartmentId = followPerson.F_DepartmentId;
                        }
                    }
                }
                //所属部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }
                //支付方式
                //var paymentMethod = dataItemBLL.GetDetailItemName(info.PaymentMethod, "Client_PaymentMode");
                //if (paymentMethod != null)
                //{
                //    info.PaymentMethodName = paymentMethod.F_ItemName;
                //}
                ////支付状态
                //var billingStatus = dataItemBLL.GetDetailItemName(info.PaymentStatus, "PaymentStatus");
                //if (billingStatus != null)
                //{
                //    info.PaymentStatusName = billingStatus.F_ItemName;
                //}
                //支付抬头
                //var paymentHeader = dataItemBLL.GetDetailItemName(info.PaymentHeader, "PaymentHeader");
                //if (paymentHeader != null)
                //{
                //    info.PaymentHeaderName = paymentHeader.F_ItemName;
                //}
                list.Add(info);
            }
            list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);


        }
        #endregion
        #region 行政付款
        /// <summary>
        /// 行政付款列表
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetAdministrativePaymenteList(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            List<PaymentVo> list = new List<PaymentVo>();
            //PaymentVo q = parameter.queryJson.ToObject<PaymentVo>();
            /*  if (q.ContractSubmitter != null)
              {
                  var user =userIBLL.GetHZUserName(q.ContractSubmitter);
              }*/


            //var data = paymentIBLL.GetPageListAPI(parameter.pagination, parameter.queryJson);
            var data = paymentIBLL.GetPageList2(parameter.pagination, parameter.queryJson);
            //付款类型下拉框
            //var PayTypeList= dataItemIBLL.GetDetailTree("Client_PaymentMode");

            foreach (var info in data)
            {
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //创建人
                var createUser = userIBLL.GetFollowPersonNameByUserId(info.CreateUser);
                if (createUser != null)
                {
                    info.CreateUserName = createUser.F_RealName;
                }
                //提审人
                var contractSubmitter = userIBLL.GetFollowPersonNameByUserId(info.ContractSubmitter);
                if (contractSubmitter != null)
                {
                    info.ContractSubmitterName = contractSubmitter.F_RealName;
                }
                //当前审核人
                var taskNode = nWFProcessIBLL.GetTaskUserList_NodbWhere(info.WorkFlowId);
                if (taskNode != null)
                {
                    if (taskNode.ToList().Count > 0)
                    {
                        var taskInfo = taskNode.ToList().OrderByDescending(i => i.F_CreateDate).FirstOrDefault();

                        NWFTaskEntity t = new NWFTaskEntity();
                        if (taskInfo.nWFUserInfoList[0].Id != null)
                        {
                            UserEntity followPerson = userIBLL.GetFollowPersonNameByUserId(taskInfo.nWFUserInfoList[0].Id);
                            if (followPerson != null)
                            {
                                info.PaymentSubmitterName = followPerson.F_RealName;
                            }
                        }
                    }
                }
                //var paymentSubmitter = userIBLL.GetFollowPersonNameByUserId(info.PaymentSubmitter);
                //if (paymentSubmitter != null)
                //{
                //    info.PaymentSubmitterName = paymentSubmitter.F_RealName;
                //}

                //开票内容
                var payType = dataItemBLL.GetDetailItemName(info.PayType, "PaymentType");
                if (payType != null)
                {
                    info.PayTypeName = payType.F_ItemName;
                }

                //所属部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }
                //支付类型
                var paymentMethod = dataItemBLL.GetDetailItemName(info.PaymentMethod, "Client_PaymentMode");
                if (paymentMethod != null)
                {
                    info.PaymentMethodName = paymentMethod.F_ItemName;
                }
                //支付状态
                var billingStatus = dataItemBLL.GetDetailItemName(info.PaymentStatus, "PaymentStatus");
                if (billingStatus != null)
                {
                    info.PaymentStatusName = billingStatus.F_ItemName;
                }

                list.Add(info);
            }
            var jsonData = new
            {
                rows = list,
                // PayTypeList = PayTypeList,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };
            return Success(jsonData);
        }
        public Response GetAdministrativePaymentById(dynamic _)
        {
            string keyValue = this.GetReqData();
            var PaymentData = paymentIBLL.GetPreviewProjectPayment(keyValue);


            var paymentFile = annexesFileIBLL.GetList(PaymentData.PaymentFile);
            if (paymentFile != null)
            {
                foreach (var item in paymentFile)
                {
                    item.F_FileType = item.F_FileType.ToLower();
                }
                PaymentData.annexesFileEntities = paymentFile;
            }

            var jsonData = new
            {
                Payment = PaymentData,
            };
            return Success(jsonData);
        }
        public Response GetAdministrativePaymentDeleteId2(dynamic _)
        {
            string keyValue = this.GetReqData();
            paymentIBLL.DeleteEntity(keyValue);
            return Success("删除成功");
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetPaymentExport(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            string sql = "";
            var data = paymentIBLL.GetPageList(parameter.queryJson, out sql);
            List<PaymentVo> list = new List<PaymentVo>();
            data = data.OrderByDescending(t => t.CreateTime).ToList();
            foreach (var info in data)
            {
                //创建时间
                string time = info.CreateTime.Value.ToString("yyyy-MM-dd");
                info.CreateTimeyMd = time;
                ////创建人
                //var createUser = userIBLL.GetFollowPersonNameByUserId(info.CreateUser);
                //if (createUser != null)
                //{
                //    info.CreateUserName = createUser.F_RealName;
                //}
                ////提审人
                //var contractSubmitter = userIBLL.GetFollowPersonNameByUserId(info.ContractSubmitter);
                //if (contractSubmitter != null)
                //{
                //    info.ContractSubmitterName = contractSubmitter.F_RealName;
                //}
                ////审核人
                //var paymentSubmitter = userIBLL.GetFollowPersonNameByUserId(info.PaymentSubmitter);
                //if (paymentSubmitter != null)
                //{
                //    info.PaymentSubmitterName = paymentSubmitter.F_RealName;
                //}
                ////所属部门
                //var department = departmentIBLL.GetEntity(info.DepartmentId);
                //if (department != null)
                //{
                //    info.DepartmentName = department.F_FullName;
                //}
                //开票内容
                var payType = dataItemBLL.GetDetailItemName(info.PayType, "PaymentType");
                if (payType != null)
                {
                    info.PayTypeName = payType.F_ItemName;
                }
                //支付类型
                var paymentMethod = dataItemBLL.GetDetailItemName(info.PaymentMethod, "Client_PaymentMode");
                if (paymentMethod != null)
                {
                    info.PaymentMethodName = paymentMethod.F_ItemName;
                }
                //支付状态
                var billingStatus = dataItemBLL.GetDetailItemName(info.PaymentStatus, "PaymentStatus");
                if (billingStatus != null)
                {
                    info.PaymentStatusName = billingStatus.F_ItemName;
                }
                //当前提审人
                info.PaymentSubmitterName = "";
                var taskNode = nWFProcessIBLL.GetTaskUserList_NodbWhere(info.WorkFlowId);
                if (taskNode != null)
                {
                    if (taskNode.ToList().Count > 0)
                    {
                        var taskInfo = taskNode.ToList().OrderByDescending(i => i.F_CreateDate).FirstOrDefault();

                        NWFTaskEntity t = new NWFTaskEntity();
                        if (taskInfo.nWFUserInfoList[0].Id != null)
                        {
                            UserEntity followPerson = userIBLL.GetFollowPersonNameByUserId(taskInfo.nWFUserInfoList[0].Id);
                            if (followPerson != null)
                            {
                                info.PaymentSubmitterName = followPerson.F_RealName;
                            }
                        }
                    }
                }
                list.Add(info);
            }


            //list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                sql = sql,
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);


        }
        public Response GetAdministrativePaymentById2(dynamic _)
        {
            string keyValue = this.GetReqData();
            var PaymentData = paymentIBLL.GetPreviewProjectPayment(keyValue);


            var paymentFile = annexesFileIBLL.GetList(PaymentData.PaymentFile);
            if (paymentFile != null)
            {
                foreach (var item in paymentFile)
                {
                    item.F_FileType = item.F_FileType.ToLower();
                }
                PaymentData.annexesFileEntities = paymentFile;
            }

            //创建人
            var createUser = userIBLL.GetFollowPersonNameByUserId(PaymentData.CreateUser);
            if (createUser != null)
            {
                PaymentData.CreateUserName = createUser.F_RealName;
            }

            //付款类型
            var payType = dataItemBLL.GetDetailItemName(PaymentData.PayType, "PaymentType");
            if (payType != null)
            {
                PaymentData.PayTypeName = payType.F_ItemName;
            }


            //支付方式
            var paymentMethod = dataItemBLL.GetDetailItemName(PaymentData.PaymentMethod, "Client_PaymentMode");
            if (paymentMethod != null)
            {
                PaymentData.PaymentMethodName = paymentMethod.F_ItemName;
            }
            //支付抬头
            //var paymentHeader = dataItemBLL.GetDetailItemName(PaymentData.PaymentHeader, "PaymentHeader");
            //if (paymentHeader != null)
            //{
            //    PaymentData.PaymentHeaderName = paymentHeader.F_ItemName;
            //}
            return Success(PaymentData);
        }
        public Response GetAdministrativePaymentByProcessId(dynamic _)
        {
            string keyValue = this.GetReqData();
            var PaymentData = paymentIBLL.GetEntityByProcessId(keyValue);


            var paymentFile = annexesFileIBLL.GetList(PaymentData.PaymentFile);
            if (paymentFile != null)
            {
                foreach (var item in paymentFile)
                {
                    item.F_FileType = item.F_FileType.ToLower();
                }
                PaymentData.annexesFileEntities = paymentFile;
            }

            //创建人
            var createUser = userIBLL.GetFollowPersonNameByUserId(PaymentData.CreateUser);
            if (createUser != null)
            {
                PaymentData.CreateUserName = createUser.F_RealName;
            }

            //付款类型
            var payType = dataItemBLL.GetDetailItemName(PaymentData.PayType, "PaymentType");
            if (payType != null)
            {
                PaymentData.PayTypeName = payType.F_ItemName;
            }


            //支付方式
            var paymentMethod = dataItemBLL.GetDetailItemName(PaymentData.PaymentMethod, "Client_PaymentMode");
            if (paymentMethod != null)
            {
                PaymentData.PaymentMethodName = paymentMethod.F_ItemName;
            }
            //支付抬头
            //var paymentHeader = dataItemBLL.GetDetailItemName(PaymentData.PaymentHeader, "PaymentHeader");
            //if (paymentHeader != null)
            //{
            //    PaymentData.PaymentHeaderName = paymentHeader.F_ItemName;
            //}
            var jsonData = new
            {
                Payment = PaymentData,
            };
            return Success(jsonData);
        }
        public Response AdministrativePaymentSave(dynamic _)
        {
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            string jsonText = string.Empty;

            HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空
            StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
            jsonText = sr.ReadToEnd();

            //ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            PaymentEntity entity = jsonText.ToObject<PaymentEntity>();
            paymentIBLL.SaveEntity(entity.Id, entity);
            return Success("保存成功！");
        }
        //提审
        public Response GetAdministrativePaymentSubmitter(dynamic _)
        {
            string keyValue = this.GetReqData();
            var PaymentData = paymentIBLL.GetPreviewProjectPayment(keyValue);

            if (PaymentData.PaymentStatus.ToInt() == 11 && PaymentData.WorkFlowId != null)
            {

                paymentIBLL.UpdateFlowId(PaymentData.Id, PaymentData.WorkFlowId);
                UserInfo userInfo = LoginUserInfo.Get();
                nWFProcessIBLL.AgainCreateFlow(PaymentData.WorkFlowId, userInfo, "");
            }
            else
            {
                paymentIBLL.UpdateFlowId(PaymentData.Id, PaymentData.WorkFlowId);
            }

            return Success("提审成功！");
        }


        #endregion




        #region 回款接口

        public Response GetPayCollectionPageList(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = projectPayCollectionIBLL.GetPageList(parameter.pagination, parameter.queryJson);
            List<ProjectPayCollectionVo> list = new List<ProjectPayCollectionVo>();
            foreach (var info in data)
            {
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //到款时间
                DateTime time1 = (DateTime)info.ReceiptDate;
                info.ReceiptDateMd = time1.ToString("yyyy-MM-dd");
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }
                //营销部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }
                //创建人
                var createUser = userIBLL.GetFollowPersonNameByUserId(info.CreateUser);
                if (createUser != null)
                {
                    info.CreateUserName = createUser.F_RealName;
                }

                list.Add(info);
            }

            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };
            return Success(jsonData);
        }
        //回款合计
        public Response GetPayCollectionSum(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = projectPayCollectionIBLL.GetPageListSUM(parameter.queryJson);
            decimal? AmountSUM = 0;
            foreach (var item in data)
            {
                AmountSUM = AmountSUM + item.Amount;
            }


            var result = new
            {
                AmountSum = AmountSUM,

            };
            var jsonData = new
            {
                rows = result
            };
            return Success(jsonData);
        }
        public Response GetPayCollectioById(dynamic _)
        {

            string keyValue = this.GetReqData();

            var ProjectPayCollectionData = projectPayCollectionIBLL.GetPreviewProjectPayCollectionById(keyValue);


            var jsonData = new
            {
                ProjectPayCollection = ProjectPayCollectionData,
            };
            return Success(jsonData);

        }
        public Response GetPayCollectioById2(dynamic _)
        {

            string keyValue = this.GetReqData();

            var ProjectPayCollectionData = projectPayCollectionIBLL.GetPreviewProjectPayCollectionById(keyValue);
            return Success(ProjectPayCollectionData);
        }
        public Response GetPayCollectioDeleteId2(dynamic _)
        {

            string keyValue = this.GetReqData();

            projectPayCollectionIBLL.DeleteEntity(keyValue);


            return Success("删除成功");
        }
        /// <summary>
        /// 开票的新增和修改
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response PayCollectioSave(dynamic _)
        {
            ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            ProjectPayCollectionEntity entity = parameter.strEntity.ToObject<ProjectPayCollectionEntity>();

            projectPayCollectionIBLL.SaveEntity(parameter.keyValue, entity); ;
            return Success("保存成功！");
        }
        public Response PayCollectioSave2(dynamic _)
        {
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            string jsonText = string.Empty;

            HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空
            StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
            jsonText = sr.ReadToEnd();
            ProjectPayCollectionEntity entity = jsonText.ToObject<ProjectPayCollectionEntity>();
            /*ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            ProjectPayCollectionEntity entity = parameter.strEntity.ToObject<ProjectPayCollectionEntity>();*/
            projectPayCollectionIBLL.SaveEntity(entity.id, entity); ;
            return Success("保存成功！");
        }
        #endregion
        #region 报备接口
        //列表
        public Response GetProjectList(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            List<ProjectVo> list = new List<ProjectVo>();

            var data = projectManageIBLL.GetProjectList2(parameter.pagination, parameter.queryJson);
            foreach (var info in data)
            {
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                //报备人员
                var preparedPerson = userIBLL.GetEntityByUserId(info.PreparedPerson);
                //项目状态
                var projectStatus = dataItemBLL.GetDetailItemName(info.ProjectStatus, "projectStatus");
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                //项目来源



                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }
                if (preparedPerson != null)
                {
                    info.PreparedPersonName = preparedPerson.F_RealName;
                    //所属部门
                    var department = departmentIBLL.GetEntity(preparedPerson.F_DepartmentId);
                    if (department != null)
                    {
                        info.PreparedPersonDeptName = department.F_FullName;
                    }
                }
                if (followPerson != null)
                {
                    info.FollowPersonName = followPerson.F_RealName;
                }
                if (projectStatus != null)
                {
                    info.ProjectStatusName = projectStatus.F_ItemName;
                }
                list.Add(info);
            }
            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };

            return Success(jsonData);
        }
        public Response GetProjectName2(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            List<ProjectVo> list = new List<ProjectVo>();
            //var data = projectManageIBLL.GetProjectList2(parameter.pagination, parameter.queryJson);
            var data = projectManageIBLL.GetSelectedProjectByContractList(parameter.pagination, parameter.queryJson);
            foreach (var info in data)
            {
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                //报备人员
                var preparedPerson = userIBLL.GetEntityByUserId(info.PreparedPerson);
                //项目状态
                var projectStatus = dataItemBLL.GetDetailItemName(info.ProjectStatus, "projectStatus");
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }
                if (preparedPerson != null)
                {
                    info.PreparedPersonName = preparedPerson.F_RealName;
                }
                if (followPerson != null)
                {
                    info.FollowPersonName = followPerson.F_RealName;
                }
                if (projectStatus != null)
                {
                    info.ProjectStatusName = projectStatus.F_ItemName;
                }
                list.Add(info);
            }
            var jsonData = new
            {
                rows = list,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };

            return Success(jsonData);
        }
        public Response GetProjectName1(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            List<ProjectVo> list = new List<ProjectVo>();
            var data = projectManageIBLL.GetSelectedProjectList(parameter.pagination, parameter.queryJson);

            var jsonData = new
            {
                rows = data,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };

            return Success(jsonData);
        }
        public Response getProjectPageListYG(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            List<ProjectVo> list = new List<ProjectVo>();
            var user = LoginUserInfo.Get();
            var userId = LoginUserInfo.Get().userId;
            var followPerson = userIBLL.GetHZUserId_2(userId);
            List<ProjectVo> listdate = new List<ProjectVo>();
            var roleList = userRelationIBLL.GetUserRoleList(userId);
            string deps = "";
            int isFinance = 0;
            if (roleList.Count > 0)
            {
                var financeRole = roleList.Where(i => i.F_ObjectId == "c38d935c-c364-41fb-8d36-304076009949").ToList();
                if (financeRole.Count > 0)
                {
                    isFinance = 1;
                }
            }
            //是否管理员
            //是否角色位财务
            if (userId == "1e5dfa6a-6f0c-454c-b1ac-aeafef95aea5" || userId == "fae74e8a-3dcc-45f2-b6e1-b6800654eaf9" || userId == "System"
                || isFinance == 1)
            {
                deps = "";
            }
            else
            {
                deps = " and ( ";
                if (followPerson.F_MoreDepartmentId != null)
                {

                    string[] strList = followPerson.F_MoreDepartmentId.Split(',');

                    for (var i = 0; i < strList.Length; i++)
                    {
                        if (i == 0)
                        {
                            deps += " ( t.DepartmentId='" + strList[i] + "' or t.FDepartmentId='" + strList[i] + "' or t.PDepartmentId='" + strList[i] + "') ";
                        }
                        else
                        {
                            deps += " or ( t.DepartmentId='" + strList[i] + "' or t.FDepartmentId='" + strList[i] + "' or t.PDepartmentId='" + strList[i] + "') ";
                        }

                    }
                }
                else
                {
                    deps += " ( t.DepartmentId='" + user.departmentId + "' or t.FDepartmentId='" + user.departmentId + "' or t.PDepartmentId='" + user.departmentId + "') ";
                }
                deps += " or pt.DepartmentId='" + user.departmentId + "' or t.CreateUser = '" + userId + "' or t.FollowPerson = '" + userId + "' or pc.DepartmentId = '" + user.departmentId + "' ) ";

            }
            var data = projectManageIBLL.GetSelectedProjectByContractList_2(parameter.pagination, parameter.queryJson, deps);

            var jsonData = new
            {
                rows = data,
                total = parameter.pagination.total,
                page = parameter.pagination.page,
                records = parameter.pagination.records
            };

            return Success(jsonData);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetProjectAll(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data1 = projectManageIBLL.GetPageListAddress2(parameter.queryJson);
            List<ProjectVo> list = new List<ProjectVo>();
            foreach (var info in data1)
            {

                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //营销人员
                var followPerson = userIBLL.GetEntityByUserId(info.FollowPerson);
                //报备人员
                var preparedPerson = userIBLL.GetEntityByUserId(info.PreparedPerson);
                //项目状态
                var projectStatus = dataItemBLL.GetDetailItemName(info.ProjectStatus, "projectStatus");
                //项目来源
                var projectSource = dataItemBLL.GetDetailItemName(info.ProjectSource, "ProjectSource");
                if (projectSource != null)
                {
                    info.ProjectSourceName = projectSource.F_ItemName;
                }
                if (preparedPerson != null)
                {
                    info.PreparedPersonName = preparedPerson.F_RealName;
                }
                if (preparedPerson != null)
                {
                    info.PreparedPersonName = preparedPerson.F_RealName;
                    //所属部门
                    var department = departmentIBLL.GetEntity(preparedPerson.F_DepartmentId);
                    if (department != null)
                    {
                        info.PreparedPersonDeptName = department.F_FullName;
                    }
                }
                if (followPerson != null)
                {
                    info.FollowPersonName = followPerson.F_RealName;
                }
                if (projectStatus != null)
                {
                    info.ProjectStatusName = projectStatus.F_ItemName;
                }
                list.Add(info);
            }
            list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);


        }


        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetProjectContractAll(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = projectContractIBLL.GetPageList(parameter.queryJson);
            List<ProjectContractVo> list = new List<ProjectContractVo>();
            foreach (var info in data)
            {
                list.Add(info);
            }
            list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                rows = list
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 获取合同编号
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetContractCode(dynamic _)
        {
            string keyValue = this.GetReqData();
            //合同归档编号
            if (keyValue == "1" || keyValue == "1FB")
            {
                //是否为通际公司
                if (userInfo.departmentId == "1b666e23-78b0-4f43-b8e8-9565602455f3")
                {
                    keyValue = "1TJ";
                }
            }
            var data = codeRuleIBLL.GetBillCode(keyValue);
            var jsonData = new
            {
                ContractCode = data
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 报告导出
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetProjectTaskAll(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = projectTaskIBLL.GetPageList(parameter.queryJson);
            List<ProjectTaskVo> list = new List<ProjectTaskVo>();
            foreach (var info in data)
            {
                if (string.IsNullOrEmpty(info.ProjectName))
                {
                    var projectInfo = projectIBLL.GetProjectEntity(info.ProjectId);
                    if (projectInfo != null)
                    {
                        info.ProjectName = projectInfo.ProjectName;
                    }
                }
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //报告时间
                if (info.FlowFinishedTime != null)
                {
                    DateTime time1 = (DateTime)info.FlowFinishedTime;
                    info.FlowFinishedTimeMD = time1.ToString("yyyy-MM-dd");
                }
                //进场时间
                if (info.ActualApproachTime != null)
                {
                    DateTime time1 = (DateTime)info.ActualApproachTime;
                    info.ApproachTimeMd = time1.ToString("yyyy-MM-dd");
                }

                //离场时间
                if (info.ActualDepartureTime != null)
                {
                    DateTime time2 = (DateTime)info.ActualDepartureTime;
                    info.ActualDepartureTimeMd = time2.ToString("yyyy-MM-dd");
                }
                //报告计划时间
                if (info.PlanTime != null)
                {
                    DateTime time3 = (DateTime)info.PlanTime;
                    info.PlanTimeMd = time3.ToString("yyyy-MM-dd");
                }


                //项目负责人
                var projectResponsible = userIBLL.GetEntityByUserId(info.ProjectResponsible);
                if (projectResponsible != null)
                {
                    info.ProjectResponsibleName = projectResponsible.F_RealName;
                }
                //所属部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }

                //报告状态
                var taskStatus = dataItemBLL.GetDetailItemName(info.TaskStatus, "TaskStatus");
                if (taskStatus != null)
                {
                    info.TaskStatusName = taskStatus.F_ItemName;
                }
                //报告主体
                var contractSubject = dataItemBLL.GetDetailItemName(info.ReportSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    info.ReportSubjectName = contractSubject.F_ItemName;
                }
                //预警

                if (info.TaskStatus.ToInt() == 5)
                {
                    info.YJ = "已完成";
                }
                else if (info.YJ.ToInt() == 999)
                {
                    info.YJ = "超时";
                }
                else if (info.YJ.ToInt() != 999 && info.TaskStatus.ToInt() != 5 && info.YJ.ToInt() != 111)
                {
                    info.YJ = "剩余时间" + info.YJ;
                }
                else if (info.YJ.ToInt() == 111)
                {
                    info.YJ = "严重超时";
                }
                //  List<ProjectContractEntity> projectContracts = projectContractIBLL.GetProjectContractByProjectId(info.ProjectId);
                /*   if (projectContracts.Count > 0)
                   {
                       info.ContractNo = projectContracts.FirstOrDefault().ContractNo;

                       //合同主体
                       var contractSubject = dataItemBLL.GetDetailItemName(projectContracts.FirstOrDefault().ContractSubject, "ContractSubject");
                       if (contractSubject != null)
                       {
                           info.ContractSubjectName = contractSubject.F_ItemName;
                       }
                   }*/
                list.Add(info);
            }
            list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);


        }
        /// <summary>
        /// 合作报告导出
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetHZProjectTaskAll(dynamic _)
        {
            ReqPageParam parameter = this.GetReqData<ReqPageParam>();
            var data = projectTaskIBLL.GetHZTaskListAll(parameter.queryJson);
            List<ProjectTaskVo> list = new List<ProjectTaskVo>();
            foreach (var info in data)
            {
                if (string.IsNullOrEmpty(info.ProjectName))
                {
                    var projectInfo = projectIBLL.GetProjectEntity(info.ProjectId);
                    if (projectInfo != null)
                    {
                        info.ProjectName = projectInfo.ProjectName;
                    }
                }
                //创建时间
                DateTime time = (DateTime)info.CreateTime;
                info.CreateTimeyMd = time.ToString("yyyy-MM-dd");
                //报告时间
                if (info.FlowFinishedTime != null)
                {
                    DateTime time1 = (DateTime)info.FlowFinishedTime;
                    info.FlowFinishedTimeMD = time1.ToString("yyyy-MM-dd");
                }
                //进场时间
                if (info.ApproachTime != null)
                {
                    DateTime time1 = (DateTime)info.ApproachTime;
                    info.ApproachTimeMd = time1.ToString("yyyy-MM-dd");
                }

                //离场时间
                if (info.ActualDepartureTime != null)
                {
                    DateTime time2 = (DateTime)info.ActualDepartureTime;
                    info.ActualDepartureTimeMd = time2.ToString("yyyy-MM-dd");
                }
                //报告计划时间
                if (info.PlanTime != null)
                {
                    DateTime time3 = (DateTime)info.PlanTime;
                    info.PlanTimeMd = time3.ToString("yyyy-MM-dd");
                }


                //项目负责人
                var projectResponsible = userIBLL.GetEntityByUserId(info.ProjectResponsible);
                if (projectResponsible != null)
                {
                    info.ProjectResponsibleName = projectResponsible.F_RealName;
                }
                //所属部门
                var department = departmentIBLL.GetEntity(info.DepartmentId);
                if (department != null)
                {
                    info.DepartmentName = department.F_FullName;
                }

                //报告状态
                var taskStatus = dataItemBLL.GetDetailItemName(info.TaskStatus, "TaskStatus");
                if (taskStatus != null)
                {
                    info.TaskStatusName = taskStatus.F_ItemName;
                }
                //报告主体
                var contractSubject = dataItemBLL.GetDetailItemName(info.ReportSubject, "ContractSubject");
                if (contractSubject != null)
                {
                    info.ReportSubjectName = contractSubject.F_ItemName;
                }
                //预警

                if (info.TaskStatus.ToInt() == 5)
                {
                    info.YJ = "已完成";
                }
                else if (info.YJ.ToInt() == 999)
                {
                    info.YJ = "超时";
                }
                else if (info.YJ.ToInt() != 999 && info.TaskStatus.ToInt() != 5 && info.YJ.ToInt() != 111)
                {
                    info.YJ = "剩余时间" + info.YJ;
                }
                else if (info.YJ.ToInt() == 111)
                {
                    info.YJ = "严重超时";
                }
                //  List<ProjectContractEntity> projectContracts = projectContractIBLL.GetProjectContractByProjectId(info.ProjectId);
                /*   if (projectContracts.Count > 0)
                   {
                       info.ContractNo = projectContracts.FirstOrDefault().ContractNo;

                       //合同主体
                       var contractSubject = dataItemBLL.GetDetailItemName(projectContracts.FirstOrDefault().ContractSubject, "ContractSubject");
                       if (contractSubject != null)
                       {
                           info.ContractSubjectName = contractSubject.F_ItemName;
                       }
                   }*/
                list.Add(info);
            }
            list = list.OrderByDescending(t => t.CreateTime).ToList();
            var jsonData = new
            {
                rows = JsonConvert.SerializeObject(list)
            };
            return Success(jsonData);


        }

        //根据id查询对应报备信息
        public Response GetProjectById(dynamic _)
        {
            string keyValue = this.GetReqData();
            var ProjectData = projectManageIBLL.GetPreviewFormDataById(keyValue);
            //营销人员
            var followPerson = userIBLL.GetEntityByUserId(ProjectData.FollowPerson);
            //报备人员
            var preparedPerson = userIBLL.GetEntityByUserId(ProjectData.PreparedPerson);
            if (preparedPerson != null)
            {
                ProjectData.PreparedPersonName = preparedPerson.F_RealName;
            }
            if (followPerson != null)
            {
                ProjectData.FollowPersonName = followPerson.F_RealName;
            }
            var jsonData = new
            {
                ProjectPayCollection = ProjectData,
            };
            return Success(jsonData);
        }
        //根据id查询对应报备信息
        public Response GetProjectById2(dynamic _)
        {
            string keyValue = this.GetReqData();
            var ProjectData = projectManageIBLL.GetPreviewFormDataById(keyValue);
            var followList = projectManageIBLL.GetProjectFollowList(keyValue);
            //营销人员
            var followPerson = userIBLL.GetEntityByUserId(ProjectData.FollowPerson);
            //报备人员
            var preparedPerson = userIBLL.GetEntityByUserId(ProjectData.PreparedPerson);
            if (preparedPerson != null)
            {
                ProjectData.PreparedPersonName = preparedPerson.F_RealName;
            }
            if (followPerson != null)
            {
                ProjectData.FollowPersonName = followPerson.F_RealName;
            }
            if (ProjectData.ProvinceId != null || ProjectData.CityId != null || ProjectData.CountyId != null)
            {

                var provinceId = "";
                var cityId = "";
                var countyId = "";

                var shen = areaIBLL.GetList(ProjectData.ProvinceId);
                if (shen.Count() > 0)
                {
                    provinceId = shen.FirstOrDefault().F_AreaName;
                }

                var shi = areaIBLL.GetList(ProjectData.CityId);

                if (shi.Count() > 0)
                {
                    cityId = shi.FirstOrDefault().F_AreaName;
                }

                var diz = areaIBLL.GetList(ProjectData.CountyId);

                if (diz.Count() > 0)
                {
                    countyId = diz.FirstOrDefault().F_AreaName;
                }
                ProjectData.ProvincesAndcities = provinceId + "" + cityId + "" + countyId + ProjectData.Address;
            }
            if (ProjectData.ProvinceId == null || ProjectData.CityId == null || ProjectData.CountyId == null)
            {
                ProjectData.ProvincesAndcities = ProjectData.Address;
            }
            var tenderFlg = dataItemBLL.GetDetailItemName(ProjectData.TenderFlg, "TenderFlg");
            if (tenderFlg != null)
            {
                ProjectData.TenderFlgName = tenderFlg.F_ItemName;
            }
            var jsonData = new
            {
                entity = ProjectData,
                followList = followList
            };
            return Success(jsonData);
        }
        //删除
        public Response getProjectDelete(dynamic _)
        {
            string keyValue = this.GetReqData();
            projectManageIBLL.DeleteEntity(keyValue);
            return Success("删除成功");
        }
        public Response getProjectDelete2(dynamic _)
        {
            string keyValue = this.GetReqData();
            projectManageIBLL.DeleteEntity(keyValue);
            return Success("删除成功");
        }
        public Response GetContractDelete2(dynamic _)
        {
            string keyValue = this.GetReqData();
            projectContractIBLL.DeleteEntity(keyValue);
            return Success("删除成功");
        }
        //项目报备的新增/修改
        public Response GetProjectSave(dynamic _)
        {
            ReqFormProjectEntity parameter = this.GetReqData<ReqFormProjectEntity>();
            ProjectEntity entity = parameter.strEntity.ToObject<ProjectEntity>();
            if (!string.IsNullOrEmpty(entity.ContactPhone))
            {
                entity.ContactPhone = entity.ContactPhone.Trim();
            }
            if (!string.IsNullOrEmpty(entity.ProjectSource) && entity.ProjectSource == "1")
            {
                if (entity.TenderFlg == "1")
                {
                    int count = projectManageIBLL.JudgePepeatProjectName(entity.ProjectName, parameter.keyValue);
                    if (count > 0)
                    {
                        return Fail("该项目系统已存在！");
                    }
                }
                else
                {
                    int count = projectManageIBLL.JudgePepeatProject(entity.ContactPhone, entity.CustName, parameter.keyValue);
                    if (count > 0)
                    {
                        if (count == 1)
                        {
                            return Fail("该手机号码系统已存在！");
                        }
                        else if (count == 2)
                        {
                            return Fail("该委托单位系统已存在！");
                        }
                    }
                }

                //根绝营销人id 获取对应的营销人员的部门/和公司信息
                if (!string.IsNullOrEmpty(entity.FollowPerson))
                {
                    UserEntity user1 = userIBLL.GetEntityByUserId(entity.FollowPerson);
                    if (user1 != null)
                    {
                        entity.FCompanyId = user1.F_CompanyId;
                        entity.FDepartmentId = user1.F_DepartmentId;
                    }
                }
                else
                {
                    entity.FCompanyId = "";
                    entity.FDepartmentId = "";
                }
                if (!string.IsNullOrEmpty(entity.PreparedPerson))
                {
                    UserEntity user2 = userIBLL.GetEntityByUserId(entity.PreparedPerson);
                    if (user2 != null)
                    {
                        entity.PCompanyId = user2.F_CompanyId;
                        entity.PDepartmentId = user2.F_DepartmentId;
                    }
                }
                else
                {
                    entity.PCompanyId = "";
                    entity.PDepartmentId = "";
                }
                projectManageIBLL.SaveEntity(parameter.keyValue, entity);
                UserEntity user = userIBLL.GetEntityByUserId(entity.PreparedPerson);
                if (parameter.autoFlag == "1")
                {
                    codeRuleIBLL.UseRuleSeed("10002");
                }
            }
            else
            {
                //根绝营销人id 获取对应的营销人员的部门/和公司信息
                if (!string.IsNullOrEmpty(entity.FollowPerson))
                {
                    UserEntity user = userIBLL.GetEntityByUserId(entity.FollowPerson);
                    if (user != null)
                    {
                        entity.FCompanyId = user.F_CompanyId;
                        entity.FDepartmentId = user.F_DepartmentId;
                    }
                }
                else
                {
                    entity.FCompanyId = "";
                    entity.FDepartmentId = "";
                }
                if (!string.IsNullOrEmpty(entity.PreparedPerson))
                {
                    UserEntity user = userIBLL.GetEntityByUserId(entity.PreparedPerson);
                    if (user != null)
                    {
                        entity.PCompanyId = user.F_CompanyId;
                        entity.PDepartmentId = user.F_DepartmentId;
                    }
                }
                else
                {
                    entity.PCompanyId = "";
                    entity.PDepartmentId = "";
                }
                projectManageIBLL.SaveEntity(parameter.keyValue, entity);
                if (parameter.autoFlag == "1")
                {
                    codeRuleIBLL.UseRuleSeed("10002");
                }
            }
            projectManageIBLL.SaveEntity(parameter.keyValue, entity);
            //return Success("保存成功！", "项目报备", string.IsNullOrEmpty(parameter.keyValue) ? OperationType.Create : OperationType.Update, parameter.keyValue, entity.ToJson());
            return Success("保存成功！");
        }
        //项目报备的新增/修改
        public Response ProjectSave2(dynamic _)
        {
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            string jsonText = string.Empty;

            HttpContext.Current.Request.InputStream.Position = 0; //这一句很重要，不然一直是空

            StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
            jsonText = sr.ReadToEnd();

            //ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            ProjectDto projectDto = jsonText.ToObject<ProjectDto>();
            var entity = projectDto.form;
            ReqFormProjectEntity parameter = this.GetReqData<ReqFormProjectEntity>();
            if (string.IsNullOrEmpty(entity.ProjectStatus))
            {
                return Fail("项目状态不能为空");
            }
            if (string.IsNullOrEmpty(entity.ProjectSource))
            {
                return Fail("项目来源不能为空");
            }
            if (string.IsNullOrEmpty(entity.TenderFlg))
            {
                return Fail("是否投标项目不能为空");
            }
            if (string.IsNullOrEmpty(entity.PreparedPerson))
            {
                return Fail("报备人员不能为空");
            }
            if (string.IsNullOrEmpty(entity.FollowPerson))
            {
                return Fail("营销人员不能为空");
            }
            if (!string.IsNullOrEmpty(entity.ProjectSource))
            {
                if (entity.TenderFlg == "1")
                {
                    int count = projectManageIBLL.JudgePepeatProjectName(entity.ProjectName, entity.Id);
                    if (count > 0)
                    {
                        return Fail("该项目系统已存在！");
                    }
                }
                else
                {
                    int count = projectManageIBLL.JudgePepeatProject(entity.ContactPhone, entity.CustName, entity.Id);
                    if (count > 0)
                    {
                        if (count == 1)
                        {
                            return Fail("该手机号码系统已存在！");
                        }
                        else if (count == 2)
                        {
                            return Fail("该委托单位系统已存在！");
                        }
                    }
                }

                //根绝营销人id 获取对应的营销人员的部门/和公司信息
                if (!string.IsNullOrEmpty(entity.FollowPerson))
                {
                    UserEntity user = userIBLL.GetEntityByUserId(entity.FollowPerson);
                    if (user != null)
                    {
                        entity.FCompanyId = user.F_CompanyId;
                        entity.FDepartmentId = user.F_DepartmentId;
                    }
                }
                else
                {
                    entity.FCompanyId = "";
                    entity.FDepartmentId = "";
                }
                if (!string.IsNullOrEmpty(entity.PreparedPerson))
                {
                    UserEntity user = userIBLL.GetEntityByUserId(entity.PreparedPerson);
                    if (user != null)
                    {
                        entity.PCompanyId = user.F_CompanyId;
                        entity.PDepartmentId = user.F_DepartmentId;
                    }
                }
                else
                {
                    entity.PCompanyId = "";
                    entity.PDepartmentId = "";
                }
                projectManageIBLL.SaveEntity(entity.Id, entity);
                codeRuleIBLL.UseRuleSeed("10002");

            }
            else
            {
                //根绝营销人id 获取对应的营销人员的部门/和公司信息
                if (!string.IsNullOrEmpty(entity.FollowPerson))
                {
                    UserEntity user = userIBLL.GetEntityByUserId(entity.FollowPerson);
                    if (user != null)
                    {
                        entity.FCompanyId = user.F_CompanyId;
                        entity.FDepartmentId = user.F_DepartmentId;
                    }
                }
                else
                {
                    entity.FCompanyId = "";
                    entity.FDepartmentId = "";
                }
                if (!string.IsNullOrEmpty(entity.PreparedPerson))
                {
                    UserEntity user = userIBLL.GetEntityByUserId(entity.PreparedPerson);
                    if (user != null)
                    {
                        entity.PCompanyId = user.F_CompanyId;
                        entity.PDepartmentId = user.F_DepartmentId;
                    }
                }
                else
                {
                    entity.PCompanyId = "";
                    entity.PDepartmentId = "";
                }
                projectManageIBLL.SaveEntity(entity.Id, entity);

                codeRuleIBLL.UseRuleSeed("10002");

            }
            if (projectDto.followList != null)
            {
                foreach (var item in projectDto.followList)
                {
                    item.ProjectId = entity.Id;
                    projectManageIBLL.SaveProjectFollowEntity(item.Id, item);
                }
            }

            //return Success("保存成功！", "项目报备", string.IsNullOrEmpty(parameter.keyValue) ? OperationType.Create : OperationType.Update, parameter.keyValue, entity.ToJson());
            return Success("保存成功！");
        }
        #endregion
        /// <summary>
        /// 获取页面显示列表数据
        /// <summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetList(dynamic _)
        {
            string queryJson = this.GetReqData();
            var data = projectIBLL.GetList(queryJson);
            return Success(data);
        }
        /// <summary>
        /// 获取表单数据
        /// <summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public Response GetForm(dynamic _)
        {
            string keyValue = this.GetReqData();
            var ProjectData = projectIBLL.GetProjectEntity(keyValue);
            var jsonData = new
            {
                Project = ProjectData,
            };
            return Success(jsonData);
        }
        #endregion

        #region 提交数据


        /// <summary>
        /// 删除实体数据
        /// <param name="_"></param>
        /// <summary>
        /// <returns></returns>
        public Response DeleteForm(dynamic _)
        {
            string keyValue = this.GetReqData();
            projectIBLL.DeleteEntity(keyValue);
            return Success("删除成功！");
        }
        /// <summary>
        /// 报备的保存实体数据（新增、修改）
        /// <param name="_"></param>
        /// <summary>
        /// <returns></returns>
        public Response SaveForm(dynamic _)
        {
            ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            ProjectEntity entity = parameter.strEntity.ToObject<ProjectEntity>();
            projectIBLL.SaveEntity(parameter.keyValue, entity);
            return Success("保存成功！");
        }
        /// <summary>
        /// 保存settlement
        /// <param name="_"></param>
        /// <summary>
        /// <returns></returns>
        public Response SaveSettlement(dynamic _)
        {
            ReqFormEntity parameter = this.GetReqData<ReqFormEntity>();
            var id_list = parameter.strEntity.ToObject<List<string>>();
            if (id_list.Count > 0)
            {
                foreach (var item in id_list)
                {
                    var exist_entity = projectContractBLL.GetSettlementByContractId(item);
                    if (string.IsNullOrEmpty(exist_entity.ProjectId))
                    {
                        var contract = projectContractBLL.GetProjectContractEntity(item);
                        if (contract != null)
                        {
                            exist_entity.ProjectId = contract.ProjectId;
                        }
                    }
                    exist_entity.ContractId = item;
                    projectContractBLL.SaveProjectSettlement(exist_entity);
                }
            }
            return Success("保存成功！");
        }
        #endregion

        #region 私有类
        /// <summary>
        /// 表单实体类
        /// <summary>
        private class ReqFormEntity
        {
            public string keyValue { get; set; }
            public string strEntity { get; set; }

            public string data { get; set; }
        }
        private class ReqFormProjectEntity
        {
            public string keyValue { get; set; }
            public string strEntity { get; set; }
            public string autoFlag { get; set; }
        }
        private class QueryUserStatusReq
        {
            public string StartTime { get; set; }
            public string days { get; set; }
        }
        public class ReqProjectName
        {
            public string userId { get; set; }

            public string time { get; set; }
        }
        #endregion

    }
}
