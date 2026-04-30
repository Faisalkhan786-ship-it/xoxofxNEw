using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class ChatMasterViewModel
    {
        public int ChatId { get; set; }
        public Guid UserId { get; set; }
        public String? MessageText { get; set; }
        public int IsUser { get; set; }
    }
    public class NewChatViewModel
    {
        public Guid UserId { get; set; }
        public string? ChatName { get; set; }

    }
    public class UseCreditViewModel
    {
        public Guid UserId { get; set; }

    }
    public class UselinkedidViewModel
    {
        public Guid UserId { get; set; }
        public string? ThirdPartyUserId { get; set; }
        public decimal? ThirdPartyPackage { get; set; }
        public DateTime? LinkedDate { get; set; }
        public string? LinkedRemark { get; set; }

    }
    public class ChatMessagesViewModel
    {
        public int? ChatId { get; set; }
        public Guid UserId { get; set; }

    }
  
}
