using System;
using System.Collections.Generic;
using System.Text;

namespace KavEngine.Engine
{
   public class KavKernel
   {
        public enum ScanResult
        {
            Infected,
            NotInfected,
            NotFoundPath
        }

        public enum CleanResult
        {
            Success,
            Failed
        }

        public enum UpdateResult
        {
            Success,
            Failed
        }
   }
}
