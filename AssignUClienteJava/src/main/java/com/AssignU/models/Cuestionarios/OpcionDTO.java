
package com.AssignU.models.Cuestionarios;

public class OpcionDTO {
    public String Texto;
    public boolean EsCorrecta;

    public OpcionDTO(String texto, boolean esCorrecta) {
        this.Texto = texto;
        this.EsCorrecta = esCorrecta;
    }

    public String getTexto() {
        return Texto;
    }

    public void setTexto(String texto) {
        this.Texto = texto;
    }

    public boolean isEsCorrecta() {
        return EsCorrecta;
    }

    public void setEsCorrecta(boolean esCorrecta) {
        this.EsCorrecta = esCorrecta;
    }
}
