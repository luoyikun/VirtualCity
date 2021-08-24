using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollCallback : MonoBehaviour {

    public delegate void Callback(int idx);
    public Callback callback;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ScrollCellIndex(int idx)
    {
        if (callback != null)
        {
            callback(idx);
        }
    }

}
