using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace DorsaFinUI.Services
{
    public class WebApiService
    {
        public static string getApiString(string Route, Dictionary<string, string> parms)
        {
            return apiResult(Route, parms);
        }
        public static T getApiObject<T>(string Route, Dictionary<string, string> parms)
        {
            var r = apiResult(Route, parms);
            try
            {
                return JsonConvert.DeserializeObject<T>(r);
            }
            catch (Exception ex)
            {
                throw new Exception(r);
            }
        }

        public static List<T> getApiList<T>(string Route, Dictionary<string, string> parms)
        {
            try
            {
                var result = apiResult(Route, parms);
                return JsonConvert.DeserializeObject<List<T>>(result);
            }
            catch (Exception e)
            {
                return new List<T>();
            }
        }



        public static void PopulateSimpleTypesForObject(object datum, string[] objectarray)
        {
            var objectproperties = datum.GetType().GetProperties().Where(x =>
                x.PropertyType.IsValueType || x.PropertyType == typeof(string));
            foreach (var property in objectproperties)
            {
                switch (Type.GetTypeCode(property.PropertyType))
                {
                    case TypeCode.String:
                        datum.GetType().GetProperty(property.Name)
                            .SetValue(datum, GetObjectPropertyValue(property.Name, objectarray));
                        break;
                    case TypeCode.Decimal:
                        datum.GetType().GetProperty(property.Name).SetValue(datum,
                            GetObjectPropertyValueAsDecimal(property.Name, objectarray));
                        break;
                    case TypeCode.Double:
                        datum.GetType().GetProperty(property.Name).SetValue(datum,
                            GetObjectPropertyValueAsDouble(property.Name, objectarray));
                        break;
                    case TypeCode.Int32:
                        datum.GetType().GetProperty(property.Name).SetValue(datum,
                            GetObjectPropertyValueAsInt(property.Name, objectarray));
                        break;
                    case TypeCode.Boolean:
                        datum.GetType().GetProperty(property.Name).SetValue(datum,
                            GetObjectPropertyValueAsBool(property.Name, objectarray));
                        break;
                    case TypeCode.DateTime:
                        datum.GetType().GetProperty(property.Name).SetValue(datum,
                            GetObjectPropertyValueAsDateTime(property.Name, objectarray));
                        break;
                    default:
                        datum.GetType().GetProperty(property.Name)
                            .SetValue(datum, GetObjectPropertyValue(property.Name, objectarray));
                        break;
                }
            }
        }
        public static string GetObjectPropertyValue(string propertyName, string[] data)
        {
            var result = "";
            var dataDictionary = new Dictionary<string, string>();
            var dataarray = data.Where(x => x.Contains(propertyName)).ToArray();
            foreach (var item in dataarray)
            {
                var temp = item.Split(':');
                if (temp.Length > 2)
                {
                    var secondpart = "";
                    for (var i = 1; i < temp.Length; i++)
                    {
                        secondpart += temp[i].Replace("\"", "").Replace("\\", "").Replace("{", "").Replace("}", "")
                            .TrimStart('[').TrimEnd(']') + ":";
                    }
                    dataDictionary.Add(temp[0].Replace("\"", "").Replace("\\", "").Replace("{", "").Replace("}", "").TrimStart('[').TrimEnd(']').TrimEnd(':'),
                        secondpart.TrimEnd(':'));
                }
                else
                {
                    dataDictionary.Add(temp[0].Replace("\"", "").Replace("\\", "").Replace("{", "").Replace("}", "").TrimStart('[').TrimEnd(']').TrimEnd(':'),
                        temp[1].Replace("\"", "").Replace("\\", "").Replace("{", "").Replace("}", "").TrimStart('[').TrimEnd(']').TrimEnd(':'));
                }
            }

            result = dataDictionary.FirstOrDefault(x => x.Key == propertyName).Value;

            return String.IsNullOrWhiteSpace(result) ? "" : result.Replace("\"", "").Replace("\\", "").Replace("T00", " 00").Replace("null", "").TrimEnd(':');
        }
        public static DateTime GetObjectPropertyValueAsDateTime(string propertyName, string[] data)
        {
            var success = DateTime.TryParse(GetObjectPropertyValue(propertyName, data), out DateTime newValue);
            return success ? newValue : new DateTime();
        }
        public static int GetObjectPropertyValueAsInt(string propertyName, string[] data)
        {
            var success = int.TryParse(GetObjectPropertyValue(propertyName, data), out int newValue);
            return success ? newValue : 0;
        }

        public static decimal GetObjectPropertyValueAsDecimal(string propertyName, string[] data)
        {
            var success = decimal.TryParse(GetObjectPropertyValue(propertyName, data), out decimal newValue);
            return success ? newValue : 0m;
        }

        public static double GetObjectPropertyValueAsDouble(string propertyName, string[] data)
        {
            var success = double.TryParse(GetObjectPropertyValue(propertyName, data), out double newValue);
            return success ? newValue : 0;
        }

        public static bool GetObjectPropertyValueAsBool(string propertyName, string[] data)
        {
            var success = bool.TryParse(GetObjectPropertyValue(propertyName, data), out bool newValue);
            return success ? newValue : false;
        }
        private static string CleanArrayFirstIndexPoint(string arrayindex, string replaceOne, string replaceTwo)
        {
            var itemsToTrim = new[] { '"', ':', '{', '[' };
            var result = string.IsNullOrWhiteSpace(replaceTwo) ?
                arrayindex.Replace(replaceOne, "").TrimStart(itemsToTrim) :
                arrayindex.Replace(replaceOne, "").Replace(replaceTwo, "").TrimStart(itemsToTrim);
            return result;
        }
        private static List<string> ParseOutData(List<string> dataList, int setIndexTo, string haltPoint, string haltPointClarificator = "")
        {
            var list = new List<string>();

            if (!string.IsNullOrWhiteSpace(haltPointClarificator))
            {
                for (var index = setIndexTo; index < dataList.Count && index >= 0; index++)
                {
                    if (index != setIndexTo && dataList[index].Contains(haltPoint) && !dataList[index].Contains(haltPointClarificator))
                    {
                        break;
                    }
                    list.Add(dataList[index]);
                    dataList[index] = "";
                }
            }
            else
            {
                for (var index = setIndexTo; index < dataList.Count && index >= 0; index++)
                {
                    if (index != setIndexTo && dataList[index].Contains(haltPoint))
                    {
                        break;
                    }
                    list.Add(dataList[index]);
                    dataList[index] = "";
                }
            }
            return list;
        }
        //private static string getToken()
        //{
        //    var u = HttpContext.Current.User as ClaimsPrincipal;
        //    return u.FindFirst("id_token").Value;
        //}

        private static string apiResult(string Route, Dictionary<string, string> parms)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(10);
                var webapi = System.Configuration.ConfigurationManager.AppSettings["DorsaFinAPI"]; //"https://localhost:62758"
                client.BaseAddress = new Uri(webapi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                //var token = getToken();
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", getToken());
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                //var response = client.GetAsync($"SearchCustomer?term=applied"); // + id.ToString());
                var response = client.GetAsync(Route);
                response.Wait();
                return response.Result.Content.ReadAsStringAsync().Result;
            }
        }


        public static string apiPost(string Route, object input)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage();
                request.Method = HttpMethod.Post;
                request.RequestUri =
                    new System.Uri(System.Configuration.ConfigurationManager.AppSettings["DorsaFinAPI"] + Route);
                request.Content =
                    new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                //var token = getToken();
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", getToken());
                var response = client.SendAsync(request);
                response.Wait();
                var json = response.Result.Content.ReadAsStringAsync().Result;
                return json;
            }
        }
    }
}