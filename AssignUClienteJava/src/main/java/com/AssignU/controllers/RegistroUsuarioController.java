package com.AssignU.controllers;

import com.AssignU.models.Usuarios.Catalogo.GradoEstudios;
import com.AssignU.models.Usuarios.Catalogo.GradoProfesional;
import com.AssignU.servicios.usuarios.ServicioAlumnos;
import com.AssignU.servicios.usuarios.ServicioDocentes;
import com.AssignU.utils.ApiCliente;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.IFormulario;
import com.AssignU.utils.Navegador;
import com.AssignU.utils.Utils;
import com.google.common.reflect.TypeToken;
import javafx.collections.FXCollections;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.fxml.Initializable;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.input.MouseEvent;
import javafx.scene.layout.HBox;
import javafx.stage.Stage;
import java.io.IOException;
import java.lang.reflect.Type;
import java.net.URL;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.ResourceBundle;

public class RegistroUsuarioController implements Initializable, IFormulario {
    private List<GradoEstudios> listaGradoEstudios;
    private List<GradoProfesional> listaGradoProfesional;
    private String mensajeError;
    
    @FXML
    public ComboBox cbTipoUsuario;
    @FXML
    public HBox hbNombreCompleto;
    @FXML
    public TextField tfNombreCompleto;
    @FXML
    public HBox hbNombreUsuario;
    @FXML
    public TextField tfNombreUsuario;
    @FXML
    public HBox hbContrasenia;
    @FXML
    public PasswordField pfContrasenia;
    @FXML
    public HBox hbConfirmarContrasenia;
    @FXML
    public PasswordField pfConfirmarContrasenia;
    @FXML
    public HBox hbCorreo;
    @FXML
    public TextField tfCorreo;
    @FXML
    public HBox hbGrado;
    @FXML
    public ComboBox cbGrado;
    @FXML
    public Label lbGrado;
    @FXML
    public Button btnRegistrarse;

    @Override
    public void initialize(URL url, ResourceBundle resourceBundle){
        cargarCatalogos();
        configurarCbTipoUsuario();
    }

    private void cargarCatalogos(){
        try {
            Type tipoLista = new TypeToken<List<GradoEstudios>>() {}.getType();
            listaGradoEstudios = ApiCliente.enviarSolicitudLista("/usuarios/catalogos/grados-estudios", "GET", null, null, GradoEstudios.class);
            listaGradoProfesional = ApiCliente.enviarSolicitudLista("/usuarios/catalogos/grados-profesionales", "GET", null, null, GradoProfesional.class);
        } catch (Exception e) {
            throw new RuntimeException(e);
        }
    }

    private void configurarCbTipoUsuario() {
        cbTipoUsuario.setItems(FXCollections.observableArrayList(Constantes.TIPO_ALUMNO, Constantes.TIPO_DOCENTE));
        cbTipoUsuario.setOnAction(event -> {
            Object tipoSeleccionado = cbTipoUsuario.getValue();

            if (tipoSeleccionado != null) {
                if (tipoSeleccionado.toString().equals(Constantes.TIPO_ALUMNO)) {
                    cambiarCamposAAlumno();
                } else if (tipoSeleccionado.toString().equals(Constantes.TIPO_DOCENTE)) {
                    cambiarCamposADocente();
                }

                mostrarCampos();
            }
        });
    }

    private void cambiarCamposAAlumno(){
        lbGrado.setText("Grado de estudios:");

        List<String> nombreGrado = new ArrayList<>();
        GradoEstudios[] array = listaGradoEstudios.toArray(new GradoEstudios[0]);
        for (int i = 0; i < array.length; i++) {
            nombreGrado.add(array[i].nombre);
        }
        cbGrado.setItems(FXCollections.observableArrayList(nombreGrado));
    }

    private void cambiarCamposADocente(){
        lbGrado.setText("Grado profesional:");

        List<String> nombreGrado = new ArrayList<>();
        GradoProfesional[] array = listaGradoProfesional.toArray(new GradoProfesional[0]);
        for (int i = 0; i < array.length; i++) {
            nombreGrado.add(array[i].nombre);
        }
        cbGrado.setItems(FXCollections.observableArrayList(nombreGrado));
    }

    private void mostrarCampos(){
        hbNombreUsuario.setVisible(true);
        hbNombreCompleto.setVisible(true);
        hbContrasenia.setVisible(true);
        hbConfirmarContrasenia.setVisible(true);
        hbCorreo.setVisible(true);
        hbGrado.setVisible(true);
        btnRegistrarse.setDisable(false);
    }

    public void clicBtnRegistrarse(ActionEvent actionEvent) {
        if (verificarCampos()) {
            registrarUsuario();
        } else {
            Utils.mostrarVentana("Campos inválidos", mensajeError, Alert.AlertType.ERROR);
        }
    }

