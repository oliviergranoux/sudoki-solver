# sudoku-solver
A Sudoku solver using only 'human' rules.

## Table of content

- [Features](#features)
- [Get Started](#get-started)
- [Author](#author)
- [Licence](#licence)

## Features

This is a sudoku solver using only 'human' rules, meaning that application will use rules as if it was a human that solve the sudoku. 

Initially I wanted to know which rules are needed to resolve myself a sudoku. Moreover, I wanted also a solution to resolve and to validate sudoku that I am not able to resolve myself.

In fact I discovered that only few rules are needed to resolve a sudoku, related to boards used to test my application. Of course you are welcome to inform me of any board that the application does not resolve itself.

_Basically, each box containts a list of possible values from 1 to 9. List of rules:_
1. if an single value is found for a box, you have to remove this possible value from other boxes of square, line and column,
2. if a value is possible only in a box of a square/line/column, remove it from possible values of other boxes in the *same* section (square/line/column),
3. if a value is possible only within a line (or column) of the current square, remove it from other boxes of the same line (or column) in other squares. Do not forget to check if it is affect the unicity of value in these squares.

That's it!

## Get Started
to solve a sudoku with the application:
1. complete a text file with the board,
2. change the call of application to check the selected board (see `Main` function in `program.cs`),
3. run the application in the root directory by using the command `dotnet run --project .\src\sudoku-solver\`

_Notes on the content of the board's file:_

* each line stating with the sharp (#) letter is interpreted as a commented line,
* each unknown box is represented by a dash (-),
* you can use any space or return line, they will be ignored.

## Author
[Olivier Granoux](http://olivier.granoux.com)

## License
Repository is licensed under the [MIT license](LICENSE).