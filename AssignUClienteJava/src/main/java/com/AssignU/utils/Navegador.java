
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
    public static <T> T cambiarVentana(Scene escenaActual, String rutaFXML, String nuevoTitulo, Consumer<T> configurador) {
        try {
            FXMLLoader loader = new FXMLLoader(Navegador.class.getResource(rutaFXML));
            Parent vista = loader.load();

            T controller = loader.getController();
            if (configurador != null)
                configurador.accept(controller);

            Stage stage = (Stage) escenaActual.getWindow();
            stage.setTitle("AssignU - " + nuevoTitulo);
            stage.setScene(new Scene(vista));

            return controller;

        } catch (IOException e) {
            Utils.mostrarAlerta("Error de Vista", "No se pudo cargar la vista:\n" + e.getMessage(), Alert.AlertType.ERROR);
            return null;
        }
    }
    
    public static <T> T cambiarVentanaConEstilos(Scene escenaActual, String rutaFXML, String nuevoTitulo, Consumer<T> configurador, String rutaEstilos) {
        try {
            FXMLLoader loader = new FXMLLoader(Navegador.class.getResource(rutaFXML));
            Parent vista = loader.load();

            T controller = loader.getController();
            if (configurador != null)
                configurador.accept(controller);

            Scene nuevaEscena = new Scene(vista);

            if (rutaEstilos != null) {
                String hojaEstilo = Navegador.class.getResource(rutaEstilos).toExternalForm();
                nuevaEscena.getStylesheets().add(hojaEstilo);
            }

            Stage stage = (Stage) escenaActual.getWindow();
            stage.setTitle("AssignU - " + nuevoTitulo);
            stage.setScene(nuevaEscena);

            return controller;

        } catch (IOException e) {
            Utils.mostrarAlerta("Error de Vista", "No se pudo cargar la vista:\n" + e.getMessage(), Alert.AlertType.ERROR);
            return null;
        }
    }

    public static <T> T abrirVentanaModal(String rutaFXML, String titulo, Consumer<T> configurador) {
        try {
            FXMLLoader loader = new FXMLLoader(Navegador.class.getResource(rutaFXML));
            Parent vista = loader.load();

            T controller = loader.getController();
            if (configurador != null)
                configurador.accept(controller);

            Stage ventana = new Stage();
            ventana.setTitle(titulo);
            ventana.setScene(new Scene(vista));
            ventana.initModality(Modality.APPLICATION_MODAL);
            ventana.showAndWait();

            return controller;
        } catch (IOException e) {
            Utils.mostrarAlerta("Error", "No se pudo abrir la ventana modal:\n" + e.getMessage(), Alert.AlertType.ERROR);
            return null;
        }
    }

}

