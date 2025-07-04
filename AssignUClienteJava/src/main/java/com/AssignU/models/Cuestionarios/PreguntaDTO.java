
package com.AssignU.models.Cuestionarios;

import java.util.List;

public class PreguntaDTO {
    public String Texto;
    public String Tipo;
    public List<OpcionDTO> Opciones;

    public PreguntaDTO(String texto, String tipo, List<OpcionDTO> opciones) {
        this.Texto = texto;
        this.Tipo = tipo;
        this.Opciones = opciones;
    }

    public String getTexto() {
        return Texto;
    }

    public void setTexto(String texto) {
        this.Texto = texto;
    }

    public String getTipo() {
        return Tipo;
    }

    public void setTipo(String tipo) {
        this.Tipo = tipo;
    }

    public List<OpcionDTO> getOpciones() {
        return Opciones;
    }

    public void setOpciones(List<OpcionDTO> opciones) {
        this.Opciones = opciones;
    }
}
