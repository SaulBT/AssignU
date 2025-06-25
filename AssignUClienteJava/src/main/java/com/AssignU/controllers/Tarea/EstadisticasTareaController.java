package com.AssignU.controllers.Tarea;

import com.AssignU.utils.VentanaEmergente;
import javafx.event.ActionEvent;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.input.MouseEvent;
import javafx.scene.layout.HBox;
import javafx.stage.Stage;

public class EstadisticasTareaController {
    public Label lbNombreClase;
    public Label lbNumeroAlumnos;
    public Label lbNumeroRespuestas;
    public HBox hbClase;
    public ScrollPane spInformacionClase;
    public TableView tbDatosTarea;
    public TableColumn tcAlumno;
    public TableColumn tcPregunta;
    public TableColumn tcTotal;
    public TableColumn tcCalificacion;

    public void btnLbVolver(MouseEvent mouseEvent) {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Tarea/tarea.fxml"));
            Parent verTarea = loader.load();
            Stage escenario = (Stage) lbNumeroRespuestas.getScene().getWindow();
            Scene nuevaEscena = new Scene(verTarea);
            escenario.setTitle("Tarea");
            escenario.setScene(nuevaEscena);
        } catch (Exception ex) {
            VentanaEmergente.mostrarVentana("Error al cambiar la vista", null, ex.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }

    public void btnVerPerfil(ActionEvent actionEvent) {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Perfil/perfil.fxml"));
            Parent vistaPerfil = loader.load();
            Stage escenario = (Stage) lbNumeroAlumnos.getScene().getWindow();
            Scene nuevaEscena = new Scene(vistaPerfil);
            escenario.setTitle("Perfil de alumno");
            escenario.setScene(nuevaEscena);
        } catch (Exception ex) {
            VentanaEmergente.mostrarVentana("Error al cambiar la vista", null, ex.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }
}
