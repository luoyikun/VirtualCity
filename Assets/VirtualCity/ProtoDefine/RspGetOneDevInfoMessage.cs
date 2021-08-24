using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspGetOneDevInfoMessage {
[ProtoMember(1)]
    public Devlopments devlopment;

    public RspGetOneDevInfoMessage() {
    }

    public Devlopments getDevlopment() {
        return devlopment;
    }

    public void setDevlopment(Devlopments devlopment) {
        this.devlopment = devlopment;
    }

    public RspGetOneDevInfoMessage(Devlopments devlopment) {
        this.devlopment = devlopment;
    }
}
}