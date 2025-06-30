package com.AssignU.controllers.Perfil;

import com.AssignU.controllers.Menu.MenuController;
import com.AssignU.models.Perfil.EstadisticasPerfilDTO;
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
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.scene.control.*;
import javafx.scene.input.MouseEvent;
import javafx.scene.layout.HBox;

public class PerfilController{
    public Label lbPerfilNombreUsuario;
    public Label lbNombreUsuario;
    public Label lbNombreCompleto;
    public Label lbCorreoElectronico;
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
    public ScrollPane spInformacionClase;
    
    private Sesion sesion;
    private String mensajeError;
    private boolean esDocente;
    private int idGrado;
    
    private Map<String, String> headers = new HashMap<String, String>();
    @FXML
    private HBox hbClase;
    @FXML
    private Label lbTextoGrado;
    @FXML
    private Label lbGrado;
    

    public void cargarValores(){
        this.sesion = Sesion.getSesion();
        if (sesion.esDocente()) {
            obtenerDatosAlumno();
        } else {
            obtenerDatosDocente();
        }
    }
    
    // A L U M N O
    private void obtenerDatosAlumno(){
        HashMap<String, Object> respuesta = ServicioAlumnos.obtenerEstadisticasPerfil();
        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            EstadisticasPerfilDTO estadisticasDto = (EstadisticasPerfilDTO) respuesta.get(Constantes.KEY_RESPUESTA);
            cargarVentanaAlumno(estadisticasDto);
        } else {
            Utils.mostrarVentana("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }
    
    private void cargarVentanaAlumno(EstadisticasPerfilDTO estadisticasDto){
        lbPerfilNombreUsuario.setText("Perfil - " + estadisticasDto.nombreUsuario);
        lbNombreUsuario.setText(estadisticasDto.nombreUsuario);
        lbNombreCompleto.setText(estadisticasDto.nombreCompleto);
        lbCorreoElectronico.setText(estadisticasDto.correo);
        idGrado = estadisticasDto.idGradoEstudios;
        cargarGradoEstudios(idGrado);
        //TO DO set comboBox y tableView
        //configurarInformacionClases()
    }
    
    private void cargarGradoEstudios(int idGradoEstudios){
        lbTextoGrado.setText("Grado estudios:");
        HashMap<String, Object> respuesta = ServicioCatalogos.obtenerGradoEstudios(idGradoEstudios);
        GradoEstudioDTO gradoEstudios = (GradoEstudioDTO) respuesta.get(Constantes.KEY_RESPUESTA);
        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            lbGrado.setText(gradoEstudios.getNombre());
        } else {
            Utils.mostrarVentana("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }
    
    private void configurarInformacionClases(EstadisticasPerfilDTO estadisticasDto){
        cbClase.getItems().setAll(estadisticasDto.getClases());
    }
    
    // D O C E N T E
    private void obtenerDatosDocente(){
        HashMap<String, Object> respuesta = ServicioDocentes.obtenerDocente();
        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            DocenteDTO docenteDto = (DocenteDTO) respuesta.get(Constantes.KEY_RESPUESTA);
            cargarVentanaDocente(docenteDto);
        } else {
            Utils.mostrarVentana("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
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
        GradoProfesionalDTO gradoProfesional = (GradoProfesionalDTO) respuesta.get(Constantes.KEY_RESPUESTA);
        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            lbGrado.setText(gradoProfesional.getNombre());
        } else {
            Utils.mostrarVentana("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
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
            controller -> ((MenuController) controller).cargarValores()
        );
    }
}
