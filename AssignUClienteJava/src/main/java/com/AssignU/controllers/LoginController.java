package com.AssignU.controllers;

import com.AssignU.controllers.Menu.MenuController;
import com.AssignU.utils.ApiCliente;
import com.AssignU.models.Usuarios.IniciarSesionDTO;
import com.AssignU.models.Usuarios.RespuestaIniciarSesionDTO;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.VentanaEmergente;
import javafx.collections.FXCollections;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.fxml.Initializable;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.input.MouseEvent;
import javafx.stage.Stage;

import java.io.IOException;
import java.net.URL;
import java.util.Map;
import java.util.ResourceBundle;


public class LoginController implements Initializable {
    Map<String, String> headers = Map.of("Content-Type", "application/json");

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

    public void btnIniciarSesion(ActionEvent actionEvent) {

        if (verificarCampos()) {
            String tipoUsuario = (cbTipousuarios.getValue().toString().toLowerCase());
            String nombreUsuarioOCorreo = tfNombreUsuarioCorreo.getText();
            String contrasenia = pfContrasenia.getText();
            enviarSolicitudLogin(tipoUsuario, nombreUsuarioOCorreo, contrasenia);
        } else {
            Alert ventana = VentanaEmergente.mostrarVentana("Error", "Campos Vacíos", "Favor de llenar todos los campos", Alert.AlertType.ERROR);
            ventana.showAndWait();
        }
    }

    public void lbRegistrate(MouseEvent mouseEvent) {
        cambiarARegistro();
    }

    private void inicializarVista(){
        cbTipousuarios.setItems(FXCollections.observableArrayList(Constantes.TIPO_ALUMNO, Constantes.TIPO_DOCENTE));
        ocultarLabelsErrores();
    }

    private void enviarSolicitudLogin(String tipoUsuario, String nombreUsuarioOCorreo, String contrasenia) {
        try {
            IniciarSesionDTO iniciarSesionDto = new IniciarSesionDTO(tipoUsuario, nombreUsuarioOCorreo, contrasenia);
            RespuestaIniciarSesionDTO respuesta = ApiCliente.enviarSolicitud("/usuarios/login", "POST", iniciarSesionDto, headers, RespuestaIniciarSesionDTO.class);

            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Menu/menu.fxml"));
            Parent root = loader.load();
            MenuController controller = loader.getController();
            controller.cargarValores(tipoUsuario, respuesta.getToken(), respuesta.getIdUsuario());
            Stage stage = (Stage) lbContraseniaError.getScene().getWindow();
            Scene nuevaEscena = new Scene(root);
            stage.setScene(nuevaEscena);

        } catch (Exception e) {
            Alert ventana = VentanaEmergente.mostrarVentana("Error", "Credenciales incorrectas", e.getMessage(), Alert.AlertType.ERROR);
            ventana.showAndWait();
        }

    }

    private void cambiarARegistro() {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/registroUsuario.fxml"));
            Parent nuevaVista = loader.load();
            Stage stage = (Stage) lbContraseniaError.getScene().getWindow();
            Scene nuevaEscena = new Scene(nuevaVista);
            stage.setScene(nuevaEscena);
        } catch (IOException ex) {

        }
    }

    private boolean verificarCampos() {
        Object tipoUsuario = cbTipousuarios.getValue();
        String nombreUsuarioOCorreo = tfNombreUsuarioCorreo.getText();
        String contrasenia = pfContrasenia.getText();
        boolean bandera = true;

        ocultarLabelsErrores();

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

    private void ocultarLabelsErrores() {
        lbTipoUsuarioError.setVisible(false);
        lbNombreUsuarioCorreoError.setVisible(false);
        lbContraseniaError.setVisible(false);
    }
}
