namespace Discord_Bot
{
    using System;
    using System.Linq;
    using System.Web.UI;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public static class Json
    {
        public static string Parse(string json, string parse)
        {
            try
            {
                if (parse != "")
                {
                    dynamic data = JsonConvert.DeserializeObject<dynamic>(json);
                    string @out = DataBinder.Eval(data, parse);
                    return @out;
                }
                else
                {
                    dynamic b = JsonConvert.DeserializeObject<dynamic>(json);
                    return (string)b;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"There was a problem reading '{json}' | ", ex);
            }
        }

        public static JArray ParseArray(string json, string parse = "")
        {
            try
            {
                if (parse != "")
                {
                    dynamic data = JsonConvert.DeserializeObject<dynamic>(json);
                    JArray @out = DataBinder.Eval(data, parse);
                    return @out;
                }
                else
                {
                    return JsonConvert.DeserializeObject<dynamic>(json);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"There was a problem reading '{json}' | ", ex);
            }
        }

        public static JArray AddToArray(JArray array, string newObject)
        {
            try
            {
                JArray obj = array;
                obj.Add(JObject.Parse(newObject));
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception($"There was a problem reading '{array.ToString()}' or '{newObject}' | ", ex);
            }
        }

        public static JObject GetLastJArrayObject(string jSON)
        {
            try
            {
                dynamic something = JsonConvert.DeserializeObject<dynamic>(jSON);
                JArray obj = something.channel_messages;
                return JObject.Parse(obj[obj.Count() - 1].ToString());
            }
            catch (Exception ex)
            {
                throw new Exception($"There was a problem reading '{jSON}' | ", ex);
            }
        }
    }
}
