//
//  SMSSDK_Unity3dBridge.m
//  SMSSDK_Unity3d
//
//  Created by 李愿生 on 16/8/1.
//  Copyright © 2016年 liys. All rights reserved.
//

#import "SMSSDK_Unity3dBridge.h"
#import <SMS_SDK/SMSSDK.h>
#import <SMS_SDK/SMSSDK+ContactFriends.h>
#import <MOBFoundation/MOBFoundation.h>
#import "SMSSDKUI.h"

#if defined (__cplusplus)
extern "C" {
#endif
    
    extern void __iosSMSSDKRegisterAppWithAppKeyAndAppSerect (void *appKey, void * appSecret);
    
    extern void __iosGetVerificationCode (SMSGetCodeMethod smsGetCodeMethod, void *phoneNumber, void *zone, void *tempCode, void *observer);
    
    extern void __iosCommitVerificationCode (void *phoneNumber, void *zone, void *verificationCode,void *observer);
    
    extern void __iosGetCountryZone(void *observer);
    
    extern void __iosGetAllContractFriends(void *observer);
    
    extern void __iosSubmitUserInfo (void *userInfoString,void *observer);
    
    extern void __iosSMSSDKVersion (void *observer);
    
    extern void __iosEnableAppContractFriends (BOOL state);
    
    
    //Demo UI
    extern void __showRegisterView (SMSGetCodeMethod smsGetCodeMethod, void *tempCode ,void *observer);
    
    extern void __showContractFriendsView (void *observer);
    
#if defined (__cplusplus)
}
#endif

