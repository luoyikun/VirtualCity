using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqCheckCommendCodeMessage {
[ProtoMember(1)]
    public string code;

    public ReqCheckCommendCodeMessage() {
    }

    public string getCode() {
        return code;
    }

    public void setCode(string code) {
        this.code = code;
    }

    public ReqCheckCommendCodeMessage(string code) {
        this.code = code;
    }
}
}