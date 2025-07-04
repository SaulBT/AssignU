
package com.AssignU.models.Tareas;

import com.AssignU.models.Cuestionarios.CuestionarioDTO;
import java.time.LocalDateTime;

public class CrearTareaDTO {
    public int idClase;
    public String nombre;
    public LocalDateTime fechaLimite;
    public CuestionarioDTO Cuestionario;

    public CrearTareaDTO(int idClase, String nombre, LocalDateTime fechaLimite, CuestionarioDTO cuestionarioDto) {
        this.idClase = idClase;
        this.nombre = nombre;
        this.fechaLimite = fechaLimite;
        this.Cuestionario = cuestionarioDto;
    }

    public int getIdClase() {
        return idClase;
    }

    public void setIdClase(int idClase) {
        this.idClase = idClase;
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
