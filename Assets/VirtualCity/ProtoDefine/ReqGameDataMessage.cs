using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqGameDataMessage {
[ProtoMember(1)]
    public string clientVersion;

    public string getVersion() {
        return clientVersion;
    }

    public void setVersion(string version) {
        this.clientVersion = version;
    }
}
}