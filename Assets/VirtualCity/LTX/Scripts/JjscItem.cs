using ProtoDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JjscItem : MonoBehaviour {
    public PartProperties m_info;
    public Image m_icon;
    public Image Gold_img;
    public Sprite[] Gold_spr;
    public Text goid;
    public Text Cnname;
    public string modleData;
    public void UpdateInfo(PartProperties info)
    {
        m_info = info;
        AssetMgr.Instance.CreateSpr(info.iconName, "homeuniticon", (spr) => { m_icon.sprite = spr; });

        if (m_info.diamond != 0)
        {
            goid.text = m_info.diamond.ToString();
            Gold_img.sprite = Gold_spr[0];
        }
        if (m_info.gold != 0)
        {
            goid.text = m_info.gold.ToString();
            Gold_img.sprite = Gold_spr[1];
        }
        Cnname.text= m_info.cnName.ToString();
        modleData = m_info.modleData;
    }
}
