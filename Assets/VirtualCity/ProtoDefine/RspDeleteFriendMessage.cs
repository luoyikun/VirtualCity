using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspDeleteFriendMessage {
[ProtoMember(1)]
    public long accountId;
[ProtoMember(2)]
    public long fromAccountId;


    public RspDeleteFriendMessage() {
    }

    public RspDeleteFriendMessage(long accountId, long fromAccountId) {
        this.accountId = accountId;
        this.fromAccountId = fromAccountId;
    }

    public long getAccountId() {
        return accountId;
    }

    public void setAccountId(long accountId) {
        this.accountId = accountId;
    }

    public long getFromAccountId() {
        return fromAccountId;
    }

    public void setFromAccountId(long fromAccountId) {
        this.fromAccountId = fromAccountId;
    }
}
}