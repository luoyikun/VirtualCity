using Framework.Event;
using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class termpanel : UGUIPanel {
    public RectTransform m_rectContent;
    public GameObject m_btnAccept;

    private void Start()
    {
        ClickListener.Get(m_btnAccept).onClick = OnBtnAccept;
    }

    public override void OnOpen()
    {
        Vector3 pos = m_rectContent.localPosition;
        pos.y = 0;
        m_rectContent.localPosition = pos;
    }

    void OnBtnAccept(GameObject obj)
    {
        EventManager.Instance.DispatchEvent(Common.EventStr.OpenLoginPart);
        UIManager.Instance.PopSelf();
        
    }
}
