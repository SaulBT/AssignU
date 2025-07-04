
package com.AssignU.models.Clases;

import java.time.LocalDateTime;

public class RegistroDTO {
    public int idRegistro;
    public int idAlumno;
    public int idClase;
    public LocalDateTime ultimoInicio;

    public RegistroDTO(int idRegistro, int idAlumno, int idClase, LocalDateTime ultimoInicio) {
        this.idRegistro = idRegistro;
        this.idAlumno = idAlumno;
        this.idClase = idClase;
        this.ultimoInicio = ultimoInicio;
    }

    public int getIdRegistro() {
        return idRegistro;
    }

    public void setIdRegistro(int idRegistro) {
        this.idRegistro = idRegistro;
    }

    public int getIdAlumno() {
        return idAlumno;
    }

    public void setIdAlumno(int idAlumno) {
        this.idAlumno = idAlumno;
    }

    public int getIdClase() {
        return idClase;
    }

    public void setIdClase(int idClase) {
        this.idClase = idClase;
    }

    public LocalDateTime getUltimoInicio() {
        return ultimoInicio;
    }

    public void setUltimoInicio(LocalDateTime ultimoInicio) {
        this.ultimoInicio = ultimoInicio;
    }
}
