using System;
using System.ComponentModel.DataAnnotations;

namespace Connect4.Models.AccountViewModels
{
    class MinIdadeAttribute : ValidationAttribute
    {
        int _minidade;

        public MinIdadeAttribute(int minidade)
        {
            _minidade = minidade;
        }

        public override bool IsValid(object value)
        {
            DateTime date;

            if(DateTime.TryParse(value.ToString(), out date)) {
                return date.AddYears(_minidade) < DateTime.Now;
            }
            return false;
        }
    }
}