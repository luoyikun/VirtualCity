using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqUpdatePasswordMessage {
[ProtoMember(1)]
    public byte[] oldPassword;
[ProtoMember(2)]
    public byte[] newPassword;

    public byte[] getOldPassword() {
        return oldPassword;
    }

    public void setOldPassword(byte[] oldPassword) {
        this.oldPassword = oldPassword;
    }

    public byte[] getNewPassword() {
        return newPassword;
    }

    public void setNewPassword(byte[] newPassword) {
        this.newPassword = newPassword;
    }

}
}