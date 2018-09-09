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
                                p.Number = reader["Number"].ToString();
                                p.Email = reader["Email"].ToString();
                                p.FullName = reader["FullName"].ToString();
                                p.Balance = (decimal)reader["Balance"];
                                p.CreatedDate = (DateTime)reader["CreatedDate"];
                                p.Address = reader["Address"].ToString();
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
                                p.Number = reader["Number"].ToString();
                                p.Email = reader["Email"].ToString();
                                p.FullName = reader["FullName"].ToString();
                                p.Balance = (decimal)reader["Balance"];
                                p.CreatedDate = (DateTime)reader["CreatedDate"];
                                p.Address = reader["Address"].ToString();
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

        public Account GetByEmail(string email)
        {
            using (var conn = new SqlConnection(_conn))
            {
                string sql = "Select * FROM Account WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Email", email);
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
                                p.Number = reader["Number"].ToString();
                                p.Email = reader["Email"].ToString();
                                p.FullName = reader["FullName"].ToString();
                                p.Balance = (decimal)reader["Balance"];
                                p.CreatedDate = (DateTime)reader["CreatedDate"];
                                p.Address = reader["Address"].ToString();
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

        public Account GetByEmailAndPassword(string email, string password)
        {
            using (var conn = new SqlConnection(_conn))
            {
                string sql = "Select * FROM Account WHERE Email=@email AND Password =@Password";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Email", email);
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
                                p.Number = reader["Number"].ToString();
                                p.Email = reader["Email"].ToString();
                                p.FullName = reader["FullName"].ToString();
                                p.Balance = (decimal)reader["Balance"];
                                p.CreatedDate = (DateTime)reader["CreatedDate"];
                                p.Address = reader["Address"].ToString();
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

        public Account GetByNumber(string number)
        {
            using (var conn = new SqlConnection(_conn))
            {
                string sql = "Select * FROM Account WHERE Number=@number";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Number", number);
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
                                p.Number = reader["Number"].ToString();
                                p.Email = reader["Email"].ToString();
                                p.FullName = reader["FullName"].ToString();
                                p.Balance = (decimal)reader["Balance"];
                                p.CreatedDate = (DateTime)reader["CreatedDate"];
                                p.Address = reader["Address"].ToString();
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
                string sql = "INSERT INTO Account (Number, Email, FullName, Balance, CreatedDate, Address) VALUES (@Number, @Email, @FullName, @Balance, @CreatedDate, @Address)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Number", entity.Number);
                cmd.Parameters.AddWithValue("@Email", entity.Email);
                cmd.Parameters.AddWithValue("@FullName", entity.FullName);
                cmd.Parameters.AddWithValue("@Password", entity.Password);
                cmd.Parameters.AddWithValue("@Balance", entity.Balance);
                cmd.Parameters.AddWithValue("@CreatedDate", entity.CreatedDate);
                cmd.Parameters.AddWithValue("@Address", entity.Address);
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
                command.CommandText = "UPDATE Account SET Number = @Number, Email = @Email, FullName = @FullName, Balance = @Balance, Address = @Address Where ID = @ID";

                if (!string.IsNullOrEmpty(entity.Number))
                {
                    command.Parameters.AddWithValue("@Number", entity.Number);
                }

                if (!string.IsNullOrEmpty(entity.Email))
                {
                    command.Parameters.AddWithValue("@Email", entity.Email);
                }

                if (!string.IsNullOrEmpty(entity.FullName))
                {
                    command.Parameters.AddWithValue("@FullName", entity.FullName);
                }

                if (!string.IsNullOrEmpty(entity.Password))
                {
                    command.Parameters.AddWithValue("@Password", entity.Password);
                }

                if (entity.Balance > 0)
                {
                    command.Parameters.AddWithValue("@Balance", entity.Balance);
                }

                if (!string.IsNullOrEmpty(entity.Address))
                {
                    command.Parameters.AddWithValue("@Address", entity.Address);
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
                            p.Number = reader["Number"].ToString();
                            p.Email = reader["Email"].ToString();
                            p.FullName = reader["FullName"].ToString();
                            p.Balance = (decimal)reader["Balance"];
                            p.CreatedDate = (DateTime)reader["CreatedDate"];
                            p.Address = reader["Address"].ToString();
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
