using Guna.UI2.WinForms;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ZakaBank_24.Global_Classes
{
    public class clsValidation
    {
        /// <summary>
        /// Checks if the given user has the specified permission.
        /// </summary>
        /// <param name="User">The user object to check.</param>
        /// <param name="Perm">The permission to check for.</param>
        /// <returns>True if the user has the permission, false otherwise.</returns>
        public static bool IsUserHaveThisPermission(int UserPermissions, int Perm)
        {
            return (UserPermissions & Perm) == Perm;
        }


        public static bool ValidateEmail(string emailAddress)
        {
            var pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";

            var regex = new Regex(pattern);

            return regex.IsMatch(emailAddress);
        }

        public static bool ValidateInteger(string Number)
        {
            var pattern = @"^[0-9]*$";

            var regex = new Regex(pattern);

            return regex.IsMatch(Number);
        }

        public static bool ValidateFloat(string Number)
        {
            var pattern = @"^[0-9]*(?:\.[0-9]*)?$";

            var regex = new Regex(pattern);

            return regex.IsMatch(Number);
        }

        public static bool IsNumber(string Number)
        {
            return (ValidateInteger(Number) || ValidateFloat(Number));
        }

        /// <summary>
        /// Validates a TextBox with a custom validation function.
        /// </summary>
        public static void ValidateTextBox(Guna2TextBox textBox, Func<string, bool> validationFunc, string errorMessage, System.ComponentModel.CancelEventArgs e, ErrorProvider errorProvider)
        {
            if (!validationFunc(textBox.Text))
            {
                errorProvider.SetError(textBox, errorMessage);
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(textBox, string.Empty);
                e.Cancel = false;
            }
        }

        /// <summary>
        /// Checks if the provided email is valid.
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the provided phone number is valid (numeric and length 10).
        /// </summary>
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            return phoneNumber.All(char.IsDigit) && phoneNumber.Length == 10;
        }
    }
}
