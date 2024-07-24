using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BookStoreOnline.Models
{
    public class EditCustomerViewModel
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Họ và tên")]
        public string FullName { get; set; }

        [Required]
        [DisplayName("Địa chỉ")]
        public string Address { get; set; }

        [Required]
        [Phone]
        [DisplayName("Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }
    }
}