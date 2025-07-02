package com.AssignU.controllers.Clase;

import com.AssignU.controllers.Menu.MenuController;
import com.AssignU.models.Clases.ClaseDTO;
import com.AssignU.models.Usuarios.Sesion;
import com.AssignU.servicios.ServicioClases;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.IFormulario;
import com.AssignU.utils.Utils;
import javafx.event.ActionEvent;
import javafx.scene.control.Alert;
import javafx.scene.control.TextField;
import javafx.stage.Stage;

import java.util.HashMap;
import javafx.fxml.FXML;
import javafx.scene.control.Button;
import javafx.scene.control.Label;

public class CrearUnirseAClaseController implements IFormulario{
    
    private MenuController menuController;
    private Sesion sesion;
    private String mensajeError;
    
    @FXML
    private Label lbTitulo;
    @FXML
    private Button btnAceptar;
    @FXML
    private Label lbTexto;
    @FXML
    private TextField tfContenido;

    public void cargarValores(MenuController menuController) {
        this.menuController = menuController;
        this.sesion = Sesion.getSesion();
        if (sesion.esDocente()) {
            cargarVentanaAlumno();
        } else {
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
        if(verificarCampos()){
            if (sesion.esDocente()){
                crearClase(tfContenido.getText());
            }else{
                unirseAClase(tfContenido.getText());
            }
        } else {
            tfContenido.setStyle("-fx-border-color: red");
            Utils.mostrarVentana("Campo inválido.", mensajeError, Alert.AlertType.ERROR);
        }
    }

    @Override
    public boolean verificarCampos() {
        restaurarCampos();
        String nombreClase = tfContenido.getText();
        boolean error = true;
        
        if(nombreClase.isEmpty()){
            if(sesion.esDocente()){
                mensajeError = "Escribe un nombre para la clase.";
            }else{
                mensajeError = "Ingrese el código de la clase.";
            }
            error = false;
        } else if (!Utils.verificarTamanioCampo(nombreClase, 44)){
            mensajeError = "El nombre debe de ser menor a 45 caracteres.";
            error = false;
        }
        return error;
    }
    
    @Override
    public void restaurarCampos(){
        tfContenido.setStyle("-fx-border-color: black");
    }

    private void crearClase(String nombreClase) {
        HashMap<String, Object> respuesta = ServicioClases.crearClase(nombreClase);
        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            Utils.mostrarVentana("Éxito", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.INFORMATION);
            menuController.enviarAClaseNueva((ClaseDTO)respuesta.get(Constantes.KEY_RESPUESTA));
            cerrarVentana();
        } else {
            Utils.mostrarVentana("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }
    
    public void unirseAClase(String codigo){
        HashMap<String, Object> respuesta = ServicioClases.unirseAClase(codigo);
        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            Utils.mostrarVentana("Éxito", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.INFORMATION);
            menuController.enviarAClaseNueva((ClaseDTO)respuesta.get(Constantes.KEY_RESPUESTA));
            cerrarVentana();
        } else {
            Utils.mostrarVentana("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }

    @FXML
    public void btnCancelar(ActionEvent actionEvent) {
        cerrarVentana();
    }

    private void cerrarVentana() {
        limpiarCampos();
        Stage stage = (Stage) tfContenido.getScene().getWindow();
        stage.close();
    }
    
    @Override
    public void limpiarCampos(){
        tfContenido.clear();
    }
}
