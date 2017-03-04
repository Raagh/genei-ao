using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AORNet.Configurations;

namespace AORNet.Model
{
    public class Cheater : Player
    {
        private static Cheater _instance;

        public static Cheater Instance => _instance ?? (_instance = new Cheater());

        public static CheaterConfiguration Configuration = new CheaterConfiguration();
    }
}
