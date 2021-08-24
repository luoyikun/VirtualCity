using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspJoinGroupMessage {
[ProtoMember(1)]
    public ChatGroup group;
[ProtoMember(2)]
    public int code;
[ProtoMember(3)]
    public string tips;

    public RspJoinGroupMessage() {
    }

    public RspJoinGroupMessage(ChatGroup group, int code, string tips) {
        this.group = group;
        this.code = code;
        this.tips = tips;
    }

    public ChatGroup getGroup() {
        return group;
    }

    public void setGroup(ChatGroup group) {
        this.group = group;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public string getTips() {
        return tips;
    }

    public void setTips(string tips) {
        this.tips = tips;
    }
}
}