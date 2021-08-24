using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using ProtoDefine;
using System;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.Pkcs;
using SGF.Codec;
using Framework.UI;
using Newtonsoft.Json;


public class RSAEncryption {

    static string privatekey;//私钥

    static string publickey;//公钥 
  
    public static string Get_privatekey()
    {
        if (File.Exists(AppConst.LocalPath + "/Rsa.txt"))
        {
            if (privatekey == null)
            {
                privatekey = JsonMgr.GetJsonString(AppConst.LocalPath + "/Rsa.txt");
            }
        }
        return privatekey;
    }

public static void Receive_data(byte[] buf)
{

        Debug.Log("数据被调用了");
        RspCommentMessage rsp = PBSerializer.NDeserialize<RspCommentMessage>(buf);
         Debug.Log(rsp.getRspcmd());
        Debug.Log(rsp.getCode());
        Debug.Log(rsp.GetHashCode());
        Debug.Log(rsp.getTip());
        Debug.Log(rsp.GetType());
        //Debug.LogError(rsp.getTip());

        //if (UIManager.Instance.IsTopPanel(UIPanelName.uiloadpanel))
        //{
        //    UIManager.Instance.PopSelf(false);
        //}
        uiloadpanel.Instance.Close();
        if (rsp.getRspcmd() == 509) // 接受rsa
        {
            if (rsp.getCode() == 0)
            {
               Debug.Log(rsp.tip);
            }
            if (rsp.getCode() == 1)
            {
                Debug.Log("保存了RSA");
                 JsonMgr.SaveJsonString(privatekey, AppConst.LocalPath + "/Rsa.txt");
                VirtualCityMgr.SaveLoginInfo();
                if (UIManager.Instance.IsTopPanel(Vc.AbName.inputonepasswordpanel))
                    {
                        UIManager.Instance.PopSelf(false);
                    }
            }
        }
        if (rsp.getRspcmd() == 512) // 修改密码成功
        {
            if (rsp.getCode() == 1)
            {
                Hint.LoadTips("成功", Color.white);

                if (UIManager.Instance.IsTopPanel(Vc.AbName.accountsecuritypanel))
                {
                    UIManager.Instance.PopSelf(false);
                }

                if (UIManager.Instance.IsTopPanel(Vc.AbName.passwordpanel))
                {
                    //empty();
                    UIManager.Instance.PopSelf(false);
                }
            }
            if (rsp.getCode() == 0)
            {
                Hint.LoadTips(rsp.tip, Color.white);
                if (rsp.getTip() == "未通过系统安全检测，您的系统可能存在风险！")
                {
                    Debug.Log("秘钥错误本地RSA被删除");
                    if (File.Exists(AppConst.LocalPath + "/Rsa.txt"))
                    {
                        File.Delete(AppConst.LocalPath + "/Rsa.txt");
                    }
                }
            }
        }
        if (rsp.getRspcmd() == 511)
        {
            if (rsp.getCode() == 0) { Hint.LoadTips(rsp.tip, Color.white); }
         }


        if (rsp.getRspcmd() == 515) 
        {
            if (rsp.getCode() == 0)
            {
                Debug.Log(rsp.tip);
            }
            if (rsp.getCode() == 1)
            {
                Debug.Log("新用户第一次注册");
                passwordpanel.io = true;
                if (UIManager.Instance.IsTopPanel(Vc.AbName.passwordpanel))
                {
                    UIManager.Instance.PopSelf(false);
                    UIManager.Instance.PushPanel(UIPanelName.tuijianpanel, false, true);
                }
            }
        }
        if (rsp.getRspcmd() == 520)
        {
            if (rsp.getCode() == 1)
            {
                if (UIManager.Instance.IsTopPanel(Vc.AbName.passwordpanel))
                {
                    UIManager.Instance.PopSelf(false);
                }
            }
        }
        //empty();
    }

