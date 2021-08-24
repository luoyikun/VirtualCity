using System.Collections.Generic;
using ProtoBuf;

namespace ProtoDefine
{
    [ProtoContract]




    public class AccountWallet
    {


        [ProtoMember(1)] public long? id;

        /**
         账户id
         */
        [ProtoMember(2)] public long accountId;

        /**
         金币数量
         */
        [ProtoMember(3)] public int goldNum;

        /**
         钻石数量
         */
        [ProtoMember(4)] public int diamondNum;

        /**
         购物金数量
         */
        [ProtoMember(5)] public double sMoneyNum;

        /**
         现金数量
         */
        [ProtoMember(6)] public double moneyNum;

        /**
         消费的钻石数量
         */
        [ProtoMember(7)] public int dCostNum;

        /**
         获取的购物金和现金数量
         */
        [ProtoMember(8)] public double mIncome;

        /**
         资产
         */
        [ProtoMember(9)] public int asset;

        /**
         提现限额
         */
        [ProtoMember(10)] public float cashLimit;

        [ProtoMember(11)] public string createtime;

        [ProtoMember(12)] public string updatetime;

        public long? getId()
        {
            return id;
        }

        public void setId(long? id)
        {
            this.id = id;
        }

        public long getAccountId()
        {
            return accountId;
        }

        public void setAccountId(long accountId)
        {
            this.accountId = accountId;
        }

        public int getGoldNum()
        {
            return goldNum;
        }

        public void setGoldNum(int goldNum)
        {
            this.goldNum = goldNum;
        }

        public int getDiamondNum()
        {
            return diamondNum;
        }

        public void setDiamondNum(int diamondNum)
        {
            this.diamondNum = diamondNum;
        }

        public double getSMoneyNum()
        {
            return sMoneyNum;
        }

        public void setSMoneyNum(double sMoneyNum)
        {
            this.sMoneyNum = sMoneyNum;
        }

        public double getMoneyNum()
        {
            return moneyNum;
        }

        public void setMoneyNum(double moneyNum)
        {
            this.moneyNum = moneyNum;
        }

        public int getDCostNum()
        {
            return dCostNum;
        }

        public void setDCostNum(int dCostNum)
        {
            this.dCostNum = dCostNum;
        }

        public double getMIncome()
        {
            return mIncome;
        }

        public void setMIncome(double mIncome)
        {
            this.mIncome = mIncome;
        }

        public int getAsset()
        {
            return asset;
        }

        public void setAsset(int asset)
        {
            this.asset = asset;
        }

        public float getCashLimit()
        {
            return cashLimit;
        }

        public void setCashLimit(float cashLimit)
        {
            this.cashLimit = cashLimit;
        }

        public string getCreatetime()
        {
            return createtime;
        }

        public void setCreatetime(string createtime)
        {
            this.createtime = createtime;
        }

        public string getUpdatetime()
        {
            return updatetime;
        }

        public void setUpdatetime(string updatetime)
        {
            this.updatetime = updatetime;
        }

        public AccountWallet()
        {
        }


    }
}