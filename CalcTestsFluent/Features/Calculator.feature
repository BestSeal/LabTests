Feature: Calculator

    @mytag
    Scenario Outline: Add two numbers
        Given the first number is <First>
        And the second number is <Second>
        When the two numbers are <Operation>
        Then the result should be <Result>

        Examples:
          | First | Operation | Second | Result |
          | 12    | Sum       | 7      | 19     |
          | 20    | Divide    | 0      |        |
          | 20    | Multiply  | 0      | 0      |
          | 20    | Subtract  | 10     | 10     |
          | 10    | Divide    | 5      | 2      |