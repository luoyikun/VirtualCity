package com.kingston.jforgame.server.game.system.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.system.SystemDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SYSTEM,cmd = SystemDataPool.REQ_UPBYMESSAGE)
public class ReqUpdatePasswordByMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.BYTES)
    private byte[] content;
    @Protobuf(order = 2)
    private String pub_key;

    public byte[] getContent() {
        return content;
    }

    public void setContent(byte[] content) {
        this.content = content;
    }

    public String getPub_key() {
        return pub_key;
    }

    public void setPub_key(String pub_key) {
        this.pub_key = pub_key;
    }
}
