using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;
using ProtoDefine;
using BestHTTP.WebSocket;
using System.Text;
using Newtonsoft.Json;
using Framework.Event;

public class WebSocket_Message
{
    public long ID;
    public string FROMNAME;
    public string TONAME;
    public string MESSAGETEXT;
    public int MESSAGESTATUS;
    public string MESSAGEDATE;
}
public class shoppingchatpanel : UGUIPanel {
    public string url;
    private WebSocket m_WebSocket;
    public GameObject ChatWindowsPar;
    public GameObject ZhangGuiChatTmp;
    public GameObject SelfChatTmp;
    public GameObject GoodsTmp;
    public GameObject SendBtn;
    public GameObject BackBtn;
    public InputField m_InputFiled;
    public Text BusinessName;
    Goods Target_Goods;
    public RawImage m_raw;
	// Use this for initialization
	void Start () {
        ClickListener.Get(BackBtn).onClick = clickBackBtn;
        ClickListener.Get(SendBtn).onClick = clickSendBtn;
        m_raw.texture = SYJMgr.m_instance.m_camZhangGui.GetComponent<Camera>().targetTexture;
    }
    public override void OnOpen()
    {
        EventManager.Instance.DispatchEvent(Common.EventStr.ChatWithShopkeeper, new EventDataEx<bool>(true));
    }
    public override void OnClose()
    {
        EventManager.Instance.DispatchEvent(Common.EventStr.ChatWithShopkeeper, new EventDataEx<bool>(false));
    }
    void webSocketInit()
    {
        url = "ws://47.99.99.79:8083/shop//websocket/socketServer.do?SESSION_USERNAME=" + DataMgr.m_account.userName;
        m_WebSocket = new WebSocket(new Uri(url));
        m_WebSocket.OnOpen += OnOpen;
        m_WebSocket.OnMessage += OnMessageRecived;
        m_WebSocket.OnError += OnError;
        m_WebSocket.OnClosed += OnClosed;
    }
    private void antiInit()
    {
        m_WebSocket.OnOpen = null;
        m_WebSocket.OnMessage = null;
        m_WebSocket.OnError = null;
        m_WebSocket.OnClosed = null;
        m_WebSocket = null;
    }
    void OnOpen(WebSocket ws)
    {
        if (Target_Goods != null)
        {
            SendGoods();
        }
        Debug.Log("Conneted");
    }
    void OnClosed(WebSocket ws,UInt16 code, string message)
    {
        antiInit();
        webSocketInit();
    }
    void OnMessageRecived(WebSocket ws, string message)
    {
        
        WebSocket_Message m_WebSocketMessage = JsonConvert.DeserializeObject<WebSocket_Message>(message);
        if (m_WebSocketMessage.FROMNAME == BusinessName.text)
        {
            if (m_WebSocketMessage.MESSAGESTATUS == 0)
            {
                UpdateWebSocketText(m_WebSocketMessage);
            }
            else if (m_WebSocketMessage.MESSAGESTATUS == 2)
            {
                UpdateWebSocketGood(m_WebSocketMessage);
            }
        }
    }
    void UpdateWebSocketText(WebSocket_Message m_WebSocketMessage)
    {
        GameObject obj = PublicFunc.CreateTmp(ZhangGuiChatTmp, ChatWindowsPar.transform);
        obj.GetComponent<Text>().text = m_WebSocketMessage.MESSAGETEXT;
        setChatWindowsParHeight();
        // setChatWindowsParHeight();
    }
    void UpdateWebSocketGood(WebSocket_Message m_WebSocketMessage)
    {
        Goods m_goods = JsonConvert.DeserializeObject<Goods>(m_WebSocketMessage.MESSAGETEXT);
        createGoodTmp(m_goods);
        //GameObject obj = PublicFunc.CreateTmp(GoodsTmp, LeftMenuPar.transform);
        //obj.name = m_goods.id.ToString();
        //obj.transform.Find("GoodImage").GetComponent<ImageDownLoader>().SetOnlineTexture(AppConst.ImageHeadUrl + m_goods.infoPicture);
        //obj.transform.Find("GoodInfo").GetComponent<Text>().text = m_goods.name;
        //ClickListener.Get(obj).onClick = clickJumpToGood;
    }
    void OnError(WebSocket ws, Exception ex)
    {
        string errorMsg = string.Empty;
#if !UNITY_WEBGL||UNITY_EDITOR
        if (ws.InternalRequest.Response != null)
        {
            errorMsg = string.Format("Status Code from Server:{0} and Message:{1}", ws.InternalRequest.Response.StatusCode, ws.InternalRequest.Response.Message);
        }
#endif
        if (ws.InternalRequest.Response.StatusCode == 404)
        {
            Hint.LoadTips("服务器异常",Color.white);
        }
        Debug.Log(errorMsg);
        antiInit();
        webSocketInit();
    }
    void clickSendBtn(GameObject obj)
    {
        if (m_InputFiled.text != "")
        {
            WebSocket_Message Target_WebSocketMassge = new WebSocket_Message();
            Target_WebSocketMassge.ID = (long)DataMgr.m_account.id;
            Target_WebSocketMassge.FROMNAME = DataMgr.m_account.userName;
            Target_WebSocketMassge.TONAME = Target_Goods.businessName;
            Target_WebSocketMassge.MESSAGETEXT = m_InputFiled.text;
            Target_WebSocketMassge.MESSAGESTATUS = 0;
            Target_WebSocketMassge.MESSAGEDATE = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
            string message = JsonConvert.SerializeObject(Target_WebSocketMassge);
            m_WebSocket.Send(message);
            createSelfTextTmp();
        }
    }
    void createSelfTextTmp()
    {
        GameObject obj = PublicFunc.CreateTmp(SelfChatTmp, ChatWindowsPar.transform);
        obj.GetComponent<Text>().text = m_InputFiled.text;
        m_InputFiled.text = "";
        setChatWindowsParHeight();
    }
    void clickBackBtn(GameObject obj)
    {
        m_WebSocket.Close();
        UIManager.Instance.PopSelf();
    }
    void setChatWindowsParHeight()
    {
        float TargetHeight = 0;
        for (int i = 0; i < ChatWindowsPar.transform.childCount; i++)
        {//遍历滚动承下所有物体，并把他们的高相加，因为子物体的高度不同，所以要每个单独计算
            TargetHeight += ChatWindowsPar.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
        }
        TargetHeight += (ChatWindowsPar.transform.childCount - 1) * ChatWindowsPar.GetComponent<VerticalLayoutGroup>().spacing;//计算每个物体之间所有间隔的高度
        ChatWindowsPar.GetComponent<RectTransform>().sizeDelta = new Vector2(ChatWindowsPar.GetComponent<RectTransform>().sizeDelta.x, TargetHeight);
        if (ChatWindowsPar.GetComponent<RectTransform>().sizeDelta.y <= 840)
        {//如果滚动承窗口小于最大高度840，就将Content的高赋予滚动承，为了让Content的子物体看起来像从底部往上增加
            ChatWindowsPar.transform.parent.parent.GetComponent<ScrollRect>().movementType = ScrollRect.MovementType.Unrestricted;
            ChatWindowsPar.transform.parent.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(ChatWindowsPar.transform.parent.parent.GetComponent<RectTransform>().sizeDelta.x, ChatWindowsPar.GetComponent<RectTransform>().sizeDelta.y);
            ChatWindowsPar.transform.parent.parent.GetComponent<ScrollRect>().movementType = ScrollRect.MovementType.Elastic;
            Debug.Log("Yes111");
        }
        else if (ChatWindowsPar.GetComponent<RectTransform>().sizeDelta.y > 840)
        {
            //LeftMenuPar.transform.parent.parent.GetComponent<ScrollRect>().movementType = ScrollRect.MovementType.Unrestricted;
            ChatWindowsPar.transform.parent.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(ChatWindowsPar.transform.parent.parent.GetComponent<RectTransform>().sizeDelta.x, 840);
            ChatWindowsPar.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (ChatWindowsPar.transform.GetComponent<RectTransform>().sizeDelta.y - 840) / 2);
            //LeftMenuPar.transform.parent.parent.GetComponent<ScrollRect>().movementType = ScrollRect.MovementType.Elastic;
            Debug.Log("No111");
        }
        
    }
    void createGoodTmp(Goods m_Goods)
    {
        GameObject obj;
        obj = PublicFunc.CreateTmp(GoodsTmp, ChatWindowsPar.transform);
        obj.name = m_Goods.id.ToString();
        obj.transform.Find("GoodImage").GetComponent<ImageDownLoader>().SetOnlineTexture(AppConst.ImageHeadUrl + m_Goods.coverPicture);
        obj.transform.Find("GoodInfo").GetComponent<Text>().text = m_Goods.name;
        ClickListener.Get(obj).onClick = clickJumpToGood;
        setChatWindowsParHeight();
    }
    void SendGoods()
    {
        WebSocket_Message Target_WebSocketMassge = new WebSocket_Message();
        Target_WebSocketMassge.ID = (long)DataMgr.m_account.id;
        Target_WebSocketMassge.FROMNAME = DataMgr.m_account.userName;
        Target_WebSocketMassge.TONAME = Target_Goods.businessName;
        Target_WebSocketMassge.MESSAGETEXT = Target_Goods.id.ToString() ;
        Target_WebSocketMassge.MESSAGESTATUS = 2;
        Target_WebSocketMassge.MESSAGEDATE = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
        string message = JsonConvert.SerializeObject(Target_WebSocketMassge);
        m_WebSocket.Send(message);
        createGoodTmp(Target_Goods);
    }
    public void Init(Goods m_Goods)
    {
        if (Target_Goods != null)
        {
            if (Target_Goods.businessId != m_Goods.businessId)
            {
                if (ChatWindowsPar.transform.childCount != 0)
                {
                    for (int i = ChatWindowsPar.transform.childCount - 1; i >= 0; i--)
                    {
                        DestroyImmediate(ChatWindowsPar.transform.GetChild(i).gameObject);
                    }
                }
            }
        }
        if (Target_Goods != m_Goods)
        {
            Target_Goods = m_Goods;
            BusinessName.text = Target_Goods.businessName;
        }
        
        webSocketInit();
        m_WebSocket.Open();
    }
    void clickJumpToGood(GameObject obj)
    { 
        UIManager.Instance.PushPanel(UIPanelName.goodsdetailspanel, false, false, paragrm => { paragrm.GetComponent<goodsdetailspanel>().Init(Target_Goods); });
    }
}
