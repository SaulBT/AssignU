const AppError = require('./app-error');

class DataError extends AppError {
    constructor(message) {
        super(message, 400);
    }
}

module.exports = DataError;