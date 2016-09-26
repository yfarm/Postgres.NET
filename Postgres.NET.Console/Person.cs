using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postgres.NET.Console
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public bool Employed { get; set; }
        public DateTime CreatedDate { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}, Age: {2}, Employed: {3}, Created Date: {4}", 
                Id, Name, Age.HasValue ? Age.Value.ToString(): "null", Employed, CreatedDate);
        }
    }
}
