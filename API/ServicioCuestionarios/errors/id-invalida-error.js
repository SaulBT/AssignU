const AppError = require('./app-error');

class IdInvalidaError extends AppError {
    constructor(message) {
        super(message, 400);
    }
}

module.exports = IdInvalidaError;