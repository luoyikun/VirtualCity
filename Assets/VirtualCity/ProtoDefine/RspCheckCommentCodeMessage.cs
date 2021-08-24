using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspCheckCommentCodeMessage {
[ProtoMember(1)]
    public int code;
[ProtoMember(2)]
    public string tip;
[ProtoMember(3)]
    public List<FatherProxyInfo> fathersId;

    public RspCheckCommentCodeMessage() {
    }

    public RspCheckCommentCodeMessage(int code, string tip, List<FatherProxyInfo> fathersId) {
        this.code = code;
        this.tip = tip;
        this.fathersId = fathersId;
    }

    public List<FatherProxyInfo> getFathersId() {
        return fathersId;
    }

    public void setFathersId(List<FatherProxyInfo> fathersId) {
        this.fathersId = fathersId;
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