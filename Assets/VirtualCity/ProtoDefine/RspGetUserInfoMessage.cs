using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspGetUserInfoMessage {
[ProtoMember(1)]
    public Dictionary<long?, Player> players;

    public RspGetUserInfoMessage() {
    }

    public RspGetUserInfoMessage(Dictionary<long?, Player> players) {
        this.players = players;
    }

    public Dictionary<long?, Player> getPlayers() {
        return players;
    }

    public void setPlayers(Dictionary<long?, Player> players) {
        this.players = players;
    }
}
}