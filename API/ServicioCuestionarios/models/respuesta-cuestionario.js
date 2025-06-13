const mongoose = require('mongoose');

const OpicionElegidaSchema = new mongoose.Schema({
    texto: {
        type: String,
        required: true
    }
})

const PreguntasResueltasSchema = new mongoose.Schema({
    texto: {
        type: String,
        required: true
    },
    opcion: {
        type: OpicionElegidaSchema,
        required: true
    },
    correcta:{
        type: Boolean,
        required: true
    }
})

const RespuestaSchema = new mongoose.Schema({
    idAlumno: {
        type: Number,
        required: true
    },
    idTarea: {
        type: Number,
        required: true
    },
    calificacion: {
        type: Number
    },
    preguntas: {
        type: [PreguntasResueltasSchema],
        required: true
    }
})

module.exports = mongoose.model('Respuesta', RespuestaSchema);