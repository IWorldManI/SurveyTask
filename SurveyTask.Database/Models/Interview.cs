namespace SurveyTask.Database.Models
{
    public class Interview
    {
        public int InterviewId { get; set; }
        public int SurveyId { get; set; }
        public string UserName { get; set; }
        public DateTime StartTime { get; set; }
        public Survey Survey { get; set; }
        public ICollection<Result> Results { get; set; }
    }
}
