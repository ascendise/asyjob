namespace AsyJob.Lib.Auth
{
    public class User(Guid id, string username, IEnumerable<Right> rights)
    {
        public Guid Id { get; } = id;
        public string Username { get; private set; } = username;
        public IEnumerable<Right> Rights { get; } = rights;

        /// <summary>
        /// Checks if user has the required rights and returns a list of all missing rights
        /// If the user has all required rights, the method returns an empty list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Right> Needs(IEnumerable<Right> required)
        {
            List<Right> missing = [];
            foreach (var requiredRight in required)
            {
                Right? userRight = Rights.SingleOrDefault(r => r.Resource == requiredRight.Resource);
                if (!userRight.HasValue)
                {
                    missing.Add(requiredRight);
                    continue;
                }
                Operation missingOps = GetMissingOperations(userRight.Value, requiredRight.Ops);
                if (missingOps != Operation.None)
                {
                    missing.Add(new Right(requiredRight.Resource, missingOps));
                }
            }
            return missing;
        }

        private static Operation GetMissingOperations(Right userRight, Operation requiredOps)
        {
            return ~userRight.Ops & requiredOps;
        }
    }

}
