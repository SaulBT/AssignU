package com.AssignU.controllers;

import com.AssignU.models.Usuarios.Alumno.RegistrarAlumnoDTO;
import com.AssignU.models.Usuarios.Docente.RegistrarDocenteDTO;
import com.AssignU.models.Usuarios.Catalogo.GradoEstudios;
import com.AssignU.models.Usuarios.Catalogo.GradoProfesional;
import com.AssignU.utils.ApiCliente;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.VentanaEmergente;
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
import java.util.List;
import java.util.Map;
import java.util.ResourceBundle;

public class RegistroUsuarioController implements Initializable {
    private List<GradoEstudios> listaGradoEstudios;
    private List<GradoProfesional> listaGradoProfesional;
    private String mensajeError;
    Map<String, String> headers = Map.of("Content-Type", "application/json");
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
    public void initialize(URL url, ResourceBundle resourceBundle) {
        cargarCatalogos();
        configurarCbTipoUsuario();
    }

    public void clicBtnRegistrarse(ActionEvent actionEvent) {
        if (verificarCampos()) {
            registrarUsuario();
        } else {
            Alert ventana = VentanaEmergente.mostrarVentana("Error", "Campos inválidos", mensajeError, Alert.AlertType.ERROR);
            ventana.showAndWait();
        }
    }

    public void btnLbVolver(MouseEvent mouseEvent) {
        volverALogin();
    }

    private void volverALogin(){
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/login.fxml"));
            Parent nuevaVista = loader.load();
            Stage stage = (Stage) cbTipoUsuario.getScene().getWindow();
            Scene nuevaEscena = new Scene(nuevaVista);
            stage.setScene(nuevaEscena);
        } catch (IOException ex) {

        }
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

    private boolean verificarCampos() {
        restaurarCampos();
        boolean bandera = true;
        String mensaje = "";
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

        mensaje = verificarCorreo(correo, 45);
        if (!mensaje.matches("ok")){
            bandera = false;
            tfCorreo.setStyle("-fx-border-color: red");
            mensajeError = "El Correo Electrónico es inválido: " + mensaje;
        }

        mensaje = verificarCampoNormal(confirmarContrasenia, 64);
        if (!mensaje.matches("ok")){
            bandera = false;
            pfConfirmarContrasenia.setStyle("-fx-border-color: red");
            mensajeError = "El Nombre Completo es inválido: " + mensaje;
        }

        mensaje = verificarCampoNormal(contrasenia, 64);
        if (!mensaje.matches("ok")){
            bandera = false;
            pfContrasenia.setStyle("-fx-border-color: red");
            mensajeError = "El Nombre Completo es inválido: " + mensaje;
        }

        if (!verificarContrasenia(contrasenia, confirmarContrasenia)) {
            bandera = false;
            pfContrasenia.setStyle("-fx-border-color: red");
            pfConfirmarContrasenia.setStyle("-fx-border-color: red");
            mensajeError = "Las contraseñas no coinciden";
        }

        mensaje = verificarNombreUsuario(nombreUsuario, 45);
        if (!mensaje.matches("ok")){
            bandera = false;
            tfNombreUsuario.setStyle("-fx-border-color: red");
            mensajeError = "El Nombre Usuario es inválido: " + mensaje;
        }

        mensaje = verificarCampoNormal(nombreCompleto, 135);
        if (!mensaje.matches("ok")){
            bandera = false;
            tfNombreCompleto.setStyle("-fx-border-color: red");
            mensajeError = "El Nombre Completo es inválido: " + mensaje;
        }

        return bandera;
    }

    private String verificarCampoNormal(String campo, int tamanioMaximo) {
        if (campo.isEmpty()){
            return "campo vacío";
        } else if (!verificarTamanioCampo(campo, tamanioMaximo)) {
            return "no debe sobrepasar de los " + tamanioMaximo + " caracteres";
        }

        return "ok";
    }

    private String verificarNombreUsuario(String nombreUsuario, int tamanioMaximo) {
        if (nombreUsuario.isEmpty()){
            return "campo vacío";
        } else if (!verificarTamanioCampo(nombreUsuario, tamanioMaximo)) {
            return "no debe sobrepasar de los " + tamanioMaximo + " caracteres";
        } else if (!nombreUsuario.matches("[a-zA-Z0-9\\s]*")) {
            return "no puede tener caracteres especiales";
        }

        return "ok";
    }

