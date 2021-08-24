package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.server.game.building.entity.PlayerStatus;
import com.kingston.jforgame.server.game.login.entity.Player;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.Map;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.RSP_RUNINSHOP)
public class RspRunInShopMessage extends Message {
    @Protobuf(order = 1)
    private String scenceId;
    @Protobuf(order = 2,fieldType = FieldType.MAP)
    private Map<Long, Player> players;
    @Protobuf(order = 3,fieldType = FieldType.MAP)
    private Map<Long,PlayerStatus> playerStatus;
    public RspRunInShopMessage() {
    }

    public RspRunInShopMessage(String scenceId, Map<Long, Player> players, Map<Long, PlayerStatus> playerStatus) {
        this.scenceId = scenceId;
        this.players = players;
        this.playerStatus = playerStatus;
    }

    public Map<Long, PlayerStatus> getPlayerStatus() {
        return playerStatus;
    }

    public void setPlayerStatus(Map<Long, PlayerStatus> playerStatus) {
        this.playerStatus = playerStatus;
    }

    public String getScenceId() {
        return scenceId;
    }

    public void setScenceId(String scenceId) {
        this.scenceId = scenceId;
    }

    public Map<Long, Player> getPlayers() {
        return players;
    }

    public void setPlayers(Map<Long, Player> players) {
        this.players = players;
    }


}
