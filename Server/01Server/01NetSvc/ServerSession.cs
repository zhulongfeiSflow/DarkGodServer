using PEProtocol;
using PENet;

public class ServerSession : PESession<GameMsg>
{
    public int sessionID = 0;

    protected override void OnConnected()
    {
        sessionID = ServerRoot.Instance.GetSessionID();

        PECommon.Log("SessionID:" + sessionID + "   Client Connect");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        PECommon.Log("SessionID:" + sessionID + " RcvPack CMD:" + ((CMD)msg.cmd).ToString());
        NetSvc.Instance.AddMsgQue(this, msg);
    }

    protected override void OnDisConnected()
    {
        LoginSys.Instance.ClearOffLineData(this);
        PECommon.Log("SessionID:" + sessionID + "   Client OffLine");
    }


}
