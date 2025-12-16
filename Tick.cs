using BveEx.Extensions.Native;
using BveEx.PluginHost;
using BveEx.PluginHost.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TM17000_TIS {
    [Plugin(PluginType.VehiclePlugin)]
    public partial class TM17000_TIS : AssemblyPluginBase {
        private readonly INative Native;
        private static VehicleSpec vehicleSpec;
        private static TimeSpan lastRefreshtime = TimeSpan.Zero;
        private static int lastBrakeOutput, lastPowerOutput, lastATOPowerOutput;

        public TM17000_TIS(PluginBuilder services) : base(services) {
            Config.Load();

            Native = Extensions.GetExtension<INative>();
        }

        public override void Dispose() {

        }

        private static int HealLastUpdateTime = 0;

        public override void Tick(TimeSpan elapsed) {
            var state = Native.VehicleState;
            var panel = Native.AtsPanelArray;
            var sound = Native.AtsSoundArray;

            var KeyPos = panel[92];
            var Shubetsu = panel[152];
            var nowHeal = panel[Config.heal_heal];

            panel[152] = KeyPos == 3 ? 0 : panel[152];
            panel[Config.panel_shubetsutokyu] = KeyPos == 3 ? Shubetsu : 0;

            var nowYear = DateTime.Now.Year;
            panel[Config.date_year_1000] = D(nowYear, 3);
            panel[Config.date_year_100] = D(nowYear, 2);
            panel[Config.date_year_10] = D(nowYear, 1);
            panel[Config.date_year_1] = D(nowYear, 0);

            var nowMonth = DateTime.Now.Month;
            panel[Config.date_month] = nowMonth;

            var nowDay = DateTime.Now.Day;
            panel[Config.date_day] = nowDay;

            var nowWeek = (int)DateTime.Now.DayOfWeek;
            panel[Config.date_week] = nowWeek;

            var nowLocation = (int)state.Location;
            //100km = 100000m
            panel[Config.odometer_Km100] = D(nowLocation, 5);
            panel[Config.odometer_Km10] = D(nowLocation, 4);
            panel[Config.odometer_Km1] = D(nowLocation, 3);
            panel[Config.odometer_Km01] = D(nowLocation, 2);
            panel[Config.odometer_Km001] = D(nowLocation, 1);

            //if (state.Time - HealLastUpdateTime > 200) {
            //    panel[Config.heal_heal] = (nowHeal + 1) % 10;
            //    //panel[Config.heal_heal] %= 10;
            //    HealLastUpdateTime = state.Time;
            //}

            panel[Config.heal_heal] = Convert.ToInt32((state.Time.TotalMilliseconds / 200) % 8);

            /*
             1 営団
　           2 東武
　           3 東急・横高
　           4 西武
             */

            if (KeyPos == 1) {
                panel[Config.panel_atoenabled] = panel[Config.autopilotpanel_atoenabled] > 0 ? 1 : 0;
                if ((!Config.panel_tascdisplayunderato) && panel[Config.autopilotpanel_atoenabled] > 0) {
                    panel[Config.panel_tascenabled] = panel[Config.panel_tascmonitor] = 0;
                } else {
                    panel[Config.panel_tascenabled] = panel[Config.autopilotpanel_tascenabled] > 0 ? 1 : 0;
                    panel[Config.panel_tascmonitor] = panel[Config.autopilotpanel_tascmonitor] > 0 ? 1 : 0;
                }
                panel[Config.panel_braketobseb] = panel[Config.panel_braketokyu] = 0;
                panel[Config.panel_powertobseb] = panel[Config.panel_powertokyu] = 0;
                if (state.Time.TotalMilliseconds - lastRefreshtime.TotalMilliseconds > Config.panel_refreshinterval) {
                    lastRefreshtime = state.Time;
                    lastBrakeOutput = panel[Config.autopilotpanel_brake];
                    lastPowerOutput = panel[Config.autopilotpanel_power];
                    lastATOPowerOutput = panel[Config.autopilotpanel_atopower]; 
                } else {
                    panel[Config.autopilotpanel_brake] = lastBrakeOutput;
                    panel[Config.autopilotpanel_power] = lastPowerOutput;
                    panel[Config.autopilotpanel_atopower] = lastATOPowerOutput;
                }
                panel[Config.panel_tokyutascbrake] = panel[Config.panel_atopowertobseb] = panel[Config.panel_atopowertokyu] = 0;
            } else if (KeyPos == 2) {
                panel[Config.panel_atoenabled] = panel[Config.autopilotpanel_atoenabled] > 0 ? 2 : 0;
                if ((!Config.panel_tascdisplayunderato) && panel[Config.autopilotpanel_atoenabled] > 0) {
                    panel[Config.panel_tascenabled] = panel[Config.panel_tascmonitor] = 0;
                } else {
                    panel[Config.panel_tascenabled] = panel[Config.autopilotpanel_tascenabled] > 0 ? 2 : 0;
                    panel[Config.panel_tascmonitor] = panel[Config.autopilotpanel_tascmonitor] > 0 ? 2 : 0;
                }
                if (state.Time.TotalMilliseconds - lastRefreshtime.TotalMilliseconds > Config.panel_refreshinterval) {
                    lastRefreshtime = state.Time;
                    lastBrakeOutput = panel[Config.autopilotpanel_brake];
                    lastPowerOutput = panel[Config.autopilotpanel_power];
                    lastATOPowerOutput = panel[Config.autopilotpanel_atopower];
                } else {
                    panel[Config.panel_braketobseb] = lastBrakeOutput;
                    panel[Config.panel_powertobseb] = lastPowerOutput;
                    panel[Config.panel_atopowertobseb] = lastATOPowerOutput;
                }
                panel[Config.panel_tokyutascbrake] = panel[Config.panel_powertokyu] = panel[Config.panel_braketokyu] = panel[Config.panel_atopowertokyu] = 0;
            } else if (KeyPos == 3) {
                panel[Config.panel_atoenabled] = panel[Config.autopilotpanel_atoenabled] > 0 ? 3 : 0;
                if ((!Config.panel_tascdisplayunderato) && panel[Config.autopilotpanel_atoenabled] > 0) {
                    panel[Config.panel_tascenabled] = panel[Config.panel_tascmonitor] = 0;
                } else {
                    panel[Config.panel_tascenabled] = panel[Config.autopilotpanel_tascenabled] > 0 ? 3 : 0;
                    panel[Config.panel_tascmonitor] = panel[Config.autopilotpanel_tascmonitor] > 0 ? 3 : 0;
                }
                if (state.Time.TotalMilliseconds - lastRefreshtime.TotalMilliseconds > Config.panel_refreshinterval) {
                    lastRefreshtime = state.Time;
                    lastBrakeOutput = panel[Config.autopilotpanel_brake];
                    lastPowerOutput = panel[Config.autopilotpanel_power];
                    lastATOPowerOutput = panel[Config.autopilotpanel_atopower];
                } else {
                    panel[Config.panel_braketokyu] = lastBrakeOutput;
                    panel[Config.panel_powertokyu] = lastPowerOutput;
                    panel[Config.panel_atopowertokyu] = lastATOPowerOutput;
                }
                panel[Config.panel_tokyutascbrake] = panel[Config.autopilotpanel_tascmonitor] > 0 ? panel[Config.autopilotpanel_tascbrake] : 0;
                panel[Config.panel_powertobseb] = panel[Config.panel_braketobseb] = panel[Config.panel_atopowertobseb] = 0;
            } else if (KeyPos == 4) {
                panel[Config.panel_atoenabled] = panel[Config.autopilotpanel_atoenabled] > 0 ? 4 : 0;
                if ((!Config.panel_tascdisplayunderato) && panel[Config.autopilotpanel_atoenabled] > 0) {
                    panel[Config.panel_tascenabled] = panel[Config.panel_tascmonitor] = 0;
                } else {
                    panel[Config.panel_tascenabled] = panel[Config.autopilotpanel_tascenabled] > 0 ? 4 : 0;
                    panel[Config.panel_tascmonitor] = panel[Config.autopilotpanel_tascmonitor] > 0 ? 4 : 0;
                }
                if (state.Time.TotalMilliseconds - lastRefreshtime.TotalMilliseconds > Config.panel_refreshinterval) {
                    lastRefreshtime = state.Time;
                    lastBrakeOutput = panel[Config.autopilotpanel_brake];
                    lastPowerOutput = panel[Config.autopilotpanel_power];
                    lastATOPowerOutput = panel[Config.autopilotpanel_atopower];
                } else {
                    panel[Config.panel_braketobseb] = lastBrakeOutput;
                    panel[Config.panel_powertobseb] = lastPowerOutput;
                    panel[Config.panel_atopowertobseb] = lastATOPowerOutput;
                }
                panel[Config.panel_tokyutascbrake] = panel[Config.panel_powertokyu] = panel[Config.panel_braketokyu] = panel[Config.panel_atopowertokyu] = 0;
            } else {
                if (state.Time.TotalMilliseconds - lastRefreshtime.TotalMilliseconds > Config.panel_refreshinterval) {
                    lastRefreshtime = state.Time;
                    lastBrakeOutput = panel[Config.autopilotpanel_brake];
                    lastPowerOutput = panel[Config.autopilotpanel_power];
                    lastATOPowerOutput = panel[Config.autopilotpanel_atopower];
                } else {
                    panel[Config.autopilotpanel_brake] = lastBrakeOutput;
                    panel[Config.autopilotpanel_power] = lastPowerOutput;
                    panel[Config.autopilotpanel_atopower] = lastATOPowerOutput;
                }
                panel[Config.panel_atoenabled] = panel[Config.panel_tascenabled] = panel[Config.panel_tascmonitor] = 0;
                panel[Config.panel_tokyutascbrake] = panel[Config.panel_braketobseb] = panel[Config.panel_braketokyu] = 0;
                panel[Config.panel_powertobseb] = panel[Config.panel_powertokyu] = 0;
                panel[Config.panel_atopowertobseb] = panel[Config.panel_atopowertokyu] = 0;
            }
        }
    }
}
