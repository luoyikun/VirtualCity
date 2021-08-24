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

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.RSP_SYNCSHOP)
public class RspSyncShopMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.MAP)
    private Map<Long, PlayerStatus> playerStatusMap;

    public RspSyncShopMessage() {
    }

    public RspSyncShopMessage(Map<Long, PlayerStatus> playerStatusMap) {
        this.playerStatusMap = playerStatusMap;
    }

    public Map<Long, PlayerStatus> getPlayerStatusMap() {
        return playerStatusMap;
    }

    public void setPlayerStatusMap(Map<Long, PlayerStatus> playerStatusMap) {
        this.playerStatusMap = playerStatusMap;
    }
}

