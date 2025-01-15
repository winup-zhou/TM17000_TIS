using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TM17000_TIS {
    public static partial class TM17000_TIS {
        static TM17000_TIS() { 
            Config.Load();
        }

        /// <summary>
        /// Called every frame
        /// </summary>
        /// <param name="vehicleState">Current state of vehicle.</param>
        /// <param name="panel">Current state of panel.</param>
        /// <param name="sound">Current state of sound.</param>
        /// <returns>Driving operations of vehicle.</returns>
        [DllExport(CallingConvention.StdCall)]
        public static AtsHandles Elapse(AtsVehicleState state, IntPtr hPanel, IntPtr hSound) {
            var panel = new AtsIoArray(hPanel);
            var sound = new AtsIoArray(hSound);

            var handles = new AtsHandles {
                Power = pPower, Brake = pBrake,
                Reverser = pReverser, ConstantSpeed = AtsCscInstruction.Continue
            };



            return handles;
        }
    }
}
