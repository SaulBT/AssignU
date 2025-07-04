
package com.AssignU.models.Cuestionarios;

import java.util.List;

public class RespuestaDTO {
    public int idAlumno;
    public int idTarea;
    public String estado;
    public float calificacion;
    public List<PreguntaRespuestaDTO> preguntas;

    public RespuestaDTO(int idAlumno, int idTarea, String estado, float calificacion, List<PreguntaRespuestaDTO> preguntas) {
        this.idAlumno = idAlumno;
        this.idTarea = idTarea;
        this.estado = estado;
        this.calificacion = calificacion;
        this.preguntas = preguntas;
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

    public String getEstado() {
        return estado;
    }

    public void setEstado(String estado) {
        this.estado = estado;
    }

    public float getCalificacion() {
        return calificacion;
    }

    public void setCalificacion(float calificacion) {
        this.calificacion = calificacion;
    }

    public List<PreguntaRespuestaDTO> getPreguntas() {
        return preguntas;
    }

    public void setPreguntas(List<PreguntaRespuestaDTO> preguntas) {
        this.preguntas = preguntas;
    }
}
