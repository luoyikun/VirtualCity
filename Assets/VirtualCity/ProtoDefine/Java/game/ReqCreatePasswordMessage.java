package com.kingston.jforgame.server.game.system.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.system.SystemDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SYSTEM,cmd = SystemDataPool.REQ_CREATEPASSWORD)
public class ReqCreatePasswordMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.BYTES)
    private byte[] newPasswordBytes;
    @Protobuf(order = 2)
    private String pubKey;

    public byte[] getNewPasswordBytes() {
        return newPasswordBytes;
    }

    public void setNewPasswordBytes(byte[] newPasswordBytes) {
        this.newPasswordBytes = newPasswordBytes;
    }

    public String getPubKey() {
        return pubKey;
    }

    public void setPubKey(String pubKey) {
        this.pubKey = pubKey;
    }
}
