using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqGetExpressInfoMessage {
    //快递单号
[ProtoMember(1)]
    public string expressCode;

    public string getExpressCode() {
        return expressCode;
    }

    public void setExpressCode(string expressCode) {
        this.expressCode = expressCode;
    }
}
}