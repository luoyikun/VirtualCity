package com.kingston.jforgame.server.game.system.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.entity.master.AccountWallet;
import com.kingston.jforgame.server.game.system.SystemDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.SYSTEM,cmd = SystemDataPool.RSP_GETWALLETDATE)
public class RspGetWalletDateMessage extends Message {
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private AccountWallet wallet;

    public RspGetWalletDateMessage() {
    }

    public RspGetWalletDateMessage(AccountWallet wallet) {
        this.wallet = wallet;
    }

    public AccountWallet getWallet() {
        return wallet;
    }

    public void setWallet(AccountWallet wallet) {
        this.wallet = wallet;
    }
}
