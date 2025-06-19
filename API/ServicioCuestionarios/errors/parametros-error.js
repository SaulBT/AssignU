const AppError = require('./app-error');

class ParametrosError extends AppError {
    constructor(message) {
        super(message, 400);
    }
}

module.exports = ParametrosError;