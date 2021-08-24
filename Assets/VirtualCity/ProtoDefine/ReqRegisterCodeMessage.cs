using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqRegisterCodeMessage {
[ProtoMember(1)]
    public string code;

    public string getCode() {
        return code;
    }

    public void setCode(string code) {
        this.code = code;
    }
}
}