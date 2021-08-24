using Newtonsoft.Json;
using ProtoDefine;
using SGF.Codec;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForTest : MonoBehaviour {

	// Use this for initialization
	void Start () {

        ReqSyncStreetMessage req = new ReqSyncStreetMessage();
        req.scenceId = "123";
        PlayerStatus playerSta = new PlayerStatus();
        playerSta.x = 1;
        playerSta.y = 1;
        playerSta.z = 1;
        playerSta.px = 2;
        playerSta.py = 2;
        playerSta.pz = 2;
        playerSta.acation = 0;
        req.playerStatus = playerSta;

        string json = JsonConvert.SerializeObject(req);
        Debug.Log(json);
        //RspSyncStreetMessage request = new RspSyncStreetMessage();
        //request.playerStatusMap = new Dictionary<long?, PlayerStatus>();
        //for (int i = 0; i < 30; i++)
        //{
        //    PlayerStatus playerStatus = new PlayerStatus();
        //    playerStatus.setAcation(1111 + i);
        //    playerStatus.setPx(1231.124f + i);
        //    playerStatus.setPy(1234.123f + i);
        //    playerStatus.setPz(12312.123f + i);
        //    playerStatus.setX(1234.123f + i);
        //    playerStatus.setY(1234.123f + i);
        //    playerStatus.setZ(1234.124f + i);
        //    //playerStatusMap.put(12315154L + i, playerStatus);
        //    request.playerStatusMap[12315154L + i] = playerStatus;
        //}


        //ReqHeartBeatMessage req = new ReqHeartBeatMessage();
        //req.accountId = 123456789;

        byte[] buf = PBSerializer.NSerialize(req);
        Debug.Log("buf:Lenght:" + buf.Length + ":--" + ByteToHexStr(buf));
        //byte[] afterYashuo = UnityGZip.Compress(buf);
        int len;
        //float Time.time;
        byte[] afterYashuo = UnityGZip.Compress(buf);
        Debug.Log("afterYashuo:lenght:" + afterYashuo.Length + ":---" + ByteToHexStr(afterYashuo));
        //byte[] jieya = UnityGZip.GZip(afterYashuo);

        byte[] jieya = UnityGZip.DeCompress(afterYashuo);

        Debug.Log("jieya:lenght:" + jieya.Length + ":---" + ByteToHexStr(jieya));
        ReqSyncStreetMessage reqJieYa = PBSerializer.NDeserialize<ReqSyncStreetMessage>(jieya);
        ////int i = 0;
        Debug.Log("1234");
    }


    public static string ByteToHexStr(byte[] bytes)
    {
        string returnStr = "";
        if (bytes != null)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                returnStr += bytes[i].ToString("x2");
            }
        }
        return returnStr;
    }


    // Update is called once per frame
    void Update () {
        //Debug.Log(gameObject.GetInstanceID());
	}


    public void WxShare()
    {
    
        string url = "http://47.106.74.189:8080/game/?code=123";
        //AndroidFunc.WxShareWebpage(url, "模拟家园：S界","一起来建造家园，购物吧","");
    }

    public void GetWxRsp(string text)
    {
        Debug.Log(text);
    }
}
