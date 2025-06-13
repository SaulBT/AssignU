const {response} = require('express');

const Cuestionario = require('../models/Cuestionario');
const Respuesta = require('../models/respuesta-cuestionario');
const { manejadorTipoErrores } = require('../validations/manejador-tipo-errores');

const {
    validarCuestionario,
    validarExistenciaCuestionarioAsync,
    validarIdTarea
 } = require('../validations/cuestionario-validaciones');
 const {
    validarRespuesta,
    validarExistenciaRespuestaYGuardarAsync,
    validarIdAlumno,
    buscarRespuestasCuestionarioAsync
 } = require('../validations/respuesta-validaciones');

const crearCuestionarioAsync = async (data) => {
    try{
        if (!data) {
            return {
                Success: false,
                Message: 'No se enviaron datos'
            }
        }
        idTarea = data.idTarea;
        preguntas = data.cuestionario.preguntas;

        validarCuestionario(idTarea, preguntas);

        const cuestionario = new Cuestionario({
            idTarea: idTarea,
            preguntas: preguntas
        });
        await cuestionario.save();
        return {
            Success: true
        }
    } catch(err) {
        console.log("Error: " + err.message);
        return manejadorTipoErrores(err, err.message);
    }
}

const editarCuestionarioAsync = async (data) => {
    try {
        if (!data) {
            return {
                Success: false,
                Message: 'No se enviaron datos'
            }
        }
        idTarea = data.idTarea;
        preguntas = data.cuestionario.preguntas;

        validarCuestionario(idTarea, preguntas);
        const cuestionario = await validarExistenciaCuestionarioAsync(idTarea);

        cuestionario.preguntas = preguntas;
        await cuestionario.save();
        return {
            Success: true
        }
    } catch(err) {
        console.log("Error: " + err.message);
        return manejadorTipoErrores(err, err.message);
    }
};

const eliminarCuestionarioAsync = async (data) => {
    try {
        if (!data) {
            return {
                Success: false,
                Message: 'No se enviaron datos'
            }
        }
        const idTarea = data.idTarea;

        validarIdTarea(idTarea);
        const cuestionario = await validarExistenciaCuestionarioAsync(idTarea);
        await buscarRespuestasCuestionarioAsync(idTarea);

        await cuestionario.deleteOne();
        return {
            Success: true
        }
    } catch (err) {
        console.log("Error: " + err.message);
        return manejadorTipoErrores(err, err.message);
    }
}

const obtenerCuestionarioAsync = async (req, res = repsonse, next) => {
    try {
        if (!req.body) {
            throw { statusCode: 400, mensaje: 'Cuerpo de la solicitud vacío o mal formado' };
        }
        const {idTarea} = req.body;

        validarIdTarea(idTarea);
        const cuestionario = await validarExistenciaCuestionarioAsync(idTarea);

        return res.status(200).json(cuestionario);
    } catch (err) {
        next(err);
    }
}

const resolverCuestionarioAsync = async (req, res = response, next) => {
    try {
        if (!req.body) {
            throw { statusCode: 400, mensaje: 'Cuerpo de la solicitud vacío o mal formado' };
        }
        const { idTarea, idAlumno, preguntas } = req.body;

        validarRespuesta(idTarea, idAlumno, preguntas);

        calificacion = await calificarRespuestaAsync(preguntas, idTarea);

        const respuestaNueva = new Respuesta({
            idAlumno: idAlumno,
            idTarea: idTarea,
            calificacion: calificacion,
            preguntas: preguntas
        })
        validarExistenciaRespuestaYGuardarAsync(respuestaNueva);
        return res.status(204).send();
    } catch (err) {
        next(err);
    }
}

