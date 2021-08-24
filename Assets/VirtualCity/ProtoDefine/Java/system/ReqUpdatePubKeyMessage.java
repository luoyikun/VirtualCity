package com.kingston.jforgame.server.game.system.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.system.SystemDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SYSTEM,cmd = SystemDataPool.REQ_UPDATEPUBKEY)
public class ReqUpdatePubKeyMessage extends Message {
    //密文
    @Protobuf(order = 1,fieldType = FieldType.BYTES)
    private byte[] content;
    //公钥
    @Protobuf(order = 2)
    private String pub_key;
    /*
    是否新注册需要创建密钥
    1,是
    0,否
     */
    @Protobuf(order = 3)
    private int isCreate;

    public ReqUpdatePubKeyMessage() {
    }


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

    public int getIsCreate() {
        return isCreate;
    }

    public void setIsCreate(int isCreate) {
        this.isCreate = isCreate;
    }
}
