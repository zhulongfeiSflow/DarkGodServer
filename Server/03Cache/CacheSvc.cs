/****************************************************
	文件：CacheSvc.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/09 17:25   	
	功能：缓存层
*****************************************************/

using PEProtocol;
using System.Collections.Generic;

public class CacheSvc {
    private static CacheSvc instance = null;

    public static CacheSvc Instance {
        get {
            if (instance == null) {
                instance = new CacheSvc();
            }
            return instance;
        }
    }
    private DBMgr dBMgr;

    private Dictionary<string, ServerSession> onLineAcctDic = new Dictionary<string, ServerSession>();
    private Dictionary<ServerSession, PlayerData> onLineSessionDic = new Dictionary<ServerSession, PlayerData>();

    public void Init() {
        dBMgr = DBMgr.Instance;
        PECommon.Log("CacheSvc Init Done.");
    }

    public bool IsAcctOnLine(string acct) {

        return onLineAcctDic.ContainsKey(acct);
    }

    /// <summary>
    /// 根据账号密码返回对应账号数据，密码错误返回null，账号不存在则默认创建新账号
    /// </summary>
    public PlayerData GetPlayerData(string acct, string pass) {
        return dBMgr.QueryPlayerData(acct, pass);
    }

    /// <summary>
    /// 账号上线，缓存数据
    /// </summary>
    public void AcctOnline(string acct, ServerSession session, PlayerData playerData) {
        onLineAcctDic.Add(acct, session);
        onLineSessionDic.Add(session, playerData);
    }

    public bool IsNameExist(string name) {
        return dBMgr.QueryNameData(name);
    }

    public List<ServerSession> GetOnLineServerSessions() {
        List<ServerSession> lst = new List<ServerSession>();
        foreach (var item in onLineSessionDic) {
            lst.Add(item.Key);
        }
        return lst;
    }

    public PlayerData GetPlayerDataBySession(ServerSession session) {
        if (onLineSessionDic.TryGetValue(session, out PlayerData playerData)) {
            return playerData;
        }
        else {
            return null;
        }
    }

    public Dictionary<ServerSession, PlayerData> GetOnlineCache() {
        return onLineSessionDic;
    }

    public ServerSession GetOnlineServerSession(int ID) {
        ServerSession session = null;
        foreach (var item in onLineSessionDic) {
            if (item.Value.id == ID) {
                session = item.Key;
                break;
            }
        }
        return session;
    }

    public bool UpdatePlayerData(int id, PlayerData playerData) {
        return dBMgr.UpdatePlayerData(id, playerData);
    }

    public void AcctOffLine(ServerSession session) {
        foreach (var item in onLineAcctDic) {
            if (item.Value == session) {
                onLineAcctDic.Remove(item.Key);
                break;
            }
        }

        bool succ = onLineSessionDic.Remove(session);
        PECommon.Log("OffLine Result: SessionID:" + session.sessionID + "   " + succ);
    }
}
