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

    private void RemoveValueInBoxes(int value, IEnumerable<Box> boxes)
    {
      foreach (var box in boxes)
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

    private void CheckUniqueValueWithinBoxes(IEnumerable<Box> boxes)
    {
      //check if a value is unique in the list of boxes with multiple values
      //In this box, add an action
      for (var value = 1; value <= 9; value++)
      {
        var filteredBoxes = _board.FilterBoxesContainingValue(boxes, value);
        if (filteredBoxes.Count() == 1 && _board.ListValuesInBox(filteredBoxes.First()).Count() > 1)
        {
          Add(new Action(filteredBoxes.First(), value));
        }
      }
    }

    public Board Solve()
    {
      while (_actions.Any())
      {
        //basic logic
        {
          var currentAction = _actions.Dequeue();

          _board.SetValueInBox(currentAction.Value, currentAction.Box);

          var boxes = new HashSet<Box>();
          boxes.UnionWith(_board.ListBoxesInLine(currentAction.Box));
          boxes.UnionWith(_board.ListBoxesInColumn(currentAction.Box));
          boxes.UnionWith(_board.ListBoxesInSquare(currentAction.Box));
          RemoveValueInBoxes(currentAction.Value, boxes);
        }
        
        //CheckUniqueValueWithinBoxes - Squares
        if (_actions.Count() == 0)
        {
          var tempBoxes = new List<Box> {
            new Box(2, 2), new Box(2, 5), new Box(2, 8),
            new Box(5, 2), new Box(5, 5), new Box(5, 8),
            new Box(8, 2), new Box(8, 5), new Box(8, 8)
          };

          foreach (var box in tempBoxes)
          {
            CheckUniqueValueWithinBoxes(_board.ListBoxesInSquare(box));
          }
        }

        //CheckUniqueValueWithinBoxes - Lines & Columns
        if (_actions.Count() == 0)
        {
          var tempBoxes = new List<Box> {
            new Box(1, 1), new Box(2, 2), new Box(3, 3),
            new Box(4, 4), new Box(5, 5), new Box(6, 6),
            new Box(7, 7), new Box(8, 8), new Box(9, 9)
          };

          foreach (var box in tempBoxes)
          {
            CheckUniqueValueWithinBoxes(_board.ListBoxesInLine(box));
            CheckUniqueValueWithinBoxes(_board.ListBoxesInColumn(box));
          }
        }

        //...
        if (_actions.Count() == 0)
        {
          var tempBoxes = new List<Box> {
            new Box(2, 2), new Box(2, 5), new Box(2, 8),
            new Box(5, 2), new Box(5, 5), new Box(5, 8),
            new Box(8, 2), new Box(8, 5), new Box(8, 8)
          };
          
          //fill the dictionary
          foreach(var boxesBySquare in _board.ListBoxesInAllSquares())
          {
            var boxesByValue = new Dictionary<int, HashSet<Box>>();

            // var boxesInTheSquare = _board.ListBoxesInSquare(tempBox);
            foreach (var box in boxesBySquare)
            {
              //fill the dictionary
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

            foreach (var groupedBoxes in boxesByValue)
            {
              var boxes = groupedBoxes.Value;
              var value = groupedBoxes.Key;

              if (boxes.Count() == 1)
              {
                Add(new Action(boxes.First(), value));
              }
              else
              {
                //boxes are on the same line
                if (boxes.GroupBy(box => box.Line).Count() == 1)
                {
                  //remove from line in other squares
                  var boxesToRemove = _board.ListBoxesInLine(boxes.First())
                      .Where(box => !boxes.Any(e => e.Equals(box)))
                      .ToList();
                  RemoveValueInBoxes(value, boxesToRemove);

                  boxesToRemove.ForEach(box => CheckUniqueValueWithinBoxes(_board.ListBoxesInSquare(box)));
                }
                //cases are on the same column
                else if (boxes.GroupBy(c => c.Column).Count() == 1)
                {
                  //remove from column in other squares
                  var boxesToRemove = _board.ListBoxesInColumn(boxes.First())
                      .Where(box => !boxes.Any(e => e.Equals(box)))
                      .ToList();
                  RemoveValueInBoxes(value, boxesToRemove);

                  boxesToRemove.ForEach(box => CheckUniqueValueWithinBoxes(_board.ListBoxesInSquare(box)));
                }
              }
            }
          }
        }
      }

      return _board;
    }

  }
}