using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class UserInfoMap {
[ProtoMember(1)]
    public int infoType;
[ProtoMember(2)]
    public string info;

    public int getInfoType() {
        return infoType;
    }

    public void setInfoType(int infoType) {
        this.infoType = infoType;
    }

    public string getInfo() {
        return info;
    }

    public void setInfo(string info) {
        this.info = info;
    }

    public UserInfoMap() {
    }

    public UserInfoMap(short infoType, string info) {
        this.infoType = infoType;
        this.info = info;
    }
}
}