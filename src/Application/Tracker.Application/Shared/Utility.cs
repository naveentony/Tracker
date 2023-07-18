using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tracker.Application.Shared
{
    public class TrackerUtility
    {
        public static bool GetStatus(string statusType)
        {
            return statusType == "Enable" ? true : false;
        }
        public static string GetStatus(bool statusType)
        {
            return statusType == true ? "Enable" : "Disable";
        }
        public static string GetAmountStatus(int statusType)
        {
            var seletedItems = (AmountStatus)statusType;

            return seletedItems.ToString();
        }
        public static int GetAmountStatus(string statusType)
        {
            var seletedItems = (AmountStatus)Enum.Parse(typeof(AmountStatus), statusType);
            return (int)seletedItems;
        }
    }
}
