/****************************************************
	文件：Class1.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/08 10:44   	
	功能：网络通信协议（客户端服务器公用）
*****************************************************/

using PENet;
using System;


namespace PEProtocol
{
    [Serializable]
    public class GameMsg : PEMsg
    {
        public ReqLogin reqLogin;
    }

    [Serializable]
    public class ReqLogin
    {
        public string acct;
        public string pass;
    }

    public enum CMD
    {
        None = 0,
        //登录相关 100
        ReqLogin = 101,
        RspLogin = 102,
    }

    public class SrvCfg
    {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 17666;
    }
}
