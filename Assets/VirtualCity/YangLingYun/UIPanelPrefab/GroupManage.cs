using Net;
using ProtoDefine;
using SGF.Codec;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;
using SuperScrollView;

public class GroupManage : MonoBehaviour {
    public GameObject BackBtn;
    public GameObject DeleteBtn;
    public GameObject MemberTmp;
    public GameObject MemberPar;
    List<long> selectlist = new List<long>();
    public GameObject SearchBtn;
    public InputField IF;
    public bool IsGourpManager = false;

    public LoopListView2 scrolLoopListView;

    private bool IsScrollInit = false;

    private int TotalCount = 0;
    //bool IsSearchPerson = false;
    private void OnEnable()
    {
    }
    private void Start()
    {
        //IF.onEndEdit.AddListener(delegate { EndInput(IF); });
        ClickListener.Get(BackBtn).onClick = clickBackBtn;
        ClickListener.Get(DeleteBtn).onClick = clickDeleteBtn;
        ClickListener.Get(SearchBtn).onClick = clickSearchBtn;
    }
    void UpdateSelectList()
    {
        for(int i = 0; i < MemberPar.transform.childCount; i++)
        {
            MemberPar.transform.GetChild(i).Find("SelectBtn").GetChild(1).gameObject.SetActive(false);
        }
        if (selectlist.Count != 0)
        {
            for (int i = 0; i < selectlist.Count; i++)
            {
                if (MemberPar.transform.Find(selectlist[i].ToString()) == null)
                {
                    selectlist.Remove(selectlist[i]);
                    continue;
                }
                MemberPar.transform.Find(selectlist[i].ToString()).Find("SelectBtn").GetChild(1).gameObject.SetActive(true);
            }
        }
    }
    public void InitMember(List<ChatUser> m_ListChatUser)
    {
        IF.text = "";
        if (m_ListChatUser == null)
        {

        }
        if (MemberPar.transform.childCount != 0)
        {
            //for (int i = 0; i < MemberPar.transform.childCount; i++)
            //{
            //    DestroyImmediate(MemberPar.transform.GetChild(i).gameObject);
            //}
            for (int i = MemberPar.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(MemberPar.transform.GetChild(i).gameObject);
            }
        }
        //if(m_ListChatUser!)
        for (int i = 0; i < m_ListChatUser.Count; i++)
        {
            if (m_ListChatUser[i].userName == DataMgr.m_account.userName)
            {
                continue;
            }
            GameObject obj = PublicFunc.CreateTmp(MemberTmp, MemberPar.transform);
            obj.name = m_ListChatUser[i].accountId.ToString();
            obj.transform.Find("MemberImage").GetChild(0).gameObject.SetActive(false);
            if (m_ListChatUser[i].accountId == chatwindowspanel.Target_ChatGroup.AccountId)
            {
                obj.transform.Find("MemberImage").Find("Image").gameObject.SetActive(true);
            }
            ClickListener.Get(obj.transform.Find("MemberImage").gameObject).onClick = clickHeadImage;
            PublicFunc.CreateHeadImg(obj.transform.Find("MemberImage").GetComponent<Image>(), m_ListChatUser[i].modelId);
            obj.transform.Find("MemberName").GetComponent<Text>().text = m_ListChatUser[i].userName;
            if (chatwindowspanel.cwp.IsManager == true)
            {
                obj.transform.Find("SelectBtn").gameObject.SetActive(true);
                ClickListener.Get(obj.transform.Find("SelectBtn").gameObject).onClick = clickSelectBtn;
            }
            else if (chatwindowspanel.cwp.IsManager == false)
            {
                obj.transform.Find("SelectBtn").gameObject.SetActive(false);
            }
        }
        if (chatwindowspanel.cwp.IsManager == true)
        {
            DeleteBtn.SetActive(true);
        }
        else if (chatwindowspanel.cwp.IsManager == false)
        {
            DeleteBtn.SetActive(false);
        }
        UpdateSelectList();
    }

