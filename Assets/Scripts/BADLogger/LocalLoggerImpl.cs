using UnityEngine;

namespace BADLogger
{
   public class LocalLoggerImpl : IBADLogger
   {
      public void Log(string msg)
      {
         Debug.Log(msg);
      }
   }
}