const AppError = require('../errors/AppError');

class CuestionarioInvalidoError extends AppError {
    constructor(message) {
        super(message, 400);
    }
}

module.exports = CuestionarioInvalidoError;