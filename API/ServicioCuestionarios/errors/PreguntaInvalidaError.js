const AppError = require('../errors/AppError');

class PreguntaInvalidaError extends AppError {
    constructor(message) {
        super(message, 400);
    }
}

module.exports = PreguntaInvalidaError;