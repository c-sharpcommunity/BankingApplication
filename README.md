# BankingApplication, a simple, cross platform, system built on .NET Core

#### Writing a simple bank system to implement the following features:

- As user they can register new account in bank system with with LoginName and Password
- Table (ID, LoginName, AccountNumber, Password, Balance, CreatedDate)
- AccountNumber auto generate by system
- Balance default value is 0
- As user they can deposit or withdraw form the bank system
- As user they can transfer funds to other existing users
- As user they can view all their transaction history

#### Requirements

- Use DotNet Core
- Use Ado.Net for database access only
- Store data to MSSQL
- Unit tests is MUST ​and please PROPERLY ​unit tests
- Handle concurrency ​issues
- DON’T​ use AspNet Core Identity library.
- Make sure login name not duplicated in database
- Handle exception is very important, please handle all exception properly
- Make sure there is not any loophole
- Use github to share the source code

#### Steps to run

1. CONFIG API:
- Update the connection string in appsettings.json.
- Build whole solution.
- In Visual Studio, press "Control + F5".

## How to contribute

- Report bugs or suggest features by create new issues or add comments to issues
- Submit pull requests

## License

BankingApplication is licensed under the Apache 2.0 license.