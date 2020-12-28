using System;

namespace sudoku_solver
{
  public class Box
  {
    private int _line;
    private int _column;

    public int Line
    {
      get {
        return _line;
      }
    }

    public int Column
    {
      get {
        return _column;
      }
    }

    /// <param name="line"></param>
    /// <param name="column"></param>
    /// <exception cref="System.ArgumentException">line or column are not in the range of board size</exception>
    /// <see cref="Board.Size"/>
    public Box(int line, int column)
    {
      if (line < 1 || Board.Size < line)
        throw new ArgumentException(nameof(line));
      if (column < 1 || Board.Size < column)
        throw new ArgumentException(nameof(column));

      _line = line;
      _column = column;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(Line, Column);
    }
  }
}
