package com.AssignU.controllers.Perfil;

import com.AssignU.controllers.Menu.MenuController;
import com.AssignU.models.Perfil.ClaseEstadisticaPerfilDTO;
import com.AssignU.models.Perfil.EstadisticasPerfilDTO;
import com.AssignU.models.Perfil.TareaEstadisticaPerfilDTO;
import com.AssignU.models.Usuarios.Catalogo.GradoEstudioDTO;
import com.AssignU.models.Usuarios.Catalogo.GradoProfesionalDTO;
import com.AssignU.models.Usuarios.Docente.DocenteDTO;
import com.AssignU.models.Usuarios.Sesion;
import com.AssignU.servicios.usuarios.ServicioAlumnos;
import com.AssignU.servicios.usuarios.ServicioCatalogos;
import com.AssignU.servicios.usuarios.ServicioDocentes;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.Navegador;
import com.AssignU.utils.Utils;
import java.util.HashMap;
import java.util.Map;
import javafx.beans.value.ChangeListener;
import javafx.beans.value.ObservableValue;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.scene.control.*;
import javafx.scene.control.cell.PropertyValueFactory;
import javafx.scene.input.MouseEvent;
import javafx.scene.layout.HBox;

public class PerfilController{
    public Label lbPerfilNombreUsuario;
    public Label lbNombreUsuario;
    public Label lbNombreCompleto;
    public Label lbCorreoElectronico;
    public ComboBox cbClase;
    @FXML
    private TableView<TareaEstadisticaPerfilDTO> tvDatosClase;
    @FXML
    private TableColumn tcIdTarea;
    @FXML
    private TableColumn tcNombre;
    @FXML
    private TableColumn tcCalificacion;
    public Label lbPromedio;
    public Button btnCambiarContrasenia;
    public Button btnEditarPerfil;
    public ScrollPane spInformacionClase;
    
    private Sesion sesion;
    private String mensajeError;
    private int idGrado;
    
    @FXML
    private HBox hbClase;
    @FXML
    private Label lbTextoGrado;
    @FXML
    private Label lbGrado;
    

    public void cargarValores(){
        this.sesion = Sesion.getSesion();
        if (!sesion.esDocente()) {
            configurarTabla();
            obtenerDatosAlumno();
        } else {
            obtenerDatosDocente();
        }
    }
    
    private void configurarTabla(){
        tcIdTarea.setCellFactory(columna -> new TableCell<TareaEstadisticaPerfilDTO, Void>() {
            @Override
            protected void updateItem(Void item, boolean empty) {
                super.updateItem(item, empty);
                if (empty) {
                    setText(null);
                } else {
                    setText(String.valueOf(getIndex() + 1));
                }
            }
        });
        tcNombre.setCellValueFactory(new PropertyValueFactory<>("nombre"));
        tcCalificacion.setCellValueFactory(new PropertyValueFactory<>("Calificacion"));
        tcCalificacion.setCellFactory(columna -> new TableCell<TareaEstadisticaPerfilDTO, Float>() {
            @Override
            protected void updateItem(Float calificacion, boolean empty) {
                super.updateItem(calificacion, empty);
                if (empty || calificacion == null) {
                    setText(null);
                    setStyle("");
                } else {
                    setText(String.valueOf(calificacion));
                    if (calificacion >= 90) {
                        //setStyle("-fx-background-color: limegreen;");
                        setStyle("-fx-text-fill: limegreen;");
                    } else if (calificacion < 60) {
                        //setStyle("-fx-background-color: crimson;");
                        setStyle("-fx-text-fill: crimson;");
                    } else {
                        setStyle("");
                    }
                }
            }
        });
    }
    
