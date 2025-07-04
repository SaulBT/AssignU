package com.AssignU.controllers.Perfil;

import com.AssignU.models.Usuarios.Catalogo.GradoEstudioDTO;
import com.AssignU.models.Usuarios.Catalogo.GradoProfesionalDTO;
import com.AssignU.models.Usuarios.Sesion;
import com.AssignU.servicios.usuarios.ServicioAlumnos;
import com.AssignU.servicios.usuarios.ServicioCatalogos;
import com.AssignU.servicios.usuarios.ServicioDocentes;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.IFormulario;
import com.AssignU.utils.Utils;
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
    private String mensajeError;
    
    @FXML
    private ComboBox<GradoEstudioDTO> cbGradoEstudios;
    @FXML
    private ComboBox<GradoProfesionalDTO> cbGradoProfesional;
    
    public void cargarValores(String nombreCompleto, String nombreUsuario, int idGrado){
        this.sesion = Sesion.getSesion();
        tfNombreCompleto.setText(nombreCompleto);
        tfNombreUsuario.setText(nombreUsuario);
        if (sesion.esDocente()) {
            lbGrado.setText("Grado de Estudios:");
            configurarGradoEstudios(idGrado);
        } else {
            lbGrado.setText("Grado Profesional:");
            configurarGradoProfesional(idGrado);
        }
        cbGradoProfesional.setVisible(sesion.esDocente());
        cbGradoEstudios.setVisible(!sesion.esDocente());
    }
    
    private void configurarGradoEstudios(int idGrado){
        HashMap<String, Object> gradosEstudios = ServicioCatalogos.obtenerGradosDeEstudios();
        if (!(boolean) gradosEstudios.get(Constantes.KEY_ERROR)) {
            List<GradoEstudioDTO> listaGradoEstudios = (List<GradoEstudioDTO>) gradosEstudios.get(Constantes.KEY_RESPUESTA);
            
            cbGradoEstudios.setItems(FXCollections.observableArrayList(listaGradoEstudios));

            cbGradoEstudios.setConverter(new StringConverter<GradoEstudioDTO>() {
                @Override
                public String toString(GradoEstudioDTO grado) {
                    return grado != null ? grado.nombre : "";
                }

                @Override
                public GradoEstudioDTO fromString(String nombre) {
                    return cbGradoEstudios.getItems().stream()
                                   .filter(g -> g.nombre.equals(nombre))
                                   .findFirst()
                                   .orElse(null);
                }
            });

            for (GradoEstudioDTO grado : listaGradoEstudios) {
                if (grado.idGradoEstudios == idGrado) {
                    cbGradoEstudios.getSelectionModel().select(grado);
                    break;
                }
            }
            
        } else {
            Utils.mostrarAlerta("Error", (String) gradosEstudios.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }
    
    private void configurarGradoProfesional(int idGrado){
        HashMap<String, Object> gradosProfesionales = ServicioCatalogos.obtenerGradosProfesionales();
        if (!(boolean) gradosProfesionales.get(Constantes.KEY_ERROR)) {
            List<GradoProfesionalDTO> listaGradoProfesional = (List<GradoProfesionalDTO>) gradosProfesionales.get(Constantes.KEY_RESPUESTA);
            
            cbGradoProfesional.setItems(FXCollections.observableArrayList(listaGradoProfesional));

            cbGradoProfesional.setConverter(new StringConverter<GradoProfesionalDTO>() {
                @Override
                public String toString(GradoProfesionalDTO grado) {
                    return grado != null ? grado.nombre : "";
                }

                @Override
                public GradoProfesionalDTO fromString(String nombre) {
                    return cbGradoProfesional.getItems().stream()
                                   .filter(g -> g.nombre.equals(nombre))
                                   .findFirst()
                                   .orElse(null);
                }
            });

            for (GradoProfesionalDTO grado : listaGradoProfesional) {
                if (grado.idGradoProfesional == idGrado) {
                    cbGradoProfesional.getSelectionModel().select(grado);
                    break;
                }
            }
            
        } else {
            Utils.mostrarAlerta("Error", (String) gradosProfesionales.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }

    @FXML
    public void btnAceptar(ActionEvent actionEvent) {
        if(verificarCampos()){
        String nombreUsuario = tfNombreUsuario.getText();
        String nombreCompleto = tfNombreCompleto.getText();
            if(sesion.esDocente()){
                guardarPerfilDocente(nombreUsuario, nombreCompleto);
            }else{
                guardarPerfilAlumno(nombreUsuario, nombreCompleto);
            }
        } else {
            Utils.mostrarAlerta("Campos inválidos", mensajeError, Alert.AlertType.ERROR);
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
        if(sesion.esDocente()){
            cbGradoProfesional.setStyle("-fx-border-color: black");
        }else{
            cbGradoEstudios.setStyle("-fx-border-color: black");
        }
    }
    
    private void guardarPerfilAlumno(String nombreUsuario, String nombreCompleto){
        GradoEstudioDTO seleccionado = cbGradoEstudios.getValue();

        HashMap<String, Object> respuesta = ServicioAlumnos.actualizarAlumno(nombreCompleto, nombreUsuario, seleccionado.idGradoEstudios);

        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            Utils.mostrarAlerta("Éxito", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.INFORMATION);
        } else {
            Utils.mostrarAlerta("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }
    
    private void guardarPerfilDocente(String nombreUsuario, String nombreCompleto){
        GradoProfesionalDTO seleccionado = cbGradoProfesional.getValue();

        HashMap<String, Object> respuesta = ServicioDocentes.actualizarDocente(nombreCompleto, nombreUsuario,seleccionado.getIdGradoProfesional());

        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            Utils.mostrarAlerta("Éxito", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.INFORMATION);
        } else {
            Utils.mostrarAlerta("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
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
