using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqZanHouseMessage {
[ProtoMember(1)]
    public long playerId;
[ProtoMember(2)]
    public string houseId;

    public long getPlayerId() {
        return playerId;
    }

    public void setPlayerId(long playerId) {
        this.playerId = playerId;
    }

    public string getHouseId() {
        return houseId;
    }

    public void setHouseId(string houseId) {
        this.houseId = houseId;
    }
}
}