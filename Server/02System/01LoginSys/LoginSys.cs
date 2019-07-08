/****************************************************
	文件：LoginSys.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/08 10:32   	
	功能：登录系统管业务
*****************************************************/


public class LoginSys
{
    private static LoginSys instance = null;

    public static LoginSys Instance {
        get {
            if (instance == null)
            {
                instance = new LoginSys();
            }
            return instance;
        }
    }

    public void Init()
    {
        PECommon.Log("LoginSys Init Done.");
    }
}