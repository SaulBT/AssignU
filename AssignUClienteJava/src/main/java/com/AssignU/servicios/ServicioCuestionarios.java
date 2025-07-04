
package com.AssignU.servicios;

import com.AssignU.models.Cuestionarios.CuestionarioDTO;
import com.AssignU.models.Usuarios.Sesion;
import com.AssignU.utils.ApiCliente;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.ExcepcionHTTP;
import java.util.HashMap;
import java.util.Map;

public class ServicioCuestionarios {
    //obtener cuestionario
    public static HashMap<String, Object> obtenerCuestionario(int idTarea) {
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);

        String endpoint = "/cuestionarios/cuestionarios/" + idTarea;

        try {
            CuestionarioDTO cuestionario = ApiCliente.enviarSolicitud(endpoint, 
                    "GET", 
                    null, 
                    null, 
                    CuestionarioDTO.class);

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Cuestionario obtenido exitosamente.");
            resultado.put(Constantes.KEY_RESPUESTA, cuestionario);

        } catch (ExcepcionHTTP e) {
            switch (e.getCodigo()) {
                case 401 -> resultado.put(Constantes.KEY_MENSAJE, "No autorizado. Su sesión puede haber expirado.");
                case 403 -> resultado.put(Constantes.KEY_MENSAJE, "Acceso denegado.");
                case 500 -> resultado.put(Constantes.KEY_MENSAJE, "Error interno del servidor.");
                default -> resultado.put(Constantes.KEY_MENSAJE, "Error HTTP (" + e.getCodigo() + "): " + e.getMessage());
            }
        } catch (Exception e) {
            resultado.put(Constantes.KEY_MENSAJE, "Error de red o inesperado: " + e.getMessage());
        }

        return resultado;
    }
    
    //enviar respuesta para ser calificada?
    
    //enviar respuesta para ser guardada
    
    //obtener respuesta de alumno
}
