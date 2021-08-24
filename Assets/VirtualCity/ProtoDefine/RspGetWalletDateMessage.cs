using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspGetWalletDateMessage {
[ProtoMember(1)]
    public AccountWallet wallet;

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
}