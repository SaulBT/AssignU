const verificarRol = (rolRequerido) => {
    return (req, res, next) => {
        if (!req.usuario) {
            return res.status(403).json({ mensaje: "Acceso denegado, no hay usuario" });
        } else if(!req.usuario.role) {
            return res.status(403).json({ mensaje: "Acceso denegado, no hay rol" });
        } else if(req.usuario.role !== rolRequerido) {
            return res.status(403).json({ mensaje: "Acceso denegado, rol inválido" });
        }
        next();
    };
};

module.exports = { verificarRol };