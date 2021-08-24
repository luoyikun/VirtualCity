using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspRunInScenceMessage {
[ProtoMember(1)]
    public Player player;
[ProtoMember(2)]
    public PlayerStatus playerStatus;

    public RspRunInScenceMessage() {
    }

    public RspRunInScenceMessage(Player player, PlayerStatus playerStatus) {
        this.player = player;
        this.playerStatus = playerStatus;
    }

    public Player getPlayer() {
        return player;
    }

    public void setPlayer(Player player) {
        this.player = player;
    }

    public PlayerStatus getPlayerStatus() {
        return playerStatus;
    }

    public void setPlayerStatus(PlayerStatus playerStatus) {
        this.playerStatus = playerStatus;
    }
}
}