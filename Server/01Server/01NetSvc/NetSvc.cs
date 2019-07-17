/****************************************************
	文件：NetSvc.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/08 10:29   	
	功能：网络服务
*****************************************************/

using PENet;
using PEProtocol;
using System.Collections.Generic;

public class MsgPack
{
    public ServerSession session;
    public GameMsg msg;
    public MsgPack(ServerSession session, GameMsg msg)
    {
        this.session = session;
        this.msg = msg;
    }
}

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
    public static readonly string obj="lock";
    private Queue<MsgPack> msgPackQue = new Queue<MsgPack>();

    public void Init()
    {
        PESocket<ServerSession, GameMsg> server = new PESocket<ServerSession, GameMsg>();
        //server.StartAsServer(SrvCfg.srvIP, SrvCfg.srvPort);
        //server.StartAsServer("192.168.254.100", SrvCfg.srvPort);  //外网测试 
        server.StartAsServer("192.168.254.100", 17888);             //内网测试

        PECommon.Log("NetSvc Init Done.");
    }

    public void AddMsgQue(ServerSession session, GameMsg msg)
    {
        lock (obj)
        {
            msgPackQue.Enqueue(new MsgPack(session, msg));
        }
    }

    public void Update()
    {
        if (msgPackQue.Count>0)
        {
            //PECommon.Log("PackCount:" + msgPackQue.Count);
            lock (obj)
            {
                MsgPack pack = msgPackQue.Dequeue();
                HandOutMsg(pack);
            }
        }
    }

    private void HandOutMsg(MsgPack pack)
    {
        switch ((CMD)pack.msg.cmd)
        {
            case CMD.ReqLogin:
                LoginSys.Instance.ReqLogin(pack);
                break;
            case CMD.ReqRename:
                LoginSys.Instance.ReqRename(pack);
                break;
            case CMD.ReqGuide:
                GuideSys.Instance.ReqGuide(pack);
                break;
            case CMD.ReqStrong:
                StrongSys.Instance.ReqStrong(pack);
                break;
        }
    }
}