const guardarRespuestaCuestionarioAsync = async (req, res = response, next) => {
    try {
        if (!req.body) {
            throw { statusCode: 400, mensaje: 'Cuerpo de la solicitud vacío o mal formado' };
        }
        const { idTarea, idAlumno, preguntas } = req.body;
        const calificacion = 0.0;

        validarRespuesta(idTarea, idAlumno, preguntas);

        const respuestaNueva = new Respuesta({
            idAlumno: idAlumno,
            idTarea: idTarea,
            calificacion: calificacion,
            preguntas: preguntas
        })
        validarExistenciaRespuestaYGuardarAsync(respuestaNueva);
        return res.status(204).send();
    } catch (err) {
        next(err);
    }
}

//Petición HTTP
const obtenerRespuestaCuestionarioHttpAsync = async (req, res = response, next) => {
    try {
        if (!req.body) {
            throw { statusCode: 400, mensaje: 'Cuerpo de la solicitud vacío o mal formado' };
        }
        const { idTarea, idAlumno } = req.body;
        
        const mensaje = await obtenerRespuestaCuestionarioAsync({
            idTarea: idTarea,
            idAlumno: idAlumno
        })

        if (!mensaje.Success){
            throw { statusCode: 400, mensaje: mensaje.Error.Mensaje };
        }

        const respuesta = mensaje.Respuesta;
        return res.status(200).json(respuesta);
    } catch (err) {
        next(err);
    }
}
//Método para RabbitMQ
const obtenerRespuestaCuestionarioAsync = async (data) => {
    try {
        if (!data) {
            return {
                Success: false,
                Message: 'No se enviaron datos'
            }
        }
        const idTarea = data.idTarea;
        const idAlumno = data.idAlumno;
        validarIdTarea(idTarea);
        validarIdAlumno(idAlumno);

        const respuesta = await Respuesta.findOne({
            idTarea: idTarea,
            idAlumno: idAlumno
        });
        return {
            Success: true,
            Respuesta: respuesta
        };
    } catch (err) {
        console.log("Error: " + err.message);
        return manejadorTipoErrores(err, err.message);
    }
}

const obtenerRespuestas = async (data) => {
    try {
        if (!data) {
            return {
                Success: false,
                Message: 'No se enviaron datos'
            }
        }
        const idTareas = data.IdTareas;
        const respuestas = await Respuesta.find({
            idTarea: { $in: idTareas }
        }).exec();

        const respuestasDto = respuestas.map( respuesta => ({
            IdAlumno: respuesta.idAlumno,
            IdTarea: respuesta.idTarea,
            Calificacion: respuesta.calificacion
        }));
        console.log("idAlumno:" + respuestasDto[0].IdAlumno);
        return {
            Success: true,
            Respuestas: respuestasDto
        }

    } catch (err) {
        console.log("Error: " + err.message);
        return manejadorTipoErrores(err, err.message);
    }
}

const calificarRespuestaAsync = async (preguntasResueltas, idTarea) => {
    const cuestionario = await validarExistenciaCuestionarioAsync(idTarea);
    preguntas = cuestionario.preguntas;
    const cantidadPreguntas = preguntas.length;
    const cantidadPreguntasResueltas = preguntasResueltas.length;
    var cantidadPreguntasCorrectas = 0;
    var calificacion = 0.0;

    for (i = 0; i < cantidadPreguntas; i++){
        for (x = 0; x < cantidadPreguntasResueltas; x++){
            if (preguntas[i].texto == preguntasResueltas[x].texto){
                if (preguntasResueltas[x].correcta == true){
                    cantidadPreguntasCorrectas++;
                }
            }
        }
    }

    calificacion = cantidadPreguntasCorrectas > 0 ? (cantidadPreguntasCorrectas * 10) / cantidadPreguntas : 0;
    return calificacion;
}

module.exports = {
    crearCuestionarioAsync,
    editarCuestionarioAsync,
    eliminarCuestionarioAsync,
    obtenerCuestionarioAsync,
    resolverCuestionarioAsync,
    guardarRespuestaCuestionarioAsync,
    obtenerRespuestaCuestionarioHttpAsync,
    obtenerRespuestaCuestionarioAsync,
    obtenerRespuestas
};