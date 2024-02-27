using System.Net.WebSockets;
using System.Security.Cryptography;

namespace AsyJob.Jobs
{
    public class DiceRollJob : Job, IInput<DiceRollInput>, IOutput<DiceRollOutput>
    {
        public DiceRollJob(string id, string name, DiceRollInput input, string description = "") : base(id, name, description)
        {
            Input = input;
        }

        public DiceRollJob(string id, DiceRollInput input, string description = "") : this(id, id, input, description) { }

        public DiceRollInput Input { get; private set; } 

        public DiceRollOutput? Output { get; private set; }

        protected override void OnRun()
        {
            var random = new Random();
            var roll = random.Next(Input.Sides + 1);
            Output = new DiceRollOutput(roll);
        }
    }

    public class DiceRollInput(int sides)
    {
        /// <summary>
        /// Defines the number of sides the dice has.
        /// E.g. a D20 would have Sides = 20
        /// </summary>
        public int Sides { get; set; } = sides;
    }

    public class DiceRollOutput(int result)
    {
        /// <summary>
        /// Result of the dice roll
        /// </summary>
        public int Result { get; set; } = result;
    }
}
