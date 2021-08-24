using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ChatMessage {
    /**消息发送者ID*/
[ProtoMember(1)]
    public long accountId;
    /**消息接受者id
     * 好友消息：好友Id
     * 群消息：群Id
     * 世界消息：null*/
[ProtoMember(2)]
    public long? Id;
    /**消息内容*/
[ProtoMember(3)]
    public string message;
    /**发送时间*/
[ProtoMember(4)]
    public string time;
    /**消息类型*/
[ProtoMember(5)]
    public int messageType;
[ProtoMember(6)]
    public string name;
[ProtoMember(7)]
    public  long modleId;
    /**功能类别*/
[ProtoMember(8)]
    public int messageFunc;

    public ChatMessage() {
    }

    public ChatMessage(long accountId, long? id, string message, string time, int messageType, string name, long modleId) {
        this.accountId = accountId;
        Id = id;
        this.message = message;
        this.time = time;
        this.messageType = messageType;
        this.name = name;
        this.modleId = modleId;
    }

    public string getName() {
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

    public long? getId() {
        return Id;
    }

    public void setId(long? id) {
        Id = id;
    }

    public string getMessage() {
        return message;
    }

    public void setMessage(string message) {
        this.message = message;
    }

    public string getTime() {
        return time;
    }

    public void setTime(string time) {
        this.time = time;
    }

    public int getMessageType() {
        return messageType;
    }

    public void setMessageType(int messageType) {
        this.messageType = messageType;
    }

    public void setName(string name) {
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
}