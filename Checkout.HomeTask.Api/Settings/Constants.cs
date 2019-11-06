using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.HomeTask.Api.Settings
{
    public static class Constants
    {
        public static class ValidationErrors
        {
            public const string CardNumber = "Card number must be 16-digit number";
            public const string Cvv = "Cvv must be a 3-digit number";
            public const string Year = "Year must be greater or equal to current year";
            public const string MonthInCurrentYear = "When expire year is a current year month must be greater than current one";
            public const string Month = "Month must be a number between 1 and 12";
            public const string Currency = "Currency must be either EUR or USD";
            public const string Amount = "Amount must be greater than 0";
        }
    }
}
