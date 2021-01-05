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
        public void Test_Board_15_SetValueInBox_InvalidValue(int value)
        {
            //Act
            var action = new TestDelegate(() => new Board().SetValueInBox(value, new Box(1,1)));

            //Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.IsNotNull(exception);
            Assert.That(exception.Message, Is.EqualTo(nameof(value)));
        }

        [Test]
        public void Test_Board_16_SetValueInBox_NullBox()
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

        //ListBoxesInSquare
        //ListBoxesInAllSquares
        //ListBoxesInLine
        //ListBoxesInColumn
        //FilterBoxesContainingValue
    }
}