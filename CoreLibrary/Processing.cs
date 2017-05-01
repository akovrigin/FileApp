using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public class Processing : IProcessibility
    {
        public byte[] Download(byte[] data)
        {
            return data;
        }

        public byte[] Upload(byte[] data)
        {
            return data;
        }
    }
}
