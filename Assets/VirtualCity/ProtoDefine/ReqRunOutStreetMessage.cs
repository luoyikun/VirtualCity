using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqRunOutStreetMessage {

[ProtoMember(1)]
    public string scenceId;
[ProtoMember(2)]
    public long playerId;

    public string getScenceId() {
        return scenceId;
    }

    public void setScenceId(string scenceId) {
        this.scenceId = scenceId;
    }

    public long getPlayerId() {
        return playerId;
    }

    public void setPlayerId(long playerId) {
        this.playerId = playerId;
    }
}
}