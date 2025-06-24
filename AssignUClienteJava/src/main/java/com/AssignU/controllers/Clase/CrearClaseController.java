package com.AssignU.controllers.Clase;

import com.AssignU.controllers.Menu.MenuController;
import com.AssignU.models.Clases.ClaseDTO;
import com.AssignU.models.Clases.CrearClaseDTO;
import com.AssignU.utils.ApiCliente;
import com.AssignU.utils.VentanaEmergente;
import javafx.event.ActionEvent;
import javafx.scene.control.Alert;
import javafx.scene.control.TextField;
import javafx.stage.Stage;

import java.util.HashMap;
import java.util.Map;

public class CrearClaseController {
    public TextField tfNombreClase;
    public MenuController menuController;
    public int idDocente;
    public String jwt;
    private String mensajeError;
    private Map<String, String> headers = new HashMap<String, String>();

    public void btnCancelar(ActionEvent actionEvent) {
        cerrarVentana();
    }

    public void btnCrearClase(ActionEvent actionEvent) {
        if (verificarCampo()) {
            crearClase(tfNombreClase.getText());
        } else {
            tfNombreClase.setStyle("-fx-border-color: red");
            VentanaEmergente.mostrarVentana("Error", "Campo inválido.", mensajeError, Alert.AlertType.ERROR).showAndWait();
        }
    }

    public void cargarDatos(MenuController menuController, int idDocente, String jwt) {
        this.menuController = menuController;
        this.idDocente = idDocente;
        this.jwt = jwt;
    }

    private boolean verificarCampo() {
        String nombreClase = tfNombreClase.getText();

        if (nombreClase.isEmpty()) {
            mensajeError = "Escribe un nombre para la clase.";
            return false;
        } else if (nombreClase.toCharArray().length > 45) {
            mensajeError = "El nombre debe de ser menor a 45 caracteres.";
            return false;
        } else {
            return true;
        }
    }

    private void crearClase(String nombreClase) {
        try {
            headers.put("Content-Type", "application/json");
            headers.put("Authorization", "Bearer " + jwt);
            CrearClaseDTO crearClaseDto = new CrearClaseDTO(nombreClase);
            ClaseDTO claseDto = ApiCliente.enviarSolicitud("/clases/clases", "POST", crearClaseDto, headers, ClaseDTO.class);
            menuController.enviarAClaseNueva(claseDto);
            cerrarVentana();
        } catch (Exception e) {
            VentanaEmergente.mostrarVentana("Error", null, e.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }

    private void cerrarVentana() {
        Stage stage = (Stage) tfNombreClase.getScene().getWindow();
        stage.close();
    }
}
