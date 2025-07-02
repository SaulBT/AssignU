package com.AssignU.controllers.Clase;

import javafx.event.ActionEvent;
import javafx.scene.control.*;
import javafx.scene.input.MouseEvent;
import javafx.scene.layout.HBox;

public class EstadisticasClaseController {
    public Label lbNombreClase;
    public Label lbNumeroAlumnos;
    public Label lbNumeroTareas;
    public HBox hbClase;
    public ScrollPane spInformacionClase;
    public TableView tbDatosClase;
    public TableColumn tcAlumno;
    public TableColumn tcTarea;
    public TableColumn tcTotal;
    public TableColumn tcPromedio;

    public void btnLbVolver(MouseEvent mouseEvent) {
        /*try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Clase/clase.fxml"));
            Parent verClase = loader.load();
            Stage escenario = (Stage) lbNumeroTareas.getScene().getWindow();
            Scene nuevaEscena = new Scene(verClase);
            escenario.setTitle("Clase");
            escenario.setScene(nuevaEscena);
        } catch (Exception ex) {
            VentanaEmergente.mostrarVentana("Error al cambiar la vista", null, ex.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }*/
    }

    public void btnVerPerfil(ActionEvent actionEvent) {
        /*try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Perfil/perfil.fxml"));
            Parent vistaPerfil = loader.load();
            Stage escenario = (Stage) lbNumeroTareas.getScene().getWindow();
            Scene nuevaEscena = new Scene(vistaPerfil);
            escenario.setTitle("Perfil de alumno");
            escenario.setScene(nuevaEscena);
        } catch (Exception ex) {
            VentanaEmergente.mostrarVentana("Error al cambiar la vista", null, ex.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }*/
    }
}
