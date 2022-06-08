using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGbot
{
    struct vkmess 
    {
        public readonly string message = "";
        public readonly string keyname = "";
        public readonly long? userid = 0;
        
        public vkmess(string message, string keyname, long? userid)
        {
            this.message = message;
            this.keyname = keyname;
            this.userid = userid;
        }  
    }

}
