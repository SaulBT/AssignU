const AppError = require('./app-error');

class CuestionarioInvalidoError extends AppError {
    constructor(message) {
        super(message, 400);
    }
}

module.exports = CuestionarioInvalidoError;