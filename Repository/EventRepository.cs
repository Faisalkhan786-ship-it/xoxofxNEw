using Common;
using Dapper;
using EmailSystem;
using RepositoryContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using static QRCoder.PayloadGenerator;

namespace Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly DapperContext _dapperContext;
        private readonly EmailService _emailService;

        public EventRepository(DapperContext dapperContext, EmailService emailService)
        {
            _dapperContext = dapperContext;
            _emailService = emailService;
        }

        public async Task<ResponseViewModelSendEventEmail> addEvent(EventViewModel eventViewModel)
        {
            string imagePath = null;

            // Use addEvent.image
            var file = eventViewModel.Image;

            if (file != null && file.Length > 0)
            {
                // Cloudflare Upload using ExtractToken
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ExtractToken.ApiToken);

                using var content = new MultipartFormDataContent();
                using var stream = file.OpenReadStream();
                content.Add(new StreamContent(stream), "file", file.FileName);

                var response = await client.PostAsync(
                    $"https://api.cloudflare.com/client/v4/accounts/{ExtractToken.AccountId}/images/v1", content
                );

                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new ResponseViewModelSendEventEmail { statusCode = 0, message = "Image upload failed", data = jsonResponse };

                // Cloudflare URL extract karna
                var json = System.Text.Json.JsonDocument.Parse(jsonResponse);
                imagePath = json.RootElement.GetProperty("result").GetProperty("variants")[0].GetString();
            }

            // Stored procedure call
            var procedureName = Constant.addEventMaster;
            var parameters = new DynamicParameters();
            parameters.Add("@Image", imagePath, DbType.String);
            parameters.Add("@Tittle", eventViewModel.Tittle, DbType.String);
            parameters.Add("@EventType", eventViewModel.EventType, DbType.String);
            parameters.Add("@EventStartDate", eventViewModel.EventStartDate, DbType.String);
            parameters.Add("@EndStartDate", eventViewModel.EndStartDate, DbType.String);
            parameters.Add("@AvailableSeats", eventViewModel.AvailableSeats, DbType.String);
            parameters.Add("@Location", eventViewModel.Location, DbType.String);
            parameters.Add("@Createdby", eventViewModel.Createdby, DbType.Guid);
            parameters.Add("@Description", eventViewModel.Description, DbType.String);

            parameters.Add("@SessionsTime", eventViewModel.SessionsTime, DbType.String);
            parameters.Add("@SessionsTimeOne", eventViewModel.SessionsTimeOne, DbType.String);
            parameters.Add("@SessionsTimeTwo", eventViewModel.SessionsTimeTwo, DbType.String);

            parameters.Add("@SessionSeats", eventViewModel.SessionSeats, DbType.Int32);
            parameters.Add("@SessionOneSeats", eventViewModel.SessionOneSeats, DbType.Int32);
            parameters.Add("@SessionTwoSeats", eventViewModel.SessionTwoSeats, DbType.Int32);

            parameters.Add("@EventMode", eventViewModel.EventMode, DbType.String);
            parameters.Add("@AccessType", eventViewModel.AccessType, DbType.String);
            parameters.Add("@EventURL", eventViewModel.EventURL, DbType.String);
            parameters.Add("@EventPrice", eventViewModel.EventPrice, DbType.Decimal);
            parameters.Add("@MultipleSeatbook", eventViewModel.MultipleSeatbook, DbType.Int32);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModelSendEventEmail>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure
                );

                result.statusCode = result.statusCode == 1 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.ExpectationFailed;
                return result;
            }
        }

        public async Task<ResponseViewModelSendEventEmail> UpdateEvent(UpdateEventViewModel updateEventViewModel)
        {
            string imagePath = null;

            // Use addEvent.image
            var file = updateEventViewModel.Image;

            if (file != null && file.Length > 0)
            {
                // Cloudflare Upload using ExtractToken
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ExtractToken.ApiToken);

                using var content = new MultipartFormDataContent();
                using var stream = file.OpenReadStream();
                content.Add(new StreamContent(stream), "file", file.FileName);

                var response = await client.PostAsync(
                    $"https://api.cloudflare.com/client/v4/accounts/{ExtractToken.AccountId}/images/v1", content
                );

                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new ResponseViewModelSendEventEmail { statusCode = 0, message = "Image upload failed", data = jsonResponse };

                // Cloudflare URL extract karna
                var json = System.Text.Json.JsonDocument.Parse(jsonResponse);
                imagePath = json.RootElement.GetProperty("result").GetProperty("variants")[0].GetString();
            }

            // Stored procedure call
            var procedureName = Constant.updateEventMaster;
            var parameters = new DynamicParameters();
            parameters.Add("@EventMasterID", updateEventViewModel.EventMasterID, DbType.Guid);
            parameters.Add("@Image", imagePath, DbType.String);
            parameters.Add("@Tittle", updateEventViewModel.Tittle, DbType.String);
            parameters.Add("@EventType", updateEventViewModel.EventType, DbType.String);

            parameters.Add("@EventStartDate", updateEventViewModel.EventStartDate, DbType.String);
            parameters.Add("@EndStartDate", updateEventViewModel.EndStartDate, DbType.String);

            parameters.Add("@AvailableSeats", updateEventViewModel.AvailableSeats, DbType.String);
            parameters.Add("@Location", updateEventViewModel.Location, DbType.String);

            parameters.Add("@UpdatedBy", updateEventViewModel.Updatedby, DbType.Guid);
            parameters.Add("@Status", updateEventViewModel.Status, DbType.Int32);
            parameters.Add("@Description", updateEventViewModel.Description, DbType.String);

            parameters.Add("@SessionsTime", updateEventViewModel.SessionsTime, DbType.String);
            parameters.Add("@SessionsTimeOne", updateEventViewModel.SessionsTimeOne, DbType.String);
            parameters.Add("@SessionsTimeTwo", updateEventViewModel.SessionsTimeTwo, DbType.String);

            parameters.Add("@SessionSeats", updateEventViewModel.SessionSeats, DbType.Int32);
            parameters.Add("@SessionOneSeats", updateEventViewModel.SessionOneSeats, DbType.Int32);
            parameters.Add("@SessionTwoSeats", updateEventViewModel.SessionTwoSeats, DbType.Int32);

            parameters.Add("@EventMode", updateEventViewModel.EventMode, DbType.String);
            parameters.Add("@AccessType", updateEventViewModel.AccessType, DbType.String);
            parameters.Add("@EventURL", updateEventViewModel.EventURL, DbType.String);
            parameters.Add("@EventPrice", updateEventViewModel.EventPrice, DbType.Decimal);
            parameters.Add("@MultipleSeatbook", updateEventViewModel.MultipleSeatbook, DbType.Int32);




            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModelSendEventEmail>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure
                );

                result.statusCode = result.statusCode == 1 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.ExpectationFailed;
                return result;
            }
        }

        public async Task<ResponseViewModelSendEventEmail> getAllEvent(int Id)
        {
            var procedureName = Constant.allEventMaster;
            DynamicParameters param = new DynamicParameters();
            param.Add("@Id", Id);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, param, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var combinedData = new
                    {
                        Event = result.ToList(),
                    };

                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Get Evet Master.",
                        data = combinedData
                    };
                }
                else
                {
                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No Event found."
                    };
                }
            }
        }

        public async Task<ResponseViewModelSendEventEmail> getAllUserEvent(int Id)
        {
            var procedureName = Constant.allUserEvent;
            DynamicParameters param = new DynamicParameters();
            param.Add("@Id", Id, DbType.Int32);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, param, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var combinedData = new
                    {
                        UserEvent = result.ToList(),
                    };

                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Get User Evet Master.",
                        data = combinedData
                    };
                }
                else
                {
                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No Event found."
                    };
                }
            }
        }

        public async Task<ResponseViewModelSendEventEmail> getScheduleByEID(Guid EventMasterID)
        {
            var procedureName = Constant.getScheduleByEID;
            DynamicParameters param = new DynamicParameters();
            param.Add("@EventMasterID", EventMasterID);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, param, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var combinedData = new
                    {
                        UserEvent = result.ToList(),
                    };

                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Get User Schedule .",
                        data = combinedData
                    };
                }
                else
                {
                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No Event found."
                    };
                }
            }
        }
        public async Task<ResponseViewModelSendEventEmail> addEventSchedule(EventScheduleMasterViewModel eventScheduleMasterViewModel)
        {

            // Stored procedure call
            var procedureName = Constant.addEventSchedule;
            var parameters = new DynamicParameters();
            parameters.Add("@Id", eventScheduleMasterViewModel.Id, DbType.Int32);
            parameters.Add("@EventMasterID", eventScheduleMasterViewModel.EventMasterID, DbType.Guid);
            parameters.Add("@Title", eventScheduleMasterViewModel.Tittle, DbType.String);
            parameters.Add("@Time", eventScheduleMasterViewModel.Time, DbType.Time);
            parameters.Add("@Createdby", eventScheduleMasterViewModel.Createdby, DbType.Guid);
            parameters.Add("@Status", eventScheduleMasterViewModel.Status, DbType.Int32);


            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModelSendEventEmail>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure
                );

                result.statusCode = result.statusCode == 1 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.ExpectationFailed;
                return result;
            }
        }
        public async Task<ResponseViewModelSendEventEmail> addUserEventbooking(AddUserEventbookingViewModel addUserEventbookingViewModel)
        {
            var procedureName = Constant.addUserEventbooking;
            var parameters = new DynamicParameters();

            parameters.Add("@URID", addUserEventbookingViewModel.URID, DbType.Guid);
            parameters.Add("@EventMasterID", addUserEventbookingViewModel.EventMasterID, DbType.Guid);
            parameters.Add("@Price", addUserEventbookingViewModel.Price, DbType.Decimal);
            parameters.Add("@RequestedSeats", addUserEventbookingViewModel.RequestedSeats, DbType.Int32);
            parameters.Add("@TicketNumber", addUserEventbookingViewModel.TicketNumber, DbType.String);
            parameters.Add("@SessionTime", addUserEventbookingViewModel.SessionTime, DbType.String);
            parameters.Add("@AccessType", addUserEventbookingViewModel.AccessType, DbType.String);
            parameters.Add("@type", addUserEventbookingViewModel.type, DbType.Int32);
            // Step 2: Email ActionType decide karo
            int actionType = 1;
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<EmailActionModel>(
                    "Sp_GetEmailByActionType",
                    commandType: CommandType.StoredProcedure
                );
                actionType = result?.ActionType ?? 1;
            }
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModelSendEventEmail>(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (result == null)
                {
                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.ExpectationFailed,
                        message = "No response from database."
                    };
                }

                // ✅ If booking success, trigger email
                if (result.statusCode == 1 && result.EventbookingID != Guid.Empty)
                {
                    //_emailService.SendBookingEmailAsync(result.EventbookingID);
                    await SendBookingEmailAsync(result.EventbookingID);
                }

                result.statusCode = result.statusCode == 1
                    ? (int)HttpStatusCode.OK
                    : (int)HttpStatusCode.ExpectationFailed;

                return result;
            }
        }
        public class EmailActionModel
        {
            public int ActionType { get; set; }
            public string? EmailTo { get; set; }
            //public string? EmailId { get; set; }
        }
        //   Send Email Function
        private async Task SendBookingEmailAsync(Guid eventBookingId)
        {
            using (var connection = _dapperContext.createConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@EventbookingID", eventBookingId, DbType.Guid);

                var bookingDetails = await connection.QueryFirstOrDefaultAsync<BookingEmailViewModel>(
                    "SendBookingEmailByID",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (bookingDetails != null)
                {
                    string subject = $"🎫 Booking Confirmed: {bookingDetails.Tittle}";

                    string body = $@"
<div style='width:100%;background-color:#f5f6fa;padding:30px 0;margin:0;font-family:Segoe UI,Roboto,Arial,sans-serif;'>
  <div style='max-width:600px;margin:auto;background:#ffffff;
              border:2px solid #4A3AFF;border-radius:12px;
              box-shadow:0 6px 16px rgba(0,0,0,0.08);
              overflow:hidden;box-sizing:border-box;'>

    <!-- Header -->
    <div style='text-align:center;padding:25px 20px 20px 20px;
                border-bottom:1px solid #f0f0f0;background:#fff;
                border-top-left-radius:12px;border-top-right-radius:12px;'>
      <img src='https://imagedelivery.net/nq9qT5FHZv9Sg48UUnD1-A/893ab68a-6977-4ac1-a97e-9c68846cf400/public'
           alt='Rentelligence Logo' 
           style='max-width:90%;height:auto;display:block;margin:0 auto;'/>
    </div>

    <!-- Body -->
    <div style='padding:30px 35px;text-align:center;background:#ffffff;'>
      <h2 style='color:#2c3e50;font-size:22px;margin:0 0 10px;font-weight:600;'>Dear {bookingDetails.Name},</h2>
      <p style='color:#555;font-size:15px;margin:5px 0 25px;'>
        Your event booking has been confirmed successfully!
      </p>

      <div style='text-align:left;margin:0 auto;display:inline-block;width:100%;max-width:400px;'>
        <table cellpadding='6' cellspacing='0' width='100%' style='font-size:15px;color:#333;line-height:1.6;'>
          <tr>
            <td width='40%' style='font-weight:600;'>Event:</td>
            <td>{bookingDetails.Tittle}</td>
          </tr>
          <tr>
            <td style='font-weight:600;'>Date & Time:</td>
            <td>{bookingDetails.EventDateTime}</td>
          </tr>
  <tr>
            <td style='font-weight:600;'>SessionTime:</td>
            <td>{bookingDetails.SessionTime}</td>
          </tr>
          <tr>
            <td style='font-weight:600;'>Location:</td>
            <td>{bookingDetails.Location}</td>
          </tr>
          <tr>
            <td style='font-weight:600;'>Seats Booked:</td>
            <td>{bookingDetails.SeatsBooked}</td>
          </tr>
          <tr>
            <td style='font-weight:600;'>Ticket Number:</td>
            <td style='color:#4A3AFF;font-weight:600;'>{bookingDetails.TicketNumber}</td>
          </tr>
        </table>
      </div>

      <p style='color:#555;font-size:14px;margin:25px 0 8px;'>
        You can show this email as your entry pass at the event gate.
      </p>
      <p style='color:#777;font-size:13px;margin:0;'>Thank you for choosing <b>Rentelligence</b>!</p>
    </div>

    <!-- Footer -->
    <div style='background:#f9f9f9;padding:15px;text-align:center;font-size:12px;color:#999;border-top:1px solid #eee;'>
      Regards,<br/>The Rentelligence Team
    </div>
 <!-- Social Icons -->
    <div style='background:#f3f4f6; padding:15px; text-align:center;'>
      <div style='display:inline-block;'>
        
        <a href='https://www.instagram.com/rentelligence.ai?igsh=bmdxcXJqaDd2empk' target='_blank' style='margin:0 10px;'>
          <img src='https://cdn-icons-png.flaticon.com/24/174/174855.png' style='width:26px; height:26px;'/>
        </a>

        <a href='https://www.facebook.com/rentelligenceai' target='_blank' style='margin:0 10px;'>
          <img src='https://cdn-icons-png.flaticon.com/24/733/733547.png' style='width:26px; height:26px;'/>
        </a>

        <a href='https://twitter.com' target='_blank' style='margin:0 10px;'>
          <img src='https://cdn-icons-png.flaticon.com/512/5968/5968958.png' style='width:26px; height:26px;'/>
        </a>

      </div>
    </div>
  </div>
</div>";



                    // ✅ Send with dual SMTP fallback
                    _emailService.SendEmailWithFallback(bookingDetails.Email, subject, body, true);
                }
            }
        }

        public class BookingEmailViewModel
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? AuthLogin { get; set; }
            public string? Email { get; set; }
            public string? TicketNumber { get; set; }
            public int SeatsBooked { get; set; }
            public string? Tittle { get; set; }
            public string? Location { get; set; }
            public string? EventDateTime { get; set; }
            public string? SessionTime { get; set; }
        }
        public async Task<ResponseViewModelSendEventEmail> getAllUserEventbookingMaster()
        {
            var procedureName = Constant.allUserEventbookingMaster;

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var combinedData = new
                    {
                        Event = result.ToList(),
                    };

                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Get Evet Master.",
                        data = combinedData
                    };
                }
                else
                {
                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No Event found."
                    };
                }
            }
        }
        public async Task<ResponseViewModelSendEventEmail> getUserEventbookingbyURID(Guid URID)
        {
            var procedureName = Constant.getUserEventbookingbyURID;
            DynamicParameters param = new DynamicParameters();
            param.Add("@URID", URID);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, param, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var combinedData = new
                    {
                        UserEvent = result.ToList(),
                    };

                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Get User Schedule .",
                        data = combinedData
                    };
                }
                else
                {
                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No Event found."
                    };
                }
            }
        }

        public async Task<ResponseViewModelSendEventEmail> CloseEventMaster()
        {
            var procedureName = Constant.allCloseEventMaster;
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var combinedData = new
                    {
                        Event = result.ToList(),
                    };

                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Get Closed Evet Master.",
                        data = combinedData
                    };
                }
                else
                {
                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No Event found."
                    };
                }
            }
        }

        public async Task<ResponseViewModelSendEventEmail> addEventPreImages(AddEventPreImagesViewModel addEventPreImagesViewModel)
        {
            string imagePath = null;
            string videoPath = null;

            var imageFile = addEventPreImagesViewModel.Image;
            var videoFile = addEventPreImagesViewModel.EventVideos;

            // --------------------------------
            // UPLOAD VIDEO TO CLOUDFLARE STREAM
            // --------------------------------
            if (videoFile != null && videoFile.Length > 0)
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", ExtractToken.ApiToken);

                using var content = new MultipartFormDataContent();
                using var stream = videoFile.OpenReadStream();
                content.Add(new StreamContent(stream), "file", videoFile.FileName);

                var response = await client.PostAsync(
                    $"https://api.cloudflare.com/client/v4/accounts/{ExtractToken.AccountId}/stream",
                    content);

                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = 0,
                        message = "Video upload failed",
                        data = jsonResponse
                    };

                var json = System.Text.Json.JsonDocument.Parse(jsonResponse);

                string uid = json.RootElement.GetProperty("result").GetProperty("uid").GetString();

                videoPath = $"https://videodelivery.net/{uid}/manifest/video.m3u8";
            }

            // --------------------------------
            // UPLOAD IMAGE TO CLOUDFLARE IMAGES
            // --------------------------------
            if (imageFile != null && imageFile.Length > 0)
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", ExtractToken.ApiToken);

                using var content = new MultipartFormDataContent();
                using var stream = imageFile.OpenReadStream();
                content.Add(new StreamContent(stream), "file", imageFile.FileName);

                var response = await client.PostAsync(
                    $"https://api.cloudflare.com/client/v4/accounts/{ExtractToken.AccountId}/images/v1",
                    content);

                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = 0,
                        message = "Image upload failed",
                        data = jsonResponse
                    };

                var json = System.Text.Json.JsonDocument.Parse(jsonResponse);

                imagePath = json.RootElement
                    .GetProperty("result")
                    .GetProperty("variants")[0]
                    .GetString();
            }

            // --------------------------------
            // SAVE IN DATABASE
            // --------------------------------
            var procedureName = Constant.addEventPreImages;

            var parameters = new DynamicParameters();
            parameters.Add("@EventImages", imagePath);
            parameters.Add("@EventVideos", videoPath);
            parameters.Add("@EventMasterID", addEventPreImagesViewModel.EventMasterID);

            using var connection = _dapperContext.createConnection();

            var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModelSendEventEmail>(
                procedureName, parameters, commandType: CommandType.StoredProcedure);

            result.statusCode = result.statusCode == 1 ? 200 : 417;

            return result;
        }

        public async Task<ResponseViewModelSendEventEmail> DeleteEventImages(int Id)
        {
            var procedureName = Constant.deleteEventImages;
            DynamicParameters param = new DynamicParameters();
            param.Add("@Id", Id);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, param, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var combinedData = new
                    {
                        Event = result.ToList(),
                    };

                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Get Closed Evet Master.",
                        data = combinedData
                    };
                }
                else
                {
                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No Event found."
                    };
                }
            }
        }

        public async Task<ResponseViewModelSendEventEmail> getEventImagesbyEMID(Guid EventMasterID)
        {
            var procedureName = Constant.getEventImagesbyEMID;
            DynamicParameters param = new DynamicParameters();
            param.Add("@EventMasterID", EventMasterID);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, param, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var combinedData = new
                    {
                        Event = result.ToList(),
                    };

                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Event Images",
                        data = combinedData
                    };
                }
                else
                {
                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No Event found."
                    };
                }
            }
        }


        public async Task<ResponseViewModelSendEventEmail> SendEmailsAllUser(SendEmailsAllUserViewModel model)
        {
            var response = new ResponseViewModelSendEventEmail();

            using (var connection = _dapperContext.createConnection())
            {
                // Email + Name dynmic
                var emailList = await connection.QueryAsync<dynamic>(
                    Constant.sendEmailsAllUser,
                    commandType: CommandType.StoredProcedure
                );

                if (!emailList.Any())
                {
                    response.statusCode = (int)HttpStatusCode.ExpectationFailed;
                    response.message = "No users found.";
                    return response;
                }

                // Send email to all
                foreach (var user in emailList)
                {
                    await SendEmailSingleSMTP(
                        (string)user.Email,
                        model.Subject,
                        model.Body,
                        model.FileUrl,
                        (string)user.UserName   // ← Name yaha mil jayega
                    );
                }

                response.statusCode = (int)HttpStatusCode.OK;
                response.message = "Emails sent successfully.";
                return response;
            }
        }




        //        private async Task SendEmailSingleSMTP(string email, string subjectText, string bodyText, string? fileUrl, string userName)
        //        {
        //            string subject = subjectText;

        //            // File type detection
        //            bool isPdf = !string.IsNullOrWhiteSpace(fileUrl) && fileUrl.ToLower().EndsWith(".pdf");
        //            bool isImage = !string.IsNullOrWhiteSpace(fileUrl) &&
        //                           (fileUrl.ToLower().EndsWith(".png") ||
        //                            fileUrl.ToLower().EndsWith(".jpg") ||
        //                            fileUrl.ToLower().EndsWith(".jpeg") ||
        //                            fileUrl.ToLower().EndsWith(".webp"));

        //            // String check hataya → URL validate kiya
        //            bool isValidUrl =
        //                !string.IsNullOrWhiteSpace(fileUrl) &&
        //                (isPdf || isImage || fileUrl.StartsWith("http"));

        //            string fileSection = "";

        //            // Image preview section
        //            if (isValidUrl && isImage)
        //            {
        //                fileSection = $@"
        //<tr>
        //    <td style='font-weight:600;'>Attachment:</td>
        //    <td>
        //        <img src='{fileUrl}' style='max-width:230px;border-radius:6px;border:1px solid #ccc;' /><br/>
        //        <a href='{fileUrl}' style='color:#4A3AFF;font-size:14px;text-decoration:none;'>{fileUrl}</a>
        //    </td>
        //</tr>";
        //            }
        //            else if (isValidUrl && isPdf)
        //            {
        //                fileSection = $@"
        //<tr>
        //    <td style='font-weight:600;'>Attachment:</td>
        //    <td>
        //        <a href='{fileUrl}' 
        //           style='padding:8px 18px;background:#4A3AFF;color:#fff;border-radius:8px;text-decoration:none;display:inline-block;margin-bottom:6px;'>
        //           View / Download PDF
        //        </a><br/>
        //        <a href='{fileUrl}' style='color:#4A3AFF;font-size:14px;text-decoration:none;'>{fileUrl}</a>
        //    </td>
        //</tr>";
        //            }
        //            else if (isValidUrl)
        //            {
        //                fileSection = $@"
        //<tr>
        //    <td style='font-weight:600;'>Attachment URL:</td>
        //    <td>
        //        <a href='{fileUrl}' style='color:#4A3AFF;font-size:14px;text-decoration:none;'>
        //            {fileUrl}
        //        </a>
        //    </td>
        //</tr>";
        //            }

        //            // Email HTML
        //            string body = $@"
        //<div style='width:100%; background:#eef1f7; padding:20px 0; margin:0; font-family:Segoe UI, Roboto, Arial, sans-serif;'>

        //  <div style='max-width:620px; width:95%; margin:auto; background:#ffffff; 
        //              border:3px solid #5E7BFF; border-radius:16px;
        //              box-shadow:0 8px 22px rgba(0,0,0,0.10); overflow:hidden;'>

        //    <!-- Logo -->
        //    <div style='text-align:center; background:#ffffff; padding:18px 10px; border-bottom:1px solid #e5e7eb;'>
        //      <img src='https://imagedelivery.net/nq9qT5FHZv9Sg48UUnD1-A/893ab68a-6977-4ac1-a97e-9c68846cf400/public'
        //           style='width:180px; max-width:90%; height:auto;' alt='Rentelligence Logo'/>
        //    </div>

        //    <!-- Main Heading -->
        //    <div style='padding:20px 15px 10px; text-align:center;'>
        //      <h2 style='color:#2536d3; font-size:22px; font-weight:700; margin:0;'>
        //        {subjectText}
        //      </h2>
        //    </div>

        //    <!-- Body -->
        //    <div style='padding:10px 15px 20px;'>
        //      <p style='color:#444; font-size:15px; margin:0 0 15px; line-height:1.7;'>
        //        Dear <b>{userName}</b>,
        //      </p>

        //      <p style='color:#555; font-size:14px; margin:0 0 20px; line-height:1.7;'>
        //        {bodyText}
        //      </p>

        //      <div style='background:#f8f9ff; padding:15px; border-radius:10px; border:1px solid #dfe3ff;'>
        //        <table cellpadding='6' cellspacing='0' width='100%' 
        //               style='font-size:14px; color:#333; line-height:1.6; word-break:break-word;'>
        //          {fileSection}
        //        </table>
        //      </div>

        //      <p style='color:#666; font-size:13px; margin-top:20px; text-align:center; line-height:1.6;'>
        //        Thank you for choosing <b>Rentelligence</b>.
        //      </p>
        //    </div>

        //    <!-- Footer -->
        //    <div style='background:#5E7BFF; padding:15px; text-align:center; color:#fff; font-size:13px;'>
        //        Regards,<br/>The Rentelligence Team
        //    </div>

        //    <!-- Social Icons -->
        //    <div style='background:#f3f4f6; padding:15px; text-align:center;'>
        //      <div style='display:inline-block;'>

        //        <a href='https://www.instagram.com/rentelligence.ai?igsh=bmdxcXJqaDd2empk' target='_blank' style='margin:0 10px;'>
        //          <img src='https://cdn-icons-png.flaticon.com/24/174/174855.png' style='width:26px; height:26px;'/>
        //        </a>

        //        <a href='https://www.facebook.com/rentelligenceai' target='_blank' style='margin:0 10px;'>
        //          <img src='https://cdn-icons-png.flaticon.com/24/733/733547.png' style='width:26px; height:26px;'/>
        //        </a>

        //        <a href='https://twitter.com' target='_blank' style='margin:0 10px;'>
        //          <img src='https://cdn-icons-png.flaticon.com/512/5968/5968958.png' style='width:26px; height:26px;'/>
        //        </a>

        //      </div>
        //    </div>

        //  </div>
        //</div>";




        //            _emailService.SendEmailWithFallback(email, subject, body, true);
        //        }

       



        public async Task<ResponseViewModelSendEventEmail> getVerifyEventUser(string AuthLogin)
        {
            var procedureName = Constant.verifyEventUser;
            DynamicParameters param = new DynamicParameters();
            param.Add("@AuthLogin", AuthLogin);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, param, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var combinedData = new
                    {
                        Event = result.ToList(),
                    };

                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Verify Evet Master.",
                        data = combinedData
                    };
                }
                else
                {
                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No Event found."
                    };
                }
            }
        }

        public async Task<ResponseViewModelSendEventEmail> editScheduleByID(int Id)
        {
            var procedureName = Constant.editScheduleByID;
            DynamicParameters param = new DynamicParameters();
            param.Add("@Id", Id, DbType.Int32);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, param, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var combinedData = new
                    {
                        UserEvent = result.ToList(),
                    };

                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Get User Evet Master.",
                        data = combinedData
                    };
                }
                else
                {
                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No Event found."
                    };
                }
            }
        }

        public async Task<ResponseViewModelSendEventEmail> getClosedEveMaster(ClosedEveMasterViewModel closedEveMasterViewModel)
        {
            var procedureName = Constant.getClosedEveMaster;
            var parameters = new DynamicParameters();

            // Default dates if empty
            var fromDate = string.IsNullOrWhiteSpace(closedEveMasterViewModel.FromDate) ? "2025-01-01" : closedEveMasterViewModel.FromDate;
            var toDate = string.IsNullOrWhiteSpace(closedEveMasterViewModel.ToDate) ? DateTime.Now.ToString("yyyy-MM-dd") : closedEveMasterViewModel.ToDate;

            parameters.Add("@FromDate", fromDate, DbType.String);
            parameters.Add("@ToDate", toDate, DbType.String);
            parameters.Add("@LoginId", string.IsNullOrWhiteSpace(closedEveMasterViewModel.LoginId) ? null : closedEveMasterViewModel.LoginId, DbType.String);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                ResponseViewModelSendEventEmail returnData;

                if (result != null && result.Any())
                {
                    var validation = result.First();
                    int status = validation.statusCode ?? -1;

                    switch (status)
                    {
                        case 1:
                            returnData = new ResponseViewModelSendEventEmail
                            {
                                statusCode = (int)HttpStatusCode.OK,
                                message = validation.message,
                                data = result
                            };
                            break;
                        case 0:
                        case -1:
                            returnData = new ResponseViewModelSendEventEmail
                            {
                                statusCode = (int)HttpStatusCode.Conflict,
                                message = validation.message
                            };
                            break;
                        default:
                            returnData = new ResponseViewModelSendEventEmail
                            {
                                statusCode = (int)HttpStatusCode.BadRequest,
                                message = validation.message ?? "Unknown error occurred"
                            };
                            break;
                    }
                }
                else
                {
                    returnData = new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No data found for the given criteria."
                    };
                }

                return returnData;
            }
        }
        public async Task<ResponseViewModelSendEventEmail> bindKitAdmin()
        {
            var procedureName = Constant.bindKitAdmin;
            DynamicParameters param = new DynamicParameters();

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var combinedData = new
                    {
                        Event = result.ToList(),
                    };

                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "bind Kit Admin.",
                        data = combinedData
                    };
                }
                else
                {
                    return new ResponseViewModelSendEventEmail
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "bind Kit Admin."
                    };
                }
            }
        }

        public async Task<ResponseViewModelSendEventEmail> SendEmailsAllUsertest(SendEmailsAllUserViewModeltest model)
        {
            var response = new ResponseViewModelSendEventEmail();

            using (var connection = _dapperContext.createConnection())
            {
                // Email + Name SQL se aa jayega (dynamic)
                var emailList = await connection.QueryAsync<dynamic>(
                    Constant.sendEmailsAllUser,
                    commandType: CommandType.StoredProcedure
                );

                if (!emailList.Any())
                {
                    response.statusCode = (int)HttpStatusCode.ExpectationFailed;
                    response.message = "No users found.";
                    return response;
                }

                // Send email to all
                foreach (var user in emailList)
                {
                    await SendEmailSingleSMTPtest(
                        (string)user.Email,
                        model.Subject,
                        model.Body,
                        model.FileUrl,
                        (string)user.UserName   // ← Name yaha mil jayega
                    );
                }

                response.statusCode = (int)HttpStatusCode.OK;
                response.message = "Emails sent successfully.";
                return response;
            }
        }

        private async Task SendEmailSingleSMTPtest(string email, string subjectText, string bodyText, string? fileUrl, string userName)
        {
            string subject = subjectText;

            // File type detection
            bool isPdf = !string.IsNullOrWhiteSpace(fileUrl) && fileUrl.ToLower().EndsWith(".pdf");
            bool isImage = !string.IsNullOrWhiteSpace(fileUrl) &&
                           (fileUrl.ToLower().EndsWith(".png") ||
                            fileUrl.ToLower().EndsWith(".jpg") ||
                            fileUrl.ToLower().EndsWith(".jpeg") ||
                            fileUrl.ToLower().EndsWith(".webp"));

            // String check hataya → URL validate kiya
            bool isValidUrl =
                !string.IsNullOrWhiteSpace(fileUrl) &&
                (isPdf || isImage || fileUrl.StartsWith("http"));

            string fileSection = "";

            // Image preview section
            if (isValidUrl && isImage)
            {
                fileSection = $@"
<tr>
    <td style='font-weight:600;'>Attachment:</td>
    <td>
        <img src='{fileUrl}' style='max-width:230px;border-radius:6px;border:1px solid #ccc;' /><br/>
        <a href='{fileUrl}' style='color:#4A3AFF;font-size:14px;text-decoration:none;'>{fileUrl}</a>
    </td>
</tr>";
            }
            else if (isValidUrl && isPdf)
            {
                fileSection = $@"
<tr>
    <td style='font-weight:600;'>Attachment:</td>
    <td>
        <a href='{fileUrl}' 
           style='padding:8px 18px;background:#4A3AFF;color:#fff;border-radius:8px;text-decoration:none;display:inline-block;margin-bottom:6px;'>
           View / Download PDF
        </a><br/>
        <a href='{fileUrl}' style='color:#4A3AFF;font-size:14px;text-decoration:none;'>{fileUrl}</a>
    </td>
</tr>";
            }
            else if (isValidUrl)
            {
                fileSection = $@"
<tr>
    <td style='font-weight:600;'>Attachment URL:</td>
    <td>
        <a href='{fileUrl}' style='color:#4A3AFF;font-size:14px;text-decoration:none;'>
            {fileUrl}
        </a>
    </td>
</tr>";
            }

            // Email HTML
            string body = $@"
<div style='width:100%; background:#eef1f7; padding:20px 0; margin:0; font-family:Segoe UI, Roboto, Arial, sans-serif;'>

  <div style='max-width:620px; width:95%; margin:auto; background:#ffffff; 
              border:3px solid #5E7BFF; border-radius:16px;
              box-shadow:0 8px 22px rgba(0,0,0,0.10); overflow:hidden;'>

    <!-- Logo -->
    <div style='text-align:center; background:#ffffff; padding:18px 10px; border-bottom:1px solid #e5e7eb;'>
      <img src='https://imagedelivery.net/nq9qT5FHZv9Sg48UUnD1-A/893ab68a-6977-4ac1-a97e-9c68846cf400/public'
           style='width:180px; max-width:90%; height:auto;' alt='Rentelligence Logo'/>
    </div>

    <!-- Main Heading -->
    <div style='padding:20px 15px 10px; text-align:center;'>
      <h2 style='color:#2536d3; font-size:22px; font-weight:700; margin:0;'>
        {subjectText}
      </h2>
    </div>

    <!-- Body -->
    <div style='padding:10px 15px 20px;'>
      <p style='color:#444; font-size:15px; margin:0 0 15px; line-height:1.7;'>
        Dear <b>{userName}</b>,
      </p>

      <p style='color:#555; font-size:14px; margin:0 0 20px; line-height:1.7;'>
        {bodyText}
      </p>

      <div style='background:#f8f9ff; padding:15px; border-radius:10px; border:1px solid #dfe3ff;'>
        <table cellpadding='6' cellspacing='0' width='100%' 
               style='font-size:14px; color:#333; line-height:1.6; word-break:break-word;'>
          {fileSection}
        </table>
      </div>

      <p style='color:#666; font-size:13px; margin-top:20px; text-align:center; line-height:1.6;'>
        Thank you for choosing <b>Rentelligence</b>.
      </p>
    </div>

    <!-- Footer -->
    <div style='background:#5E7BFF; padding:15px; text-align:center; color:#fff; font-size:13px;'>
        Regards,<br/>The Rentelligence Team
    </div>

    <!-- Social Icons -->
    <div style='background:#f3f4f6; padding:15px; text-align:center;'>
      <div style='display:inline-block;'>
        
        <a href='https://www.instagram.com/rentelligence.ai?igsh=bmdxcXJqaDd2empk' target='_blank' style='margin:0 10px;'>
          <img src='https://cdn-icons-png.flaticon.com/24/174/174855.png' style='width:26px; height:26px;'/>
        </a>

        <a href='https://www.facebook.com/rentelligenceai' target='_blank' style='margin:0 10px;'>
          <img src='https://cdn-icons-png.flaticon.com/24/733/733547.png' style='width:26px; height:26px;'/>
        </a>

        <a href='https://twitter.com' target='_blank' style='margin:0 10px;'>
          <img src='https://cdn-icons-png.flaticon.com/512/5968/5968958.png' style='width:26px; height:26px;'/>
        </a>

      </div>
    </div>

  </div>
</div>";




            _emailService.SendEmailWithFallback(email, subject, body, true);
        }



        private async Task SendEmailSingleSMTP(string email,string subjectText,string bodyText,string? fileUrl,string userName)
        {
            string subject = subjectText;

            //1️⃣ Extract ALL image URLs
            var urlRegex = @"https?:\/\/[^\s<>'""]+";

            List<string> imageUrls = System.Text.RegularExpressions.Regex
                .Matches(bodyText, urlRegex)
                .Select(m => m.Value)
                .Where(x =>
                    x.Contains("imagedelivery.net", StringComparison.OrdinalIgnoreCase) ||
                    x.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                    x.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                    x.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    x.EndsWith(".webp", StringComparison.OrdinalIgnoreCase)
                )
                .Distinct() //  duplicate remove
                .ToList();

            // 2️⃣ Clean body text (extra gaps fix)
            string pureText = System.Text.RegularExpressions.Regex
    .Replace(bodyText, urlRegex, "")   // remove URLs
    .Replace("&nbsp;", " ")
    .Replace("<p><br></p>", "")
    .Replace("<p></p>", "")
    .Replace("<p>", "")
    .Replace("</p>", "\n")
    .Trim();

            //  remove duplicate text lines
            var lines = pureText
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Distinct();   //  same line repeat remove

            pureText = string.Join("<br/>", lines);


            // Email-safe IMAGE GRID (TABLE based)
            string imageHtml = "";

            if (imageUrls.Count > 0)
            {
                imageHtml += @"
<table align='center' cellpadding='0' cellspacing='0' width='100%' style='margin-top:15px;'>";

                for (int i = 0; i < imageUrls.Count; i += 3)
                {
                    imageHtml += "<tr>";

                    var row = imageUrls.Skip(i).Take(3).ToList();

                    foreach (var url in row)
                    {
                        imageHtml += $@"
<td align='center' width='33%' style='padding:6px;'>
    <img src='{url}'
         style='display:block;
                width:100%;
                max-width:170px;
                border-radius:8px;
                border:1px solid #ddd;' />
</td>";
                    }

                    // empty cells if less than 3 images
                    for (int j = row.Count; j < 3; j++)
                    {
                        imageHtml += "<td width='33%'></td>";
                    }

                    imageHtml += "</tr>";
                }

                imageHtml += "</table>";
            }

            //  Attachment section
            bool isPdf = !string.IsNullOrWhiteSpace(fileUrl) && fileUrl.ToLower().EndsWith(".pdf");
            bool isImage = !string.IsNullOrWhiteSpace(fileUrl) &&
                           (fileUrl.ToLower().EndsWith(".png") ||
                            fileUrl.ToLower().EndsWith(".jpg") ||
                            fileUrl.ToLower().EndsWith(".jpeg") ||
                            fileUrl.ToLower().EndsWith(".webp"));

            string fileSection = "";

            if (!string.IsNullOrWhiteSpace(fileUrl))
            {
                if (isImage)
                {
                    fileSection = $@"
<tr>
<td style='padding:6px 0;'>
    <img src='{fileUrl}'
         style='display:block;
                max-width:230px;
                border-radius:6px;
                border:1px solid #ccc;' />
</td>
</tr>";
                }
                else if (isPdf)
                {
                    fileSection = $@"
<tr>
<td style='padding:6px 0;'>
    <a href='{fileUrl}'
       style='padding:8px 18px;
              background:#4A3AFF;
              color:#fff;
              border-radius:8px;
              text-decoration:none;
              display:inline-block;'>
        View / Download PDF
    </a>
</td>
</tr>";
                }
            }
            // 5️⃣ FINAL EMAIL TEMPLATE
            string body = $@"
<div style='width:100%; background:#eef1f7; padding:20px 0;
            font-family:Segoe UI, Roboto, Arial;'>

<div style='max-width:620px; margin:auto; background:#fff;
            border:3px solid #5E7BFF; border-radius:16px;
            box-shadow:0 8px 22px rgba(0,0,0,0.10);'>

    <!-- LOGO -->
    <div style='text-align:center; padding:18px;
                border-bottom:1px solid #e5e7eb;'>
        <img src='https://imagedelivery.net/nq9qT5FHZv9Sg48UUnD1-A/893ab68a-6977-4ac1-a97e-9c68846cf400/public'
             style='display:block; margin:auto;
                    width:180px; max-width:90%;' />
    </div>

    <!-- TITLE -->
    <div style='padding:20px 15px 10px; text-align:center;'>
        <h2 style='color:#2536d3;
                   font-size:22px;
                   margin:0;'>{subjectText}</h2>
    </div>

    <!-- CONTENT -->
    <div style='padding:15px;'>

        <p style='color:#444; font-size:15px; line-height:1.7;'>
            Dear <b>{userName}</b>,
        </p>

        <p style='color:#555; font-size:14px; line-height:1.7;'>
            {pureText}
        </p>

        {imageHtml}

        <table width='100%' cellpadding='0' cellspacing='0'
               style='margin-top:20px;'>
            {fileSection}
        </table>

        <p style='color:#666; font-size:13px;
                  margin-top:20px; text-align:center;'>
            Thank you for choosing <b>Rentelligence</b>.
        </p>
    </div>

    <!-- FOOTER -->
    <div style='background:#5E7BFF;
                padding:15px;
                text-align:center;
                color:#fff;
                font-size:13px;'>

        <p style='margin:0 0 8px;'>Follow us on:</p>
        <a href='https://www.instagram.com/rentelligence.ai?igsh=bmdxcXJqaDd2empk'><img src='https://img.icons8.com/color/48/instagram-new.png' width='22'/></a>
        <a href='#'><img src='https://img.icons8.com/color/48/twitter--v1.png' width='22'/></a>
        <a href='#'><img src='https://img.icons8.com/color/48/whatsapp.png' width='22'/></a>
        <a href='https://www.facebook.com/rentelligenceai'><img src='https://img.icons8.com/color/48/facebook-new.png' width='22'/></a>
        <a href='https://www.youtube.com/@rentelligenceai'><img src='https://img.icons8.com/color/48/youtube-play.png' width='22'/></a>
    </div>

</div>
</div>";
            _emailService.SendEmailWithFallback(email, subject, body, true);
        }

    }
}


