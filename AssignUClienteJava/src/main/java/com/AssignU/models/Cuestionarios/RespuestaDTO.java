
package com.AssignU.models.Cuestionarios;

import java.util.List;

public class RespuestaDTO {
    public int IdAlumno;
    public int idTarea;
    public String Estado;
    public float Calificacion;
    public List<PreguntaRespuestaDTO> Preguntas;

    public RespuestaDTO(int idAlumno, int idTarea, String estado, float calificacion, List<PreguntaRespuestaDTO> preguntas) {
        this.IdAlumno = idAlumno;
        this.idTarea = idTarea;
        this.Estado = estado;
        this.Calificacion = calificacion;
        this.Preguntas = preguntas;
    }

    public int getIdAlumno() {
        return IdAlumno;
    }

    public void setIdAlumno(int idAlumno) {
        this.IdAlumno = idAlumno;
    }

    public int getIdTarea() {
        return idTarea;
    }

    public void setIdTarea(int idTarea) {
        this.idTarea = idTarea;
    }

    public String getEstado() {
        return Estado;
    }

    public void setEstado(String estado) {
        this.Estado = estado;
    }

    public float getCalificacion() {
        return Calificacion;
    }

    public void setCalificacion(float calificacion) {
        this.Calificacion = calificacion;
    }

    public List<PreguntaRespuestaDTO> getPreguntas() {
        return Preguntas;
    }

    public void setPreguntas(List<PreguntaRespuestaDTO> preguntas) {
        this.Preguntas = preguntas;
    }
}
