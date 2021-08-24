using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftScroll : MonoBehaviour {
    public LoopVerticalScrollRect m_loop;
    public List<string> m_listName = new List<string>();
    public int m_selectIdx = 0;
    // Use this for initialization
    void Start () {
        for (int i = 0; i < 20; i++)
            m_listName.Add(i.ToString());

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void Create(int cnt)
    {
        m_loop.totalCount = cnt;
        m_loop.RefillCells();
    }

    public void SetSelect(int idx)
    {
        m_selectIdx = idx;
        for (int i = 0; i < m_loop.content.transform.childCount; i++)
        {
            Transform child = m_loop.content.transform.GetChild(i);
            if (child.GetComponent<LeftScrollItem>().m_idx == m_selectIdx)
            {
                child.GetComponent<LeftScrollItem>().OnSelect();
            }
            else
            {
                child.GetComponent<LeftScrollItem>().OnUnSelect();
            }
        }
    }
}
