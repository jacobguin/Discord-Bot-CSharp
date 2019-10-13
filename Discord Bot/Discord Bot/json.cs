using Newtonsoft.Json;
using System;
using System.Web.UI;

namespace Discord_Bot
{
    class Json
    {
        public static string Parse(string JSON, string Parse)
        {
            try
            {
                if (Parse != "")
                {
                    dynamic Data = JsonConvert.DeserializeObject<dynamic>(JSON);
                    string Out = DataBinder.Eval(Data, Parse);
                    return Out;
                }
                else
                {
                    dynamic b = JsonConvert.DeserializeObject<dynamic>(JSON);
                    return (string)b;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"There was a problem reading '{JSON}' | ", ex);
            }
        }
    }
}
