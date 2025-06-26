package com.AssignU.controllers.Perfil;

import com.AssignU.models.Perfil.EstadisticasPerfilDTO;
import com.AssignU.models.Usuarios.Catalogo.GradoEstudios;
import com.AssignU.models.Usuarios.Catalogo.GradoProfesional;
import com.AssignU.models.Usuarios.Docente.DocenteDTO;
import com.AssignU.models.Usuarios.Sesion;
import com.AssignU.utils.ApiCliente;
import com.AssignU.utils.VentanaEmergente;
import java.util.HashMap;
import java.util.Map;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.input.MouseEvent;
import javafx.scene.layout.HBox;
import javafx.stage.Modality;
import javafx.stage.Stage;

public class PerfilController{
    public Label lbPerfilNombreUsuario;
    public Label lbNombreUsuario;
    public Label lbNombreCompleto;
    public Label lbCorreoElectronico;
    public Label lbGradoEstudios;
    public ComboBox cbClase;
    @FXML
    private TableView<?> tvDatosClase;
    public TableColumn tcNombre;
    public TableColumn tcPregunta;
    public TableColumn tcTotal;
    public TableColumn tcCalificacion;
    public Label lbPromedio;
    public Button btnCambiarContrasenia;
    public Button btnEditarPerfil;
    public Label lbTextoGradoEstudios;
    public ScrollPane spInformacionClase;
    
    private Sesion sesion;
    private String mensajeError;
    private boolean esDocente;
    private int idGrado;
    
    private Map<String, String> headers = new HashMap<String, String>();
    @FXML
    private HBox hbClase;
    

    public void cargarValores(Sesion sesion){
        this.sesion = sesion;
        if (sesion.tipoUsuario.matches("alumno")) {
            esDocente = false;
            obtenerDatosAlumno();
        } else if (sesion.tipoUsuario.matches("docente")) {
            esDocente = true;
            obtenerDatosDocente();
        }
    }
    
    // A L U M N O
    private void obtenerDatosAlumno(){
        try {
            headers.put("Content-Type", "application/json");
            headers.put("Authorization", "Bearer " + sesion.jwt);
            String endpoint = String.format("/usuarios/alumnos/%s/estadisticas", sesion.idUsuario);
            EstadisticasPerfilDTO estadisticasDto = ApiCliente.enviarSolicitud(endpoint, "GET", null, headers, EstadisticasPerfilDTO.class);
            cargarVentanaAlumno(estadisticasDto);
        } catch (Exception e) {
            VentanaEmergente.mostrarVentana("Error", null, e.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }
    
    private void cargarVentanaAlumno(EstadisticasPerfilDTO estadisticasDto){
        lbPerfilNombreUsuario.setText("Perfil - " + estadisticasDto.nombreUsuario);
        lbNombreUsuario.setText(estadisticasDto.nombreUsuario);
        lbNombreCompleto.setText(estadisticasDto.nombreCompleto);
        lbCorreoElectronico.setText(estadisticasDto.correo);
        idGrado = estadisticasDto.idGradoEstudios;
        cargarGradoEstudios(estadisticasDto.idGradoEstudios);
        //TO DO set comboBox y tableView
    }
    
    private void cargarGradoEstudios(int idGradoEstudios){
        lbTextoGradoEstudios.setText("Grado estudios:");
        try {
            headers.put("Content-Type", "application/json");
            headers.put("Authorization", "Bearer " + sesion.jwt);
            String endpoint = String.format("/usuarios/catalogos/grados-estudios/%s", idGradoEstudios);
            GradoEstudios gradoEstudios = ApiCliente.enviarSolicitud(endpoint, "GET", null, headers, GradoEstudios.class);
            lbGradoEstudios.setText(gradoEstudios.getNombre());
        } catch (Exception e) {
            VentanaEmergente.mostrarVentana("Error", null, e.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }
    
    private void configurarInformacionClases(EstadisticasPerfilDTO estadisticasDto){
        cbClase.getItems().setAll(estadisticasDto.getClases());
    }
    
    // D O C E N T E
    private void obtenerDatosDocente(){
        try {
            headers.put("Content-Type", "application/json");
            headers.put("Authorization", "Bearer " + sesion.jwt);
            String endpoint = String.format("/usuarios/docentes/%s", sesion.idUsuario);
            DocenteDTO docenteDto = ApiCliente.enviarSolicitud(endpoint, "GET", null, headers, DocenteDTO.class);
            cargarVentanaDocente(docenteDto);
        } catch (Exception e) {
            VentanaEmergente.mostrarVentana("Error", null, e.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }
    
    private void cargarVentanaDocente(DocenteDTO docenteDto){
        lbPerfilNombreUsuario.setText("Perfil - " + docenteDto.nombreUsuario);
        lbNombreUsuario.setText(docenteDto.nombreUsuario);
        lbNombreCompleto.setText(docenteDto.nombreCompleto);
        lbCorreoElectronico.setText(docenteDto.correo);
        idGrado = docenteDto.idGradoProfesional;
        cargarGradoProfesional(docenteDto.idGradoProfesional);
        
        hbClase.setVisible(false);
        spInformacionClase.setVisible(false);
    }
    
    private void cargarGradoProfesional(int idGradoProfesional){
        lbTextoGradoEstudios.setText("Grado profesional:");
        try {
            headers.put("Content-Type", "application/json");
            headers.put("Authorization", "Bearer " + sesion.jwt);
            String endpoint = String.format("/usuarios/catalogos/grados-profesionales/%s", idGradoProfesional);
            GradoProfesional gradoProfesional = ApiCliente.enviarSolicitud(endpoint, "GET", null, headers, GradoProfesional.class);
            lbGradoEstudios.setText(gradoProfesional.getNombre());
        } catch (Exception e) {
            VentanaEmergente.mostrarVentana("Error", null, e.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }
    
    // U S U A R I O
    @FXML
    public void clicBtnCambiarContrasenia(ActionEvent actionEvent) {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Perfil/cambiarContrasenia.fxml"));
            Parent vistaCambiarContrasenia = loader.load();
            Stage nuevaVentana = new Stage();
            nuevaVentana.setTitle("Cambiar contraseña");
            nuevaVentana.setScene(new Scene(vistaCambiarContrasenia));
            nuevaVentana.initModality(Modality.APPLICATION_MODAL);
            nuevaVentana.showAndWait();
        } catch (Exception ex) {
            VentanaEmergente.mostrarVentana("Error al cambiar la vista", null, ex.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }

    @FXML
    public void clicBtnEditarPerfil(ActionEvent actionEvent) {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Perfil/editarPerfil.fxml"));
            Parent vistaEditarPerfil = loader.load();
            
            EditarPerfilController controller = loader.getController();
            controller.cargarValores(sesion, lbNombreCompleto.getText(), lbNombreUsuario.getText(), idGrado);
            
            Stage nuevaVentana = new Stage();
            nuevaVentana.setTitle("Editar perfil");
            nuevaVentana.setScene(new Scene(vistaEditarPerfil));
            nuevaVentana.initModality(Modality.APPLICATION_MODAL);
            nuevaVentana.showAndWait();
        } catch (Exception ex) {
            VentanaEmergente.mostrarVentana("Error al cambiar la vista", null, ex.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }
    
    @FXML
    public void btnLbVolver(MouseEvent mouseEvent) {
    }
}