#if defined (__cplusplus)
extern "C" {
#endif
    
    void __iosSMSSDKRegisterAppWithAppKeyAndAppSerect(void *appKey, void * appSecret)
    {
        NSLog(@"3.0.0 以后版本的无需注册 appkey和appSecret，已配置到info.plist内");
    }
    
    void __iosGetVerificationCode (SMSGetCodeMethod smsGetCodeMethod, void *phoneNumber, void *zone, void *tempCode, void *observer)
    {

        
        if (phoneNumber && zone && observer)
        {
            
            NSString  *phoneNumberStr = [NSString stringWithCString:phoneNumber encoding:NSUTF8StringEncoding];
            
            NSString *zoneStr = [NSString stringWithCString:zone encoding:NSUTF8StringEncoding];
            
            NSString *tempCodeStr = nil;
            if(tempCode)
                tempCodeStr = [NSString stringWithCString:tempCode encoding:NSUTF8StringEncoding];
            
            NSString *observerStr  = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
            
            
            [SMSSDK getVerificationCodeByMethod:smsGetCodeMethod phoneNumber:phoneNumberStr zone:zoneStr template:tempCodeStr result:^(NSError *error) {
                
                NSMutableDictionary *resultDic = [NSMutableDictionary dictionaryWithCapacity:0];
                [resultDic setObject:[NSNumber numberWithInt:1] forKey:@"action"];
                
                if (!error )
                {
                    NSString *resultMsg = @"getCodeSuccess";
                    [resultDic setObject:resultMsg forKey:@"res"];
                    [resultDic setObject:[NSNumber numberWithInt:1] forKey:@"status"];
                    //转化回到JSONString的状
                    if (smsGetCodeMethod == SMSGetCodeMethodSMS)
                    {
                        NSLog(@"getTextCode Success_%s",[resultMsg UTF8String]);
                    }
                    else
                    {
                        NSLog(@"getVoiceCode Success_%s",[resultMsg UTF8String]);
                    }
                    
                    NSString *resultStr = [MOBFJson jsonStringFromObject:resultDic];
                    UnitySendMessage([observerStr UTF8String], "_callBack", [resultStr UTF8String]);
                    
                }
                else
                {
                    NSMutableDictionary * resultErrorMsg =[NSMutableDictionary dictionaryWithObjectsAndKeys:@(error.code),@"code" ,error.domain,@"domain",error.userInfo,@"userInfo",  nil];
                    //转化回到JSONString的状态码
                    [resultDic setObject:[NSNumber numberWithInt:2] forKey:@"status"];
                    NSString *resultMsg= [MOBFJson jsonStringFromObject:resultErrorMsg];
                    [resultDic setObject:resultMsg forKey:@"res"];
                    NSString *resultStr = [MOBFJson jsonStringFromObject:resultDic];
                    
                    // 参数1: unity中的场景对象名称,
                    //参数2: unity中场景对象要执行的方法
                    //参数3: ios向unity传递的参数
                    UnitySendMessage([observerStr UTF8String], "_callBack", [resultStr UTF8String]);
                }
                
            }];
        }
        
    }
    
    void __iosCommitVerificationCode (void *phoneNumber, void *zone, void *verificationCode,void *observer)
    {
        NSString *phoneNumberStr = nil;
        NSString *zoneStr = nil;
        NSString *verificationCodeStr = nil;
        NSString *observerStr = nil;
        
        if (phoneNumber && zone && verificationCode && observer)
        {
            phoneNumberStr = [NSString stringWithCString:phoneNumber encoding:NSUTF8StringEncoding];
            zoneStr = [NSString stringWithCString:zone encoding:NSUTF8StringEncoding];
            verificationCodeStr = [NSString stringWithCString:verificationCode encoding:NSUTF8StringEncoding];
            observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
            
            [SMSSDK commitVerificationCode:verificationCodeStr phoneNumber:phoneNumberStr zone:zoneStr result:^(NSError *error) {
                
                NSMutableDictionary *resultDic = [NSMutableDictionary dictionaryWithCapacity:0];
                [resultDic setObject:[NSNumber numberWithInt:2] forKey:@"action"];
                
                if (!error)
                {
                    NSString *resultMsg = @"commitVerifyCode Success";
                    [resultDic setObject:[NSNumber numberWithInt:1] forKey:@"status"];
                    [resultDic setObject:resultMsg forKey:@"res"];
                    NSString *resultString = [MOBFJson jsonStringFromObject:resultDic];
                    //转化回到JSONString的状
                    UnitySendMessage([observerStr UTF8String], "_callBack", [resultString UTF8String]);
                    
                }
                else
                {
                    NSMutableDictionary * resultErrorMsg =[NSMutableDictionary dictionaryWithObjectsAndKeys:@(error.code),@"code" ,error.domain,@"domain",error.userInfo,@"userInfo",  nil];
                    [resultDic setObject:[NSNumber numberWithInt:2] forKey:@"status"];
                    //转化回到JSONString的状态码
                    NSString *resultMsg = [MOBFJson jsonStringFromObject:resultErrorMsg];
                    [resultDic setObject:resultMsg forKey:@"res"];
                    NSString *resultStr = [MOBFJson jsonStringFromObject:resultDic];
                    
                    UnitySendMessage([observerStr UTF8String], "_callBack", [resultStr UTF8String]);
                    
                }
            }];
        }
    }
    
    void __iosGetCountryZone(void *observer)
    {
        if (observer)
        {
            NSString *observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
            [SMSSDK getCountryZone:^(NSError *error, NSArray *zonesArray) {
                NSMutableDictionary *resultDic = [NSMutableDictionary dictionaryWithCapacity:0];
                [resultDic setObject:[NSNumber numberWithInt:3] forKey:@"action"];
                
                if (!error)
                {
                    NSString *zoneString = [MOBFJson jsonStringFromObject:zonesArray];
                    [resultDic setObject:[NSNumber numberWithInt:1] forKey:@"status"];
                    [resultDic setObject:zoneString forKey:@"res"];
                    NSString *resultString = [MOBFJson jsonStringFromObject:resultDic];
                    //转化回到JSONString的状
                    UnitySendMessage([observerStr UTF8String], "_callBack", [resultString UTF8String]);
                    
                }
                else
                {
                    NSMutableDictionary * resultErrorMsg =[NSMutableDictionary dictionaryWithObjectsAndKeys:@(error.code),@"code" ,error.domain,@"domain",error.userInfo,@"userInfo",  nil];
                    [resultDic setObject:[NSNumber numberWithInt:2] forKey:@"status"];
                    NSString *resultMsg = [MOBFJson jsonStringFromObject:resultErrorMsg];
                    [resultDic setObject:resultMsg forKey:@"res"];
                    
                    //转化回到JSONString的状态码
                    NSString *resultStr = [MOBFJson jsonStringFromObject:resultDic];
                    UnitySendMessage([observerStr UTF8String], "_callBack", [resultStr UTF8String]);
                    
                }
            }];
            
        }
        
    }
    
    void __iosGetAllContractFriends(void *observer)
    {
        if (observer)
        {
            NSString *observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
            
            [SMSSDK getAllContactFriends:^(NSError *error, NSArray *friendsArray) {
                
                NSMutableDictionary *resultDic = [NSMutableDictionary dictionaryWithCapacity:0];
                [resultDic setObject:[NSNumber numberWithInt:5] forKey:@"action"];
                
                if (!error)
                {
                    NSString *friendsString = [MOBFJson jsonStringFromObject:friendsArray];
                    [resultDic setObject:[NSNumber numberWithInt:1] forKey:@"status"];
                    [resultDic setObject:friendsString?friendsString:@"No friends was found" forKey:@"res"];
                    NSString *resultString = [MOBFJson jsonStringFromObject:resultDic];
                    UnitySendMessage([observerStr UTF8String], "_callBack", [resultString UTF8String]);
                    
                }
                else
                {
                    NSMutableDictionary * resultErrorMsg =[NSMutableDictionary dictionaryWithObjectsAndKeys:@(error.code),@"code" ,error.domain,@"domain",error.userInfo,@"userInfo",  nil];
                    NSString *resultString = [MOBFJson jsonStringFromObject:resultErrorMsg];
                    [resultDic setObject:[NSNumber numberWithInt:2] forKey:@"status"];
                    [resultDic setObject:resultString forKey:@"res"];
                    
                    //转化回到JSONString的状态码
                    NSString *resultStr = [MOBFJson jsonStringFromObject:resultDic];
                    UnitySendMessage([observerStr UTF8String], "_callBack", [resultStr UTF8String]);
                    
                }
            }];
        }
    }
    
    void __iosSubmitUserInfo (void *userInfoString,void *observer)
    {
        
        if (observer && userInfoString)
        {
            NSString *observerStr =  [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
            NSString *userInfoStr = [NSString stringWithCString:userInfoString encoding:NSUTF8StringEncoding];
            
            NSDictionary *userInfoDic = [MOBFJson objectFromJSONString:userInfoStr];
            SMSSDKUserInfo *userInfo = [[SMSSDKUserInfo alloc] init];
            
            if (userInfoDic.count != 0 && [userInfoDic isKindOfClass:[NSDictionary class]]) {
                
                NSString *phoneNumber = [userInfoDic objectForKey:@"phoneNumber"];
                NSString *avatar = [userInfoDic objectForKey:@"avatar"];
                NSString *nickName = [userInfoDic objectForKey:@"nickName"];
                NSString *uid = [userInfoDic objectForKey:@"uid"];
                userInfo.phone = phoneNumber;
                userInfo.nickname = nickName;
                userInfo.uid = uid;
                userInfo.avatar = avatar;
                
            }
            
            [SMSSDK submitUserInfo:userInfo result:^(NSError *error)
             {
                 NSMutableDictionary *resultDic = [NSMutableDictionary dictionaryWithCapacity:0];
                 [resultDic setObject:[NSNumber numberWithInt:4] forKey:@"action"];
                 if (!error)
                 {
                     NSString *resultMsg = @"submitUserInfoSuccess";
                     [resultDic setObject:[NSNumber numberWithInt:1] forKey:@"status"];
                     [resultDic setObject:resultMsg forKey:@"res"];
                     NSString *resultStr = [MOBFJson jsonStringFromObject:resultDic];
                     UnitySendMessage([observerStr UTF8String], "_callBack", [resultStr UTF8String]);
                     
                 }
                 else
                 {
                     NSMutableDictionary * resultErrorMsg =[NSMutableDictionary dictionaryWithObjectsAndKeys:@(error.code),@"code" ,error.domain,@"domain",error.userInfo,@"userInfo",  nil];
                     NSString *resultString = [MOBFJson jsonStringFromObject:resultErrorMsg];
                     [resultDic setObject:[NSNumber numberWithInt:2] forKey:@"status"];
                     [resultDic setObject:resultString forKey:@"res"];
                     //转化回到JSONString的状态码
                     NSString *resultStr = [MOBFJson jsonStringFromObject:resultDic];
                     UnitySendMessage([observerStr UTF8String], "_callBack", [resultStr UTF8String]);
                 }
                 
             }];
        }
        
    }
    
    void __iosSMSSDKVersion (void *observer)
    {
        if (observer)
        {
            NSMutableDictionary *resultDic = [NSMutableDictionary dictionaryWithCapacity:0];
            [resultDic setObject:[NSNumber numberWithInt:6] forKey:@"action"];
            NSString *observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
            [resultDic setObject:[NSNumber numberWithInt:1] forKey:@"status"];
            
            NSString *versionString = [SMSSDK sdkVersion];
            [resultDic setObject:versionString forKey:@"res"];
            NSString *resultString = [MOBFJson jsonStringFromObject:resultDic];
            UnitySendMessage([observerStr UTF8String], "_callBack", [resultString UTF8String]);
        }
        
    }
    
    void __iosEnableAppContractFriends (BOOL state)
    {
        NSLog(@"__iosEnableAppContractFriends__**state_%d",state);
        [SMSSDK enableAppContactFriends:state];
    }
    
    
    //SMSSDK_Demo UI
    
    void __showRegisterView (SMSGetCodeMethod smsGetCodeMethod,  void *tempCode, void *observer)
    {
        if (observer)
        {
            UIViewController *rootVC =[(id)[UIApplication sharedApplication].delegate rootViewController];
            
            
            NSString *tempCodeStr = [NSString stringWithCString:tempCode encoding:NSUTF8StringEncoding];
            SMSSDKUIGetCodeViewController *vc = [[SMSSDKUIGetCodeViewController alloc] initWithMethod:SMSGetCodeMethodSMS template:tempCodeStr];
            
            UINavigationController *nav = [[UINavigationController alloc] initWithRootViewController:vc];
            
            [rootVC presentViewController:nav animated:YES completion:nil];
        }
    }
    
    void __showContractFriendsView (void *observer)
    {
        if (observer)
        {
            [SMSSDK getAllContactFriends:^(NSError *error, NSArray *friendsArray) {
                
                if (error)
                {
                    NSLog(@"%s,%@",__func__,error);
                }
                else
                {
                    NSLog(@"%@",friendsArray);
                    
                    SMSSDKUIContactFriendsViewController *vc = [[SMSSDKUIContactFriendsViewController alloc] initWithContactFriends:friendsArray];
                    UINavigationController *nav = [[UINavigationController alloc] initWithRootViewController:vc];
                    
                    UIViewController *rootVC =[(id)[UIApplication sharedApplication].delegate rootViewController];
                    
                    [rootVC presentViewController:nav animated:YES completion:nil];
                    
                }
            }];
            
        }
        
    }
    
    @implementation SMSSDK_Unity3dBridge

@end
