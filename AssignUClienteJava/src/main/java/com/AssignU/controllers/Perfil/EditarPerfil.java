package com.AssignU.controllers.Perfil;

import javafx.event.ActionEvent;
import javafx.scene.control.ComboBox;
import javafx.scene.control.TextField;
import javafx.stage.Stage;

public class EditarPerfil {
    public TextField tfNombreCompleto;
    public TextField tfNombreUsuario;
    public ComboBox cbGrados;

    public void btnCancelar(ActionEvent actionEvent) {
        cerrarVentana();
    }

    public void btnAceptar(ActionEvent actionEvent) {
        cerrarVentana();
    }

    private void cerrarVentana(){
        Stage escenario = (Stage) tfNombreCompleto.getScene().getWindow();
        escenario.close();
    }
}
