using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;
using Newtonsoft.Json;

public class JieXi
{
    public string v;//Json表里的变量名为v，如果改掉会报错
}
public class cishanpanel : UGUIPanel {
    public GameObject BackBtn;
    public Text ChouKuanText;
    public Text RenCiText;

    public override void OnOpen()
    {
        init();
    }
    public override void OnClose()
    {

    }
    void init()
    {
        ClickListener.Get(BackBtn).onClick = clickBackBtn;
        JieXi m_JieXi= new JieXi();
        for (int i = 0; i < DataMgr.businessModelProperties.Count; i++)
        {
            switch (DataMgr.businessModelProperties[i].Name)
            {
                case "totalPlayer":
                    m_JieXi = JsonConvert.DeserializeObject<JieXi>(DataMgr.businessModelProperties[i].Con);
                    ChouKuanText.text = m_JieXi.v+"次";
                    break;
                case "charityMoney":
                    m_JieXi = JsonConvert.DeserializeObject<JieXi>(DataMgr.businessModelProperties[i].Con);
                    RenCiText.text = m_JieXi.v+"元";
                    break;
            }
        }
    }
    void clickBackBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf();
    }
}
