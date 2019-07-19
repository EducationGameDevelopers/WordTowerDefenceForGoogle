using System;
using System.Collections.Generic;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using UnityEngine;
using System.Text.RegularExpressions;

public static class ALiCloudSMSTool
{

    /// <summary>
    /// 向手机发送验证码
    /// </summary>
    /// <param name="phoneNumber">手机号码</param>
    /// <param name="vertifyCode">验证码（使用随机数，111111——999999）</param>
    public static void SendPhoneVertifyCode(string phoneNumber, string vertifyCode)
    {
        SendSMS(phoneNumber, Config.AccessKeyId, Config.AccessSecret, vertifyCode);
    }


    /// <summary>
    /// 发送验证码
    /// </summary>
    /// <param name="phoneNumber">手机号码</param>
    /// <param name="accessKeyId"></param>
    /// <param name="accessSecret"></param>
    /// <param name="code">需发送给该手机的验证码</param>
    public static void SendSMS(string phoneNumber, string accessKeyId, string accessSecret, string code)
    {
        IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessSecret);
        DefaultAcsClient client = new DefaultAcsClient(profile);
        CommonRequest request = new CommonRequest();
        request.Method = MethodType.POST;
        request.Domain = "dysmsapi.aliyuncs.com";
        request.Version = "2017-05-25";
        request.Action = "SendSms";
        // request.Protocol = ProtocolType.HTTP;
        //相关验证参数设置
        request.AddQueryParameters("PhoneNumbers", phoneNumber);
        request.AddQueryParameters("SignName", "勇者的呼唤");
        request.AddQueryParameters("TemplateCode", "SMS_164510461");
        request.AddQueryParameters("TemplateParam", "{'code':'" + code + "'}");
        try
        {
            CommonResponse response = client.GetCommonResponse(request);
            Debug.Log(System.Text.Encoding.Default.GetString(response.HttpResponse.Content));
        }
        catch (ServerException e)
        {
            Debug.LogWarning(e);
        }
        catch (ClientException e)
        {
            Debug.LogWarning(e);
        }
    }


    ///// <summary>
    ///// 检验输入的手机号码是否符合相应格式
    ///// </summary>
    //private static bool VertifyPhoneNumber(string phoneNumber)
    //{
    //    //正则校验手机号码
    //    if (Regex.IsMatch(phoneNumber, Config.PhoneNumberPatten))
    //    {
    //        return true;
    //    }

    //    return false;
    //}
}

