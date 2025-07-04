
package com.AssignU.models.Cuestionarios;

import java.util.List;

public class CuestionarioDTO {
    public int idTarea;
    public List<PreguntaDTO> preguntas;

    public CuestionarioDTO(int idTarea, List<PreguntaDTO> preguntas) {
        this.idTarea = idTarea;
        this.preguntas = preguntas;
    }

    public int getIdTarea() {
        return idTarea;
    }

    public void setIdTarea(int idTarea) {
        this.idTarea = idTarea;
    }

    public List<PreguntaDTO> getPreguntas() {
        return preguntas;
    }

    public void setPreguntas(List<PreguntaDTO> preguntas) {
        this.preguntas = preguntas;
    }
}
