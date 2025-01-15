using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TM17000_TIS {
    public static partial class TM17000_TIS {
        /// <summary>
        /// Called when the door is opened
        /// </summary>
        [DllExport(CallingConvention.StdCall)]
        public static void DoorOpen() {

        }

        /// <summary>
        /// Called when the door is closed
        /// </summary>
        [DllExport(CallingConvention.StdCall)]
        public static void DoorClose() {

        }
    }
}
