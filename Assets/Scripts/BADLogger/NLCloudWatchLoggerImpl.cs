using System;
using UnityEngine;
using NLog;
using NLog.Config;
using NLog.AWS.Logger;
using System.Threading.Tasks;

namespace BADLogger
{
   public class NLCloudWatchLoggerImpl : IBADLogger
   {
      private NLog.Logger _logger;
      private static string _assumeRoleArn = "arn:aws:iam::654368844800:role/GameLiftServerRole1";
      private string _logGroup = "GameLiftServerLogGroup1";
      private string _awsRegion = "us-east-1";

      private static Amazon.SecurityToken.Model.Credentials GetTemporaryCredentials()
      {
         Debug.Log("GetTemporaryCredentials");

         var amazonSecurityTokenServiceClient = new Amazon.SecurityToken.AmazonSecurityTokenServiceClient();

         var assumeRoleRequest = new Amazon.SecurityToken.Model.AssumeRoleRequest();
         assumeRoleRequest.DurationSeconds = 1600;
         assumeRoleRequest.RoleSessionName = "Session_" + DateTime.UtcNow.ToString("yyyy-MM-ddTHH.mm.ssZ");
         assumeRoleRequest.RoleArn = _assumeRoleArn;

         var assumeRoleResponse = amazonSecurityTokenServiceClient.AssumeRoleAsync(assumeRoleRequest);
         assumeRoleResponse.Wait();

         if (assumeRoleResponse.Status != TaskStatus.RanToCompletion)
         {
            Debug.Log("LOGGING FAILED: AssumeRoleAsync returned " + assumeRoleResponse.Status.ToString());
         }
         return assumeRoleResponse.Result.Credentials;
      }

      public void ConfigureNLog()
      {
         Debug.Log("ConfigureNLog");

         var loggingConfig = new LoggingConfiguration();

         var awsTarget = new AWSTarget()
         {
            LogGroup = _logGroup,
            Region = _awsRegion,
            Credentials = GetTemporaryCredentials()
         };
         loggingConfig.AddTarget("aws", awsTarget);
         loggingConfig.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, awsTarget));

         LogManager.Configuration = loggingConfig;

         _logger = LogManager.GetCurrentClassLogger();
         _logger.Info("Logger is configured.");
      }

      public NLCloudWatchLoggerImpl()
      {
         Debug.Log("BADLoggerCloudWatchServiceImpl constructor");
         ConfigureNLog();
      }

      public void Log(string msg)
      {
         // log to both Unity Editor console and CW
         Debug.Log(msg);
         _logger.Info(msg);
      }
   }
}