    @Override
    public boolean verificarCampos() {
        restaurarCampos();
        boolean bandera = true;
        String mensaje;
        String nombreCompleto = tfNombreCompleto.getText();
        String nombreUsuario = tfNombreUsuario.getText();
        String contrasenia = pfContrasenia.getText();
        String confirmarContrasenia = pfConfirmarContrasenia.getText();
        String correo = tfCorreo.getText();

        if (cbGrado.getValue() == null) {
            bandera = false;
            cbGrado.setStyle("-fx-border-color: red");
            mensajeError = "Selecciona un Grado";
        }

        mensaje = Utils.verificarCorreo(correo, 45);
        if (!mensaje.equals("ok")){
            bandera = false;
            tfCorreo.setStyle("-fx-border-color: red");
            mensajeError = "El Correo Electrónico es inválido: " + mensaje;
        }

        mensaje = Utils.verificarCampoNormal(confirmarContrasenia, 64);
        if (!mensaje.equals("ok")){
            bandera = false;
            pfConfirmarContrasenia.setStyle("-fx-border-color: red");
            mensajeError = "La contraseña es inválida: " + mensaje;
        }

        mensaje = Utils.verificarCampoNormal(contrasenia, 64);
        if (!mensaje.equals("ok")){
            bandera = false;
            pfContrasenia.setStyle("-fx-border-color: red");
            mensajeError = "La contraseña es inválida: " + mensaje;
        }

        if (!Utils.verificarContrasenia(contrasenia, confirmarContrasenia)) {
            bandera = false;
            pfContrasenia.setStyle("-fx-border-color: red");
            pfConfirmarContrasenia.setStyle("-fx-border-color: red");
            mensajeError = "Las contraseñas no coinciden";
        }

        mensaje = Utils.verificarNombreUsuario(nombreUsuario, 45);
        if (!mensaje.equals("ok")){
            bandera = false;
            tfNombreUsuario.setStyle("-fx-border-color: red");
            mensajeError = "El Nombre Usuario es inválido: " + mensaje;
        }

        mensaje = Utils.verificarCampoNormal(nombreCompleto, 135);
        if (!mensaje.equals("ok")){
            bandera = false;
            tfNombreCompleto.setStyle("-fx-border-color: red");
            mensajeError = "El Nombre Completo es inválido: " + mensaje;
        }

        return bandera;
    }

    @Override
    public void restaurarCampos(){
        tfNombreUsuario.setStyle("-fx-border-color: black");
        tfNombreCompleto.setStyle("-fx-border-color: black");
        pfContrasenia.setStyle("-fx-border-color: black");
        pfConfirmarContrasenia.setStyle("-fx-border-color: black");
        tfCorreo.setStyle("-fx-border-color: black");
        cbGrado.setStyle("-fx-border-color: black");
    }

    private void registrarUsuario(){
        String tipoUsuario = cbTipoUsuario.getValue().toString();
        String nombreCompleto = tfNombreCompleto.getText();
        String nombreUsuario = tfNombreUsuario.getText();
        String contrasenia = pfContrasenia.getText();
        String correo = tfCorreo.getText();
        int idGrado = determinarGrado(tipoUsuario, cbGrado.getValue().toString());

        HashMap<String, Object> respuesta = new HashMap<>();
        
        if (tipoUsuario.equals("Alumno")) {
            respuesta = ServicioAlumnos.registrarAlumno(nombreCompleto, nombreUsuario, contrasenia, correo, idGrado);
        } else if (tipoUsuario.equals("Docente")) {
            respuesta = ServicioDocentes.registrarDocente(nombreCompleto, nombreUsuario, contrasenia, correo, idGrado);
        } else {
            respuesta.put(Constantes.KEY_ERROR, true);
            respuesta.put(Constantes.KEY_MENSAJE, "Tipo de usuario no reconocido.");
        }
        
        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            Utils.mostrarVentana("Registro exitoso", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.INFORMATION);
            volverALogin();
        } else {
            Utils.mostrarVentana("Error al registrar", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }

    private int determinarGrado(String tipoUsuario, String grado) {
        if (tipoUsuario.equals("Alumno")) {
            GradoEstudios[] arrayGradoEstudios = listaGradoEstudios.toArray(new GradoEstudios[0]);
            for (GradoEstudios gradoEstudios : arrayGradoEstudios) {
                if (gradoEstudios.getNombre().equals(grado)) {
                    return gradoEstudios.getIdGradoEstudios();
                }
            }
        } else if (tipoUsuario.equals("Docente")) {
            GradoProfesional[] arrayGradoProfesional = listaGradoProfesional.toArray(new GradoProfesional[0]);
            for (GradoProfesional gradoProfesional : arrayGradoProfesional) {
                if (gradoProfesional.getNombre().equals(grado)) {
                    return gradoProfesional.getIdGradoProfesional();
                }
            }
        }

        return 0;
    }

    public void btnLbVolver(MouseEvent mouseEvent) {
        volverALogin();
    }

    private void volverALogin(){
        limpiarCampos();
        Navegador.cambiarVentana(
            cbTipoUsuario.getScene(),
            "/views/login.fxml",
            null
        );
    }
    
    @Override
    public void limpiarCampos(){
        tfNombreCompleto.clear();
        tfNombreUsuario.clear();
        pfContrasenia.clear();
        pfConfirmarContrasenia.clear();
        tfCorreo.clear();
    }
}