    void InitScroll()
    {

    }
    public void Init()
    {
        //ReqSearchUserMessage ReqSUM = new ReqSearchUserMessage();
        //ReqSUM.groupId = chatwindowspanel.Target_ChatGroup.Id;
        //ReqSUM.userName = null;
        //ChatSocket.Instance.SendMsgProto(MsgIdDefine.ReqSearchUserMessage, ReqSUM, EnSocket.Chat);
        //IsSearchPerson = false;
        //if (chatwindowspanel.Target_ChatGroup.AccountId == DataMgr.m_account.id)
        //{
        //    IsGourpManager = true;
        //}
        //else if (chatwindowspanel.Target_ChatGroup.AccountId != DataMgr.m_account.id)
        //{
        //    IsGourpManager = false;
        //}
        if (chatwindowspanel.cwp.IsManager == true)
        {
            DeleteBtn.SetActive(true);
        }
        else if (chatwindowspanel.cwp.IsManager == false)
        {
            DeleteBtn.SetActive(false);
        }
    }
    public void EndInput(InputField ipt)
    {
        if (ipt.text == "")
        {
            chatwindowspanel.cwp.IsSearch =false;
        }
        ReqSearchUserMessage ReqSUM = new ReqSearchUserMessage();
        ReqSUM.groupId = chatwindowspanel.Target_ChatGroup.Id;
        ReqSUM.userName = ipt.text;
        ChatSocket.Instance.SendMsgProto(MsgIdDefine.ReqSearchUserMessage, ReqSUM, EnSocket.Chat);
        chatwindowspanel.cwp.IsSearch = true;
        //IsSearchPerson = true;
    }
    void clickSearchBtn(GameObject obj)
    {
        EndInput(IF);
    }
    void clickSelectBtn(GameObject obj)
    {
        //if (obj.transform.GetChild(1).gameObject.activeSelf == true)
        //{
        //    for (int i = 0; i < selectlist.Count; i++)
        //    {
        //        if (selectlist[i].gameObject == obj.transform.parent.gameObject)
        //        {
        //            selectlist.Remove(obj.transform.parent.gameObject);
        //            obj.transform.GetChild(1).gameObject.SetActive(false);
        //        }
        //    }
        //}
        //else if (obj.transform.GetChild(1).gameObject.activeSelf == false)
        //{
        //    selectlist.Add(obj.transform.parent.gameObject);
        //    obj.transform.GetChild(1).gameObject.SetActive(true);
        //}
        if (selectlist.Contains(long.Parse(obj.transform.parent.name)))
        {
            selectlist.Remove(long.Parse(obj.transform.parent.name));
        }
        else
        {
            selectlist.Add(long.Parse(obj.transform.parent.name));
        }
        UpdateSelectList();
    }
    void clickHeadImage(GameObject obj)
    {
        chatwindowspanel.cwp.clickHeadImage(obj.transform.parent.gameObject);
    }
    void clickBackBtn(GameObject obj)
    {
        chatwindowspanel.cwp.clickBackBtn(obj);
    }
    void clickDeleteBtn(GameObject obj)
    {
        ReqDeleteParaMateMessage ReqDPMM = new ReqDeleteParaMateMessage();
        ReqDPMM.paramateIds = selectlist;
        ReqDPMM.groupId = chatwindowspanel.Target_ChatGroup.Id;
        //for (int i=0;i< selectlist.Count; i++)
        //{
        //    ReqDPMM.paramateIds.Add(selectlist[i]);
        //    //selectlist[i].transform.Find("SelectBtn").GetChild(1).gameObject.SetActive(false);
        //}
        ChatSocket.Instance.SendMsgProto(MsgIdDefine.ReqDeleteParaMateMessage, ReqDPMM);
        selectlist.Clear();
        UpdateSelectList();
    }
}
