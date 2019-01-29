using Newtonsoft.Json.Converters;

namespace MicwayTech.Domain.Helper
{
    /// <summary>
    /// Help class to convert data to the dd/MM/yyyy format
    /// </summary>
    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            base.DateTimeFormat = "dd/MM/yyyy";
        }
    }
}
