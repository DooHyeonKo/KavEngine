using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KavEngine.Engine
{
    class KavDB
    {
        public List<string> GetVirusList()
        {
            return KavConfig.VirusNameList;
        }
    }
}
