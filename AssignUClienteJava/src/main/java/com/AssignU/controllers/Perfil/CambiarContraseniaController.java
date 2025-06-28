package com.AssignU.controllers.Perfil;

import com.AssignU.models.Perfil.CambiarContraseniaDTO;
import com.AssignU.models.Usuarios.Sesion;
import com.AssignU.utils.ApiCliente;
import com.AssignU.utils.IFormulario;
import com.AssignU.utils.Utils;
import com.AssignU.utils.VentanaEmergente;
import java.util.HashMap;
import java.util.Map;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.scene.control.Alert;
import javafx.scene.control.PasswordField;
import javafx.stage.Stage;

public class CambiarContraseniaController implements IFormulario{
    private PasswordField pfContraseniaNueva;
    private PasswordField pfContraseniaActual;
    private String mensajeError;
    private boolean esDocente;
    private Sesion sesion;
    private Map<String, String> headers = new HashMap<String, String>();

    public void cargarValores(Sesion sesion){
        this.sesion = sesion;
        
        headers.put("Content-Type", "application/json");
        headers.put("Authorization", "Bearer " + sesion.jwt);
        
        mensajeError = "";
        if (sesion.tipoUsuario.matches("alumno")) {
            esDocente = false;
        } else if (sesion.tipoUsuario.matches("docente")) {
            esDocente = true;
        }
    }
    
    @FXML
    public void btnAceptar(ActionEvent actionEvent) {
        if (verificarCampos()) {
            CambiarContraseniaDTO cambiarContraseniaDto = new CambiarContraseniaDTO(pfContraseniaActual.getText(), pfContraseniaNueva.getText());
            guardarContrasenia(cambiarContraseniaDto);
        } else {
            VentanaEmergente.mostrarVentana("Error", "Campos inválidos", mensajeError, Alert.AlertType.ERROR).showAndWait();
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
    
    private void guardarContrasenia(CambiarContraseniaDTO cambiarContraseniaDto){
        try {
            String endpoint;
            if(esDocente){
                endpoint = String.format("/usuarios/docentes/%s/contrasenia", sesion.idUsuario);
            }else{
                endpoint = String.format("/usuarios/alumnos/%s/contrasenia", sesion.idUsuario);
            }
            ApiCliente.enviarSolicitud(endpoint, "PUT", cambiarContraseniaDto, headers, Object.class);
            VentanaEmergente.mostrarVentana("Éxito", null, "Contraseña guardada con éxito.", Alert.AlertType.INFORMATION).showAndWait();
            cerrarVentana();
        } catch (Exception e) {
            VentanaEmergente.mostrarVentana("Error", "Contraseña Incorrecta", e.getMessage(), Alert.AlertType.ERROR).showAndWait();
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
