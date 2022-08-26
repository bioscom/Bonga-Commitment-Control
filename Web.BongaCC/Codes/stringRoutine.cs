using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Globalization;

/// <summary>
/// Summary description for stringRoutine
/// </summary>
public class stringRoutine
{
	public stringRoutine()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static string capFirstLetter(string sText)
    {
        string sRet = "";
        try
        {
            sText = sText.Trim();
            switch(sText.Length)
            {
                case 0:
                    sRet = "";break;
                case 1:
                    sRet = sText.ToUpper();break;
                default:
                    sRet = sText.Substring(0, 1).ToUpper();
                    sRet = sRet + sText.Substring(1, sText.Length - 1).ToLower();
                    break;
            }
            
        }
        catch (Exception ex)
        {
            Debug.Fail(ex.Message);
        }
            
        return sRet;
    }

     public static string formatAsBankMoney(decimal gValue) //decimal ,comma
     {
        string sRet = "";
        try
        {
            sRet = Strings.Format(gValue, "##,##0.00");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
        }
            
        return sRet;
     }

    public static string formatAsBankMoney(decimal? gValue) //decimal ,comma
    {
        string sRet = "";
        try
        {
            sRet = Strings.Format(gValue, "##,##0.00");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
        }

        return sRet;
    }

    public static string formatAsNumber(int gValue) //decimal ,comma
     {
         string sRet = "";
         try
         {
             sRet = Strings.Format(gValue, "##,##0");
         }
         catch (Exception ex)
         {
             System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
         }

         return sRet;
     }

     public static string formatAsNumber(decimal gValue) //decimal ,comma
     {
         string sRet = "";
         try
         {
             sRet = Strings.Format(gValue, "##,##0");
         }
         catch (Exception ex)
         {
             System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
         }

         return sRet;
     }
         
    public static string formatAsBankMoney(Char cMoneySign, decimal gValue)
    {
        string sRet = "";
        try
        {
            sRet = cMoneySign + Strings.Format(gValue, "##,##0.00");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
        }
            
        return sRet;
    }
        
    public static string  formatAsBankMoney(string cMoneySign, decimal gValue)
    {
        string sRet = "";
        try
        {
            sRet = cMoneySign + Strings.Format(gValue, "##,##0.00");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
        }
            
        return sRet;
    }

    public static string formatAsBankMoney(string cMoneySign, decimal? gValue)
    {
        string sRet = "";
        try
        {
            sRet = cMoneySign + Strings.Format(gValue, "##,##0.00");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
        }

        return sRet;
    }

    public static string formatAsBankMoney(string sValue)
    {
        string sRet = "";
        try
        {
            sRet = Strings.Format(sValue, "##,##0.00");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
        }

        return sRet;
    }

    public static string formatAsBankMoney(string cMoneySign, string sValue)
    {
        string sRet = "";
        try
        {
            sRet = cMoneySign + Strings.Format(sValue, "##,##0.00");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
        }

        return sRet;
    }


    public static string formatAsBankMoney(Decimal gValue, string cMoneyName)
    {
        string sRet = "";
        try
        {
            sRet = Strings.Format(gValue, "##,##0.00") + " " + cMoneyName;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
        }
        return sRet;
    }

    public static string strLeft(string sString, short xLent)
    {
        string sRet = "";
        try
        {
             if (xLent > sString.Length) xLent = short.Parse(sString.Length.ToString());
             sRet = sString.Substring(0, xLent);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
        }
        return sRet;
    }

    public static string strRight(string sString, short xLent)
    {
        string sRet = "";
        try
        {
            if (xLent > sString.Length) xLent = short.Parse(sString.Length.ToString());
            sRet = sString.Substring(sString.Length - xLent);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
        }
        return sRet;
    }

