using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspCreateGroupMessage {
[ProtoMember(1)]
    public ChatGroup group;

    public RspCreateGroupMessage() {
    }

    public RspCreateGroupMessage(ChatGroup group) {
        this.group = group;
    }

    public ChatGroup getGroup() {
        return group;
    }

    public void setGroup(ChatGroup group) {
        this.group = group;
    }
}
}