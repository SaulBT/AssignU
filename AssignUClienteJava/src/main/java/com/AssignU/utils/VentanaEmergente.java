package com.AssignU.utils;

import javafx.scene.control.Alert;

public class VentanaEmergente {
    public static Alert mostrarVentana(String tituloVentana, String tituloCuerpo, String cuerpo, Alert.AlertType tipoVentana) {
        Alert alerta = new Alert(tipoVentana);
        alerta.setTitle(tituloVentana);
        alerta.setHeaderText(tituloCuerpo);
        alerta.setContentText(cuerpo);

        return alerta;
    }
}
