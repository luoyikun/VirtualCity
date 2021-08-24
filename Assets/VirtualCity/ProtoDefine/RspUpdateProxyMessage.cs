using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspUpdateProxyMessage {
[ProtoMember(1)]
    public ProxyUser proxyUser;
[ProtoMember(2)]
    public int proxyLeve;

    public RspUpdateProxyMessage() {
    }

    public RspUpdateProxyMessage(ProxyUser proxyUser, int proxyLeve) {
        this.proxyUser = proxyUser;
        this.proxyLeve = proxyLeve;
    }

    public int getProxyLeve() {
        return proxyLeve;
    }

    public void setProxyLeve(int proxyLeve) {
        this.proxyLeve = proxyLeve;
    }

    public ProxyUser getProxyUser() {
        return proxyUser;
    }

    public void setProxyUser(ProxyUser proxyUser) {
        this.proxyUser = proxyUser;
    }
}
}