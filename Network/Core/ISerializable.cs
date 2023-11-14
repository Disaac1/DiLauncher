
public interface ISerializable
{
    abstract string Serialize();
    abstract object Deserialize(string jsonString);


    public static string Serialize<L>(L data)
    {
        if(typeof(L).IsAssignableTo(typeof(string)))
        {
            return (string) (object) data;
        } else if (typeof(L).IsAssignableFrom(typeof(ISerializable)))
        {
            return ((ISerializable) data).Serialize();
        } else {
            GodotLogger.error("Invalid Type for RequestEvent");
            return "";
        }
    }

    public static L Deserialize<L>(string data)
    {
        if(typeof(L).IsAssignableTo(typeof(string)))
        {
            return (L) (object) data;
        } else if (typeof(L).IsAssignableTo(typeof(ISerializable)))
        {
            return (L) ((ISerializable) typeof(L)).Deserialize(data);
        } else {
            GodotLogger.error("Invalid Type for RequestEvent");
            return default(L);
        }
    }
}
