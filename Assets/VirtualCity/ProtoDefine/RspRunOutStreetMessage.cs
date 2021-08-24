using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspRunOutStreetMessage {
[ProtoMember(1)]
    public int code;
[ProtoMember(2)]
    public string tip;

    public RspRunOutStreetMessage() {
    }

    public RspRunOutStreetMessage(int code, string tip) {
        this.code = code;
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