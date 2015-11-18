using System;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModel
{
    public class LoginModel
    {
        [Required(ErrorMessage="請輸入帳號")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "請輸入密碼")]
        public string Password { get; set; }
    }
}