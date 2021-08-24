using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpProto
{ 
    public static List<string> m_listRecv = new List<string>();
    public static List<string> m_listSend = new List<string>();
    //public static List<string> m_listNeedLoad = new List<string>();
    //public static List<string> m_listCancelLoad = new List<string>();
    public static Dictionary<string, string> m_dicLoad = new Dictionary<string, string>();
    public static void StartUp()
    {
        m_listRecv.Add(MsgIdDefine.RspGameDataMessage);
        m_listRecv.Add(MsgIdDefine.RspSyncStreetMessage);
        m_listRecv.Add(MsgIdDefine.RspSyncShopMessage);
        m_listRecv.Add(MsgIdDefine.RspBuildInfoMessage);
        m_listRecv.Add(MsgIdDefine.RspRunInShopMessage);
        m_listRecv.Add(MsgIdDefine.RspRunInStreetMessage);
        m_listRecv.Add(MsgIdDefine.RspGetHousePartMessage);
        m_listRecv.Add(MsgIdDefine.RspLoadPlayerMessage);
        m_listRecv.Add(MsgIdDefine.RspGetSocialityInfoMessage);
        m_listRecv.Add(MsgIdDefine.RspQueryCommentsMessage);

        m_listRecv.Add(MsgIdDefine.RspGetGoodsListMessage);
        m_listRecv.Add(MsgIdDefine.RspGoodsInfoMessage);
        m_listRecv.Add(MsgIdDefine.RspQueryBillMessage);
        m_listRecv.Add(MsgIdDefine.RspQueryOrderMessage);
        m_listRecv.Add(MsgIdDefine.RspGetRankMessage);

        m_dicLoad.Add(MsgIdDefine.ReqCreateCommentsMessage, MsgIdDefine.RspCreateCommentsMessage);
        m_dicLoad.Add(MsgIdDefine.ReqCreateOrderMessage, MsgIdDefine.RspCreateOrderMessage);
        m_dicLoad.Add(MsgIdDefine.ReqConfirmReceiptMessage, MsgIdDefine.RspComfirmReceiptMessage);
        m_dicLoad.Add(MsgIdDefine.ReqGetPorxyUserMessage, MsgIdDefine.RspGetProxyUserMessage);
        m_dicLoad.Add(MsgIdDefine.ReqGetRankMessage, MsgIdDefine.RspGetRankMessage);
        m_dicLoad.Add(MsgIdDefine.ReqGoodsInfoMessage,MsgIdDefine.RspGoodsInfoMessage);
        m_dicLoad.Add(MsgIdDefine.ReqSearchUserMessage,MsgIdDefine.RspSearchUserMessage);
        m_dicLoad.Add(MsgIdDefine.ReqQueryCommentsMessage,MsgIdDefine.RspQueryCommentsMessage);
        m_dicLoad.Add(MsgIdDefine.ReqRewardMoneyTreeMessage,MsgIdDefine.RspRewardMoneyTreeMessage);
        m_dicLoad.Add(MsgIdDefine.ReqGetProxyMessage,MsgIdDefine.RspGetProxyPayMessage);
    }

    public static bool IsRecvContain(string str)
    {
        for (int i = 0; i < m_listRecv.Count; i++)
        {
            if (m_listRecv[i] == str)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsSendContain(string str)
    {
        for (int i = 0; i < m_listSend.Count; i++)
        {
            if (m_listSend[i] == str)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsNeedLoadContain(string str)
    {
        //for (int i = 0; i < m_listNeedLoad.Count; i++)
        //{
        //    if (m_listNeedLoad[i] == str)
        //    {
        //        return true;
        //    }
        //}
        if (m_dicLoad.ContainsKey(str))
        {
            return true;
        }
        return false;
    }

    public static bool IsCancelLoadContain(string str)
    {
        //for (int i = 0; i < m_listCancelLoad.Count; i++)
        //{
        //    if (m_listCancelLoad[i] == str)
        //    {
        //        return true;
        //    }
        //}
        if (m_dicLoad.ContainsValue(str))
        {
            return true;
        }
        return false;
    }
}
