package com.kingston.jforgame.server.game.building.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.building.BuildingDataPool;
import com.kingston.jforgame.server.game.building.entity.Devlopments;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.BULIDING,cmd = BuildingDataPool.RSP_GETONEDEVINFO)
public class RspGetOneDevInfoMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private Devlopments devlopment;

    public RspGetOneDevInfoMessage() {
    }

    public Devlopments getDevlopment() {
        return devlopment;
    }

    public void setDevlopment(Devlopments devlopment) {
        this.devlopment = devlopment;
    }

    public RspGetOneDevInfoMessage(Devlopments devlopment) {
        this.devlopment = devlopment;
    }
}
