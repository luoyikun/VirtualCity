using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class ReqOpenAreaMessage {
[ProtoMember(1)]
    public string code;
    /**
     * 0:金币
     * 1:钻石
     */
[ProtoMember(2)]
    public int costDiamond;


    public ReqOpenAreaMessage() {
    }

    public string getCode() {
        return code;
    }

    public void setCode(string code) {
        this.code = code;
    }

    public int getCostDiamond() {
        return costDiamond;
    }

    public void setCostDiamond(int costDiamond) {
        this.costDiamond = costDiamond;
    }
}
}