    private String verificarCorreo(String correo, int tamanioMaximo) {
        if (correo.isEmpty()){
            return "campo vacío";
        } else if (!verificarTamanioCampo(correo, tamanioMaximo)) {
            return "no debe sobrepasar de los " + tamanioMaximo + " caracteres";
        } else if (!correo.matches("^[\\w.-]+@[\\w.-]+\\.[a-zA-Z]{2,}$")) {
            return "no tiene formato de correo electrónico";
        }

        return "ok";
    }

    private boolean verificarContrasenia(String contrasenia, String confirmarContrasenia) {
        if (!contrasenia.matches(confirmarContrasenia)){
            return false;
        } else {
            return true;
        }
    }

    private boolean verificarTamanioCampo(String campo, int tamanioMaximo) {
        char[] cadena = campo.toCharArray();
        if (cadena.length > tamanioMaximo) {
            return false;
        }
        return true;
    }

    private void restaurarCampos(){
        tfNombreUsuario.setStyle("-fx-border-color: black");
        tfNombreCompleto.setStyle("-fx-border-color: black");
        pfContrasenia.setStyle("-fx-border-color: black");
        pfConfirmarContrasenia.setStyle("-fx-border-color: black");
        tfCorreo.setStyle("-fx-border-color: black");
        cbGrado.setStyle("-fx-border-color: black");
    }

    private void registrarUsuario(){
        try {
            String tipoUsuario = cbTipoUsuario.getValue().toString();
            String nombreCompleto = tfNombreCompleto.getText();
            String nombreUsuario = tfNombreUsuario.getText();
            String contrasenia = pfContrasenia.getText();
            String correo = tfCorreo.getText();
            int idGrado = determinarGrado(tipoUsuario, cbGrado.getValue().toString());

            if (tipoUsuario.matches("Alumno")) {
                RegistrarAlumnoDTO registrarAlumnoDto = new RegistrarAlumnoDTO(nombreCompleto, nombreUsuario, contrasenia, correo, idGrado);
                ApiCliente.enviarSolicitud("/usuarios/alumnos", "POST", registrarAlumnoDto, headers, Object.class);
                VentanaEmergente.mostrarVentana("Alumno registrado", null, "Alumno registrado con éxito, por favor inicia sesión para continuar", Alert.AlertType.INFORMATION).showAndWait();
                volverALogin();
            } else if (tipoUsuario.matches("Docente")) {
                RegistrarDocenteDTO registrarDocenteDto = new RegistrarDocenteDTO(nombreCompleto, nombreUsuario, contrasenia, correo, idGrado);
                ApiCliente.enviarSolicitud("/usuarios/docentes", "POST", registrarDocenteDto, headers, Object.class);
                VentanaEmergente.mostrarVentana("Docente registrado", null, "Docente registrado con éxito, por favor inicia sesión para continuar", Alert.AlertType.INFORMATION).showAndWait();
                volverALogin();
            }
        } catch (Exception e) {
            VentanaEmergente.mostrarVentana("Error", null, e.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }

    private int determinarGrado(String tipoUsuario, String grado) {
        if (tipoUsuario.matches("Alumno")) {
            GradoEstudios[] arrayGradoEstudios = listaGradoEstudios.toArray(new GradoEstudios[0]);
            for (GradoEstudios gradoEstudios : arrayGradoEstudios) {
                if (gradoEstudios.getNombre().matches(grado)) {
                    return gradoEstudios.getIdGradoEstudios();
                }
            }
        } else if (tipoUsuario.matches("Docente")) {
            GradoProfesional[] arrayGradoProfesional = listaGradoProfesional.toArray(new GradoProfesional[0]);
            for (GradoProfesional gradoProfesional : arrayGradoProfesional) {
                if (gradoProfesional.getNombre().matches(grado)) {
                    return gradoProfesional.getIdGradoProfesional();
                }
            }
        }

        return 0;
    }
}
