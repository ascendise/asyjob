using AsyJob.Lib;
using AsyJob.Lib.Jobs;
using AsyJob.Lib.Jobs.Factory;
using AsyJob.Lib.Runner;

namespace AsyJob.Web.Jobs
{
    /// <summary>
    /// Example job for representing input and output.
    /// This job simulates a dice roll, with a user-defined dice.
    /// It outputs a random result, in the range of the dice.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="input"></param>
    /// <param name="description"></param>
    public class DiceRollJob(string id, string name, DiceRollInput input, string description = "") : Job(id, name, description), IInput<DiceRollInput>, IOutput<DiceRollOutput>
    {
        public DiceRollJob(string id, DiceRollInput input, string description = "") : this(id, id, input, description) { }

        public DiceRollInput Input { get; private set; } = input;

        public DiceRollOutput? Output { get; private set; }

        protected override void OnRun()
        {
            var random = new Random();
            var roll = random.Next(Input.Sides) + 1;
            Output = new DiceRollOutput(roll);
        }

        public override void Update(Job job)
        {
            base.Update(job);
            var diceJob = (job as DiceRollJob)!;
            Input = diceJob.Input;
            Output = diceJob.Output;
        }

        public IDictionary<string, object?> GetInputDict()
            => new Dictionary<string, object?>()
            {
                { nameof(Input.Sides), Input.Sides },
            };

        public IDictionary<string, object?> GetOutputDict()
            => new Dictionary<string, object?>()
            {
                { nameof(Output.Result), Output?.Result },
            };
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

    public class DiceRollJobFactory : IJobWithInputFactory
    {
        public string JobType { get; } = nameof(DiceRollJob);

        public Job CreateJobWithInput(string _, string id, IDictionary<string, object?> input, string name = "", string description = "")
        {
            int sides = input.Get<int?>(nameof(DiceRollInput.Sides)) 
                ?? throw new JobInputMismatchException(nameof(DiceRollInput.Sides), typeof(int));
            return new DiceRollJob(id, name, new(sides), description);
        }
    }
}
