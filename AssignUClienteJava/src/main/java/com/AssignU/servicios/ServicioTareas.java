
package com.AssignU.servicios;

import com.AssignU.models.Tareas.CrearTareaDTO;
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
    public static HashMap<String, Object> crearTarea(CrearTareaDTO crearTarea) {
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);

        Sesion sesion = Sesion.getSesion();

        Map<String, String> headers = new HashMap<>();
        headers.put("Authorization", "Bearer " + sesion.getJwt());

        try {
            TareaDTO tarea = ApiCliente.enviarSolicitud("/tareas/tareas", 
                    "POST", 
                    crearTarea, 
                    headers, 
                    TareaDTO.class);

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Tarea creada exitosamente.");
            resultado.put(Constantes.KEY_RESPUESTA, tarea);

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
    
    //Editar Tarea
    
    //Borrar Tarea
    public static HashMap<String, Object> borrarTarea(int idTarea){
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);
        
        Sesion sesion = Sesion.getSesion();
        
        Map<String, String> headers = new HashMap<>();
        headers.put("Authorization", "Bearer " + sesion.getJwt());
        
        String endpoint = "/tareas/" + idTarea;
        
        try {
            ApiCliente.enviarSolicitud(endpoint, 
                    "DELETE", 
                    null, 
                    headers, 
                    Object.class);
            
            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Tarea borrada exitosamente.");
            
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
            System.out.println("Error: " + e.getMessage());
        }

        return resultado;
    }
    
    //Obtener Estadísticas de Tarea
}