    /// <summary>
    /// 新用户注册
    /// </summary>
    /// <param name="setNewPasswor"></param>
    public static void star_Encryption(string setNewPasswor)
    {
        Debug.Log("玩家的密码" + DataMgr.m_account.password);
        Debug.Log("新玩家生成秘钥");
        ReqCreatePasswordMessage pak = new ReqCreatePasswordMessage();
        RSACryptoServiceProvider oRSA = new RSACryptoServiceProvider();
        privatekey = oRSA.ToXmlString(true);//私钥
        publickey = oRSA.ToXmlString(false);//公钥 
        pak.setNewPasswordBytes(RsaEncrypt_(setNewPasswor, privatekey));
        pak.setPubKey(RSAPublicKeyDotNet2Java(publickey));
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqCreatePasswordMessage, pak);
        Debug.Log("私钥   " + RSAPrivateKeyDotNet2Java(privatekey));
        Debug.Log("公钥   " + RSAPublicKeyDotNet2Java(publickey));
        JsonMgr.SaveJsonString(privatekey, AppConst.LocalPath + "/Rsa.txt");
        VirtualCityMgr.SaveLoginInfo();
    }


    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="password"></param>
    /// <param name="oid_password"></param>
    /// <param name="is_newplayer"></param>
    public static void Password_Send(string password,string oid_password, bool is_newplayer)
    {
        if (File.Exists(AppConst.LocalPath + "/Rsa.txt"))
        {
            ReqUpdatePasswordMessage pak = new ReqUpdatePasswordMessage();
            if (is_newplayer)
            {
                pak.oldPassword = null;
            }
            else
            {
                 pak.setOldPassword(RsaEncrypt_(oid_password, JsonMgr.GetJsonString(AppConst.LocalPath + "/Rsa.txt")));
            }

             pak.setNewPassword(RsaEncrypt_(password, JsonMgr.GetJsonString(AppConst.LocalPath + "/Rsa.txt")));
            HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqUpdatePasswordMessage, pak);
        }
        else
        {
            Hint.LoadTips("文件丢失不能修改密码,请重新登录", Color.white);
        }
    }

    
    /// <summary>
    /// 登陆的时候修改密码
    /// </summary>
    /// <param name="password"></param>
    public static void Password_Send_Land(string password)
    {
        ReqUpdatePasswordByMessage pak = new ReqUpdatePasswordByMessage();
        RSACryptoServiceProvider oRSA = new RSACryptoServiceProvider();
        privatekey = oRSA.ToXmlString(true);//私钥
        publickey = oRSA.ToXmlString(false);//公钥 
        pak.setContent(RsaEncrypt_(password, privatekey));
        pak.setPub_key(RSAPublicKeyDotNet2Java(publickey));
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqUpdatePasswordByMessage, pak);
        Debug.Log("发送成功");
    }


    /// <summary>
    /// 本地RSA被删除的时候重新发一个秘钥
    /// </summary>
    /// <param name="password"></param>
    public static void Resend(string password)
    {
        ReqUpdatePubKeyMessage pak = new ReqUpdatePubKeyMessage();
        RSACryptoServiceProvider oRSA = new RSACryptoServiceProvider();
        privatekey = oRSA.ToXmlString(true);//私钥
        publickey = oRSA.ToXmlString(false);//公钥 
        pak.setContent(RsaEncrypt_(password, privatekey));
        pak.setPub_key(RSAPublicKeyDotNet2Java(publickey));
        pak.setIsCreate(0);
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqUpdatePubKeyMessage, pak);
        Debug.Log("重新生成了秘钥！");
    }

    /// <summary>
    /// 加密（用于公钥加密）
    /// </summary>
    /// <param name="rawInput">要加密的数据</param>
    /// <param name="publicKey">公钥</param>
    /// <returns></returns>
    public static string RsaEncrypt(string rawInput, string publicKey)
    {
        if (string.IsNullOrEmpty(rawInput)) { return string.Empty; }
        // if (string.IsNullOrWhiteSpace(publicKey)) { throw new ArgumentException("Invalid Public Key"); }
        using (var rsaProvider = new RSACryptoServiceProvider())
        {
            var inputBytes = Encoding.UTF8.GetBytes(rawInput);
            //有含义的字符串转化为字节流 
            rsaProvider.FromXmlString(publicKey);
            //载入公钥 
            int bufferSize = (rsaProvider.KeySize / 8) - 11;
            //单块最大长度 
            var buffer = new byte[bufferSize];
            using (MemoryStream inputStream = new MemoryStream(inputBytes), outputStream = new MemoryStream())
            {
                while (true)
                {
                    //分段加密
                    int readSize = inputStream.Read(buffer, 0, bufferSize);
                    if (readSize <= 0)
                    {
                        break;
                    }
                    var temp = new byte[readSize];
                    Array.Copy(buffer, 0, temp, 0, readSize);
                    var encryptedBytes = rsaProvider.Encrypt(temp, false);
                    outputStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                }
                return Convert.ToBase64String(outputStream.ToArray());//转化为字节流方便传输 } } }
            }
        }
    }


    /// <summary>
    /// 解密 （用于公钥加密的的解密）
    /// </summary>
    /// <param name="encryptedInput">加密后的数据</param>
    /// <param name="privateKey">私钥</param>
    /// <returns></returns>
    public static string RsaDecrypt(string encryptedInput, string privateKey)
    {
        if (string.IsNullOrEmpty(encryptedInput))
        {
            return string.Empty;
        }
        // if (string.IsNullOrWhiteSpace(privateKey)){ throw new ArgumentException("Invalid Private Key"); }
        using (var rsaProvider = new RSACryptoServiceProvider())
        {
            var inputBytes = Convert.FromBase64String(encryptedInput);
            rsaProvider.FromXmlString(privateKey);
            int bufferSize = rsaProvider.KeySize / 8;
            var buffer = new byte[bufferSize];
            using (MemoryStream inputStream = new MemoryStream(inputBytes), outputStream = new MemoryStream())
            {
                while (true)
                {
                    int readSize = inputStream.Read(buffer, 0, bufferSize);
                    if (readSize <= 0)
                    {
                        break;
                    }
                    var temp = new byte[readSize];
                    Array.Copy(buffer, 0, temp, 0, readSize);
                    var rawBytes = rsaProvider.Decrypt(temp, false);
                    outputStream.Write(rawBytes, 0, rawBytes.Length);
                }
                return Encoding.UTF8.GetString(outputStream.ToArray());
            }
        }
    }


    /// <summary>
    /// （用于私钥加密）
    /// </summary>
    /// <param name="rawInput"></param>
    /// <param name="publicKey"></param>
    /// <returns></returns>
    public static byte[] RsaEncrypt_(string rawInput, string publicKey)
    {
        try
        {
            using (var rsaProvider = new RSACryptoServiceProvider())
            {
                var inputBytes = Encoding.UTF8.GetBytes(rawInput);
                //有含义的字符串转化为字节流 
                rsaProvider.FromXmlString(publicKey);
                //载入公钥 
                int bufferSize = (rsaProvider.KeySize / 8) - 11;
                //单块最大长度 
                var buffer = new byte[bufferSize];
                using (MemoryStream inputStream = new MemoryStream(inputBytes), outputStream = new MemoryStream())
                {
                    while (true)
                    {
                        //分段加密
                        int readSize = inputStream.Read(buffer, 0, bufferSize);
                        if (readSize <= 0)
                        {
                            break;
                        }
                        var temp = new byte[readSize];
                        Array.Copy(buffer, 0, temp, 0, readSize);

                        AsymmetricCipherKeyPair keyPair = DotNetUtilities.GetKeyPair(rsaProvider);
                        IBufferedCipher c = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
                        c.Init(true, keyPair.Private);

                        var encryptedBytes = c.DoFinal(temp);//加密
                        outputStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                    }
                    return outputStream.ToArray();//转化为字节流方便传输 } } }
                }
            }
        }
        catch (Exception)
        {
            if (File.Exists(AppConst.LocalPath + "/Rsa.txt"))
            {
                File.Delete(AppConst.LocalPath + "/Rsa.txt");
            }
            throw;
        }

    }


    /// <summary>
    /// 解密(用于私钥加密的解密)
    /// </summary>
    /// <param name="encryptedInput">加密后的数据</param>
    /// <param name="privateKey">私钥</param>
    /// <returns></returns>
    public static string RsaDecrypt_(string encryptedInput, string privateKey)
    {
        if (string.IsNullOrEmpty(encryptedInput))
        {
            return string.Empty;
        }
        // if (string.IsNullOrWhiteSpace(privateKey)){throw new ArgumentException("Invalid Private Key");}
        using (var rsaProvider = new RSACryptoServiceProvider())
        {
            var inputBytes = Convert.FromBase64String(encryptedInput);
            rsaProvider.FromXmlString(privateKey);
            int bufferSize = rsaProvider.KeySize / 8;
            var buffer = new byte[bufferSize];
            using (MemoryStream inputStream = new MemoryStream(inputBytes), outputStream = new MemoryStream())
            {
                while (true)
                {
                    int readSize = inputStream.Read(buffer, 0, bufferSize);
                    if (readSize <= 0)
                    {
                        break;
                    }
                    var temp = new byte[readSize];
                    Array.Copy(buffer, 0, temp, 0, readSize);

                    RSAParameters rp = rsaProvider.ExportParameters(false);
                    AsymmetricKeyParameter pbk = DotNetUtilities.GetRsaPublicKey(rp);

                    IBufferedCipher c = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
                    //第一个参数为true表示加密，为false表示解密；第二个参数表示密钥
                    c.Init(false, pbk);

                    var rawBytes = c.DoFinal(temp);
                    outputStream.Write(rawBytes, 0, rawBytes.Length);
                }
                return Encoding.UTF8.GetString(outputStream.ToArray());
            }
        }
    }


    /// <summary>
    /// RSA私钥格式转换，.net->java
    /// </summary>
    /// <param name="privateKey">.net生成的私钥</param>
    /// <returns></returns>
    public static string RSAPrivateKeyDotNet2Java(string privateKey)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(privateKey);
        BigInteger m = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Modulus")[0].InnerText));
        BigInteger exp = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Exponent")[0].InnerText));
        BigInteger d = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("D")[0].InnerText));
        BigInteger p = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("P")[0].InnerText));
        BigInteger q = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Q")[0].InnerText));
        BigInteger dp = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("DP")[0].InnerText));
        BigInteger dq = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("DQ")[0].InnerText));
        BigInteger qinv = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("InverseQ")[0].InnerText));

        RsaPrivateCrtKeyParameters privateKeyParam = new RsaPrivateCrtKeyParameters(m, exp, d, p, q, dp, dq, qinv);

        PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKeyParam);
        byte[] serializedPrivateBytes = privateKeyInfo.ToAsn1Object().GetEncoded();
        return Convert.ToBase64String(serializedPrivateBytes);
    }


    /// <summary>
    /// RSA公钥格式转换，.net->java
    /// </summary>
    /// <param name="publicKey">.net生成的公钥</param>
    /// <returns></returns>
    public static string RSAPublicKeyDotNet2Java(string publicKey)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(publicKey);
        BigInteger m = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Modulus")[0].InnerText));
        BigInteger p = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Exponent")[0].InnerText));
        RsaKeyParameters pub = new RsaKeyParameters(false, m, p);

        SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(pub);
        byte[] serializedPublicBytes = publicKeyInfo.ToAsn1Object().GetDerEncoded();
        return Convert.ToBase64String(serializedPublicBytes);
    }

}

 public class password_data
{
    public string newPassword;
}

public class password_oid
{
    public string oldPassword;
}
