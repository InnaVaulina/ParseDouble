
using ConsoleApp1;



Console.Write("Введите число: ");
string input = Console.ReadLine();

MyParser parser = new MyParser();
double result = parser.Parse(input);

Console.WriteLine(parser.ErrorMessage);
Console.Write("Результат: ");
Console.WriteLine(result);

