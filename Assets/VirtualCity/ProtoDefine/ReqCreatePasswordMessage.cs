using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqCreatePasswordMessage {
[ProtoMember(1)]
    public byte[] newPasswordBytes;
[ProtoMember(2)]
    public string pubKey;

    public byte[] getNewPasswordBytes() {
        return newPasswordBytes;
    }

    public void setNewPasswordBytes(byte[] newPasswordBytes) {
        this.newPasswordBytes = newPasswordBytes;
    }

    public string getPubKey() {
        return pubKey;
    }

    public void setPubKey(string pubKey) {
        this.pubKey = pubKey;
    }
}
}