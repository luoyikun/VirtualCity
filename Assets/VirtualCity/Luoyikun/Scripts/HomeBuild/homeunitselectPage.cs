using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class homeunitselectPage : MonoBehaviour {
    public GameObject m_selectObj;
    public Text m_text;
    public int m_idx;
    public void UiUpdate(int idx,int selectIdx,string text)
    {
        m_idx = idx;
        if (selectIdx == idx)
        {
            m_selectObj.SetActive(true);
        }
        else {
            m_selectObj.SetActive(false);
        }
        m_text.text = text;
    }

    public void SetSelect(int selectIdx)
    {
        if (m_idx == selectIdx)
        {
            m_selectObj.SetActive(true);
        }
        else {
            m_selectObj.SetActive(false);
        }
    }
}
