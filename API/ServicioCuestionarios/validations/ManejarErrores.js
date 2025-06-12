const CuestionarioInvalidoError = require('../errors/CuestionarioInvalidoError');
const PreguntaInvalidaError = require('../errors/PreguntaInvalidaError');
const ValorInvalidoError = require('../errors/ValorInvalidoError');

const ManejarErrores = (error, mensaje) =>{

    if (error instanceof CuestionarioInvalidoError) {
        return {
            Success: false,
            Error: {
                Tipo: "CuestionarioInvalido",
                Mensaje: mensaje
            }
        };
    }
    if (error instanceof PreguntaInvalidaError) {
        return {
            Success: false,
            Error: {
                Tipo: "PreguntaInvalida",
                Mensaje: mensaje
            }
        };
    }
    if (error instanceof ValorInvalidoError) {
        return {
            Success: false,
            Error: {
                Tipo: "ValorInvalido",
                Mensaje: mensaje
            }
        };
    }

    return {
        Success: false,
        Error: {
            Tipo: "ErrorDesconocido",
            Mensaje: mensaje
        }
    };
}

module.exports = { ManejarErrores };