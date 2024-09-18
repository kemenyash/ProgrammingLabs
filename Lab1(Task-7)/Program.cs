Console.WriteLine("Task #1\r\n");


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

Console.WriteLine($"\r\n{new string('#', 30)}");

Console.WriteLine("Task #2\r\n");

Stack<int> numbers = new Stack<int>();

numbers.Push(-9);
numbers.Push(2);
numbers.Push(3);
numbers.Push(7);
numbers.Push(8);
numbers.Push(-3);
numbers.Push(9);

var positiveNumbers = numbers
    .Where(x => x > 0 && x % 2 == 0)
    .Sum();

if (positiveNumbers > 0)
{
    Console.WriteLine($"Positive total amount: {positiveNumbers}");
}
else
{
    Console.WriteLine("Positive numbers not found.");
}
