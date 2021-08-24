using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspComfirmReceiptMessage {
[ProtoMember(1)]
    public int code;
[ProtoMember(2)]
    public string tips;

    public RspComfirmReceiptMessage() {
    }

    public RspComfirmReceiptMessage(int code, string tips) {
        this.code = code;
        this.tips = tips;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public string getTips() {
        return tips;
    }

    public void setTips(string tips) {
        this.tips = tips;
    }

}
}