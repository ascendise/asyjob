using System.Text;

namespace AsyJob.Lib.Auth
{
    public readonly struct Right
    {
        public string Resource { get; }
        public Operation Ops { get; }

        public Right(string resource, Operation ops)
        {
            Resource = resource;
            Ops = ops;
        }

        public Right(string str)
        {
            Resource = GetResourceName(str);
            Ops = GetOperations(str);
        }

        private static string GetResourceName(string str)
        {
            var splitCharIndex = str.IndexOf('_');
            return str[..splitCharIndex];
        }

        private static Operation GetOperations(string str)
        {
            var splitCharIndex = str.IndexOf('_');
            var opsString = str[(splitCharIndex + 1)..];
            if (opsString.Equals(0))
                return Operation.None;
            var op = Operation.None;
            foreach (var c in opsString)
            {
                op |= GetOperation(c);
            }
            return op;
        }

        private static Operation GetOperation(char c) => c switch
        {
            'r' => Operation.Read,
            'w' => Operation.Write,
            'x' => Operation.Execute,
            '0' => Operation.None,
            _ => throw new FormatException($"Invalid char '{c}' used for operation")
        };

        public override string ToString()
        {
            if (Ops == Operation.None)
                return $"{Resource}_0";
            var opsString = GetOpsString();
            return $"{Resource}_{opsString}";
        }

        private string GetOpsString()
        {
            var opsString = new StringBuilder();
            if (Ops.HasFlag(Operation.Read))
                opsString.Append('r');
            if (Ops.HasFlag(Operation.Write))
                opsString.Append('w');
            if (Ops.HasFlag(Operation.Execute))
                opsString.Append('x');
            return opsString.ToString();
        }
    }

}
