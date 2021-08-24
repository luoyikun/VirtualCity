using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspGetMoneyTreeMessage {
[ProtoMember(1)]
    public MoneyTree moneyTree;

    public RspGetMoneyTreeMessage() {
    }

    public RspGetMoneyTreeMessage(MoneyTree moneyTree) {
        this.moneyTree = moneyTree;
    }

    public MoneyTree getMoneyTree() {
        return moneyTree;
    }

    public void setMoneyTree(MoneyTree moneyTree) {
        this.moneyTree = moneyTree;
    }
}
}