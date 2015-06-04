using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using GeekQuiz.Models;

namespace WebApplication2.Controllers
{
    public class TriviaController : ApiController
    {
        private TriviaContext db = new TriviaContext();

        private async Task<TriviaQuestion> NextQuestionAsync(string userId)
        {
            var lastQuestionId = await db.TriviaAnswers
                .Where(x => x.UserId == userId)
                .GroupBy(x => x.QuestionId)
                .Select(x => new {QuestionId = x.Key, Count = x.Count()})
                .OrderByDescending(x => new {x.Count, QuestionId = x.QuestionId})
                .Select(x => x.QuestionId)
                .FirstOrDefaultAsync();

            var questionCount = await db.TriviaQuestions.CountAsync();

            var nextQuestionId = (lastQuestionId%questionCount) + 1;
            return await db.TriviaQuestions.FindAsync(CancellationToken.None, nextQuestionId);
        }

        [ResponseType(typeof (TriviaQuestion))]
        public async Task<IHttpActionResult> Get()
        {
            var userId = User.Identity.Name;
            TriviaQuestion nextQuestion = await this.NextQuestionAsync(userId);
            if (nextQuestion == null)
            {
                return NotFound();
            }
            return Ok(nextQuestion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
