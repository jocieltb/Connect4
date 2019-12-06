using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Connect4.Models.AccountViewModels
{
    public class MaxIdade : ValidationAttribute
    {
        int _maxidade;

            public MaxIdade (int maxidade)
            {
            _maxidade = maxidade;
            }

            public override bool IsValid(object value)
            {
                DateTime date;

                if (DateTime.TryParse(value.ToString(), out date))
                {
                    return date.AddYears(_maxidade) > DateTime.Now;
                }
                return false;
            }
        
    }
}
