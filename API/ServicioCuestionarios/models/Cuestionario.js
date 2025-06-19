const mongoose = require('mongoose');

const OpcionSchema = new mongoose.Schema({
    Texto: String,
    EsCorrecta: Boolean
});

const PreguntaSchema = new mongoose.Schema({
    Texto: {
        type: String,
        required: true
    },
    Tipo: {
        type: String,
        enum: ['opcion_multiple', 'verdadero_falso'],
        required: true
    },
    Opciones: {
        type: [OpcionSchema]
    }
});

const CuestionarioSchema = new mongoose.Schema({
    IdTarea: { type: Number },
    Preguntas: {
        type: [PreguntaSchema]
    }
});

module.exports = mongoose.model('Cuestionario', CuestionarioSchema);