package com.kingston.jforgame.server.game.system.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.system.SystemDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SYSTEM,cmd = SystemDataPool.REQ_PASSWORD)
public class ReqUpdatePasswordMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.BYTES)
    private byte[] oldPassword;
    @Protobuf(order = 2,fieldType = FieldType.BYTES)
    private byte[] newPassword;

    public byte[] getOldPassword() {
        return oldPassword;
    }

    public void setOldPassword(byte[] oldPassword) {
        this.oldPassword = oldPassword;
    }

    public byte[] getNewPassword() {
        return newPassword;
    }

    public void setNewPassword(byte[] newPassword) {
        this.newPassword = newPassword;
    }

}
