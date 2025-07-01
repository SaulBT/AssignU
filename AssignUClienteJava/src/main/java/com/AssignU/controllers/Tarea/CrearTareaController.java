package com.AssignU.controllers.Tarea;

import com.AssignU.utils.VentanaEmergente;
import javafx.event.ActionEvent;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.Alert;
import javafx.scene.control.DatePicker;
import javafx.scene.control.Label;
import javafx.scene.control.TextField;
import javafx.scene.input.MouseEvent;
import javafx.stage.Stage;

public class CrearTareaController {
    public TextField tfNombreTarea;
    public DatePicker dpFechaLimite;
    public Label lbNombreArchivo;

    public void btnLbVolver(MouseEvent mouseEvent) {
        volverAClase();
    }

    public void btnSeleccionarArchivo(ActionEvent actionEvent) {
    }

    public void cbAgregarPregunta(ActionEvent actionEvent) {
    }

    public void btnEliminar(ActionEvent actionEvent) {
    }

    public void btnCrear(ActionEvent actionEvent) {
        volverAClase();
    }

    private void volverAClase() {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Clase/clase.fxml"));
            Parent vistClase = loader.load();
            Stage escenario = (Stage) lbNombreArchivo.getScene().getWindow();
            Scene nuevaEscena = new Scene(vistClase);
            escenario.setTitle("Clase");
            escenario.setScene(nuevaEscena);
        } catch (Exception ex) {
            VentanaEmergente.mostrarVentana("Error al cambiar la vista", null, ex.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }
}
