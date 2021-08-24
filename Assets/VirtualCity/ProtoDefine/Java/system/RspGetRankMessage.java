package com.kingston.jforgame.server.game.system.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.system.SystemDataPool;
import com.kingston.jforgame.server.game.system.entity.RankInfo;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;

@MessageMeta(module = Modules.SYSTEM,cmd = SystemDataPool.RSP_GETRANK)
public class RspGetRankMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private List<RankInfo> list;

    public RspGetRankMessage() {
    }

    public RspGetRankMessage(List<RankInfo> list) {
        this.list = list;
    }

    public List<RankInfo> getList() {
        return list;
    }

    public void setList(List<RankInfo> list) {
        this.list = list;
    }
}
