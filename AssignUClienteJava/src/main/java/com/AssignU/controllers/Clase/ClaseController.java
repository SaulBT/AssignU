package com.AssignU.controllers.Clase;

import com.AssignU.controllers.Menu.MenuController;
import com.AssignU.controllers.Tarea.CrearTareaController;
import com.AssignU.models.Clases.ClaseDTO;
import com.AssignU.models.Tareas.TareaDTO;
import com.AssignU.models.Usuarios.Sesion;
import com.AssignU.servicios.ServicioClases;
import com.AssignU.servicios.ServicioTareas;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.Navegador;
import com.AssignU.utils.Utils;
import java.io.IOException;
import java.time.LocalDateTime;
import java.util.HashMap;
import java.util.List;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.control.Alert;
import javafx.scene.control.Button;
import javafx.scene.control.Label;
import javafx.scene.input.MouseEvent;
import javafx.scene.layout.FlowPane;
import javafx.scene.layout.StackPane;

public class ClaseController {
    @FXML
    private Label lbNombreClase;
    public Label lbCodigoClase;
    public Button btnCrearTarea;
    public FlowPane fpContenedorTareas;
    public Label lbEliminarClase;
    public Label lbCambiarNombre;
    public Label lbVerEstadisticas;
    
    private Sesion sesion;
    private ClaseDTO claseDto;
    
    public void cargarValoresDeMenu(ClaseDTO claseDto){
        HashMap<String, Object> respuesta = ServicioClases.actualizarUltimaConexion(claseDto.idClase, LocalDateTime.now());
        if ((boolean) respuesta.get(Constantes.KEY_ERROR)) {
            Utils.mostrarVentana("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
        cargarValores(claseDto);
    }
    
    public void cargarValores(ClaseDTO claseDto){
        this.sesion = Sesion.getSesion();
        this.claseDto = claseDto;
        lbNombreClase.setText(claseDto.nombreClase);
        lbCodigoClase.setText(claseDto.codigoClase);
        if(!sesion.esDocente()){
            cargarVentanaAlumno();
        }
        cargarTareas();
    }
    
    private void cargarVentanaAlumno(){
        btnCrearTarea.setVisible(false);
        lbEliminarClase.setVisible(false);
        lbCambiarNombre.setVisible(false);
        lbVerEstadisticas.setVisible(false);
    }
    
    private void cargarTareas(){
        HashMap<String, Object> respuesta = ServicioTareas.obtenerTareas(claseDto.idClase);
        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            List<TareaDTO> listaTareas = (List<TareaDTO>) respuesta.get(Constantes.KEY_RESPUESTA);
            desplegarTareas(listaTareas);
        } else {
            Utils.mostrarVentana("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }
    
    private void desplegarTareas(List<TareaDTO> listaTareas){
        try {
            fpContenedorTareas.getChildren().clear();
            for (TareaDTO tarea : listaTareas) {
                FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Clase/tarjetaTarea.fxml"));
                StackPane tarjeta = loader.load();
                TarjetaTareaController controller = loader.getController();
                controller.cargarTarea(tarea.nombre, tarea.fechaLimite.toString(), sesion.esDocente(), tarea.idTarea);
                fpContenedorTareas.getChildren().add(tarjeta);
            }
        } catch (IOException ex) {
            System.out.println("Error: " + ex.getMessage());
        }
    }

    // D O C E N T E
    @FXML
    public void clicBtnCrearTarea(ActionEvent actionEvent) {
        Navegador.cambiarVentana(
            lbCambiarNombre.getScene(),
            "/views/Tarea/crearTarea.fxml",
            "Crear Tarea",
            controller -> ((CrearTareaController) controller).cargarValores(claseDto.idClase)
        );
    }

    @FXML
    public void btnLbEliminarClase(MouseEvent mouseEvent) {
        if(sesion.esDocente()){
            HashMap<String, Object> respuesta = ServicioClases.borrarClase(claseDto.idClase, claseDto.nombreClase);
            if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
                Utils.mostrarVentana("Éxito", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.INFORMATION);
            } else {
                Utils.mostrarVentana("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
            }
        } else {
            Utils.mostrarVentana("Advertencia", "No estás autorizado para borrar esta clase.", Alert.AlertType.WARNING);
        }
    }

    @FXML
    public void btnLbCambiarNombre(MouseEvent mouseEvent) {
        if(sesion.esDocente()){
            Navegador.abrirVentanaModal(
                "/views/Clase/cambiarNombre.fxml",
                "Cambiar Nombre",
                controller -> ((CambiarNombreController) controller).cargarValores(this, claseDto.idClase, claseDto.nombreClase)
            );
        } else {
            Utils.mostrarVentana("Advertencia", "No estás autorizado para cambiar el nombre de la clase.", Alert.AlertType.WARNING);
        }
    }

    @FXML
    public void btnLbVerEstadisticas(MouseEvent mouseEvent) {
        /*if(sesion.esDocente()){
            Navegador.cambiarVentana(
                lbNombreClase.getScene(),
                "/views/Clase/estadisticasClase.fxml",
                "Estadísticas Clase",
                controller -> ((EstadisticasClaseController) controller).cargarValores()
            );
        } else {
            Utils.mostrarVentana("Advertencia", "No estás autorizado para cambiar el nombre de la clase.", Alert.AlertType.WARNING);
        }*/
    }
    
    // U S U A R I O
    @FXML
    public void btnLbVolver(MouseEvent mouseEvent) {
        Navegador.cambiarVentana(
            lbNombreClase.getScene(),
            "/views/Menu/menu.fxml",
            "Clases",
            controller -> ((MenuController) controller).cargarValores()
        );
    }
}
