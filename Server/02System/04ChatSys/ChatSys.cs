/****************************************************
	文件：ChatSys.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/18 16:12   	
	功能：
*****************************************************/
using PEProtocol;
using System.Collections.Generic;

public class ChatSys {
    private static ChatSys instance = null;

    public static ChatSys Instance {
        get {
            if (instance == null) {
                instance = new ChatSys();
            }
            return instance;
        }
    }
    private CacheSvc cacheSvc = null;

    public void Init() {
        cacheSvc = CacheSvc.Instance;
        PECommon.Log("ChatSys Init Done.");
    }

    public void SndChat(MsgPack pack) {
        SndChat data = pack.msg.sndChat;
        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);

        GameMsg msg = new GameMsg {
            cmd = (int)CMD.PshChat,
            pshChat = new PshChat {
                name = pd.name,
                chat = data.chat
            },
        };

        //同样的数据包多次分发只序列化一次
        byte[] bytes = PENet.PETool.PackNetMsg(msg);

        //并包处理
        //任务进度数据更新
        msg.pshTaskPrgs = TaskSys.Instance.GetPshTaskPrgs(pd, 6);

        if (!cacheSvc.UpdatePlayerData(pd.id, pd)) {
            msg.err = (int)ErrorCode.UpdateDBError;
            pack.session.SendMsg(msg);
        }
        else {
            //广播所有在线客户端
            List<ServerSession> lst = cacheSvc.GetOnLineServerSessions();
            for (int i = 0; i < lst.Count; i++) {

                if (lst[i] != pack.session) {
                    lst[i].SendMsg(bytes);
                }
                else {
                    //自己发的消息要更新任务进度
                    pack.session.SendMsg(msg);
                }
            }
        }


    }

}
