using System.Text;

namespace Postgres.NET
{
    public interface IDbNamingConverter
    {
        string GetPropertyName(string columnName);
    }

    public class DbNamingConverter : IDbNamingConverter
    {
        public DbNamingConverter()
        { 
        
        }

        public string GetPropertyName(string columnName)
        {
            var propertyName = new StringBuilder();
            var parts = columnName.Split('_');
            foreach (var part in parts)
            {
                propertyName.Append(part.Substring(0, 1).ToUpper() + part.Substring(1));
            }
            return propertyName.ToString();
        }
    }
}
