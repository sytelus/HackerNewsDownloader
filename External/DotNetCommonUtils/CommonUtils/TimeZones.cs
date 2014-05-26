using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils
{
    internal static class TimeZones
    {
        public static readonly Dictionary<string, string> AbbreviationsMap = new Dictionary<string, string>() {
            {"ACDT", "+1030"}, // Australian Central Daylight
            {"ACST", "+0930"}, // Australian Central Standard
            {"ADT", "-0300"}, // (US) Atlantic Daylight
            {"AEDT", "+1100"}, // Australian East Daylight
            {"AEST", "+1000"}, // Australian East Standard
            {"AHDT", "-0900"},
            {"AHST", "-1000"},
            {"AST", "-0400"}, // (US) Atlantic Standard
            {"AT", "-0200"}, // Azores
            {"AWDT", "+0900"}, // Australian West Daylight
            {"AWST", "+0800"}, // Australian West Standard
            {"BAT", "+0300"}, // Bhagdad
            {"BDST", "+0200"}, // British Double Summer
            {"BET", "-1100"}, // Bering Standard
            {"BST", "-0300"}, // Brazil Standard
            {"BT", "+0300"}, // Baghdad
            {"BZT2", "-0300"}, //Brazil Zone 2
            {"CADT", "+1030"}, // Central Australian Daylight
            {"CAST", "+0930"}, // Central Australian Standard
            {"CAT", "-1000"}, // Central Alaska
            {"CCT", "+0800"}, // China Coast
            {"CDT", "-0500"}, // (US) Central Daylight
            {"CED", "+0200"}, // Central European Daylight
            {"CET", "+0100"}, // Central European
            {"CST", "-0600"}, // (US) Central Standard
            {"CENTRAL", "-0600"}, // (US) Central Standard
            {"EAST", "+1000"}, // Eastern Australian Standard
            {"EDT", "-0400"}, // (US) Eastern Daylight
            {"EED", "+0300"}, // Eastern European Daylight
            {"EET", "+0200"}, // Eastern Europe
            {"EEST", "+0300"}, // Eastern Europe Summer
            {"EST", "-0500"}, // (US) Eastern Standard
            {"EASTERN", "-0500"}, // (US) Eastern Standard
            {"FST", "+0200"}, // French Summer
            {"FWT", "+0100"}, // French Winter
            {"GMT", "-0000"}, // Greenwich Mean
            {"GST", "+1000"}, // Guam Standard
            {"HDT", "-0900"}, // Hawaii Daylight
            {"HST", "-1000"}, // Hawaii Standard
            {"IDLE", "+1200"}, // Internation Date Line East
            {"IDLW", "-1200"}, // Internation Date Line West
            {"IST", "+0530"}, // Indian Standard
            {"IT", "+0330"}, // Iran
            {"JST", "+0900"}, // Japan Standard
            {"JT", "+0700"}, // Java
            {"MDT", "-0600"}, // (US) Mountain Daylight
            {"MED", "+0200"}, // Middle European Daylight
            {"MET", "+0100"}, // Middle European
            {"MEST", "+0200"}, // Middle European Summer
            {"MEWT", "+0100"}, // Middle European Winter
            {"MST", "-0700"}, // (US) Mountain Standard
            {"MOUNTAIN", "-0700"}, // (US) Mountain Standard
            {"MT", "+0800"}, // Moluccas
            {"NDT", "-0230"}, // Newfoundland Daylight
            {"NFT", "-0330"}, // Newfoundland
            {"NT", "-1100"}, // Nome
            {"NST", "+0630"}, // North Sumatra
            {"NZ", "+1100"}, // New Zealand 
            {"NZST", "+1200"}, // New Zealand Standard
            {"NZDT", "+1300"}, // New Zealand Daylight 
            {"NZT", "+1200"}, // New Zealand
            {"PDT", "-0700"}, // (US) Pacific Daylight
            {"PST", "-0800"}, // (US) Pacific Standard
            {"PACIFIC", "-0800"}, // (US) Pacific Standard
            {"ROK", "+0900"}, // Republic of Korea
            {"SAD", "+1000"}, // South Australia Daylight
            {"SAST", "+0900"}, // South Australia Standard
            {"SAT", "+0900"}, // South Australia Standard
            {"SDT", "+1000"}, // South Australia Daylight
            {"SST", "+0200"}, // Swedish Summer
            {"SWT", "+0100"}, // Swedish Winter
            {"USZ3", "+0400"}, //USSR Zone 3
            {"USZ4", "+0500"}, //USSR Zone 4
            {"USZ5", "+0600"}, //USSR Zone 5
            {"USZ6", "+0700"}, //USSR Zone 6
            {"UT", "-0000"}, // Universal Coordinated
            {"UTC", "-0000"}, // Universal Coordinated
            {"UZ10", "+1100"}, //USSR Zone 10
            {"WAT", "-0100"}, // West Africa
            {"WET", "-0000"}, // West European
            {"WST", "+0800"}, // West Australian Standard
            {"YDT", "-0800"}, // Yukon Daylight
            {"YST", "-0900"}, // Yukon Standard
            {"ZP4", "+0400"}, // USSR Zone 3
            {"ZP5", "+0500"}, //USSR Zone 4
            {"ZP6", "+0600"}   //USSR Zone 5
        };
    }
}
