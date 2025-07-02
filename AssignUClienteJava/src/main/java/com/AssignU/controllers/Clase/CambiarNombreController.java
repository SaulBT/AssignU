package com.AssignU.controllers.Clase;

import com.AssignU.models.Clases.ClaseDTO;
import com.AssignU.servicios.ServicioClases;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.IFormulario;
import com.AssignU.utils.Utils;
import java.util.HashMap;
import javafx.event.ActionEvent;
import javafx.scene.control.Alert;
import javafx.scene.control.TextField;
import javafx.stage.Stage;

public class CambiarNombreController implements IFormulario{
    public TextField tfNombre;
    private String mensajeError;
    private int idClase;
    private ClaseController claseController;
    
    public void cargarValores(ClaseController claseController, int idClase, String nombreClase){
        this.claseController = claseController;
        tfNombre.setText(nombreClase);
        this.idClase = idClase;
    }

    public void btnAceptar(ActionEvent actionEvent) {
        if(verificarCampos()){
            guardarCambios();
        } else {
            tfNombre.setStyle("-fx-border-color: red");
            Utils.mostrarVentana("Campo inválido.", mensajeError, Alert.AlertType.ERROR);
        }
    }

    @Override
    public boolean verificarCampos() {
        restaurarCampos();
        String nombreClase = tfNombre.getText();
        boolean error = true;
        
        if(nombreClase.isEmpty()){
            error = false;
            tfNombre.setStyle("-fx-border-color: red");
        } else if (!Utils.verificarTamanioCampo(nombreClase, 44)){
            mensajeError = "El nombre debe de ser menor a 45 caracteres.";
            error = false;
            tfNombre.setStyle("-fx-border-color: red");
        }
        return error;
    }

    @Override
    public void restaurarCampos() {
        tfNombre.setStyle("-fx-border-color: black");
    }
    
    private void guardarCambios(){
        String nombreClase = tfNombre.getText();
        HashMap<String, Object> respuesta = ServicioClases.actualizarClase(idClase, nombreClase);
        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            Utils.mostrarVentana("Éxito", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.INFORMATION);
            claseController.cargarValores((ClaseDTO)respuesta.get(Constantes.KEY_RESPUESTA));
            cerrarVentana();
        } else {
            Utils.mostrarVentana("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }

    public void btnCancelar(ActionEvent actionEvent) {
        cerrarVentana();
    }

    private void cerrarVentana() {
        limpiarCampos();
        Stage stage = (Stage) tfNombre.getScene().getWindow();
        stage.close();
    }

    @Override
    public void limpiarCampos() {
        tfNombre.clear();
    }
}
