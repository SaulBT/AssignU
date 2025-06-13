const AppError = require('./app-error');

class ValorInvalidoError extends AppError {
    constructor(message) {
        super(message, 400);
    }
}

module.exports = ValorInvalidoError;