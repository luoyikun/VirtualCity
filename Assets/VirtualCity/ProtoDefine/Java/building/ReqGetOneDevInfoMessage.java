package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.REQ_GETONEDEVINFO)
public class ReqGetOneDevInfoMessage extends Message {
    /**玩家id*/
    @Protobuf(order = 1)
    private Long friendId;
    /**建筑id*/
    @Protobuf(order = 2)
    private String devId;

    public ReqGetOneDevInfoMessage() {
    }

    public ReqGetOneDevInfoMessage(Long friendId, String devId) {
        this.friendId = friendId;
        this.devId = devId;
    }

    public Long getFriendId() {
        return friendId;
    }

    public void setFriendId(Long friendId) {
        this.friendId = friendId;
    }

    public String getDevId() {
        return devId;
    }

    public void setDevId(String devId) {
        this.devId = devId;
    }
}
