using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;

namespace CsvApp.Service.Helpers.Converters
{
    public class CustomBooleanConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                // Handle empty or whitespace strings as false
                return false;
            }

            if (bool.TryParse(text, out bool result))
            {
                return result;
            }

            // Case-insensitive check for 'True' or 'False'
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
