using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspGetProxyMessage {
[ProtoMember(1)]
    public int code;
[ProtoMember(2)]
    public string tip;
[ProtoMember(3)]
    public string payInfo;

    public RspGetProxyMessage() {
    }

    public RspGetProxyMessage(int code, string tip, string payInfo) {
        this.code = code;
        this.tip = tip;
        this.payInfo = payInfo;
    }

    public string getPayInfo() {
        return payInfo;
    }

    public void setPayInfo(string payInfo) {
        this.payInfo = payInfo;
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