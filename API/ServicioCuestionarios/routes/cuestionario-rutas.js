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

/**
 * @swagger
 * /{idTarea}:
 *   get:
 *     summary: Obtener un Cuestionario
 *     tags:
 *       - Cuestionarios
 *     parameters:
 *       - in: path
 *         name: idTarea
 *         required: true
 *         schema:
 *           type: int
 *         description: ID de la Tarea del Cuestionario
 *     responses:
 *       200:
 *         description: Respuesta encontrada
 *       400:
 *         description: Parámetros inválidos
 *       404:
 *         description: No se encontró la Rrespuesta
 */
router.get('/:idTarea', obtenerCuestionarioAsync);
/**
 * @swagger
 * /{idTarea}/calificar:
 *   post:
 *     summary: Enviar una Respuesta para ser calificada
 *     tags:
 *       - Cuestionarios
 *     parameters:
 *       - in: path
 *         name: idTarea
 *         required: true
 *         schema:
 *           type: int
 *         description: ID de la Tarea
 *     requestBody:
 *       required: true
 *       content:
 *         application/json:
 *           schema:
 *             type: object
 *             properties:
 *               preguntas:
 *                 type: array
 *                 items:
 *                   type: object
 *                   properties:
 *                     Texto:
 *                       type: string
 *                     Opcion:
 *                       type: object
 *                       properties:
 *                          Texto:
 *                            type: string
 *                     Correcta:
 *                       type: boolean
 *     responses:
 *       202:
 *         description: Respuesta guardada
 *       401:
 *         description: No autorizado
 *       403:
 *         description: Rol no permitido
 *       400:
 *         description: Datos inválidos
 *       404:
 *         description: No se encontró al Cuestionario
 */
router.post('/:idTarea/calificar', verificarToken, verificarRol('alumno'), resolverCuestionarioAsync);
/**
 * @swagger
 * /{idTarea}/guardar-estado:
 *   post:
 *     summary: Enviar una Respuesta para ser guardada
 *     tags:
 *       - Cuestionarios
 *     parameters:
 *       - in: path
 *         name: idTarea
 *         required: true
 *         schema:
 *           type: int
 *         description: ID de la Tarea
 *     requestBody:
 *       required: true
 *       content:
 *         application/json:
 *           schema:
 *             type: object
 *             properties:
 *               preguntas:
 *                 type: array
 *                 items:
 *                   type: object
 *                   properties:
 *                     Texto:
 *                       type: string
 *                     Opcion:
 *                       type: object
 *                       properties:
 *                          Texto:
 *                            type: string
 *                     Correcta:
 *                       type: boolean
 *     responses:
 *       202:
 *         description: Respuesta guardada
 *       401:
 *         description: No autorizado
 *       403:
 *         description: Rol no permitido
 *       400:
 *         description: Datos inválidos
 *       404:
 *         description: No se encontró al Cuestionario
 */
router.post('/:idTarea/guardar-resultado', verificarToken, verificarRol('alumno'), guardarRespuestaCuestionarioAsync);
/**
 * @swagger
 * /{idTarea}/{idAlumno}:
 *   get:
 *     summary: Obtener la Respuesta de un Alumno a un Cuestionario
 *     tags:
 *       - Cuestionarios
 *     parameters:
 *       - in: path
 *         name: idTarea
 *         required: true
 *         schema:
 *           type: int
 *         description: ID de la Tarea del Cuestionario
 *       - in: path
 *         name: idAlumno
 *         required: true
 *         schema:
 *           type: int
 *         description: ID del Alumno
 *     responses:
 *       200:
 *         description: Respuesta encontrada
 *       404:
 *         description: No se encontró la respuesta
 */
router.get('/:idTarea/:idAlumno', obtenerRespuestaCuestionarioHttpAsync);

module.exports = router;