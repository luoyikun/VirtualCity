using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class HouseParts {
    /**模型ID*/
[ProtoMember(1)]
    public long? moudelId = 1;

    /**数量*/
[ProtoMember(2)]
    public int num = 99;

    public HouseParts() {
    }

    public HouseParts(long? moudelId, int num) {
        this.moudelId = moudelId;
        this.num = num;
    }

    public long? getMoudelId() {
        return moudelId;
    }

    public void setMoudelId(long? moudelId) {
        this.moudelId = moudelId;
    }

    public int getNum() {
        return num;
    }

    public void setNum(int num) {
        this.num = num;
    }

}
}