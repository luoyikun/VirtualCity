package com.kingston.jforgame.server.game.system.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.system.SystemDataPool;
import com.kingston.jforgame.server.game.system.entity.FatherProxyInfo;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;

@MessageMeta(module = Modules.SYSTEM,cmd = SystemDataPool.RSP_CKCODE)
public class RspCheckCommentCodeMessage extends Message {
    @Protobuf(order = 1)
    private int code;
    @Protobuf(order = 2)
    private String tip;
    @Protobuf(order = 3,fieldType = FieldType.INT64)
    private List<FatherProxyInfo> fathersId;

    public RspCheckCommentCodeMessage() {
    }

    public RspCheckCommentCodeMessage(int code, String tip, List<FatherProxyInfo> fathersId) {
        this.code = code;
        this.tip = tip;
        this.fathersId = fathersId;
    }

    public List<FatherProxyInfo> getFathersId() {
        return fathersId;
    }

    public void setFathersId(List<FatherProxyInfo> fathersId) {
        this.fathersId = fathersId;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public String getTip() {
        return tip;
    }

    public void setTip(String tip) {
        this.tip = tip;
    }


}