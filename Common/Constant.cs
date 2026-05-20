using Nethereum.RPC.Eth.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Constant
    {
        //-----------Admin Authentication
        public static string spAdminUserLogin = "SpAdminUserLogin";
        public static string addAdminUser = "SpAddAdminUser";
        public static string spGetAdminDetails = "SpGetAdminDetails";
        public static string getAdminDashboardDetails = "SpGetAdminDashboardDetails";
        public static string spGetAllAdminList = "SpGetAllAdminList";


        //----------------END

        //----------Menu And SubMenu
        public static string spGetByIdMenu = "SpGetByIdMenu";
        public static string spGetAllMenu = "SpGetAllMenu";
        public static string getAllSubMenu = "SpGetAllSubMenu";
        public static string spDeleteMenu = "SpDeleteMenu";
        public static string getMenubyMenuId = "SpGetMenubyMenuId";
        public static string addMenu = "SpAddMenu";
        public static string updateMenu = "SpUpdateMenu";
        public static string spGetMenuByUserRole = "SpGetMenuByUserRole";
        public static string getMenuWithSubMenu = "SpGetMenuWithSubMenu";
        public static string addMenuWithSubMenu = "SpAddMenuWithSubMenu";
        public static string addMenuWithSubMenuBatch = "SpAddMenuWithSubMenuBatch";

        public static string spGetByIdSubMenu = "SpGetByIdSubMenu";
        public static string spGetAllSubMenu = "SpGetAllSubMenu";
        public static string spAddSubMenu = "SpAddSubMenu";
        public static string spUpdateSubMenu = "SpUpdateSubMenu";
        public static string spDeleteSubMenu = "SpDeleteSubMenu";
        public static string spGetSubMenubyMenuId = "SpGetSubMenubyMenuId";

        public static string spGetByIdRoleMenu = "SpGetByIdRoleMenu";
        public static string spGetAllRoleMenu = "SpGetAllRoleMenu";
        public static string spAddRoleMenu = "SpAddRoleMenu";
        public static string spUpdateRoleMenu = "SpUpdateRoleMenu";
        public static string spDeleteRoleMenu = "SpDeleteRoleMenu";

        //-------------END

        //-------Wallet Request And Withdrawal

        public static string getAllUnAppFundRequestReport_Admin = "SpGetAllUnAppFundRequestReport_Admin";
        public static string getAllApprovedFundRequestReport_Admin = "SpGetAllApprovedFundRequestReport_Admin";

        public static string allUnApprIncWithdrawalHistory_Admin = "SpAllUnApprIncWithdrawalHistory_Admin";
        public static string allApprIncWithdrawalHistory_Admin = "SpAllApprIncWithdrawalHistory_Admin";

        public static string allApprROIWithdrawalHistory_Admin = "SpAllApprROIWithdrawalHistory_Admin";
        public static string unApprROIWithdrawalHistory_Admin = "SpAllUnApprROIWithdrawalHistory_Admin";


        public static string updateIncomeWalletAdress = "SpUpdateIncomeWalletAdress";
        public static string upIncWithdReqStatus_Admin = "SpUpIncWithdReqStatus_Admin";

        public static string upROIWithdReqStatus_Admin = "SpUpROIWithdReqStatus_Admin";
        public static string updateROIWalletAdress = "SpUpdateROIWalletAdress";

        public static string updateFundRequestStatus_Admin = "SpUpdateFundRequestStatus_Admin";

        public static string addFundRequest = "SpAddFundRequest";
        public static string getFundRequestReport = "SpGetFundRequestReport";
        public static string addRequestUserwithdrawal = "SpAddRequestUserwithdrawal";
        public static string fundTransferDepositToDeposit = "SpFundTransferDepositToDeposit";
        public static string transferIncomeToDepositWallet = "SpTransferIncomeToDepositWallet";
        public static string getIncomeToDepositWalletReport = "SpGetIncomeToDepositWalletReport";
        public static string spGetUser_WalletBalance = "SpGetUser_WalletBalance";
        public static string spDepositWalletReport = "SpDepositWalletReport";


        public static string getIncomeWalletTransType = "DDSpGetIncomeWalletTransType";
        public static string getRoiWalletTransType = "DDSpGetROIWalletTransType";
        public static string getDepositWalletTransType = "DDSpGetDepositWalletTransType";

        public static string getInComeWalletStatement = "SpGetInComeWalletStatement";
        public static string getDepositWalletStatement = "SpGetDepositWalletStatement";
        public static string getRoiWalletStatement = "SpGetROIWalletStatement";
        public static string incomeOrRoiWithdrawalHistory = "SpIncOrRoiWithdrawalHistory";


        //------------END

        //------Admin Master
        public static string spGetUserNameByLoginId = "SpGetUserNameByLoginId";
        public static string spFundFromAdmin = "SpFundFromAdmin";
        public static string spUpdateAdminPassWord = "SpAdminChangePassword";
        public static string fundType = "SpFundType";
        public static string fundTypeWiseCrDr = "spFundTypeWiseCrDr";
        public static string getUser_WalletDetails = "SpGetUser_WalletDetails";

        //-----End 

        //-----Country , State , City 
        public static string spGetAllCountry = "SpGetAllCountry";
        public static string spGetAllState = "SpGetAllState";
        public static string spGetAllCity = "SpGetAllCity";
        //-----End


        //user Authentication 
        public static string spAddUserRegistration = "SpAddUserRegistration";
        public static string welcomeDetails = "SpWelcomeDetails";
        public static string spWelcomeDetails = "spWelcomeDetails;";
        public static string spUserForgotPassword = "SpUserForgotPassword";
        public static string spAppLogin = "SpUserLogin";
        public static string validateOtp = "SpValidateOtp";
        public static string spSendOtp = "SpSendOtp";
        public static string spVerifyOtp = "SpVerifyOtp";

        //End


        //-----------Users Ticket
        public static string spGetAllTicket = "SpGetAllTicket";
        public static string spAddTicket = "SpAddTicket";
        public static string updateTicket = "SpUpdateTicket";
        public static string deleteTicket = "SpDeleteTicket";
        public static string getAllTicketReplyByTicketId = "SpGetAllTicketReplyByTicketId";
        public static string getAllTicketByTicketId = "SpGetAllTicketByTicketId";
        public static string getAllTicketByUserId = "SpGetAllTicketByUserId";
        public static string insert_TicketTest = "SpAddTicket";
        public static string insert_TicketReplyTest = "SpAddTicketReply";
        public static string getAllTicketBYURIDTEst = "SpGetAllTicketBYURID";
        public static string getAllTicketBYTicketIdTEst = "SpGetAllTicketBYTicketId";
        public static string getAllTicketReplyByTicketIDTest = "SpGetAllTicketReplyByTicketID";
        public static string getAllTicket = "SpGetAllTicket";
        public static string closeTicketTest = "SpCloseTicket";
        public static string getAllClosedlistTicket = "SpGetAllClosedlistTicket";
        public static string adminReplyCount = "SpAdminReplyCount";
        public static string userReplyCount = "SpUserReplyCount";
        public static string updateUserReplyCount = "SpUpdateUserReplyCount";
        public static string updateAdminReplyCount = "SpUpdateAdminReplyCount";
        public static string spGetAllTicketReply = "SpGetAllTicketReply";
        public static string addTicketReply = "SpAddTicketReply";
        public static string updateTicketReply = "SpUpdateTicketReply";
        public static string spDeleteTicketReply = "SpDeleteTicketReply";
        //-----------Ticket Ends 

        //-----------buy package
        public static string SpAddRechargeTransactionUser = "SpAddRechargeTransactionUser";
        public static string downLineTree_Details_fourlvl = "nsp_downLineTree_Details_fourlvl";
        public static string downlineLeftRightCount = "SPGetDownlineLeftRightCount";
        public static string leftRightdownlineTeam = "spGetLeftRightdownlineTeam";
        public static string spGetPersonalTeamList_Search = "spGetPersonalTeamList_Search";
        public static string getPersonalTeamList_Search = "SpGetPersonalTeamList_Search";
        public static string accStatementAccType = "SpGetAccStatementAccType";
        //------------End

        //------------Category
        public static string spGetByIdCategory = "SpGetByIdCategory";
        public static string spGetAllCategory = "SpGetAllCategory";
        public static string spGetAllCategoryForUser = "SpGetAllCategoryForUser";
        public static string addCategory = "SpAddCategory";
        public static string spAddCategorytest = "SpAddCategorytest";
        public static string spUpdateCategory = "SpUpdateCategory";
        public static string spDeleteCategory = "SpDeleteCategory";
        //-------------End

        //--------------Product
        public static string getAllProByproduId = "SpGetAllProByproduId";
        public static string spGetAllProduct = "SpGetAllProduct";
        public static string getAllProductForUser = "SpGetAllProductForUser";
        public static string spAddProduct = "SpAddProduct";
        public static string spUpdateProduct = "SpUpdateProduct";
        public static string spDeleteProduct = "SpDeleteProduct";

        //---------------End


        public static string userLoginAdmin = "SpUserLoginAdmin";
        public static string updateCredit = "UpdateCredit";
        public static string insertlinkedid = "spInsertLinkedId";
        public static string userDeleteChat = "SpUserDeleteChat";
        public static string SpGetAllWalletHistory = "SpGetAllWalletHistory";
        public static string adminUserLogin = "SpAdminUserLogin";
        public static string directMemberSearch = "SpDirectMemberSearch";
        public static string spGetAllUserRegitration = "SpGetAllUserRegitration";
        public static string getUserNameByEmailId = "SpGetUserNameByEmailId";
        public static string getNetworkTree = "SpGetNetworkTree";
        public static string agentLeaseCredit = "GetAgentLeaseCredit";
        public static string getRechargeTransaction_ByTId = "SpGetRechargeTransaction_ByTId";
        public static string getRentWalletByURID = "SpGetRentWalletByURID";
        public static string get_TransactionIncome_History = "SpGet_TransactionIncome_History";
        public static string get_Diff_Rank_LeaderShip = "SPGet_Diff_Rank_LeaderShip";
        public static string getUser_WalletBalance = "SpGetUser_WalletBalance";
        public static string recDetails_ByTId = "SpGetRecDetails_ByTId";
        public static string getSingleLeg_Report = "SpGetSingleLeg_Report";
        public static string getPerformanceRewardList = "SPGetPerformanceRewardList";
        public static string bindBuyPackage = "SPBindBuyPackage";
        public static string getUserUnseenNotiCount = "SpGetUserUnseenNotiCount";
        public static string updateUserNotiSeenStatus = "SpUpdateUserNotiSeenStatus";
        public static string getUserNotificationList = "SpGetUserNotificationList";
        public static string getAllUserDashbaordNotifyList = "SpGetAllUserDashbaordNotifyList";
        public static string getExpoNotiByURID = "SpGetExpoNotiByURID";
        public static string allApprRentWallet = "SpAllApprRentWalletWithdrawal";
        public static string allUnApprRentWalletWithdrawal = "SpAllUnApprRentWalletWithdrawal";
        public static string addRechargeTransaction = "SpAddRechargeTransaction";
        public static string getUserDashboardDetails = "SpGetUserDashboardDetails";
        public static string getTransactionLog = "SpGetTransactionLog";
        public static string getABREngine = "SpGetABREngine";
        public static string getUserAnalytics = "SpGetUserAnalytics";
        public static string getUserLinkedIds = "SpGetUserLinkedIds";
        public static string getUserAffiliateDashboard = "SpGetUserAffiliateDashboard";
        public static string getLBRank = "spGetLBRank";
        public static string leaseAgent = "SpLeaseAgent";
        public static string SpAddRechargeTransactionAdmin = "SpAddRechargeTransactionAdmin";

        public static string getLeaseStatement = "SpGetLeaseStatement";
        public static string validateOtpbtEmailId = "SpValidateOtpbtEmailId";
        public static string getAgentAnalyticsUser = "SpGetAgentAnalyticsUser";

        public static string getAllWalletHistory = "SpGetAllWalletHistory";
        public static string addRechargeTransactionAdmin = "SpAddRechargeTransactionAdmin";
        public static string addAIUnlockUserPlans = "SpAddAIUnlockUserPlans";
        public static string getAIPlansByURID = "SpGetAIPlansByURID";
        public static string addAIUnlockActLog = "AddAIUnlockActLog";
        public static string getAgentLeaseCreditBYRID = "GetAgentLeaseCreditBYRID";
        public static string getAgentLeaseCredit = "GetAgentLeaseCredit";
        public static string addWalletAddress = "SpAddWalletAddress";
        public static string getUserWalletAddressListForAdmin = "SpGetUserWalletAddressListForAdmin";
        public static string getWalletAddresByURID = "SpGetWalletAddresByURID";
        public static string getSelfDepsiteByURID = "SpGetSelfDepsiteByURID";
        public static string getAllSelfDepositeAdmin = "SpGetAllSelfDepositeAdmin";
        public static string getAllContacUs = "SpGetAllContacUs";
        public static string addContactUs = "SpAddContactUs";
        public static string getRecDetails_ByTId = "SpGetRecDetails_ByTId";
        public static string updateUserProfileImage = "SpUpdateUserProfileImage";
        public static string getCareerType = "SpGetCareerType";
        public static string updateOtp = "SpUpdateOtp";
        public static string getAllAdminName = "SpGetAllAdminName";
        public static string spUpdatePassword = "SpChangePassword";
        public static string updateUserProfile = "SpUpdateUserProfile";
        public static string getDormantReport = "SpGetDormantReport";
        public static string getRechargeTransaction = "SpGetRechargeTransaction";
        public static string depositWalletBal = "SpDepositWalletBal";
        public static string spPayModeMaster = "SpPayModeMaster";
        public static string spGetByReferralId = "SpGetByReferralId";
        public static string spChangePassword = "SpChangePassword";
        public static string spGetUserKyc = "SpGetUserKyc";
        public static string spGetUserWalletDetails = "SpGetUserWalletDetails";

        public static string spAddAppUser = "SpAddAppUser";
        public static string SpUpdateAppuser = "spUpdateAppuser";
        public static string spGetMandatoryDetailsWithdrawl = "SpGetMandatoryDetailsWithdrawl";
        public static string spGetIncomeWalletBalance = "SpGetIncomeWalletBalance";
        public static string searchAllUsers = "SpSearchAllUsers";
        public static string bindPackageUserSide = "SPBindPackageUserSide";
        public static string upRentWithdReqStatus_Admin = "SpUpRentWithdReqStatus_Admin";
        public static string getTokenDepositsByURID = "SpGetTokenDepositsByURID";
        public static string spChangeSponsorID = "SpChangeSponsorID";
        public static string updateNews = "SpUpdateNews";
        public static string updateSettings = "SpUpdateSettings";
        public static string spAdminSendOtp = "SpAdminSendOtp";
        public static string spAdminVerifyOtp = "SpAdminVerifyOtp";
        public static string spAdminUpdatePassword = "SpdminUpdatePassword";
        public static string spUpdateAdminStatusActivate = "spUpdateAdminStatusActivate";
        public static string spUpdateAdminStatusDeActivate = "spUpdateAdminStatusDeActivate";
        public static string updateAdminProfile = "SpUpdateAdminProfile";
        public static string SpAddRechargeTransaction = "SpAddRechargeTransaction";
        public static string bulkRegistrationAdmin = "SpBulkRegistrationAdmin";
        public static string addToken_Deposits = "SpAddToken_Deposits";
        public static string adminForgotPassword = "SpAdminForgotPassword";
        public static string spGetTodayOrderList = "SpGetTodayOrderList";
        public static string spGetByIdAppRole = "SpGetByIdAppRole";
        public static string spGetAllAppRole = "SpGetAllAppRole";
        public static string spAddAppRole = "SpAddAppRole";
        public static string spUpdateAppRole = "SpUpdateAppRole";
        public static string spDeleteAppRole = "SpDeleteAppRole";       
        public static string bindKitAdmin = "SPBindKitAdmin";
        public static string spGetByIdSubCategory = "SpGetByIdSubCategory";
        public static string spGetAllSubCategory = "SpGetAllSubCategory";
        public static string spGetAllSubCategoryForUser = "SpGetAllSubCategoryForUser";
        public static string spAddSubCategory = "SpAddSubCategory";
        public static string updateSubCategory = "SpUpdateSubCategory";
        public static string spDeleteSubCategory = "SpDeleteSubCategory";
        public static string sendEmailsAllUser = "SpSendEmailsAllUser";
        public static string spGetByIdSubCategoryType = "SpGetByIdSubCategoryType";
        public static string spGetAllSubCategoryType = "SpGetAllSubCategoryType";
        public static string spGetAllSubCategoryTypeForUser = "SpGetAllSubCategoryTypeForUser";
        public static string spAddSubCategoryType = "SpAddSubCategoryType";
        public static string spUpdateSubCategoryType = "SpUpdateSubCategoryType";
        public static string spDeleteSubCategoryType = "SpDeleteSubCategoryType";

        public static string spGetAllProductById = "SpGetAllProductById";
        public static string spGetByIdProduct = "SpGetByIdProduct";
        public static string spGetAllProductDetails = "SpGetAllProductDetails";
        
        public static string spGetAllImageById = "SpGetAllImageById";
        public static string spGetAllSteps = "SpGetAllSteps";
        public static string spGetActiveAllSteps = "SpGetActiveAllSteps";
        public static string spAddSteps = "SpAddSteps";
        public static string spUpdateSteps = "SpUpdateSteps";
        public static string spDeleteSteps = "SpDeleteSteps";
        public static string spGetByIdProductImage = "SpGetByIdProductImage";
        public static string spGetAllProductImage = "SpGetAllProductImage";
        public static string spGetAllProductImageForUser = "SpGetAllProductImageForUser";
        public static string spAddProductImage = "SpAddProductImage";
        public static string spUpdateProductImage = "SpUpdateProductImage";
        public static string spDeleteProductImage = "SpDeleteProductImage";
        public static string spGetByIdDiscount = "SpGetByIdDiscount";
        public static string spGetAllDiscount = "SpGetAllDiscount";
        public static string spAddDiscount = "SpAddDiscount";
        public static string spUpdateDiscount = "SpUpdateDiscount";
        public static string spDeleteDiscount = "SpDeleteDiscount";
        public static string spGetAllCouponbyId = "SpGetAllCouponbyId";
        public static string spGetAllCoupon = "SpGetAllCoupon";
        public static string spAddCoupon = "SpAddCoupon";
        public static string updateCoupon = "SpUpdateCoupon";
        public static string spDeleteCoupon = "SpDeleteCoupon";
        public static string spGetByIdGiftCard = "SpGetByIdGiftCard";
        public static string spGetAllGiftCard = "SpGetAllGiftCard";
        public static string spAddGiftCard = "SpAddGiftCard";
        public static string spUpdateGiftCard = "SpUpdateGiftCard";
        public static string spDeleteGiftCard = "SpDeleteGiftCard";
        public static string spGetByIdNotification = "SpGetByIdNotification";
        public static string spGetAllNotification = "SpGetAllNotification";
        public static string spGetAllNotificationForUser = "SpGetAllNotificationForUser";
        public static string spAddNotification = "SpAddNotification";
        public static string spUpdateNotification = "SpUpdateNotification";
        public static string spDeleteNotification = "SpDeleteNotification";
        public static string spGetAllOrder = "SpGetAllOrderList";
        public static string spGetAllOrderlist = "SpGetAllOrder";
        public static string spReturnOrderCompleted = "SpReturnOrderCompleted";
        public static string spReturnOrderAccepted = "SpReturnOrderAccepted";
        public static string spGetAllcancelAccepted = "SpGetAllcancelAccepted";
        public static string spGetAllcancelAcceptedCompleted = "SpGetAllcancelAcceptedCompleted";
        public static string spGetAllReturnOrder = "SpGetAllReturnOrder";
        public static string spGetOrderArrivedTo = "SpGetOrderArrivedTo";
        public static string spGetAllPendingOrder = "SpGetAllPeningOrder";
        public static string spGetAllProcessingOrder = "SpGetAllProcessingOrder";
        public static string spGetAllCompletedOrder = "SpGetAllCompletedOrder";
        public static string spGetAllCancelOrder = "SpGetAllCancelOrder";
        public static string spUpdateOrderStatus = "SpUpdateOrderStatus";
        public static string getOrderWithItems = "GetOrderWithItems";
        public static string spBlockUserByAdmin = "SpBlockUserByAdmin";
        public static string sPDownloadExcel = "SPDownloadExcel";
        public static string getEditNews = "SpGetEditNews";
        public static string getSettings = "SpGetSettings";
        public static string spGetAllOrderDetailSearch = "SpGetAllOrderDetails";
        public static string spGetTrackOrder = "SpGetTrackOrder";
        public static string spGetAllOrderDetails = "SpGetAllOrderListDetails";
        public static string spAddOrderWithDetails = "SpAddOrderWithDetails";
        public static string spGetCheckPaymentStatus = "SpGetCheckPaymentStatus";
        public static string spGetPaymentList = "SpGetPaymentList";
        public static string spGetAllPaymentMode = "SpGetAllPaymentMode";
        public static string spAddPaymentMode = "SpAddPaymentMode";
        public static string spUpdatePaymentMode = "SpUpdatePaymentMode";
        public static string spDeletePaymentMode = "SpDeletePaymentMode";
        public static string spGetByIdShipping = "SpGetByIdShipping";
        public static string spgetAllPinCodeActive = "SpgetAllPinCodeActive";
        public static string spGetAllShipping = "SpGetAllShipping";
        public static string spGetContactus = "SpGetContactus";
        public static string spAddShipping = "SpAddShipping";
        public static string spInsertContactus = "SpInsertContactus";
        public static string spgetProductNamebyProductId = "SpgetProductNamebyProductId";
        public static string spGetAdressbyAddressId = "spGetAdressbyAddressId";
        public static string spGetMRPByProductId = "SpGetMRPByProductId";
        public static string spAddSimilarProduct = "SpAddSimilarProduct";
        public static string spDeleteSPByProducId = "SpDeleteSPByProducId";
        public static string spDeleteSimilarProduct = "SpDeleteSimilarProduct";
        public static string spGetAllAppUser = "SpGetAllAppUser";
        public static string spGetAllOrderByUserId = "SpGetAllOrderByUserId";
        public static string spAppUserGetProfileDetails = "SpAppUserGetProfileDetails";
        public static string spAppUserUpdateProfile = "SpAppUserUpdateProfile";
        public static string spAppUserUpdatePassword = "SpAppUserUpdatePassword";
        public static string spGetDefaultAddress = "SpGetDefaultAddress";
        public static string spGetAddressList = "SpGetAddressList";
        public static string spAddAddress = "SpAddAddress";
        public static string spUpdateAddress = "SpUpdateAddress";
        public static string spDeleteAddress = "SpDeleteAddress";
        public static string sp_GetRatingPercentage = "sp_GetRatingPercentage";
        public static string addTransactionsLog = "SPInsertAPITransactionsLog";
    }
}
