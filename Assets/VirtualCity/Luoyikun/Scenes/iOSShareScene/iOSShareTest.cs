using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iOSShareTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GJCNativeShare.Instance.onShareSuccess = OnShareSuccess;
        GJCNativeShare.Instance.onShareCancel = OnShareCancel;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnShareSuccess(string platform)
    {
        Debug.Log("ios分享成功");
    }
    void OnShareCancel(string platform)
    {
        Debug.Log("ios分享取消");
    }

    public void TestShare()
    {
        GJCNativeShare.Instance.NativeShare("分享自ios");
    }
}
