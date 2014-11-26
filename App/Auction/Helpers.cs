using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Auction
{
    class Helpers
    {
        public static Boolean emailIsValid(String email){
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
             + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
             + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            Regex emailRegex = new Regex(validEmailPattern, RegexOptions.IgnoreCase);
            return emailRegex.IsMatch(email);
        }  

        public static Boolean nameIsValid(String name){
            return name.Length > 2;
        }

        public static Boolean passwordIsValid(String password){
            return password.Length > 4;
        }
    }
}
