using System.Collections;
using System.Collections.Generic;
using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class hongbaopanel : UGUIPanel
{
    public GameObject QueRenBtn;
    public Text MoneyText;
    public override void OnOpen()
    {

    }

    public override void OnClose()
    {

    }

    void Start()
    {
        ClickListener.Get(QueRenBtn).onClick = clickQueRenBtn;
    }

    void clickQueRenBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf();
    }
    public void Init(float amount)
    {
        MoneyText.text = "<size=178>" + amount + "</size><size=53>元</size>";
  
    }
}
