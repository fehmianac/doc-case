namespace Slot.Gateway.Service.Json
{
    public interface ISerializationService
    {
        T Deserialize<T>(string input);

        string Serialize(object input);
    }
}