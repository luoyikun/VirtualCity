using Framework.Pattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using LitJson;

public class TinyPlayer
{
    public long accountId;
    public string name;
}
public class VcData : SingletonMono<VcData>
{
    bool m_isLoad = false;

    public static DicHomeBuildPos m_dicHomeTownBuildPos = new DicHomeBuildPos();
    public static Rootobject m_rootobject = new Rootobject();
    // Use this for initialization
    public static List<TinyPlayer> m_listFakeName = new List<TinyPlayer>();
    public void LoadData(System.Action varFinish = null)
    {
        if (m_isLoad)
        {
            if (varFinish != null)
            {
                varFinish();
            }
            return;
        }
        Instance.StartCoroutine(LoadDataSync(varFinish));
    }

    public IEnumerator LoadDataSync(System.Action varFinish = null)
    {
        yield return Instance.StartCoroutine(AssetMgr.Instance.YieldCreateText(Vc.AbName.buildhomeidxtopos, Vc.AbName.buildhomeidxtopos, Onbuildhomeidxtopos));
        yield return Instance.StartCoroutine(AssetMgr.Instance.YieldCreateText(Vc.AbName.city, Vc.AbName.city, add_data));
        yield return Instance.StartCoroutine(AssetMgr.Instance.YieldCreateText(Vc.AbName.notice, Vc.AbName.notice, OnLoadNotice));
        yield return Instance.StartCoroutine(AssetMgr.Instance.YieldCreateText(Vc.AbName.termofservice, Vc.AbName.termofservice, OnLoadServer));
        yield return Instance.StartCoroutine(AssetMgr.Instance.YieldCreateText(Vc.AbName.fakenames, Vc.AbName.fakenames, fakename));
        yield return Instance.StartCoroutine(AssetMgr.Instance.YieldCreateText(Vc.AbName.gouwujiangli, Vc.AbName.gouwujiangli, gouwujiangli_text));
        yield return Instance.StartCoroutine(AssetMgr.Instance.YieldCreateText(Vc.AbName.dailijiangli, Vc.AbName.dailijiangli, dailijiangli_text));
        yield return Instance.StartCoroutine(AssetMgr.Instance.YieldCreateText(Vc.AbName.newguide, Vc.AbName.newguide, NewGuideData));
        yield return Instance.StartCoroutine(
            AssetMgr.Instance.YieldCreateText(Vc.AbName.rankhelp, Vc.AbName.rankhelp, rankhelp_text));

        if (varFinish != null)
        {
            varFinish();
        }
        m_isLoad = true;
    }

    void Onbuildhomeidxtopos(string text)
    {
        m_dicHomeTownBuildPos = JsonConvert.DeserializeObject<DicHomeBuildPos>(text);
    }

    void gouwujiangli_text(string text)
    {
        NoticePanel.m_GouWuJiangLi.content = text;
    }

    void dailijiangli_text(string text)
    {
        NoticePanel.m_DaiLiJiangLi.content = text;
    }

    void rankhelp_text(string text)
    {
        NoticePanel.m_RankHelp.content = text;
    }
    void add_data(string text)
    {
        m_rootobject = JsonConvert.DeserializeObject<Rootobject>(text);
    }

    void OnLoadNotice(string text)
    {
        NoticePanel.m_info.content = text;
    }

    void OnLoadServer(string text)
    {
        NoticePanel.m_sevice.v = text;
    }
    void fakename(string text)
    {
        string[] arr = text.Split('|');
        m_listFakeName.Clear();
        for (int i = 0; i < arr.Length; i++)
        {
            TinyPlayer player = new TinyPlayer();
            string[] info = arr[i].Split(',');
            player.accountId = long.Parse(info[0]);
            player.name = info[1];
            m_listFakeName.Add(player);
        }
    }
    void NewGuideData(string text)
    {
        NewGuideMgr.DataInit(text);
    }
}