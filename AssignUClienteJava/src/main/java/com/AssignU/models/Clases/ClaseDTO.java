package com.AssignU.models.Clases;

public class ClaseDTO {
    public int idClase;
    public String nombreClase;
    public String codigoClase;
    public int idDocente;

    public ClaseDTO(int idClase, String nombreClase, String codigoClase, int idDocente) {
        this.idClase = idClase;
        this.nombreClase = nombreClase;
        this.codigoClase = codigoClase;
        this.idDocente = idDocente;
    }

    public int getIdClase() {
        return idClase;
    }

    public void setIdClase(int idClase) {
        this.idClase = idClase;
    }

    public String getNombreClase() {
        return nombreClase;
    }

    public void setNombreClase(String nombreClase) {
        this.nombreClase = nombreClase;
    }

    public String getCodigoClase() {
        return codigoClase;
    }

    public void setCodigoClase(String codigoClase) {
        this.codigoClase = codigoClase;
    }

    public int getIdDocente() {
        return idDocente;
    }

    public void setIdDocente(int idDocente) {
        this.idDocente = idDocente;
    }
}
