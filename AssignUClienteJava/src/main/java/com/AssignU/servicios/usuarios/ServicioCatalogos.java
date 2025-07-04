
package com.AssignU.servicios.usuarios;

import com.AssignU.models.Usuarios.Catalogo.GradoEstudioDTO;
import com.AssignU.models.Usuarios.Catalogo.GradoProfesionalDTO;
import com.AssignU.utils.ApiCliente;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.ExcepcionHTTP;
import java.util.HashMap;
import java.util.List;

public class ServicioCatalogos {
    public static HashMap<String, Object> obtenerGradosDeEstudios() {
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);

        try {
            List<GradoEstudioDTO> grados = ApiCliente.enviarSolicitudLista(
                "/usuarios/catalogos/grados-estudios",
                "GET",
                null,
                null,
                GradoEstudioDTO.class
            );

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Grados de estudios obtenidos correctamente.");
            resultado.put(Constantes.KEY_RESPUESTA, grados);

        } catch (ExcepcionHTTP e) {
            resultado.put(Constantes.KEY_MENSAJE, "Error HTTP (" + e.getCodigo() + "): " + e.getMessage());
        } catch (Exception e) {
            resultado.put(Constantes.KEY_MENSAJE, "Error de red o inesperado: " + e.getMessage());
        }

        return resultado;
    }

    public static HashMap<String, Object> obtenerGradosProfesionales() {
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);

        try {
            List<GradoProfesionalDTO> grados = ApiCliente.enviarSolicitudLista(
                "/usuarios/catalogos/grados-profesionales",
                "GET",
                null,
                null,
                GradoProfesionalDTO.class
            );

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Grados profesionales obtenidos correctamente.");
            resultado.put(Constantes.KEY_RESPUESTA, grados);

        } catch (ExcepcionHTTP e) {
            resultado.put(Constantes.KEY_MENSAJE, "Error HTTP (" + e.getCodigo() + "): " + e.getMessage());
        } catch (Exception e) {
            resultado.put(Constantes.KEY_MENSAJE, "Error de red o inesperado: " + e.getMessage());
        }

        return resultado;
    }

    public static HashMap<String, Object> obtenerGradoEstudios(int idGrado) {
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);

        try {
            GradoEstudioDTO grado = ApiCliente.enviarSolicitud(
                "/usuarios/catalogos/grado-estudio/" + idGrado,
                "GET",
                null,
                null,
                GradoEstudioDTO.class
            );

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Grado de estudio obtenido correctamente.");
            resultado.put(Constantes.KEY_RESPUESTA, grado);

        } catch (ExcepcionHTTP e) {
            if (e.getCodigo() == 404) {
                resultado.put(Constantes.KEY_MENSAJE, "El grado de estudios no existe.");
            } else {
                resultado.put(Constantes.KEY_MENSAJE, "Error HTTP (" + e.getCodigo() + "): " + e.getMessage());
            }
        } catch (Exception e) {
            resultado.put(Constantes.KEY_MENSAJE, "Error de red o inesperado: " + e.getMessage());
        }

        return resultado;
    }

    public static HashMap<String, Object> obtenerGradoProfesional(int idGrado) {
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);

        try {
            GradoProfesionalDTO grado = ApiCliente.enviarSolicitud(
                "/usuarios/catalogos/grado-profesional/" + idGrado,
                "GET",
                null,
                null,
                GradoProfesionalDTO.class
            );

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Grado profesional obtenido correctamente.");
            resultado.put(Constantes.KEY_RESPUESTA, grado);

        } catch (ExcepcionHTTP e) {
            if (e.getCodigo() == 404) {
                resultado.put(Constantes.KEY_MENSAJE, "El grado profesional no existe.");
            } else {
                resultado.put(Constantes.KEY_MENSAJE, "Error HTTP (" + e.getCodigo() + "): " + e.getMessage());
            }
        } catch (Exception e) {
            resultado.put(Constantes.KEY_MENSAJE, "Error de red o inesperado: " + e.getMessage());
        }

        return resultado;
    }
}
