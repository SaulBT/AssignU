const AppError = require('./app-error');

class CampoObligatorioError extends AppError {
    constructor(message) {
        super(message, 400);
    }
}

module.exports = CampoObligatorioError;