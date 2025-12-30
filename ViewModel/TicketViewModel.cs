using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class TicketViewModel
    {
        public class AddTicket
        {
            [Required]
            public Guid URID { get; set; }
            [Required]
            public string? Subject { get; set; }

            [Required]
            public string? Message { get; set; }

            [Required]
            public string? TicketType { get; set; }
            public IFormFile? ImagePath { get; set; }
        }
        public class AddTicketReply
        {
            [Required]
            public Guid TicketId { get; set; }
            [Required]

            public Guid CreatedBy { get; set; }
            [Required]
            public string? Message { get; set; }

            public IFormFile? ImagePath { get; set; }
            public int Status { get; set; }
            public int Seen { get; set; }
        }

        public class AddExpoTokensViewModel
        {
            [Required]
            public Guid URID { get; set; }

            [Required]
            public string? ExpoToken { get; set; }
        }

 

        public class SendNotificationViewModel
        {
            public string Title { get; set; }
            public string Body { get; set; }
            public string ImageUrl { get; set; }

        }

    }
}
