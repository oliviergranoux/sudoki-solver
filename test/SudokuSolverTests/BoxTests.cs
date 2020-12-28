using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace sudoku_solver.Tests
{
    public class BoxTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase(0)]
        [TestCase(10)]
        public void Test_10_Contructor_InvalidLine(int line)
        {
            //Arrange
            var column = 1;

            //Act
            var exception = Assert.Throws<ArgumentException>(() => new Box(line, column));

            //Assert
            Assert.IsNotNull(exception);
            Assert.That(exception.Message, Is.EqualTo("line"));
        }

        [Test]
        [TestCase(0)]
        [TestCase(10)]
        public void Test_11_Contructor_InvalidColumn(int column)
        {
            //Arrange
            var line = 1;

            //Act
            // ActualValueDelegate<object> exception = () => new Box(line, column);
            var exception = Assert.Throws<ArgumentException>(() => new Box(line, column));

            //Assert
            // Assert.That(exception, Throws.TypeOf<ArgumentException>());
            // Assert.That(exception, Throws.ArgumentNullException);
            Assert.IsNotNull(exception);
            Assert.That(exception.Message, Is.EqualTo("column"));
           
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
        public void Test_12_Contructor_ValidLine(int line)
        {
            //Arrange
            var column = 1;

            //Act
            var result = new Box(line, column);

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.Line, Is.EqualTo(line));
            Assert.That(result.Column, Is.EqualTo(column));
        }

        [Test]
        [Order(4)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        public void Test_13_Contructor_ValidColumn(int column)
        {
            //Arrange
            var line = 1;

            //Act
            var result = new Box(line, column);

            var a = new Box(1,2).Equals(new Box(1,2));

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.Line, Is.EqualTo(line));
            Assert.That(result.Column, Is.EqualTo(column));
        }

        [Test]
        public void Test_20_GetHashcode_UniqueForEachBox()
        {
            //Arrange
            var hashSets = new HashSet<int>();

            //Act
            for (var line = 1; line <= 9; line++)
            {
                for (var column = 1; column <= 9; column++)
                {
                    hashSets.Add(new Box(line, column).GetHashCode());
                }
            }

            hashSets.Add(new Box(1, 1).GetHashCode()); //already generated, thus it should not increase the size of hashset

            //Assert
            Assert.That(hashSets.Count, Is.EqualTo(9*9));
        }
    }
}