using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedRequest
{
    public struct Cookies
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public string Port { get; set; }
        public bool Secure { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public System.DateTime Expires { get; set; }
        public bool Expired { get; set; }
        public bool Discard { get; set; }
        public string Comment { get; set; }
        public System.Uri CommentUri { get; set; }
        public int Version { get; set; }
    }
}
