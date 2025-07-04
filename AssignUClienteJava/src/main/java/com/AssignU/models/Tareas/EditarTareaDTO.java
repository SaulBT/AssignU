
package com.AssignU.models.Tareas;

import com.AssignU.models.Cuestionarios.CuestionarioDTO;
import java.time.LocalDateTime;

public class EditarTareaDTO {
    public int idTarea;
    public String nombre;
    public LocalDateTime fechaLimite;
    public CuestionarioDTO Cuestionario;

    public EditarTareaDTO(int idTarea, String nombre, LocalDateTime fechaLimite, CuestionarioDTO Cuestionario) {
        this.idTarea = idTarea;
        this.nombre = nombre;
        this.fechaLimite = fechaLimite;
        this.Cuestionario = Cuestionario;
    }

    public int getIdTarea() {
        return idTarea;
    }

    public void setIdTarea(int idTarea) {
        this.idTarea = idTarea;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    public LocalDateTime getFechaLimite() {
        return fechaLimite;
    }

    public void setFechaLimite(LocalDateTime fechaLimite) {
        this.fechaLimite = fechaLimite;
    }

    public CuestionarioDTO getCuestionario() {
        return Cuestionario;
    }

    public void setCuestionario(CuestionarioDTO cuestionario) {
        this.Cuestionario = cuestionario;
    }
}
