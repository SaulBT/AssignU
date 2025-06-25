package com.AssignU.controllers.Menu;

import com.AssignU.utils.VentanaEmergente;
import javafx.event.ActionEvent;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.Alert;
import javafx.scene.control.Label;
import javafx.stage.Stage;

public class TarjetaClaseController {
    public Label lbNombreClase;
    public Label lbNombreDocente;
    public int idClase;

    public void btnVerClase(ActionEvent actionEvent) {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Clase/clase.fxml"));
            Parent vistaClase = loader.load();
            Stage escenario = (Stage) lbNombreDocente.getScene().getWindow();
            Scene nuevaEscena = new Scene(vistaClase);
            escenario.setScene(nuevaEscena);
        } catch (Exception ex) {
            VentanaEmergente.mostrarVentana("Error al cambiar la vista", null, ex.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }

    public void cargarDatos(String nombreClase, String nombreDocente, int idClase) {
        lbNombreClase.setText(nombreClase);
        lbNombreDocente.setText(nombreDocente);
        this.idClase = idClase;
    }
}
