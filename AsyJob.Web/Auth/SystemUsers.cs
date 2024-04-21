using AsyJob.Lib.Auth;

namespace AsyJob.Web.Auth
{
    public static class SystemUsers
    {
        public static User Admin { 
            get => new("Admin") 
            { 
                Rights = [
                    new(Resources.Jobs, Operation.Read | Operation.Write | Operation.Execute),
                    new("Users", Operation.Read | Operation.Write | Operation.Execute)
                    ] 
            }; 
        }
    }
}
