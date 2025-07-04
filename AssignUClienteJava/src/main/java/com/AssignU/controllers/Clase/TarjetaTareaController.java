package com.AssignU.controllers.Clase;

import com.AssignU.controllers.Tarea.CrearTareaController;
import com.AssignU.controllers.Tarea.TareaController;
import com.AssignU.models.Tareas.TareaDTO;
import com.AssignU.servicios.ServicioTareas;
import com.AssignU.utils.Navegador;
import java.time.LocalDateTime;
import java.util.HashMap;
import javafx.event.ActionEvent;
import javafx.scene.control.Button;
import javafx.scene.control.Label;

public class TarjetaTareaController {
    public Label lbNombreTarea;
    public Label lbFechaLimite;
    public Button btnAccionTarea;
    private boolean esDocente;
    private TareaDTO tarea;
    
    public void cargarTarea(boolean esDocente, TareaDTO tarea){
        lbNombreTarea.setText(tarea.nombre);
        lbFechaLimite.setText(tarea.fechaLimite.toString());
        this.tarea = tarea;
        this.esDocente = esDocente;
        if(esDocente){
            btnAccionTarea.setText("Editar");
        } else {
            if(tarea.fechaLimite.isBefore(LocalDateTime.now())){
                btnAccionTarea.setText("Ver Tarea");
            } else {
                btnAccionTarea.setText("Responder");
            }
        }
    }

    public void clicBtnAccionTarea(ActionEvent actionEvent) {
        if(esDocente){
            cargarTareaDocente();
        } else {
            cargarTareaAlumno();
        }
    }
    
    public void cargarTareaDocente(){
        Navegador.cambiarVentanaConEstilos(
            lbNombreTarea.getScene(),
            "/views/Tarea/crearTarea.fxml",
            "Editar Tarea",
            controller -> ((CrearTareaController) controller).cargarValoresEdicion(tarea),
            "/views/stylesheets/estilos_date_picker.css"
        );
    }
    
    public void cargarTareaAlumno(){
        /*Navegador.cambiarVentana(
            lbNombreTarea.getScene(),
            "/views/Tarea/tarea.fxml",
            "Tarea: " + lbNombreTarea.getText(),
            controller -> ((TareaController) controller).cargarValores(tarea, tarea.fechaLimite.isBefore(LocalDateTime.now()))
        );*/
    }
}
