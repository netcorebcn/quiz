const fetch = require('node-fetch');

const url = `http://${process.env.QUIZ_URL}:81/quiz/`;
const iterations = process.env.ITERATIONS || 100;
const interval = process.env.INTERVAL || 500;

const getQuiz = () => fetch(url).then(response => {
  if (response.status === 204) {
    return null;
  }
  if (response.status === 200) {
    return response.json();
  }
});

const postQuizAnswers = (quizId, answers) => fetch(`${url}${quizId}`, {
  method: 'POST',
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json'
  },
  body: JSON.stringify({ answers })
}).catch(err => console.log(err));

const random = max => {
  min = Math.ceil(0);
  max = Math.floor(max);
  return Math.floor(Math.random() * (max - min + 1)) + min;
};

const randomOption = options => options[random(options.length - 1)];

function vote(index, quizId, quizModel) {
  return new Promise((resolve, reject) => {
    const answers = quizModel.questions.map(q => {
      return {
        questionId: q.id,
        optionId: randomOption(q.options).id
      };
    });
    setTimeout(
      () => {
        console.log(`voting for quizId:${quizId}`);
        postQuizAnswers(quizId, answers)
          .then(() => {
            resolve();
          })
          .catch(() => {
            resolve();
          });
      },
      interval * index
    );
  });
}

console.log('loading quiz...');
getQuiz().then(data => {
  console.log('start voting...');
  const requests = [];
  for (let i = 0; i <= iterations; i++) {
    requests.push(vote(i, data.quizId, data.quizModel));
  }
  Promise.all(requests);
  console.log('finished voting...');
});
