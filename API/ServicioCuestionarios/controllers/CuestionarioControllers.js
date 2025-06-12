const {response} = require('express');
const mongoose = require('mongoose');
const asyncHandler = require('../middleware/asyncHandler');

const Cuestionario = require('../models/Cuestionario');
const { ManejarErrores } = require('../validations/ManejarErrores');

const {
    validarCuestionario,
    validarExistenciaCuestionarioAsync,
    validarIdTarea
 } = require('../validations/CuestionarioValidations');

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
        return ManejarErrores(err, err.message);
    }
}
/*
const editarCuestionarioAsync = async (req, res = response, next) => {
    try {
        if (!req.body) {
            throw { statusCode: 400, mensaje: 'Cuerpo de la solicitud vacío o mal formado' };
        }
        const {idTarea, preguntas} = req.body;

        validarCuestionario(idTarea, preguntas);
        const cuestionario = await validarExistenciaCuestionarioAsync(idTarea);

        cuestionario.preguntas = preguntas;
        await cuestionario.save();
        res.status(200).json(cuestionario);
    } catch(err) {
        next(err);
    }
};

const eliminarCuestionarioAsync = async (req, res = response, next) => {
    try {
        if (!req.body) {
            throw { statusCode: 400, mensaje: 'Cuerpo de la solicitud vacío o mal formado' };
        }
        const {idTarea} = req.body;

        validarIdTarea(idTarea);
        const cuestionario = await validarExistenciaCuestionarioAsync(idTarea);

        await cuestionario.deleteOne();
        return res.status(204).send();
    } catch (err) {
        next(err);
    }
}
*/
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

module.exports = {
    crearCuestionarioAsync,
    //editarCuestionarioAsync,
    //eliminarCuestionarioAsync,
    obtenerCuestionarioAsync
};