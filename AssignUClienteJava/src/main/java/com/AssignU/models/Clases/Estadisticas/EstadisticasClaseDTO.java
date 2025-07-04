
package com.AssignU.models.Clases.Estadisticas;

import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;

public class EstadisticasClaseDTO {
   public int idClase;
   public List<AlumnoEstadisticaClaseDTO> alumnos;
   public List<TareaEstadisticasClaseDTO> tareas;

    public EstadisticasClaseDTO(int idClase, List<AlumnoEstadisticaClaseDTO> alumnos, List<TareaEstadisticasClaseDTO> tareas) {
        this.idClase = idClase;
        this.alumnos = alumnos;
        this.tareas = tareas;
    }

    public int getIdClase() {
        return idClase;
    }

    public void setIdClase(int idClase) {
        this.idClase = idClase;
    }

    public List<AlumnoEstadisticaClaseDTO> getAlumnos() {
        return alumnos;
    }

    public void setAlumnos(List<AlumnoEstadisticaClaseDTO> alumnos) {
        this.alumnos = alumnos;
    }

    public List<TareaEstadisticasClaseDTO> getTareas() {
        return tareas;
    }

    public void setTareas(List<TareaEstadisticasClaseDTO> tareas) {
        this.tareas = tareas;
    }
    
    public int contarTareasCompletadas(List<RespuestasEstadisticaClaseDTO> respuestasAlumno) {
        if (respuestasAlumno == null || tareas == null) return 0;

        Set<Integer> idsTareasClase = tareas.stream()
            .map(TareaEstadisticasClaseDTO::getIdTarea)
            .collect(Collectors.toSet());

        return (int) respuestasAlumno.stream()
            .map(RespuestasEstadisticaClaseDTO::getIdTarea)
            .filter(idsTareasClase::contains)
            .count();
    }
}
