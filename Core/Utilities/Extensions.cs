// In short apache 2 license, check LICENSE file for the legalese

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace Lambda
{
    /// <summary>
    /// This class contains a lot of Extensions some generic some type specific most of them are self explanatory, those wich are not are documented properly
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// Serializes an object
        /// </summary>
        /// <returns>A byte[] containing the serialized object</returns>
        public static byte[] Bytes<T>(this T i)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    bf.Serialize(ms, i);
                    return ms.ToArray();
                }
                catch (SerializationException e)
                {
                    throw new Exception("Object not serializable", e);
                }
            }
        }

        /// <summary>
        /// !!!Stream is flushed but kept open!!!
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <param name="cl">The compression level to use</param>
        /// <param name="data">The data to be used</param>
        /// <returns>A GZip stream thats open and with a clean buffer</returns>
        public static GZipStream Compress<T>(this T i, CompressionLevel cl, byte[] data) where T : Stream
        {
            var gzs = new GZipStream(i, cl);
            gzs.Write(data, 0, data.Length);
            gzs.Flush();
            return gzs;
        }

        public static byte[] Hash<T>(this T io) => SHA256.Create().ComputeHash(io.Bytes());

        public static string Snapshot<T>(this T obj, string ext)
        {
            var hash = obj.Hash();
            var image = new List<byte>(Guid.NewGuid().ToByteArray().Concat(obj.Bytes()).ToArray());
            string file = $"{Environment.CurrentDirectory}\\~~{Guid.NewGuid().ToString("N")}~~{ext}";
            FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write);
            var imgtmp = image.ToArray();
            var cs = fs.Compress(CompressionLevel.Fastest, imgtmp);
            cs.Dispose();
            fs.Dispose();
            File.SetAttributes(file, FileAttributes.Hidden | FileAttributes.System);
            return file;
        }

        public static string ToBase64(this byte[] data) => Convert.ToBase64String(data, Base64FormattingOptions.None);

        public static string ToBase64WLB(this byte[] data) => Convert.ToBase64String(data, Base64FormattingOptions.InsertLineBreaks);

        public static string ToBase64(this string str) => Convert.ToBase64String(Encoding.UTF8.GetBytes(str), Base64FormattingOptions.None);

        public static string ToBase64WLB(this string str) => Convert.ToBase64String(Encoding.UTF8.GetBytes(str), Base64FormattingOptions.InsertLineBreaks);

        public static byte[] FromBase64(this string str) => Convert.FromBase64String(str);

        public static byte[] FromBase64(this char[] chars) => Convert.FromBase64CharArray(chars, 0, chars.Length);

        public static byte[] GetBytes(this string str) => Encoding.UTF8.GetBytes(str);

        public static TResult FetchWebPage<TResult>(this string url, Func<string, StreamReader, TResult> callback)
        {
            var req = WebRequest.Create(url);
            using (var resp = req.GetResponse())
            {
                using (var stream = resp.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return callback(url, reader);
                    }
                }
            }
        }

        public static string FromISO3166(this string str)
        {
            var q = ISO3166.Where(p => p.Key == str.ToUpper()).Select(p => p.Value);
            foreach (var item in q)
            {
                return item;
            }
            return "Not Found";
        }

        public static string ToISO3166(this string str)
        {
            var q = ISO3166.Where(p => p.Value == str).Select(p => p.Key);
            foreach (var item in q)
            {
                return item;
            }
            return "Not Found";
        }

        public enum OS
        {
            Windows = 0,
            Linux,
            MacOS,
            Unix,
            Other
        }

        /// <summary>
        /// Forma a path to a file from a string default to windows style paths
        /// </summary>
        /// <remarks>Make sure to set os property or it will default to windows</remarks>
        public static string FormPath(this string i, char separator)
        {
            if (os == OS.Windows)
                i.Replace(separator, '\\');
            else if (os == OS.Linux || os == OS.MacOS || os == OS.Unix)
                i.Replace(separator, '/');
            else if (os == OS.Other)
            {
                i.Replace(separator, customSep);
            }
            return i;
        }

        public static char customSep = '\0';

        /// <summary>
        /// The os to assume on OS dependent utilities
        /// </summary>
        public static OS os = OS.Windows;

        /// <summary>
        /// Last updated 2017-09-06
        /// </summary>
        public static Dictionary<string, string> ISO3166 = new Dictionary<string, string>() {
            { "AF" , "Afghanistan" },
            {"AX", "Aland Islands"},
            {"AL", "Albania"},
            {"DZ", "Algeria"},
            {"AS", "American Samoa"},
            {"AD", "Andorra"},
            {"AO", "Angola"},
            {"AI", "Anguilla"},
            {"AQ", "Antarctica"},
            {"AG", "Antigua And Barbuda"},
            {"AR", "Argentina"},
            {"AM", "Armenia"},
            {"AW", "Aruba"},
            {"AU", "Australia"},
            {"AT", "Austria"},
            {"AZ", "Azerbaijan"},
            {"BS", "Bahamas"},
            {"BH", "Bahrain"},
            {"BD", "Bangladesh"},
            {"BB", "Barbados"},
            {"BY", "Belarus"},
            {"BE", "Belgium"},
            {"BZ", "Belize"},
            {"BJ", "Benin"},
            {"BM", "Bermuda"},
            {"BT", "Bhutan"},
            {"BO", "Bolivia"},
            {"BA", "Bosnia And Herzegovina"},
            {"BW", "Botswana"},
            {"BV", "Bouvet Island"},
            {"BR", "Brazil"},
            {"IO", "British Indian Ocean Territory"},
            {"BN", "Brunei Darussalam"},
            {"BG", "Bulgaria"},
            {"BF", "Burkina Faso"},
            {"BI", "Burundi"},
            {"KH", "Cambodia"},
            {"CM", "Cameroon"},
            {"CA", "Canada"},
            {"CV", "Cape Verde"},
            {"KY", "Cayman Islands"},
            {"CF", "Central African Republic"},
            {"TD", "Chad"},
            {"CL", "Chile"},
            {"CN", "China"},
            {"CX", "Christmas Island"},
            {"CC", "Cocos (Keeling) Islands"},
            {"CO", "Colombia"},
            {"KM", "Comoros"},
            {"CG", "Congo"},
            {"CD", "Congo Democratic Republic"},
            {"CK", "Cook Islands"},
            {"CR", "Costa Rica"},
            {"CI", "Cote D\"Ivoire"},
            {"HR", "Croatia"},
            {"CU", "Cuba"},
            {"CY", "Cyprus"},
            {"CZ", "Czech Republic"},
            {"DK", "Denmark"},
            {"DJ", "Djibouti"},
            {"DM", "Dominica"},
            {"DO", "Dominican Republic"},
            {"EC", "Ecuador"},
            {"EG", "Egypt"},
            {"SV", "El Salvador"},
            {"GQ", "Equatorial Guinea"},
            {"ER", "Eritrea"},
            {"EE", "Estonia"},
            {"ET", "Ethiopia"},
            {"FK", "Falkland Islands (Malvinas)"},
            {"FO", "Faroe Islands"},
            {"FJ", "Fiji"},
            {"FI", "Finland"},
            {"FR", "France"},
            {"GF", "French Guiana"},
            {"PF", "French Polynesia"},
            {"TF", "French Southern Territories"},
            {"GA", "Gabon"},
            {"GM", "Gambia"},
            {"GE", "Georgia"},
            {"DE", "Germany"},
            {"GH", "Ghana"},
            {"GI", "Gibraltar"},
            {"GR", "Greece"},
            {"GL", "Greenland"},
            {"GD", "Grenada"},
            {"GP", "Guadeloupe"},
            {"GU", "Guam"},
            {"GT", "Guatemala"},
            {"GG", "Guernsey"},
            {"GN", "Guinea"},
            {"GW", "Guinea-Bissau"},
            {"GY", "Guyana"},
            {"HT", "Haiti"},
            {"HM", "Heard Island & Mcdonald Islands"},
            {"VA", "Holy See (Vatican City State)"},
            {"HN", "Honduras"},
            {"HK", "Hong Kong"},
            {"HU", "Hungary"},
            {"IS", "Iceland"},
            {"IN", "India"},
            {"ID", "Indonesia"},
            {"IR", "Iran Islamic Republic Of"},
            {"IQ", "Iraq"},
            {"IE", "Ireland"},
            {"IM", "Isle Of Man"},
            {"IL", "Israel"},
            {"IT", "Italy"},
            {"JM", "Jamaica"},
            {"JP", "Japan"},
            {"JE", "Jersey"},
            {"JO", "Jordan"},
            {"KZ", "Kazakhstan"},
            {"KE", "Kenya"},
            {"KI", "Kiribati"},
            {"KR", "Korea"},
            {"KW", "Kuwait"},
            {"KG", "Kyrgyzstan"},
            {"LA", "Lao People\"s Democratic Republic"},
            {"LV", "Latvia"},
            {"LB", "Lebanon"},
            {"LS", "Lesotho"},
            {"LR", "Liberia"},
            {"LY", "Libyan Arab Jamahiriya"},
            {"LI", "Liechtenstein"},
            {"LT", "Lithuania"},
            {"LU", "Luxembourg"},
            {"MO", "Macao"},
            {"MK", "Macedonia"},
            {"MG", "Madagascar"},
            {"MW", "Malawi"},
            {"MY", "Malaysia"},
            {"MV", "Maldives"},
            {"ML", "Mali"},
            {"MT", "Malta"},
            {"MH", "Marshall Islands"},
            {"MQ", "Martinique"},
            {"MR", "Mauritania"},
            {"MU", "Mauritius"},
            {"YT", "Mayotte"},
            {"MX", "Mexico"},
            {"FM", "Micronesia Federated States Of"},
            {"MD", "Moldova"},
            {"MC", "Monaco"},
            {"MN", "Mongolia"},
            {"ME", "Montenegro"},
            {"MS", "Montserrat"},
            {"MA", "Morocco"},
            {"MZ", "Mozambique"},
            {"MM", "Myanmar"},
            {"NA", "Namibia"},
            {"NR", "Nauru"},
            {"NP", "Nepal"},
            {"NL", "Netherlands"},
            {"AN", "Netherlands Antilles"},
            {"NC", "New Caledonia"},
            {"NZ", "New Zealand"},
            {"NI", "Nicaragua"},
            {"NE", "Niger"},
            {"NG", "Nigeria"},
            {"NU", "Niue"},
            {"NF", "Norfolk Island"},
            {"MP", "Northern Mariana Islands"},
            {"NO", "Norway"},
            {"OM", "Oman"},
            {"PK", "Pakistan"},
            {"PW", "Palau"},
            {"PS", "Palestinian Territory Occupied"},
            {"PA", "Panama"},
            {"PG", "Papua New Guinea"},
            {"PY", "Paraguay"},
            {"PE", "Peru"},
            {"PH", "Philippines"},
            {"PN", "Pitcairn"},
            {"PL", "Poland"},
            {"PT", "Portugal"},
            {"PR", "Puerto Rico"},
            {"QA", "Qatar"},
            {"RE", "Reunion"},
            {"RO", "Romania"},
            {"RU", "Russian Federation"},
            {"RW", "Rwanda"},
            {"BL", "Saint Barthelemy"},
            {"SH", "Saint Helena"},
            {"KN", "Saint Kitts And Nevis"},
            {"LC", "Saint Lucia"},
            {"MF", "Saint Martin"},
            {"PM", "Saint Pierre And Miquelon"},
            {"VC", "Saint Vincent And Grenadines"},
            {"WS", "Samoa"},
            {"SM", "San Marino"},
            {"ST", "Sao Tome And Principe"},
            {"SA", "Saudi Arabia"},
            {"SN", "Senegal"},
            {"RS", "Serbia"},
            {"SC", "Seychelles"},
            {"SL", "Sierra Leone"},
            {"SG", "Singapore"},
            {"SK", "Slovakia"},
            {"SI", "Slovenia"},
            {"SB", "Solomon Islands"},
            {"SO", "Somalia"},
            {"ZA", "South Africa"},
            {"GS", "South Georgia And Sandwich Isl."},
            {"ES", "Spain"},
            {"LK", "Sri Lanka"},
            {"SD", "Sudan"},
            {"SR", "Suriname"},
            {"SJ", "Svalbard And Jan Mayen"},
            {"SZ", "Swaziland"},
            {"SE", "Sweden"},
            {"CH", "Switzerland"},
            {"SY", "Syrian Arab Republic"},
            {"TW", "Taiwan"},
            {"TJ", "Tajikistan"},
            {"TZ", "Tanzania"},
            {"TH", "Thailand"},
            {"TL", "Timor-Leste"},
            {"TG", "Togo"},
            {"TK", "Tokelau"},
            {"TO", "Tonga"},
            {"TT", "Trinidad And Tobago"},
            {"TN", "Tunisia"},
            {"TR", "Turkey"},
            {"TM", "Turkmenistan"},
            {"TC", "Turks And Caicos Islands"},
            {"TV", "Tuvalu"},
            {"UG", "Uganda"},
            {"UA", "Ukraine"},
            {"AE", "United Arab Emirates"},
            {"GB", "United Kingdom"},
            {"US", "United States"},
            {"UM", "United States Outlying Islands"},
            {"UY", "Uruguay"},
            {"UZ", "Uzbekistan"},
            {"VU", "Vanuatu"},
            {"VE", "Venezuela"},
            {"VN", "Viet Nam"},
            {"VG", "Virgin Islands British"},
            {"VI", "Virgin Islands U.S."},
            {"WF", "Wallis And Futuna"},
            {"EH", "Western Sahara"},
            {"YE", "Yemen"},
            {"ZM", "Zambia"},
            {"ZW", "Zimbabwe"}
        };
    }
}