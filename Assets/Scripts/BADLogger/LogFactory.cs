// This class along with the logging structure is for demo purposes only, it is not ideal for production.
// src: https://stackoverflow.com/a/8486054/1956540

namespace BADLogger
{
   public abstract class LogFactory
   {
      private static IBADLogger _instance;

      // As a limitation of this design, this Assign must be set before any calls to Intance,
      // or else you'll end up with the default LogFactory implimentation 
      public static void Assign(IBADLogger instance)
      {
         _instance = instance;
      }

      public static IBADLogger Instance
      {
         get
         {
            return _instance;
         }
      }
   }
}