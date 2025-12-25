using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class GeographyViewModel
    {
        public class CityViewModel
        {
            [Required]
            public int Fk_StateId { get; set; }
            public int Pk_CityId { get; set; }
            public string? CityName { get; set; }
        }
        public class StateViewModel
        {
            [Required]
            public int Pk_StateId { get; set; }
            public int Fk_CountryId { get; set; }
            public string? StateName { get; set; }
        }

        public class CountryViewModel
        {
            [Required]
            public int Country_Id { get; set; }
            [Required]
            public int Country_Code { get; set; }

            [Required]
            public int phonecode { get; set; }
            [Required]
            public string? Country_Name { get; set; }
            public bool active { get; set; }
        }

        public class ContactUsViewModel
        {
            public string? Name { get; set; }
            public string? Email { get; set; }
            public string? Mobile { get; set; }
            public string? Subject { get; set; }
            public string? Message { get; set; }
            public string? CareerName { get; set; }
        }
    }
}
