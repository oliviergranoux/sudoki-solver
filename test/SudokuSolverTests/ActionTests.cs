using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace sudoku_solver.Tests
{
    public class ActionTests
    {
        [Test]
        public void Test_Action_10_Contructor_NullBox()
        {
            //Arrange
            Box box = null;
            
            //Act
            var action = new TestDelegate(() => { new Action(box, 1); });

            //Assert
            var exception = Assert.Throws<ArgumentNullException>(action);
            Assert.IsNotNull(exception);
            StringAssert.Contains(nameof(box), exception.Message);

        }

        [Test]
        [TestCase(0)]
        [TestCase(Board.Size + 1)]
        public void Test_Action_11_Contructor_InvalidValue(int value)
        {
            //Act
            var action = new TestDelegate(() => new Action(new Box(1,1), value));

            //Arrange
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.IsNotNull(exception);
            Assert.That(exception.Message, Is.EqualTo(nameof(value)));
        }

        [Test]
        public void Test_Action_13_Constructor()
        {
            //Arrange
            var box = new Box(1, 2);
            var value = 3;

            //Act
            var action = new Action(box, value);

            //Assert
            Assert.IsNotNull(action);
            Assert.That(action.Box, Is.EqualTo(box));
            Assert.That(action.Value, Is.EqualTo(value));
        }
   }
}