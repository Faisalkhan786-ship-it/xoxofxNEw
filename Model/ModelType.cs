using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ModelType
    {
        //public record RegistrationValidation(Int32 statusCode, string message);
        public class RegistrationValidation
        {
            public int statusCode { get; set; }
            public string message
            {
                get
                {
                    return statusCode switch
                    {
                        > 0 => "User registered successfully",
                        -1 => "Email already exists",
                        -2 => "Mobile number already exists",
                        _ => "Something went wrong"
                    };
                }
            }
        }
        public class ChartDataWrapper
        {
            public IEnumerable<dynamic> BarChart { get; set; }
            public IEnumerable<dynamic> PieChart { get; set; }
        }

        public class ReferralDetails
        {
            public Guid URID { get; set; }
            public string? FullName { get; set; }
            public string? AuthLogin { get; set; }
        }
        public class WalletType
        {
            public int Id { get; set; }
            public string? Type { get; set; }
        }

        public class Ticket
        {
            public int id { get; set; }
            public Guid ticketId { get; set; }
            public string? ticketType { get; set; }
            public string? subject { get; set; }
            public string? message { get; set; }
            public string? appUserId { get; set; }
            public string? image { get; set; }
            public string? createdDate { get; set; }
            public Guid? createdBy { get; set; }
            public bool active { get; set; }
            public string? AuthLogin { get; set; }
            public string? Name { get; set; }
        }

        public class TicketReply
        {
            public int id { get; set; }
            public Guid ticketReplyId { get; set; }
            public Guid ticketId { get; set; }
            public string? ticketType { get; set; }
            public string? message { get; set; }
            public string? appUserId { get; set; }
            public string? createdDate { get; set; }
            public Guid? createdBy { get; set; }
            public string? image { get; set; }
            public bool active { get; set; }
        }
        public class FundTypeWiseCrDr
        {
            public int Id { get; set; }
            public int CrDr { get; set; }
            //public int WalletId { get; set; }
            public string? Type { get; set; }
        }

        public class WalletDetails
        {
            public string FullName { get; set; }
            public Guid URID { get; set; }
            public decimal IncomeWallet { get; set; }
            public decimal DepositWallet { get; set; }
        }
        public class SubMenu
        {
            public Guid SubMenuId { get; set; }
            public Guid MenuId { get; set; }
            public string? SubMenuName { get; set; }
            public string? SubMenuPageName { get; set; }
            public string? MenuName { get; set; }
            public string? PageName { get; set; }
        }
        public record class CountryMethod(
            int Country_Id, string Country_Code, string Country_Name, int phonecode, bool IsActive, string CountryFlag);

        public record BannerForUser(
         Int64 id, Guid bannerId, Guid categoryId, Guid subCategoryId, Guid subCategoryTypeId, string image,
         string title, string subTitle, string createdDate);
        public record class StateMethod(
            int Pk_StateId, string StateName, int Fk_CountryId)
        {
            public StateMethod() : this(0, string.Empty, 0) { }
        }
        public record City(
            int Fk_StateId, string CityName, int Pk_CityId);
        public record SubMenubyid(int id, Guid subMenuId, Guid menuId, string subMenuName, string subMenuPageName, int displayOrder, string createdDate,
                Guid createdBy, string Status, bool active);
        public class DirectMember
        {
            public Guid URID { get; set; }
            public string? Loginid { get; set; }
            public string? Name { get; set; }
            public string? RegDate { get; set; }
            public string? Email { get; set; }
            public string? Mobile { get; set; }
            public string? TopupDate { get; set; }
            public string? Topup { get; set; }
            public decimal SelfTopup { get; set; }
            public decimal TopupValue { get; set; }
            public decimal TeamBusiness { get; set; }
            public decimal MonthlySelf { get; set; }
            public int MonthlyTeam { get; set; }
            public int totTeam { get; set; }
            public int ActiveTeam { get; set; }
            public string? Urank { get; set; }
            public string? LeaseAmount { get; set; }
            public string? DYRPercentage { get; set; }

        }

        //public class PersonalTeam
        //{
        //    public int id { get; set; }
        //    public string? Loginid { get; set; }
        //    public string? SponsorId { get; set; }
        //    public string? Name { get; set; }
        //    public string? RegDate { get; set; }
        //    public string? Email { get; set; }
        //    public string? Mobile { get; set; }
        //    public string? TopupDate { get; set; }
        //    public decimal TopupValue { get; set; }
        //    public decimal TeamBusiness { get; set; }
        //    public int uLvl { get; set; }
        //    public string? Urank { get; set; }
        //    public string? status { get; set; }
        //    public string? LeaseAmount { get; set; }
        //    public decimal TotTeam { get; set; }
        //    public int ActiveTeam { get; set; }
        //    public decimal MonthlySelf { get; set; }
        //    public decimal MonthlyTeam { get; set; }
        //}
        public class PersonalTeam
        {
            public int id { get; set; }
            public string? Loginid { get; set; }
            public string? SponsorId { get; set; }
            public string? Name { get; set; }
            public string? RegDate { get; set; }
            public string? Email { get; set; }
            public string? Mobile { get; set; }

            // Topup related
            public string? Topup { get; set; }          // Activated
            public string? TopupDate { get; set; }
            public decimal TopupValue { get; set; }     // agar kahin use ho raha ho
            public decimal TopupAmount { get; set; }    // 125.00

            // Team / Business
            public decimal TeamBusiness { get; set; }
            public decimal TotTeam { get; set; }
            public int ActiveTeam { get; set; }

            // Monthly
            public decimal MonthlySelf { get; set; }
            public decimal MonthlyTeam { get; set; }

            // Level / Rank
            public int uLvl { get; set; }
            public string? Urank { get; set; }

            // Status
            public string? status { get; set; }         // Active (User)
            public int statusCode { get; set; }         // 1
            public string? message { get; set; }        // Success

            // Country
            public string? Country_Code { get; set; }   // IN
            public string? Country_Name { get; set; }   // India
            public string? CountryFlag { get; set; }    // flag url

            // Others
            public string? LeaseAmount { get; set; }
        }

        public class UserRegistrationModel
        {
            public Guid URID { get; set; }
            public string? AuthLogin { get; set; }
            public string? AuthPass { get; set; }
            public string? FName { get; set; }
            public string? LName { get; set; }
            public string? Mobile { get; set; }
            public string? Email { get; set; }
            public string? Address { get; set; }
            public int CountryId { get; set; }
            public string? RegDate { get; set; }
            public bool Active { get; set; }
        }

        public class UserKyc
        {
            public long Id { get; set; }
            public string? Name { get; set; }
            public string? BankName { get; set; }
            public string? AccountNo { get; set; }
            public string? IfscCode { get; set; }
            public string? PanCardNo { get; set; }
            public string? PanImage { get; set; }
            public string? PassBookImage { get; set; }
            public string? Status { get; set; }
            public string? UserID { get; set; }
            public string? DateUserkyc { get; set; }
            public string? UPINo { get; set; }
            public string? LoginID { get; set; }
            public string? UpdateDate { get; set; }
        }
        public record GetAllSubMenu(int id, Guid subMenuId, Guid menuId, string subMenuName, string subMenuPageName, string menuName, string pageName, string status, bool active);

        public record MenuByUserRole(int id, string menuName, string pageName,
            int displayOrder, string roleName);
        public class Menu
        {
            public int id { get; set; }
            public Guid menuId { get; set; }
            public string? menuName { get; set; }
            public string? menuIcon { get; set; }
            public string? status { get; set; }
            public bool active { get; set; }
        }
        public class SimilarProductNew
        {
            public int id { get; set; }
            public Guid SimilarProductId { get; set; }
            public Guid productId { get; set; }
            public Guid subProductId { get; set; }
            public string? createdDate { get; set; }
            public Guid createdBy { get; set; }
            public string? Status { get; set; }
            public bool active { get; set; }
            public string? productName { get; set; }
            public string? description { get; set; }
            public decimal discountPrice { get; set; }
            public decimal price { get; set; }
            public int rating { get; set; }

            public List<string>? image { get; set; } = new List<string>();
        }

        public class SimilarProductDTO
        {
            public int id { get; set; }
            public Guid SimilarProductId { get; set; }
            public Guid productId { get; set; }
            public Guid subProductId { get; set; }
            public string? createdDate { get; set; }
            public Guid createdBy { get; set; }
            public string? Status { get; set; }
            public bool active { get; set; }
            public string? productName { get; set; }
            public string? description { get; set; }
            public decimal discountPrice { get; set; }
            public decimal price { get; set; }
            public int rating { get; set; }
            public List<string>? images { get; set; } = new List<string>();

        }
        public record SortBy(Int64 id, Guid sortById, string sortByName, string createdDate, string status, bool active);

        public record Category(
     long id,Guid categoryId,string name,string createdDate,string status,bool active);
        public record AdminUserDetails(Guid adminUserId, Guid appRoleId, string username, string firstName,
          string lastName, string email, string phoneNumber, string password, string type);
        public class AdminAllUserDetails
        {
            public Guid AdminUserId { get; set; }
            public Guid AppRoleId { get; set; }
            public string? Username { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Password { get; set; }
            public string? Email { get; set; }
            public string? PhoneNumber { get; set; }
            public string? CreatedDate { get; set; }
            public Guid CreatedBy { get; set; }
            public string? UpdatedDate { get; set; }
            public Guid UpdatedBy { get; set; }
            public bool Active { get; set; }
            public int Otp { get; set; }
            public string ActiveStatus { get; set; }
        }

        //public record AdminAllUserDetails(Guid adminUserId, Guid appRoleId, string username, string firstName, 
        //string lastName, string password, string email, string phoneNumber, string createdDate, Guid createdBy, string updatedDate, Guid updatedBy, bool active, int otp, string activeStatus);
        public class SubCategoryNew
        {
            public Guid subcategoryId { get; set; }
            public Guid categoryId { get; set; }
            public string? name { get; set; }
            public string createdDate { get; set; }
            public string? status { get; set; }
            public bool active { get; set; }
            public string? image { get; set; }
            public decimal? AssetRate { get; set; }
            public string? SmartContractAddress { get; set; }
        }


        public class SubCategory
        {
            public long SubcategoryId { get; set; }
            public Guid CategoryId { get; set; }
            public string CategoryName { get; set; }
            public Guid SubCategoryGUID { get; set; }
            public string Name { get; set; }
            public string CreatedDate { get; set; }
            public string Status { get; set; }
            public bool Active { get; set; }
            public string Image { get; set; }
            public decimal? AssetRate { get; set; }
            public string? SmartContractAddress { get; set; }
        }

        // public record SubCategory( Int64 subcategoryId, Guid categoryId, string categoryName, Guid subCategoryGUID, string name, string createdDate, string status, bool active, string image,decimal? AssetRate,string? SmartContractAddress)
        public record SubCategoryType(
            Int64 subcategoryTypeId, Guid categoryId, string categorName, Guid subcategoryGUID, string subCategoryName, Int64 subcategoryId, Guid subCategoryTypeGUID, string name, string createdDate, string status, bool active);


        public record SubCategoryTypeForUser(
            Int64 subcategoryTypeId, Guid categoryId, string categorName, Guid subcategoryGUID, string subCategoryName,
            Int64 subcategoryId, Guid subCategoryTypeGUID, string name, string createdDate, string status, bool active);
        public class Seller
        {
            public long Id { get; set; }
            public Guid SellerId { get; set; }
            public string? Name { get; set; }
            public string? Mobile { get; set; }
            public string? Email { get; set; }
            public string? StreetAddress { get; set; }
            public string? State { get; set; }
            public string? City { get; set; }
            public string? Pincode { get; set; }
            public string? Country { get; set; }
            public string? Description { get; set; }
            public string CreatedDate { get; set; }
            public string? status { get; set; }
            public bool Active { get; set; }
            public string? UserName { get; set; }
            public string? UserPassword { get; set; }
            public string? Otp { get; set; }
        }

        public class Product
        {
            public long id { get; set; }
            public Guid? productId { get; set; }
            public Guid? categoryId { get; set; }
            public string? categoryName { get; set; }
            public Guid subCategoryId { get; set; }
            public string? subCategoryName { get; set; }
            //public Guid subCategoryTypeId { get; set; }
            //public string? subCategoryTypeName { get; set; }
            public Guid sellerId { get; set; }
            public string? sellerName { get; set; }


            public string? productName { get; set; }
            public string? subName { get; set; }
            public string? description { get; set; }
            public decimal rating { get; set; }
            public int noOfRating { get; set; }
            public int stock { get; set; }
            public decimal price { get; set; }
            public decimal discountPrice { get; set; }
            public string createdDate { get; set; }
            public string updatedDate { get; set; }
            public string? status { get; set; }
            public bool active { get; set; }
            public string? imageUrl { get; set; }

            public string? MRP { get; set; }
            public string? metaTitle { get; set; }
            public string? metaDescription { get; set; }
            public string? metakeyword { get; set; }
            public int? PerHour { get; set; }
            public int? Unit { get; set; }
            public string? Specification { get; set; }
            public string? task { get; set; }
            public string? NFTurL { get; set; }
            public string? TOATALMONTH { get; set; }
            public decimal totalReturn { get; set; }
            public decimal weeklyReturn { get; set; }
            public int month { get; set; }
            public string? TokenId { get; set; }
        }
        public class AllProduct
        {
            public long id { get; set; }
            public Guid productId { get; set; }
            public Guid categoryId { get; set; }
            public string? categoryName { get; set; }           
            public string? productName { get; set; }
            public string? tittle { get; set; }
            public string? type { get; set; }
            public decimal? roi { get; set; }
            public decimal? mininvest { get; set; }
            public decimal? winrate { get; set; }
            public decimal? traders { get; set; }

        
            public string? createdDate { get; set; }
            public string? updatedDate { get; set; }

            public string? status { get; set; }
            public bool active { get; set; }
           
        }

        public record searchProductNew(
             Guid commonId, string typeName, string commonProduct, int Ids, string createdDate, Guid createdBy, string status, bool active);

        public class Productdetails
        {
            public Int64 Id { get; set; }
            public Guid ProductId { get; set; }
            public Guid CategoryId { get; set; }
            public string? CategoryName { get; set; }
            public Guid? SubCategoryId { get; set; }
            public string? SubCategoryName { get; set; }
            public Guid? SubCategoryTypeId { get; set; }
            public string? SubCategoryTypeName { get; set; }
            public Guid SellerId { get; set; }
            public string? SellerName { get; set; }
            public string? ProductName { get; set; }
            public string? SubName { get; set; }
            public string? Description { get; set; }
            public decimal Rating { get; set; }
            public Int32 NoOfRating { get; set; }
            public Int32 Stock { get; set; }
            public Decimal Price { get; set; }
            public Decimal DiscountPrice { get; set; }
            public string CreatedDate { get; set; }
            public string UpdatedDate { get; set; }
            public string? Status { get; set; }
            public bool Active { get; set; }
            public string? ImageUrl { get; set; }
            public List<string> ImageUrls { get; set; }
            public string? MRP { get; set; }
            public string? metaTitle { get; set; }
            public string? metaDescription { get; set; }
            public string? metakeyword { get; set; }
            public int? PerHour { get; set; }
            public int? Unit { get; set; }
            public string? Specification { get; set; }
            public string? task { get; set; }
            public string? NFTurL { get; set; }
            public string? TOATALMONTH { get; set; }
            public decimal? totalReturn { get; set; }
            public decimal? weeklyReturn { get; set; }
            public int? month { get; set; }
            public string? TokenId { get; set; }

        }
        public record AllSteps(
        Int64 id, Guid StepsId, string name, string description, string createdDate, string status, bool active);
        public record AllSkinInsightProduct(long id, Guid skininsightproductId, Guid productId, string Age, string Gender, string Skintype,
                 string SkinSensitive, string createdDate, Guid createdBy, string Status, bool active
             );

        public record FaqWithProduct(long id, Guid ProductFaqid, Guid productId, string Title, string Description, Guid CreatedBy, string createdDate, string Status, bool active, string faqType);

        public record FaqIngredient(long id, Guid ProductFaqid, Guid productId, string Title, string Description, Guid CreatedBy, string createdDate, string Status, bool active, string faqType);
        public record ProductSpecification(long id, Guid ProductSpecificationid, Guid productId, string producttype, string netquantity, string shelfLife, string countryOfOrigin, string SKUcode, Guid ManufacturedBy, string ConsumerCareAddress, Guid CreatedBy, string CreatedDate, Guid updatedBy, string updatedDate, string status, bool active);
        public record Faq(long id, Guid ProductFaqid, Guid productId, string Title, string Description, Guid CreatedBy, string createdDate, string Status, bool active, string faqType);


        public class ProductbyIdImage
        {
            public string name { get; set; }
            public string imageUrl { get; set; }
            public Guid productImageId { get; set; }
            public Guid productId { get; set; }

            public ProductbyIdImage() { }
        }
        public class ProductDetails
        {
            public long id { get; set; }
            public Guid productId { get; set; }
            public Guid categoryId { get; set; }
            public string? categoryName { get; set; }
            public Guid subCategoryId { get; set; }
            public string? subCategoryName { get; set; }
            //public Guid subCategoryTypeId { get; set; }
            //public string? subCategoryTypeName { get; set; }
            public Guid sellerId { get; set; }
            public string? sellerName { get; set; }
            public string? productName { get; set; }
            public string? subName { get; set; }
            public string? description { get; set; }
            public decimal rating { get; set; }
            public int noOfRating { get; set; }
            public int stock { get; set; }
            public decimal price { get; set; }
            public decimal discountPrice { get; set; }
            public string? createdDate { get; set; }
            public string? updatedDate { get; set; }
            public string? TOATALMONTH { get; set; }
            public string? NFTurL { get; set; }
            public string? status { get; set; }
            public bool active { get; set; }
            public string? image { get; set; }
            public string? MRP { get; set; }
            public string? metaTitle { get; set; }
            public string? metaDescription { get; set; }
            public string? metakeyword { get; set; }
            public int? PerHour { get; set; }
            public int? Unit { get; set; }
            public string? Specification { get; set; }
            public string? task { get; set; }
            public decimal totalReturn { get; set; }
            public decimal weeklyReturn { get; set; }
            public int month { get; set; }
            public string? TokenId { get; set; }
        }


        public class ProductImage
        {
            public long id { get; set; }
            public Guid productImageId { get; set; }
            public Guid productId { get; set; }
            public string? title { get; set; }
            public string? imageUrl { get; set; }
            public string createdDate { get; set; }
        }
        public record Banner(
           Int64 id, Guid bannerId, Guid categoryId, Guid subCategoryId, Guid subCategoryTypeId, string image,
           string title, string subTitle, string createdDate, string status, bool active);

        public class PrdoctSearchByFilter
        {
            public int categoryId { get; set; }
            public int subcategoryId { get; set; }
            public int subcategoryTypeId { get; set; }
            public int productId { get; set; }
            public Guid GproductId { get; set; }
            public int sellerId { get; set; }
            public string? categoryName { get; set; }
            public string? subCategoryName { get; set; }
            public string? subcategoryTypeName { get; set; }
            public string? productName { get; set; }
            public long stock { get; set; }
            public string? productDescription { get; set; }
            public decimal productPrice { get; set; }
            public decimal discountPrice { get; set; }
            public string? productImage { get; set; }
            public decimal rating { get; set; }
            public int noOfRating { get; set; }
            public string? sellerName { get; set; }
            public int? PerHour { get; set; }
            public int? Unit { get; set; }
            public decimal? totalReturn { get; set; }
            public decimal? weeklyReturn { get; set; }
            public int? month { get; set; }
            public string? Specification { get; set; }
            public string? TOATALMONTH { get; set; }
            public string? NFTurL { get; set; }
            public string? TokenId { get; set; }
            public List<string>? images { get; set; } = new List<string>();
        }

        public class PrdoctSearchByFilterNewModel
        {
            public int categoryId { get; set; }
            public int subcategoryId { get; set; }
            public int subcategoryTypeId { get; set; }
            public int productId { get; set; }
            public Guid GproductId { get; set; }
            public int sellerId { get; set; }
            public string? categoryName { get; set; }
            public string? subCategoryName { get; set; }
            public string? subcategoryTypeName { get; set; }
            public string? productName { get; set; }
            public long stock { get; set; }
            public string? productDescription { get; set; }
            public decimal productPrice { get; set; }
            public decimal discountPrice { get; set; }
            public string? productImage { get; set; }
            public decimal rating { get; set; }
            public int noOfRating { get; set; }
            public string? sellerName { get; set; }


            public List<string> Images { get; set; }
            public string? MRP { get; set; }
            public int? PerHour { get; set; }
            public int? Unit { get; set; }
            public string? Specification { get; set; }
            public decimal? totalReturn { get; set; }
            public decimal? weeklyReturn { get; set; }
            public int? month { get; set; }
            public string? TokenId { get; set; }
        }
        public class SimilarProductImagesimilor
        {
            public string? image { get; set; }
        }
    }
}
