package com.AssignU.controllers.Tarea;

import javafx.event.ActionEvent;
import javafx.scene.control.Label;
import javafx.scene.input.MouseEvent;
import javafx.scene.layout.VBox;

public class TareaController {
    public Label lbNombreTarea;
    public Label lbFechaLimite;
    public Label lbNombreArchivo;
    public VBox vbCuestionario;
    public Label lbEstadistica;

    public void btnLbVolver(MouseEvent mouseEvent) {
        regresarAClase();
    }

    public void btnDescargar(ActionEvent actionEvent) {
    }

    public void btnResponder(ActionEvent actionEvent) {
        regresarAClase();
    }

    public void btnLbEstadistica(MouseEvent mouseEvent) {
    }

    private void regresarAClase() {
        /*try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Clase/clase.fxml"));
            Parent vistClase = loader.load();
            Stage escenario = (Stage) lbFechaLimite.getScene().getWindow();
            Scene nuevaEscena = new Scene(vistClase);
            escenario.setTitle("Clase");
            escenario.setScene(nuevaEscena);
        } catch (Exception ex) {
            VentanaEmergente.mostrarVentana("Error al cambiar la vista", null, ex.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }*/
    }
}
