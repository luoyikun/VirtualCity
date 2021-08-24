package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.REQ_GETHOMETONE)
public class ReqGetHometoneInfoMessage extends Message {
    /**玩家id*/
    @Protobuf(order = 1)
    private Long friendId;

    public ReqGetHometoneInfoMessage() {
    }

    public ReqGetHometoneInfoMessage(Long friendId) {
        this.friendId = friendId;
    }

    public Long getFriendId() {
        return friendId;
    }

    public void setFriendId(Long friendId) {
        this.friendId = friendId;
    }
}
