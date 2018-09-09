using BankingApplication.Infrastructure.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace BankingApplication.Infrastructure.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private string _conn = "Server = localhost; Database = SimpleBankApp; Trusted_Connection = True; ";

        public TransactionRepository()
        {
        }

        public void Delete(Transaction entity)
        {
            throw new NotImplementedException();
        }
        
        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public Transaction GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Transaction GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Transaction GetByLoginNameAndPassword(string loginName, string password)
        {
            throw new NotImplementedException();
        }

        public Transaction GetByAccountNumber(string number)
        {
            throw new NotImplementedException();
        }

        public void Create(Transaction entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Transaction entity)
        {
            throw new NotImplementedException();
        }

        public void SaveTransaction(Transaction transaction)
        {
            using (SqlConnection connection = new SqlConnection(_conn))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction sqlTransaction;

                // Start a local transaction.
                sqlTransaction = connection.BeginTransaction("SimpleBankTransaction");

                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                command.Connection = connection;
                command.Transaction = sqlTransaction;

                try
                {
                    // Save the money transaction Info:

                    command.CommandText = "INSERT INTO [Transaction] (ToAccount, FromAccount, CreatedDate, Amount, Type, Status) VALUES (@ToAccount, @FromAccount, @CreatedDate, @Amount, @Type, @Status)";

                    if (transaction.ToAccount != null)
                    {
                        command.Parameters.AddWithValue("@ToAccount", transaction.ToAccount.ID);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ToAccount", DBNull.Value);
                    }

                    if (transaction.FromAccount != null)
                    {
                        command.Parameters.AddWithValue("@FromAccount", transaction.FromAccount.ID);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@FromAccount", DBNull.Value);
                    }

                    command.Parameters.AddWithValue("@CreatedDate", transaction.CreatedDate);
                    command.Parameters.AddWithValue("@Amount", transaction.Amount);
                    command.Parameters.AddWithValue("@Type", (int)transaction.Type);
                    command.Parameters.AddWithValue("@Status", (int)transaction.Status);

                    command.ExecuteNonQuery();

                    // Update account balance info:
                    if (transaction.FromAccount != null)
                    {
                        command.CommandText = "UPDATE Account SET Balance = @Balance1 Where ID = @ID1";

                        if (transaction.FromAccount.Balance > 0)
                        {
                            command.Parameters.AddWithValue("@Balance1", transaction.FromAccount.Balance);
                        }

                        if (transaction.FromAccount.ID > 0)
                        {
                            command.Parameters.AddWithValue("@ID1", transaction.FromAccount.ID);
                        }

                        command.ExecuteNonQuery();
                    }

                    if (transaction.ToAccount != null)
                    {
                        command.CommandText = "UPDATE Account SET Balance = @Balance2 Where ID = @ID2";

                        if (transaction.ToAccount.Balance > 0)
                        {
                            command.Parameters.AddWithValue("@Balance2", transaction.ToAccount.Balance);
                        }

                        if (transaction.ToAccount.ID > 0)
                        {
                            command.Parameters.AddWithValue("@ID2", transaction.ToAccount.ID);
                        }

                        command.ExecuteNonQuery();
                    }

                    // Attempt to commit the transaction.
                    sqlTransaction.Commit();
                    Console.WriteLine("Both records are written to database.");

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

                    // Attempt to roll back the transaction.
                    try
                    {
                        sqlTransaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }
            }
        }


        public IEnumerable<Transaction> GetAll(int id)
        {
            string sql = "SELECT * FROM [Transaction] Where ToAccount = @id OR FromAccount = @id";
            
            using (var conn = new SqlConnection(_conn))
            {
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                List<Transaction> list = new List<Transaction>();
                Transaction p = null;
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            p = new Transaction();
                            p.ID = (int)reader["ID"];
                            p.ToAccountId = (reader["ToAccount"].GetType() != typeof(DBNull)) ? (int)reader["ToAccount"] : 0;
                            p.FromAccountId = (reader["FromAccount"].GetType() != typeof(DBNull)) ? (int)reader["FromAccount"] : 0;
                            p.CreatedDate = (DateTime)reader["CreatedDate"];
                            p.Amount = (Decimal)reader["Amount"];
                            p.Type = (TransactionType)reader["Type"];
                            p.Status = (TransactionStatus)reader["Status"];

                            list.Add(p);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                return list;
            }
        }

        public Transaction GetByLoginName(string loginName)
        {
            throw new NotImplementedException();
        }
    }
}
