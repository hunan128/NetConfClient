using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetConfClientSoftware
{
    class TSConversion
    {
        public static string Ts (string OtuRate, string Oduk,string TsInt){
            string ts_detai = "";

            if (OtuRate == "OTU0") {
                ts_detai = "1-80";
            }
            if (OtuRate.Contains("OTU1")|| OtuRate.Contains("ODU1"))
            {
                string oturate = "2-";
                if (Oduk.Contains("ODU1")) {
                    ts_detai = oturate +"C0";

                }
                if (Oduk.Contains("ODU0"))
                {
                    switch (TsInt) {
                        case "1":
                            ts_detai = oturate + "80";
                            break;
                        case "2":
                            ts_detai = oturate + "40";
                            break;
                    }
                }
            }

            if (OtuRate == "OTU2"|| OtuRate.Contains("ODU2"))
            {
                string oturate = "8-";
                if (Oduk.Contains("ODU2") || Oduk.Contains("ODU2e"))
                {
                    ts_detai = oturate + "FF";

                }
                if (Oduk.Contains("ODU1"))
                {
                    switch (TsInt)
                    {
                        case "1":
                            ts_detai = oturate + "C0";
                            break;
                        case "2":
                            ts_detai = oturate + "30";
                            break;
                        case "3":
                            ts_detai = oturate + "0C";
                            break;
                        case "4":
                            ts_detai = oturate + "03";
                            break;
                    }
                }
                if (Oduk.Contains("ODU0"))
                {
                    switch (TsInt)
                    {
                        case "1":
                            ts_detai = oturate + "80";
                            break;
                        case "2":
                            ts_detai = oturate + "40";
                            break;
                        case "3":
                            ts_detai = oturate + "20";
                            break;
                        case "4":
                            ts_detai = oturate + "10";
                            break;
                        case "5":
                            ts_detai = oturate + "08";
                            break;
                        case "6":
                            ts_detai = oturate + "04";
                            break;
                        case "7":
                            ts_detai = oturate + "02";
                            break;
                        case "8":
                            ts_detai = oturate + "01";
                            break;
                    }
                }
                if (Oduk.Contains("flex")|| Oduk.Contains("Flex"))
                {
                    switch (TsInt)
                    {
                        case "1":
                            ts_detai = oturate + "80";
                            break;
                        case "2":
                            ts_detai = oturate + "C0";
                            break;
                        case "3":
                            ts_detai = oturate + "E0";
                            break;
                        case "4":
                            ts_detai = oturate + "F0";
                            break;
                        case "5":
                            ts_detai = oturate + "F8";
                            break;
                        case "6":
                            ts_detai = oturate + "FC";
                            break;
                        case "7":
                            ts_detai = oturate + "FE";
                            break;
                        case "8":
                            ts_detai = oturate + "FF";
                            break;
                    }
                }
            }
            if (OtuRate.Contains("STM-16")) { ts_detai = "2-C0"; }
            if (OtuRate.Contains("STM-1")|| OtuRate.Contains("STM-4")|| OtuRate.Contains("GE") || OtuRate.Contains("ETH-1Gb") || OtuRate.Contains("FE")) { ts_detai = "1-80"; }
            if (OtuRate.Contains("10GE")) { ts_detai = "8-FF"; }

            return ts_detai;



            }
    }
}
