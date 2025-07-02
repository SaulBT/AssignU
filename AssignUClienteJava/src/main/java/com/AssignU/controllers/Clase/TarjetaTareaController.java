package com.AssignU.controllers.Clase;

import com.AssignU.controllers.Tarea.CrearTareaController;
import com.AssignU.controllers.Tarea.TareaController;
import com.AssignU.utils.Navegador;
import javafx.event.ActionEvent;
import javafx.scene.control.Button;
import javafx.scene.control.Label;

public class TarjetaTareaController {
    public Label lbNombreTarea;
    public Label lbFechaLimite;
    public Button btnAccionTarea;
    private int idTarea;
    private boolean esDocente;
    
    public void cargarTarea(String nombreTarea, String fechaLimite, boolean esDocente, int idTarea){
        lbNombreTarea.setText(nombreTarea);
        lbFechaLimite.setText(fechaLimite);
        this.idTarea = idTarea;
        this.esDocente = esDocente;
        if(esDocente){
            btnAccionTarea.setText("Editar");
        } else {
            btnAccionTarea.setText("Responder");
        }
    }

    public void clicBtnAccionTarea(ActionEvent actionEvent) {
        if(esDocente){
            /*Navegador.cambiarVentana(
                lbNombreTarea.getScene(),
                "/views/Tarea/crearTarea.fxml",
                "Editar Tarea",
                controller -> ((CrearTareaController) controller).cargarValores(claseDto.idClase)
            );*/
        } else {
            /*Navegador.cambiarVentana(
                lbNombreTarea.getScene(),
                "/views/Tarea/tarea.fxml",
                "Tarea: " + lbNombreTarea.getText(),
                controller -> ((TareaController) controller).cargarValores(idTarea)
            );*/
        }
    }
}
