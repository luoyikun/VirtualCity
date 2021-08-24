using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspLeaveGroupMessage {
[ProtoMember(1)]
    public long groupId;
[ProtoMember(2)]
    public long accounId;
    public RspLeaveGroupMessage() {
    }

    public RspLeaveGroupMessage(long groupId, long accounId) {
        this.groupId = groupId;
        this.accounId = accounId;
    }

    public long getAccounId() {
        return accounId;
    }

    public void setAccounId(long accounId) {
        this.accounId = accounId;
    }

    public long getGroupId() {
        return groupId;
    }

    public void setGroupId(long groupId) {
        this.groupId = groupId;
    }
}


}