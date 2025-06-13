const jwt = require('jsonwebtoken');

const verificarToken = (req, res, next) => {
    const token = req.headers.authorization?.split(" ")[1];

    if (!token) {
        return res.status(401).json({ mensaje: "Acceso denegado, token requerido" });
    }

    try {
        const decoded = jwt.verify(token, process.env.KEY);
        req.usuario = decoded;
        //console.log(req.usuario);
        next();
    } catch (error) {
        return res.status(403).json({ mensaje: "Token inválido" });
    }
};

module.exports = { verificarToken };