    // A L U M N O
    private void obtenerDatosAlumno(){
        HashMap<String, Object> respuesta = ServicioAlumnos.obtenerEstadisticasPerfil();
        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            EstadisticasPerfilDTO estadisticasDto = (EstadisticasPerfilDTO) respuesta.get(Constantes.KEY_RESPUESTA);
            cargarVentanaAlumno(estadisticasDto);
        } else {
            Utils.mostrarAlerta("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }
    
    private void cargarVentanaAlumno(EstadisticasPerfilDTO estadisticasDto){
        lbPerfilNombreUsuario.setText("Perfil - " + estadisticasDto.nombreUsuario);
        lbNombreUsuario.setText(estadisticasDto.nombreUsuario);
        lbNombreCompleto.setText(estadisticasDto.nombreCompleto);
        lbCorreoElectronico.setText(estadisticasDto.correo);
        idGrado = estadisticasDto.idGradoEstudios;
        hbClase.setVisible(true);
        spInformacionClase.setVisible(true);
        cargarGradoEstudios(idGrado);configurarInformacionClases(estadisticasDto);
    }
    
    private void cargarGradoEstudios(int idGradoEstudios){
        lbTextoGrado.setText("Grado estudios:");
        HashMap<String, Object> respuesta = ServicioCatalogos.obtenerGradoEstudios(idGradoEstudios);
        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            GradoEstudioDTO gradoEstudios = (GradoEstudioDTO) respuesta.get(Constantes.KEY_RESPUESTA);
            lbGrado.setText(gradoEstudios.getNombre());
        } else {
            Utils.mostrarAlerta("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }
    
    private void configurarInformacionClases(EstadisticasPerfilDTO estadisticasDto){
        ObservableList<ClaseEstadisticaPerfilDTO> clases = FXCollections.observableArrayList(estadisticasDto.clases);
        cbClase.setItems(clases);
        
        cbClase.valueProperty().addListener(new ChangeListener<ClaseEstadisticaPerfilDTO>(){
            @Override
            public void changed(ObservableValue<? extends ClaseEstadisticaPerfilDTO> observable, ClaseEstadisticaPerfilDTO oldValue, ClaseEstadisticaPerfilDTO newValue) {
                if(newValue != null){
                    mostrarTareasDeClase(newValue);
                }
            }
        });
        
        if (!clases.isEmpty()) {
            cbClase.getSelectionModel().selectFirst();
        }
    }
    
    private void mostrarTareasDeClase(ClaseEstadisticaPerfilDTO clase) {
        ObservableList<TareaEstadisticaPerfilDTO> tareas = FXCollections.observableArrayList();
        if (clase.tareas != null) {
            tareas.addAll(clase.tareas);
        }
        tvDatosClase.setItems(tareas);
        mostrarPromedio(tareas);
    }
    
    private void mostrarPromedio(ObservableList<TareaEstadisticaPerfilDTO> tareas) {
        if (tareas.isEmpty()) {
            lbPromedio.setText("N/A");
            return;
        }
        double promedio = tareas.stream().mapToDouble(TareaEstadisticaPerfilDTO::getCalificacion).average().orElse(0);
        lbPromedio.setText(String.format("%.2f", promedio));
    }
    
    // D O C E N T E
    private void obtenerDatosDocente(){
        HashMap<String, Object> respuesta = ServicioDocentes.obtenerDocente();
        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            DocenteDTO docenteDto = (DocenteDTO) respuesta.get(Constantes.KEY_RESPUESTA);
            cargarVentanaDocente(docenteDto);
        } else {
            Utils.mostrarAlerta("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }
    
    private void cargarVentanaDocente(DocenteDTO docenteDto){
        lbPerfilNombreUsuario.setText("Perfil - " + docenteDto.nombreUsuario);
        lbNombreUsuario.setText(docenteDto.nombreUsuario);
        lbNombreCompleto.setText(docenteDto.nombreCompleto);
        lbCorreoElectronico.setText(docenteDto.correoElectronico);
        idGrado = docenteDto.idGradoProfesional;
        cargarGradoProfesional(idGrado);
        
        hbClase.setVisible(false);
        spInformacionClase.setVisible(false);
    }
    
    private void cargarGradoProfesional(int idGradoProfesional){
        lbTextoGrado.setText("Grado profesional:");
        HashMap<String, Object> respuesta = ServicioCatalogos.obtenerGradoProfesional(idGradoProfesional);
        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            GradoProfesionalDTO gradoProfesional = (GradoProfesionalDTO) respuesta.get(Constantes.KEY_RESPUESTA);
            lbGrado.setText(gradoProfesional.getNombre());
        } else {
            Utils.mostrarAlerta("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }
    
    // U S U A R I O
    @FXML
    public void clicBtnCambiarContrasenia(ActionEvent actionEvent) {
        Navegador.abrirVentanaModal(
            "/views/Perfil/cambiarContrasenia.fxml",
            "Cambiar Contraseña",
            controller -> ((CambiarContraseniaController) controller).cargarValores()
        );
    }

    @FXML
    public void clicBtnEditarPerfil(ActionEvent actionEvent) {
        Navegador.abrirVentanaModal(
            "/views/Perfil/editarPerfil.fxml",
            "Editar Perfil",
            controller -> ((EditarPerfilController) controller).cargarValores(
                lbNombreCompleto.getText(),lbNombreUsuario.getText(),idGrado)
        );
    }
    
    @FXML
    public void btnLbVolver(MouseEvent mouseEvent) {
        Navegador.cambiarVentana(
            lbNombreCompleto.getScene(),
            "/views/Menu/menu.fxml",
            "Clases",
            controller -> ((MenuController) controller).cargarValores()
        );
    }
}
