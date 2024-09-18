Stack<string> operationSystems = new Stack<string>();
operationSystems.Push("Windows");
operationSystems.Push("Linux");
operationSystems.Push("macOS");
operationSystems.Push("Ubuntu");
operationSystems.Push("Fedora");

Console.WriteLine("Adding OS order:");
operationSystems.Reverse().ToList().ForEach(os => Console.WriteLine(os));

Console.WriteLine("\nReverse OS order:");
operationSystems.ToList().ForEach(os => Console.WriteLine(os));

Console.WriteLine($"\nOS count: {operationSystems.Count()}");

operationSystems.Clear();
Console.WriteLine("\nOS stack was cleared");
