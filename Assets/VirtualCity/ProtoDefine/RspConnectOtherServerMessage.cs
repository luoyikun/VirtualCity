using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspConnectOtherServerMessage {
[ProtoMember(1)]
    public int code;
[ProtoMember(2)]
    public string serverName;


    public RspConnectOtherServerMessage() {
    }


    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public RspConnectOtherServerMessage(int code, string serverName) {
        this.code = code;
        this.serverName = serverName;
    }

    public string getServerName() {
        return serverName;
    }

    public void setServerName(string serverName) {
        this.serverName = serverName;
    }
}
}