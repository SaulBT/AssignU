
package com.AssignU.models.Cuestionarios;

import java.util.List;

public class CuestionarioDTO {
    public int IdTarea;
    public List<PreguntaDTO> Preguntas;

    public CuestionarioDTO(int idTarea, List<PreguntaDTO> preguntas) {
        this.IdTarea = idTarea;
        this.Preguntas = preguntas;
    }

    public int getIdTarea() {
        return IdTarea;
    }

    public void setIdTarea(int idTarea) {
        this.IdTarea = idTarea;
    }

    public List<PreguntaDTO> getPreguntas() {
        return Preguntas;
    }

    public void setPreguntas(List<PreguntaDTO> preguntas) {
        this.Preguntas = preguntas;
    }
}
