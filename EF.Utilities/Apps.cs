public static class Apps
{
    static Apps()
    {

    }


    // 3.   The Method that Generates Token

    public static string GenerateToken()
    {
        return System.Guid.NewGuid().ToString();
    }

    public static string LoginIDNoDomain(string loginID)
    {
        if (loginID.IndexOf("\\") >= 0)
        {
            loginID = loginID.Substring(loginID.IndexOf("\\") + 1);
        }

        return loginID;
    }

    public static string LoginIDNoEmailSign(string loginID)
    {
        string[] o = loginID.Split('@');

        return o[0].ToString();
        //if (loginID.IndexOf("@") >= 0)
        //{
        //    loginID = loginID.Substring(loginID.IndexOf("@") + 1);
        //}

        //return loginID;
    }

    public static string getFullEmail(string LoginUserIdentity)
    {
        return LoginIDNoDomain(LoginUserIdentity) + "@shell.com";
    }

    public static string getUserIdentity(string LoginUserIdentity)
    {
        return LoginIDNoDomain(LoginUserIdentity);
    }
}