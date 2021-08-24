using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ChangeAmountMessage {
[ProtoMember(1)]
    public float amount;
    /**
     * 0:金币
     * 1:钻石
     * 2:购物金
     * 3:现金
     */
[ProtoMember(2)]
    public int amountType;
[ProtoMember(3)]
    public long accountId;

    public ChangeAmountMessage() {
    }

    public ChangeAmountMessage(float amount, int amountType, long accountId) {
        this.amount = amount;
        this.amountType = amountType;
        this.accountId = accountId;
    }

    public long getAccountId() {
        return accountId;
    }

    public void setAccountId(long accountId) {
        this.accountId = accountId;
    }

    public float getAmount() {
        return amount;
    }

    public void setAmount(float amount) {
        this.amount = amount;
    }

    public int getAmountType() {
        return amountType;
    }

    public void setAmountType(int amountType) {
        this.amountType = amountType;
    }
}
}