const {response} = require('express');

const Cuestionario = require('../models/cuestionario');
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
    buscarYEliminarRespuestasCuestionarioAsync
 } = require('../validations/respuesta-validaciones');

const crearCuestionarioAsync = async (data) => {
    try{
        if (!data) {
            return {
                Success: false,
                Message: 'No se enviaron datos'
            }
        }
        idTarea = data.IdTarea;
        preguntas = data.Cuestionario.Preguntas;

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
        idTarea = data.IdTarea;
        preguntas = data.Cuestionario.Preguntas;

        validarCuestionario(idTarea, preguntas);
        const cuestionario = await validarExistenciaCuestionarioAsync(idTarea);

        cuestionario.Preguntas = preguntas;
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
        const idTarea = data.IdTarea;

        validarIdTarea(idTarea);
        const cuestionario = await validarExistenciaCuestionarioAsync(idTarea);
        
        await buscarYEliminarRespuestasCuestionarioAsync(idTarea);
        await cuestionario.deleteOne();
        
        return {
            Success: true
        }
    } catch (err) {
        console.log("Error: " + err.message);
        return manejadorTipoErrores(err, err.message);
    }
}

const obtenerCuestionarioAsync = async (req, res = response, next) => {
    try {
        if (!req.params) {
            throw { statusCode: 400, mensaje: 'No se enviaron parámetros.' };
        }
        const {idTarea} = req.params;

        validarIdTarea(idTarea);
        const cuestionario = await validarExistenciaCuestionarioAsync(idTarea);

        return res.status(200).json(cuestionario);
    } catch (err) {
        next(err);
    }
}

const resolverCuestionarioAsync = async (req, res = response, next) => {
    try {
        if (!req.params) {
            throw { statusCode: 400, mensaje: 'No se enviaron parámetros.' };
        }
        if (!req.body) {
            throw { statusCode: 400, mensaje: 'No se envió un cuerpo.' };
        }
        const { idTarea, idAlumno } = req.params;
        const preguntas = req.body;

        validarRespuesta(idTarea, idAlumno, preguntas);

        calificacion = await calificarRespuestaAsync(preguntas, idTarea);

        const respuestaNueva = new Respuesta({
            IdAlumno: idAlumno,
            IdTarea: idTarea,
            Calificacion: calificacion,
            Preguntas: preguntas
        })
        validarExistenciaRespuestaYGuardarAsync(respuestaNueva);
        return res.status(202).send();
    } catch (err) {
        next(err);
    }
}

const guardarRespuestaCuestionarioAsync = async (req, res = response, next) => {
    try {
        if (!req.params) {
            throw { statusCode: 400, mensaje: 'No se enviaron parámetros.' };
        }
        if (!req.body) {
            throw { statusCode: 400, mensaje: 'No se envió un cuerpo.' };
        }
        const { idTarea, idAlumno } = req.params;
        const preguntas = req.body;
        const calificacion = 0.0;

        validarRespuesta(idTarea, idAlumno, preguntas);

        const respuestaNueva = new Respuesta({
            IdAlumno: idAlumno,
            IdTarea: idTarea,
            Calificacion: calificacion,
            Preguntas: preguntas
        })
        validarExistenciaRespuestaYGuardarAsync(respuestaNueva);
        return res.status(202).send();
    } catch (err) {
        next(err);
    }
}

//Petición HTTP
const obtenerRespuestaCuestionarioHttpAsync = async (req, res = response, next) => {
    try {
        if (!req.params) {
            throw { statusCode: 400, mensaje: 'No se enviaron parámetros.' };
        }
        const { idTarea, idAlumno } = req.params;
        
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
            IdTarea: idTarea,
            IdAlumno: idAlumno
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
            IdTarea: { $in: idTareas }
        }).exec();

        const respuestasDto = respuestas.map( respuesta => ({
            IdAlumno: respuesta.IdAlumno,
            IdTarea: respuesta.IdTarea,
            Calificacion: respuesta.Calificacion
        }));
        return {
            Success: true,
            Respuestas: respuestasDto
        }

    } catch (err) {
        console.log("Error: " + err.message);
        return manejadorTipoErrores(err, err.message);
    }
}

const obtenerRespuestasDeTareaAsync = async (data) =>{
    try{
        if (!data) {
            return {
                Success: false,
                Message: 'No se enviaron datos'
            }
        }
        const idTarea = data.IdTarea;
        validarIdTarea(idTarea);

        const respuestas = await Respuesta.find({
            IdTarea: idTarea
        });
        /*const respuestasEnviadas = respuestas.map( respuesta => ({
            idAlumno: respuesta.IdAlumno,
            idTarea: respuesta.IdTarea,
            Calificacion: respuesta.Calificacion,
            Preguntas: respuesta.Preguntas.map( pregunta => ({
                Texto: pregunta.Texto,
                Opcion: {
                    Texto: pregunta.Opcion.Texto
                },
                Correcta: pregunta.Correcta
            }))
        }));*/

        return {
            Success: true,
            RespuestasDeTarea: respuestas
        };
    } catch (err) {
        console.log("Error: " + err.message);
        return manejadorTipoErrores(err, err.message);
    }
}

const obtenerPreguntasDeTareaAsync = async (data) => {
    try{
        if (!data) {
            return {
                Success: false,
                Message: 'No se enviaron datos'
            }
        }
        const idTarea = data.IdTarea;
        validarIdTarea(idTarea);

        const listaPreguntas = generarListaPreguntasAsync(idTarea);
        return {
            Success: true,
            Preguntas: preguntas
        };
    } catch (err) {
        console.log("Error: " + err.message);
        return manejadorTipoErrores(err, err.message);
    }
}

const calificarRespuestaAsync = async (preguntasResueltas, idTarea) => {
    const cuestionario = await validarExistenciaCuestionarioAsync(idTarea);
    preguntas = cuestionario.Preguntas;
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

const generarListaPreguntasAsync = async (idTarea) => {
    const cuestionario = await Cuestionario.findOne({
        IdTarea: idTarea
    });
    const listaPreguntas = cuestionario.Preguntas.map( pregunta => ({
        Texto: pregunta.Texto
    }));

    return preguntas;
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
    obtenerRespuestas,
    obtenerRespuestasDeTareaAsync,
    obtenerPreguntasDeTareaAsync
};