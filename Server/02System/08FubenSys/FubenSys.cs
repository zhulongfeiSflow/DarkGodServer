/****************************************************
	文件：FubenSys.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/21 0:35   	
	功能：副本系统
*****************************************************/

using PEProtocol;

public class FubenSys
{
    private static FubenSys instance = null;

    public static FubenSys Instance {
        get {
            if (instance == null) {
                instance = new FubenSys();
            }
            return instance;
        }
    }
    private CacheSvc cacheSvc = null;
    private CfgSvc cfgSvc = null;

    public void Init() {
        cacheSvc = CacheSvc.Instance;
        cfgSvc = CfgSvc.Instance;

        PECommon.Log("FubenSys Init Done.");
    }

    public void ReqFBFight(MsgPack pack) {
        ReqFBFight data = pack.msg.reqFBFight;

        GameMsg msg = new GameMsg {
            cmd = (int)CMD.RspFBFight,
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        int power = cfgSvc.GetMapCfg(data.fbid).power;

        if (pd.fuben < data.fbid) {
            msg.err = (int)ErrorCode.ClientDataError;
        }
        else if (pd.power < power) {
            msg.err = (int)ErrorCode.LackPower;
        }
        else {
            pd.power -= power;
            if (cacheSvc.UpdatePlayerData(pd.id, pd)) {
                msg.rspFBFight = new RspFBFight {
                    fbid = data.fbid,
                    power = pd.power,
                };
            }
            else {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
        }
        pack.session.SendMsg(msg);
    }

}