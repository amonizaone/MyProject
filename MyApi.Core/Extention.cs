using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace MyApi.Core
{
    public static class Extention
    {
        public static T MappingObject<T>(this object obj)
        {
            string json = SerializeObject(obj);
            return DeserializeObject<T>(json);
        }

        // public static T MappingDeepObject<T>(this object obj) => Mapper.Map<object, T>(obj);
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
        public static T DeserializeObject<T>(this string str) => JsonConvert.DeserializeObject<T>(str);

        public static string SerializeObject(this object obj, bool IgnoreNull = false)
        {
            JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,

            };
            if (IgnoreNull)
            {
                microsoftDateFormatSettings.NullValueHandling = NullValueHandling.Ignore;
            }

            return JsonConvert.SerializeObject(obj, microsoftDateFormatSettings);
        }
        public static Uri GetUri(this HttpRequest request)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Port = request.Host.Port.GetValueOrDefault(80),
                Path = request.Path.ToString(),
                Query = request.QueryString.ToString()
            };
            return uriBuilder.Uri;
        }
        public static Dictionary<string, TValue> ToDictionary<TValue>(object obj)
        {
            var json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, TValue>>(json);
            return dictionary;
        }
        public static string SerializeObject(this object obj, string strKey)
        {
            JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            };
            string strJson = JsonConvert.SerializeObject(obj, microsoftDateFormatSettings);
            string jsonWithKey = string.Format("\"{0}\" : {1}", strKey, strJson);
            strJson = string.Concat("{", jsonWithKey, "}");
            return strJson;
        }
        public static T KeyByValue<T, W>(this Dictionary<T, W> dict, W val)
        {
            T key = default;
            foreach (KeyValuePair<T, W> pair in dict)
            {
                if (EqualityComparer<W>.Default.Equals(pair.Value, val))
                {
                    key = pair.Key;
                    break;
                }
            }
            return key;
        }
        public static bool HasElement(this ICollection collection) => collection != null && collection.Count > 0;
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source != null && toCheck != null && source.IndexOf(toCheck, comp) >= 0;
        }
        public static bool HasElement(this object obj) => obj != null;

        public static bool HasElement(this string str) => !string.IsNullOrEmpty(str);

        public static string trimStr(this string str) => !string.IsNullOrEmpty(str) ? str.Trim() : str;
        public static string GetQueryString(this object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return string.Join("&", properties.ToArray());
        }
        //      public static void ThrowNew<T>(this Exception ex, string message)
        //where T : Exception
        //      {

        //          if (null == ex)
        //              return; // or throw ArgumentNullException("ex");

        //          throw (Exception)(typeof(T)
        //            .GetConstructor(new Type[] { typeof(String), typeof(Exception) })
        //            .Invoke(message, ex));
        //      }
        public static string ExceptionToString(this Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Date/Time: " + DateTime.UtcNow.ToString());
            sb.AppendLine("Exception Type: " + ex.GetType().FullName);
            sb.AppendLine("Message: " + ex.Message);
            sb.AppendLine("Source: " + ex.Source);
            foreach (var key in ex.Data.Keys)
            {
                sb.AppendLine(key.ToString() + ": " + ex.Data[key].ToString());
            }

            if (String.IsNullOrEmpty(ex.StackTrace))
            {
                sb.AppendLine("Environment Stack Trace: " + ex.StackTrace);
            }
            else
            {
                sb.AppendLine("Stack Trace: " + ex.StackTrace);
            }
            if (ex.InnerException != null)
            {
                sb.AppendLine("Inner Exception: " + ex.InnerException.ExceptionToString());
            }

            return sb.ToString();
        }
        public static IEnumerable<KeyValuePair<string, T>> PropertiesOfType<T>(object obj)
        {
            return from p in obj.GetType().GetProperties()
                   where p.PropertyType == typeof(T)
                   select new KeyValuePair<string, T>(p.Name, (T)p.GetValue(obj));
        }
        public static string ToNullSafeString(this object obj)
        {
            return (obj ?? string.Empty).ToString();
        }
        public static bool IsValidEmail(this string emailAddress)
        {
            const string pattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

            return Regex.IsMatch(emailAddress, pattern);
        }
        public static bool IsValidEmailAddress(string emailAddress)
        {
            return new System.ComponentModel.DataAnnotations
                                .EmailAddressAttribute()
                                .IsValid(emailAddress);
        }
        public delegate bool TryParseDelegate<T>(string str, out T value);

        public static T? TryParseOrNull<T>(TryParseDelegate<T> parse, string str) where T : struct
        {
            return parse(str, out T value) ? value : (T?)null;
        }

        public static void CreateDir(string path)
        {
            if (!IsExistDir(path)) Directory.CreateDirectory(path);
        }

        public static bool IsExistDir(string path) => Directory.Exists(path);

        //public static bool IsValidHeaderFile(string fileName)
        //{
        //    try
        //    {
        //        string path = $"{AppDomain.CurrentDomain.BaseDirectory}/FileTemplate/";
        //        if (IsExistDir(path)) CreateDir(path);

        //        string sysDir = $"{path}{fileName}";

        //        using var workbook = new XLWorkbook(sysDir);
        //        var worksheet = workbook.Worksheets.FirstOrDefault();
        //        IXLRow iRow = (IXLRow)worksheet.RowsUsed();
        //        string readRange = string.Format("{0}:{1}", 1, iRow.LastCellUsed().Address.ColumnNumber);
        //        foreach (IXLCell cell in iRow.Cells(readRange))
        //        {
        //            dt.Columns.Add(cell.Value.ToString());
        //        }
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //        throw;
        //    }

        //}
        public static T DeepCopy<T>(T other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Context = new StreamingContext(StreamingContextStates.Clone);
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
        public static string RemovePrefixOperation(this string text, string replacePrefixTo = " ")
        {
            string prefixPattern = @"เขต|แขวง|จังหวัด|อำเภอ|ตำบล|ต\.|อ\.|จ\.";
            string replaceSpace = Regex.Replace(text, @"(\s+|@|&|'|\(|\)|<|>|#)", replacePrefixTo);

            replaceSpace = new Regex(prefixPattern).Replace(replaceSpace, "");

            //string pattern = @"^(\S+เขต|แขวง|จังหวัด|อำเภอ|ตำบล|อ\.|ต\.|จ\./g)$";
            //Match match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);
            return replaceSpace;
        }
        public static Dictionary<string, string> IsValidPhone(this string strPhoneInput)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            // Remove symbols (dash, space and parentheses, etc.)
            string strPhone = Regex.Replace(strPhoneInput, @"[- ()\*\!]", String.Empty);

            // Check for exactly 10 numbers left over
            Regex regTenDigits = new Regex(@"^([0|\+[0-9]{1,5})?([7-9][0-9]{9})$");
            Match matTenDigits = regTenDigits.Match(strPhone);
            if (!matTenDigits.Success)
            {
                string[] prefixPhone = new[] { "02", "03", "04", "05", "06" };
                if (!prefixPhone.Contains(strPhone.Substring(0, 2)))
                {
                    keyValuePairs.Add("Phone", "Phone isvalid");
                }
                return keyValuePairs;
            }
            keyValuePairs.Add("Mobile", "Phone isvalid");

            return keyValuePairs;
            //matTenDigits.Success;
        }

        public static string GetFileExtension(string path) => Path.GetExtension(path);

        //public static DataTable ImportExceltoDatatable(Stream stream, string sheetName = null)
        //{
        //    return MapToDataTable(stream);
        //}
        public static T? GetValue<T>(object value) where T : struct
        {
            if (value == null || value is DBNull) return null;
            if (value is T t) return t;
            return (T)Convert.ChangeType(value, typeof(T));
        }
        //public static DataTable MapToDataTable(Stream file)
        //{
        //    // Open the Excel file using ClosedXML.
        //    // Keep in mind the Excel file cannot be open when trying to read it
        //    using XLWorkbook workBook = new XLWorkbook(file);
        //    //Read the first Sheet from Excel file.
        //    IXLWorksheet workSheet = workBook.Worksheet(1);

        //    //Create a new DataTable.
        //    DataTable dt = new DataTable();

        //    //Loop through the Worksheet rows.
        //    bool firstRow = true;
        //    foreach (IXLRow row in workSheet.RowsUsed())
        //    {
        //        //Use the first row to add columns to DataTable.
        //        if (firstRow)
        //        {
        //            foreach (IXLCell cell in row.Cells())
        //            {
        //                DataColumn cols = new DataColumn
        //                {
        //                    ColumnName = !string.IsNullOrEmpty(cell.Value.ToString()) ? cell.Value.ToString() : cell.Address.ColumnLetter.ToString(),
        //                    Prefix = cell.Address.ColumnLetter?.ToString(),
        //                    Caption = $"{cell.Address.ColumnLetter.ToNullSafeString()} {cell.Value}",

        //                };
        //                dt.Columns.Add(cols);
        //                // Name = 
        //                //dt.Columns.Add(cell.Value.ToString());
        //            }
        //            firstRow = false;
        //        }
        //        else
        //        {
        //            //Add rows to DataTable.
        //            dt.Rows.Add();
        //            int i = 0;
        //            try
        //            {
        //                var firstRowCell = row.FirstCellUsed().Address?.ColumnNumber;
        //                var lastRowCell = row.LastCellUsed().Address?.ColumnNumber;
        //                if (firstRowCell.HasValue && lastRowCell.HasValue)
        //                    foreach (IXLCell cell in row.Cells(firstRowCell.Value, lastRowCell.Value))
        //                    {
        //                        dt.Rows[^1][i] = cell.Value.ToString();
        //                        i++;
        //                    }
        //            }
        //            catch (Exception)
        //            {

        //                throw;
        //            }

        //        }
        //    }

        //    return dt;
        //}
        public static DataTable ImportExceltoDatatable(string filePath, string sheetNamesheetName = null)
        {
            // return MapToDataTable(filePath);
            return new DataTable();
        }
        public static string StripPrefix(this string text, string prefix)
        {
            return text.StartsWith(prefix) ? text.Substring(prefix.Length) : text;
        }
        public static string RemoveDigits(string key)
        {
            return Regex.Replace(key, @"\d", "");
        }

        public static decimal PercentCalculation(decimal current, double percent) => (current / 100) * (decimal)percent;
        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
                return min;
            if (value.CompareTo(max) > 0)
                return max;

            return value;
        }

        public static T ToObject<T>(this JsonElement element, JsonSerializerOptions options = null)
        {
            var bufferWriter = new ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(bufferWriter))
                element.WriteTo(writer);
            return System.Text.Json.JsonSerializer.Deserialize<T>(bufferWriter.WrittenSpan, options);
        }

        public static T ToObject<T>(this JsonDocument document, JsonSerializerOptions options = null)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));
            return document.RootElement.ToObject<T>(options);
        }

        public static string RemoveSpecialCharecter(this string input)
        {
            var pattern = "[\\~#%&*{}/:<>?|\" -]";
            var regexItem = new Regex(pattern);
            return regexItem.Replace(input, "");
        }
        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new();
            dataTable.TableName = typeof(T).FullName;
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
}
