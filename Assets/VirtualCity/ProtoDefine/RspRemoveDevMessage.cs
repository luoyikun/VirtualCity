using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspRemoveDevMessage {
[ProtoMember(1)]
    Devlopments devlopments;

    public RspRemoveDevMessage() {
    }

    public RspRemoveDevMessage(Devlopments devlopments) {
        this.devlopments = devlopments;
    }

    public Devlopments getDevlopments() {
        return devlopments;
    }

    public void setDevlopments(Devlopments devlopments) {
        this.devlopments = devlopments;
    }
}
}