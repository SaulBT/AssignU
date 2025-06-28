
package com.AssignU.servicios.usuarios;

import com.AssignU.models.Usuarios.Alumno.RegistrarAlumnoDTO;
import com.AssignU.utils.ApiCliente;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.ExcepcionHTTP;
import java.util.HashMap;
import java.util.Map;

public class ServicioAlumnos {
    public static HashMap<String, Object> registrarAlumno(String nombreCompleto, String nombreUsuario, String contrasenia, String correo, int idGrado) {
        HashMap<String, Object> resultado = new HashMap<>();
        resultado.put(Constantes.KEY_ERROR, true);

        try {
            RegistrarAlumnoDTO dto = new RegistrarAlumnoDTO(nombreCompleto, nombreUsuario, contrasenia, correo, idGrado);
            Map<String, String> headers = Map.of("Content-Type", "application/json");

            ApiCliente.enviarSolicitud("/usuarios/alumnos", "POST", dto, headers, Object.class);

            resultado.put(Constantes.KEY_ERROR, false);
            resultado.put(Constantes.KEY_MENSAJE, "Alumno registrado con éxito.\n Por favor inicia sesión para continuar.");

        } catch (ExcepcionHTTP e) {
            switch (e.getCodigo()) {
                case 400:
                    resultado.put(Constantes.KEY_MENSAJE, "Datos inválidos. Verifique los campos.");
                    break;
                case 409:
                    resultado.put(Constantes.KEY_MENSAJE, "El usuario ya existe.");
                    break;
                case 500:
                    resultado.put(Constantes.KEY_MENSAJE, "Error del servidor. Intente más tarde.");
                    break;
                default:
                    resultado.put(Constantes.KEY_MENSAJE, "Error HTTP (" + e.getCodigo() + "): " + e.getMessage());
                    break;
            }
        } catch (Exception e) {
            resultado.put(Constantes.KEY_MENSAJE, e.getMessage());
        }

        return resultado;
    }
}
