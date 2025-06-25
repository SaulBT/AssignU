package com.AssignU.controllers.Clase;

import com.AssignU.utils.VentanaEmergente;
import javafx.event.ActionEvent;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.Alert;
import javafx.scene.control.Button;
import javafx.scene.control.Label;
import javafx.scene.input.MouseEvent;
import javafx.scene.layout.FlowPane;
import javafx.stage.Modality;
import javafx.stage.Stage;

public class ClaseController {
    public Label lnNombreClase;
    public Label lbCodigoClase;
    public Button btnCrearTarea;
    public FlowPane fpContenedorTareas;
    public Label lbEliminarClase;
    public Label lbCambiarNombre;
    public Label lbVerEstadisticas;

    public void btnLbVolver(MouseEvent mouseEvent) {
    }

    public void clicBtnCrearTarea(ActionEvent actionEvent) {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Tarea/crearTarea.fxml"));
            Parent vistaCrearTarea = loader.load();
            Stage escenario = (Stage) lbCambiarNombre.getScene().getWindow();
            Scene nuevaEscena = new Scene(vistaCrearTarea);
            escenario.setTitle("Crear tarea");
            escenario.setScene(nuevaEscena);
        } catch (Exception ex) {
            VentanaEmergente.mostrarVentana("Error al cambiar la vista", null, ex.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }

    public void btnLbEliminarClase(MouseEvent mouseEvent) {
    }

    public void btnLbCambiarNombre(MouseEvent mouseEvent) {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Clase/cambiarNombre.fxml"));
            Parent vistaCambiarNombre = loader.load();
            Stage nuevaVentana = new Stage();
            nuevaVentana.setTitle("Cambiar nombre");
            nuevaVentana.setScene(new Scene(vistaCambiarNombre));
            nuevaVentana.initModality(Modality.APPLICATION_MODAL);
            nuevaVentana.showAndWait();
        } catch (Exception ex) {
            VentanaEmergente.mostrarVentana("Error al cambiar la vista", null, ex.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }

    public void btnLbVerEstadisticas(MouseEvent mouseEvent) {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Clase/estadisticasClase.fxml"));
            Parent vistaEstadisticasClase = loader.load();
            Stage escenario = (Stage) lbCambiarNombre.getScene().getWindow();
            Scene nuevaEscena = new Scene(vistaEstadisticasClase);
            escenario.setTitle("Estadísticas clase");
            escenario.setScene(nuevaEscena);
        } catch (Exception ex) {
            VentanaEmergente.mostrarVentana("Error al cambiar la vista", null, ex.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }
}
