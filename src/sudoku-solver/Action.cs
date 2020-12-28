
using System;

namespace sudoku_solver
{
  public class Action
  {
    private Box _box;

    private int _value;

    public Box Box
    {
      get {
        return _box;
      }
    }

    public int Value
    {
      get {
        return _value;
      }
    }

    public Action(Box box, int value)
    {
      if (box == null)
        throw new ArgumentNullException(nameof(box));

      if (value < 1 || value > Board.Size)
        throw new ArgumentException(nameof(value));

      _box = box;
      _value = value;
    }
  }

}