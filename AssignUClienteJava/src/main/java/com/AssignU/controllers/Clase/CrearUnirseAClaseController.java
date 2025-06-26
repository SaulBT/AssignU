package com.AssignU.controllers.Clase;

import com.AssignU.controllers.Menu.MenuController;
import com.AssignU.models.Clases.ClaseDTO;
import com.AssignU.models.Clases.CrearClaseDTO;
import com.AssignU.models.Usuarios.Sesion;
import com.AssignU.utils.ApiCliente;
import com.AssignU.utils.VentanaEmergente;
import javafx.event.ActionEvent;
import javafx.scene.control.Alert;
import javafx.scene.control.TextField;
import javafx.stage.Stage;

import java.util.HashMap;
import java.util.Map;
import javafx.fxml.FXML;
import javafx.scene.control.Button;
import javafx.scene.control.Label;

public class CrearUnirseAClaseController {
    
    public MenuController menuController;
    private Sesion sesion;
    private String mensajeError;
    private boolean esDocente;
    
    private Map<String, String> headers = new HashMap<String, String>();
    
    @FXML
    private Label lbTitulo;
    @FXML
    private Button btnAceptar;
    @FXML
    private Label lbTexto;
    @FXML
    private TextField tfContenido;

    public void cargarDatos(MenuController menuController, Sesion sesion) {
        this.menuController = menuController;
        this.sesion = sesion;
        if (sesion.tipoUsuario.matches("alumno")) {
            esDocente = false;
            cargarVentanaAlumno();
        } else if (sesion.tipoUsuario.matches("docente")) {
            esDocente = true;
            cargarVentanaDocente();
        }
    }
    
    public void cargarVentanaAlumno(){
        lbTitulo.setText("Unirse a Clase");
        lbTexto.setText("Código de la clase:");
        btnAceptar.setText("Unirse");
    }
    
    public void cargarVentanaDocente(){
        lbTitulo.setText("Crear Clase");
        lbTexto.setText("Nombre de la clase:");
        btnAceptar.setText("Crear Clase");
    }

    @FXML
    public void btnAceptar(ActionEvent actionEvent) {
        if (verificarCampo()) {
            if (esDocente){
                crearClase(tfContenido.getText());
            }else{
                unirseAClase(tfContenido.getText());
            }
        } else {
            tfContenido.setStyle("-fx-border-color: red");
            VentanaEmergente.mostrarVentana("Error", "Campo inválido.", mensajeError, Alert.AlertType.ERROR).showAndWait();
        }
    }

    private boolean verificarCampo() {
        String nombreClase = tfContenido.getText();
        boolean error;
        
        if (nombreClase.isEmpty()) {
            if(esDocente){
                mensajeError = "Escribe un nombre para la clase.";
            }else{
                mensajeError = "Ingrese el código de la clase.";
            }
            error = false;
        } else if (nombreClase.toCharArray().length > 45) {
            mensajeError = "El nombre debe de ser menor a 45 caracteres.";
            error = false;
        } else {
            error = true;
        }
        return error;
    }

    private void crearClase(String nombreClase) {
        try {
            headers.put("Content-Type", "application/json");
            headers.put("Authorization", "Bearer " + sesion.jwt);
            CrearClaseDTO crearClaseDto = new CrearClaseDTO(nombreClase);
            ClaseDTO claseDto = ApiCliente.enviarSolicitud("/clases/clases", "POST", crearClaseDto, headers, ClaseDTO.class);
            menuController.enviarAClaseNueva(claseDto);
            cerrarVentana();
        } catch (Exception e) {
            VentanaEmergente.mostrarVentana("Error", null, e.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }
    
    public void unirseAClase(String codigo){
        try {
            headers.put("Content-Type", "application/json");
            headers.put("Authorization", "Bearer " + sesion.jwt);
            String endpoint = String.format("/clases/clases/%s/unirse", codigo);
            ClaseDTO claseDto = ApiCliente.enviarSolicitud(endpoint, "POST", null, headers, ClaseDTO.class);
            menuController.enviarAClaseNueva(claseDto);
            cerrarVentana();
        } catch (Exception e) {
            VentanaEmergente.mostrarVentana("Error", null, e.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }

    @FXML
    public void btnCancelar(ActionEvent actionEvent) {
        cerrarVentana();
    }

    private void cerrarVentana() {
        Stage stage = (Stage) tfContenido.getScene().getWindow();
        stage.close();
    }
}
