
package com.AssignU.models.Clases.Estadisticas;

import java.time.LocalDateTime;
import java.util.List;

public class AlumnoEstadisticaClaseDTO {
    public int idAlumno;
    public String nombreCompleto;
    public List<RespuestasEstadisticaClaseDTO> respuestas;
    public LocalDateTime ultimaConexion;

    public AlumnoEstadisticaClaseDTO(int idAlumno, String nombreCompleto, List<RespuestasEstadisticaClaseDTO> respuestas, LocalDateTime ultimaConexion) {
        this.idAlumno = idAlumno;
        this.nombreCompleto = nombreCompleto;
        this.respuestas = respuestas;
        this.ultimaConexion = ultimaConexion;
    }

    public int getIdAlumno() {
        return idAlumno;
    }

    public void setIdAlumno(int idAlumno) {
        this.idAlumno = idAlumno;
    }

    public String getNombreCompleto() {
        return nombreCompleto;
    }

    public void setNombreCompleto(String nombreCompleto) {
        this.nombreCompleto = nombreCompleto;
    }

    public List<RespuestasEstadisticaClaseDTO> getRespuestas() {
        return respuestas;
    }

    public void setRespuestas(List<RespuestasEstadisticaClaseDTO> respuestas) {
        this.respuestas = respuestas;
    }

    public LocalDateTime getUltimaConexion() {
        return ultimaConexion;
    }

    public void setUltimaConexion(LocalDateTime ultimaConexion) {
        this.ultimaConexion = ultimaConexion;
    }
    
    public String calcularPromedio(int totalTareas) {
        if (respuestas == null || respuestas.isEmpty() || totalTareas == 0)
            return "0.00";

        double suma = respuestas.stream()
            .mapToDouble(RespuestasEstadisticaClaseDTO::getCalificacion)
            .sum();

        return String.format("%.2f", suma / totalTareas);
    }
}
