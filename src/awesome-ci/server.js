var express = require('express');
var app = express();
var port = process.env.PORT || 8080;

var bodyParser = require('body-parser');
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));

const token = process.env.CI_TOKEN;
const registry = process.env.REGISTRY;

var GitHubApi = require("github");
var github = new GitHubApi({
    debug: true,
    protocol: "https",
    host: "api.github.com",
    headers: { "user-agent": "awesome-ci-server" }
});

github.authenticate({
    type: "oauth",
    token: token
});

app.post('/api/ci', function(req, res) {
    var event = req.headers['x-github-event'];
    console.log("Event:" + event);

    if (event === 'pull_request')
    {
      var pull_request = req.body.pull_request;
      var action = req.body.action;

      if (action === 'opened')
      {
        // CONTINUOUS INTEGRATION
        createStatus(pull_request)
            .then(resp => runCI(pull_request, registry));
      }

      if (action === 'closed' && pull_request.merged)
      {
        // CONTINUOUS DELIVERY
        runCD(pull_request, registry, token);
      }
    }
                    
    res.sendStatus(200);
});

// start the server
app.listen(port);
console.log('Server started! At http://localhost:' + port);

const createStatus = (pull_request, state = 'pending') =>
  github.repos.createStatus({
    owner:pull_request.base.repo.owner.login,
    repo:pull_request.base.repo.name,
    sha:pull_request.head.sha,
    state:state 
  });

const runCI = (pull_request, registry) =>
    runScript(getParameters(pull_request, registry),
        () => createStatus(pull_request, 'success'),
        () => createStatus(pull_request, 'failure'));

const runCD = (pull_request, registry, ci_token) =>
    runScript(getParameters(pull_request, registry, ci_token),
        () => console.log('deployment success'),
        () => console.log("deployment failed"));

const getParameters = (pull_request, registry, ci_token = '') =>
            [`${pull_request.base.repo.full_name}.git`,
            pull_request.head.sha.slice(0, 7),
            registry,
            ci_token];

const spawn = require('child_process').spawn;

const runScript = (parameters, successHandler, errorHandler) => {
    const ciScript = spawn('./run.sh', parameters);

    ciScript.stdout.on('data', (data) => {
        console.log(data.toString());
    });

    ciScript.stderr.on('data', (data) => {
        console.log(`ciScript stderr: ${data}`);
    });

    ciScript.on('close', (code) => {
        console.log(`ciScript process exited with code ${code}`);
        if (code === 0) {
            successHandler();
        } else {
            errorHandler();
        }
    });
};
