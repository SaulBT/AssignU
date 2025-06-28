package com.AssignU.controllers.Perfil;

import com.AssignU.models.Usuarios.Alumno.ActualizarAlumnoDTO;
import com.AssignU.models.Usuarios.Catalogo.GradoEstudios;
import com.AssignU.models.Usuarios.Catalogo.GradoProfesional;
import com.AssignU.models.Usuarios.Docente.ActualizarDocenteDTO;
import com.AssignU.models.Usuarios.Sesion;
import com.AssignU.utils.ApiCliente;
import com.AssignU.utils.IFormulario;
import com.AssignU.utils.Utils;
import com.AssignU.utils.VentanaEmergente;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import javafx.collections.FXCollections;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.scene.control.Alert;
import javafx.scene.control.ComboBox;
import javafx.scene.control.Label;
import javafx.scene.control.TextField;
import javafx.stage.Stage;
import javafx.util.StringConverter;

public class EditarPerfilController implements IFormulario{
    private TextField tfNombreCompleto;
    private TextField tfNombreUsuario;
    @FXML
    private Label lbGrado;
    
    private Sesion sesion;
    private boolean esDocente;
    private String mensajeError;
    
    private Map<String, String> headers = new HashMap<String, String>();
    
    @FXML
    private ComboBox<GradoEstudios> cbGradoEstudios;
    @FXML
    private ComboBox<GradoProfesional> cbGradoProfesional;
    
    public void cargarValores(Sesion sesion, String nombreCompleto, String nombreUsuario, int idGrado){
        this.sesion = sesion;
        tfNombreCompleto.setText(nombreCompleto);
        tfNombreUsuario.setText(nombreUsuario);
        if (sesion.tipoUsuario.matches("alumno")) {
            esDocente = false;
            lbGrado.setText("Grado de Estudios:");
            configurarGradoEstudios(idGrado);
        } else if (sesion.tipoUsuario.matches("docente")) {
            esDocente = true;
            lbGrado.setText("Grado Profesional:");
            configurarGradoProfesional(idGrado);
        }
        cbGradoProfesional.setVisible(esDocente);
        cbGradoEstudios.setVisible(!esDocente);
    }
    
    private void configurarGradoEstudios(int idGrado){
        try {
            List<GradoEstudios> listaGradoEstudios = ApiCliente.enviarSolicitudLista("/usuarios/catalogos/grados-estudios", "GET", null, null, GradoEstudios.class);
            
            cbGradoEstudios.setItems(FXCollections.observableArrayList(listaGradoEstudios));

            cbGradoEstudios.setConverter(new StringConverter<GradoEstudios>() {
                @Override
                public String toString(GradoEstudios grado) {
                    return grado != null ? grado.nombre : "";
                }

                @Override
                public GradoEstudios fromString(String nombre) {
                    return cbGradoEstudios.getItems().stream()
                                   .filter(g -> g.nombre.equals(nombre))
                                   .findFirst()
                                   .orElse(null);
                }
            });

            for (GradoEstudios grado : listaGradoEstudios) {
                if (grado.idGradoEstudios == idGrado) {
                    cbGradoEstudios.getSelectionModel().select(grado);
                    break;
                }
            }
            
        } catch (Exception e) {
            VentanaEmergente.mostrarVentana("Error", null, e.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }
    
    private void configurarGradoProfesional(int idGrado){
        try {
            List<GradoProfesional> listaGradoProfesional = ApiCliente.enviarSolicitudLista("/usuarios/catalogos/grados-profesionales", "GET", null, null, GradoProfesional.class);
            
            cbGradoProfesional.setItems(FXCollections.observableArrayList(listaGradoProfesional));

            cbGradoProfesional.setConverter(new StringConverter<GradoProfesional>() {
                @Override
                public String toString(GradoProfesional grado) {
                    return grado != null ? grado.nombre : "";
                }

                @Override
                public GradoProfesional fromString(String nombre) {
                    return cbGradoProfesional.getItems().stream()
                                   .filter(g -> g.nombre.equals(nombre))
                                   .findFirst()
                                   .orElse(null);
                }
            });

            for (GradoProfesional grado : listaGradoProfesional) {
                if (grado.idGradoProfesional == idGrado) {
                    cbGradoProfesional.getSelectionModel().select(grado);
                    break;
                }
            }
            
        } catch (Exception e) {
            VentanaEmergente.mostrarVentana("Error", null, e.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }

    @FXML
    public void btnAceptar(ActionEvent actionEvent) {
        if(verificarCampos()){
            if(esDocente){
                guardarPerfilDocente();
            }else{
                guardarPerfilAlumno();
            }
        } else {
            Alert ventana = VentanaEmergente.mostrarVentana("Error", "Campos inválidos", mensajeError, Alert.AlertType.ERROR);
            ventana.showAndWait();
        }
        
    }
    
    @Override
    public boolean verificarCampos(){
        restaurarCampos();
        boolean bandera = true;
        String mensaje = "";

        if (cbGradoEstudios.getValue() == null || cbGradoProfesional.getValue() == null ) {
            bandera = false;
            cbGradoEstudios.setStyle("-fx-border-color: red");
            cbGradoProfesional.setStyle("-fx-border-color: red");
            mensajeError = "Selecciona un Grado";
        }

        mensaje = Utils.verificarNombreUsuario(tfNombreUsuario.getText(), 45);
        if (!mensaje.equals("ok")){
            bandera = false;
            tfNombreUsuario.setStyle("-fx-border-color: red");
            mensajeError = "El Nombre Usuario es inválido: " + mensaje;
        }

        mensaje = Utils.verificarCampoNormal(tfNombreCompleto.getText(), 135);
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
        if(esDocente){
            cbGradoProfesional.setStyle("-fx-border-color: black");
        }else{
            cbGradoEstudios.setStyle("-fx-border-color: black");
        }
    }
    
    private void guardarPerfilAlumno(){
        GradoEstudios seleccionado = cbGradoEstudios.getValue();
        ActualizarAlumnoDTO alumno = new ActualizarAlumnoDTO(tfNombreCompleto.getText(), tfNombreUsuario.getText(), seleccionado.idGradoEstudios);
        try {
            headers.put("Content-Type", "application/json");
            headers.put("Authorization", "Bearer " + sesion.jwt);
            String endpoint = String.format("/usuarios/alumnos/%s", sesion.idUsuario);
            ApiCliente.enviarSolicitud(endpoint, "PUT", alumno, headers, Object.class);
        } catch (Exception e) {
            VentanaEmergente.mostrarVentana("Error", null, e.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }
    
    private void guardarPerfilDocente(){
        GradoProfesional seleccionado = cbGradoProfesional.getValue();
        ActualizarDocenteDTO docente = new ActualizarDocenteDTO(tfNombreUsuario.getText(), tfNombreCompleto.getText(), seleccionado.idGradoProfesional);
        try {
            headers.put("Content-Type", "application/json");
            headers.put("Authorization", "Bearer " + sesion.jwt);
            String endpoint = String.format("/usuarios/docentes/%s", sesion.idUsuario);
            ApiCliente.enviarSolicitud(endpoint, "PUT", docente, headers, Object.class);
        } catch (Exception e) {
            VentanaEmergente.mostrarVentana("Error", null, e.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }

    @FXML
    public void btnCancelar(ActionEvent actionEvent) {
        cerrarVentana();
    }

    private void cerrarVentana(){
        Stage escenario = (Stage) tfNombreCompleto.getScene().getWindow();
        escenario.close();
    }
    
    @Override
    public void limpiarCampos(){
        tfNombreCompleto.clear();
        tfNombreUsuario.clear();
    }
}
