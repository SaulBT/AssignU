package com.AssignU.controllers.Menu;

import com.AssignU.controllers.Clase.ClaseController;
import com.AssignU.models.Clases.ClaseDTO;
import com.AssignU.utils.Navegador;
import javafx.event.ActionEvent;
import javafx.scene.control.Label;

public class TarjetaClaseController {
    public Label lbNombreClase;
    public Label lbNombreDocente;
    public int idClase;
    public ClaseDTO claseDto;
    
    public void cargarDatos(ClaseDTO claseDto, String nombreDocente) {
        lbNombreClase.setText(claseDto.nombreClase);
        lbNombreDocente.setText(nombreDocente);
        this.idClase = claseDto.idClase;
    }

    public void btnVerClase(ActionEvent actionEvent) {
        Navegador.cambiarVentana(
            lbNombreClase.getScene(),
            "/views/Clase/clase.fxml",
            claseDto.nombreClase,
            controller -> ((ClaseController) controller).cargarValoresDeMenu(claseDto)
        );
    }
}
