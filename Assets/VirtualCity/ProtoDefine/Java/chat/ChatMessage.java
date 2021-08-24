package com.kingston.jforgame.server.game.sociality.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.sociality.SocialityDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SOCIALITY,cmd = SocialityDataPool.CHAT)
public class ChatMessage extends Message {
    /**消息发送者ID*/
    @Protobuf(order = 1)
    private long accountId;
    /**消息接受者id
     * 好友消息：好友Id
     * 群消息：群Id
     * 世界消息：null*/
    @Protobuf(order = 2)
    private Long Id;
    /**消息内容*/
    @Protobuf(order = 3)
    private String message;
    /**发送时间*/
    @Protobuf(order = 4)
    private String time;
    /**消息类型*/
    @Protobuf(order = 5)
    private int messageType;
    @Protobuf(order = 6)
    private String name;
    @Protobuf(order = 7)
    private  long modleId;
    /**功能类别*/
    @Protobuf(order = 8)
    private int messageFunc;

    public ChatMessage() {
    }

    public ChatMessage(long accountId, Long id, String message, String time, int messageType, String name, long modleId) {
        this.accountId = accountId;
        Id = id;
        this.message = message;
        this.time = time;
        this.messageType = messageType;
        this.name = name;
        this.modleId = modleId;
    }

    public String getName() {
        return name;
    }

    public long getModleId() {
        return modleId;
    }

    public long getAccountId() {
        return accountId;
    }

    public void setAccountId(long accountId) {
        this.accountId = accountId;
    }

    public Long getId() {
        return Id;
    }

    public void setId(Long id) {
        Id = id;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public String getTime() {
        return time;
    }

    public void setTime(String time) {
        this.time = time;
    }

    public int getMessageType() {
        return messageType;
    }

    public void setMessageType(int messageType) {
        this.messageType = messageType;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setModleId(long modleId) {
        this.modleId = modleId;
    }

    public int getMessageFunc() {
        return messageFunc;
    }

    public void setMessageFunc(int messageFunc) {
        this.messageFunc = messageFunc;
    }
}