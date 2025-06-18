const mongoose = require('mongoose');

const OpicionElegidaSchema = new mongoose.Schema({
    Texto: {
        type: String,
        required: true
    }
})

const PreguntasResueltasSchema = new mongoose.Schema({
    Texto: {
        type: String,
        required: true
    },
    Opcion: {
        type: OpicionElegidaSchema,
        required: true
    },
    Correcta:{
        type: Boolean,
        required: true
    }
})

const RespuestaSchema = new mongoose.Schema({
    IdAlumno: {
        type: Number,
        required: true
    },
    IdTarea: {
        type: Number,
        required: true
    },
    Calificacion: {
        type: Number
    },
    Preguntas: {
        type: [PreguntasResueltasSchema],
        required: true
    }
})

module.exports = mongoose.model('Respuesta', RespuestaSchema);