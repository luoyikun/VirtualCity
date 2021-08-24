using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]




public class RspRunInStreetMessage {
[ProtoMember(1)]
    public string scenceId;
[ProtoMember(2)]
    public Dictionary<long?, Player> players;
[ProtoMember(3)]
    public Dictionary<long?,PlayerStatus> playerStatus;

    public RspRunInStreetMessage() {
    }

    public RspRunInStreetMessage(string scenceId, Dictionary<long?, Player> players, Dictionary<long?, PlayerStatus> playerStatus) {
        this.scenceId = scenceId;
        this.players = players;
        this.playerStatus = playerStatus;
    }

    public Dictionary<long?, PlayerStatus> getPlayerStatus() {
        return playerStatus;
    }

    public void setPlayerStatus(Dictionary<long?, PlayerStatus> playerStatus) {
        this.playerStatus = playerStatus;
    }

    public Dictionary<long?, Player> getPlayers() {
        return players;
    }

    public void setPlayers(Dictionary<long?, Player> players) {
        this.players = players;
    }

    public string getScenceId() {
        return scenceId;
    }

    public void setScenceId(string scenceId) {
        this.scenceId = scenceId;
    }
}
}