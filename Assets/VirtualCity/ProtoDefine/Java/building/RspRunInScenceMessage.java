package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.server.game.building.entity.PlayerStatus;
import com.kingston.jforgame.server.game.login.entity.Player;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.RSP_RUNINPLAYER)
public class RspRunInScenceMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private Player player;
    @Protobuf(order = 2,fieldType = FieldType.OBJECT)
    private PlayerStatus playerStatus;

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
