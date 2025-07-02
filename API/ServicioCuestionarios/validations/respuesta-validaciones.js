const ValorInvalidoError = require('../errors/valor-invalido-error');
const Respuesta = require('../models/respuesta-cuestionario');
const CampoObligatorioError = require('../errors/campo-obligatorio-error');

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
    validarTextoOpcion(pregunta.Opcion, index);
}

const validarExistenciaRespuestaYGuardarAsync = async (respuesta) => {
    const respuestaExistente = await Respuesta.findOne({IdTarea: respuesta.IdTarea, IdAlumno: respuesta.IdAlumno});
    if (respuestaExistente != null) {
        respuestaExistente.Calificacion = respuesta.Calificacion;
        respuestaExistente.Preguntas = respuesta.Preguntas;
        respuestaExistente.Estado = respuesta.Estado;
        await respuestaExistente.save();
    } else {
        await respuesta.save();
    }
};

const buscarYEliminarRespuestasCuestionarioAsync = async (idTarea) => {
    const respuestas = await Respuesta.find({IdTarea: idTarea});
    if (respuestas != null) {
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
        throw new CampoObligatorioError("La idAlumno no puede ser 0 o menor");
    } else if (!idAlumno) {
        throw new ValorInvalidoError("La idAlumno es nulo")
    }
}

const validarDatosPreguntas = (preguntas) => {
    if (preguntas == null) {
        throw new CampoObligatorioError("Las preguntas son nulas");
    } else if (!Array.isArray(preguntas)) {
        throw new ValorInvalidoError("Las preguntas no son array");
    }
}

//Validar pregunta
const validarTextoPregunta = (pregunta, index) => {
    const texto = pregunta.Texto;
    
    if(!texto) {
        throw new CampoObligatorioError(`El texto de una pregunta es nulo`);
    } else if (typeof texto !== 'string') {
        throw new ValorInvalidoError(`El texto de una pregunta no es una cadena`);
    } else if (texto.trim() === '') {
        throw new CampoObligatorioError(`El texto de una pregunta es una cadena vacía`);
    }
}

const validarDatoCorrecta = (respuesta, index) => {
    const correcta = respuesta.Correcta;

    if (correcta == null) {
        throw new CampoObligatorioError(`El valor 'correcta' de una pregunta es nulo`);
    } else if (typeof correcta !== 'boolean') {
        throw new ValorInvalidoError(`El valor 'correcta' de una pregunta no es un booleano`);
    }
}

const validarTextoOpcion = (opcion, index) => {
    const texto = opcion.Texto;
    
    if(!texto) {
        throw new CampoObligatorioError(`El texto de una opción es nulo`);
    } else if (typeof texto !== 'string') {
        throw new ValorInvalidoError(`El texto de una opción no es string`);
    } else if (texto.trim() === '') {
        throw new CampoObligatorioError(`El texto de una opción es una cadena vacía`);
    }
}

module.exports = {
    validarRespuesta,
    validarExistenciaRespuestaYGuardarAsync,
    validarIdTarea,
    validarIdAlumno,
    buscarYEliminarRespuestasCuestionarioAsync
 };