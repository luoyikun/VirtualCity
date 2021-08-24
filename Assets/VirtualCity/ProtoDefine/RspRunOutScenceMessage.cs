using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspRunOutScenceMessage {
[ProtoMember(1)]
    public long playerId;

    public RspRunOutScenceMessage() {
    }

    public RspRunOutScenceMessage(long playerId) {
        this.playerId = playerId;
    }

    public long getPlayerId() {
        return playerId;
    }

    public void setPlayerId(long playerId) {
        this.playerId = playerId;
    }
}
}