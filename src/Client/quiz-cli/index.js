var quiz = require('./defaultQuiz');
const fetch = require('node-fetch');

const url = `http://${process.env.QUIZ_URL || 'localhost'}/commands/quiz`;
const iterations = process.env.ITERATIONS || 100;
const interval = process.env.INTERVAL || 500;

const getQuiz = () => fetch(`${url}`).then(response => {
  if (response.status === 204) {
    return null;
  }
  if (response.status === 200) {
    return response.json();
  }
});

const startQuiz = () => fetch(`${url}`, {
  method: 'POST',
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json'
  },
  body: JSON.stringify( quiz )
}).then(response => {
  if (response.status === 204) {
    return null;
  }
  if (response.status === 200) {
    return response.json();
  }
}).catch(err => console.log(err));

const postQuizAnswers = (quizId, answers) => fetch(`${url}/${quizId}`, {
  method: 'PUT',
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

function vote(index, quizId, questions) {
  return new Promise((resolve, reject) => {
    const answers = questions.map(q => {
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
  if (data.quizState === 'Created') {
    startQuiz().then(data => {
      console.log('start quiz...');
      startvoting(data);
    });
  } else {
    startvoting(data);
  }
}).catch(err => {
  console.log(err);
  process.exit(1);
});

function startvoting(data) {
    console.log('start voting...');
    const requests = [];
    for (let i = 0; i <= iterations; i++) {
      requests.push(vote(i, data.quizId, data.questions));
    }
    Promise.all(requests);
    console.log('finished voting...');
}
