using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace gateway.domain
{
    // customs validations annotations
    
    /// <summary>
    /// Custom attribute validation class. This could be use as entity attribute (field) validations.
    /// Check if the value contains only alphanumeric characters.
    /// </summary>
    /// <remarks>Runs (in model-binding) before controller.</remarks>
    public class Alphanumeric : ValidationAttribute
    {
        /// <summary>Check if the value contains only alphanumeric characters</summary>
        /// <remarks>Very important to stay safe from SQL injections</remarks>
        /// <param name="value" example="DName">E.g Table field name</param>
        /// <returns>True if pass</returns>
        public override bool IsValid(object value)
        {
            // so it can be used along 'AllowNull' validations
            if (value == null) return true;
            
            // changing the error message
            ErrorMessage = MsgAttrVal.MsgInvalidString;

            // doing the validation
            var chk = Regex.IsMatch(value.ToString() ?? "", RegxPatterns.Alphanumeric, RegexOptions.IgnoreCase);
            return chk;
        }
    }
    
    /// <summary>Custom attribute validation class. Try to validate a phone number</summary>
    public class IPv4 : ValidationAttribute
    {
        /// <summary>Allow only between 8 and 14 digits</summary>
        /// <param name="value">Any field we need to check for letters only</param>
        public override bool IsValid(object value)
        {
            // so it can be used along 'AllowNull' validations
            if (value == null) return true;

            // changing the error message
            ErrorMessage = MsgAttrVal.MsgInvalidIpv4;

            // running the validation
            var chk = Regex.IsMatch(value.ToString() ?? "", RegxPatterns.IPv4);
            return chk;
        }
    }
}