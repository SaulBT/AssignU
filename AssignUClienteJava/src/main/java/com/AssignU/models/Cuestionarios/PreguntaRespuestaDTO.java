
package com.AssignU.models.Cuestionarios;

public class PreguntaRespuestaDTO {
    public String Texto;
    public OpcionRespuestaDTO Opcion;
    public boolean Correcta;

    public PreguntaRespuestaDTO(String texto, OpcionRespuestaDTO opcion, boolean correcta) {
        this.Texto = texto;
        this.Opcion = opcion;
        this.Correcta = correcta;
    }

    public String getTexto() {
        return Texto;
    }

    public void setTexto(String texto) {
        this.Texto = texto;
    }

    public OpcionRespuestaDTO getOpcion() {
        return Opcion;
    }

    public void setOpcion(OpcionRespuestaDTO opcion) {
        this.Opcion = opcion;
    }

    public boolean isCorrecta() {
        return Correcta;
    }

    public void setCorrecta(boolean correcta) {
        this.Correcta = correcta;
    }
}
