const CuerpoError = require('../errors/cuerpo-error');
const CuestionarioInvalidoError = require('../errors/cuestionario-invalido-error');
const DataError = require('../errors/data-error');
const ParametrosError = require('../errors/parametros-error');
const PreguntaInvalidaError = require('../errors/pregunta-invalida-error');
const ValorInvalidoError = require('../errors/valor-invalido-error');

const manejadorTipoErrores = (error, mensaje) =>{

    if (error instanceof DataError) {
        return {
            Success: false,
            Error: {
                Tipo: "Data",
                Mensaje: `Error en ServicioCuestionarios: ${mensaje}`
            }
        };
    }
    if (error instanceof CuestionarioInvalidoError) {
        return {
            Success: false,
            Error: {
                Tipo: "CuestionarioInvalido",
                Mensaje: `Error en ServicioCuestionarios: ${mensaje}`
            }
        };
    }
    if (error instanceof PreguntaInvalidaError) {
        return {
            Success: false,
            Error: {
                Tipo: "PreguntaInvalida",
                Mensaje: `Error en ServicioCuestionarios: ${mensaje}`
            }
        };
    }
    if (error instanceof ValorInvalidoError) {
        return {
            Success: false,
            Error: {
                Tipo: "ValorInvalido",
                Mensaje: `Error en ServicioCuestionarios: ${mensaje}`
            }
        };
    }

    return {
        Success: false,
        Error: {
            Tipo: "ErrorDesconocido",
            Mensaje: `Error en ServicioCuestionarios: ${mensaje}`
        }
    };
}

module.exports = { manejadorTipoErrores };