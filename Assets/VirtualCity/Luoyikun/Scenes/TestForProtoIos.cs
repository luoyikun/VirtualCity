using ProtoDefine;
using SGF.Codec;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForProtoIos : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ReqLoginMessage req = new ReqLoginMessage();
        Debug.Log("初始化成功");
        req.accountId = 123;
        req.phone = "177";

        byte[] buf = PBSerializer.NSerialize(req);
        Debug.Log("序列化成功");
        ReqLoginMessage reqJieYa = PBSerializer.NDeserialize<ReqLoginMessage>(buf);
        Debug.Log(reqJieYa.phone);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
