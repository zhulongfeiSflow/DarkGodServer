/****************************************************
	文件：PECommon.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/08 11:49   	
	功能：客户端服务端公用工具类
*****************************************************/
using PENet;
using PEProtocol;

public enum LogType
{
    Log = 0,
    Warn = 1,
    Error = 2,
}

public class PECommon
{
    public static void Log(string msg = "", LogType tp = LogType.Log)
    {
        LogLevel lv = (LogLevel)tp;
        PETool.LogMsg(msg, lv);
    }

    public static int GetFightByProps(PlayerData pd)
    {
        return pd.lv * 100 + pd.ad + pd.ap + pd.addef + pd.apdef;
    }

    public static int GetPowerLimit(int lv)
    {
        return (lv - 1) / 10 * 150 + 150;
    }

    public static int GetExpUpValByLv(int lv)
    {
        return 100 * lv * lv;
    }
}