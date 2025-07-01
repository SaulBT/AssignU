
package com.AssignU.servicios.usuarios;

import com.AssignU.models.Perfil.CambiarContraseniaDTO;
import com.AssignU.models.Perfil.EstadisticasPerfilDTO;
import com.AssignU.models.Usuarios.Alumno.ActualizarAlumnoDTO;
import com.AssignU.models.Usuarios.Alumno.AlumnoDTO;
import com.AssignU.models.Usuarios.Alumno.RegistrarAlumnoDTO;
import com.AssignU.models.Usuarios.Sesion;
import com.AssignU.utils.ApiCliente;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.ExcepcionHTTP;
import java.util.HashMap;
import java.util.Map;

public class ServicioAlumnos {
    public static HashMap<String, Object> registrarAlumno(String nombreCompleto, String nombreUsuario, String contrasenia, String correo, int idGrado) {
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);

        Map<String, String> headers = Map.of("Content-Type", "application/json");
        
        try {
            RegistrarAlumnoDTO registrarAlumnoDto = new RegistrarAlumnoDTO(nombreCompleto, nombreUsuario, contrasenia, correo, idGrado);

            ApiCliente.enviarSolicitud("/usuarios/alumnos", 
                    "POST", 
                    registrarAlumnoDto, 
                    headers, 
                    Object.class);

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Alumno registrado con éxito.\n Por favor inicia sesión para continuar.");

        } catch (ExcepcionHTTP e) {
            switch (e.getCodigo()) {
                case 400 -> resultado.put(Constantes.KEY_MENSAJE, "Datos inválidos. Verifique los campos.");
                case 409 -> resultado.put(Constantes.KEY_MENSAJE, "El nombre de usuario ya existe.");
                case 500 -> resultado.put(Constantes.KEY_MENSAJE, "Error del servidor. Intente más tarde.");
                default -> resultado.put(Constantes.KEY_MENSAJE, "Error HTTP (" + e.getCodigo() + "): " + e.getMessage());
            }
        } catch (Exception e) {
            resultado.put(Constantes.KEY_MENSAJE, e.getMessage());
        }

