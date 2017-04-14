# quiz-cli

CLI for testing quiz app. Allows to generate test submissions.

## Variables
- `QUIZ_URL`: Host of the Quiz api
- `ITERATIONS`: Number of submissions to be performed
- `INTERVAL`: Interval between submissions

## Example usage
Send 100 submissions to local instance of quiz api, with 500ms interval between submissions

```sh
export QUIZ_URL=localhost
export ITERATIONS=100
export INTERVAL=500

npm i
npm run start
```