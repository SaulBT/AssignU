const AppError = require('../errors/AppError');

function errorHandler(err, req, res, next) {
    if (err.statusCode && err.mensaje) {
        return res.status(err.statusCode).json({ error: err.mensaje });
    }

    if (err instanceof AppError) {
        return res.status(err.statusCode).json({ error: err.message });
    }

    console.error('Error inesperado: ', err);
    return res.status(500).json({
        error: 'Error interno del servidor',
        detalle: err.message
    });
}

module.exports = errorHandler;