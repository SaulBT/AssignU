
package com.AssignU.models.Perfil;

public class CambiarContraseniaDTO {
    public String contraseniaActual;
    public String contraseniaNueva;

    public CambiarContraseniaDTO(String contraseniaActual, String contraseniaNueva) {
        this.contraseniaActual = contraseniaActual;
        this.contraseniaNueva = contraseniaNueva;
    }

    public String getContraseniaActual() {
        return contraseniaActual;
    }

    public void setContraseniaActual(String contraseniaActual) {
        this.contraseniaActual = contraseniaActual;
    }

    public String getContraseniaNueva() {
        return contraseniaNueva;
    }

    public void setContraseniaNueva(String contraseniaNueva) {
        this.contraseniaNueva = contraseniaNueva;
    }
}
