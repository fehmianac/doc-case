using System.Text.Json;

namespace Slot.Gateway.Service.Json
{
    public class SerializationService : ISerializationService
    {
        public T Deserialize<T>(string input)
        {
            return JsonSerializer.Deserialize<T>(input);
        }

        public string Serialize(object input)
        {
            return JsonSerializer.Serialize(input);
        }
    }
}
