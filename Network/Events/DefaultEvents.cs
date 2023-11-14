public class DefaultEvents : IEventRegistry
{
    public static readonly Event<string> STATUS = new("status");

    public static readonly RequestEvent<string, Player> LOGIN = new("login");

    
    static DefaultEvents()
    {
        
    }

    public static void Init()
    {
        GodotLogger.info("Initializing Default Events");
    }
}