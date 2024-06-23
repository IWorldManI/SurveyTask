using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyTask.Database;
using SurveyTask.Database.Models;
using SurveyTask.ModelsDto;

namespace SurveyTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ResultController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<int?>> SaveResult(ResultDto resultDto)
        {
            var surveyExists = await _context.Surveys.AnyAsync(s => s.SurveyId == resultDto.SurveyId);
            if (!surveyExists)
            {
                return NotFound("Survey not found.");
            }

            var interview = await _context.Interviews
                .Where(i => i.InterviewId == resultDto.InterviewId)
                .FirstOrDefaultAsync();

            if (interview == null)
            {
                interview = new Interview
                {
                    InterviewId = resultDto.InterviewId,
                    SurveyId = resultDto.SurveyId,
                    UserName = resultDto.UserName, 
                    StartTime = DateTime.UtcNow 
                };

                _context.Interviews.Add(interview);
                await _context.SaveChangesAsync();
            }

            var result = new Result
            {
                InterviewId = resultDto.InterviewId,
                QuestionId = resultDto.QuestionId,
                AnswerId = resultDto.AnswerId
            };

            _context.Results.Add(result);
            await _context.SaveChangesAsync();

            var nextQuestionId = await GetNextQuestionId(resultDto.InterviewId);

            return nextQuestionId;
        }

        private async Task<int?> GetNextQuestionId(int interviewId)
        {
            var lastResult = await _context.Results
                .Where(r => r.InterviewId == interviewId)
                .OrderByDescending(r => r.ResultId)
                .FirstOrDefaultAsync();

            if (lastResult != null)
            {
                var nextQuestion = await _context.Questions
                    .Where(q => q.SurveyId == lastResult.Interview.SurveyId && q.QuestionId > lastResult.QuestionId)
                    .OrderBy(q => q.QuestionId)
                    .FirstOrDefaultAsync();

                return nextQuestion?.QuestionId;
            }

            var firstQuestion = await _context.Questions
                .Where(q => q.SurveyId == interviewId)
                .OrderBy(q => q.QuestionId)
                .FirstOrDefaultAsync();

            return firstQuestion?.QuestionId;
        }
    }
}
