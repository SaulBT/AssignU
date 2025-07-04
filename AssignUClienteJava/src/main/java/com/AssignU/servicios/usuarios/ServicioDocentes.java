
package com.AssignU.servicios.usuarios;

import com.AssignU.models.Perfil.CambiarContraseniaDTO;
import com.AssignU.models.Usuarios.Docente.ActualizarDocenteDTO;
import com.AssignU.models.Usuarios.Docente.DocenteDTO;
import com.AssignU.models.Usuarios.Docente.RegistrarDocenteDTO;
import com.AssignU.models.Usuarios.Sesion;
import com.AssignU.utils.ApiCliente;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.ExcepcionHTTP;
import java.util.HashMap;
import java.util.Map;

public class ServicioDocentes {
    public static HashMap<String, Object> registrarDocente(String nombreCompleto, String nombreUsuario, String contrasenia, String correo, int idGrado) {
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);

        try {
            RegistrarDocenteDTO registrarDocenteDto = new RegistrarDocenteDTO(nombreCompleto, nombreUsuario, contrasenia, correo, idGrado);
            Map<String, String> headers = Map.of("Content-Type", "application/json");

            ApiCliente.enviarSolicitud("/usuarios/docentes", 
                    "POST", 
                    registrarDocenteDto, 
                    headers, 
                    Object.class);

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Docente registrado con éxito.\n Por favor inicia sesión para continuar.");

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
    
    public static HashMap<String, Object> obtenerDocente(){
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);

        Sesion sesion = Sesion.getSesion();
        String endpoint = "/usuarios/docentes/" + sesion.getIdUsuario();

        Map<String, String> headers = new HashMap<>();
        headers.put("Authorization", "Bearer " + sesion.getJwt());

        try {
            DocenteDTO docente = ApiCliente.enviarSolicitud(
                endpoint,
                "GET",
                null,
                headers,
                DocenteDTO.class
            );

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Perfil del docente obtenido correctamente.");
            resultado.put(Constantes.KEY_RESPUESTA, docente);

        } catch (ExcepcionHTTP e) {
            switch (e.getCodigo()) {
                case 401 -> resultado.put(Constantes.KEY_MENSAJE, "No autorizado. Inicie sesión nuevamente.");
                case 404 -> resultado.put(Constantes.KEY_MENSAJE, "No se encontró el perfil del docente.");
                case 500 -> resultado.put(Constantes.KEY_MENSAJE, "Error interno del servidor.");
                default -> resultado.put(Constantes.KEY_MENSAJE, "Error HTTP (" + e.getCodigo() + "): " + e.getMessage());
            }
        } catch (Exception e) {
            resultado.put(Constantes.KEY_MENSAJE, "Error de red o inesperado: " + e.getMessage());
        }
        return resultado;
    }
    
    public static HashMap<String, Object> obtenerNombreDocente(int idDocente){
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);

        Sesion sesion = Sesion.getSesion();
        String endpoint = "/usuarios/docentes/" + idDocente;

        Map<String, String> headers = new HashMap<>();
        headers.put("Authorization", "Bearer " + sesion.getJwt());

        try {
            DocenteDTO docente = ApiCliente.enviarSolicitud(
                endpoint,
                "GET",
                null,
                headers,
                DocenteDTO.class
            );
            
            String nombreDocente = docente.nombreCompleto;

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Nombre del docente obtenido correctamente.");
            resultado.put(Constantes.KEY_RESPUESTA, nombreDocente);

        } catch (ExcepcionHTTP e) {
            switch (e.getCodigo()) {
                case 401 -> resultado.put(Constantes.KEY_MENSAJE, "No autorizado. Inicie sesión nuevamente.");
                case 404 -> resultado.put(Constantes.KEY_MENSAJE, "No se encontró el perfil del docente.");
                case 500 -> resultado.put(Constantes.KEY_MENSAJE, "Error interno del servidor.");
                default -> resultado.put(Constantes.KEY_MENSAJE, "Error HTTP (" + e.getCodigo() + "): " + e.getMessage());
            }
        } catch (Exception e) {
            resultado.put(Constantes.KEY_MENSAJE, "Error de red o inesperado: " + e.getMessage());
        }
        return resultado;
    }
    
    public static HashMap<String, Object> actualizarDocente(String nombreCompleto, String nombreUsuario, int idGrado) {
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);

        Sesion sesion = Sesion.getSesion();
        String endpoint = "/usuarios/docentes/" + sesion.getIdUsuario();

        Map<String, String> headers = new HashMap<>();
        headers.put("Content-Type", "application/json");
        headers.put("Authorization", "Bearer " + sesion.getJwt());

        try {
            ActualizarDocenteDTO actualizarDocenteDto = new ActualizarDocenteDTO(nombreUsuario, nombreCompleto, idGrado);
            
            ApiCliente.enviarSolicitud(endpoint, 
                    "PUT", 
                    actualizarDocenteDto, 
                    headers, 
                    Object.class);

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Docente actualizado con éxito.");

        } catch (ExcepcionHTTP e) {
            switch (e.getCodigo()) {
                case 400 -> resultado.put(Constantes.KEY_MENSAJE, "Datos inválidos. Verifique los campos.");
                case 404 -> resultado.put(Constantes.KEY_MENSAJE, "No se encontró el docente.");
                case 409 -> resultado.put(Constantes.KEY_MENSAJE, "El nombre de usuario ya existe.");
                case 500 -> resultado.put(Constantes.KEY_MENSAJE, "Error del servidor. Intente más tarde.");
                default -> resultado.put(Constantes.KEY_MENSAJE, "Error HTTP (" + e.getCodigo() + "): " + e.getMessage());
            }
        } catch (Exception e) {
            resultado.put(Constantes.KEY_MENSAJE, "Error de red o inesperado: " + e.getMessage());
        }

        return resultado;
    }
    
    public static HashMap<String, Object> borrarDocente() {
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);

        Sesion sesion = Sesion.getSesion();
        String endpoint = "/usuarios/docentes/" + sesion.getIdUsuario();

        Map<String, String> headers = new HashMap<>();
        headers.put("Authorization", "Bearer " + sesion.getJwt());

        try {
            ApiCliente.enviarSolicitud(endpoint, 
                    "DELETE", 
                    null, 
                    headers, 
                    Object.class);

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Docente borrado con éxito.");

        } catch (ExcepcionHTTP e) {
            switch (e.getCodigo()) {
                case 400 -> resultado.put(Constantes.KEY_MENSAJE, "Datos inválidos. Verifique los campos.");
                case 404 -> resultado.put(Constantes.KEY_MENSAJE, "No se encontró el docente.");
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
        String endpoint = "/usuarios/docentes/" + sesion.getIdUsuario() + "/contrasenia";

        Map<String, String> headers = new HashMap<>();
        headers.put("Content-Type", "application/json");
        headers.put("Authorization", "Bearer " + sesion.getJwt());

        try {
            CambiarContraseniaDTO cambiarContraseniaDto = new CambiarContraseniaDTO(contraseniaActual, contraseniaNueva);
            
            ApiCliente.enviarSolicitud(endpoint, 
                    "PUT", 
                    cambiarContraseniaDto, 
                    headers, 
                    Object.class);

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Contraseña actualizada con éxito.");

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
}
