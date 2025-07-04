package com.AssignU.controllers;

import com.AssignU.controllers.Menu.MenuController;
import com.AssignU.models.Usuarios.RespuestaIniciarSesionDTO;
import com.AssignU.models.Usuarios.Sesion;
import com.AssignU.servicios.usuarios.ServicioLogin;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.IFormulario;
import com.AssignU.utils.Navegador;
import com.AssignU.utils.Utils;
import javafx.collections.FXCollections;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.Initializable;
import javafx.scene.control.*;
import javafx.scene.input.MouseEvent;

import java.net.URL;
import java.util.HashMap;
import java.util.ResourceBundle;


public class LoginController implements Initializable, IFormulario {

    @FXML
    public Label lbTipoUsuarioError;
    @FXML
    public TextField tfNombreUsuarioCorreo;
    @FXML
    public Label lbNombreUsuarioCorreoError;
    @FXML
    public PasswordField pfContrasenia;
    @FXML
    public Label lbContraseniaError;
    @FXML
    public ComboBox cbTipousuarios;

    @Override
    public void initialize(URL url, ResourceBundle resourceBundle) {
        inicializarVista();
    }

    private void inicializarVista(){
        cbTipousuarios.setItems(FXCollections.observableArrayList(Constantes.TIPO_ALUMNO, Constantes.TIPO_DOCENTE));
        restaurarCampos();
    }

    public void btnIniciarSesion(ActionEvent actionEvent) {
        if (verificarCampos()) {
            enviarSolicitudLogin((cbTipousuarios.getValue().toString().toLowerCase()),
                tfNombreUsuarioCorreo.getText(), pfContrasenia.getText());
        } else {
            Utils.mostrarAlerta("Campos Vacíos", "Favor de llenar todos los campos", Alert.AlertType.ERROR);
        }
    }

    @Override
    public boolean verificarCampos() {
        restaurarCampos();
        Object tipoUsuario = cbTipousuarios.getValue();
        String nombreUsuarioOCorreo = tfNombreUsuarioCorreo.getText();
        String contrasenia = pfContrasenia.getText();
        boolean bandera = true;


        if (tipoUsuario == null || tipoUsuario.toString().isEmpty() ) {
            lbTipoUsuarioError.setVisible(true);
            bandera = false;
        }
        if (nombreUsuarioOCorreo == null || nombreUsuarioOCorreo.isEmpty()) {
            lbNombreUsuarioCorreoError.setVisible(true);
            bandera = false;
        }
        if (contrasenia == null || contrasenia.isEmpty()) {
            lbContraseniaError.setVisible(true);
            bandera = false;
        }

        return bandera;
    }

    @Override
    public void restaurarCampos() {
        lbTipoUsuarioError.setVisible(false);
        lbNombreUsuarioCorreoError.setVisible(false);
        lbContraseniaError.setVisible(false);
    }

    private void enviarSolicitudLogin(String tipoUsuario, String nombreUsuarioOCorreo, String contrasenia) {
        HashMap<String, Object> respuesta = ServicioLogin.iniciarSesion(tipoUsuario, nombreUsuarioOCorreo, contrasenia);
        boolean esDocente = cbTipousuarios.getValue().equals(Constantes.TIPO_DOCENTE);
        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)){
            RespuestaIniciarSesionDTO credenciales = (RespuestaIniciarSesionDTO) respuesta.get(Constantes.KEY_RESPUESTA);
            
            Sesion.iniciarSesion(esDocente, credenciales.getToken(), credenciales.getIdUsuario());
            
            limpiarCampos();
            //----------------------------------------------------
            Navegador.cambiarVentana(
                lbContraseniaError.getScene(),
                "/views/Menu/menu.fxml",
                "Clases", 
                controller -> ((MenuController) controller).cargarValores()
            );
        } else {
            Utils.mostrarAlerta("Error al iniciar sesión", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }

    public void lbRegistrate(MouseEvent mouseEvent) {
        cambiarARegistro();
    }

    private void cambiarARegistro() {
        limpiarCampos();
        Navegador.cambiarVentana(
            lbContraseniaError.getScene(),
            "/views/registroUsuario.fxml",
            "Registro",
            null
        );
    }
    
    @Override
    public void limpiarCampos(){
        tfNombreUsuarioCorreo.clear();
        pfContrasenia.clear();
    }
}
