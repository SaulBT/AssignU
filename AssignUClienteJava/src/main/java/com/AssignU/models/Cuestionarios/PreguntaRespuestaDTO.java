
package com.AssignU.models.Cuestionarios;

public class PreguntaRespuestaDTO {
    public String texto;
    public OpcionRespuestaDTO opcion;
    public boolean correcta;

    public PreguntaRespuestaDTO(String texto, OpcionRespuestaDTO opcion, boolean correcta) {
        this.texto = texto;
        this.opcion = opcion;
        this.correcta = correcta;
    }

    public String getTexto() {
        return texto;
    }

    public void setTexto(String texto) {
        this.texto = texto;
    }

    public OpcionRespuestaDTO getOpcion() {
        return opcion;
    }

    public void setOpcion(OpcionRespuestaDTO opcion) {
        this.opcion = opcion;
    }

    public boolean isCorrecta() {
        return correcta;
    }

    public void setCorrecta(boolean correcta) {
        this.correcta = correcta;
    }
}
