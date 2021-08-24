package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.server.game.building.entity.PlayerStatus;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.REQ_GETHOUSEINFO)
public class ReqSyncHouseMessage extends Message {
    @Protobuf(order = 1)
    private Long friendId;
    @Protobuf(order = 2)
    private String houseId;
    @Protobuf(order = 3,fieldType = FieldType.OBJECT)
    private PlayerStatus playerStatus;

    public ReqSyncHouseMessage() {
    }

    public ReqSyncHouseMessage(Long friendId, String houseId, PlayerStatus playerStatus) {
        this.friendId = friendId;
        this.houseId = houseId;
        this.playerStatus = playerStatus;
    }

    public Long getFriendId() {
        return friendId;
    }

    public void setFriendId(Long friendId) {
        this.friendId = friendId;
    }

    public String getHouseId() {
        return houseId;
    }

    public void setHouseId(String houseId) {
        this.houseId = houseId;
    }

    public PlayerStatus getPlayerStatus() {
        return playerStatus;
    }

    public void setPlayerStatus(PlayerStatus playerStatus) {
        this.playerStatus = playerStatus;
    }
}
