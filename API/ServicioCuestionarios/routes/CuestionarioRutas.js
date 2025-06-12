const express = require('express');
const router = express.Router();

const {
    obtenerCuestionarioAsync
} = require('../controllers/CuestionarioControllers');

router.get('/:id', obtenerCuestionarioAsync);

module.exports = router;