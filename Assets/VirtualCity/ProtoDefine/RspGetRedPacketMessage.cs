using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspGetRedPacketMessage {
[ProtoMember(1)]
    public float amount;

    public RspGetRedPacketMessage() {
    }

    public RspGetRedPacketMessage(float amount) {
        this.amount = amount;
    }

    public float getAmount() {
        return amount;
    }

    public void setAmount(float amount) {
        this.amount = amount;
    }
}
}