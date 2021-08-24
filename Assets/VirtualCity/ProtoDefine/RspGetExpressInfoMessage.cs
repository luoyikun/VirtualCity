using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspGetExpressInfoMessage {
    //快递信息
[ProtoMember(1)]
    public ExpressInfo expressInfo;

[ProtoMember(2)]
    public int code;

[ProtoMember(3)]
    public string tip;

    public RspGetExpressInfoMessage() {
    }

    public RspGetExpressInfoMessage(ExpressInfo expressInfo, int code, string tip) {
        this.expressInfo = expressInfo;
        this.code = code;
        this.tip = tip;
    }

    public ExpressInfo getExpressInfo() {
        return expressInfo;
    }

    public void setExpressInfo(ExpressInfo expressInfo) {
        this.expressInfo = expressInfo;
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