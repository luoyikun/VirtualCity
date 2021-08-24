using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspUpdateHousePartMessage {
[ProtoMember(1)]
    public int code;
[ProtoMember(2)]
    public string tip;

    public RspUpdateHousePartMessage(int code, string tip) {
        this.code = code;
        this.tip = tip;
    }

    public RspUpdateHousePartMessage() {
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