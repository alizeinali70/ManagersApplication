using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagersApplication.Shared
{
    public class ConvertDate
    {
        public static string MiladiToShamsi(string date)
        {

            var cultureInfo = new CultureInfo("en-US");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            DateTime datetime;
            string result = string.Empty;
            bool success = DateTime.TryParse(date, out datetime);
            if (success)
            {
                PersianCalendar pc = new PersianCalendar();

                var month = pc.GetMonth(datetime).ToString().Length == 1 ? "0" + pc.GetMonth(datetime).ToString() : pc.GetMonth(datetime).ToString();
                var day = pc.GetDayOfMonth(datetime).ToString().Length == 1 ? "0" + pc.GetDayOfMonth(datetime).ToString() : pc.GetDayOfMonth(datetime).ToString();
                result = pc.GetYear(datetime).ToString() + "/" + month + "/" + day;
            }
            return result;
        }

        public static DateTime ShamsiToMiladi(string date)
        {
            DateTime datetime;
            DateTime? result = null;

            bool success = DateTime.TryParse(date, out datetime);

            if (success)
            {
                PersianCalendar pc = new PersianCalendar();
                result = pc.ToDateTime(datetime.Year, datetime.Month, datetime.Day, 0, 0, 0, 0);
            }

            return result.Value;
        }       
    }
}
