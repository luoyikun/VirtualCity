using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspRunOutShopMessage {
[ProtoMember(1)]
    Dictionary<long?, PlayerStatus> playerStatusMap;

    public RspRunOutShopMessage() {
    }

    public RspRunOutShopMessage(Dictionary<long?, PlayerStatus> playerStatusMap) {
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