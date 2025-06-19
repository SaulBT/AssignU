const AppError = require('./app-error');

class CuerpoError extends AppError {
    constructor(message) {
        super(message, 400);
    }
}

module.exports = CuerpoError;