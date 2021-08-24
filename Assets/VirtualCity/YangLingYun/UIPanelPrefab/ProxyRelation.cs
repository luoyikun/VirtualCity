using Framework.UI;
using Newtonsoft.Json;
using ProtoDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProxyRelation : MonoBehaviour
{
    public GameObject UserInfo;
    public GameObject ProxyPar;
    public GameObject ProxyTmp;
    public GameObject AddTmp;
    List<ProxyUser> ProxyUserList;
    int ProxyMaxNumber = 0;
    ProxyUser m_ProxyUser;
    // Use this for initialization
    void Start()
    {

    }

    
    public void UpdateProxyUser(RspUpdateProxyMessage m_RspUPM)
    {
        if (m_RspUPM.proxyLeve == 1)
        {//代理等级1代表为自己的直接下级
            GameObject obj = PublicFunc.CreateTmp(ProxyTmp, ProxyPar.transform);
            obj.name = m_RspUPM.proxyUser.user.accountId.ToString();
            ClickListener.Get(obj.transform.Find("HeadImage").gameObject).onClick = clickHeadImage;
            obj.transform.Find("UserName").GetComponent<Text>().text = m_RspUPM.proxyUser.user.userName;
            PublicFunc.CreateHeadImg(obj.transform.Find("HeadImage").GetComponent<Image>(), m_RspUPM.proxyUser.user.modelId);
            obj.transform.Find("ShengYu").GetComponent<Text>().text = "个人剩余代理位：<color=#0A7AE8>" + (ProxyMaxNumber - m_RspUPM.proxyUser.proxyNum) + "</color>";
            obj.transform.Find("ZongDaiLi").GetComponent<Text>().text = "总代理位：<color=#0A7AE8>" + m_RspUPM.proxyUser.proxyTotleNum+ "</color>";
            ClickListener.Get(obj.transform.Find("ChatBtn").gameObject).onClick = clickChat;
            if ((ProxyMaxNumber - m_ProxyUser.proxyNum) != 0)
            {
                if (ProxyPar.transform.Find("AddTmp") == null)
                {
                    obj = PublicFunc.CreateTmp(AddTmp, ProxyPar.transform);
                    obj.name = "AddTmp";
                    ClickListener.Get(obj.transform.Find("AddImage").gameObject).onClick = clickAdd;
                }
                else
                {
                    obj = ProxyPar.transform.Find("AddTmp").gameObject;
                    obj.transform.SetSiblingIndex(ProxyPar.transform.childCount);
                }
            }
            m_ProxyUser.proxyNum = m_ProxyUser.proxyNum - 1;//个人剩余代理数量减1
            m_ProxyUser.proxyNum= m_ProxyUser.proxyTotleNum + 1;//个人总代理数量加1
        }
        else
        {//如果代理等级不为1，表示不是自己的直接下级，不需要创建子物体
            m_ProxyUser.proxyTotleNum = m_ProxyUser.proxyTotleNum + 1;//个人总代理数量加1
        }
        UpdateUserInfo(m_ProxyUser);//更新个人文字信息
    }
    public void Init(List<ProxyUser> m_ProxyUserList)
    {
        ProxyUserList = m_ProxyUserList;
        for (int i = 0; i < DataMgr.businessModelProperties.Count; i++)
        {
            if (DataMgr.businessModelProperties[i].Name == "proxy_max")
            {
                JieXi m_JieXi = new JieXi();
                m_JieXi = JsonConvert.DeserializeObject<JieXi>(DataMgr.businessModelProperties[i].Con);
                ProxyMaxNumber = int.Parse(m_JieXi.v);
            }
        }
        m_ProxyUser = m_ProxyUserList[0];
        // UserInfo.transform.Find("UserName").GetComponent<Text>().text = DataMgr.m_proxyUser[0].user.userName;
        //// UserInfo.transform.Find("HeadImage")
        // PublicFunc.CreateHeadImg(UserInfo.transform.Find("HeadImage").GetComponent<Image>(), DataMgr.m_proxyUser[0].user.modelId);
        // UserInfo.transform.Find("ShengYuText").GetComponent<Text>().text = "个人剩余可代理位：<color=#0A7AE8>" + (ProxyMaxNumber - DataMgr.m_proxyUser[0].proxyNum).ToString() + "</color>";
        // UserInfo.transform.Find("ZongDaiLi").GetComponent<Text>().text = "总代理位数：<color=#0A7AE8>" + DataMgr.m_proxyUser[0].proxyTotleNum.ToString() + "</color>";
        for (int i = ProxyPar.transform.childCount-1; i >=0; i--)
        {
            DestroyImmediate(ProxyPar.transform.GetChild(i).gameObject);
        }
        if (m_ProxyUserList != null)
        {
            for (int i = 0; i < m_ProxyUserList.Count; i++)
            {
                if (m_ProxyUserList[i].user.accountId == DataMgr.m_account.id)
                {
                    m_ProxyUser = m_ProxyUserList[i];
                    UpdateUserInfo(m_ProxyUser);
                    continue;
                }
                GameObject obj = PublicFunc.CreateTmp(ProxyTmp, ProxyPar.transform);
                obj.name = m_ProxyUserList[i].user.accountId.ToString();
                ClickListener.Get(obj.transform.Find("HeadImage").gameObject).onClick = clickHeadImage;
                obj.transform.Find("UserName").GetComponent<Text>().text = m_ProxyUserList[i].user.userName;
                PublicFunc.CreateHeadImg(obj.transform.Find("HeadImage").GetComponent<Image>(), m_ProxyUserList[i].user.modelId);
                obj.transform.Find("ShengYu").GetComponent<Text>().text = "个人剩余代理位：<color=#0A7AE8>" + (ProxyMaxNumber - m_ProxyUserList[i].proxyNum) + "</color>";
                obj.transform.Find("ZongDaiLi").GetComponent<Text>().text = "总代理位：<color=#0A7AE8>" + m_ProxyUserList[i].proxyTotleNum+ "</color>";
                ClickListener.Get(obj.transform.Find("ChatBtn").gameObject).onClick = clickChat;
                //obj.transform.Find("ChatBtn").gameObject.SetActive(false);
            }
            //if(DataMgr.m_pro)
            if ((ProxyMaxNumber - m_ProxyUser.proxyNum) != 0)
            {
                if (ProxyPar.transform.Find("AddTmp") == null)
                {
                    GameObject obj = PublicFunc.CreateTmp(AddTmp, ProxyPar.transform);
                    obj.name = "AddTmp";
                    ClickListener.Get(obj.transform.Find("AddImage").gameObject).onClick = clickAdd;
                }
            }
        }
        else if (m_ProxyUserList == null)
        {
            Debug.Log("没有代理关系！");
        }
    }
    void UpdateUserInfo(ProxyUser AccountProxyUser)
    {
        UserInfo.transform.Find("UserName").GetComponent<Text>().text = DataMgr.m_account.userName;
        PublicFunc.CreateHeadImg(UserInfo.transform.Find("HeadImage").GetComponent<Image>(), (long)DataMgr.m_account.modleId);
        UserInfo.transform.Find("ShengYuText").GetComponent<Text>().text = "个人剩余代理位：<color=#0A7AE8>" + (ProxyMaxNumber - AccountProxyUser.proxyNum).ToString() + "</color>";
        UserInfo.transform.Find("ZongDaiLi").GetComponent<Text>().text = "总代理位：<color=#0A7AE8>" + AccountProxyUser.proxyTotleNum.ToString() + "</color>";
    }
    void clickChat(GameObject obj)
    {
        Debug.Log("ClickChat");
    }
    void clickHeadImage(GameObject obj)
    {
        for (int i = 0; i < ProxyUserList.Count; i++)
        {
            if (ProxyUserList[i].user.accountId == long.Parse(obj.transform.parent.name))
            {
                ChatUser Target_Chatuser = ProxyUserList[i].user;
                UIManager.Instance.PushPanel(UIPanelName.userinfopanel, false, true, paragrm => { paragrm.GetComponent<userinfopanel>().Init(Target_Chatuser); });
                break;
            }
        }
    }
    void clickAdd(GameObject obj)
    {
        
        if (DataMgr.m_account.hadProxy == 0)
        {
            Hint.LoadTips("请先获取代理权", Color.white);
        }
        else
        {
            string url = AppConst.m_shareUrl + DataMgr.m_account.commendCode;
            AndroidFunc.WxShareWebpageCross(url);
        }
    }
}
