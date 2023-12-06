using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;

namespace CsvApp.Service.Helpers.Converters
{
    public class CustomInt32Converter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (int.TryParse(text, out int result))
            {
                return result;
            }

            if (text.Equals("None", StringComparison.OrdinalIgnoreCase))
            {
                return 0;
            }

            return base.ConvertFromString(text, row, memberMapData);
        }
    }
}
