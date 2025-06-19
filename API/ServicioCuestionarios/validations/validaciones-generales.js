const CuerpoError = require("../errors/cuerpo-error");
const DataError = require("../errors/data-error");
const ParametrosError = require("../errors/parametros-error");

const validarData = (data) => {
    if (!data) {
        throw new DataError("No se enviaron datos");
    }
}

const validarParametros = (req) => {
    if (!req.params){
        throw new ParametrosError("No se enviaron parámetros");
    }
}

const validarBody = (req) => {
    if (!req.body){
        throw new CuerpoError("No se envió cuerpo con datos");
    }
}

module.exports = {
    validarData,
    validarParametros,
    validarBody
}