
package com.AssignU.utils;

import java.io.IOException;
import java.util.function.Consumer;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.Alert;
import javafx.stage.Modality;
import javafx.stage.Stage;

public class Navegador {
    public static <T> T cambiarVentana(Scene escenaActual, String rutaFXML, Consumer<T> configurador) {
        try {
            FXMLLoader loader = new FXMLLoader(Navegador.class.getResource(rutaFXML));
            Parent vista = loader.load();

            T controller = loader.getController();
            if (configurador != null)
                configurador.accept(controller);

            Stage stage = (Stage) escenaActual.getWindow();
            stage.setScene(new Scene(vista));

            return controller;

        } catch (IOException e) {
            VentanaEmergente.mostrarVentana("Error", "No se pudo cargar la vista", e.getMessage(), Alert.AlertType.ERROR).showAndWait();
            return null;
        }
    }

    public static <T> T abrirVentanaModal(String rutaFXML, Consumer<T> configurador) {
        try {
            FXMLLoader loader = new FXMLLoader(Navegador.class.getResource(rutaFXML));
            Parent vista = loader.load();

            T controller = loader.getController();
            if (configurador != null)
                configurador.accept(controller);

            Stage ventana = new Stage();
            ventana.setScene(new Scene(vista));
            ventana.initModality(Modality.APPLICATION_MODAL);
            ventana.showAndWait();

            return controller;

        } catch (IOException e) {
            VentanaEmergente.mostrarVentana("Error", "No se pudo abrir ventana modal", e.getMessage(), Alert.AlertType.ERROR).showAndWait();
            return null;
        }
    }
}

