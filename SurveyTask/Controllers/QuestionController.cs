using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyTask.Database;
using SurveyTask.ModelsDto;

namespace SurveyTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public QuestionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{questionId}")]
        public async Task<ActionResult<QuestionDto>> GetQuestion(int questionId)
        {
            var question = await _context.Questions
                .Where(q => q.QuestionId == questionId)
                .Select(q => new QuestionDto
                {
                    QuestionId = q.QuestionId,
                    Text = q.Text,
                    Answers = q.Answers.Select(a => new AnswerDto
                    {
                        AnswerId = a.AnswerId,
                        Text = a.Text
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (question == null)
            {
                return NotFound();
            }

            return question;
        }
    }
}
