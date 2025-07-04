package com.AssignU.controllers.Perfil;

import com.AssignU.models.Usuarios.Sesion;
import com.AssignU.servicios.usuarios.ServicioAlumnos;
import com.AssignU.servicios.usuarios.ServicioDocentes;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.IFormulario;
import com.AssignU.utils.Utils;
import java.util.HashMap;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.scene.control.Alert;
import javafx.scene.control.PasswordField;
import javafx.stage.Stage;

public class CambiarContraseniaController implements IFormulario{
    private PasswordField pfContraseniaNueva;
    private PasswordField pfContraseniaActual;
    private String mensajeError;
    private Sesion sesion;

    public void cargarValores(){
        this.sesion = Sesion.getSesion();
        mensajeError = "";
    }
    
    @FXML
    public void btnAceptar(ActionEvent actionEvent) {
        if (verificarCampos()) {
            guardarContrasenia(pfContraseniaActual.getText(), pfContraseniaNueva.getText());
        } else {
            Utils.mostrarAlerta("Campos inválidos", mensajeError, Alert.AlertType.ERROR);
        }
    }
    
    @Override
    public boolean verificarCampos(){
        restaurarCampos();
        boolean bandera = true;
        String mensaje;
        mensaje = Utils.verificarCampoNormal(pfContraseniaNueva.getText(), 64);
        if (!mensaje.equals("ok")){
            bandera = false;
            pfContraseniaNueva.setStyle("-fx-border-color: red");
            mensajeError = "La contraseña nueva es inválida: " + mensaje;
        }
        mensaje = Utils.verificarCampoNormal(pfContraseniaActual.getText(), 64);
        if (!mensaje.equals("ok")){
            bandera = false;
            pfContraseniaNueva.setStyle("-fx-border-color: red");
            mensajeError = "La contraseña actual es inválida: " + mensaje;
        }
        return bandera;
    }
    
    @Override
    public void restaurarCampos(){
        pfContraseniaActual.setStyle("-fx-border-color: black");
        pfContraseniaNueva.setStyle("-fx-border-color: black");
    }
    
    private void guardarContrasenia(String contraseniaActual, String contraseniaNueva){
        HashMap<String, Object> respuesta;
        if (sesion.esDocente()) {
            respuesta = ServicioDocentes.cambiarContrasenia(contraseniaActual, contraseniaNueva);
        } else {
            respuesta = ServicioAlumnos.cambiarContrasenia(contraseniaActual, contraseniaNueva);
        }

        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            Utils.mostrarAlerta("Éxito", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.INFORMATION);
            cerrarVentana();
        } else {
            Utils.mostrarAlerta("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }

    @FXML
    public void btnCancelar(ActionEvent actionEvent) {
        cerrarVentana();
    }

    private void cerrarVentana(){
        limpiarCampos();
        Stage escenario = (Stage) pfContraseniaActual.getScene().getWindow();
        escenario.close();
    }
    
    @Override
    public void limpiarCampos(){
        pfContraseniaActual.clear();
        pfContraseniaNueva.clear();
    }
}
