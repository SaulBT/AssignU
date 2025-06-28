
package com.AssignU.servicios.usuarios;

import com.AssignU.models.Usuarios.IniciarSesionDTO;
import com.AssignU.models.Usuarios.RespuestaIniciarSesionDTO;
import com.AssignU.utils.ApiCliente;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.ExcepcionHTTP;
import java.util.HashMap;
import java.util.Map;

public class ServicioLogin {
    
    public static HashMap<String, Object> iniciarSesion(String tipoUsuario, String nombreUsuarioOCorreo, String contrasenia) {
        
        IniciarSesionDTO dto = new IniciarSesionDTO(tipoUsuario, nombreUsuarioOCorreo, contrasenia);
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);

        try {

            Map<String, String> headers = Map.of("Content-Type", "application/json");

            RespuestaIniciarSesionDTO respuesta = ApiCliente.enviarSolicitud(
                "/usuarios/login",
                "POST",
                dto,
                headers,
                RespuestaIniciarSesionDTO.class
            );

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Inicio de sesión exitoso.");
            resultado.put(Constantes.KEY_RESPUESTA, respuesta);

        } catch (ExcepcionHTTP e) {
            switch (e.getCodigo()) {
                case 401:
                    resultado.put(Constantes.KEY_MENSAJE, "Credenciales incorrectas.");
                    break;
                case 500:
                    resultado.put(Constantes.KEY_MENSAJE, "Error interno del servidor. Intente más tarde.");
                    break;
                default:
                    resultado.put(Constantes.KEY_MENSAJE, "Error del servidor (" + e.getCodigo() + "): " + e.getMessage());
                    break;
            }
        } catch (Exception e) {
            resultado.put(Constantes.KEY_MENSAJE, e.getMessage());
        }

        return resultado;
    }

}
