using System;
using System.Collections.Generic;
using System.Web;
using System.Globalization;

/// <summary>
/// Summary description for TimeDateCulture
/// </summary>
public class TimeDateCulture
{
	public TimeDateCulture()
	{
		
	}

    public string SpecificDatePattern(int yyear, int mmonth, int dday)
    {
        DateTimeFormatInfo dtFormat = new CultureInfo(CultureInfo.CurrentCulture.ToString(), false).DateTimeFormat;
        DateTime dt = new DateTime();
        try
        {
            dt = new DateTime(yyear, mmonth, dday);
        }
        catch (Exception ex)
        {
            //MessageBox.Show(ex.Message.ToString());
            System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
        }
        string dateString = dt.ToString(dtFormat.ShortDatePattern);

        return dateString;
    }

    public string GetTodaysDateInBritishFormat()
    {
        string TodaysDate = "";
        CultureInfo culture = new CultureInfo("pt-BR"); 
        TodaysDate = DateTime.Today.Date.ToString("d", culture);

        return TodaysDate;
    }
}
