const mongoose = require('mongoose');

const OpcionSchema = new mongoose.Schema({
    texto: String,
    esCorrecta: Boolean
});

const PreguntaSchema = new mongoose.Schema({
    texto: {
        type: String,
        required: true
    },
    tipo: {
        type: String,
        enum: ['opcion_multiple', 'verdadero_falso'],
        required: true
    },
    opciones: {
        type: [OpcionSchema]
    }
});

const CuestionarioSchema = new mongoose.Schema({
    idTarea: { type: Number },
    preguntas: {
        type: [PreguntaSchema]
    }
});

module.exports = mongoose.model('Cuestionario', CuestionarioSchema);