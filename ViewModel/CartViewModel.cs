using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class CartViewModel
    {
        public class AddCartViewModel
        {
            [Required]
            public Guid userId { get; set; }
            [Required]
            public Guid productId { get; set; }

        }
        public class DeleteCartViewModel
        {
            [Required]
            public Guid userId { get; set; }
            [Required]
            public Guid productId { get; set; }
        }
    }
}
