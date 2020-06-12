namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetProjectOverallProgress
{
    public class ProjectOverallView
    {
        private int _completedTasks;

        private int _refusedTasks;

        private int _totalTasks;

        public int CompletedTasks
        {
            get => _completedTasks;
            set => _completedTasks = value;
        }

        public int RefusedTasks
        {
            get => _refusedTasks;
            set => _refusedTasks = value;
        }

        public int TotalTasks
        {
            get => _totalTasks;
            set => _totalTasks = value;
        }

        public int DaysFromStartOfProject { get; set; }


        public int SuccessRatio => _totalTasks > 0 ? 100 * _completedTasks / _totalTasks : 0;

        public int CompletionRatio => _totalTasks > 0 ? 100 * (_completedTasks + _refusedTasks) / _totalTasks : 0;
    }
}
