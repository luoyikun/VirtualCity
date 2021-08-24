using Framework.UI;
using ProtoDefine;
using SGF.Codec;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class inputonepasswordpanel : UGUIPanel {

    public static inputonepasswordpanel instance;
    public Text Tips;
    public InputField password;
    public GameObject ResetBtn;
    public static int Type=0;
    // Use this for initialization

    private void Awake()
    {
        instance = this;
    }

    void Start () {
        Debug.Log("本地Rsa被删除");
        //  RSAEncryption.register();
        ClickListener.Get(ResetBtn).onClick = clickResetBtn;
    }
     
    public void star_cxfs()
    {
        Debug.Log(password.text);
        //UIManager.Instance.PushPanel(UIPanelName.uiloadpanel, false, false, null, true);
        uiloadpanel.Instance.Open();
        RSAEncryption.Resend(password.text);
    }

    public void clickResetBtn(GameObject obj)
    {
        passwordpanel.type = 1;
        if (Type==1) {
            UIManager.Instance.PushPanel(UIPanelName.passwordpanel, false, true, null, true);
        }
  
    }

    public override void OnOpen()
    {
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCommentMessage, Receive_data);
    }
    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspCommentMessage, Receive_data);
    }

    public void Receive_data(byte[] buf)
    {
        RSAEncryption.Receive_data(buf);
    }
}
