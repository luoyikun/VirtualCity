using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspSendMessage {
[ProtoMember(1)]
    public int code = SocialityDataPool.FAIL;
[ProtoMember(2)]
    public string tip;

    public RspSendMessage() {
    }

    public RspSendMessage(string tip) {
        this.tip = tip;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public string getTip() {
        return tip;
    }

    public void setTip(string tip) {
        this.tip = tip;
    }
}
}