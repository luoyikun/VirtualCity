package com.kingston.jforgame.server.game.system.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.system.SystemDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;

@MessageMeta(module = Modules.SYSTEM,cmd = SystemDataPool.REQ_GETRANK)
public class ReqGetRankMessage extends Message {
    //排行榜类型
    @Protobuf(order = 1)
    private int rankType;
    //排行对象（全服或好友）
    @Protobuf(order = 2)
    private int relationType;
    //好友ID列表 如果是查询全服数据传null
    @Protobuf(order = 3,fieldType = FieldType.INT64)
    private List<Long> friendIds;

    public List<Long> getFriendIds() {
        return friendIds;
    }

    public void setFriendIds(List<Long> friendIds) {
        this.friendIds = friendIds;
    }

    public int getRelationType() {
        return relationType;
    }

    public void setRelationType(int relationType) {
        this.relationType = relationType;
    }

    public int getRankType() {
        return rankType;
    }

    public void setRankType(int rankType) {
        this.rankType = rankType;
    }
}
