
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ViewModel
{


    public class EventViewModel
    {
        public IFormFile? Image { get; set; }
        public string? Tittle { get; set; }
        public string? EventType { get; set; }
        public string? EventStartDate { get; set; }
        public string? EndStartDate { get; set; }
        public string? AvailableSeats { get; set; }
        public string? Location { get; set; }
        public string? EventMode { get; set; }

        public string? SessionsTime { get; set; }
        public string? SessionsTimeOne { get; set; }
        public string? SessionsTimeTwo { get; set; }


        public int? SessionSeats { get; set; }
        public int? SessionOneSeats { get; set; }
        public int? SessionTwoSeats { get; set; }
        public int? MultipleSeatbook { get; set; }

        public string? Description { get; set; }
        public Guid? Createdby { get; set; }
        public string? AccessType { get; set; }
        public string? EventURL { get; set; }
        public Decimal? EventPrice { get; set; }

    }

    public class UpdateEventViewModel
    {

        public Guid? EventMasterID { get; set; }

        public IFormFile? Image { get; set; }

        public string? Tittle { get; set; }
        public string? EventType { get; set; }

        public string? EventStartDate { get; set; }
        public string? EndStartDate { get; set; }

        public string? AvailableSeats { get; set; }
        public string? Location { get; set; }

        public Guid? Updatedby { get; set; }
        public int Status { get; set; }

        public string? Description { get; set; }

        public string? SessionsTime { get; set; }
        public string? SessionsTimeOne { get; set; }
        public string? SessionsTimeTwo { get; set; }

        public int? SessionSeats { get; set; }
        public int? SessionOneSeats { get; set; }
        public int? SessionTwoSeats { get; set; }

        public string? EventMode { get; set; }
        public string? AccessType { get; set; }
        public string? EventURL { get; set; }
        public Decimal? EventPrice { get; set; }
        public int? MultipleSeatbook { get; set; }

    }


    public class EventScheduleMasterViewModel
    {
        public int? Id { get; set; }
        public int? Status { get; set; }
        public Guid? EventMasterID { get; set; }
        public String? Tittle { get; set; }
        public TimeOnly? Time { get; set; }
        public Guid? Createdby { get; set; }

    }

    public class AddUserEventbookingViewModel
    {

        public Guid? URID { get; set; }
        public Guid? EventMasterID { get; set; }
        public string? TicketNumber { get; set; }
        public string? SessionTime { get; set; }
        public string? AccessType { get; set; }
        public decimal? Price { get; set; }
        public int? RequestedSeats { get; set; }
        public int? type { get; set; }
    }

    public class AddEventPreImagesViewModel
    {
        public IFormFile? Image { get; set; }            
        public IFormFile? EventVideos { get; set; }       
        public Guid? EventMasterID { get; set; }         
    }
    public class SendEmailsAllUserViewModel
    {
        public string Subject { get; set; }        // Email Subject
        public string Body { get; set; }           // Email Body (HTML allowed)
        public string? FileUrl { get; set; }       // LIVE URL (image/pdf)
        public bool SendToAll { get; set; }        // Send mail to all users    
    }

    //public class SendEmailsAllUserViewModel
    //{
    //    public string Subject { get; set; }            // Email Subject
    //    public string Body { get; set; }               // Email Body (HTML allowed)
    //    public IFormFile? Attachment { get; set; }     // Optional Attachment
    //    public bool SendToAll { get; set; }            // Send to ALL users?
    //}
    public class ClosedEveMasterViewModel
    {
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public string LoginId { get; set; }
    }
    public class SendEmailsAllUserViewModeltest
    {
        public string Subject { get; set; }        // Email Subject
        public string Body { get; set; }           // Email Body (HTML allowed)
        public string? FileUrl { get; set; }       // LIVE URL (image/pdf)
        public bool SendToAll { get; set; }        // Send mail to all users    
    }
}
