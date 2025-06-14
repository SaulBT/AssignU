const ValorInvalidoError = require('../errors/valor-invalido-error');
const PreguntaInvalidaError = require('../errors/pregunta-invalida-error');
const Respuesta = require('../models/respuesta-cuestionario');

function validarRespuesta(idTarea, idAlumno, preguntas) {
    validarIdTarea(idTarea);
    validarIdAlumno(idAlumno);
    validarDatosPreguntas(preguntas);

    preguntas.forEach(pregunta => {
        validarPregunta(pregunta);
    });
}

function validarPregunta(pregunta, index) {
    validarTextoPregunta(pregunta, index);
    validarDatoCorrecta(pregunta, index);
    validarTextoOpcion(pregunta.opcion, index);
}

const validarExistenciaRespuestaYGuardarAsync = async (respuesta) => {
    const respuestaExistente = await Respuesta.findOne({idTarea: respuesta.idTarea, idAlumno: respuesta.idAlumno});
    if (respuestaExistente != null) {
        respuestaExistente.calificacion = respuesta.calificacion;
        respuestaExistente.preguntas = respuesta.preguntas;
        await respuestaExistente.save();
    } else {
        await respuesta.save();
    }
};

const buscarRespuestasCuestionarioAsync = async (idTarea) => {
    console.log("Se buscan las respuestas")
    const respuestas = await Respuesta.find({idTarea});
    if (respuestas != null) {
        console.log("Se borran");
        respuestas.forEach(async respuesta => {
            await respuesta.deleteOne();
        });
    }
}

//Validaciones respuesta
const validarIdTarea = (idTarea) => {
    if (idTarea <= 0) {
        throw new ValorInvalidoError("La idTarea es inválida");
    }
}

const validarIdAlumno = (idAlumno) => {
    if (idAlumno <= 0) {
        throw new ValorInvalidoError("La idAlumno es inválida: No puede ser 0 o menor");
    } else if (!idAlumno) {
        throw new ValorInvalidoError("La idAlumno es inválida: Valor nulo")
    }
}

const validarDatosPreguntas = (preguntas) => {
    if (preguntas == null) {
        throw new ValorInvalidoError("Las preguntas son inválidas: Valor nulo");
    } else if (!Array.isArray(preguntas)) {
        throw new ValorInvalidoError("Las preguntas son inválidas: No es array");
    }
}

//Validar pregunta
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

const validarDatoCorrecta = (respuesta, index) => {
    const correcta = respuesta.correcta;

    if (correcta == null) {
        throw new PreguntaInvalidaError(`El valor 'correcta' de la pregunta ${index + 1} es inválido: Valor nulo`);
    } else if (typeof correcta !== 'boolean') {
        throw new PreguntaInvalidaError(`El valor 'correcta' de la pregunta ${index + 1} es inválido: No es boolean`);
    }
}

const validarTextoOpcion = (opcion, index) => {
    const texto = opcion.texto;
    
    if(!texto) {
        throw new PreguntaInvalidaError(`El texto de la opción ${index + 1} es inválido: Valor nulo`);
    } else if (typeof texto !== 'string') {
        throw new PreguntaInvalidaError(`El texto de la opción ${index + 1} es inválido: No es string`);
    } else if (texto.trim() === '') {
        throw new PreguntaInvalidaError(`El texto de la opción ${index + 1} es inválido: Cadena vacía`);
    }
}

module.exports = {
    validarRespuesta,
    validarExistenciaRespuestaYGuardarAsync,
    validarIdTarea,
    validarIdAlumno,
    buscarRespuestasCuestionarioAsync
 };