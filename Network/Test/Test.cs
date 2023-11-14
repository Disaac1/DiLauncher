using System;
using System.Linq;
using Godot;

public partial class Test : Node
{

	// This will run through some example code to make sure the Event system is functional and healthy

	public override void _Ready()
	{
		GD.Print("EventTester Ready");
		run();
	}



	public static void run()
	{
		selfTests();
		RemoteTests();


	}


	public static void RemoteTests()
	{
		rTest1();
	}

	public static void rTest1()
	{
		TestEvent<string>.PassSent.Pass("test", "Hello World!");
		Registry.GetEvent("TestEvent")._send("test", "Hello World!");
	}


	public static void selfTests()
	{
		sTest1();
		sTest2();
		sTest3();
	}

	public static void sTest3()
	{

		TestRequestEvent<string, string> testRequestEvent = new("TestRequestEvent",
		(data) => {

		},
		(resp) => {
			GodotLogger.info(resp);
		});

		testRequestEvent.add((data) => {
			return "Response";
		});

		testRequestEvent._emit("Hello World!", "1234");


	}

	public static void sTest2()
	{
		//Send "Hello World!" to the TestEvent Event through the registry
		//Normally, this wouldn't be called by the user
		Registry.GetEvent("TestEvent")._emit("Hello World!", "");

	}

	public static void sTest1()
	{
		// Test 1: Send "Hello World!" to the TestEvent Event
		// Expected Result: Event should be sent, but to no listeners, so nothing should happen

		TestEvent<string> e = new TestEvent<string>((data) =>
		{
			
			assert(data == "Hello World!", "TestEvent<string> failed to send data correctly");


		}, "TestEvent");



		e.Emit("Hello World!");

	}

	public static void assert(bool condition, string message)
	{
		if (!condition)
		{
			GodotLogger.error("Assertion Failed: " + message);
		}
	}


}
