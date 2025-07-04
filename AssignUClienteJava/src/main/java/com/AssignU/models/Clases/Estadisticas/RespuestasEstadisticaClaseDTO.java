
package com.AssignU.models.Clases.Estadisticas;

public class RespuestasEstadisticaClaseDTO {
    public int idAlumno;
    public int idTarea;
    public float calificacion;

    public RespuestasEstadisticaClaseDTO(int idAlumno, int idTarea, float calificacion) {
        this.idAlumno = idAlumno;
        this.idTarea = idTarea;
        this.calificacion = calificacion;
    }

    public int getIdAlumno() {
        return idAlumno;
    }

    public void setIdAlumno(int idAlumno) {
        this.idAlumno = idAlumno;
    }

    public int getIdTarea() {
        return idTarea;
    }

    public void setIdTarea(int idTarea) {
        this.idTarea = idTarea;
    }

    public float getCalificacion() {
        return calificacion;
    }

    public void setCalificacion(float calificacion) {
        this.calificacion = calificacion;
    }
}
