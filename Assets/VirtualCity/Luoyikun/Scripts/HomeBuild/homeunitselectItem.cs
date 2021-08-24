using ProtoDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class homeunitselectItem : MonoBehaviour {
    public Image m_imgIcon;
    public Text m_textNum;
    public PartProperties m_info;
    public HouseParts m_userPart;

    public void OnUpdate(HouseParts info)
    {
        m_userPart = info;
        m_info = DataMgr.m_dicPartProperties[(long)m_userPart.moudelId];

        m_textNum.text = m_userPart.num.ToString();

        AssetMgr.Instance.CreateSpr(m_info.iconName, "homeuniticon", (param) => { m_imgIcon.sprite = param; });
    }
}
