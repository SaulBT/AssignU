package com.AssignU.controllers.Perfil;

import javafx.event.ActionEvent;
import javafx.scene.control.PasswordField;
import javafx.stage.Stage;

public class CambiarContraseniaController {
    public PasswordField pfContraseniaNueva;
    public PasswordField pfContraseniaActual;

    public void btnCancelar(ActionEvent actionEvent) {
        cerrarVentana();
    }

    public void btnAceptar(ActionEvent actionEvent) {
        cerrarVentana();
    }

    private void cerrarVentana(){
        Stage escenario = (Stage) pfContraseniaActual.getScene().getWindow();
        escenario.close();
    }
}
