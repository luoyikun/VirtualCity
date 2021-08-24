using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqRemoveHouseMessage {
[ProtoMember(1)]
    public string houseId;

    public ReqRemoveHouseMessage() {
    }

    public ReqRemoveHouseMessage(string houseId) {
        this.houseId = houseId;
    }

    public string gethouseId() {
        return houseId;
    }

    public void sethouseId(string houseId) {
        this.houseId = houseId;
    }
}
}