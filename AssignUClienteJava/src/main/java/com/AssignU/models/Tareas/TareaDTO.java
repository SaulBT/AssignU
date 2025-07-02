
package com.AssignU.models.Tareas;

import java.time.LocalDateTime;

public class TareaDTO {
    public int idTarea;
    public int idClase;
    public String nombre;
    public LocalDateTime fechaLimite;

    public TareaDTO(int idTarea, int idClase, String nombre, LocalDateTime fechaLimite) {
        this.idTarea = idTarea;
        this.idClase = idClase;
        this.nombre = nombre;
        this.fechaLimite = fechaLimite;
    }

    public int getIdTarea() {
        return idTarea;
    }

    public void setIdTarea(int idTarea) {
        this.idTarea = idTarea;
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
}
