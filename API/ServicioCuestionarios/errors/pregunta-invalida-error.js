const AppError = require('./app-error');

class PreguntaInvalidaError extends AppError {
    constructor(message) {
        super(message, 400);
    }
}

module.exports = PreguntaInvalidaError;