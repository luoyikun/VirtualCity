using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyLoopTest : MonoBehaviour {
    public GameObject m_btnTest;
    public GameObject m_btnDelete;
    public LoopScrollRect m_loop;
    public List<int> m_list = new List<int>();
    public static MyLoopTest Instance;
    // Use this for initialization
    void Start () {
        for (int i = 0; i < 20; i++)
        {
            m_list.Add(i);
        }
        Instance = this;
        ClickListener.Get(m_btnTest).onClick = OnTest;
        ClickListener.Get(m_btnDelete).onClick = OnDelete;
    }


    void OnTest(GameObject obj)
    {
        m_loop.totalCount = 20;
        m_loop.RefillCells();
    }

    void OnDelete(GameObject obj)
    {
        PublicFunc.RemoveFromChild(m_loop.content);
        m_loop.totalCount = 20;
        m_loop.RefillCells();
    }
}
