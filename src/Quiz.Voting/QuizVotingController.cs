using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quiz.Messages;

namespace Quiz.Api.Controllers
{
    [Route("[controller]")]
    public class QuizController
    {
        public QuizController()
        {
        }

        [HttpGet]
        public QuizModel Get()
        {
            return new QuizModel(new List<Question>{
                    new Question("What .NET Standard implements net461", new List<QuestionOption> {
                        new QuestionOption(".NET Standard 1.8"),
                        new QuestionOption(".NET Standard 1.6"),
                        new QuestionOption(".NET Standard 2.0"),
                    })
            });
        }

        [HttpPost("answer")]
        public async Task Answer([FromBody]Messages.QuestionAnswerCommand answer)
        {
            await Task.FromResult(true);
            // distributed transaction
            // eventStore.Save(event);
            // bus.Publish(event);
        }
    }
}