package com.AssignU.controllers.Menu;

import com.AssignU.controllers.Clase.CrearClaseController;
import com.AssignU.models.Clases.ClaseDTO;
import com.AssignU.utils.ApiCliente;
import com.AssignU.utils.VentanaEmergente;
import javafx.event.ActionEvent;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.Alert;
import javafx.scene.control.Button;
import javafx.scene.input.MouseEvent;
import javafx.scene.layout.FlowPane;
import javafx.scene.layout.StackPane;
import javafx.stage.Modality;
import javafx.stage.Stage;

import java.io.IOException;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class MenuController {
    public FlowPane fpContenedorClases;
    public Button btnAccionClase;
    public String tipoUsuario;
    public String jwt;
    public int idUsuario;
    public List<ClaseDTO> listaClases;
    Map<String, String> headers = new HashMap<String, String>();

    public void btnLbCerrarSesion(MouseEvent mouseEvent) {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/login.fxml"));
            Parent nuevaVista = loader.load();
            Stage stage = (Stage) btnAccionClase.getScene().getWindow();
            Scene nuevaEscena = new Scene(nuevaVista);
            stage.setScene(nuevaEscena);
        } catch (IOException ex) {

        }
    }

    public void btnLbPerfil(MouseEvent mouseEvent) {
    }

    public void clicBtnAccionClase(ActionEvent actionEvent) {
        try {
            if (tipoUsuario.matches("alumno")) {
                VentanaEmergente.mostrarVentana("Advertencia", null, "¡Vaya! Parece que esto todavía no está terminado", Alert.AlertType.WARNING).showAndWait();
            } else if (tipoUsuario.matches("docente")) {
                FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Clase/crearClase.fxml"));
                Parent nuevaVista = loader.load();
                CrearClaseController controller = loader.getController();
                controller.cargarDatos(this, idUsuario, jwt);
                Stage nuevaVentana = new Stage();
                nuevaVentana.setTitle("Crear clase");
                nuevaVentana.setScene(new Scene(nuevaVista));
                nuevaVentana.initModality(Modality.APPLICATION_MODAL); // ← Esto bloquea la ventana principal
                nuevaVentana.showAndWait();
            }
        } catch (Exception e) {
            System.out.println("Error:" + e.getMessage());
        }
    }

    public void cargarValores(String tipoUsuario, String jwt, int idUsuario) {
        this.tipoUsuario = tipoUsuario;
        this.jwt = jwt;
        this.idUsuario = idUsuario;

        cargarClases();
        desplegarClases();
    }

    private void cargarClases() {
        try {
            headers.put("Content-Type", "application/json");
            headers.put("Authorization", "Bearer " + jwt);
            if (tipoUsuario.matches("alumno")) {
                listaClases = ApiCliente.enviarSolicitudLista("/clases/alumnos/" + idUsuario + "/clases", "GET", null, headers, ClaseDTO.class);
                btnAccionClase.setText("+ Unirme a clase");
            } else if (tipoUsuario.matches("docente")) {
                listaClases = ApiCliente.enviarSolicitudLista("/clases/docentes/" + idUsuario + "/clases", "GET", null, headers, ClaseDTO.class);
                btnAccionClase.setText("+ Crear clase");
            }
        } catch (Exception e) {
            VentanaEmergente.mostrarVentana("Error", null, e.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }

    private void desplegarClases() {
        try {
            fpContenedorClases.getChildren().clear();
            for (ClaseDTO clase : listaClases) {
                FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Menu/tarjetaClase.fxml"));
                StackPane tarjeta = loader.load();
                TarjetaClaseController controller = loader.getController();
                controller.cargarDatos(clase.getNombreClase(), "Prueba", clase.getIdClase());

                fpContenedorClases.getChildren().add(tarjeta);
            }
        } catch (Exception ex) {
            System.out.println("Error: " + ex.getMessage());
        }
    }

    public void enviarAClaseNueva(ClaseDTO claseDto){
        VentanaEmergente.mostrarVentana("¡BIEN!", null, "La clase " + claseDto.getNombreClase() + " fue creada.", Alert.AlertType.INFORMATION).showAndWait();
    }
}
