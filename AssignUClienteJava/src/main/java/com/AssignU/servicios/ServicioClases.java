
package com.AssignU.servicios;

import com.AssignU.models.Clases.ClaseDTO;
import com.AssignU.models.Usuarios.Sesion;
import com.AssignU.utils.ApiCliente;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.ExcepcionHTTP;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class ServicioClases {
    public static HashMap<String, Object> obtenerClasesDeUsuario() {
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);

        Sesion sesion = Sesion.getSesion();
        
        String endpoint;
        if(sesion.esDocente()){
            endpoint = "/clases/docentes/" + sesion.getIdUsuario() + "/clases";
        }else{
            endpoint = "/clases/alumnos/" + sesion.getIdUsuario() + "/clases";
        }

        Map<String, String> headers = new HashMap<>();
        headers.put("Content-Type", "application/json");
        headers.put("Authorization", "Bearer " + sesion.getJwt());

        try {
            List<ClaseDTO> clases = ApiCliente.enviarSolicitudLista(endpoint, "GET", null, headers, ClaseDTO.class);

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Lista de clases obtenida exitosamente.");
            resultado.put(Constantes.KEY_RESPUESTA, clases);

        } catch (ExcepcionHTTP e) {
            switch (e.getCodigo()) {
                case 401:
                    resultado.put(Constantes.KEY_MENSAJE, "No autorizado. Su sesión puede haber expirado.");
                    break;
                case 403:
                    resultado.put(Constantes.KEY_MENSAJE, "Acceso denegado.");
                    break;
                case 500:
                    resultado.put(Constantes.KEY_MENSAJE, "Error interno del servidor.");
                    break;
                default:
                    resultado.put(Constantes.KEY_MENSAJE, "Error HTTP (" + e.getCodigo() + "): " + e.getMessage());
                    break;
            }
        } catch (Exception e) {
            resultado.put(Constantes.KEY_MENSAJE, "Error de red o inesperado: " + e.getMessage());
        }

        return resultado;
    }
}
