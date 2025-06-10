const AppError = require('../errors/AppError');

class ValorInvalidoError extends AppError {
    constructor(message) {
        super(message, 400);
    }
}

module.exports = ValorInvalidoError;