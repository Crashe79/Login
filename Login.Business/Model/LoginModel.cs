using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Login.Business.Model
{
   public class LoginModel
    {
       public string Login { get; set; }
       public string Password { get; set; }
       public string RememberMe { get; set; }
       public string ErrorMessage { get; set; }
       public bool IsChecked
       {
           get
           {
               return !string.IsNullOrEmpty(RememberMe) 
                   && RememberMe.Equals("on", StringComparison.InvariantCultureIgnoreCase);
           }
       }

       private bool _isValidEmail;
       public bool IsValidEmail
       {
           get
           {
               if (!string.IsNullOrEmpty(Login))
               {
                   try
                   {
                       var m = new MailAddress(Login);
                       _isValidEmail = true;
                   }
                   catch
                   {
                       _isValidEmail = false;
                   }
               }
               return _isValidEmail;
           }
           set { _isValidEmail = value; }
       }

       private bool _isValidPassword;
       ////@"!@#$%^&*()_-+=;',.?/:\][", 
       public bool IsValidPassword
       {
           get
           {
               if (!string.IsNullOrEmpty(Password) && Password.Length <= 32)
               {
                   try
                   {
                       if (Regex.Match(Password, @"(\w|[!@#$%^&*()_:[]])", RegexOptions.IgnoreCase).Success)
                       {
                           _isValidPassword = true;
                           return true;
                       }
                   }
                   catch (Exception)
                   {
                       _isValidPassword = false;
                       throw;
                   }
                   _isValidPassword = false;
               }
               return _isValidPassword;
           }
           set { _isValidPassword = value; }
       }
    }
}
