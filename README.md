# Postgres.NET
Postgres.NET is a simple .NET database wrapper for PostgreSQL. It is an efficient replacement for those who do not want to use an ORM framework, like
Entity Framework or NHibernate. While ORM frameworks are nice and easy to use at times, they also come with come with several drawbacks, especially
when trying to build complex and high performace applications. PostgreSQL is an amazing database and it is fairy easy to use for an experienced developer 
who wants to put his or her business logic in SQL statements or database functions and call them through a thin wrapper, which is way more efficient 
and faster than using an ORM. The goal of this project is to provide a simple database wrapper to work with PostgreSQL. It can also be easily modified 
to work with any database that has an ADO.NET data provider, such as MySQL, SQL Server, Oracle, etc. The project is dependent on the Npsql data provider 
for PostgreSQL. You can read more about it at http://www.npgsql.org. Please download the sample project or get it through GIT and play around with 
the samples before importing the code into your own project. In the console project you will find a test.script that you need to run on a 
ProgreSQL database before running the samples. Also don't forget to updated the database credentials in the connection string. 
The samples are fairly salf-explanatory.


Creating a database session is fairly easy. Do not forget to wrap it in a using statement to dispose resources after you are done working with the session.
```C#
using (var session = new DbSession(new NpgsqlConnection(connectionString)))
{
	//do stuff here
}
```

This sample shows how to get list of items from a table. The matching between the database columns and the C# properties is done through 
the NamingConverter. You can modify the NamingConverter according to your needs. For now it converts a database column named a_b_c into Abc in C#. 
```C#
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
							  Id, Name, Age, Employed, CreatedDate);
	}
}

var persons = session.Sql("select * from person").ToList<Person>();
foreach (var person in persons)
{
	System.Console.WriteLine(person);
}
```

This sample shows how to get a scalar value from a SQL statement and at the same time how to pass parameters.
```C#
var personCount = session.Sql("select count(*) from person where name = @name")
					 .Parameter("@name", "jane")
					 .ToScalar<long>();
```  

This sample show to call a database function, which is no different from calling a SQL statement and passing complex parameters, such as arrays.
```C#
var persons2 = session.Sql("select * from func_get_persons(@name)")
					.Parameter("@name", new string[] { "john", "jane" }, NpgsqlDbType.Array | NpgsqlDbType.Text)
					.ToList<Person>();

foreach (var person in persons2)
{
	System.Console.WriteLine(person);
}
```  

