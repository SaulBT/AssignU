
package com.AssignU.models.Perfil;

import java.time.LocalDateTime;
import java.util.List;

public class ClaseEstadisticaPerfilDTO {
    public int idClase;
    public String nombre;
    public LocalDateTime ultimaConexion;
    public List<TareaEstadisticaPerfilDTO> tareas;

    public ClaseEstadisticaPerfilDTO(int idClase, String nombre, LocalDateTime ultimaConexion, List<TareaEstadisticaPerfilDTO> tareas) {
        this.idClase = idClase;
        this.nombre = nombre;
        this.ultimaConexion = ultimaConexion;
        this.tareas = tareas;
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

    public LocalDateTime getUltimaConexion() {
        return ultimaConexion;
    }

    public void setUltimaConexion(LocalDateTime ultimaConexion) {
        this.ultimaConexion = ultimaConexion;
    }

    public List<TareaEstadisticaPerfilDTO> getTareas() {
        return tareas;
    }

    public void setTareas(List<TareaEstadisticaPerfilDTO> tareas) {
        this.tareas = tareas;
    }
    
    @Override
    public String toString() {
        return nombre;
    }
}
