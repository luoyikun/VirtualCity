using Framework.UI;
using Net;
using ProtoDefine;
using SGF.Codec;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupSetting : MonoBehaviour {
    public GameObject BackBtn;
    //public bool IsManager=false;
    //bool IsQuit=false;
    //bool IsEditOver = false;
    //bool IsCreateAdd = false;
    public GameObject IFMask;
    public GameObject MemberPar;
    public GameObject MemberTmp;
    public GameObject AddMemberBtn;
    public GameObject DeleteBtn;
    public InputField IF;
    public GameObject MemberBG;

    // Use this for initialization
    void Start () {
        IF.onEndEdit.AddListener(delegate { EndInput(IF); });
        ClickListener.Get(BackBtn).onClick = clickBackBtn;
        ClickListener.Get(MemberBG).onClick = clickMemberBG;
    }
    void clickMemberBG(GameObject obj)
    {
        chatwindowspanel.cwp.InitMember("GroupManage");
    }

    public void updateGroupName(string groupName)
    {
        //IF.transform.Find("Placeholder").GetComponent<Text>().text = chatwindowspanel.Target_ChatGroup.Name;
    }
    public void InitMember(List<ChatUser> mListChatUser)
    {
        IF.transform.Find("Placeholder").GetComponent<Text>().text = chatwindowspanel.Target_ChatGroup.Name;
        for (int i = MemberPar.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(MemberPar.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < mListChatUser.Count; i++)
        {
            if (i == 4 )
            {
                break;
            }
            GameObject obj = PublicFunc.CreateTmp(MemberTmp, MemberPar.transform);
            obj.name = mListChatUser[i].accountId.ToString();
            PublicFunc.CreateHeadImg(obj.transform.GetComponent<Image>(), mListChatUser[i].modelId);
            //Debug.Log("CreateSetting1");
            // ClickListener.Get(obj).onClick = chatpanel.cp.clickHeadImage;
        }
        if (MemberPar.transform.Find("AddBtn")==null)
        {
            GameObject addmember = PublicFunc.CreateTmp(AddMemberBtn, MemberPar.transform);
            addmember.name = "AddBtn";
            ClickListener.Get(addmember).onClick = clickAddMemberBtn;
        }
        if (chatwindowspanel.cwp.IsManager == true)
        {
            IFMask.SetActive(false);
            DeleteBtn.transform.GetChild(0).GetComponent<Text>().text = "解散此群";
            ClickListener.Get(DeleteBtn).onClick = clickDissolveGroupBtn;
        }
        else if (chatwindowspanel.cwp.IsManager == false)
        {
            IFMask.SetActive(true);
            DeleteBtn.transform.GetChild(0).GetComponent<Text>().text = "退出此群";
            ClickListener.Get(DeleteBtn).onClick = clickQuitGroupBtn;
        }
    }
    public void Init()
    {
    }
    void EndInput(InputField IF)
    {
        if (IF.text == "" || IF.text == null)
        {
            Hint.LoadTips("修改群名称不能为空", Color.white);
            return;
        }
        chatwindowspanel.cwp.SendEditGroupName(IF.text);
    }
    void clickAddMemberBtn(GameObject obj)
    {
        chatwindowspanel.cwp.InitMember("InvitationFriend");
    }
    void clickDissolveGroupBtn(GameObject obj)
    {
        ispanel ispanel = (ispanel)UIManager.Instance.PushPanelFromRes(UIPanelName.ispanel, UIManager.CanvasType.Screen, false, false);
        ispanel.SetContent("提示", "是否要解散此群？");
        ispanel.m_ok = () =>
        {
            //解散群
            ReqDeleteGroupMessage ReqDGM = new ReqDeleteGroupMessage();
            ReqDGM.groupId = chatwindowspanel.Target_ChatGroup.Id;
            ChatSocket.Instance.SendMsgProto(MsgIdDefine.ReqDeleteGroupMessage, ReqDGM, EnSocket.Chat);
        };
        //IsQuit = true;
    }

    void DissolveGroup()
    {

    }

    void clickBackBtn(GameObject obj)
    {
        chatwindowspanel.cwp.clickBackBtn(obj);
    }
    void clickQuitGroupBtn(GameObject obj)
    {
        ispanel ispanel = (ispanel)UIManager.Instance.PushPanelFromRes(UIPanelName.ispanel, UIManager.CanvasType.Screen, false, false);
        ispanel.SetContent("提示", "是否要退出此群？");
        ispanel.m_ok = () =>
        {
            //退出群
            ReqDeleteParaMateMessage ReqDPMM = new ReqDeleteParaMateMessage();
            ReqDPMM.paramateIds = new List<long>();
            ReqDPMM.paramateIds.Add((long)DataMgr.m_account.id);
            ReqDPMM.groupId = chatwindowspanel.Target_ChatGroup.Id;
            ChatSocket.Instance.SendMsgProto(MsgIdDefine.ReqDeleteParaMateMessage, ReqDPMM, EnSocket.Chat);
        };

        //IsQuit = true;
    }

    void QuitGroup()
    {

    }


    void clickHeadImage(GameObject obj)
    {
        chatpanel.cp.clickHeadImage(obj);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
