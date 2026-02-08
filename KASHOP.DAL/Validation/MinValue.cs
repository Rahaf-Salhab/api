using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Validation
{
    public class MinValue : ValidationAttribute
    {
        private readonly int length;

        public MinValue(int length = 10)
        {
            this.length = length;
        }
        public override bool IsValid(object? value)
        {
            if(value is decimal val)
            {
                if(val > length) return true;
            }
            return false;
         }
        public override string FormatErrorMessage(string name)
        {
            return $"{name} is invalid.";
        }
    }
}
