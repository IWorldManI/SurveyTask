namespace SurveyTask.Database.Models
{
    public class Result
    {
        public int ResultId { get; set; }
        public int InterviewId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public Interview Interview { get; set; }
        public Question Question { get; set; }
        public Answer Answer { get; set; }
    }
}
