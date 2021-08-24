using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqConnectOtherServerMessage  {
[ProtoMember(1)]
    public string connectInfo;
[ProtoMember(2)]
    public string fromConnectInfo;

    public ReqConnectOtherServerMessage() {
    }

    public ReqConnectOtherServerMessage(string fromConnectInfo,string connectInfo) {
        this.connectInfo = connectInfo;
        this.fromConnectInfo = fromConnectInfo;
    }

    public string getFromConnectInfo() {
        return fromConnectInfo;
    }

    public void setFromConnectInfo(string fromConnectInfo) {
        this.fromConnectInfo = fromConnectInfo;
    }

    public string getConnectInfo() {
        return connectInfo;
    }

    public void setConnectInfo(string connectInfo) {
        this.connectInfo = connectInfo;
    }
}
}