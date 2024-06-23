namespace SurveyTask.ModelsDto
{
    public class QuestionDto
    {
        public int QuestionId { get; set; }
        public int SurveyId { get; set; } 
        public string Text { get; set; }
        public ICollection<AnswerDto> Answers { get; set; }
    }
}
