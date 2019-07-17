/****************************************************
	文件：GuideSys.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/16 13:10   	
	功能：引导业务系统
*****************************************************/

using PEProtocol;

public class GuideSys
{
    private static GuideSys instance = null;

    public static GuideSys Instance {
        get {
            if (instance == null)
            {
                instance = new GuideSys();
            }
            return instance;
        }
    }
    private CacheSvc cacheSvc = null;
    private CfgSvc cfgSvc = null;


    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        cfgSvc = CfgSvc.Instance;
        PECommon.Log("GuideSys Init Done.");
    }

    public void ReqGuide(MsgPack pack)
    {
        ReqGuide data = pack.msg.reqGuide;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspGuide
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        GuideCfg gc = cfgSvc.GetGuideCfg(data.guideid);

        //更新引导ID
        if (pd.guideid == data.guideid)
        {
            pd.guideid += 1;

            //更新玩家数据
            pd.coin += gc.coin;
            CalcExp(pd, gc.exp);

            if (!cacheSvc.UpdatePlayerData(pd.id, pd))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.rspGuide = new RspGuide
                {
                    guideid = pd.guideid,
                    coin = pd.coin,
                    lv = pd.lv,
                    exp = pd.exp,
                };
            }
        }
        else
        {
            msg.err = (int)ErrorCode.ServerDataError;
        }
        pack.session.SendMsg(msg);
    }

    private void CalcExp(PlayerData pd, int addExp)
    {
        int curtLv = pd.lv;
        int curtExp = pd.exp;
        int addRestExp = addExp;
        while (true)
        {
            int upNeedExp = PECommon.GetExpUpValByLv(curtLv) - curtExp;
            if (addRestExp >= upNeedExp)
            {
                curtLv += 1;
                curtExp = 0;
                addRestExp -= upNeedExp;
            }
            else
            {
                pd.lv = curtLv;
                pd.exp = curtExp + addRestExp;
                break;
            }
        }
    }
}