using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopItem : MonoBehaviour {
    public int m_idx;
    public bool m_isSelect = false;
    public delegate void VoidDelegate(GameObject go);
    public VoidDelegate m_onSelect;
    public VoidDelegate m_onUnselect;
    // Use this for initialization
    void Start () {
      
    }

    

}
