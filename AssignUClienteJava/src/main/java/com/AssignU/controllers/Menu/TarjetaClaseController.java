package com.AssignU.controllers.Menu;

import javafx.event.ActionEvent;
import javafx.scene.control.Label;

public class TarjetaClaseController {
    public Label lbNombreClase;
    public Label lbNombreDocente;
    public int idClase;

    public void btnVerClase(ActionEvent actionEvent) {
    }

    public void cargarDatos(String nombreClase, String nombreDocente, int idClase) {
        lbNombreClase.setText(nombreClase);
        lbNombreDocente.setText(nombreDocente);
        this.idClase = idClase;
    }
}
