const ValorInvalidoError = require('../errors/ValorInvalidoError');
const CuestionarioInvalidoError = require('../errors/CuestionarioInvalidoError');
const PreguntaInvalidaError = require('../errors/PreguntaInvalidaError');
const Cuestionario = require('../models/Cuestionario');

function validarCuestionario(idTarea, preguntas) {
    validarIdTarea(idTarea);
    validarDatosPreguntas(preguntas);
    validarCantidadPreguntas(preguntas);

    preguntas.forEach(pregunta => {
        validarPregunta(pregunta);
    });
}

function validarPregunta(pregunta, index) {
    validarTipoPregunta(pregunta, index);
    validarTextoPregunta(pregunta, index);
    validarCantidadOpciones(pregunta, index);
}

const validarExistenciaCuestionarioAsync = async (idTarea) => {
    const cuestionario = await Cuestionario.findOne({idTarea: idTarea});
    if (cuestionario == null) {
        throw { statusCode: 404, mensaje: `No se encontró un Cuestionario con la IdTarea ${idTarea}` };
    } else {
        return cuestionario;
    }
};

//Validaciones de todo el cuestionario

const validarIdTarea = (idTarea) => {
    if (idTarea <= 0) {
        throw new ValorInvalidoError("La id es inválida");
    }
}

const validarDatosPreguntas = (preguntas) => {
    if (preguntas == null) {
        throw new ValorInvalidoError("Las preguntas son inválidas: Valor nulo");
    } else if (!Array.isArray(preguntas)) {
        throw new ValorInvalidoError("Las preguntas son inválidas: No es array");
    }
}

const validarCantidadPreguntas = (preguntas) => {
    const numeroPreguntas = preguntas.length;
    if (numeroPreguntas < 1) {
        throw new CuestionarioInvalidoError("El Cuestionario debe de tener más de una pregunta");
    } else if (numeroPreguntas > 50) {
        throw new CuestionarioInvalidoError("El Cuestionario tiene demasiadas preguntas");
    }
}

//Validaciones de cada pregunta

const validarTipoPregunta = (pregunta, index) => {
    const tiposValidos = ['opcion_multiple', 'verdadero_falso'];
    const tipo = pregunta.tipo;

    if(!tipo) {
        throw new PreguntaInvalidaError(`El tipo de la pregunta ${index + 1} es inválido: Valor nulo`);
    } else if (typeof tipo !== 'string') {
        throw new PreguntaInvalidaError(`El tipo de la pregunta ${index + 1} es inválido: No es string`);
    } else if (tipo.trim() === '') {
        throw new PreguntaInvalidaError(`El tipo de la pregunta ${index + 1} es inválido: Cadena vacía`);
    } else if (!tiposValidos.includes(tipo)) {
        throw new PreguntaInvalidaError(`El tipo de la pregunta ${index + 1} es inválido: Tipo desconocido`);
    }
}

const validarTextoPregunta = (pregunta, index) => {
    const texto = pregunta.texto;

    if(!texto) {
        throw new PreguntaInvalidaError(`El tipo de la pregunta ${index + 1} es inválido: Valor nulo`);
    } else if (typeof texto !== 'string') {
        throw new PreguntaInvalidaError(`El tipo de la pregunta ${index + 1} es inválido: No es string`);
    } else if (texto.trim() === '') {
        throw new PreguntaInvalidaError(`El tipo de la pregunta ${index + 1} es inválido: Cadena vacía`);
    }
}

const validarCantidadOpciones = (pregunta, index) => {
    const numeroOpciones = pregunta.opciones.length;

    if (numeroOpciones < 2) {
        throw new PreguntaInvalidaError(`La pregunta ${index + 1} debe tener mínimo dos opciones`);
    } else if (numeroOpciones > 4) {
        throw new PreguntaInvalidaError(`La pregunta ${index + 1} debe tener máximo cuatro opciones`);
    }
}

module.exports = {
    validarCuestionario,
    validarExistenciaCuestionarioAsync,
    validarIdTarea
}