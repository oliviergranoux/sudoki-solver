using System;
using System.Collections.Generic;
using System.Linq;

namespace sudoku_solver
{
  public class Board
  {
    private HashSet<int>[,] _board;

    public const int Size = 9;

    public Board()
    {
      InitializeBoard();
    }

    private void InitializeBoard()
    {
      _board = new HashSet<int>[Size, Size];

      var values = new List<int>(Size);
      for (var value = 1; value <= Size; value++)
      {
        values.Add(value);
      }

      for (var lineIdx = 0; lineIdx < Size; lineIdx++)
      {
        for (var columnIdx = 0; columnIdx < Size; columnIdx++)
        {
          _board[lineIdx, columnIdx] = new HashSet<int>(values);
        }
      }
    }

    /// <summary>
    /// List the possible values for the specified box
    /// </summary>
    /// <param name="box"></param>
    /// <returns>the list of values</returns>
    /// <exception cref="System.ArgumentNullException">the parameter box is null</exception>
    public IEnumerable<int> ListValuesInBox(Box box)
    {
      if (box == null)
        throw new ArgumentNullException(nameof(box));

      return _board[box.Line - 1, box.Column - 1];
    }

    /// <summary>
    /// Set a unique value for the specified box
    /// </summary>
    /// <param name="value"></param>
    /// <param name="box"></param>
    /// <exception cref="System.ArgumentException">value is not in the range of the board size</exception>
    /// <exception cref="System.ArgumentNullException">the parameter box is null</exception>
    public void SetValueInBox(int value, Box box)
    {
      if (value < 1 || value > Size)
        throw new ArgumentException(nameof(value));

      if (box == null)
        throw new ArgumentNullException(nameof(box));

      _board[box.Line - 1, box.Column - 1] = new HashSet<int> { value };
    }

    /// <summary>
    /// Remove a value for the specified box
    /// </summary>
    /// <param name="value"></param>
    /// <param name="box"></param>
    /// <exception cref="System.ArgumentException">value is not in the range of the board size</exception>
    /// <exception cref="System.ArgumentNullException">the parameter box is null</exception>
    public void RemoveValueInBox(int value, Box box)
    {
      if (value < 1 || value > Size)
        throw new ArgumentException(nameof(value));

      if (box == null)
        throw new ArgumentNullException(nameof(box));

      _board[box.Line - 1, box.Column - 1].Remove(value);
    }

    public IEnumerable<Box> ListBoxesInSquare(Box box)
    {
      if (box == null)
        throw new ArgumentNullException(nameof(box));

      //get position of the first box of the square
      var line = ((box.Line - 1) / 3) * 3 + 1;
      var column = ((box.Column - 1) / 3) * 3 + 1;
      var boxes = new HashSet<Box>();

      for (var i = line; i <= line + 2; i++)
      {
        for (var j = column; j <= column + 2; j++)
        {
          boxes.Add(new Box(i, j));
        }
      }

      return boxes;
    }

    public IList<IEnumerable<Box>> ListBoxesInAllSquares()
    {
      var boxes = new List<IEnumerable<Box>>();

      for (var line = 1; line <= Size; line += 3)
      {
        for (var column = 1; column <= Size; column += 3)
        {
          boxes.Add(ListBoxesInSquare(new Box(line, column)));
        }
      }
      return boxes;
    }

    public IList<Box> ListBoxesInLine(Box box)
    {
      if (box == null)
        throw new ArgumentNullException(nameof(box));

      var boxes = new List<Box>();

      for (var column = 1; column <= Size; column++)
      {
        boxes.Add(new Box(box.Line, column));
      }

      return boxes;
    }

    public IList<Box> ListBoxesInColumn(Box box)
    {
      if (box == null)
        throw new ArgumentNullException(nameof(box));

      var boxes = new List<Box>();

      for (var line = 1; line <= Size; line++)
      {
        boxes.Add(new Box(line, box.Column));
      }

      return boxes;
    }

    public IList<Box> FilterBoxesContainingValue(IEnumerable<Box> boxes, int value)
    {
      if (boxes == null)
        throw new ArgumentNullException(nameof(boxes));

      return boxes
          .Where(box => box != null)
          .Where(box => ListValuesInBox(box).Contains(value))
          .ToList();
    }

    public string ToString(bool displayCount = false)
    {
      var lines = new string[Size];

      for (var line = 1; line <= Size; line++)
      {
        var currentLine = string.Empty;

        for (var column = 1; column <= Size; column++)
        {
          if (column != 1 && column % 3 == 1)
            currentLine += " ";

          var currentBox = _board[line - 1, column - 1];
          if (displayCount)
          {
            currentLine += currentBox.Count();
          }
          else
          {
            currentLine += (currentBox.Count() == 1)
                ? currentBox.First()
                : "-";
          }
        }

        lines[line - 1] = (line != 1 && line % 3 == 1)
          ? Environment.NewLine + currentLine
          : currentLine;
      }

      return string.Join(Environment.NewLine, lines);
    }
  }

}