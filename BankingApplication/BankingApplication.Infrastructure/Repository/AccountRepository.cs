using BankingApplication.Infrastructure.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace BankingApplication.Infrastructure.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private string _conn = "Server = localhost; Database = SimpleBankApp; Trusted_Connection = True; ";

        public AccountRepository()
        {
        }

        public void Delete(Account entity)
        {
            using (var conn = new SqlConnection(_conn))
            {
                string sql = "DELETE Account Where ID=@ID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ID", entity.ID);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public void DeleteById(int id)
        {
            using (var conn = new SqlConnection(_conn))
            {
                string sql = "DELETE Account Where ID=@Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public Account GetById(int id)
        {
            using (var conn = new SqlConnection(_conn))
            {
                string sql = "Select * FROM Account WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                Account p = null;
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                p = new Account();
                                p.ID = (int)reader["ID"];
                                p.AccountNumber = reader["AccountNumber"].ToString();
                                p.LoginName = reader["LoginName"].ToString();
                                p.Balance = (decimal)reader["Balance"];
                                p.CreatedDate = (DateTime)reader["CreatedDate"];
                                p.RowVersion = (byte[])reader["RowVersion"];
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                return p;
            }
        }

        public Account GetByVersion(int id, byte[] rowVersion)
        {
            using (var conn = new SqlConnection(_conn))
            {
                string sql = "Select * FROM Account WHERE id = @Id AND RowVersion = @RowVersion";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@RowVersion", rowVersion);
                Account p = null;
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                p = new Account();
                                p.ID = (int)reader["ID"];
                                p.AccountNumber = reader["AccountNumber"].ToString();
                                p.LoginName = reader["LoginName"].ToString();
                                p.Balance = (decimal)reader["Balance"];
                                p.CreatedDate = (DateTime)reader["CreatedDate"];
                                p.RowVersion = (byte[])reader["RowVersion"];
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                return p;
            }
        }
        
        public Account GetByLoginNameAndPassword(string loginName, string password)
        {
            using (var conn = new SqlConnection(_conn))
            {
                string sql = "Select * FROM Account WHERE LoginName=@loginName AND Password =@Password";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@LoginName", loginName);
                cmd.Parameters.AddWithValue("@Password", password);
                Account p = null;
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                p = new Account();
                                p.ID = (int)reader["ID"];
                                p.AccountNumber = reader["AccountNumber"].ToString();
                                p.LoginName = reader["LoginName"].ToString();
                                p.Balance = (decimal)reader["Balance"];
                                p.CreatedDate = (DateTime)reader["CreatedDate"];
                                p.RowVersion = (byte[])reader["RowVersion"];
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                return p;
            }
        }

        public Account GetByAccountNumber(string accountNumber)
        {
            using (var conn = new SqlConnection(_conn))
            {
                string sql = "Select * FROM Account WHERE AccountNumber=@accountNumber";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                Account p = null;
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                p = new Account();
                                p.ID = (int)reader["ID"];
                                p.AccountNumber = reader["AccountNumber"].ToString();
                                p.LoginName = reader["LoginName"].ToString();
                                p.Balance = (decimal)reader["Balance"];
                                p.CreatedDate = (DateTime)reader["CreatedDate"];
                                p.RowVersion = (byte[])reader["RowVersion"];
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                return p;
            }
        }

        public Account GetByLoginName(string loginName)
        {
            using (var conn = new SqlConnection(_conn))
            {
                string sql = "Select * FROM Account WHERE LoginName=@loginName";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@LoginName", loginName);
                Account p = null;
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                p = new Account();
                                p.ID = (int)reader["ID"];
                                p.AccountNumber = reader["AccountNumber"].ToString();
                                p.LoginName = reader["LoginName"].ToString();
                                p.Balance = (decimal)reader["Balance"];
                                p.CreatedDate = (DateTime)reader["CreatedDate"];
                                p.RowVersion = (byte[])reader["RowVersion"];
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                return p;
            }
        }

        public void Create(Account entity)
        {
            using (var conn = new SqlConnection(_conn))
            {
                string sql = "INSERT INTO Account (AccountNumber, LoginName, Balance, CreatedDate) VALUES (@AccountNumber, @LoginName, @Balance, @CreatedDate)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@AccountNumber", entity.AccountNumber);
                cmd.Parameters.AddWithValue("@LoginName", entity.LoginName);
                cmd.Parameters.AddWithValue("@Password", entity.Password);
                cmd.Parameters.AddWithValue("@Balance", entity.Balance);
                cmd.Parameters.AddWithValue("@CreatedDate", entity.CreatedDate);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public void Update(Account entity)
        {
            using (var conn = new SqlConnection(_conn))
            using (SqlCommand command = conn.CreateCommand())
            {
                command.CommandText = "UPDATE Account SET AccountNumber = @AccountNumber, LoginName = @LoginName, Balance = @Balance Where ID = @ID";

                if (!string.IsNullOrEmpty(entity.AccountNumber))
                {
                    command.Parameters.AddWithValue("@AccountNumber", entity.AccountNumber);
                }
                
                if (!string.IsNullOrEmpty(entity.LoginName))
                {
                    command.Parameters.AddWithValue("@LoginName", entity.LoginName);
                }

                if (!string.IsNullOrEmpty(entity.Password))
                {
                    command.Parameters.AddWithValue("@Password", entity.Password);
                }

                if (entity.Balance > 0)
                {
                    command.Parameters.AddWithValue("@Balance", entity.Balance);
                }
                
                if (entity.ID > 0)
                {
                    command.Parameters.AddWithValue("@ID", entity.ID);
                }

                conn.Open();

                command.ExecuteNonQuery();

                conn.Close();
            }
        }

        public IEnumerable<Account> GetAll(int id)
        {
            string sql = "Select * FROM Account ORDER BY Nome";
            using (var conn = new SqlConnection(_conn))
            {
                var cmd = new SqlCommand(sql, conn);
                List<Account> list = new List<Account>();
                Account p = null;
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            p = new Account();
                            p.ID = (int)reader["ID"];
                            p.AccountNumber = reader["AccountNumber"].ToString();
                            p.LoginName = reader["LoginName"].ToString();
                            p.Balance = (decimal)reader["Balance"];
                            p.CreatedDate = (DateTime)reader["CreatedDate"];
                            p.RowVersion = (byte[])reader["RowVersion"];
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
    }
}
