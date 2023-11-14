public struct StringSerial : ISerializable
{

    string data;

    public object Deserialize(string jsonString)
    {
        return jsonString;
    }

    public string Serialize()
    {
        return data;
    }
}