    public static string TrimString(string input, int length)
    {
        // return nothing if the string is null
        if (String.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        // invalid length submitted
        if (length <= 0)
        {
            length = 100;
        }

        if (input.Length > length)
        {
            return input.Substring(0, length) + "...";
        }

        return input;
    }


    public static int getWeek(DateTime date)
    {
        DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
        Calendar cal = dfi.Calendar;

        return cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
    }


    //public static string sentenceCase(string sText)
    //{
    //     string sRet = "";
    //    sRet = Strings.StrConv(sText, Constants.vbProperCase); //' prepare the result
    //    //' restore all those characters that were capitalized
    //    int iLength = sText.Length;
    //    for(int iLoop = 1; (iLoop < Strings.Len(sText)); iLoop++)
    //    {
    //        switch (Strings.Asc(Strings.Mid(sText, iLoop, 1)))
    //        {
    //            case 65 to 90:   //' A-Z
    //                Strings.Mid(sRet, iLoop, 1) = Strings.Mid(sText, iLoop, 1);
    //        }                
    //    }

    //    return sRet;
    //}


}



    
//    Public Shared Function isEmailFormat(ByVal sEmail As String) As Boolean
//        Dim bRet As Boolean
//        Try
//            Dim oRegex As New System.Text.RegularExpressions.Regex("\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", Text.RegularExpressions.RegexOptions.IgnoreCase)
//            bRet = oRegex.IsMatch(sEmail)
//        Catch ex As Exception
//            Debug.Fail(ex.TargetSite.Name & vbNewLine & ex.Message & vbNewLine & ex.StackTrace)
//        End Try
//        Return bRet
//    End Function
//    Public Shared Function strDuplicate(ByVal sString As String, ByVal xLent As Short) As String
//        Dim sRet As String = ""
//        Try
//            sRet = StrDup(xLent, sString)
//        Catch ex As Exception
//            Debug.Fail(ex.TargetSite.Name & vbNewLine & ex.Message & vbNewLine & ex.StackTrace)
//        End Try
//        Return sRet
//    End Function
//    Public Overloads Shared Function trimPadAtRite(ByVal sString As String, ByVal xLent As Short, ByVal cPadChar As Char) As String
//        Dim sRet As String = ""
//        Try
//            If sString.Length < xLent Then 'pad
//                sString = sString & StrDup(xLent, cPadChar)
//                sRet = sString.Substring(0, xLent)
//            Else
//                sRet = sString
//            End If
//        Catch ex As Exception
//            Debug.Fail(ex.TargetSite.Name & vbNewLine & ex.Message & vbNewLine & ex.StackTrace)
//        End Try
//        Return sRet
//    End Function
//    Public Overloads Shared Function trimPadAtRite(ByVal sString As String, ByVal xLent As Short) As String
//        Dim sRet As String = ""
//        Try
//            sRet = trimPadAtRite(sString, xLent, " ")
//        Catch ex As Exception
//            Debug.Fail(ex.TargetSite.Name & vbNewLine & ex.Message & vbNewLine & ex.StackTrace)
//        End Try
//        Return sRet
//    End Function
//    Public Overloads Shared Function trimPadAtLeft(ByVal sString As String, ByVal xLent As Short, ByVal cPadChar As Char) As String
//        Dim sRet As String = ""
//        Try
//            If sString.Length < xLent Then 'pad
//                sString = StrDup(xLent, cPadChar) & sString
//                sRet = sString.Substring(sString.Length - xLent)
//            Else
//                sRet = sString
//            End If
//        Catch ex As Exception
//            Debug.Fail(ex.TargetSite.Name & vbNewLine & ex.Message & vbNewLine & ex.StackTrace)
//        End Try
//        Return sRet
//    End Function
//    Public Overloads Shared Function trimPadAtLeft(ByVal sString As String, ByVal xLent As Short) As String
//        Dim sRet As String = ""
//        Try
//            sRet = trimPadAtLeft(sString, xLent, " ")
//        Catch ex As Exception
//            Debug.Fail(ex.TargetSite.Name & vbNewLine & ex.Message & vbNewLine & ex.StackTrace)
//        End Try
//        Return sRet
//    End Function
//    
//    Public Overloads Shared Function formatAsMoney(ByVal gValue As Decimal) As String 'no decimal, comma
//        Dim sRet As String = ""
//        Try
//            sRet = Format(gValue, "##,##0")
//        Catch ex As Exception
//            Debug.Fail(ex.TargetSite.Name & vbNewLine & ex.Message & vbNewLine & ex.StackTrace)
//        End Try
//        Return sRet
//    End Function
//    Public Overloads Shared Function formatAsMoney(ByVal cMoneySign As Char, ByVal gValue As Decimal) As String
//        Dim sRet As String = ""
//        Try
//            sRet = cMoneySign & Format(gValue, "##,##0")
//        Catch ex As Exception
//            Debug.Fail(ex.TargetSite.Name & vbNewLine & ex.Message & vbNewLine & ex.StackTrace)
//        End Try
//        Return sRet
//    End Function

//    'Public Overloads Shared Function formatAsMoney(ByVal cMoneySign As String, ByVal gValue As Decimal) As String
//    '    Dim sRet As String = ""
//    '    Try
//    '        sRet = cMoneySign & Format(gValue, "##,##0")
//    '    Catch ex As Exception
//    '        Debug.Fail(ex.TargetSite.Name & vbNewLine & ex.Message & vbNewLine & ex.StackTrace)
//    '    End Try
//    '    Return sRet
//    'End Function
//    Public Overloads Shared Function formatAsMoney(ByVal gValue As Decimal, ByVal cMoneyName As String) As String
//        Dim sRet As String = ""
//        Try
//            sRet = Format(gValue, "##,##0") & " " & cMoneyName
//        Catch ex As Exception
//            Debug.Fail(ex.TargetSite.Name & vbNewLine & ex.Message & vbNewLine & ex.StackTrace)
//        End Try
//        Return sRet
//    End Function

//    Public Overloads Shared Function formatAsMoney(ByVal iValue As Long) As String
//        Dim sRet As String = ""
//        Try
//            sRet = Format(iValue, "##,##0")
//        Catch ex As Exception
//            Debug.Fail(ex.TargetSite.Name & vbNewLine & ex.Message & vbNewLine & ex.StackTrace)
//        End Try
//        Return sRet
//    End Function
//    Public Overloads Shared Function formatAsMoney(ByVal cMoneySign As Char, ByVal iValue As Long) As String
//        Dim sRet As String = ""
//        Try
//            sRet = cMoneySign & Format(iValue, "##,##0")
//        Catch ex As Exception
//            Debug.Fail(ex.TargetSite.Name & vbNewLine & ex.Message & vbNewLine & ex.StackTrace)
//        End Try
//        Return sRet
//    End Function
//    Public Overloads Shared Function formatAsMoney(ByVal iValue As Long, ByVal cMoneyName As String) As String
//        Dim sRet As String = ""
//        Try
//            sRet = Format(iValue, "##,##0") & " " & cMoneyName
//        Catch ex As Exception
//            Debug.Fail(ex.TargetSite.Name & vbNewLine & ex.Message & vbNewLine & ex.StackTrace)
//        End Try
//        Return sRet
//    End Function

   

//    Public Overloads Shared Function formatAsNumberMoney(ByVal gValue As Decimal) As String 'decimal,no comme
//        Dim sRet As String = ""
//        Try
//            sRet = Format(gValue, "##,##0.00").Replace(",", "")
//        Catch ex As Exception
//            Debug.Fail(ex.TargetSite.Name & vbNewLine & ex.Message & vbNewLine & ex.StackTrace)
//        End Try
//        Return sRet
//    End Function
//    Public Overloads Shared Function formatAsNumberMoney(ByVal cMoneySign As Char, ByVal gValue As Decimal) As String
//        Dim sRet As String = ""
//        Try
//            sRet = cMoneySign & Format(gValue, "##,##0.00").Replace(",", "")
//        Catch ex As Exception
//            Debug.Fail(ex.TargetSite.Name & vbNewLine & ex.Message & vbNewLine & ex.StackTrace)
//        End Try
//        Return sRet
//    End Function
//    Public Overloads Shared Function formatAsNumberMoney(ByVal gValue As Decimal, ByVal cMoneyName As String) As String
//        Dim sRet As String = ""
//        Try
//            sRet = Format(gValue, "##,##0.00").Replace(",", "") & " " & cMoneyName
//        Catch ex As Exception
//            Debug.Fail(ex.TargetSite.Name & vbNewLine & ex.Message & vbNewLine & ex.StackTrace)
//        End Try
//        Return sRet
//    End Function

//    Public Shared Function splitString(ByVal sData As String, ByVal sSpliter As String) As List(Of String)
//        Dim oRet As New List(Of String)
//        Try
//            'qwe==sns
//            Dim iPos As Integer
//            While sData.Length > 0
//                iPos = sData.IndexOf(sSpliter)
//                If iPos >= 0 Then
//                    If iPos > 0 Then oRet.Add(sData.Substring(0, iPos))
//                    sData = sData.Substring(iPos + sSpliter.Length)
//                Else
//                    If sData.Length > 0 Then oRet.Add(sData)
//                    sData = ""
//                End If
//            End While
//        Catch ex As Exception
//            Debug.Fail(ex.TargetSite.Name & vbNewLine & ex.Message & vbNewLine & ex.StackTrace)
//        End Try
//        Return oRet
//    End Function
//    Public Shared Function clipBegin(ByVal sString As String, ByVal sClip As String) As String
//        Dim sRet As String = ""
//        Try
//            If sString.StartsWith(sClip) Then
//                sRet = sString.Substring(sClip.Length)
//            Else
//                sRet = sString
//            End If
//        Catch ex As Exception
//            Debug.Fail(ex.TargetSite.Name & vbNewLine & ex.Message & vbNewLine & ex.StackTrace)
//        End Try
//        Return sRet
//    End Function
//    Public Shared Function clipEndin(ByVal sString As String, ByVal sClip As String) As String
//        Dim sRet As String = ""
//        Try
//            If sString.EndsWith(sClip) Then
//                sRet = sString.Substring(0, sString.Length - sClip.Length)
//            Else
//                sRet = sString
//            End If
//        Catch ex As Exception
//            Debug.Fail(ex.TargetSite.Name & vbNewLine & ex.Message & vbNewLine & ex.StackTrace)
//        End Try
//        Return sRet
//    End Function
//End Class