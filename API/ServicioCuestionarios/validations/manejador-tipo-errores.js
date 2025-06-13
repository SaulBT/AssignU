const CuestionarioInvalidoError = require('../errors/cuestionario-invalido-error');
const PreguntaInvalidaError = require('../errors/pregunta-invalida-error');
const ValorInvalidoError = require('../errors/valor-invalido-error');

const manejadorTipoErrores = (error, mensaje) =>{

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

module.exports = { manejadorTipoErrores };