using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspCashFishMessage {
[ProtoMember(1)]
    public string type;
[ProtoMember(2)]
    public long number;

    public RspCashFishMessage() {
    }

    public RspCashFishMessage(string type, long number) {
        this.type = type;
        this.number = number;
    }

    public string getType() {
        return type;
    }

    public void setType(string type) {
        this.type = type;
    }

    public long getNumber() {
        return number;
    }

    public void setNumber(long number) {
        this.number = number;
    }
}
}