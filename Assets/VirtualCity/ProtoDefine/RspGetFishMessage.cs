using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspGetFishMessage {
[ProtoMember(1)]
    public long fishId;

    public long getFishId() {
        return fishId;
    }

    public void setFishId(long fishId) {
        this.fishId = fishId;
    }

    public RspGetFishMessage() {
    }

    public RspGetFishMessage(long fishId) {
        this.fishId = fishId;
    }
}
}