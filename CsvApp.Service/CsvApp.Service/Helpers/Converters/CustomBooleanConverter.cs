using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;

namespace CsvApp.Service.Helpers.Converters
{
    public class CustomBooleanConverter : DefaultTypeConverter
    {
        /// <summary>
        /// Custom Converter for string "bool" to bool
        /// </summary>
        /// <param name="text"></param>
        /// <param name="row"></param>
        /// <param name="memberMapData"></param>
        /// <returns></returns>
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                
                return false;
            }

            if (bool.TryParse(text, out bool result))
            {
                return result;
            }

            
            if (text.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else if (text.Equals("false", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return base.ConvertFromString(text, row, memberMapData);
        }
    }
}
