/****************************************************
	文件：Class1.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/08 10:44   	
	功能：网络通信协议（客户端服务器公用）
*****************************************************/

using PENet;
using System;


namespace PEProtocol {
    [Serializable]
    public class GameMsg : PEMsg {
        public ReqLogin reqLogin;
        public RspLogin rspLogin;

        public ReqRename reqRename;
        public RspRename rspRename;

        public ReqGuide reqGuide;
        public RspGuide rspGuide;

        public ReqStrong reqStrong;
        public RspStrong rspStrong;

        public SndChat sndChat;
        public PshChat pshChat;

        public ReqBuy reqBuy;
        public RspBuy rspBuy;

        public PshPower pshPower;

        public ReqTakeTaskReward reqTakeTaskReward;
        public RspTakeTaskReward rspTakeTaskReward;

        public PshTaskPrgs pshTaskPrgs;
    }

    #region 登录相关
    [Serializable]
    public class ReqLogin {
        public string acct;
        public string pass;
    }

    [Serializable]
    public class RspLogin {
        public PlayerData playerData;
        //TODO
    }

    [Serializable]
    public class PlayerData {
        public int id;
        public string name;
        public int lv;
        public int exp;
        public int power;
        public int coin;
        public int diamond;
        public int crystal;

        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int dodge;//闪避概率
        public int pierce;//穿透比率
        public int critical;//暴击概率

        public int guideid;
        public int[] strongArr;

        public long time;
        public string[] taskArr;
        public int fuben;


        //TOADD
    }

    [Serializable]
    public class ReqRename {
        public string name;
    }
    [Serializable]
    public class RspRename {
        public string name;
    }
    #endregion

    #region 引导相关
    [Serializable]
    public class ReqGuide {
        public int guideid;
    }
    [Serializable]
    public class RspGuide {
        public int guideid;
        public int coin;
        public int lv;
        public int exp;
    }
    #endregion

    #region 强化相关
    [Serializable]
    public class ReqStrong {
        public int pos;
    }

    [Serializable]
    public class RspStrong {
        public int coin;
        public int crystal;
        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int[] strongArr;
    }
    #endregion

    #region 聊天相关
    [Serializable]
    public class SndChat {
        public string chat;
    }

    [Serializable]
    public class PshChat {
        public string name;
        public string chat;
    }
    #endregion

    #region 资源交易相关
    [Serializable]
    public class ReqBuy {
        public int type;
        public int cost;
    }
    [Serializable]
    public class RspBuy {
        public int type;
        public int diamond;
        public int coin;
        public int power;
    }
    [Serializable]
    public class PshPower {
        public int power;
    }
    #endregion

    #region 任务奖励
    [Serializable]
    public class ReqTakeTaskReward {
        public int rid;
    }
    [Serializable]
    public class RspTakeTaskReward {
        public int coin;
        public int lv;
        public int exp;
        public string[] taskArr;
    }
    [Serializable]
    public class PshTaskPrgs {
        public string[] taskArr;
    }
    #endregion

    public enum ErrorCode {
        None = 0,         //没有错误

        ServerDataError,//服务器数据异常
        UpdateDBError,  //更新数据库错误
        ClientDataError,  //更新数据库错误

        AcctIsOnLine,   //账号已经上线
        WrongPass,      //密码错误
        NameIsExist,    //名字已经存在

        LackLevel,
        LackCoin,
        LackCrystal,
        LackDiamond,
    }

    public enum CMD {
        None = 0,
        //登录相关 100
        ReqLogin = 101,
        RspLogin = 102,

        ReqRename = 103,
        RspRename = 104,

        //主城相关 200
        ReqGuide = 201,
        RspGuide = 202,

        ReqStrong = 203,
        RspStrong = 204,

        SndChat = 205,
        PshChat = 206,

        ReqBuy = 207,
        RspBuy = 208,

        PshPower = 209,

        ReqTakeTaskReward = 210,
        RspTakeTaskReward = 211,

        PshTaskPrgs = 212,
    }

    public class SrvCfg {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 17666;

        public const string localIP = "192.168.254.100";
        public const string publicID = "125.118.111.61"; //临时的公网IP
        public const string strDomain = "2a580b6706.zicp.vip"; //我的花生壳域名访问

        public const int innerPort = 17888;//内网测试端口
        public const int innerPort2 = 17666;//用于外网连接的内部转接端口
        public const int externalPort = 9009;//外网接入端口


    }
}
