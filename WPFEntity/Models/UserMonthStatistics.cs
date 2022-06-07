
namespace WPFEntity.Models
{
    public class UserMonthStatistics
    {
        public string Name { get; }

        public double AvgSteps { get; }

        public int MaxSteps { get; }

        public int MinSteps { get; }

        public UserMonthStatistics(string name, double avgSteps, int minSteps, int maxSteps)
        {
            Name = name;
            AvgSteps = avgSteps;
            MaxSteps = maxSteps;
            MinSteps = minSteps;
        }
    }
}
