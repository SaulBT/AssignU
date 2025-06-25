package com.AssignU.controllers.Perfil;

import com.AssignU.utils.VentanaEmergente;
import javafx.event.ActionEvent;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.input.MouseEvent;
import javafx.stage.Modality;
import javafx.stage.Stage;

public class PerfilController {
    public Label lbPerfilNombreUsuario;
    public Label lbNombreUsuario;
    public Label lbNombreCompleto;
    public Label lbCorreoElectronico;
    public Label lbGradoEstudios;
    public ComboBox cbClase;
    public TableView tbDatosClase;
    public TableColumn tcNombre;
    public TableColumn tcPregunta;
    public TableColumn tcTotal;
    public TableColumn tcCalificacion;
    public Label lbPromedio;
    public Button btnCambiarContrasenia;
    public Button btnEditarPerfil;
    public Label lbTextoGradoEstudios;
    public ScrollPane spInformacionClase;

    public void btnLbVolver(MouseEvent mouseEvent) {
    }

    public void clicBtnCambiarContrasenia(ActionEvent actionEvent) {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Perfil/cambiarContrasenia.fxml"));
            Parent vistaCambiarContrasenia = loader.load();
            Stage nuevaVentana = new Stage();
            nuevaVentana.setTitle("Cambiar contraseña");
            nuevaVentana.setScene(new Scene(vistaCambiarContrasenia));
            nuevaVentana.initModality(Modality.APPLICATION_MODAL);
            nuevaVentana.showAndWait();
        } catch (Exception ex) {
            VentanaEmergente.mostrarVentana("Error al cambiar la vista", null, ex.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }

    public void clicBtnEditarPerfil(ActionEvent actionEvent) {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Perfil/editarPerfil.fxml"));
            Parent vistaEditarPerfil = loader.load();
            Stage nuevaVentana = new Stage();
            nuevaVentana.setTitle("Editar perfil");
            nuevaVentana.setScene(new Scene(vistaEditarPerfil));
            nuevaVentana.initModality(Modality.APPLICATION_MODAL);
            nuevaVentana.showAndWait();
        } catch (Exception ex) {
            VentanaEmergente.mostrarVentana("Error al cambiar la vista", null, ex.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }
}
