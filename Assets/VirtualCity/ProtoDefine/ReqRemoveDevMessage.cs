using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqRemoveDevMessage {
[ProtoMember(1)]
    public string devId;

    public ReqRemoveDevMessage() {
    }

    public ReqRemoveDevMessage(string devId) {
        this.devId = devId;
    }

    public string getDevId() {
        return devId;
    }

    public void setDevId(string devId) {
        this.devId = devId;
    }
}
}