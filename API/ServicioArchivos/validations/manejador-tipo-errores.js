const manejadorTipoErrores = (error, mensaje) =>{

    return {
        Success: false,
        Error: {
            Tipo: "ErrorDesconocido",
            Mensaje: mensaje
        }
    };
}

module.exports = { manejadorTipoErrores };