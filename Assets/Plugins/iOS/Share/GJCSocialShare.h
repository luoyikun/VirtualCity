#import <UIKit/UIKit.h>

@interface GJCSocialShare : NSObject

+ (id)sharedInstance;

-(void)nativeShare:(NSString*)text media : (NSString*)media;

@end

@interface GJC_DataConvertor : NSObject

+ (NSString*)charToNSString : (char*)value;
+(const char*)NSIntToChar: (NSInteger)value;
+(const char*)NSStringToChar: (NSString*)value;
+(NSArray*)charToNSArray: (char*)value;

+(const char*)serializeErrorWithData:(NSString*)description code : (int)code;
+(const char*)serializeError:(NSError*)error;

+(NSMutableString*)serializeErrorWithDataToNSString:(NSString*)description code : (int)code;
+(NSMutableString*)serializeErrorToNSString:(NSError*)error;


+(const char*)NSStringsArrayToChar:(NSArray*)array;
+(NSString*)serializeNSStringsArray:(NSArray*)array;

@end
