using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace sudoku_solver
{
  public class Parser
  {
    internal static Solver Parse(string fileName)
    {
      if (!File.Exists(fileName))
        throw new ArgumentException($"The file '{fileName}' does not exist");

      var board = new Board();
      var solver = new Solver(board);


      string line;
      var lineNumber = 0;
      using (StreamReader reader = File.OpenText(fileName))
      {
        while (lineNumber < Board.Size && (line = reader.ReadLine()) != null)
        {
          if (!string.IsNullOrWhiteSpace(line) && line.TrimStart()[0] != '#')
          {
            lineNumber++;
            ParseLine(lineNumber, line, solver);
          }
        }
      }

      return solver;
    }

    internal static void ParseLine(int lineNumber, string line, Solver solver)
    {
      var characters = Regex.Replace(line, @"\s+", string.Empty).ToCharArray();
      if (characters.Count() != Board.Size)
        throw new Exception(nameof(characters));

      for (var i = 0; i < Board.Size; i++)
      {
        var value = (int)char.GetNumericValue(characters[i]);
        if (0 <= value && value <= 9)
        {
          solver.Add(new Action(new Box(lineNumber, i + 1), value));
        }
      }
    }
  }
}