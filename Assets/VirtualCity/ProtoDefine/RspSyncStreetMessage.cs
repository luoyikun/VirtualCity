using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspSyncStreetMessage {
[ProtoMember(1)]
    public Dictionary<long?, PlayerStatus> playerStatusMap;

    public RspSyncStreetMessage() {
    }

    public RspSyncStreetMessage(Dictionary<long?, PlayerStatus> playerStatusMap) {
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