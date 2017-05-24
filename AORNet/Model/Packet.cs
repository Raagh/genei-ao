using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AORNet.Model
{
    public class Packet
    {
        public string Content { get; set; }
        public PacketDirection Direction { get; set; }

        public Packet(string _content, PacketDirection _direction)
        {
            Content = _content;
            Direction = _direction;
        }
    }

    public enum PacketDirection
    {
        ToClient = 0,
        ToServer = 1
    }

}
