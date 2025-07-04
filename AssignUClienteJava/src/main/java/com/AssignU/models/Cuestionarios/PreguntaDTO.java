
package com.AssignU.models.Cuestionarios;

import java.util.List;

public class PreguntaDTO {
    public String texto;
    public String tipo;
    public List<OpcionDTO> opciones;

    public PreguntaDTO(String texto, String tipo, List<OpcionDTO> opciones) {
        this.texto = texto;
        this.tipo = tipo;
        this.opciones = opciones;
    }

    public String getTexto() {
        return texto;
    }

    public void setTexto(String texto) {
        this.texto = texto;
    }

    public String getTipo() {
        return tipo;
    }

    public void setTipo(String tipo) {
        this.tipo = tipo;
    }

    public List<OpcionDTO> getOpciones() {
        return opciones;
    }

    public void setOpciones(List<OpcionDTO> opciones) {
        this.opciones = opciones;
    }
}
