package com.kingston.jforgame.server.game.sociality.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.sociality.SocialityDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SOCIALITY,cmd = SocialityDataPool.CHANGE_AMOUNT)
public class ChangeAmountMessage extends Message {
    @Protobuf(order = 1)
    private float amount;
    /**
     * 0:金币
     * 1:钻石
     * 2:购物金
     * 3:现金
     */
    @Protobuf(order = 2)
    private int amountType;
    @Protobuf(order = 3)
    private long accountId;

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
