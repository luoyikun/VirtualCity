using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspCreateCommentsMessage {
[ProtoMember(1)]
    public int code;
[ProtoMember(2)]
    public string tips;

    public RspCreateCommentsMessage() {
    }

    public RspCreateCommentsMessage(int code, string tips) {
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