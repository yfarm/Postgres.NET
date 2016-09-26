using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postgres.NET.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string username = "postgres";
            string password = "asdf1982";
            string connectionString = string.Format(@"Server=127.0.0.1;Port=5432;Database=test;User Id={0};Password={1};", username, password);


            using (var session = new DbSession(new NpgsqlConnection(connectionString)))
            {
                try
                {
                    session.BeginTransaction();

                    session.Sql("delete from person")
                           .Execute();

                    session.Sql(@"insert into person (id, name, age, employed, created_date)
                                  values(@id, @name, @age, @employed, @created_date)")
                           .Parameter("@id", 1)
                           .Parameter("@name", "jake")
                           .Parameter("@age", 23)
                           .Parameter("@employed", true)
                           .Parameter("@created_date", DateTime.Now)
                           .Execute();

                    session.Sql(@"insert into person (id, name, age, employed, created_date)
                                  values(@id, @name, @age, @employed, @created_date)")
                           .Parameter("@id", 2)
                           .Parameter("@name", "cindi")
                           .Parameter("@age", null)
                           .Parameter("@employed", false)
                           .Parameter("@created_date", DateTime.Now)
                           .Execute();

                    session.Sql(@"insert into person (id, name, age, employed, created_date)
                                  values(@id, @name, @age, @employed, @created_date)")
                       .Parameter("@id", 3)
                       .Parameter("@name", "john")
                       .Parameter("@age", 32)
                       .Parameter("@employed", false)
                       .Parameter("@created_date", DateTime.Now)
                       .Execute();

                    session.Sql(@"insert into person (id, name, age, employed, created_date)
                                  values(@id, @name, @age, @employed, @created_date)")
                       .Parameter("@id", 4)
                       .Parameter("@name", "jenny")
                       .Parameter("@age", null)
                       .Parameter("@employed", true)
                       .Parameter("@created_date", DateTime.Now)
                       .Execute();

                    session.Sql(@"insert into person (id, name, age, employed, created_date)
                                  values(@id, @name, @age, @employed, @created_date)")
                       .Parameter("@id", 5)
                       .Parameter("@name", "mike")
                       .Parameter("@age", 25)
                       .Parameter("@employed", false)
                       .Parameter("@created_date", DateTime.Now)
                       .Execute();

                    session.CommitTransaction();

                    System.Console.WriteLine("Transaction was committed");
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);

                    session.RollbackTransaction();

                    System.Console.WriteLine("Transaction was rolled back");
                }
            }

            System.Console.WriteLine("---------------------------");

            using (var session = new DbSession(new NpgsqlConnection(connectionString)))
            {
                var personList = session.Sql("select * from person").ToList<Person>();
                foreach (var person in personList)
                {
                    System.Console.WriteLine(person);
                }

                System.Console.WriteLine("---------------------------");

                var personCount = session.Sql("select count(*) from person where name = @name")
                                    .Parameter("@name", "jenny")
                                    .ToScalar<long>();

                System.Console.WriteLine(personCount);

                System.Console.WriteLine("---------------------------");

                var personList2 = session.Sql("select * from func_get_persons(@name)")
                                   .Parameter("@name", new string[] { "john", "cindi" }, NpgsqlDbType.Array | NpgsqlDbType.Text)
                                   .ToList<Person>();

                foreach (var person in personList2)
                {
                    System.Console.WriteLine(person);
                }

                System.Console.WriteLine("---------------------------");
            }

            System.Console.WriteLine("Press any key to exit.");
            System.Console.Read();
        }
    }
}
