
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;

namespace TM17000_TIS {

    public static class Config {
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public const double LessInf = 0x7fffffff;
        public static string PluginDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string path;
        private const int buffer_size = 4096;

        //配置项
        //[odometer]
        public static int odometer_Km100 = 255;
        public static int odometer_Km10 = 255;
        public static int odometer_Km1 = 255;
        public static int odometer_Km01 = 255;
        public static int odometer_Km001 = 255;

        //[heal]
        public static int heal_heal = 255;

        //[handle]
        public static int handle_reverser = 255;
        public static int handle_power = 255;
        public static int handle_brake = 255;

        //[date]
        public static int date_year_1000 = 255;
        public static int date_year_100 = 255;
        public static int date_year_10 = 255;
        public static int date_year_1 = 255;
        public static int date_month = 255;
        public static int date_day = 255;
        public static int date_week = 255;

        //[autopilotpanel]
        public static int autopilotpanel_atoenabled = 255;
        public static int autopilotpanel_tascenabled = 255;
        public static int autopilotpanel_tascmonitor = 255;
        public static int autopilotpanel_atopower = 255;
        public static int autopilotpanel_brake = 255;
        public static int autopilotpanel_power = 255;
        public static int autopilotpanel_tascbrake = 255;

        //[panel]
        public static int panel_atoenabled = 255;
        public static int panel_tascenabled = 255;
        public static int panel_tascmonitor = 255;
        public static int panel_powertobseb = 255;
        public static int panel_braketobseb = 255;
        public static int panel_shubetsutokyu = 255;
        public static int panel_powertokyu = 255;
        public static int panel_braketokyu = 255;
        public static int panel_atopowertobseb = 255;
        public static int panel_atopowertokyu = 255;
        public static int panel_tokyutascbrake = 255;
        public static bool panel_tascdisplayunderato = false;
        public static int panel_refreshinterval = 0;
        public static void Load() {
            path = new FileInfo(Path.Combine(PluginDir, "TISConfig.ini")).FullName;
            if (File.Exists(path)) {
                try {
                    //[odometer]
                    ReadConfig("odometer", "km100", ref odometer_Km100);
                    ReadConfig("odometer", "km10", ref odometer_Km10);
                    ReadConfig("odometer", "km1", ref odometer_Km1);
                    ReadConfig("odometer", "km0.1", ref odometer_Km01);
                    ReadConfig("odometer", "Km0.01", ref odometer_Km001);

                    //[handle]
                    ReadConfig("handle", "reverser", ref handle_reverser);
                    ReadConfig("handle", "power", ref handle_power);
                    ReadConfig("handle", "brake", ref handle_brake);

                    //[heal]
                    ReadConfig("heal", "heal", ref heal_heal);

                    //[date]
                    ReadConfig("date", "year1000", ref date_year_1000);
                    ReadConfig("date", "year100", ref date_year_100);
                    ReadConfig("date", "year10", ref date_year_10);
                    ReadConfig("date", "year1", ref date_year_1);
                    ReadConfig("date", "month", ref date_month);
                    ReadConfig("date", "day", ref date_day);
                    ReadConfig("date", "week", ref date_week);

                    //[autopilotpanel]
                    ReadConfig("autopilotpanel", "atoenabled", ref autopilotpanel_atoenabled);
                    ReadConfig("autopilotpanel", "tascenabled", ref autopilotpanel_tascenabled);
                    ReadConfig("autopilotpanel", "tascmonitor", ref autopilotpanel_tascmonitor);
                    ReadConfig("autopilotpanel", "atopower", ref autopilotpanel_atopower);
                    ReadConfig("autopilotpanel", "brake", ref autopilotpanel_brake);
                    ReadConfig("autopilotpanel", "power", ref autopilotpanel_power);
                    ReadConfig("autopilotpanel", "tascbrake", ref autopilotpanel_tascbrake);

                    //[panel]
                    ReadConfig("panel", "atoenabled", ref panel_atoenabled);
                    ReadConfig("panel", "tascenabled", ref panel_tascenabled);
                    ReadConfig("panel", "tascmonitor", ref panel_tascmonitor);
                    ReadConfig("panel", "powertobseb", ref panel_powertobseb);
                    ReadConfig("panel", "braketobseb", ref panel_braketobseb);
                    ReadConfig("panel", "shubetsutokyu", ref panel_shubetsutokyu);
                    ReadConfig("panel", "powertokyu", ref panel_powertokyu);
                    ReadConfig("panel", "braketokyu", ref panel_braketokyu);
                    ReadConfig("panel", "atopowertokyu", ref panel_atopowertokyu);
                    ReadConfig("panel", "atopowertobseb", ref panel_atopowertobseb);
                    ReadConfig("panel", "tokyutascbrake", ref panel_tokyutascbrake);
                    ReadConfig("panel", "tascdisplayunderato", ref panel_tascdisplayunderato);
                    ReadConfig("panel", "refreshinterval", ref panel_refreshinterval);
                } catch (Exception ex) {
                    throw ex;
                }
            } else throw new Exception("Unable to find configuration file: TISConfig.ini");
        }

        //读取配置相关函数
        private static void ReadConfig(string Section, string Key, ref int Value) {
            var OriginalVal = Value;
            var RetVal = new StringBuilder(buffer_size);
            var Readsize = GetPrivateProfileString(Section, Key, "", RetVal, buffer_size, path);
            if (Readsize > 0 && Readsize < buffer_size - 1) {
                Value = Convert.ToInt32(RetVal.ToString());
            } else {
                Value = OriginalVal;
            }
        }

        private static void ReadConfig(string Section, string Key, ref double Value) {
            var OriginalVal = Value;
            var RetVal = new StringBuilder(buffer_size);
            var Readsize = GetPrivateProfileString(Section, Key, "", RetVal, buffer_size, path);
            if (Readsize > 0 && Readsize < buffer_size - 1) {
                Value = Convert.ToDouble(RetVal.ToString());
            } else {
                Value = OriginalVal;
            }
        }

        private static void ReadConfig(string Section, string Key, ref bool Value) {
            var OriginalVal = Value;
            var RetVal = new StringBuilder(buffer_size);
            var Readsize = GetPrivateProfileString(Section, Key, "", RetVal, buffer_size, path);
            if (Readsize > 0 && Readsize < buffer_size - 1) {
                Value = Convert.ToBoolean(RetVal.ToString());
            } else {
                Value = OriginalVal;
            }
        }

        private static void ReadConfig(string Section, string Key, ref string Value) {
            var OriginalVal = Value;
            var RetVal = new StringBuilder(buffer_size);
            var Readsize = GetPrivateProfileString(Section, Key, "", RetVal, buffer_size, path);
            if (Readsize > 0 && Readsize < buffer_size - 1) {
                Value = RetVal.ToString();
            } else {
                Value = OriginalVal;
            }
        }

        private static void ReadConfig(string Section, string Key, ref Keys Value) {
            var OriginalVal = Value;
            var RetVal = new StringBuilder(buffer_size);
            var Readsize = GetPrivateProfileString(Section, Key, "", RetVal, buffer_size, path);
            if (Readsize > 0 && Readsize < buffer_size - 1) {
                Value = (Keys)Enum.Parse(typeof(Keys), RetVal.ToString(), false);
            } else {
                Value = OriginalVal;
            }
        }
    }
}
