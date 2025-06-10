const express = require('express');
const router = express.Router();

const {
    crearCuestionarioAsync,
    editarCuestionarioAsync,
    eliminarCuestionarioAsync,
    obtenerCuestionarioAsync
} = require('../controllers/CuestionarioControllers');

/**
 * @swagger
 * /cuestionario:
 *   post:
 *     summary: Crear un nuevo cuestionario
 *     tags: [Cuestionarios]
 *     requestBody:
 *       required: true
 *       content:
 *         application/json:
 *           schema:
 *             $ref: '#/components/schemas/CuestionarioInput'
 *     responses:
 *       201:
 *         description: Cuestionario creado
 *       400:
 *         description: Error de validación
 */
router.post('/', crearCuestionarioAsync);
router.put('/:id', editarCuestionarioAsync);
router.delete('/:id', eliminarCuestionarioAsync);
router.get('/:id', obtenerCuestionarioAsync);

module.exports = router;