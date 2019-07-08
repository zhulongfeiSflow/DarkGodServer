/****************************************************
	文件：PECommon.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/08 11:49   	
	功能：客户端服务端公用工具类
*****************************************************/
using PENet;

public enum LogType
{
    Log=0,
    Warn=1,
    Error=2,
}

public class PECommon
{
    public static void Log(string msg = "", LogType tp= LogType.Log)
    {
        LogLevel lv = (LogLevel)tp;
        PETool.LogMsg(msg, lv);
    }
}