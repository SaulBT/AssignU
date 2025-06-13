const express = require('express');
const router = express.Router();

const { verificarRol } = require('../middleware/verificar-rol');
const { verificarToken } = require('../middleware/verificar-token');

const {
    obtenerCuestionarioAsync,
    resolverCuestionarioAsync,
    guardarRespuestaCuestionarioAsync,
    obtenerRespuestaCuestionarioHttpAsync

} = require('../controllers/cuestionario-controladores');

router.get('/:id', obtenerCuestionarioAsync);
router.post('/:id/calificar', verificarToken, verificarRol('alumno'), resolverCuestionarioAsync);
router.post('/:id/guardar-resultado', verificarToken, verificarRol('alumno'), guardarRespuestaCuestionarioAsync);
router.get('/:codigo/:id', obtenerRespuestaCuestionarioHttpAsync);

module.exports = router;