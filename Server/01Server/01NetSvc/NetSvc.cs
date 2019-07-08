/****************************************************
	文件：NetSvc.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/08 10:29   	
	功能：网络服务
*****************************************************/

using PENet;
using PEProtocol;

public class NetSvc
{
    private static NetSvc instance = null;

    public static NetSvc Instance {
        get {
            if (instance == null)
            {
                instance = new NetSvc();
            }
            return instance;
        }
    }

    public void Init()
    {
        PESocket<ServerSession, GameMsg> server = new PESocket<ServerSession, GameMsg>();
        server.StartAsServer(SrvCfg.srvIP, SrvCfg.srvPort);

        PECommon.Log("NetSvc Init Done.");
    }

}