
package com.AssignU.servicios;

import com.AssignU.models.Tareas.TareaDTO;
import com.AssignU.models.Usuarios.Sesion;
import com.AssignU.utils.ApiCliente;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.ExcepcionHTTP;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class ServicioTareas {
    //Crear Tarea
    
    //Editar Tarea
    
    //Borrar Tarea
    
    public static HashMap<String, Object> obtenerTareas(int idClase) {
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);

//        Sesion sesion = Sesion.getSesion();
        
        String endpoint = "/tareas/clases/" + idClase + "/tareas";

//        Map<String, String> headers = new HashMap<>();
//        headers.put("Authorization", "Bearer " + sesion.getJwt());

        try {
            List<TareaDTO> tareas = ApiCliente.enviarSolicitudLista(endpoint, 
                    "GET", 
                    null, 
                    null, 
                    TareaDTO.class);

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Lista de tareas obtenida exitosamente.");
            resultado.put(Constantes.KEY_RESPUESTA, tareas);

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
    
    //Obtener Estadísticas de Tarea
}
