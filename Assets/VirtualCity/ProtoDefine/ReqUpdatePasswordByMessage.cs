using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqUpdatePasswordByMessage {
[ProtoMember(1)]
    public byte[] content;
[ProtoMember(2)]
    public string pub_key;

    public byte[] getContent() {
        return content;
    }

    public void setContent(byte[] content) {
        this.content = content;
    }

    public string getPub_key() {
        return pub_key;
    }

    public void setPub_key(string pub_key) {
        this.pub_key = pub_key;
    }
}
}