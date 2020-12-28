using System;

namespace sudoku_solver
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("===========");
      try
      {
        Console.WriteLine(Parser.Parse("board-3.txt").Solve().ToString(false));
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      Console.WriteLine("===========");
      // Console.ReadLine();
    }
  }
}
