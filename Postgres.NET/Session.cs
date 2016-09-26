using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace Postgres.NET
{
    public interface IDbSession : IDisposable
    {
        IDbSession Sql(string sql);

        IDbSession Parameter(string name, object value);

        IDbSession Parameter(string name, object value, NpgsqlDbType dbType);

        TType ToScalar<TType>();

        int Execute();

        IEnumerable<TEntity> ToList<TEntity>();

        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();

        IDbConnection Connection { get; }

        IDbNamingConverter NamingConverter { get; }
    }

    public sealed class DbSession : IDbSession
    {
        private bool disposed;

        private NpgsqlConnection connection;
        private IDbNamingConverter namingConverter;
        
        private String sql;
        private IDictionary<string, IDbParameter> parameters;

        private NpgsqlTransaction transaction;

        private DbSession()
        { 
        
        }

        public DbSession(NpgsqlConnection connection)
            :this(connection,  new DbNamingConverter())
        {
    
        }

        public DbSession(NpgsqlConnection connection, IDbNamingConverter namingConverter)
        {
            this.connection = connection;
            this.namingConverter = namingConverter;
            
            sql = null;
            parameters = new Dictionary<string, IDbParameter>();

            if (this.connection.State != ConnectionState.Open)
            {
                this.connection.Open();
            }
        }

        ~DbSession()
        {
            Dispose(false);
        }

        public IDbSession Sql(string sql)
        {
            this.sql = sql;
            return this;
        }

        public IDbSession Parameter(string name, object value)
        {
            parameters[name] = new DbParameter(name, value);
            return this;
        }

        public IDbSession Parameter(string name, object value, NpgsqlDbType dbType)
        {
            parameters[name] = new DbParameter(name, value, dbType);
            return this;
        }

        public TType ToScalar<TType>()
        {
            using (var command = CreateCommand())
            {
                var value = command.ExecuteScalar();
                return value is DBNull ? default(TType) : (TType)value;
            }
        }

        public int Execute()
        {
            using (var command = CreateCommand())
            {
                return command.ExecuteNonQuery();
            }
        }

        public IEnumerable<TEntity> ToList<TEntity>()
        {
            using(var command = CreateCommand())
            {
                using (var reader = command.ExecuteReader())
                {
                    return ToList<TEntity>(reader);
                }
            }
        }

        private IDbCommand CreateCommand()
        {
            var command = connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            command.Transaction = transaction != null ? transaction : null;

            foreach (var param in parameters)
            {
                var paramName = param.Key;

                var parameter = param.Value;

                NpgsqlParameter dp = command.CreateParameter();

                dp.ParameterName = paramName;

                dp.Value = parameter.Value ?? DBNull.Value;

                if (parameter.DbType.HasValue)
                {
                    dp.NpgsqlDbType = parameter.DbType.Value;
                }

                command.Parameters.Add(dp);
            }

            return command;
        }

        private IList<T> ToList<T>(IDataReader reader)
        {
            var type = typeof(T);

            var list = new List<T>();

            var mappings = new Dictionary<string, string>();
            var readMappings = false;

            while (reader.Read())
            {
                T item = Activator.CreateInstance<T>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var column = reader.GetName(i);
                    string property = null;

                    if (!readMappings)
                    {
                        property = namingConverter != null ? namingConverter.GetPropertyName(column) : column;
                        mappings[column] = property;
                    }
                    else
                    {
                        property = mappings[column];
                    }

                    var p = type.GetProperty(property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    p.SetValue(item, reader[column] != DBNull.Value ? reader[column] : null, null);
                }

                list.Add(item);

                readMappings = true;
            }

            return list;
        }


        public void BeginTransaction()
        {
            transaction = connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (transaction != null)
            {
                transaction.Commit();
                transaction = null;
            }
        }

        public void RollbackTransaction()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (transaction != null)
                    {
                        transaction.Dispose();
                        transaction = null;
                    }

                    if (connection != null)
                    {
                        connection.Dispose();
                        connection = null;
                    }
                }
            }

            disposed = true;
        }

        public IDbConnection Connection
        {
            get { return connection; }
        }

        public IDbNamingConverter NamingConverter
        {
            get { return namingConverter; }
        }
    }
}
