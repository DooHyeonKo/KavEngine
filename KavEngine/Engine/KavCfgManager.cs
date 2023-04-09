using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KavEngine.Engine
{
    class KavCfgManager
    {
        public void LoadConfing()
        {
            StreamReader Sr = new StreamReader(KavConfig.EngineConfig);

            while (!Sr.EndOfStream)
            {
                // Set Config
            }
        }
    }
}
