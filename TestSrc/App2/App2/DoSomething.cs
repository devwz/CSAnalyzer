using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2
{
    public class DoSomething
    {
        Log _log = new Log();

        public void Logic1()
        {
            _log.LogInfo();
        }

        public void Logic2()
        {
            _log.LogInfo();
        }
    }
}
