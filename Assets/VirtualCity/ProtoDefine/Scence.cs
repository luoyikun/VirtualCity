using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public  class Scence {
[ProtoMember(1)]
    public Dictionary<long?, PlayerStatus> playerStatuses;
[ProtoMember(2)]
    public string scenceId;

    public Dictionary<long?, PlayerStatus> getPlayerStatuses() {
        return playerStatuses;
    }

    public void setPlayerStatuses(Dictionary<long?, PlayerStatus> playerStatuses) {
        this.playerStatuses = playerStatuses;
    }

    public string getScenceId() {
        return scenceId;
    }

    public void setScenceId(string scenceId) {
        this.scenceId = scenceId;
    }
}
}