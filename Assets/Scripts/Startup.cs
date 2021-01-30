using UnityEngine;
using BADLogger;

public class Startup : MonoBehaviour
{
   private IBADLogger _logger;

   void Awake()
   {
      Debug.Log("Startup awake");
      if (IsArgFlagPresent("-isProd"))
      {
         Debug.Log("isProd flag is present");
         LogFactory.Assign(new NLCloudWatchLoggerImpl());
      }
      else
      {
         Debug.Log("isProd flag is NOT present");
         LogFactory.Assign(new LocalLoggerImpl());
      }

      // do this to kick off the Instance initialization before the other scripts use it
      _logger = LogFactory.Instance;
      _logger.Log("Startup awake complete!");
   }

   // Helper function for getting the command line arguments
   // src: https://stackoverflow.com/a/45578115/1956540
   private static bool IsArgFlagPresent(string name)
   {
      var args = System.Environment.GetCommandLineArgs();
      for (int i = 0; i < args.Length; i++)
      {
         // Debug.Log("Arg: " + args[i]);
         if (args[i] == name)
         {
            return true;
         }
      }
      return false;
   }
}
