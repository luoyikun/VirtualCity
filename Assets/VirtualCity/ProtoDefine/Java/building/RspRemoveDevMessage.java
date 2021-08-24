package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.server.game.building.entity.Devlopments;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.RSP_REMOVEDEVLOPMENT)
public class RspRemoveDevMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    Devlopments devlopments;

    public RspRemoveDevMessage() {
    }

    public RspRemoveDevMessage(Devlopments devlopments) {
        this.devlopments = devlopments;
    }

    public Devlopments getDevlopments() {
        return devlopments;
    }

    public void setDevlopments(Devlopments devlopments) {
        this.devlopments = devlopments;
    }
}
