namespace SurveyTask.Database.Models
{
    public class Survey
    {
        public int SurveyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<Interview> Interviews { get; set; }
    }
}
