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

            if (OtuRate == "acc-otn-types:OTU0") {
                ts_detai = "1-80";
            }
            if (OtuRate == "acc-otn-types:OTU1")
            {
                string oturate = "2-";
                if (Oduk == "acc-otn-types:ODU1") {
                    ts_detai = oturate +"C0";

                }
                if (Oduk == "acc-otn-types:ODU0")
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

            if (OtuRate == "acc-otn-types:OTU2")
            {
                string oturate = "8-";
                if (Oduk == "acc-otn-types:ODU2" || Oduk == "acc-otn-types:ODU2e")
                {
                    ts_detai = oturate + "FF";

                }
                if (Oduk == "acc-otn-types:ODU1")
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
                if (Oduk == "acc-otn-types:ODU0")
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
                if (Oduk == "acc-otn-types:ODUflex-GFP")
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
            if (OtuRate.Contains("STM-1")|| OtuRate.Contains("STM-4")|| OtuRate.Contains("GE") || OtuRate.Contains("FE")) { ts_detai = "1-80"; }
            if (OtuRate.Contains("10GE")) { ts_detai = "8-FF"; }

            return ts_detai;



            }
    }
}
