using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace sudoku_solver.Tests
{
    public class BoardTests
    {
        [Test]
        public void Test_Board_10_ListValuesInBox_NullBox()
        {
            //Arrange
            Box box = null;

            //Act
            var action = new TestDelegate(() => new Board().ListValuesInBox(box));

            //Assert
            var exception = Assert.Throws<ArgumentNullException>(action);
            Assert.IsNotNull(exception);
            StringAssert.Contains(nameof(box), exception.Message);
        }

        [Test]
        public void Test_Board_11_InitializeBoard()
        {
            //Act
            var board = new Board();

            //Assert
            for (var line = 1; line <= Board.Size; line++)
            {
                for (var column = 1; column <= Board.Size; column++)
                {
                    var values = board.ListValuesInBox(new Box(line, column));
                    
                    Assert.IsNotNull(values);
                    Assert.That(values.Count(), Is.EqualTo(Board.Size));
                    Assert.That(values.Distinct().Count(), Is.EqualTo(Board.Size));

                    Assert.That(values.Min(), Is.EqualTo(1));
                    Assert.That(values.Max(), Is.EqualTo(Board.Size));
                }
            }        
        }

        [Test]
        [TestCase(0)]
        [TestCase(Board.Size + 1)]
        public void Test_Board_14_SetValueInBox_InvalidValue(int value)
        {
            //Act
            var action = new TestDelegate(() => new Board().SetValueInBox(value, new Box(1,1)));

            //Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.IsNotNull(exception);
            Assert.That(exception.Message, Is.EqualTo(nameof(value)));
        }

        [Test]
        public void Test_Board_15_SetValueInBox_NullBox()
        {
            //Arrange
            Box box = null;

            //Act
            var action = new TestDelegate(() => new Board().SetValueInBox(1, box));

            //Assert
            var exception = Assert.Throws<ArgumentNullException>(action);
            Assert.IsNotNull(exception);
            StringAssert.Contains(nameof(Box).ToLower(), exception.Message);
        }

        [Test]
        public void Test_Board_17_SetValueInBox()
        {
            //Arrange
            var board = new Board();
            var box = new Box(2, 3);

            //Act
            board.SetValueInBox(1, box);

            //Assert
            AssertDefaultBoardAndSpecificBox(board, 
                box, 
                (valuesOfBox) => {
                    Assert.That(valuesOfBox.Count(), Is.EqualTo(1));
                    Assert.That(valuesOfBox.First(), Is.EqualTo(1));
                });
        }
    
        [Test]
        [TestCase(0)]
        [TestCase(Board.Size + 1)]
        public void Test_Board_20_RemoveValueInBox_InvalidValue(int value)
        {
            //Act
            var action = new TestDelegate(() => { new Board().RemoveValueInBox(value, new Box(1, 1)); });

            //Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.IsNotNull(exception);
            Assert.That(exception.Message, Is.EqualTo(nameof(value)));
        }

        [Test]
        public void Test_Board_21_RemoveValueInBox_NullBox()
        {
            //Arrange
            Box box = null;
            
            //Act
            var action = new TestDelegate(() => { new Board().RemoveValueInBox(1, box); });

            //Assert
            var exception = Assert.Throws<ArgumentNullException>(action);
            Assert.IsNotNull(exception);
            StringAssert.Contains(nameof(box), exception.Message);
        }

        [Test]
        public void Test_Board_23_RemoveValueInBox()
        {
            //Arrange
            var board = new Board();
            var box = new Box(2, 3);

            //Act
            board.RemoveValueInBox(1, box);

            //Assert
            AssertDefaultBoardAndSpecificBox(board, 
                box, 
                (valuesOfBox) => {
                    Assert.That(valuesOfBox.Count(), Is.EqualTo(Board.Size - 1));
                    Assert.That(valuesOfBox.All(v => v != 1));
                });
        }

        [Test]
        public void Test_Board_26_ListBoxesInSquare_NullBox()
        {
            //Arrange
            Box box = null;

            //Act
            var action = new TestDelegate(() => new Board().ListValuesInBox(box));

            //Assert
            var exception = Assert.Throws<ArgumentNullException>(action);
            Assert.IsNotNull(exception);
            StringAssert.Contains(nameof(Box).ToLower(), exception.Message);
        }

        [Test]
        public void Test_Board_28_ListBoxesInSquare()
        {
            //Arrange - part 1
            var board = new Board();

            for (var line = 1; line <= Board.Size; line++)
            {
                for (var column = 1; column <= Board.Size; column++)
                {
                    //Arrange - part 2
                    var lines = ListPositionsOfSquare(line);
                    var columns = ListPositionsOfSquare(column);           
            
                    //Act
                    var boxesInSquare = board.ListBoxesInSquare(new Box(line, column));

                    //Assert
                    Assert.IsNotNull(boxesInSquare);
                    Assert.That(boxesInSquare.Count, Is.EqualTo(Board.Size));
                    foreach(var b in boxesInSquare)
                    {
                        Assert.That(b.Line, Is.InRange(lines.Min(), lines.Max()), $"Invalid line {b.Line} for {line}x{column} [{lines.Min()}; {lines.Max()}]");
                        Assert.That(b.Column, Is.InRange(columns.Min(), columns.Max()), $"Invalid columns {b.Column} for {line}x{column} [{lines.Min()}; {lines.Max()}]");
                    }
                }
            }
        }

        [Test]
        public void Test_Board_30_ListBoxesInAllSquares()
        {
            //Act
            var squares = new Board().ListBoxesInAllSquares();

            //Assert
            Assert.IsNotNull(squares);
            Assert.That(squares.Count(), Is.EqualTo(Board.Size));
        }

        [Test]
        public void Test_Board_33_ListBoxesInLine_NullBox()
        {
            //Arrange
            Box box = null;

            //Act
            var action = new TestDelegate(() => new Board().ListBoxesInLine(box));

            //Assert
            var exception = Assert.Throws<ArgumentNullException>(action);
            Assert.IsNotNull(exception);
            StringAssert.Contains(nameof(Box).ToLower(), exception.Message);
        }

        [Test]
        public void Test_Board_35_ListBoxesInLine()
        {
            //Arrange
            var board = new Board();

            for (var line = 1; line <= Board.Size; line++)
            {
                for (var column = 1; column <= Board.Size; column++)
                {
                    //Act
                    var boxesInLine = board.ListBoxesInLine(new Box(line, column));

                    //Assert
                    Assert.IsNotNull(boxesInLine);
                    Assert.That(boxesInLine.Count, Is.EqualTo(Board.Size));
                    foreach(var b in boxesInLine)
                    {
                        Assert.That(b.Line, Is.EqualTo(line), $"Invalid line {b.Line} for {line}x{column}");
                        Assert.That(b.Column, Is.InRange(1, Board.Size), $"Invalid column {b.Column} for {line}x{column}");
                    }
                }
            }
        }

        [Test]
        public void Test_Board_38_ListBoxesInColumn_NullBox()
        {
            //Arrange
            Box box = null;

            //Act
            var action = new TestDelegate(() => new Board().ListBoxesInColumn(box));

            //Assert
            var exception = Assert.Throws<ArgumentNullException>(action);
            Assert.IsNotNull(exception);
            StringAssert.Contains(nameof(Box).ToLower(), exception.Message);
        }

        [Test]
        public void Test_Board_40_ListBoxesInColumn()
        {
            //Arrange
            var board = new Board();

            for (var line = 1; line <= Board.Size; line++)
            {
                for (var column = 1; column <= Board.Size; column++)
                {
                    //Act
                    var boxesInColumn = board.ListBoxesInColumn(new Box(line, column));

                    //Assert
                    Assert.IsNotNull(boxesInColumn);
                    Assert.That(boxesInColumn.Count, Is.EqualTo(Board.Size));
                    foreach(var b in boxesInColumn)
                    {
                        Assert.That(b.Line, Is.InRange(1, Board.Size), $"Invalid line {b.Line} for {line}x{column}");
                        Assert.That(b.Column, Is.EqualTo(column), $"Invalid column {b.Column} for {line}x{column}");
                    }
                }
            }
        }

        [Test]
        public void Test_Board_43_FilterBoxesContainingValue_NullListOfBoxes()
        {
            //Arrange
            IEnumerable<Box> boxes = null;

            //Act
            var action = new TestDelegate(() => new Board().FilterBoxesContainingValue(boxes, 1));

            //Assert
            var exception = Assert.Throws<ArgumentNullException>(action);
            Assert.IsNotNull(exception);
            StringAssert.Contains(nameof(boxes), exception.Message);
        }

        [Test]
        public void Test_Board_44_FilterBoxesContainingValue_ListsWithNullBoxes()
        {
            //Arrange
            var boxes = new List<Box>() { null };

            //Act
            var filteredBoxes = new Board().FilterBoxesContainingValue(boxes, 1);

            //Assert
            Assert.IsNotNull(filteredBoxes);
            Assert.IsEmpty(filteredBoxes);
        }

        [Test]
        [TestCase(0)]
        [TestCase(Board.Size + 1)]
        public void Test_Board_45_FilterBoxesContainingValue_InvalidValue(int value)
        {
            //Act
            var filteredBoxes = new Board().FilterBoxesContainingValue(new List<Box>() { new Box(1, 1) }, value);

            //Assert
            Assert.IsNotNull(filteredBoxes);
            Assert.IsEmpty(filteredBoxes);
        }

        [Test]
        public void Test_Board_47_FilterBoxesContainingValue_InvalidValue()
        {
            //Arrange
            var board = new Board();
            board.SetValueInBox(1, new Box(1, 1));
            board.RemoveValueInBox(1, new Box(2, 2));

            var boxesToCheck = new List<Box>() {
                new Box(1, 1),
                new Box(2, 2),
                new Box(3, 3)
            };

            //Act
            var filteredBoxes = board.FilterBoxesContainingValue(boxesToCheck, 1);

            //Assert
            Assert.IsNotNull(filteredBoxes);
            Assert.That(filteredBoxes.Count(), Is.EqualTo(2));
            Assert.IsFalse(filteredBoxes.Select(b => b.Line).Any(line => line == 2));
        }

        private int[] ListPositionsOfSquare(int positionOfBox)
        {
            if (positionOfBox <= 3)
                return new int[3]{1, 2, 3};

            if (positionOfBox >= 7)
                return new int[3]{7, 8, 9};

            return new int[3]{4, 5, 6};    
        }
        private void AssertDefaultBoardAndSpecificBox(Board board, Box boxToAssert, Action<IEnumerable<int>> specificAssertion)
        {
            for (var line = 1; line <= Board.Size; line++)
            {
                for (var column = 1; column <= Board.Size; column++)
                {                  
                    var values = board.ListValuesInBox(new Box(line, column));
                    
                    Assert.IsNotNull(values);
                    if (line == boxToAssert.Line && column == boxToAssert.Column)
                    {
                        specificAssertion(values);
                    }
                    else
                    {
                        Assert.That(values.Count(), Is.EqualTo(Board.Size));
                        Assert.That(values.Distinct().Count(), Is.EqualTo(Board.Size));

                        Assert.That(values.Min(), Is.EqualTo(1));
                        Assert.That(values.Max(), Is.EqualTo(Board.Size));
                    }
                }
            }
        }
    }
}