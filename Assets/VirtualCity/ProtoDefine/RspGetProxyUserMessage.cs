using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspGetProxyUserMessage {
[ProtoMember(1)]
    public List<ProxyUser> proxyUsers;

    public RspGetProxyUserMessage() {
    }

    public RspGetProxyUserMessage(List<ProxyUser> proxyUsers) {
        this.proxyUsers = proxyUsers;
    }

    public List<ProxyUser> getProxyUsers() {
        return proxyUsers;
    }

    public void setProxyUsers(List<ProxyUser> proxyUsers) {
        this.proxyUsers = proxyUsers;
    }
}
}