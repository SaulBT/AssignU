
package com.AssignU.servicios;

import com.AssignU.models.Clases.ClaseDTO;
import com.AssignU.models.Clases.CrearClaseDTO;
import com.AssignU.models.Clases.Estadisticas.EstadisticasClaseDTO;
import com.AssignU.models.Clases.RegistroDTO;
import com.AssignU.models.Usuarios.Sesion;
import com.AssignU.utils.ApiCliente;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.ExcepcionHTTP;
import java.net.URLEncoder;
import java.nio.charset.StandardCharsets;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class ServicioClases {
    public static HashMap<String, Object> crearClase(String nombreClase){
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);
        
        Sesion sesion = Sesion.getSesion();
        
        Map<String, String> headers = new HashMap<>();
        headers.put("Content-Type", "application/json");
        headers.put("Authorization", "Bearer " + sesion.getJwt());
        
        CrearClaseDTO crearClaseDto = new CrearClaseDTO(nombreClase);
        
        try {
            ClaseDTO respuesta = ApiCliente.enviarSolicitud("/clases/clases", 
                    "POST", 
                    crearClaseDto, 
                    headers, 
                    ClaseDTO.class);
            
            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Clase " + nombreClase + " creada exitosamente.");
            resultado.put(Constantes.KEY_RESPUESTA, respuesta);
            
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
    
    public static HashMap<String, Object> actualizarClase(int idClase, String nombreClase){
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);
        
        Sesion sesion = Sesion.getSesion();
        
        Map<String, String> headers = new HashMap<>();
        headers.put("Content-Type", "application/json");
        headers.put("Authorization", "Bearer " + sesion.getJwt());
        
        CrearClaseDTO crearClaseDto = new CrearClaseDTO(nombreClase);
        
        String endpoint = "/clases/clases/" + idClase;
        
        try {
            ClaseDTO respuesta = ApiCliente.enviarSolicitud(endpoint, 
                    "PUT", 
                    crearClaseDto, 
                    headers, 
                    ClaseDTO.class);
            
            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Clase " + nombreClase + " actualizada exitosamente.");
            resultado.put(Constantes.KEY_RESPUESTA, respuesta);
            
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
    
    public static HashMap<String, Object> borrarClase(int idClase, String nombreClase){
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);
        
        Sesion sesion = Sesion.getSesion();
        
        Map<String, String> headers = new HashMap<>();
        headers.put("Authorization", "Bearer " + sesion.getJwt());
        
        String endpoint = "/clases/clases/" + idClase;
        
        try {
            ApiCliente.enviarSolicitud(endpoint, 
                    "DELETE", 
                    null, 
                    headers, 
                    Object.class);
            
            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Clase " + nombreClase + " borrada exitosamente.");
            
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
    
    public static HashMap<String, Object> actualizarUltimaConexion(int idClase, LocalDateTime fechaVisualizacion) {
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);

        Sesion sesion = Sesion.getSesion();

        String endpoint = "/clases/clases/" + idClase + "/ultima-conexion?fechaVisualizacion=" + fechaVisualizacion;

        Map<String, String> headers = new HashMap<>();
        headers.put("Content-Type", "application/json");
        headers.put("Authorization", "Bearer " + sesion.getJwt());

        try {
            ApiCliente.enviarSolicitud(endpoint, "PUT", null, headers, Object.class);

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Última conexión actualizada correctamente.");

        } catch (ExcepcionHTTP e) {
            switch (e.getCodigo()) {
                case 400 -> resultado.put(Constantes.KEY_MENSAJE, "Datos inválidos. Verifique la fecha de visualización.");
                case 401 -> resultado.put(Constantes.KEY_MENSAJE, "Sesión no autorizada o expirada.");
                case 404 -> resultado.put(Constantes.KEY_MENSAJE, "Clase no encontrada.");
                case 500 -> resultado.put(Constantes.KEY_MENSAJE, "Error interno del servidor.");
                default -> resultado.put(Constantes.KEY_MENSAJE, "Error HTTP (" + e.getCodigo() + "): " + e.getMessage());
            }

        } catch (Exception e) {
            resultado.put(Constantes.KEY_MENSAJE, "Error de red o inesperado: " + e.getMessage());
        }

        return resultado;
    }
    
    public static HashMap<String, Object> obtenerClase(int idClase) {
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);
        
        String endpoint = "/clases/clases/" + idClase;
        
        try {
            ClaseDTO clase = ApiCliente.enviarSolicitud(endpoint, "GET", null, null, ClaseDTO.class);

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Última conexión actualizada correctamente.");
            resultado.put(Constantes.KEY_RESPUESTA, clase);

        } catch (ExcepcionHTTP e) {
            switch (e.getCodigo()) {
                case 400 -> resultado.put(Constantes.KEY_MENSAJE, "Datos inválidos. Verifique la fecha de visualización.");
                case 401 -> resultado.put(Constantes.KEY_MENSAJE, "Sesión no autorizada o expirada.");
                case 404 -> resultado.put(Constantes.KEY_MENSAJE, "Clase no encontrada.");
                case 500 -> resultado.put(Constantes.KEY_MENSAJE, "Error interno del servidor.");
                default -> resultado.put(Constantes.KEY_MENSAJE, "Error HTTP (" + e.getCodigo() + "): " + e.getMessage());
            }

        } catch (Exception e) {
            resultado.put(Constantes.KEY_MENSAJE, "Error de red o inesperado: " + e.getMessage());
        }

        return resultado;
    }
    
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
        headers.put("Authorization", "Bearer " + sesion.getJwt());

        try {
            List<ClaseDTO> clases = ApiCliente.enviarSolicitudLista(endpoint, 
                    "GET", 
                    null, 
                    headers, 
                    ClaseDTO.class);

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Lista de clases obtenida exitosamente.");
            resultado.put(Constantes.KEY_RESPUESTA, clases);

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
    
    public static HashMap<String, Object> unirseAClase(String codigo) {
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);
        
        Sesion sesion = Sesion.getSesion();
        
        Map<String, String> headers = new HashMap<>();
        headers.put("Authorization", "Bearer " + sesion.getJwt());
        
        String endpoint = "/clases/clases/" + codigo + "/unirse";
        
        try {
            ClaseDTO respuesta = ApiCliente.enviarSolicitud(endpoint, 
                    "POST", 
                    null, 
                    headers, 
                    ClaseDTO.class);
            
            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Te uniste a " + respuesta.getNombreClase() + " exitosamente.");
            resultado.put(Constantes.KEY_RESPUESTA, respuesta);
            
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
    
    public static HashMap<String, Object> salirDeClase(int idClase, String nombreClase) {
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);
        
        Sesion sesion = Sesion.getSesion();
        
        Map<String, String> headers = new HashMap<>();
        headers.put("Authorization", "Bearer " + sesion.getJwt());
        
        String endpoint = "/clases/alumnos/" + sesion.getIdUsuario() + "/clases/" + idClase + "/salir";
        
        try {
            ApiCliente.enviarSolicitud(endpoint, 
                    "DELETE", 
                    null, 
                    headers, 
                    ClaseDTO.class);
            
            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Saliste de la clase '" + nombreClase + "' exitosamente.");
            
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
    
    public static HashMap<String, Object> obtenerRegistroDeAlumno(int idAlumno, int idClase) {
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);
        
        Sesion sesion = Sesion.getSesion();
        
        Map<String, String> headers = new HashMap<>();
        headers.put("Authorization", "Bearer " + sesion.getJwt());
        
        String endpoint = "/clases/alumnos/" + idAlumno + "/clases/" + idClase;
        
        try {
            RegistroDTO registroDto = ApiCliente.enviarSolicitud(endpoint, 
                    "GET", 
                    null, 
                    headers, 
                    RegistroDTO.class);
            
            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Registro obtenido.");
            resultado.put(Constantes.KEY_RESPUESTA, registroDto);
            
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
    
    public static HashMap<String, Object> obtenerEstadisticasClase(int idClase) {
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);
        
        String endpoint = "/clases/clases/" + idClase + "/estadisticas";
        
        try {
            EstadisticasClaseDTO respuesta = ApiCliente.enviarSolicitud(endpoint, 
                    "GET",
                    null, 
                    null, 
                    EstadisticasClaseDTO.class);
            
            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Estadisticas de clase obtenidas exitosamente.");
            resultado.put(Constantes.KEY_RESPUESTA, respuesta);
            
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
}
