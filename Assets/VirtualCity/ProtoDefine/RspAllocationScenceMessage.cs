using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspAllocationScenceMessage  {
[ProtoMember(1)]
    public string serverIp;

    public RspAllocationScenceMessage() {
    }

    public RspAllocationScenceMessage(string serverIp) {
        this.serverIp = serverIp;
    }

    public string getServerIp() {
        return serverIp;
    }

    public void setServerIp(string serverIp) {
        this.serverIp = serverIp;
    }
}
}