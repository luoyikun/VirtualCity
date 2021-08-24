using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqUpdatePubKeyMessage {
    //密文
[ProtoMember(1)]
    public byte[] content;
    //公钥
[ProtoMember(2)]
    public string pub_key;
    /*
    是否新注册需要创建密钥
    1,是
    0,否
     */
[ProtoMember(3)]
    public int isCreate;

    public ReqUpdatePubKeyMessage() {
    }


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

    public int getIsCreate() {
        return isCreate;
    }

    public void setIsCreate(int isCreate) {
        this.isCreate = isCreate;
    }
}
}