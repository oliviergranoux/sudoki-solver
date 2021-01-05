using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace sudoku_solver.Tests
{
    public class BoxTests
    {
        [Test]
        [TestCase(0)]
        [TestCase(Board.Size + 1)]
        public void Test_Box_10_Contructor_InvalidLine(int line)
        {
            //Act
            var action = new TestDelegate(() => new Box(line, 1));

            //Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.IsNotNull(exception);
            Assert.That(exception.Message, Is.EqualTo(nameof(line)));
        }

        [Test]
        [TestCase(0)]
        [TestCase(Board.Size + 1)]
        public void Test_Box_11_Contructor_InvalidColumn(int column)
        {
            //Act
            // ActualValueDelegate<object> exception = () => new Box(1, column);
            var action = new TestDelegate(() => new Box(1, column));

            //Assert
            // Assert.That(exception, Throws.TypeOf<ArgumentException>());
            // Assert.That(exception, Throws.ArgumentNullException);
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.IsNotNull(exception);
            Assert.That(exception.Message, Is.EqualTo(nameof(column)));
           
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        public void Test_Box_12_Contructor_ValidLine(int line)
        {
            //Act
            var result = new Box(line, 1);

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.Line, Is.EqualTo(line));
            Assert.That(result.Column, Is.EqualTo(1));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        public void Test_Box_13_Contructor_ValidColumn(int column)
        {
            //Act
            var result = new Box(1, column);

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.Line, Is.EqualTo(1));
            Assert.That(result.Column, Is.EqualTo(column));
        }

        [Test]
        public void Test_Box_20_GetHashcode_UniqueForEachBox()
        {
            //Arrange
            var hashSets = new HashSet<int>();
            for (var line = 1; line <= Board.Size; line++)
            {
                for (var column = 1; column <= Board.Size; column++)
                {
                    hashSets.Add(new Box(line, column).GetHashCode());
                }
            }

            //Act: already generated, thus it should not increase the size of hashset
            hashSets.Add(new Box(1, 1).GetHashCode()); 

            //Assert
            Assert.That(hashSets.Count, Is.EqualTo(Board.Size * Board.Size));
        }
    }
}