using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using PhoneNumbers;

namespace Mc2.CrudTest.Shared.Utilities
{
    public static class Validator
    {
        public static bool PhoneIsValid(string number)
        {
            PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();
            try
            {
                phoneNumberUtil.Parse(number, "US");
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public static bool BankAccountIsValid(string number)
        {
            return number != null && number.Length == 10 && Regex.IsMatch(number, @"^\d+$");
        }

        /// <summary>
        /// I found it in this answer:
        /// https://stackoverflow.com/a/1374644/1474613
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool EmailIsValid(string email)
        {
            if (email == null)
                return false;

            string trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                MailAddress addr = new(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }
}
