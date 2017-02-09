using System;

namespace Postgres.NET
{
    public class LazyDbSession : Lazy<IDbSession>, IDisposable
    {
        public LazyDbSession(Func<IDbSession> sessionFactory)
            :base(sessionFactory)
        {

        }

        public void Dispose()
        {
            if (IsValueCreated)
            {
                Value.Dispose();
            }
        }
    }
}
