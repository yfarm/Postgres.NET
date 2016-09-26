using NpgsqlTypes;

namespace Postgres.NET
{
    public interface IDbParameter
    {
        string Name { get; }

        object Value { get; }

        NpgsqlDbType? DbType { get; }
    }

    public sealed class DbParameter : IDbParameter
    {
        private DbParameter()
        {

        }

        internal DbParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }

        internal DbParameter(string name, object value, NpgsqlDbType dbType)
        {
            Name = name;
            Value = value;
            DbType = dbType;
        }

        public string Name { get; private set; }
        
        public object Value { get; private set; }

        public NpgsqlDbType? DbType { get; private set; }
    }
}
