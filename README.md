Assessment: Design an Expense Management System
===============================================

This assessment consist of designing a Web API in .NET with REST to:

- Create expenses.
- List expenses.

Specifications
--------------

An expense is characterized by:

- A user (the person who made the purchase).
- A date.
- A type (possible values: Restaurant, Hotel, and Misc).
- An amount and currency.
- A comment.

A user is characterized by:

- A last name.
- A first name.
- A currency in which they make their expenses.

Creating an Expense
-------------------

This API should create an expense taking into account validation rules:

- An expense cannot have a future date.
- An expense cannot be dated more than 3 months ago.
- The comment is mandatory.
- A user cannot declare the same expense twice (same date and amount).
- The expense currency must be identical to the user's currency.

Listing Expenses
----------------

This API should:

- List expenses for a given user.
- Sorting expenses by amount or date.
- Displaying all expense properties, and the expense user should appear in the format `{FirstName} {LastName}` (e.g. "Anthony Stark").

Technical Constraints
---------------------

- The application should be developed in .NET/C#
- Data should be persisted in a SQL database
- The users table should be initialized with the following users:
  - STARK Anthony (with US dollar as currency)
  - ROMANOVA Natasha (with Russian ruble as currency)
- You're free to use any relevant libraries.
- It's estimated to take about 5 hours to complete this technical assessment.

Evaluation Criteria
-------------------

- Quality
  - The code should be clean, readable, extensible, well-structured, and easily maintainable.
  - The code should follow best practices.
  - The proposed solution must include unit tests.
- Acceptance
  - All features described in the instructions should be implemented and functional.
  - The expense validation rules must be unit tested.
- Performance
  - The application should be fast and responsive.
  - Loading time should be minimal.
