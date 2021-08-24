using UnityEngine;
using System.Collections;
using Net;
using ProtoBuf;
using SGF.Codec;
using SGF.Network.Core;

[ProtoContract]
public class TestProto
{
    [ProtoMember(1)]
    public long accountId;
    [ProtoMember(2)]
    public string password;
}

[ProtoContract]
public class LoginRspxx
{
    [ProtoMember(1)]
    public int code;
    [ProtoMember(2)]
    public string tips;
}
public class Main : MonoBehaviour 
{
    TestProto test = new TestProto();
    NetMessage msg = new NetMessage();
    void Start () 
    {
        InitNet();

        //        ChatView.OpenView("Prefabs/ChatView/ChatView");

        test.accountId = 123;
        test.password = "123";
        byte[] buf = PBSerializer.NSerialize(test);

        TestProto pro1 = PBSerializer.NDeserialize<TestProto>(buf);


        msg.head.moduleId = 101;
        msg.head.cmd = 1;
        msg.content = buf;
        msg.head.packetLength = 2 + 2 + buf.Length;

        //整个NetMessage 转byte
        byte[] tmp = null;
        int len = msg.Serialize(out tmp);


        byte[] tmp2 = null;
        NetMessage msg1 = new NetMessage();
        msg1.Deserialize(tmp, len);

        TestProto pro = PBSerializer.NDeserialize<TestProto>(msg1.content);
        int i1 = 0;


        Debug.Log(System.Text.Encoding.Default.EncodingName);

        //for (int i = 0; i < 100; i++)
        NetManager.Instance.SendMsg(msg);
    }

    private void InitNet()
    {
        gameObject.AddComponent<NetManager>();
        //NetManager.Instance.SendConnect();
    }

    
    public void Send()
    {
        NetManager.Instance.SendMsg(msg);
    }
}