        return resultado;
    }
    
    public static HashMap<String, Object> obtenerAlumno(){
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);

        Sesion sesion = Sesion.getSesion();
        String endpoint = "/usuarios/alumnos/" + sesion.getIdUsuario();

        Map<String, String> headers = new HashMap<>();
        headers.put("Authorization", "Bearer " + sesion.getJwt());

        try {
            AlumnoDTO alumnoDto = ApiCliente.enviarSolicitud(
                endpoint,
                "GET",
                null,
                headers,
                AlumnoDTO.class
            );

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Perfil del alumno obtenido correctamente.");
            resultado.put(Constantes.KEY_RESPUESTA, alumnoDto);

        } catch (ExcepcionHTTP e) {
            switch (e.getCodigo()) {
                case 401 -> resultado.put(Constantes.KEY_MENSAJE, "No autorizado. Inicie sesión nuevamente.");
                case 404 -> resultado.put(Constantes.KEY_MENSAJE, "No se encontró el perfil del alumno.");
                case 500 -> resultado.put(Constantes.KEY_MENSAJE, "Error interno del servidor.");
                default -> resultado.put(Constantes.KEY_MENSAJE, "Error HTTP (" + e.getCodigo() + "): " + e.getMessage());
            }
        } catch (Exception e) {
            resultado.put(Constantes.KEY_MENSAJE, "Error de red o inesperado: " + e.getMessage());
        }

        return resultado;
    }
    
    public static HashMap<String, Object> actualizarAlumno(String nombreCompleto, String nombreUsuario, int idGrado) {
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);

        Sesion sesion = Sesion.getSesion();
        String endpoint = "/usuarios/alumnos/" + sesion.getIdUsuario();

        Map<String, String> headers = new HashMap<>();
        headers.put("Content-Type", "application/json");
        headers.put("Authorization", "Bearer " + sesion.getJwt());

        try {
            ActualizarAlumnoDTO actualizarAlumnoDto = new ActualizarAlumnoDTO(nombreCompleto, nombreUsuario, idGrado);
            
            ApiCliente.enviarSolicitud(endpoint, 
                    "PUT", 
                    actualizarAlumnoDto, 
                    headers, 
                    Object.class);

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Alumno actualizado con éxito.");

        } catch (ExcepcionHTTP e) {
            switch (e.getCodigo()) {
                case 400 -> resultado.put(Constantes.KEY_MENSAJE, "Datos inválidos. Verifique los campos.");
                case 404 -> resultado.put(Constantes.KEY_MENSAJE, "No se encontró el alumno.");
                case 409 -> resultado.put(Constantes.KEY_MENSAJE, "El nombre de usuario ya existe.");
                case 500 -> resultado.put(Constantes.KEY_MENSAJE, "Error del servidor. Intente más tarde.");
                default -> resultado.put(Constantes.KEY_MENSAJE, "Error HTTP (" + e.getCodigo() + "): " + e.getMessage());
            }
        } catch (Exception e) {
            resultado.put(Constantes.KEY_MENSAJE, "Error de red o inesperado: " + e.getMessage());
        }

        return resultado;
    }
    
    public static HashMap<String, Object> cambiarContrasenia(String contraseniaActual, String contraseniaNueva) {
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);

        Sesion sesion = Sesion.getSesion();
        String endpoint = "/usuarios/alumnos/" + sesion.getIdUsuario() + "/contrasenia";

        Map<String, String> headers = new HashMap<>();
        headers.put("Authorization", "Bearer " + sesion.getJwt());
        headers.put("Content-Type", "application/json");

        try {
            CambiarContraseniaDTO cambiarContraseniaDto = new CambiarContraseniaDTO(contraseniaActual, contraseniaNueva);
            
            ApiCliente.enviarSolicitud(endpoint, 
                    "PUT", 
                    cambiarContraseniaDto, 
                    headers, 
                    Object.class);

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Contraseña actualizada correctamente.");

        } catch (ExcepcionHTTP e) {
            switch (e.getCodigo()) {
                case 400 -> resultado.put(Constantes.KEY_MENSAJE, "Datos inválidos. Verifique la contraseña actual y nueva.");
                case 401 -> resultado.put(Constantes.KEY_MENSAJE, "No autorizado. Por favor, inicie sesión nuevamente.");
                case 409 -> resultado.put(Constantes.KEY_MENSAJE, "Conflicto: La contraseña nueva no puede ser igual a la anterior.");
                case 500 -> resultado.put(Constantes.KEY_MENSAJE, "Error del servidor. Intente más tarde.");
                default -> resultado.put(Constantes.KEY_MENSAJE, "Error HTTP (" + e.getCodigo() + "): " + e.getMessage());
            }
        } catch (Exception e) {
            resultado.put(Constantes.KEY_MENSAJE, "Error de red o inesperado: " + e.getMessage());
        }

        return resultado;
    }
    
    public static HashMap<String, Object> obtenerEstadisticasPerfil() {
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);

        Sesion sesion = Sesion.getSesion();
        String endpoint = "/usuarios/alumnos/" + sesion.getIdUsuario() + "/estadisticas";

        Map<String, String> headers = new HashMap<>();
        headers.put("Authorization", "Bearer " + sesion.getJwt());

        try {
            EstadisticasPerfilDTO estadisticas = ApiCliente.enviarSolicitud(
                endpoint,
                "GET",
                null,
                headers,
                EstadisticasPerfilDTO.class
            );

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Estadísticas obtenidas correctamente.");
            resultado.put(Constantes.KEY_RESPUESTA, estadisticas);

        } catch (ExcepcionHTTP e) {
            switch (e.getCodigo()) {
                case 401 -> resultado.put(Constantes.KEY_MENSAJE, "No autorizado. Por favor, inicie sesión nuevamente.");
                case 404 -> resultado.put(Constantes.KEY_MENSAJE, "No se encontraron estadísticas.");
                case 500 -> resultado.put(Constantes.KEY_MENSAJE, "Error interno del servidor.");
                default -> resultado.put(Constantes.KEY_MENSAJE, "Error HTTP (" + e.getCodigo() + "): " + e.getMessage());
            }
        } catch (Exception e) {
            resultado.put(Constantes.KEY_MENSAJE, "Error de red o inesperado: " + e.getMessage());
        }

        return resultado;
    }
}
