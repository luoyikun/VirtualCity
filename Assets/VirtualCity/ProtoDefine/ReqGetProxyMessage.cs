using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqGetProxyMessage {
    //推荐码
[ProtoMember(1)]
    public string code;
    //0:支付宝
    //1:微信
[ProtoMember(2)]
    public int payType;

    public int getPayType() {
        return payType;
    }

    public void setPayType(int payType) {
        this.payType = payType;
    }

    public ReqGetProxyMessage() {
    }

    public string getCode() {
        return code;
    }

    public void setCode(string code) {
        this.code = code;
    }

    public ReqGetProxyMessage(string code) {
        this.code = code;
    }
}
}