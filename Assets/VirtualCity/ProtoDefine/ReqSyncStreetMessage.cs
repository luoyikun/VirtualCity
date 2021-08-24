using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqSyncStreetMessage {

[ProtoMember(1)]
    public PlayerStatus playerStatus;
[ProtoMember(2)]
    public string scenceId;

    public PlayerStatus getPlayerStatus() {
        return playerStatus;
    }

    public void setPlayerStatus(PlayerStatus playerStatus) {
        this.playerStatus = playerStatus;
    }

    public string getScenceId() {
        return scenceId;
    }

    public void setScenceId(string scenceId) {
        this.scenceId = scenceId;
    }

}
}