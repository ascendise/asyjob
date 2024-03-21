using AspNetCore.Identity.MongoDbCore.Models;
using AsyJob.Lib.Auth;
using System.Text.RegularExpressions;

namespace AsyJob.Auth
{
    public class ClaimToRightConverter
    {
        public Right ToRight(MongoClaim claim)
            => new(claim.Type, ToOperation(claim.Value));

        private static Operation ToOperation(string value)
        {
            if (value.Length == 0)
                return Operation.None;
            if (value.Length > 3)
                throw new ArgumentException("Invalid string", nameof(value));
            var operation = Operation.None;
            foreach (var c in value)
                operation = CharToOperation(c);
            return operation;
        }

        private static Operation CharToOperation(char c)
            => c switch
            {
                'r' or 'R' => Operation.Read,
                'w' or 'W' => Operation.Write,
                'x' or 'X' => Operation.Execute,
                _ => throw new ArgumentException("Invalid string")
            };
    }
}
