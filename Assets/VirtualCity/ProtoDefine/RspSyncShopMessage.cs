using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspSyncShopMessage {
[ProtoMember(1)]
    public Dictionary<long?, PlayerStatus> playerStatusMap;

    public RspSyncShopMessage() {
    }

    public RspSyncShopMessage(Dictionary<long?, PlayerStatus> playerStatusMap) {
        this.playerStatusMap = playerStatusMap;
    }

    public Dictionary<long?, PlayerStatus> getPlayerStatusMap() {
        return playerStatusMap;
    }

    public void setPlayerStatusMap(Dictionary<long?, PlayerStatus> playerStatusMap) {
        this.playerStatusMap = playerStatusMap;
    }
}

}