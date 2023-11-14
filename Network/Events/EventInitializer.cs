using System;

public static class EventInitializer
{
        public static void InitializeAllEvents()
    {
            // Initialize all events here
            DefaultEvents.Init();
            RoomEvents.Init();

            GodotLogger.info("Initialized Events"); 
     }
}
