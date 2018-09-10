# A whole Banking Application is built from scratch, a simple, cross platform, system built on .NET Core

## Writing a simple bank system to implement the following features:

- As user they can register new account in bank system with with LoginName and Password
- Table (ID, LoginName, AccountNumber, Password, Balance, CreatedDate)
- AccountNumber auto generate by system
- Balance default value is 0
- As user they can deposit or withdraw form the bank system
- As user they can transfer funds to other existing users
- As user they can view all their transaction history

## Requirements

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

## Steps to run

1. Download source code to your local computer
2. Run the scripts in following path: "~/BankingApplication/BankingApplication/BankingApplication.Infrastructure/Scripts/Create_Tables_Insert_Data.sql" in your SSMS SQL SERVER Database
3. Build the whole solution
4. In Visual Studio, press "Control + F5", please keep calm some second until the beautiful template website run up.

* Note: I created some existing Users in the scripts, you can you them or can register the new one by own yourself with the features on web.

5. Since then you can enjoy and test some main features on the Banking website, such as: 
* WITHDRAW THE MONEY. 
* DEPOSIT THE MONEY.
* TRANSFER YOUR MONEY TO ANOTHER ACCOUNT.
* VIEW HISTORY TRANSACTION.

## How to contribute

- Report bugs or suggest features by create new issues or add comments to issues
- Submit pull requests

## License

BankingApplication is licensed under the Apache 2.0 license.