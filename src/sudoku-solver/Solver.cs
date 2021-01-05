using System;
using System.Collections.Generic;
using System.Linq;

namespace sudoku_solver
{
  internal class Solver
  {
    private readonly Queue<Action> _actions;
    private readonly Board _board;

    public Solver(Board board)
    {
      _board = board;
      _actions = new Queue<Action>();
    }

    public void Add(Action action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof(action));

      if (_actions.Any(a => a.Equals(action.Box)))
        return;

      _actions.Enqueue(action);
    }

    public Board Solve()
    {
      while (_actions.Any())
      {
        //first step: Set value in specified box and remove this possible value from other box of square, line and column
        var currentAction = _actions.Dequeue();
        ApplyValueInBox(currentAction.Value, currentAction.Box);

        //second step: for each section (square, line, column), check if a value is possible only in one box
        if (_actions.Count() == 0)
          CheckValueInSingleBoxBySquare();
        if (_actions.Count() == 0)
          CheckValueInSingleBoxByLine();
        if (_actions.Count() == 0)
          CheckValueInSingleBoxByColumn();

        //third step: check that value is possible only in a same line (or column) of the current square
        if (_actions.Count() == 0)
        {
          var boxesBySquare = _board.ListBoxesInAllSquares();
          foreach (var boxesInSquare in boxesBySquare)
          {
            CheckValueIsInSameLineOrColumnWithinSquare(boxesInSquare);
          }
        }
      }

      return _board;
    }

    /// <summary>
    /// Set a value in a box.
    /// Remove this value in other boxes (excluded those that value is found) of same square/line/column.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="box"></param>
    private void ApplyValueInBox(int value, Box box)
    {
      _board.SetValueInBox(value, box);

      RemoveValueInBoxes(value, _board.ListBoxesInLine(box));
      RemoveValueInBoxes(value, _board.ListBoxesInColumn(box));
      RemoveValueInBoxes(value, _board.ListBoxesInSquare(box));
    }

    private void CheckValueInSingleBoxBySquare()
    {
      for (var line = 1; line <= Board.Size; line += 2)
      {
        for (var column = 1; column <= Board.Size; column += 2)
        {
          CheckValueInSingleBox(_board.ListBoxesInSquare(new Box(line, column)));
        }
      }
    }

    private void CheckValueInSingleBoxByLine()
    {
      for (var pos = 1; pos <= Board.Size; pos++)
      {
        CheckValueInSingleBox(_board.ListBoxesInLine(new Box(pos, pos)));
      }
    }

    private void CheckValueInSingleBoxByColumn()
    {
      for (var pos = 1; pos <= Board.Size; pos++)
      {
        CheckValueInSingleBox(_board.ListBoxesInColumn(new Box(pos, pos)));
      }
    }

    private void CheckValueIsInSameLineOrColumnWithinSquare(IEnumerable<Box> boxesInSquare)
    {     
      foreach (var groupedBoxes in ListBoxesByValue(boxesInSquare))
      {
        var boxes = groupedBoxes.Value;
        var value = groupedBoxes.Key;

        if (boxes.Count() == 1)
        {
          //The case with one possible box should not happen as it is already managed before.
          Add(new Action(boxes.First(), value));
        }
        else
        {
          var boxesWhereToRemoveValue = new List<Box>();

          if (boxes.GroupBy(box => box.Line).Count() == 1)
          {
            //list boxes in same line where the value has to be removed
            boxesWhereToRemoveValue = _board.ListBoxesInLine(boxes.First())
                .Where(box => !boxes.Any(e => e.Equals(box)))
                .ToList();
          }
          else if (boxes.GroupBy(c => c.Column).Count() == 1)
          {
            //list boxes in same column where the value has to be removed           
            boxesWhereToRemoveValue = _board.ListBoxesInColumn(boxes.First())
                .Where(box => !boxes.Any(e => e.Equals(box)))
                .ToList();
          }

          if (boxesWhereToRemoveValue.Count > 0)
          {
            //remove value from other boxes of the same column ...
            RemoveValueInBoxes(value, boxesWhereToRemoveValue);

            //... and check if removed value in these boxes affect the unicity of the value in their own square
            boxesWhereToRemoveValue.ForEach(box => CheckValueInSingleBox(_board.ListBoxesInSquare(box)));       
          }
        }
      }
    }

    private Dictionary<int, HashSet<Box>> ListBoxesByValue(IEnumerable<Box> boxes)
    {
      var boxesByValue = new Dictionary<int, HashSet<Box>>();

      //determine the list of possible boxes for each value (and thus exclude boxes with an unique value)
      foreach (var box in boxes)
      {
        var values = _board.ListValuesInBox(box);
        if (values.Count() > 1)
        {
          foreach (var value in values)
          {
            if (!boxesByValue.ContainsKey(value))
              boxesByValue.Add(value, new HashSet<Box> { box });
            else
              boxesByValue[value].Add(box);
          }
        }
      }

      return boxesByValue;
    }

    private void CheckValueInSingleBox(IEnumerable<Box> boxes)
    {
      //check if a value is unique in the list of boxes with multiple values
      //In this box, add an action
      for (var value = 1; value <= Board.Size; value++)
      {
        var filteredBoxes = _board.FilterBoxesContainingValue(boxes, value);
        if (filteredBoxes.Count() == 1 && _board.ListValuesInBox(filteredBoxes[0]).Count() > 1)
        {
          Add(new Action(filteredBoxes.First(), value));
        }
      }
    }

    private void RemoveValueInBoxes(int value, IEnumerable<Box> boxes)
    {
      foreach(var box in boxes)
      {
        RemoveValueInBox(value, box);
      }
    }

    /// <summary>
    /// Remove value in box if there are two or more possibel values.
    /// After that, add an action if box stay with only one value possible.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="boxes"></param>
    private void RemoveValueInBox(int value, Box box)
    {
      var values = _board.ListValuesInBox(box);
      if (values.Count() > 1)
      {
        _board.RemoveValueInBox(value, box);

        var valuesLeft = _board.ListValuesInBox(box);
        if (valuesLeft.Count() == 1)
          Add(new Action(box, valuesLeft.First()));
      }
    }
  }
}