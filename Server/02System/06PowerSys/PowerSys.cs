/****************************************************
	文件：PowerSys.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/19 13:06   	
	功能：体力恢复系统
*****************************************************/

using PEProtocol;
using System.Collections.Generic;

public class PowerSys {
    private static PowerSys instance = null;

    public static PowerSys Instance {
        get {
            if (instance == null) {
                instance = new PowerSys();
            }
            return instance;
        }
    }
    private CacheSvc cacheSvc = null;
    private TimerSvc timerSvc = null;

    public void Init() {
        cacheSvc = CacheSvc.Instance;
        timerSvc = TimerSvc.Instance;

        TimerSvc.Instance.AddTimeTask(CalcPowerAdd, PECommon.PowerAddSpace, PETimeUnit.Minute, 0);

        PECommon.Log("PowerSys Init Done.");
    }

    private void CalcPowerAdd(int tid) {
        //计算体力正增长 TODO
        PECommon.Log("All Online Player Calc Power Increase...");
        GameMsg msg = new GameMsg {
            cmd = (int)CMD.PshPower,
        };
        msg.pshPower = new PshPower();

        //所有在线玩家获得实时的体力增长推送数据
        Dictionary<ServerSession, PlayerData> onlineDic = cacheSvc.GetOnlineCache();
        foreach (var item in onlineDic) {
            PlayerData pd = item.Value;
            ServerSession session = item.Key;

            int powerMax = PECommon.GetPowerLimit(pd.lv);
            if (pd.power >= powerMax) {
                continue;
            }
            else {
                pd.power += PECommon.PowerAddCount;
                pd.time = timerSvc.GetNowTime();
                if (pd.power >= powerMax) {
                    pd.power = powerMax;
                }
            }

            if (!cacheSvc.UpdatePlayerData(pd.id, pd)) {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else {
                msg.pshPower.power = pd.power;
                session.SendMsg(msg);
            }
        }
    }
}