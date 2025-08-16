using BveEx.PluginHost.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TM17000_TIS {
    public partial class TM17000_TIS : AssemblyPluginBase {
        /// <summary>
        /// Called when the door is opened
        /// </summary>
        public static void DoorOpen() {

        }

        /// <summary>
        /// Called when the door is closed
        /// </summary>
        public static void DoorClose() {

        }

        static int[] pow10 = new int[] { 1, 10, 100, 1000, 10000, 100000 };
        static int D(int src, int digit) {
            if (pow10[digit] > src) {
                return 10;
            } else if (digit == 0 && src == 0) {
                return 0;
            } else {
                return src / pow10[digit] % 10;
            }
        }
    }
}
